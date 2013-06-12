using System;
using System.IO;

using Krowiorsch.Gnip.Model;

using NLog;

namespace Krowiorsch.Gnip
{
    class Program
    {
        static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            const string FileToAccessToken = @"c:\Data\gnip_access.json";
            var accessToken = GnipAccessToken.FromJsonStream(new FileStream(FileToAccessToken, FileMode.Open));

            Logger.Info(string.Format("Use AccessToken: Username:{0} Password:{1}", accessToken.Username, accessToken.Password));

            Console.ReadLine();
        }
    }
}
