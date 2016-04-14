using System;
using System.Data;
using System.Text;
using System.Data.OracleClient;
using GhtnTech.SEP.DBUtility;

/// <summary>
///DALM_OBJECT 的摘要说明
/// </summary>
namespace GhtnTech.SEP.OraclDAL
{
    public class DALM_OBJECT
    {
        public DALM_OBJECT()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 获得管理对象数据列表
        /// </summary>
        /// <param name="strWhere">Where条件</param>
        /// <param name="strOrder">排序条件</param>
        /// <returns></returns>
        public DataSet GetDALM_OBJECT(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * FROM M_OBJECT where 1=1");

            if (System.Web.HttpContext.Current.Session["fxlxID"] != null)
            {
                if (strWhere != "")
                {
                    strSql.Append(" and RISK_TYPESID = " + int.Parse(System.Web.HttpContext.Current.Session["fxlxID"].ToString()) + strWhere);
                }
                else
                {
                    strSql.Append(" and RISK_TYPESID = " + int.Parse(System.Web.HttpContext.Current.Session["fxlxID"].ToString()));
                }
            }
            //if (strWhere.Trim() != "")
            //{
            //    strSql.Append(" where 1=1 " + strWhere);
            //}

            return OracleHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 增加一个管理对象
        /// </summary>
        /// <param name="model">工序实体类</param>
        /// <returns></returns>
        public bool CreateDALM_OBJECT(Model.M_OBJECT model)
        {
            int fxlx = int.Parse(System.Web.HttpContext.Current.Session["fxlxID"].ToString());
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into M_OBJECT(");
            strSql.Append("NAME,RISK_TYPESID)");
            strSql.Append(" values(");
            strSql.Append(":NAME,:RISK_TYPESID)");
            OracleParameter[] parameters = {
					new OracleParameter(":NAME", OracleType.NVarChar,200),
                    new OracleParameter(":RISK_TYPESID", OracleType.Number,9)
                   
					};
            parameters[0].Value = model.NAME;
            parameters[1].Value = fxlx;

            if (OracleHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                OracleHelper.Query("update M_OBJECT set MO_F_PINYIN=F_PINYIN('" + model.NAME + "') where NAME='" + model.NAME + "' and MO_F_PINYIN is null");
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 更新管理对象
        /// </summary>
        /// <param name="model">工序实体类</param>
        /// <returns></returns>
        public bool UpdateDALM_OBJECT(Model.M_OBJECT model)
        {
            bool bl = true;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update M_OBJECT set ");
            strSql.Append(" NAME=:NAME ");
            strSql.Append(" where M_OBJECTID=:M_OBJECTID ");
            OracleParameter[] parameters = {
					new OracleParameter(":NAME", OracleType.NVarChar,200),
                    new OracleParameter(":M_OBJECTID", OracleType.Number,9)};
            parameters[0].Value = model.NAME;
            parameters[1].Value = model.M_OBJECTID;
            if (OracleHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                OracleHelper.Query("update M_OBJECT set MO_F_PINYIN=F_PINYIN('" + model.NAME + "') where m_Objectid=" + model.M_OBJECTID);
                bl = true;
            }
            else
            {
                bl = false;
            }

            return bl;
        }


        /// <summary>
        /// 删除/禁用一个管理对象
        /// </summary>
        /// <param name="OperatorID">工序ID</param>
        /// <returns></returns>
        public bool DeleteDALM_OBJECT(Model.M_OBJECT model)
        {
            bool bl = true;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete M_OBJECT");
            strSql.Append(" where M_OBJECTID=:M_OBJECTID ");
            OracleParameter[] parameters = {
                new OracleParameter(":M_OBJECTID", OracleType.Number,9)};
            parameters[0].Value = model.M_OBJECTID;

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
