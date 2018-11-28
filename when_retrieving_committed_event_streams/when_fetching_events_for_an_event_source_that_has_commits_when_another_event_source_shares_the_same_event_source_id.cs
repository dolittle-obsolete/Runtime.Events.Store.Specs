using System;
using System.Linq;
using System.Collections.Generic;
using Machine.Specifications;

namespace Dolittle.Runtime.Events.Store.Specs.when_retrieving_committed_event_streams
{

    [Subject(typeof(IFetchCommittedEvents))]
    public class when_fetching_events_for_an_event_source_that_has_commits_when_another_event_source_shares_the_same_event_source_id : given.an_event_store
    {
        static IEventStore event_store;
        static CommittedEventStream first_commit;
        static CommittedEventStream second_commit;
        static UncommittedEventStream uncommitted_events;
        static EventSourceKey first_event_source;
        static EventSourceKey second_event_source;
        static DateTimeOffset? occurred;
        static Commits result;

        Establish context = () => 
        {
            event_store = get_event_store();
            var id = EventSourceId.New();
            first_event_source = get_event_source_key(id,event_source_artifact);
            second_event_source = get_event_source_key(id,another_event_source_artifact);
            occurred = DateTimeOffset.UtcNow.AddSeconds(-10);
            uncommitted_events = first_event_source.BuildUncommitted(occurred);
            event_store._do((es) => first_commit = es.Commit(uncommitted_events));
            uncommitted_events = first_commit.BuildNext(DateTimeOffset.UtcNow);
            event_store._do((es) => second_commit = es.Commit(uncommitted_events));

            uncommitted_events = second_event_source.BuildUncommitted(occurred);
            event_store._do((es) => es.Commit(uncommitted_events));
        };

        Because of = () => result = event_store.Fetch(first_event_source);

        It should_retrieve_all_the_commits_for_the_event_source = () => (result as IEnumerable<CommittedEventStream>).Count().ShouldEqual(2);
        It should_retrieve_the_commits_in_order = () => 
        {
            result.First().Sequence.ShouldEqual(first_commit.Sequence);
            result.Last().Sequence.ShouldEqual(second_commit.Sequence);
        };
        It should_have_the_events_in_each_commit = () => 
        {
            result.First().Events.ShouldContainOnly(first_commit.Events);
            result.Last().Events.ShouldContainOnly(second_commit.Events);
        };
        Cleanup nh = () => event_store.Dispose();               
    }
}