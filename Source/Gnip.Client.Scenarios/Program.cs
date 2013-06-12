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
            const string FileToStreamingEndpoint = @"c:\Data\gnip_endpoint.json";
            
            var accessToken = GnipAccessToken.FromJsonStream(new FileStream(FileToAccessToken, FileMode.Open));
            var streamingEndpoint = JsonConvert.DeserializeObject<string>(File.ReadAllText(FileToStreamingEndpoint));

            Logger.Info(string.Format("Use AccessToken: Username:{0} Password:{1}", accessToken.Username, accessToken.Password));
            Logger.Info(string.Format("Use Endpoint: {0}", streamingEndpoint));

            var simpleHttpStreaming = new ReconnectableHttpStreaming(streamingEndpoint, accessToken);
            simpleHttpStreaming.Stream.Subscribe(line => Logger.Debug("l:{0}", line));

            simpleHttpStreaming.ReadAsync().ContinueWith(t => Logger.Info("Streaming terminated"));


            Console.ReadLine();
        }
    }
}
