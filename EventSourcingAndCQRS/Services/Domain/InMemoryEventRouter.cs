using EventSourcingAndCQRS.Models;
using EventSourcingAndCQRS.Services;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingAndCQRS.Services.Domain
{
    public class EventObservation<TEvent>
    {
        public Type EventType { get; private set; }
        public SubscriptionId SubscriptionId { get; private set; }
        public Action<TEvent> EventHandler { get; private set; }
        public SourceId SourceId { get; private set; }

        public EventObservation(Type eventType, SubscriptionId subscriptionId, SourceId sourceId, Action<TEvent> eventHandler)
        {
            EventType = eventType;
            SubscriptionId = subscriptionId;
            SourceId = sourceId;
            EventHandler = eventHandler;
        }
    }

    public class InMemoryEventRouter<TEvent> 
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static Lazy<Dictionary<Tuple<Type, SubscriptionId, SourceId>, EventObservation<TEvent>>> _activeObservations = new Lazy<Dictionary<Tuple<Type,SubscriptionId,SourceId>,EventObservation<TEvent>>>();
        private static Dictionary<Tuple<Type, SubscriptionId, SourceId>, EventObservation<TEvent>> ActiveObservations {  get  { return _activeObservations.Value; } }

        public void Observe(IEnumerable<EventObservation<TEvent>> eventObservations)
        {
            eventObservations.ForEach(eventObservation => Observe(eventObservation.EventType, eventObservation.EventHandler, eventObservation.SubscriptionId, eventObservation.SourceId));
        }

        public void Observe(EventObservation<TEvent> eventObservation)
        {
            Observe(eventObservation.EventType, eventObservation.EventHandler, eventObservation.SubscriptionId, eventObservation.SourceId);
        }

        public void Observe(Type eventType, Action<TEvent> onEventLaunched, SubscriptionId subscriptionId, SourceId sourceId)
        {
            if (!IsObserving(eventType, subscriptionId, sourceId))
            {
                Observe(eventType, subscriptionId, sourceId, onEventLaunched);
            }
        }

        private EventObservation<TEvent> Observe(Type eventType, SubscriptionId subscriptionId, SourceId sourceId, Action<TEvent> eventHandler)
        {
            var observation = ExistingObservation(eventType, subscriptionId, sourceId);
            if ((observation != null) && (observation.EventHandler != null)) 
                throw new InvalidOperationException(String.Format("There is already a observation for event type '{0}' and observation id {1} and source id {2}", eventType, subscriptionId, sourceId));

            observation = NewObservation(eventType, subscriptionId, sourceId, eventHandler);

            return observation;
        }

        private EventObservation<TEvent> NewObservation(Type eventType, SubscriptionId subscriptionId, SourceId sourceId, Action<TEvent> eventHandler)
        {
            Log.Debug("Acquired lock to ActiveObservations at method NewSubscriptionFor(...)");
            var observation = new EventObservation<TEvent>(eventType, subscriptionId, sourceId, eventHandler);
            lock (ActiveObservations)
            {
                ActiveObservations[new Tuple<Type, SubscriptionId, SourceId>(eventType, subscriptionId, sourceId)] = observation;
            }
            Log.Debug("Released lock to ActiveObservations at method NewSubscriptionFor(...)");

            return observation;
        }

        public bool IsObserving(Type eventType, SubscriptionId subscriptionId, SourceId sourceId)
        {
            return ExistingObservation(eventType, subscriptionId, sourceId) != null;
        }

        private EventObservation<TEvent> ExistingObservation(Type eventType, SubscriptionId subscriptionId, SourceId sourceId)
        {
            Log.Debug("Acquired lock to ActiveObservations at method ExistingSubscriptionFor(...)");
            try
            {
                lock (ActiveObservations)
                {
                    EventObservation<TEvent> observation = null;
                    ActiveObservations.TryGetValue(new Tuple<Type, SubscriptionId, SourceId>(eventType, subscriptionId, sourceId), out observation);
                    return observation;
                }
            }
            catch (Exception e)
            {
                Log.Error(String.Format("Exception at method ExistingSubscriptionFor(...) - messageType '{0}' subscription '{1}' source {2}", eventType, subscriptionId, sourceId), e);
                throw e;
            }
            finally
            {
                Log.Debug("Released lock to ActiveObservations at method ExistingSubscriptionFor(...)");
            }
        }

        public void RemoveObservation(Type eventType, SubscriptionId subscriptionId, SourceId sourceId)
        {
            var observation = ExistingObservation(eventType, subscriptionId, sourceId);
            if (observation != null)
                RemoveObservation(observation);
        }

        private void RemoveObservation(EventObservation<TEvent> observation)
        {
            if (observation == null)
                return;

            Log.Debug("Acquired lock to ActiveObservations at method RemoveSubscription(...)");
            lock (ActiveObservations)
            {
                ActiveObservations.Remove(new Tuple<Type, SubscriptionId, SourceId>(observation.EventType, observation.SubscriptionId, observation.SourceId));
            }
            Log.Debug("Released lock to ActiveObservations at method RemoveSubscription(...)");
        }

        public void RemoveObservations(SubscriptionId subscriptionId)
        {
            Tuple<Type, SubscriptionId, SourceId>[] subscriptionsSharingTheGivenId;
            Log.Debug("Acquired lock to ActiveObservations at method RemoveSubscriptions(...)");
            lock (ActiveObservations)
            {
                subscriptionsSharingTheGivenId = ActiveObservations.Where(s => s.Key.Item2 == subscriptionId).Select(s => s.Key).ToArray();
            }
            Log.Debug("Released lock to ActiveObservations at method RemoveSubscriptions(...)");
            foreach (var observation in subscriptionsSharingTheGivenId)
            {
                RemoveObservation(observation.Item1, observation.Item2, observation.Item3);
            }
        }

        public void Raise(SourceId sourceId, TEvent evnt)
        {
            lock (ActiveObservations)
            {
                var eventType = evnt.GetType();
                foreach (var observation in ActiveObservations.Values)
                {
                    if (observation.EventType == eventType)
                        if (observation.SourceId == sourceId)
                        {
                            AsyncRaisesEvent(evnt, observation);
                        }
                }
            }
               
        }    

        private void AsyncRaisesEvent(TEvent evnt, EventObservation<TEvent> observation)
        {
            Task.Factory.StartNew(() =>
            {
                observation.EventHandler(evnt);
            });
        }
    }
}
