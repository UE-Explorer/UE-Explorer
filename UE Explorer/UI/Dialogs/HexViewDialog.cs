using System;
using System.IO;
using System.Windows.Forms;
using UEExplorer.Properties;
using UEExplorer.UI.Tabs;
using UELib;
using UELib.Core;

namespace UEExplorer.UI.Dialogs
{
	public partial class HexViewDialog : Form
	{
		private readonly UC_PackageExplorer _Owner;

		public HexViewDialog()
		{
			InitializeComponent();
		}

		public HexViewDialog( UObject uObject, UC_PackageExplorer owner ) : this()
		{
			if( uObject is UPackageObject )
			{
				editToolStripMenuItem.Enabled = false;
			}

			_Owner = owner;
			userControl_HexView1.SetHexData( uObject );
			string subTitle;
			try
			{
				toolStripStatusLabel1.Text += String.Format( ": {0}", userControl_HexView1.Buffer.Length ).PadLeft( 8, '0' );
				subTitle = uObject.Package.PackageName + "." + uObject.GetOuterGroup();
			}
			catch( Exception )
			{
				subTitle = uObject.Name;
			}
			Text = String.Format( "{0} - {1}", Text, subTitle );
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

		private void exportBinaryFileToolStripMenuItem_Click( object sender, EventArgs e )
		{
			var fsd = new SaveFileDialog()
			{
				FileName = userControl_HexView1.Target.GetOuterGroup() 
					+ "." + userControl_HexView1.Target.GetClassName()
			};

			if( fsd.ShowDialog() == DialogResult.OK )
			{
				File.WriteAllBytes( fsd.FileName, userControl_HexView1.Buffer );
			}
		}

		private void importBinaryFileToolStripMenuItem_Click( object sender, EventArgs e )
		{
			var hexObject = userControl_HexView1.Target;

			var osd = new OpenFileDialog()
			{
				FileName = hexObject.GetOuterGroup() 
					+ "." + hexObject.GetClassName()	
			};

			if( osd.ShowDialog() == DialogResult.OK )
			{
				var buffer = File.ReadAllBytes( osd.FileName );		
				if( buffer.Length != userControl_HexView1.Buffer.Length )
				{
					MessageBox.Show( Resources.CANNOT_IMPORT_BINARY_NOTEQUAL_LENGTH );		
					return;
				}

				userControl_HexView1.Buffer = buffer;
				userControl_HexView1.Refresh();

				var result = MessageBox.Show(
					Resources.SAVE_QUESTION_WARNING, 
					Resources.SAVE_QUESTION, 
					MessageBoxButtons.YesNo 
				);
				if( result == DialogResult.Yes )
				{
					hexObject.Package.Stream.Close();
					hexObject.Package.Stream.Dispose();

					using(  var package = UnrealPackage.DeserializePackage
							( 
								hexObject.Package.FullPackageName, 
								FileAccess.ReadWrite 
							)
						)
					{ 
						package.Stream.Seek( hexObject.ExportTable.SerialOffset, SeekOrigin.Begin );
						try
						{ 
							package.Stream.Write( buffer, 0, buffer.Length );
							package.Stream.Flush();

							Close();
							_Owner.ReloadPackage();
						}
						catch( IOException exc )
						{
							MessageBox.Show( string.Format( Resources.COULDNT_SAVE_EXCEPTION, exc ) );
						}	
					}
				}
			}
		}

		private void infoToolStripMenuItem_Click( object sender, EventArgs e )
		{
			MessageBox.Show(
				"A red underline indicates the start position of the script related bytes.\r\n" +
				"A orange underline indicates the end of the script related bytes(not necessary accurate!)"		
			);
		}
	}
}
