using EventSourcingAndCQRS.Models;
using EventSourcingAndCQRS.Models.Command;
using EventSourcingAndCQRS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventSourcingAndCQRS.Models.Command
{
    public class TakeSnapshotCommand : ICommand
    {
        public CommandId CommandId { get; set; }
        public AggregateRootId AggregateRootId { get; set;  }
        public Type AggregateRootType { get; set; }
        public EntityId EntityId { get; set; }
        public Type EntityType { get; set; }
        public TimeSpan Created { get; set; }
        public int Version { get; set; }

        public TakeSnapshotCommand()
        {
            CommandId = CommandId.New();
            Created = TimeSpan.FromTicks(DateTime.Now.Ticks);
        }
    }
}
