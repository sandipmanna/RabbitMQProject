using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQPublisher.Interface
{
    public interface IEventPublisher
    {
        void PublishAuditLogChangedEvent(string data);
    }
}
