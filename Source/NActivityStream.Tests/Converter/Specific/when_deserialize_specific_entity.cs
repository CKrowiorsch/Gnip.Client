using Krowiorsch.Model;
using Krowiorsch.Samples;

using Machine.Specifications;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global
namespace Krowiorsch.Converter.Specific
{
    public class when_deserialize_specific_entity : with_ActivityConvert
    {
        Because of = () =>
            _result = GnipActivityConvert.FromJson(SamplesProvider.GnipTwitter.GetGnipSpecific("entity"));

        It should_converted = () => 
            _result.ShouldNotBeNull();

        It should_have_a_id = () =>
            _result.Id.ShouldEqual("tag:search.twitter.com,2005:397352542696251395");

        static Activity _result;
    }
}