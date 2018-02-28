using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernateTDD.Tests
{
    public static class NHConfigurator
    {
        private const string CONN_STR =
          "Data Source=:memory:;Version=3;New=True;";

        private static Lazy<Configuration> _configuration = new Lazy<Configuration>(GetConfiguration);
        private static readonly Lazy<ISessionFactory> _sessionFactory = new Lazy<ISessionFactory>(() => Configuration.BuildSessionFactory());

        private static Configuration GetConfiguration()
        {
            var configuration = new Configuration().Configure()
             .DataBaseIntegration(db =>
             {
                 db.Dialect<SQLiteDialect>();
                 db.Driver<SQLite20Driver>();
                 db.ConnectionProvider<TestConnectionProvider>();
                 db.ConnectionString = CONN_STR;
             })
             .SetProperty(NHibernate.Cfg.Environment.CurrentSessionContextClass,
               "thread_static");

            var props = configuration.Properties;
            if (props.ContainsKey(NHibernate.Cfg.Environment.ConnectionStringName))
                props.Remove(NHibernate.Cfg.Environment.ConnectionStringName);
            return configuration;
        }

        /*
        static NHConfigurator()
        {
            _configuration = new Configuration().Configure()
              .DataBaseIntegration(db =>
              {
                  db.Dialect<SQLiteDialect>();
                  db.Driver<SQLite20Driver>();
                  db.ConnectionProvider<TestConnectionProvider>();
                  db.ConnectionString = CONN_STR;
              })
              .SetProperty(NHibernate.Cfg.Environment.CurrentSessionContextClass,
                "thread_static");

            var props = _configuration.Properties;
            if (props.ContainsKey(NHibernate.Cfg.Environment.ConnectionStringName))
                props.Remove(NHibernate.Cfg.Environment.ConnectionStringName);

            _sessionFactory = _configuration.BuildSessionFactory();
        }*/

        public static Configuration Configuration
        {
            get
            {
                return _configuration.Value;
            }
        }

        public static ISessionFactory SessionFactory
        {
            get
            {
                return _sessionFactory.Value;
            }
        }

    }
}
