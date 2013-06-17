using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

using Krowiorsch.Gnip.Model;

using NLog;

using Newtonsoft.Json;

using System.Linq;

namespace Krowiorsch.Gnip.Impl
{
    public class HttpGnipRulesRepository : IGnipRulesRepository, IDisposable
    {
        static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        readonly HttpClient _client;

        readonly ICredentials _credentials;
        readonly string _baseUrl;

        public HttpGnipRulesRepository(string baseUrl, GnipAccessToken accessToken)
        {
            _baseUrl = baseUrl;
            _client = new HttpClient(new HttpClientHandler
            {
                PreAuthenticate = true,
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
                Credentials = _credentials = new NetworkCredential(accessToken.Username, accessToken.Password)
            }) { BaseAddress = new Uri(baseUrl) };
        }

        public void Clear()
        {
            var rules = List().ToArray();
            Delete(rules);
        }

        /// <exception cref="InvalidOperationException"></exception>
        public void Add(Rule[] rules)
        {
            _client.PostAsJsonAsync("rules.json", new Rules(rules))
                .ContinueWith(t =>
                {
                    if (!t.Result.IsSuccessStatusCode)
                    {
                        Logger.Warn(string.Format("Addes Failed because of: {0}", t.Result.Content.ReadAsStringAsync().Result));
                        //throw new InvalidOperationException(t.Result.ReasonPhrase);
                    }
                    Logger.Info("added successful");
                }).Wait();
        }



        public void DeleteByTag(string tag)
        {
            var rule = List().FirstOrDefault(t => !string.IsNullOrEmpty(t.Tag) && t.Tag.Equals(tag, StringComparison.OrdinalIgnoreCase));

            if (rule == null)
                throw new ArgumentException("rule does not exist");

            var request = BuildWebRequest(_baseUrl + "rules.json", _credentials);
            request.Method = "DELETE";
            byte[] byteArray = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new Rules(rule)));
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;


            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            using (WebResponse response = request.GetResponse())
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string responseFromServer = reader.ReadToEnd();
            }
        }

        public void Delete(Rule[] rules)
        {
            var request = BuildWebRequest(_baseUrl + "/rules.json", _credentials);
            request.Method = "DELETE";
            byte[] byteArray = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new Rules(rules)));
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;


            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            using (var response = request.GetResponse())
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                string responseFromServer = reader.ReadToEnd();
            }
        }

        public Rule[] List()
        {
            return _client.GetStringAsync("rules.json")
                .ContinueWith(t => JsonConvert.DeserializeObject<Rules>(t.Result).List).Result;
        }

        public string Endpoint
        {
            get
            {
                return _baseUrl;
            }
        }

        public HttpWebRequest BuildWebRequest(string url, ICredentials credentials)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Credentials = credentials;
            request.PreAuthenticate = true;

            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            request.Headers.Add("Accept-Encoding", "gzip");

            return request;
        }

        public void Dispose()
        {
            if (_client != null)
                _client.Dispose();
        }
    }
}