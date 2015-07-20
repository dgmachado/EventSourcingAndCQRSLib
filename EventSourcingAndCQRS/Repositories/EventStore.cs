using EventSourcingAndCQRS.Models;
using EventSourcingAndCQRS.Services;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventSourcingAndCQRS;
using EventSourcingAndCQRS.Models.Command;
using EventSourcingAndCQRS.Models.Event;
using EventSourcingAndCQRS.Models.Exceptions;
using EventSourcingAndCQRS.Services.Event;

namespace EventSourcingAndCQRS.Repositories
{
    public abstract class EventStore : IEventStore
    {
        public const int NewEntityVersion = -1;
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private readonly IEventPublisher EventPublisher;

        public EventStore(IEventPublisher eventPublisher)
        {
            EventPublisher = eventPublisher;
        }

        private IEnumerable<IEvent> RewriteVersioningForEvents(int currentEntityVersion, IEnumerable<IEvent> events)
        {
            var currentEventVersion = currentEntityVersion;
            foreach (var evnt in events)
            {
                currentEventVersion++;
                evnt.Version = currentEventVersion;
                yield return evnt;
            }
        }

        private void PublishEvents(IEnumerable<IEvent> events)
        {
            events.ForEach(evnt =>
            {
                EventPublisher.Publish(evnt);
                Log.Debug("Published the event '{0}' entityVersion '{1}'!", evnt.GetType().Name, evnt.Version);
            });
        }

        public void SaveEvents(ICommand command, EntityId entityId, IEnumerable<IEvent> events, int expectedEntityVersion)
        {
            var aggregateRootId = command.AggregateRootId;
            Log.Debug("Starting saving events for aggregate '{0}' entity '{1}' entityVersion '{2}' originated by command '{3}'!", aggregateRootId, entityId, expectedEntityVersion, command.CommandId);
            CheckEntityConsistencyConditions(entityId, expectedEntityVersion);
            CheckIfEventsAreUpdated(entityId, expectedEntityVersion);
            var versionedEvents = RewriteVersioningForEvents(expectedEntityVersion, events);
            SaveEventsOnEventStore(command, entityId, versionedEvents);
            Log.Debug("Stopped saving events for aggregate '{0}' entity '{1}' entityVersion '{2}' originated by command '{3}'!", aggregateRootId, entityId, expectedEntityVersion, command.CommandId);
            PublishEvents(versionedEvents);
        }

        private void CheckEntityConsistencyConditions(EntityId entityId, int expectedEntityVersion)
        {
            CheckIfNewEntityDoesNotHaveAnyAlreadySavedEvent(entityId, expectedEntityVersion);
            CheckIfAlreadySavedEntityHasAnySavedEvent(entityId, expectedEntityVersion);
        }

        private void CheckIfNewEntityDoesNotHaveAnyAlreadySavedEvent(EntityId entityId, int expectedEntityVersion)
        {
            if (EqualsNewEntityVersion(expectedEntityVersion))
            {
                if (HasAnySavedEventFor(entityId))
                {
                    ThrowConcurrencyException(entityId, expectedEntityVersion, CurrentEntityVersion(entityId));
                }
            }
        }

        private bool HasAnySavedEventFor(EntityId entityId)
        {
            return LatestEvents(entityId).Any();
        }

        private void CheckIfAlreadySavedEntityHasAnySavedEvent(EntityId entityId, int expectedEntityVersion)
        {
            if (!EqualsNewEntityVersion(expectedEntityVersion))
            {
                if (!HasAnySavedEventFor(entityId))
                {
                    ThrowConcurrencyException(entityId, expectedEntityVersion, NewEntityVersion);
                }
            }
        }

        private void CheckIfEventsAreUpdated(EntityId entityId, int expectedEntityVersion)
        {
            try
            {
                if (!EqualsNewEntityVersion(expectedEntityVersion))
                {
                    if (!DoesLatestEventVersionMatchVersion(entityId, expectedEntityVersion))
                    {
                        ThrowConcurrencyException(entityId, expectedEntityVersion, CurrentEntityVersion(entityId));
                    }
                }
            }
            catch (NoEventsWereFoundException e)
            { 
                Log.Error("Throwing a NoEventsWereFoundException on saving events for entityId '{0}' expected entity version '{1}'!", entityId, expectedEntityVersion);
                throw e;
            }
            catch (ConcurrencyException e)
            {
                Log.Error("Throwing a ConcurrencyException on saving events for entityId '{0}', expected entity version '{1}' and current entity version '{2}'!", e.EntityId, e.ExpectedEntityVersion, e.CurrentEntityVersion);
                throw e;
            }
            catch (Exception e)
            {
                Log.Error("Throwing a unknown exception on saving events for entityId '{0}' expected entityVersion '{1}'!", entityId, expectedEntityVersion);
                throw e;
            }
        }

        private int CurrentEntityVersion(EntityId entityId)
        {
            return LatestEventsDescendingOrderedByVersion(entityId).First().Version;
        }

        private IOrderedEnumerable<IEvent> LatestEventsDescendingOrderedByVersion(EntityId entityId)
        {
            return LatestEvents(entityId).OrderByDescending(s => s.Version);
        }

        private bool DoesLatestEventVersionMatchVersion(EntityId entityId, int entityVersion)
        {
            var latestEventDescriptorCommitted = LatestEventsDescendingOrderedByVersion(entityId).FirstOrDefault();
            if (latestEventDescriptorCommitted != null)
            {
                return latestEventDescriptorCommitted.Version == entityVersion;
            }
            else
            {
                throw new NoEventsWereFoundException(entityId);
            }
        }

        private bool EqualsNewEntityVersion(int entityVersion)
        {
            return entityVersion == NewEntityVersion;
        }

        public IEnumerable<IEvent> AllEvents(EntityId entityId)
        {
            return LatestEventsSince(entityId, EventStore.NewEntityVersion);
        }

        private void ThrowConcurrencyException(EntityId entityId, int expectedEntityVersion, int currentEntityVersion)
        {
            throw new ConcurrencyException(entityId, expectedEntityVersion, currentEntityVersion);
        }

        protected abstract void SaveEventsOnEventStore(ICommand command, EntityId entityId, IEnumerable<IEvent> versionedEvents);

        public abstract IEnumerable<IEvent> LatestEvents(EntityId entityId);

        public abstract IEnumerable<IEvent> LatestEventsSince(EntityId entityId, int entityVersion);

        public abstract void DeleteEvents(EntityId entityId);

        public abstract void Clear();
    }
}
