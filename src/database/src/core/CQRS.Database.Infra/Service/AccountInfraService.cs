using CQRS.Database.Domain;
using CQRS.Database.Domain.Contracts.Repository;
using CQRS.Database.Domain.Entities;
using CQRS.Database.Infra.Queue;
using Tools.Utils.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using RabbitMQ.Client;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace CQRS.Database.Infra.Service
{
    public class AccountInfraService: IAccountInfraService
    {
        public IModel channel;
        public Settings settings;
        public IBasicProperties properties;
        public IDistributedCache cache;
        readonly IAccountRepository accountRepository;

        public AccountInfraService(
            IModel channel, 
            IDistributedCache cache, 
            Settings settings,
            IAccountRepository accountRepository
        ) : base()
        {
            this.channel = channel;
            this.settings = settings;

            properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            this.cache = cache;

            this.accountRepository = accountRepository;
        }

        public void Delete(Account entity)
        {
            Topic.SendQueue(channel, properties, settings.RabbitConfig.Exchange, settings.RabbitConfig.Queues.AccountsDelete, entity.Id.ToString());
        }

        public void Delete(Guid id)
        {
            Topic.SendQueue(channel, properties, settings.RabbitConfig.Exchange, settings.RabbitConfig.Queues.AccountsDelete, id.ToString());
        }

        public async Task<Account> Get(Guid id)
        {
            var result = await cache.GetStringAsync(id.AsString());
            if (!string.IsNullOrEmpty(result))
                return result.JsonTo<Account>();

            return await accountRepository.Get(id);
        }

        public async void Save(Account entity)
        {
            Topic.SendQueue(channel, properties, settings.RabbitConfig.Exchange, settings.RabbitConfig.Queues.AccountsSave, JsonSerializer.Serialize(entity));
        }

        public void Update(Account entity)
        {
            Topic.SendQueue(channel, properties, settings.RabbitConfig.Exchange, settings.RabbitConfig.Queues.AccountsSave, JsonSerializer.Serialize(entity));
        }
        
    }
}
