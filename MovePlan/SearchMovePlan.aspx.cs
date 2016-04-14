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
using GhtnTech.SEP.DAL;
using GhtnTech.SecurityFramework.BLL;
public partial class SearchMovePlan : BasePage
{
    //string moveID;//人员number
    //int userID1;
    //int roleID;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
           // PCPersonID();
            //PersonUserID();
            //PersonRoleID();
            //if (roleID != 8)
            //{
                //Label1.Visible = false;
                //ComboBox6.Visible = false;
                Button5.Visible = false;
                bindPlan();
            //}
            //else
            //{
            //    Label1.Visible = true;
            //    ComboBox6.Visible = true;
            //    Button5.Visible = true;
            
            //}
             //初始化模块权限
            //UserHandle.InitModule("SearchMovePlan");
            //是否有浏览权限
            //if (UserHandle.ValidationHandle(PermissionTag.Browse))
            //{
            //    //bindPlan();
            //    if (!UserHandle.ValidationHandle(PermissionTag.MoveBeginTime))
            //    {
            //        Button1.Visible = false;
            //    }
            //    if (!UserHandle.ValidationHandle(PermissionTag.MoveEndTime))
            //    {
            //        Button2.Visible = false;
            //    }
            //    if (!UserHandle.ValidationHandle(PermissionTag.View))
            //    {
            //        btn_fcfk.Visible = false;
            //    }
            //}
        }
        
    }
    DBSCMDataContext db = new DBSCMDataContext();
    
    protected void RowClick(object sender, AjaxEventArgs e)//流程处理
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        
        if (sm.SelectedRows.Count > 0)
        {
            //btn_fcfk.Disabled = false;
            var mp1 = db.Moveplan.Where(p => p.Id == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
            foreach (var d in mp1)
            {
                switch (d.Movestate.Trim())
                {
                    case "未走动":
                        btn_fcfk.Disabled = false;
                        break;
                    case "已走动": case "逾期未走动":
                        btn_fcfk.Disabled = true;
                        break;
                }
                hdnid.Value = d.Placeid;
                Hidden1.Value = d.Movestate;
            }
        }
        else
        {
            btn_fcfk.Disabled = true;
            hdnid.Value = "0";
        }
            
        bindYH();
    }
    protected void RowClick1(object sender, AjaxEventArgs e)//流程处理
    {
        string c = Hidden1.Value.ToString().Trim();
        //fcfk_fcyj.Value = "";
        //fcfk_fcqk.SelectedItem.Value = "";
    }
    //protected void Button1_Click(object sender, AjaxEventArgs e)//记录走动开始时间
    //{
    //    //PCPersonID();
    //    RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
    //    if (sm.SelectedRows.Count > 0)
    //    {
    //        updateStartTime(Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
    //        bindPlan();
    //    }
    //}
    //protected void Button2_Click(object sender, AjaxEventArgs e)//记录走动结束时间
    //{
    //    //PCPersonID();
    //    RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
    //    if (sm.SelectedRows.Count > 0)
    //    {
    //        updateEndTime(Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
    //        bindPlan();
    //    }
    //}
    protected void Button3_Click(object sender, AjaxEventArgs e)
    {
        Window1.Show();
    }
    protected void Button4_Click(object sender, AjaxEventArgs e)
    {
        bindPlan();
    }

    //绑定走动计划
    private void bindPlan()
    {
            var sqltext = from a in db.Moveplan
                          from b in db.Person
                          from c in db.Place
                          from d in db.Department
                          where a.Personid == b.Personnumber && a.Placeid == c.Placeid && b.Deptid == d.Deptnumber
                          && a.Personid == SessionBox.GetUserSession().PersonNumber 
&& (a.Starttime.Value <= System.DateTime.Today && System.DateTime.Today <= a.Endtime.Value)
                          select new
                          {
                             ID= a.Id,
                             EndTime= a.Endtime,
                             StartTime= a.Starttime,
                             PersonID= a.Personid,
                             MoveStartTime= a.Movestarttime,
                             MoveEndTime = a.Moveendtime,
                             Name= b.Name,
                             PlaceName= c.Placename,
                             DeptName= d.Deptname,
                             MoveState= a.Movestate
                          };
            Store1.DataSource = sqltext;
            Store1.DataBind();
            Button4.Disabled = sqltext.Count() > 0 ? false : true;
      
       
    }
    //绑定待复查隐患
    private void bindYH()
    {
        int b = Convert.ToInt32(hdnid.Value);
        var query = from a in db.Yhview
                    where a.Status == "隐患已整改" && a.Placeid == b
                    select
                        new
                        {
                           BanCi= a.Banci,
                           DeptName= a.Deptname,
                           INTime= a.Intime,
                           Name= a.Personname,
                           PCTime= a.Pctime,
                           PlaceName= a.Placename,
                           Remarks= a.Remarks,
                           YHContent= a.HBm,
                           YHLevel= a.Levelname,
                           YHType= a.Zyname,
                           YHPutinID= a.Yhputinid
                        };
        Store2.DataSource = query;
        Store2.DataBind();
        Button3.Disabled = query.Count() > 0 ? false : true;
       
    }
    
    ////更新走动开始时间
    //private void updateStartTime(int planID)
    //{
    //    //var sqltext = from a in db.MovePlan where  
    //    MovePlan mp = db.MovePlan.First(c => c.ID == planID);
    //    if (mp.MoveStartTime == null)
    //    {
    //        mp.MoveStartTime = DateTime.Parse(System.DateTime.Now.ToString("G"));
    //        mp.MoveState = "走动中";
    //    }
     
    //    db.SubmitChanges();
    //}
    ////更新走动结束时间
    //private void updateEndTime(int planID)
    //{
    //    //var sqltext = from a in db.MovePlan where  
    //    MovePlan mp = db.MovePlan.First(c => c.ID == planID);
    //    if (mp.MoveStartTime == null)
    //    {
    //        Ext.Msg.Alert("提示", "未记录走动开始时间!").Show();
    //    }

    //    else
    //    {
    //        mp.MoveEndTime = DateTime.Parse(System.DateTime.Now.ToString("G"));
    //        mp.MoveState = "已走动";
    //    }
    //    db.SubmitChanges();
    //}
    
    //走动人员id
    //private void PCPersonID()
    //{

    //    //string username = "";
    //    string PersonNumber = SessionBox.GetUserSession().PersonNumber;
    //    var renyuan = db.Person.Where(p => p.Personnumber == PersonNumber);
    //    foreach (var ren in renyuan)
    //    {//获取session中帐号信息
    //        //renid = Convert.ToInt32(ren.PersonID);
    //        moveID = ren.Personnumber;
    //        //username = ren.Name.Trim();
    //    }
    //}
    //private void PersonUserID()
    //{
    //    string PersonNumber = SessionBox.GetUserSession().PersonNumber;
    //    var user1 = db.SF_Users.Where(p => p.PersonNumber == PersonNumber);
    //    foreach (var us in user1)
    //    {
    //        userID1 = us.UserID;
    //    }
    //}
    //////人员角色id
    //private void PersonRoleID()
    //{
    //    var role = db.SF_UserRoles.Where(p => p.UserID == userID1);
    //    foreach (var ro in role)
    //    {
    //        roleID = ro.RoleID;
    //    }
    //}


    //导出报表
    protected void ToExcel(object sender, EventArgs e)
    {
        string json = GridData.Value.ToString();
        foreach (var r in GridPanel2.ColumnModel.Columns)
        {

            json = json.Replace("\"" + r.DataIndex.Trim() + "\"", "\"" + r.Header + "\"");
        }
        json = json.Replace("T00:00:00", "");

        string strFileName = "待复查隐患信息详情报表";
        Response.Clear();
        Response.Buffer = true;
        Response.Charset = "GB2312";

        StoreSubmitDataEventArgs eSubmit = new StoreSubmitDataEventArgs(json, null);
        XmlNode xml = eSubmit.Xml;

        this.Response.Clear();
        this.Response.ContentType = "application nd.ms-excel";
        Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(strFileName) + ".xls");
        XslCompiledTransform xtExcel = new XslCompiledTransform();
        xtExcel.Load(Server.MapPath("Excel.xsl"));
        xtExcel.Transform(xml, null, this.Response.OutputStream);
        this.Response.End();
    }
    //导出报表
    protected void ToExcelPL(object sender, EventArgs e)
    {
        string json = GridDataPL.Value.ToString();
        foreach (var r in GridPanel1.ColumnModel.Columns)
        {
            json = json.Replace("\"ID\"", "\"序号\"");
            json = json.Replace("\"" + r.DataIndex.Trim() + "\"", "\"" + r.Header + "\"");
        }
        json = json.Replace("T00:00:00", "");

        string strFileName = "干部走动管理信息报表";
        Response.Clear();
        Response.Buffer = true;
        Response.Charset = "GB2312";

        StoreSubmitDataEventArgs eSubmit = new StoreSubmitDataEventArgs(json, null);
        XmlNode xml = eSubmit.Xml;

        this.Response.Clear();
        this.Response.ContentType = "application nd.ms-excel";
        Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(strFileName) + ".xls");
        XslCompiledTransform xtExcel = new XslCompiledTransform();
        xtExcel.Load(Server.MapPath("Excel0.xsl"));
        xtExcel.Transform(xml, null, this.Response.OutputStream);
        this.Response.End();

    }
}
