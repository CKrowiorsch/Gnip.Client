using System;

using Krowiorsch.Converter;

using Newtonsoft.Json;

namespace Krowiorsch.Model
{
    /// <summary>
    /// Activityclass
    /// </summary>
    /// <see cref="http://activitystrea.ms/specs/json/1.0/"/>
    public class Activity
    {
        /// <summary> Provides a permanent, universally unique identifier for the activity in the form of an absolute IRI </summary>
        public string Id { get; set; }

        /// <summary> Identifies the action that the activity describes. </summary>
        public string Verb { get; set; }

        /// <summary> Describes the entity that performed the activity. </summary>
        public ActivityObject Actor { get; set; }

        /// <summary> Natural-language title or headline for the activity </summary>
        public string Title { get; set; }

        /// <summary> Natural-language description of the activity </summary>
        public string Content { get; set; }

        /// <summary> The date and time at which the activity was published.  </summary>
        public DateTime Published { get; set; }

        /// <summary> The date and time at which a previously published activity has been modified. </summary>
        public DateTime Updated { get; set; }

        /// <summary> An IRI [RFC3987] identifying a resource providing an HTML representation of the activity. </summary>
        public string Url { get; set; }

        /// <summary> Describes the target of the activity. </summary>
        public ActivityObject Target { get; set; }

        /// <summary> Describes the application that published the activity. </summary>
        public ActivityObject Provider { get; set; }

        /// <summary> Describes the application that generated the activity. </summary>
        public ActivityObject Generator { get; set; }

        /// <summary> Describes the primary object of the activity </summary>
        [JsonConverter(typeof(ActivityObjectConverter))]
        public ActivityObject Object { get; set; }
    }
}