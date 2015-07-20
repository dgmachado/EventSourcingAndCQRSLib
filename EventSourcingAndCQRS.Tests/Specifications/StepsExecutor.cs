using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using SimpleInjector;
using EventSourcingAndCQRS.Tests.Specifications;
using EventSourcingAndCQRS.Tests.Fakes;
using EventSourcingAndCQRS.Services;
using EventSourcingAndCQRS.Repositories;
using EventSourcingAndCQRS.Models;
using EventSourcingAndCQRS.InMemory.Services;
using EventSourcingAndCQRS.InMemory.Repositories;
using System.Threading;
using EventSourcingAndCQRS.Services.Commands;
using EventSourcingAndCQRS.Models.Exceptions;
using EventSourcingAndCQRS.Services.Event;
using EventSourcingAndCQRS.Services.Domain;
using EventSourcingAndCQRS.Models.Event;
using EventSourcingAndCQRS.Models.Command;

namespace EventSourcingAndCQRS.Tests
{
    sealed class StepsExecutor : IDisposable
    {
        private Container Container;
        private CreateUserCommandHandler CreateUserCommandHandler;
        private CreateUserCommand CreateUserCommandReceivedOnHandler;
        private ChangeUserEmailCommandHandler ChangeUserEmailCommandHandler;
        private TakeSnapshotCommandHandler<User> TakeUserSnapshotCommandHandler;
        private List<UserEmailChanged> PublisedUserEmailChangedEvents;
        private UserConsolidator UserConsolidator;
        private IEventStore EventStore;
        private EntityNotFoundException EntityNotFoundException;
        private ConcurrencyException ConcurrencyTransactionException;
        private ICommandPublisher CommandPublisher;
        private ICommandSubscriber CommandSubscriber;
        private IEventPublisher EventPublisher;
        private IEventSubscriber EventSubscriber;
        private DailySummaryAccountsRepository DailySummaryAccountsRepository;
        private TotalNewAccountsPerDaySummarizer TotalNewAccountsPerDaySummarizer;
        private InMemoryDomainEvents DomainEvents;
        private SubscriptionId CreateUserCommandHandlerSubscriptionId;
        private SubscriptionId TakeUserSnapshotCommandHandlerSubscriptionId;
        private SubscriptionId ChangeUserEmailCommandHandlerSubscriptionId;

        public StepsExecutor()
        {
            PublisedUserEmailChangedEvents = new List<UserEmailChanged>();
        }

        internal void InstantiateDomainEventsDispatcher()
        {
            DomainEvents = new InMemoryDomainEvents();
        }

        public void Dispose()
        {
            Container.GetInstance<IEventStore>().Clear();
            PublisedUserEmailChangedEvents.Clear();
            if (CommandSubscriber != null)
            {
                CommandSubscriber.RemoveSubscriptions(CreateUserCommandHandlerSubscriptionId);
                CommandSubscriber.RemoveSubscriptions(TakeUserSnapshotCommandHandlerSubscriptionId);
                CommandSubscriber.RemoveSubscriptions(ChangeUserEmailCommandHandlerSubscriptionId);
            }
        }

        internal void InitializeDependencyResolverContainer()
        {
            Container = new Container();
            RegisterAllDependenciesForInMemoryDependencyResolver(Container);
            Container.Verify();
        }

        private void RegisterAllDependenciesForInMemoryDependencyResolver(SimpleInjector.Container container)
        {
            var inMemoryEventRouter = new InMemoryEventRouter();
            var inMemoryCommandRouter = new InMemoryCommandRouter();
            container.Register<ICommandSubscriber>(() => inMemoryCommandRouter, Lifestyle.Singleton);
            container.Register<ICommandPublisher>(() => inMemoryCommandRouter, Lifestyle.Singleton);
            container.Register<IEventPublisher>(() => inMemoryEventRouter, Lifestyle.Singleton);
            container.Register<IEventSubscriber>(() => inMemoryEventRouter, Lifestyle.Singleton);
            container.Register<IEventStore>(() => new InMemoryEventStore(container.GetInstance<IEventPublisher>()), Lifestyle.Singleton);
        }

        internal void InstantiateCommandSubscriber()
        {
            CommandSubscriber = Container.GetInstance<ICommandSubscriber>();
        }

