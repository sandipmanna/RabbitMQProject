using MQPublisher.Event;
using MQPublisher.Interface;
using RabbitMQEvent.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQPublisher.Services
{
    class EventPublisher : IEventPublisher
    {
        private readonly IEventBus _eventBus;
        public EventPublisher(IEventBus eventBus)
        {
            this._eventBus = eventBus;
        }

        public void PublishAuditLogChangedEvent(string data)
        {
            var @event = new AuditLogChangeEvent(data);
            this._eventBus.Publish(@event);
        }
    }
}
