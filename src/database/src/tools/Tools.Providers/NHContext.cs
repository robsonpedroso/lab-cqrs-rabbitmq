using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Mapping;
using FluentNHibernate.Mapping.Providers;
using NHibernate;
using NHibernate.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using nh = NHibernate;
using Microsoft.Extensions.Configuration;


namespace Gestor.Tools.Providers
{
    public class NHContext
    {
        public ISessionFactory SessionFactory { get; private set; }

        public NHContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Gestor");

            SessionFactory = Fluently
                .Configure()
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionString))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<NHContext>())
                .BuildSessionFactory();
        }
    }
}
