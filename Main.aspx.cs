using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using GhtnTech.SEP.DBUtility;
using Coolite.Ext.Web;
using System.Data.OracleClient;
using GhtnTech.SecurityFramework.BLL;
using GhtnTech.SEP.DAL;

public partial class Main : BasePage
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            if (!SessionBox.CheckUserSession())
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                
                //this.ScriptManager1.RegisterClientScriptBlock("part", string.Format("var part=\"{0}\";", text));
                //this.ScriptManager1.RegisterClientScriptBlock("text", string.Format("var text=\"{0}\";", yhText));
                //Portlet2.Html = "={part}";
                //Portlet1.Html = "={text}";
                Ext.DoScript("Coolite.AjaxMethods.SetDBSY();");
                Ext.DoScript("Coolite.AjaxMethods.SetNewYH();");
            }
        }
    }

    [AjaxMethod]
    public void SetDBSY()
    {
        string text = "<ul>";
        //最新动态
        bool bl = false;
        //string yhText = GetLastHYInfo(DateTime.Now.Date);

        #region 隐患信息待办
        //获取当前待处理隐患
        UserHandle.InitModule("YSNewProcess_YHProcess");

        string PerID = SessionBox.GetUserSession().PersonNumber;
        string DepID = SessionBox.GetUserSession().DeptNumber;
        string DepIDK = GetDeptIDViaPersonNumber(SessionBox.GetUserSession().PersonNumber);
        string sql = "SELECT sum(CASE WHEN t.STATUS='新增' THEN 1 ELSE 0 END) XZYH,sum(CASE WHEN (t.STATUS='提交审批' AND t.REFER=1 AND t.REFERID='{0}') THEN 1 ELSE 0 END) LDSP, sum(CASE WHEN (t.STATUS='提交审批' AND t.REFER=2 AND t.REFERID='{1}') THEN 1 ELSE 0 END) ZNKSSP, sum(CASE WHEN (t.STATUS='隐患未整改' AND ch.RESPONSIBLEDEPT='{1}') THEN 1 ELSE 0 END) DZGYH, sum(CASE WHEN (t.STATUS='隐患已整改' AND m.MOVESTARTTIME<sysdate AND m.ENDTIME>sysdate) THEN 1 ELSE 0 END) DFCYH, sum(CASE WHEN t.STATUS='逾期未整改' THEN 1 ELSE 0 END) YQWZG,sum(CASE WHEN t.STATUS='复查未通过' THEN 1 ELSE 0 END) FCWTG FROM NYHINPUT t LEFT JOIN Moveplan m ON (t.PLACEID=m.PLACEID AND m.MOVESTATE='未走动' AND m.PERSONID='{0}') LEFT JOIN Nyinhuancheck ch ON t.YHPUTINID=ch.YHPUTINID WHERE t.MAINDEPTID='{2}'";
        DataTable dt = OracleHelper.Query(string.Format(sql, PerID, DepIDK, DepID)).Tables[0];
        //DataTable dt = SlowHelper.GetDBSY(PerID, DepIDK, DepID);
        if (decimal.Parse(dt.Rows[0]["XZYH"].ToString()) > 0 && (UserHandle.ValidationHandle(PermissionTag.YH_fbtj) || UserHandle.ValidationHandle(PermissionTag.YH_bhxz)))
        {//发布或闭合新增
            bl = true;
            text += string.Format("<li><span><a href={1}>{0}条</a></span>当前，您有待处理新增隐患信息：</li>", dt.Rows[0]["XZYH"].ToString(), "YSNewProcess/YHProcess.aspx?YHState=" + Server.UrlEncode("新增"));
        }
        if (decimal.Parse(dt.Rows[0]["LDSP"].ToString()) > 0 && UserHandle.ValidationHandle(PermissionTag.YH_kldsh))
        {//提交领导审批
            bl = true;
            string[] countYHdsp = MainData.getYHDSP_kld(PerID, DepID);//待审批
            text += string.Format("<li><span><a href={1}>{0}条</a></span>当前，您有领导审批的隐患信息：</li>", countYHdsp[0], "YSNewProcess/YHProcess.aspx?YHIDgroup=" + countYHdsp[1]);
        }
        if (decimal.Parse(dt.Rows[0]["ZNKSSP"].ToString()) > 0 && UserHandle.ValidationHandle(PermissionTag.YH_znkssh))
        {//提交职能科室审批

            bl = true;
            string[] countYHdsp = MainData.getYHDSP_znks(PerID, DepIDK);
            text += string.Format("<li><span><a href={1}>{0}条</a></span>当前，您有职能科室审批的隐患信息：</li>", countYHdsp[0], "YSNewProcess/YHProcess.aspx?YHIDgroup=" + countYHdsp[1]);
        }
        if (decimal.Parse(dt.Rows[0]["DZGYH"].ToString()) > 0 && UserHandle.ValidationHandle(PermissionTag.YH_zgfk))
        {//责任部门需要反馈
            bl = true;
            //string[] countYHwzg = MainData.getYHWZG_zrbmw(PerID, DepIDK);//未整改
            text += string.Format("<li><span><a href={1}>{0}条</a></span>当前，您单位需要整改的隐患信息：</li>", dt.Rows[0]["DZGYH"].ToString(), "YSNewProcess/YHProcess.aspx");
        }
        if (decimal.Parse(dt.Rows[0]["DFCYH"].ToString()) > 0 && UserHandle.ValidationHandle(PermissionTag.YH_fcfk))
        {//走动干部需要复查

            bl = true;
            string[] countYHdfc = MainData.getYHDFC_zdgb(PerID, DepID);//待复查
            text += string.Format("<li><span><a href={1}>{0}条</a></span>当前，您有‘待复查’的隐患信息：</li>", countYHdfc[0], "YSNewProcess/YHProcess.aspx?YHIDgroup=" + countYHdfc[1]);
        }

        if (UserHandle.ValidationHandle(PermissionTag.YH_fjpccl))//拥有分级排查权限--安监人员
        {
            if (decimal.Parse(dt.Rows[0]["YQWZG"].ToString()) > 0)
            {
                bl = true;
                text += string.Format("<li><span><a href={1}>{0}条</a></span>当前，您有待处理逾期未整改隐患信息：</li>", dt.Rows[0]["YQWZG"].ToString(), "YSNewProcess/YHProcess.aspx?YHState=" + Server.UrlEncode("逾期未整改"));
            }
            if (decimal.Parse(dt.Rows[0]["FCWTG"].ToString()) > 0)
            {
                bl = true;
                text += string.Format("<li><span><a href={1}>{0}条</a></span>当前，您有待处理复查未通过隐患信息：</li>", dt.Rows[0]["FCWTG"].ToString(), "YSNewProcess/YHProcess.aspx?YHState=" + Server.UrlEncode("复查未通过"));
            }
            //升级的先注释掉了
            //int up = MainData.getNocloseUpYH(DepID);
            //if (up > 0)
            //{
            //    bl = true;
            //    text += string.Format("<li><span><a href={1}>{0}条</a></span>当前，未闭合升级隐患信息：</li>", up.ToString(), "YSNewProcess/YHProcess.aspx");
            //}  
        }

        
        #endregion

        #region 走动计划信息待办
        //获取当前走动计划 --没找到走动干部的权限在哪 先注释掉
        //if (UserHandle.ValidationHandle(PermissionTag.Move))//拥有走动权限--走动干部
        //{
        //int countZD = MainData.getPLinfoD(PerID);
        var move = from a in dc.Moveplan
                   where (a.Starttime.Value <= DateTime.Today && DateTime.Today <= a.Endtime.Value) && a.Movestate == "未走动" && a.Personid == PerID
                   select new
                   {
                       a.Placeid
                   };
        if (move.Count() > 0)
        {
            bl = true;
            text += string.Format("<li><span><a href={1}>{0}条</a></span>您当前的走动任务有：</li>", move.Count(), "MovePlan/SearchMovePlan.aspx?do=1");
        }

        #endregion

        #region 三违信息待办
        //获取当前待处理三违
        UserHandle.InitModule("ThreeViolate_SWTreatment");
        if (UserHandle.ValidationHandle(PermissionTag.SW_xxcl))
        {
            string[] countSW = MainData.getSWinfoD();
            if (countSW[0] != "0")
            {
                bl = true;
                text += string.Format("<li><span><a href={1}>{0}条</a></span>当前您有需要处理的三违信息有：</li>", countSW[0], "ThreeViolate/SWTreatment.aspx?SWIDgroup=" + countSW[1]);
            }

            int yj = MainData.getYujingSW(SessionBox.GetUserSession().DeptNumber);
            if (yj > 0)
            {
                bl = true;
                text += string.Format("<li><span><a href={1}>{0}人</a></span>本年度，三违预警人员数量：</li>", yj.ToString(), "YSNewSearch/SWwarning.aspx");
            }
        }
        #endregion

        text += "</ul>";


        text += "</ul>";
        if (!bl)
        {
            text = "您当前无待处理信息！";
        }
        Portlet2.Html = text;
    }

    [AjaxMethod]
    public void SetNewYH()
    {
        Portlet1.Html = GetLastHYInfo(DateTime.Now.Date);
    }

    protected string GetDeptIDViaPersonNumber(string pn)//获取登陆用户单位ID
    {
        try
        {
            return (from psn in dc.Person where psn.Personnumber == pn select psn.Areadeptid).Single().Trim();
        }
        catch
        {
            return "";
        }
    }

    

    protected string GetLastHYInfo()//获取最新动态
    {
        string YH = "";
        string sql = "SELECT wm_concat(t.NAME||'在'||t.PLACENAME||'发现隐患：'||nvl(t.REMARKS,t.YHCONTENT)) kk FROM GETYHINPUT t WHERE (t.DEPTID='{0}' OR t.UNITID='{1}') AND to_char(t.PCTIME,'YYYY-MM-DD')= to_char(SYSDATE,'YYYY-MM-DD') AND rownum<15 GROUP BY t.YHPUTINID ORDER BY t.YHPUTINID DESC";
        DataTable dtable = OracleHelper.Query(string.Format(sql, GetDeptIDViaPersonNumber(SessionBox.GetUserSession().PersonNumber), SessionBox.GetUserSession().DeptNumber)).Tables[0];
        //DataTable dtable = SlowHelper.GetLastHYInfo(GetDeptIDViaPersonNumber(SessionBox.GetUserSession().PersonNumber), SessionBox.GetUserSession().DeptNumber);
        if (dtable.Rows.Count > 0)
        {
            foreach (DataRow hy in dtable.Rows)
            {

                YH += hy["KK"].ToString() + "<br />";
            }
        }
        else
        {
            YH = "当前无最新动态！";
        }
        return YH;
    }
    protected string GetLastHYInfo(DateTime dt)//获取最新动态
    {
        string YH = @"<br /><h2>本部门最新隐患：</h2>";
        var hyBBM = from h in dc.Getyhinput
                    where h.Intime.Value.Date == dt.Date && h.Deptid == dc.Person.First(p => p.Personnumber == SessionBox.GetUserSession().PersonNumber).Areadeptid //&& h.Maindeptid == SessionBox.GetUserSession().DeptNumber
                    orderby h.Intime descending
                    select h;
        if (hyBBM.Count() > 0)
        {
            foreach (var h in hyBBM)
            {
                //if (h.Yhid.ToString().Trim() != "")
                //{
                //    var hyTextQuery = from hyNum in dc.Yhbase
                //                      where hyNum.Yhid == h.Yhid
                //                      select hyNum.Yhcontent;
                //    var hyPlaceQuery = from hyplc in dc.Place where hyplc.Placeid == h.Placeid select hyplc.Placename;
                //    var hyPsnQuery = from hyPsn in dc.Person where hyPsn.Personnumber == h.Personid select hyPsn.Name;
                YH += string.Format("{0}在{1}发现隐患：{2}<br />", h.Name, h.Placename, h.Yhcontent);
                //}
            }
        }
        else
        {
            YH = "";
        }

        string lastYH = @"<br /><h2>本单位最新隐患：</h2>";
        var hyQuery = (from hy in dc.Getyhinput where hy.Intime.Value.Date == dt.Date && hy.Unitid == SessionBox.GetUserSession().DeptNumber orderby hy.Intime descending select hy).Take(20);
        if (hyQuery.Count() > 0)
        {
            foreach (var hy in hyQuery)
            {
                //if (hy.Yhid != null)
                //{
                //    var hyTextQuery = from hyNum in dc.Yhbase
                //                      where hyNum.Yhid == hy.Yhid
                //                      select hyNum.Yhcontent;
                //    var hyPlaceQuery = from hyplc in dc.Place where hyplc.Placeid == hy.Placeid select hyplc.Placename;
                //    var hyPsnQuery = from hyPsn in dc.Person where hyPsn.Personnumber == hy.Personid select hyPsn.Name;
                lastYH += string.Format("{0}在{1}发现隐患：{2}<br />", hy.Name, hy.Placename, hy.Yhcontent);
                //}
            }
        }
        else
        {
            lastYH = "当前无最新动态！";
        }
        return YH + lastYH;
    }
}
