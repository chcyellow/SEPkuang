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
using System.Data;

public partial class LeaderSearch_MPChart : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            dfBegin.SelectedDate = System.DateTime.Today;
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
        pnlChart.Html = FusionCharts.RenderChartHTML("../FC/ScrollColumn2D.swf?ChartNoDataText=当前查询结果为空", "", GetDataXML(), "myNext", "100%", "100%", false);
    }

    private string GetDataXML()
    {
        DBSCMDataContext dc = new DBSCMDataContext();
        var table = GetSafeInfo.GetAllSafetyCountByDept(cbbUnit.SelectedItem.Value, dfBegin.SelectedDate, dfEnd.SelectedDate).Tables[0];
        try
        {
            var group = from t in table.AsEnumerable().ToList()
                        orderby int.Parse(t["YZD"].ToString()) descending, Convert.ToInt32(t["YHALL"]) + Convert.ToInt32(t["SWALL"]) descending
                        select new
                        {
                            //Key = t.Field<string>("DEPTNAME"),
                            //Total = t.Field<int>("YZD"),
                            //Fine = t.Field<int>("YHALL") + t.Field<int>("SWALL")
                            Key = t["DEPTNAME"].ToString(),
                            Total = int.Parse(t["YZD"].ToString()),
                            Fine = Convert.ToInt32(t["YHALL"]) + Convert.ToInt32(t["SWALL"])
                        };

            Store1.DataSource = group;
            Store1.DataBind();

            if (group.Count() == 0)
            {
                return "<chart />";
            }

            StringBuilder chartBuilder = new StringBuilder();
            chartBuilder.Append("<chart caption='走动情况分析' xAxisName='单位名称' yAxisName='数量'  showValues='0' palette='2' shownames='1' legendBorderAlpha='0' useRoundEdges='1' animation='1' decimalPrecision='0' formatNumberScale='0' baseFont='Arial' baseFontSize='12'>");

            string categories = "<categories>";
            string dataset1 = "<dataset seriesName='走动次数' color='AFD8F8' showValues='1'>";
            string dataset2 = "<dataset seriesName='问题数量' color='8BBA00' showValues='1'>";
            foreach (var r in group)
            {
                categories += "<category label='" + r.Key + "' />";
                dataset1 += "<set value='" + r.Total + "' />";
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
        catch
        {
            //Store1.DataSource = null;
            //Store1.DataBind();
            return "<chart />";
        }
    }
}
