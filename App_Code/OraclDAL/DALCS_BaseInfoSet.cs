using System;
using System.Data;
using System.Text;
using System.Data.OracleClient;
using GhtnTech.SecurityFramework.BLL;
using GhtnTech.SEP.DBUtility;

namespace GhtnTech.SEP.OraclDAL
{
    /// <summary>
    ///DALCS_BaseInfoSet 的摘要说明
    /// </summary>
    public class DALCS_BaseInfoSet
    {
        public DALCS_BaseInfoSet()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        /// <summary>
        /// 获得编码基础信息设置数据列表
        /// </summary>
        /// <param name="strWhere">Where条件</param>
        /// <param name="strOrder">排序条件</param>
        /// <returns></returns>
        public DataSet GetBaseInfoSetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            //strSql.Append("select * from CS_BaseInfoSet");
            strSql.Append("select c.INFOID,c.INFOCODE,c.INFONAME,c.FID,c.CODINGTYPE,c.CODINGL,c.PDEPART,c.PDAY,c.SDAY,c.STATUS,d.DEPTNAME,p.NAME,pp.NAME as PPNAME from CS_BaseInfoSet c left join Person p on c.PPERID=p.PERSONNUMBER left join Department d on c.PDEPART=d.DEPTNUMBER left join Person pp on c.SPERID=pp.PERSONNUMBER");

