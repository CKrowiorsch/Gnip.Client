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
    public class when_deserialize_gnip_twitter_sample : with_ActivityConvert
    {
        Because of = () =>
            _result = GnipActivityConvert.FromJson(SamplesProvider.GnipTwitter.GetGnipTwitter());

        It should_be_of_type_TwitterActivity = () => _result.ShouldBeOfType<TwitterActivity>();

        It should_have_an_id = () => _result.Id.ShouldEqual("tag:search.twitter.com,2005:379576600850755585");

        It should_have_an_published = () => _result.Published.ShouldNotEqual(default(DateTime));

        It should_have_an_verb = () => _result.Verb.ShouldBeEqualIgnoringCase("post");

        It should_have_an_actor_with_id = () => _result.Actor.Id.ShouldEqual("id:twitter.com:556635430");

        It should_have_an_url_from_link_property = () => _result.Url.ShouldEqual("http://twitter.com/AlexListen/statuses/379576600850755585");

        It should_have_an_tweet = () => _result.As<TwitterActivity>().Tweet.ShouldEqual("tnt mod для minecraft 1 0 0: tnt mod для minecraft 1 0 0 http://t.co/l1B5bMQ69B http://t.co/72QVQzJje0");

        It should_have_a_provider = () => _result.Provider.ShouldNotBeNull();

        It should_have_a_provider_with_type_service = () => _result.Provider.ObjectType.ShouldEqual("service");
        
        It should_have_a_provider_with_name_twitter = () => _result.Provider.DisplayName.ShouldEqual("Twitter");

        It should_have_an_actor_of_type_gnipActor = () => _result.Actor.ShouldBeOfType<TwitterActor>();

        It should_have_an_TwitterActor_with_friendsCount_of_4 = () => _result.Actor.As<TwitterActor>().FriendsCount.ShouldEqual(4);

        It should_have_Entities_with_2_urls = () => _result.As<TwitterActivity>().TwitterEntities.Urls.Count().ShouldEqual(2);

        It should_have_an_gnip_object = () => _result.As<TwitterActivity>().Gnip.ShouldNotBeNull();

        It should_have_the_language_de = () => _result.As<TwitterActivity>().Gnip.Language.Value.ShouldBeEqualIgnoringCase("de");

        It should_have_objecrt_of_type_note = () => _result.As<TwitterActivity>().Object.ShouldBeOfType<ActivityObjectNote>();

        It should_have_object_of_type_note_with_link = () => 
            _result.As<TwitterActivity>().Object.As<ActivityObjectNote>().Link.ShouldEqual("http://twitter.com/AlexListen/statuses/379576600850755585");

        It should_have_rules = () => _result.As<TwitterActivity>().Gnip.MatchingRules.ShouldNotBeNull();

        It should_have_two_gnipUrls = () => _result.As<TwitterActivity>().Gnip.Urls.Count().ShouldEqual(2);

        It should_have_expandedUrl = () => _result.As<TwitterActivity>().Gnip.Urls[0].ExpandedUrl.ShouldEqual("http://buketvpodolske.ru/tnt-mod-dlya-minecraft-1-0-0.html?utm_source=twitterfeed&utm_medium=twitter");

        static Activity _result;
    }
}