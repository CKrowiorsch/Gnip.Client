using System.Xml;

namespace Krowiorsch.Gnip.Extensions
{
    public static class XmlDocumentExtensions
    {
        public static string InnerTextOrEmpty(this XmlNode node)
        {
            return node == null ? string.Empty : node.InnerText;
        }

        public static string ValueOrEmpty(this XmlNode node)
        {
            if (node == null || node.Value == null)
                return string.Empty;

            return node.Value;
        }
    }
}