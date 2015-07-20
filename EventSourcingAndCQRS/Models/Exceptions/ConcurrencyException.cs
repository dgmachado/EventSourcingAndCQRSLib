using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingAndCQRS.Models.Exceptions
{
    public class ConcurrencyException : Exception
    {
        public int ExpectedEntityVersion { get; private set; }
        public int CurrentEntityVersion { get; private set; }
        public EntityId EntityId { get; private set; }

        public ConcurrencyException(EntityId entityId, int expectedEntityVersion, int currentEntityVersion) : base() {
            EntityId = entityId;
            ExpectedEntityVersion = expectedEntityVersion;
            CurrentEntityVersion = currentEntityVersion;
        }
    }
}
