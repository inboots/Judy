///--------------------------------------------------------------------
/// ****************************************************************
/// Peter (c)2006 All rights reserved.
/// 李 岩 (c)2006 版权所有.
/// 
/// Peter Lee is 李岩,Need Contact Please MailTo:ColdColdMan@163.com
/// ****************************************************************
///--------------------------------------------------------------------

using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Text;

/// <summary>
/// 数据库访问辅助类，该类中都是静态的方法，以更方便的调用存储过程
/// </summary>
/// <remarks>
/// <author>张川波</author>
/// <created>2004-11-18</created>
/// <modified>By Peter Lee @ 2006-03-16</modified>
/// </remarks>	
public sealed class MSSQLHelper
{
    /// <summary>
    /// 这里用私有函数，防止实例化该类
    /// </summary>
    private MSSQLHelper()
    {

    }
    /// <summary>
    /// 获取数据库连接字符串
    /// </summary>
    public static string connectionString
    {
        /* get { return ConfigurationSettings.AppSettings["eProcessDBConnectionString"]; } */
        get { return ConfigurationManager.ConnectionStrings["DBConnectString"].ConnectionString; }
    }

    /// <summary>
    /// Private routine allowed only by this base class, it automates the task
    /// of building a SqlCommand object designed to obtain a return value from
    /// the stored procedure.
    /// </summary>
    /// <param name="storedProcName">Name of the stored procedure in the DB, eg. sp_DoTask</param>
    /// <param name="parameters">Array of IDataParameter objects containing parameters to the stored proc</param>
    /// <returns>Newly instantiated SqlCommand instance</returns>
    private static SqlCommand BuildIntCommand(
        SqlConnection connection,
        string storedProcName,
        IDataParameter[] parameters)
    {
        SqlCommand command =
            BuildQueryCommand(connection, storedProcName, parameters);

        command.Parameters.Add(new SqlParameter("ReturnValue",
            SqlDbType.Int,
            4,			/* Size */
            ParameterDirection.ReturnValue,
            false,		/* is nullable */
            0,			/* byte precision */
            0,			/* byte scale */
            string.Empty,
            DataRowVersion.Default,
            null));

        return command;
    }

    /// <summary>
    /// Builds a SqlCommand designed to return a SqlDataReader, and not
    /// an actual integer value.
    /// </summary>
    /// <param name="storedProcName">Name of the stored procedure</param>
    /// <param name="parameters">Array of IDataParameter objects</param>
    /// <returns></returns>
    private static SqlCommand BuildQueryCommand(
        SqlConnection connection,
        string storedProcName,
        IDataParameter[] parameters)
    {
        if (connectionString == null)
            throw new ApplicationException("Sql连接字符串connectionString没有初始化");

        SqlCommand command = new SqlCommand(storedProcName, connection);
        command.CommandType = CommandType.StoredProcedure;

        // 此判断出现错误(parameters有值但未执行,导致发送或暂存邮件,草稿失败)@2006-4-24
        // Edited by Peter Lee @ 2006-04-20 当存储过程没有参数时不要加参数
        //if(null != parameters && parameters[0].Value != null)
        //{
        if (parameters != null)
        {
            foreach (SqlParameter parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }

        }

        //}
        return command;

    }



    private static SqlCommand BuildQueryCommandNoPar(SqlConnection connection, string storedProcName)
    {
        if (connectionString == null)
            throw new ApplicationException("Sql连接字符串connectionString没有初始化");
        SqlCommand command = new SqlCommand(storedProcName, connection);
        command.CommandType = CommandType.StoredProcedure;
        return command;
    }

    /// <summary>
    /// Runs a stored procedure, can only be called by those classes deriving
    /// from this base. It returns an integer indicating the return value of the
    /// stored procedure, and also returns the value of the RowsAffected aspect
    /// of the stored procedure that is returned by the ExecuteNonQuery method.
    /// </summary>
    /// <param name="storedProcName">Name of the stored procedure</param>
    /// <param name="parameters">Array of IDataParameter objects</param>
    /// <param name="rowsAffected">Number of rows affected by the stored procedure.</param>
    /// <returns>An integer indicating return value of the stored procedure</returns>
    public static int RunIntProcedure(
        string storedProcName,
        IDataParameter[] parameters,
        out int rowsAffected)
    {
        int result = 0;
        rowsAffected = 0;
        SqlConnection connection = new SqlConnection(MSSQLHelper.connectionString);
        try
        {
            SqlCommand command = BuildIntCommand(connection, storedProcName, parameters);
            rowsAffected = command.ExecuteNonQuery();
            result = (int)command.Parameters["ReturnValue"].Value;
        }
        finally
        {
            connection.Close();
        }
        return result;
    }

