using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingAndCQRS.Models.Event
{
    public interface IEvent
    {
        int Version { get; set; }
    }
}
