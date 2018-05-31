using FX.Core.Utils;
using log4net;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Configuration;

namespace FX.Utils.MQUtils
{
    public static class MessageQueues<T>
    {
        private static ILog log = LogManager.GetLogger("MessageQueues");

        private static string MQHostName = ConfigurationManager.AppSettings["MQHostName"];
        private static string MQUserName = ConfigurationManager.AppSettings["MQUserName"];
        private static string MQPassword = ConfigurationManager.AppSettings["MQPassword"];
        private static string MQQueueName = ConfigurationManager.AppSettings["MQQueueName"];

        private static ConnectionFactory factory;
        private static IConnection connection;
        private static IModel chanel;
        private static readonly object padlock = new object();

        public static ConnectionFactory MQFactory
        {
            get
            {
                if (factory == null)
                {
                    lock (padlock)
                    {
                        if (factory == null)
                        {
                            factory = new ConnectionFactory() { HostName = MQHostName, UserName = MQUserName, Password = MQPassword };
                            // attempt recovery every 1 seconds
                            factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(1);
                            factory.AutomaticRecoveryEnabled = true;
                        }
                    }
                }
                return factory;
            }
        }

        public static IConnection MQConnection
        {
            get
            {
                if (connection == null)
                {
                    lock (padlock)
                        if (connection == null)
                            connection = MQFactory.CreateConnection();
                }
                if (!connection.IsOpen)
                {
                    connection.Dispose();
                    connection = MQFactory.CreateConnection();
                }
                return connection;
            }
        }

        public static IModel MQChanel
        {
            get
            {
                if (chanel == null)
                    chanel = MQConnection.CreateModel();
                if (chanel.IsClosed)
                    chanel = MQConnection.CreateModel();
                return chanel;
            }
        }

        public static bool Push(T message)
        {
            try
            {
                using (var channel = MQConnection.CreateModel())
                {
                    channel.QueueDeclare(queue: MQQueueName,
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;
                    byte[] body = ByteArrayHelpler.ObjectToByteArray(message);
                    //---------------------------------
                    channel.BasicPublish(exchange: "",
                                         routingKey: MQQueueName,
                                         basicProperties: properties,
                                         body: body);
                    //--------------------
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        public static bool Pull(IModel channel, Func<T, bool> excuteMessage)
        {
            BasicGetResult result = channel.BasicGet(MQQueueName, false);
            if (result != null)
            {
                T msg = ByteArrayHelpler.ByteArrayToObject<T>(result.Body);
                if (excuteMessage(msg))
                {
                    // đánh dấu đã đọc msg này
                    channel.BasicAck(result.DeliveryTag, false);
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        public static bool Pull(Func<T, bool> excuteMessage)
        {
            try
            {
                using (var channel = MQConnection.CreateModel())
                {
                    channel.QueueDeclare(queue: MQQueueName,
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);
                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                    BasicGetResult result = channel.BasicGet(MQQueueName, false);
                    if (result != null)
                    {
                        T msg = ByteArrayHelpler.ByteArrayToObject<T>(result.Body);
                        if (excuteMessage(msg))
                        {
                            // đánh dấu đã đọc msg này
                            channel.BasicAck(result.DeliveryTag, false);
                            return true;
                        }
                        else
                            return false;
                    }
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return false;
        }

        public static T Pull()
        {
            try
            {
                using (var channel = MQConnection.CreateModel())
                {
                    channel.QueueDeclare(queue: MQQueueName,
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);
                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                    BasicGetResult result = channel.BasicGet(MQQueueName, false);
                    if (result != null)
                    {
                        T model = ByteArrayHelpler.ByteArrayToObject<T>(result.Body);
                        channel.BasicAck(result.DeliveryTag, false);
                        return model;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return default(T);
        }

        public static bool QueueIsEmpty()
        {
            using (var channel = MQConnection.CreateModel())
            {
                var count = channel.MessageCount(MQQueueName);
                if (count == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool Consumer(Func<T, bool> excuteMessage)
        {
            try
            {
                using (var channel = MQConnection.CreateModel())
                {
                    channel.QueueDeclare(queue: MQQueueName,
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);
                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (ch, ea) =>
                    {
                        var body = ea.Body;
                        // ... process the message
                        T msg = ByteArrayHelpler.ByteArrayToObject<T>(body);
                        if (excuteMessage(msg))
                            channel.BasicAck(ea.DeliveryTag, false);
                    };
                    String consumerTag = channel.BasicConsume(MQQueueName, false, consumer);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return false;
        }
    }
}