using System.Linq;
using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Krowiorsch.Gnip.Extensions;
using Krowiorsch.Gnip.Model;
using Krowiorsch.Gnip.Model.Data;

using NLog;

namespace Krowiorsch.Gnip.Scenarios.InstagramScenario
{
    public class Scenario
    {
        static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        readonly GnipAccessToken _gnipAccessToken;
        readonly string[] _streamingEndpoints;

        public Scenario(GnipAccessToken gnipAccessToken, string[] streamingEndpoints)
        {
            _gnipAccessToken = gnipAccessToken;
            _streamingEndpoints = streamingEndpoints;
        }

        public Task Start()
        {
            //var streams = _streamingEndpoints.Select(s => new ReconnectableHttpStreaming(s, _gnipAccessToken)).ToArray();
            var streams = _streamingEndpoints.Select(s => new ReplayAndObserveActivityStreaming(s, _gnipAccessToken, DateTime.Now.AddDays(-3))).ToArray();

            streams.Select(t => t.Stream)
                .Merge()
                .Subscribe(OnNewActivity);

            streams.ElementAt(0).Processing.Subscribe(i => Logger.Debug(i));

            return Task.WhenAll(streams.Select(s => s.ReadAsync()));
        }

        static void OnNewActivity(Activity activity)
        {
            Logger.Info(string.Format("New Activity Provider:{2} [{0}]: {1}", activity.GetType().Name, activity.GetContent().ToSingleLine(), activity.Link));
        }
    }
}