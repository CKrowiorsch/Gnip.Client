using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;

using Krowiorsch.Model;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Krowiorsch.Converter
{
    class ActivityObjectConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            serializer.ContractResolver = null;

            var commonObject = JObject.Load(reader);

            var typeOfObjectActivity = ActivityObjectTypeResolver.GetByName(GetObjectTypeString(commonObject));
            existingValue = commonObject.ToObject(typeOfObjectActivity, serializer);


            // Set links
            var links = ConvertLinks(commonObject);
            var prop = existingValue.GetType().GetProperty("Links", BindingFlags.Public | BindingFlags.Instance);
            if (null != prop && prop.CanWrite)
            {
                prop.SetValue(existingValue, links, null);
            }

            return existingValue;
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var t = JToken.FromObject(value);
            t.WriteTo(writer);
        }

        Link[] ConvertLinks(JObject commonObject)
        {
            JToken token;

            if (commonObject.TryGetValue("link", StringComparison.OrdinalIgnoreCase, out token))
            {
                if (token is JArray)
                {
                    return (token as JArray).Select(singleToken => new Link(singleToken["@href"].ToString()) { Relation = singleToken["@rel"].ToString(), Type = singleToken["@type"].ToString() }).ToArray();
                }

                if (token is JValue)
                {
                    return new[] { new Link(token.ToString()) };
                }

                return new[] { new Link(token["@href"].ToString()) { Relation = token["@rel"].ToString(), Type = token["@type"].ToString() } };
            }

            return null;
        }

        string GetObjectTypeString(JObject commonObject)
        {
            JToken token;

            if (commonObject.TryGetValue("objectType", StringComparison.OrdinalIgnoreCase, out token))
            {
                return token.ToString();
            }

            if (commonObject.TryGetValue("object-type", StringComparison.OrdinalIgnoreCase, out token))
            {
                return token.ToString();
            }

            return string.Empty;
        }
    }
}