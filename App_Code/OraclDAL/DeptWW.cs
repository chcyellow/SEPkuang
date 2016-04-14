using System;
using System.Data;
using System.Text;
using System.Data.OracleClient;
using GhtnTech.SEP.DBUtility;
using GhtnTech.SecurityFramework.BLL;


/// <summary>
///DeptWW 的摘要说明
/// </summary>
/// 
namespace GhtnTech.SEP.OraclDAL
{
    public class DeptWW
    {
        public DeptWW()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }


        /// <summary>
        /// 获得外围单位数据列表
        /// </summary>
        /// <param name="strWhere">Where条件</param>
        /// <param name="strOrder">排序条件</param>
        /// <returns></returns>
        public DataSet GetDeptWW()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * FROM Department where DEPT_WW ='外围' and substr(deptnumber,1,4) = '" + SessionBox.GetUserSession().DeptNumber.Substring(0,4) + "'");


          //  strSql.Append(" where 1=1 and dept_ww ='外围' and deptnumber = '" + SessionBox.GetUserSession().DeptNumber + "');

            return OracleHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 增加一个外围单位
        /// </summary>
        /// <param name="model">外围单位实体类</param>
        /// <returns></returns>
        public bool CreateDeptWW(Department model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Department(");
            strSql.Append("DEPTNUMBER,DEPTNAME,FATHERID,DEPT_WW)");
            strSql.Append(" values(");
            strSql.Append(":DEPTNUMBER,:DEPTNAME,:FATHERID,:DEPT_WW)");
            OracleParameter[] parameters = {
					new OracleParameter(":DEPTNUMBER", OracleType.NVarChar,30),
					new OracleParameter(":DEPTNAME", OracleType.NVarChar,20),
                    new OracleParameter(":FATHERID", OracleType.NVarChar,20),
                    new OracleParameter(":DEPT_WW",OracleType.NVarChar,10)
                   };
            parameters[0].Value = GetMaxdeptnumber();
            parameters[1].Value = model.DEPTNAME;
            parameters[2].Value = SessionBox.GetUserSession().DeptNumber.Substring(0,4)+90000;
            parameters[3].Value = "外围";
            if (OracleHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static string GetMaxdeptnumber()
        {
            string strsql = "select max(deptnumber) from Department where dept_ww ='外围' and substr(deptnumber,1,4) = '" + SessionBox.GetUserSession().DeptNumber.Substring(0, 4) + "'";
            object obj =OracleHelper.GetSingle(strsql);
            if (obj == null)
            {
                return (SessionBox.GetUserSession().DeptNumber.Substring(0,4)+95100).Trim();
            }
            else
            {
                return (int.Parse(obj.ToString().Substring(0,7))+1).ToString().Trim()+"00";
            }
        }

        /// <summary>
        /// 更新外围单位
        /// </summary>
        /// <param name="model">外围单位实体类</param>
        /// <returns></returns>
        public bool UpdateDeptWW(Department model)
        {
            bool bl = true;
           
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Department set ");
            strSql.Append("DEPTNAME=:DEPTNAME ");
            //strSql.Append("ISOMUX=:ISOMUX");
            strSql.Append(" where DEPTNUMBER=:DEPTNUMBER ");
            OracleParameter[] parameters = {
				new OracleParameter(":DEPTNAME", OracleType.NVarChar,200),
                //new OracleParameter(":ISOMUX", OracleType.NVarChar,200),
                new OracleParameter(":DEPTNUMBER", OracleType.VarChar,20)
             };
            parameters[0].Value = model.DEPTNAME;
            parameters[1].Value = model.DEPTNUMBER;
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

      


        /// <summary>
        /// 删除/禁用一个外围单位
        /// </summary>
        /// <param name="OperatorID">外围单位ID</param>
        /// <returns></returns>
        public bool DeleteDeptWW(Department model)
        {
            bool bl = true;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete Department");
            strSql.Append(" where DEPTNUMBER=:DEPTNUMBER ");
            OracleParameter[] parameters = {
            new OracleParameter(":DEPTNUMBER", OracleType.VarChar,20)};
            parameters[0].Value = model.DEPTNUMBER;

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
