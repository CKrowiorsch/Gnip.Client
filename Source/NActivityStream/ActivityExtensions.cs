using Krowiorsch.Model;

namespace Krowiorsch
{
    public static class ActivityExtensions
    {
        public static T As<T>(this ActivityObject activityObject)
            where T : ActivityObject
        {
            return (T)activityObject;
        }

        public static T As<T>(this Activity activityObject)
            where T : Activity
        {
            return (T)activityObject;
        }
    }
}