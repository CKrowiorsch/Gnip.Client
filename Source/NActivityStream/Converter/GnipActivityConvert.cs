using System.IO;
using System.Linq;
using System.Xml.Linq;

using Krowiorsch.Model;
using Krowiorsch.Model.Gnip.Facebook;
using Krowiorsch.Model.Gnip.Twitter;

using Newtonsoft.Json;

namespace Krowiorsch.Converter
{
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

        public static Activity FromXml(string xmlString)
        {
            XDocument document = XDocument.Load(new StringReader(xmlString));
            
            // remove namespaces to flatten attributes
            document.Descendants().Attributes().Where(a => a.IsNamespaceDeclaration).Remove();

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new GnipXmlContractResolver(),
            };

            return JsonConvert.DeserializeObject<FacebookActivity>(JsonConvert.SerializeXNode(document.Root, Formatting.None, true), settings);
        }


        static Activity FromJson(string jsonString, JsonSerializerSettings settings)
        {
            return JsonConvert.DeserializeObject<TwitterActivity>(jsonString, settings);
        }
    }
}