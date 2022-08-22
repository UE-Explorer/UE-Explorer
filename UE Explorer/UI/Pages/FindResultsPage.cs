using Krypton.Navigator;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using UEExplorer.Properties;
using UEExplorer.UI.Panels;
using UELib;

namespace UEExplorer.UI.Pages
{
    public class FindResultsPage : KryptonPage
    {
        public FindResultsPanel FindResultsPanel { get; }

        public FindResultsPage()
        {
            FindResultsPanel = new FindResultsPanel();
            FindResultsPanel.Dock = DockStyle.Fill;
            Controls.Add(FindResultsPanel);
        }

        public async Task PerformSearch(List<IUnrealDecompilable> contents, string searchText)
        {
            UpdateText(searchText);

            var documentResults = new List<TextSearchHelpers.DocumentResult>();
            await Task.Run(() =>
            {
                foreach (var content in contents)
                {
                    string textContent = content.Decompile();
                    var findResults = TextSearchHelpers.FindText(textContent, searchText);
                    if (!findResults.Any()) continue;

                    var document = new TextSearchHelpers.DocumentResult
                    {
                        Results = findResults,
                        Document = content
                    };
                    documentResults.Add(document);
                }

            });
            FindResultsPanel.BuildTreeFromDocumentResults(documentResults);
        }

        private void UpdateText(string searchText)
        {
            Text = string.Format(Resources.FIND_RESULTS_TITLE, searchText);
            TextTitle = string.Format(Resources.FindResultsPage_TextTitle___0__, searchText);
        }
    }
}
