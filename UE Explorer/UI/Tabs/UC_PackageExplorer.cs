using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Krypton.Navigator;
using Krypton.Docking;
using Storm.TabControl;
using UEExplorer.Properties;
using UEExplorer.UI.Forms;
using UEExplorer.UI.Main;
using UEExplorer.UI.Pages;
using UELib.Annotations;
using UELib.Engine;

namespace UEExplorer.UI.Tabs
{
    using Dialogs;
    using UELib;
    using UELib.Core;

    [ComVisible(false)]
    public partial class UC_PackageExplorer : UserControl_Tab
    {
        public string FilePath { get; set; }

        /// <summary>
        /// Null if the user cancels the exception popup.
        /// </summary>
        [CanBeNull] private UnrealPackage _UnrealPackage;

        private KryptonDockingWorkspace _Workspace;
        private PackageExplorerPage _PackageExplorerPage;

        public UC_PackageExplorer(string filepath)
        {
            FilePath = filepath;

            InitializeComponent();

            contextService.ContextChanged += ContextServiceOnContextChanged;
        }

        private void ContextServiceOnContextChanged(object sender, ContextChangedEventArgs e)
        {
        }

        private void UC_PackageExplorer_Load(object sender, EventArgs e)
        {
            kryptonDockingManagerMain.ManageFloating("Floating", ParentForm);

            var navControl = kryptonDockingManagerMain.ManageControl("Nav", dockingPanel);
            _Workspace = kryptonDockingManagerMain.ManageWorkspace("Workspace", kryptonDockableWorkspaceMain);
            var viewControl = kryptonDockingManagerMain.ManageControl("View", dockingPanel);

            if (File.Exists(Program.DockingConfigPath))
                kryptonDockingManagerMain.LoadConfigFromFile(Program.DockingConfigPath);

            _PackageExplorerPage = CreatePackageExplorerPage();
            var expSpace = kryptonDockingManagerMain.AddDockspace(
                "Nav",
                DockingEdge.Left,
                new KryptonPage[] { _PackageExplorerPage });
            expSpace.DockspaceControl.Size = new Size(340, 0);

            InitializeFromFile(FilePath);
        }

        private PackageExplorerPage CreatePackageExplorerPage()
        {
            var page = new PackageExplorerPage(contextService);
            page.ClearFlags(KryptonPageFlags.DockingAllowClose |
                            KryptonPageFlags.DockingAllowFloating |
                            KryptonPageFlags.DockingAllowWorkspace);
            return page;
        }

        private T CreateTrackingPage<T>(bool isTracking)
            where T : TrackingPage
        {
            var page = (T)Activator.CreateInstance(typeof(T), contextService);
            page.IsTracking = isTracking;
            page.ClearFlags(KryptonPageFlags.DockingAllowAutoHidden);
            return page;
        }

        private void InitializeFromFile(string filePath)
        {
            try
            {
                LoadPackageData(filePath);
            }
            catch (Exception exception)
            {
                throw new UnrealException("Couldn't load or initialize package", exception);
            }
        }

