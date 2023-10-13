using System;
using System.Windows.Forms;

namespace UEExplorer.UI.Panels
{
    public partial class WebViewPanel : UserControl
    {
        private readonly string _InitialUrl;

        public WebViewPanel(string initialUrl)
        {
            SetStyle(ControlStyles.ContainerControl, true);

            _InitialUrl = initialUrl;

            InitializeComponent();
        }

        private void WebViewPanel_Load(object sender, EventArgs e)
        {
            webView2.Source = new Uri(_InitialUrl, UriKind.Absolute);
        }
    }
}
