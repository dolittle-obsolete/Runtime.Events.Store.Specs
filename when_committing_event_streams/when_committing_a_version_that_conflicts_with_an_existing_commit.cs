// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Machine.Specifications;

namespace Dolittle.Runtime.Events.Store.Specs.when_committing_event_streams
{
    [Subject(typeof(ICommitEventStreams))]
    public class when_committing_a_version_that_conflicts_with_an_existing_commit : given.an_event_store
    {
        static IEventStore event_store;
        static UncommittedEventStream uncommitted_events;
        static UncommittedEventStream conflicting_uncommitted_events;
        static DateTimeOffset? occurred;
        static Exception exception;

        Establish context = () =>
        {
            event_store = get_event_store();
            occurred = DateTimeOffset.UtcNow.AddSeconds(-10);
            var event_source = get_event_source_key();
            uncommitted_events = event_source.BuildUncommitted(occurred);
            var next = uncommitted_events.BuildNext();
            event_store._do(_ => _.Commit(uncommitted_events));
            event_store._do(_ => _.Commit(next));
            conflicting_uncommitted_events = event_source.BuildUncommitted(occurred);
        };

        Because of = () => event_store._do((es) => exception = Catch.Exception(() => es.Commit(conflicting_uncommitted_events)));

        It fails_as_the_commit_has_a_concurrency_conflict = () => exception.ShouldBeOfExactType<EventSourceConcurrencyConflict>();

        Cleanup nh = () => event_store.Dispose();
    }
}