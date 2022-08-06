using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Krypton.Navigator;
using Krypton.Docking;
using UEExplorer.Properties;
using UEExplorer.UI.ActionPanels;
using UEExplorer.UI.Forms;
using UEExplorer.UI.Nodes;
using UEExplorer.UI.Pages;
using UELib.Annotations;

namespace UEExplorer.UI.Tabs
{
    using Dialogs;
    using UELib;
    using UELib.Core;

    [System.Runtime.InteropServices.ComVisible(false)]
    public partial class UC_PackageExplorer : UserControl_Tab
    {
        public string FilePath { get; set; }

        /// <summary>
        /// Null if the user cancels the exception popup.
        /// </summary>
        [CanBeNull] private UnrealPackage _UnrealPackage;

        internal static string DockingConfigPath = Path.Combine(Application.StartupPath, "Docking.xml");

        private PackageExplorerPage _PackageExplorerPage;

        private void UC_PackageExplorer_Load(object sender, EventArgs e)
        {
            kryptonDockingManagerMain.ManageFloating("Floating", ParentForm);

            //if (File.Exists(DockingPath)) kryptonDockingManagerMain.LoadConfigFromFile(DockingPath);

            kryptonDockingManagerMain.ManageControl("Nav", packageStripContainer.ContentPanel);
            _PackageExplorerPage = CreatePackageExplorerPage();
            
            kryptonDockingManagerMain.AddDockspace(
                "Nav",
                DockingEdge.Left,
                new KryptonPage[] { _PackageExplorerPage });

            var defaultDecompilerPage = CreateDecompilerPage(null);
            defaultDecompilerPage.IsDefault = true;
            
            var workSpace = kryptonDockingManagerMain.ManageWorkspace("Workspace", kryptonDockableWorkspaceMain);
            kryptonDockingManagerMain.AddToWorkspace("Workspace", new KryptonPage[] { defaultDecompilerPage });

            var defaultBinaryPage = CreateBinaryPage(null);
            defaultBinaryPage.IsDefault = true;
            
            var c = kryptonDockingManagerMain.ManageControl("View", packageStripContainer.ContentPanel);
            kryptonDockingManagerMain.AddDockspace(
                "View",
                DockingEdge.Bottom,
                new KryptonPage[] { CreateBinaryPage(null) });
        }

        private PackageExplorerPage CreatePackageExplorerPage()
        {
            var page = new PackageExplorerPage();
            page.ClearFlags(KryptonPageFlags.DockingAllowClose | KryptonPageFlags.DockingAllowFloating | KryptonPageFlags.DockingAllowWorkspace);
            return page;
        }

        private DecompilerPage CreateDecompilerPage(object target, bool isDefault = true)
        {
            var page = new DecompilerPage
            {
                IsDefault = isDefault
            };
            page.ClearFlags(KryptonPageFlags.DockingAllowAutoHidden);
            page.SetNewObjectTarget(target, ContentNodeAction.Decompile, false);
            return page;
        }

        private BinaryPage CreateBinaryPage(object target, bool isDefault = true)
        {
            var page = new BinaryPage
            {
                IsDefault = isDefault
            };
            //page.ClearFlags(KryptonPageFlags.DockingAllowAutoHidden);
            page.SetNewObjectTarget(target, ContentNodeAction.Decompile, false);
            return page;
        }

        public void PostInitialize()
        {
            try
            {
                LoadPackageData();
            }
            catch (Exception exception)
            {
                throw new UnrealException("Couldn't load or initialize package", exception);
            }
        }

        private void LoadPackageData()
        {
            if (Program.Options.bForceLicenseeMode)
                UnrealPackage.OverrideLicenseeVersion = Program.Options.LicenseeMode;

            if (Program.Options.bForceVersion) 
                UnrealPackage.OverrideVersion = Program.Options.Version;

            UnrealConfig.SuppressSignature = false;
            UnrealConfig.Platform = (UnrealConfig.CookedPlatform)Enum.Parse
            (
                typeof(UnrealConfig.CookedPlatform),
                Tabs.Form.Platform.Text, true
            );
            
            // Open the file.
        reload:
            ProgressStatus.SetStatus(Resources.PACKAGE_LOADING);
            // Note: We cannot dispose neither the stream nor package, because we require the stream to be hot so that we can lazily-deserialize objects.
            var stream = new UPackageStream(FilePath, FileMode.Open, FileAccess.Read);
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
                        MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    Tabs.Remove(this, true);
                    return;
                }

