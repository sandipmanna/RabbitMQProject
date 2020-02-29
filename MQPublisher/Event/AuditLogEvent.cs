using RabbitMQEvent.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQPublisher.Event
{
    class AuditLogChangeEvent : IntegrationEvent
    {
        public string Payload { get; private set; }

        public AuditLogChangeEvent(string payload)
        {
            this.Payload = payload;
        }
    }
}
