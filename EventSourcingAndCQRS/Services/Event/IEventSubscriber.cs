using EventSourcingAndCQRS.Models;
using EventSourcingAndCQRS.Models.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventSourcingAndCQRS.Services.Event
{
    public interface IEventSubscriber: IDisposable
    {
        void SubscribeTo<TEvent>(Action<IEvent> onEventReceived, SubscriptionId subscriptionId) where TEvent : IEvent;
        void RemoveSubscription<TEvent>(SubscriptionId subscriptionId) where TEvent : IEvent;
        void RemoveSubscriptions(SubscriptionId subscriptionId);
    }
}
