using System;
using System.Net;

using Krowiorsch.Gnip.Model;

namespace Krowiorsch.Gnip.Impl
{
    internal class GnipWebRequest 
    {
        public static HttpWebRequest Create(GnipAccessToken accessToken, string endpoint)
        {
            var request = (HttpWebRequest)WebRequest.Create(endpoint + "/stream.xml");
            request.Method = "GET";

            string username = accessToken.Username;
            string password = accessToken.Password;

            var nc = new NetworkCredential(username, password);
            request.Credentials = nc;

            request.PreAuthenticate = true;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            request.Headers.Add("Accept-Encoding", "gzip");

            return request;        
        }

        public static HttpWebRequest CreateForDateSlice(GnipAccessToken accessToken, string endpoint, DateTime startDate, DateTime stopDate, int maxCount)
        {
            var endpointWithParameters = string.Format(
                "{0}/activities.xml?max={3}&since_date={1}&to_date={2}", 
                endpoint, 
                startDate.ToUniversalTime().ToString("yyyyMMddHHmmss"), 
                stopDate.ToUniversalTime().ToString("yyyyMMddHHmmss"),
                maxCount);


            var request = (HttpWebRequest)WebRequest.Create(endpointWithParameters);
            request.Method = "GET";

            string username = accessToken.Username;
            string password = accessToken.Password;

            var nc = new NetworkCredential(username, password);
            request.Credentials = nc;

            request.PreAuthenticate = true;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            request.Headers.Add("Accept-Encoding", "gzip");

            return request;
        }
    }
}