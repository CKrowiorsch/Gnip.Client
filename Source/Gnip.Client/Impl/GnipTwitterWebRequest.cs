using System;
using System.Net;

using Krowiorsch.Gnip.Model;

namespace Krowiorsch.Gnip.Impl
{
    internal class GnipTwitterWebRequest
    {
        public static HttpWebRequest Create(GnipAccessToken accessToken, string endpoint)
        {
            var request = (HttpWebRequest)WebRequest.Create(endpoint);
            request.Method = "GET";

            var username = accessToken.Username;
            var password = accessToken.Password;

            var nc = new NetworkCredential(username, password);
            request.Credentials = nc;

            request.PreAuthenticate = true;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            request.Headers.Add("Accept-Encoding", "gzip");

            return request;
        }

        public static HttpWebRequest CreateForDateSlice(GnipAccessToken accessToken, string endpoint, DateTime from, DateTime to)
        {
            var endpointwithdate = string.Format("{0}?fromDate={1}&toDate={2}", endpoint, from.ToString("yyyyMMddHHmm"), to.ToString("yyyyMMddHHmm"));

            var request = (HttpWebRequest)WebRequest.Create(endpointwithdate);
            request.Method = "GET";

            var username = accessToken.Username;
            var password = accessToken.Password;

            var nc = new NetworkCredential(username, password);
            request.Credentials = nc;

            request.PreAuthenticate = true;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            request.Headers.Add("Accept-Encoding", "gzip");

            return request;
        }
    }
}