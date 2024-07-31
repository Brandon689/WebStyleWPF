using HtmlTags;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebView2WpfApp
{
    public class HtmlGenerator
    {
        public string GenerateHtml(List<string> imageUrls)
        {
            var document = new HtmlDocument();
            document.Title = "Dynamic HTML Content with Gallery";

            document.Head.Append(GenerateMetaTags());


            document.Body.Append(GenerateHeader());
            document.Body.Append(GenerateContent(imageUrls));

            var htmlString = document.ToString();

            // Insert the script tag after the body
            var scriptTag = GenerateScriptTag();
            var styles = $"<style>{GenerateStyles()}</style>";
            htmlString = htmlString.Replace("</body>", $"{scriptTag}</body>");
            htmlString = htmlString.Replace("</body>", $"{styles}</body>");

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
            return @"
                body { 
                    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; 
                    margin: 0; 
                    padding: 20px; 
                    background-color: #f0f0f0;
                    color: #333;
                }
                h1 { 
                    color: #2c3e50; 
                    text-align: center;
                    margin-bottom: 30px;
                }
                .content { 
                    background-color: #fff; 
                    padding: 20px; 
                    border-radius: 8px;
                    box-shadow: 0 2px 4px rgba(0,0,0,0.1);
                    max-width: 800px;
                    margin: 0 auto;
                }
                button {
                    background-color: #3498db;
                    color: white;
                    border: none;
                    padding: 10px 20px;
                    border-radius: 5px;
                    cursor: pointer;
                    transition: background-color 0.3s;
                }
                button:hover {
                    background-color: #2980b9;
                }
                #result {
                    margin-top: 20px;
                    font-weight: bold;
                }
                .gallery {
                    display: grid;
                    grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
                    gap: 15px;
                    margin-top: 20px;
                }
                .gallery img {
                    width: 100%;
                    height: 200px;
                    object-fit: cover;
                    border-radius: 5px;
                    transition: transform 0.3s;
                }
                .gallery img:hover {
                    transform: scale(1.05);
                }
            ";
        }

        private HtmlTag GenerateHeader()
        {
            return new HtmlTag("h1").Text("Welcome to WebView2 Gallery");
        }

        private HtmlTag GenerateContent(List<string> imageUrls)
        {
            var content = new HtmlTag("div").AddClass("content");
            content.Append(new HtmlTag("p").Text("This content is dynamically generated from C#."));
            content.Append(new HtmlTag("p").Text($"Current time: {DateTime.Now}"));
            content.Append(new HtmlTag("button").Attr("onclick", "getDataFromCSharp()").Text("Get Data from C#"));
            content.Append(new HtmlTag("p").Id("result"));

            var gallery = new HtmlTag("div").AddClass("gallery");
            foreach (var url in imageUrls)
            {
                gallery.Append(new HtmlTag("img").Attr("src", url).Attr("alt", "Gallery Image"));
            }
            content.Append(gallery);

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
