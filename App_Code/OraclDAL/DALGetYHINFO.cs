using System;
using System.Data;
using System.Text;
using System.Data.OracleClient;
using GhtnTech.SEP.DBUtility;

namespace GhtnTech.SEP.OraclDAL
{

    /// <summary>
    ///DALGetDepartmentbyK 的摘要说明
    /// </summary>
    public class DALGetYHINFO
    {
        public DALGetYHINFO()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 获取矿级单位信息--不包括队
        /// </summary>
        /// <param name="DeptNumber">部门编码</param>
        /// <returns></returns>
        public DataSet GetDepartmentbyK(string DeptNumber)
        {
            StringBuilder strSql = new StringBuilder();
            string sql = "select department.*,F_PINYIN(deptname) pyall from department where substr(deptnumber, -2, 2)='00' and deptnumber!='{0}'  start with deptnumber='{0}' connect by prior deptnumber = fatherid  order by deptnumber";
            strSql.Append(string.Format(sql, DeptNumber));

            return OracleHelper.Query(strSql.ToString());
        }

        public DataSet GetYHZGbyID(string YHID)//隐患整改信息
        {
            StringBuilder strSql = new StringBuilder();
            string sql = "SELECT YinHuanCheck.*, (CASE WHEN Person.Name IS NULL THEN '无' ELSE Person.Name END) RName,DeptName FROM YinHuanCheck LEFT JOIN Person ON YinHuanCheck.ResponsibleID = Person.personnumber INNER JOIN Department ON YinHuanCheck.ResponsibleDept=Department.Deptnumber WHERE YHPutinID = {0}";
            strSql.Append(string.Format(sql, YHID));

            return OracleHelper.Query(strSql.ToString());
        }

        public DataSet GetNYHZGbyID(string YHID)//新隐患整改信息
        {
            StringBuilder strSql = new StringBuilder();
            string sql = "SELECT nYinHuanCheck.*, (CASE WHEN Person.Name IS NULL THEN '无' ELSE Person.Name END) RName,DeptName FROM nYinHuanCheck LEFT JOIN Person ON nYinHuanCheck.ResponsibleID = Person.personnumber INNER JOIN Department ON nYinHuanCheck.ResponsibleDept=Department.Deptnumber WHERE YHPutinID = {0}";
            strSql.Append(string.Format(sql, YHID));

            return OracleHelper.Query(strSql.ToString());
        }

        public DataSet GetYHZGFKbyID(string YHID)//隐患整改反馈信息
        {
            StringBuilder strSql = new StringBuilder();
            string sql = "SELECT YinHuanRectification.*, a.Name RecPerson,b.Name YanshouName FROM YinHuanRectification INNER JOIN Person a ON YinHuanRectification.RecPersonID = a.personnumber INNER JOIN Person b ON YinHuanRectification.YanshouID = b.personnumber WHERE YHPutinID = {0}";
            strSql.Append(string.Format(sql, YHID));

            return OracleHelper.Query(strSql.ToString());
        }

        public DataSet GetNYHZGFKbyID(string YHID)//新隐患整改反馈信息
        {
            StringBuilder strSql = new StringBuilder();
            string sql = "SELECT nYinHuanRectification.*, a.Name RecPerson,b.Name YanshouName FROM nYinHuanRectification INNER JOIN Person a ON nYinHuanRectification.RecPersonID = a.personnumber INNER JOIN Person b ON nYinHuanRectification.YanshouID = b.personnumber WHERE YHPutinID = {0}";
            strSql.Append(string.Format(sql, YHID));

            return OracleHelper.Query(strSql.ToString());
        }

        public bool AutoSetYHStatus()//自动处理流程
        {
            bool bl = true;
            string sql = "UPDATE YHInput SET Status=( CASE Status WHEN '隐患未整改' THEN '逾期未整改' WHEN '隐患整改中' THEN '逾期整改未完成' END) WHERE Status in ('隐患未整改','隐患整改中') AND YHPutinID in (SELECT YHPutinID FROM YinHuanCheck WHERE RecLimit<sysdate)";

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

        public bool AutoSetNYHStatus()//新自动处理流程
        {
            bool bl = true;
            string sql = "UPDATE nYHInput SET Status=( CASE Status WHEN '隐患未整改' THEN '逾期未整改' WHEN '隐患整改中' THEN '逾期整改未完成' END) WHERE Status in ('隐患未整改','隐患整改中') AND YHPutinID in (SELECT YHPutinID FROM nYinHuanCheck WHERE RecLimit<sysdate)";

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
