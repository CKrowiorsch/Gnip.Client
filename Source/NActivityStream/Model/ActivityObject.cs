namespace Krowiorsch.Model
{
    public class ActivityObject
    {
        /// <summary> Provides a permanent, universally unique identifier for the object in the form of an absolute IRI [RFC3987] </summary>
        public string Id { get; set; }

        /// <summary> Identifies the type of object. </summary>
        public string ObjectType { get; set; }

        /// <summary> A collection of one or more additional, associated objects, similar to the concept of attached files in an email message. </summary>
        public ActivityObject[] Attachments { get; set; }

        /// <summary> An IRI [RFC3987] identifying a resource providing an HTML representation of the object </summary>
        public string Url { get; set; }

        /// <summary> A natural-language, human-readable and plain-text name for the object </summary>
        public string DisplayName { get; set; }

        /// <summary> Description of a resource providing a visual representation of the object, intended for human consumption. </summary>
        public string Image { get; set; }
    }
}