        private void LoadPackageData(string filePath)
        {
            if (Program.Options.bForceLicenseeMode)
                UnrealPackage.OverrideLicenseeVersion = Program.Options.LicenseeMode;

            if (Program.Options.bForceVersion)
                UnrealPackage.OverrideVersion = Program.Options.Version;

            UnrealConfig.SuppressSignature = false;
            UnrealConfig.Platform = (UnrealConfig.CookedPlatform)Enum.Parse
            (
                typeof(UnrealConfig.CookedPlatform),
                ((ProgramForm)ParentForm).Platform.Text, true
            );

            // Open the file.
        reload:
            var shouldDeserialize = true;
            ProgressStatus.SetStatus(Resources.PACKAGE_LOADING);
            // Note: We cannot dispose neither the stream nor package, because we require the stream to be hot so that we can lazily-deserialize objects.
            var stream = new UPackageStream(filePath, FileMode.Open, FileAccess.Read);
            try
            {
                _UnrealPackage = new UnrealPackage(stream);
                UnrealConfig.SuppressSignature = false;
            }
            catch (FileLoadException)
            {
                _UnrealPackage?.Dispose();

                if (MessageBox.Show(Resources.PACKAGE_UNKNOWN_SIGNATURE,
                        Resources.Warning,
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    UnrealConfig.SuppressSignature = true;
                    goto reload;
                }

                shouldDeserialize = false;
            }

            Contract.Assert(_UnrealPackage != null, "Package is null");
            PreInitializeContentTree(_UnrealPackage);

            if (shouldDeserialize)
            {
                _UnrealPackage.Deserialize(stream);
            }

            LinkPackageData(_UnrealPackage);
            LinkSummaryData(ref _UnrealPackage.Summary);
            if (IsPackageCompressed())
            {
                if (MessageBox.Show(Resources.PACKAGE_IS_COMPRESSED,
                        Resources.NOTICE_TITLE,
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Question) == DialogResult.OK)
                {
                    Process.Start("https://www.gildor.org/downloads");
                    MessageBox.Show(Resources.COMPRESSED_HOWTO,
                        Resources.NOTICE_TITLE,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }

            if (!IsPackageCompressed())
            {
                string ntlFilePath = Path.Combine(Application.StartupPath, "Native Tables", Program.Options.NTLPath);
                if (File.Exists(ntlFilePath + NativesTablePackage.Extension))
                {
                    _UnrealPackage.NTLPackage = new NativesTablePackage();
                    _UnrealPackage.NTLPackage.LoadPackage(ntlFilePath);
                }

                InitializePackageObjects();
            }
        }

        // Pre-initialize, this package has not been serialized yet, but we can initialize pre-assumed nodes here.
        private void PreInitializeContentTree(UnrealPackage linker)
        {
            var rootPackageNode = _PackageExplorerPage.PackageExplorerPanel.CreateRootPackageNode(linker);
            _PackageExplorerPage.PackageExplorerPanel.AddRootPackageNode(rootPackageNode);
        }

        private void LinkPackageData(UnrealPackage linker)
        {
            // ??? Move tables to their own dock page.
        }

        private void LinkSummaryData(ref UnrealPackage.PackageFileSummary summary)
        {
            if (summary.ImportCount != 0)
            {
                _PackageExplorerPage.PackageExplorerPanel.CreatePackageDependenciesNode(_UnrealPackage);
            }
        }

        private void InitializePackageObjects()
        {
            Debug.Assert(_UnrealPackage != null, nameof(_UnrealPackage) + " != null");
            if (_UnrealPackage.Names == null ||
                _UnrealPackage.Exports == null ||
                _UnrealPackage.Imports == null)
            {
                throw new UnrealException($"Invalid data for package {_UnrealPackage}");
            }

            ProgressStatus.ResetValue();
            int max = Program.Options.InitFlags.HasFlag(UnrealPackage.InitFlags.Construct)
                ? _UnrealPackage.Exports.Count + _UnrealPackage.Imports.Count
                : 0;

            if (Program.Options.InitFlags.HasFlag(UnrealPackage.InitFlags.Deserialize))
                max += _UnrealPackage.Exports.Count;

            if (Program.Options.InitFlags.HasFlag(UnrealPackage.InitFlags.Link))
                max += _UnrealPackage.Exports.Count;

            ProgressStatus.SetMaxProgress(max);
            ProgressStatus.Loading.Visible = true;

            Refresh();
            try
            {
                _UnrealPackage.NotifyPackageEvent += OnNotifyPackageEvent;
                _UnrealPackage.NotifyObjectAdded += OnNotifyObjectAdded;
                _UnrealPackage.InitializePackage(Program.Options.InitFlags);
                LinkPackageObjects();
            }
            catch (Exception exception)
            {
                throw new UnrealException($"Couldn't initialize package {_UnrealPackage}", exception);
            }
            finally
            {
                _UnrealPackage.NotifyObjectAdded -= OnNotifyObjectAdded;
                _UnrealPackage.NotifyPackageEvent -= OnNotifyPackageEvent;
            }

            InitializeContentTree();
        }

        private void OnNotifyObjectAdded(object sender, ObjectEventArgs e)
        {
        }

        private void LinkPackageObjects()
        {
        }

        private bool IsPackageCompressed()
        {
            return _UnrealPackage.Summary.CompressedChunks != null &&
                   _UnrealPackage.Summary.CompressedChunks.Any();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            Debug.WriteLine("Disposing UC_PackageExplorer " + disposing);

            ProgressStatus.ResetStatus();
            ProgressStatus.ResetValue();

            kryptonDockingManagerMain.SaveConfigToFile(Program.DockingConfigPath);

            if (_UnrealPackage != null)
            {
                _UnrealPackage.Dispose();
                _UnrealPackage = null;
            }

            base.Dispose(disposing);
        }

        private void OnNotifyPackageEvent(object sender, UnrealPackage.PackageEventArgs e)
        {
            switch (e.EventId)
            {
                case UnrealPackage.PackageEventArgs.Id.Construct:
                    ProgressStatus.SetStatus(Resources.CONSTRUCTING_OBJECTS);
                    break;

                case UnrealPackage.PackageEventArgs.Id.Deserialize:
                    ProgressStatus.SetStatus(Resources.DESERIALIZING_OBJECTS);
                    break;

                case UnrealPackage.PackageEventArgs.Id.Link:
                    ProgressStatus.SetStatus(Resources.LINKING_OBJECTS);
                    break;

                case UnrealPackage.PackageEventArgs.Id.Object:
                    ProgressStatus.IncrementValue();
                    break;
            }
        }

        private void InitializeContentTree()
        {
            Debug.Assert(_UnrealPackage != null);
            Debug.Assert(_UnrealPackage.Exports != null);

            _PackageExplorerPage.PackageExplorerPanel.BuildRootPackageTree(_UnrealPackage);

            var state = Program.Options.GetState(_UnrealPackage.FullPackageName);
            if (state.SearchObjectValue != null)
            {
                PerformActionByObjectPath(state.SearchObjectValue);
            }
        }

        // TODO: Deprecate
        internal void ReloadPackage()
        {
            string filePath = _UnrealPackage.FullPackageName;

            ((ProgramForm)ParentForm).Tabs.CloseTab((TabStripItem)Parent);
            ((ProgramForm)ParentForm).LoadFile(filePath);
        }

        private static string GetExceptionMessage(UObject errorObject)
        {
            return "Deserialization failed by the following exception:"
                   + "\n\n" + errorObject.ThrownException.Message
                   + "\n\n" + errorObject.ThrownException.InnerException?.Message
                   + "\n\n" + "Occurred on position:"
                   + errorObject.ExceptionPosition + "/" + errorObject.ExportTable.SerialSize
                   + "\n\nStackTrace:" + errorObject.ThrownException.StackTrace
                   + "\n\nInnerStackTrace:" + errorObject.ThrownException.InnerException?.StackTrace;
        }

        #region Node-ContextMenu Methods

        private static string FormatTokenHeader(UStruct.UByteCodeDecompiler.Token token, bool acronymizeName = true)
        {
            string name = token.GetType().Name;
            if (!acronymizeName)
                return $"{name}({token.Size}/{token.StorageSize})";

            name = string.Concat(name.Substring(0, name.Length - 5).Select(
                c => char.IsUpper(c) ? c.ToString(CultureInfo.InvariantCulture) : string.Empty
            ));

            return $"{name}({token.Size}/{token.StorageSize})";
        }

        private static string _DisassembleTokensTemplate;

        private static string DisassembleTokens(UStruct container, UStruct.UByteCodeDecompiler decompiler,
            int tokenCount)
        {
            var content = string.Empty;
            for (var i = 0; i + 1 < tokenCount; ++i)
            {
                var token = decompiler.NextToken;
                int firstTokenIndex = decompiler.CurrentTokenIndex;
                int lastTokenIndex;
                int subTokensCount;

                string value;
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
                    lastTokenIndex = decompiler.CurrentTokenIndex;
                    subTokensCount = lastTokenIndex - firstTokenIndex;
                    decompiler.CurrentTokenIndex = firstTokenIndex;
                }

                var buffer = new byte[token.StorageSize];
                container.Package.Stream.Position = container.ExportTable.SerialOffset + container.ScriptOffset +
                                                    token.StoragePosition;
                container.Package.Stream.Read(buffer, 0, buffer.Length);

                string header = FormatTokenHeader(token, false);
                string bytes = BitConverter.ToString(buffer).Replace('-', ' ');

                content += string.Format(_DisassembleTokensTemplate.Replace("%INDENTATION%", UDecompilingState.Tabs),
                    token.Position, token.StoragePosition,
                    header, bytes,
                    value != string.Empty ? value + "\r\n" : value, firstTokenIndex, lastTokenIndex
                );

                if (subTokensCount > 0)
                {
                    UDecompilingState.AddTab();
                    content += DisassembleTokens(container, decompiler, subTokensCount + 1);
                    i += subTokensCount;
                    UDecompilingState.RemoveTab();
                }
            }

            return content;
        }

        public object PickBestTarget(object target, ContextActionKind actionKind)
        {
            if (target == null)
                return null;

            object tag;
            switch (target)
            {
                // Redirect this node to its tag if set (i.e. Within -> Class)
                case TreeNode node:
                    tag = node.Tag;
                    if (tag is UObjectTableItem item && item.Object != null)
                    {
                        tag = item.Object;
                    }

                    break;

                // Probably an UObject
                default:
                    tag = target;
                    break;
            }

            return tag;
        }

        public ContextActionKind PickBestContentNodeAction(object tag, ContextActionKind actionKind)
        {
            if (actionKind != ContextActionKind.Auto)
            {
                return actionKind;
            }

            switch (tag)
            {
                case UPalette _:
                case UTexture _:
                case USound _:
                    return ContextActionKind.Open;

                case IUnrealDecompilable _:
                    return ContextActionKind.Decompile;

                case IBinaryData _:
                    return ContextActionKind.Binary;
            }

            return actionKind;
        }

        private void PerformNodeAction(object target, ContextActionKind actionKind, bool trackHistory = true)
        {
            object tag = PickBestTarget(target, actionKind);
            if (tag == null && target is TreeNode node)
            {
                switch (actionKind)
                {
                    //case ContentNodeAction.Decompile:
                    //    var s = new StringBuilder();
                    //    foreach (object subNode in node.Nodes)
                    //    {
                    //        if (subNode is IUnrealDecompilable decompilable)
                    //        {
                    //            s.AppendLine(decompilable.Decompile());
                    //        }
                    //    }
                    //    UpdateContentPanel(target, ContentNodeAction.Decompile);
                    //    if (trackHistory) TrackNodeAction(node, target, ContentNodeAction.Decompile);
                    //    return;

                    default:
                        return;
                }
            }

            //action = PickBestContentNodeAction(tag, action);
            if (trackHistory)
            {
                TrackNodeAction(target, tag, actionKind);
            }

            var obj = tag as UObject;
            if (obj != null)
            {
                // Ensure that we are about to perform an action on a deserialized object!
                if (obj.DeserializationState == 0)
                {
                    obj.BeginDeserializing();
                }
            }

            Debug.Assert(tag != null, nameof(tag) + " != null");
            try
            {
                switch (actionKind)
                {
                    case ContextActionKind.Auto:
                    case ContextActionKind.Open:
                    case ContextActionKind.Decompile:
                    case ContextActionKind.Binary:
                        UpdateContext(tag, actionKind);
                        break;

                    case ContextActionKind.DisassembleTokens:
                    {
                        var unStruct = tag as UStruct;
                        if (unStruct?.ByteCodeManager != null)
                        {
                            var codeDec = unStruct.ByteCodeManager;
                            codeDec.Deserialize();
                            codeDec.InitDecompile();

                            _DisassembleTokensTemplate = LoadTemplate("struct.tokens-disassembled");
                            string text = DisassembleTokens(unStruct, codeDec, codeDec.DeserializedTokens.Count);
                            UpdateContext(unStruct.Default, ContextActionKind.Decompile);
                        }

                        break;
                    }

                    case ContextActionKind.DecompileTokens:
                    {
                        var unStruct = tag as UStruct;
                        if (unStruct?.ByteCodeManager != null)
                        {
                            string tokensTemplate = LoadTemplate("struct.tokens");
                            var codeDec = unStruct.ByteCodeManager;
                            codeDec.Deserialize();
                            codeDec.InitDecompile();

                            var text = string.Empty;
                            while (codeDec.CurrentTokenIndex + 1 < codeDec.DeserializedTokens.Count)
                            {
                                var t = codeDec.NextToken;
                                int orgIndex = codeDec.CurrentTokenIndex;
                                string output;
                                var breakOut = false;
                                try
                                {
                                    output = t.Decompile();
                                }
                                catch
                                {
                                    output = "Exception occurred while decompiling token: " + t.GetType().Name;
                                    breakOut = true;
                                }

                                string chain = FormatTokenHeader(t);
                                int inlinedTokens = codeDec.CurrentTokenIndex - orgIndex;
                                if (inlinedTokens > 0)
                                {
                                    ++orgIndex;
                                    for (var i = 0; i < inlinedTokens; ++i)
                                        chain += " -> " + FormatTokenHeader(codeDec.DeserializedTokens[orgIndex + i]);
                                }

                                var buffer = new byte[t.StorageSize];
                                _UnrealPackage.Stream.Position = unStruct.ExportTable.SerialOffset +
                                                                 unStruct.ScriptOffset + t.StoragePosition;
                                _UnrealPackage.Stream.Read(buffer, 0, buffer.Length);

                                text += string.Format(tokensTemplate,
                                    t.Position, t.StoragePosition,
                                    chain, BitConverter.ToString(buffer).Replace('-', ' '),
                                    output != string.Empty ? output + "\r\n" : output
                                );

                                if (breakOut)
                                    break;
                            }

                            UpdateContext(unStruct.Default, ContextActionKind.Decompile);
                        }

                        break;
                    }

                    case ContextActionKind.ViewBuffer:
                    {
                        var bufferObject = (IBuffered)tag;
                        Debug.Assert(bufferObject != null, nameof(bufferObject) + " != null");
                        if (bufferObject.GetBufferSize() > 0) ViewBufferFor(bufferObject);
                        break;
                    }

                    case ContextActionKind.ViewTableBuffer:
                    {
                        var tableObject = (IContainsTable)tag;
                        Debug.Assert(tableObject != null);
                        ViewBufferFor(tableObject.Table);
                        break;
                    }

                    case ContextActionKind.ViewException:
                    {
                        Debug.Assert(obj != null);
                        string text = GetExceptionMessage(obj);
                        UpdateContext(obj, ContextActionKind.Decompile);
                        break;
                    }

                    case ContextActionKind.ExportAs:
                    {
                        var exportableObject = (IUnrealExportable)tag;
                        Debug.Assert(exportableObject != null);

                        obj?.BeginDeserializing();

                        var fileExtensions = string.Empty;
                        foreach (string ext in exportableObject.ExportableExtensions)
                        {
                            fileExtensions += $"{ext}(*" + $".{ext})|*.{ext}";
                            if (ext != exportableObject.ExportableExtensions.Last()) fileExtensions += "|";
                        }

                        var fileName = tag.ToString();
                        var dialog = new SaveFileDialog { Filter = fileExtensions, FileName = fileName };
                        if (dialog.ShowDialog() != DialogResult.OK)
                            return;

                        using (var stream = new FileStream(dialog.FileName, FileMode.Create, FileAccess.Write))
                        {
                            exportableObject.SerializeExport(
                                exportableObject.ExportableExtensions.ElementAt(dialog.FilterIndex - 1), stream);
                            stream.Flush();
                        }

                        break;
                    }

                    case ContextActionKind.DecompileExternal:
                    {
                        Process.Start
                        (
                            Program.Options.UEModelAppPath,
                            "-path=" + _UnrealPackage.PackageDirectory
                                     + " " + _UnrealPackage.PackageName
                                     + " " + ((TreeNode)target).Text
                        );
                        break;
                    }

                    case ContextActionKind.ExportExternal:
                    {
                        string packagePath = Application.StartupPath
                                             + "\\Exported\\"
                                             + _UnrealPackage.PackageName;

                        string contentDir = packagePath + "\\Content";
                        Directory.CreateDirectory(contentDir);
                        string appArguments = "-path=" + _UnrealPackage.PackageDirectory
                                                       + " " + "-out=" + contentDir
                                                       + " -export"
                                                       + " " + _UnrealPackage.PackageName
                                                       + " " + ((TreeNode)target).Text;
                        var appInfo = new ProcessStartInfo(Program.Options.UEModelAppPath, appArguments)
                        {
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            CreateNoWindow = false
                        };
                        var app = Process.Start(appInfo);
                        var log = string.Empty;
                        app.OutputDataReceived += (sender, e) => log += e.Data;
                        //app.WaitForExit();

                        if (Directory.GetFiles(contentDir).Length > 0)
                        {
                            if (MessageBox.Show(
                                    Resources.UC_PackageExplorer_PerformNodeAction_QUESTIONEXPORTFOLDER,
                                    ProductName,
                                    MessageBoxButtons.YesNo
                                ) == DialogResult.Yes)
                                Process.Start(contentDir);
                        }
                        else
                        {
                            MessageBox.Show
                            (
                                $"The object was not exported.\r\n\r\nArguments:{appArguments}\r\n\r\nLog:{log}",
                                ProductName
                            );
                        }

                        break;
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionDialog.Show("An exception occurred while performing: " + actionKind, e);
            }
        }

        [CanBeNull]
        private KryptonPage GetObjectPage()
        {
            return kryptonDockableWorkspaceMain
                .AllPages()
                .FirstOrDefault(p => p.Name == "ObjectPage");
        }

        [CanBeNull]
        private IActionPanel<object> GetObjectPanel()
        {
            var objectPanel = GetObjectPage()?.Controls["Panel"].Controls["ObjectPage"];
            return (IActionPanel<object>)objectPanel;
        }

        private ContextInfo UpdateContext([NotNull] object target, ContextActionKind actionKind)
        {
            Debug.Assert(target != null, nameof(target) + " != null");

            var context = new ContextInfo(actionKind, target);

            // TODO: Separate this into its own Control
            string path = ObjectPathBuilder.GetPath((dynamic)target);
            ProgressStatus.SetStatus(path);
            SetActiveObjectPath(path);
            // Give all current pages a chance to react to context changes. This has to happen before we create any new pages.
            var contextEvent = new ContextChangedEventArgs(context);
            contextService.OnContextChanged(contextEvent);
            // Insert default pages, if we don't have any active pages for this action kind.
            InsertDefaultPages(context);
            ProgressStatus.ResetStatus();

            // TODO: Bind this to the current active shown page
            bool withEditorTools = actionKind == ContextActionKind.Decompile;
            copyToolStripButton.Enabled = withEditorTools;
            EditorFindTextBox.Enabled = withEditorTools;
            findNextToolStripMenuItem.Enabled = withEditorTools;

            return context;
        }

        [CanBeNull]
        private TrackingPage CreatePageByAction(object target, ContextActionKind actionKind,
            bool isTracking)
        {
            switch (actionKind)
            {
                case ContextActionKind.Auto:
                    switch (target)
                    {
                        case IUnrealDecompilable _:
                            return CreateTrackingPage<DecompilerPage>(true);

                        case IBinaryData _:
                            return CreateTrackingPage<BinaryPage>(true);

                        default:
                            return null;
                    }

                case ContextActionKind.Open:
                {
                    switch (target)
                    {
                        case USound _:
                            return CreateTrackingPage<WavePlayerPage>(isTracking);

                        case UTexture _:
                        case UPalette _:
                            return CreateTrackingPage<ImageEditorPage>(isTracking);
                    }

                    return null;
                }

                case ContextActionKind.Decompile:
                    return CreateTrackingPage<DecompilerPage>(isTracking);

                case ContextActionKind.Binary:
                    return CreateTrackingPage<BinaryPage>(isTracking);

                default:
                    return null;
            }
        }

        private void InsertDefaultPages([NotNull] ContextInfo context)
        {
            if (context.ActionKindKind == ContextActionKind.Auto)
            {
                bool hasAny = kryptonDockingManagerMain.PagesWorkspace
                    .OfType<TrackingPage>()
                    .Any(p => p.IsTracking && p.CanAccept(context));

                if (hasAny)
                {
                    return;
                }
            }

            var isTracking = context.ActionKindKind == ContextActionKind.Auto;
            var page = CreatePageByAction(context.Target, context.ActionKindKind, isTracking);
            if (page == null) return;

            _Workspace.Append(page);
            _Workspace.SelectPage(page.UniqueName);

            page.Accept(context);
        }

        private static readonly string _TemplateDir = Path.Combine(Program.ConfigDir, "Templates");

        private static string LoadTemplate(string name)
        {
            return File.ReadAllText(Path.Combine(_TemplateDir, name + ".txt"), Encoding.ASCII);
        }

        #endregion

        public override void TabFind()
        {
            using (var findDialog = new FindDialog())
            {
                findDialog.FindNext += (sender, e) => { EmitFind(e.FindText); };
                findDialog.ShowDialog();
            }
        }

        private readonly List<ActionState> _ContentHistory = new List<ActionState>();
        private int _CurrentHistoryIndex = -1;

        private void SetActiveObjectPath(string path)
        {
            ActiveObjectPath.Text = path;
        }

        private void TrackNodeAction(object node, object target, ContextActionKind actionKind)
        {
            switch (actionKind)
            {
                case ContextActionKind.ExportAs:
                case ContextActionKind.ExportExternal:
                case ContextActionKind.DecompileExternal:
                case ContextActionKind.ViewBuffer:
                case ContextActionKind.ViewTableBuffer:
                    return;
            }

            if (_ContentHistory.Count > 0)
            {
                // No need to buffer the same content
                var last = _ContentHistory.Last();
                if (last.Target == target && last.ActionKind == actionKind)
                {
                    return;
                }

                // Preserve the text editor state of the last action.
                UpdateContentHistoryData(_CurrentHistoryIndex);

                // Clean all above buffers when a new node was user-selected
                if (_ContentHistory.Count - 1 - _CurrentHistoryIndex > 0)
                {
                    _ContentHistory.RemoveRange(_CurrentHistoryIndex, _ContentHistory.Count - _CurrentHistoryIndex);
                    _CurrentHistoryIndex = _ContentHistory.Count - 1;
                    NextButton.Enabled = false;
                }
            }

            // Maximum 10 can be buffered; remove last one
            if (_ContentHistory.Count + 1 > 15)
                _ContentHistory.RemoveRange(0, 1);

            var data = new ActionState
            {
                SelectedNode = node,
                Target = target,
                ActionKind = actionKind
            };
            _ContentHistory.Add(data);
            _CurrentHistoryIndex = _ContentHistory.Count - 1;

            if (_CurrentHistoryIndex > 0) PrevButton.Enabled = true;
        }

        private void UpdateContentHistoryData(int historyIndex)
        {
            var data = _ContentHistory[historyIndex];
            var panel = GetObjectPanel();
            panel?.StoreState(ref data);
            _ContentHistory[historyIndex] = data;
        }

        private void RestoreContentHistoryData(int historyIndex)
        {
            var data = _ContentHistory[historyIndex];
            PerformNodeAction(data.Target, data.ActionKind, false);

            if (data.SelectedNode is TreeNode selectedNode)
            {
                RestoreSelectedNode(selectedNode);
            }

            var panel = GetObjectPanel();
            panel?.RestoreState(ref data);
        }

        private void ToolStripButton_Backward_Click(object sender, EventArgs e)
        {
            Debug.Assert(_CurrentHistoryIndex - 1 >= 0);

            UpdateContentHistoryData(_CurrentHistoryIndex);
            RestoreContentHistoryData(--_CurrentHistoryIndex);

            if (_CurrentHistoryIndex == 0) PrevButton.Enabled = false;
            NextButton.Enabled = true;
        }

        private void ToolStripButton_Forward_Click(object sender, EventArgs e)
        {
            Debug.Assert(_CurrentHistoryIndex + 1 < _ContentHistory.Count);

            UpdateContentHistoryData(_CurrentHistoryIndex);
            RestoreContentHistoryData(++_CurrentHistoryIndex);

            if (_CurrentHistoryIndex == _ContentHistory.Count - 1) NextButton.Enabled = false;
            PrevButton.Enabled = true;
        }

        private void RestoreSelectedNode(TreeNode node)
        {
            if (node == null)
                return;

            node.TreeView.Show();
            node.TreeView.SelectedNode = node;
        }

        private void ViewBufferFor(IBuffered target)
        {
            if (target == null)
            {
                MessageBox.Show(Resources.NoTargetForViewBuffer);
                return;
            }

            var hexDialog = new HexViewerForm(target, this);
            hexDialog.Show(ParentForm);
        }

        private void ReloadPackage_Click(object sender, EventArgs e)
        {
            ReloadPackage();
        }

        private FindResultsPage CreateFindPage()
        {
            var page = new FindResultsPage();
            return page;
        }

        public async void PerformSearchIn<T>(string searchText) where T : UObject
        {
            Debug.Assert(_UnrealPackage != null, nameof(_UnrealPackage) + " != null");

            var findPage = CreateFindPage();
            kryptonDockingManagerMain.AddDockspace("Nav", DockingEdge.Left, new KryptonPage[] { findPage });
            kryptonDockingManagerMain.ShowPage(findPage);

            var contents = _UnrealPackage.Objects
                .OfType<T>()
                .Where(c => c.ExportTable != null)
                .ToList<IUnrealDecompilable>();

            ProgressStatus.SaveStatus();
            ProgressStatus.SetStatus(Resources.SEARCHING_CLASSES_STATUS);
            try
            {
                await findPage.PerformSearch(contents, searchText);
            }
            finally
            {
                ProgressStatus.Reset();
            }
        }

        private void FindInDocumentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditorFindTextBox.Focus();
        }

        private void EditorFindTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\r')
                return;

            EmitFind(EditorFindTextBox.Text);
            e.Handled = true;
        }

        private void findNextToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            EmitFind(EditorFindTextBox.Text);
        }

        private void copyToolStripButton_Click(object sender, EventArgs e)
        {
            // FIXME:
        }

        // TODO: Re-implement in a package agnostic way (Blocked till UELib 2.0)
        private bool PerformActionByObjectPath(string objectGroup)
        {
            Debug.Assert(_UnrealPackage != null, nameof(_UnrealPackage) + " != null");

            var obj = _UnrealPackage.FindObjectByGroup(objectGroup);
            if (obj == null)
                return false;

            PerformNodeAction(obj, ContextActionKind.Auto);
            return true;
        }

        private void SearchObjectTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case '\r':
                    e.Handled = PerformActionByObjectPath(ActiveObjectPath.Text);
                    break;
            }
        }

