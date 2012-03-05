using System;
using System.Windows.Forms;
using UELib.Core;

namespace UEExplorer.UI.Dialogs
{
	public partial class HexViewDialog : Form
	{
		public HexViewDialog()
		{
			InitializeComponent();
		}

		public HexViewDialog( UObject uObject ) : this()
		{
			userControl_HexView1.SetHexData( uObject );
			try
			{
				toolStripStatusLabel1.Text += ": " + String.Format( "0x{0:8x}", userControl_HexView1.Buffer.Length ).PadLeft( 8, '0' );
				Text = uObject.Package.PackageName + "." + uObject.GetOuterGroup() + " " + Text;
			}
			catch( Exception )
			{
				Text = uObject.Name;
			}
		}

		private void viewASCIIToolStripMenuItem_CheckedChanged( object sender, EventArgs e )
		{
			userControl_HexView1.DrawASCII = !userControl_HexView1.DrawASCII;
		}

		private void viewByteToolStripMenuItem_CheckedChanged( object sender, EventArgs e )
		{
			userControl_HexView1.DrawByte = !userControl_HexView1.DrawByte;
		}
	}
}
