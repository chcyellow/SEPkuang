using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GhtnTech.SecurityFramework;
using GhtnTech.SecurityFramework.BLL;
using GhtnTech.SecurityFramework.Model;
using GhtnTech.SecurityFramework.Utility;
using DevExpress.Web.ASPxGridView;
using GhtnTech.SEP.DAL;
public partial class SystemManage_RoleManage : BasePage
{
    Role rbll = new Role();
    decimal rolelevel = 0;
    string maindeptid = "";
    DBSCMDataContext dc = new DBSCMDataContext();
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
                //初始化模块权限
                UserHandle.InitModule(this.PageTag);
                //是否有浏览权限
                if (UserHandle.ValidationHandle(PermissionTag.Browse))
                {
                    GridViewCommandColumn colEdit = (GridViewCommandColumn)gridRole.Columns["编辑"];
                    if (!UserHandle.ValidationHandle(PermissionTag.Add))
                    {
                        colEdit.NewButton.Visible = false;
                    }
                    if (!UserHandle.ValidationHandle(PermissionTag.Edit))
                    {
                        colEdit.EditButton.Visible = false;
                    }
                    if (!UserHandle.ValidationHandle(PermissionTag.Delete))
                    {
                        colEdit.DeleteButton.Visible = false;
                    }
                    SF_Role r = rbll.GetRoleModel(decimal.Parse(SessionBox.GetUserSession().CurrentRole[0].ToString().Split(',')[0]));
                    rolelevel = r.LevelID;
                    maindeptid = SessionBox.GetUserSession().DeptNumber;
                    switch ((int)rolelevel)
                    {
                        case 0:
                            Session["WhereRole"] = " ";
                            break;
                        case 1:
                            Session["WhereRole"] = string.Format("levelid >={0}", rolelevel);
                            break;
                        case 2:
                            Session["WhereRole"] = string.Format("MAINDEPTID='{0}'", maindeptid);
                            gridRole.Columns["创建单位"].Visible = false;
                            break;
                        default:
                            Session["WhereRole"] = string.Format("MAINDEPTID='{0}'", maindeptid);
                            gridRole.Columns["创建单位"].Visible = false;
                            break;
                    }
                    BindRole();
                }
            }
        }
        //if(rolelevel > 1)
        //{
        //    Session["WhereRole"] = string.Format("MAINDEPTID='{0}'",maindeptid);
        //}
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindRole();
    }
    private void BindRole()
    {
        Session["WhereRole"] = null;
        string strWhere = "1=1";
        SF_Role r = rbll.GetRoleModel(decimal.Parse(SessionBox.GetUserSession().CurrentRole[0].ToString().Split(',')[0]));
        rolelevel = r.LevelID;
        maindeptid = SessionBox.GetUserSession().DeptNumber;
        switch ((int)rolelevel)
        {
            case 0:
                strWhere += string.Format(" and levelid >={0}", rolelevel);
                break;
            case 1:
                strWhere += string.Format(" and levelid >={0}", rolelevel);
                break;
            case 2:
                strWhere += string.Format(" and MAINDEPTID='{0}'", maindeptid);
                break;
            default:
                strWhere += string.Format(" and MAINDEPTID='{0}'", maindeptid);
                break;
        }
        //if (rolelevel > 1)
        //{
        //    strWhere += string.Format("MAINDEPTID='{0}'", maindeptid);
        //}
        if (txtRoleAbout.Text.Trim() != "")
        {
            strWhere += string.Format(" and roleabout like '%{0}%'",txtRoleAbout.Text.Trim());
        }
        if (txtRoleName.Text.Trim() != "")
        {
            strWhere += string.Format(" and rolename like '%{0}%'", txtRoleName.Text.Trim());
        }

        //var ds = rbll.GetRoleList(strWhere, " ORDER BY CreateTime DESC");
        ObjectDataSource1.SelectParameters["strWhere"].DefaultValue = strWhere;
        ObjectDataSource1.SelectParameters["strOrder"].DefaultValue = " ORDER BY CreateTime DESC";
        gridRole.DataSourceID = ObjectDataSource1.ID;
        gridRole.DataBind();
        gridRole.KeyFieldName = "ROLEID";
    }
    protected void gridRole_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
    {
        SF_Role r = rbll.GetRoleModel(decimal.Parse(SessionBox.GetUserSession().CurrentRole[0].ToString().Split(',')[0]));
        rolelevel = r.LevelID;
        maindeptid = SessionBox.GetUserSession().DeptNumber;
        e.NewValues["LEVELID"] = rolelevel;
        if (rolelevel > 1)
        {
            e.NewValues["MAINDEPTID"] = maindeptid;
        }
        else
        {
            e.NewValues["MAINDEPTID"] = "000000000";
        }
    }
    protected void gridRole_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
    {
        if (e.Column.FieldName == "MAINDEPTID")
        {

            e.DisplayText = (from dept in dc.Department
                             where dept.Deptnumber == e.Value.ToString()
                             select dept).Single().Deptname;
        }
    }
}
