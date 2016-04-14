using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.Data.OracleClient;
using GhtnTech.SEP.DBUtility;
using System.Xml;

/// <summary>
///PublicMethod 的摘要说明
/// </summary>
public class PublicMethod
{
	public PublicMethod()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    //根据人员编码获取单位信息
    public static DataSet GetDept(string per)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append("select DEPARTMENT.DEPTNUMBER,DEPARTMENT.DEPTNAME from PERSON inner join DEPARTMENT on PERSON.MAINDEPTID=DEPARTMENT.DEPTNUMBER");
        strSql.Append(" where PERSON.PERSONNUMBER=" + per);
        DataSet ds = OracleHelper.Query(strSql.ToString());
        return ds;
    }

    #region 读取XML配置文件信息
    public static string ReadXmlReturnNode(string Node,System.Web.UI.Page page)
    {
        string url=page.Server.MapPath("BaseSet.xml");
        XmlDocument docXml = new XmlDocument();
        loadXML(url, docXml);
        XmlNodeList xn = docXml.GetElementsByTagName(Node);
        return xn.Item(0).InnerText.ToString();
    }
    private static void loadXML(string url, XmlDocument docXml)
    {
        try
        {
            docXml.Load(url);
        }
        catch
        {
            string p=url.Substring(0,url.LastIndexOf("\\"));
            p = p.Substring(0, p.LastIndexOf("\\")) + "\\BaseSet.xml";
            loadXML(p, docXml);
        }

    }
    #endregion

    #region 获取隐患三违编号
    /// <summary>
    /// 获取隐患三违编号
    /// </summary>
    /// <param name="zyID">级别ID</param>
    /// <returns></returns>
    public static string GetYSNO(string jbID, string zyID)
    {
        string oracletext = "select * from YSNO where LEVELID=" + jbID + " and TYPEID=" + zyID;
        DataTable dt = OracleHelper.Query(oracletext).Tables[0];
        if (dt.Rows.Count == 0)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into YSNO (LEVELID,TYPEID,AUTONO) values( ");
            strSql.Append(":LEVELID,:TYPEID,:AUTONO)");
            OracleParameter[] parameters = {
                    new OracleParameter(":LEVELID", OracleType.Number,9),//ID
                    new OracleParameter(":TYPEID", OracleType.Number,9),
                    new OracleParameter(":AUTONO", OracleType.Int16)
                                           };
            parameters[0].Value = jbID;
            parameters[1].Value = zyID;
            parameters[2].Value = 2;
            OracleHelper.ExecuteSql(strSql.ToString(), parameters);
            return "00001";
        }
        else
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update YSNO set AUTONO=:AUTONO  ");
            strSql.Append("where LEVELID=:LEVELID and TYPEID=:TYPEID");
            OracleParameter[] parameters = {
                    new OracleParameter(":AUTONO", OracleType.Number,9),
                    new OracleParameter(":LEVELID", OracleType.Number,9),
                     new OracleParameter(":TYPEID", OracleType.Number,9)
                                           };
            parameters[0].Value = Convert.ToInt32(dt.Rows[0]["AUTONO"]) + 1;
            parameters[1].Value = jbID;
            parameters[2].Value = zyID;
            OracleHelper.ExecuteSql(strSql.ToString(), parameters);
            return dt.Rows[0]["AUTONO"].ToString().Trim().PadLeft(5, '0');
        }
    }
    #endregion

    #region 获取工作任务流水号
    /// <summary>
    /// 获取工作任务流水号
    /// </summary>
    /// <param name="zyID">辨识单元ID（专业）</param>
    /// <returns></returns>
    public static string GetWORKTASKSNO(string zyID)
    {
        string oracletext = "select * from WORKTASKSNO where ZYID=" + zyID;
        DataTable dt = OracleHelper.Query(oracletext).Tables[0];
        if (dt.Rows.Count == 0)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into WORKTASKSNO (ZYID,SERIALNUMBER) values( ");
            strSql.Append(":ZYID,:SERIALNUMBER)");
            OracleParameter[] parameters = {
                    new OracleParameter(":ZYID", OracleType.Number,9),//ID
                    new OracleParameter(":SERIALNUMBER", OracleType.Number,9)
                                           };
            parameters[0].Value = zyID;
            parameters[1].Value = 2;
            OracleHelper.ExecuteSql(strSql.ToString(), parameters);
            return "0001";
        }
        else
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update WORKTASKSNO set SERIALNUMBER=:SERIALNUMBER  ");
            strSql.Append("where ZYID=:ZYID");
            OracleParameter[] parameters = {
                    new OracleParameter(":SERIALNUMBER", OracleType.Number,9),
                    new OracleParameter(":ZYID", OracleType.Number,9)//ID
                                           };
            parameters[0].Value = Convert.ToInt32(dt.Rows[0]["SERIALNUMBER"]) + 1;
            parameters[1].Value = zyID;
            OracleHelper.ExecuteSql(strSql.ToString(), parameters);
            return dt.Rows[0]["SERIALNUMBER"].ToString().Trim().PadLeft(4, '0');
        }
    }
    #endregion

    #region 获取工序流水号
    /// <summary>
    /// 获取工序流水号
    /// </summary>
    /// <param name="wtID">工作任务ID</param>
    /// <returns></returns>
    public static string GetPROCESSNO(string wtID)
    {
        string oracletext = "select * from PROCESSNO where WORKTASKID=" + wtID;
        DataTable dt = OracleHelper.Query(oracletext).Tables[0];
        if (dt.Rows.Count == 0)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into PROCESSNO (WORKTASKID,SERIALNUMBER) values( ");
            strSql.Append(":WORKTASKID,:SERIALNUMBER)");
            OracleParameter[] parameters = {
                    new OracleParameter(":WORKTASKID", OracleType.Number,9),//ID
                    new OracleParameter(":SERIALNUMBER", OracleType.Number,9)
                                           };
            parameters[0].Value = wtID;
            parameters[1].Value = 2;
            OracleHelper.ExecuteSql(strSql.ToString(), parameters);
            return "01";
        }
        else
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update PROCESSNO set SERIALNUMBER=:SERIALNUMBER  ");
            strSql.Append("where WORKTASKID=:WORKTASKID");
            OracleParameter[] parameters = {
                    new OracleParameter(":SERIALNUMBER", OracleType.Number,9),
                    new OracleParameter(":WORKTASKID", OracleType.Number,9)//ID
                                           };
            parameters[0].Value = Convert.ToInt32(dt.Rows[0]["SERIALNUMBER"]) + 1;
            parameters[1].Value = wtID;
            OracleHelper.ExecuteSql(strSql.ToString(), parameters);
            return dt.Rows[0]["SERIALNUMBER"].ToString().Trim().PadLeft(2, '0');
        }
    }
    #endregion

    #region 获取危险源流水号
    /// <summary>
    /// 获取危险源流水号
    /// </summary>
    /// <param name="proID">工序ID</param>
    /// <returns></returns>
    public static string GetHAZARDSNO(string proID)
    {
        string oracletext = "select * from PROCESS inner join WORKTASKS on PROCESS.WORKTASKID=WORKTASKS.WORKTASKID inner join CS_BASEINFOSET on WORKTASKS.PROFESSIONALID=CS_BASEINFOSET.INFOID where PROCESSID=" + proID;
        DataTable dtbase = OracleHelper.Query(oracletext).Tables[0];
        string strbase = dtbase.Rows[0]["INFOCODE"].ToString().Trim() + dtbase.Rows[0]["TASK_NUMBER"].ToString().Trim() + dtbase.Rows[0]["ISOMUX"].ToString().Trim();
        oracletext = "select * from HAZARDSNO where PROCESSID=" + proID;
        DataTable dt = OracleHelper.Query(oracletext).Tables[0];
        if (dt.Rows.Count == 0)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into HAZARDSNO (PROCESSID,SERIALNUMBER) values( ");
            strSql.Append(":PROCESSID,:SERIALNUMBER)");
            OracleParameter[] parameters = {
                    new OracleParameter(":PROCESSID", OracleType.Number,9),//ID
                    new OracleParameter(":SERIALNUMBER", OracleType.Number,9)
                                           };
            parameters[0].Value = proID;
            parameters[1].Value = 2;
            OracleHelper.ExecuteSql(strSql.ToString(), parameters);
            return strbase + "01";
        }
        else
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update HAZARDSNO set SERIALNUMBER=:SERIALNUMBER  ");
            strSql.Append("where PROCESSID=:PROCESSID");
            OracleParameter[] parameters = {
                    new OracleParameter(":SERIALNUMBER", OracleType.Number,9),
                    new OracleParameter(":PROCESSID", OracleType.Number,9)//ID
                                           };
            parameters[0].Value = Convert.ToInt32(dt.Rows[0]["SERIALNUMBER"]) + 1;
            parameters[1].Value = proID;
            OracleHelper.ExecuteSql(strSql.ToString(), parameters);
            return strbase + dt.Rows[0]["SERIALNUMBER"].ToString().Trim().PadLeft(2, '0');
        }
    }
    #endregion

    #region 自动更新走动计划状态
    public static void AutoSetMovePlanStatus()
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append("update MovePlan SET MoveState='逾期未走动'");
        strSql.Append(" WHERE (MoveState ='未走动' or MoveState ='走动中') AND EndTime < to_date(to_char(sysdate,'yyyy-MM-dd'),'yyyy-MM-dd')");
        OracleHelper.Query(strSql.ToString());
    }
    #endregion

    #region 隐患录入-自动更新走动计划状态
    public static void AutoUpdateMovestatusR(string personnumber,int placeid,DateTime pctime)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append("update moveplan set MoveState='已走动'");
        strSql.Append(" where PersonID='" + personnumber + "' and ('" + pctime + "' between StartTime and EndTime) and (PlaceID='" + placeid + "' or PlaceID in (select PlaceID from Place where PAreasID in (select PAreasID from Place where PlaceID='" + placeid + "'))) and MoveState='未走动' ");
        OracleHelper.Query(strSql.ToString());
    }
    #endregion

    #region 公告附件下载
    public static bool FileDown(System.Web.UI.Page pg, string fuFileName, string fileName)//fuFileName文件存储的全路径名，fileName文件名
    {
        if (!System.IO.File.Exists(fuFileName))
        {
            return (false);
        }
        //创建一个文件实体，方便对文件的操作
        System.IO.FileInfo toDownload = new System.IO.FileInfo(fuFileName);
        //清空输出流
        pg.Response.Clear();
        pg.Response.Charset = "utf-8";
        pg.Response.Buffer = true;
        //关闭ViewState以提高速度
        pg.EnableViewState = false;
        //定义输出文件编码、类型和文件名
        pg.Response.ContentEncoding = System.Text.Encoding.UTF8;
        pg.Response.AppendHeader("Content-disposition", "attachment;filename=" + fileName);
        //保存类型不限
        pg.Response.ContentType = "application/unknown";
        pg.Response.WriteFile(fuFileName);
        //清空并关闭输出流
        pg.Response.Flush();
        pg.Response.Close();
        pg.Response.End();
        return (true);
    }
    #endregion
}
