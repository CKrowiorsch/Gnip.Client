namespace Krowiorsch.Model.Gnip.Twitter
{
    public class TwitterActor : ActivityObject
    {
        public int FriendsCount { get; set; }

        public int FollowersCount { get; set; }

        public int ListedCount { get; set; }
    }
}