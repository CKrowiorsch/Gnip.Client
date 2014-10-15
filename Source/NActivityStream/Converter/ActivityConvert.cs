using Krowiorsch.Model;

using Newtonsoft.Json;

namespace Krowiorsch.Converter
{
    /// <summary> Convert a jsonstring from and into a valid Activity </summary>
    public static class ActivityConvert
    {
        /// <summary> reads activity from jsonstring </summary>
        public static Activity FromJson(string jsonString)
        {
            return JsonConvert.DeserializeObject<Activity>(jsonString);
        }

        /// <summary> Convert an Activity Into JsonString </summary>
        public static string ToJson(Activity activity)
        {
            return JsonConvert.SerializeObject(activity);
        }
    }
}