        internal void InstantiateDailySummaryAccountsRepository()
        {
            DailySummaryAccountsRepository = new DailySummaryAccountsRepository();
        }

        internal void InstantiateTotalNewAccountsPerDaySumarizer()
        {
            TotalNewAccountsPerDaySummarizer = new TotalNewAccountsPerDaySummarizer(DailySummaryAccountsRepository);
        }

        internal void InstantiateCommandPublisher()
        {
            CommandPublisher = Container.GetInstance<ICommandPublisher>();
        }

        internal void InstantiateEventSubscriber()
        {
            EventSubscriber = Container.GetInstance<IEventSubscriber>();
        }

        internal void InstantiateEventPublisher()
        {
            EventPublisher = Container.GetInstance<IEventPublisher>();
        }

        internal void InstantiateCreateUserCommandHandler()
        {
            CreateUserCommandHandler = new CreateUserCommandHandler(DomainEvents, EventStore);
        }

        internal void InstantiateEventStore()
        {
            EventStore = Container.GetInstance<IEventStore>();
        }

        internal void InstantiateUserConsolidator()
        {
            UserConsolidator = new UserConsolidator(EventStore, DomainEvents);
        }

        internal void InstantiateChangeUserEmailCommandHandler()
        {
            ChangeUserEmailCommandHandler = new ChangeUserEmailCommandHandler(DomainEvents, EventStore);
        }

        internal void InstantiateTakeUserSnapshotCommandHandler()
        {
            TakeUserSnapshotCommandHandler = new TakeSnapshotCommandHandler<User>(EventStore);
        }

        internal void InstantiateCreateUserCommandHandlerSubscriptionId()
        {
            CreateUserCommandHandlerSubscriptionId = SubscriptionId.New();
        }

        internal void ConfigureCreateUserCommandHandlerToSubscribeForCreateUserCommandsFromCommandSubscriber()
        {
            CommandSubscriber.SubscribeTo<CreateUserCommand>((command) =>
            {
                CreateUserCommandHandler.Handle((CreateUserCommand)command);
            }, CreateUserCommandHandlerSubscriptionId);
        }

        internal void ConfigureTotalNewAccountsPerDaySumarizerToSubscribeForUserCreatedFromCommandSubscriber()
        {
            TotalNewAccountsPerDaySummarizer.SubscribeToUserCreatedEvents(EventSubscriber);
        }

        internal void InstantiateChangeUserEmailCommandHandlerSubscriptionId()
        {
            ChangeUserEmailCommandHandlerSubscriptionId = SubscriptionId.New();
        }

        internal void ConfigureChangeUserEmailCommandHandlerToSubscribeForCreateUserCommandsFromCommandSubscriber()
        {
            CommandSubscriber.SubscribeTo<ChangeUserEmailCommand>((command) =>
            {
                ChangeUserEmailCommandHandler.Handle((ChangeUserEmailCommand)command);
            }, CreateUserCommandHandlerSubscriptionId);
        }

        internal void InstantiateTakeUserSnapshotCommandHandlerSubscriptionId()
        {
            TakeUserSnapshotCommandHandlerSubscriptionId = SubscriptionId.New();
        }

        internal void ConfigureTakeUserSnapshotCommandHandlerToSubscribeForTakeUserSnapshotCommandsFromCommandSubscriber()
        {
            CommandSubscriber.SubscribeTo<TakeSnapshotCommand>((command) =>
            {
                TakeUserSnapshotCommandHandler.Handle((TakeSnapshotCommand)command);
            }, CreateUserCommandHandlerSubscriptionId);
        }

        internal void BuildAndPublishCreateUserCommandOnCommandPublisherAndWaitForVerification(string userId, string userName, string userEmail, int version)
        {
            CommandPublisher.Publish(BuildCreateUserCommand(userId, userName, userEmail, version));
            WaitForVerification(3);
        }

        internal void BuildAndPublishChangeUserEmailCommandOnCommandPublisherAndWaitForVerification(string userId, string newUserEmail, int version)
        {
            CommandPublisher.Publish(BuildChangeUserEmailCommand(userId, newUserEmail, version));
            WaitForVerification(3);
        }

        internal void BuildAndPublishTakeUserSnapshotCommandOnCommandPublisherAndWaitForVerification(string userId)
        {
            CommandPublisher.Publish(BuildTakeUserSnapshotCommand(userId));
            WaitForVerification(3);
        }

