// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Concepts;
using Dolittle.Events;

namespace Dolittle.Runtime.Events.Store.Specs
{
    public class SimpleEvent : Value<SimpleEvent>, IEvent
    {
        public SimpleEvent(EventId id, string contents, int count)
        {
            Id = id;
            Contents = contents;
            Count = count;
        }

        public SimpleEvent(string contents, int count)
            : this(EventId.New(), contents, count)
        {
        }

        public EventId Id { get; }

        public string Contents { get; }

        public int Count { get; }
    }
}