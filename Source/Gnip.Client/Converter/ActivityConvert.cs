using System;
using System.Collections.Generic;
using System.Xml;

using Krowiorsch.Gnip.Extensions;
using Krowiorsch.Gnip.Model.Data;

using NLog;

namespace Krowiorsch.Gnip.Converter
{
    public class ActivityConvert
    {
        static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static Activity Deserialize(string xml)
        {
            try
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xml);

                var detector = new ActivityTypeDetector();

                var namespaceManager = CreateNamespaceManager(xmlDocument);
                var type = detector.Detect(xmlDocument, namespaceManager);

                if (type == typeof(ActivityComment))
                    return DeserializeComment(xmlDocument, namespaceManager);

                if (type == typeof(ActivityNote))
                    return DeserializeNote(xmlDocument, namespaceManager);

                if (type == typeof(ActivityPhoto))
                    return DeserializePhoto(xmlDocument, namespaceManager);

                if (type == typeof(ActivityBookmark))
                    return DeserializeBookmark(xmlDocument, namespaceManager);

                if (type == typeof(ActivityVideo))
                    return DeserializeVideo(xmlDocument, namespaceManager);

                if (type == typeof(ActivityQuestion))
                    return DeserializeQuestion(xmlDocument, namespaceManager);

                if (type == typeof(ActivitySwf))
                    return DeserializeSwf(xmlDocument, namespaceManager);

                if (type == typeof(ActivityOffer))
                    return DeserializeOffer(xmlDocument, namespaceManager);

                if (type == typeof(ActivityMusic))
                    return DeserializeMusic(xmlDocument, namespaceManager);

