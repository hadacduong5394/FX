using FX.Utils.Helper;
using RabbitMQ.Client;
using System;

namespace FX.Utils.MQUtils
{
    public class MessageQueues<T>
    {
        public string QueueName { get; set; }

        private MessageQueues()
        {
        }

        public MessageQueues(string queueName)
        {
            this.QueueName = queueName;
        }

        public bool Push(T message)
        {
            try
            {
                using (var channel = MQFactory.Connection.CreateModel())
                {
                    channel.QueueDeclare(queue: QueueName,
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;
                    byte[] body = ByteArrayHelpler.ObjectToByteArray(message);
                    //---------------------------------
                    channel.BasicPublish(exchange: "",
                                         routingKey: QueueName,
                                         basicProperties: properties,
                                         body: body);
                    //--------------------
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool Pull(Func<T, bool> excuteMessage)
        {
            try
            {
                using (var channel = MQFactory.Connection.CreateModel())
                {
                    channel.QueueDeclare(queue: QueueName,
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);
                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                    BasicGetResult result = channel.BasicGet(QueueName, false);
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
            catch
            {
                return false;
            }
        }

        public T Pull()
        {
            using (var channel = MQFactory.Connection.CreateModel())
            {
                channel.QueueDeclare(queue: QueueName,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                BasicGetResult result = channel.BasicGet(QueueName, false);
                if (result != null)
                {
                    T model = ByteArrayHelpler.ByteArrayToObject<T>(result.Body);
                    channel.BasicAck(result.DeliveryTag, false);
                    return model;
                }
            }
            return default(T);
        }

        public bool QueueIsEmpty()
        {
            using (var channel = MQFactory.Connection.CreateModel())
            {
                var count = channel.MessageCount(QueueName);
                if (count == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public uint Count()
        {
            using (var channel = MQFactory.Connection.CreateModel())
            {
                var count = channel.MessageCount(QueueName);
                return count;
            }
        }
    }
}