using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Threading;
using ICSharpCode.AvalonEdit;
using UEExplorer.Framework;
using UEExplorer.Framework.UI.Commands;
using UEExplorer.Framework.UI.Editor;
using UEExplorer.Properties;
using UELib;
using UELib.Core;
using UELib.Decompiler;
using MenuItem = System.Windows.Controls.MenuItem;

namespace UEExplorer.UI.Pages
{
    public sealed class DecompilerContextPage : TrackingContextPage
    {
        private const string TokensTemplate = "({0:X3}h/{1:X3}h) [{3}]\r\n" +
                                              "\t{2}\r\n" +
                                              "\t{4}";

        private const string DisassembleTokensTemplate = "%INDENTATION%({5}-{6};{0:X3}h/{1:X3}h) [{3}]\r\n" +
                                                         "%INDENTATION%\t{2}\r\n" +
                                                         "%INDENTATION%\t{4}";

        private readonly BehaviorSubject<ContextInfo> _ActiveContext =
            new BehaviorSubject<ContextInfo>(new ContextInfo(ContextActionKind.Target, null));

        private readonly CancellationTokenSource _CancellationTokenSource = new CancellationTokenSource();

        private readonly CommandsBuilder _CommandsBuilder;

        private readonly ContextService _ContextService;
        private readonly TextEditorPanel _EditorPanel;
        private readonly TextEditor _TextEditor;

        public DecompilerContextPage()
        {
            _ContextService = ServiceHost.GetRequired<ContextService>();
            _ContextService.ContextChanged += ContextServiceOnContextChanged;

            _CommandsBuilder = new CommandsBuilder(new ICommandBuilder<IEnumerable<IContextCommand>>[] { });

            SuspendLayout();
            TextPrefix = Resources.DecompilerPage_Decompile_Title;
            _EditorPanel = new TextEditorPanel();
            _EditorPanel.Dock = DockStyle.Fill;
            Controls.Add(_EditorPanel);
            _TextEditor = _EditorPanel.TextEditorControl.TextEditor;
            _EditorPanel.TextArea.Caret.PositionChanged += TextEditorCaretOnPositionChanged;
            _EditorPanel.ActiveSegmentChanged += TextEditorPanelOnActiveSegmentChanged;
            _EditorPanel.SegmentDoubleClicked += TextEditorPanelOnSegmentDoubleClicked;
            _EditorPanel.ContextMenuOpening += TextEditorPanelOnContextMenuOpening;
            ResumeLayout();

            _ActiveContext
                .ObserveOn(Dispatcher.CurrentDispatcher)
                .Where(context => context.Target != null)
                .Subscribe(context =>
                {
                    UpdateContextText(context);

                    async void UIThreadInvoker()
                    {
                        bool success = await Accept(context);
                        if (!success)
                        {
                            return;
                        }

                        var source = context.Location.SourceLocation;
                        if (Equals(source, SourceLocation.Empty))
                        {
                            return;
                        }

                        _EditorPanel.FocusSource(source);
                    }

                    BeginInvoke((MethodInvoker)UIThreadInvoker);
                });
        }

        public IDecompiler<IOutputDecompiler<IAcceptable>, IAcceptable> Decompiler { get; set; }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);

