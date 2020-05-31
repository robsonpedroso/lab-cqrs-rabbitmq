using CQRS.Core.Domain;
using CQRS.Core.Domain.Entities;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CQRS.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private IModel channel;
        private string queueName = "cqrs.commands";
        private Settings settings = new Settings();

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;

            var factory = new ConnectionFactory()
            {
                HostName = settings.RabbitConfig.HostName,
                UserName = settings.RabbitConfig.UserName,
                Password = settings.RabbitConfig.Password,
            };

            IConnection connection = factory.CreateConnection();
            channel = connection.CreateModel();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, ea) =>
            {
                var brokerMessage = Encoding.UTF8.GetString(ea.Body.ToArray());

                if (ea.RoutingKey == "cqrs.commands.delete")
                    _logger.LogInformation($"Deletar usuário: {brokerMessage}");
                else
                {
                    Account account = JsonSerializer.Deserialize<Account>(brokerMessage);
                    _logger.LogInformation($"Fila: {queueName}; Routing Key; {ea.RoutingKey}; Id: {account.Id}; Nome: {account.Name};  E-mail: {account.Email}; ");
                }

                //channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: true);
            };

            channel.BasicConsume(queueName, autoAck: true, consumer);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
