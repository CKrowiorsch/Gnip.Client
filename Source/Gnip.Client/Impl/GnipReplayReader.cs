using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

using Krowiorsch.Gnip.Model;

namespace Krowiorsch.Gnip.Impl
{
    internal class GnipReplayReader
    {
        readonly Func<HttpWebRequest> _webRequestBuilder;

        public int MaxCount { get; set; }

        public GnipReplayReader(string endpoint, GnipAccessToken accessToken, DateTime startDate, DateTime stopDate)
        {
            MaxCount = 10000;
            _webRequestBuilder = () => GnipWebRequest.CreateForDateSlice(accessToken, endpoint, startDate, stopDate, MaxCount);
        }

        public IEnumerable<string> ReadLines()
        {
            using (var response = _webRequestBuilder().GetResponse())
            using (var reponseStream = response.GetResponseStream())
            using (var reader = new StreamReader(reponseStream))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    if (!string.IsNullOrEmpty(line))
                        yield return line;
                }
            }
        }
    }
}