                return DeserializeUnknown(xmlDocument, namespaceManager);
            }
            catch (Exception e)
            {
                Logger.TraceException("Problem beim Convert", e);
                throw;
            }

        }

        static Activity DeserializeMusic(XmlDocument document, XmlNamespaceManager namespaceManager)
        {
            var music = new ActivityMusic();
            var root = music as Activity;

            FillAtomFeedProperties(ref root, document, namespaceManager);

            music.MusicTitle = document.SelectSingleNode("atom:entry/activity:object/atom:title", namespaceManager).InnerTextOrEmpty();
            music.MusicSubTitle = document.SelectSingleNode("atom:entry/activity:object/atom:subtitle", namespaceManager).InnerTextOrEmpty();
            music.MusicContent = document.SelectSingleNode("atom:entry/activity:object/atom:content", namespaceManager).InnerTextOrEmpty();
            music.Via = document.SelectSingleNode("atom:entry/activity:object/atom:link[@rel='via']/@href", namespaceManager).InnerTextOrEmpty();
            music.Enclosure = document.SelectSingleNode("atom:entry/activity:object/atom:link[@rel='enclosure']/@href", namespaceManager).InnerTextOrEmpty();

            var likeCount = document.SelectSingleNode("atom:entry/activity:object/gnip:statistics/@favoriteCount", namespaceManager).ValueOrEmpty();
            music.LikeCount = string.IsNullOrEmpty(likeCount) ? 0 : int.Parse(likeCount);

            return music;
        }

        static Activity DeserializeQuestion(XmlDocument document, XmlNamespaceManager namespaceManager)
        {
            var question = new ActivityQuestion();
            var root = question as Activity;

            FillAtomFeedProperties(ref root, document, namespaceManager);

            question.QuestionTitle = document.SelectSingleNode("atom:entry/activity:object/atom:title", namespaceManager).InnerTextOrEmpty();
            question.Via = document.SelectSingleNode("atom:entry/activity:object/atom:link[@rel='via']/@href", namespaceManager).InnerTextOrEmpty();
            question.QuestionLink = document.SelectSingleNode("atom:entry/activity:object/atom:link[@rel='alternate']/@href", namespaceManager).InnerTextOrEmpty();

            var likeCount = document.SelectSingleNode("atom:entry/activity:object/gnip:statistics/@favoriteCount", namespaceManager).ValueOrEmpty();
            question.LikeCount = string.IsNullOrEmpty(likeCount) ? 0 : int.Parse(likeCount);

            return question;
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

        static Activity DeserializeUnknown(XmlDocument document, XmlNamespaceManager namespaceManager)
        {
            var root = new Activity();

            FillAtomFeedProperties(ref root, document, namespaceManager);
            return root;
        }

        static Activity DeserializeNote(XmlDocument document, XmlNamespaceManager namespaceManager)
        {
            var note = new ActivityNote();
            var root = note as Activity;

            FillAtomFeedProperties(ref root, document, namespaceManager);

            note.NoteTitle = document.SelectSingleNode("atom:entry/activity:object/atom:title", namespaceManager).InnerTextOrEmpty();
            note.NoteContent = document.SelectSingleNode("atom:entry/activity:object/atom:content", namespaceManager).InnerTextOrEmpty();
            note.NoteLink = document.SelectSingleNode("atom:entry/activity:object/atom:link[@rel='alternate']/@href", namespaceManager).InnerTextOrEmpty();
            note.Via = document.SelectSingleNode("atom:entry/activity:object/atom:link[@rel='via']/@href", namespaceManager).InnerTextOrEmpty();

            var likeCount = document.SelectSingleNode("atom:entry/activity:object/gnip:statistics/@favoriteCount", namespaceManager).ValueOrEmpty();
            note.LikeCount = string.IsNullOrEmpty(likeCount) ? 0 : int.Parse(likeCount);

            return note;
        }

        static Activity DeserializeVideo(XmlDocument document, XmlNamespaceManager namespaceManager)
        {
            var video = new ActivityVideo();
            var root = video as Activity;

            FillAtomFeedProperties(ref root, document, namespaceManager);

            video.VideoTitle = document.SelectSingleNode("atom:entry/activity:object/atom:title", namespaceManager).InnerTextOrEmpty();
            video.VideoSubtitle = document.SelectSingleNode("atom:entry/activity:object/atom:subtitle", namespaceManager).InnerTextOrEmpty();
            video.VideoSummary = document.SelectSingleNode("atom:entry/activity:object/atom:summary", namespaceManager).InnerTextOrEmpty();
            video.VideoContent = document.SelectSingleNode("atom:entry/activity:object/atom:content", namespaceManager).InnerTextOrEmpty();
            video.Via = document.SelectSingleNode("atom:entry/activity:object/atom:link[@rel='via']/@href", namespaceManager).InnerTextOrEmpty();
            video.Related = document.SelectSingleNode("atom:entry/activity:object/atom:link[@rel='related']/@href", namespaceManager).InnerTextOrEmpty();
            video.Preview = document.SelectSingleNode("atom:entry/activity:object/atom:link[@rel='preview']/@href", namespaceManager).InnerTextOrEmpty();

            return video;
        }

        static Activity DeserializeOffer(XmlDocument document, XmlNamespaceManager namespaceManager)
        {
            var offer = new ActivityOffer();
            var root = offer as Activity;

            FillAtomFeedProperties(ref root, document, namespaceManager);

            offer.OfferTitle = document.SelectSingleNode("atom:entry/activity:object/atom:title", namespaceManager).InnerTextOrEmpty();
            offer.OfferContent = document.SelectSingleNode("atom:entry/activity:object/atom:content", namespaceManager).InnerTextOrEmpty();
            offer.Via = document.SelectSingleNode("atom:entry/activity:object/atom:link[@rel='via']/@href", namespaceManager).InnerTextOrEmpty();
            offer.Related = document.SelectSingleNode("atom:entry/activity:object/atom:link[@rel='related']/@href", namespaceManager).InnerTextOrEmpty();
            offer.Preview = document.SelectSingleNode("atom:entry/activity:object/atom:link[@rel='preview']/@href", namespaceManager).InnerTextOrEmpty();

            var likeCount = document.SelectSingleNode("atom:entry/activity:object/gnip:statistics/@favoriteCount", namespaceManager).ValueOrEmpty();
            offer.LikeCount = string.IsNullOrEmpty(likeCount) ? 0 : int.Parse(likeCount);

            return offer;
        }

        static Activity DeserializeSwf(XmlDocument document, XmlNamespaceManager namespaceManager)
        {
            var video = new ActivitySwf();
            var root = video as Activity;

            FillAtomFeedProperties(ref root, document, namespaceManager);

            video.SwfTitle = document.SelectSingleNode("atom:entry/activity:object/atom:title", namespaceManager).InnerTextOrEmpty();
            video.SwfSubtitle = document.SelectSingleNode("atom:entry/activity:object/atom:subtitle", namespaceManager).InnerTextOrEmpty();
            video.SwfContent = document.SelectSingleNode("atom:entry/activity:object/atom:summary", namespaceManager).InnerTextOrEmpty();
            video.SwfContent = document.SelectSingleNode("atom:entry/activity:object/atom:content", namespaceManager).InnerTextOrEmpty();
            video.Via = document.SelectSingleNode("atom:entry/activity:object/atom:link[@rel='via']/@href", namespaceManager).InnerTextOrEmpty();
            video.Related = document.SelectSingleNode("atom:entry/activity:object/atom:link[@rel='related']/@href", namespaceManager).InnerTextOrEmpty();
            video.Preview = document.SelectSingleNode("atom:entry/activity:object/atom:link[@rel='preview']/@href", namespaceManager).InnerTextOrEmpty();

            return video;
        }

        static Activity DeserializeComment(XmlDocument document, XmlNamespaceManager namespaceManager)
        {
            var comment = new ActivityComment();
            var root = comment as Activity;

            FillAtomFeedProperties(ref root, document, namespaceManager);

            comment.CommentTitle = document.SelectSingleNode("atom:entry/activity:object/atom:title", namespaceManager).InnerTextOrEmpty();
            comment.CommentContent = document.SelectSingleNode("atom:entry/activity:object/atom:content", namespaceManager).InnerTextOrEmpty();
            comment.CommentLink = document.SelectSingleNode("atom:entry/activity:object/atom:link[@rel='alternate']/@href", namespaceManager).InnerTextOrEmpty();
            comment.Via = document.SelectSingleNode("atom:entry/activity:object/atom:link[@rel='via']/@href", namespaceManager).InnerTextOrEmpty();
            comment.InReplyTo = document.SelectSingleNode("atom:entry/activity:object/thread:in-reply-to/@href", namespaceManager).InnerTextOrEmpty();

            return comment;
        }

        static ActivityPhoto DeserializePhoto(XmlDocument document, XmlNamespaceManager namespaceManager)
        {
            var photo = new ActivityPhoto();
            var root = photo as Activity;

            FillAtomFeedProperties(ref root, document, namespaceManager);

            photo.PhotoTitle = document.SelectSingleNode("atom:entry/activity:object/atom:title", namespaceManager).InnerTextOrEmpty();
            photo.PhotoSubtitle = document.SelectSingleNode("atom:entry/activity:object/atom:subtitle", namespaceManager).InnerTextOrEmpty();
            photo.PhotoContent = document.SelectSingleNode("atom:entry/activity:object/atom:content", namespaceManager).InnerTextOrEmpty();

            photo.Via = document.SelectSingleNode("atom:entry/activity:object/atom:link[@rel='via']/@href", namespaceManager).InnerTextOrEmpty();
            photo.PhotoLink = document.SelectSingleNode("atom:entry/activity:object/atom:link[@rel='alternate']/@href", namespaceManager).InnerTextOrEmpty();
            photo.Related = document.SelectSingleNode("atom:entry/activity:object/atom:link[@rel='related']/@href", namespaceManager).InnerTextOrEmpty();
            photo.Preview = document.SelectSingleNode("atom:entry/activity:object/atom:link[@rel='preview']/@href", namespaceManager).InnerTextOrEmpty();

            return photo;
        }

        static ActivityBookmark DeserializeBookmark(XmlDocument document, XmlNamespaceManager namespaceManager)
        {
            var bookmark = new ActivityBookmark();
            var root = bookmark as Activity;

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

        static void FillAtomFeedProperties(ref Activity root, XmlDocument document, XmlNamespaceManager namespaceManager)
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

        static EntryAuthor ReadAuthor(XmlDocument document, XmlNamespaceManager namespaceManager)
        {
            return new EntryAuthor
            {
                Name = document.SelectSingleNode("atom:entry/atom:author/atom:name", namespaceManager).InnerTextOrEmpty(),
                Link = document.SelectSingleNode("atom:entry/atom:author/atom:uri", namespaceManager).InnerTextOrEmpty()
            };
        }
    }
}