        internal void WaitForVerification(int someSeconds)
        {
            Thread.Sleep(TimeSpan.FromSeconds(someSeconds));
        }

        private CreateUserCommand BuildCreateUserCommand(string userId, string userName, string userEmail, int version)
        {
            return new CreateUserCommand() { UserId = UserId.FromString(userId), UserName = userName, UserEmail = userEmail, Version = version };
        }

        private ChangeUserEmailCommand BuildChangeUserEmailCommand(string userId, string newUserEmail, int version)
        {
            return new ChangeUserEmailCommand() { UserId = UserId.FromString(userId), NewUserEmail = newUserEmail, Version = version };
        }

        private TakeSnapshotCommand BuildTakeUserSnapshotCommand(string userId)
        {
            return new TakeSnapshotCommand() { AggregateRootId = AggregateRootId.FromString(userId), EntityId = EntityId.FromString(userId), AggregateRootType = typeof(User), EntityType = typeof(User) };
        }

        private UserCreated BuildUserCreatedEvent(UserId userId, string userName, string email, int version)
        {
            return new UserCreated() { UserId = userId, UserName = userName, UserEmail = email, Version = version };
        }

        private UserEmailChanged BuildUserEmailChangedEvent(UserId userId, string userName, string newUserEmail, int version)
        {
            return new UserEmailChanged() { UserId = userId, UserName = userName, NewUserEmail = newUserEmail, Version = version };
        }

        internal void SubscribeToCreateUserCommandReceived()
        {
            CreateUserCommandHandler.RegisterObserverForCreateUserCommandReceived(OnCreatUserCommandReceivedOnHandler);
        }

        internal void OnCreatUserCommandReceivedOnHandler(CreateUserCommand createUserCommand)
        {
            CreateUserCommandReceivedOnHandler = createUserCommand;
        }

        internal void SubscribeToUserEmailChangedEvent()
        {
            EventSubscriber.SubscribeTo<UserEmailChanged>((message) => OnUserEmailChangedEventPublished(message as UserEmailChanged), SubscriptionId.FromString("UserEmailChangedEvent"));
        }

        internal void OnUserEmailChangedEventPublished(UserEmailChanged userEmailChanged)
        {
            PublisedUserEmailChangedEvents.Add(userEmailChanged);
        }

        internal void CreateUserCreatedEventAndSaveOnEventStore(string userCreateUserId, string userCreatedUserName, string userCreatedUserEmail, int userCreatedVersion, string createUserUserId, string createUserUserName, string createUserUserEmail, int createUserVersion)
        {
            var createUserCommand = BuildCreateUserCommand(createUserUserId, createUserUserName, createUserUserEmail, createUserVersion);
            var userCreatedEvent = BuildUserCreatedEvent(UserId.FromString(userCreateUserId), userCreatedUserName, userCreatedUserEmail, userCreatedVersion);
            EventStore.SaveEvents(createUserCommand, EntityId.FromString(userCreateUserId), new IEvent[] { userCreatedEvent }, createUserCommand.Version);
        }

        internal void CreateUserEmailChangedEventAndSaveOnEventStore(string userEmailChangedUserId, string userEmailChangedUserName, string userEmailChangedNewUserEmail, int userEmailChangedEventVersion, string createUserUserId, string createUserUserEmail, int createUserVersion)
        {
            var changeUserEmailCommand = BuildChangeUserEmailCommand(createUserUserId, createUserUserEmail, createUserVersion);
            var userEmailChangedEvent = BuildUserEmailChangedEvent(UserId.FromString(userEmailChangedUserId), userEmailChangedUserName, userEmailChangedNewUserEmail, userEmailChangedEventVersion);
            try
            {
                EventStore.SaveEvents(changeUserEmailCommand, EntityId.FromString(userEmailChangedUserId), new IEvent[] { userEmailChangedEvent }, changeUserEmailCommand.Version);
            }
            catch (ConcurrencyException e)
            {
                ConcurrencyTransactionException = e;
            }
        }

        internal User FindUserOnRepository(string userId)
        {
            try
            {
                return UserConsolidator.GetById(EntityId.FromString(userId));
            }
            catch (EntityNotFoundException e)
            {
                EntityNotFoundException = e;
            }

            return null;
        }

