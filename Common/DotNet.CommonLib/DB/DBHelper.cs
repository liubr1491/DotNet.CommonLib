using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace DotNet.CommonLib.DB
{
    /// <summary>
    /// 这个数据库操作类的主要特色有
    /// 1>事务操作更加的方便
    /// 2>变更数据库更加的容易
    /// </summary>
    public class DBHelper
    {
        #region Constuctor

        public DBHelper() { }

        private static string ConnectionString = ConfigurationManager.AppSettings["DBConnectionString"];

        private static IDBClient DBClient = DBClientFactory.GetDBClient(ConfigurationManager.AppSettings["DBClient"]);

        [ThreadStatic]
        private static TransConnection TransConnectionObj = null;

        #endregion

        #region ExecuteNonQuery

        public static int ExecuteNonQuery(CommandType cmdType, string cmdText, params DbParameter[] parameterValues)
        {
            int result = 0;
            bool mustCloseConn = true;

            DbCommand cmd = PrepareCmd(cmdType, cmdText, parameterValues, out mustCloseConn);
            OpenConn(cmd.Connection);
            result = cmd.ExecuteNonQuery();

            if (mustCloseConn)
            {
                CloseConn(cmd.Connection);
            }
            ClearCmdParameters(cmd);
            cmd.Dispose();

            return result;
        }

        #endregion ExecuteNonQuery

        #region ExecuteScalar

        public static object ExecuteScalar(CommandType cmdType, string cmdText, params DbParameter[] parameterValues)
        {
            object result = 0;
            bool mustCloseConn = true;

            DbCommand cmd = PrepareCmd(cmdType, cmdText, parameterValues, out mustCloseConn);
            OpenConn(cmd.Connection);
            result = cmd.ExecuteScalar();

            if (mustCloseConn)
            {
                CloseConn(cmd.Connection);
            }
            ClearCmdParameters(cmd);
            cmd.Dispose();

            return result;
        }
        #endregion ExecuteScalar

        #region ExecuteReader
        public static DbDataReader ExecuteReader(CommandType cmdType, string cmdText, params DbParameter[] parameterValues)
        {
            DbDataReader result = null;
            bool mustCloseConn = true;
            DbCommand cmd = PrepareCmd(cmdType, cmdText, parameterValues, out mustCloseConn);
            try
            {
                OpenConn(cmd.Connection);
                if (mustCloseConn)
                {
                    result = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
                else
                {
                    result = cmd.ExecuteReader();
                }
                ClearCmdParameters(cmd);
                return result;
            }
            catch (Exception ex)
            {
                if (mustCloseConn)
                {
                    CloseConn(cmd.Connection);
                }
                ClearCmdParameters(cmd);
                cmd.Dispose();
                throw;
            }
        }
        #endregion ExecuteReader

        #region ExecuteDataset
        public static DataSet ExecuteDataSet(CommandType cmdType, string cmdText, params DbParameter[] parameterValues)
        {
            DataSet result = null;
            bool mustCloseConn = true;

            DbCommand cmd = PrepareCmd(cmdType, cmdText, parameterValues, out mustCloseConn);
            using (DbDataAdapter da = DBClient.GetDbDataAdappter())
            {
                da.SelectCommand = cmd;
                result = new DataSet();

                da.Fill(result);
            }

            if (mustCloseConn)
            {
                CloseConn(cmd.Connection);
            }
            ClearCmdParameters(cmd);
            cmd.Dispose();

            return result;
        }
        #endregion ExecuteDataset

        #region ExecuteDataTable
        public static DataTable ExecuteDataTable(CommandType cmdType, string cmdText, params DbParameter[] parameterValues)
        {
            DataSet ds = ExecuteDataSet(cmdType, cmdText, parameterValues);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Transaction
        public static void BeginTransaction()
        {
            if (TransConnectionObj == null)
            {
                DbConnection conn = DBClient.GetDbConnection(ConnectionString);
                OpenConn(conn);
                DbTransaction trans = conn.BeginTransaction();
                TransConnectionObj = new TransConnection();
                TransConnectionObj.DBTransaction = trans;
            }
            else
            {
                TransConnectionObj.Deeps += 1;
            }
        }

        public static void CommitTransaction()
        {
            if (TransConnectionObj == null) return;
            if (TransConnectionObj.Deeps > 0)
            {
                TransConnectionObj.Deeps -= 1;
            }
            else
            {
                TransConnectionObj.DBTransaction.Commit();
                ReleaseTransaction();
            }
        }

        public static void RollbackTransaction()
        {
            if (TransConnectionObj == null)
            {
                return;
            }
            if (TransConnectionObj.Deeps > 0)
            {
                TransConnectionObj.Deeps -= 1;
            }
            else
            {
                TransConnectionObj.DBTransaction.Rollback();
                ReleaseTransaction();
            }
        }

        private static void ReleaseTransaction()
        {
            if (TransConnectionObj == null)
            {
                return;
            }
            DbConnection conn = TransConnectionObj.DBTransaction.Connection;
            TransConnectionObj.DBTransaction.Dispose();
            TransConnectionObj = null;
            CloseConn(conn);
        }

        #endregion

        #region Connection
        private static void OpenConn(DbConnection conn)
        {
            if (conn == null)
            {
                conn = DBClient.GetDbConnection(ConnectionString);
            }
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
        }

        private static void CloseConn(DbConnection conn)
        {
            if (conn == null)
            {
                return;
            }
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Dispose();
            conn = null;
        }
        #endregion

        #region Create DbParameter

        public static DbParameter CreateInDbParameter(string paraName, DbType type, int size, object value)
        {
            return CreateDbParameter(paraName, type, size, value, ParameterDirection.Input);
        }

        public static DbParameter CreateInDbParameter(string paraName, DbType type, object value)
        {
            return CreateDbParameter(paraName, type, 0, value, ParameterDirection.Input);
        }

        public static DbParameter CreateOutDbParameter(string paraName, DbType type, int size)
        {
            return CreateDbParameter(paraName, type, size, null, ParameterDirection.Output);
        }

        public static DbParameter CreateOutDbParameter(string paraName, DbType type)
        {
            return CreateDbParameter(paraName, type, 0, null, ParameterDirection.Output);
        }

        public static DbParameter CreateReturnDbParameter(string paraName, DbType type, int size)
        {
            return CreateDbParameter(paraName, type, size, null, ParameterDirection.ReturnValue);
        }

        public static DbParameter CreateReturnDbParameter(string paraName, DbType type)
        {
            return CreateDbParameter(paraName, type, 0, null, ParameterDirection.ReturnValue);
        }

        public static DbParameter CreateDbParameter(string paraName, DbType type, int size, object value, ParameterDirection direction)
        {
            DbParameter para = DBClient.GetDbParameter();

            para.ParameterName = paraName;

            if (size != 0)
            {
                para.Size = size;
            }

            para.DbType = type;

            if (value != null)
            {
                para.Value = value;
            }
            else
            {
                para.Value = DBNull.Value;
            }

            para.Direction = direction;

            return para;
        }

        #endregion

        #region Command and Parameter
        /// <summary>
        /// 预处理用户提供的命令,数据库连接/事务/命令类型/参数
        /// </summary>
        /// <param>要处理的DbCommand</param>
        /// <param>数据库连接</param>
        /// <param>一个有效的事务或者是null值</param>
        /// <param>命令类型 (存储过程,命令文本, 其它.)</param>
        /// <param>存储过程名或都T-SQL命令文本</param>
        /// <param>和命令相关联的DbParameter参数数组,如果没有参数为'null'</param>
        /// <param><c>true</c> 如果连接是打开的,则为true,其它情况下为false.</param>
        private static DbCommand PrepareCmd(CommandType cmdType, string cmdText, DbParameter[] cmdParams, out bool mustCloseConn)
        {
            DbCommand cmd = DBClient.GetDbCommand(cmdText);

            DbConnection conn = null;
            if (TransConnectionObj != null)
            {
                conn = TransConnectionObj.DBTransaction.Connection;
                cmd.Transaction = TransConnectionObj.DBTransaction;
                mustCloseConn = false;
            }
            else
            {
                conn = DBClient.GetDbConnection(ConnectionString);
                mustCloseConn = true;
            }
            cmd.Connection = conn;

            cmd.CommandType = cmdType;

            AttachParameters(cmd, cmdParams);

            return cmd;
        }

        /// <summary>
        /// 将DbParameter参数数组(参数值)分配给DbCommand命令.
        /// 这个方法将给任何一个参数分配DBNull.Value;
        /// 该操作将阻止默认值的使用.
        /// </summary>
        /// <param>命令名</param>
        /// <param>SqlParameters数组</param>
        private static void AttachParameters(DbCommand command, DbParameter[] commandParameters)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            if (commandParameters != null)
            {
                foreach (DbParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        // 检查未分配值的输出参数,将其分配以DBNull.Value.
                        if ((p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Input) && (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }

        private static void ClearCmdParameters(DbCommand cmd)
        {
            bool canClear = true;
            if (cmd.Connection != null && cmd.Connection.State != ConnectionState.Open)
            {
                foreach (DbParameter commandParameter in cmd.Parameters)
                {
                    if (commandParameter.Direction != ParameterDirection.Input)
                    {
                        canClear = false;
                        break;
                    }
                }
            }
            if (canClear)
            {
                cmd.Parameters.Clear();
            }
        }
        #endregion
    }
}