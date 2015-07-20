using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingAndCQRS.Models.Event
{
    public interface ISnapshot : IEvent
    {
    }
}
