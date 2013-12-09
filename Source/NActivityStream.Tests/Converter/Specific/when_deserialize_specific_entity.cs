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

        static Activity _result;
    }
}