    /// <summary>
    /// 运行存储过程，并且返回存储过程的结果
    /// </summary>
    /// <param name="storedProcName">Name of the stored procedure</param>
    /// <param name="parameters">Array of IDataParameter objects</param>
    /// <returns>An integer indicating return value of the stored procedure</returns>
    public static int RunProcedure(string storedProcName, IDataParameter[] parameters)
    {
        int result = 0;

        SqlConnection connection = new SqlConnection(MSSQLHelper.connectionString);
        try
        {
            connection.Open();
            SqlCommand command = BuildIntCommand(connection, storedProcName, parameters);
            command.CommandType = CommandType.StoredProcedure;
            command.ExecuteNonQuery();

            result = (int)command.Parameters["ReturnValue"].Value;
        }
        finally
        {
            connection.Close();
        }

        return result;
    }

    /// <summary>
    /// 运行存储过程，并且返回存储过程的结果
    /// 张贤勇增加
    /// </summary>
    /// <param name="storedProcName">Name of the stored procedure</param>
    /// <param name="parameters">Array of IDataParameter objects</param>
    /// <returns>An integer indicating return value of the stored procedure</returns>
    public static int RunProcedureWithoutReturnValue(string storedProcName, IDataParameter[] parameters)
    {
        int result = 0;

        SqlConnection connection = new SqlConnection(MSSQLHelper.connectionString);
        try
        {
            connection.Open();
            SqlCommand command = BuildIntCommand(connection, storedProcName, parameters);
            command.CommandType = CommandType.StoredProcedure;
            result = command.ExecuteNonQuery();

            //result = (int)command.Parameters["ReturnValue"].Value;
        }
        finally
        {
            connection.Close();
        }

        return result;
    }

    /// <summary>
    /// Will run a stored procedure, can only be called by those classes deriving
    /// from this base. It returns a SqlDataReader containing the result of the stored
    /// procedure.
    /// </summary>
    /// <param name="storedProcName">Name of the stored procedure</param>
    /// <param name="parameters">Array of parameters to be passed to the procedure</param>
    /// <returns>A newly instantiated SqlDataReader object</returns>
    /// <remarks>
    /// 返回的SqlDataReader保持了一个打开的连接，一定要记住用完SqlDataReader后调用close方法。
    /// </remarks>
    public static SqlDataReader RunDataReaderProcedure(string storedProcName, IDataParameter[] parameters)
    {
        SqlDataReader returnReader;
        SqlConnection connection = new SqlConnection(MSSQLHelper.connectionString);

        connection.Open();
        SqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);
        command.CommandType = CommandType.StoredProcedure;

