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

public partial class Main_Leader : System.Web.UI.Page
{
    private static string YHstr = "<a href=\"LeaderSearch/YHcondition.aspx?YHLevel={1}&begin={2}&end={3}\">{4}</a>";
    private static string SWstr = "<a href=\"LeaderSearch/SWcondition.aspx?SWLevel={1}&begin={2}&end={3}\">{4}</a>";

    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            //改变走动状态
            //AutoDoSth();
            //待办事宜
            string text = "<ul>";
            string[] countYH = MainData.getYHDSP_kld(SessionBox.GetUserSession().PersonNumber, SessionBox.GetUserSession().DeptNumber);
            text += string.Format("<span><font color=#cc0000><b>待办事宜：</b></font></span>您有待审批隐患：<a href={1}>{0}条</a>", countYH[0], "YSNewProcess/YHProcess.aspx?YHIDgroup=" + (countYH[1].Length == 0 ? "-1" : countYH[1]));
            //待督促隐患
            var sms = from s in dc.TblSmsendtask
                      from p in dc.Person
                      from yh in dc.Getyhinput
                      where s.Destaddr== p.Tel && s.Msgid == yh.Yhputinid.ToString()
                      && p.Personnumber == SessionBox.GetUserSession().PersonNumber && yh.Status == "隐患未整改"
                      group s by s.Msgid into g
                      select new
                      {
                          YHPutinID = g.Key
                      };//group by 不要重复数据
            string group = "";
            foreach (var r in sms)
            {
                group += r.YHPutinID.ToString() + ",";
            }
            if (group.Length > 0)
            {
                group = group.Substring(0, group.Length - 1);
            }
            else
            {
                group = "-1";
            }
            text += string.Format("&nbsp;&nbsp;&nbsp;&nbsp;<span>您有待督促隐患：<a href={1}>{0}条</a></span>", sms.Count(), "YSNewProcess/YHProcess.aspx?YHIDgroup=" + group);

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
        //获取当天全矿安全状况
        string textDay = @"<br /><b>今日安全动态：</b><br /><br />";

