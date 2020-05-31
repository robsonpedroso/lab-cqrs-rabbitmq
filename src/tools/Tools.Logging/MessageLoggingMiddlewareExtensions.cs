using log4net;
using log4net.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Reflection;

namespace CQRS.Tools.Logging
{
    public static class MessageNotifierExtensions
    {
        public static IServiceCollection AddMessageNotifier(this IServiceCollection services, IConfiguration configuration)
        {
            var loggerName = configuration["Logging:Name"];
            var log = LogManager.GetLogger(Assembly.GetEntryAssembly(), loggerName);
            XmlConfigurator.Configure(log.Logger.Repository, new FileInfo("log4net.config"));

            services.AddSingleton(log);

            services.AddScoped<INotifier, MessageNotifier>();

            return services;
        }
    }
}