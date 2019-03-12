using Newtonsoft.Json;

namespace Krowiorsch.Gnip.Model
{
    public class Rule
    {
        [JsonProperty("tag")]
        public string Tag { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        public override int GetHashCode()
        {
            int hash = 13;

            if (!string.IsNullOrEmpty(Tag))
                hash = (hash * 7) + Tag.GetHashCode();

            if (!string.IsNullOrEmpty(Value))
                hash = (hash * 7) + Value.GetHashCode();

            return hash;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            // If parameter cannot be cast to Point return false.
            var p = obj as Rule;
            return p != null && Equals(p);
        }

        public bool Equals(Rule p)
        {
            if (p == null)
                return false;

            // Return true if the fields match:
            return (Tag == p.Tag) && (Value == p.Value);
        }
    }
}