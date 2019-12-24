// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Machine.Specifications;

namespace Dolittle.Runtime.Events.Store.Specs.when_getting_the_current_version_for_an_event_source
{
    [Subject(typeof(IFetchEventSourceVersion))]
    public class for_a_new_event_source : given.an_event_store
    {
        static IEventStore event_store;
        static EventSourceVersion result;

        Establish context = () => event_store = get_event_store();

        Because of = () => event_store._do((event_store) => result = event_store.GetCurrentVersionFor(get_event_source_key()));

        It should_return_no_version = () => result.ShouldEqual(EventSourceVersion.NoVersion);

        Cleanup nh = () => event_store.Dispose();
    }
}