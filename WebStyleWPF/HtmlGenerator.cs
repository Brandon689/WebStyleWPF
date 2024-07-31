using HtmlTags;
using System.IO;
using System.Text;
using WebStyleWPF.Components;

namespace WebView2WpfApp
{
    public class HtmlGenerator
    {
        public string GenerateHtml()
        {
            var document = new HtmlDocument();
            document.Title = "Dynamic HTML Content with Gallery";
            document.Head.Append(GenerateMetaTags());
            document.Body.Append(GenerateHeader());
            document.Body.Append(GenerateContent());

            var htmlString = document.ToString();

            // Insert the script tag after the body
            var scriptTag = GenerateScriptTag();
            var styles = $"<style>{GenerateStyles()}</style>";
            var scriptAndStyles = $"{styles}{scriptTag}</body>";
            htmlString = htmlString.Replace("</body>", scriptAndStyles);

            return htmlString;
        }

        private HtmlTag GenerateMetaTags()
        {
            var metaTags = new HtmlTag("meta").Attr("charset", "UTF-8");
            metaTags.After(new HtmlTag("meta").Attr("name", "viewport").Attr("content", "width=device-width, initial-scale=1.0"));
            return metaTags;
        }

        private string GenerateStyles()
        {
            return File.ReadAllText("Styles/styles.css");
        }

        private HtmlTag GenerateHeader()
        {
            return new HtmlTag("h1").Text("Welcome to WebView2 Gallery");
        }

        private HtmlTag GenerateContent()
        {
            var content = new HtmlTag("div").AddClass("content");
            content.Append(new HtmlTag("p").Text("This content is dynamically generated from C#."));
            content.Append(new HtmlTag("p").Text($"Current time: {DateTime.Now}"));
            content.Append(new HtmlTag("button").Attr("onclick", "getDataFromCSharp()").Text("Get Data from C#"));
            content.Append(new HtmlTag("p").Id("result"));
            var imageUrls = new List<string>
            {
                "https://picsum.photos/id/1018/800/600",
                "https://picsum.photos/id/1015/800/600",
                "https://picsum.photos/id/1019/800/600",
                "https://picsum.photos/id/1016/800/600",
                "https://picsum.photos/id/1020/800/600",
                "https://picsum.photos/id/1021/800/600"
            };
            content.Append(Cpt.Gallery(imageUrls));
            content.Append(Cpt.Button());
            content.Append(Cpt.Image("https://picsum.photos/id/1038/800/600"));

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
