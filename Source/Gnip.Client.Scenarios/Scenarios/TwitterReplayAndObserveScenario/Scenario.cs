using System;
using System.Threading.Tasks;

using Krowiorsch.Gnip.Model;

using NLog;

namespace Krowiorsch.Gnip.Scenarios.TwitterReplayAndObserveScenario
{
    public class Scenario
    {
        static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        readonly GnipAccessToken _gnipAccessToken;
        readonly string _streamingEndpointReplay;

        string _streamingEndpointObserve;

        public Scenario(GnipAccessToken gnipAccessToken, string streamingEndpointReplay, string streamingEndpointObserve)
        {
            _gnipAccessToken = gnipAccessToken;
            _streamingEndpointReplay = streamingEndpointReplay;
            _streamingEndpointObserve = streamingEndpointObserve;
        }

        public Task Start()
        {
            DateTime startDate = DateTime.Now.AddMinutes(-31).ToUniversalTime();

            var streaming = new ReplayAndObserveTwitterStreaming(_streamingEndpointReplay, _streamingEndpointObserve, _gnipAccessToken, startDate);

            streaming.Stream.Subscribe(OnNewActivity);

            return streaming.ReadAsync().ContinueWith(t => Logger.Warn(t.Exception, "Stream aborted"));
        }

        void OnNewActivity(Krowiorsch.Model.Activity activity)
        {
            Logger.Info("New Activity ... {0}", activity.Published.ToLocalTime());
        }
    }
}