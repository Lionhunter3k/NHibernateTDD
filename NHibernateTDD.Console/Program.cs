using NHibernate;
using NHibernate.Context;
using NHibernateTDD.Core;
using NHibernateTDD.Tests;
using NHibernateTDD.Tests.Conventions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernateTDD.Console
{
    class Program
    {
        protected static ISessionFactory SessionFactory
        {
            get
            {
                return NHConfigurator.SessionFactory;
            }
        }

        protected static ISession Session
        {
            get { return SessionFactory.GetCurrentSession(); }
        }

        static void Main(string[] args)
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
                FilterAssembly = t => t.IsSubclassOf(typeof(Entity)),
                Conventions = new List<IAmConvention>{
                     new VersionConvention(),
                     new BidirectionalManyToManyRelationsConvention
                     {
                          BaseEntityType = typeof(Entity),
                     },
                     new BidirectionalOneToManyConvention
                     {
                          BaseEntityType = typeof(Entity),
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
                     },
                     new UnidirectionalOneToManyConvention
                     {
                           BaseEntityType = typeof(Entity),
                     },
                     new ReadOnlyConvention
                     {
                         BaseEntityType = typeof(Entity),
                     }
                 }
            };
            conventionBuilder.ProcessConfiguration(cfg);
            var session = SessionFactory.OpenSession();
            CurrentSessionContext.Bind(session);
            using (var tx = session.BeginTransaction())
            {

                //create a person
                Person p = new Person("Bruce", "Wayne");
                p.DateOfBirth = new DateTime(1972, 6, 2);
                p.HomePhone = "12345678";
                p.AltPhone = "987675123";
                p.Address.Line1 = "123 Bruce Manor";
                p.Address.State = "GT";
                p.Address.City = "Gotham";
                p.Address.Zip = "99999";
                //save the person
                session.Save(p);
                Assert.IsTrue(p.Id != 0, "Save should give p it’s primary key");
                Person pVerify = (Person)session.Load(typeof(Person), p.Id);
                Assert.AreEqual(p, pVerify, "They weren’t the same");
            }



        }
    }
}
