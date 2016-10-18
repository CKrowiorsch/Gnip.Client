using System;
using System.Threading.Tasks;

using Krowiorsch.Gnip.Model;

using NLog;

namespace Krowiorsch.Gnip.Scenarios.RawDataScenario
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
            Logger.Debug("Start Scenario");
            var httpStreaming = new ReconnectableHttpStreaming(_streamingEndpoint, _gnipAccessToken);

            httpStreaming.Stream.Subscribe(OnNewLine);
            return httpStreaming.ReadAsync();
        }

        static void OnNewLine(string line)
        {
            Logger.Debug(string.Format("line:{0}", line.Substring(0,300)));
        }
    }
}