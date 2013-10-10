namespace Krowiorsch.Model.Gnip.Twitter
{
    public class TwitterActor : ActivityObject
    {
        public int FriendsCount { get; set; }

        public int FollowersCount { get; set; }

        public int ListedCount { get; set; }

        public int FavoritesCount { get; set; }

        public string Summary { get; set; }

        public TwitterLocation Location { get; set; }

        public int StatusesCount { get; set; }

        public string[] Languages { get; set; }

        public string Link { get; set; }
    }
}