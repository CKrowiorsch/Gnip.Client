using System.IO;
using System.Linq;
using System.Xml.Linq;

using Krowiorsch.Model;
using Krowiorsch.Model.Gnip.Facebook;
using Krowiorsch.Model.Gnip.Twitter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Krowiorsch.Converter
{
    /// <summary> Convertiert Gnipdaten in Activities </summary>
    public class GnipActivityConvert
    {
        public static Activity FromJson(string jsonString)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new GnipContractResolver(),
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            };

            return FromJson(jsonString, settings);
        }

        /// <summary> detects, if there is an activity in the string </summary>
        public static bool IsActivity(string jsonString)
        {
            var o = JObject.Parse(jsonString);
            return o["objectType"] != null;
        }

        public static Activity FromXml(string xmlString)
        {
            var document = XDocument.Load(new StringReader(xmlString));

            // remove namespaces to flatten attributes
            var root = RemoveAllNamespaces(document.Root);
            var json = JsonConvert.SerializeXNode(root, Formatting.None, true);

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new GnipXmlContractResolver(),
            };

            return JsonConvert.DeserializeObject<FacebookActivity>(json, settings);
        }

        static XElement RemoveAllNamespaces(XElement xmlDocument)
        {
            if (!xmlDocument.HasElements)
            {
                var xElement = new XElement(xmlDocument.Name.LocalName) { Value = xmlDocument.Value };

                foreach (var attribute in xmlDocument.Attributes())
                    xElement.Add(attribute);

                return xElement;
            }
            return new XElement(xmlDocument.Name.LocalName, xmlDocument.Elements().Select(RemoveAllNamespaces));
        }


        static Activity FromJson(string jsonString, JsonSerializerSettings settings)
        {
            return JsonConvert.DeserializeObject<TwitterActivity>(jsonString, settings);
        }
    }
}