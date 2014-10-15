using System;
using System.Collections.Generic;
using System.Linq;

using Krowiorsch.Model;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Krowiorsch.Converter
{
    public class MediaLinkConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
                existingValue = new MediaLink() { Url = reader.Value.ToString() };
            else
                existingValue = JObject.Load(reader).ToObject<MediaLink>();

            return existingValue;
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken t = JToken.FromObject(value);

            if(t.Type != JTokenType.Object)
            {
                t.WriteTo(writer);
            }
            else
            {
                t.WriteTo(writer);
            }

            
        }
    }
}