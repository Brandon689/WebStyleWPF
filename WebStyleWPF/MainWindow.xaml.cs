using Microsoft.Web.WebView2.Core;
using System;
using System.Windows;

namespace WebView2WpfApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeAsync();
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
            string htmlContent = GenerateHtmlContent();
            webView.NavigateToString(htmlContent);
        }

        private string GenerateHtmlContent()
        {
            return @"
            <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Dynamic HTML Content</title>
                <style>
                    body { font-family: Arial, sans-serif; margin: 0; padding: 20px; }
                    h1 { color: #333; }
                    .content { background-color: #f0f0f0; padding: 15px; border-radius: 5px; }
                </style>
            </head>
            <body>
                <h1>Welcome to WebView2</h1>
                <div class='content'>
                    <p>This content is dynamically generated from C#.</p>
                    <p>Current time: " + DateTime.Now.ToString() + @"</p>
                    <button onclick='getDataFromCSharp()'>Get Data from C#</button>
                    <p id='result'></p>
                </div>
                <script>
                    function getDataFromCSharp() {
                        chrome.webview.postMessage('GetDataFromCSharp');
                    }

                    chrome.webview.addEventListener('message', event => {
                        document.getElementById('result').textContent = event.data;
                    });
                </script>
            </body>
            </html>";
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
