namespace Dolittle.Runtime.Events.Store.Specs.when_getting_the_next_version_for_an_event_source
{
    using Machine.Specifications;
    using Dolittle.Runtime.Events.Store;
    using System;

    [Subject(typeof(IFetchEventSourceVersion))]
    public class for_a_new_event_source : given.an_event_store
    {
        static EventSourceId event_source_id;
        static IEventStore event_store;
        static EventSourceVersion result;

        Establish context = () => 
        {
            event_store = get_event_store();
        };

        Because of = () => event_store._do((event_store) => result = event_store.GetNextVersionFor(Guid.NewGuid()));

        It should_get_the_initial_version = () => result.ShouldEqual(EventSourceVersion.Initial);

        Cleanup nh = () => event_store.Dispose();       
    }
}