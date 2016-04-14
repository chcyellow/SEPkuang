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
    public class DALHAZARDSall_1
    {
        public DALHAZARDSall_1()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 获取临时危险源全连接信息
        /// </summary>
        /// <param name="strWhere">Where条件</param>
        /// <returns></returns>
        public DataSet GetDALHAZARD_TempSall(string strWhere, string strInner)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select HAZARDS_TEMP.*,PROCESS_TEMP.NAME gxname,c.infoname fclevel,d.name gldxname,e.name glrn,f.name zjzzr,g.name jgzzr,h.infoname pl,i.infoname ss,person.name lrr,department.deptname lrdw,k.infoname fxlx,j.infoname sglx  from HAZARDS_TEMP inner join PROCESS_TEMP on HAZARDS_TEMP.PROCESSNUMBER=PROCESS_TEMP.PROCESSID inner join CS_BASEINFOSET c on HAZARDS_TEMP.Risk_Evelnumber=c.infoid inner join M_OBJECT d on HAZARDS_TEMP.m_Objectnumber=d.M_OBJECTID inner join M_OBJECT e on HAZARDS_TEMP.m_Personnumber=e.M_OBJECTID inner join M_OBJECT f on HAZARDS_TEMP.Directlyresponsiblepersonsnumb=f.M_OBJECTID inner join M_OBJECT g on HAZARDS_TEMP.Regulatorypartnersnumber=g.M_OBJECTID inner join CS_BASEINFOSET h on HAZARDS_TEMP.Frequencynumber=h.infoid inner join CS_BASEINFOSET i on HAZARDS_TEMP.Lossnumber=i.infoid inner join person on HAZARDS_TEMP.Personid=person.personid inner join department on HAZARDS_TEMP.Deptnumber=department.deptnumber inner join CS_BASEINFOSET k on HAZARDS_TEMP.Risk_Typesnumber=k.infoid inner join CS_BASEINFOSET j on HAZARDS_TEMP.Accident_Typenumber=j.infoid");
            if (strInner.Trim() != "")
            {
                strSql.Append(" " + strInner);
            }
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where 1=1 " + strWhere);
            }

            return OracleHelper.Query(strSql.ToString());
        }
    }
}
