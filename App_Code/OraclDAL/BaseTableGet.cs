using System;
using System.Data;
using System.Text;
using System.Data.OracleClient;
using GhtnTech.SEP.DBUtility;

namespace GhtnTech.SEP.OraclDAL
{
    /// <summary>
    ///BaseTableGet 的摘要说明
    /// </summary>
    public class BaseTableGet
    {
        public BaseTableGet()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        private static string strHAZARDSEasyView = "select HAZARDSID,a.infoname ZYNAME,worktasks.worktask GZRWNAME,process.name GXNAME,H_CONTENT,b.infoname FXLX,H_CONSEQUENCES,c.infoname SGLX,ISPASS,'A' ISFROM,'通用危险源' ISGROUP from HAZARDS inner join process on HAZARDS.PROCESSID=process.processid inner join worktasks on process.worktaskid=worktasks.worktaskid inner join CS_BASEINFOSET a on worktasks.professionalid=a.infoid inner join CS_BASEINFOSET b on HAZARDS.Risk_Typesnumber=b.infoid inner join CS_BASEINFOSET c on HAZARDS.Accident_Typenumber=c.infoid ";
        private static string strHAZARDS_TEMPEasyViewHelf = "select HAZARDSTEMPID HAZARDSID,a.infoname ZYNAME,worktasks.worktask GZRWNAME,process.name GXNAME,H_CONTENT,b.infoname FXLX,H_CONSEQUENCES,c.infoname SGLX,ISPASS,'H' ISFROM,'新增危险源' ISGROUP from HAZARDS_TEMP inner join process on HAZARDS_TEMP.PROCESSNUMBER=process.processid inner join worktasks on process.worktaskid=worktasks.worktaskid inner join CS_BASEINFOSET a on worktasks.professionalid=a.infoid inner join CS_BASEINFOSET b on HAZARDS_TEMP.Risk_Typesnumber=b.infoid inner join CS_BASEINFOSET c on HAZARDS_TEMP.Accident_Typenumber=c.infoid left join AUDITHAZARD on HAZARDS_TEMP.HAZARDSTEMPID=AUDITHAZARD.HAZARDSID where ISPASS !='已发布' ";
        private static string strHAZARDS_TEMPEasyViewAll = "select HAZARDSTEMPID HAZARDSID,a.infoname ZYNAME,WORKTASKS_TEMP.worktask GZRWNAME,PROCESS_TEMP.name GXNAME,H_CONTENT,b.infoname FXLX,H_CONSEQUENCES,c.infoname SGLX,ISPASS,'N' ISFROM,'新增危险源' ISGROUP from HAZARDS_TEMP inner join PROCESS_TEMP on HAZARDS_TEMP.PROCESSNUMBER=PROCESS_TEMP.PROCESSID inner join WORKTASKS_TEMP on PROCESS_TEMP.worktaskid=WORKTASKS_TEMP.worktaskid inner join CS_BASEINFOSET a on WORKTASKS_TEMP.professionalid=a.infoid inner join CS_BASEINFOSET b on HAZARDS_TEMP.Risk_Typesnumber=b.infoid inner join CS_BASEINFOSET c on HAZARDS_TEMP.Accident_Typenumber=c.infoid left join AUDITHAZARD on HAZARDS_TEMP.HAZARDSTEMPID=AUDITHAZARD.HAZARDSID where ISPASS !='已发布' ";
        private static string strHAZARDSUsingView = "select HAZARDSID,a.infoname ZYNAME,worktasks.worktask GZRWNAME,process.name GXNAME,H_CONTENT,b.infoname FXLX,H_CONSEQUENCES,c.infoname SGLX,HAZARDS.ISPASS,(case nvl(d.id,-1) when -1 then  '通用' else '已引用' end) ISGROUP from HAZARDS inner join process on HAZARDS.PROCESSID=process.processid inner join worktasks on process.worktaskid=worktasks.worktaskid inner join CS_BASEINFOSET a on worktasks.professionalid=a.infoid inner join CS_BASEINFOSET b on HAZARDS.Risk_Typesnumber=b.infoid inner join CS_BASEINFOSET c on HAZARDS.Accident_Typenumber=c.infoid left join (select * from HAZARDS_ORE where HAZARDS_ORE.DEPTNUMBER='{0}') d on HAZARDS.h_Number=d.H_NUMBER ";

        /// <summary>
        /// 获取通用危险源与新增危险源
        /// </summary>
        /// <param name="strWhere">Where条件</param>
        /// <returns></returns>
        public DataSet GetHAZARDSForAddNew(string strWhere, string kind,string processid)
        {
            StringBuilder strSql = new StringBuilder();
            if (kind == "T")//上级属于临时表
            {
                strSql.Append(strHAZARDS_TEMPEasyViewAll + " and HAZARDS_TEMP.PROCESSNUMBER='" + processid + "' " + strWhere);
            }
            else
            {
                string sql = "( " + strHAZARDSEasyView + " where HAZARDS.PROCESSID='" + processid + "' )";
                string sql2 = "( " + strHAZARDS_TEMPEasyViewHelf + " and HAZARDS_TEMP.PROCESSNUMBER='" + processid + "' " +strWhere+ " )";

                strSql.Append(sql + " union " + sql2);
            }

            return OracleHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取通用危险源引用信息
        /// </summary>
        /// <param name="strWhere">Where条件</param>
        /// <returns></returns>
        public DataSet GetHAZARDSForUsingView(string strWhere, string DeptNumber)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format(strHAZARDSUsingView, DeptNumber));
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where 1=1 " + strWhere);
            }

            return OracleHelper.Query(strSql.ToString());
        }
    }
}
