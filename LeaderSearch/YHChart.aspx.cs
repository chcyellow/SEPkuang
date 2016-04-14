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

public partial class LeaderSearch_YHChart : System.Web.UI.Page
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

        
        if (cbbKind.SelectedItem.Value == "1" || cbbKind.SelectedItem.Value=="")
        {
            var table = GetSafeInfo.GetAllSafetyCountByDept(cbbUnit.SelectedItem.Value,dfBegin.SelectedDate, dfEnd.SelectedDate).Tables[0];
            try
            {
                //var group = from t in table.AsEnumerable()
                //            where (t["YHALL"].ToString() != "0" || t["YHYZG"].ToString() != "0" || t["YHWZG"].ToString()!="0")
                //            orderby int.Parse(t["YHALL"].ToString()) descending
                //            select new
                //            {
                //                Key = t["DEPTNAME"].ToString(),
                //                Total = t["YHALL"].ToString(),
                //                Pass = t["YHYZG"].ToString(),
                //                NPass = t["YHWZG"].ToString()
                //            };
                var data = (from sw in dc.Getyhinput
                           where sw.Unitid == cbbUnit.SelectedItem.Value &&
                           sw.Pctime >= dfBegin.SelectedDate && sw.Pctime <= dfEnd.SelectedDate
                           select sw).ToList();
                var group = from sw in data
                            group sw by sw.Deptname into g
                            select new
                            {
                                Key = g.Key,
                                Total = g.Count(),
                                Pass = g.Count(p => p.Status == "复查通过" || p.Status == "现场整改"),
                                NPass = g.Count(p => p.Status != "复查通过" && p.Status != "现场整改")
                            };
                GridPanel1.ColumnModel.SetColumnHeader(0, cbbKind.SelectedItem.Text.Trim() == "" ? "隐患部门" : cbbKind.SelectedItem.Text);
                Store1.DataSource = group;
                Store1.DataBind();

                if (group.Count() == 0)
                {
                    return "<chart />";
                }

                StringBuilder chartBuilder = new StringBuilder();
                chartBuilder.Append("<chart caption='隐患分析' xAxisName='" + (cbbKind.SelectedItem.Text.Trim() == "" ? "隐患部门" : cbbKind.SelectedItem.Text) + "' yAxisName='数量'  showValues='0' palette='2' shownames='1' legendBorderAlpha='0' useRoundEdges='1' animation='1' decimalPrecision='0' formatNumberScale='0' baseFont='Arial' baseFontSize='12'>");

                string categories = "<categories>";
                string dataset1 = "<dataset seriesName='隐患数量' color='AFD8F8' showValues='1'>";
                string dataset2 = "<dataset seriesName='已解决' color='8BBA00' showValues='1'>";
                string dataset3 = "<dataset seriesName='未解决' color='8B0000' showValues='1'>";
                foreach (var r in group)
                {
                    categories += "<category label='" + r.Key + "' />";
                    dataset1 += "<set value='" + r.Total + "' />";
                    dataset2 += "<set value='" + r.Pass + "' />";
                    dataset3 += "<set value='" + r.NPass + "' />";
                }
                categories += "</categories>";
                dataset1 += "</dataset>";
                dataset2 += "</dataset>";
                dataset3 += "</dataset>";
                chartBuilder.Append(categories);
                chartBuilder.Append(dataset1);
                chartBuilder.Append(dataset2);
                chartBuilder.Append(dataset3);
                chartBuilder.Append("</chart>");
                return chartBuilder.ToString();
            }
            catch
            {
                Store1.DataSource = null;
                Store1.DataBind();
                return "<chart />";
            }
        }
        else
        {
            var date = (from sw in dc.Getyhinput
                       where sw.Unitid == cbbUnit.SelectedItem.Value &&
                         sw.Pctime >= dfBegin.SelectedDate && sw.Pctime <= dfEnd.SelectedDate
                            select sw).ToList();
            var group = from sw in date
                        group sw by sw.Placename into g
                        orderby g.Count() descending
                        select new
                        {
                            Key = g.Key,
                            Total = g.Count(),
                            Pass = g.Count(p => p.Status == "复查通过" || p.Status == "现场整改"),
                            NPass = g.Count(p => p.Status != "复查通过" && p.Status != "现场整改")
                        };
            GridPanel1.ColumnModel.SetColumnHeader(0, cbbKind.SelectedItem.Text.Trim() == "" ? "隐患部门" : cbbKind.SelectedItem.Text);
            Store1.DataSource = group;
            Store1.DataBind();

            if (group.Count() == 0)
            {
                return "<chart />";
            }

            StringBuilder chartBuilder = new StringBuilder();
            chartBuilder.Append("<chart caption='隐患分析' xAxisName='" + (cbbKind.SelectedItem.Text.Trim() == "" ? "隐患部门" : cbbKind.SelectedItem.Text) + "' yAxisName='数量'  showValues='0' palette='2' shownames='1' legendBorderAlpha='0' useRoundEdges='1' animation='1' decimalPrecision='0' formatNumberScale='0' baseFont='Arial' baseFontSize='12'>");

            string categories = "<categories>";
            string dataset1 = "<dataset seriesName='隐患数量' color='AFD8F8' showValues='1'>";
            string dataset2 = "<dataset seriesName='已解决' color='8BBA00' showValues='1'>";
            string dataset3 = "<dataset seriesName='未解决' color='8B0000' showValues='1'>";
            foreach (var r in group)
            {
                categories += "<category label='" + r.Key + "' />";
                dataset1 += "<set value='" + r.Total + "' />";
                dataset2 += "<set value='" + r.Pass + "' />";
                dataset3 += "<set value='" + r.NPass + "' />";
            }
            categories += "</categories>";
            dataset1 += "</dataset>";
            dataset2 += "</dataset>";
            dataset3 += "</dataset>";
            chartBuilder.Append(categories);
            chartBuilder.Append(dataset1);
            chartBuilder.Append(dataset2);
            chartBuilder.Append(dataset3);
            chartBuilder.Append("</chart>");
            return chartBuilder.ToString();
        }
        //GridPanel1.ColumnModel.SetColumnHeader(0, cbbKind.SelectedItem.Text.Trim() == "" ? "隐患部门" : cbbKind.SelectedItem.Text);
        //Store1.DataSource = group;
        //Store1.DataBind();

        //if (group.Count() == 0)
        //{
        //    return "<chart />";
        //}

        //StringBuilder chartBuilder = new StringBuilder();
        //chartBuilder.Append("<chart caption='隐患分析' xAxisName='" + (cbbKind.SelectedItem.Text.Trim() == "" ? "隐患部门" : cbbKind.SelectedItem.Text) + "' yAxisName='数量'  showValues='0' palette='2' shownames='1' legendBorderAlpha='0' useRoundEdges='1' animation='1' decimalPrecision='0' formatNumberScale='0' baseFont='Arial' baseFontSize='12'>");

        //string categories = "<categories>";
        //string dataset1 = "<dataset seriesName='隐患数量' color='AFD8F8' showValues='0'>";
        //string dataset2 = "<dataset seriesName='已解决' color='8BBA00' showValues='0'>";
        //string dataset3 = "<dataset seriesName='未解决' color='8B0000' showValues='0'>";
        //foreach (var r in group)
        //{
        //    categories += "<category label='" + r.Key + "' />";
        //    dataset1 += "<set value='" + r.Total + "' />";
        //    dataset2 += "<set value='" + r.Pass + "' />";
        //    dataset3 += "<set value='" + r.NPass + "' />";
        //}
        //categories += "</categories>";
        //dataset1 += "</dataset>";
        //dataset2 += "</dataset>";
        //dataset3 += "</dataset>";
        //chartBuilder.Append(categories);
        //chartBuilder.Append(dataset1);
        //chartBuilder.Append(dataset2);
        //chartBuilder.Append(dataset3);
        //chartBuilder.Append("</chart>");
        //return chartBuilder.ToString();
    }
}
