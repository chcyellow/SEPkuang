using System;
using System.Data;
using System.Text;
using System.Data.OracleClient;
using GhtnTech.SEP.DBUtility;


/// <summary>
///DALWORKTASK 的摘要说明
/// </summary>
namespace GhtnTech.SEP.OraclDAL
{
    public class DALWORKTASK
    {
        public DALWORKTASK()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }


        /// <summary>
        /// 获得工作任务数据列表
        /// </summary>
        /// <param name="strWhere">Where条件</param>
        /// <param name="strOrder">排序条件</param>
        /// <returns></returns>
        public DataSet GetDALWORKTASKS(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * FROM WORKTASKS");

            if (System.Web.HttpContext.Current.Session["zyID"] != null)
            {
                if (strWhere != "")
                {
                    strSql.Append(" where 1=1 and PROFESSIONALID = " + int.Parse(System.Web.HttpContext.Current.Session["zyID"].ToString()) + strWhere);
                }
                else
                {
                    strSql.Append(" where 1=1 and PROFESSIONALID = " + int.Parse(System.Web.HttpContext.Current.Session["zyID"].ToString()));
                }
            }

            return OracleHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 增加一个工作任务
        /// </summary>
        /// <param name="model">工作任务实体类</param>
        /// <returns></returns>
        public bool CreateDALWORKTASKS(Model.WORKTASKS model)
        {
            int zy = int.Parse(System.Web.HttpContext.Current.Session["zyID"].ToString());

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into WORKTASKS(");
            strSql.Append("WORKTASK,PROFESSIONALID,TASK_NUMBER)");
            strSql.Append(" values(");
            strSql.Append(":WORKTASK,:PROFESSIONALID,:TASK_NUMBER)");
            OracleParameter[] parameters = {
					new OracleParameter(":WORKTASK", OracleType.NVarChar,400),
                    new OracleParameter(":PROFESSIONALID", OracleType.Number,9),
                    new OracleParameter(":TASK_NUMBER",OracleType.NVarChar,8)};
            parameters[0].Value = model.WORKTASK;
            parameters[1].Value = zy;
            parameters[2].Value = PublicMethod.GetWORKTASKSNO(zy.ToString());

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
        /// 更新工作任务
        /// </summary>
        /// <param name="model">工作任务实体类</param>
        /// <returns></returns>
        public bool UpdateDALWORKTASKS(Model.WORKTASKS model)
        {
            bool bl = true;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update WORKTASKS set ");
            strSql.Append("WORKTASK=:WORKTASK,");
            //strSql.Append("TASK_NUMBER=:TASK_NUMBER ");
            strSql.Append(" where WORKTASKID=:WORKTASKID ");
            OracleParameter[] parameters = {
					new OracleParameter(":WORKTASK", OracleType.NVarChar,200),
                    //new OracleParameter(":TASK_NUMBER", OracleType.NVarChar,8),
                    new OracleParameter(":WORKTASKID", OracleType.Number,9),
                 };
            parameters[0].Value = model.WORKTASK;
            //parameters[1].Value = model.TASK_NUMBER;
            parameters[1].Value = model.WORKTASKID;
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
        /// 删除/禁用一个工作任务
        /// </summary>
        /// <param name="OperatorID">工作任务ID</param>
        /// <returns></returns>
        public bool DeleteDALWORKTASKS(Model.WORKTASKS model)
        {
            bool bl = true;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete WORKTASKS");
            strSql.Append(" where WORKTASKID=:WORKTASKID ");
            OracleParameter[] parameters = {
                new OracleParameter(":WORKTASKID", OracleType.Number,9)};
            parameters[0].Value = model.WORKTASKID;

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
