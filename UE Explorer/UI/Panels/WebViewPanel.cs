using Microsoft.Web.WebView2.Core;
using System;
using System.Windows.Forms;

namespace UEExplorer.UI.Panels
{
    public partial class WebViewPanel : UserControl
    {
        public WebViewPanel()
        {
            InitializeComponent();
        }

        public string InitialUrl { get; set; }

        private async void WebViewPanel_Load(object sender, EventArgs e)
        {
            string dataFolder = Application.UserAppDataPath;
            var webViewEnvironment = await CoreWebView2Environment.CreateAsync(null, dataFolder);
            await webView2.EnsureCoreWebView2Async(webViewEnvironment);
            webView2.Source = new Uri(InitialUrl, UriKind.Absolute);
        }
    }
}