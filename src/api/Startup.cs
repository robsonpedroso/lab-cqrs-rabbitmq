using CQRS.Core.Domain;
using CQRS.Core.Application;
using CQRS.Core.Domain.Contracts.Services;
using CQRS.Core.Infra.Repository;
using CQRS.Tools.Logging;
using CQRS.Tools.WebApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace CQRS
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

            services.AddInvalidModelStateResponse();

            services.AddAPIResult();

            services.AddMessageNotifier(Configuration);

            services.AddServiceMappingsFromAssemblies<BaseApplication, IBaseService, AccountRepository>();
            
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = settings.RedisConfig.HostName;
                options.InstanceName = settings.RedisConfig.InstanceName;
            });

            ConnectionRabbitMQ(services);
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
    }
}
