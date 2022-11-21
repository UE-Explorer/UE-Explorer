using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Krypton.Docking;
using Krypton.Navigator;
using UEExplorer.Framework;
using UEExplorer.Properties;
using UEExplorer.UI.Dialogs;
using UEExplorer.UI.Forms;
using UEExplorer.UI.Main;
using UEExplorer.UI.Pages;
using UELib;
using UELib.Annotations;
using UELib.Core;
using UELib.Engine;

namespace UEExplorer.UI.Tabs
{
    public partial class UC_PackageExplorer : UserControl
    {
        private readonly List<ActionState> _ContentHistory = new List<ActionState>();
        private int _CurrentHistoryIndex = -1;
        private KryptonDockingWorkspace _DocumentsWorkspace;
        private KryptonDockingSpace _ExplorerSpace;

        private KryptonDockingControl _NavSpace;
        private KryptonDockingControl _ViewSpace;

        public UC_PackageExplorer()
        {
            InitializeComponent();

            contextService.ContextChanged += ContextServiceOnContextChanged;
        }

        public string FilePath { get; set; }

        private void UC_PackageExplorer_Load(object sender, EventArgs e)
        {
            //kryptonDockingManagerMain.ManageFloating("Floating", ParentForm);

            _NavSpace = kryptonDockingManagerMain.ManageControl("Nav", dockingPanel);
            _DocumentsWorkspace = kryptonDockingManagerMain.ManageWorkspace("Workspace", "Docs", kryptonDockableWorkspaceMain);
            _ViewSpace = kryptonDockingManagerMain.ManageControl("View", dockingPanel);

            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                if (File.Exists(Program.DockingConfigPath))
                {
                    kryptonDockingManagerMain.LoadConfigFromFile(Program.DockingConfigPath);
                }

                packageManager.PackageRegistered += PackageManagerOnPackageRegistered;
                packageManager.PackageLoaded += PackageManagerOnPackageLoaded;
                packageManager.PackageInitialized += PackageManagerOnPackageInitialized;
            }
        }

        private void ContextServiceOnContextChanged(object sender, ContextChangedEventArgs e)
        {
            if (sender == this)
            {
                return;
            }

            PerformNodeAction(e.Context.Target, e.Context.ActionKind);
        }

