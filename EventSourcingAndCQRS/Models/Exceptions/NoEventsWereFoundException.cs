using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingAndCQRS.Models.Exceptions
{
    public class NoEventsWereFoundException : Exception
    {
        public EntityId EntityId { get; private set; }

        public NoEventsWereFoundException(EntityId entityId)
        {
            EntityId = entityId;
        }
    }
}
