using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GhtnTech.SecurityFramework;
using GhtnTech.SecurityFramework.BLL;
using GhtnTech.SecurityFramework.Model;
using GhtnTech.SecurityFramework.Utility;
using GhtnTech.SEP.DAL;
public partial class SystemManage_AddUser : BasePage
{
    Role Rolebll = new Role();
    int rolelevel = 0;
    string roledeptid = "";
    PersonBll p = new PersonBll();

    DepartmentBll dept = new DepartmentBll();
    DBSCMDataContext dc = new DBSCMDataContext();
    static readonly int _pageSize = 10;
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
                roledeptid = SessionBox.GetUserSession().DeptNumber;
                if (rolelevel > 1)
                {
                    Session["WhereRole"] = string.Format("MAINDEPTID='{0}' or (maindeptid='{1}' and levelid>={2})", SessionBox.GetUserSession().DeptNumber,"000000000",rolelevel.ToString());
                    Session["WhereUserGroup"] = "usergroupid not in(2,3,23)";
                    Session["maindeptid"] = SessionBox.GetUserSession().DeptNumber;
                    Session["deptid"] = SessionBox.GetUserSession().DeptNumber.Remove(4);
                }
                InitData();
            }
        }
    }
    protected void btnAddPerson_Click(object sender, EventArgs e)
    {
        User usersCrud = new User();
        SF_User user = new SF_User();
        if (txtPwd.Text != "" && ddlUserGroup.SelectedValue != "-1")
        {
            int usercount = 0;
            string userName = "";
            user.PassWord = SecurityEncryption.MD5(txtPwd.Text.Trim(), 32);
            int roleID = 0;
            roleID = Convert.ToInt32(ddlRole.SelectedValue);
            user.UserGroupID = Convert.ToInt32(ddlUserGroup.SelectedValue);
            user.UserStatus = ddlState.SelectedValue;
            for (int i = 0; i < xlstSelected.Items.Count; i++)
            {
                user.UserName = xlstSelected.Items[i].Value.ToString();
                user.PersonNumber = xlstSelected.Items[i].Value.ToString();
                if (!usersCrud.UserExists(user.UserName))
                {
                    int n = (int)usersCrud.CreateUser(user);
                    if (n >= 1)
                    {
                        if (roleID != 0)
                        {
                            usersCrud.AddUserRole(n, roleID);
                        }
                    }
                    usercount++;
                }
                else
                {
                    userName += user.UserName + ",";
                }
            }
            JSHelper.Alert(string.Format("共添加{0}个用户！", usercount), this);
        }
        else
        {
            JSHelper.Alert("初始化信息请填写完整！", this);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        JSHelper.RefreshParent("UserManage.aspx", this);
    }
    private void BindGridView(int index, string table, int pageSize, string where, string column, string order)
    {
        DataTable dt = p.GetList(index, table, pageSize, where, column, order).Tables[0];
        int dtcount = dt.Rows.Count;
        xgvPerson.DataSource = dt;
        xgvPerson.DataBind();
        AspNetPager1.RecordCount = p.PersonCount();
        AspNetPager1.PageSize = pageSize;
        AspNetPager1.CustomInfoHTML = "记录总数：<font color=\"blue\"><b>" + AspNetPager1.RecordCount.ToString() + "</b></font>";
        AspNetPager1.CustomInfoHTML += " 总页数：<font color=\"blue\"><b>" + AspNetPager1.PageCount.ToString() + "</b></font>";
        AspNetPager1.CustomInfoHTML += " 当前页：<font color=\"red\"><b>" + AspNetPager1.CurrentPageIndex.ToString() + "</b></font>";
    }
    private void GetPersonData()
    {
        int index = 0;
        string table = "PERSON";
        int pageSize = _pageSize;
        string where = "1=1";
        string column = "PERSONID";
        string order = "ASC";
        if (AspNetPager1.CurrentPageIndex > 0)
        {
            index = AspNetPager1.CurrentPageIndex - 1;
        }
        if (txtPsnNo.Text.Trim() != "")
        {
            where += string.Format(" and PERSONNUMBER like'%{0}%'", txtPsnNo.Text.Trim());
        }
        if (txtLightNo.Text.Trim() != "")
        {
            where += string.Format(" and LIGHTNUMBER like'%{0}%'", txtLightNo.Text.Trim());
        }
        if (txtName.Text.Trim() != "")
        {
            where += string.Format(" and NAME like'%{0}%'", txtName.Text.Trim());
        }
        //if (txtPhone.Text.Trim() != "")
        //{
        //    where += string.Format(" and TEL like'%{0}%'", txtPhone.Text.Trim());
        //}
        if (ddlDept.SelectedValue != "-1")
        {
            where += string.Format(" and MAINDEPTID='{0}'", ddlDept.SelectedValue.Trim());
        }
        if (ddlKQ.SelectedValue != "-1")
        {
            where += string.Format(" and DEPTID='{0}'", ddlKQ.SelectedValue.Trim());
        }
        if (ddlSex.SelectedValue != "-1")
        {
            where += string.Format(" and SEX='{0}'", ddlSex.SelectedValue.Trim());
        }
        BindGridView(index, table, pageSize, where, column, order);
        AspNetPager1.RecordCount = p.PersonCount();
    }

    protected void InitData()
    {
        GetDeptData();
        GetKQData();
        GetPersonData();
    }
    
    private void GetDeptData()
    {
        //BindDll(ddlDept, "ID", @"24\d\d0{5}", "NAME", "ID");
        //BindDll(ddlDept, "deptnumber like'%00000' and deptname like'%矿'", "DEPTNAME", "DEPTNUMBER");
        BindDll(ddlDept, "(substr(deptnumber,1,2)='23' or substr(deptnumber,1,2)='24' or substr(deptnumber,1,2)='13'  or substr(deptnumber,1,3)='552' or substr(deptnumber,1,4)='5503'or substr(deptnumber,1,4)='5513') and substr(deptnumber,5,5)='00000'", "DEPTNAME", "DEPTNUMBER");
        ddlDept.Items.Insert(0, new ListItem("--全部--", "-1"));
        if (!SessionBox.GetUserSession().rolelevel.Contains("1") && !SessionBox.GetUserSession().rolelevel.Contains("0"))
        {
            ddlDept.SelectedValue = SessionBox.GetUserSession().DeptNumber;
            ddlDept.Enabled = false;
        }
    }
    /// <summary>
    /// AspNetPager1   PageChanged事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void AspNetPager1_PageChanged(object sender, EventArgs e)
    {

        //BindGridView(AspNetPager1.CurrentPageIndex - 1, "person", _pageSize, "", "", "");
        GetPersonData();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {

        GetPersonData();
    }

    protected void BindDll(DropDownList ddl, string column, string regex, string txtField, string valField)
    {
        ddl.DataSource = dept.GetTreeList(column, regex);
        ddl.DataTextField = txtField;
        ddl.DataValueField = valField;
        ddl.DataBind();
    }
    protected void BindDll(DropDownList ddl, string where, string txtField, string valField)
    {
        ddl.DataSource = dept.GetList(where);
        ddl.DataTextField = txtField;
        ddl.DataValueField = valField;
        ddl.DataBind();
    }
    protected void ddlDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetKQData();
    }

    private void GetKQData()
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
    
   
    protected void btnCancelTxt_Click(object sender, EventArgs e)
    {
        txtLightNo.Text = "";
        txtName.Text = "";
        //txtPhone.Text = "";
        txtPsnNo.Text = "";
        ddlKQ.SelectedValue = "-1";
        ddlDept.SelectedValue = "-1";
        ddlSex.SelectedValue = "-1";
        GetPersonData();
    }
    protected void xgvPerson_CustomColumnDisplayText(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewColumnDisplayTextEventArgs e)
    {
        if (e.Column.FieldName == "DEPTID" || e.Column.FieldName == "MAINDEPTID")
        {
            
            e.DisplayText = (from dept in dc.Department
                             where dept.Deptnumber == e.Value.ToString()
                             select dept).Single().Deptname;
        }
        
    }
    protected void xgvPerson_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableRowEventArgs e)
    {
        if (e.RowType != DevExpress.Web.ASPxGridView.GridViewRowType.Data)
        {
            return;
        }
        //当鼠标放上去的时候 先保存当前行的背景颜色 并给附一颜色
        e.Row.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#ffffcd',this.style.fontWeight='';");
        //当鼠标离开的时候 将背景颜色还原的以前的颜色
        e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor,this.style.fontWeight='';");
    }
}
