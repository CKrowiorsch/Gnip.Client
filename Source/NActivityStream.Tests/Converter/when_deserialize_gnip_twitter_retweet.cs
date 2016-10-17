using System;
using System.Linq;

using Krowiorsch.Model;
using Krowiorsch.Model.Gnip.Twitter;
using Krowiorsch.Samples;

using Machine.Specifications;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global
namespace Krowiorsch.Converter
{
    [Subject("ActivityConvert")]
    public class when_deserialize_gnip_twitter_retweet : with_ActivityConvert
    {
        Because of = () =>
            _result = GnipActivityConvert.FromJson(SamplesProvider.GnipTwitter.GetGnipTwitterShare());

        It should_be_of_type_TwitterActivity = () => _result.ShouldBeAssignableTo<TwitterActivity>();

        It should_have_an_id = () => _result.Id.ShouldEqual("tag:search.twitter.com,2005:424141963890528256");

        It should_have_an_published = () => _result.Published.ShouldNotEqual(default(DateTime));

        It should_have_an_verb = () => _result.Verb.ShouldBeEqualIgnoringCase("share");
        
        It should_have_an_tweet = () => _result.As<TwitterActivity>().Tweet.ShouldEqual("RT @lebelge: Avec Jason Derulo !! jasonderulo #skyrock #paris #radio #live http://t.co/oigzucmd1F");

        It should_have_an_actor_with_id = () => _result.Actor.Id.ShouldEqual("id:twitter.com:455136656");

        It should_have_an_url_from_link_property = () => _result.Url.ShouldEqual("http://twitter.com/ZinedineDu77/statuses/424141963890528256");

        It should_have_a_provider = () => _result.Provider.ShouldNotBeNull();

        It should_have_a_provider_with_type_service = () => _result.Provider.ObjectType.ShouldEqual("service");
        
        It should_have_a_provider_with_name_twitter = () => _result.Provider.DisplayName.ShouldEqual("Twitter");

        It should_have_an_actor_of_type_gnipActor = () => _result.Actor.ShouldBeAssignableTo<TwitterActor>();

        It should_have_an_TwitterActor_with_friendsCount_of_248 = () => _result.Actor.As<TwitterActor>().FriendsCount.ShouldEqual(248);

        It should_have_Entities_with_1_urls = () => _result.As<TwitterActivity>().TwitterEntities.Urls.Count().ShouldEqual(1);

        It should_have_an_gnip_object = () => _result.As<TwitterActivity>().Gnip.ShouldNotBeNull();

        It should_have_the_language_de = () => _result.As<TwitterActivity>().Gnip.Language.Value.ShouldBeEqualIgnoringCase("de");

        It should_have_objecrt_of_type_activity = () => _result.As<TwitterActivity>().Object.ShouldBeAssignableTo<ActivityObjectActivity>();

        It should_have_object_of_type_activity_with_link = () => _result.As<TwitterActivity>().Object.As<ActivityObjectActivity>().Link.ShouldEqual("http://twitter.com/lebelge/statuses/423952828860997632");

        It should_have_object_of_type_activyity_with_summary = () =>
            _result.As<TwitterActivity>().Object.As<ActivityObjectActivity>().Summary.ShouldEqual("Avec Jason Derulo !! jasonderulo #skyrock #paris #radio #live http://t.co/oigzucmd1F");

        It should_have_rules = () => _result.As<TwitterActivity>().Gnip.MatchingRules.ShouldNotBeNull();

        It should_have_1_gnipUrls = () => _result.As<TwitterActivity>().Gnip.Urls.Count().ShouldEqual(1);

        It should_have_expandedUrl = () => _result.As<TwitterActivity>().Gnip.Urls[0].ExpandedUrl.ShouldEqual("http://instagram.com/p/jP1Sjvsm5F/");

        static Activity _result;
    }
}