using EventSourcingAndCQRS.Models;
using EventSourcingAndCQRS.Repositories;
using EventSourcingAndCQRS.Services;
using EventSourcingAndCQRS.Services.Domain;
using EventSourcingAndCQRS.Services.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingAndCQRS.Tests.Fakes
{
    public class UserConsolidator : EntityConsolidator<User> 
    {
        public UserConsolidator(IEventStore storage, InMemoryDomainEvents domainEvents)
            : base(storage, domainEvents)
        {
        }
    }
}
