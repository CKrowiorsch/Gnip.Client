using System;
using System.Linq;

using Krowiorsch.Model;
using Krowiorsch.Model.Gnip.Facebook;
using Krowiorsch.Samples;

using Machine.Specifications;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global
namespace Krowiorsch.Converter
{
    public class when_deserialize_gnip_facebook : with_ActivityConvert
    {
        Because of = () =>
            _result = GnipActivityConvert.FromXml(SamplesProvider.GnipFacebook.GetComment());

        It should_be_of_type_activity = () => _result.ShouldBeOfType<FacebookActivity>();

        It should_have_object_of_type_comment = () => _result.Object.ShouldBeOfType<ActivityObjectComment>();

        It should_have_comment_with_title = () => _result.Object.As<ActivityObjectComment>().Title.ShouldEqual("ja sicher :-D ");
        It should_have_comment_with_Id = () => _result.Object.As<ActivityObjectComment>().Id.ShouldEqual("50957555959_10151634728400960_28482842");

        It should_have_comment_with_ViaLink = () =>
            _result.Object.As<ActivityObjectComment>().Links.First(t => t.Relation.Equals("Via", StringComparison.OrdinalIgnoreCase)).Url.ShouldEqual("http://www.facebook.com/RadiobigFM");

        It should_have_comment_with_AlternateLink = () =>
            _result.Object.As<ActivityObjectComment>().Links.First(t => t.Relation.Equals("Alternate", StringComparison.OrdinalIgnoreCase)).Url.ShouldEqual("http://www.facebook.com/50957555959/posts/10151634728400960");

        It should_have_one_matching_rule = () => _result.As<FacebookActivity>().MatchingRules.Count().ShouldEqual(1);

        It should_have_a_matching_rule_with_tag_lm = () => _result.As<FacebookActivity>().MatchingRules[0].Tag.ShouldEqual("LM");
        
        It should_have_a_matching_rule_with_value_50957555959 = () => _result.As<FacebookActivity>().MatchingRules[0].Value.ShouldEqual("50957555959");

        It should_have_a_actor = () => _result.Actor.ShouldNotBeNull();

        static Activity _result;
    }
}