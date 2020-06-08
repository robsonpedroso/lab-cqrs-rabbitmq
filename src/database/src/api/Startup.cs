using CQRS.Database.Application;
using CQRS.Database.Domain;
using CQRS.Database.Domain.Contracts.Services;
using CQRS.Database.Infra.Providers;
using CQRS.Database.Infra.Repository.Migrations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using Tools.Contracts.Repository;
using Tools.Logging;
using Tools.WebApi;

namespace CQRS.Database
{
    public class Startup
    {
        Settings settings;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            this.settings = new Settings();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersCustom();

            services.AddSingleton<NHContext>();

            services.AddScoped<IUnitOfWork, NHUnitOfWork>();

            services.AddInvalidModelStateResponse();

            services.AddAPIResult();

            AddOneTransactionPerHttpCall(services);

            services.AddMessageNotifier(Configuration);

            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = settings.RedisConfig.HostName;
                options.InstanceName = settings.RedisConfig.InstanceName;
            });

            ConnectionRabbitMQ(services);
        
            services.AddServiceMappingsFromAssemblies<BaseApplication, IBaseService, GestorDbMigrator>();

            GestorDbMigrator.RunMigrationUp(Configuration);
        }

        public void ConnectionRabbitMQ(IServiceCollection services)
        {
            var factory = new ConnectionFactory()
            {
                HostName = settings.RabbitConfig.HostName,
                UserName = settings.RabbitConfig.UserName,
                Password = settings.RabbitConfig.Password,
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            services.AddSingleton(channel);
            services.AddSingleton(settings);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder.AllowAnyMethod()
                                          .AllowAnyOrigin()
                                          .AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<MessageLoggingMiddleware>(Configuration["Logging:Name"]);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void AddOneTransactionPerHttpCall(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWorkTransaction>((serviceProvider) =>
            {
                var wow = serviceProvider.GetService<IUnitOfWork>();

                wow.Open();

                return wow.BeginTransaction();
            });

            services.AddScoped<UnitOfWorkFilter>();
            services.Configure<MvcOptions>(o => o.Filters.AddService<UnitOfWorkFilter>(2));
        }
    }
}
