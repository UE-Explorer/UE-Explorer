using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using UEExplorer.Properties;
using UEExplorer.UI.Tabs;
using UELib;

namespace UEExplorer.UI.Dialogs
{
    public partial class HexViewDialog : Form
    {
        private readonly UC_PackageExplorer _Owner;

        public HexViewDialog()
        {
            InitializeComponent();
            InitializeUserSettings();     
        }

        private void OnHexPanelOffsetChanged( int selectedOffset )
        {
            ToolStripStatusLabel_Position.Text = String.Format
            (
                Resources.HexView_Position, 
                HexPanel.Target.GetBufferPosition(), 
                selectedOffset, 
                HexPanel.Target.GetBufferPosition() 
            );
        }

        private void InitializeUserSettings()
        {
            WindowState = Settings.Default.HexViewerState;
            Size = Settings.Default.HexViewerSize;
            Location = Settings.Default.HexViewerLocation;
            ViewASCIIItem.Checked = Settings.Default.HexViewer_ViewASCII;
            ViewByteItem.Checked = Settings.Default.HexViewer_ViewByte;
        }

        public HexViewDialog( IBuffered target, UC_PackageExplorer owner ) : this()
        {
            if( target == null )
            {
                throw new NullReferenceException( "No target for HexViewDialog()" );
            }

            if( target is UnrealPackage )
            {
                editToolStripMenuItem.Enabled = false;
            }

            _Owner = owner;
            HexPanel.SetHexData( target );
            Text = String.Format( "{0} - {1}", Text, target.GetBufferId( true ) );

            SizeLabel.Text = String.Format( SizeLabel.Text, 
                target.GetBufferSize().ToString( CultureInfo.InvariantCulture )
            );

            OnHexPanelOffsetChanged( 0 );
            HexPanel.OffsetChangedEvent += OnHexPanelOffsetChanged;
        }

        private void ViewASCIIToolStripMenuItem_CheckedChanged( object sender, EventArgs e )
        {
            HexPanel.DrawASCII = !HexPanel.DrawASCII;
            Settings.Default.HexViewer_ViewASCII = HexPanel.DrawASCII;
            Settings.Default.Save();
        }

        private void ViewByteToolStripMenuItem_CheckedChanged( object sender, EventArgs e )
        {
            HexPanel.DrawByte = !HexPanel.DrawByte;
            Settings.Default.HexViewer_ViewByte = HexPanel.DrawByte;
            Settings.Default.Save();
        }

        private void CopyToolStripMenuItem_Click( object sender, EventArgs e )
        {
            Clipboard.SetText( BitConverter.ToString( HexPanel.Buffer ) );
        }

        private void CopyAsViewToolStripMenuItem_Click( object sender, EventArgs e )
        {
            const int		firstColumnWidth		= 8;
            const int		secondColumnWidth		= 32;
            const int		thirdColumnWidth		= 16;
            const string	columnMargin			= "  ";
            const string	byteValueWidth			= "  ";
            const char		charValueWidth			= ' ';
            const char		valueMargin				= ' ';
            const int		columnWidth				= 16;
            
            var buffer = HexPanel.Buffer;
            var input = Resources.HexView_Offset.PadRight( firstColumnWidth, valueMargin ) + columnMargin
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
                        input += UserControl_HexView.FilterByte( buffer[index] ).ToString( CultureInfo.InvariantCulture );	
                    }
    
                    if( j < columnWidth - 1 )
                    {
                        input += valueMargin;
                    }
                }
            }

            Clipboard.SetText( input );
        }

        private void ExportBinaryFileToolStripMenuItem_Click( object sender, EventArgs e )
        {
            var fsd = new SaveFileDialog{ FileName = HexPanel.Target.GetBufferId() };
            if( fsd.ShowDialog() == DialogResult.OK )
            {
                File.WriteAllBytes( fsd.FileName, HexPanel.Buffer );
            }
        }

        private void ImportBinaryFileToolStripMenuItem_Click( object sender, EventArgs e )
        {
            var target = HexPanel.Target;
            var osd = new OpenFileDialog{ FileName = target.GetBufferId() };
            if( osd.ShowDialog() == DialogResult.OK )
            {
                var buffer = File.ReadAllBytes( osd.FileName );		
                if( buffer.Length != HexPanel.Buffer.Length )
                {
                    MessageBox.Show( Resources.CANNOT_IMPORT_BINARY_NOTEQUAL_LENGTH );		
                    return;
                }

                HexPanel.Buffer = buffer;
                HexPanel.Refresh();

                var result = MessageBox.Show(
                    Resources.SAVE_QUESTION_WARNING, 
                    Resources.SAVE_QUESTION, 
                    MessageBoxButtons.YesNo 
                );
                if( result == DialogResult.Yes )
                {
                    target.GetBuffer().Dispose();
                    using( var package = UnrealPackage.DeserializePackage( target.GetBuffer().Name, FileAccess.ReadWrite ) )
                    { 
                        package.Stream.Seek( target.GetBufferPosition(), SeekOrigin.Begin );
                        try
                        { 
                            package.Stream.Write( buffer, 0, buffer.Length );
                            package.Stream.Flush();
                            package.Stream.Dispose();

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

        private void HexViewDialog_FormClosing( object sender, FormClosingEventArgs e )
        {
            if( WindowState != FormWindowState.Normal )
            {
                Settings.Default.HexViewerLocation = RestoreBounds.Location;
                Settings.Default.HexViewerSize = RestoreBounds.Size;
            }
            else
            {
                Settings.Default.HexViewerLocation = Location;
                Settings.Default.HexViewerSize = ClientSize;	
            }

            Settings.Default.HexViewerState = WindowState == FormWindowState.Minimized ? FormWindowState.Normal : WindowState;
            Settings.Default.Save();
        }
    }
}
