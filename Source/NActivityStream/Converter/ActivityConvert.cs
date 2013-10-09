using Krowiorsch.Model;

using Newtonsoft.Json;

namespace Krowiorsch.Converter
{
    public static class ActivityConvert
    {
        public static Activity FromJson(string jsonString)
        {
            return JsonConvert.DeserializeObject<Activity>(jsonString);
        }
    }
}