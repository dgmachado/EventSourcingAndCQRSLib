using EventSourcingAndCQRS.Models;
using EventSourcingAndCQRS.Models.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingAndCQRS.Services.Entity
{
    public interface IEntityBuilder<TEntity>
        where TEntity : IEntity, new()
    {
        TEntity BuildFromHistory(IEnumerable<IEvent> entityEvents);
    }
}
