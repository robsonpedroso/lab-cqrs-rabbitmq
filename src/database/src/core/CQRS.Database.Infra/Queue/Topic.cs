using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.Database.Infra.Queue
{
    public static class Topic
    {
        public static void SendQueue(ConnectionFactory factory, string exchange, string queueName, string jsonBody)
        {
            IConnection connection = factory.CreateConnection();
            IModel channel = connection.CreateModel();

            IBasicProperties properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            SendQueue(channel, properties, exchange, queueName, jsonBody);
        }

        public static void SendQueue(IModel channel, IBasicProperties properties, string exchange, string queueName, string jsonBody)
        {
            byte[] content = Encoding.Default.GetBytes(jsonBody);
            channel.ExchangeDeclare(exchange, ExchangeType.Topic);
            channel.QueueDeclare(queueName, true, false, false, null);
            channel.QueueBind(queueName, exchange, queueName, null);

            channel.BasicPublish(exchange: exchange, routingKey: queueName, basicProperties: properties, body: content);
        }
    }
}
