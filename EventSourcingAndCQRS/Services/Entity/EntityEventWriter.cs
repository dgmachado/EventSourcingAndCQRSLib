using EventSourcingAndCQRS.Models;
using EventSourcingAndCQRS.Models.Command;
using EventSourcingAndCQRS.Models.Event;
using EventSourcingAndCQRS.Repositories;
using EventSourcingAndCQRS.Services;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingAndCQRS.Services.Entity
{
    public class EntityEventWriter
    {
        private readonly IEventStore Storage;

        public EntityEventWriter(IEventStore storage)
        {
            Storage = storage;
        }

        public void SaveEvents(ICommand command, EntityId entityId, IEnumerable<IEvent> events, int entityVersion = EventStore.NewEntityVersion)
        {
            Storage.SaveEvents(command, entityId, events, entityVersion);
        }
    }
}
