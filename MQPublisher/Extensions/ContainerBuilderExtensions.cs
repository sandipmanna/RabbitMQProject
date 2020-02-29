using Autofac;
using MQPublisher.Interface;
using MQPublisher.Services;
using RabbitMQ.Client;
using RabbitMQEvent;
using RabbitMQEvent.Interface;

namespace MQPublisher.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterEventBus(this ContainerBuilder builder)
        {

            string EnableRabbitMQ = System.Configuration.ConfigurationManager.AppSettings["RabittMQ:EnableRabbitMQ"];
            if (EnableRabbitMQ != "1")
            {
                return builder;
            }
            // Register RabbitMQ Connection
            builder.Register<RabbitMQConnection>(options =>
            {
                ConnectionFactory factory = new ConnectionFactory()
                {
                    HostName = System.Configuration.ConfigurationManager.AppSettings.Get("RabittMQ:EventBusHostName")
                };

                if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings.Get("RabittMQ:EventBusUserName")))
                {
                    factory.UserName = System.Configuration.ConfigurationManager.AppSettings.Get("RabittMQ:EventBusUserName");
                }

                if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings.Get("RabittMQ:EventBusPassword")))
                {
                    factory.Password = System.Configuration.ConfigurationManager.AppSettings.Get("RabittMQ:EventBusPassword");
                }

                int retryCount = 5;
                if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings.Get("RabittMQ:EventBusRetryCount")))
                {
                    retryCount = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("RabittMQ:EventBusRetryCount"));
                }

                return new RabbitMQConnection(factory, retryCount);

            }).As<IRabbitMQConnection>().SingleInstance();

            //Register Event Bus
            builder.Register<EventBus>(sp =>
            {
                string subscriptionClientName = string.Empty;
                IRabbitMQConnection rabbitMQConnection = sp.Resolve<IRabbitMQConnection>();
                ILifetimeScope iLifetimeScope = sp.Resolve<ILifetimeScope>();
                int retryCount = 5;
                if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings.Get("RabittMQ:EventBusRetryCount")))
                {
                    retryCount = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("RabittMQ:EventBusRetryCount"));
                }

                return new EventBus(rabbitMQConnection, iLifetimeScope, subscriptionClientName, retryCount);
            }).As<IEventBus>().SingleInstance();
            return builder;
        }

        public static ContainerBuilder RegisterTypeDependencies(this ContainerBuilder builder)
        {

            // Register all Transient dependencies here
            builder.RegisterType<EventBus>().As<IEventBus>();
            builder.RegisterType<EventPublisher>().As<IEventPublisher>();
            builder.RegisterType<AuditLogEventPublisher>().As<IAuditLogEventPublisher>();
            builder.RegisterType<AuditLogService>().As<IAuditLogService>();
            return builder;
        }
    }
}
