using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Web.WebView2.Core;
using System;
using System.Windows;

namespace WebView2WpfApp
{
    public partial class MainWindow : Window
    {
        private readonly HtmlGenerator _htmlGenerator;
        public MainWindow()
        {
            InitializeComponent();
            InitializeAsync();
            _htmlGenerator = new HtmlGenerator();
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
                string data = new WebViewBridge().GetDataFromCSharp();
                webView.CoreWebView2.PostWebMessageAsString(data);
            }
        }

        private void LoadHtmlContent()
        {
            string htmlContent = _htmlGenerator.GenerateHtml();
            webView.NavigateToString(htmlContent);
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadHtmlContent();
        }
    }

    [System.Runtime.InteropServices.ComVisible(true)]
    public class WebViewBridge
    {
        public string GetDataFromCSharp()
        {
            return "This data comes from C#! Current time: " + DateTime.Now.ToString();
        }
    }
}
