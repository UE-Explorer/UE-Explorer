using System.IO;
using System.Windows.Forms;

namespace UEExplorer.UI.Panels
{
    public partial class TextEditorPanel : Panel
    {
        public TextEditorPanel()
        {
            InitializeComponent();
            InitializeTextEditorControl();
        }

        private void InitializeTextEditorControl()
        {
            string syntaxXSHDFilePath = Path.Combine(Application.StartupPath, "Config", "UnrealScript.xshd");
            if (File.Exists(syntaxXSHDFilePath))
            {
                TextEditorControl.TextEditor.SyntaxHighlighting =
                    ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load(
                        new System.Xml.XmlTextReader(syntaxXSHDFilePath),
                        ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance
                    );
            }

            // Fold all { } blocks
            //var foldingManager = ICSharpCode.AvalonEdit.Folding.FoldingManager.Install(myTextEditor1.textEditor.TextArea);
            //var foldingStrategy = new ICSharpCode.AvalonEdit.Folding.XmlFoldingStrategy();
            //foldingStrategy.UpdateFoldings(foldingManager, myTextEditor1.textEditor.Document);
        }

        public void SetText(string text, bool resetView = true)
        {
            TextEditorControl.TextEditor.Text = text;
            if (resetView) TextEditorControl.TextEditor.ScrollToHome();
        }
    }
}