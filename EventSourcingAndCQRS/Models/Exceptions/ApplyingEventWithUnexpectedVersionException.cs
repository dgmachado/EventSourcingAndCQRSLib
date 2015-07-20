using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingAndCQRS.Models.Exceptions
{
    public class ApplyingEventWithUnexpectedVersionException : Exception
    {
        public EntityId EntityId { get; private set; }
        public int CurrentVersion { get; private set; }
        public int EventVersion { get; private set; }
        public ApplyingEventWithUnexpectedVersionException(EntityId entityId, int currentVersion, int eventVersion) : base() {
            EntityId = entityId;
            CurrentVersion = currentVersion;
            EventVersion = eventVersion;
        }
    }
}
