using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQPublisher.Interface
{
    public interface IAuditLogService
    {
        bool PublishAuditLogData(int HHA, int UserID, string Data);
    }
}