        internal static UC_PackageExplorer Traverse(Control parent)
        {
            for (var c = parent; c != null; c = c.Parent)
            {
                if (c is UC_PackageExplorer packageExplorer) return packageExplorer;
            }

            throw new NotSupportedException();
        }

        public void EmitObjectNodeAction(object target, ContextActionKind actionKind)
        {
            PerformNodeAction(target, actionKind);
        }

        public void EmitSearchObjectByPath(string path)
        {
            PerformActionByObjectPath(path);
        }

        public void EmitSearch<T>(string searchText) where T : UObject
        {
            PerformSearchIn<T>(searchText);
        }

        // FIXME: Find a better way to propagate commands down the docked pages.

        public void EmitFind(string text)
        {
            var pages = kryptonDockingManagerMain.PagesWorkspace
                .OfType<TrackingPage>()
                .Where(p => p.IsTracking)
                .ToList();

            foreach (var page in pages)
            {
                page.OnFind(text);
            }
        }

        public void EmitFind(TextSearchHelpers.FindResult findResult)
        {
            var pages = kryptonDockingManagerMain.PagesWorkspace
                .OfType<TrackingPage>()
                .Where(p => p.IsTracking)
                .ToList();

            foreach (var page in pages)
            {
                page.OnFind(findResult);
            }
        }

        private void kryptonDockingManagerMain_PageFloatingRequest(object sender, CancelUniqueNameEventArgs e)
        {
            var page = kryptonDockingManagerMain.PageForUniqueName(e.UniqueName);
            if (page is TrackingPage trackingPage && trackingPage.IsTracking)
            {
                trackingPage.IsTracking = false;
            }
        }

        private void kryptonDockingManagerMain_PageCloseRequest(object sender, CloseRequestEventArgs e)
        {
            var page = kryptonDockingManagerMain.PageForUniqueName(e.UniqueName);
            if (page is TrackingPage trackingPage)
            {
                page.Dispose();
            }
        }
    }
}