        internal void AssertThatCommandHandlerReceivedCreateUserCommand(string userId, string userName, string userEmail, int version)
        {
            CreateUserCommandReceivedOnHandler.Should().NotBeNull();
            CreateUserCommandReceivedOnHandler.UserId.Should().Be(UserId.FromString(userId));
            CreateUserCommandReceivedOnHandler.UserName.Should().Be(userName);
            CreateUserCommandReceivedOnHandler.UserEmail.Should().Be(userEmail);
            CreateUserCommandReceivedOnHandler.Version.Should().Be(version);
        }

        internal void AssertThatUserEmailChangedWasPublished(string userId, string userName, string newUserEmail, int version)
        {
            var userEventChangedEvent = BuildUserEmailChangedEvent(UserId.FromString(userId), userName, newUserEmail, version);
            userEventChangedEvent.Should().NotBeNull();
            var userEmailChangedPublished = PublisedUserEmailChangedEvents.FirstOrDefault(s => s.UserId == userEventChangedEvent.UserId && s.Version == userEventChangedEvent.Version);
            userEmailChangedPublished.Should().NotBeNull();
            AreUserEmailChangeEventsEqual(userEmailChangedPublished, userEventChangedEvent).Should().BeTrue();
        }

        internal void AssertThatUserEmailChangedWasnotPublishedOnEventPublisher(string userId, string userName, string newUserEmail, int version)
        {
            var userEventChangedEvent = BuildUserEmailChangedEvent(UserId.FromString(userId), userName, newUserEmail, version);
            userEventChangedEvent.Should().NotBeNull();
            var userEmailChangedPublished = PublisedUserEmailChangedEvents.FirstOrDefault(s => s.UserId == userEventChangedEvent.UserId && s.Version == userEventChangedEvent.Version);
            if (userEmailChangedPublished != null)
                AreUserEmailChangeEventsEqual(userEmailChangedPublished, userEventChangedEvent).Should().BeFalse();
        }

        internal void AssertThatUserEmailChangedEventWasStoredOnEventStore(string userId, string userName, string newUserEmail, int version)
        {
            var userEventChangedEvent = BuildUserEmailChangedEvent(UserId.FromString(userId), userName, newUserEmail, version);
            userEventChangedEvent.Should().NotBeNull();
            var userEvents = EventStore.AllEvents(EntityId.FromString(userId));
            var userEmailChangedEventWasStoredOnEventStore = userEvents.Any(userEvent => 
                {
                    return IsOfType<UserEmailChanged>(userEvent) && AreUserEmailChangeEventsEqual(userEventChangedEvent, userEvent as UserEmailChanged);
                });
            userEmailChangedEventWasStoredOnEventStore.Should().BeTrue();
        }

        internal void AssertThatUserCreatedEventWasStoredOnEventStore(string userId, string userName, string userEmail, int version)
        {
            var userCreatedEvent = BuildUserCreatedEvent(UserId.FromString(userId), userName, userEmail, version);
            userCreatedEvent.Should().NotBeNull();
            var userEvents = EventStore.AllEvents(EntityId.FromString(userId));
            var userCreatedEventWasStoredOnEventStore = userEvents.Any(userEvent =>
            {
                return IsOfType<UserCreated>(userEvent) && AreUserCreatedEventsEqual(userCreatedEvent, userEvent as UserCreated);
            });
            userCreatedEventWasStoredOnEventStore.Should().BeTrue();
        }

        internal void AssertThatUserEmailChangedEventWasnotStoredOnEventStore(string userId, string userName, string newUserEmail, int version)
        {
            var userEventChangedEvent = BuildUserEmailChangedEvent(UserId.FromString(userId), userName, newUserEmail, version);
            userEventChangedEvent.Should().NotBeNull();
            var userEvents = EventStore.AllEvents(EntityId.FromString(userId));
            var userEmailChangedEventWasStoredOnEventStore = userEvents.Any(userEvent =>
            {
                return IsOfType<UserEmailChanged>(userEvent) && AreUserEmailChangeEventsEqual(userEventChangedEvent, userEvent as UserEmailChanged);
            });
            userEmailChangedEventWasStoredOnEventStore.Should().BeFalse();
        }

