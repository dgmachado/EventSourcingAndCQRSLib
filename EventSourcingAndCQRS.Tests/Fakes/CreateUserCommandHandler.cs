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

namespace EventSourcingAndCQRS.Tests.Fakes
{
    public class CreateUserCommandHandler : CommandHandler<CreateUserCommand, User>
    {
        private Action<CreateUserCommand> OnCreatUserCommandReceived;
        private CreateUserCommand CreateUserCommand { get { return ExecutingCommand; } }
        public CreateUserCommandHandler(InMemoryDomainEvents domainEvents, IEventStore eventStore)
            : base(eventStore, domainEvents)
        {
        }

        private void OnCommandExecuted(CreateUserCommand createUserCommand)
        {
            if (OnCreatUserCommandReceived != null)
            {
                OnCreatUserCommandReceived(createUserCommand);
            }
        }

        internal void RegisterObserverForCreateUserCommandReceived(Action<CreateUserCommand> creatUserCommandReceived)
        {
            OnCreatUserCommandReceived = creatUserCommandReceived;
        }

        protected override void ExecuteCommandInSingleThread()
        {
            Log.Debug("Starting executing command createUser for user '{0}'", CreateUserCommand.UserId);
            var newUser = new User(DomainEvents, CreateUserCommand.UserId, CreateUserCommand.UserName, CreateUserCommand.UserEmail);
            OnCommandExecuted(CreateUserCommand);
            Log.Debug("Finished create command createUser for user '{0}'", CreateUserCommand.UserId);
        }

        protected override int TimeToWaitToLaunchedEventsInSeconds
        {
            get { return 1; }
        }

        protected override IEnumerable<MonitoredEvent> MonitoredEventTypes()
        {
            return new MonitoredEvent[] { new MonitoredEvent(SourceId.FromString(CreateUserCommand.EntityId.Value), typeof(UserCreated)) };
        }
        
        protected override void PrepareForExecuteCommand()
        {
        }

        protected override void Complete()
        {
            Log.Debug("Completed createUser command executing for user '{0}'", CreateUserCommand.UserId);
        }

        protected override void Fail(Exception e)
        {
            Log.Error(String.Format("Failed createUser command executing for user '{0}'", CreateUserCommand.UserId), e);
        }
    }
}
