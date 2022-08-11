using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using UEExplorer.UI.Nodes;
using UEExplorer.UI.Tabs;
using UELib;
using UELib.Core;
using UEExplorer.UI.Dialogs;
using UEExplorer.Properties;

namespace UEExplorer.UI.ActionPanels
{
    // TODO: Implement a PackageManager (Controller?) to hold an available list of linkers.
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
            var linkers = GetLinkers().ToList();

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

        private void TreeViewPackages_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.Unknown) return;
            UC_PackageExplorer.Traverse(Parent).EmitObjectNodeAction(e.Node, ContentNodeAction.Auto);
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
            foreach ((string text, var action) in actions)
            {
                var node = objectContextMenu.Items.Add(text);
                node.Tag = action;
            }
        }
        
        private void objectContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var action = (ContentNodeAction)e.ClickedItem.Tag;
            UC_PackageExplorer.Traverse(Parent).EmitObjectNodeAction(TreeViewPackages.SelectedNode, action);
        }
        
        private void TreeViewPackages_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            TreeViewPackages.SelectedNode = e.Node;
        }

        private void toolStripMenuItemView_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var target = TreeViewPackages.SelectedNode;
            UC_PackageExplorer.Traverse(Parent).EmitObjectNodeAction(target, ContentNodeAction.Auto);
        }

        private void toolStripMenuItemReload_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // We should reload the package in place, but legacy code prevents us from making this easy, let's fallback.
            var target = TreeViewPackages.SelectedNode;
            if (target.Tag is UnrealPackage package)
            {
                UC_PackageExplorer.Traverse(Parent).ReloadPackage();
            }
        }
        
        private void findInDocumentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string findText;
            using (var findDialog = new FindDialog())
            {
                findDialog.FindInput.Text = Clipboard.GetText();
                if (findDialog.ShowDialog() != DialogResult.OK) return;
                findText = findDialog.FindInput.Text;
            }
            
            UC_PackageExplorer.Traverse(Parent).EmitFind(findText);
        }

        private void findInClassesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string findText;
            using (var findDialog = new FindDialog())
            {
                findDialog.FindInput.Text = Clipboard.GetText();
                if (findDialog.ShowDialog() != DialogResult.OK) return;
                findText = findDialog.FindInput.Text;
            }

            UC_PackageExplorer.Traverse(Parent).PerformSearchIn<UClass>(findText);
        }
        
        private void exportScriptsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportPackageObjects<UTextBuffer>();
        }

        private void exportClassesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ExportPackageObjects<UClass>();
        }

        private void ExportPackageObjects<T>()
        {
            var exportPaths = GetLinkers().Select(linker => linker.ExportPackageObjects<T>());
            if (!exportPaths.Any())
            {
                return;
            }
            
            var dialogResult = MessageBox.Show(
                string.Format(Resources.EXPORTED_ALL_PACKAGE_CLASSES, ExportHelpers.PackageExportPath),
                Application.ProductName,
                MessageBoxButtons.YesNo
            );
            if (dialogResult == DialogResult.Yes) Process.Start(ExportHelpers.PackageExportPath);
        }

        private IEnumerable<UnrealPackage> GetLinkers()
        {
            return TreeViewPackages.Nodes.OfType<TreeNode>().Select(node => (UnrealPackage)node.Tag);
        }

        private void toolStripMenuItem1_DropDownOpening(object sender, EventArgs e)
        {
            var linkers = GetLinkers().ToList();
            
            bool hasAnyClasses = linkers.Any(linker => linker.Exports.Any(exp => exp.ClassIndex == 0));
            exportClassesToolStripMenuItem.Enabled = hasAnyClasses;
            findInClassesToolStripMenuItem.Enabled = hasAnyClasses;

            bool hasAnyScripts = linkers.Any(linker => linker.Exports.Any(exp => exp.Class?.ObjectName == "TextBuffer"));
            exportScriptsToolStripMenuItem.Enabled = hasAnyScripts;
        }
    }
}