using Krowiorsch.Model;
using Krowiorsch.Samples;

using Machine.Specifications;
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global



namespace Krowiorsch.Converter
{
    public class when_serialize_and_deserialize_minimal_activity : with_ActivityConvert
    {
        Establish context = () => 
            _activity = ActivityConvert.FromJson(SamplesProvider.Specifications.GetMinmal());

        Because of = () => 
            _dumpAndLoadedActivity = ActivityConvert.FromJson(ActivityConvert.ToJson(_activity));

        It should_have_the_same_title = () => 
            _dumpAndLoadedActivity.Title.ShouldEqual(_activity.Title);

        It should_have_the_same_Content = () =>
            _dumpAndLoadedActivity.Content.ShouldEqual(_activity.Content);


        static Activity _activity;

        static Activity _dumpAndLoadedActivity;
    }
}