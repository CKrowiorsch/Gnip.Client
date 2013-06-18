namespace Krowiorsch.Gnip.Model.Data
{
    public class ActivityQuestion : Activity
    {
        public string QuestionTitle { get; set; }

        public string QuestionLink { get; set; }

        public string Via { get; set; }

        public int LikeCount { get; set; }
    }
}