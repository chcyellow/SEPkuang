using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using InfoSoftGlobal;
using Coolite.Ext.Web;
using System.Text;
using GhtnTech.SEP.DAL;
using GhtnTech.SecurityFramework.BLL;

public partial class LeaderSearch_TotalChart : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            //初始化年份
            cbbKind.Items.Clear();
            for (int i = 2010; i < 2099; i++)
            {
                cbbKind.Items.Add(new Coolite.Ext.Web.ListItem(i.ToString(), i.ToString()));
            }
            cbbKind.SelectedItem.Value = System.DateTime.Today.Year.ToString();
            lblYH.Show();
            lblSW.Show();
            if (!string.IsNullOrEmpty(this.Request["year"]))
            {
                //year = int.Parse(Request["year"]);
                pnlChart.Header = false;
                ddd.Visible = false;
            }
            LoadData();
        }
    }
    [AjaxMethod]
    public void LoadData()
    {
        lblYH.Html = FusionCharts.RenderChartHTML("../FC/Pie3D.swf?ChartNoDataText=当前查询结果为空", "", GetDataXMLYH(), "myNext", "100%", "100%", false);
        lblSW.Html = FusionCharts.RenderChartHTML("../FC/Pie3D.swf?ChartNoDataText=当前查询结果为空", "", GetDataXMLSW(), "myNext1", "100%", "100%", false);
    }

    private string GetDataXMLYH()
    {
        DBSCMDataContext dc = new DBSCMDataContext();
        string year = cbbKind.SelectedItem.Text.Trim() == "" ? System.DateTime.Today.Year.ToString() : cbbKind.SelectedItem.Text;
        var yh =from y in  dc.Nyhinput
                from c in dc.CsBaseinfoset
                where y.Levelid==c.Infoid
                select new
                {
                    y.Maindeptid,
                    y.Pctime,
                    c.Infoname
                };
        var group = from sw in yh
                    where sw.Maindeptid == SessionBox.GetUserSession().DeptNumber &&
                         sw.Pctime.Value.Year == int.Parse(year)
                    group sw by sw.Infoname into g
                    select new
                    {
                        Key = g.Key,
                        Total=g.Count()
                    };

        if (group.Count() == 0)
        {
            return "<chart />";
        }

        StringBuilder chartBuilder = new StringBuilder();
        chartBuilder.Append("<chart caption='隐患信息' palette='" + Functions.GetPalette() + "' animation='" + Functions.GetAnimationState() + "' subCaption='(" + year + "年度)'  showValues='0' formatNumberScale='0' showPercentInToolTip='0' baseFont='Arial' baseFontSize='12'>");

        foreach (var r in group)
        {
            string color = "";
            switch (r.Key.Trim().ToUpper())
            {
                case "A":
                    color = "FF0000";
                    break;
                case "B":
                    color = "FFA500";
                    break;
                case "C":
                    color = "000000";
                    break;
                case "D":
                    color = "0000FF";
                    break;
                default:
                    color = "C0C0C0";
                    break;
            }
            chartBuilder.Append("<set label='"+r.Key+"' value='"+r.Total+"' color='"+color+"' />");
        }
        chartBuilder.Append("<styles><definition><style type='font' name='CaptionFont' size='15' color='" + Functions.getCaptionFontColor + "' /><style type='font' name='SubCaptionFont' bold='0' /></definition><application><apply toObject='caption' styles='CaptionFont' /><apply toObject='SubCaption' styles='SubCaptionFont' /></application></styles>");
        chartBuilder.Append("<styles><definition><style type='font' name='CaptionFont' size='15' /><style type='font' name='SubCaptionFont' bold='0' /></definition><application><apply toObject='caption' styles='CaptionFont' /><apply toObject='SubCaption' styles='SubCaptionFont' /></application></styles>");
        chartBuilder.Append("</chart>");
        return chartBuilder.ToString();
    }

    private string GetDataXMLSW()
    {
        DBSCMDataContext dc = new DBSCMDataContext();
        string year = cbbKind.SelectedItem.Text.Trim() == "" ? System.DateTime.Today.Year.ToString() : cbbKind.SelectedItem.Text;
        var sw1 = from s in dc.Nswinput
                 from sb in dc.Swbase
                 from c in dc.CsBaseinfoset
                 where s.Swid == sb.Swid && sb.Levelid == c.Infoid
                 select new
                 {
                     s.Id,
                     Levelname = c.Infoname,
                     Dwid=s.Maindeptid,
                     s.Pctime
                 };
        var group = from sw in sw1
                    where sw.Dwid == SessionBox.GetUserSession().DeptNumber &&
                         sw.Pctime.Value.Year == int.Parse(year)
                    group sw by sw.Levelname into g
                    select new
                    {
                        Key = g.Key,
                        Total = g.Count()
                    };

        if (group.Count() == 0)
        {
            return "<chart />";
        }

        StringBuilder chartBuilder = new StringBuilder();
        chartBuilder.Append("<chart caption='三违信息' palette='" + Functions.GetPalette() + "' animation='" + Functions.GetAnimationState() + "' subCaption='(" + year + "年度)'  showValues='0' formatNumberScale='0' showPercentInToolTip='0' baseFont='Arial' baseFontSize='12'>");

        foreach (var r in group)
        {
            string color = "";
            switch (r.Key.Trim())
            {
                case "严重":
                    color = "FF0000";
                    break;
                case "一般":
                    color = "FFA500";
                    break;
                case "轻微":
                    color = "0000FF";
                    break;
                default:
                    color = "00FF00";
                    break;
            }
            chartBuilder.Append("<set label='" + r.Key + "' value='" + r.Total + "' color='" + color + "' />");
        }
        chartBuilder.Append("<styles><definition><style type='font' name='CaptionFont' size='15' color='" + Functions.getCaptionFontColor + "' /><style type='font' name='SubCaptionFont' bold='0' /></definition><application><apply toObject='caption' styles='CaptionFont' /><apply toObject='SubCaption' styles='SubCaptionFont' /></application></styles>");
        chartBuilder.Append("<styles><definition><style type='font' name='CaptionFont' size='15' /><style type='font' name='SubCaptionFont' bold='0' /></definition><application><apply toObject='caption' styles='CaptionFont' /><apply toObject='SubCaption' styles='SubCaptionFont' /></application></styles>");
        chartBuilder.Append("</chart>");
        return chartBuilder.ToString();
    }
}
