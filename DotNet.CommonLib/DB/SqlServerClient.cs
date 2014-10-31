using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DotNet.CommonLib.DB
{
    public class SqlServerClient : IDBClient
    {
        public DbConnection GetDbConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        public DbCommand GetDbCommand(string cmdText)
        {
            return new SqlCommand(cmdText);
        }

        public DbDataAdapter GetDbDataAdappter()
        {
            return new SqlDataAdapter();
        }

        public DbParameter GetDbParameter()
        {
            return new SqlParameter();
        }
    }
}