            if (strWhere.Trim() != "")
            {
                strSql.Append(" where 1=1 " + strWhere);
            }
            strSql.Append(" order by c.INFOID asc");
            return OracleHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 增加一个编码基础信息设置
        /// </summary>
        /// <param name="model">编码基础信息设置实体类</param>
        /// <returns></returns>
        public bool CreateBaseInfoSet(Model.CS_BaseInfoSet model)
        {
            #region 信息新增
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into CS_BaseInfoSet(");
            strSql.Append("INFONAME,INFOCODE,FID,CODINGTYPE,CODINGL,PDEPART,PPERID,PDAY,SPERID,SDAY,REMARKS,STATUS)");
            strSql.Append(" values(");
            strSql.Append(":INFONAME,:INFOCODE,:FID,:CODINGTYPE,:CODINGL,:PDEPART,:PPERID,:PDAY,:SPERID,:SDAY,:REMARKS,:STATUS)");
            OracleParameter[] parameters = {
					new OracleParameter(":INFONAME", OracleType.NVarChar,500),
					new OracleParameter(":INFOCODE", OracleType.NVarChar,20),
                    new OracleParameter(":FID", OracleType.Number,4),
					new OracleParameter(":CODINGTYPE", OracleType.NVarChar,20),
					new OracleParameter(":CODINGL", OracleType.Number,4),
                    new OracleParameter(":PDEPART", OracleType.NVarChar,20),
					new OracleParameter(":PPERID", OracleType.NVarChar,20),
					new OracleParameter(":PDAY", OracleType.DateTime),
                    new OracleParameter(":SPERID", OracleType.NVarChar,20),
					new OracleParameter(":SDAY", OracleType.DateTime),
					new OracleParameter(":REMARKS", OracleType.NVarChar,200),
                    new OracleParameter(":STATUS", OracleType.Char,10)};
            parameters[0].Value = model.INFONAME;//名称
            parameters[1].Value = model.INFOCODE;//编码
            if (model.FID == null)
            {
                parameters[2].Value = -1;//父节点
            }
            else
            {
                parameters[2].Value = model.FID;//父节点
            }
            parameters[3].Value = model.CODINGTYPE;//下属节点类型
            parameters[4].Value = model.CODINGL;//下属节点长度
            parameters[5].Value = SessionBox.GetUserSession().DeptNumber;//登录用户单位-编辑
            parameters[6].Value = SessionBox.GetUserSession().PersonNumber;//登录用户人员-编辑
            parameters[7].Value = model.PDAY;//编辑日期
            parameters[8].Value = "";//登录用户人员-启用
            parameters[9].Value = model.PDAY;//启用日期-赋值编辑日期
            parameters[10].Value = "";//备注-暂时未用
            parameters[11].Value = "编辑";//信息状态
            #endregion

            if (OracleHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 更新指定的编码基础信息设置
        /// </summary>
        /// <param name="model">编码基础信息设置实体类</param>
        /// <returns></returns>
        public bool UpdateRulesTreeKind(Model.CS_BaseInfoSet model)
        {
            bool bl = true;
            StringBuilder strsSql = new StringBuilder();
            strsSql.Append("select * from CS_BaseInfoSet");
            strsSql.Append(" where INFOID='" + model.INFOID + "'");
            DataSet ds = OracleHelper.Query(strsSql.ToString());
            if (model.STATUS.Trim() == "编辑")
            {
                try
                {
                    #region 信息修改
                    //本单位信息方可修改
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("update CS_BaseInfoSet set ");
                    strSql.Append("INFONAME=:INFONAME,");
                    strSql.Append("INFOCODE=:INFOCODE,");
                    strSql.Append("CODINGTYPE=:CODINGTYPE,");
                    strSql.Append("CODINGL=:CODINGL,");
                    strSql.Append("PDEPART=:PDEPART,");
                    strSql.Append("PPERID=:PPERID,");
                    strSql.Append("PDAY=:PDAY,");
                    strSql.Append("STATUS=:STATUS");
                    strSql.Append(" where INFOID=:INFOID and PDEPART='" + SessionBox.GetUserSession().DeptNumber + "'");
                    OracleParameter[] parameters = {
					new OracleParameter(":INFONAME", OracleType.NVarChar,500),
					new OracleParameter(":INFOCODE", OracleType.NVarChar,20),
					new OracleParameter(":CODINGTYPE", OracleType.NVarChar,20),
					new OracleParameter(":CODINGL", OracleType.Number,4),
                    new OracleParameter(":PDEPART", OracleType.NVarChar,20),
					new OracleParameter(":PPERID", OracleType.NVarChar,20),
					new OracleParameter(":PDAY", OracleType.DateTime),
                    new OracleParameter(":STATUS", OracleType.Char,10),
                    new OracleParameter(":INFOID", OracleType.Number,4)};
                    parameters[0].Value = model.INFONAME;//名称
                    parameters[1].Value = model.INFOCODE;//编码
                    parameters[2].Value = model.CODINGTYPE;//下属节点类型
                    parameters[3].Value = model.CODINGL;//下属节点长度
                    parameters[4].Value = SessionBox.GetUserSession().DeptNumber;//登录用户单位-编辑
                    parameters[5].Value = SessionBox.GetUserSession().PersonNumber;//登录用户人员-编辑
                    parameters[6].Value = model.PDAY;//编辑日期
                    parameters[7].Value = "编辑";//信息状态
                    parameters[8].Value = model.INFOID;//自增主键
                    #endregion

                    if (OracleHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
                    {
                        bl = true;
                    }
                    else
                    {
                        bl = false;
                    }
                }
                catch
                {
                    bl = false;
                }
            }
            if (model.STATUS.Trim() == "启用" || model.STATUS.Trim() == "禁用")
            {
                bl = false;
            }
            return bl;
        }

        /// <summary>
        /// 删除/禁用一个编码基础信息设置
        /// </summary>
        /// <param name="OperatorID">编码基础信息设置ID</param>
        /// <returns></returns>
        public bool DeleteBaseInfoSet(Model.CS_BaseInfoSet model)
        {
            bool bl = true;
            StringBuilder strsSql = new StringBuilder();
            //根据主键获取CS_BaseInfoSet表中信息
            strsSql.Append("select * from CS_BaseInfoSet");
            strsSql.Append(" where INFOID='" + model.INFOID + "'");
            DataSet ds = OracleHelper.Query(strsSql.ToString());
            
            StringBuilder strSql = new StringBuilder();
            //根据信息状态不同，处理过程不同
            if (ds.Tables[0].Rows[0]["STATUS"].ToString().Trim() == "编辑")
            {
                try
                {
                    #region 信息删除
                    //直接删除-本单位信息
                    strSql.Append("delete CS_BaseInfoSet");
                    strSql.Append(" where INFOID=:INFOID and PDEPART='" + SessionBox.GetUserSession().DeptNumber + "'");
                    OracleParameter[] parameters = {
                new OracleParameter(":INFOID", OracleType.Number,4)};
                    parameters[0].Value = model.INFOID;
                    #endregion

                    if (OracleHelper.ExecuteSql(strSql.ToString(), parameters) >= 1)
                    {
                        bl = true;
                    }
                    else
                    {
                        bl = false;
                    }
                }
                catch
                {
                    bl = false;
                }
            }
            if (ds.Tables[0].Rows[0]["STATUS"].ToString().Trim() == "启用")
            {
                try
                {
                    #region 主表信息禁用
                    //1、若有根节点，禁用
                    //2、主信息禁用
                    //strSql.Append("update CS_BaseInfoSet set ");
                    //strSql.Append("STATUS=:STATUS");
                    //strSql.Append(" where INFOID in ( ");
                    //strSql.Append(" select INFOID from CS_BaseInfoSet ");
                    //strSql.Append(" start with INFOID=:INFOID ");
                    //strSql.Append(" connect by prior INFOID=FID ) ");

                    //信息禁用
                    strSql.Append("update CS_BaseInfoSet set ");
                    strSql.Append("STATUS=:STATUS");
                    strSql.Append(" where INFOID=:INFOID and PDEPART='" + SessionBox.GetUserSession().DeptNumber + "'");
                    OracleParameter[] parameters = {
                    new OracleParameter(":STATUS", OracleType.Char,10),
                    new OracleParameter(":INFOID", OracleType.Number,4)};
                    parameters[0].Value = "禁用";
                    parameters[1].Value = model.INFOID;
                    OracleHelper.ExecuteSql(strSql.ToString(), parameters);
                    #endregion

                    #region 历史入库
                    StringBuilder strhSql = new StringBuilder();
                    strhSql.Append("insert into CS_BaseInfoSetHistory(");
                    strhSql.Append("INFOID,INFONAME,INFOCODE,FID,CODINGTYPE,CODINGL,PDEPART,PPERID,PDAY,SPERID,SDAY,REMARKS,STATUS,HPERID,HDAY,HREMARKS,HSTATUS)");
                    strhSql.Append(" values(");
                    strhSql.Append(":INFOID,:INFONAME,:INFOCODE,:FID,:CODINGTYPE,:CODINGL,:PDEPART,:PPERID,:PDAY,:SPERID,:SDAY,:REMARKS,:STATUS,:HPERID,:HDAY,:HREMARKS,:HSTATUS)");
                    OracleParameter[] parametersH = {
                    new OracleParameter(":INFOID", OracleType.Number,4),
					new OracleParameter(":INFONAME", OracleType.NVarChar,500),
					new OracleParameter(":INFOCODE", OracleType.NVarChar,20),
                    new OracleParameter(":FID", OracleType.Number,4),
					new OracleParameter(":CODINGTYPE", OracleType.NVarChar,20),
					new OracleParameter(":CODINGL", OracleType.NVarChar,20),
                    new OracleParameter(":PDEPART", OracleType.NVarChar,20),
					new OracleParameter(":PPERID", OracleType.NVarChar,20),
					new OracleParameter(":PDAY", OracleType.DateTime),
                    new OracleParameter(":SPERID", OracleType.NVarChar,20),
					new OracleParameter(":SDAY", OracleType.DateTime),
					new OracleParameter(":REMARKS", OracleType.NVarChar,200),
                    new OracleParameter(":STATUS", OracleType.Char,10),
                    new OracleParameter(":HPERID", OracleType.NVarChar,20),
					new OracleParameter(":HDAY", OracleType.DateTime),
					new OracleParameter(":HREMARKS", OracleType.NVarChar,200),
                    new OracleParameter(":HSTATUS", OracleType.Char,10)};
                    parametersH[0].Value = model.INFOID;//自增主键
                    parametersH[1].Value = model.INFONAME;//名称
                    parametersH[2].Value = model.INFOCODE;//编码
                    parametersH[3].Value = model.FID;//父节点
                    parametersH[4].Value = model.CODINGTYPE;//下属节点类型
                    parametersH[5].Value = model.CODINGL;//下属节点长度
                    parametersH[6].Value = model.PDEPART;//登录用户单位-编辑
                    parametersH[7].Value = model.PPERID;//登录用户人员-编辑
                    parametersH[8].Value = model.PDAY;//编辑日期
                    parametersH[9].Value = model.SPERID;//登录用户人员-启用
                    parametersH[10].Value = model.PDAY;//启用日期-赋值编辑日期
                    parametersH[11].Value = model.REMARKS;//备注-暂时未用
                    parametersH[12].Value = model.STATUS;//信息状态
                    parametersH[13].Value = SessionBox.GetUserSession().PersonNumber;//历史记录人员
                    parametersH[14].Value = System.DateTime.Today;//历史记录日期
                    parametersH[15].Value = "";//历史备注
                    parametersH[16].Value = "禁用";//历史状态
                    OracleHelper.ExecuteSql(strhSql.ToString(), parametersH);
                    #endregion

                    if (OracleHelper.ExecuteSql(strSql.ToString(), parameters) >= 1 && OracleHelper.ExecuteSql(strhSql.ToString(), parametersH) >= 1)
                    {
                        bl = true;
                    }
                    else
                    {
                        bl = false;
                    }
                }
                catch
                {
                    bl = false;
                }
            }
            if (ds.Tables[0].Rows[0]["STATUS"].ToString().Trim() == "禁用")
            {
                bl = false;
            }
            return bl;
        }

    }
}
