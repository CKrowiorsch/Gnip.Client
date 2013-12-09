using System;
using System.Collections.Generic;
using System.Reflection;

using Krowiorsch.Model.Gnip.Twitter;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Krowiorsch.Converter
{
    public class GnipContractResolver : DefaultContractResolver
    {
        readonly Dictionary<string, string> _remappedProperties = new Dictionary<string, string>
        {
            {"published", "postedTime"},
            {"url", "link"},
        };

        readonly Dictionary<string, Type> _remappedTypes = new Dictionary<string, Type>
        {
            {"actor", typeof(TwitterActor)},
        };

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            if (_remappedTypes.ContainsKey(member.Name.ToLower()))
            {
                var property = base.CreateProperty(member, memberSerialization);
                property.PropertyType = _remappedTypes[member.Name.ToLower()];
                return property;
            }

            return base.CreateProperty(member, memberSerialization);
        }

        protected override string ResolvePropertyName(string propertyName)
        {
            return _remappedProperties.ContainsKey(propertyName.ToLower())
                ? _remappedProperties[propertyName.ToLower()]
                : base.ResolvePropertyName(propertyName);
        }
    }
}