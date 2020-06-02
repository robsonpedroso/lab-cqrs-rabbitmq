using CQRS.Core.Domain;
using CQRS.Core.Domain.Contracts.Repository;
using CQRS.Core.Domain.Entities;
using CQRS.Core.Infra.Queues;
using CQRS.Tools.Utils.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using RabbitMQ.Client;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace CQRS.Core.Infra.Repository
{
    public class AccountRepository : RepositoryBase, IAccountRepository
    {
        public IModel channel;
        public Settings settings;
        public IBasicProperties properties;
        public IDistributedCache cache;

        public AccountRepository(IModel channel, IDistributedCache cache, Settings settings) : base() {
            this.channel = channel;
            this.settings = settings;

            properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            this.cache = cache;
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
            {
                return result.JsonTo<Account>();
            }
            else
            {
                // listar do banco
            }

            return null;
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