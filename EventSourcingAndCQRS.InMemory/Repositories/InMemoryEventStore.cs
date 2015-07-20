using EventSourcingAndCQRS.InMemory.Models;
using EventSourcingAndCQRS.Models;
using EventSourcingAndCQRS.Models.Command;
using EventSourcingAndCQRS.Models.Event;
using EventSourcingAndCQRS.Models.Exceptions;
using EventSourcingAndCQRS.Repositories;
using EventSourcingAndCQRS.Services;
using EventSourcingAndCQRS.Services.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingAndCQRS.InMemory.Repositories
{
    public class InMemoryEventStore : EventStore
    {
        private readonly Dictionary<EntityId, List<EventDescriptor>> EventDescriptors = new Dictionary<EntityId, List<EventDescriptor>>();
        private readonly Dictionary<AggregateRootId, List<CommandDescriptor>> CommandDescriptors = new Dictionary<AggregateRootId, List<CommandDescriptor>>();

        public InMemoryEventStore(IEventPublisher eventPublisher)
            : base(eventPublisher)
        {
        }

        protected override void SaveEventsOnEventStore(ICommand command, EntityId entityId, IEnumerable<IEvent> versionedEvents)
        {
            var aggregateRootId = command.AggregateRootId;
            var eventDescriptors = versionedEvents.Select(evnt => BuildEventDescriptor(command.CommandId, entityId, evnt));
            CommandDescriptorsFor(aggregateRootId).Add(new CommandDescriptor(command.CommandId, aggregateRootId, command.Created, AggregateRootType(aggregateRootId), CommandType(command), command.ToString()));
            EventDescriptorsForEntity(entityId).AddRange(eventDescriptors);
        }

        private string CommandType(ICommand command)
        {
            return command.GetType().AssemblyQualifiedName;
        }

        private string AggregateRootType(AggregateRootId aggregateRootId)
        {
            return aggregateRootId.GetType().AssemblyQualifiedName;
        }

        private List<CommandDescriptor> CommandDescriptorsFor(AggregateRootId aggregateRootId)
        {
            if (CommandDescriptors.Keys.Any(s => s == aggregateRootId))
            {
                return CommandDescriptors[aggregateRootId];
            }
            else
            {
                var commandDescriptors = new List<CommandDescriptor>();
                CommandDescriptors.Add(aggregateRootId, commandDescriptors);
                return commandDescriptors;
            }
        }

        private List<EventDescriptor> EventDescriptorsForEntity(EntityId id)
        {
            if (EventDescriptors.Keys.Any(s=> s == id))
            {
                return EventDescriptors[id];
            }
            else
            {
                var eventDescriptors = new List<EventDescriptor>();
                EventDescriptors.Add(id, eventDescriptors);
                return eventDescriptors;
            }
        }

        private EventDescriptor BuildEventDescriptor(CommandId commandId, EntityId entityId, IEvent evnt)
        {
            return new EventDescriptor(commandId, entityId, evnt, evnt.Version);
        }

        public override IEnumerable<IEvent> LatestEvents(EntityId entityId)
        {
            var eventDescriptors = EventDescriptorsForEntity(entityId);
            if (eventDescriptors.Any())
            {
                var allEvents = eventDescriptors.Select(desc => desc.EventData).ToArray();
                var latestEventsOrderedByVersion = allEvents.OrderByDescending(s => s.Version);
                var snapshotType = typeof(ISnapshot);
                var latestSnapshot = latestEventsOrderedByVersion.SkipWhile(s => !snapshotType.IsAssignableFrom(s.GetType()));
                var snapshot = latestSnapshot.FirstOrDefault();
                if (snapshot != null)
                {
                    return new IEvent[] { snapshot }.Union(LatestEventsSince(entityId, snapshot.Version));
                }
                else
                {
                    return AllEvents(entityId);
                }
            }
            else
            {
                return new IEvent[] { };
            }
        }

        public override IEnumerable<IEvent> LatestEventsSince(EntityId entityId, int entityVersion)
        {
            var eventDescriptors = EventDescriptorsForEntity(entityId);
            if (eventDescriptors.Any())
            {
                var allEvents = eventDescriptors.Select(desc => desc.EventData).ToArray();
                var latestEventsOrderedByVersion = allEvents.OrderByDescending(s => s.Version);
                return latestEventsOrderedByVersion.Where(s=> s.Version > entityVersion);
            }
            else
            {
                return new IEvent[] { };
            }
        }

        public override void DeleteEvents(EntityId entityId)
        {
            if (EventDescriptorsForEntity(entityId).Any())
            {
                EventDescriptors.Remove(entityId);
            }
            else
            {
                throw new EntityNotFoundException(entityId);
            }
        }

        public override void Clear()
        {
            CommandDescriptors.Clear();
            EventDescriptors.Clear();
        }
    }
}
