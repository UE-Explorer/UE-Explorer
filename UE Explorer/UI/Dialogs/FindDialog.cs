using System;
using System.Windows.Forms;

namespace UEExplorer.UI.Dialogs
{
    public class FindEventArgs : EventArgs
    {
        public string FindText;
    }

    public partial class FindDialog : Form
    {
        public event EventHandler<FindEventArgs> FindNext;

        public FindDialog()
        {
            InitializeComponent();
        }

        private void Find_Click(object sender, EventArgs e)
        {
            FindNext?.Invoke(this, new FindEventArgs { FindText = FindInput.Text });
        }

        private void FindDialog_Shown(object sender, EventArgs e)
        {
            FindInput.Focus();
        }
    }
}