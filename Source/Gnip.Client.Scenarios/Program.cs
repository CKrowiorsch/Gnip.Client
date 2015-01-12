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

            Logger.Info(string.Format("Use AccessToken: Username:{0} Password:{1}", accessToken.Username, accessToken.Password));
            Logger.Info(string.Format("Use Endpoint: {0}", streamingEndpoint[0]));

            Console.ReadLine();
        }
    }
}
