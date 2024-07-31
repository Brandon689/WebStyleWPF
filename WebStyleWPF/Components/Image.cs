using HtmlTags;

namespace WebStyleWPF.Components;
public static partial class Cpt
{
    public static HtmlTag Image(string src)
    {
        return new HtmlTag("img").Attr("src", src);
    }
}
