namespace Krowiorsch.Samples
{
    public static class SamplesProvider
    {
        public static TwitterSamplesProvider GnipTwitter
        {
            get { return new TwitterSamplesProvider(); }
        }

        public static FacebookSamplesProvider GnipFacebook
        {
            get { return new FacebookSamplesProvider(); }
        }

        public static SpeciaificationSamplesProvider Specifications
        {
            get { return new SpeciaificationSamplesProvider(); }
        }
    }
}