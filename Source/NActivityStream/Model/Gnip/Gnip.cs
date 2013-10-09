namespace Krowiorsch.Model.Gnip
{
    public class Gnip
    {
        public MatchingRule[] Rules { get; set; }

        public GnipUrl[] Urls { get; set; }

        public GnipLanguage Language { get; set; }
    }
}