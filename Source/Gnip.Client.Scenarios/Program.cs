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
            const string FileToAccessTokenTwitter = @"c:\Data\gnip_twitter_access.json";
            const string FileToStreamingEndpoints = @"c:\Data\gnip_twitter_endpoint.json";

            var accessToken = GnipAccessToken.FromJsonStream(new FileStream(FileToAccessToken, FileMode.Open));
            var twitterAccessToken = GnipAccessToken.FromJsonStream(new FileStream(FileToAccessTokenTwitter, FileMode.Open));
            var streamingEndpoint = JsonConvert.DeserializeObject<string[]>(File.ReadAllText(FileToStreamingEndpoints));

            var powertrackRules = "https://gnip-api.twitter.com/rules/powertrack/accounts/LandauMedia/publishers/twitter/dev.json";

            //new Scenarios.RulesScenario.Scenario(accessToken, powertrackRules).Start();
            
            Logger.Info(string.Format("Use AccessToken: Username:{0} Password:{1}", accessToken.Username, accessToken.Password));
            Logger.Info(string.Format("Use Endpoint: {0}", streamingEndpoint[0]));

            new Scenarios.TwitterObserveScenario.Scenario(accessToken, streamingEndpoint[0]).Start();

            //new Scenarios.InstagramScenario.Scenario(accessToken, new[] {"https://landaumedia1.gnip.com/data_collectors/4"}).Start();

            //new Scenarios.TwitterReplayScenario.Scenario(twitterAccessToken, "https://stream.gnip.com:443/accounts/LandauMedia/publishers/twitter/replay/track/prod").Start();
            Console.ReadLine();
        }
    }
}
