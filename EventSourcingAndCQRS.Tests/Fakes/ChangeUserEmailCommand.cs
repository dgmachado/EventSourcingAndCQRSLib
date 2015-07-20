using EventSourcingAndCQRS.Models;
using EventSourcingAndCQRS.Models.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventSourcingAndCQRS.Tests.Fakes
{
    public class ChangeUserEmailCommand : ICommand
    {
        public UserId UserId { get; set; }
        public string NewUserEmail { get; set; }
        public CommandId CommandId { get; set; }
        public AggregateRootId AggregateRootId { get { return AggregateRootId.FromString(UserId.Value); } }
        public Type AggregateRootType { get { return typeof(User); } }
        public EntityId EntityId { get { return EntityId.FromString(UserId.Value); } }
        public TimeSpan Created { get; set; }
        public int Version { get; set; }

        public ChangeUserEmailCommand()
        {
            CommandId = CommandId.New();
            Created = TimeSpan.FromTicks(DateTime.Now.Ticks);
        }
    }
}
