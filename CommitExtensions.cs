using System;
using System.Linq;
using System.Collections.Generic;
using Machine.Specifications;

namespace Dolittle.Runtime.Events.Store.Specs
{

    public static class CommitExtensions 
    {
        public static void ShouldEqual(CommittedEventStream returned, CommittedEventStream sent)
        {
            returned.Id.ShouldEqual(sent.Id);
            returned.CorrelationId.ShouldEqual(sent.CorrelationId);
            returned.Sequence.ShouldEqual(sent.Sequence);
            returned.Source.ShouldEqual(sent.Source);
            returned.Timestamp.ToUnixTimeMilliseconds().ShouldEqual(sent.Timestamp.ToUnixTimeMilliseconds());
        } 

        public static void ShouldEqual(this EventStream returned, EventStream sent)
        {
            var returnedEvents = returned.ToArray();
            var sentEvents = sent.ToArray();
            returnedEvents.Count().ShouldEqual(sentEvents.Count());
            for(int i = 0; i < returnedEvents.Count(); i++)
            {
               returnedEvents[i].ShouldEqual(sentEvents[i]);
            }
        } 

        public static void ShouldEqual(this EventEnvelope first, EventEnvelope other)
        {
            first.Id.ShouldEqual(other.Id);
            first.Metadata.ShouldEqual(other.Metadata);
            first.Event.ShouldEqual(other.Event);
        }  

        public static void ShouldEqual(this EventMetadata first, EventMetadata second)
        {
            first.Id.ShouldEqual(second.Id);
            first.Artifact.Equals(first.Artifact);
            first.CorrelationId.ShouldEqual(second.CorrelationId);
            first.EventSourceId.ShouldEqual(second.EventSourceId);
            first.Occurred.ToUnixTimeMilliseconds().ShouldEqual(second.Occurred.ToUnixTimeMilliseconds());
            first.OriginalContext.ShouldEqual(second.OriginalContext);
            first.VersionedEventSource.ShouldEqual(second.VersionedEventSource);
        }
    }
}