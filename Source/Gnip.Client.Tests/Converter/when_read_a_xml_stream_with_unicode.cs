using Krowiorsch.Gnip.Model.Data;
using Krowiorsch.Gnip.Samples;

using Machine.Specifications;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global
namespace Krowiorsch.Gnip.Converter
{
    public class when_read_a_xml_stream_with_unicode : with_activityConvert
    {
        Because of = () =>
            _result = ActivityConvert.Deserialize(XmlSamplesProvider.GetCommentWithUniCode());

        It should_be_a_Note = () =>
            _result.ShouldBeOfType<ActivityNote>();

        It should_have_a_unicodestring = () =>
            ((ActivityNote)_result).NoteContent.ShouldEqual("정희원 이건뭐얔ㅋㅋㅋㅋㅋㅋ 티켓구해와 ㅋㅋㅋㅋ");

        static Activity _result;
    }
}