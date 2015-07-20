using EventSourcingAndCQRS.Models;
using EventSourcingAndCQRS.Models.Command;
using EventSourcingAndCQRS.Models.Event;
using EventSourcingAndCQRS.Repositories;
using EventSourcingAndCQRS.Services.Domain;
using EventSourcingAndCQRS.Services.Entity;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace EventSourcingAndCQRS.Services.Commands
{
    public abstract class CommandHandler<TCommand, TEntity>
        where TCommand : ICommand
        where TEntity : IEntity, new()
    {
        protected static readonly Logger Log = LogManager.GetCurrentClassLogger();
        protected InMemoryDomainEvents DomainEvents;
        private EntityEventWriter EntityEventWriter;
        private SubscriptionId CommandSubscriptionId;
        protected TCommand ExecutingCommand { get; private set; }
        private EntityConsolidator<TEntity> EntityConsolidator;
        protected TEntity CurrentEntity { get { return EntityConsolidator.GetById(ExecutingCommand.EntityId); } }

        public CommandHandler(IEventStore eventStore, InMemoryDomainEvents domainEvents)
        {
            DomainEvents = domainEvents;
            EntityEventWriter = new EntityEventWriter(eventStore);
            CommandSubscriptionId = SubscriptionId.New();
            EntityConsolidator = new EntityConsolidator<TEntity>(eventStore, domainEvents);
        }

        public void Handle(TCommand command)
        {
            lock (this)
            {
                try
                {
                    ExecutingCommand = command;
                    var domainEventSubscriptionId = SubscriptionId.New();
                    var sourceEvents = new List<IEvent>();
                    PrepareForExecuteCommand();
                    var monitoredEventTypes = MonitoredEventTypes();
                    RegisterObserverForLaunchedEvents(domainEventSubscriptionId, monitoredEventTypes, (entityPropertyChanged) => { 
                        sourceEvents.Add(entityPropertyChanged);
                    });
                    ExecuteCommandInSingleThread();
                    WaitForVerification(TimeToWaitToLaunchedEventsInSeconds);
                    SaveLaunchedEventsDueToCommandExecution(sourceEvents);
                    UnregisterObserverForLaunchedEvent(domainEventSubscriptionId, monitoredEventTypes);
                    Complete();
                    ExecutingCommand = default(TCommand);
                }
                catch (Exception e)
                {
                    Fail(e);
                }
            }
        }

        internal void WaitForVerification(int someSeconds)
        {
            Thread.Sleep(TimeSpan.FromSeconds(someSeconds));
        }

        private void SaveLaunchedEventsDueToCommandExecution(IEnumerable<IEvent> sourceEvents)
        {
            EntityEventWriter.SaveEvents(ExecutingCommand, ExecutingCommand.EntityId, sourceEvents, ExecutingCommand.Version);
        }

        private void RegisterObserverForLaunchedEvents(SubscriptionId subscriptionId, IEnumerable<MonitoredEvent> observations, Action<IEvent> eventHandler)
        {
            observations.ForEach(observation =>
            {
                DomainEvents.Observe(observation.EventType,
                       (launchedEvent) =>
                       {
                           eventHandler(launchedEvent);
                       }, subscriptionId, observation.SourceId);
            });
            WaitForVerification(1);
        }

        private void UnregisterObserverForLaunchedEvent(SubscriptionId subscriptionId, IEnumerable<MonitoredEvent> observations)
        {
            observations.ForEach(observation => UnregisterObserverForLaunchedEvent(subscriptionId, observation.SourceId, observation.EventType));
        }

        private void UnregisterObserverForLaunchedEvent(SubscriptionId subscriptionId, SourceId sourceId, Type eventType)
        {
            DomainEvents.RemoveObservation(eventType, subscriptionId, sourceId);
        }

        protected abstract IEnumerable<MonitoredEvent> MonitoredEventTypes();

        protected abstract int TimeToWaitToLaunchedEventsInSeconds { get; }

        protected abstract void PrepareForExecuteCommand();

        protected abstract void ExecuteCommandInSingleThread();
        
        /// <summary>
        /// This method is executed when the work is completed with success, nor failed or timed out
        /// </summary>
        /// <remarks>
        /// You should use this method to commit the changes made during the work execution, in case of success
        /// </remarks>
        protected abstract void Complete();

        /// <summary>
        /// This method is executed when the work fails or times out
        /// </summary>
        /// <remarks>
        /// You should use this method to rollback the changes made during the work execution, in case of failure
        /// </remarks>
        protected abstract void Fail(Exception e);

    }
}
