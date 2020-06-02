namespace CQRS.Core.Domain
{
    public class Settings
    {
        public RabbitConfig RabbitConfig { get; set; }
        public RedisConfig RedisConfig { get; set; }

        public Settings()
        {
            RabbitConfig = new RabbitConfig();
            RedisConfig = new RedisConfig();
        }
    }

    public class RabbitConfig
    {
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Exchange { get; set; }
        public RabbitQueue Queues { get; set; }

        public RabbitConfig()
        {
            this.HostName = "192.168.33.10";
            this.UserName = "admin";
            this.Password = "123";
            this.Exchange = "cqrs";
            this.Queues = new RabbitQueue();
        }
    }

    public class RabbitQueue
    {
        public string AccountsDelete { get; set; }
        public string AccountsSave { get; set; }
        public string ProductsDelete { get; set; }
        public string ProductsSave { get; set; }

        public RabbitQueue()
        {
            this.AccountsDelete = "cqrs.accounts.delete";
            this.AccountsSave = "cqrs.accounts.save";

            this.ProductsDelete = "cqrs.products.delete";
            this.ProductsSave = "cqrs.products.save";
        }
    }

    public class RedisConfig
    {
        public string HostName { get; set; }
        public string InstanceName { get; set; }

        public RedisConfig()
        {
            this.HostName = "192.168.33.10";
            this.InstanceName = "master";
        }
    }
}
