﻿using System;
using System.Data;
using System.Text;
using System.Data.OracleClient;
using GhtnTech.SEP.DBUtility;


namespace GhtnTech.SEP.OraclDAL
{
    /// <summary>
    ///DALPROCESS 的摘要说明
    /// </summary>
    public class DALPROCESS
    {
        public DALPROCESS()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 获得工序数据列表
        /// </summary>
        /// <param name="strWhere">Where条件</param>
        /// <param name="strOrder">排序条件</param>
        /// <returns></returns>
        public DataSet GetDALPROCESS(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * FROM PROCESS");

            if (System.Web.HttpContext.Current.Session["gzrwID"] != null)
            {
                if (strWhere != "")
                {
                    strSql.Append(" where 1=1 and WORKTASKID = " + int.Parse(System.Web.HttpContext.Current.Session["gzrwID"].ToString()) + strWhere);
                }
                else
                {
                    strSql.Append(" where 1=1 and WORKTASKID = " + int.Parse(System.Web.HttpContext.Current.Session["gzrwID"].ToString()));
                }
            }
            strSql.Append(" order by SERIALNUMBER,PROCESSID");

            return OracleHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 增加一个工序
        /// </summary>
        /// <param name="model">工序实体类</param>
        /// <returns></returns>
        public bool CreateDALPROCESS(Model.PROCESS model)
        {
            //传参-专业与工作任务
            int gzrw = int.Parse(System.Web.HttpContext.Current.Session["gzrwID"].ToString());
            int zy = int.Parse(System.Web.HttpContext.Current.Session["zyID"].ToString());

            //获取数据库中已经存在的SERIALNUMBER的最大值
            DataSet ds = OracleHelper.Query("select SERIALNUMBER from (select SERIALNUMBER from PROCESS where WORKTASKID='" + gzrw + "' order by SERIALNUMBER desc nulls last) where ROWNUM = 1");

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into PROCESS(");
            strSql.Append("NAME,WORKTASKID,PROFESSIONALID,DANWEIID,ISOMUX,SERIALNUMBER)");
            strSql.Append(" values(");
            strSql.Append(":NAME,:WORKTASKID,:PROFESSIONALID,:DANWEIID,:ISOMUX,:SERIALNUMBER)");
            OracleParameter[] parameters = {
					new OracleParameter(":NAME", OracleType.NVarChar,200),
					new OracleParameter(":WORKTASKID", OracleType.Number,9),
                    new OracleParameter(":PROFESSIONALID", OracleType.Number,9),
                    new OracleParameter(":DANWEIID",OracleType.Number,10),
                    new OracleParameter(":ISOMUX",OracleType.NVarChar,18),
                    new OracleParameter(":SERIALNUMBER",OracleType.Number,9)};
            parameters[0].Value = model.NAME;
            parameters[1].Value = gzrw;
            parameters[2].Value = zy;
            parameters[3].Value = 200000000;
            parameters[4].Value = PublicMethod.GetPROCESSNO(gzrw.ToString());
            try
            {
                parameters[5].Value = int.Parse(ds.Tables[0].Rows[0]["SERIALNUMBER"].ToString()) + 1;
            }
            catch
            {
                parameters[5].Value = 1;
            }
            
            if (OracleHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 更新工序
        /// </summary>
        /// <param name="model">工序实体类</param>
        /// <returns></returns>
        public bool UpdateDALPROCESS(Model.PROCESS model)
        {
            bool bl = true;
           
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update PROCESS set ");
            strSql.Append("NAME=:NAME,");
            //strSql.Append("ISOMUX=:ISOMUX");
            strSql.Append(" where PROCESSID=:PROCESSID ");
            OracleParameter[] parameters = {
				new OracleParameter(":NAME", OracleType.NVarChar,200),
                //new OracleParameter(":ISOMUX", OracleType.NVarChar,200),
                new OracleParameter(":PROCESSID", OracleType.Number,9),
             };
            parameters[0].Value = model.NAME;
            //parameters[1].Value = model.ISOMUX;
            parameters[1].Value = model.PROCESSID;
            if (OracleHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                bl = true;
            }
            else
            {
                bl = false;
            }

            return bl;
        }

        /// <summary>
        /// 更新工序排序
        /// </summary>
        /// <param name="model">工序实体类</param>
        /// <returns></returns>
        public bool UpdateDALPROCESS_SERIALNUMBER(int ID,int sort)
        {
            bool bl = true;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update PROCESS set ");
            strSql.Append("SERIALNUMBER=:SERIALNUMBER");
            strSql.Append(" where PROCESSID=:PROCESSID ");
            OracleParameter[] parameters = {
					new OracleParameter(":SERIALNUMBER", OracleType.Number,10),
                    new OracleParameter(":PROCESSID", OracleType.Number,9),
                 };
            parameters[0].Value = sort;
            parameters[1].Value = ID;
            if (OracleHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                bl = true;
            }
            else
            {
                bl = false;
            }

            return bl;
        }


        /// <summary>
        /// 删除/禁用一个工序
        /// </summary>
        /// <param name="OperatorID">工序ID</param>
        /// <returns></returns>
        public bool DeleteDALPROCESS(Model.PROCESS model)
        {
            bool bl = true;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete PROCESS");
            strSql.Append(" where PROCESSID=:PROCESSID ");
            OracleParameter[] parameters = {
            new OracleParameter(":PROCESSID", OracleType.Number,9)};
            parameters[0].Value = model.PROCESSID;

            if (OracleHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                bl = true;
            }
            else
            {
                bl = false;
            }

            return bl;
        }
    }
    
}
