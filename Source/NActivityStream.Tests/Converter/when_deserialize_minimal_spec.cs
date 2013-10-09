using System;

using Krowiorsch.Model;
using Krowiorsch.Samples;

using Machine.Specifications;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global
namespace Krowiorsch.Converter
{
    public class when_deserialize_minimal_spec : with_ActivityConvert
    {
        Because of = () =>
           _result = ActivityConvert.FromJson(SamplesProvider.Specifications.GetMinmal());

        It should_get_a_result = () => _result.ShouldNotBeNull();

        It should_have_the_published_date = () => _result.Published.ShouldEqual(DateTimeOffset.Parse("2011-02-10T15:04:55Z").DateTime);

        It should_have_a_verb = () => _result.Verb.ShouldEqual("post");

        It should_have_an_target = () => _result.Target.ShouldNotBeNull();

        It should_have_an_target_with_url = () => _result.Target.Url.ShouldEqual("http://example.org/blog/");

        It should_have_an_target_with_object_type = () => _result.Target.ObjectType.ShouldEqual("blog");

        static Activity _result;
    }
}