        private void PackageManagerOnPackageRegistered(object sender, PackageEventArgs e)
        {
            ProgressStatus.SetStatus(Resources.PACKAGE_LOADING);

            var packageReference = e.Package;
            Debug.Assert(packageReference.Linker == null, "packageReference.Linker == null");

            UnrealConfig.SuppressSignature = false;
            bool isUnrealFileExtension = UnrealLoader.IsUnrealFileExtension(packageReference.FilePath);
            if (!isUnrealFileExtension)
            {
                if (MessageBox.Show(Resources.PACKAGE_UNKNOWN_SIGNATURE,
                        Resources.Warning,
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    UnrealConfig.SuppressSignature = true;
                }
                else
                {
                    return;
                }
            }

            if (UserHistory.Default.OpenFiles == null)
            {
                UserHistory.Default.OpenFiles = new PackageCollection();
            }
            UserHistory.Default.OpenFiles.Add(e.Package);

            if (Program.Options.bForceLicenseeMode)
            {
                UnrealPackage.OverrideLicenseeVersion = Program.Options.LicenseeMode;
            }

            if (Program.Options.bForceVersion)
            {
                UnrealPackage.OverrideVersion = Program.Options.Version;
            }

            UnrealConfig.Platform = (UnrealConfig.CookedPlatform)Enum.Parse
            (
                typeof(UnrealConfig.CookedPlatform),
                ((ProgramForm)ParentForm).Platform.Text, true
            );

            BeginInvoke((MethodInvoker)(() => packageManager.LoadPackage(packageReference)));
        }

        private void PackageManagerOnPackageLoaded(object sender, PackageEventArgs e)
        {
            var packageReference = e.Package;
            Debug.Assert(packageReference.Linker != null, "packageReference.Linker != null");

            if (HasCompressedChunks(packageReference.Linker))
            {
                if (MessageBox.Show(Resources.PACKAGE_IS_COMPRESSED,
                        Resources.NOTICE_TITLE,
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Question) == DialogResult.OK)
                {
                    Process.Start(Resources.GildorDownloadsWebUrl);
                    MessageBox.Show(Resources.COMPRESSED_HOWTO,
                        Resources.NOTICE_TITLE,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }

                // Don't initialize this package.
                return;
            }

            string ntlFilePath = Path.Combine(Application.StartupPath, "Native Tables", Program.Options.NTLPath);
            if (File.Exists(ntlFilePath + NativesTablePackage.Extension))
            {
                packageReference.Linker.NTLPackage = new NativesTablePackage();
                packageReference.Linker.NTLPackage.LoadPackage(ntlFilePath);
            }

            BeginInvoke((MethodInvoker)(() => packageManager.InitializePackage(packageReference)));
        }

        private void PackageManagerOnPackageInitialized(object sender, PackageEventArgs e)
        {
            var packageReference = e.Package;
            Debug.Assert(packageReference.Linker != null, "packageReference.Linker != null");

            ProgressStatus.ResetValue();
            ProgressStatus.SetMaxProgress(packageReference.Linker.Objects.Count + packageReference.Linker.Exports.Count);
            packageReference.Linker.NotifyPackageEvent += OnNotifyPackageEvent;

            try
            {
                packageReference.Linker.InitializePackage(
                    UnrealPackage.InitFlags.Deserialize |
                    UnrealPackage.InitFlags.Link);
            }
            catch (Exception ex)
            {
                throw new UnrealException($"Couldn't initialize package \"{packageReference.FilePath}\"", ex);
            }
            finally
            {
                packageReference.Linker.NotifyPackageEvent -= OnNotifyPackageEvent;
                ProgressStatus.Reset();
            }
        }

        private T CreateTrackingPage<T>(bool isTracking)
            where T : TrackingPage
        {
            var page = (T)Activator.CreateInstance(typeof(T), contextService);
            page.IsTracking = isTracking;
            page.ClearFlags(KryptonPageFlags.DockingAllowAutoHidden);
            return page;
        }

        private static bool HasCompressedChunks(UnrealPackage linker) =>
            linker.Summary.CompressedChunks != null &&
            linker.Summary.CompressedChunks.Any();

        /// <summary>
        ///     Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            Debug.WriteLine("Disposing UC_PackageExplorer " + disposing);

            kryptonDockingManagerMain.SaveConfigToFile(Program.DockingConfigPath);
            kryptonDockingManagerMain.Pages.ToList().ForEach(p => p.Dispose());

            base.Dispose(disposing);
        }

        private void OnNotifyPackageEvent(object sender, UnrealPackage.PackageEventArgs e) => UpdateStatus(e);

        //BeginInvoke((MethodInvoker)(() => UpdateStatus(e)));
        private void UpdateStatus(UnrealPackage.PackageEventArgs e)
        {
            switch (e.EventId)
            {
                case UnrealPackage.PackageEventArgs.Id.Object:
                    ProgressStatus.IncrementValue();
                    break;
            }
        }

        private static string GetExceptionMessage(UObject errorObject) =>
            "Deserialization failed by the following exception:"
            + "\n\n" + errorObject.ThrownException.Message
            + "\n\n" + errorObject.ThrownException.InnerException?.Message
            + "\n\n" + "Occurred on position:"
            + errorObject.ExceptionPosition + "/" + errorObject.ExportTable.SerialSize
            + "\n\nStackTrace:" + errorObject.ThrownException.StackTrace
            + "\n\nInnerStackTrace:" + errorObject.ThrownException.InnerException?.StackTrace;

        public void TabFind()
        {
            using (var findDialog = new FindDialog())
            {
                findDialog.FindNext += (sender, e) => { EmitFind(e.FindText); };
                findDialog.ShowDialog();
            }
        }

        private void SetActiveObjectPath(string path) => ActiveObjectPath.Text = path;

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
            {
                _ContentHistory.RemoveRange(0, 1);
            }

            var data = new ActionState { SelectedNode = node, Target = target, ActionKind = actionKind };
            _ContentHistory.Add(data);
            _CurrentHistoryIndex = _ContentHistory.Count - 1;

            if (_CurrentHistoryIndex > 0)
            {
                PrevButton.Enabled = true;
            }
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

        internal void ToolStripButton_Backward_Click(object sender, EventArgs e)
        {
            Debug.Assert(_CurrentHistoryIndex - 1 >= 0);

            UpdateContentHistoryData(_CurrentHistoryIndex);
            RestoreContentHistoryData(--_CurrentHistoryIndex);

            if (_CurrentHistoryIndex == 0)
            {
                PrevButton.Enabled = false;
            }

            NextButton.Enabled = true;
        }

        internal void ToolStripButton_Forward_Click(object sender, EventArgs e)
        {
            Debug.Assert(_CurrentHistoryIndex + 1 < _ContentHistory.Count);

            UpdateContentHistoryData(_CurrentHistoryIndex);
            RestoreContentHistoryData(++_CurrentHistoryIndex);

            if (_CurrentHistoryIndex == _ContentHistory.Count - 1)
            {
                NextButton.Enabled = false;
            }

            PrevButton.Enabled = true;
        }

        private void RestoreSelectedNode(TreeNode node)
        {
            if (node == null)
            {
                return;
            }

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

        public IEnumerable<IUnrealDecompilable> GetContents<T>(UnrealPackage linker)
            where T : UObject =>
            linker.Objects
                .OfType<T>()
                .Where(c => c.ExportTable != null)
                .ToList<IUnrealDecompilable>();

        public async void PerformSearchIn<T>(string searchText) where T : UObject
        {
            var findPage = new FindResultsPage();
            AddNav(findPage);
            kryptonDockingManagerMain.ShowPage(findPage);

            IEnumerable<IUnrealDecompilable> contents = new List<IUnrealDecompilable>();
            foreach (var packageReference in packageManager.EnumeratePackages())
            {
                if (packageReference.Linker == null)
                {
                    continue;
                }

                var next = GetContents<T>(packageReference.Linker);
                contents = contents.Concat(next);
            }

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

        private void EditorFindTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\r')
            {
                return;
            }

            EmitFind(EditorFindTextBox.Text);
            e.Handled = true;
        }

        private void findNextToolStripMenuItem_Click_1(object sender, EventArgs e) => EmitFind(EditorFindTextBox.Text);

        private void copyToolStripButton_Click(object sender, EventArgs e)
        {
            // FIXME:
        }

        // TODO: Re-implement in a package agnostic way (Blocked till UELib 2.0)
        private bool PerformActionByObjectPath(UnrealPackage linker, string objectGroup)
        {
            var obj = linker.FindObjectByGroup(objectGroup);
            if (obj == null)
            {
                return false;
            }

            PerformNodeAction(obj, ContextActionKind.Auto);
            return true;
        }

        private void SearchObjectTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case '\r':
                    foreach (var packageReference in packageManager.EnumeratePackages())
                    {
                        if (packageReference.Linker == null)
                        {
                            continue;
                        }

                        if (PerformActionByObjectPath(packageReference.Linker, ActiveObjectPath.Text))
                        {
                            e.Handled = true;
                            break;
                        }
                    }

                    break;
            }
        }

