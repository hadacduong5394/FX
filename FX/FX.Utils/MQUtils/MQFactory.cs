using RabbitMQ.Client;
using System;
using System.Configuration;

namespace FX.Utils.MQUtils
{
    public static class MQFactory
    {
        private static string MQHostName = ConfigurationManager.AppSettings["MQHostName"];
        private static string MQUserName = ConfigurationManager.AppSettings["MQUserName"];
        private static string MQPassword = ConfigurationManager.AppSettings["MQPassword"];

        private static ConnectionFactory factory;
        private static IConnection connection;
        private static IModel chanel;
        private static readonly object padlock = new object();

        public static ConnectionFactory Factory
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

        public static IConnection Connection
        {
            get
            {
                if (connection == null)
                {
                    lock (padlock)
                        if (connection == null)
                            connection = Factory.CreateConnection();
                }
                if (!connection.IsOpen)
                {
                    connection.Dispose();
                    connection = Factory.CreateConnection();
                }
                return connection;
            }
        }

        public static IModel Chanel
        {
            get
            {
                if (chanel == null)
                    chanel = Connection.CreateModel();
                if (chanel.IsClosed)
                    chanel = Connection.CreateModel();
                return chanel;
            }
        }
    }
}