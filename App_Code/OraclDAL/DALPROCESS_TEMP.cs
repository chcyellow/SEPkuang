using System;
using System.Data;
using System.Text;
using System.Data.OracleClient;
using GhtnTech.SEP.DBUtility;


namespace GhtnTech.SEP.OraclDAL
{
    /// <summary>
    ///DALPROCESS 的摘要说明
    /// </summary>
    public class DALPROCESS_TEMP
    {
        public DALPROCESS_TEMP()
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
        public DataSet GetDALPROCESS_TEMP(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * FROM PROCESS_TEMP ");

            if (System.Web.HttpContext.Current.Session["gzrwID"] != null)
            {
                if (strWhere != "")
                {
                    strSql.Append(" where STATUS='保存' and WORKTASKID = " + int.Parse(System.Web.HttpContext.Current.Session["gzrwID"].ToString()) + strWhere);
                }
                else
                {
                    strSql.Append(" where STATUS='保存' and WORKTASKID = " + int.Parse(System.Web.HttpContext.Current.Session["gzrwID"].ToString()));
                }
            }
            strSql.Append(" order by PROCESSID");

            return OracleHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 增加一个工序
        /// </summary>
        /// <param name="model">工序实体类</param>
        /// <returns></returns>
        public bool CreateDALPROCESS_TEMP(Model.PROCESS model)
        {
            //传参-专业与工作任务
            int gzrw = int.Parse(System.Web.HttpContext.Current.Session["gzrwID"].ToString());

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into PROCESS_TEMP (");
            strSql.Append("NAME,WORKTASKID,STATUS,DEPTNUMBER,DATEINPUT,PERSONID)");
            strSql.Append(" values(");
            strSql.Append(":NAME,:WORKTASKID,:STATUS,:DEPTNUMBER,:DATEINPUT,:PERSONID)");
            OracleParameter[] parameters = {
					new OracleParameter(":NAME", OracleType.NVarChar,600),
					new OracleParameter(":WORKTASKID", OracleType.Number,9),
                    new OracleParameter(":STATUS", OracleType.NVarChar,10),
                    new OracleParameter(":DEPTNUMBER", OracleType.NVarChar,20),
                    new OracleParameter(":DATEINPUT",OracleType.DateTime),
                    new OracleParameter(":PERSONID",OracleType.Number,9)};
            parameters[0].Value = model.NAME;
            parameters[1].Value = gzrw;
            parameters[2].Value = "保存";
            parameters[3].Value = "000000000";
            parameters[4].Value = System.DateTime.Now;
            parameters[5].Value = "343";
            
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
        public bool UpdateDALPROCESS_TEMP(Model.PROCESS model)
        {
            bool bl = true;
           
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update PROCESS_TEMP set ");
            strSql.Append("NAME=:NAME");
            strSql.Append(" where PROCESSID=:PROCESSID ");
            OracleParameter[] parameters = {
				new OracleParameter(":NAME", OracleType.NVarChar,600),
                new OracleParameter(":PROCESSID", OracleType.Number,9),
             };
            parameters[0].Value = model.NAME;
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
        /// 删除/禁用一个工序
        /// </summary>
        /// <param name="OperatorID">工序ID</param>
        /// <returns></returns>
        public bool DeleteDALPROCESS_TEMP(Model.PROCESS model)
        {
            bool bl = true;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete PROCESS_TEMP");
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
