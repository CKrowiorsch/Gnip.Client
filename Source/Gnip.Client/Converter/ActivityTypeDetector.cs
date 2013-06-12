using System;
using System.Xml;

using Krowiorsch.Gnip.Model.Data;

namespace Krowiorsch.Gnip.Converter
{
    public class ActivityTypeDetector
    {
        public Type Detect(XmlDocument document, XmlNamespaceManager namespaceManager)
        {
            var typeNode = document.SelectSingleNode("//activity:object/activity:object-type", namespaceManager);

            if (typeNode == null)
                throw new ArgumentOutOfRangeException();

            if (typeNode.InnerText.Equals("http://activitystrea.ms/schema/1.0/comment", StringComparison.OrdinalIgnoreCase))
                return typeof(ActivityComment);

            if (typeNode.InnerText.Equals("http://activitystrea.ms/schema/1.0/note", StringComparison.OrdinalIgnoreCase))
                return typeof(ActivityNote);

            if (typeNode.InnerText.Equals("http://activitystrea.ms/schema/1.0/photo", StringComparison.OrdinalIgnoreCase))
                return typeof(ActivityPhoto);

            if (typeNode.InnerText.Equals("http://activitystrea.ms/schema/1.0/bookmark", StringComparison.OrdinalIgnoreCase))
                return typeof(ActivityBookmark);


            return typeof(Activity);
        }
    }
}