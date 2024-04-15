using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows.Forms;
using UELib;

namespace UEExplorer.UI.Dialogs
{
    public partial class ObjectReferenceInputDialog : Form
    {
        public UnrealPackage Linker;

        public ObjectReferenceInputDialog()
        {
            InitializeComponent();
        }

        public object DefaultObjectReference;

        public object InputObjectReference
        {
            get { return inputComboBox.SelectedItem; }
            set { inputComboBox.SelectedItem = value; }
        }

        private void ObjectReferenceInputDialog_Load(object sender, EventArgs e)
        {
            Contract.Assert(Linker != null);

            object[] items = Linker.Objects.ToArray<object>();
            inputComboBox.Items.AddRange(items);
            inputComboBox.SelectedItem = DefaultObjectReference;
        }

        private void ObjectReferenceInputDialog_Shown(object sender, EventArgs e)
        {
            inputComboBox.Focus();
        }
    }
}
