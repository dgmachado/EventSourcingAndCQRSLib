using EventSourcingAndCQRS.Services.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingAndCQRS.Models
{
    public interface IEntity
    {
        SourceId SourceId { get; }
        EntityId EntityId { get; }
        int Version { get; set; }
        void WithDomainEvents(InMemoryDomainEvents domainEvents);
    }

}
