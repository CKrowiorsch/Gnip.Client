using System;
using System.Collections.Generic;
using System.Xml;

using Krowiorsch.Gnip.Extensions;
using Krowiorsch.Gnip.Model.Facebook;

using NLog;

namespace Krowiorsch.Gnip.Converter
{
    public class FacebookConvert
    {
        static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static FacebookRoot Deserialize(string xml)
        {
            try
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xml);

                var detector = new FaceBookTypeDetector();

                var namespaceManager = CreateNamespaceManager(xmlDocument);
                var type = detector.Detect(xmlDocument, namespaceManager);

                if (type == typeof(FacebookComment))
                    return DeserializeComment(xmlDocument, namespaceManager);

                if (type == typeof(FacebookNote))
                    return DeserializeNote(xmlDocument, namespaceManager);

                if (type == typeof(FacebookPhoto))
                    return DeserializePhoto(xmlDocument, namespaceManager);

                if (type == typeof(FacebookBookmark))
                    return DeserializeBookmark(xmlDocument, namespaceManager);

                return DeserializeUnknown(xmlDocument, namespaceManager);
            }
            catch (Exception e)
            {
                Logger.TraceException("Problem beim Convert", e);
                throw;
            }

        }

        static XmlNamespaceManager CreateNamespaceManager(XmlDocument xmlDocument)
        {
            var namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
            namespaceManager.AddNamespace("atom", "http://www.w3.org/2005/Atom");
            namespaceManager.AddNamespace("activity", "http://activitystrea.ms/spec/1.0/");
            namespaceManager.AddNamespace("service", "http://activitystrea.ms/service-provider");
            namespaceManager.AddNamespace("thread", "http://purl.org/syndication/thread/1.0");
            namespaceManager.AddNamespace("gnip", "http://www.gnip.com/schemas/2010");

            return namespaceManager;
        }

        static FacebookRoot DeserializeUnknown(XmlDocument document, XmlNamespaceManager namespaceManager)
        {
            var root = new FacebookRoot();

            FillAtomFeedProperties(ref root, document, namespaceManager);
            return root;
        }

        static FacebookRoot DeserializeNote(XmlDocument document, XmlNamespaceManager namespaceManager)
        {
            var note = new FacebookNote();
            var root = note as FacebookRoot;

            FillAtomFeedProperties(ref root, document, namespaceManager);

            note.NoteTitle = document.SelectSingleNode("atom:entry/activity:object/atom:title", namespaceManager).InnerTextOrEmpty();
            note.NoteContent = document.SelectSingleNode("atom:entry/activity:object/atom:content", namespaceManager).InnerTextOrEmpty();
            note.NoteLink = document.SelectSingleNode("atom:entry/activity:object/atom:link[@rel='alternate']/@href", namespaceManager).InnerTextOrEmpty();
            note.Via = document.SelectSingleNode("atom:entry/activity:object/atom:link[@rel='via']/@href", namespaceManager).InnerTextOrEmpty();

            var likeCount = document.SelectSingleNode("atom:entry/activity:object/gnip:statistics/@favoriteCount", namespaceManager).ValueOrEmpty();
            note.LikeCount = string.IsNullOrEmpty(likeCount) ? 0 : int.Parse(likeCount);

            return note;
        }

        static FacebookRoot DeserializeComment(XmlDocument document, XmlNamespaceManager namespaceManager)
        {
            var comment = new FacebookComment();
            var root = comment as FacebookRoot;

            FillAtomFeedProperties(ref root, document, namespaceManager);

            comment.CommentTitle = document.SelectSingleNode("atom:entry/activity:object/atom:title", namespaceManager).InnerTextOrEmpty();
            comment.CommentContent = document.SelectSingleNode("atom:entry/activity:object/atom:content", namespaceManager).InnerTextOrEmpty();
            comment.CommentLink = document.SelectSingleNode("atom:entry/activity:object/atom:link[@rel='alternate']/@href", namespaceManager).InnerTextOrEmpty();
            comment.Via = document.SelectSingleNode("atom:entry/activity:object/atom:link[@rel='via']/@href", namespaceManager).InnerTextOrEmpty();
            comment.InReplyTo = document.SelectSingleNode("atom:entry/activity:object/thread:in-reply-to/@href", namespaceManager).InnerTextOrEmpty();

            return comment;
        }

        static FacebookPhoto DeserializePhoto(XmlDocument document, XmlNamespaceManager namespaceManager)
        {
            var photo = new FacebookPhoto();
            var root = photo as FacebookRoot;

            FillAtomFeedProperties(ref root, document, namespaceManager);

            photo.PhotoTitle = document.SelectSingleNode("atom:entry/activity:object/atom:title", namespaceManager).InnerTextOrEmpty();
            photo.PhotoSubtitle = document.SelectSingleNode("atom:entry/activity:object/atom:subtitle", namespaceManager).InnerTextOrEmpty();
            photo.PhotoContent = document.SelectSingleNode("atom:entry/activity:object/atom:content", namespaceManager).InnerTextOrEmpty();

            photo.Via = document.SelectSingleNode("atom:entry/activity:object/atom:link[@rel='via']/@href", namespaceManager).InnerTextOrEmpty();
            photo.Link = document.SelectSingleNode("atom:entry/activity:object/atom:link[@rel='alternate']/@href", namespaceManager).InnerTextOrEmpty();
            photo.Related = document.SelectSingleNode("atom:entry/activity:object/atom:link[@rel='related']/@href", namespaceManager).InnerTextOrEmpty();
            photo.Preview = document.SelectSingleNode("atom:entry/activity:object/atom:link[@rel='preview']/@href", namespaceManager).InnerTextOrEmpty();

            return photo;
        }

        static FacebookBookmark DeserializeBookmark(XmlDocument document, XmlNamespaceManager namespaceManager)
        {
            var bookmark = new FacebookBookmark();
            var root = bookmark as FacebookRoot;

            FillAtomFeedProperties(ref root, document, namespaceManager);

            bookmark.BookmarkTitle = document.SelectSingleNode("atom:entry/activity:object/atom:title", namespaceManager).InnerTextOrEmpty();
            bookmark.BookmarkSubtitle = document.SelectSingleNode("atom:entry/activity:object/atom:subtitle", namespaceManager).InnerTextOrEmpty();
            bookmark.BookmarkContent = document.SelectSingleNode("atom:entry/activity:object/atom:content", namespaceManager).InnerTextOrEmpty();
            bookmark.BookmarkSummary = document.SelectSingleNode("atom:entry/activity:object/atom:summary", namespaceManager).InnerTextOrEmpty();

            bookmark.Via = document.SelectSingleNode("atom:entry/activity:object/atom:link[@rel='via']/@href", namespaceManager).InnerTextOrEmpty();
            bookmark.Link = document.SelectSingleNode("atom:entry/activity:object/atom:link[@rel='alternate']/@href", namespaceManager).InnerTextOrEmpty();
            bookmark.Related = document.SelectSingleNode("atom:entry/activity:object/atom:link[@rel='related']/@href", namespaceManager).InnerTextOrEmpty();
            bookmark.Preview = document.SelectSingleNode("atom:entry/activity:object/atom:link[@rel='preview']/@href", namespaceManager).InnerTextOrEmpty();

            var likeCount = document.SelectSingleNode("atom:entry/activity:object/gnip:statistics/@favoriteCount", namespaceManager).ValueOrEmpty();
            bookmark.LikeCount = string.IsNullOrEmpty(likeCount) ? 0 : int.Parse(likeCount);

            return bookmark;
        }

        static void FillAtomFeedProperties(ref FacebookRoot root, XmlDocument document, XmlNamespaceManager namespaceManager)
        {
            root.Id = document.SelectSingleNode("atom:entry/atom:id", namespaceManager).InnerTextOrEmpty();
            root.Title = document.SelectSingleNode("atom:entry/atom:title", namespaceManager).InnerTextOrEmpty();
            root.Link = document.SelectSingleNode("atom:entry/atom:link/@href", namespaceManager).ValueOrEmpty();
            root.Created = DateTime.Parse(document.SelectSingleNode("atom:entry/atom:created", namespaceManager).InnerTextOrEmpty());
            root.Published = DateTime.Parse(document.SelectSingleNode("atom:entry/atom:published", namespaceManager).InnerTextOrEmpty());
            root.Updated = DateTime.Parse(document.SelectSingleNode("atom:entry/atom:updated", namespaceManager).InnerTextOrEmpty());
            root.ObjectTypeUri = document.SelectSingleNode("atom:entry/activity:object/activity:object-type", namespaceManager).InnerTextOrEmpty();

            root.Author = ReadAuthor(document, namespaceManager);
            root.Rules = ReadMatchingRule(document, namespaceManager);

            root.RawXml = document.OuterXml;
        }


        static MatchingRule[] ReadMatchingRule(XmlDocument document, XmlNamespaceManager namespaceManager)
        {
            var list = new List<MatchingRule>();


            foreach (var rule in document.SelectNodes("atom:entry/gnip:matching_rules", namespaceManager))
            {
                var ruleNode = rule as XmlNode;

                list.Add(new MatchingRule
                {
                    Rule = ruleNode.InnerTextOrEmpty(),
                    Tag = ruleNode == null ? string.Empty : ruleNode.Attributes["tag"].ValueOrEmpty()
                });
            }

            return list.ToArray();
        }

        static FacebookAuthor ReadAuthor(XmlDocument document, XmlNamespaceManager namespaceManager)
        {
            return new FacebookAuthor
            {
                Name = document.SelectSingleNode("atom:entry/atom:author/atom:name", namespaceManager).InnerTextOrEmpty(),
                Link = document.SelectSingleNode("atom:entry/atom:author/atom:uri", namespaceManager).InnerTextOrEmpty()
            };
        }
    }
}