using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using UEExplorer.Framework;
using UEExplorer.Properties;
using UEExplorer.UI.Dialogs;
using UEExplorer.UI.Nodes;
using UEExplorer.UI.Tabs;
using UELib;
using UELib.Core;

namespace UEExplorer.UI.ActionPanels
{
    public partial class PackageExplorerPanel : UserControl
    {
        private readonly ObjectActionsBuilder _ActionsBuilder = new ObjectActionsBuilder();

        private readonly Color _LoadedColor = Color.Black;
        private readonly Color _UnloadedColor = Color.DarkGray;
        
        private readonly ObjectTreeBuilder _ObjectTreeBuilder;
        private readonly ContextProvider _ContextProvider;
        private readonly PackageManager _PackageManager;

        private string _CurrentFilterText = string.Empty;

        private Timer _FilterTextChangedTimer;

        public PackageExplorerPanel(ContextProvider contextProvider, PackageManager packageManager)
        {
            InitializeComponent();

            _ObjectTreeBuilder = new ObjectTreeBuilder(FilterNodeDelegate, SortNodeDelegate);

            _ContextProvider = contextProvider;
            _ContextProvider.ContextChanged += ContextProviderOnContextChanged;

            _PackageManager = packageManager;
            _PackageManager.PackageRegistered += PackageManagerOnPackageRegistered;
            _PackageManager.PackageLoaded += PackageManagerOnPackageLoaded;
            _PackageManager.PackageInitialized += PackageManagerOnPackageInitialized;
            _PackageManager.PackageUnloaded += PackageManagerOnPackageUnloaded;
        }

        ~PackageExplorerPanel()
        {
            _PackageManager.PackageRegistered -= PackageManagerOnPackageRegistered;
            _PackageManager.PackageLoaded -= PackageManagerOnPackageLoaded;
            _PackageManager.PackageInitialized -= PackageManagerOnPackageInitialized;
            _PackageManager.PackageUnloaded -= PackageManagerOnPackageUnloaded;
        }

        private void PackageExplorerPanel_Load(object sender, EventArgs e)
        {
            var packages = _PackageManager.EnumeratePackages();
            foreach (var packageReference in packages)
            {
                var rootPackageNode = CreateRootPackageNode(packageReference);
                TreeViewPackages.SelectedNode = rootPackageNode;
                AddRootPackageNode(rootPackageNode);
            }
        }

        private void ContextProviderOnContextChanged(object sender, ContextChangedEventArgs e)
        {
            object target = e.Context.Target;
            if (target == null)
            {
                TreeViewPackages.SelectedNode = null;
                return;
            }

            var matchingNode = FindTaggedNode(target, TreeViewPackages.Nodes);
            //TreeViewPackages.SelectedNode = matchingNode;
        }

        private TreeNode FindTaggedNode(object tag, TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Tag == tag)
                {
                    return node;
                }

                var subNode = FindTaggedNode(tag, node.Nodes);
                if (subNode != null)
                {
                    return subNode;
                }
            }

