using Microsoft.Web.WebView2.Core;
using System.Windows;
using WebStyleWPF.Bridge;

namespace WebView2WpfApp
{
    public partial class MainWindow : Window
    {
        private readonly HtmlGenerator _htmlGenerator;
        private WebViewBridge _webViewBridge;
        public MainWindow()
        {
            InitializeComponent();
            InitializeAsync();
            _htmlGenerator = new HtmlGenerator();
            _webViewBridge = new WebViewBridge();
        }

        async void InitializeAsync()
        {
            await webView.EnsureCoreWebView2Async(null);
            webView.CoreWebView2.AddHostObjectToScript("bridge", new WebViewBridge());
            webView.CoreWebView2.Settings.IsScriptEnabled = true;
            webView.CoreWebView2.Settings.AreDefaultScriptDialogsEnabled = true;
            webView.CoreWebView2.Settings.IsWebMessageEnabled = true;

            // Add a handler for web message received
            webView.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;

            LoadHtmlContent();
        }

        private void CoreWebView2_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            if (e.TryGetWebMessageAsString() == "GetDataFromCSharp")
            {
                string data = _webViewBridge.GetDataFromCSharp();
                webView.CoreWebView2.PostWebMessageAsString(data);
            }
        }

        private void LoadHtmlContent()
        {
            string htmlContent = _htmlGenerator.GenerateHtml();
            //Console.WriteLine(htmlContent);
            webView.NavigateToString(htmlContent);
        }


        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadHtmlContent();
        }
    }
}
