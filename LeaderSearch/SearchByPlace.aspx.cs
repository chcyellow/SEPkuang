using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GhtnTech.SEP.DAL;
using GhtnTech.SecurityFramework.BLL;
using Coolite.Ext.Web;

public partial class LeaderSearch_YHSearch : BasePage
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_PreRenderComplete(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cbbUnit.Items.Insert(0, new Coolite.Ext.Web.ListItem("--全部--", "-1"));
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request["head"]))
            {
                GridPanel1.TopBar[0].Visible = false;
                if (Request["head"].Trim() == "0")
                {
                    GridPanel1.Header = false;
                    GridPanel1.Height = 160;
                    Window1.Listeners.BeforeShow.Fn = "function(el) { el.setHeight(Ext.getBody().getViewSize().height);el.setWidth(Ext.getBody().getViewSize().width-20); }";
                }
                if (Request["head"].Trim() == "1")
                {
                    GridPanel1.Height = 300;
                    Window1.Listeners.BeforeShow.Fn = "function(el) { el.setHeight(Ext.getBody().getViewSize().height-20);el.setWidth(Ext.getBody().getViewSize().width-20); }";
                }
            }
            dfBegin.SelectedDate = System.DateTime.Today.AddDays(1 - System.DateTime.Today.Day);
            dfEnd.SelectedDate = System.DateTime.Today;
            dfBegin.MaxDate = System.DateTime.Today;
            dfEnd.MaxDate = System.DateTime.Today;
            #region 初始化单位
            DBSCMDataContext dc = new DBSCMDataContext();
            if (SessionBox.GetUserSession().rolelevel.Contains("1") || SessionBox.GetUserSession().rolelevel.Contains("0"))
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
                cbbUnit.SelectedItem.Value = "-1";
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
            storebind();

        }
    }
    protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)
    {
        storebind();
    }

    [AjaxMethod]
    public void storebind()
    {
        if (dfBegin.SelectedDate > dfEnd.SelectedDate)
        {
            Ext.Msg.Alert("提示", "请选择正确日期").Show();
            return;
        }
        //var data = dc.GetAllSafetyCountByPAreas(dfBegin.SelectedDate, dfEnd.SelectedDate);
        
        if (SessionBox.GetUserSession().rolelevel.Contains("1") || SessionBox.GetUserSession().rolelevel.Contains("0"))
        {
            if (cbbUnit.SelectedIndex > -1 && cbbUnit.SelectedItem.Value != "-1")
            {
                var data = GetSafeInfo.GetAllSafetyCountByPAreas(dfBegin.SelectedDate, dfEnd.SelectedDate, cbbUnit.SelectedItem.Value);
                Store1.DataSource = data;
                Store1.DataBind();
            }
            else
            {
                var data = GetSafeInfo.GetAllSafetyCountByPAreas(dfBegin.SelectedDate, dfEnd.SelectedDate, "");
                Store1.DataSource = data;
                Store1.DataBind();
            }
        }
        else
        {
            var data = GetSafeInfo.GetAllSafetyCountByPAreas(dfBegin.SelectedDate, dfEnd.SelectedDate, SessionBox.GetUserSession().DeptNumber);
            Store1.DataSource = data;
            Store1.DataBind();
        }
    }

    protected void Cell_Click(object sender, AjaxEventArgs e)
    {
        CellSelectionModel sm = this.GridPanel1.SelectionModel.Primary as CellSelectionModel;
        if (sm.SelectedCell.ColIndex == 0 ||sm.SelectedCell.ColIndex == 1|| sm.SelectedCell.Value.Trim() == "0")
            return;
        Window1.Title = dc.Placeareas.First(p => p.Pareasid == int.Parse(sm.SelectedCell.RecordID)).Pareasname.Trim() + "---";
        string url = "";
        switch (sm.SelectedCell.Name.Trim())
        {
            case "YHALL":
                Window1.Title += "所有隐患";
                url = string.Format("PlaceSummary.aspx?PAreasID={0}&begin={1}&end={2}&url=YHcondition.aspx", sm.SelectedCell.RecordID.Trim(), dfBegin.SelectedDate.ToString("yyyy-MM-dd"), dfEnd.SelectedDate.ToString("yyyy-MM-dd"));
                break;
            case "YHYZG":
                Window1.Title += "已闭合隐患";
                url = string.Format("PlaceSummary.aspx?PAreasID={0}&begin={1}&end={2}&status={3}&url=YHcondition.aspx", sm.SelectedCell.RecordID.Trim(), dfBegin.SelectedDate.ToString("yyyy-MM-dd"), dfEnd.SelectedDate.ToString("yyyy-MM-dd"), "1");
                break;
            case "YHWZG":
                Window1.Title += "未闭合隐患";
                url = string.Format("PlaceSummary.aspx?PAreasID={0}&begin={1}&end={2}&status={3}&url=YHcondition.aspx", sm.SelectedCell.RecordID.Trim(), dfBegin.SelectedDate.ToString("yyyy-MM-dd"), dfEnd.SelectedDate.ToString("yyyy-MM-dd"), "0");
                break;
            case "SWALL":
                Window1.Title += "‘三违’信息";
                url = string.Format("PlaceSummary.aspx?PAreasID={0}&begin={1}&end={2}&url=SWcondition.aspx", sm.SelectedCell.RecordID.Trim(), dfBegin.SelectedDate.ToString("yyyy-MM-dd"), dfEnd.SelectedDate.ToString("yyyy-MM-dd"));
                break;
            case "YZD":
                Window1.Title += "已走动信息";
                url = string.Format("PlaceSummary.aspx?PAreasID={0}&begin={1}&end={2}&status={3}&url=MPcondition.aspx", sm.SelectedCell.RecordID.Trim(), dfBegin.SelectedDate.ToString("yyyy-MM-dd"), dfEnd.SelectedDate.ToString("yyyy-MM-dd"), "1");
                break;
            case "WZD":
                Window1.Title += "未走动信息";
                url = string.Format("PlaceSummary.aspx?PAreasID={0}&begin={1}&end={2}&status={3}&url=MPcondition.aspx", sm.SelectedCell.RecordID.Trim(), dfBegin.SelectedDate.ToString("yyyy-MM-dd"), dfEnd.SelectedDate.ToString("yyyy-MM-dd"), "0");
                break;
        }
        //url=Server.HtmlEncode(url);
        Ext.DoScript("#{Window1}.load('" + url + "');");
        Window1.Show();
    }
}
