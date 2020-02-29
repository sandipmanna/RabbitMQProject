using Autofac;
using MQPublisher.Helper;
using MQPublisher.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MQPublisher.Services
{
    public interface IDataChangesPublisher
    {
        bool PublishAuditLogData(int HHA, int UserID, int ClientID, string Data);
    }

    public class DataChangesPublisher : IDataChangesPublisher
    {
        public bool PublishAuditLogData(int HHA, int UserID, int ClientID, string Data)
        {
            try
            {
                var container = ContainerHelper.GetContainer(HttpContext.Current);

                return container.Resolve<IAuditLogService>().PublishAuditLogData(HHA, UserID, Data);
            }
            catch (Exception)
            {
                throw;
                //return false;
            }
        }
    }
}
