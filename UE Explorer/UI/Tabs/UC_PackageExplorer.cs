using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
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
            {
                var subNode = node.Nodes.Add(item.ObjectName);
                subNode.Tag = item.Object;
                GetDependencyOn(item, subNode);
            }

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
                     where obj.Outer == null && obj.Archetype == null 
                     select CreateNode(obj))
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

        #region Node-ContextMenu Methods

        private void TreeView_Content_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.Right) 
                return;
            
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

        private static void BuildItemNodes(object target, ToolStripItemCollection itemCollection,
            ToolStripItemClickedEventHandler itemClickEvent = null)
        {
            itemCollection.Clear();

            var addItem = (Action<string, ContentNodeAction>)((title, id) =>
            {
                var item = itemCollection.Add(title);
                item.Name = Enum.GetName(typeof(ContentNodeAction), id);
            });
            
            var obj = target as UObject;
            if (obj == null && target is IWithDecompilableObject<UObject> decompilableObject)
                obj = decompilableObject.Object;
            if (obj == null) return;

            if (obj is IUnrealDecompilable) addItem(Resources.NodeItem_ViewObject, ContentNodeAction.Decompile);
            if (obj is IBinaryData) addItem(Resources.UC_PackageExplorer_BuildItemNodes_View_Binary, ContentNodeAction.Binary);
            if (obj is IUnrealViewable)
            {
                if (File.Exists(Program.Options.UEModelAppPath))
                {
                    addItem(Resources.NodeItem_OpenInUEModelViewer, ContentNodeAction.DecompileExternal);
#if DEBUG
                    addItem(Resources.NodeItem_ExportWithUEModelViewer, ContentNodeAction.ExportExternal);
#endif
                }
            }
            if (obj is IUnrealExportable exportableObj && exportableObj.CanExport())
            {
                addItem(Resources.EXPORT_AS, ContentNodeAction.ExportAs);
            }

            if (obj.Outer != null) addItem(Resources.NodeItem_ViewOuter, ContentNodeAction.DecompileOuter);

            if (obj is UStruct uStruct)
            {
                var @class = obj as UClass;
                if (uStruct.ByteCodeManager != null)
                {
                    if (@class != null) addItem(Resources.NodeItem_ViewReplication, ContentNodeAction.DecompileClassReplication);
                    addItem(Resources.NodeItem_ViewTokens, ContentNodeAction.DecompileTokens);
                    addItem(Resources.NodeItem_ViewDisassembledTokens, ContentNodeAction.DisassembleTokens);
                }

                if (uStruct.Properties != null && uStruct.Properties.Any())
                    addItem(Resources.NodeItem_ViewDefaultProperties, ContentNodeAction.DecompileScriptProperties);
            }

            var bufferedObject = obj as IBuffered;
            if (bufferedObject.GetBuffer() != null)
            {
                var bufferedItem = new ToolStripMenuItem
                {
                    Text = Resources.NodeItem_ViewBuffer,
                    Name = Enum.GetName(typeof(ContentNodeAction), ContentNodeAction.ViewBuffer)
                };

                bool shouldAddBufferItem = bufferedObject.GetBufferSize() > 0;

                var tableNode = obj as IContainsTable;
                if (tableNode.Table != null)
                {
                    var tableBufferItem = bufferedItem.DropDownItems.Add(Resources.NodeItem_ViewTableBuffer);
                    tableBufferItem.Name = Enum.GetName(typeof(ContentNodeAction), ContentNodeAction.ViewTableBuffer);
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
                addItem(Resources.NodeItem_ViewException, ContentNodeAction.ViewException);
            }
        }

        private void ViewTools_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (_CurrentContentTarget == null)
                return;

            Enum.TryParse(e.ClickedItem.Name, out ContentNodeAction action);
            PerformNodeAction(_CurrentContentTarget, action);
        }

        private void _OnContentItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Enum.TryParse(e.ClickedItem.Name, out ContentNodeAction action);
            PerformNodeAction(TreeView_Content.SelectedNode, action);
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

        private void PerformNodeAction(object target, ContentNodeAction action, bool trackHistory = true)
        {
            if (target == null)
                return;

            var obj = target as UObject;
            if (obj == null)
            {
                switch (target)
                {
                    case IWithDecompilableObject<UObject> decompilableObject:
                        obj = decompilableObject.Object;
                        break;

                    // Redirect this node to its tag if set (i.e. Within -> Class)
                    case TreeNode node when node.Tag is UObject _obj:
                        obj = _obj;
                        break;
                    
                    // See if we can concatenate all sub tree nodes that have decompilable content.
                    case TreeNode node:
                        switch (action)
                        {
                            case ContentNodeAction.Decompile:
                                var s = new StringBuilder();
                                foreach (var subNode in node.Nodes)
                                {
                                    if (subNode is IUnrealDecompilable decompilable)
                                    {
                                        s.AppendLine(decompilable.Decompile());
                                    }
                                }
                                SwitchContentPanel(target, ContentNodeAction.Decompile, s.ToString());
                                TrackNodeAction(node, target, ContentNodeAction.Decompile);
                                return;
                        
                            default:
                                return;
                        }
                }
            }
            
            if (trackHistory)
            {
                TrackNodeAction(target, obj, action);
            }
            
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
                        SwitchContentPanel(obj, ContentNodeAction.Decompile);
                        break;

                    case ContentNodeAction.Binary:
                        SwitchContentPanel(obj, action);
                        break;

                    case ContentNodeAction.DecompileOuter:
                        Debug.Assert(obj.Outer != null, "obj.Outer != null");
                        SwitchContentPanel(obj.Outer, ContentNodeAction.Decompile);
                        break;

                    case ContentNodeAction.DecompileClassReplication:
                    {
                        if (obj is UClass replicationClass)
                        {
                            string text = replicationClass.FormatReplication();
                            SwitchContentPanel(replicationClass, ContentNodeAction.Decompile, text);
                        }

                        break;
                    }

                    case ContentNodeAction.DecompileScriptProperties:
                    {
                        if (obj is UStruct unStruct)
                        {
                            string text = obj.Default is UStruct ? unStruct.FormatDefaultProperties() : null;
                            SwitchContentPanel(obj.Default, ContentNodeAction.Decompile, text);
                        }

                        break;
                    }

                    case ContentNodeAction.DisassembleTokens:
                    {
                        var unStruct = obj as UStruct;
                        if (unStruct?.ByteCodeManager != null)
                        {
                            var codeDec = unStruct.ByteCodeManager;
                            codeDec.Deserialize();
                            codeDec.InitDecompile();

                            _DisassembleTokensTemplate = LoadTemplate("struct.tokens-disassembled");
                            string text = DisassembleTokens(unStruct, codeDec, codeDec.DeserializedTokens.Count);
                            SwitchContentPanel(obj.Default, ContentNodeAction.Decompile, text);
                        }

                        break;
                    }

                    case ContentNodeAction.DecompileTokens:
                    {
                        var unStruct = obj as UStruct;
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

                            SwitchContentPanel(obj.Default, ContentNodeAction.Decompile, text);
                        }
                        
                        break;
                    }

                    case ContentNodeAction.ViewBuffer:
                    {
                        var bufferObject = (IBuffered)obj;
                        Debug.Assert(bufferObject != null, nameof(bufferObject) + " != null");
                        if (bufferObject.GetBufferSize() > 0) ViewBufferFor(bufferObject);
                        break;
                    }

                    case ContentNodeAction.ViewTableBuffer:
                    {
                        var tableObject = target as IContainsTable ?? obj;
                        Debug.Assert(tableObject != null, nameof(tableObject) + " != null");
                        ViewBufferFor(tableObject.Table);
                        break;
                    }

                    case ContentNodeAction.ViewException:
                    {
                        if (target is ObjectNode oNode) SetContentText(oNode, GetExceptionMessage(oNode.Object));
                        break;
                    }

                    case ContentNodeAction.ExportAs:
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

                            string fileName = obj.Name;
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

        private void SwitchContentPanel(object target, ContentNodeAction action, [CanBeNull] string contentText = null)
        {
            Debug.Assert(target != null, nameof(target) + " != null");
            _CurrentContentTarget = target;
                        
            TextContentPanel.Visible = false;
            BinaryDataPanel.Visible = false;
            
            BuildItemNodes(target, ViewTools.DropDownItems);
            ViewTools.Enabled = ViewTools.DropDownItems.Count > 0;
            

            switch (action)
            {
                case ContentNodeAction.Decompile:
                    TextContentPanel.Visible = true;
                    if (contentText != null)
                    {
                        SetContentTitle(target.ToString());
                        SetContentText(target, contentText);
                    }
                    else if (target is UObject obj)
                    {
                        SetContentTitle(obj.GetOuterGroup());
                        SetContentText(obj, obj.Decompile());
                    }
                    break;

                case ContentNodeAction.Binary:
                    Debug.Assert(target is IBinaryData);
                    BinaryDataPanel.Visible = true;
                    if (target is IBinaryData binaryDataObject)
                    {
                        SetContentTitle(binaryDataObject.GetBufferId(true));
                        BinaryDataFieldsPanel.BinaryFieldBindingSource.DataSource = binaryDataObject.BinaryMetaData?.Fields;
                    }
                    break;
                
                default:
                    throw new NotImplementedException($"Invalid action {action}");
            }
        }

        private static readonly string _TemplateDir = Path.Combine(Program.ConfigDir, "Templates");

        private static string LoadTemplate(string name)
        {
            return File.ReadAllText(Path.Combine(_TemplateDir, name + ".txt"), Encoding.ASCII);
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

        private struct ContentHistoryData
        {
            public object SelectedNode;
            public object Target;
            public ContentNodeAction Action;

            public double Y, X;
            
            public int SelectStart;
            public int SelectLength;
        }

        private readonly List<ContentHistoryData> _ContentHistory = new List<ContentHistoryData>();
        private int _CurrentHistoryIndex = -1;
        private object _CurrentContentTarget;

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

        public void SetContentText(object node, string content, bool resetView = true)
        {
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

            var data = new ContentHistoryData
            {
                SelectedNode = node, 
                Target = target, 
                Action = action
            };
            _ContentHistory.Add(data);

            // Maximum 10 can be buffered; remove last one
            if (_ContentHistory.Count > 10)
                _ContentHistory.RemoveRange(0, 1);
            else ++_CurrentHistoryIndex;

            if (_CurrentHistoryIndex > 0) PrevButton.Enabled = true;
        }

        private void UpdateContentHistoryData(int historyIndex)
        {
            var content = _ContentHistory[historyIndex];
            content.X = TextEditorPanel.textEditor.HorizontalOffset;
            content.Y = TextEditorPanel.textEditor.VerticalOffset;
            content.SelectStart = TextEditorPanel.textEditor.SelectionStart;
            content.SelectLength = TextEditorPanel.textEditor.SelectionLength;
            _ContentHistory[historyIndex] = content;
        }

        private void RestoreContentHistoryData(int historyIndex)
        {
            var data = _ContentHistory[historyIndex];
            _CurrentContentTarget = data.Target;
            PerformNodeAction(data.Target, data.Action, false);
            
            if (data.SelectedNode is TreeNode selectedNode)
            {
                RestoreSelectedNode(selectedNode);
            }

            TextEditorPanel.textEditor.ScrollToVerticalOffset(data.Y);
            TextEditorPanel.textEditor.ScrollToHorizontalOffset(data.X);
            TextEditorPanel.textEditor.Select(data.SelectStart, data.SelectLength);
        }

        private void ToolStripButton_Backward_Click(object sender, EventArgs e)
        {
            if (_CurrentHistoryIndex - 1 <= -1)
                return;

            FilterText.Text = string.Empty;
            UpdateContentHistoryData(_CurrentHistoryIndex);
            RestoreContentHistoryData(--_CurrentHistoryIndex);

            if (_CurrentHistoryIndex == 0) PrevButton.Enabled = false;
            NextButton.Enabled = true;
        }

        private void ToolStripButton_Forward_Click(object sender, EventArgs e)
        {
            if (_CurrentHistoryIndex + 1 >= _ContentHistory.Count)
                return;

            FilterText.Text = string.Empty;
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

        private Timer _FilterTextChangedTimer = null;
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
            
            _FilterTextChangedTimer = new Timer();
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
            e.Graphics.DrawRectangle(_BorderPen, 0, 0, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1);
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
            PerformNodeAction(e.Node, ContentNodeAction.Decompile);
        }

        private int _FindCount;
        private TabPage _FindTab;

        private void FindInClassesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Debug.Assert(_UnrealPackage != null, nameof(_UnrealPackage) + " != null");
            
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
                    PerformNodeAction(unClass, ContentNodeAction.Decompile);
                    TrackNodeAction(nodeEvent.Node, unClass, ContentNodeAction.Decompile);
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

            Debug.Assert(_UnrealPackage != null, nameof(_UnrealPackage) + " != null");
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
                            PerformNodeAction(obj, ContentNodeAction.DecompileClassReplication);
                            TrackNodeAction(null, obj, ContentNodeAction.DecompileClassReplication);
                            return true;
                        }

                        break;

                    case "tokens":
                        if (obj is UStruct)
                        {
                            PerformNodeAction(obj, ContentNodeAction.DecompileTokens);
                            TrackNodeAction(null, obj, ContentNodeAction.DecompileTokens);
                            return true;
                        }

                        break;

                    case "tokens-disassembled":
                        if (obj is UStruct)
                        {
                            PerformNodeAction(obj, ContentNodeAction.DisassembleTokens);
                            TrackNodeAction(null, obj, ContentNodeAction.DisassembleTokens);
                            return true;
                        }

                        break;

                    case "default-properties":
                        if (obj is UStruct)
                        {
                            PerformNodeAction(obj, ContentNodeAction.DecompileScriptProperties);
                            TrackNodeAction(null, obj, ContentNodeAction.DecompileScriptProperties);
                            return true;
                        }

                        break;
                }
            }

            SwitchContentPanel(obj, ContentNodeAction.Decompile);
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
                             where obj.Outer == tableItem || obj.Archetype == tableItem
                             select CreateNode(obj))
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
                            {
                                var node = e.Node.Nodes.Add(importItem.ObjectName);
                                node.Tag = importItem.Object;
                                GetDependencyOn(importItem, node);
                            }
                        }
                    }

                    break;
                }
            }
            TreeView_Content.EndUpdate();
        }
    }
}