using System;

namespace Krowiorsch.Gnip.Events
{
    public class ExecuteWebRequest : ProcessingEventBase
    {
        public Uri Url { get; set; }
    }
}