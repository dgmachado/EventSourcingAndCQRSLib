using EventSourcingAndCQRS.Models;
using EventSourcingAndCQRS.Services;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingAndCQRS.InMemory.Services
{
    internal class InMemoryMessageSubscription<TMessage>
    {
        public Type MessageType { get; private set; }
        public SubscriptionId SubscriptionId { get; private set; }
        public Action<TMessage> MessageHandler { get; private set; }

        public InMemoryMessageSubscription(Type messageType, SubscriptionId subscriptionId, Action<TMessage> messageHandler)
        {
            MessageType = messageType;
            SubscriptionId = subscriptionId;
            MessageHandler = messageHandler;
        }
    }

    public class InMemoryMessageRouter<TMessage> : IDisposable
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private readonly Dictionary<Tuple<Type, SubscriptionId>, InMemoryMessageSubscription<TMessage>> ActiveSubscriptions = new Dictionary<Tuple<Type, SubscriptionId>, InMemoryMessageSubscription<TMessage>>();

        public void SubscribeTo(Type type, Action<TMessage> onCommandReceveid, SubscriptionId subscriptionId)
        {
            if (!IsListenningTo(type, subscriptionId))
            {
                SubscriptionFor(type, subscriptionId, onCommandReceveid);
            }
        }

        private InMemoryMessageSubscription<TMessage> SubscriptionFor(Type messageType, SubscriptionId subscriptionId, Action<TMessage> messageHandler)
        {
            var subscription = ExistingSubscriptionFor(messageType, subscriptionId);
            if ((subscription != null) && (subscription.MessageHandler != null)) 
                throw new InvalidOperationException(String.Format("There is already a subscription for message type '{0}' and subscription id {1}", messageType, subscriptionId));

            subscription = NewSubscriptionFor(messageType, subscriptionId, messageHandler);

            return subscription;
        }

        private InMemoryMessageSubscription<TMessage> NewSubscriptionFor(Type messageType, SubscriptionId subscriptionId, Action<TMessage> messageHandler)
        {
            Log.Debug("Acquired lock to ActiveSubscriptions at method NewSubscriptionFor(...)");
            var subscription = NewSubscribeSubscription(messageType, subscriptionId, messageHandler);
            lock (ActiveSubscriptions)
            {
                ActiveSubscriptions[new Tuple<Type, SubscriptionId>(messageType, subscriptionId)] = subscription;
            }
            Log.Debug("Released lock to ActiveSubscriptions at method NewSubscriptionFor(...)");

            return subscription;
        }

        private InMemoryMessageSubscription<TMessage> NewSubscribeSubscription(Type messageType, SubscriptionId subscriptionId, Action<TMessage> messageHandler)
        {
            var subscription = new InMemoryMessageSubscription<TMessage>(messageType, subscriptionId, messageHandler);
            return subscription;
        }

        public bool IsListenningTo(Type messageType, SubscriptionId subscriptionId)
        {
            return ExistingSubscriptionFor(messageType, subscriptionId) != null;
        }

        private InMemoryMessageSubscription<TMessage> ExistingSubscriptionFor(Type messageType, SubscriptionId subscriptionId)
        {
            Log.Debug("Acquired lock to ActiveSubscriptions at method ExistingSubscriptionFor(...)");
            try
            {
                lock (ActiveSubscriptions)
                {
                    InMemoryMessageSubscription<TMessage> subscription = null;
                    ActiveSubscriptions.TryGetValue(new Tuple<Type, SubscriptionId>(messageType, subscriptionId), out subscription);
                    return subscription;
                }
            }
            catch (Exception e)
            {
                Log.Error(String.Format("Exception at method ExistingSubscriptionFor(...) - messageType '{0}' subscription '{1}'", messageType, subscriptionId), e);
                throw e;
            }
            finally
            {
                Log.Debug("Released lock to ActiveSubscriptions at method ExistingSubscriptionFor(...)");
            }
        }
        
        public void RemoveSubscription(Type messageType, SubscriptionId subscriptionId)
        {
            var subscription = ExistingSubscriptionFor(messageType, subscriptionId);
            if (subscription != null)
                RemoveSubscription(subscription);
        }

        private void RemoveSubscription(InMemoryMessageSubscription<TMessage> subscription)
        {
            if (subscription == null)
                return;

            Log.Debug("Acquired lock to ActiveSubscriptions at method RemoveSubscription(...)");
            lock (ActiveSubscriptions)
            {
                ActiveSubscriptions.Remove(new Tuple<Type, SubscriptionId>(subscription.MessageType, subscription.SubscriptionId));
            }
            Log.Debug("Released lock to ActiveSubscriptions at method RemoveSubscription(...)");
        }

        public void RemoveSubscriptions(SubscriptionId subscriptionId)
        {
            Tuple<Type, SubscriptionId>[] subscriptionsSharingTheGivenId;
            Log.Debug("Acquired lock to ActiveSubscriptions at method RemoveSubscriptions(...)");
            lock (ActiveSubscriptions)
            {
                subscriptionsSharingTheGivenId = ActiveSubscriptions.Where(s => s.Key.Item2 == subscriptionId).Select(s => s.Key).ToArray();
            }
            Log.Debug("Released lock to ActiveSubscriptions at method RemoveSubscriptions(...)");
            foreach (var subscription in subscriptionsSharingTheGivenId)
            {
                RemoveSubscription(subscription.Item1, subscription.Item2);
            }
        }

        public void Publish(TMessage message)
        {
            lock (ActiveSubscriptions)
            {
                var messageType = message.GetType();
                foreach (var subscription in ActiveSubscriptions.Values)
                {
                    if (subscription.MessageType == messageType)
                    {
                        AsyncHandlesMessage(message, subscription);
                    }
                }
            }
               
        }    

        private void AsyncHandlesMessage(TMessage message, InMemoryMessageSubscription<TMessage> subscription)
        {
            Task.Factory.StartNew(() =>
            {
                subscription.MessageHandler(message);
            });
        }

        public void Dispose()
        {
            ActiveSubscriptions.Clear();
        }
    }
}
