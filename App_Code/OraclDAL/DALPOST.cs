using System;
using System.Data;
using System.Text;
using System.Data.OracleClient;
using GhtnTech.SEP.DBUtility;

/// <summary>
///DALPOST 的摘要说明
/// </summary>
namespace GhtnTech.SEP.OraclDAL
{
    public class DALPOST
    {
        public DALPOST()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 获得岗位数据列表
        /// </summary>
        /// <param name="strWhere">Where条件</param>
        /// <param name="strOrder">排序条件</param>
        /// <returns></returns>
        public DataSet GetDALPOST(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * FROM POST");

            if (System.Web.HttpContext.Current.Session["zyID"] != null)
            {
                if (strWhere != "")
                {
                    strSql.Append(" where 1=1 and ZYID = " + int.Parse(System.Web.HttpContext.Current.Session["zyID"].ToString()) + strWhere);
                }
                else
                {
                    strSql.Append(" where 1=1 and ZYID = " + int.Parse(System.Web.HttpContext.Current.Session["zyID"].ToString()));
                }
            }

            return OracleHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 增加一个岗位
        /// </summary>
        /// <param name="model">岗位实体类</param>
        /// <returns></returns>
        public bool CreateDALPOST(Model.Post model)
        {
            int zy = int.Parse(System.Web.HttpContext.Current.Session["zyID"].ToString());

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into POST(");
            strSql.Append("POSTNAME,ZYID,SANXIANG,PTYPE,STATUS)");
            strSql.Append(" values(");
            strSql.Append(":POSTNAME,:ZYID,:SANXIANG,:PTYPE,:STATUS)");
            OracleParameter[] parameters = {
					new OracleParameter(":POSTNAME", OracleType.NVarChar,40),
                    new OracleParameter(":ZYID", OracleType.Number,9),
                    new OracleParameter(":SANXIANG",OracleType.NVarChar,4),
                    new OracleParameter(":PTYPE",OracleType.NVarChar,20),
                    new OracleParameter(":STATUS",OracleType.Char,10)};
            parameters[0].Value = model.POSTNAME;
            parameters[1].Value = zy;
            parameters[2].Value = model.SANXIANG;
            parameters[3].Value = model.PTYPE;
            parameters[4].Value = "有效";

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
        /// 更新岗位
        /// </summary>
        /// <param name="model">岗位实体类</param>
        /// <returns></returns>
        public bool UpdateDALPOST(Model.Post model)
        {
            bool bl = true;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update POST set ");
            strSql.Append("POSTNAME=:POSTNAME,");
            strSql.Append("SANXIANG=:SANXIANG, ");
            strSql.Append("PTYPE=:PTYPE ");
            strSql.Append(" where WORKTASKID=:WORKTASKID ");
            OracleParameter[] parameters = {
					new OracleParameter(":POSTNAME", OracleType.NVarChar,40),
                    new OracleParameter(":SANXIANG", OracleType.NVarChar,4),
                    new OracleParameter(":PTYPE", OracleType.NVarChar,20),
                    new OracleParameter(":POSTID", OracleType.Number,9),
                 };
            parameters[0].Value = model.POSTNAME;
            parameters[1].Value = model.SANXIANG;
            parameters[2].Value = model.PTYPE;
            parameters[3].Value = model.POSTID;
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
        /// 删除/禁用一个岗位
        /// </summary>
        /// <param name="OperatorID">岗位ID</param>
        /// <returns></returns>
        public bool DeleteDALPOST(Model.Post model)
        {
            bool bl = true;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete POST");
            strSql.Append(" where POSTID=:POSTID ");
            OracleParameter[] parameters = {
                new OracleParameter(":POSTID", OracleType.Number,9)};
            parameters[0].Value = model.POSTID;

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
