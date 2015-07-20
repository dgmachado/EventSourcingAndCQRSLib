using EventSourcingAndCQRS.Models;
using EventSourcingAndCQRS.Models.Command;
using EventSourcingAndCQRS.Models.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingAndCQRS.InMemory.Models
{
    internal class EventDescriptor
    {
        public readonly CommandId CommandId;
        public readonly IEvent EventData;
        public readonly EntityId EntityId;
        public readonly int Version;

        public EventDescriptor(CommandId commandId, EntityId id, IEvent eventData, int entityVersion)
        {
            CommandId = commandId;
            EventData = eventData;
            Version = entityVersion;
            EntityId = id;
        }
    }
}
