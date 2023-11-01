using Microsoft.Web.WebView2.Core;
using System;
using System.Windows.Forms;

namespace UEExplorer.UI.Panels
{
    public partial class WebViewPanel : UserControl
    {
        public WebViewPanel()
        {
            SetStyle(ControlStyles.ContainerControl, true);

            InitializeComponent();
        }

        private CoreWebView2Environment _Environment;

        public async void NavigateTo(string url)
        {
            if (_Environment == null)
            {
                string dataFolder = Application.UserAppDataPath;
                _Environment = await CoreWebView2Environment.CreateAsync(null, dataFolder).ConfigureAwait(false);
            }

            webView2.Source = new Uri(url, UriKind.Absolute);
        }
    }
}
