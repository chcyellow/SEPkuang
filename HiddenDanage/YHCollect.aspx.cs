using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Coolite.Ext.Web;
using GhtnTech.SEP.DAL;
using GhtnTech.SecurityFramework.BLL;
using GhtnTech.SecurityFramework.DBUtility;
using GhtnTech.SEP.OraclDAL;

public partial class HiddenDanage_YHCollect : System.Web.UI.Page
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!Ext.IsAjaxRequest)
            {
                btn_ContentIn.Disabled = true;
                btn_ContentDel.Disabled = true;
                dfBegin.SelectedDate = System.DateTime.Today.AddDays(1 - System.DateTime.Today.Day);
                dfEnd.SelectedDate = System.DateTime.Today;
                dfBegin.MaxDate = System.DateTime.Today;
                dfEnd.MaxDate = System.DateTime.Today;
                storeload();
            }
        }
    }

    protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)
    {
        storeload();
    }

    [AjaxMethod]
    public void storeload()//执行查询
    {
        if (dfBegin.SelectedDate > dfEnd.SelectedDate)
        {
            Ext.Msg.Alert("提示", "请选择正确日期").Show();
            return;
        }
        //各基层单位查询本矿新增隐患信息
        var q = from h in dc.Yscollect
                from p in dc.Person
                from d in dc.Department
                where h.Personnumber == p.Personnumber && h.Maindept == d.Deptnumber && h.Status == "是"
                && h.Intime.Value.Date >= dfBegin.SelectedDate.Date && h.Intime.Value.Date <= dfEnd.SelectedDate.Date
                select new
                {
                    h.Cid,
                    YinHuanContent = h.Yhcontent,
                    p.Name,
                    h.Ysbz,
                    InTime = h.Intime,
                    DeptName = d.Deptname
                };
        Store1.DataSource = q;
        Store1.DataBind();
    }

    #region 单击每行-根据不同状态获取权限
    protected void RowClick(object sender, AjaxEventArgs e)//流程处理
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            btn_ContentIn.Disabled = false;
            btn_ContentDel.Disabled = false;
        }
        else
        {
            btn_ContentIn.Disabled = true;
            btn_ContentDel.Disabled = true;
        }
    }
    #endregion

    #region 进入危险源辨识页面
    [AjaxMethod]
    public void ContentIn()
    {
        //传参进入危险源辨识页面
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        string url = string.Format("~/HazardManage/Inputext.aspx?TempId={0}", sm.SelectedRow.RecordID);
        Response.Redirect(url);
    }
    #endregion

    #region 删除新增隐患/三违信息
    [AjaxMethod]
    public void ContentDel()
    {
        Ext.Msg.Confirm("提示", "是否删除该条新增信息?", new MessageBox.ButtonsConfig
        {
            Yes = new MessageBox.ButtonConfig
            {
                Handler = "Coolite.AjaxMethods.AgStatus();",
                Text = "确 定"
            },
            No = new MessageBox.ButtonConfig
            {
                Text = "取 消"
            }
        }).Show();
    }

    [AjaxMethod]
    public void AgStatus()
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        var yhN = dc.Yscollect.First(p => p.Cid == decimal.Parse(sm.SelectedRecordID.Trim()));
        dc.Yscollect.DeleteOnSubmit(yhN);
        dc.SubmitChanges();
        Ext.Msg.Alert("提示", "删除成功!").Show();
        storeload();
    }
    #endregion
}
