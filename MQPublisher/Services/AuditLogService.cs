using MQPublisher.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQPublisher.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IAuditLogEventPublisher _auditLogEventPublisher;

        public AuditLogService(IAuditLogEventPublisher auditLogEventPublisher)
        {
            _auditLogEventPublisher = auditLogEventPublisher;
        }

        public bool PublishAuditLogData(int HHA, int UserID, string Data)
        {
            _auditLogEventPublisher.PublishAuditLogEvent(HHA, UserID, Data);
            return true;
        }
    }
}
