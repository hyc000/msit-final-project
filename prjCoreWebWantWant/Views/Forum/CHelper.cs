using HtmlAgilityPack;

namespace prjCoreWebWantWant.Views.Forum
{
    public class CHelper
    {
        public static string CutHtml(string html, int maxLength)//用HtmlAgility來截html
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var text = doc.DocumentNode.InnerText;

            if (text.Length <= maxLength)
            {
                return html;
            }

            var truncatedText = text.Substring(0, maxLength);
            var truncatedHtml = html.Substring(0, truncatedText.Length);

            return truncatedHtml + "...";
        }

        public static string TruncateText(string text, int maxLength)
        {
            if (text.Length <= maxLength)
            {
                return text;
            }
            else
            {
                return text.Substring(0, maxLength) + " ...";
            }
        }
    }
}
