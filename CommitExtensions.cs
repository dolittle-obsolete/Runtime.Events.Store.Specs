// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using Machine.Specifications;

namespace Dolittle.Runtime.Events.Store.Specs
{
    public static class CommitExtensions
    {
        public static void ShouldEqual(this CommittedEventStream returned, CommittedEventStream sent)
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
            returnedEvents.Length.ShouldEqual(sentEvents.Length);
            for (int i = 0; i < returnedEvents.Length; i++)
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