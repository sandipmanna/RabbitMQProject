using Autofac;
using Autofac.Core;
using Autofac.Integration.Web;
using MQPublisher.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace SenderApplication
{
    public class Global : HttpApplication, IContainerProviderAccessor
    {
        // Provider that holds the application container.
        static IContainerProvider _containerProvider;

        // Instance property that will be used by Autofac HttpModules to resolve and inject dependencies.
        public IContainerProvider ContainerProvider
        {
            get { return _containerProvider; }
        }
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            var EnableRabbitMQ = ConfigurationManager.AppSettings["RabittMQ:EnableRabbitMQ"];
            if (EnableRabbitMQ == "1")
            {

                // Build up application container and register your dependencies.
                ContainerBuilder builder = new ContainerBuilder();

                builder
                    .RegisterTypeDependencies()
                    .RegisterEventBus();


                _containerProvider = new ContainerProvider(builder.Build());

                //_containerProvider.ConfigureEventBusSubscription();
            }
        }
    }
}