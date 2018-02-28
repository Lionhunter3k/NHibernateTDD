using log4net;
using NHibernate;
using NHibernate.Context;
using NHibernate.Tool.hbm2ddl;
using NHibernateTDD.Tests.Conventions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernateTDD.Tests
{
    public abstract class BaseTest
    {
        protected static ILog log = new Func<ILog>(() =>
        {
            log4net.Config.XmlConfigurator.Configure();
            return LogManager.GetLogger(typeof(BaseTest));
        }).Invoke();

        //called before every test
        [SetUp]
        public virtual void SetUp()
        {
            TestConnectionProvider.CloseDatabase();
            var cfg = NHConfigurator.Configuration;
            var conventionBuilder = new ConventionBuilder
            {
                DbSchemaOutputFile = "schema.sql",
                DropTablesCreateDbSchema = true,
                OutputXmlMappingsFile = "schema.hbm.xml",
                MappingsAssembly = typeof(Entity).Assembly,
                ShowLogs = true,
                FilterAssembly = t => t.IsSubclassOf(typeof(Entity)) && t.Namespace.EndsWith("Domain"),
                Conventions = new List<IAmConvention>{
                     new VersionConvention(),
                     new BidirectionalManyToManyRelationsConvention
                     {
                          BaseEntityType = typeof(Entity),
                          TypeFilter = p=> p.Namespace.EndsWith("Domain")
                     },
                     new BidirectionalOneToManyConvention
                     {
                          BaseEntityType = typeof(Entity),
                          TypeFilter = p=> p.Namespace.EndsWith("Domain")
                     },
                     new DefineBaseClassConvention
                     {
                         BaseEntityToIgnore = typeof(Entity)
                     },
                     new EnumConvention(),
                     new NamingConvention(),
                     new UnidirectionalManyToOne
                     {
                          BaseEntityType = typeof(Entity),
                          TypeFilter = p=> p.Namespace.EndsWith("Domain")
                     },
                     new UnidirectionalOneToManyConvention
                     {
                           BaseEntityType = typeof(Entity),
                          TypeFilter = p=> p.Namespace.EndsWith("Domain")
                     }
                 }
            };
            conventionBuilder.ProcessConfiguration(cfg);
            var session = SessionFactory.OpenSession();
            CurrentSessionContext.Bind(session);
            //var schemaExport = new SchemaExport(cfg);
            //schemaExport.Create(false, true);
        }

        protected ISessionFactory SessionFactory
        {
            get
            {
                return NHConfigurator.SessionFactory;
            }
        }

        protected ISession Session
        {
            get { return SessionFactory.GetCurrentSession(); }
        }


        //called after every test
        [TearDown]
        public virtual void TearDown()
        {
            var sessionFactory = NHConfigurator.SessionFactory;
            var session = CurrentSessionContext.Unbind(sessionFactory);
            session.Close();
            TestConnectionProvider.CloseDatabase();
        }

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            //noop
        }

        [TestFixtureTearDown]
        public void FixtureTeardown()
        {
            //noop
        }

    }
}
