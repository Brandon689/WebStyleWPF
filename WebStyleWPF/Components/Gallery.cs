using HtmlTags;

namespace WebStyleWPF.Components;
public static partial class Cpt
{
    public static HtmlTag Gallery(List<string> imageUrls)
    {
        var gallery = new HtmlTag("div").AddClass("gallery");
        foreach (var url in imageUrls)
        {
            gallery.Append(new HtmlTag("img").Attr("src", url).Attr("alt", "Gallery Image"));
        }
        return gallery;
    }
}
