using System;
using System.Data;
using System.Text;
using System.Data.OracleClient;
using GhtnTech.SEP.DBUtility;

namespace GhtnTech.SEP.OraclDAL
{
    /// <summary>
    ///DALHAZARDSall 的摘要说明
    /// </summary>
    public class DALHAZARDSall
    {
        public DALHAZARDSall()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 获取危险源全连接信息
        /// </summary>
        /// <param name="strWhere">Where条件</param>
        /// <returns></returns>
        public DataSet GetDALHAZARDSall(string strWhere, string strInner)
        {
            StringBuilder strSql = new StringBuilder();
            //strSql.Append("select HAZARDS.*,PROCESS.NAME gxname,a.infoname zyname,b.worktask gzrwname,c.infoname fclevel,d.name gldxname,e.name glrn,f.name zjzzr,g.name jgzzr,h.infoname fxlx,j.infoname sglx,hazards_ore.processingmode,hazards_ore.punishmentstandard,hazards_ore.note,hazards_ore.H_BJW,hazards_ore.H_BM from HAZARDS inner join hazards_ore on HAZARDS.H_NUMBER=hazards_ore.h_number inner join PROCESS on HAZARDS.PROCESSID=PROCESS.PROCESSID inner join CS_BASEINFOSET a on PROCESS.PROFESSIONALID=a.infoid inner join WORKTASKS b on PROCESS.WORKTASKID=b.WORKTASKID inner join CS_BASEINFOSET c on hazards_ore.RISK_EVELNUMBER=c.infoid left join M_OBJECT d on hazards_ore.m_Objectnumber=d.M_OBJECTID left join M_OBJECT e on hazards_ore.m_Personnumber=e.M_OBJECTID left join M_OBJECT f on hazards_ore.Directlyresponsiblepersonsnumb=f.M_OBJECTID left join M_OBJECT g on hazards_ore.Regulatorypartnersnumber=g.M_OBJECTID inner join CS_BASEINFOSET h on HAZARDS.Risk_Typesnumber=h.infoid inner join CS_BASEINFOSET j on HAZARDS.Accident_Typenumber=j.infoid");
            //strSql.Append("select * from vcommhazards");
            strSql.Append("select HAZARDS.HAZARDSID,hazards.processid,hazards.ispass,hazards.h_content,hazards.h_number,hazards.h_consequences,hazards.m_standards,hazards.m_measures,hazards.regulatorymeasures,hazards.risk_typesnumber,hazards.accident_typenumber,PROCESS.NAME gxname,a.infoname zyname,b.worktask gzrwname,c.infoname fclevel,d.name gldxname,e.name glrn,f.name zjzzr,g.name jgzzr,h.infoname fxlx,j.infoname sglx,hazards_ore.processingmode,hazards_ore.punishmentstandard,hazards_ore.note,hazards_ore.H_BJW,hazards_ore.H_BM from HAZARDS inner join hazards_ore on HAZARDS.H_NUMBER=hazards_ore.h_number inner join PROCESS on HAZARDS.PROCESSID=PROCESS.PROCESSID inner join CS_BASEINFOSET a on PROCESS.PROFESSIONALID=a.infoid inner join WORKTASKS b on PROCESS.WORKTASKID=b.WORKTASKID inner join CS_BASEINFOSET c on hazards_ore.RISK_EVELNUMBER=c.infoid left join M_OBJECT d on hazards_ore.m_Objectnumber=d.M_OBJECTID left join M_OBJECT e on hazards_ore.m_Personnumber=e.M_OBJECTID left join M_OBJECT f on hazards_ore.Directlyresponsiblepersonsnumb=f.M_OBJECTID left join M_OBJECT g on hazards_ore.Regulatorypartnersnumber=g.M_OBJECTID inner join CS_BASEINFOSET h on HAZARDS.Risk_Typesnumber=h.infoid inner join CS_BASEINFOSET j on HAZARDS.Accident_Typenumber=j.infoid where 1=1 and hazards_ore.H_BJW='通用' ");
            if (strInner.Trim() != "")
            {
                strSql.Append(" " + strInner);
            }
            if (strWhere.Trim() != "")
            {
                strSql.Append(strWhere);
            }
            //strSql.Append(" order by a.infoid");

            return OracleHelper.Query(strSql.ToString());
        }
    }
}
