using System.Collections.Generic;

using Newtonsoft.Json.Serialization;

namespace Krowiorsch.Converter
{
    class GnipXmlContractResolver : DefaultContractResolver
    {
        readonly Dictionary<string, string> _remappedProperties = new Dictionary<string, string>
        {
            {"matchingrules", "matching_rules"},
        };

        protected override string ResolvePropertyName(string propertyName)
        {
            return _remappedProperties.ContainsKey(propertyName.ToLower())
                ? _remappedProperties[propertyName.ToLower()]
                : base.ResolvePropertyName(propertyName);
        }

    }
}