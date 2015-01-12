using System;
using System.Threading.Tasks;

using Krowiorsch.Gnip.Extensions;
using Krowiorsch.Gnip.Model;
using Krowiorsch.Gnip.Model.Data;

using NLog;

namespace Krowiorsch.Gnip.Scenarios.ReplayStreamScenario
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
            var startDate = DateTime.Today.AddDays(-2);

            var streaming = new ReplayActivityHttpStreaming(_streamingEndpoint, _gnipAccessToken, startDate);

            streaming.Stream.Subscribe(OnNewActivity);
            return streaming.ReadAsync().ContinueWith(t => Logger.Warn(string.Format("{0} Activities", _count)));
        }

        void OnNewActivity(Activity activity)
        {
            Logger.Info(string.Format("{3} - New Activity Provider:{2} [{0}]: {1}", activity.GetType().Name, activity.GetContent().ToSingleLine(), activity.Link, activity.Published.ToShortDateString() + " " + activity.Published.ToShortTimeString()));
            _count++;
        }
    }
}