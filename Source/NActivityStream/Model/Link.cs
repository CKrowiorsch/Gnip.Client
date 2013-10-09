namespace Krowiorsch.Model
{
    public class Link
    {
        public Link(string url)
        {
            Url = url;
        }

        public string Url { get; set; }

        public string Relation { get; set; }

        public string Type { get; set; }
    }
}