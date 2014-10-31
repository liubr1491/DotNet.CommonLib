using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DotNet.CommonLib.DB
{
    public interface IDBClient
    {
        DbConnection GetDbConnection(string connectionString);

        DbCommand GetDbCommand(string cmdText);

        DbDataAdapter GetDbDataAdappter();

        DbParameter GetDbParameter();
    }
}