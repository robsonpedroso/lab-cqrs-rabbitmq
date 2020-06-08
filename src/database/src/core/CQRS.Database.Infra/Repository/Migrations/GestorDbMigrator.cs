using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CQRS.Database.Infra.Repository.Migrations
{
    public class GestorDbMigrator
    {
        public static void RunMigrationDown(IConfiguration configuration)
        {
            var service = CreateService(configuration);

            using (var scope = service.CreateScope())
            {
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

                runner.MigrateDown(0);
            }
        }

        public static void RunMigrationUp(IConfiguration configuration)
        {
            var service = CreateService(configuration);

            using (var scope = service.CreateScope())
            {
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

                runner.MigrateUp();
            }
        }

        private static ServiceProvider CreateService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("CQRSConn");

            var service = new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(r => r
                    .AddSqlServer()
                    .WithGlobalConnectionString(connectionString)
                    .WithMigrationsIn(typeof(GestorDbMigrator).Assembly)
                    .WithVersionTable(new VersionTable(configuration))
                )
                .Configure<RunnerOptions>(opt =>
                {
                    opt.Tags = new[] { "CQRS" };
                })
                .BuildServiceProvider(true);

            return service;
        }
    }
}
