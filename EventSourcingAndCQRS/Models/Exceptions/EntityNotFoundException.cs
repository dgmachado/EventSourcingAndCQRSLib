using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingAndCQRS.Models.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityId EntityId { get; private set; }

        public EntityNotFoundException(EntityId entityId)
        {
            EntityId = entityId;
        }
    }
}
