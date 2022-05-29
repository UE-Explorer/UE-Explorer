using System;
using System.Windows.Forms;

namespace UEExplorer.UI.Dialogs
{
    using Tabs;

    public partial class FindDialog : Form
    {
        private readonly TextEditorPanel _Editor;

        public FindDialog()
        {
            InitializeComponent();
        }

        public FindDialog(TextEditorPanel editor)
        {
            _Editor = editor;

            InitializeComponent();
        }

        private void Find_Click(object sender, EventArgs e)
        {
            if (_Editor == null)
                return;

            EditorUtil.FindText(_Editor, FindInput.Text);
        }

        private void FindDialog_Shown(object sender, EventArgs e)
        {
            FindInput.Focus();
        }
    }
}