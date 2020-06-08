using CQRS.Database.Application;
using CQRS.Database.Domain;
using CQRS.Database.Domain.Contracts.Services;
using CQRS.Database.Infra.Providers;
using CQRS.Database.Infra.Repository.Migrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using Tools.Contracts.Repository;
using Tools.WebApi;

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
