using Autofac;
using Autofac.Integration.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MQPublisher.Helper
{
    class ContainerHelper
    {
        public static ILifetimeScope GetContainer(HttpContext context)
        {
            // Get the instance of our application
            var application = context.ApplicationInstance as IContainerProviderAccessor;

            // Get the global container from the application
            return application
                    .ContainerProvider
                    .ApplicationContainer;
        }
    }
}
