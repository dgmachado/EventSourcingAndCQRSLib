using EventSourcingAndCQRS.Models.Event;
using EventSourcingAndCQRS.Repositories;
using EventSourcingAndCQRS.Services.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingAndCQRS.Models
{
    public abstract class Entity : IEntity
    {
        private InMemoryDomainEvents DomainEvents;
        public abstract SourceId SourceId { get; }
        public abstract EntityId EntityId { get; }
        public int Version { get; set; }

        public Entity()
        {
            Version = EventStore.NewEntityVersion;
        }

        public Entity(InMemoryDomainEvents domainEvents) : base()
        {
            WithDomainEvents(domainEvents);
        }

        protected void TryRaiseEvent(IDomainEvent evnt)
        {
            if (DomainEvents != null)
                DomainEvents.Raise(SourceId, evnt);
        }

        public void WithDomainEvents(InMemoryDomainEvents domainEvents)
        {
            DomainEvents = domainEvents;
        }
    }

}
