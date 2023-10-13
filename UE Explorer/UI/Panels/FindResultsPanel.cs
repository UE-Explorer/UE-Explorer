using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using UEExplorer.Framework;
using UELib.Core;
using static UEExplorer.TextSearchHelpers;

namespace UEExplorer.UI.Panels
{
    public partial class FindResultsPanel : UserControl
    {
        public FindResultsPanel()
        {
            InitializeComponent();
        }

        private void UpdateDataCells(List<List<DocumentResult>> results)
        {
            // We add the rows programmatically, because assigning a data source to a data table overrides the columns setup.
            // Not to mention using a data table is wasteful.
            Debug.Assert(findResultsTreeGridView.DataSource == null);

            if (results == null)
            {
                findResultsTreeGridView.GridNodes.Clear();
                return;
            }

            findResultsTreeGridView.BeginUpdate();
            // Horrible solution, but holy the tree data grid isn't convenient either.
            foreach (var documentResults in results)
            {
                var packageResult = documentResults[0];
                string packagePath = ObjectPathBuilder.GetPath((PackageReference)packageResult.Document);
                int packageResultsCount = documentResults
                    .Select(r => r.Results?.Count ?? 0)
                    .Aggregate((a, b) => a + b);

                var packageRow = findResultsTreeGridView.GridNodes.Add($"{packagePath} ({packageResultsCount})");
                packageRow.ImageIndex = 0;

                for (int i = 1; i < documentResults.Count; i++)
                {
                    var documentResult = documentResults[i];
                    object document = documentResult.Document;
                    string path = ObjectPathBuilder.GetPath((UObject)document);
                    int documentResultsCount = documentResult.Results.Count;

                    var documentRow = packageRow.Nodes.Add($"{path} ({documentResultsCount})");
                    documentRow.Tag = (document, SourceLocation.Empty);
                    documentRow.ImageIndex = 1;

                    foreach (var result in documentResult.Results)
                    {
                        var matchRow = documentRow.Nodes.Add(path, result.Location.Line, result.Location.Column);
                        matchRow.Tag = (document, result);
                        matchRow.ImageIndex = 2;
                    }
                }
            }

            findResultsTreeGridView.EndUpdate();
        }

        private void treeGridFindResults_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            var row = findResultsTreeGridView.GetNodeForRow(e.RowIndex);
            if (row.Tag == null)
            {
                return;
            }

            (object target, var source) = ((object, SourceLocation))row.Tag;

            var contextService = ServiceHost.GetRequired<ContextService>();
            if (!contextService.IsActiveTarget(target))
            {
                contextService.OnContextChanged(this, new ContextChangedEventArgs(new ContextInfo(
                    ContextActionKind.Target,
                    target,
                    source)));
            }

            // We need to invoke so that the TextEditor will have reflected the changes.
            BeginInvoke((MethodInvoker)(() =>
            {
                contextService.OnContextChanged(this, new ContextChangedEventArgs(new ContextInfo(
                    ContextActionKind.Location,
                    target,
                    source)));
            }));
        }

        public void UpdateResults(List<List<DocumentResult>> results) =>
            BeginInvoke((MethodInvoker)(() => UpdateDataCells(results)));
    }
}
