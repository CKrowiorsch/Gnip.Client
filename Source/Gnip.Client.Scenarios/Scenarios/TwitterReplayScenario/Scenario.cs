using System;
using System.Reactive.Linq;
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

            var streaming = new ReplayTwitterHttpStreaming(_streamingEndpoint, _gnipAccessToken, new DateTime(2015, 3, 2, 10, 00, 00));

            //streaming.Stream.Subscribe(OnNewActivity);
            streaming.StreamRaw
                .Where(t => t.Contains("Frühstadium"))
                .Subscribe(s => OnNewLine(s));

            return streaming.ReadAsync().ContinueWith(t => Logger.WarnException("Stream aborted", t.Exception));
        }

        void OnNewLine(string s)
        {
            Logger.Debug(s);
        }

        void OnNewActivity(Krowiorsch.Model.Activity activity)
        {
            Logger.Info("New Activity ... {0}", activity.Published.ToLocalTime());
            _count++;
        }
    }
}