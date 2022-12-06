using System.Windows.Forms;
using UEExplorer.UI.Panels;

namespace UEExplorer.UI.Tabs
{
    public partial class UC_Default : UserControl_Tab
    {
        public UC_Default()
        {
            SuspendLayout();
            var webViewPanel = new WebViewPanel();
            webViewPanel.Dock = DockStyle.Fill;
            webViewPanel.InitialUrl = webViewPanel.InitialUrl = $"{Program.APPS_URL}?version={Application.ProductVersion}";
            Controls.Add(webViewPanel);
            ResumeLayout();
        }
    }
}