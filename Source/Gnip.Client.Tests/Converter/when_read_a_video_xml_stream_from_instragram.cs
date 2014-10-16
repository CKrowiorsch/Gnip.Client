using Krowiorsch.Gnip.Model.Data;
using Krowiorsch.Gnip.Samples;

using Machine.Specifications;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global

namespace Krowiorsch.Gnip.Converter
{
    [Subject("ActivityConvert")]
    public class when_read_a_video_xml_stream_from_instragram : with_activityConvert
    {
        Because of = () =>
            _result = ActivityConvert.Deserialize(XmlSamplesProvider.GetVideoFromInstagram());

        It should_be_a_Image = () =>
            _result.ShouldBeOfType<ActivityVideo>();

        It should_have_video_content = () =>
            ((ActivityVideo)_result).VideoContent.ShouldEqual(@"Tom im Sonnenuntergang... immer noch ohne seine Julia. Hilf unserem Pärchen sich wiederzusehen: auf www.yourfone.de/holi #holicalling #holifestival #holifestivalofcolours #holifestival2014 #berlin #holi #holifestivalberlin #yourfone #summer #music #goodafternoon #sunset #festival");

        It should_have_category_brazil = () =>
            ((ActivityVideo)_result).Category.ShouldContain("berlin");

        static Activity _result;
    }
}