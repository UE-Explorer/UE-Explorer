using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using UEExplorer.UI.Nodes;
using UEExplorer.UI.Tabs;
using UELib;
using UELib.Core;

namespace UEExplorer.UI.ActionPanels
{
    public partial class PackageExplorerPanel : UserControl
    {
        private readonly ObjectTreeBuilder _ObjectTreeBuilder = new ObjectTreeBuilder();
        private readonly ObjectActionsBuilder _ActionsBuilder = new ObjectActionsBuilder();

        public PackageExplorerPanel()
        {
            InitializeComponent();
        }

        public void AddRootPackageNode(TreeNode node)
        {
            TreeViewPackages.Nodes.Add(node);
        }

        public TreeNode GetRootPackageNode(UnrealPackage linker)
        {
            Debug.Assert(linker != null);
            return TreeViewPackages.Nodes[linker.PackageName];
        }

        public TreeNode CreateRootPackageNode(UnrealPackage linker)
        {
            return ObjectTreeFactory.CreateNode(linker);
        }

        private void AddToRoot(TreeNode root, TreeNode node)
        {
            root.Nodes.Add(node);
            if (root.Nodes.Count == 1)
            {
                root.Expand();
            }
        }
        
        private void FilterRootPackagesTree(string filterText)
        {
            var linkers = new List<UnrealPackage>();
            foreach (TreeNode node in TreeViewPackages.Nodes)
            {
                var linker = (UnrealPackage)node.Tag;
                Debug.Assert(linker != null);
                linkers.Add(linker);
            }

            TreeViewPackages.Nodes.Clear();
            foreach (var linker in linkers)
            {
                var node = CreateRootPackageNode(linker);
                AddRootPackageNode(node);
                BuildRootPackageTree(linker, filterText);
            }
        }
        
        public void BuildRootPackageTree(UnrealPackage linker, string filterText = null)
        {
            Debug.Assert(linker != null);
            Debug.Assert(linker.Exports != null);

            TreeViewPackages.BeginUpdate();

            var rootPackageNode = GetRootPackageNode(linker);

            // Lazy recursive, creates a base node for each export with no Outer, if a matching outer is found it will be appended to that base node upon expansion.
            foreach (var objectNode in
                     from exp in linker.Exports
                     // Filter out deleted exports
                     where exp.ObjectName != "None"
                     where filterText == null
                         ? exp.Outer == null
                         : exp.ObjectName.ToString().IndexOf(filterText, StringComparison.InvariantCultureIgnoreCase) !=
                           -1
                     select ObjectTreeFactory.CreateNode(exp))
            {
                AddToRoot(rootPackageNode, objectNode);
            }

            TreeViewPackages.EndUpdate();
        }

        public void BuildDependenciesTree(UnrealPackage linker)
        {
            var rootPackageNode = GetRootPackageNode(linker);
            var dependenciesNode = CreatePackageDependenciesNode(linker);
            rootPackageNode.Nodes.Add(dependenciesNode);
        }

        public void BuildImportTree(UImportTableItem outerImp, TreeNode parentNode)
        {
            foreach (var imp in
                     from imp in outerImp.Owner.Imports
                     where imp != outerImp && imp.Outer == outerImp
                     select imp)
            {
                var objectNode = ObjectTreeFactory.CreateNode(imp);
                parentNode.Nodes.Add(objectNode);
                BuildImportTree(imp, objectNode);
            }
        }

        public TreeNode CreatePackageDependenciesNode(UnrealPackage linker)
        {
            var dependenciesNode = new TreeNode("Dependencies")
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
                case UObjectTableItem item:
                {
                    var subNodes = item.Object?.Accept(_ObjectTreeBuilder);
                    if (subNodes != null) node.Nodes.AddRange(subNodes);

                    foreach (var objectNode in
                             from exp in item.Owner.Exports
                             where exp.Outer == item
                             select ObjectTreeFactory.CreateNode(exp))
                    {
                        node.Nodes.Add(objectNode);
                    }

                    break;
                }

                case UObject uObject:
                {
                    var subNodes = uObject.Accept(_ObjectTreeBuilder);
                    if (subNodes != null) node.Nodes.AddRange(subNodes);

                    var item = uObject.Table;
                    foreach (var objectNode in
                             from exp in item.Owner.Exports
                             where exp.Outer == item
                             select ObjectTreeFactory.CreateNode(exp))
                    {
                        node.Nodes.Add(objectNode);
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
                                             item.OuterIndex == 0 && item.ClassName == "Package"))
                                {
                                    var importNode = ObjectTreeFactory.CreateNode(imp);
                                    node.Nodes.Add(importNode);
                                    BuildImportTree(imp, importNode);
                                }
                            }

                            break;
                        }
                    }

                    break;
            }

            TreeViewPackages.EndUpdate();
        }

        // Hacky
        private UC_PackageExplorer GetMain()
        {
            for (var c = Parent; c != null; c = c.Parent)
            {
                if ((c is UC_PackageExplorer packageExplorer)) return packageExplorer;
            }
            throw new NotSupportedException();
        }

        private void TreeViewPackages_AfterSelect(object sender, TreeViewEventArgs e)
        {
            GetMain().OnObjectNodeAction(e.Node, ContentNodeAction.Auto);
        }

        private Timer _FilterTextChangedTimer;

        private void toolStripTextBoxFilter_TextChanged(object sender, EventArgs e)
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
            _FilterTextChangedTimer.Tick += DelayedTextChanges;
            _FilterTextChangedTimer.Start();
        }

        private void DelayedTextChanges(object sender, EventArgs e)
        {
            _FilterTextChangedTimer.Stop();
            _FilterTextChangedTimer.Dispose();
            _FilterTextChangedTimer = null;

            string query = toolStripTextBoxFilter.Text.Trim();
            FilterRootPackagesTree(query.Length == 0 ? null : query);
        }
        
        private void objectContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var actions = _ActionsBuilder
                .Visit(TreeViewPackages.SelectedNode);

            objectContextMenu.Items.Clear();
            foreach ((string text, ContentNodeAction action) in actions)
            {
                var node = objectContextMenu.Items.Add(text);
                node.Tag = action;
            }
        }
        
        private void objectContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var action = (ContentNodeAction)e.ClickedItem.Tag;
            GetMain().OnObjectNodeAction(TreeViewPackages.SelectedNode, action);
        }
        
            //var main = GetMain();
            //object tag = main.PickBestTarget(TreeViewPackages.SelectedNode, ContentNodeAction.Auto);
            //var action = main.PickBestContentNodeAction(tag, ContentNodeAction.Auto);
            //main.InsertNewContentPanel(tag, action);
        
        private void TreeViewPackages_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            TreeViewPackages.SelectedNode = e.Node;
        }

        private void toolStripMenuItemView_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var target = TreeViewPackages.SelectedNode;
            GetMain().OnObjectNodeAction(target, ContentNodeAction.Auto);
        }

        private void toolStripMenuItemReload_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // We should reload the package in place, but legacy code prevents us from making this easy, let's fallback.
            var target = TreeViewPackages.SelectedNode;
            if (target.Tag is UnrealPackage package)
            {
                GetMain().ReloadPackage();
            }
        }
    }
}