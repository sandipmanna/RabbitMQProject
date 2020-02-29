using Autofac;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using RabbitMQEvent.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQEvent
{
    public class EventBus : IEventBus
    {
        const string EXCHANGE_NAME = "main.direct.exchange";
        private readonly IRabbitMQConnection _persistentConnection;
        private readonly ILifetimeScope _autofac;
        private readonly int _retryCount;
        private readonly string _queueName;

        public EventBus(IRabbitMQConnection persistentConnection, /*ILogger<EventBusRabbitMQ> logger, */
            ILifetimeScope autofac, string queueName = null, int retryCount = 5)
        {
            try
            {
                if (persistentConnection != null)
                    _persistentConnection = persistentConnection;
                _queueName = queueName;
                _autofac = autofac;
                _retryCount = retryCount;
            }
            catch (Exception)
            {
                //TODO: log exception here
            }
        }

        public void Publish(IntegrationEvent @event)  //, Action<IntegrationEvent> callback=null
        {
            try
            {
                if (!_persistentConnection.IsConnected)
                {
                    _persistentConnection.TryConnect();
                }

                var policy = RetryPolicy.Handle<BrokerUnreachableException>()
                    .Or<SocketException>()
                    .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                    {
                        //_logger.LogWarning(ex.ToString());
                    });

                using (var channel = _persistentConnection.CreateModel())
                {
                    var eventName = @event.GetType().Name;

                    channel.ExchangeDeclare(exchange: EXCHANGE_NAME, type: "direct");

                    channel.CallbackException += (sender, ea) =>
                    {
                        //TODO: write logs for call failure here
                    };

                    channel.BasicAcks += (sender, args) =>
                    {
                        //callback?.Invoke(@event);
                    };

                    channel.BasicNacks += (sender, args) =>
                    {

                    };

                    var message = JsonConvert.SerializeObject(@event);
                    var body = Encoding.UTF8.GetBytes(message);
                    policy.Execute(() =>
                    {
                        var properties = channel.CreateBasicProperties();
                        properties.DeliveryMode = 2; // persistent
                        channel.ConfirmSelect();
                        channel.BasicPublish(exchange: EXCHANGE_NAME,
                                         routingKey: eventName,
                                         mandatory: true,
                                         basicProperties: properties,
                                         body: body);

                        channel.WaitForConfirmsOrDie();
                    });
                }
            }
            catch (Exception)
            {
                //TODO: log exception here
            }
        }

        private void Channel_BasicAcks(object sender, BasicAckEventArgs e)
        {

        }
    }
}
