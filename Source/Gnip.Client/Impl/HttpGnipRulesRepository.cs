using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using Krowiorsch.Gnip.Extensions;
using Krowiorsch.Gnip.Model;
using Newtonsoft.Json;
using System.Linq;

namespace Krowiorsch.Gnip.Impl
{
    public class HttpGnipRulesRepository : IGnipRulesRepository, IDisposable
    {
        readonly HttpClient _client;

        readonly ICredentials _credentials;
        readonly string _baseUrl;

        readonly string _rulesEndpoint;
        readonly GnipAccessToken _accessToken;

        public HttpGnipRulesRepository(string rulesEndpoint, GnipAccessToken accessToken)
        {
            //_client = new HttpClient(new HttpClientHandler
            //{
            //    PreAuthenticate = true,
            //    AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
            //    Credentials = _credentials = new NetworkCredential(accessToken.Username, accessToken.Password)
            //}) { BaseAddress = new Uri(baseUrl) };

            _rulesEndpoint = rulesEndpoint;
            _accessToken = accessToken;
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
                        throw new InvalidOperationException(t.Result.Content.ReadAsStringAsync().Result);
                    }
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
            string content;
            var resultCode = Rest.GetRestResponse("Get", _rulesEndpoint, _accessToken.Username, _accessToken.Password, out content);

            if (resultCode == HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<Rules>(content).List;
            
            throw new InvalidOperationException(string.Format("Invalid HttpCode: {0}", resultCode));
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