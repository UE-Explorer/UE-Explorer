using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using UEExplorer.Properties;
using UEExplorer.UI.Forms;
using UEExplorer.UI.Nodes;
using UELib.Annotations;

namespace UEExplorer.UI.Tabs
{
    using Dialogs;
    using UELib;
    using UELib.Core;

    [System.Runtime.InteropServices.ComVisible(false)]
    public partial class UC_PackageExplorer : UserControl_Tab
    {
        private readonly Pen _BorderPen = new Pen(Color.FromArgb(237, 237, 237));
        private readonly Pen _LinePen = new Pen(Color.White);
        
        public string FilePath { get; set; }

        /// <summary>
        /// Null if the user cancels the exception popup.
        /// </summary>
        [CanBeNull] private UnrealPackage _UnrealPackage;
        private XMLSettings.State _State;

        public override void TabInitialize()
        {
            splitContainer1.SplitterDistance = Settings.Default.PackageExplorer_SplitterDistance;
            base.TabInitialize();
        }

        /// <summary>
        /// Called when the Tab is added to the chain.
        /// </summary>
        protected override void TabCreated()
        {
            string syntaxXSHDFilePath = Path.Combine(Application.StartupPath, "Config", "UnrealScript.xshd");
            if (File.Exists(syntaxXSHDFilePath))
            {
                try
                {
                    TextEditorPanel.textEditor.SyntaxHighlighting =
                        ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load(
                            new System.Xml.XmlTextReader(syntaxXSHDFilePath),
                            ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance
                        );
                    TextEditorPanel.searchWiki.Click += SearchWiki_Click;
                    TextEditorPanel.searchDocument.Click += SearchDocument_Click;
                    TextEditorPanel.searchPackage.Click += SearchClasses_Click;
                    TextEditorPanel.searchObject.Click += SearchObject_Click;
                    TextEditorPanel.textEditor.ContextMenuOpening += ContextMenu_ContextMenuOpening;
                    TextEditorPanel.copy.Click += Copy_Click;

                    // Fold all { } blocks
                    //var foldingManager = ICSharpCode.AvalonEdit.Folding.FoldingManager.Install(myTextEditor1.textEditor.TextArea);
                    //var foldingStrategy = new ICSharpCode.AvalonEdit.Folding.XmlFoldingStrategy();
                    //foldingStrategy.UpdateFoldings(foldingManager, myTextEditor1.textEditor.Document);
                }
                catch (Exception exception)
                {
                    ExceptionDialog.Show(exception.GetType().Name, exception);
                }
            }

            _Form = Tabs.Form;
            
            base.TabCreated();
        }

        private void Copy_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            TextEditorPanel.textEditor.Copy();
        }

        private string GetSelection()
        {
            return TextEditorPanel.textEditor.TextArea.Selection.GetText();
        }

        private void ContextMenu_ContextMenuOpening(object sender, System.Windows.Controls.ContextMenuEventArgs e)
        {
            if (TextEditorPanel.textEditor.TextArea.Selection.Length == 0)
            {
                TextEditorPanel.searchWiki.Visibility = System.Windows.Visibility.Collapsed;
                TextEditorPanel.searchDocument.Visibility = System.Windows.Visibility.Collapsed;
                TextEditorPanel.searchObject.Visibility = System.Windows.Visibility.Collapsed;
                return;
            }

            string selection = GetSelection();
            if (selection.IndexOf('\n') != -1)
            {
                TextEditorPanel.searchWiki.Visibility = System.Windows.Visibility.Collapsed;
                return;
            }

            TextEditorPanel.searchDocument.Visibility = System.Windows.Visibility.Visible;
            TextEditorPanel.searchObject.Visibility = System.Windows.Visibility.Visible;
            TextEditorPanel.searchWiki.Visibility = System.Windows.Visibility.Visible;
            TextEditorPanel.searchWiki.Header = string.Format(Resources.SEARCH_WIKI_ITEM, selection);

            TextEditorPanel.searchDocument.Header = findInDocumentToolStripMenuItem.Text;
            TextEditorPanel.searchPackage.Header = findInClassesToolStripMenuItem.Text;
            TextEditorPanel.searchObject.Header = Resources.SEARCH_AS_OBJECT;
        }

