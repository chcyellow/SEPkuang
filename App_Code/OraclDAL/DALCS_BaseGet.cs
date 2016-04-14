using System;
using System.Data;
using System.Text;
using System.Data.OracleClient;
using GhtnTech.SEP.DBUtility;

namespace GhtnTech.SEP.OraclDAL
{
    /// <summary>
    ///DALCS_BaseGet 的摘要说明
    /// </summary>
    public class DALCS_BaseGet
    {
        public DALCS_BaseGet()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 获取根节点获取Tree
        /// </summary>
        /// <param name="strWhere">Where条件</param>
        /// <returns></returns>
        public DataSet GetDALDEPARTMENTtree(string ID, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();

            strSql.Append(string.Format("select INFOID,INFOCODE,INFONAME,FID,STATUS from CS_BASEINFOSET {0} start with INFOID={1} connect by prior INFOID = FID", "where 1=1" + strWhere, ID));//where STATUS='启用
            return OracleHelper.Query(strSql.ToString());
        }
        //获取节点名称
        public static string GetBAseSetName(string ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format("select INFONAME from CS_BASEINFOSET where INFOID={0}", ID));
            return OracleHelper.Query(strSql.ToString()).Tables[0].Rows[0]["INFONAME"].ToString();
        }
    }
}
