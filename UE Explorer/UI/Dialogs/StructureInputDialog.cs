using System;
using System.Windows.Forms;

namespace UEExplorer.UI.Dialogs
{
    public partial class StructureInputDialog : Form
    {
        public StructureInputDialog() => InitializeComponent();

        public string InputStructType => typeComboBox.SelectedItem?.ToString();

        public string InputStructName => nameComboBox.SelectedItem?.ToString();

        private void StructureInputDialog_Shown(object sender, EventArgs e) => typeComboBox.Focus();
    }
}
