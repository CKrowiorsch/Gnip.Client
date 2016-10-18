using System;
using System.Net;
using System.Text;
using Krowiorsch.Gnip.Model;

namespace Krowiorsch.Gnip.Impl
{
    internal class GnipTwitterWebRequest
    {
        public static HttpWebRequest Create(GnipAccessToken accessToken, string endpoint, bool useMaxBackfile)
        {
            var usedEndpoint = endpoint;

            if (useMaxBackfile)
                usedEndpoint += "?backfillMinutes=5";


            var request = (HttpWebRequest)WebRequest.Create(usedEndpoint);
            request.Method = "GET";

            var username = accessToken.Username;
            var password = accessToken.Password;

            var authInfo = string.Format("{0}:{1}", username, password);
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            request.Headers.Add("Authorization", "Basic " + authInfo);

            request.PreAuthenticate = true;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            request.Headers.Add("Accept-Encoding", "gzip");
            request.Accept = "application/json";
            request.ContentType = "application/json";
            request.ReadWriteTimeout = 30000;
            request.AllowReadStreamBuffering = false;
            request.Timeout = 30000;

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