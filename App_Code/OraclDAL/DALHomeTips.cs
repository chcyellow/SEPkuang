using System;
using System.Data;
using System.Text;
using System.Data.OracleClient;
using GhtnTech.SEP.DBUtility;

namespace GhtnTech.SEP.OraclDAL
{
    /// <summary>
    ///DALHomeTips 的摘要说明
    /// </summary>
    public class DALHomeTips
    {
        public DALHomeTips()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        ///<summary>
        ///获取（新增/提交/发布/（提交单位、审核角色））审核通过/审核不通过危险源条数
        /// </summary>
        /// <param name="DeptNumber">部门参数</param>
        /// <param name="IsPass">主体信息表状态参数</param>
        /// <returns>条数</returns>
        public int GetAddedOrSubmittedOrPublishedCount(string DeptNumber, string IsPass)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from HAZARDS_TEMP where 1 = 1");
            if (DeptNumber != "")
            {
                strSql.Append(" and DEPTNUMBER = '" + DeptNumber + "'");
            }
            if (IsPass != "")
            {
                strSql.Append(" and ISPASS = '" + IsPass + "'");
            }
            DataSet ds = OracleHelper.Query(strSql.ToString());
            return ds.Tables[0].Rows.Count > 0 ? ds.Tables[0].Rows.Count : 0;
        }

        ///<summary>
        ///审核角色获取审核流程危险源条数
        /// </summary>
        /// <param name="DeptNumber">审核角色</param>
        /// <param name="IsPass">主体信息表状态参数</param>
        /// <param name="Pass">审核表状态参数</param>
        /// <returns>条数</returns>
        public int GetAuditCount(string Role, string IsPass, string Pass)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select HAZARDS_TEMP.HAZARDSTEMPID from HAZARDS_TEMP inner join AUDITHAZARD on HAZARDS_TEMP.HAZARDSTEMPID = AUDITHAZARD.HAZARDSID where 1 = 1 ");
            if (Role != "")
            {
                strSql.Append(" and AUDITHAZARD.DEPT = " + Role);
            }
            if (IsPass != "")
            {
                strSql.Append(" and HAZARDS_TEMP.ISPASS = " + IsPass);
            }
            if (Pass != "")
            {
                strSql.Append(" and AUDITHAZARD.PASS = " + Pass);
            }
            strSql.Append(" group by HAZARDS_TEMP.HAZARDSTEMPID");
            DataSet ds = OracleHelper.Query(strSql.ToString());
            return ds.Tables[0].Rows.Count > 0 ? ds.Tables[0].Rows.Count : 0;
        }
    }
}
