using EventSourcingAndCQRS.Models;
using EventSourcingAndCQRS.Models.Command;
using EventSourcingAndCQRS.Models.Event;
using EventSourcingAndCQRS.Repositories;
using EventSourcingAndCQRS.Services;
using EventSourcingAndCQRS.Services.Domain;
using EventSourcingAndCQRS.Services.Entity;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace EventSourcingAndCQRS.Services.Commands
{
    public class TakeSnapshotCommandHandler<TEntity>
            where TEntity : IEntity, new()
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private readonly EntityEventWriter EntityEventWriter;
        private IEventStore EventStore;

        public TakeSnapshotCommandHandler(IEventStore eventStore)
        {
            EntityEventWriter = new EntityEventWriter(eventStore);
            EventStore = eventStore;
        }

        public void Handle(TakeSnapshotCommand takeSnapshotCommand)
        {
            Log.Info("Starting executing command takeSnapshot for aggregate '{0}' of id '{1}'", takeSnapshotCommand.AggregateRootType, takeSnapshotCommand.AggregateRootId);
            var entity = BuildEntityConsolidator().GetById(takeSnapshotCommand.EntityId);
            RewriteSnapshotVersion(takeSnapshotCommand, entity.Version);
            SaveSnapshot(takeSnapshotCommand, BuildSnapshotEvent(entity));
            Log.Info("Finished execute command takeSnapshot for aggregate '{0}' of id '{1}'", takeSnapshotCommand.AggregateRootType, takeSnapshotCommand.AggregateRootId);
        }

        private Snapshot<TEntity> BuildSnapshotEvent(TEntity entity)
        {
            return new Snapshot<TEntity>() { Object = entity, Version = entity.Version };
        }

        private void RewriteSnapshotVersion(TakeSnapshotCommand takeSnapshotCommand, int version)
        {
            takeSnapshotCommand.Version = version;
        }

        private EntityConsolidator<TEntity> BuildEntityConsolidator()
        {
            return new EntityConsolidator<TEntity>(EventStore, new InMemoryDomainEvents());
        }

        private void SaveSnapshot(ICommand command, ISnapshot snapshot)
        {
            EntityEventWriter.SaveEvents(command, command.EntityId, new IEvent[] { snapshot }, command.Version);
        }
    }
}