        internal static UC_PackageExplorer Traverse(Control parent)
        {
            for (var c = parent; c != null; c = c.Parent)
            {
                if (c is UC_PackageExplorer packageExplorer)
                {
                    return packageExplorer;
                }
            }

            throw new NotSupportedException();
        }

        public void EmitObjectNodeAction(object target, ContextActionKind actionKind) =>
            PerformNodeAction(target, actionKind);

        public void EmitSearchObjectByPath(string path)
        {
            foreach (var packageReference in packageManager.EnumeratePackages())
            {
                if (packageReference.Linker == null)
                {
                    continue;
                }

                if (PerformActionByObjectPath(packageReference.Linker, path))
                {
                    break;
                }
            }
        }

        public void EmitSearch<T>(string searchText) where T : UObject => PerformSearchIn<T>(searchText);

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

        private void kryptonDockableWorkspaceMain_ActivePageChanged(object sender, Krypton.Workspace.ActivePageChangedEventArgs e)
        {
            bool withEditorTools = false;
            
            var page = e.NewPage;
            switch (page)
            {
                case DecompilerPage _:
                    withEditorTools = true;
                    break;
            }
            
            copyToolStripButton.Enabled = withEditorTools;
            EditorFindTextBox.Enabled = withEditorTools;
            findNextToolStripMenuItem.Enabled = withEditorTools;
        }