            _ActiveContext.Dispose();
        }

        private void TextEditorCaretOnPositionChanged(object sender, EventArgs e)
        {
            // HACK: Should not be using a resolved reference here.
            object target = _ActiveContext.Value.ResolvedTarget is UObject uObject
                ? uObject.Table
                : _ActiveContext.Value.ResolvedTarget;
            if (target == null)
            {
                return;
            }

            // TODO: Hookup to AST, but we don't have one yet for UnrealScript!
            var context = new ContextInfo(
                ContextActionKind.Target,
                target,
                SourceLocation.Empty);

            var contextChangedEvent = new ContextChangedEventArgs(context);
            _ContextService.OnContextChanged(this, contextChangedEvent);
        }

        private void TextEditorPanelOnSegmentDoubleClicked(object sender, SegmentEventArgs e)
        {
            var segment = e.ProgramSegment;
            if (segment == null)
            {
                return;
            }

            object target = segment.Location.StreamLocation.Source;

            var context = new ContextInfo(
                ContextActionKind.Target,
                target,
                segment.Location);

            var contextChangedEvent = new ContextChangedEventArgs(context);
            _ContextService.OnContextChanged(this, contextChangedEvent);
        }

        private void TextEditorPanelOnActiveSegmentChanged(object sender, SegmentEventArgs e)
        {
            var segment = e.ProgramSegment;
            if (segment == null)
            {
                return;
            }

            object target = _ActiveContext.Value.ResolvedTarget;
            var location = segment.Location;
            var context = new ContextInfo(
                ContextActionKind.Location,
                target,
                location);

            var contextChangedEvent = new ContextChangedEventArgs(context);
            _ContextService.OnContextChanged(this, contextChangedEvent);
        }

        private void TextEditorPanelOnContextMenuOpening(object sender, SegmentEventArgs e)
        {
            object target = _ActiveContext.Value.ResolvedTarget;

            var objectContextMenu = _TextEditor.ContextMenu;
            Debug.Assert(objectContextMenu != null, nameof(objectContextMenu) + " != null");

            objectContextMenu.Items.Clear();

            var commands = _CommandsBuilder
                .Build(target)
                .ToList();

            // FIXME: Generalize this using dependency injection services along with the cmd builder usage in the PackageExplorerPanel class.
            var items = commands
                .Select(cmd =>
                {
                    string name = cmd.GetType().Name;

                    var displayNameAttribute = cmd.GetType().GetCustomAttribute<DisplayNameAttribute>();
                    Contract.Assert(displayNameAttribute != null);
                    string text = displayNameAttribute.DisplayName;

                    var item = new MenuItem { Name = name, Header = text };
                    return item;
                });

            foreach (var item in items)
            {
                objectContextMenu.Items.Add(item);
                if (item.Name == "")
                {
                    continue;
                }

                BuildSubItems(commands, item);
            }

            if (objectContextMenu.Items.Count > 0)
            {
                objectContextMenu.Items.Add(new Separator());
            }

            objectContextMenu.Items.Add(_EditorPanel.TextEditorControl.CopyMenuItem);
        }

        private void BuildSubItems(IList<IContextCommand> commands, ItemsControl control)
        {
            var subItems = commands
                //.Where(cmd => cmd.ParentName == control.Name)
                //.OrderBy(cmd => cmd.Category)
                .Select(cmd =>
                {
                    string name = cmd.GetType().Name;

                    var displayNameAttribute = cmd.GetType().GetCustomAttribute<DisplayNameAttribute>();
                    Contract.Assert(displayNameAttribute != null);
                    string text = displayNameAttribute.DisplayName;

                    var item = new MenuItem { Name = name, Header = text };
                    return item;
                });

            foreach (var subItem in subItems)
            {
                control.Items.Add(subItem);
                if (subItem.Name == control.Name)
                {
                    continue;
                }

                BuildSubItems(commands, subItem);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _ContextService.ContextChanged -= ContextServiceOnContextChanged;
            }

            base.Dispose(disposing);
        }

        private void ContextServiceOnContextChanged(object sender, ContextChangedEventArgs e)
        {
            if (sender == this)
            {
                return;
            }

            if (!IsTracking)
            {
                return;
            }

            if (e.Context.ActionKind == ContextActionKind.Location)
            {
                return;
            }

            if (!CanAccept(e.Context))
            {
                return;
            }

            Debug.Assert(e.Context.ResolvedTarget is IAcceptable);
            _ActiveContext.OnNext(e.Context);
        }

        public override bool CanAccept(ContextInfo context) =>
            Decompiler.CanDecompile(context.ResolvedTarget as IAcceptable);

        public override async Task<bool> Accept(ContextInfo context)
        {
            Console.Write(Thread.CurrentThread);
            var editor = _EditorPanel.TextEditorControl.TextEditor;
            var document = editor.Document;

            var text = new StringBuilder();
            var stream = new StringWriter(text, CultureInfo.InvariantCulture);
            var outputStream = new TextEditorOutputStream(document, stream);

            var outputDecompiler = (IOutputDecompiler<IAcceptable>)Activator.CreateInstance(
                Decompiler.GetOutputDecompilerType(),
                outputStream);

            try
            {
                await Decompiler
                    .Run((IAcceptable)context.ResolvedTarget, outputDecompiler,
                        _CancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                throw new Exception("Decompiler run exception", ex);
            }

            document.Text = stream.ToString();


            //switch (context.ActionKind)
            //{
            //    // Hacky support for a legacy feature that we still have a need for.
            //    case ContextActionKind.DecompileTokens:
            //        {
            //            if (context.ResolvedTarget is UStruct uStruct && uStruct.ByteCodeManager != null)
            //            {
            //                var script = uStruct.ByteCodeManager;
            //                script.Deserialize();
            //                script.InitDecompile();

            //                string text = LegacyDecompileTokens(uStruct, script);
            //                _Panel.Object = text;
            //            }
            //        }
            //        break;

            //    // Hacky support for a legacy feature that we still have a need for.
            //    case ContextActionKind.DisassembleTokens:
            //        {
            //            if (context.ResolvedTarget is UStruct uStruct && uStruct.ByteCodeManager != null)
            //            {
            //                var script = uStruct.ByteCodeManager;
            //                script.Deserialize();
            //                script.InitDecompile();

            //                string text = LegacyDisassembleTokens(uStruct, script, script.DeserializedTokens.Count);
            //                _Panel.Object = text;
            //            }

            //            break;
            //        }

            //    default:
            //        _Panel.Object = context.ResolvedTarget;
            //        break;
            //}

            return true;
        }

        /// ----------- OLD CODE, to be deprecated when we have a better "Listing" panel.
        [Obsolete("To be displaced by Listing")]
        private string LegacyDecompileTokens(UStruct context, UStruct.UByteCodeDecompiler script)
        {
            var stream = context.Package.Stream;
            long scriptOffset = context.ExportTable.SerialOffset + context.ScriptOffset;
            var code = new StringBuilder();
            while (script.CurrentTokenIndex + 1 < script.DeserializedTokens.Count)
            {
                var t = script.NextToken;
                int orgIndex = script.CurrentTokenIndex;
                string output;
                bool breakOut = false;
                try
                {
                    output = t.Decompile();
                }
                catch
                {
                    output = "Exception occurred while decompiling token: " + t.GetType().Name;
                    breakOut = true;
                }

                string chain = LegacyFormatTokenHeader(t);
                int inlinedTokens = script.CurrentTokenIndex - orgIndex;
                if (inlinedTokens > 0)
                {
                    ++orgIndex;
                    for (int i = 0; i < inlinedTokens; ++i)
                    {
                        chain += " -> " +
                                 LegacyFormatTokenHeader(script.DeserializedTokens[orgIndex + i]);
                    }
                }

                byte[] buffer = new byte[t.StorageSize];
                stream.Position = scriptOffset + t.StoragePosition;
                stream.Read(buffer, 0, buffer.Length);

                code.Append(string.Format(TokensTemplate,
                    t.Position, t.StoragePosition,
                    chain, BitConverter.ToString(buffer).Replace('-', ' '),
                    output != string.Empty ? output + "\r\n" : output
                ));

                if (breakOut)
                {
                    break;
                }
            }

            return code.ToString();
        }

        [Obsolete("To be displaced by Listing")]
        private static string LegacyFormatTokenHeader(UStruct.UByteCodeDecompiler.Token token)
        {
            string name = token.GetType().Name;
            string abbrName = string.Concat(name.Where(char.IsUpper));
            return $"{abbrName}({token.Size}/{token.StorageSize})";
        }

        [Obsolete("To be displaced by Listing")]
        private static string LegacyDisassembleTokens(UStruct context, UStruct.UByteCodeDecompiler script,
            int tokenCount)
        {
            var stream = context.Package.Stream;
            long scriptOffset = context.ExportTable.SerialOffset + context.ScriptOffset;
            var code = new StringBuilder();
            for (int i = 0; i + 1 < tokenCount; ++i)
            {
                string value;
                var token = script.NextToken;
                int firstTokenIndex = script.CurrentTokenIndex;
                int lastTokenIndex;
                int subTokensCount;

                try
                {
                    value = token.Decompile();
                }
                catch (Exception e)
                {
                    value = "Exception occurred while decompiling token: " + e;
                }
                finally
                {
                    lastTokenIndex = script.CurrentTokenIndex;
                    subTokensCount = lastTokenIndex - firstTokenIndex;
                    script.CurrentTokenIndex = firstTokenIndex;
                }

                byte[] buffer = new byte[token.StorageSize];
                stream.Position = scriptOffset + token.StoragePosition;
                stream.Read(buffer, 0, buffer.Length);

                string header = LegacyFormatTokenHeader(token);
                string bytes = BitConverter.ToString(buffer).Replace('-', ' ');

                code.Append(string.Format(DisassembleTokensTemplate.Replace("%INDENTATION%", UDecompilingState.Tabs),
                    token.Position, token.StoragePosition,
                    header, bytes,
                    value != string.Empty ? value + "\r\n" : value, firstTokenIndex, lastTokenIndex
                ));

                if (subTokensCount > 0)
                {
                    UDecompilingState.AddTab();
                    try
                    {
                        code.Append(LegacyDisassembleTokens(context, script, subTokensCount + 1));
                    }
                    catch (Exception ex)
                    {
                        code.Append($"/*Disassemble error: {ex}*/");
                    }
                    finally
                    {
                        i += subTokensCount;
                        UDecompilingState.RemoveTab();
                    }
                }
            }

            return code.ToString();
        }
    }
}
