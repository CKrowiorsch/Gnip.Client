using System;
using System.Collections.Generic;
using System.Linq;

using Krowiorsch.Model.Gnip;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Krowiorsch.Converter.Gnip
{
    public class MatchingRulesConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var commonObject = JObject.Load(reader);

            var children = commonObject.Children();
            existingValue = children.SelectMany(token => ConvertToken(token.First)).ToArray();

            return existingValue;
        }

        IEnumerable<MatchingRule> ConvertToken(JToken token)
        {
            if (token is JArray)
                return (token as JArray).Select(singleToken => new MatchingRule { Tag = singleToken["@tag"].ToString(), Value = singleToken["#text"].ToString() }).ToArray();

            if (token is JProperty)
            {
                var propertyToken = (token as JProperty).First;
                return new[] { new MatchingRule { Tag = propertyToken["@tag"].ToString(), Value = propertyToken["#text"].ToString() } };
            }

            return new[] { new MatchingRule { Tag = token["@tag"].ToString(), Value = token["#text"].ToString() } };
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}