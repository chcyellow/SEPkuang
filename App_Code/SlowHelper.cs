using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using GhtnTech.SecurityFramework.DBUtility;

/// <summary>
///SlowHelper 的摘要说明
/// </summary>
    public class SlowHelper
    {
        public static DataTable GetDBSY(string person, string kqid, string maindept)
        {
            OracleParameter[] param = {
                    new OracleParameter("person",OracleType.NVarChar),
                    new OracleParameter("kqid",OracleType.NVarChar),
                    new OracleParameter("maindept",OracleType.NVarChar),
                    new OracleParameter("v_cur",OracleType.Cursor)    
                    };
            param[0].Value = person;
            param[1].Value = kqid;
            param[2].Value = maindept;
            param[0].Direction = ParameterDirection.Input;
            param[1].Direction = ParameterDirection.Input;
            param[2].Direction = ParameterDirection.Input;
            param[3].Direction = ParameterDirection.Output;
            DataSet ds = OracleHelper.RunProcedure("HOME_DBSY.HOME_DBSY_body", param, "ds");
            return ds.Tables["ds"];
        }
        public static DataTable GetLastHYInfo(string kqid, string maindept)
        {
            OracleParameter[] param = {
                    new OracleParameter("kqid",OracleType.NVarChar),
                    new OracleParameter("maindept",OracleType.NVarChar),
                    new OracleParameter("v_cur",OracleType.Cursor)    
                    };
            param[0].Value = kqid;
            param[1].Value = maindept;
            param[0].Direction = ParameterDirection.Input;
            param[1].Direction = ParameterDirection.Input;
            param[2].Direction = ParameterDirection.Output;
            DataSet ds = OracleHelper.RunProcedure("HOME_NEWYH.HOME_NEWYH_body", param, "ds");
            return ds.Tables["ds"];
        }
    }