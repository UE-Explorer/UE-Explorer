using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows.Forms;
using UELib;
using UELib.Core;

namespace UEExplorer.UI.Dialogs
{
    public partial class ObjectReferenceInputDialog : Form
    {
        public UObject DefaultObjectReference;
        public UnrealPackage Linker;

        public ObjectReferenceInputDialog() => InitializeComponent();

        public UObject InputObjectReference => (UObject)inputComboBox.SelectedItem;

        private void ObjectReferenceInputDialog_Load(object sender, EventArgs e)
        {
            Contract.Assert(Linker != null);

            object[] items = Linker.Objects.ToArray<object>();
            inputComboBox.Items.AddRange(items);
            inputComboBox.SelectedItem = DefaultObjectReference;
        }

        private void ObjectReferenceInputDialog_Shown(object sender, EventArgs e) => inputComboBox.Focus();

        private void inputComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            var obj = (UObject)inputComboBox.Items[e.Index];
            TextRenderer.DrawText(e.Graphics, obj.GetReferencePath(), inputComboBox.Font, e.Bounds, inputComboBox.ForeColor);
        }
    }
}
