using System;
using System.Windows.Forms;

namespace UEExplorer.UI.Dialogs
{
	public partial class StructureInputDialog : Form
	{
		public StructureInputDialog()
		{
			InitializeComponent();
		}

		private void StructureInputDialog_Shown( object sender, EventArgs e )
		{
			TextBoxName.Focus();
		}
	}
}