        //var YHdataDay = from c in dc.Yhview
        //                where c.Pctime == System.DateTime.Today && c.Maindeptid==SessionBox.GetUserSession().DeptNumber
        //                group c by c.Levelname into g 
        //                orderby g.Key
        //                select new
        //                {
        //                    YHLevel = g.Key,
        //                    YHnum = g.Count()
        //                };
        //var SWdataDay = from c in dc.Getsanwei
        //                where c.Pctime == System.DateTime.Today && c.Dwid == SessionBox.GetUserSession().DeptNumber
        //                group c by c.Levelname into g
        //                select new
        //                {
        //                    SWLevel = g.Key,
        //                    SWnum = g.Count()
        //                };
        //几个视图未改成最新
        var YHdataDay = GetSafeInfo.GetMainLeaderYH(System.DateTime.Today, System.DateTime.Today, SessionBox.GetUserSession().DeptNumber).Tables[0];
        //var SWdataDay = GetSafeInfo.GetMainLeaderSW(System.DateTime.Today, System.DateTime.Today, SessionBox.GetUserSession().DeptNumber).Tables[0];
        var SWdataDay = from s in dc.Getsanwei
                        where s.Pctime == System.DateTime.Today
                        && s.Dwid == SessionBox.GetUserSession().DeptNumber
                        group s by s.Levelname into g
                        select new
                        {
                            g.Key,
                            swnum = g.Count()
                        };
        if (YHdataDay.Rows.Count <= 0 && SWdataDay.Count() <= 0)
        {
            textDay += "&nbsp;&nbsp;今日无安全动态!";
        }
        else
        {
            if (YHdataDay.Rows.Count > 0)
            {
                //textDay += "<font color=red>全矿隐患信息：</font><br />";
                textDay += "全矿隐患信息：<br />";
                for (int i = 0; i < YHdataDay.Rows.Count; i++)
                {
                    textDay += string.Format("&nbsp;&nbsp;&nbsp;&nbsp;<span align=left><font color={5}><b>{0}</b></font>级隐患：" + YHstr + "条</span> <br />", YHdataDay.Rows[i]["LEVELNAME"].ToString().Trim(), Server.UrlEncode(YHdataDay.Rows[i]["LEVELNAME"].ToString().Trim()), System.DateTime.Today.ToString("yyyy-MM-dd"), System.DateTime.Today.ToString("yyyy-MM-dd"), YHdataDay.Rows[i]["YHNUM"].ToString().Trim(), GetColor(YHdataDay.Rows[i]["LEVELNAME"].ToString().Trim()));
                }
            }
            if (SWdataDay.Count() > 0)
            {
                textDay += "全矿三违信息：<br />";
                foreach (var r in SWdataDay)
                {
                    textDay += string.Format("&nbsp;&nbsp;&nbsp;&nbsp;<span align=left><font color={5}><b>{0}</b></font>级别三违：" + SWstr + "条 </span><br />", r.Key, Server.UrlEncode(r.Key), System.DateTime.Today.ToString("yyyy-MM-dd"), System.DateTime.Today.ToString("yyyy-MM-dd"), r.swnum, GetColor1(r.Key));
                }
            }
        }
        this.DayPanel.Html = textDay;
    }

    [AjaxMethod]
    public void WeekDataLoad()
    {
        //获取本周全矿安全状况
        string textWeek = @"<br /><b>本周安全动态：</b><br /><br />";
        //var YHdataWeek = from c in dc.Yhview
        //                 where (c.Pctime.Value >= System.DateTime.Today.AddDays(1 - (int)System.DateTime.Today.DayOfWeek) && c.Pctime.Value <= System.DateTime.Today) && c.Maindeptid == SessionBox.GetUserSession().DeptNumber
        //                 group c by c.Levelname into g
        //                 orderby g.Key
        //                 select new
        //                 {
        //                     YHLevel = g.Key,
        //                     YHnum = g.Count()
        //                 };
        //var SWdataWeek = from c in dc.Getsanwei
        //                 where (c.Pctime.Value >= System.DateTime.Today.AddDays(1 - (int)System.DateTime.Today.DayOfWeek) && c.Pctime.Value <= System.DateTime.Today) && c.Dwid == SessionBox.GetUserSession().DeptNumber
        //                 group c by c.Levelname into g
        //                 select new
        //                 {
        //                     SWLevel = g.Key,
        //                     SWnum = g.Count()
        //                 };
        var YHdataWeek = GetSafeInfo.GetMainLeaderYH(System.DateTime.Today.AddDays(1 - (int)System.DateTime.Today.DayOfWeek), System.DateTime.Today, SessionBox.GetUserSession().DeptNumber).Tables[0];
        //var SWdataWeek = GetSafeInfo.GetMainLeaderSW(System.DateTime.Today.AddDays(1 - (int)System.DateTime.Today.DayOfWeek), System.DateTime.Today, SessionBox.GetUserSession().DeptNumber).Tables[0];
        var SWdataWeek = from s in dc.Getsanwei
                         where s.Pctime >= System.DateTime.Today.AddDays(1 - (int)System.DateTime.Today.DayOfWeek) && s.Pctime <= System.DateTime.Today
                         && s.Dwid == SessionBox.GetUserSession().DeptNumber
                         group s by s.Levelname into g
                         select new
                         {
                             g.Key,
                             swnum = g.Count()
                         };
        if (YHdataWeek.Rows.Count <= 0 && SWdataWeek.Count() <= 0)
        {
            textWeek += "&nbsp;&nbsp;本周无安全动态!";
        }
        else
        {
            if (YHdataWeek.Rows.Count > 0)
            {
                textWeek += "全矿隐患信息：<br />";
                for (int i = 0; i < YHdataWeek.Rows.Count; i++)
                {
                    textWeek += string.Format("&nbsp;&nbsp;&nbsp;&nbsp;<span align=left><font color={5}><b>{0}</b></font>级隐患：" + YHstr + "条</span> <br />", YHdataWeek.Rows[i]["LEVELNAME"].ToString().Trim(), Server.UrlEncode(YHdataWeek.Rows[i]["LEVELNAME"].ToString().Trim()), System.DateTime.Today.AddDays(1 - (int)System.DateTime.Today.DayOfWeek).ToString("yyyy-MM-dd"), System.DateTime.Today.ToString("yyyy-MM-dd"), YHdataWeek.Rows[i]["YHNUM"].ToString().Trim(), GetColor(YHdataWeek.Rows[i]["LEVELNAME"].ToString().Trim()));
                }
            }
            if (SWdataWeek.Count() > 0)
            {
                textWeek += "全矿三违信息：<br />";
                foreach (var r in SWdataWeek)
                {
                    textWeek += string.Format("&nbsp;&nbsp;&nbsp;&nbsp;<span align=left><font color={5}><b>{0}</b></font>级别三违：" + SWstr + "条 </span><br />", r.Key, Server.UrlEncode(r.Key), System.DateTime.Today.AddDays(1 - (int)System.DateTime.Today.DayOfWeek).ToString("yyyy-MM-dd"), System.DateTime.Today.ToString("yyyy-MM-dd"), r.swnum, GetColor1(r.Key));
                }
            }
        }
        this.WeekPanel.Html = textWeek;
    }

    [AjaxMethod]
    public void MonthDataLoad()
    {
        //获取当月全矿安全状况
        DateTime start = new DateTime(System.DateTime.Today.Year, System.DateTime.Today.Month, 1);
        DateTime end = new DateTime(System.DateTime.Today.Year, System.DateTime.Today.Month, DateTime.DaysInMonth(System.DateTime.Today.Year, System.DateTime.Today.Month));
        string textMonth = @"<br /><b>本月安全动态：</b><br /><br />";
        //var YHdataMonth = from c in dc.Yhview
        //                  where (c.Pctime.Value >= start && c.Pctime.Value <= end) && c.Maindeptid == SessionBox.GetUserSession().DeptNumber
        //                  group c by c.Levelname into g
        //                  orderby g.Key
        //                  select new
        //                  {
        //                      YHLevel = g.Key,
        //                      YHnum = g.Count()
        //                  };
        //var SWdataMonth = from c in dc.Getsanwei
        //                  where (c.Pctime.Value >= start && c.Pctime.Value <= end) && c.Dwid == SessionBox.GetUserSession().DeptNumber
        //                  group c by c.Levelname into g
        //                  select new
        //                  {
        //                      SWLevel = g.Key,
        //                      SWnum = g.Count()
        //                  };
        var YHdataMonth = GetSafeInfo.GetMainLeaderYH(start, end, SessionBox.GetUserSession().DeptNumber).Tables[0];
        //var SWdataMonth = GetSafeInfo.GetMainLeaderSW(start, end, SessionBox.GetUserSession().DeptNumber).Tables[0];
        var SWdataMonth = from s in dc.Getsanwei
                          where s.Pctime >= start && s.Pctime <= end
                          && s.Dwid == SessionBox.GetUserSession().DeptNumber
                          group s by s.Levelname into g
                          select new
                          {
                              g.Key,
                              swnum = g.Count()
                          };
        if (YHdataMonth.Rows.Count <= 0 && SWdataMonth.Count() <= 0)
        {
            textMonth += "&nbsp;&nbsp;本月无安全动态!";
        }
        else
        {
            if (YHdataMonth.Rows.Count > 0)
            {
                textMonth += "全矿隐患信息：<br />";
                for (int i = 0; i < YHdataMonth.Rows.Count; i++)
                {
                    textMonth += string.Format("&nbsp;&nbsp;&nbsp;&nbsp;<span align=left><font color={5}><b>{0}</b></font>级隐患：" + YHstr + "条 </span><br />", YHdataMonth.Rows[i]["LEVELNAME"].ToString().Trim(), Server.UrlEncode(YHdataMonth.Rows[i]["LEVELNAME"].ToString().Trim()), System.DateTime.Today.AddDays(1 - System.DateTime.Today.Day).ToString("yyyy-MM-dd"), System.DateTime.Today.AddDays(1 - System.DateTime.Today.Day).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd"), YHdataMonth.Rows[i]["YHNUM"].ToString().Trim(), GetColor(YHdataMonth.Rows[i]["LEVELNAME"].ToString().Trim()));
                }
            }
            if (SWdataMonth.Count() > 0)
            {
                textMonth += "全矿三违信息：<br />";
                foreach (var r in SWdataMonth)
                {
                    textMonth += string.Format("&nbsp;&nbsp;&nbsp;&nbsp;<span align=left><font color={5}><b>{0}</b></font>级别三违：" + SWstr + "条 </span><br />", r.Key, Server.UrlEncode(r.Key), start, end, r.swnum, GetColor1(r.Key));
                }
                //for (int i = 0; i < SWdataMonth.Rows.Count;i++)
                //{
                //    textMonth += string.Format("&nbsp;&nbsp;&nbsp;&nbsp;<span align=left><font color={5}>{0}</font>级别三违：" + SWstr + "条 </span><br />", SWdataMonth.Rows[i]["LEVELNAME"].ToString().Trim(), Server.UrlEncode(SWdataMonth.Rows[i]["LEVELNAME"].ToString().Trim()), System.DateTime.Today.AddDays(1 - System.DateTime.Today.Day).ToString("yyyy-MM-dd"), System.DateTime.Today.AddDays(1 - System.DateTime.Today.Day).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd"), SWdataMonth.Rows[i]["SWNUM"].ToString().Trim(), GetColor1(SWdataMonth.Rows[i]["LEVELNAME"].ToString().Trim()));
                //}
            }
        }
        this.MonthPanel.Html = textMonth;
    }

    [AjaxMethod]
    public void YearDataLoad()
    {
        //获取当年全矿安全状况
        int date = System.DateTime.Today.Year;
        string textYear = @"<br /><b>" + date + "年全矿安全动态：</b><br /><br />";
        //var YHdata = from c in dc.Yhview
        //             where c.Pctime.Value.Year == System.DateTime.Today.Year && c.Maindeptid == SessionBox.GetUserSession().DeptNumber
        //             group c by c.Levelname into g
        //             orderby g.Key
        //             select new
        //             {
        //                 YHLevel = g.Key,
        //                 YHnum = g.Count()
        //             };
        //var SWdata = from c in dc.Getsanwei
        //             where c.Pctime.Value.Year == System.DateTime.Today.Year && c.Dwid == SessionBox.GetUserSession().DeptNumber
        //             group c by c.Levelname into g
        //             select new
        //             {
        //                 SWLevel = g.Key,
        //                 SWnum = g.Count()
        //             };
        DateTime daybegin = new DateTime(System.DateTime.Today.Year, 1, 1);
        DateTime dayend = new DateTime(System.DateTime.Today.Year, 12, 31);
        var YHdata = GetSafeInfo.GetMainLeaderYH(daybegin, dayend, SessionBox.GetUserSession().DeptNumber).Tables[0];
        //var SWdata = GetSafeInfo.GetMainLeaderSW(daybegin, dayend, SessionBox.GetUserSession().DeptNumber).Tables[0];
        var SWdata = from s in dc.Getsanwei
                     where s.Pctime >= daybegin && s.Pctime <= dayend
                          && s.Dwid == SessionBox.GetUserSession().DeptNumber
                     group s by s.Levelname into g
                     select new
                     {
                         g.Key,
                         swnum = g.Count()
                     };
        if (YHdata.Rows.Count <= 0 && SWdata.Count() <= 0)
        {
            textYear += "&nbsp;&nbsp;本年度无安全动态!";
        }
        else
        {
            if (YHdata.Rows.Count > 0)
            {
                textYear += "全矿隐患信息：<br />";
                for (int i = 0; i < YHdata.Rows.Count; i++)
                {
                    textYear += string.Format("&nbsp;&nbsp;&nbsp;&nbsp;<span align=left> <font color={2}><b>{0}</b></font>级隐患：{1}条 <br /></span>", YHdata.Rows[i]["LEVELNAME"].ToString().Trim(), YHdata.Rows[i]["YHNUM"].ToString().Trim(), GetColor(YHdata.Rows[i]["LEVELNAME"].ToString().Trim()));
                }
            }
            if (SWdata.Count() > 0)
            {
                textYear += "全矿三违信息：<br />";
                foreach (var r in SWdata)
                {
                    textYear += string.Format("&nbsp;&nbsp;&nbsp;&nbsp;<span align=left><font color={5}><b>{0}</b></font>级别三违：" + SWstr + "条 </span><br />", r.Key, Server.UrlEncode(r.Key), daybegin, dayend, r.swnum, GetColor1(r.Key));
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

    private void AutoDoSth()//自动处理流程-干部走动
    {
        PublicMethod.AutoSetMovePlanStatus();
    }
}
