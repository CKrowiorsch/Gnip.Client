using System;
using System.IO;
using System.Linq;

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
            const string FileToStreamingEndpointsFacebook = @"c:\Data\gnip_endpoint_facebook.json";
            const string FileToStreamingEndpointsInstagram = @"c:\Data\gnip_endpoint_instagram.json";

            var accessToken = GnipAccessToken.FromJsonStream(new FileStream(FileToAccessToken, FileMode.Open));
            var twitterAccessToken = GnipAccessToken.FromJsonStream(new FileStream(FileToAccessTokenTwitter, FileMode.Open));
            var streamingEndpointFacebook = JsonConvert.DeserializeObject<string[]>(File.ReadAllText(FileToStreamingEndpointsFacebook));
            var streamingEndpointInstagram = JsonConvert.DeserializeObject<string[]>(File.ReadAllText(FileToStreamingEndpointsInstagram));

            Logger.Info(string.Format("Use AccessToken: Username:{0} Password:{1}", accessToken.Username, accessToken.Password));
            Logger.Info(string.Format("Use AccessToken Twitter: Username:{0} Password:{1}", twitterAccessToken.Username, twitterAccessToken.Password));
            Logger.Info(string.Format("Use Endpoint Facebook: {0}", streamingEndpointFacebook[0]));
            Logger.Info(string.Format("Use Endpoint Instagram: {0}", streamingEndpointInstagram[0]));

            var scenario = new Scenarios.ReplayAndObserveScenario.Scenario(accessToken, streamingEndpointInstagram.First());

            scenario.Start();
            Console.ReadLine();
        }
    }
}
