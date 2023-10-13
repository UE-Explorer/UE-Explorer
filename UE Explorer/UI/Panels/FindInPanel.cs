using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using UEExplorer.Framework;
using UELib.Core;

namespace UEExplorer.UI.Panels
{
    public partial class FindInPanel : UserControl
    {
        public FindInPanel()
        {
            InitializeComponent();
        }

        public event EventHandler<FindInEventArgs> Find;

        private void FindInPanel_Load(object sender, EventArgs e)
        {
            lookInComboBox.Enabled = lookInCheckBox.Checked;

            findTextBox.Focus();

            var packageManager = ServiceHost.GetRequired<PackageManager>();
            var packageReferences = packageManager.Packages;
            packageReferenceBindingSource.DataSource = packageReferences;
        }

        private void findAllCommand_Execute(object sender, EventArgs e)
        {
            var findInEvent = new FindInEventArgs
            {
                SearchText = findTextBox.Text,
                IsCaseSensitive = matchCaseCheckBox.Checked,
                IsRegEx = false,
                PackageReference = lookInCheckBox.Checked
                    ? (PackageReference)lookInComboBox.SelectedItem
                    : null,
                Types = null
            };

            Debug.Assert(Find != null, nameof(Find) + " != null");
            Find.Invoke(this, findInEvent);
        }

        private void findTextBox_TextChanged(object sender, EventArgs e)
        {
            bool canFind = findTextBox.Text.Trim() != string.Empty;
            findAllButton.Enabled = canFind;
        }

        private void findTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                findAllButton.PerformClick();
            }
        }

        private void lookInCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            lookInComboBox.Enabled = lookInCheckBox.Checked;
        }
    }

    public class FindInEventArgs : EventArgs
    {
        public string SearchText;
        public bool IsCaseSensitive;
        public bool IsRegEx;
        public PackageReference PackageReference;
        public List<UObject> Types;
    }
}
