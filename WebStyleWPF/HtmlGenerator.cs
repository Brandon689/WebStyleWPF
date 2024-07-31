using HtmlTags;
using System;
using System.Text;

namespace WebView2WpfApp
{
    public class HtmlGenerator
    {
        public string GenerateHtml()
        {
            var document = new HtmlDocument();
            document.Title = "Dynamic HTML Content";

            document.Head.Append(GenerateMetaTags());
            document.Head.Append(GenerateStyles());

            document.Body.Append(GenerateHeader());
            document.Body.Append(GenerateContent());

            var htmlString = document.ToString();

            // Insert the script tag after the body
            var scriptTag = GenerateScriptTag();
            htmlString = htmlString.Replace("</body>", $"{scriptTag}</body>");

            return htmlString;
        }

        private HtmlTag GenerateMetaTags()
        {
            var metaTags = new HtmlTag("meta").Attr("charset", "UTF-8");
            metaTags.After(new HtmlTag("meta").Attr("name", "viewport").Attr("content", "width=device-width, initial-scale=1.0"));
            return metaTags;
        }

        private HtmlTag GenerateStyles()
        {
            return new HtmlTag("style").Text(@"
                body { font-family: Arial, sans-serif; margin: 0; padding: 20px; }
                h1 { color: #333; }
                .content { background-color: #f0f0f0; padding: 15px; border-radius: 5px; }
            ");
        }

        private HtmlTag GenerateHeader()
        {
            return new HtmlTag("h1").Text("Welcome to WebView2");
        }

        private HtmlTag GenerateContent()
        {
            var content = new HtmlTag("div").AddClass("content");
            content.Append(new HtmlTag("p").Text("This content is dynamically generated from C#."));
            content.Append(new HtmlTag("p").Text($"Current time: {DateTime.Now}"));
            content.Append(new HtmlTag("button").Attr("onclick", "getDataFromCSharp()").Text("Get Data from C#"));
            content.Append(new HtmlTag("p").Id("result"));
            return content;
        }

        private string GenerateScriptTag()
        {
            var sb = new StringBuilder();
            sb.AppendLine("<script>");
            sb.AppendLine("    function getDataFromCSharp() {");
            sb.AppendLine("        chrome.webview.postMessage('GetDataFromCSharp');");
            sb.AppendLine("    }");
            sb.AppendLine();
            sb.AppendLine("    chrome.webview.addEventListener('message', event => {");
            sb.AppendLine("        document.getElementById('result').textContent = event.data;");
            sb.AppendLine("    });");
            sb.AppendLine("</script>");
            return sb.ToString();
        }
    }
}
