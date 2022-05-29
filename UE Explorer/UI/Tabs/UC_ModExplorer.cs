using System;
using System.Windows.Forms;

namespace UEExplorer.UI.Tabs
{
    [System.Runtime.InteropServices.ComVisible(false)]
    public partial class UC_ModExplorer : UserControl_Tab
    {
        public string FileName { get; set; }

        protected override void TabCreated()
        {
            /*UPackageStream s = new UPackageStream( FileName, FileMode.Open, FileAccess.Read );
            byte[] buffer = new byte[(int)s.Length];
            s.Read( buffer, 0, (int)s.Length );
            buffer.Reverse();
            UObjectStream stream = new UObjectStream( s, buffer );

            UnrealMod mod = new UnrealMod();
            mod.Serialize( stream );
            foreach( UnrealMod.FileTable ft in mod.FileTableList )
            {
                listView1.Items.Add( ft.FileName );
            }*/
            base.TabCreated();
        }

        private void ToolStripButton1_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog
            {
                Title = "Export File",
                FileName = Label_ObjectName.Text
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                //File.WriteAllText( sfd.FileName, ScriptPage.Document.Text );
            }
        }
    }
}