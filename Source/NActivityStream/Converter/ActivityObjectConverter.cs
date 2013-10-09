using System;
using System.Collections.Generic;

using Krowiorsch.Model;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Krowiorsch.Converter
{
    class ActivityObjectConverter : JsonConverter
    {
        Dictionary<string,Type> _mapping = new Dictionary<string, Type>()
        {
            {"note", typeof(ActivityObjectNote)}
        }; 

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            serializer.ContractResolver = null;

            var commonObject = JObject.Load(reader);

            if (_mapping.ContainsKey(commonObject["objectType"].ToString()))
                return commonObject.ToObject(_mapping[commonObject["objectType"].ToString()], serializer);

            return commonObject.ToObject<ActivityObject>();
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