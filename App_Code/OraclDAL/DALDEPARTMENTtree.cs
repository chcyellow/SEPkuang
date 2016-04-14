using System;
using System.Data;
using System.Text;
using System.Data.OracleClient;
using GhtnTech.SEP.DBUtility;

namespace GhtnTech.SEP.OraclDAL
{
    /// <summary>
    ///DALDEPARTMENTtree 的摘要说明
    /// </summary>
    public class DALDEPARTMENTtree
    {
        public DALDEPARTMENTtree()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }


        /// <summary>
        /// 获取根部门和本身
        /// </summary>
        /// <param name="strWhere">Where条件</param>
        /// <returns></returns>
        public DataSet GetDALDEPARTMENTtree(string DEPTNUMBER)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format("select * from department start with DEPTNUMBER={0} connect by prior FATHERID = DEPTNUMBER", DEPTNUMBER));
            DataSet ds = OracleHelper.Query(strSql.ToString());
            if (ds.Tables[0].Rows.Count <= 2)
            {
                return ds;
            }
            else
            {
                DataTable dt = ds.Tables[0].Clone();
                DataRow dr = dt.NewRow();
                dr.ItemArray=ds.Tables[0].Rows[0].ItemArray;
                dt.Rows.Add(dr);
                DataRow dr1 = dt.NewRow(); 
                dr1.ItemArray=ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1].ItemArray;
                dt.Rows.Add(dr1);
                ds.Tables.Clear();
                ds.Tables.Add(dt);
                return ds;
            }
        }
    }
}
