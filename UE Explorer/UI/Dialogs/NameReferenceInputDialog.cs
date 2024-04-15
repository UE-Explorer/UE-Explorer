using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows.Forms;
using UELib;
using UELib.Core;

namespace UEExplorer.UI.Dialogs
{
    public partial class NameReferenceInputDialog : Form
    {
        public UName DefaultNameReference;
        public UnrealPackage Linker;

        public NameReferenceInputDialog() => InitializeComponent();

        public UNameTableItem InputNameItem => (UNameTableItem)inputComboBox.SelectedItem;

        public int InputNameNumber => (int)numberNumericUpDown.Value;

        private void NameReferenceInputDialog_Load(object sender, EventArgs e)
        {
            Contract.Assert(Linker != null);

            object[] items = Linker.Names.ToArray<object>();
            inputComboBox.Items.AddRange(items);
            inputComboBox.SelectedIndex = (int)DefaultNameReference;

            numberNumericUpDown.Value = DefaultNameReference.Number + 1;
        }

        private void NameReferenceInputDialog_Shown(object sender, EventArgs e) => inputComboBox.Focus();
    }
}
