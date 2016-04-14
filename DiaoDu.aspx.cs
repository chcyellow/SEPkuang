using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Xsl;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Coolite.Ext.Web;
using GhtnTech.SecurityFramework.IDAL;
using GhtnTech.SecurityFramework.BLL;
using System.Data.OracleClient;
using GhtnTech.SEP.DAL;

public partial class DiaoDu : System.Web.UI.Page
{
    DBSCMDataContext db = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            BindData();
            bindName_K(cbbDept.SelectedItem.Value);
            bindName_Z(cbbDept.SelectedItem.Value);
            bindPlan(cbbDept.SelectedItem.Value);
        }
    }

    protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)
    {
        bindPlan(cbbDept.SelectedItem.Value);
    }

    private void BindData()//基础信息绑定
    {
        if (SessionBox.GetUserSession().rolelevel.Trim().IndexOf("1") > -1)
        {
            Store5.DataSource = PublicCode.GetMaindept("");
            Store5.DataBind();
            cbbDept.SelectedItem.Value = "241700000";
            cbbDept.Disabled = false;
        }
        else
        {
            var dept = from d in db.Department
                       where d.Deptnumber == SessionBox.GetUserSession().DeptNumber
                       select new
                       {
                           d.Deptname,
                           Deptid = d.Deptnumber
                       };
            Store5.DataSource = dept;
            Store5.DataBind();
            cbbDept.SelectedItem.Value = SessionBox.GetUserSession().DeptNumber;
            cbbDept.Disabled = true;
        }
    }

    protected void Button3_Click(object sender, AjaxEventArgs e)
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;

        if (sm.SelectedRows.Count > 0)
        {
            Window1.Show();
            detailPlan(sm.SelectedRows[0].RecordID.Trim());
        }
       
    }
    protected void RowClick(object sender, AjaxEventArgs e)//流程处理
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;

        if (sm.SelectedRows.Count > 0)
        {
            btn_fcfk.Disabled = false;
        }
        else
        {
            btn_fcfk.Disabled = true;
        }
    }

    //绑定走动人员
    private void bindPlan(string maindept)
    {
        //var sqltext = from a in db.Moveplan
        //              from b in db.Person
        //              from c in db.Position
        //              from d in db.Department
        //              where a.Personid == b.Personnumber && b.Posid == c.Posid && b.Deptid == d.Deptnumber && a.Starttime.Value.Month == System.DateTime.Now.Month && System.DateTime.Now.Month == a.Endtime.Value.Month
        //              && b.Maindeptid == maindept
        //              select new
        //              {
        //                  PersonID = a.Personid,
        //                  b.Name,
        //                  PosName = c.Posname,
        //                  DeptName = d.Deptname
        //              };
        var sqltext = GetSafeInfo.GetDiaoDu(maindept).Tables[0];
        Store1.DataSource = sqltext;
        Store1.DataBind();
    }

    //绑定详细计划
    private void detailPlan(string PersonID)
    {
        //var sqltext = from a in db.Moveplan
        //              from b in db.Person
        //              from c in db.Place
        //              from d in db.Department
        //              where a.Personid == b.Personnumber && a.Placeid == c.Placeid && b.Deptid == d.Deptnumber && a.Personid == PersonID && a.Starttime.Value.Month == System.DateTime.Now.Month && System.DateTime.Now.Month == a.Endtime.Value.Month
        //              && b.Maindeptid == SessionBox.GetUserSession().DeptNumber
        //              select new
        //              {
        //                  ID = a.Id,
        //                  EndTime = a.Endtime,
        //                  StartTime = a.Starttime,
        //                  PersonID = a.Personid,
        //                  b.Name,
        //                  PlaceName = c.Placename,
        //                  DeptName = d.Deptname,
        //                  MoveState = a.Movestate
        //              };
        var sqltext = GetSafeInfo.GetDiaoDuInfo(PersonID).Tables[0];
        this.Store2.DataSource = sqltext;
        this.Store2.DataBind();
    }

    [AjaxMethod]
    public void DetailLoad(string PersonID)
    {
        detailPlan(PersonID);
        Window1.Show();
    }
    [AjaxMethod]
    public void DetailLoad1(string PersonID)
    {
        detailPlan(PersonID);
        Window1.Show();
    }

    //条件查询
    [AjaxMethod]
    public void LoadData()
    {
        bindName_K(cbbDept.SelectedItem.Value);
        bindName_Z(cbbDept.SelectedItem.Value);
        bindPlan(cbbDept.SelectedItem.Value);
    }

    //绑定矿领导
    private void bindName_K(string maindept)
    {
        var query = (from a in db.Person
                     from b in db.Position
                     where a.Posid==b.Posid && a.Maindeptid == maindept && b.Movegblevel=="矿领导"
                     orderby b.Posname.StartsWith("矿") descending, b.Posname.StartsWith("党") descending, a.Personnumber ascending
                     select new
                     {
                         a.Name,
                         PersonID = a.Personnumber
                     });
        this.Store3.DataSource = query;
        this.Store3.DataBind();
    }
    //绑定中层领导姓名
    private void bindName_Z(string maindept)
    {
        //string text = "<ul>";
        var query = (from a in db.Person
                     from b in db.Position
                     from c in db.Moveplan
                     where a.Posid == b.Posid && a.Personnumber == c.Personid && b.Movegblevel.Trim() == "中层领导"
                     && b.Maindeptid == maindept
                     select new
                     {
                         a.Name,
                         PersonID = a.Personnumber
                     }).Distinct();
        Store4.DataSource = query;
        Store4.DataBind();
    }
}
