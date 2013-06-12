using System.IO;
using System.Text;

using Newtonsoft.Json;

namespace Krowiorsch.Gnip.Model
{
    public class GnipAccessToken
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public static GnipAccessToken FromJsonStream(Stream stream)
        {
            using (var reader = new StreamReader(stream, Encoding.Default))
                return JsonConvert.DeserializeObject<GnipAccessToken>(reader.ReadToEnd());
        }
    }
}