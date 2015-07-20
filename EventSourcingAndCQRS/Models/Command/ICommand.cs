using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingAndCQRS.Models.Command
{
    public interface ICommand
    {
        CommandId CommandId { get; }
        AggregateRootId AggregateRootId { get; }
        Type AggregateRootType { get; }
        EntityId EntityId { get; }
        TimeSpan Created { get; }
        int Version { get; }
    }
}