            return null;
        }

        private void PackageManagerOnPackageRegistered(object sender, PackageEventArgs e)
        {
            var packageReference = e.Package;

            var rootPackageNode = CreateRootPackageNode(packageReference);
            AddRootPackageNode(rootPackageNode);
        }

        private void PackageManagerOnPackageLoaded(object sender, PackageEventArgs e)
        {
            var packageReference = e.Package;

            var rootPackageNode = GetRootPackageNode(packageReference);
            rootPackageNode.Nodes.Add(ObjectTreeFactory.DummyNodeKey, "Expandable");

            rootPackageNode.ForeColor = Color.Empty;
        }

        private void PackageManagerOnPackageInitialized(object sender, PackageEventArgs e)
        {
            var packageReference = e.Package;

            var rootPackageNode = GetRootPackageNode(packageReference.Linker);
            rootPackageNode.ForeColor = _LoadedColor;

            if (TreeViewPackages.Nodes.Count == 1)
            {
                rootPackageNode.Expand();
            }
        }

        private void PackageManagerOnPackageUnloaded(object sender, PackageEventArgs e)
        {
            var packageReference = e.Package;

            var rootPackageNode = GetRootPackageNode(packageReference);
            rootPackageNode.Nodes.Clear();
            rootPackageNode.ForeColor = _UnloadedColor;
            rootPackageNode.Remove();
        }

        private IComparable SortNodeDelegate<T>(T exp)
        {
            switch (orderByToolStripComboBox.SelectedIndex)
            {
                case 0:
                    return (exp as UObjectTableItem).Offset;

                case 1:
                    return (exp as UObjectTableItem).ObjectName.ToString();
            }

            return 0;
        }

        private bool FilterNodeDelegate<T>(T exp)
        {
            if (string.IsNullOrEmpty(_CurrentFilterText))
            {
                return false;
            }

            return exp.ToString()
                .IndexOf(_CurrentFilterText, StringComparison.InvariantCultureIgnoreCase) <= 0;
        }

        public void AddRootPackageNode(TreeNode node) => TreeViewPackages.Nodes.Add(node);

        public TreeNode GetRootPackageNode(PackageReference packageReference)
        {
            Debug.Assert(packageReference != null);
            return TreeViewPackages.Nodes[packageReference.FilePath];
        }

        public TreeNode GetRootPackageNode(UnrealPackage linker)
        {
            Debug.Assert(linker != null);
            string name = linker.FullPackageName;
            return TreeViewPackages.Nodes[name];
        }

        public TreeNode CreateRootPackageNode(PackageReference packageReference)
        {
            var node = ObjectTreeFactory.CreateNode(packageReference);
            node.ForeColor = packageReference.IsActive()
                ? _UnloadedColor
                : _LoadedColor;
            return node;
        }

        private void RebuildRootPackagesTree()
        {
            var packages = _PackageManager
                .EnumeratePackages();

            TreeViewPackages.Nodes.Clear();
            foreach (var packageReference in packages)
            {
                var node = CreateRootPackageNode(packageReference);
                AddRootPackageNode(node);

                BuildRootPackageTree(packageReference);
            }
        }

        private void BuildRootPackageTree(PackageReference packageReference)
        {
            var linker = packageReference.Linker;
            Debug.Assert(linker != null);

            var rootPackageNode = GetRootPackageNode(linker);

            TreeViewPackages.BeginUpdate();
            
            if (linker.Summary.CompressedChunks != null &&
                linker.Summary.CompressedChunks.Any())
            {
                var dependenciesNode = new TreeNode("Chunks")
                {
                    Name = "Chunks",
                    Tag = linker,
                    ImageKey = "Chunks",
                    SelectedImageKey = "Chunks"
                };
                dependenciesNode.Nodes.Add(ObjectTreeFactory.DummyNodeKey, "Expandable");
                rootPackageNode.Nodes.Add(dependenciesNode);
            }

            if (linker.Summary.ImportCount != 0)
            {
                var depsNode = CreatePackageDependenciesNode(linker);
                rootPackageNode.Nodes.Add(depsNode);
            }

            var nodes = _ObjectTreeBuilder.Visit(linker);
            if (nodes != null) foreach (var treeNode in nodes)
            {
                rootPackageNode.Nodes.Add(treeNode);
            }

            TreeViewPackages.EndUpdate();
        }

        private TreeNode CreatePackageDependenciesNode(UnrealPackage linker)
        {
            var dependenciesNode = new TreeNode("Imports")
            {
                Name = "Dependencies",
                Tag = linker,
                ImageKey = "Diagram",
                SelectedImageKey = "Diagram"
            };
            dependenciesNode.Nodes.Add(ObjectTreeFactory.DummyNodeKey, "Expandable");
            return dependenciesNode;
        }

        private void TreeViewPackages_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            // Already lazy-loaded.
            if (!e.Node.Nodes.ContainsKey(ObjectTreeFactory.DummyNodeKey))
            {
                return;
            }

            TreeViewPackages.BeginUpdate();
            e.Node.Nodes.RemoveByKey(ObjectTreeFactory.DummyNodeKey);

            var node = e.Node;
            switch (e.Node.Tag)
            {
                case PackageReference packageReference:
                    BuildRootPackageTree(packageReference);
                    break;

                case UImportTableItem item:
                    {
                        node.Nodes.AddRange(_ObjectTreeBuilder.Visit(item).ToArray());
                        break;
                    }

                case UExportTableItem item:
                    {
                        node.Nodes.AddRange(_ObjectTreeBuilder.Visit(item).ToArray());
                        break;
                    }

                case UObject uObject:
                    {
                        var item = uObject.Table;
                        dynamic nodes = _ObjectTreeBuilder.Visit((dynamic)item);
                        if (nodes != null)
                        {
                            node.Nodes.AddRange(nodes.ToArray());
                        }

                        break;
                    }

                default:
                    switch (e.Node.Name)
                    {
                        case "Dependencies":
                            {
                                var linker = (UnrealPackage)node.Tag;
                                if (linker.Imports != null)
                                {
                                    foreach (var imp in linker.Imports.Where(item =>
                                                 item.OuterIndex == 0))
                                    {
                                        var importNode = ObjectTreeFactory.CreateNode(imp);
                                        node.Nodes.Add(importNode);
                                    }
                                }

                                break;
                            }
                    }

                    break;
            }

            TreeViewPackages.EndUpdate();
        }

        private void TreeViewPackages_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.Unknown)
            {
                return;
            }

            _ContextProvider.OnContextChanged(this, new ContextChangedEventArgs(new ContextInfo(ContextActionKind.Auto, e.Node)));
        }

        private void toolStripTextBoxFilter_TextChanged(object sender, EventArgs e)
        {
            if (_FilterTextChangedTimer != null && _FilterTextChangedTimer.Enabled)
            {
                _FilterTextChangedTimer.Stop();
                _FilterTextChangedTimer.Dispose();
                _FilterTextChangedTimer = null;
            }

            if (_FilterTextChangedTimer != null)
            {
                return;
            }

            _FilterTextChangedTimer = new Timer();
            _FilterTextChangedTimer.Interval = 350;
            _FilterTextChangedTimer.Tick += DelayedTextChanges;
            _FilterTextChangedTimer.Start();
        }

        private void DelayedTextChanges(object sender, EventArgs e)
        {
            _FilterTextChangedTimer.Stop();
            _FilterTextChangedTimer.Dispose();
            _FilterTextChangedTimer = null;

            _CurrentFilterText = toolStripTextBoxFilter.Text.Trim();
            RebuildRootPackagesTree();
        }

        private void objectContextMenu_Opening(object sender, CancelEventArgs e)
        {
            var actions = _ActionsBuilder
                .Visit(TreeViewPackages.SelectedNode);

            objectContextMenu.Items.Clear();
            foreach ((string text, var action) in actions)
            {
                var node = objectContextMenu.Items.Add(text);
                node.Tag = action;
            }
        }

        private void objectContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var action = (ContextActionKind)e.ClickedItem.Tag;
            _ContextProvider.OnContextChanged(this, new ContextChangedEventArgs(new ContextInfo(action, TreeViewPackages.SelectedNode)));
        }

        private void TreeViewPackages_NodeMouseHover(object sender, TreeNodeMouseHoverEventArgs e)
        {
            string newToolTipText = ObjectTreeFactory.GetTreeNodeToolTipText(e.Node);
            e.Node.ToolTipText = newToolTipText;
        }

        private void TreeViewPackages_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
            {
                return;
            }

            TreeViewPackages.SelectedNode = e.Node;
        }

        private void toolStripMenuItemReload_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // We should reload the package in place, but legacy code prevents us from making this easy, let's fallback.
            var target = TreeViewPackages.SelectedNode;
            if (target.Tag is PackageReference packageReference)
            {
                _PackageManager.UnloadPackage(packageReference);
                _PackageManager.LoadPackage(packageReference);
            }
        }

        private void findInDocumentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string findText;
            using (var findDialog = new FindDialog())
            {
                if (findDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                findText = findDialog.FindInput.Text;
            }

            UC_PackageExplorer.Traverse(Parent).EmitFind(findText);
        }

        private void findInClassesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string findText;
            using (var findDialog = new FindDialog())
            {
                if (findDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                findText = findDialog.FindInput.Text;
            }

            UC_PackageExplorer.Traverse(Parent).PerformSearchIn<UClass>(findText);
        }

        private void exportScriptsToolStripMenuItem_Click(object sender, EventArgs e) =>
            ExportPackageObjects<UTextBuffer>();

        private void exportClassesToolStripMenuItem1_Click(object sender, EventArgs e) =>
            ExportPackageObjects<UClass>();

        private void ExportPackageObjects<T>()
        {
            var exportPaths = _PackageManager.EnumeratePackages()
                .Select(package => package.Linker)
                .Select(linker => linker.ExportPackageObjects<T>());
            if (!exportPaths.Any())
            {
                return;
            }

            var dialogResult = MessageBox.Show(
                string.Format(Resources.EXPORTED_ALL_PACKAGE_CLASSES, ExportHelpers.PackageExportPath),
                Application.ProductName,
                MessageBoxButtons.YesNo
            );
            if (dialogResult == DialogResult.Yes)
            {
                Process.Start(ExportHelpers.PackageExportPath);
            }
        }

        private void toolStripMenuItem1_DropDownOpening(object sender, EventArgs e)
        {
            var linkers = _PackageManager.EnumeratePackages()
                .Select(package => package.Linker)
                .ToList();

            bool hasAnyClasses = linkers.Any(linker => linker.Exports.Any(exp => exp.ClassIndex == 0));
            exportClassesToolStripMenuItem.Enabled = hasAnyClasses;
            findInClassesToolStripMenuItem.Enabled = hasAnyClasses;

            bool hasAnyScripts =
                linkers.Any(linker => linker.Exports.Any(exp => exp.Class?.ObjectName == "TextBuffer"));
            exportScriptsToolStripMenuItem.Enabled = hasAnyScripts;
        }

        private void orderByToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e) =>
            RebuildRootPackagesTree();

        private void TreeViewPackages_DragOver(object sender, DragEventArgs e) =>
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop)
                ? DragDropEffects.Link
                : DragDropEffects.None;

        private void TreeViewPackages_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return;
            }

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string filePath in files)
            {
                if (!UnrealLoader.IsUnrealFileSignature(filePath))
                {
                    continue;
                }

                BeginInvoke((MethodInvoker)(() => _PackageManager.RegisterPackage(filePath)));
            }
        }
    }
}
