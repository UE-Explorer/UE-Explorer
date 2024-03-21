using System;
using System.Windows.Forms;
using UEExplorer.UI.Panels;

namespace UEExplorer.UI.Tabs
{
    public partial class UC_Default : UserControl_Tab
    {
        public UC_Default()
        {
            InitializeComponent();
        }
        
        private void UC_Default_Load(object sender, EventArgs e)
        {
            BeginInvoke((MethodInvoker)(() =>
            {
                SuspendLayout();
                var webViewPanel = new WebViewPanel();
                webViewPanel.Dock = DockStyle.Fill;
                webViewPanel.InitialUrl = webViewPanel.InitialUrl = $"{Program.APPS_URL}?version={Application.ProductVersion}";
                Controls.Add(webViewPanel);
                ResumeLayout();
            }));
        }
    }
}