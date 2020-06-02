using CQRS.Core.Domain;
using CQRS.Core.Domain.Entities;
using CQRS.Tools.Utils.Extensions;
using Microsoft.Extensions.Caching.Distributed;
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
        private string queueName;
        private Settings settings = new Settings();
        private readonly IDistributedCache cache;

        public Worker(ILogger<Worker> logger, IDistributedCache _cache)
        {
            _logger = logger;
            queueName = settings.RabbitConfig.Queues.AccountsSave;

            var factory = new ConnectionFactory()
            {
                HostName = settings.RabbitConfig.HostName,
                UserName = settings.RabbitConfig.UserName,
                Password = settings.RabbitConfig.Password,
            };

            IConnection connection = factory.CreateConnection();
            channel = connection.CreateModel();

            cache = _cache;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, ea) =>
            {
                var brokerMessage = Encoding.UTF8.GetString(ea.Body.ToArray());

                Account account = JsonSerializer.Deserialize<Account>(brokerMessage);
                var acc = Save<Account>(account.Id.ToString(), account);
            };

            channel.BasicConsume(queueName, autoAck: true, consumer);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

        public T Save<T>(string cacheKey, T body) where T: class
        {
            var result = cache.GetString(cacheKey);
            if (!string.IsNullOrEmpty(result))
            {
                _logger.LogWarning($"cache encontrado: {cacheKey}");
                cache.SetStringAsync(cacheKey, body.ToJsonString());
                return result.JsonTo<T>();
            }
            else
            {
                _logger.LogInformation($"cache gravado: {cacheKey}");
                cache.SetStringAsync(cacheKey, body.ToJsonString());
                return body;
            }
        }
    }
}
