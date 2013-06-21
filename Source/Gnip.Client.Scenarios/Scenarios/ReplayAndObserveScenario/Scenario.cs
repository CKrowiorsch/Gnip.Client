using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Krowiorsch.Gnip.Extensions;
using Krowiorsch.Gnip.Model;
using Krowiorsch.Gnip.Model.Data;

using NLog;

namespace Krowiorsch.Gnip.Scenarios.ReplayAndObserveScenario
{
    public class Scenario
    {
        static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        readonly GnipAccessToken _gnipAccessToken;
        readonly string _streamingEndpoint;

        int _count;

        public Scenario(GnipAccessToken gnipAccessToken, string streamingEndpoint)
        {
            _gnipAccessToken = gnipAccessToken;
            _streamingEndpoint = streamingEndpoint;
        }

        public Task Start()
        {
            var startDate = DateTime.Today.AddDays(-2);

            var streaming = new ReplayAndObserveActivityStreaming(_streamingEndpoint, _gnipAccessToken, startDate);

            streaming.Stream.Subscribe(OnNewActivity);
            Observable.Interval(TimeSpan.FromSeconds(10)).Subscribe(s => Logger.Info(string.Format("{0} Activities arrived", _count)));

            return streaming.ReadAsync();
        }

        void OnNewActivity(Activity activity)
        {
            Logger.Info(string.Format("{3} - New Activity Provider:{2} [{0}]: {1}", activity.GetType().Name, activity.GetContent().ToSingleLine(), activity.Link, activity.Published.ToShortDateString() + " " + activity.Published.ToShortTimeString()));
            _count++;
        }
    }
}