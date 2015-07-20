using EventSourcingAndCQRS.Models;
using EventSourcingAndCQRS.Models.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventSourcingAndCQRS.Services.Event
{
    public interface IEventPublisher
    {
        void Publish<TEvent>(TEvent evnt) where TEvent : IEvent;
    }
}
