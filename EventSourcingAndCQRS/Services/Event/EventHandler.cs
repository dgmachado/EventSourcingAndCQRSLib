using EventSourcingAndCQRS.Models;
using EventSourcingAndCQRS.Models.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingAndCQRS.Services.Event
{
    public abstract class EventHandler<TEvent> 
        where TEvent : class, IEvent
    {
        private SubscriptionId SubscriptionId;

        public EventHandler(SubscriptionId subscriptionId)
        {
            SubscriptionId = subscriptionId;
        }

        public void SubscribeToUserCreatedEvents(IEventSubscriber eventSubscriber)
        {
            eventSubscriber.SubscribeTo<TEvent>((message) => Handle(message as TEvent), SubscriptionId);
        }

        protected abstract void Handle(TEvent evnt);
    }
}
