using System;

namespace Krowiorsch.Model
{
    /// <summary>
    /// Represents a short-form text message. This object is intended primarily for use in "micro-blogging" scenarios and in systems where users are invited to publish short, often plain-text messages whose useful lifespan is generally shorter than that of an article of weblog entry. A note is similar in structure to an article, but typically does not have a title or distinct paragraphs and tends to be much shorter in length.
    /// </summary>
    public class ActivityObjectNote : ActivityObject
    {
        public string Summary { get; set; }

        public string Link { get; set; }

        public DateTime PostedTime { get; set; }
    }
}