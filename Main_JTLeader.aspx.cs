using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GhtnTech.SecurityFramework.IDAL;
using GhtnTech.SecurityFramework.BLL;
using Coolite.Ext.Web;
using System.Data.OracleClient;
using GhtnTech.SEP.DAL;

public partial class Main_JTLeader : System.Web.UI.Page
{
    private static string YHstr = "<a href=\"LeaderSearch/YHcondition.aspx?YHLevel={1}&begin={2}&end={3}\">{4}</a>";
    private static string SWstr = "<a href=\"LeaderSearch/SWcondition.aspx?SWLevel={1}&begin={2}&end={3}\">{4}</a>";
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            //待办事宜
            string text = "<ul>";
            //string[] countYH = MainData.getYHDSP_kld(SessionBox.GetUserSession().PersonNumber, SessionBox.GetUserSession().DeptNumber);
            text += string.Format("<span><font color=#cc0000><b>待办事宜：</b></font></span>您有待审批隐患：<a href={1}>{0}条</a>", 0, "HiddenDanage/HDprocess.aspx?YHIDgroup=''");
            //待督促隐患
            //var sms = from s in dc.TblSmsendtask
            //          from p in dc.Person
            //          from yh in dc.Yhinput
            //          where s.Destaddr == p.Tel && s.Msgid == yh.Yhputinid.ToString()
            //          && p.Personnumber == SessionBox.GetUserSession().PersonNumber && yh.Status == "隐患未整改"
            //          group s by s.Msgid into g
            //          select new
            //          {
            //              YHPutinID = g.Key
            //          };//group by 不要重复数据
            //string group = "";
            //foreach (var r in sms)
            //{
            //    group += r.YHPutinID.ToString() + ",";
            //}
            //if (group.Length > 0)
            //{
            //    group = group.Substring(0, group.Length - 1);
            //}
            //text += string.Format("&nbsp;&nbsp;&nbsp;&nbsp;<span>您有待督促隐患：<a href={1}>{0}条</a></span>", sms.Count(), "HiddenDanage/HDprocess.aspx?YHIDgroup=" + group);

            text += "</ul>";
            this.ScriptManager1.RegisterClientScriptBlock("part", string.Format("var part=\"{0}\";", text));
            lab.Html = "={part}";

