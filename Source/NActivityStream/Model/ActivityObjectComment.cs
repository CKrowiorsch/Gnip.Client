namespace Krowiorsch.Model
{
    public class ActivityObjectComment : ActivityObject
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public Link[] Links { get; set; }
    }
}