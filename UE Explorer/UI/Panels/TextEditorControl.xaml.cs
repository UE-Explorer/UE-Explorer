using System.Diagnostics;

namespace UEExplorer.UI.Panels
{
    public partial class TextEditorControl
    {
        public TextEditorControl()
        {
            InitializeComponent();
        }

        public string GetSelection()
        {
            return TextEditor.TextArea.Selection.GetText();
        }

        private void Copy_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            TextEditor.Copy();
        }

        private void SearchWiki_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Process.Start(string.Format(Properties.Resources.URL_UnrealWikiSearch, GetSelection()));

        }

        private void TextEditor_ContextMenuOpening(object sender, System.Windows.Controls.ContextMenuEventArgs e)
        {
            if (TextEditor.TextArea.Selection.Length == 0)
            {
                SearchWiki.Visibility = System.Windows.Visibility.Collapsed;
                SearchInDocument.Visibility = System.Windows.Visibility.Collapsed;
                SearchObject.Visibility = System.Windows.Visibility.Collapsed;
                return;
            }

            string selection = GetSelection();
            if (selection.IndexOf('\n') != -1)
            {
                SearchWiki.Visibility = System.Windows.Visibility.Collapsed;
                return;
            }

            SearchInDocument.Visibility = System.Windows.Visibility.Visible;
            SearchObject.Visibility = System.Windows.Visibility.Visible;
            SearchWiki.Visibility = System.Windows.Visibility.Visible;
            SearchWiki.Header = string.Format(Properties.Resources.SEARCH_WIKI_ITEM, selection);
        }
    }
}