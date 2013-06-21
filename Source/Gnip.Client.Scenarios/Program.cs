using System;
using System.IO;

using Krowiorsch.Gnip.Model;

using NLog;

using Newtonsoft.Json;

namespace Krowiorsch.Gnip
{
    class Program
    {
        static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            const string FileToAccessToken = @"c:\Data\gnip_access.json";
            const string FileToStreamingEndpoints = @"c:\Data\gnip_endpoint.json";

            var accessToken = GnipAccessToken.FromJsonStream(new FileStream(FileToAccessToken, FileMode.Open));
            var streamingEndpoint = JsonConvert.DeserializeObject<string[]>(File.ReadAllText(FileToStreamingEndpoints));

            Logger.Info(string.Format("Use AccessToken: Username:{0} Password:{1}", accessToken.Username, accessToken.Password));
            Logger.Info(string.Format("Use Endpoint: {0}", streamingEndpoint[0]));

            //new Scenarios.RawDataScenario.Scenario(accessToken, streamingEndpoint[0])
            //    .Start()
            //    .ContinueWith(t => Logger.Info("Scenario abgeschlossen"));

            //new Scenarios.MultipleStreamScenario.Scenario(accessToken, streamingEndpoint)
            //    .Start()
            //    .ContinueWith(t => Logger.Info("Scenario abgeschlossen"));

            //new Scenarios.AddRulesScenario.Scenario(accessToken, streamingEndpoint[0])
            //    .Start()
            //    .ContinueWith(t => "Finish");

            //new Scenarios.RulesScenario.Scenario(accessToken, streamingEndpoint[0])
            //    .Start()
            //    .ContinueWith(t => "Finish");

            //new Scenarios.ReplayStreamScenario.Scenario(accessToken, streamingEndpoint[1])
            //    .Start()
            //    .ContinueWith(t => "Finish");

            new Scenarios.ReplayAndObserveScenario.Scenario(accessToken, streamingEndpoint[1])
                    .Start()
                    .ContinueWith(t => "Finish");

            Console.ReadLine();
        }
    }
}
