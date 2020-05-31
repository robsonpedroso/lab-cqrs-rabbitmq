using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace CQRS.Core.Infra.Queues
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
            channel.BasicPublish(exchange: exchange, routingKey: queueName, basicProperties: properties, body: content);
        }
    }
}