            Ext.DoScript("Coolite.AjaxMethods.DayDataLoad();");
            Ext.DoScript("Coolite.AjaxMethods.WeekDataLoad();");
            Ext.DoScript("Coolite.AjaxMethods.MonthDataLoad();");
            Ext.DoScript("Coolite.AjaxMethods.YearDataLoad();");
            
        }
    }

    #region 全局安全动态

    [AjaxMethod]
    public void DayDataLoad()
    {
        //获取当天全局安全状况
        string textDay = @"<br /><b>今日安全动态：</b><br /><br />";
        var YHdataDay = GetSafeInfo.GetMainLeaderJTYH(System.DateTime.Today, System.DateTime.Today).Tables[0];
        var SWdataDay = GetSafeInfo.GetMainLeaderJTSW(System.DateTime.Today, System.DateTime.Today).Tables[0];
        if (YHdataDay.Rows.Count <= 0 && SWdataDay.Rows.Count <= 0)
        {
            textDay += "&nbsp;&nbsp;今日无安全动态!";
        }
        else
        {
            if (YHdataDay.Rows.Count > 0)
            {
                //textDay += "全局隐患信息：<br />";
                for (int i = 0; i < YHdataDay.Rows.Count; i++)
                {
                    textDay += string.Format("&nbsp;&nbsp;&nbsp;&nbsp;<span align=left><font color={5}><b>{0}</b></font>级隐患：" + YHstr + "条</span> <br />", YHdataDay.Rows[i]["LEVELNAME"].ToString().Trim(), Server.UrlEncode(YHdataDay.Rows[i]["LEVELNAME"].ToString().Trim()), System.DateTime.Today.ToString("yyyy-MM-dd"), System.DateTime.Today.ToString("yyyy-MM-dd"), YHdataDay.Rows[i]["YHNUM"].ToString().Trim(), GetColor(YHdataDay.Rows[i]["LEVELNAME"].ToString().Trim()));
                }
            }
            if (SWdataDay.Rows.Count > 0)
            {
                //textDay += "全局三违信息：<br />";
                for (int i = 0; i < SWdataDay.Rows.Count; i++)
                {
                    textDay += string.Format("&nbsp;&nbsp;&nbsp;&nbsp;<span align=left><font color={5}><b>{0}</b></font>级别三违：" + SWstr + "条</span> <br />", SWdataDay.Rows[i]["LEVELNAME"].ToString().Trim(), Server.UrlEncode(SWdataDay.Rows[i]["LEVELNAME"].ToString().Trim()), System.DateTime.Today.ToString("yyyy-MM-dd"), System.DateTime.Today.ToString("yyyy-MM-dd"), SWdataDay.Rows[i]["SWNUM"].ToString().Trim(), GetColor1(SWdataDay.Rows[i]["LEVELNAME"].ToString().Trim()));
                }
            }
        }
        this.DayPanel.Html = textDay;
    }

    [AjaxMethod]
    public void WeekDataLoad()
    {
        //获取本周全局安全状况
        string textWeek = @"<br /><b>本周安全动态：</b><br /><br />";
        var YHdataWeek = GetSafeInfo.GetMainLeaderJTYH(System.DateTime.Today.AddDays(1 - (int)System.DateTime.Today.DayOfWeek), System.DateTime.Today).Tables[0];
        var SWdataWeek = GetSafeInfo.GetMainLeaderJTSW(System.DateTime.Today.AddDays(1 - (int)System.DateTime.Today.DayOfWeek), System.DateTime.Today).Tables[0];
        if (YHdataWeek.Rows.Count <= 0 && SWdataWeek.Rows.Count <= 0)
        {
            textWeek += "&nbsp;&nbsp;本周无安全动态!";
        }
        else
        {
            if (YHdataWeek.Rows.Count > 0)
            {
                //textWeek += "全局隐患信息：<br />";
                for (int i = 0; i < YHdataWeek.Rows.Count; i++)
                {
                    textWeek += string.Format("&nbsp;&nbsp;&nbsp;&nbsp;<span align=left><font color={5}><b>{0}</b></font>级隐患：" + YHstr + "条</span> <br />", YHdataWeek.Rows[i]["LEVELNAME"].ToString().Trim(), Server.UrlEncode(YHdataWeek.Rows[i]["LEVELNAME"].ToString().Trim()), System.DateTime.Today.AddDays(1 - (int)System.DateTime.Today.DayOfWeek).ToString("yyyy-MM-dd"), System.DateTime.Today.ToString("yyyy-MM-dd"), YHdataWeek.Rows[i]["YHNUM"].ToString().Trim(), GetColor(YHdataWeek.Rows[i]["LEVELNAME"].ToString().Trim()));
                }
            }
            if (SWdataWeek.Rows.Count > 0)
            {
                //textWeek += "全局三违信息：<br />";
                for (int i = 0; i < SWdataWeek.Rows.Count; i++)
                {
                    textWeek += string.Format("&nbsp;&nbsp;&nbsp;&nbsp;<span align=left><font color={5}><b>{0}</b></font>级别三违：" + SWstr + "条 </span><br />", SWdataWeek.Rows[i]["LEVELNAME"].ToString().Trim(), Server.UrlEncode(SWdataWeek.Rows[i]["LEVELNAME"].ToString().Trim()), System.DateTime.Today.AddDays(1 - (int)System.DateTime.Today.DayOfWeek).ToString("yyyy-MM-dd"), System.DateTime.Today.ToString("yyyy-MM-dd"), SWdataWeek.Rows[i]["SWNUM"].ToString().Trim(), GetColor1(SWdataWeek.Rows[i]["LEVELNAME"].ToString().Trim()));
                }
            }
        }
        this.WeekPanel.Html = textWeek;
    }

    [AjaxMethod]
    public void MonthDataLoad()
    {
        //获取当月全局安全状况
        DateTime start = new DateTime(System.DateTime.Today.Year, System.DateTime.Today.Month, 1);
        DateTime end = new DateTime(System.DateTime.Today.Year, System.DateTime.Today.Month, DateTime.DaysInMonth(System.DateTime.Today.Year, System.DateTime.Today.Month));
        string textMonth = @"<br /><b>本月安全动态：</b><br /><br />";
        var YHdataMonth = GetSafeInfo.GetMainLeaderJTYH(start, end).Tables[0];
        var SWdataMonth = GetSafeInfo.GetMainLeaderJTSW(start, end).Tables[0];
        if (YHdataMonth.Rows.Count <= 0 && SWdataMonth.Rows.Count <= 0)
        {
            textMonth += "&nbsp;&nbsp;本月无安全动态!";
        }
        else
        {
            if (YHdataMonth.Rows.Count > 0)
            {
                //textMonth += "全局隐患信息：<br />";
                for (int i = 0; i < YHdataMonth.Rows.Count; i++)
                {
                    textMonth += string.Format("&nbsp;&nbsp;&nbsp;&nbsp;<span align=left><font color={5}><b>{0}</b></font>级隐患：" + YHstr + "条 </span><br />", YHdataMonth.Rows[i]["LEVELNAME"].ToString().Trim(), Server.UrlEncode(YHdataMonth.Rows[i]["LEVELNAME"].ToString().Trim()), System.DateTime.Today.AddDays(1 - System.DateTime.Today.Day).ToString("yyyy-MM-dd"), System.DateTime.Today.AddDays(1 - System.DateTime.Today.Day).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd"), YHdataMonth.Rows[i]["YHNUM"].ToString().Trim(), GetColor(YHdataMonth.Rows[i]["LEVELNAME"].ToString().Trim()));
                }
            }
            if (SWdataMonth.Rows.Count > 0)
            {
                //textMonth += "全局三违信息：<br />";
                for (int i = 0; i < SWdataMonth.Rows.Count; i++)
                {
                    textMonth += string.Format("&nbsp;&nbsp;&nbsp;&nbsp;<span align=left><font color={5}>{0}</font>级别三违：" + SWstr + "条 </span><br />", SWdataMonth.Rows[i]["LEVELNAME"].ToString().Trim(), Server.UrlEncode(SWdataMonth.Rows[i]["LEVELNAME"].ToString().Trim()), System.DateTime.Today.AddDays(1 - System.DateTime.Today.Day).ToString("yyyy-MM-dd"), System.DateTime.Today.AddDays(1 - System.DateTime.Today.Day).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd"), SWdataMonth.Rows[i]["SWNUM"].ToString().Trim(), GetColor1(SWdataMonth.Rows[i]["LEVELNAME"].ToString().Trim()));
                }
            }
        }
        this.MonthPanel.Html = textMonth;
    }

    [AjaxMethod]
    public void YearDataLoad()
    {
        //获取当年全局安全状况
        int date = System.DateTime.Today.Year;
        string textYear = @"<br /><b>" + date + "年全局安全动态：</b><br /><br />";
        DateTime daybegin = new DateTime(System.DateTime.Today.Year, 1, 1);
        DateTime dayend = new DateTime(System.DateTime.Today.Year, 12, 31);
        var YHdata = GetSafeInfo.GetMainLeaderJTYH(daybegin, dayend).Tables[0];
        var SWdata = GetSafeInfo.GetMainLeaderJTSW(daybegin, dayend).Tables[0];
        if (YHdata.Rows.Count <= 0 && SWdata.Rows.Count <= 0)
        {
            textYear += "&nbsp;&nbsp;本年度无安全动态!";
        }
        else
        {
            if (YHdata.Rows.Count > 0)
            {
                //textYear += "全局隐患信息：<br />";
                for (int i = 0; i < YHdata.Rows.Count; i++)
                {
                    textYear += string.Format("&nbsp;&nbsp;&nbsp;&nbsp;<span align=left> <font color={2}><b>{0}</b></font>级隐患：{1}条 <br /></span>", YHdata.Rows[i]["LEVELNAME"].ToString().Trim(), YHdata.Rows[i]["YHNUM"].ToString().Trim(), GetColor(YHdata.Rows[i]["LEVELNAME"].ToString().Trim()));
                }
            }
            if (SWdata.Rows.Count > 0)
            {
                //textYear += "全局三违信息：<br />";

                for (int i = 0; i < SWdata.Rows.Count; i++)
                {
                    textYear += string.Format("&nbsp;&nbsp;&nbsp;&nbsp;<span align=left><font color={2}><b>{0}</b></font>级别三违：{1}条 <br /></span>", SWdata.Rows[i]["LEVELNAME"].ToString().Trim(), SWdata.Rows[i]["SWNUM"].ToString().Trim(), GetColor1(SWdata.Rows[i]["LEVELNAME"].ToString().Trim()));
                }
            }
        }
        this.YearPanel.Html = textYear;
    }

    #endregion

    private Coolite.Ext.Web.HyperLink HLCreate(string text, string url, Icon icon)
    {
        Coolite.Ext.Web.HyperLink hl = new Coolite.Ext.Web.HyperLink();
        hl.Text = text;
        hl.NavigateUrl = url;
        hl.Icon = icon;
        return hl;
    }

    //隐患颜色处理
    private string GetColor(string lvl)
    {
        switch (lvl)
        {
            case "A":
                return "#cc0000";
            case "B":
                return "#FF9900";
            case "C":
                return "black";
            case "D":
                return "blue";
            default:
                return "black";
        }
    }
    //三违颜色处理
    private string GetColor1(string lvl)
    {
        switch (lvl.ToLower().Trim())
        {
            case "严重":
                return "#cc0000";
            case "一般":
                return "#FF9900";
            case "轻微":
                return "blue";
            default:
                return "black";
        }
    }
}
