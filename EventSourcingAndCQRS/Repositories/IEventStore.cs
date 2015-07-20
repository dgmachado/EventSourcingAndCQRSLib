using EventSourcingAndCQRS.Models;
using EventSourcingAndCQRS.Models.Command;
using EventSourcingAndCQRS.Models.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingAndCQRS.Repositories
{
    public interface IEventStore
    {
        void Clear();
        void SaveEvents(ICommand command, EntityId entityId, IEnumerable<IEvent> events, int entityVersion = EventStore.NewEntityVersion);
        IEnumerable<IEvent> AllEvents(EntityId id);
        IEnumerable<IEvent> LatestEventsSince(EntityId entityId, int entityVersion);
        IEnumerable<IEvent> LatestEvents(EntityId id);
        void DeleteEvents(EntityId id);
    }
}
