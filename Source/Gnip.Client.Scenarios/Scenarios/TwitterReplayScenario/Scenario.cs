using System;
using System.Threading.Tasks;

using Krowiorsch.Gnip.Model;

using NLog;

namespace Krowiorsch.Gnip.Scenarios.TwitterReplayScenario
{
    public class Scenario
    {
        static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        readonly GnipAccessToken _gnipAccessToken;
        readonly string _streamingEndpoint;

        int _count = 0;

        public Scenario(GnipAccessToken gnipAccessToken, string streamingEndpoint)
        {
            _gnipAccessToken = gnipAccessToken;
            _streamingEndpoint = streamingEndpoint;
        }

        public Task Start()
        {
            DateTime startDate = DateTime.Now.AddMinutes(-31).ToUniversalTime();

            var streaming = new ReplayTwitterHttpStreaming(_streamingEndpoint, _gnipAccessToken, startDate);

            streaming.Stream.Subscribe(OnNewActivity);
            //streaming.StreamRaw.Subscribe(s => OnNewLine(s));

            return streaming.ReadAsync().ContinueWith(t => Logger.WarnException("Stream aborted", t.Exception));
        }

        void OnNewActivity(Krowiorsch.Model.Activity activity)
        {
            Logger.Info("New Activity ... {0}", activity.Published.ToLocalTime());
            _count++;
        }
    }
}