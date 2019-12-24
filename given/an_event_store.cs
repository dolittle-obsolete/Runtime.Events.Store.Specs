// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using Dolittle.Artifacts;

namespace Dolittle.Runtime.Events.Store.Specs.given
{
    public class an_event_store
    {
        public static readonly ArtifactId event_source_artifact = Guid.NewGuid();
        public static readonly ArtifactId another_event_source_artifact = Guid.NewGuid();
        public static ConcurrentDictionary<Type, ArtifactId> event_artifacts = new ConcurrentDictionary<Type, ArtifactId>();

        protected static Func<IEventStore> get_event_store = () =>
        {
            if (_sut_provider_type == null)
            {
                var asm = Assembly.GetExecutingAssembly();
                _sut_provider_type = asm.GetExportedTypes().Single(t => t.IsClass && typeof(IProvideTheEventStore).IsAssignableFrom(t));
            }

            var factory = Activator.CreateInstance(_sut_provider_type) as IProvideTheEventStore;
            return factory.Build();
        };

        static Type _sut_provider_type;

        public static EventSourceKey get_event_source_key(EventSourceId event_source_id = null, ArtifactId artifact_id = null)
        {
            var es = event_source_id ?? EventSourceId.New();
            var a = artifact_id ?? ArtifactId.New();

            return new EventSourceKey(es, a);
        }
    }
}