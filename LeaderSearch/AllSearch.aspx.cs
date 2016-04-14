using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GhtnTech.SEP.DAL;
using GhtnTech.SecurityFramework.BLL;
using Coolite.Ext.Web;
using System.Data;

public partial class LeaderSearch_AllSearch : System.Web.UI.Page
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
                }
                if (Request["head"].Trim() == "1")
                {
                    GridPanel1.Height = 300;
                }
                Window1.Listeners.BeforeShow.Fn = "function(el) { el.setHeight(Ext.getBody().getViewSize().height-20);el.setWidth(Ext.getBody().getViewSize().width-20); }";
            }
            dfBegin.SelectedDate = System.DateTime.Today.AddDays(1 - System.DateTime.Today.Day);
            dfEnd.SelectedDate = System.DateTime.Today;
            //dfBegin.MaxDate = System.DateTime.Today;
            //dfEnd.MaxDate = System.DateTime.Today;
            #region 初始化单位
            DBSCMDataContext dc = new DBSCMDataContext();
            if (SessionBox.GetUserSession().rolelevel.Contains("1") || SessionBox.GetUserSession().rolelevel.Contains("0"))
            {
                //var dept = from d in dc.Department
                //           where d.Deptnumber.Substring(4) == "00000" && d.Deptname.EndsWith("矿")
                //           select new
                //           {
                //               d.Deptname,
                //               Deptid = d.Deptnumber
                //           };

                UnitStore.DataSource = PublicCode.GetMaindept("");
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
            //storebind();//第一次不加载

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
        DataSet ds = new DataSet();
        //var data = ds;
        if (SessionBox.GetUserSession().rolelevel.Contains("1") || SessionBox.GetUserSession().rolelevel.Contains("0"))
        {
            if (cbbUnit.SelectedIndex > -1 && cbbUnit.SelectedItem.Value != "-1")
            {
                var data = GetSafeInfo.GetAllSafetyCountByUnit(cbbUnit.SelectedItem.Value, dfBegin.SelectedDate, dfEnd.SelectedDate);
                Store1.DataSource = data;
                Store1.DataBind();
            }
            else
            {
                var data = GetSafeInfo.GetAllSafetyCountByUnit("", dfBegin.SelectedDate, dfEnd.SelectedDate);
                Store1.DataSource = data;
                Store1.DataBind();
            }
        }
        else
        {
            var data = GetSafeInfo.GetAllSafetyCountByUnit(SessionBox.GetUserSession().DeptNumber, dfBegin.SelectedDate, dfEnd.SelectedDate);
            Store1.DataSource = data;
            Store1.DataBind();
        }

    }

    protected void Cell_Click(object sender, AjaxEventArgs e)
    {
        CellSelectionModel sm = this.GridPanel1.SelectionModel.Primary as CellSelectionModel;
        if (sm.SelectedCell.ColIndex == 0 || sm.SelectedCell.Value.Trim() == "0")
            return;
        Window1.Title = dc.Department.First(p => p.Deptnumber == sm.SelectedCell.RecordID).Deptname.Trim() + "---";
        string url = "";
        switch (sm.SelectedCell.Name.Trim())
        {
            case "YHALL":
                Window1.Width = 840;
                Window1.Height = 422;
                Window1.Title += "所有隐患";
                url = string.Format("YHcondition.aspx?MainDeptID={0}&begin={1}&end={2}", sm.SelectedCell.RecordID.Trim(), dfBegin.SelectedDate.ToString("yyyy-MM-dd"), dfEnd.SelectedDate.ToString("yyyy-MM-dd"));
                break;
            case "YHYZG":
                Window1.Width = 840;
                Window1.Height = 422;
                Window1.Title += "已闭合隐患";
                url = string.Format("YHcondition.aspx?MainDeptID={0}&begin={1}&end={2}&status={3}", sm.SelectedCell.RecordID.Trim(), dfBegin.SelectedDate.ToString("yyyy-MM-dd"), dfEnd.SelectedDate.ToString("yyyy-MM-dd"), "1");
                break;
            case "YHWZG":
                Window1.Width = 840;
                Window1.Height = 422;
                Window1.Title += "未闭合隐患";
                url = string.Format("YHcondition.aspx?MainDeptID={0}&begin={1}&end={2}&status={3}", sm.SelectedCell.RecordID.Trim(), dfBegin.SelectedDate.ToString("yyyy-MM-dd"), dfEnd.SelectedDate.ToString("yyyy-MM-dd"), "0");
                break;
            case "A":
                Window1.Width = 840;
                Window1.Height = 422;
                Window1.Title += "A级隐患";
                url = string.Format("YHcondition.aspx?MainDeptID={0}&begin={1}&end={2}&YHLevel={3}", sm.SelectedCell.RecordID.Trim(), dfBegin.SelectedDate.ToString("yyyy-MM-dd"), dfEnd.SelectedDate.ToString("yyyy-MM-dd"), "A");
                break;
            case "B":
                Window1.Width = 840;
                Window1.Height = 422;
                Window1.Title += "B级隐患";
                url = string.Format("YHcondition.aspx?MainDeptID={0}&begin={1}&end={2}&YHLevel={3}", sm.SelectedCell.RecordID.Trim(), dfBegin.SelectedDate.ToString("yyyy-MM-dd"), dfEnd.SelectedDate.ToString("yyyy-MM-dd"), "B");
                break;
            case "C":
                Window1.Width = 840;
                Window1.Height = 422;
                Window1.Title += "C级隐患";
                url = string.Format("YHcondition.aspx?MainDeptID={0}&begin={1}&end={2}&YHLevel={3}", sm.SelectedCell.RecordID.Trim(), dfBegin.SelectedDate.ToString("yyyy-MM-dd"), dfEnd.SelectedDate.ToString("yyyy-MM-dd"), "C");
                break;
            case "D":
                Window1.Width = 840;
                Window1.Height = 422;
                Window1.Title += "D级隐患";
                url = string.Format("YHcondition.aspx?MainDeptID={0}&begin={1}&end={2}&YHLevel={3}", sm.SelectedCell.RecordID.Trim(), dfBegin.SelectedDate.ToString("yyyy-MM-dd"), dfEnd.SelectedDate.ToString("yyyy-MM-dd"), "D");
                break;
            case "SWALL":
                Window1.Width = 890;
                Window1.Height = 400;
                Window1.Title += "三违信息";
                url = string.Format("SWcondition.aspx?MainDeptID={0}&begin={1}&end={2}", sm.SelectedCell.RecordID.Trim(), dfBegin.SelectedDate.ToString("yyyy-MM-dd"), dfEnd.SelectedDate.ToString("yyyy-MM-dd"));
                break;
            case "YZ":
                Window1.Width = 890;
                Window1.Height = 400;
                Window1.Title += "严重三违信息";
                url = string.Format("SWcondition.aspx?MainDeptID={0}&begin={1}&end={2}&SWLevel={3}", sm.SelectedCell.RecordID.Trim(), dfBegin.SelectedDate.ToString("yyyy-MM-dd"), dfEnd.SelectedDate.ToString("yyyy-MM-dd"), "严重");
                break;
            case "YB":
                Window1.Width = 890;
                Window1.Height = 400;
                Window1.Title += "一般三违信息";
                url = string.Format("SWcondition.aspx?MainDeptID={0}&begin={1}&end={2}&SWLevel={3}", sm.SelectedCell.RecordID.Trim(), dfBegin.SelectedDate.ToString("yyyy-MM-dd"), dfEnd.SelectedDate.ToString("yyyy-MM-dd"), "一般");
                break;
            case "QW":
                Window1.Width = 890;
                Window1.Height = 400;
                Window1.Title += "轻微三违信息";
                url = string.Format("SWcondition.aspx?MainDeptID={0}&begin={1}&end={2}&SWLevel={3}", sm.SelectedCell.RecordID.Trim(), dfBegin.SelectedDate.ToString("yyyy-MM-dd"), dfEnd.SelectedDate.ToString("yyyy-MM-dd"), "轻微");
                break;
            case "YZD":
                Window1.Width = 840;
                Window1.Height = 422;
                Window1.Title += "已走动信息";
                url = string.Format("MPcondition.aspx?MainDeptID={0}&begin={1}&end={2}&status={3}", sm.SelectedCell.RecordID.Trim(), dfBegin.SelectedDate.ToString("yyyy-MM-dd"), dfEnd.SelectedDate.ToString("yyyy-MM-dd"), "1");
                break;
            case "WZD":
                Window1.Width = 840;
                Window1.Height = 422;
                Window1.Title += "未走动信息";
                url = string.Format("MPcondition.aspx?MainDeptID={0}&begin={1}&end={2}&status={3}", sm.SelectedCell.RecordID.Trim(), dfBegin.SelectedDate.ToString("yyyy-MM-dd"), dfEnd.SelectedDate.ToString("yyyy-MM-dd"), "0");
                break;
            //--------------------
            case "FXCZG":
                 Window1.Width = 840;
                Window1.Height = 422;
                Window1.Title += "非现场整改隐患";
                url = string.Format("YHcondition.aspx?MainDeptID={0}&begin={1}&end={2}&status={3}", sm.SelectedCell.RecordID.Trim(), dfBegin.SelectedDate.ToString("yyyy-MM-dd"), dfEnd.SelectedDate.ToString("yyyy-MM-dd"), "2");
                break;
            case "XCZG":
                 Window1.Width = 840;
                Window1.Height = 422;
                Window1.Title += "现场整改隐患";
                url = string.Format("YHcondition.aspx?MainDeptID={0}&begin={1}&end={2}&status={3}", sm.SelectedCell.RecordID.Trim(), dfBegin.SelectedDate.ToString("yyyy-MM-dd"), dfEnd.SelectedDate.ToString("yyyy-MM-dd"), "3");
                break;
            case "KC":
                 Window1.Width = 840;
                Window1.Height = 422;
                Window1.Title += "矿查隐患";
                url = string.Format("YHcondition.aspx?MainDeptID={0}&begin={1}&end={2}&status={3}", sm.SelectedCell.RecordID.Trim(), dfBegin.SelectedDate.ToString("yyyy-MM-dd"), dfEnd.SelectedDate.ToString("yyyy-MM-dd"), "4");
                break;
            case "ZC":
                 Window1.Width = 840;
                Window1.Height = 422;
                Window1.Title += "自查隐患";
                url = string.Format("YHcondition.aspx?MainDeptID={0}&begin={1}&end={2}&status={3}", sm.SelectedCell.RecordID.Trim(), dfBegin.SelectedDate.ToString("yyyy-MM-dd"), dfEnd.SelectedDate.ToString("yyyy-MM-dd"), "5");
                break;
            case "SJPC":
                Window1.Width = 840;
                Window1.Height = 422;
                Window1.Title += "上级排查隐患";
                url = string.Format("YHcondition.aspx?MainDeptID={0}&begin={1}&end={2}&status={3}", sm.SelectedCell.RecordID.Trim(), dfBegin.SelectedDate.ToString("yyyy-MM-dd"), dfEnd.SelectedDate.ToString("yyyy-MM-dd"), "6");
                break;
            case "TBYZ":
                Window1.Width = 890;
                Window1.Height = 400;
                Window1.Title += "特别严重三违信息";
                url = string.Format("SWcondition.aspx?MainDeptID={0}&begin={1}&end={2}&SWLevel={3}", sm.SelectedCell.RecordID.Trim(), dfBegin.SelectedDate.ToString("yyyy-MM-dd"), dfEnd.SelectedDate.ToString("yyyy-MM-dd"), "特别严重");
                break;
            case "BGFXW":
                Window1.Width = 890;
                Window1.Height = 400;
                Window1.Title += "不规范行为三违信息";
                url = string.Format("SWcondition.aspx?MainDeptID={0}&begin={1}&end={2}&SWLevel={3}", sm.SelectedCell.RecordID.Trim(), dfBegin.SelectedDate.ToString("yyyy-MM-dd"), dfEnd.SelectedDate.ToString("yyyy-MM-dd"), "不规范行为");
                break;
        }
        //url=Server.HtmlEncode(url);
        Ext.DoScript("#{Window1}.load('" + url + "');");
        Window1.Show();
    }

}
