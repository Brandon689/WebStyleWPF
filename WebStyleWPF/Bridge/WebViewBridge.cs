namespace WebStyleWPF.Bridge;
[System.Runtime.InteropServices.ComVisible(true)]
public class WebViewBridge
{
    public string GetDataFromCSharp()
    {
        return "This data comes from C#! Current time: " + DateTime.Now.ToString();
    }
}