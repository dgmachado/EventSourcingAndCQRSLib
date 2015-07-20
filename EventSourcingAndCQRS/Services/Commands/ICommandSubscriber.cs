using EventSourcingAndCQRS.Models;
using EventSourcingAndCQRS.Models.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventSourcingAndCQRS.Services.Commands
{
    public interface ICommandSubscriber : IDisposable
    {
        void SubscribeTo<TCommand>(Action<ICommand> commandHandler, SubscriptionId subscriptionId) where TCommand : ICommand;
        void RemoveSubscription<TCommand>(SubscriptionId subscriptionId) where TCommand : ICommand;
        void RemoveSubscriptions(SubscriptionId subscriptionId);
    }
}
