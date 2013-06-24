using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Krowiorsch.Gnip.Extensions;
using Krowiorsch.Gnip.Impl;
using Krowiorsch.Gnip.Model;
using Krowiorsch.Gnip.Model.Data;

using NLog;

namespace Krowiorsch.Gnip.Scenarios.SingleTypeScenario
{
    public class Scenario
    {
        static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        readonly GnipAccessToken _gnipAccessToken;
        readonly string _streamingEndpoint;

        public Scenario(GnipAccessToken gnipAccessToken, string streamingEndpoint)
        {
            _gnipAccessToken = gnipAccessToken;
            _streamingEndpoint = streamingEndpoint;
        }

        public Task Start()
        {
            var stream = new ReconnectableHttpStreaming(_streamingEndpoint, _gnipAccessToken);

            stream.Stream.ToActivity()
                .OfType<ActivityComment>()
                .Subscribe(OnNewActivity);

            stream.Stream.Buffer(TimeSpan.FromSeconds(10)).Subscribe(t => Logger.Info(string.Format("{0} Activties received", t.Count)));

            return stream.ReadAsync();
        }

        static void OnNewActivity(ActivityComment activity)
        {
            Logger.Info(string.Format("New Activity Provider:{2} [{0}]: {1}", activity.GetType().Name, activity.GetContent().ToSingleLine(), activity.Link));
        }
    }
}