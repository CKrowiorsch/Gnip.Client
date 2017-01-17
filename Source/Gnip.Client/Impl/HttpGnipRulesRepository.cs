using System;
using System.IO;
using System.Net;
using System.Text;
using Krowiorsch.Gnip.Extensions;
using Krowiorsch.Gnip.Model;
using Newtonsoft.Json;
using System.Linq;

namespace Krowiorsch.Gnip.Impl
{
    public class HttpGnipRulesRepository : IGnipRulesRepository
    {
        readonly string _rulesEndpoint;
        readonly GnipAccessToken _accessToken;

        public HttpGnipRulesRepository(string rulesEndpoint, GnipAccessToken accessToken)
        {
            _rulesEndpoint = rulesEndpoint;
            _accessToken = accessToken;
        }

        public void Clear()
        {
            var rules = List().ToArray();
            Delete(rules);
        }

        public void Add(Rule[] rules)
        {
            // custom serializer to prevent the escaping of quotes on propertynames
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);

            using (JsonWriter jw = new JsonTextWriter(sw))
            {
                jw.WriteStartObject();
                jw.WritePropertyName("rules", false);
                jw.WriteStartArray();
                foreach (var rule in rules)
                {
                    jw.WriteStartObject();
                    jw.WritePropertyName("value", false);
                    jw.WriteValue(rule.Value);
                    jw.WritePropertyName("tag", false);
                    jw.WriteValue(rule.Tag);
                    jw.WriteEndObject();
                }

                jw.WriteEndArray();
                jw.WriteEndObject();
            }

            var addJson = sb.ToString();
            string responseString;

            var resultCode = Rest.GetRestResponse(
                "POST",
                _rulesEndpoint,
                _accessToken.Username,
                _accessToken.Password,
                out responseString,
                addJson);

            if (resultCode == HttpStatusCode.Created)
                return;

            throw new InvalidOperationException(string.Format("AddRules, returned an HTTP Status Code  {0} \r\n{1}", resultCode, responseString));
        }

        public void Delete(Rule[] rules)
        {
            var deleteJson = JsonConvert.SerializeObject(new Rules(rules));
            string responseString;

            var resultCode = Rest.GetRestResponse(
                       "POST",
                       _rulesEndpoint + @"?_method=delete",
                       _accessToken.Username,
                       _accessToken.Password,
                       out responseString,
                       deleteJson);

            if ((int)resultCode >= 200 && (int)resultCode < 300)
                return;

            throw new InvalidOperationException(string.Format("Invalid HttpCode: {0}", resultCode));
        }

        public Rule[] List()
        {
            string content;
            var resultCode = Rest.GetRestResponse("GET", _rulesEndpoint, _accessToken.Username, _accessToken.Password, out content);

            if (resultCode == HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<Rules>(content).List;

            throw new InvalidOperationException(string.Format("Invalid HttpCode: {0}", resultCode));
        }

        public string Endpoint
        {
            get
            {
                return _rulesEndpoint;
            }
        }
    }
}