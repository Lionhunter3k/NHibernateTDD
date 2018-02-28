using NHibernate.Connection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace NHibernateTDD.Tests
{
    public class TestConnectionProvider : DriverConnectionProvider
    {
        [ThreadStatic]
        private static IDbConnection Connection;

        public static void CloseDatabase()
        {
            if (Connection != null)
                Connection.Dispose();
            Connection = null;
        }

        public override IDbConnection GetConnection()
        {
            if (Connection == null)
            {
                Connection = Driver.CreateConnection();
                Connection.ConnectionString = ConnectionString;
                Connection.Open();
            }
            return Connection;
        }

        public override void CloseConnection(IDbConnection conn)
        {
            // Do nothing
        }
    }

}
