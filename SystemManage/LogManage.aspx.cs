using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GhtnTech.SecurityFramework;
using GhtnTech.SecurityFramework.Model;
using GhtnTech.SecurityFramework.BLL;
using GhtnTech.SecurityFramework.Utility;
using GhtnTech.SEP.DBUtility;
public partial class SystemManage_LogManage : BasePage
{
    User bll = new User();
    Role Rolebll = new Role();
    DepartmentBll dept = new DepartmentBll();
    decimal rolelevel = 0;
    string maindeptid = "";
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
                UserHandle.InitModule(this.PageTag);//初始化此模块的权限。
                if (UserHandle.ValidationHandle(PermissionTag.Browse))//是否有浏览权限
                {
                    if (!UserHandle.ValidationHandle(PermissionTag.Delete))
                    {

                        btnBatchDelete.Visible = false;
                    }
                    if (!UserHandle.ValidationHandle(PermissionTag.Search))
                    {
                        btnSearch.Visible = false;
                    }
                    if (!UserHandle.ValidationHandle(PermissionTag.Export))
                    {
                        btnExportLog.Visible = false;
                    }
                }
                else
                {
                    Session["ErrorNum"] = "0";
                    Response.Redirect("~/Error.aspx");
                }
                InitData();
            }
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        GetLogData();
    }

    /// <summary>
    /// 绑定用户数据
    /// </summary>
    protected void InitData()
    {
        string strWhere = "1=1";
        SF_Role r = Rolebll.GetRoleModel(decimal.Parse(SessionBox.GetUserSession().CurrentRole[0].ToString().Split(',')[0]));
        rolelevel = r.LevelID;
        maindeptid = SessionBox.GetUserSession().DeptNumber;
        switch ((int)rolelevel)
        {
            case 0:
                BindDll(ddlDept, "(substr(deptnumber,1,2)='23' or substr(deptnumber,1,2)='24' or substr(deptnumber,1,2)='13'  or substr(deptnumber,1,3)='552' or substr(deptnumber,1,4)='5503') and substr(deptnumber,5,5)='00000'", "DEPTNAME", "DEPTNUMBER");
                break;
            case 1:
                strWhere += " and username != 'yu'";
                BindDll(ddlDept, "(substr(deptnumber,1,2)='23' or substr(deptnumber,1,2)='24' or substr(deptnumber,1,2)='13'  or substr(deptnumber,1,3)='552' or substr(deptnumber,1,4)='5503') and substr(deptnumber,5,5)='00000'", "DEPTNAME", "DEPTNUMBER");
                break;
            case 2:
                strWhere += " and username != 'yu' and Maindeptid=" + SessionBox.GetUserSession().DeptNumber;
                BindDll(ddlDept, "(substr(deptnumber,1,2)='23' or substr(deptnumber,1,2)='24' or substr(deptnumber,1,2)='13'  or substr(deptnumber,1,3)='552' or substr(deptnumber,1,4)='5503') and substr(deptnumber,5,5)='00000'", "DEPTNAME", "DEPTNUMBER");
                ddlDept.SelectedValue = SessionBox.GetUserSession().DeptNumber;
                BindDll(ddlKQ, "ID", ddlDept.SelectedItem.Value.Remove(4), "NAME", "ID");
                ddlDept.Enabled = false;
                break;
            default:
                strWhere += " and username != 'yu' and Maindeptid=" + SessionBox.GetUserSession().DeptNumber;
                BindDll(ddlDept, "(substr(deptnumber,1,2)='23' or substr(deptnumber,1,2)='24' or substr(deptnumber,1,2)='13'  or substr(deptnumber,1,3)='552' or substr(deptnumber,1,4)='5503') and substr(deptnumber,5,5)='00000'", "DEPTNAME", "DEPTNUMBER");
                ddlDept.SelectedValue = SessionBox.GetUserSession().DeptNumber;
                BindDll(ddlKQ, "ID", ddlDept.SelectedItem.Value.Remove(4), "NAME", "ID");
                ddlDept.Enabled = false;
                break;
        }
        dateBegin.Date = DateTime.Parse(System.DateTime.Now.Year + "-" + System.DateTime.Now.Month + "-01");
        dateEnd.Date = DateTime.Today.AddDays(1);
        strWhere += " and activetime between to_date('" + dateBegin.Date.ToShortDateString() + "','YYYY-MM-DD') and to_date('" + dateEnd.Date.ToShortDateString() + "','YYYY-MM-DD')";
        string strSql = "select * from vuserlog where " + strWhere;
        gvLogManage.DataSource = OracleHelper.Query(strSql);
        gvLogManage.DataBind();
    }

    protected void GetLogData()
    {
        string strWhere = "1=1";
        SF_Role r = Rolebll.GetRoleModel(decimal.Parse(SessionBox.GetUserSession().CurrentRole[0].ToString().Split(',')[0]));
        rolelevel = r.LevelID;
        maindeptid = SessionBox.GetUserSession().DeptNumber;
        switch ((int)rolelevel)
        {
            case 0:

                break;
            case 1:
                strWhere += " and username != 'yu'";
                break;
            case 2:
                strWhere += " and username != 'yu' and maindeptid=" + SessionBox.GetUserSession().DeptNumber;
                ddlDept.SelectedValue = SessionBox.GetUserSession().DeptNumber;
                ddlDept.Enabled = false;
                break;
            default:
                strWhere += " and username != 'yu' and maindeptid=" + SessionBox.GetUserSession().DeptNumber;
                ddlDept.SelectedValue = SessionBox.GetUserSession().DeptNumber;
                ddlDept.Enabled = false;
                break;
        }
        
        if (ddlDept.SelectedValue != "-1")
        {
            strWhere += " and maindeptid=" + ddlDept.SelectedValue;
        }
        if (ddlKQ.SelectedValue != "-1")
        {
            strWhere += " and DEPTNUMBER=" + ddlKQ.SelectedValue;
        }
        if (txtIP.Text.Trim() != "")
        {
            strWhere += " and IP like '%" + txtIP.Text.Trim() + "%'";
        }
        if (txtName.Text.Trim() != "")
        {
            strWhere += " and Name like '%" + txtName.Text.Trim() + "%'";
        }
        if (txtUser.Text.Trim() != "")
        {
            strWhere += " and UserName like '%" + txtUser.Text.Trim() + "%'";
        }
        if (txtPsnNo.Text.Trim() != "")
        {
            strWhere += " and personnumber like '%" + txtPsnNo.Text.Trim() + "%'";
        }
        strWhere += " and activetime between to_date('" + dateBegin.Date.ToShortDateString() + "','YYYY-MM-DD') and to_date('" + dateEnd.Date.ToShortDateString() + "','YYYY-MM-DD')";
        string strSql = "select * from vuserlog where " + strWhere;
        gvLogManage.DataSource = OracleHelper.Query(strSql);
        gvLogManage.DataBind();
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
        ddl.DataSource = dept.GetList2(where);
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
    protected void btnExportLog_Click(object sender, EventArgs e)
    {
        GetLogData();
        expLog.WriteXlsToResponse();
    }
    protected void btnBatchDelete_Click(object sender, EventArgs e)
    {

    }
    protected void Page_PreRenderComplete(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddlDept.Items.Insert(0, new ListItem("--全部--", "-1"));
            ddlKQ.Items.Insert(0, new ListItem("--全部--", "-1"));

        }
    }
    protected void gvLogManage_PageIndexChanged(object sender, EventArgs e)
    {
        GetLogData();
    }

    protected int DeleteLog()
    {
        return 0;
    }
    protected void gvLogManage_BeforeColumnSortingGrouping(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
    {
        GetLogData();
    }
}
