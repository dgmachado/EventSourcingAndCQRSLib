using EventSourcingAndCQRS.Models;
using EventSourcingAndCQRS.Models.Exceptions;
using EventSourcingAndCQRS.Repositories;
using EventSourcingAndCQRS.Services;
using EventSourcingAndCQRS.Services.Domain;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingAndCQRS.Services.Entity
{
    public class EntityConsolidator<TEntity>
        where TEntity : IEntity, new()
    {
        private readonly IEventStore Storage;
        private IEntityBuilder<TEntity> EntityBuilder;
        private InMemoryDomainEvents DomainEvents;

        public EntityConsolidator(IEventStore storage, InMemoryDomainEvents domainEvents)
        {
            EntityBuilder = EntityBuilderActivator.InstantiateEntityBuilderFor<TEntity>();
            DomainEvents = domainEvents;
            Storage = storage;
        }

        public TEntity GetById(EntityId id)
        {
            var entityEvents = Storage.LatestEvents(id);
            if (!entityEvents.Any()) throw new EntityNotFoundException(id);
            var result = EntityBuilder.BuildFromHistory(entityEvents);
            result.WithDomainEvents(DomainEvents);
            return result;
        }
    }
}
