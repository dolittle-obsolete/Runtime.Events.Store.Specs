// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Machine.Specifications;

namespace Dolittle.Runtime.Events.Store.Specs.when_committing_event_streams
{
    [Subject(typeof(ICommitEventStreams))]
    public class when_committing_a_duplicate_commit : given.an_event_store
    {
        static IEventStore event_store;
        static UncommittedEventStream uncommitted_events;
        static DateTimeOffset? occurred;
        static Exception exception;

        Establish context = () =>
        {
            event_store = get_event_store();
            occurred = DateTimeOffset.UtcNow.AddSeconds(-10);
            uncommitted_events = get_event_source_key().BuildUncommitted(occurred);
            event_store._do(_ => _.Commit(uncommitted_events));
        };

        Because of = () => event_store._do((es) => exception = Catch.Exception(() => es.Commit(uncommitted_events)));

        It fails_as_the_commit_is_a_duplicate = () => exception.ShouldBeOfExactType<CommitIsADuplicate>();

        Cleanup nh = () => event_store.Dispose();
    }
}