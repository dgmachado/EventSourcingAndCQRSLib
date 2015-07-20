using EventSourcingAndCQRS.Models;
using EventSourcingAndCQRS.Models.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingAndCQRS.InMemory.Models
{
    internal class CommandDescriptor
    {
        public readonly CommandId CommandId;
        public readonly TimeSpan Created;
        public readonly AggregateRootId AggregateRootId;
        public readonly string AggregateRootType;
        public readonly string CommandType;
        public readonly string CommandData;

        public CommandDescriptor(CommandId commandId, AggregateRootId aggregateRootId, TimeSpan created, string aggregateRootType, string commandType, string commandData)
        {
            CommandId = commandId;
            AggregateRootId = aggregateRootId;
            Created = created;
            AggregateRootType = aggregateRootType;
            CommandType = commandType;
            CommandData = commandData;
        }
    }
}
