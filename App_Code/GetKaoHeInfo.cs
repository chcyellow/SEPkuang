using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using GhtnTech.SecurityFramework.DBUtility;

/// <summary>
///GetKaoHeInfo 的摘要说明
/// </summary>
public class GetKaoHeInfo
{
    public GetKaoHeInfo()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }

    //个人排查考核统计-调用存储过程
    public static DataSet getallyhswcountbyper(DateTime begindate, DateTime enddate,string deptid)
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
        DataSet ds = OracleHelper.RunProcedure("PCpersonKH.getallyhswcountbyper", param, "ds");
        return ds;
    }

    //个人三违积分统计-调用存储过程
    public static DataSet GetAllPerSWCount(DateTime begindate, DateTime enddate, string deptid)
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
        DataSet ds = OracleHelper.RunProcedure("PersonSWKH.GETALLPERSWCOUNT", param, "ds");
        return ds;
    }

    //部门隐患积分统计-调用存储过程
    public static DataSet GetAllYHCountByDEPT(DateTime begindate, DateTime enddate, string deptid)
    {
        OracleParameter[] param = {
                    new OracleParameter("begindate",OracleType.DateTime),
                    new OracleParameter("enddate",OracleType.DateTime),
                    new OracleParameter("dwid",OracleType.VarChar),
                    new OracleParameter("v_cur",OracleType.Cursor)    
                    };
        param[0].Value = begindate;
        param[1].Value = enddate;
        param[2].Value = deptid;
        param[0].Direction = ParameterDirection.Input;
        param[1].Direction = ParameterDirection.Input;
        param[2].Direction = ParameterDirection.Input;
        param[3].Direction = ParameterDirection.Output;
        DataSet ds = OracleHelper.RunProcedure("deptYHKH.getAllYHcountbyDEPT", param, "ds");
        return ds;
    }

    //部门三违积分统计-调用存储过程
    public static DataSet GetAllSWCountByDEPT(DateTime begindate, DateTime enddate, string deptid)
    {
        OracleParameter[] param = {
                    new OracleParameter("begindate",OracleType.DateTime),
                    new OracleParameter("enddate",OracleType.DateTime),
                    new OracleParameter("dwid",OracleType.VarChar),
                    new OracleParameter("v_cur",OracleType.Cursor)    
                    };
        param[0].Value = begindate;
        param[1].Value = enddate;
        param[2].Value = deptid;
        param[0].Direction = ParameterDirection.Input;
        param[1].Direction = ParameterDirection.Input;
        param[2].Direction = ParameterDirection.Input;
        param[3].Direction = ParameterDirection.Output;
        DataSet ds = OracleHelper.RunProcedure("deptSWKH.getAllSWcountbyDEPT", param, "ds");
        return ds;
    }

    //新！得到隐患积分
    public static DataSet GetDeptYHPoint(DateTime begindate, DateTime enddate, string deptid,string kequ)
    {
        OracleParameter[] param = {
                    new OracleParameter("begindate",OracleType.DateTime),
                    new OracleParameter("enddate",OracleType.DateTime),
                    new OracleParameter("dwid",OracleType.VarChar),
                    new OracleParameter("kequ",OracleType.VarChar),
                    new OracleParameter("v_cur",OracleType.Cursor)    
                    };
        param[0].Value = begindate;
        param[1].Value = enddate;
        param[2].Value = deptid;
        param[3].Value = kequ;
        param[0].Direction = ParameterDirection.Input;
        param[1].Direction = ParameterDirection.Input;
        param[2].Direction = ParameterDirection.Input;
        param[3].Direction = ParameterDirection.Input;
        param[4].Direction = ParameterDirection.Output;
        DataSet ds = OracleHelper.RunProcedure("DeptYHPoint.getDeptYHPoint", param, "ds");
        return ds;
    }


    //新！得到隐患罚款
    public static DataSet GetDeptYHFine(DateTime begindate, DateTime enddate, string deptid,string kequ)
    {
        OracleParameter[] param = {
                    new OracleParameter("begindate",OracleType.DateTime),
                    new OracleParameter("enddate",OracleType.DateTime),
                    new OracleParameter("dwid",OracleType.VarChar),
                    new OracleParameter("kequ",OracleType.VarChar),
                    new OracleParameter("v_cur",OracleType.Cursor) 
                    };
        param[0].Value = begindate;
        param[1].Value = enddate;
        param[2].Value = deptid;
        param[3].Value = kequ;
        param[0].Direction = ParameterDirection.Input;
        param[1].Direction = ParameterDirection.Input;
        param[2].Direction = ParameterDirection.Input;
        param[3].Direction = ParameterDirection.Input;
        param[4].Direction = ParameterDirection.Output;
        DataSet ds = OracleHelper.RunProcedure("DeptYHFine.getDeptYHFine", param, "ds");
        return ds;
    }

    //新！得到三违积分
    public static DataSet GetPersonSWPoint(DateTime begindate, DateTime enddate, string deptid,string psn)
    {
        OracleParameter[] param = {
                    new OracleParameter("begindate",OracleType.DateTime),
                    new OracleParameter("enddate",OracleType.DateTime),
                    new OracleParameter("deptid",OracleType.VarChar),
                    new OracleParameter("psn",OracleType.VarChar),
                    new OracleParameter("v_cur",OracleType.Cursor)
                    };
        param[0].Value = begindate;
        param[1].Value = enddate;
        param[2].Value = deptid;
        param[3].Value = psn;
        param[0].Direction = ParameterDirection.Input;
        param[1].Direction = ParameterDirection.Input;
        param[2].Direction = ParameterDirection.Input;
        param[3].Direction = ParameterDirection.Input;
        param[4].Direction = ParameterDirection.Output;
        DataSet ds = OracleHelper.RunProcedure("PersonSWPoint.getPersonSWPoint", param, "ds");
        return ds;
    }

    //新！得到三违罚款
    public static DataSet GetPersonSWFine(DateTime begindate, DateTime enddate, string deptid,string psn)
    {
        OracleParameter[] param = {
                    new OracleParameter("begindate",OracleType.DateTime),
                    new OracleParameter("enddate",OracleType.DateTime),
                    new OracleParameter("deptid",OracleType.VarChar),
                    new OracleParameter("psn",OracleType.VarChar),
                    new OracleParameter("v_cur",OracleType.Cursor)    
                    };
        param[0].Value = begindate;
        param[1].Value = enddate;
        param[2].Value = deptid;
        param[3].Value = psn;
        param[0].Direction = ParameterDirection.Input;
        param[1].Direction = ParameterDirection.Input;
        param[2].Direction = ParameterDirection.Input;
        param[3].Direction = ParameterDirection.Input;
        param[4].Direction = ParameterDirection.Output;
        DataSet ds = OracleHelper.RunProcedure("PersonSWFine.getPersonSWFine", param, "ds");
        return ds;
    }

    //得到用户登录统计
    public static DataSet GetUserLoginTotal(DateTime begindate, DateTime enddate, string maindept, string deptid, string psn)
    {
        OracleParameter[] param = {
                    new OracleParameter("begindate",OracleType.DateTime),
                    new OracleParameter("enddate",OracleType.DateTime),
                    new OracleParameter("maindept",OracleType.VarChar),
                    new OracleParameter("deptid",OracleType.VarChar),
                    new OracleParameter("psn",OracleType.VarChar),
                    new OracleParameter("v_cur",OracleType.Cursor)    
                    };
        param[0].Value = begindate;
        param[1].Value = enddate;
        param[2].Value = maindept;
        param[3].Value = deptid;
        param[4].Value = psn;
        param[0].Direction = ParameterDirection.Input;
        param[1].Direction = ParameterDirection.Input;
        param[2].Direction = ParameterDirection.Input;
        param[3].Direction = ParameterDirection.Input;
        param[4].Direction = ParameterDirection.Input;
        param[5].Direction = ParameterDirection.Output;
        DataSet ds = OracleHelper.RunProcedure("userlogintotal.getUserLoginTotal", param, "ds");
        return ds;
    }

    //得到用户操作统计
    public static DataSet GetUserOptTotal(DateTime begindate, DateTime enddate, string maindept, string deptid, string psn)
    {
        OracleParameter[] param = {
                    new OracleParameter("begindate",OracleType.DateTime),
                    new OracleParameter("enddate",OracleType.DateTime),
                    new OracleParameter("maindept",OracleType.VarChar),
                    new OracleParameter("deptid",OracleType.VarChar),
                    new OracleParameter("psn",OracleType.VarChar),
                    new OracleParameter("v_cur",OracleType.Cursor)    
                    };
        param[0].Value = begindate;
        param[1].Value = enddate;
        param[2].Value = maindept;
        param[3].Value = deptid;
        param[4].Value = psn;
        param[0].Direction = ParameterDirection.Input;
        param[1].Direction = ParameterDirection.Input;
        param[2].Direction = ParameterDirection.Input;
        param[3].Direction = ParameterDirection.Input;
        param[4].Direction = ParameterDirection.Input;
        param[5].Direction = ParameterDirection.Output;
        DataSet ds = OracleHelper.RunProcedure("useropttotal.getUseroptTotal", param, "ds");
        return ds;
    }
}
