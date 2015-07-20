using EventSourcingAndCQRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventSourcingAndCQRS.Models.Event
{
    public interface ISnapshot<TEntity> : ISnapshot, IDomainEvent
    {
        TEntity Object { get; set; }
    }

    public class Snapshot<TEntity> : ISnapshot<TEntity>
    {
        public TEntity Object { get; set; }
        public int Version { get; set; }
    }
}
