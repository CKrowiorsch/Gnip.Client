using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Krowiorsch.Gnip.Model;
using Krowiorsch.Gnip.Model.Data;

namespace Krowiorsch.Gnip
{
    /// <summary>
    /// starts a stream from a specific date
    /// </summary>
    public class ReplayAndObserveActivityStreaming : IHttpStreaming<Activity>
    {
        readonly ReplayActivityHttpStreaming _replayStreaming;
        readonly ObserveActivityStreaming _observeActivityStreaming;

        /// <summary>
        /// created a stream
        /// </summary>
        /// <param name="endpoint">Endpoint to GnipStream</param>
        /// <param name="accessToken">Credentials</param>
        /// <param name="startDate">StartDate of the stream (LocalTime)</param>
        public ReplayAndObserveActivityStreaming(string endpoint, GnipAccessToken accessToken, DateTime startDate)
        {
            _replayStreaming = new ReplayActivityHttpStreaming(endpoint, accessToken, startDate);
            _observeActivityStreaming = new ObserveActivityStreaming(endpoint, accessToken);

            Stream = _replayStreaming.Stream.Merge(_observeActivityStreaming.Stream);
        }

        public void Dispose()
        {
            _replayStreaming.Dispose();
            _observeActivityStreaming.Dispose();
        }

        public IObservable<Activity> Stream { get; set; }

        public async Task ReadAsync()
        {
            await _replayStreaming.ReadAsync();
            await _observeActivityStreaming.ReadAsync();
        }

        public void StopStreaming()
        {
            _replayStreaming.StopStreaming();
            _observeActivityStreaming.StopStreaming();
        }
    }
}