        public bool HasPage(string uniqueName) =>
            kryptonDockingManagerMain.ContainsPage(uniqueName);

        public void AddNav(KryptonPage page, DockingEdge edge = DockingEdge.Left) =>
            kryptonDockingManagerMain.AddDockspace("Nav", edge,
                new[] { page });

        public void AddDocument(KryptonPage page)
        {
            _DocumentsWorkspace.Append(page);
            _DocumentsWorkspace.SelectPage(page.UniqueName);
        }

        public void AddDocument<T>(string title, string uniqueName)
            where T : Control, new()
        {
            var page = new KryptonPage(title, uniqueName);

            var control = new T();
            page.Controls.Add(control);

            _DocumentsWorkspace.Append(page);
        }

        public void AddPackage(string filePath) =>
            BeginInvoke((MethodInvoker)(() => packageManager.RegisterPackage(filePath)));

        public void AddPackage(PackageReference packageReference)
        {
            BeginInvoke((MethodInvoker)(() => packageManager.RegisterPackage(packageReference)));
        }

        public void AddPackageExplorer()
        {
            var page = new PackageExplorerPage(contextService, packageManager);
            page.ClearFlags(KryptonPageFlags.DockingAllowFloating |
                            KryptonPageFlags.DockingAllowWorkspace);

            AddNav(page);
        }

        #region Node-ContextMenu Methods

        private static string FormatTokenHeader(UStruct.UByteCodeDecompiler.Token token, bool acronymizeName = true)
        {
            string name = token.GetType().Name;
            if (!acronymizeName)
            {
                return $"{name}({token.Size}/{token.StorageSize})";
            }

            name = string.Concat(name.Substring(0, name.Length - 5).Select(
                c => char.IsUpper(c) ? c.ToString(CultureInfo.InvariantCulture) : string.Empty
            ));

            return $"{name}({token.Size}/{token.StorageSize})";
        }

        private static string _DisassembleTokensTemplate;

