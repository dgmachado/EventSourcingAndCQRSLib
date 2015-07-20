using EventSourcingAndCQRS.Models;
using EventSourcingAndCQRS.Services.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingAndCQRS.Tests.Fakes
{
    [EntityBuilder(typeof(UserBuilder))]
    public class User : Entity
    {
        public UserId Id { get; set; }
        public override EntityId EntityId { get { return EntityId.FromString(Id.Value); } }
        public override SourceId SourceId { get { return SourceId.FromString(Id.Value); } }
        public string Name { get; set; }
        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    TryRaiseEvent(new UserEmailChanged() { UserId = Id, UserName = Name, NewUserEmail = _email, Version = Version });
                }
            }
        }

        public User() { }

        public User(InMemoryDomainEvents domainEvents, UserId id, string name, string email) : base(domainEvents)
        {
            Id = id;
            Name = name;
            _email = email;
            TryRaiseEvent(new UserCreated() { UserId = Id, UserName = Name, UserEmail = Email, Version = Version });
        }
    }
}
