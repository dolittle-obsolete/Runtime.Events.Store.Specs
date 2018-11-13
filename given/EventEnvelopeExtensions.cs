using Dolittle.Time;

namespace Dolittle.Runtime.Events.Store.Specs.given
{
    public static class EventMetadataExtensions
    {
        public static bool LossyEquals(this EventMetadata first, EventMetadata second)
        {
            return 
                    first.Id.Equals(second.Id) 
                &&  first.Artifact.Equals(first.Artifact)
                &&  first.CorrelationId.Equals(second.CorrelationId)
                &&  first.EventSourceId.Equals(second.EventSourceId)
                &&  first.Occurred.LossyEquals(second.Occurred)
                &&  first.OriginalContext.Equals(second.OriginalContext)
                &&  first.VersionedEventSource.Equals(second.VersionedEventSource);
        }
    }
}