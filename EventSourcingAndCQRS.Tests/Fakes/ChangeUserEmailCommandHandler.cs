using EventSourcingAndCQRS.Models;
using EventSourcingAndCQRS.Models.Event;
using EventSourcingAndCQRS.Repositories;
using EventSourcingAndCQRS.Services;
using EventSourcingAndCQRS.Services.Commands;
using EventSourcingAndCQRS.Services.Domain;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace EventSourcingAndCQRS.Tests.Fakes
{
    public class ChangeUserEmailCommandHandler : CommandHandler<ChangeUserEmailCommand, User>
    {
        private Action<ChangeUserEmailCommand> OnChangeUserEmailCommandReceived;
        private ChangeUserEmailCommand ChangeUserEmailCommand { get { return ExecutingCommand; } }
        private User CurrentUser;

        public ChangeUserEmailCommandHandler(InMemoryDomainEvents domainEvents, IEventStore eventStore)
            : base(eventStore, domainEvents)
        {
        }
        
        private void OnCommandReceived(ChangeUserEmailCommand changeUserEmailCommand)
        {
            if (OnChangeUserEmailCommandReceived != null)
            {
                OnChangeUserEmailCommandReceived(changeUserEmailCommand);
            }
        }

        internal void RegisterObserverForChangeUserEmailCommandReceived(Action<ChangeUserEmailCommand> changeUserEmailCommandReceived)
        {
            OnChangeUserEmailCommandReceived = changeUserEmailCommandReceived;
        }

        protected override int TimeToWaitToLaunchedEventsInSeconds
        {
            get { return 1; }
        }

        protected override IEnumerable<MonitoredEvent> MonitoredEventTypes()
        {
            return new MonitoredEvent[] { new MonitoredEvent(SourceId.FromString(ChangeUserEmailCommand.EntityId.Value), typeof(UserEmailChanged)) };
        }

        protected override void PrepareForExecuteCommand()
        {
            CurrentUser = CurrentEntity;
        }

        protected override void ExecuteCommandInSingleThread()
        {
            Log.Info("Starting executing command changeUserEmail for user '{0}'", ChangeUserEmailCommand.UserId);
            OnCommandReceived(ChangeUserEmailCommand);
            CurrentUser.Email = ChangeUserEmailCommand.NewUserEmail;
            Log.Info("Fininshed execute command changeUserEmail for user '{0}'", ChangeUserEmailCommand.UserId);
        }

        protected override void Complete()
        {
            Log.Debug("Completed changeUserEmail command executing for user '{0}'", ChangeUserEmailCommand.UserId);
        }

        protected override void Fail(Exception e)
        {
            Log.Error(String.Format("Failed changeUserEmail command executing for user '{0}'", ChangeUserEmailCommand.UserId), e);
        }
    }
}
