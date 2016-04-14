using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Npgsql;
using System.Data;

/// <summary>
///PostGreSQLHelper 的摘要说明
/// </summary>
public class PostGreSQLHelper
{

    private static string ConnectStr = "Server=10.1.10.216;Port=5432;User Id=ahwb;Password=asdf1234;Database=mine;";
	public PostGreSQLHelper()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    public string TestConnection()
    {
        string str = ConnectStr;
        string strMessage = string.Empty;
        try
        {
            NpgsqlConnection conn = new NpgsqlConnection(str);
            conn.Open();
            strMessage = "Success";
            conn.Close();
        }
        catch
        {
            strMessage = "Failure";
        }
        return strMessage;
    }

    public DataSet ExecuteQuery(string sql)
    {
        NpgsqlConnection conn = new NpgsqlConnection(ConnectStr);
        conn.Open();
        NpgsqlCommand command = new NpgsqlCommand(sql, conn);
        //command.Parameters.Add(new NpgsqlParameter("value0", DbType.Int32));
        //command.Prepare();
        //command.Parameters[0].Value = 1000;
        DataSet ds = new DataSet();
        try
        {
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(command);
            da.Fill(ds);

        }
        finally
        {
            conn.Close();
        }
        return ds;
    }

    public string GetPointNameNote(string pointid)
    {
        IDbConnection dbcon = new NpgsqlConnection(ConnectStr);
        dbcon.Open();
        IDbCommand dbcmd = dbcon.CreateCommand();
        dbcmd.CommandText = string.Format("SELECT point_id, point_name, position_note FROM v_jykj_points where point_id='{0}';", pointid);
        IDataReader dr = dbcmd.ExecuteReader();
        string strResult = "无";
        while (dr.Read())
        {
            strResult = dr[2].ToString() + "[" + dr[1].ToString() + "]";
            break;
        }
        dr.Close();
        dr = null;
        dbcon.Close();
        return strResult;
    }
}
