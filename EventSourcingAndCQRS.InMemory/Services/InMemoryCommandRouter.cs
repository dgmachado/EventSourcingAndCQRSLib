using EventSourcingAndCQRS.Models;
using EventSourcingAndCQRS.Models.Command;
using EventSourcingAndCQRS.Services;
using EventSourcingAndCQRS.Services.Commands;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingAndCQRS.InMemory.Services
{
    public class InMemoryCommandRouter : InMemoryMessageRouter<ICommand>, ICommandPublisher, ICommandSubscriber
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public void Publish<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            base.Publish(command);
        }

        public void SubscribeTo<TCommand>(Action<ICommand> onCommandReceveid, SubscriptionId subscriptionId)
            where TCommand : ICommand
        {
            base.SubscribeTo(typeof(TCommand), onCommandReceveid, subscriptionId);
        }

        public void RemoveSubscription<TCommand>(SubscriptionId subscriptionId) where TCommand : ICommand
        {
            base.RemoveSubscription(typeof(TCommand), subscriptionId);
        }

        public void Dispose()
        {
            base.Dispose();
        }
    }
}
