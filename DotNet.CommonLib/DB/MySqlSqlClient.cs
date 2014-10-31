using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace DotNet.CommonLib.DB
{
    public class MySqlSqlClient : IDBClient
    {
        public DbConnection GetDbConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }

        public DbCommand GetDbCommand(string cmdText)
        {
            return new MySqlCommand(cmdText);
        }

        public DbDataAdapter GetDbDataAdappter()
        {
            return new MySqlDataAdapter();
        }

        public DbParameter GetDbParameter()
        {
            return new MySqlParameter();
        }
    }
}