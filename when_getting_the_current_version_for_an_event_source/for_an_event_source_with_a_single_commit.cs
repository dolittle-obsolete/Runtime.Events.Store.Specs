// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using Machine.Specifications;

namespace Dolittle.Runtime.Events.Store.Specs.when_getting_the_current_version_for_an_event_source
{
    [Subject(typeof(IFetchEventSourceVersion))]
    public class for_an_event_source_with_a_single_commit : given.an_event_store
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
            event_store._do(_ => _.Commit(uncommitted_events));
        };

        Because of = () => event_store._do((es) => version = es.GetCurrentVersionFor(event_source));

        It should_get_the_initial_commit = () => version.Commit.ShouldEqual(EventSourceVersion.Initial.Commit);
        It should_get_a_version_with_the_latest_sequence = () => ((long)version.Sequence).ShouldEqual(uncommitted_events.Events.Count() - 1);
        It should_not_be_an_intial_version = () => version.ShouldNotEqual(EventSourceVersion.Initial);
        It should_not_be_no_version = () => version.ShouldNotEqual(EventSourceVersion.NoVersion);

        Cleanup nh = () => event_store.Dispose();
    }
}