using System;

using Krowiorsch.Gnip.Model;
using Krowiorsch.Gnip.Model.Data;

using Krowiorsch.Gnip.Extensions;

namespace Krowiorsch.Gnip
{
    public class ObserveActivityStreaming : ReconnectableHttpStreaming, IHttpStreaming<Activity>
    {
        public ObserveActivityStreaming(string streamingEndpoint, GnipAccessToken accessToken)
            : base(streamingEndpoint, accessToken)
        {
            Stream = base.Stream.ToActivity();
        }

        public IObservable<Activity> Stream { get; set; }
    }
}