using System;
using System.Data;
using System.Text;
using System.Data.OracleClient;
using GhtnTech.SEP.DBUtility;

namespace GhtnTech.SEP.OraclDAL
{
    /// <summary>
    ///DALHAZARDS_Pending 的摘要说明
    /// </summary>
    public class DALHAZARDS_Pending
    {
        public DALHAZARDS_Pending()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 获取危险源全连接信息-待办事宜
        /// </summary>
        /// <param name="strWhere">Where条件</param>
        /// <returns></returns>
        public DataSet GetHazards_Pending(string strWhere)
        {
            //获取新增工作任务、工序下的危险源信息
            string TstrSql = "select HAZARDSTEMPID,H_CONTENT,H_CONSEQUENCES,ISPASS,ISFROMTEMP,cs_baseinfoset.infoname BSDY,worktasks_temp.worktask GZRW,process_temp.name GX,a.infoname FXLB,b.infoname FXDJ,c.infoname SGLX from hazards_temp inner join process_temp on hazards_temp.processnumber=process_temp.processid inner join worktasks_temp on process_temp.worktaskid=worktasks_temp.worktaskid inner join cs_baseinfoset on worktasks_temp.professionalid=cs_baseinfoset.infoid inner join cs_baseinfoset a on hazards_temp.risk_typesnumber=a.infoid inner join cs_baseinfoset b on hazards_temp.risk_evelnumber=b.infoid inner join cs_baseinfoset c on hazards_temp.accident_typenumber=c.infoid where ISFROMTEMP='T' ";
            //获取原有工作任务、工序下的危险源信息
            string FstrSql = "select hazardstempid,H_CONTENT,H_CONSEQUENCES,ISPASS,ISFROMTEMP,cs_baseinfoset.infoname bsdy,worktasks.worktask gzrw,process.name gx,a.infoname fxlb,b.infoname fxdj,c.infoname sglx from hazards_temp inner join process on hazards_temp.processnumber=process.processid inner join worktasks on process.worktaskid=worktasks.worktaskid inner join cs_baseinfoset on worktasks.professionalid=cs_baseinfoset.infoid inner join cs_baseinfoset a on hazards_temp.risk_typesnumber=a.infoid inner join cs_baseinfoset b on hazards_temp.risk_evelnumber=b.infoid inner join cs_baseinfoset c on hazards_temp.accident_typenumber=c.infoid where ISFROMTEMP='F' ";
            if (strWhere.Trim() != "")
            {
                TstrSql += strWhere;
                FstrSql += strWhere;
            }

            StringBuilder strSql = new StringBuilder();
            strSql.Append("(" + TstrSql + ")");
            strSql.Append(" union ");
            strSql.Append("(" + FstrSql + ")");
            return OracleHelper.Query(strSql.ToString());
        }
    }
}