        private bool AreUserCreatedEventsEqual(UserCreated oneUserCreatedEvent, UserCreated anotherUserCreatedEvent)
        {
            if (anotherUserCreatedEvent.UserId == oneUserCreatedEvent.UserId)
                if (anotherUserCreatedEvent.UserName == oneUserCreatedEvent.UserName)
                    if (anotherUserCreatedEvent.UserEmail == oneUserCreatedEvent.UserEmail)
                        if (anotherUserCreatedEvent.Version == oneUserCreatedEvent.Version)
                        {
                            return true;
                        }
            return false;
        }

        private bool AreUserEmailChangeEventsEqual(UserEmailChanged oneUserEmailChanged, UserEmailChanged anotherUserEmailChanged)
        {
            if (anotherUserEmailChanged.UserId == oneUserEmailChanged.UserId)
                if (anotherUserEmailChanged.UserName == oneUserEmailChanged.UserName)
                    if (anotherUserEmailChanged.NewUserEmail == oneUserEmailChanged.NewUserEmail)
                        if (anotherUserEmailChanged.Version == oneUserEmailChanged.Version)
                        {
                            return true;
                        }
            return false;
        }

        private bool IsOfType<T>(IEvent userEvent)
        {
            return userEvent.GetType().Equals(typeof(T));
        }

        internal void AssertThatLatestUserEventHasVersion(string userId, int version)
        {
            var id = EntityId.FromString(userId);
            var entityEvents = EventStore.AllEvents(id);
            entityEvents.Should().NotBeNull();
            entityEvents.Should().NotBeEmpty();
            entityEvents.Max(s=> s.Version).Should().Be(version);
        }

        internal void AssertThatUserIsSavedRepository(string userId, string userName, string userEmail, int version)
        {
            var user = FindUserOnRepository(userId);
            user.Should().NotBeNull();
            user.Id.Should().Be(UserId.FromString(userId));
            user.Name.Should().Be(userName);
            user.Email.Should().Be(userEmail);
            user.Version.Should().Be(version);
        }

        internal void AssertThatUserIsnotSavedRepository(string userId, int version)
        {
            var user = FindUserOnRepository(userId);
            if (user != null)
            {
                user.Version.Should().NotBe(version);
            }
        }

        internal void AssertThatEntityNotFoundExceptionWasThrew()
        {
            EntityNotFoundException.Should().NotBeNull();
        }

        internal void AssertThatConcurrencyTransactionExceptionWasThrew(string userId)
        {
            ConcurrencyTransactionException.Should().NotBeNull();
            ConcurrencyTransactionException.EntityId.Should().Be(EntityId.FromString(userId));
        }

        internal void AssertThatUserSnapshotEventWasStoredOnEventStore(string userId, string userName, string userEmail, int version)
        {
            var savedUserSnapshotEvent = BuildUserSnapshotEvent(userId, userName, userEmail, version);
            savedUserSnapshotEvent.Should().NotBeNull();
            var userEvents = EventStore.AllEvents(EntityId.FromString(userId));
            var userCreatedEventWasStoredOnEventStore = userEvents.Any(userSnapshot =>
            {
                return IsOfType<Snapshot<User>>(userSnapshot) && AreUserSnapshotEventsEqual(savedUserSnapshotEvent, userSnapshot as Snapshot<User>);
            });
            userCreatedEventWasStoredOnEventStore.Should().BeTrue();
        }

        private bool AreUserSnapshotEventsEqual(Snapshot<User> oneUserSnapshot, Snapshot<User> anotherUserSnapshot)
        {
            if (anotherUserSnapshot.Object.Id == oneUserSnapshot.Object.Id)
                if (anotherUserSnapshot.Object.Name == oneUserSnapshot.Object.Name)
                    if (anotherUserSnapshot.Object.Email == oneUserSnapshot.Object.Email)
                        if (anotherUserSnapshot.Version == oneUserSnapshot.Version)
                        {
                            return true;
                        }
            return false;
        }

        private Snapshot<User> BuildUserSnapshotEvent(string userId, string userName, string userEmail, int version)
        {
            var user = new User();
            user.Id = UserId.FromString(userId);
            user.Name = userName;
            user.Email = userEmail;
            user.Version = version;
            return new Snapshot<User>() { Object = user, Version = version };
        }

        internal void AssertThatTodayTotalNewAccountsIs(int todayTotalNewAccounts)
        {
            DailySummaryAccountsRepository.FindByDate(DateTime.Today).TotalNewAccountsPerDay.Should().Be(todayTotalNewAccounts);
        }
    }
}
