using System.Net;

using Krowiorsch.Gnip.Model;

namespace Krowiorsch.Gnip.Impl
{
    public class GnipTwitterWebRequest
    {
        public static HttpWebRequest Create(GnipAccessToken accessToken, string endpoint)
        {
            var request = (HttpWebRequest)WebRequest.Create(endpoint);
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