                UnrealConfig.SuppressSignature = true;
                goto reload;
            }

            PreInitializeContentTree(_UnrealPackage);

            Contract.Assert(_UnrealPackage != null, "Package is null");
            _UnrealPackage.Deserialize(stream);
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
            InitializeUI();
        }

        // Pre-initialize, this package has not been serialized yet, but we can initialize pre-assumed nodes here.
        private void PreInitializeContentTree(UnrealPackage linker)
        {
            var rootPackageNode = _PackageExplorerPage.PackageExplorerPanel.CreateRootPackageNode(_UnrealPackage);
            _PackageExplorerPage.PackageExplorerPanel.AddRootPackageNode(rootPackageNode);
        }
        
        private void LinkPackageData(UnrealPackage linker)
        {
            uNameTableItemBindingSource.DataSource = linker.Names;
            uImportTableItemBindingSource.DataSource = linker.Imports;
            uExportTableItemBindingSource.DataSource = linker.Exports;
        }

        private void LinkSummaryData(ref UnrealPackage.PackageFileSummary summary)
        {
            uGenerationTableItemBindingSource.DataSource = summary.Generations;
            compressedChunkBindingSource.DataSource = summary.CompressedChunks;

            if (!IsPackageCompressed())
            {
                TabPage_Chunks.Enabled = false;
                TabControl_Tables.Controls.Remove(TabPage_Chunks);
            }

            if (summary.Generations == null || summary.Generations.Count == 0)
            {
                TabPage_Generations.Enabled = false;
                TabControl_Tables.Controls.Remove(TabPage_Generations);
            }

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
                throw new UnrealException($"Invalid data for package {FilePath}");
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
                _UnrealPackage.NotifyPackageEvent += _OnNotifyPackageEvent;
                _UnrealPackage.NotifyObjectAdded += _OnNotifyObjectAdded;
                _UnrealPackage.InitializePackage(Program.Options.InitFlags);
                LinkPackageObjects();
            }
            catch (Exception exception)
            {
                throw new UnrealException($"Couldn't initialize package {FilePath}", exception);
            }
            finally
            {
                _UnrealPackage.NotifyObjectAdded -= _OnNotifyObjectAdded;
                _UnrealPackage.NotifyPackageEvent -= _OnNotifyPackageEvent;
            }

            InitializeContentTree();
        }

        private void _OnNotifyObjectAdded(object sender, ObjectEventArgs e)
        {
        }

        private void LinkPackageObjects()
        {
            if (_UnrealPackage.Imports.Any(exp => exp.ClassName == "TextBuffer"))
            {
                exportScriptClassesToolStripMenuItem.Enabled = true;
            }

            if (_UnrealPackage.Exports.Any(exp => exp.ClassIndex == 0))
            {
                findInClassesToolStripMenuItem.Enabled = true;
                exportDecompiledClassesToolStripMenuItem.Enabled = true;
            }
        }
        
        private bool IsPackageCompressed()
        {
            return _UnrealPackage.Summary.CompressedChunks != null &&
                   _UnrealPackage.Summary.CompressedChunks.Any();
        }

        public override void TabClosing()
        {
            base.TabClosing();

            ProgressStatus.ResetStatus();
            ProgressStatus.ResetValue();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            Debug.WriteLine("Disposing UC_PackageExplorer " + disposing);
            
            if (_UnrealPackage != null)
            {
                _UnrealPackage.Dispose();
                _UnrealPackage = null;
            }
            
            kryptonDockingManagerMain.SaveConfigToFile(Path.Combine(Application.StartupPath, "Docking.xml"));

            base.Dispose(disposing);
        }

        private void _OnNotifyPackageEvent(object sender, UnrealPackage.PackageEventArgs e)
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

        private void InitializeUI()
        {
            ProgressStatus.SetStatus(Resources.INITIALIZING_UI);
            SuspendLayout();
            exportDecompiledClassesToolStripMenuItem.Click += _OnExportClassesClick;
            exportScriptClassesToolStripMenuItem.Click += _OnExportScriptsClick;

            var state = Program.Options.GetState(_UnrealPackage.FullPackageName);
            if (state.SearchObjectValue != null)
            {
                PerformActionByObjectPath(state.SearchObjectValue);
            }

            ResumeLayout();
        }

        private void InitializeContentTree()
        {
            Debug.Assert(_UnrealPackage != null);
            Debug.Assert(_UnrealPackage.Exports != null);

            _PackageExplorerPage.PackageExplorerPanel.BuildRootPackageTree(_UnrealPackage);
        }

        private void _OnExportClassesClick(object sender, EventArgs e)
        {
            DoExportPackageClasses();
        }

        private void _OnExportScriptsClick(object sender, EventArgs e)
        {
            DoExportPackageClasses(true);
        }

        private void DoExportPackageClasses(bool exportScripts = false)
        {
            string exportPath = _UnrealPackage.ExportPackageClasses(exportScripts);
            var dialogResult = MessageBox.Show(
                string.Format(
                    Resources.EXPORTED_ALL_PACKAGE_CLASSES,
                    _UnrealPackage.PackageName,
                    exportPath
                ),
                Application.ProductName,
                MessageBoxButtons.YesNo
            );
            if (dialogResult == DialogResult.Yes) Process.Start(exportPath);
        }

        internal void ReloadPackage()
        {
            Tabs.Remove(this, true);
            Tabs.Form.LoadFile(FilePath);
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

        private ObjectActionsBuilder _ActionsBuilder = new ObjectActionsBuilder();
        
        private void ViewTools_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (CurrentContentTarget == null)
                return;

            Enum.TryParse(e.ClickedItem.Name, out ContentNodeAction action);
            PerformNodeAction(CurrentContentTarget, action);
        }

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

        public object PickBestTarget(object target, ContentNodeAction action)
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

        public ContentNodeAction PickBestContentNodeAction(object tag, ContentNodeAction action)
        {
            switch (tag)
            {
                case IUnrealDecompilable _:
                    if (action == ContentNodeAction.Auto) return ContentNodeAction.Decompile;
                    break;

                case IBinaryData _:
                    if (action == ContentNodeAction.Auto) return ContentNodeAction.Binary;
                    break;
            }

            return action;
        }

        private void PerformNodeAction(object target, ContentNodeAction action, bool trackHistory = true)
        {
            object tag = PickBestTarget(target, action);
            if (tag == null && target is TreeNode node)
            {
                switch (action)
                {
                    case ContentNodeAction.Decompile:
                        var s = new StringBuilder();
                        foreach (object subNode in node.Nodes)
                        {
                            if (subNode is IUnrealDecompilable decompilable)
                            {
                                s.AppendLine(decompilable.Decompile());
                            }
                        }
                        UpdateContentPanel(target, ContentNodeAction.Decompile, s.ToString());
                        if (trackHistory) TrackNodeAction(node, target, ContentNodeAction.Decompile);
                        return;
                
                    default:
                        return;
                }
            }

            action = PickBestContentNodeAction(tag, action);
            if (trackHistory)
            {
                TrackNodeAction(target, tag, action);
            }
            
            var obj = tag as UObject;
            try
            {
                switch (action)
                {
                    case ContentNodeAction.DecompileExternal:
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

                    case ContentNodeAction.ExportExternal:
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
                                    Application.ProductName,
                                    MessageBoxButtons.YesNo
                                ) == DialogResult.Yes)
                                Process.Start(contentDir);
                        }
                        else
                        {
                            MessageBox.Show
                            (
                                $"The object was not exported.\r\n\r\nArguments:{appArguments}\r\n\r\nLog:{log}",
                                Application.ProductName
                            );
                        }

                        break;
                    }

                    case ContentNodeAction.Decompile:
                        UpdateContentPanel(tag, ContentNodeAction.Decompile);
                        break;

                    case ContentNodeAction.Binary:
                        UpdateContentPanel(tag, action);
                        break;

                    case ContentNodeAction.DecompileOuter:
                        Debug.Assert(obj != null);
                        Debug.Assert(obj.Outer != null, "obj.Outer != null");
                        UpdateContentPanel(obj.Outer, ContentNodeAction.Decompile);
                        break;

                    case ContentNodeAction.DecompileClassReplication:
                    {
                        if (tag is UClass replicationClass)
                        {
                            string text = replicationClass.FormatReplication();
                            UpdateContentPanel(replicationClass, ContentNodeAction.Decompile, text);
                        }

                        break;
                    }

                    case ContentNodeAction.DecompileScriptProperties:
                    {
                        if (tag is UStruct unStruct)
                        {
                            string text = unStruct.Default is UStruct ? unStruct.FormatDefaultProperties() : null;
                            UpdateContentPanel(unStruct.Default, ContentNodeAction.Decompile, text);
                        }

                        break;
                    }

                    case ContentNodeAction.DisassembleTokens:
                    {
                        var unStruct = tag as UStruct;
                        if (unStruct?.ByteCodeManager != null)
                        {
                            var codeDec = unStruct.ByteCodeManager;
                            codeDec.Deserialize();
                            codeDec.InitDecompile();

                            _DisassembleTokensTemplate = LoadTemplate("struct.tokens-disassembled");
                            string text = DisassembleTokens(unStruct, codeDec, codeDec.DeserializedTokens.Count);
                            UpdateContentPanel(unStruct.Default, ContentNodeAction.Decompile, text);
                        }

                        break;
                    }

                    case ContentNodeAction.DecompileTokens:
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

                            UpdateContentPanel(unStruct.Default, ContentNodeAction.Decompile, text);
                        }
                        
                        break;
                    }

                    case ContentNodeAction.ViewBuffer:
                    {
                        var bufferObject = (IBuffered)tag;
                        Debug.Assert(bufferObject != null, nameof(bufferObject) + " != null");
                        if (bufferObject.GetBufferSize() > 0) ViewBufferFor(bufferObject);
                        break;
                    }

                    case ContentNodeAction.ViewTableBuffer:
                    {
                        var tableObject = (IContainsTable)tag;
                        Debug.Assert(tableObject != null);
                        ViewBufferFor(tableObject.Table);
                        break;
                    }

                    case ContentNodeAction.ViewException:
                    {
                        Debug.Assert(obj != null);
                        UpdateContentPanel(obj, ContentNodeAction.Decompile, GetExceptionMessage(obj));
                        break;
                    }

                    case ContentNodeAction.ExportAs:
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
                }
            }
            catch (Exception e)
            {
                ExceptionDialog.Show("An exception occurred while performing: " + action, e);
            }
        }

        [CanBeNull]
        private KryptonPage GetObjectPage()
        {
            return kryptonDockableWorkspaceMain.AllPages().FirstOrDefault(p => p.Name == "ObjectPage");
        }

        [CanBeNull]
        private IActionPanel<object> GetObjectPanel()
        {
            var objectPanel = GetObjectPage()?.Controls["Panel"].Controls["ObjectPage"];
            return (IActionPanel<object>)objectPanel;
        }

        private void UpdateContentPanel([NotNull] object target, ContentNodeAction action, [CanBeNull] string contentText = null)
        {
            Debug.Assert(target != null, nameof(target) + " != null");

            CurrentContentTarget = target;

            string path = ObjectPathBuilder.GetPath((dynamic)target);
            ProgressStatus.SetStatus(path);
            SetActiveObjectPath(path);
            SwitchContentPanel(target, action);
            ProgressStatus.ResetStatus();

            bool withEditorTools = action == ContentNodeAction.Decompile;
            textEditorToolstrip.Enabled = withEditorTools;

            //saveToolStripButton.Enabled = withEditorTools;
            //copyToolStripButton.Enabled = withEditorTools;
            //findNextToolStripMenuItem.Enabled = withEditorTools;
            //EditorFindTextBox.Enabled = withEditorTools;
        }

        public void InsertNewContentPanel(object target, ContentNodeAction action)
        {
            InsertContentPanel(target, action, false);
        }

        private void InsertContentPanel(object target, ContentNodeAction action, bool isDefault)
        {
            KryptonPage page;
            switch (action)
            {
                case ContentNodeAction.Decompile:
                    page = CreateDecompilerPage(target, isDefault);
                    break;

                case ContentNodeAction.Binary:
                    page = CreateBinaryPage(target, isDefault);
                    break;

                default:
                    throw new NotSupportedException();
            }

            kryptonDockingManagerMain.AddToWorkspace("Workspace", new[] { page });
        }

        private void SwitchContentPanel(object target, ContentNodeAction action)
        {
            List<ObjectBoundPage> pages;
            switch (action)
            {
                // For decompilation we want to only auto update default workspace pages.
                case ContentNodeAction.Decompile:
                    pages = kryptonDockingManagerMain.PagesWorkspace
                        .OfType<ObjectBoundPage>()
                        .Where(p => p.IsDefault)
                        .ToList();
                    SwitchContentPanel(target, ContentNodeAction.Binary);
                    break;
                
                case ContentNodeAction.Binary:
                    pages = kryptonDockingManagerMain.Pages
                        .OfType<ObjectBoundPage>()
                        .Where(p => p.IsDefault)
                        .ToList();

                    break;
                
                default:
                    throw new NotSupportedException();
            }
            
            if (pages.Any())
            {
                foreach (var page in pages)
                {
                    bool isPending = !kryptonDockingManagerMain.IsPageShowing(page);
                    page.SetNewObjectTarget(target, action, isPending);
                }
            }
            else
            {
                InsertContentPanel(target, action, true);
            }
        }

        private void SearchDocument_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var panel = (DecompileOutputPanel)GetObjectPanel();
            Debug.Assert(panel != null);
            
            EditorFindTextBox.Text = panel.TextEditorPanel.TextEditorControl.TextEditor.TextArea.Selection.GetText();
            //FindButton.PerformClick();
        }

        private void SearchClasses_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var panel = (DecompileOutputPanel)GetObjectPanel();
            Debug.Assert(panel != null);
            
            EditorFindTextBox.Text = panel.TextEditorPanel.TextEditorControl.TextEditor.TextArea.Selection.GetText();
            findInClassesToolStripMenuItem.PerformClick();
        }

        private static readonly string _TemplateDir = Path.Combine(Program.ConfigDir, "Templates");

        private static string LoadTemplate(string name)
        {
            return File.ReadAllText(Path.Combine(_TemplateDir, name + ".txt"), Encoding.ASCII);
        }

        #endregion

        public override void TabFind()
        {
            //using (var findDialog = new FindDialog(textEditorPanel.TextEditorControl))
            //{
            //    findDialog.ShowDialog();
            //}
        }

        private readonly List<ActionState> _ContentHistory = new List<ActionState>();
        private int _CurrentHistoryIndex = -1;

        private object _CurrentContentTarget;
        public object CurrentContentTarget
        {
            get => _CurrentContentTarget;
            set => _CurrentContentTarget = value;
        }

        private void SetActiveObjectPath(string path)
        {
            ActiveObjectPath.Text = path;
        }

        private void TrackNodeAction(object node, object target, ContentNodeAction action)
        {
            switch (action)
            {
                case ContentNodeAction.ExportAs:
                case ContentNodeAction.ExportExternal:
                case ContentNodeAction.DecompileExternal:
                case ContentNodeAction.ViewBuffer:
                case ContentNodeAction.ViewTableBuffer:
                    return;
            }
            
            if (_ContentHistory.Count > 0)
            {
                // No need to buffer the same content
                var last = _ContentHistory.Last();
                if (last.Target == target && last.Action == action)
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
                Action = action
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
            CurrentContentTarget = data.Target;
            PerformNodeAction(data.Target, data.Action, false);
            
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
            hexDialog.Show(Tabs.Form);
        }

        private void ReloadPackage_Click(object sender, EventArgs e)
        {
            ReloadPackage();
        }

        private KryptonPage CreateFindPage(string text)
        {
            var page = new KryptonPage()
            {
                TextTitle = text
            };
            return page;
        }

        // TODO: Rework with docking
        private void PerformFindInClasses(string searchText)
        {
            Debug.Assert(_UnrealPackage != null, nameof(_UnrealPackage) + " != null");
            
            ProgressStatus.SaveStatus();
            ProgressStatus.SetStatus(Resources.SEARCHING_CLASSES_STATUS);

            var documentResults = new List<TextSearchHelpers.DocumentResult>();
            foreach (var content in _UnrealPackage.Objects
                         .OfType<UClass>()
                         .Where(c => c.ExportTable != null))
            {
                string findContent = content.Decompile();
                var findResults = TextSearchHelpers.FindText(findContent, searchText);
                if (!findResults.Any()) continue;

                var document = new TextSearchHelpers.DocumentResult
                {
                    Results = findResults,
                    Document = content
                };
                documentResults.Add(document);
            }

            ProgressStatus.Reset();

            if (documentResults.Count == 0)
            {
                MessageBox.Show(string.Format(Resources.NO_FIND_RESULTS, searchText));
                return;
            }

            var findPage = CreateFindPage(string.Format(Resources.FIND_RESULTS_TITLE, searchText));
            kryptonDockingManagerMain.AddDockspace("Nav", DockingEdge.Left, new KryptonPage[] { findPage });

            var treeResults = new TreeView
            {
                Dock = DockStyle.Fill,
            };
            findPage.Controls.Add(treeResults);

            kryptonDockingManagerMain.ShowPage(findPage);

            var imageKeySelector = new ObjectImageKeySelector();
            foreach (var documentResult in documentResults)
            {
                var @class = (UClass)documentResult.Document;
                string imageKey = @class.Accept(imageKeySelector);

                string text = ObjectPathBuilder.GetPath(@class);
                var documentNode = treeResults.Nodes.Add(text);
                documentNode.Tag = documentResult;
                documentNode.ImageKey = imageKey;
                documentNode.SelectedImageKey = imageKey;
                foreach (var result in documentResult.Results)
                {
                    var resultNode = documentNode.Nodes.Add(result.ToString());
                    resultNode.Tag = result;
                }
            }

            treeResults.AfterSelect += (nodeSender, nodeEvent) =>
            {
                // Assuming this was triggered by assigning SelectNode
                if (nodeEvent.Action == TreeViewAction.Unknown)
                {
                    return;
                }

                if (!(nodeEvent.Node.Tag is TextSearchHelpers.FindResult findResult)) return;

                if (nodeEvent.Node.Parent.Tag is TextSearchHelpers.DocumentResult documentResult)
                {
                    var unClass = (UClass)documentResult.Document;
                    PerformNodeAction(unClass, ContentNodeAction.Decompile);

                    // FIXME: what a mess...
                    //var panel = (DecompileOutputPanel)GetObjectPanel();
                    //Debug.Assert(panel != null);
                    //panel.TextEditorPanel.TextEditorControl.TextEditor.ScrollTo(findResult.TextLine, findResult.TextColumn);
                    //panel.TextEditorPanel.TextEditorControl.TextEditor.Select(findResult.TextIndex, searchText.Length);
                }
            };
        }

        private void FindInClassesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Debug.Assert(_UnrealPackage != null, nameof(_UnrealPackage) + " != null");
            
            string findText;
            using (var findDialog = new FindDialog())
            {
                findDialog.FindInput.Text = EditorFindTextBox.Text;
                if (findDialog.ShowDialog() != DialogResult.OK) return;
                findText = findDialog.FindInput.Text;
            }

            PerformFindInClasses(findText);
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            var panel = (DecompileOutputPanel)GetObjectPanel();
            Debug.Assert(panel != null);

            using (var sfd = new SaveFileDialog
                   {
                       DefaultExt = "uc",
                       Filter = $"{Resources.UnrealClassFilter}(*.uc)|*.uc",
                       FilterIndex = 1,
                       Title = Resources.ExportTextTitle,
                       FileName = ActiveObjectPath.Text + UnrealExtensions.UnrealCodeExt
                   })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                    File.WriteAllText(sfd.FileName, panel.TextEditorPanel.TextEditorControl.TextEditor.Text);
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

            var panel = (DecompileOutputPanel)GetObjectPanel();
            Debug.Assert(panel != null);
            
            EditorUtil.FindText(panel.TextEditorPanel.TextEditorControl.TextEditor, EditorFindTextBox.Text);
            e.Handled = true;
        }

        private void FindNextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var panel = (DecompileOutputPanel)GetObjectPanel();
            Debug.Assert(panel != null);

            EditorUtil.FindText(panel.TextEditorPanel.TextEditorControl.TextEditor, EditorFindTextBox.Text);
        }

        private void copyToolStripButton_Click(object sender, EventArgs e)
        {
            var panel = (DecompileOutputPanel)GetObjectPanel();
            Debug.Assert(panel != null);
            
            panel.TextEditorPanel.TextEditorControl.TextEditor.Copy();
        }

        private bool PerformActionByObjectPath(string objectGroup)
        {
            Debug.Assert(_UnrealPackage != null, nameof(_UnrealPackage) + " != null");
            
            var obj = _UnrealPackage.FindObjectByGroup(objectGroup);
            if (obj == null) 
                return false;
            
            PerformNodeAction(obj, ContentNodeAction.Auto);
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

        public void OnObjectNodeAction(object target, ContentNodeAction action)
        {
            PerformNodeAction(target, action);
        }

        public void OnSearchObjectByPath(string path)
        {
            PerformActionByObjectPath(path);
        }

        public void OnSearchInClasses(string searchText)
        {
            PerformFindInClasses(searchText);
        }
    }
}