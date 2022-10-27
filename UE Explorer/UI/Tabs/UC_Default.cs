using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace UEExplorer.UI.Tabs
{
    [ComVisible(false)]
    public partial class UC_Default : UserControl_Tab
    {
        public UC_Default()
        {
            InitializeComponent();
        }
        
        private void UC_Default_Load(object sender, EventArgs e)
        {
            DefaultPage.BeginInvoke((Action)(() =>
                DefaultPage.Navigate(Program.APPS_URL + "?version=" + Application.ProductVersion)));
        }
    }
}