using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using RabbitMQEvent.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace RabbitMQEvent
{
    public class RabbitMQConnection : IRabbitMQConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly int _retryCount;
        IConnection _connection;
        bool _disposed;

        readonly object sync_root = new object();

        public RabbitMQConnection(IConnectionFactory connectionFactory, int retryCount = 5)
        {
            try
            {
                if (connectionFactory != null)
                    _connectionFactory = connectionFactory;

                _retryCount = retryCount;
            }
            catch (Exception exc)
            {
                Utility.WriteExceptionLog(exc);
            }
        }

        public bool IsConnected
        {
            get
            {
                return _connection != null && _connection.IsOpen && !_disposed;
            }
        }

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }

            return _connection.CreateModel();
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;

            try
            {
                _connection.Dispose();
            }
            catch (IOException exc)
            {
                Utility.WriteExceptionLog(exc);
            }
        }

        public bool TryConnect()
        {
            var ret = false;
            try
            {

                lock (sync_root)
                {
                    //PolicyBuilder 
                    var policy = RetryPolicy.Handle<SocketException>()
                        .Or<BrokerUnreachableException>()
                        .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                        {
                            Utility.WriteExceptionLog(ex);
                        }
                    );

                    policy.Execute(() =>
                    {
                        _connection = _connectionFactory
                              .CreateConnection();
                    });

                    if (IsConnected)
                    {
                        _connection.ConnectionShutdown += OnConnectionShutdown;
                        _connection.CallbackException += OnCallbackException;
                        _connection.ConnectionBlocked += OnConnectionBlocked;

                        Utility.WritemessageToFile("RabbitMQ persistent connection acquired a connection {_connection.Endpoint.HostName} and is subscribed to failure events");
                        ret = true;
                    }
                    else
                    {
                        Utility.WritemessageToFile("FATAL ERROR: RabbitMQ connections could not be created and opened");
                        ret = false;
                    }
                }
            }
            catch (Exception exc)
            {
                Utility.WriteExceptionLog(exc);
            }

            return ret;
        }

        private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed) return;
            Utility.WritemessageToFile("A RabbitMQ connection is shutdown. Trying to re-connect...");

            TryConnect();
        }

        void OnCallbackException(object sender, CallbackExceptionEventArgs e)
        {
            if (_disposed) return;
            Utility.WritemessageToFile("A RabbitMQ connection throw exception. Trying to re-connect...");

            TryConnect();
        }

        void OnConnectionShutdown(object sender, ShutdownEventArgs reason)
        {
            if (_disposed) return;
            Utility.WritemessageToFile("A RabbitMQ connection is on shutdown. Trying to re-connect...");
            TryConnect();
        }
    }
}
