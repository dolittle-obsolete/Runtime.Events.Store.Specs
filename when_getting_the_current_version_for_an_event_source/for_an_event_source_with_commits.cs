namespace Dolittle.Runtime.Events.Store.Specs.when_getting_the_current_version_for_an_event_source
{
    using Machine.Specifications;
    using Dolittle.Runtime.Events.Store;
    using System;
    using System.Linq;

    [Subject(typeof(IFetchEventSourceVersion))]
    public class for_an_event_source_with_commits : given.an_event_store
    {
        static IEventStore event_store;
        static UncommittedEventStream uncommitted_events;
        static EventSourceKey event_source;
        static DateTimeOffset? occurred;
        static EventSourceVersion version;

        Establish context = () => 
        {
            event_store = get_event_store();
            occurred = DateTimeOffset.UtcNow.AddSeconds(-10);
            event_source = get_event_source_key();
            uncommitted_events = event_source.BuildUncommitted(occurred);
            CommittedEventStream commit = null;
            event_store._do(_ => commit = _.Commit(uncommitted_events));
            uncommitted_events = commit.BuildNext(DateTimeOffset.UtcNow);
            CommittedEventStream second_commit = null;
            event_store._do(_ => second_commit = _.Commit(uncommitted_events));
        };

        Because of = () => event_store._do((es) => version = es.GetCurrentVersionFor(event_source));

        It should_get_a_version_with_the_latest_commit = () => ((long)version.Commit).ShouldEqual(2);
        It should_get_a_version_with_the_latest_sequence = () => ((long)version.Sequence).ShouldEqual(uncommitted_events.Events.Count() - 1);
        It should_not_be_an_intial_version = () => version.ShouldNotEqual(EventSourceVersion.Initial);
        It should_not_be_no_version = () => version.ShouldNotEqual(EventSourceVersion.NoVersion);

        Cleanup nh = () => event_store.Dispose();
    }
}