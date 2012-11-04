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

		private void copyToolStripMenuItem_Click( object sender, EventArgs e )
		{
			Clipboard.SetText( BitConverter.ToString( userControl_HexView1.Buffer ) );
		}

		private void copyAsViewToolStripMenuItem_Click( object sender, EventArgs e )
		{
			const int		firstColumnWidth		= 8;
			const int		secondColumnWidth		= 32;
			const int		thirdColumnWidth		= 16;
			const string	columnMargin			= "  ";
			const string	byteValueWidth			= "  ";
			const char		charValueWidth			= ' ';
			const char		valueMargin				= ' ';
			const int		columnWidth				= 16;
			
			var buffer = userControl_HexView1.Buffer;
			var input = "Offset".PadRight( firstColumnWidth, valueMargin ) + columnMargin
				+ "0  1  2  3  4  5  6  7  8  9  A  B  C  D  E  F ".PadRight( secondColumnWidth, valueMargin ) + columnMargin
				+ "0 1 2 3 4 5 6 7 8 9 A B C D E F ".PadRight( thirdColumnWidth, valueMargin )
				+ "\r\n";

			var lines = (int)Math.Ceiling( (double)buffer.Length/columnWidth );
			for( int i = 0; i < lines; ++ i )
			{
				input += String.Format( "\r\n{0:x8}", i*columnWidth ).ToUpper() + columnMargin;
				for( int j = 0; j < columnWidth; ++ j )
				{
					var index =	i*columnWidth + j;
					if( index >= buffer.Length )
					{
						input += byteValueWidth;
					}
					else
					{ 
						input += String.Format( "{0:x2}", buffer[index] ).ToUpper();
					}

					if( j < columnWidth - 1 )
					{
						input += valueMargin;
					}
				}

				input += columnMargin;

				for( int j = 0; j < columnWidth; ++ j )
				{
					var index =	i*columnWidth + j;
					if( index >= buffer.Length )
					{
						input += charValueWidth;
					}
					else
					{
						input += UserControl_HexView.FilterByte( buffer[index] ).ToString();	
					}
	
					if( j < columnWidth - 1 )
					{
						input += valueMargin;
					}
				}
			}

			Clipboard.SetText( input );
		}
	}
}
