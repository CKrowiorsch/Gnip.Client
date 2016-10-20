using System;
using System.Reactive.Linq;

using Krowiorsch.Converter;
using Krowiorsch.Gnip.Impl;
using Krowiorsch.Gnip.Model;
using Krowiorsch.Model;

namespace Krowiorsch.Gnip
{
    public class ObserveTwitterHttpStreaming : ReconnectableHttpStreaming, IHttpStreaming<Activity>
    {
        public ObserveTwitterHttpStreaming(string streamingEndpoint, GnipAccessToken accessToken, bool useBackfill = false)
            : base(streamingEndpoint, accessToken)
        {
            _webRequestBuilder = () => GnipTwitterWebRequest.Create(accessToken, streamingEndpoint, useBackfill);
            Stream = base.Stream.Select(GnipActivityConvert.FromJson);
        }

        public new IObservable<Activity> Stream { get; set; }
    }
}