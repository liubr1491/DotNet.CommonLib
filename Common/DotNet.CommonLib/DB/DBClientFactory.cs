using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Reflection;

namespace DotNet.CommonLib.DB
{
    public class DBClientFactory
    {
        private static readonly string path = "Project.BaseFramework";

        public static IDBClient GetDBClient(string dbClientClassName)
        {
            if (string.IsNullOrEmpty(dbClientClassName))
            {
                dbClientClassName = "SqlServerClient";
            }
            string className = string.Format("{0}.DataProvider.{1}", path, dbClientClassName);
            return (IDBClient)Assembly.Load(path).CreateInstance(className);
        }
    }
}