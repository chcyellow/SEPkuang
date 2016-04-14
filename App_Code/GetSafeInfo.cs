using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using GhtnTech.SecurityFramework.DBUtility;
/// <summary>
///GetSafeInfo 的摘要说明
/// </summary>
public class GetSafeInfo
{
    public GetSafeInfo()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }
    public static DataSet GetAllSafetyCountByDept(string maindept,DateTime begindate, DateTime enddate)
    {
        OracleParameter[] param = {
                    new OracleParameter("maindept",OracleType.NVarChar),
                    new OracleParameter("begindate",OracleType.DateTime),
                    new OracleParameter("enddate",OracleType.DateTime),
                    new OracleParameter("v_cur",OracleType.Cursor)    
                    };
        param[0].Value = maindept;
        param[1].Value = begindate;
        param[2].Value = enddate;
        param[0].Direction = ParameterDirection.Input;
        param[1].Direction = ParameterDirection.Input;
        param[2].Direction = ParameterDirection.Input;
        param[3].Direction = ParameterDirection.Output;
        DataSet ds = OracleHelper.RunProcedure("SafeInfo.GetAllSafetyCountByDept", param, "ds");
        return ds;
    }
    public static DataSet GetAllSafetyCountByUnit(string maindept, DateTime begindate, DateTime enddate)
    {
        OracleParameter[] param = {
                    new OracleParameter("maindept",OracleType.NVarChar),
                    new OracleParameter("begindate",OracleType.DateTime),
                    new OracleParameter("enddate",OracleType.DateTime),
                    new OracleParameter("v_cur",OracleType.Cursor)    
                    };
        param[0].Value = maindept;
        param[1].Value = begindate;
        param[2].Value = enddate;
        param[0].Direction = ParameterDirection.Input;
        param[1].Direction = ParameterDirection.Input;
        param[2].Direction = ParameterDirection.Input;
        param[3].Direction = ParameterDirection.Output;
        DataSet ds = OracleHelper.RunProcedure("AllSafeInfo.GetAllSafetyCountByUnit", param, "ds");
        return ds;
    }
    //周——地点区域
    public static DataSet GetAllSafetyCountByPAreas(DateTime begindate, DateTime enddate, string deptid)
    {

        OracleParameter[] param = {
                    new OracleParameter("begindate",OracleType.DateTime),
                    new OracleParameter("enddate",OracleType.DateTime),
                    new OracleParameter("deptid",OracleType.VarChar),
                    new OracleParameter("v_cur",OracleType.Cursor)    
                    };
        param[0].Value = begindate;
        param[1].Value = enddate;
        param[2].Value = deptid;
        param[0].Direction = ParameterDirection.Input;
        param[1].Direction = ParameterDirection.Input;
        param[2].Direction = ParameterDirection.Input;
        param[3].Direction = ParameterDirection.Output;
        DataSet ds = OracleHelper.RunProcedure("GetAllCountByPAreas.GetAllSafetyCountByPAreas", param, "ds");
        return ds;
    }

    public static DataSet GetYHView(string maindept, DateTime begindate, DateTime enddate)
    {
        OracleParameter[] param = {
                    new OracleParameter("maindept",OracleType.NVarChar),
                    new OracleParameter("begindate",OracleType.DateTime),
                    new OracleParameter("enddate",OracleType.DateTime),
                    new OracleParameter("v_cur",OracleType.Cursor)    
                    };
        param[0].Value = maindept;
        param[1].Value = begindate;
        param[2].Value = enddate;
        param[0].Direction = ParameterDirection.Input;
        param[1].Direction = ParameterDirection.Input;
        param[2].Direction = ParameterDirection.Input;
        param[3].Direction = ParameterDirection.Output;
        DataSet ds = OracleHelper.RunProcedure("GetYHView.prc_yhview", param, "ds");
        return ds;
    }
    #region 干部走动模块-存储过程
    //走动调度人员信息
    public static DataSet GetDiaoDu(string maindept)
    {
        OracleParameter[] param = {
                    new OracleParameter("dept",OracleType.VarChar),
                    new OracleParameter("v_cur",OracleType.Cursor)    
                    };
        param[0].Value = maindept;
        param[0].Direction = ParameterDirection.Input;
        param[1].Direction = ParameterDirection.Output;
        DataSet ds = OracleHelper.RunProcedure("DiaoDu.GetDiaoDu", param, "ds");
        return ds;
    }

    //走动调度计划信息
    public static DataSet GetDiaoDuInfo(string personnumber)
    {
        OracleParameter[] param = {
                    new OracleParameter("perno",OracleType.VarChar),
                    new OracleParameter("v_cur",OracleType.Cursor)    
                    };
        param[0].Value = personnumber;
        param[0].Direction = ParameterDirection.Input;
        param[1].Direction = ParameterDirection.Output;
        DataSet ds = OracleHelper.RunProcedure("DiaoDuInfo.GetDiaoDuInfo", param, "ds");
        return ds;
    }
    #endregion
