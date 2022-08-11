using System.Collections.Generic;
using System.Windows.Forms;
using Krypton.Toolkit;
using UEExplorer.UI.Nodes;
using UEExplorer.UI.Tabs;
using UELib.Core;

namespace UEExplorer.UI.Panels
{
    public partial class FindResultsPanel : UserControl
    {
        public FindResultsPanel()
        {
            InitializeComponent();
        }

        public void BuildTreeFromDocumentResults(List<TextSearchHelpers.DocumentResult> documentResults)
        {
            treeViewFindResults.BeginUpdate();
            var imageKeySelector = new ObjectImageKeySelector();
            foreach (var documentResult in documentResults)
            {
                var @class = (UClass)documentResult.Document;
                string imageKey = @class.Accept(imageKeySelector);

                var text = $"{ObjectPathBuilder.GetPath(@class)} ({documentResult.Results.Count})";
                var documentNode = treeViewFindResults.Nodes.Add(text);
                documentNode.Tag = documentResult;
                documentNode.ImageKey = imageKey;
                documentNode.SelectedImageKey = imageKey;
                foreach (var result in documentResult.Results)
                {
                    var resultNode = documentNode.Nodes.Add(result.ToString());
                    resultNode.Tag = result;
                }
            }
            treeViewFindResults.EndUpdate();
        }

        private void treeViewFindResults_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // Assuming this was triggered by assigning SelectNode
            if (e.Action == TreeViewAction.Unknown)
            {
                return;
            }
            
            if (e.Node.Tag is TextSearchHelpers.FindResult findResult)
            {
                object document = ((TextSearchHelpers.DocumentResult)e.Node.Parent.Tag).Document;
                UC_PackageExplorer.Traverse(Parent).EmitObjectNodeAction(document, ContentNodeAction.Decompile);
                UC_PackageExplorer.Traverse(Parent).EmitFind(findResult);
            }
        }
    }
}
