using System.Windows.Forms;
using UEExplorer.Framework.UI.Pages;
using UEExplorer.Properties;
using UEExplorer.UI.Panels;

namespace UEExplorer.UI.Pages
{
    public sealed class StartPage : Page
    {
        private WebViewPanel _WebViewPanel;
        
        public StartPage()
        {
            Text = Resources.Homepage;
            TextTitle = Resources.Homepage;

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            _WebViewPanel = new WebViewPanel($"{Program.StartUrl}?version={Application.ProductVersion}");
            _WebViewPanel.Dock = DockStyle.Fill;
            Controls.Add(_WebViewPanel);
        }
    }
}
