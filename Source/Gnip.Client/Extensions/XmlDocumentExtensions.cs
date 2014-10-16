using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Krowiorsch.Gnip.Extensions
{
    public static class XmlDocumentExtensions
    {
        internal static string InnerTextOrEmpty(this XmlNode node)
        {
            return node == null ? string.Empty : node.InnerText;
        }

        internal static DateTime AsDateTimeOrDefault(this XmlNode node)
        {
            DateTime output;

            if (node == null)
                return default(DateTime);

            return DateTime.TryParse(node.InnerText, out output) 
                ? output 
                : default( DateTime );
        }

        internal static DateTime AsDateTime(this XmlNode node)
        {
            return DateTime.Parse(node.InnerText);
        }

        internal static string ValueOrEmpty(this XmlNode node)
        {
            if (node == null || node.Value == null)
                return string.Empty;

            return node.Value;
        }

        internal static IEnumerable<XmlNode> SelectNodesOrEmpty(this XmlNode node, string xPath, XmlNamespaceManager manager)
        {
            var nodeList = node.SelectNodes(xPath, manager);
            return nodeList == null ? Enumerable.Empty<XmlNode>() : nodeList.OfType<XmlNode>();
        }
    }
}