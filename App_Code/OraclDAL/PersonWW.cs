using System;
using System.Data;
using System.Text;
using System.Data.OracleClient;
using GhtnTech.SEP.DBUtility;
using GhtnTech.SecurityFramework.BLL;

/// <summary>
///PersonWW 的摘要说明
/// </summary>
namespace GhtnTech.SEP.OraclDAL
{
    public class PersonWW
    {
        public PersonWW()
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
        public DataSet GetPersonWW()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from person where  per_ww='外围' and maindeptid = '" + SessionBox.GetUserSession().DeptNumber + "'");

            return OracleHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 增加一个外围单位
        /// </summary>
        /// <param name="model">外围单位实体类</param>
        /// <returns></returns>
        public bool CreatePerWW(Person model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Person(");
            //strSql.Append("PERSONNUMBER,NAME,SEX,TEL,POSID,MAINDEPTID,LIGHTNUMBER,POSTID,DEPTID,PINYIN,AREADEPTID,PER_WW)");
            strSql.Append("PERSONNUMBER,NAME,SEX,TEL,POSID,MAINDEPTID,LIGHTNUMBER,DEPTID,PINYIN,AREADEPTID,PER_WW)");
            strSql.Append(" values(");
            //strSql.Append(":PERSONNUMBER,:NAME,:SEX,:TEL,:POSID,:MAINDEPTID,:LIGHTNUMBER,:POSTID,:DEPTID,f_pinyin('"+model.NAME+"'),:AREADEPTID,:PER_WW)");
            strSql.Append(":PERSONNUMBER,:NAME,:SEX,:TEL,:POSID,:MAINDEPTID,:LIGHTNUMBER,:DEPTID,f_pinyin('" + model.NAME + "'),:AREADEPTID,:PER_WW)");
            OracleParameter[] parameters = {
					new OracleParameter(":PERSONNUMBER", OracleType.NVarChar,15),
					new OracleParameter(":NAME", OracleType.NVarChar,15),
                    new OracleParameter(":SEX", OracleType.NVarChar,10),
                    new OracleParameter(":TEL",OracleType.NVarChar,15),
                    new OracleParameter(":POSID", OracleType.Number,4),
                    new OracleParameter(":MAINDEPTID",OracleType.NVarChar,10),
                    new OracleParameter(":LIGHTNUMBER", OracleType.NVarChar,15),
                    //new OracleParameter(":POSTID",OracleType.Number,4),
                    new OracleParameter(":DEPTID",OracleType.NVarChar,10),
                   // new OracleParameter(":PINYIN",OracleType.NVarChar,20),
                    new OracleParameter(":AREADEPTID",OracleType.NVarChar,10),
                    new OracleParameter(":PER_WW",OracleType.NVarChar,10)
                   };
            parameters[0].Value = GetMaxpernumber();
            parameters[1].Value = model.NAME;
            parameters[2].Value = model.SEX;
            parameters[3].Value = model.TEL;
            try
            {
                parameters[4].Value = model.POSID;
            }
            catch { }
            parameters[5].Value = SessionBox.GetUserSession().DeptNumber.Trim();
            parameters[6].Value = model.LIGHTNUMBER;
            
            parameters[7].Value = model.DEPTID;
            parameters[8].Value = model.DEPTID;
            parameters[9].Value = "外围";
            if (OracleHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static string GetMaxpernumber()
        {
            string strsql = "select max( to_number(substr(PERSONNUMBER,2))) from Person where PER_WW ='外围' ";
            object obj = OracleHelper.GetSingle(strsql);
            if (obj == null)
            {
                return "W1";
            }
            else
            {
                return "W"+(int.Parse(obj.ToString())+1).ToString();
            }
        }

        /// <summary>
        /// 更新外围单位
        /// </summary>
        /// <param name="model">外围单位实体类</param>
        /// <returns></returns>
        public bool UpdatePerWW(Person model)
        {
            bool bl = true;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Person set ");
            strSql.Append("NAME=:NAME, ");
            strSql.Append("SEX=:SEX, ");
            strSql.Append("TEL=:TEL, ");
            strSql.Append("POSID=:POSID, ");
            strSql.Append("LIGHTNUMBER=:LIGHTNUMBER, ");
            strSql.Append("DEPTID=:DEPTID, ");
            strSql.Append("PINYIN=f_pinyin('" + model.NAME + "'), ");
            strSql.Append("AREADEPTID=:AREADEPTID ");
            strSql.Append(" where PERSONID=:PERSONID ");
            OracleParameter[] parameters = {
					new OracleParameter(":NAME", OracleType.NVarChar,15),
                    new OracleParameter(":SEX", OracleType.NVarChar,10),
                    new OracleParameter(":TEL",OracleType.NVarChar,15),
                    new OracleParameter(":POSID", OracleType.Number,4),
                    new OracleParameter(":LIGHTNUMBER", OracleType.NVarChar,15),
                    new OracleParameter(":DEPTID",OracleType.NVarChar,10),
                   // new OracleParameter(":PINYIN",OracleType.NVarChar,20),
                    new OracleParameter(":AREADEPTID",OracleType.NVarChar,10),
                    new OracleParameter(":PERSONID",OracleType.Number,10)
             };
            parameters[0].Value = model.NAME;
            parameters[1].Value = model.SEX;
            parameters[2].Value = model.TEL;
            parameters[3].Value = model.POSID;
            parameters[4].Value = model.LIGHTNUMBER;
            parameters[5].Value = model.DEPTID;
            parameters[6].Value = model.DEPTID;
            parameters[7].Value = model.PERSONID;
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
        public bool DeletePerWW(Person model)
        {
            bool bl = true;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete Person");
            strSql.Append(" where PERSONID=:PERSONID ");
            OracleParameter[] parameters = {
            new OracleParameter(":PERSONID", OracleType.Number,9)};
            parameters[0].Value = model.PERSONID;

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
