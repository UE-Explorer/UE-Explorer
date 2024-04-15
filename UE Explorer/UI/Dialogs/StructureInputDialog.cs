using System;
using System.Windows.Forms;

namespace UEExplorer.UI.Dialogs
{
	public partial class StructureInputDialog : Form
    {
        public string InputStructType
        {
            get
            {
                return typeComboBox.SelectedText;
            }
        }

        public string InputStructName
        {
            get
            {
                return nameComboBox.SelectedText;
            }
        }
        
		public StructureInputDialog()
		{
			InitializeComponent();
		}

		private void StructureInputDialog_Shown( object sender, EventArgs e )
		{
			typeComboBox.Focus();
		}
	}
}
