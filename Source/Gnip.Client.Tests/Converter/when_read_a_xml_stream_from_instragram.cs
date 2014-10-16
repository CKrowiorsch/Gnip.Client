using Krowiorsch.Gnip.Model.Data;
using Krowiorsch.Gnip.Samples;

using Machine.Specifications;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global

namespace Krowiorsch.Gnip.Converter
{
    [Subject("ActivityConvert")]
    public class when_read_a_xml_stream_from_instragram : with_activityConvert
    {
        Because of = () =>
            _result = ActivityConvert.Deserialize(XmlSamplesProvider.GetImageFromInstagram());

        It should_be_a_Image = () =>
            _result.ShouldBeAssignableTo<ActivityImage>();

        It should_have_content = () => 
            ((ActivityImage)_result).ImageContent.ShouldEqual("Old school meet new school... #Neymar #Kaka #Brazil #football");

        It should_have_category_brazil = () =>
            ((ActivityImage)_result).Category.ShouldContain("brazil");

        static Activity _result;
    }
}