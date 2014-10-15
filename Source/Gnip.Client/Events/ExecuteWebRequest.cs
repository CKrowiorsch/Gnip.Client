using System;

namespace Krowiorsch.Gnip.Events
{
    /// <summary> an WebRequest was executed </summary>
    public class ExecuteWebRequest : ProcessingEventBase
    {
        /// <summary> url of WebRequest </summary>
        public Uri Url { get; set; }
    }

    /// <summary>  a webrequest failed  </summary>
    public class FailedWebRequest : ProcessingEventBase
    {
        /// <summary> Url of WebRequest </summary>
        public Uri Url { get; set; }
    }

    /// <summary>  an Observer going in live state </summary>
    public class ObserverGoingLive : ProcessingEventBase
    {
        /// <summary> eindeutige Stream id </summary>
        public string StreamId { get; set; }
    }
}