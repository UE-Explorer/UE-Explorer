using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace UEExplorer.UI.Dialogs
{
	public partial class SelectDialog : Form
	{
		public int Result = new int();

		public SelectDialog( IEnumerable<string> comboBoxText, string title )
		{
			InitializeComponent();

			Text = title;

			foreach( var s in comboBoxText )
			{
				comboBox1.Items.Add( s );
			}
		}

		private void button1_Click( object sender, EventArgs e )
		{
			Result = comboBox1.SelectedIndex;
			DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}
