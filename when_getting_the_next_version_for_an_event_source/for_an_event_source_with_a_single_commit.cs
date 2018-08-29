﻿namespace Dolittle.Runtime.Events.Store.Specs.when_getting_the_next_version_for_an_event_source
{
    using Machine.Specifications;
    using Dolittle.Runtime.Events.Store;
    using System;
    using System.Linq;

    [Subject(typeof(IFetchEventSourceVersion))]
    public class for_an_event_source_with_a_single_commit : given.an_event_store
    {
        static IEventStore event_store;
        static CommittedEventStream committed_events;
        static UncommittedEventStream uncommitted_events;
        static EventSourceId event_source_id;
        static DateTimeOffset? occurred;
        static EventSourceVersion version;

        Establish context = () => 
        {
            event_store = get_event_store();
            occurred = DateTimeOffset.UtcNow.AddSeconds(-10);
            event_source_id = EventSourceId.New();
            uncommitted_events = event_source_id.BuildUncommitted(event_source_artifact, occurred);
            event_store.Commit(uncommitted_events);
        };

        Because of = () => event_store._do((es) => version = es.GetNextVersionFor(event_source_id));

        It should_get_the_the_second_commit = () => version.Commit.ShouldEqual(2ul);
        It should_have_a_sequence_of_0 = () => version.Sequence.ShouldEqual(0u);
        It should_not_be_an_intial_version = () => version.ShouldNotEqual(EventSourceVersion.Initial);
        It should_not_be_no_version = () => version.ShouldNotEqual(EventSourceVersion.NoVersion);

        Cleanup nh = () => event_store.Dispose();
    }
}