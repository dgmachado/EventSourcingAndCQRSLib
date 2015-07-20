using EventSourcingAndCQRS.Models;
using EventSourcingAndCQRS.Models.Command;
using EventSourcingAndCQRS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventSourcingAndCQRS.Tests.Fakes
{
    public class CreateUserCommand : ICommand
    {
        public UserId UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public CommandId CommandId { get; private set; }
        public AggregateRootId AggregateRootId { get { return AggregateRootId.FromString(UserId.Value); } }
        public Type AggregateRootType {  get { return typeof(User); } }
        public EntityId EntityId { get { return EntityId.FromString(UserId.Value); } }
        public TimeSpan Created { get; set; }
        public int Version { get; set; }

        public CreateUserCommand()
        {
            CommandId = CommandId.New();
            Created = TimeSpan.FromTicks(DateTime.Now.Ticks);
        }
    }
}
