using System.Windows;

namespace UEExplorer.Framework.UI.Editor
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

        private void CopyMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            TextEditor.Copy();
        }
    }
}
