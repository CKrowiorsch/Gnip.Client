using System;
using System.Threading.Tasks;

using Krowiorsch.Gnip.Model;
using Krowiorsch.Model.Gnip.Twitter;
using NLog;

namespace Krowiorsch.Gnip.Scenarios.TwitterObserveScenario
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
            var streaming = new ObserveTwitterHttpStreaming(_streamingEndpoint, _gnipAccessToken);

            streaming.Stream.Subscribe(OnNewActivity);

            return streaming.ReadAsync().ContinueWith(t => Logger.WarnException("Stream aborted", t.Exception));
        }

        void OnNewActivity(Krowiorsch.Model.Activity activity)
        {
            Logger.Info("New Activity ... {0}", ((TwitterActivity)activity).Tweet);
            _count++;
        }
    }
}