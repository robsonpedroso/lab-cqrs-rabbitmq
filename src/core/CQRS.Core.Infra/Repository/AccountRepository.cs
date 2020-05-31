using CQRS.Core.Domain;
using CQRS.Core.Infra.Queues;
using CQRS.Core.Infra.Repository;
using CQRS.Core.Domain.Contracts.Repository;
using CQRS.Core.Domain.Entities;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CQRS.Core.Infra.Repository
{
    public class AccountRepository : RepositoryBase, IAccountRepository
    {
        public IModel channel;
        public Settings settings;
        public IBasicProperties properties;

        public AccountRepository(IModel channel, Settings settings) : base() {
            this.channel = channel;
            this.settings = settings;

            properties = channel.CreateBasicProperties();
            properties.Persistent = true;
        }

        public void Delete(Account entity)
        {
            Topic.SendQueue(channel, properties, "cqrs", "cqrs.commands.delete", entity.Id.ToString());
        }

        public void Delete(Guid id)
        {
            Topic.SendQueue(channel, properties, "cqrs", "cqrs.commands.delete", id.ToString());
        }

        public Task Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Save(Account entity)
        {
            Topic.SendQueue(channel, properties, "cqrs", "cqrs.commands.save", JsonSerializer.Serialize(entity));
        }

        public void Update(Account entity)
        {
            throw new NotImplementedException();
        }
    }
}