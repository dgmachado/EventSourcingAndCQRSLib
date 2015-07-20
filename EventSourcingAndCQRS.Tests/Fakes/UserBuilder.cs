using EventSourcingAndCQRS.Models;
using EventSourcingAndCQRS.Models.Event;
using EventSourcingAndCQRS.Services;
using EventSourcingAndCQRS.Services.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingAndCQRS.Tests.Fakes
{
    internal class UserBuilder : EntityBuilder<User>
    {
        private void Apply(User user, UserCreated userCreated)
        {
            user.Id = userCreated.UserId;
            user.Name = userCreated.UserName;
            user.Email = userCreated.UserEmail;
        }

        private void Apply(User user, UserEmailChanged userEmailChanged)
        {
            user.Email = userEmailChanged.NewUserEmail;
        }

        protected override void Apply(User user, ISnapshot<User> snapshot)
        {
            user.Id = snapshot.Object.Id;
            user.Name = snapshot.Object.Name;
            user.Email = snapshot.Object.Email;
        }
    }
}
