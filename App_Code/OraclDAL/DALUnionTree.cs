using System;
using System.Data;
using System.Text;
using System.Data.OracleClient;
using GhtnTech.SEP.DBUtility;

namespace GhtnTech.SEP.OraclDAL
{
    /// <summary>
    ///DALUnionTree 的摘要说明
    /// </summary>
    public class DALUnionTree
    {
        public DALUnionTree()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 获取专业、工作任务、工序全连接信息--主信息和临时信息
        /// </summary>
        /// <param name="strWhere">Where条件</param>
        /// <returns></returns>
        public DataSet GetUnionTree(string ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format("select * from ( select ''||INFOID INFOID,INFONAME,''||FID FID from CS_BASEINFOSET where infoid!={0} start with infoid={0} connect by prior INFOID = FID union select 'w'||worktaskid INFOID,worktask INFONAME,''||professionalid FID from worktasks union select 'p'||processid INFOID,process.name INFONAME,'w'||process.worktaskid FID from process union select 'a'||WORKTASKID INFOID,WORKTASK INFONAME,''||PROFESSIONALID FID from WORKTASKS_TEMP where WORKTASKS_TEMP.STATUS!='已发布' union select 'b'||PROCESSID INFOID,NAME INFONAME,'a'||WORKTASKID FID from PROCESS_TEMP where STATUS!='已发布')q order by q.infoid", ID));

            return OracleHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取专业、工作任务、工序全连接信息--主信息表
        /// </summary>
        /// <param name="strWhere">Where条件</param>
        /// <returns></returns>

        public DataSet GetUnionTreeBase(string ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format("select * from ( select ''||INFOID INFOID,INFONAME,''||FID FID from CS_BASEINFOSET where infoid!={0} start with infoid={0} connect by prior INFOID = FID union select 'w'||worktaskid INFOID,worktask INFONAME,''||professionalid FID from worktasks union select 'p'||processid INFOID,process.name INFONAME,'w'||process.worktaskid FID from process ) q order by q.infoid", ID));

            return OracleHelper.Query(strSql.ToString());
        }
    }
}
