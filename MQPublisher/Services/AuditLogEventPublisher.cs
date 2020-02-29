using MQPublisher.Event;
using RabbitMQEvent.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQPublisher.Services
{
    public interface IAuditLogEventPublisher
    {
        bool PublishAuditLogEvent(int HHA, int UserID, string Data);
    }
    public class AuditLogEventPublisher : IAuditLogEventPublisher
    {
        private readonly IEventBus _eventBus;
        public AuditLogEventPublisher(IEventBus eventBus) => _eventBus = eventBus;
        public bool PublishAuditLogEvent(int HHA, int UserID, string Data)
        {
            try
            {
                AuditLogChangeEvent @event = new AuditLogChangeEvent(Data)
                {
                    HHA = HHA,
                    UserId = UserID
                };

                _eventBus.Publish(@event);
            }
            catch (Exception)
            {
                throw;
            }
            return true;
        }
    }
}
