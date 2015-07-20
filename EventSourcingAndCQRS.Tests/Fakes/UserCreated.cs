using EventSourcingAndCQRS.Models;
using EventSourcingAndCQRS.Models.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventSourcingAndCQRS.Tests.Fakes
{
    public class UserCreated : IDomainEvent
    {
        public UserId UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public int Version { get; set; }
    }
}
