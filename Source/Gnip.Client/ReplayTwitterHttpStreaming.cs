using System;
using System.Reactive.Linq;

using Krowiorsch.Converter;
using Krowiorsch.Gnip.Impl;
using Krowiorsch.Gnip.Model;
using Krowiorsch.Model;

namespace Krowiorsch.Gnip
{
    /// <summary> request old tweet from replay stream </summary>
    public class ReplayTwitterHttpStreaming : ReconnectableHttpStreaming, IHttpStreaming<Activity>
    {
        readonly DateTime _fromDate;
        readonly DateTime _toDate;

        public ReplayTwitterHttpStreaming(string streamingEndpoint, GnipAccessToken accessToken, DateTime fromDate)
            : this(streamingEndpoint, accessToken, fromDate, DateTime.Now.AddMinutes(-30).ToUniversalTime())
        {
        }

        public ReplayTwitterHttpStreaming(string streamingEndpoint, GnipAccessToken accessToken, DateTime fromDate, DateTime toDate)
            : base(streamingEndpoint, accessToken)
        {
            _fromDate = fromDate;
            _toDate = toDate;

            _webRequestBuilder = () => GnipTwitterWebRequest.CreateForDateSlice(accessToken, streamingEndpoint, _fromDate, _toDate);
            Stream = base.Stream
                .Where(GnipActivityConvert.IsActivity)
                .Select(GnipActivityConvert.FromJson);

            StreamRaw = base.Stream;
        }

        public IObservable<Activity> Stream { get; set; }
        
        public IObservable<string> StreamRaw { get; set; }
    }
}