using HtmlTags;

namespace WebStyleWPF.Components;
public static partial class Cpt
{
    public static HtmlTag Button()
    {
        return new HtmlTag("button").Text("button uno");
    }
}
