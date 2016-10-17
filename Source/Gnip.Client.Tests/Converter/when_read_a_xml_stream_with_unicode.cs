using Krowiorsch.Gnip.Model.Data;
using Krowiorsch.Gnip.Samples;

using Machine.Specifications;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global
namespace Krowiorsch.Gnip.Converter
{
    [Subject("ActivityConvert")]
    public class when_read_a_xml_stream_with_unicode : with_activityConvert
    {
        Because of = () =>
            _result = ActivityConvert.Deserialize(XmlSamplesProvider.GetCommentWithMultipleRules());

        It should_be_a_Note = () =>
            _result.ShouldBeAssignableTo<ActivityComment>();

        It should_have_two_rules = () => 
            _result.Rules.Length.ShouldEqual(2);

        It should_have_first_rule_with_179800383306 = () =>
            _result.Rules[0].Rule.ShouldEqual("179800383306");

        It should_have_second_rule_with_microsoft = () =>
            _result.Rules[1].Rule.ShouldEqual("Microsoft");
        
        It should_have_second_rule_rel_as_source = () =>
            _result.Rules[0].Related.ShouldEqual("source");

        It should_have_second_rule_res_as_inferred = () =>
            _result.Rules[1].Related.ShouldEqual("inferred");

        static Activity _result;
    }
}