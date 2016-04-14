using System;
using System.Data;
using System.Text;
using System.Data.OracleClient;
using GhtnTech.SEP.DBUtility;

namespace GhtnTech.SEP.OraclDAL
{

    /// <summary>
    ///DALAUDITupdate 的摘要说明
    /// </summary>
    public class DALAUDITupdate
    {
        public DALAUDITupdate()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 更新审核表
        /// </summary>
        /// <param name="strWhere">Where条件</param>
        /// <returns></returns>
        public bool UpdateAUDIT(string OPINION, string AUDITPERSONID, string PASS, string ID, string DEPT)
        {
            bool bl = true;
            string sql = string.Format("update AUDITHAZARD set OPINION='{0}',AUDITPERSONID='{1}',PASS='{2}',AUDITDATE=sysdate where HAZARDSID={3} and DEPT in ({4})", OPINION, AUDITPERSONID, PASS, ID, DEPT);
            //StringBuilder strSql = new StringBuilder();
            //strSql.Append("update AUDITHAZARD set ");
            //strSql.Append("OPINION=:OPINION,");
            //strSql.Append("AUDITPERSONID=:AUDITPERSONID,");
            //strSql.Append("PASS=:PASS,");
            //strSql.Append("AUDITDATE=:AUDITDATE");
            //strSql.Append(" where HAZARDSID=:HAZARDSID and DEPT=:DEPT");
            //OracleParameter[] parameters = {
            //        new OracleParameter(":OPINION", OracleType.NVarChar,200),
            //        new OracleParameter(":AUDITPERSONID", OracleType.Number,9),
            //        new OracleParameter(":PASS", OracleType.NVarChar,20),
            //        new OracleParameter(":AUDITDATE", OracleType.Number,9),
            //        new OracleParameter(":HAZARDSID", OracleType.Number,9),
            //        new OracleParameter(":DEPT", OracleType.VarChar,10),
            //     };
            //parameters[0].Value = OPINION;
            //parameters[1].Value = AUDITPERSONID;
            //parameters[2].Value = PASS;
            //parameters[3].Value = "sysdate";
            //parameters[4].Value = int.Parse(ID);
            //parameters[5].Value = DEPT;

            if (OracleHelper.ExecuteSql(sql) >= 1)
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