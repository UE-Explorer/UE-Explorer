using System;
using System.IO;
using System.Windows.Forms;

namespace UEExplorer.UI.Tabs
{
    using Dialogs;

    public class UC_UClassFile : UserControl_Tab
    {
        public string FilePath { get; set; }

        private System.Windows.Forms.Integration.ElementHost _WpfHost;
        private TextEditorPanel _MyTextEditor1;
        private TableLayoutPanel _EditorLayout;

        /*public class UCSyntax : ICSharpCode.AvalonEdit.Highlighting.IHighlightingDefinition
        {
        }*/

        /// <summary>
        /// Called when the Tab is added to the chain.
        /// </summary>
        protected override void TabCreated()
        {
            string langPath = Path.Combine(Application.StartupPath, "Config", "UnrealScript.xshd");
            if (File.Exists(langPath))
            {
                try
                {
                    _MyTextEditor1.textEditor.SyntaxHighlighting =
                        ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load(
                            new System.Xml.XmlTextReader(langPath),
                            ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance
                        );
                }
                catch (Exception e)
                {
                    ExceptionDialog.Show(e.GetType().Name, e);
                }
            }

            base.TabCreated();
        }

        public void PostInitialize()
        {
            _MyTextEditor1.textEditor.IsReadOnly = false;
            _MyTextEditor1.textEditor.Load(FilePath);
        }

        protected override void InitializeComponent()
        {
            _WpfHost = new System.Windows.Forms.Integration.ElementHost();
            _MyTextEditor1 = new TextEditorPanel();
            _EditorLayout = new TableLayoutPanel();
            _EditorLayout.SuspendLayout();
            SuspendLayout();
            // 
            // WPFHost
            // 
            _WpfHost.Dock = DockStyle.Fill;
            _WpfHost.Location = new System.Drawing.Point(3, 20);
            _WpfHost.Name = "_WpfHost";
            _WpfHost.Size = new System.Drawing.Size(1306, 561);
            _WpfHost.TabIndex = 21;
            _WpfHost.Text = "elementHost1";
            _WpfHost.Child = _MyTextEditor1;
            // 
            // EditorLayout
            // 
            _EditorLayout.ColumnCount = 1;
            _EditorLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _EditorLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            _EditorLayout.Controls.Add(_WpfHost, 0, 1);
            _EditorLayout.Dock = DockStyle.Fill;
            _EditorLayout.Location = new System.Drawing.Point(0, 0);
            _EditorLayout.Name = "_EditorLayout";
            _EditorLayout.RowCount = 2;
            _EditorLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 2.910959F));
            _EditorLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 97.08904F));
            _EditorLayout.Size = new System.Drawing.Size(1312, 584);
            _EditorLayout.TabIndex = 23;
            // 
            // UC_UClassFile
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            Controls.Add(_EditorLayout);
            Name = "UC_UClassFile";
            Size = new System.Drawing.Size(1312, 584);
            _EditorLayout.ResumeLayout(false);
            ResumeLayout(false);
        }

        public override void TabSave()
        {
            _MyTextEditor1.textEditor.Save(FilePath);
        }

        public override void TabFind()
        {
            using (var findDialog = new FindDialog(_MyTextEditor1))
            {
                findDialog.ShowDialog();
            }
        }
    }

    public static class EditorUtil
    {
        public static void FindText(TextEditorPanel editor, string text)
        {
            var fails = 0;

            int currentIndex = editor.textEditor.CaretOffset;
            if (currentIndex >= editor.textEditor.Text.Length)
                return;

        searchAgain:
            int textIndex = editor.textEditor.Text.IndexOf(text, currentIndex, StringComparison.OrdinalIgnoreCase);
            if (textIndex == -1)
            {
                currentIndex = 0;
                if (fails > 0)
                    return;

                ++fails;
                goto searchAgain;
            }

            var line = editor.textEditor.TextArea.Document.GetLocation(textIndex);

            editor.textEditor.ScrollTo(line.Line, line.Column);
            editor.textEditor.Select(textIndex, text.Length);

            editor.textEditor.CaretOffset = textIndex + text.Length;
        }
    }
}