using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using UEExplorer.Framework;
using UEExplorer.Framework.Commands;
using UEExplorer.Framework.UI;
using UEExplorer.Framework.UI.Commands;
using UEExplorer.UI.Nodes;
using UELib;

namespace UEExplorer.UI.ActionPanels
{
    public partial class PackageExplorerPanel : UserControl, ITrackingContext
    {
        private readonly ContextService _ContextService;

        private readonly NodeSwapCollection _NodesSwap;
        private readonly ObjectTreeBuilder _ObjectTreeBuilder;

        private readonly PackageManager _PackageManager;

        private readonly Brush _Brush;

        private string _CurrentFilterText = string.Empty;

        public PackageExplorerPanel()
        {
            InitializeComponent();

            _Brush = new SolidBrush(packagesTreeView.ForeColor);

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }

            IsTracking = isTrackingToolStripButton.Checked;
            _NodesSwap = new NodeSwapCollection(packagesTreeView);

            UpdateSortBy(sortByNameToolStripMenuItem);

            _ContextService = ServiceHost.GetRequired<ContextService>();
            _ContextService.ContextChanged += ContextServiceOnContextChanged;

            _PackageManager = ServiceHost.GetRequired<PackageManager>();
            _PackageManager.PackageRegistered += PackageManagerOnPackageRegistered;
            _PackageManager.PackageLoaded += PackageManagerOnPackageLoaded;
            _PackageManager.PackageInitialized += PackageManagerOnPackageInitialized;
            _PackageManager.PackageUnloaded += PackageManagerOnPackageUnloaded;
            _PackageManager.PackageRemoved += PackageManagerOnPackageRemoved;
            _PackageManager.PackageError += PackageManagerOnPackageError;

            _ObjectTreeBuilder = new ObjectTreeBuilder(FilterNodeDelegate);
        }

        public bool IsTracking { get; set; }

