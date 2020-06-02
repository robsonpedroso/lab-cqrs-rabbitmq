using CQRS.Core.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CQRS.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) {
            var settings = new Settings();

            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDistributedRedisCache(options =>
                    {
                        options.Configuration = settings.RedisConfig.HostName;
                        options.InstanceName = settings.RedisConfig.InstanceName;
                    });

                    services.AddHostedService<Worker>();
                });
        }
    }
}
