using EventSourcingAndCQRS.Models.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingAndCQRS.Services.Domain
{
    public class InMemoryDomainEvents : InMemoryEventRouter<IDomainEvent>
    { 
    }
}