        returnReader = command.ExecuteReader();
        //connection.Close();
        return returnReader;
    }

    /// <summary>
    /// Creates a DataSet by running the stored procedure and placing the results
    /// of the query/proc into the given tablename.
    /// </summary>
    /// <param name="storedProcName">存储过程名称</param>
    /// <param name="parameters">存储过程参数</param>
    /// <param name="tableName">返回的DataSet中的Table的名称</param>
    /// <returns>存储过程的结果集</returns>
    public static DataSet RunDataSetProcedure(
        string storedProcName,
        IDataParameter[] parameters,
        string tableName)
    {
        DataSet dataSet = new DataSet();
        SqlConnection connection = new SqlConnection(MSSQLHelper.connectionString);
        try
        {
            connection.Open();
            SqlDataAdapter sqlDA = new SqlDataAdapter();
            sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
            sqlDA.Fill(dataSet, tableName);
        }
        catch (Exception e)
        {
            Console.Write(e.Message);
            return dataSet;
        }
        finally
        {
            connection.Close();
        }

        return dataSet;
    }

    /// <summary>
    /// 运行一个存储过程，并且结果集用DataSet形式返回
    /// </summary>
    /// <param name="storedProcName">存储过程名称</param>
    /// <param name="parameters">存储过程参数</param>
    /// <returns>存储过程的结果集，DataSet中的表名为Sql操作的数据表名</returns>
    public static DataSet RunDataSetProcedure(string storedProcName, IDataParameter[] parameters)
    {
        DataSet dataSet = new DataSet();
        SqlConnection connection = new SqlConnection(MSSQLHelper.connectionString);

        try
        {
            connection.Open();
            SqlDataAdapter sqlDA = new SqlDataAdapter();
            sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
            sqlDA.Fill(dataSet);
        }
        catch (Exception e)
        {
            Console.Write(e.Message);
            return dataSet;
        }
        finally
        {
            connection.Close();
        }

        return dataSet;
    }


    /// <summary>
    /// 执行无参数的存储过程
    /// </summary>
    /// <param name="storedProcName">存储过程名</param>
    /// <param name="tableName">DataSet结果中的表名</param>
    /// <returns>DataSet</returns>
    public static DataSet RunProcedureNoPar(string storedProcName)
    {
        using (SqlConnection connection = new SqlConnection(MSSQLHelper.connectionString))
        {
            DataSet dataSet = new DataSet();
            connection.Open();
            SqlDataAdapter sqlDA = new SqlDataAdapter();
            sqlDA.SelectCommand = BuildQueryCommandNoPar(connection, storedProcName);
            sqlDA.Fill(dataSet);
            connection.Close();
            return dataSet;
        }
    }

    /// <summary>
    /// Takes an -existing- dataset and fills the given table name with the results
    /// of the stored procedure.
    /// </summary>
    /// <param name="storedProcName">存储过程名称</param>
    /// <param name="parameters">存储过程参数</param>
    /// <param name="dataSet">已有的DataSet,将向其中添加表数据</param>
    /// <param name="tableName">将向DataSet中添加数据的表名称</param>
    /// <returns>无</returns>
    public static void RunDataSetProcedure(
        string storedProcName,
        IDataParameter[] parameters,
        DataSet dataSet,
        string tableName)
    {
        SqlConnection connection = new SqlConnection(MSSQLHelper.connectionString);
        try
        {
            connection.Open();
            SqlDataAdapter sqlDA = new SqlDataAdapter();
            sqlDA.SelectCommand = BuildIntCommand(connection, storedProcName, parameters);
            sqlDA.Fill(dataSet, tableName);
        }
        catch (Exception e)
        {
            Console.Write(e.Message);
            return;
        }
        finally
        {
            connection.Close();
        }
    }

    /// <summary>
    /// 运行一个存储过程，并且结果集用DataTable形式返回,这是DataSet中的第一个表
    /// </summary>
    /// <param name="storedProcName">存储过程名字</param>
    /// <param name="parameters">Sql参数</param>
    /// <returns>结果集的第一个表</returns>
    /// <remarks>不管结果集有多少个表，该方法仅仅返回结果集的第一个表.如果结果集不存在,返回null
    /// </remarks>
    public static DataTable RunDataTableProcedure(string storedProcName, IDataParameter[] parameters)
    {
        DataSet dataSet = RunDataSetProcedure(storedProcName, parameters);
        if (dataSet != null && dataSet.Tables.Count > 0)
        {
            return dataSet.Tables[0];
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 运行一个存储过程，并且结果集用DataRow形式返回,这是DataSet中的第一个表的第一行
    /// </summary>
    /// <param name="storedProcName">存储过程名字</param>
    /// <param name="parameters">Sql参数</param>
    /// <returns>结果集的第一个表的第一行</returns>
    /// <remarks>不管结果集有多少行，该方法仅仅返回第一行,如果结果集不存在,返回null
    /// </remarks>
    public static DataRow RunDataRowProcedure(string storedProcName, IDataParameter[] parameters)
    {
        DataTable dataTable = RunDataTableProcedure(storedProcName, parameters);
        if (dataTable != null && dataTable.Rows.Count > 0)
        {
            return dataTable.Rows[0];
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 运行一个存储过程，并且结果集用DataTable形式返回,这是DataSet中的第一个表的第一行
    /// </summary>
    /// <param name="storedProcName">存储过程名字</param>
    /// <param name="parameters">Sql参数</param>
    /// <returns>结果集的第一个表的第一行的第一列</returns>
    /// <remarks>
    /// 不管结果集有多少行，该方法仅仅返回第一行的第一个值,如果结果集不存在,返回null
    /// </remarks>
    public static object RunScalarProcedure(string storedProcName, IDataParameter[] parameters)
    {
        DataRow row = RunDataRowProcedure(storedProcName, parameters);
        if (row != null && row.ItemArray.Length > 0)
        {
            return row.ItemArray[0];
        }
        else
        {
            return null;
        }
    }

    # region 制造SqlParamter参数的公共方法，Author: 杨俊  Date: 2007-8-23
    public static SqlParameter GetVarcharInputParam(string parameterName, int size, string value)
    {
        return GetParam(parameterName, SqlDbType.VarChar, size, value, false);
    }

    public static SqlParameter GetIntInputParam(string parameterName, int value)
    {
        return GetParam(parameterName, SqlDbType.Int, 0, value, false);
    }

    public static SqlParameter GetIntOutputParam(string parameterName)
    {
        return GetParam(parameterName, SqlDbType.Int, 0, null, true);
    }

    public static SqlParameter GetBitInputParam(string parameterName, bool value)
    {
        return GetParam(parameterName, SqlDbType.Bit, 0, value, false);
    }

    public static SqlParameter GetSmallDateTimeInputParam(string parameterName, DateTime value)
    {
        return GetParam(parameterName, SqlDbType.SmallDateTime, 0, value, false);
    }

    private static SqlParameter GetParam(
        string parameterName, SqlDbType type, int size, object value, bool output)
    {
        SqlParameter result = new SqlParameter(parameterName, type);

        if (type == SqlDbType.VarChar) result.Size = size;

        if (output) result.Direction = ParameterDirection.Output;
        else result.Value = value;

        return result;
    }
    # endregion

    #region 执行sql语句 ,吴鸣震,2007年9月5日13:45:50
    public static int RunInsertSQL(string sql, SqlParameter[] sp)
    {
        SqlConnection connection = new SqlConnection(MSSQLHelper.connectionString);
        try
        {
            connection.Open();
            SqlCommand comm = new SqlCommand(sql, connection);
            if (sp != null)
            {
                comm.Parameters.AddRange(sp);
            }
            return comm.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Console.Write(e.Message);
            return 0;
        }
        finally
        {
            connection.Close();
        }
    }
    #endregion

    #region 执行增，删，改sql语句 ,王小乐,2008.3.7
    public static int RunInsertOrUpdateOrDeleteSQL(string sql)
    {
        SqlConnection connection = new SqlConnection(MSSQLHelper.connectionString);
        try
        {
            connection.Open();
            SqlCommand comm = new SqlCommand(sql, connection);
            return comm.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            throw (e);
        }
        finally
        {
            connection.Close();
        }
    }
    #endregion

    #region 执行查询sql语句 ,王小乐,2008.3.7
    public static DataSet RunDataSetSQL(string sql)
    {
        SqlConnection connection = new SqlConnection(MSSQLHelper.connectionString);
        DataSet ds = new DataSet();
        try
        {
            connection.Open();
            SqlCommand comm = new SqlCommand(sql, connection);
            new SqlDataAdapter(comm).Fill(ds);
            return ds;
        }
        catch (Exception e)
        {
            Console.Write(e.Message);
            return new DataSet();
        }
        finally
        {
            connection.Close();
        }
    }
    #endregion

    #region 在事务中执行一批sql语句 ,王小乐,2008.3.7
    public static int RunSQLInTransaction(string[] sqls)
    {
        SqlConnection connection = new SqlConnection(MSSQLHelper.connectionString);

        connection.Open();
        SqlTransaction trans = connection.BeginTransaction();

        try
        {
            SqlCommand comm = new SqlCommand();
            comm.Connection = connection;
            comm.Transaction = trans;
            foreach (string sql in sqls)
            {
                if (sql != "")
                {
                    comm.CommandText = sql;
                    comm.ExecuteNonQuery();
                }
            }
            trans.Commit();
            return 1;
        }
        catch (Exception e)
        {
            Console.Write(e.Message);
            trans.Rollback();
            return 0;
        }
        finally
        {
            connection.Close();
        }
    }
    #endregion
    #region 在事务中执行一批sql语句 ,王小乐,2008.3.7
    public static int RunSQLInTransaction(ArrayList sqls)
    {
        SqlConnection connection = new SqlConnection(MSSQLHelper.connectionString);

        connection.Open();
        SqlTransaction trans = connection.BeginTransaction();

        try
        {
            SqlCommand comm = new SqlCommand();
            comm.Connection = connection;
            comm.Transaction = trans;
            foreach (string sql in sqls)
            {
                comm.CommandText = sql;
                comm.ExecuteNonQuery();
            }
            trans.Commit();
            return 1;
        }
        catch (Exception e)
        {
            Console.Write(e.Message);
            trans.Rollback();
            throw (e);
        }
        finally
        {
            connection.Close();
        }
    }
    #endregion

    #region 执行查询sql语句 ,王小乐,2008.3.7
    public static DataTable RunDataTableSQL(string sql)
    {
        SqlConnection connection = new SqlConnection(MSSQLHelper.connectionString);
        DataTable dt = new DataTable();
        try
        {
            connection.Open();
            SqlCommand comm = new SqlCommand(sql, connection);
            new SqlDataAdapter(comm).Fill(dt);
            return dt;
        }
        catch (Exception e)
        {
            Console.Write(e.Message);
            return new DataTable();
        }
        finally
        {
            connection.Close();
        }
    }
    #endregion

    #region 执行查询sql语句,返回该记录是否已经存在,王小乐,2008.3.10
    /// <summary>
    ///  执行查询sql语句,返回该记录是否已经存在,王小乐,2008.3.10
    /// </summary>
    /// <param name="sql">Sql语句，唯一字段cnt</param>
    /// <returns>是否存在记录</returns>
    public static bool RunIsExsitSQL(string sql)
    {
        SqlConnection connection = new SqlConnection(MSSQLHelper.connectionString);
        try
        {
            int cnt = 0;
            connection.Open();
            SqlCommand comm = new SqlCommand(sql, connection);
            SqlDataReader rdr = comm.ExecuteReader();
            if (rdr.Read())
            {
                cnt = int.Parse(rdr["cnt"].ToString());
            }
            rdr.Close();
            return cnt > 0 ? true : false;
        }
        catch (Exception e)
        {
            Console.Write(e.Message);
            return false;
        }
        finally
        {
            connection.Close();
        }
    }
    #endregion
    #region 在其它服务器上执行SQL事务，返回是否执行成功,魏厚龙 2008.9.4
    public static bool RunTransaction(string conn, ArrayList sqls)
    {
        SqlConnection connection = new SqlConnection(conn);

        connection.Open();
        SqlTransaction trans = connection.BeginTransaction();
        try
        {
            SqlCommand comm = new SqlCommand();
            comm.Connection = connection;
            comm.Transaction = trans;
            foreach (string sql in sqls)
            {
                comm.CommandText = sql;
                comm.ExecuteNonQuery();
            }
            trans.Commit();
            return true;
        }
        catch (Exception e)
        {
            Console.Write(e.Message);
            trans.Rollback();
            throw (e);
        }
        finally
        {
            connection.Close();
        }
    }
    #endregion
    #region 在其它服务器上执行查询sql语句,返回数据表 ,魏厚龙 2008.9.5
    public static DataTable RunDataTableSQLOnOther(string conn, string sql)
    {
        SqlConnection connection = new SqlConnection(conn);
        DataTable dt = new DataTable();
        try
        {
            connection.Open();
            SqlCommand comm = new SqlCommand(sql, connection);
            new SqlDataAdapter(comm).Fill(dt);
            return dt;
        }
        catch (Exception e)
        {
            Console.Write(e.Message);
            return new DataTable();
        }
        finally
        {
            connection.Close();
        }
    }
    #endregion
    #region AJAX方法 ,魏厚龙 2011.1.11
    /// <summary>
    /// 获取查询脚本的数据
    /// </summary>
    /// <param name="sqlstr"></param>
    /// <returns></returns>
    public static string GetTableForJson(string sqlstr)
    {
        StringBuilder StrResult = new StringBuilder();
        string FormatStr = string.Empty;
        DataTable dt = MSSQLHelper.RunDataTableSQL(sqlstr);
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {

                FormatStr = "{";
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    FormatStr += "\"" + dt.Columns[i].ColumnName.ToString().ToLower() + "\":\"" + dr[i].ToString() + "\",";
                }
                FormatStr = FormatStr.ToString().TrimEnd(',') + "},";
                StrResult.Append(FormatStr);
            }

        }
        return "[" + StrResult.ToString().TrimEnd(',') + "]";
    }
    #endregion
    #region 参数化插入更新删除数据库记录 魏厚龙 2011.1.11
    /// <summary>
    /// 参数化插入更新删除数据库记录
    /// </summary>
    /// <param name="insertsql">带参数的sql语句</param>
    /// <param name="param">参数哈希表</param>
    /// <returns></returns>
    //public static bool InsertUpdateDeleteByParam(string insertsql, Hashtable param)
    //{
    //    SqlConnection connection = new SqlConnection(MSSQLHelper.connectionString);
    //    string valuestr = "";
    //    try
    //    {
    //        SqlCommand comm = new SqlCommand(insertsql, connection);
    //        for (int i = 1; i <= param.Count; i++)
    //        {
    //            if (param.Contains("@" + i.ToString()))
    //            {
    //                comm.Parameters.Add("@" + i.ToString(), param["@" + i.ToString()]);
    //                valuestr += param["@" + i.ToString()] + ",";
    //            }
    //        }
    //        connection.Open();
    //        comm.ExecuteNonQuery();
    //    }
    //    catch (Exception e)
    //    {
    //        Console.Write(e.Message);
    //        return false;
    //    }
    //    finally
    //    {
    //        connection.Close();
    //    }
    //    return true;
    //}
    public static bool InsertUpdateDeleteByParam(string insertsql, Hashtable param)
    {
        SqlConnection connection = new SqlConnection(MSSQLHelper.connectionString);
        try
        {
            SqlCommand comm = new SqlCommand(insertsql, connection);

            foreach (DictionaryEntry de in param)
            {
                comm.Parameters.Add(de.Key.ToString(), de.Value);
            }
            connection.Open();
            comm.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Console.Write(e.Message);
            return false;
        }
        finally
        {
            connection.Close();
        }
        return true;
    }
    /// <summary>
    /// 参数化查询数据库记录
    /// </summary>
    /// <param name="insertsql">带参数的sql语句</param>
    /// <param name="paramnum">参数数量</param>
    /// <param name="param">参数哈希表</param>
    /// <returns></returns>
    public static DataTable RunSqlParam(string sqlstr, Hashtable param)
    {
        SqlConnection connection = new SqlConnection(MSSQLHelper.connectionString);
        DataTable dt = new DataTable();
        try
        {
            SqlCommand comm = new SqlCommand(sqlstr, connection);
            foreach (DictionaryEntry de in param)
            {
                comm.Parameters.Add(de.Key.ToString(), de.Value);
            }
            connection.Open();
            new SqlDataAdapter(comm).Fill(dt);
            return dt;
        }
        catch (Exception e)
        {
            Console.Write(e.Message);
            return new DataTable();
        }
        finally
        {
            connection.Close();
        }
    }
    #endregion
    #region 执行查询sql语句,返回该记录的第一行第一个记录
    public static string GetSingleValue(string sql)
    {

        string SingleValue = string.Empty;
        try
        {
            DataTable dt = RunDataTableSQL(sql);
            if (dt.Rows.Count > 0)
            {
                SingleValue = dt.Rows[0][0].ToString();
            }
        }
        catch (Exception ex)
        {
            return "";
        }
        return SingleValue;
    }
    #endregion

    #region 返回添加后的主键值 (sql语句后加上SELECT @@IDENTITY AS returnName;)
    public static int RunSaclar(string sql)
    {
        SqlConnection connection = new SqlConnection(MSSQLHelper.connectionString);
        if (connection.State == ConnectionState.Closed) connection.Open();
        SqlCommand cmd = new SqlCommand(sql, connection);
        int id = Convert.ToInt32(cmd.ExecuteScalar());
        return id;
    }
    #endregion
}
