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

public partial class LeaderSearch_SWChart : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            dfBegin.SelectedDate = System.DateTime.Today.AddDays(1-System.DateTime.Today.Day);
            dfEnd.SelectedDate = System.DateTime.Today;
            dfBegin.MaxDate = System.DateTime.Today;
            dfEnd.MaxDate = System.DateTime.Today;

            #region 初始化单位
            DBSCMDataContext dc = new DBSCMDataContext();
            if (SessionBox.GetUserSession().rolelevel.Trim().IndexOf("1") > -1)
            {
                var dept = from d in dc.Department
                           where d.Deptnumber.Substring(4) == "00000" && d.Deptname.EndsWith("矿")
                           select new
                           {
                               d.Deptname,
                               Deptid = d.Deptnumber
                           };
                UnitStore.DataSource = dept;
                UnitStore.DataBind();
                cbbUnit.SelectedItem.Value = "241700000";
                cbbUnit.Disabled = false;

            }
            else
            {
                var dept = from d in dc.Department
                           where d.Deptnumber == SessionBox.GetUserSession().DeptNumber
                           select new
                           {
                               d.Deptname,
                               Deptid = d.Deptnumber
                           };
                UnitStore.DataSource = dept;
                UnitStore.DataBind();
                cbbUnit.SelectedItem.Value = SessionBox.GetUserSession().DeptNumber;
                cbbUnit.Disabled = true;
            }
            #endregion

            LoadData();
        }
    }
    [AjaxMethod]
    public void LoadData()
    {
        pnlChart.Html = FusionCharts.RenderChartHTML("../FC/MSColumn3DLineDY.swf?ChartNoDataText=当前查询结果为空", "", GetDataXML(), "myNext", "100%", "100%", false);
    }

    private string GetDataXML()
    {
        DBSCMDataContext dc = new DBSCMDataContext();
        var data = (from sw in dc.Nswinput
                   from sb in dc.Swbase
                   from c in dc.CsBaseinfoset
                   from p in dc.Person 
                   from d in dc.Department
                   from pl in dc.Place
                   where sw.Swid == sb.Swid && sb.Levelid == c.Infoid && sw.Swpersonid==p.Personnumber && p.Areadeptid==d.Deptnumber &&
                   sw.Placeid==pl.Placeid &&
                   sw.Pctime >= dfBegin.SelectedDate && sw.Pctime <= dfEnd.SelectedDate && sw.Maindeptid == cbbUnit.SelectedItem.Value
                   select new
                   {
                       sw.Id,
                       c.Infoname,
                       sw.Fine,
                       d.Deptname,
                       pl.Placename
                   }).ToList();
        var group = from sw in data
                    group sw by sw.Infoname into g
                    select new
                    {
                        Key = g.Key,
                        Total = g.Count(),
                        Fine = g.Sum(p => p.Fine).GetValueOrDefault(0)
                    };
        switch(cbbKind.SelectedItem.Value)
        {
            case "1":
                group = from sw in data
                        group sw by sw.Deptname into g
                            select new
                            {
                                Key = g.Key,
                                Total = g.Count(),
                                Fine = g.Sum(p => p.Fine).GetValueOrDefault(0)
                            };
                break;
            case "2":
                group = from sw in data
                        group sw by sw.Placename into g
                        select new
                        {
                            Key = g.Key,
                            Total = g.Count(),
                            Fine = g.Sum(p => p.Fine).GetValueOrDefault(0)
                        };
                break;
        }
        group = group.OrderByDescending(p => p.Total);
                        
        GridPanel1.ColumnModel.SetColumnHeader(0, cbbKind.SelectedItem.Text.Trim() == "" ? "三违级别" : cbbKind.SelectedItem.Text);
        Store1.DataSource = group;
        Store1.DataBind();

        string labelFormatting = " labelDisplay='WRAP' "; 
        if (group.Count() == 0)
        {
            return "<chart />";
        }
        else if (group.Count() > 10)
        {
            labelFormatting = " labelDisplay='ROTATE' slantLabels='1' ";
        }

        StringBuilder chartBuilder = new StringBuilder();
        chartBuilder.Append("<chart caption='三违分析' xAxisName='" + (cbbKind.SelectedItem.Text.Trim() == "" ? "三违级别" : cbbKind.SelectedItem.Text) + "' sYAxisValuesDecimals='2' connectNullData='0' PYAxisName='三违数量' SYAxisName='罚款金额(元)'  showValues='0' palette='2' shownames='1' legendBorderAlpha='0' useRoundEdges='1' animation='1' decimalPrecision='0' formatNumberScale='0' baseFont='Arial' baseFontSize='12' " + labelFormatting + ">");
        
        string categories = "<categories>";
        string dataset1 = "<dataset seriesName='三违数量' color='AFD8F8' showValues='1'>";
        string dataset2 = "<dataset seriesName='罚款金额' color='8BBA00' showValues='1' parentYAxis='S'>";
        foreach (var r in group)
        {
            categories += "<category label='"+r.Key+"' />";
            dataset1 += "<set value='"+r.Total+"' />";
            dataset2 += "<set value='" + r.Fine + "' />";
        }
        categories += "</categories>";
        dataset1 += "</dataset>";
        dataset2 += "</dataset>";
        chartBuilder.Append(categories);
        chartBuilder.Append(dataset1);
        chartBuilder.Append(dataset2);
        chartBuilder.Append("</chart>");
        return chartBuilder.ToString();
    }
}
