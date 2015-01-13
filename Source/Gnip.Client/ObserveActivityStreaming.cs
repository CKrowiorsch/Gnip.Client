using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

using Krowiorsch.Gnip.Events;
using Krowiorsch.Gnip.Model;
using Krowiorsch.Gnip.Model.Data;

using Krowiorsch.Gnip.Extensions;

namespace Krowiorsch.Gnip
{
    /// <summary>
    /// Stream observes all new Activities on endpoint
    /// </summary>
    public class ObserveActivityStreaming : ReconnectableHttpStreaming, IHttpStreaming<Activity>, IProcessEvents
    {
        readonly Subject<ProcessingEventBase> _internalProcessing;

        /// <summary>
        /// created a stream on specific endpoint
        /// </summary>
        /// <param name="streamingEndpoint"></param>
        /// <param name="accessToken"></param>
        public ObserveActivityStreaming(string streamingEndpoint, GnipAccessToken accessToken)
            : base(streamingEndpoint, accessToken)
        {
            Stream = base.Stream.ToActivityOrNull()
                .Select(a => 
                {
                    if(a == null)
                    {
                        _internalProcessing.OnNext(new FailedDeserialization());        // show error
                        return null;
                    }

                    a.Received = DateTime.Now;              // set received to Now
                    return a;
                })
                .Where(a => a != null);         // filter erroractivities

            Processing = _internalProcessing = new Subject<ProcessingEventBase>();
        }

        public Task ReadAsync()
        {
            _internalProcessing.OnNext(new ObserverGoingLive {StreamId = Endpoint.AbsoluteUri});
            return base.ReadAsync();
        }

        /// <summary> stream of activities  </summary>
        public IObservable<Activity> Stream { get; set; }

        public IObservable<ProcessingEventBase> Processing { get; private set; }
    }
}