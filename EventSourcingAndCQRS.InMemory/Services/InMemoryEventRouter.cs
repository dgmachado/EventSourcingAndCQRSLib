using EventSourcingAndCQRS.Models;
using EventSourcingAndCQRS.Models.Event;
using EventSourcingAndCQRS.Services;
using EventSourcingAndCQRS.Services.Event;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingAndCQRS.InMemory.Services
{
    public class InMemoryEventRouter : InMemoryMessageRouter<IEvent>, IEventPublisher, IEventSubscriber
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public void Publish<TEvent>(TEvent evnt) where TEvent : IEvent
        {
            base.Publish(evnt);
        }

        public void SubscribeTo<TEvent>(Action<IEvent> onEventReceveid, SubscriptionId subscriptionId) where TEvent : IEvent
        {
            base.SubscribeTo(typeof(TEvent), onEventReceveid, subscriptionId);
        }

        public void RemoveSubscription<TEvent>(SubscriptionId subscriptionId) where TEvent : IEvent
        {
            base.RemoveSubscription(typeof(TEvent), subscriptionId);
        }
    }
}
