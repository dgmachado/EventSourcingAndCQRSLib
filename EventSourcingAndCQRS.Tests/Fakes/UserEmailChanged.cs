using EventSourcingAndCQRS.Models;
using EventSourcingAndCQRS.Models.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventSourcingAndCQRS.Tests.Fakes
{
    public class UserEmailChanged : IDomainEvent
    {
        public UserId UserId { get; set; }
        public string UserName { get; set; }
        public string NewUserEmail { get; set; }
        public int Version { get; set; }
    }
}
