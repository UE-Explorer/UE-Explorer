using System;
using System.Windows.Forms;

namespace UEExplorer.UI.Tabs
{
    [System.Runtime.InteropServices.ComVisible( false )]
    public partial class UC_ModExplorer : UserControl_Tab
    {
        public string FileName{ get; set; }

        public UC_ModExplorer()
        {
            InitializeComponent();
        }

        private void ToolStripButton1_Click( object sender, EventArgs e )
        {
            var sfd = new SaveFileDialog
            {
                Title = "Export File", 
                FileName = Label_ObjectName.Text
            };

            if( sfd.ShowDialog() == DialogResult.OK )
            {
                //File.WriteAllText( sfd.FileName, ScriptPage.Document.Text );
            }
        }	
    }
}