        ~PackageExplorerPanel()
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }

            _PackageManager.PackageRegistered -= PackageManagerOnPackageRegistered;
            _PackageManager.PackageLoaded -= PackageManagerOnPackageLoaded;
            _PackageManager.PackageInitialized -= PackageManagerOnPackageInitialized;
            _PackageManager.PackageUnloaded -= PackageManagerOnPackageUnloaded;
            _PackageManager.PackageRemoved -= PackageManagerOnPackageRemoved;
            _PackageManager.PackageError -= PackageManagerOnPackageError;
        }

        private void PackageExplorerPanel_Load(object sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }

            // Present the already-loaded packages.
            var packages = _PackageManager.EnumeratePackages();
            packagesTreeView.BeginUpdate();
            foreach (var packageReference in packages)
            {
                var rootPackageNode = CreateRootPackageNode(packageReference);
                rootPackageNode.Nodes.Add(ObjectTreeFactory.DummyNodeKey, "Expandable");
                AddRootPackageNode(rootPackageNode);
                packagesTreeView.SelectedNode = rootPackageNode;
            }

            UpdateTreeViewNodeSorter();
            packagesTreeView.EndUpdate();
        }

        private void ContextServiceOnContextChanged(object sender, ContextChangedEventArgs e)
        {
            if (!IsTracking)
            {
                return;
            }

            object target = e.Context.Target;
            if (target == null)
            {
                //packagesTreeView.SelectedNode = null;
                return;
            }

            var matchingNode = EnumerateNodes(_NodesSwap.Nodes).FirstOrDefault(node => node.Tag == target);
            packagesTreeView.SelectedNode = matchingNode;
        }

        private static IEnumerable<TreeNode> EnumerateNodes(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                yield return node;

                foreach (var child in EnumerateNodes(node.Nodes))
                {
                    yield return child;
                }
            }
        }

        private void PackageManagerOnPackageRegistered(object sender, PackageEventArgs e)
        {
            var packageReference = e.Package;

            packagesTreeView.BeginUpdate();
            var rootPackageNode = CreateRootPackageNode(packageReference);
            AddRootPackageNode(rootPackageNode);
            packagesTreeView.EndUpdate();
        }

        private void PackageManagerOnPackageLoaded(object sender, PackageEventArgs e)
        {
            var packageReference = e.Package;

            packagesTreeView.BeginUpdate();
            var rootPackageNode = GetRootPackageNode(packageReference);
            rootPackageNode.ForeColor = ObjectTreeFactory.PackageLoadedColor;
            rootPackageNode.EnsureVisible();
            packagesTreeView.EndUpdate();
        }

        private void PackageManagerOnPackageInitialized(object sender, PackageEventArgs e)
        {
            var packageReference = e.Package;

            packagesTreeView.BeginUpdate();
            var rootPackageNode = GetRootPackageNode(packageReference.Linker);
            rootPackageNode.Nodes.Add(ObjectTreeFactory.DummyNodeKey, "Expandable");
            if (_NodesSwap.Count == 1)
            {
                rootPackageNode.Expand();
            }

            packagesTreeView.EndUpdate();
        }

        private void PackageManagerOnPackageUnloaded(object sender, PackageEventArgs e)
        {
            var packageReference = e.Package;

            packagesTreeView.BeginUpdate();
            var rootPackageNode = GetRootPackageNode(packageReference);
            rootPackageNode.ForeColor = ObjectTreeFactory.PackageUnloadedColor;
            rootPackageNode.Collapse();
            rootPackageNode.Nodes.Clear();
            packagesTreeView.EndUpdate();
        }

        private void PackageManagerOnPackageRemoved(object sender, PackageEventArgs e)
        {
            var packageReference = e.Package;

            packagesTreeView.BeginUpdate();
            var rootPackageNode = GetRootPackageNode(packageReference);
            RemoveRootPackageNode(rootPackageNode);
            packagesTreeView.EndUpdate();
        }

        private void PackageManagerOnPackageError(object sender, PackageEventArgs e)
        {
            var packageReference = e.Package;

            var rootPackageNode = GetRootPackageNode(packageReference);
            rootPackageNode.ForeColor = ObjectTreeFactory.ErrorColor;
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

        private void AddRootPackageNode(TreeNode node)
        {
            _NodesSwap.Add(node);
            filterTextBox.Enabled = true;
            packagesTreeView.Sort();
        }

        private void RemoveRootPackageNode(TreeNode node)
        {
            node.ForeColor = ObjectTreeFactory.PackageUnloadedColor;
            node.Remove();
            filterTextBox.Enabled = _NodesSwap.Count != 0;
        }

        private TreeNode GetRootPackageNode(PackageReference packageReference)
        {
            Debug.Assert(packageReference != null);
            string name = packageReference.FilePath;
            if (_NodesSwap.IsInFilterState)
            {
                return _NodesSwap.CachedNodes.Find(t => t.Name == name);
            }

            return _NodesSwap.Nodes[name];
        }

        private TreeNode GetRootPackageNode(UnrealPackage linker)
        {
            Debug.Assert(linker != null);
            string name = linker.FullPackageName;
            if (_NodesSwap.IsInFilterState)
            {
                return _NodesSwap.CachedNodes.Find(t => t.Name == name);
            }

            return _NodesSwap.Nodes[name];
        }

        private TreeNode CreateRootPackageNode(PackageReference packageReference)
        {
            var node = ObjectTreeFactory.CreateNode(packageReference);
            node.ForeColor = packageReference.IsActive()
                ? ObjectTreeFactory.PackageLoadedColor
                : ObjectTreeFactory.PackageUnloadedColor;
            return node;
        }

        private void toolStripTextBoxFilter_TextChanged(object sender, EventArgs e)
        {
            filterTreeDelayTimer.Stop();
            filterTreeDelayTimer.Start();
        }

        private void filterTreeDelayTimer_Tick(object sender, EventArgs e)
        {
            filterTreeDelayTimer.Stop();

            string searchText = filterTextBox.Text.Trim();
            _CurrentFilterText = searchText;

            if (searchText == string.Empty)
            {
                packagesTreeView.BeginUpdate();
                _NodesSwap.SwapFilterState(true);
                packagesTreeView.EndUpdate();
                return;
            }

            var nodes = new List<TreeNode>();
            foreach (var packageReference in _PackageManager.EnumeratePackages())
            {
                if (packageReference.Linker == null)
                {
                    continue;
                }

                var groupNode = ObjectTreeFactory.CreateNode(packageReference);
                groupNode.Expand();
                foreach (var exp in packageReference.Linker.Exports)
                {
                    if (exp.ObjectName.ToString().IndexOf(searchText, StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        continue;
                    }

                    var expNode = ObjectTreeFactory.CreateNode(exp);
                    expNode.Text = ObjectPathBuilder.GetPath(exp.Object);
                    groupNode.Nodes.Add(expNode);
                }

                nodes.Add(groupNode);
            }

            packagesTreeView.BeginUpdate();
            _NodesSwap.SwapFilterState(false);
            packagesTreeView.Nodes.AddRange(nodes.ToArray());
            packagesTreeView.EndUpdate();
        }

        private void TreeViewPackages_NodeMouseHover(object sender, TreeNodeMouseHoverEventArgs e)
        {
            const int leftMargin = 8;

            // Cursor.Current.HotSpot
            string newToolTipText = ObjectToolTipTextBuilder.GetToolTipText(e.Node.Tag);
            treeToolTip.Show(newToolTipText,
                this,
                packagesTreeView.Bounds.Left + e.Node.Bounds.Right + leftMargin,
                packagesTreeView.Bounds.Top + e.Node.Bounds.Top,
                2500);
        }

        private void TreeViewPackages_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (packagesTreeView.SelectedNode != e.Node)
                {
                    packagesTreeView.SelectedNode = e.Node;
                }
            }
        }

        private void TreeViewPackages_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Action == TreeViewAction.Unknown)
            {
                // Cancels NodeMouseClick too :(
                //e.Cancel = true;
            }
        }

        private void TreeViewPackages_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.Unknown)
            {
                return;
            }

            object target = e.Node.Tag;
            Contract.Assert(target != null);
            var newContext = new ContextInfo(ContextActionKind.Target, target)
            {
                ResolvedTarget = TargetResolver.Resolve(target)
            };

            _ContextService.OnContextChanged(this,
                new ContextChangedEventArgs(newContext));
        }

        private void packagesTreeView_DoubleClick(object sender, EventArgs e)
        {
            var node = packagesTreeView.SelectedNode;
            if (node == null)
            {
                // No assert, because a user can double click outside of a node's boundary.
                return;
            }

            object subject = node.Tag;
            switch (subject)
            {
                case PackageReference packageReference when packageReference.Linker == null:
                    {
                        BeginInvoke((MethodInvoker)(() => _PackageManager.LoadPackage(packageReference)));
                        break;
                    }
            }
        }

        private void TreeViewPackages_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            var node = e.Node;

            // Already lazy-loaded.
            if (!node.Nodes.ContainsKey(ObjectTreeFactory.DummyNodeKey))
            {
                //e.Cancel = true;
                return;
            }

            packagesTreeView.SuspendLayout();
            packagesTreeView.BeginUpdate();
            node.Nodes.RemoveByKey(ObjectTreeFactory.DummyNodeKey);

            object subject = node.Tag;
            switch (subject)
            {
                case PackageReference packageReference:
                    {
                        var treeNodes = _ObjectTreeBuilder
                            .Visit(packageReference)
                            .OrderBy(n => n, (IComparer<TreeNode>)packagesTreeView.TreeViewNodeSorter)
                            .ToArray();
                        node.Nodes.AddRange(treeNodes);
                        break;
                    }

                case UImportTableItem item:
                    {
                        var treeNodes = _ObjectTreeBuilder
                            .Visit(item)
                            .OrderBy(n => n, (IComparer<TreeNode>)packagesTreeView.TreeViewNodeSorter)
                            .ToArray();
                        node.Nodes.AddRange(treeNodes);
                        break;
                    }

                case UExportTableItem item:
                    {
                        var treeNodes = _ObjectTreeBuilder
                            .Visit(item)
                            .OrderBy(n => n, (IComparer<TreeNode>)packagesTreeView.TreeViewNodeSorter)
                            .ToArray();
                        node.Nodes.AddRange(treeNodes);
                        break;
                    }

                // Note: The exports table is lazily-expanded with "PackageReference".
                case List<UImportTableItem> imports:
                    {
                        var treeNodes = _ObjectTreeBuilder
                            .Visit(imports)
                            .OrderBy(n => n, (IComparer<TreeNode>)packagesTreeView.TreeViewNodeSorter)
                            .ToArray();

                        node.Nodes.AddRange(treeNodes);
                        break;
                    }
            }

            packagesTreeView.EndUpdate();
            packagesTreeView.ResumeLayout();
        }

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

                Program.PushRecentOpenedFile(filePath);
                BeginInvoke((MethodInvoker)(() => _PackageManager.RegisterPackage(filePath)));
            }
        }

        private void objectContextMenu_Opening(object sender, CancelEventArgs e)
        {
            // Occurs if no packages are loaded.
            if (packagesTreeView.SelectedNode == null)
            {
                // TODO: Instead let's popup a menu with the option to choose "Open package from file"
                //e.Cancel = true;
                //return;
            }

            objectContextMenu.Items.Clear();

            object subject = packagesTreeView.SelectedNode?.Tag;

            var builder = new ContextCommandBuilder(ServiceHost.GetRequired<IServiceProvider>());
            var commands = builder.Build<IContextCommand>(subject);
            var items = CommandMenuItemFactory.Create(commands,
                command =>
                {
                    BeginInvoke((MethodInvoker)(() =>
                        ServiceHost.GetRequired<CommandService>().Execute(command, subject)));
                });

            objectContextMenu.Items.AddRange(items.ToArray());

            e.Cancel = objectContextMenu.Items.Count == 0;
        }

        private void objectContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }

        private void packageToolsStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            var linkers = _PackageManager.EnumeratePackages()
                .Select(package => package.Linker)
                .Where(linker => linker?.Exports != null)
                .ToList();
        }

        private void toolStripTextBoxFilter_Paint(object sender, PaintEventArgs e)
        {
            if (filterTextBox.Text != string.Empty)
            {
                return;
            }

            var brush = new SolidBrush(filterTextBox.ForeColor);
            e.Graphics.DrawString(
                "Search",
                filterTextBox.Font,
                brush,
                e.ClipRectangle.X,
                e.ClipRectangle.Y + e.ClipRectangle.Height / 2);
        }

        private void collapseAllToolStripButton_Click(object sender, EventArgs e) => packagesTreeView.CollapseAll();

        private void sortByToolStripSplitButton_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            UpdateSortBy(e.ClickedItem);
            var n = packagesTreeView.SelectedNode;
            UpdateTreeViewNodeSorter();
            // Due sorting the selected node gets lost, so restore it! And scroll it into view as well :)
            if (n != null)
            {
                packagesTreeView.SelectedNode = n;
                n.EnsureVisible();
            }
        }

        private void UpdateSortBy(ToolStripItem item)
        {
            sortByToolStripSplitButton.Image = item.Image;
            foreach (ToolStripMenuItem dropDownItem in sortByToolStripSplitButton.DropDownItems)
            {
                if (dropDownItem == item)
                {
                    dropDownItem.Checked = true;
                    continue;
                }

                dropDownItem.Checked = false;
            }
        }

        private void UpdateTreeViewNodeSorter()
        {
            if (sortByNameToolStripMenuItem.Checked)
            {
                packagesTreeView.TreeViewNodeSorter = new ItemSortByNameComparer();
            }
            else if (sortByTypeToolStripMenuItem.Checked)
            {
                packagesTreeView.TreeViewNodeSorter = new ItemSortByTypeComparer();
            }
            else if (sortByOffsetToolStripMenuItem.Checked)
            {
                packagesTreeView.TreeViewNodeSorter = new ItemSortByOffsetComparer();
            }
            else
            {
                packagesTreeView.TreeViewNodeSorter = new ItemSortByNameComparer();
            }
        }

        private void isTrackingToolStripButton_CheckStateChanged(object sender, EventArgs e) =>
            IsTracking = isTrackingToolStripButton.Checked;

        private void packagesTreeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if ((e.State & TreeNodeStates.Indeterminate) != 0)
            {
                return;
            }

            float typeColumnX = (float)(packagesTreeView.Width * 0.55);

            e.Graphics.DrawString(e.Node.Text, packagesTreeView.Font, _Brush, e.Bounds);
            if (e.Node.Tag is UExportTableItem exportItem)
            {
                e.Graphics.DrawString(exportItem.Class?.GetPath(), packagesTreeView.Font, _Brush,
                    typeColumnX, e.Bounds.Y);
            }
        }

        private class NodeSwapCollection
        {
            public NodeSwapCollection(TreeView view) => Nodes = view.Nodes;

            public List<TreeNode> CachedNodes { get; private set; }

            public TreeNodeCollection Nodes { get; }

            public int Count => Nodes.Count;

            public bool IsInFilterState { get; private set; }

            public void SwapFilterState(bool needsRestore)
            {
                if (needsRestore)
                {
                    Nodes.Clear();
                    Nodes.AddRange(CachedNodes.ToArray());
                    IsInFilterState = false;
                    return;
                }

                if (!IsInFilterState)
                {
                    var nodes = new TreeNode[Nodes.Count];
                    Nodes.CopyTo(nodes, 0);
                    CachedNodes = nodes.ToList();
                    IsInFilterState = true;
                }

                Nodes.Clear();
            }

            public void Add(TreeNode node)
            {
                if (IsInFilterState)
                {
                    CachedNodes.Add(node);
                    return;
                }

                Nodes.Add(node);
            }

            public void Remove(TreeNode node)
            {
                if (IsInFilterState)
                {
                    CachedNodes.Remove(node);
                    return;
                }

                Nodes.Remove(node);
            }
        }

        private class ItemSortByNameComparer : IComparer, IComparer<TreeNode>
        {
            int IComparer.Compare(object x, object y)
            {
                var nodeX = x as TreeNode;
                Debug.Assert(nodeX != null, nameof(nodeX) + " != null");
                var nodeY = y as TreeNode;
                Debug.Assert(nodeY != null, nameof(nodeY) + " != null");

                return Compare(nodeX, nodeY);
            }

            public int Compare(TreeNode x, TreeNode y)
            {
                if (x is UnsortedTreeNode)
                {
                    return y is UnsortedTreeNode ? 0 : -1;
                }

                if (y is UnsortedTreeNode)
                {
                    return 1;
                }

                return string.Compare(x.Text, y.Text, StringComparison.Ordinal);
            }
        }

        private class ItemSortByTypeComparer : IComparer, IComparer<TreeNode>
        {
            int IComparer.Compare(object x, object y)
            {
                var nodeX = x as TreeNode;
                Debug.Assert(nodeX != null, nameof(nodeX) + " != null");
                var nodeY = y as TreeNode;
                Debug.Assert(nodeY != null, nameof(nodeY) + " != null");

                return Compare(nodeX, nodeY);
            }

            public int Compare(TreeNode x, TreeNode y)
            {
                if (x is UnsortedTreeNode)
                {
                    return y is UnsortedTreeNode ? 0 : -1;
                }

                if (y is UnsortedTreeNode)
                {
                    return 1;
                }

                if (GetType(x.Tag) == null)
                {
                    return 0;
                }

                if (GetType(y.Tag) == null)
                {
                    return 0;
                }

                int compare = string.Compare(GetType(x.Tag), GetType(y.Tag), StringComparison.Ordinal);
                if (compare != 0)
                {
                    return compare;
                }

                return string.Compare(x.Text, y.Text, StringComparison.Ordinal);
            }

            private static string GetType(object tag)
            {
                switch (tag)
                {
                    case null:
                        return null;

                    case UObjectTableItem item when item.Object != null:
                        return item.Object.GetClassName();

                    case UObjectTableItem item:
                        switch (item)
                        {
                            case UExportTableItem exp:
                                return exp.Class?.ObjectName.ToString() ?? "Class";

                            case UImportTableItem imp:
                                return imp.ClassName.ToString();
                        }

                        break;
                }

                return null;
            }
        }

        private class ItemSortByOffsetComparer : IComparer, IComparer<TreeNode>
        {
            int IComparer.Compare(object x, object y)
            {
                var nodeX = x as TreeNode;
                Debug.Assert(nodeX != null, nameof(nodeX) + " != null");
                var nodeY = y as TreeNode;
                Debug.Assert(nodeY != null, nameof(nodeY) + " != null");

                return Compare(nodeX, nodeY);
            }

            public int Compare(TreeNode x, TreeNode y)
            {
                if (x is UnsortedTreeNode)
                {
                    return y is UnsortedTreeNode ? 0 : -1;
                }

                if (y is UnsortedTreeNode)
                {
                    return 1;
                }

                if (x.Tag is IBuffered bufferedX && y.Tag is IBuffered bufferedY)
                {
                    return bufferedX.GetBufferPosition().CompareTo(bufferedY.GetBufferPosition());
                }

                return string.Compare(x.Text, y.Text, StringComparison.Ordinal);
            }
        }
    }
}