#region 领导首页
    //领导首页-隐患
    public static DataSet GetMainLeaderYH(DateTime begindate, DateTime enddate,string maindept)
    {
        OracleParameter[] param = {
                    new OracleParameter("begindate",OracleType.DateTime),
                    new OracleParameter("enddate",OracleType.DateTime),
                    new OracleParameter("dwid",OracleType.VarChar),
                    new OracleParameter("v_cur",OracleType.Cursor)    
                    };
        param[0].Value = begindate;
        param[1].Value = enddate;
        param[2].Value = maindept;
        param[0].Direction = ParameterDirection.Input;
        param[1].Direction = ParameterDirection.Input;
        param[2].Direction = ParameterDirection.Input;
        param[3].Direction = ParameterDirection.Output;
        DataSet ds = OracleHelper.RunProcedure("MainLeaderYH.GetMainLeaderYH", param, "ds");
        return ds;
    }

    //领导首页-三违
    public static DataSet GetMainLeaderSW(DateTime begindate, DateTime enddate, string maindept)
    {
        OracleParameter[] param = {
                    new OracleParameter("begindate",OracleType.DateTime),
                    new OracleParameter("enddate",OracleType.DateTime),
                    new OracleParameter("deptid",OracleType.VarChar),
                    new OracleParameter("v_cur",OracleType.Cursor)    
                    };
        param[0].Value = begindate;
        param[1].Value = enddate;
        param[2].Value = maindept;
        param[0].Direction = ParameterDirection.Input;
        param[1].Direction = ParameterDirection.Input;
        param[2].Direction = ParameterDirection.Input;
        param[3].Direction = ParameterDirection.Output;
        DataSet ds = OracleHelper.RunProcedure("MainLeaderSW.GetMainLeaderSW", param, "ds");
        return ds;
    }
    #endregion
#region 集团领导首页
    //领导首页-隐患
    public static DataSet GetMainLeaderJTYH(DateTime begindate, DateTime enddate)
    {
        OracleParameter[] param = {
                    new OracleParameter("begindate",OracleType.DateTime),
                    new OracleParameter("enddate",OracleType.DateTime),
                    new OracleParameter("v_cur",OracleType.Cursor)    
                    };
        param[0].Value = begindate;
        param[1].Value = enddate;
        param[0].Direction = ParameterDirection.Input;
        param[1].Direction = ParameterDirection.Input;
        param[2].Direction = ParameterDirection.Output;
        DataSet ds = OracleHelper.RunProcedure("MainLeaderJTYH.GetMainLeaderJTYH", param, "ds");
        return ds;
    }

    //领导首页-三违
    public static DataSet GetMainLeaderJTSW(DateTime begindate, DateTime enddate)
    {
        OracleParameter[] param = {
                    new OracleParameter("begindate",OracleType.DateTime),
                    new OracleParameter("enddate",OracleType.DateTime),
                    new OracleParameter("v_cur",OracleType.Cursor)    
                    };
        param[0].Value = begindate;
        param[1].Value = enddate;
        param[0].Direction = ParameterDirection.Input;
        param[1].Direction = ParameterDirection.Input;
        param[2].Direction = ParameterDirection.Output;
        DataSet ds = OracleHelper.RunProcedure("MainLeaderJTSW.GetMainLeaderJTSW", param, "ds");
        return ds;
    }
    #endregion

}
