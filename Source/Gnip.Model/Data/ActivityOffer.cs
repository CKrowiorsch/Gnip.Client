namespace Krowiorsch.Gnip.Model.Data
{
    public class ActivityOffer : Activity
    {
        public string OfferTitle { get; set; }

        public string OfferContent { get; set; }

        public string Related { get; set; }

        public string Via { get; set; }

        public string Preview { get; set; }

        public int LikeCount { get; set; }
    }
}