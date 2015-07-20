using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingAndCQRS.Models.Event
{
    public class MonitoredEvent
    {
        public SourceId SourceId { get; private set;}
        public Type EventType { get; private set; }

        public MonitoredEvent(SourceId sourceId, Type eventType)
        {
            SourceId = sourceId;
            EventType = eventType;
        }
    }
}
