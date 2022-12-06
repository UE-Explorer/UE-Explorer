using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

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

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            TextEditor.Copy();
        }

        private void SearchWiki_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(string.Format(Properties.Resources.URL_UnrealWikiSearch, GetSelection()));
        }

        private void TextEditor_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (TextEditor.TextArea.Selection.Length == 0)
            {
                SearchWiki.Visibility = Visibility.Collapsed;
                SearchInDocument.Visibility = Visibility.Collapsed;
                SearchObject.Visibility = Visibility.Collapsed;
                return;
            }

            string selection = GetSelection();
            if (selection.IndexOf('\n') != -1)
            {
                SearchWiki.Visibility = Visibility.Collapsed;
                return;
            }

            SearchInDocument.Visibility = Visibility.Visible;
            SearchObject.Visibility = Visibility.Visible;
            SearchWiki.Visibility = Visibility.Visible;
            SearchWiki.Header = string.Format(Properties.Resources.SEARCH_WIKI_ITEM, selection);
        }
    }
}