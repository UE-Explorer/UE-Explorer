using System;
using System.Windows.Forms;

namespace UEExplorer.UI.Dialogs
{
    using Panels;

    public partial class FindDialog : Form
    {
        private readonly TextEditorControl _TextEditorControl;

        public FindDialog()
        {
            InitializeComponent();
        }

        public FindDialog(TextEditorControl textEditorControl)
        {
            _TextEditorControl = textEditorControl;

            InitializeComponent();
        }

        private void Find_Click(object sender, EventArgs e)
        {
            if (_TextEditorControl == null)
                return;

            EditorUtil.FindText(_TextEditorControl.TextEditor, FindInput.Text);
        }

        private void FindDialog_Shown(object sender, EventArgs e)
        {
            FindInput.Focus();
        }
    }
}