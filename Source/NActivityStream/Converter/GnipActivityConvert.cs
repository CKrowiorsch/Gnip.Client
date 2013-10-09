using Krowiorsch.Model;
using Krowiorsch.Model.Gnip;
using Krowiorsch.Model.Gnip.Twitter;

using Newtonsoft.Json;

namespace Krowiorsch.Converter
{
    public class GnipActivityConvert
    {
        public static Activity FromJson(string jsonString)
        {
            var settings = new JsonSerializerSettings { ContractResolver = new GnipContractResolver() };
            settings.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;

            return JsonConvert.DeserializeObject<TwitterActivity>(jsonString, settings);
        }
    }
}