        private static string DisassembleTokens(UStruct container, UStruct.UByteCodeDecompiler decompiler,
            int tokenCount)
        {
            string content = string.Empty;
            for (int i = 0; i + 1 < tokenCount; ++i)
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

                byte[] buffer = new byte[token.StorageSize];
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

        private object PickBestTarget(object target, ContextActionKind actionKind)
        {
            if (target == null)
            {
                return null;
            }

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
            if (tag == null && target is TreeNode)
            {
                return;
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

                                string text = string.Empty;
                                while (codeDec.CurrentTokenIndex + 1 < codeDec.DeserializedTokens.Count)
                                {
                                    var t = codeDec.NextToken;
                                    int orgIndex = codeDec.CurrentTokenIndex;
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

                                    string chain = FormatTokenHeader(t);
                                    int inlinedTokens = codeDec.CurrentTokenIndex - orgIndex;
                                    if (inlinedTokens > 0)
                                    {
                                        ++orgIndex;
                                        for (int i = 0; i < inlinedTokens; ++i)
                                        {
                                            chain += " -> " +
                                                     FormatTokenHeader(codeDec.DeserializedTokens[orgIndex + i]);
                                        }
                                    }

                                    byte[] buffer = new byte[t.StorageSize];
                                    unStruct.Buffer.Position = unStruct.ScriptOffset + t.StoragePosition;
                                    unStruct.Buffer.Read(buffer, 0, buffer.Length);

                                    text += string.Format(tokensTemplate,
                                        t.Position, t.StoragePosition,
                                        chain, BitConverter.ToString(buffer).Replace('-', ' '),
                                        output != string.Empty ? output + "\r\n" : output
                                    );

                                    if (breakOut)
                                    {
                                        break;
                                    }
                                }

                                UpdateContext(unStruct.Default, ContextActionKind.Decompile);
                            }

                            break;
                        }

                    case ContextActionKind.ViewBuffer:
                        {
                            var bufferObject = (IBuffered)tag;
                            Debug.Assert(bufferObject != null, nameof(bufferObject) + " != null");
                            if (bufferObject.GetBufferSize() > 0)
                            {
                                ViewBufferFor(bufferObject);
                            }

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

                            string fileExtensions = string.Empty;
                            foreach (string ext in exportableObject.ExportableExtensions)
                            {
                                fileExtensions += $"{ext}(*" + $".{ext})|*.{ext}";
                                if (ext != exportableObject.ExportableExtensions.Last())
                                {
                                    fileExtensions += "|";
                                }
                            }

                            string fileName = tag.ToString();
                            var dialog = new SaveFileDialog { Filter = fileExtensions, FileName = fileName };
                            if (dialog.ShowDialog() != DialogResult.OK)
                            {
                                return;
                            }

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
                            Debug.Assert(obj != null, nameof(obj) + " != null");
                            var linker = obj.Package;
                            Process.Start
                            (
                                Program.Options.UEModelAppPath,
                                "-path=" + linker.PackageDirectory
                                         + " " + linker.PackageName
                                         + " " + ((TreeNode)target).Text
                            );
                            break;
                        }

                    case ContextActionKind.ExportExternal:
                        {
                            Debug.Assert(obj != null, nameof(obj) + " != null");
                            var linker = obj.Package;

                            string packagePath = Application.StartupPath
                                                 + "\\Exported\\"
                                                 + linker.PackageName;

                            string contentDir = packagePath + "\\Content";
                            Directory.CreateDirectory(contentDir);
                            string appArguments = "-path=" + linker.PackageDirectory
                                                           + " " + "-out=" + contentDir
                                                           + " -export"
                                                           + " " + linker.PackageName
                                                           + " " + ((TreeNode)target).Text;
                            var appInfo = new ProcessStartInfo(Program.Options.UEModelAppPath, appArguments)
                            {
                                UseShellExecute = false, RedirectStandardOutput = true, CreateNoWindow = false
                            };
                            var app = Process.Start(appInfo);
                            string log = string.Empty;
                            app.OutputDataReceived += (sender, e) => log += e.Data;
                            //app.WaitForExit();

                            if (Directory.GetFiles(contentDir).Length > 0)
                            {
                                if (MessageBox.Show(
                                        Resources.UC_PackageExplorer_PerformNodeAction_QUESTIONEXPORTFOLDER,
                                        ProductName,
                                        MessageBoxButtons.YesNo
                                    ) == DialogResult.Yes)
                                {
                                    Process.Start(contentDir);
                                }
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
        private KryptonPage GetObjectPage() =>
            kryptonDockableWorkspaceMain
                .AllPages()
                .FirstOrDefault(p => p.Name == "ObjectPage");

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
            contextService.OnContextChanged(this, contextEvent);
            // Insert default pages, if we don't have any active pages for this action kind.
            InsertDefaultPages(context);
            ProgressStatus.ResetStatus();

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
            if (context.ActionKind == ContextActionKind.Auto)
            {
                bool hasAny = kryptonDockingManagerMain.PagesWorkspace
                    .OfType<TrackingPage>()
                    .Any(p => p.IsTracking && p.CanAccept(context));

                if (hasAny)
                {
                    return;
                }
            }

            bool isTracking = context.ActionKind == ContextActionKind.Auto;
            var page = CreatePageByAction(context.Target, context.ActionKind, isTracking);
            if (page == null)
            {
                return;
            }

            AddDocument(page);
            page.Accept(context);
        }

        private static string LoadTemplate(string name) =>
            File.ReadAllText(Path.Combine(Program.s_templateDir, name + ".txt"), Encoding.ASCII);

        #endregion
    }
}