        private void SearchWiki_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Process.Start(string.Format(Resources.URL_UnrealWikiSearch, GetSelection()));
        }

        private void SearchDocument_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SearchBox.Text = GetSelection();
            FindButton.PerformClick();
        }

        private void SearchClasses_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SearchBox.Text = GetSelection();
            findInClassesToolStripMenuItem.PerformClick();
        }

        private void SearchObject_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DoSearchObjectByGroup(GetSelection());
        }
        
        public void PostInitialize()
        {
            _State = Program.Options.GetState(FilePath);
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
                _Form.Platform.Text, true
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
                    Process.Start("http://www.gildor.org/downloads");
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

        private void LinkPackageData(UnrealPackage package)
        {
            uNameTableItemBindingSource.DataSource = package.Names;
            uImportTableItemBindingSource.DataSource = package.Imports;
            uExportTableItemBindingSource.DataSource = package.Exports;
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
                InitializeDependenciesTree();
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
        
        private void ToolStripButton1_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog
                   {
                       DefaultExt = "uc",
                       Filter = $"{Resources.UnrealClassFilter}(*.uc)|*.uc",
                       FilterIndex = 1,
                       Title = Resources.ExportTextTitle,
                       FileName = Label_ObjectName.Text + UnrealExtensions.UnrealCodeExt
                   })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                    File.WriteAllText(sfd.FileName, TextEditorPanel.textEditor.Text);
            }
        }

        private ProgramForm _Form;

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
            
            _BorderPen.Dispose();
            _LinePen.Dispose();

            if (_UnrealPackage != null)
            {
                _UnrealPackage.Dispose();
                _UnrealPackage = null;
            }

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
            SearchObjectTextBox.Text = state.SearchObjectValue;
            DoSearchObjectByGroup(SearchObjectTextBox.Text);

            SearchObjectTextBox.TextChanged += (e, sender) =>
            {
                _State.SearchObjectValue = SearchObjectTextBox.Text;
                _State.Update();
            };

            ResumeLayout();
        }

        private ObjectTreeBuilder _ObjectTreeBuilder = new ObjectTreeBuilder();
        private ObjectImageKeySelector _ObjectImageKeySelector = new ObjectImageKeySelector();
        
        protected void InitializeImportNode(UImportTableItem item, TreeNode node)
        {
            if (item.ClassName == "Class")
            {
                node.ForeColor = Color.DarkCyan;
            }
            
            if (item.Object != null)
            {
                if (item.Object.DeserializationState.HasFlag(UObject.ObjectState.Errorlized))
                {
                    InitializeNodeError(node, item.Object);
                }
            }

            if (!(item.Object is IAcceptable acceptableObj))
                return;
            
            string imageKey = acceptableObj.Accept(_ObjectImageKeySelector);
            node.ImageKey = imageKey;
            node.SelectedImageKey = imageKey;
        }

        private ObjectNode CreateNode(UObjectTableItem item)
        {
            var node = new ObjectNode(item.Object)
            {
                Text = item.ObjectName,
                Tag = item
            };
            return node;
        }

        private void InitializeDependenciesTree()
        {
            var dependenciesNode = new TreeNode("Dependencies")
            {
                Tag = "Dependencies",
                ImageKey = "Diagram",
                SelectedImageKey = "Diagram"
            };
            dependenciesNode.Nodes.Add(ObjectNode.DummyNodeKey, "Expandable");
            TreeView_Content.Nodes.Add(dependenciesNode);
        }

        private void GetDependencyOn(UImportTableItem outerImp, TreeNode node)
        {
            foreach (var item in _UnrealPackage.Imports
                         .Where(imp => imp != outerImp && _UnrealPackage.GetIndexTable(imp.OuterIndex) == outerImp))
                GetDependencyOn(item, node.Nodes.Add(item.ObjectName));

            node.ToolTipText = outerImp.ClassName;
            InitializeImportNode(outerImp, node);
        }

        private void InitializeContentTree()
        {
            Debug.Assert(_UnrealPackage != null);
            Debug.Assert(_UnrealPackage.Exports != null);

            TreeView_Content.BeginUpdate();
            // Lazy recursive, creates a base node for each export with no Outer, if a matching outer is found it will be appended to that base node upon expansion.
            foreach (var objectNode in 
                     from obj in _UnrealPackage.Exports 
                     where obj.OuterTable == null && obj.ArchetypeTable == null 
                     select CreateNode((UObjectTableItem)obj))
            {
                InitializeObjectNode(objectNode);
                TreeView_Content.Nodes.Add(objectNode);
            }
            TreeView_Content.EndUpdate();
        }

        private void InitializeObjectNode(ObjectNode node)
        {
            var obj = node.Object;
            if (obj == null)
                return;
            
            if (obj.DeserializationState.HasFlag(UObject.ObjectState.Errorlized))
            {
                InitializeNodeError(node, obj);
            }

            if (obj.Class == null)
            {
                node.ForeColor = Color.DarkCyan;
            }
        }

        private void ExpandObjectNode(ObjectNode node)
        {
            var subNodes = node.Object?.Accept(_ObjectTreeBuilder);
            if (subNodes != null) node.Nodes.AddRange(subNodes);
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

        private void OutputNodeObject(TreeNode treeNode)
        {
            try
            {
                if (!(treeNode is IUnrealDecompilable unrealDecompilable))
                    return;

                SetContentText(unrealDecompilable, unrealDecompilable.Decompile());
                // Assemble a title
                string newTitle;
                if (treeNode is IWithDecompilableObject<IUnrealDecompilable> decompilableObject)
                {
                    UObject obj;
                    if ((obj = decompilableObject.Object as UObject) != null)
                    {
                        newTitle = obj.GetOuterGroup();
                        SetContentTitle(newTitle);
                        if (obj.DeserializationState.HasFlag(UObject.ObjectState.Errorlized))
                            InitializeNodeError(treeNode, obj);
                    }
                    else
                    {
                        newTitle = treeNode.Text;
                        SetContentTitle(newTitle, false);
                    }
                }
                else
                {
                    if (treeNode.Parent != null)
                        newTitle = treeNode.Parent.Text + "." + treeNode.Text;
                    else
                        newTitle = treeNode.Text;
                    SetContentTitle(newTitle, false);
                }
            }
            catch (Exception except)
            {
                ExceptionDialog.Show(
                    "An exception occurred while attempting to display content of node: " + treeNode.Text, except);
                treeNode.ForeColor = Color.Red;
                treeNode.ToolTipText = except.Message;
            }
        }

        private static void InitializeNodeError(TreeNode node, UObject errorObject)
        {
            node.ForeColor = Color.Red;
            node.ToolTipText = GetExceptionMessage(errorObject);
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

        private void _OnExportsNodeSelected(object sender, TreeViewEventArgs e)
        {
            OutputNodeObject(e.Node);
        }

        #region Node-ContextMenu Methods

        private void TreeView_Exports_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            ShowNodeContextMenuStrip(TreeView_Exports, e, _OnExportsItemClicked);
        }

        private bool _SuppressNodeSelect;

        private void TreeView_Content_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.Right) 
                return;
            
            _SuppressNodeSelect = true;
            ShowNodeContextMenuStrip(TreeView_Content, e, _OnContentItemClicked);
        }

        private static void ShowNodeContextMenuStrip(TreeView tree, TreeNodeMouseClickEventArgs e,
            ToolStripItemClickedEventHandler itemClicked)
        {
            tree.SelectedNode = e.Node;

            var viewToolsContextMenu = new ContextMenuStrip();
            BuildItemNodes(e.Node, viewToolsContextMenu.Items, itemClicked);
            if (viewToolsContextMenu.Items.Count == 0)
            {
                viewToolsContextMenu.Dispose();
                return;
            }

            viewToolsContextMenu.ItemClicked += itemClicked;
            viewToolsContextMenu.Closed += (eSender, eEvent) =>
            {
                ((ContextMenuStrip)eSender).ItemClicked -= itemClicked;
                //((ContextMenuStrip)eSender).Dispose();
            };
            viewToolsContextMenu.Show(tree, e.Location);
        }
        
        private const string Action_ExportAs = "EXPORT_AS";

        private static void BuildItemNodes(object target, ToolStripItemCollection itemCollection,
            ToolStripItemClickedEventHandler itemClickEvent = null)
        {
            itemCollection.Clear();

            var addItem = (Action<string, string>)((title, id) =>
            {
                var item = itemCollection.Add(title);
                item.Name = id;
            });

            if (target is IUnrealDecompilable) addItem(Resources.NodeItem_ViewObject, "OBJECT");

            var obj = target as UObject;
            if (obj == null && target is IWithDecompilableObject<UObject> decompilableObject)
                obj = decompilableObject.Object;
            
            if (obj == null) return;
            
            if (obj is IUnrealExportable exportableObj && exportableObj.CanExport())
            {
                addItem(Resources.EXPORT_AS, Action_ExportAs);
            }

            if (obj is IUnrealViewable)
            {
                if (File.Exists(Program.Options.UEModelAppPath))
                {
                    addItem(Resources.NodeItem_OpenInUEModelViewer, "OPEN_UEMODELVIEWER");
#if DEBUG
                    addItem(Resources.NodeItem_ExportWithUEModelViewer, "EXPORT_UEMODELVIEWER");
#endif
                }
            }

            if (obj.Outer != null) addItem(Resources.NodeItem_ViewOuter, "VIEW_OUTER");

            if (obj is UStruct uStruct)
            {
                if (uStruct.Super != null) addItem(Resources.NodeItem_ViewParent, "VIEW_SUPER");

                var @class = obj as UClass;
                if (@class != null)
                {
                    if (@class.IsClassWithin()) addItem(Resources.NodeItem_ViewOuter, "VIEW_WITHIN");
                }

                if (uStruct.ByteCodeManager != null)
                {
                    if (@class != null) addItem(Resources.NodeItem_ViewReplication, "REPLICATION");
                    addItem(Resources.NodeItem_ViewTokens, "TOKENS");
                    addItem(Resources.NodeItem_ViewDisassembledTokens, "TOKENS_DISASSEMBLE");
                }

                if (uStruct.ScriptText != null) addItem(Resources.NodeItem_ViewScript, "SCRIPT");

                if (uStruct.ProcessedText != null) addItem(Resources.NodeItem_ViewProcessedScript, "PROCESSEDSCRIPT");

                if (uStruct.CppText != null) addItem(Resources.NodeItem_ViewCPPText, "CPPSCRIPT");

                if (uStruct.Properties != null && uStruct.Properties.Any())
                    addItem(Resources.NodeItem_ViewDefaultProperties, "DEFAULTPROPERTIES");
            }

            var bufferedObject = obj as IBuffered;
            if (bufferedObject.GetBuffer() != null)
            {
                var bufferedItem = new ToolStripMenuItem
                {
                    Text = Resources.NodeItem_ViewBuffer,
                    Name = "BUFFER"
                };

                bool shouldAddBufferItem = bufferedObject.GetBufferSize() > 0;

                var tableNode = obj as IContainsTable;
                if (tableNode.Table != null)
                {
                    var tableBufferItem = bufferedItem.DropDownItems.Add(Resources.NodeItem_ViewTableBuffer);
                    tableBufferItem.Name = "TABLEBUFFER";
                    shouldAddBufferItem = true;
                }

                if (obj.Default != null && obj.Default != obj)
                {
                    var defaultBufferItem = bufferedItem.DropDownItems.Add(Resources.NodeItem_DefaultBuffer);
                    defaultBufferItem.Name = "DEFAULTBUFFER";
                    shouldAddBufferItem = true;
                }

                if (shouldAddBufferItem)
                {
                    bufferedItem.DropDownItemClicked += itemClickEvent;
                    itemCollection.Add(bufferedItem);
                }
            }

            if (obj.ThrownException != null)
            {
                itemCollection.Add(new ToolStripSeparator());
                addItem(Resources.NodeItem_ViewException, "EXCEPTION");
            }

            itemCollection.Add(new ToolStripSeparator());
            addItem(Resources.NodeItem_ManagedProperties, "MANAGED_PROPERTIES");
        }

        private void _OnImportsItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            PerformNodeAction(TreeView_Imports.SelectedNode, e.ClickedItem.Name);
        }

        private void TreeView_Imports_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            ShowNodeContextMenuStrip(TreeView_Imports, e, _OnImportsItemClicked);
        }

        private void ViewTools_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (_LastNodeContent == null)
                return;

            PerformNodeAction(_LastNodeContent, e.ClickedItem.Name);
        }

        private void _OnExportsItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            PerformNodeAction(TreeView_Exports.SelectedNode, e.ClickedItem.Name);
        }

        private void _OnContentItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            PerformNodeAction(TreeView_Content.SelectedNode, e.ClickedItem.Name);
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

        private void PerformNodeAction(object target, string action)
        {
            if (target == null)
                return;

            var obj = target as UObject;
            if (obj == null && target is IWithDecompilableObject<UObject> decompilableObject) obj = decompilableObject.Object;

            try
            {
                switch (action)
                {
                    case "OPEN_UEMODELVIEWER":
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

                    case "EXPORT_UEMODELVIEWER":
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
                                string.Format
                                (
                                    "The object was not exported.\r\n\r\nArguments:{0}\r\n\r\nLog:{1}",
                                    appArguments,
                                    log
                                ),
                                Application.ProductName
                            );
                        }

                        break;
                    }

                    case "OBJECT":
                    {
                        if (obj != null)
                        {
                            SetContentTitle(obj.GetOuterGroup());
                            SetContentText(obj, obj.Decompile());
                        }
                        else if (target is IUnrealDecompilable)
                        {
                            var node = target as TreeNode;
                            SetContentTitle(node.Text);
                            SetContentText(node, (target as IUnrealDecompilable).Decompile());
                        }

                        break;
                    }

                    case "VIEW_OUTER":
                        if (obj != null && obj.Outer != null)
                        {
                            SetContentTitle(obj.Outer.GetOuterGroup());
                            SetContentText(obj.Outer, obj.Outer.Decompile());
                        }

                        break;

                    case "VIEW_SUPER":
                        if (obj is UStruct && ((UStruct)obj).Super != null)
                        {
                            var super = ((UStruct)obj).Super;
                            SetContentTitle(super.GetOuterGroup());
                            SetContentText(super, super.Decompile());
                        }

                        break;

                    case "VIEW_WITHIN":
                        if (obj is UClass && ((UClass)obj).Within != null)
                        {
                            var within = ((UClass)obj).Within;
                            SetContentTitle(within.GetOuterGroup());
                            SetContentText(within, within.Decompile());
                        }

                        break;

                    case "MANAGED_PROPERTIES":
                        using (var propDialog = new PropertiesDialog
                               {
                                   ObjectLabel = { Text = ((TreeNode)target).Text },
                                   ObjectPropertiesGrid = { SelectedObject = obj }
                               })
                        {
                            propDialog.ShowDialog(this);
                        }

                        break;

                    case "REPLICATION":
                    {
                        var unClass = obj as UClass;
                        if (unClass != null)
                        {
                            SetContentTitle(unClass.Name, true, "Replication");
                            SetContentText(unClass, unClass.FormatReplication());
                        }

                        break;
                    }

                    case "SCRIPT":
                    {
                        var str = obj as UStruct;
                        if (str != null && str.ScriptText != null)
                        {
                            SetContentTitle(str.ScriptText.GetOuterGroup());
                            SetContentText(str.ScriptText, str.ScriptText.Decompile());
                        }

                        break;
                    }

                    case "CPPSCRIPT":
                    {
                        var str = obj as UStruct;
                        if (str != null && str.CppText != null)
                        {
                            SetContentTitle(str.CppText.GetOuterGroup());
                            SetContentText(str.CppText, str.CppText.Decompile());
                        }

                        break;
                    }

                    case "PROCESSEDSCRIPT":
                    {
                        var str = obj as UStruct;
                        if (str != null && str.ProcessedText != null)
                        {
                            SetContentTitle(str.ProcessedText.GetOuterGroup());
                            SetContentText(str.ProcessedText, str.ProcessedText.Decompile());
                        }

                        break;
                    }

                    case "DEFAULTPROPERTIES":
                    {
                        var unStruct = obj as UStruct;
                        if (unStruct != null)
                        {
                            SetContentTitle(unStruct.Default.GetOuterGroup(), true, "Default-Properties");
                            SetContentText(unStruct, unStruct.FormatDefaultProperties());
                        }

                        break;
                    }

                    case "TOKENS_DISASSEMBLE":
                    {
                        var unStruct = obj as UStruct;
                        if (unStruct?.ByteCodeManager != null)
                        {
                            var codeDec = unStruct.ByteCodeManager;
                            codeDec.Deserialize();
                            codeDec.InitDecompile();

                            _DisassembleTokensTemplate = LoadTemplate("struct.tokens-disassembled");
                            string content = DisassembleTokens(unStruct, codeDec, codeDec.DeserializedTokens.Count);
                            SetContentTitle(unStruct.GetOuterGroup(), true, "Tokens-Disassembled");
                            SetContentText(unStruct, content);
                        }

                        break;
                    }

                    case "TOKENS":
                    {
                        var unStruct = obj as UStruct;
                        if (unStruct?.ByteCodeManager != null)
                        {
                            string tokensTemplate = LoadTemplate("struct.tokens");
                            var codeDec = unStruct.ByteCodeManager;
                            codeDec.Deserialize();
                            codeDec.InitDecompile();

                            var content = string.Empty;
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

                                content += string.Format(tokensTemplate,
                                    t.Position, t.StoragePosition,
                                    chain, BitConverter.ToString(buffer).Replace('-', ' '),
                                    output != string.Empty ? output + "\r\n" : output
                                );

                                if (breakOut)
                                    break;
                            }

                            SetContentTitle(unStruct.GetOuterGroup(), true, "Tokens");
                            SetContentText(unStruct, content);
                        }

                        break;
                    }

                    case "BUFFER":
                    {
                        var bufferObject = (IBuffered)obj;
                        Debug.Assert(bufferObject != null, nameof(bufferObject) + " != null");
                        if (bufferObject.GetBufferSize() > 0) ViewBufferFor(bufferObject);
                        break;
                    }

                    case "TABLEBUFFER":
                    {
                        var tableObject = target as IContainsTable ?? obj;
                        Debug.Assert(tableObject != null, nameof(tableObject) + " != null");
                        ViewBufferFor(tableObject.Table);
                        break;
                    }

                    case "DEFAULTBUFFER":
                    {
                        var unObject = obj;
                        if (unObject != null) ViewBufferFor(unObject.Default);
                        break;
                    }

                    case "EXCEPTION":
                    {
                        if (target is ObjectNode oNode) SetContentText(oNode, GetExceptionMessage((UObject)oNode.Object));
                        break;
                    }

                    case Action_ExportAs:
                    {
                        if (obj is IUnrealExportable exportableObject)
                        {
                            obj.BeginDeserializing();

                            var fileExtensions = string.Empty;
                            foreach (string ext in exportableObject.ExportableExtensions)
                            {
                                fileExtensions += $"{ext}(*" + $".{ext})|*.{ext}";
                                if (ext != exportableObject.ExportableExtensions.Last()) fileExtensions += "|";
                            }

                            var fileName = obj.Name;
                            var dialog = new SaveFileDialog { Filter = fileExtensions, FileName = fileName };
                            if (dialog.ShowDialog() != DialogResult.OK)
                                return;

                            using (var stream = new FileStream(dialog.FileName, FileMode.Create, FileAccess.Write))
                            {
                                exportableObject.SerializeExport(
                                    exportableObject.ExportableExtensions.ElementAt(dialog.FilterIndex - 1), stream);
                                stream.Flush();
                            }
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

        private static readonly string _TemplateDir = Path.Combine(Program.ConfigDir, "Templates");

        private static string LoadTemplate(string name)
        {
            return File.ReadAllText(Path.Combine(_TemplateDir, name + ".txt"), System.Text.Encoding.ASCII);
        }

        #endregion

        private void ToolStripButton_Find_Click(object sender, EventArgs e)
        {
            if (TextEditorPanel == null) return;
            EditorUtil.FindText(TextEditorPanel, SearchBox.Text);
        }

        private void SearchBox_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (TextEditorPanel == null) return;

            if (e.KeyChar != '\r')
                return;

            EditorUtil.FindText(TextEditorPanel, SearchBox.Text);
            e.Handled = true;
        }

        public override void TabFind()
        {
            using (var findDialog = new FindDialog(TextEditorPanel))
            {
                findDialog.ShowDialog();
            }
        }

        private struct BufferData
        {
            public string Text;
            public string Label;
            public object Node;

            public double Y, X;
        }

        private readonly List<BufferData> _ContentBuffer = new List<BufferData>();
        private int _BufferIndex = -1;
        private object _LastNodeContent;

        public void SetContentTitle(string title, bool isSearchable = true, string sub = "")
        {
            Label_ObjectName.Text = title;
            if (sub != "") Label_ObjectName.Text += " -> " + sub.Replace('-', ' ');

            if (!isSearchable)
                return;

            SearchObjectTextBox.Text = title;
            if (sub != "") SearchObjectTextBox.Text += ":" + sub;
            SearchObjectTextBox.SelectAll();
        }

        public void SetContentText(object node, string content, bool skip = false, bool resetView = true)
        {
            if (_LastNodeContent != node) BuildItemNodes(node, ViewTools.DropDownItems);
            ViewTools.Enabled = ViewTools.DropDownItems.Count > 0 && node != null;
            _LastNodeContent = node;

            content = content.TrimStart('\r', '\n').TrimEnd('\r', '\n');
            if (content.Length > 0)
            {
                SearchBox.Enabled = true;
                ExportButton.Enabled = true;
                WPFHost.Enabled = true;
                ViewTools.Enabled = true;
                findInDocumentToolStripMenuItem.Enabled = true;
            }

            TextEditorPanel.textEditor.Text = content;
            if (resetView) TextEditorPanel.textEditor.ScrollToHome();

            if (skip)
                return;

            if (_ContentBuffer.Count > 0)
            {
                // No need to buffer the same content
                if (_BufferIndex > -1 && _BufferIndex < _ContentBuffer.Count &&
                    _ContentBuffer[_BufferIndex].Node == node) return;

                StoreViewForBuffer(_BufferIndex);
                // Clean all above buffers when a new node was user-selected
                if (_ContentBuffer.Count - 1 - _BufferIndex > 0)
                {
                    _ContentBuffer.RemoveRange(_BufferIndex, _ContentBuffer.Count - _BufferIndex);
                    _BufferIndex = _ContentBuffer.Count - 1;
                    NextButton.Enabled = false;
                }
            }

            var bd = new BufferData { Text = content, Node = node, Label = Label_ObjectName.Text };
            _ContentBuffer.Add(bd);

            // Maximum 10 can be buffered; remove last one
            if (_ContentBuffer.Count > 10)
                _ContentBuffer.RemoveRange(0, 1);
            else ++_BufferIndex;

            if (_BufferIndex > 0) PrevButton.Enabled = true;
        }

        private void StoreViewForBuffer(int bufferIndex)
        {
            var content = _ContentBuffer[bufferIndex];
            content.X = TextEditorPanel.textEditor.HorizontalOffset;
            content.Y = TextEditorPanel.textEditor.VerticalOffset;
            _ContentBuffer[bufferIndex] = content;
        }

        private void RestoreBufferedContent(int bufferIndex)
        {
            SetContentTitle(_ContentBuffer[bufferIndex].Label, false);
            SetContentText(_ContentBuffer[bufferIndex].Node, _ContentBuffer[bufferIndex].Text, true);
            SelectNode(_ContentBuffer[bufferIndex].Node as TreeNode);

            TextEditorPanel.textEditor.ScrollToVerticalOffset(_ContentBuffer[bufferIndex].Y);
            TextEditorPanel.textEditor.ScrollToHorizontalOffset(_ContentBuffer[bufferIndex].X);
        }

        private void ToolStripButton_Backward_Click(object sender, EventArgs e)
        {
            if (_BufferIndex - 1 <= -1)
                return;

            FilterText.Text = string.Empty;
            StoreViewForBuffer(_BufferIndex);
            RestoreBufferedContent(--_BufferIndex);

            if (_BufferIndex == 0) PrevButton.Enabled = false;
            NextButton.Enabled = true;
        }

        private void ToolStripButton_Forward_Click(object sender, EventArgs e)
        {
            if (_BufferIndex + 1 >= _ContentBuffer.Count)
                return;

            FilterText.Text = string.Empty;
            StoreViewForBuffer(_BufferIndex);
            RestoreBufferedContent(++_BufferIndex);

            if (_BufferIndex == _ContentBuffer.Count - 1) NextButton.Enabled = false;
            PrevButton.Enabled = true;
        }

        private void SelectNode(TreeNode node)
        {
            if (node == null)
                return;

            node.TreeView.Show();
            node.TreeView.Select();
            node.TreeView.SelectedNode = node;
        }

        private void ViewBufferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewBufferFor(_UnrealPackage);
        }

        private void ViewBufferFor(IBuffered target)
        {
            if (target == null)
            {
                MessageBox.Show(Resources.NoTargetForViewBuffer);
                return;
            }

            var hexDialog = new HexViewerForm(target, this);
            hexDialog.Show(_Form);
        }

        private System.Windows.Forms.Timer _FilterTextChangedTimer = null;
        private readonly List<TreeNode> _FilteredNodes = new List<TreeNode>();

        private void FilterText_TextChanged(object sender, EventArgs e)
        {
            if (_FilterTextChangedTimer != null && _FilterTextChangedTimer.Enabled)
            {
                _FilterTextChangedTimer.Stop();
                _FilterTextChangedTimer.Dispose();
                _FilterTextChangedTimer = null;
            }

            if (_FilterTextChangedTimer != null) 
                return;
            
            _FilterTextChangedTimer = new System.Windows.Forms.Timer();
            _FilterTextChangedTimer.Interval = 350;
            _FilterTextChangedTimer.Tick += _FilterTextChangedTimer_Tick;
            _FilterTextChangedTimer.Start();
        }

        private void _FilterTextChangedTimer_Tick(object sender, EventArgs e)
        {
            _FilterTextChangedTimer.Stop();
            _FilterTextChangedTimer.Dispose();
            _FilterTextChangedTimer = null;

            for (var i = 0; i < TreeView_Content.Nodes.Count; ++i)
            {
                if (TreeView_Content.Nodes[i].Text.IndexOf(FilterText.Text, StringComparison.OrdinalIgnoreCase) != -1)
                    continue;

                _FilteredNodes.Add(TreeView_Content.Nodes[i]);
                TreeView_Content.Nodes[i].Remove();
                --i;
            }

            for (var i = 0; i < _FilteredNodes.Count; ++i)
            {
                if (FilterText.Text != string.Empty &&
                    _FilteredNodes[i].Text.IndexOf(FilterText.Text, StringComparison.OrdinalIgnoreCase) < 0)
                    continue;

                TreeView_Content.Nodes.Add(_FilteredNodes[i]);
                _FilteredNodes.Remove(_FilteredNodes[i]);
                --i;
            }

            TreeView_Content.Sort();
        }

        private void ReloadButton_Click(object sender, EventArgs e)
        {
            ReloadPackage();
        }

        private void Panel4_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(_BorderPen, 0, 0, panel4.Width - 1, panel4.Height - 1);
        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(_BorderPen, 0, 0, panel1.Width - 1, panel1.Height - 1);
        }

        private void ToolStripSeparator1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(_LinePen.Brush, 2, 0, panel1.Width - 4, panel1.Height);
            e.Graphics.DrawLine(_BorderPen, e.ClipRectangle.Left, e.ClipRectangle.Top, e.ClipRectangle.Left,
                e.ClipRectangle.Bottom);
            e.Graphics.DrawLine(_BorderPen, e.ClipRectangle.Right - 1, e.ClipRectangle.Top, e.ClipRectangle.Right - 1,
                e.ClipRectangle.Bottom);
        }

        private void ToolStrip_Content_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(_BorderPen, 0, 0, ((Control)sender).Width - 1, panel1.Height - 1);
        }

        private void FilterByClassCheckBox(object sender, EventArgs e)
        {
            var checkBox = (CheckBox)sender;
            if (!checkBox.Checked)
            {
                TreeView_Exports.BeginUpdate();
                var removedNodes = new List<TreeNode>();
                for (var i = 0; i < TreeView_Exports.Nodes.Count; ++i)
                {
                    if (TreeView_Exports.Nodes[i].ImageKey != checkBox.ImageKey)
                        continue;

                    removedNodes.Add(TreeView_Exports.Nodes[i]);
                    TreeView_Exports.Nodes.RemoveAt(i);
                }

                checkBox.Tag = removedNodes;
                TreeView_Exports.EndUpdate();
            }
            else
            {
                if (checkBox.Tag == null)
                {
                    checkBox.Checked = false;
                    return;
                }

                TreeView_Exports.Nodes.AddRange(((List<TreeNode>)checkBox.Tag).ToArray());
            }
        }

        private void TreeView_Content_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // Selection shouldn't view object, for example on contextmenu selection.
            if (_SuppressNodeSelect)
            {
                _SuppressNodeSelect = false;
                return;
            }

            PerformNodeAction(e.Node, "OBJECT");
        }

        private int _FindCount;
        private TabPage _FindTab;

        private void FindInClassesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string findText;
            using (var findDialog = new FindDialog())
            {
                findDialog.FindInput.Text = SearchBox.Text;
                if (findDialog.ShowDialog() != DialogResult.OK) return;
                findText = findDialog.FindInput.Text;
            }

            ProgressStatus.SaveStatus();
            ProgressStatus.SetStatus(Resources.SEARCHING_CLASSES_STATUS);

            var documentResults = new List<TextSearchHelpers.DocumentResult>();
            foreach (var content in _UnrealPackage.Objects.OfType<UClass>())
            {
                string findContent = content.Decompile();
                var findResults = TextSearchHelpers.FindText(findContent, findText);
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
                MessageBox.Show(string.Format(Resources.NO_FIND_RESULTS, findText));
                return;
            }

            if (_FindTab != null) TabControl_General.TabPages.Remove(_FindTab);

            _FindTab = new TabPage
            {
                Text = string.Format(Resources.FIND_RESULTS_TITLE, ++_FindCount)
            };

            var treeResults = new TreeView { Dock = DockStyle.Fill };
            _FindTab.Controls.Add(treeResults);
            TabControl_General.TabPages.Add(_FindTab);

            TabControl_General.SelectTab(_FindTab);

            foreach (var documentResult in documentResults)
            {
                var documentNode = treeResults.Nodes.Add(((UClass)documentResult.Document).Name);
                documentNode.Tag = documentResult;
                foreach (var result in documentResult.Results)
                {
                    var resultNode = documentNode.Nodes.Add(result.ToString());
                    resultNode.Tag = result;
                }
            }

            treeResults.AfterSelect += (nodeSender, nodeEvent) =>
            {
                var findResult = nodeEvent.Node.Tag as TextSearchHelpers.FindResult;
                if (findResult == null) return;

                if (nodeEvent.Node.Parent.Tag is TextSearchHelpers.DocumentResult documentResult)
                {
                    var unClass = (UClass)documentResult.Document;
                    SetContentTitle(
                        $"{unClass.Name}: {findResult.TextLine}, {findResult.TextColumn}",
                        false
                    );
                    SetContentText(nodeEvent.Node, unClass.Decompile(), false, false);
                }

                TextEditorPanel.textEditor.ScrollTo(findResult.TextLine, findResult.TextColumn);
                TextEditorPanel.textEditor.Select(findResult.TextIndex, findText.Length);
            };
        }

        private void FindInDocumentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchBox.Focus();
        }

        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            FindButton.Enabled = SearchBox.Text.Length > 0;
            findNextToolStripMenuItem.Enabled = FindButton.Enabled;
        }

        private void FindNextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TextEditorPanel == null) return;
            EditorUtil.FindText(TextEditorPanel, SearchBox.Text);
        }

        private void SplitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            Settings.Default.PackageExplorer_SplitterDistance = splitContainer1.SplitterDistance;
            Settings.Default.Save();
        }

        private bool DoSearchObjectByGroup(string objectGroup)
        {
            var protocol = string.Empty;
            var page = string.Empty;
            if (objectGroup.Contains(':'))
            {
                protocol = objectGroup.Substring(0, objectGroup.IndexOf(':')).ToLower();
                page = objectGroup.Substring(protocol.Length + 1).ToLower();
            }

            var obj = _UnrealPackage.FindObjectByGroup(protocol == "" ? objectGroup : protocol);
            if (obj == null) 
                return false;
            
            if (page != "")
            {
                switch (page)
                {
                    case "replication":
                        if (obj is UClass)
                        {
                            PerformNodeAction(obj, "REPLICATION");
                            return true;
                        }

                        break;

                    case "tokens":
                        if (obj is UStruct)
                        {
                            PerformNodeAction(obj, "TOKENS");
                            return true;
                        }

                        break;

                    case "tokens-disassembled":
                        if (obj is UStruct)
                        {
                            PerformNodeAction(obj, "TOKENS_DISASSEMBLE");
                            return true;
                        }

                        break;

                    case "default-properties":
                        if (obj is UStruct)
                        {
                            PerformNodeAction(obj, "DEFAULTPROPERTIES");
                            return true;
                        }

                        break;
                }
            }

            string content = obj.ImportTable == null
                ? obj.Decompile()
                : string.Format("// No decompilable data available for {0}", obj.GetOuterGroup());

            SetContentTitle(obj.GetOuterGroup());
            SetContentText(obj, content);
            return true;

        }

        private void SearchObjectTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case '\r':
                    e.Handled = DoSearchObjectByGroup(SearchObjectTextBox.Text);
                    break;
            }
        }

        private void SearchObjectButton_Click(object sender, EventArgs e)
        {
            DoSearchObjectByGroup(SearchObjectTextBox.Text);
        }

        private void TreeView_Content_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            // Already lazy-loaded.
            if (!e.Node.Nodes.ContainsKey(ObjectNode.DummyNodeKey))
            {
                return;
            }

            TreeView_Content.BeginUpdate();
            e.Node.Nodes.RemoveByKey(ObjectNode.DummyNodeKey);

            switch (e.Node)
            {
                case ObjectNode expandingObjectNode when expandingObjectNode.Object != null:
                {
                    ExpandObjectNode(expandingObjectNode);
                    var tableItem = expandingObjectNode.Object.Table;
                    foreach (var objectNode in
                             from obj in _UnrealPackage.Exports
                             where obj.OuterTable == tableItem || obj.ArchetypeTable == tableItem
                             select CreateNode((UObjectTableItem)obj))
                    {
                        InitializeObjectNode(objectNode);
                        expandingObjectNode.Nodes.Add(objectNode);
                    }

                    break;
                }
                
                default:
                {
                    if ((string)e.Node.Tag == "Dependencies")
                    {
                        if (_UnrealPackage.Imports != null)
                        {
                            foreach (var importItem in _UnrealPackage.Imports.Where(table =>
                                         table.OuterIndex == 0 && table.ClassName == "Package"))
                                GetDependencyOn(importItem, e.Node.Nodes.Add(importItem.ObjectName));
                        }
                    }

                    break;
                }
            }
            TreeView_Content.EndUpdate();
        }
    }
}