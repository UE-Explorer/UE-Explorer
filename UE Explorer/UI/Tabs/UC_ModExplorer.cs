using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace UEExplorer.UI.Tabs
{
    [ComVisible(false)]
    public partial class UC_ModExplorer : UserControl
    {
        public UC_ModExplorer()
        {
            InitializeComponent();
        }

        public string FileName { get; set; }

        private void ToolStripButton1_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog { Title = "Export File", FileName = Label_ObjectName.Text };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                //File.WriteAllText( sfd.FileName, ScriptPage.Document.Text );
            }
        }
    }
}
