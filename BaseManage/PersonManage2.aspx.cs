using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using System.Data;
using GhtnTech.SEP.DAL;
using GhtnTech.SecurityFramework;
using GhtnTech.SecurityFramework.Model;
using GhtnTech.SecurityFramework.BLL;
using GhtnTech.SecurityFramework.Utility;
public partial class BaseManage_PersonManage2 : BasePage
{
    DBSCMDataContext db = new DBSCMDataContext();
    PersonBll p = new PersonBll();
    DepartmentBll dept = new DepartmentBll();
    Role Rolebll = new Role();
    decimal rolelevel = 0;
    string maindeptid = "";
    int _pageSize = 10;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!SessionBox.CheckUserSession())
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                SF_Role r = Rolebll.GetRoleModel(decimal.Parse(SessionBox.GetUserSession().CurrentRole[0].ToString().Split(',')[0]));
                rolelevel = (int)r.LevelID;
                Session["rolelevel"] = r.LevelID;
                maindeptid = SessionBox.GetUserSession().DeptNumber;
                switch ((int)rolelevel)
                {
                    case 0:
                        BindGridView(0, "PERSON", _pageSize, "", "", "");
                        AspNetPager1.RecordCount = p.PersonCount();
                        BindDll(ddlDept, "ID", @"24\d\d0{5}", "NAME", "ID");
                        break;
                    case 1:
                        BindGridView(0, "PERSON", _pageSize, "", "", "");
                        AspNetPager1.RecordCount = p.PersonCount();
                        BindDll(ddlDept, "ID", @"24\d\d0{5}", "NAME", "ID");
                        break;
                    case 2:
                        BindGridView(0, "PERSON", _pageSize, string.Format("maindeptid='{0}'", SessionBox.GetUserSession().DeptNumber), "", "");
                        AspNetPager1.RecordCount = p.PersonCount();
                        BindDll(ddlDept, "ID", @"24\d\d0{5}", "NAME", "ID");
                        ddlDept.SelectedValue = SessionBox.GetUserSession().DeptNumber;
                        BindDll(ddlKQ, "ID", ddlDept.SelectedItem.Value.Remove(4), "NAME", "ID");
                        ddlKQ.Items.Insert(0, new ListItem("--全部--", "-1"));
                        ddlDept.Enabled = false;
                        break;
                    default:
                        BindGridView(0, "PERSON", _pageSize, string.Format("maindeptid='{0}'", SessionBox.GetUserSession().DeptNumber), "", "");
                        AspNetPager1.RecordCount = p.PersonCount();
                        BindDll(ddlDept, "ID", @"24\d\d0{5}", "NAME", "ID");
                        ddlDept.SelectedValue = SessionBox.GetUserSession().DeptNumber;
                        BindDll(ddlKQ, "ID", ddlDept.SelectedItem.Value.Remove(4), "NAME", "ID");
                        ddlKQ.Items.Insert(0, new ListItem("--全部--", "-1"));
                        ddlDept.Enabled = false;
                        break;
                }
                
                //初始化模块权限
                UserHandle.InitModule(this.PageTag);
                //是否有浏览权限
                if (UserHandle.ValidationHandle(PermissionTag.Browse))
                {
                    GridViewCommandColumn colEdit = (GridViewCommandColumn)gvPerson.Columns["操作"];
                    if (!UserHandle.ValidationHandle(PermissionTag.Edit))
                    {
                        colEdit.EditButton.Visible = false;
                    }
                }
            }
        }
        //adsPosition.Where = "Maindeptid == \"" + SessionBox.GetUserSession().DeptNumber + "\"";
        //adsDept.Where = "Deptnumber.StartsWith(\"" + SessionBox.GetUserSession().DeptNumber.Remove(4) + "\")";
    
    }
    private void BindGridView(int index, string table, int pageSize, string where, string column, string order)
    {
        DataTable dt = p.GetList(index, table, pageSize, where, column, order).Tables[0];
        int dtcount = dt.Rows.Count;
        gvPerson.DataSource = dt;
        gvPerson.DataBind();
        AspNetPager1.RecordCount = p.PersonCount();
        AspNetPager1.PageSize = pageSize;
        AspNetPager1.CustomInfoHTML = "记录总数：<font color=\"blue\"><b>" + AspNetPager1.RecordCount.ToString() + "</b></font>";
        AspNetPager1.CustomInfoHTML += " 总页数：<font color=\"blue\"><b>" + AspNetPager1.PageCount.ToString() + "</b></font>";
        AspNetPager1.CustomInfoHTML += " 当前页：<font color=\"red\"><b>" + AspNetPager1.CurrentPageIndex.ToString() + "</b></font>";
    }

    protected void AspNetPager1_PageChanged(object sender, EventArgs e)
    {
        switch (int.Parse(Session["rolelevel"].ToString()))
        {
            case 0:
                BindGridView(AspNetPager1.CurrentPageIndex - 1, "PERSON", _pageSize, "", "", "");
                
                break;
            case 1:
                BindGridView(AspNetPager1.CurrentPageIndex - 1, "PERSON", _pageSize, "", "", "");
                 
                break;
            case 2:
                BindGridView(AspNetPager1.CurrentPageIndex - 1, "PERSON", _pageSize, string.Format("maindeptid='{0}'", SessionBox.GetUserSession().DeptNumber), "", "");
                
                break;
            default:
                BindGridView(AspNetPager1.CurrentPageIndex - 1, "PERSON", _pageSize, string.Format("maindeptid='{0}'", SessionBox.GetUserSession().DeptNumber), "", "");
                break;
        }

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        //string strWhere = "1=1";
        if (txtPsnNo.Text.Trim() != "")
        {

        }
        if (txtName.Text.Trim() != "")
        {

        }
        if (txtPhone.Text.Trim() != "")
        {

        }
        if (txtLightNo.Text.Trim() != "")
        {

        }
        if (ddlDept.SelectedValue != "-1")
        {
        }
        if (ddlKQ.SelectedValue != "-1")
        {
        }
        if (ddlPos.SelectedValue != "")
        {
        }
    }

    protected void BindDll(DropDownList ddl, string column, string regex, string txtField, string valField)
    {
        ddl.DataSource = dept.GetTreeList(column, regex);
        ddl.DataTextField = txtField;
        ddl.DataValueField = valField;
        ddl.DataBind();
    }
    protected void ddlDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDept.SelectedValue != "-1")
        {
            BindDll(ddlKQ, "ID", ddlDept.SelectedItem.Value.Remove(4), "NAME", "ID");
            ddlKQ.Items.Insert(0, new ListItem("--全部--", "-1"));
        }
        else
        {
            ddlKQ.Items.Clear();
            ddlKQ.Items.Insert(0, new ListItem("--全部--", "-1"));
        }
    }
    //添加-隐患字母编码
    //protected void LinqDataSource1_Inserting(object sender, LinqDataSourceInsertEventArgs e)
    //{
    //    Person person = (Person)e.NewObject;
    //    person.pinyin = db.f_GetPy(person.Name);
    //}
    //修改
    //protected void LinqDataSource1_Updating(object sender, LinqDataSourceUpdateEventArgs e)
    //{
    //    Person person = (Person)e.NewObject;
    //    person.pinyin = db.f_GetPy(person.Name);
    //}
    //删除

    //protected void GridView_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
    //{
    //    if (!GridView.IsNewRowEditing)
    //    {
    //        ((GridViewDataColumn)GridView.Columns["PersonNumber"]).ReadOnly = true;
    //    }
    //}
    protected void gvPerson_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
    {

    }
}
