using System;
using System.Data;
using System.Text;
using System.Data.OracleClient;
using GhtnTech.SEP.DBUtility;

namespace GhtnTech.SEP.OraclDAL
{
    /// <summary>
    ///DALWORKTASKS_TEMP 的摘要说明
    /// </summary>
    public class DALWORKTASKS_TEMP
    {
        public DALWORKTASKS_TEMP()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 新增工作任务临时表数据
        /// </summary>
        /// <param name="strWhere">Where条件</param>
        /// <returns></returns>
        public bool InsertDALWORKTASKS_TEMP(string WORKTASK, int PROFESSIONALID, string DEPTNUMBER, string PERSONID)
        {
            bool bl = true;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into WORKTASKS_TEMP ");
            strSql.Append("(WORKTASK,PROFESSIONALID,STATUS,DEPTNUMBER,DATEINPUT,PERSONID) values");
            strSql.Append("(:WORKTASK,:PROFESSIONALID,:STATUS,:DEPTNUMBER,:DATEINPUT,:PERSONID)");
            OracleParameter[] parameters = {
					new OracleParameter(":WORKTASK", OracleType.NVarChar,600),
                    new OracleParameter(":PROFESSIONALID", OracleType.Number,9),
                    new OracleParameter(":STATUS", OracleType.NVarChar,10),
                    new OracleParameter(":DEPTNUMBER", OracleType.VarChar,9),
                    new OracleParameter(":DATEINPUT", OracleType.DateTime),
                    new OracleParameter(":PERSONID", OracleType.VarChar,20)
                 };
            parameters[0].Value = WORKTASK;
            parameters[1].Value = PROFESSIONALID;
            parameters[2].Value = "保存";
            parameters[3].Value = DEPTNUMBER;
            parameters[4].Value = System.DateTime.Now;
            parameters[5].Value = PERSONID;

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
