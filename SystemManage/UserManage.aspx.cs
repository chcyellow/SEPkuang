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
using System.Text;
using System.Data;
public partial class SystemManage_UserManage : BasePage
{
    User bll = new User();
    Role Rolebll = new Role();
    DepartmentBll dept = new DepartmentBll();
    Role role = new Role();
    UserGroup ug = new UserGroup();
    decimal rolelevel = 0;
    string maindeptid = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Aspose.Cells.License license = new Aspose.Cells.License();

            //license.SetLicense("License.lic");


            if (!SessionBox.CheckUserSession())
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {

                SF_Role r = Rolebll.GetRoleModel(decimal.Parse(SessionBox.GetUserSession().CurrentRole[0].ToString().Split(',')[0]));
                rolelevel = (int)r.LevelID;
                maindeptid = SessionBox.GetUserSession().DeptNumber;
                switch ((int)rolelevel)
                {
                    case 0:
                        //Session["WhereRole"] = string.Format(" levelid >={0}", rolelevel);
                        InitRole(string.Format(" levelid >={0}", rolelevel));
                        InitUserGroup("");
                        break;
                    case 1:
                        //Session["WhereRole"] = string.Format(" levelid >={0}", rolelevel);
                        InitRole(string.Format(" levelid >={0}", rolelevel));
                        InitUserGroup("");
                        break;
                    case 2:
                        //Session["WhereRole"] = string.Format("MAINDEPTID='{0}' or (maindeptid='{1}' and levelid>={2})", SessionBox.GetUserSession().DeptNumber, "000000000", rolelevel.ToString());
                        //Session["WhereUserGroup"] = "usergroupid not in(2,3,23)";
                        InitRole(string.Format("MAINDEPTID='{0}' or (maindeptid='{1}' and levelid>={2})", SessionBox.GetUserSession().DeptNumber, "000000000", rolelevel.ToString()));
                        InitUserGroup("usergroupid not in(2,3,23)");
                        break;
                    default:
                        //Session["WhereRole"] = string.Format("MAINDEPTID='{0}' or (maindeptid='{1}' and levelid>={2})", SessionBox.GetUserSession().DeptNumber, "000000000", rolelevel.ToString());
                        //Session["WhereUserGroup"] = "usergroupid not in(2,3,23)";
                        InitRole(string.Format("MAINDEPTID='{0}' or (maindeptid='{1}' and levelid>={2})", SessionBox.GetUserSession().DeptNumber, "000000000", rolelevel.ToString()));
                        InitUserGroup("usergroupid not in(2,3,23)");
                        break;
                }


                UserHandle.InitModule(this.PageTag);//初始化此模块的权限。
                if (UserHandle.ValidationHandle(PermissionTag.Browse))//是否有浏览权限
                {
                    //UserHandle.BindDropDownList(ddlUserGroup, 0);
                    //DevExpress.Web.ASPxGridView.GridViewCommandColumn colEdit = (DevExpress.Web.ASPxGridView.GridViewCommandColumn)gridUser.Columns["编辑"];
                    //DevExpress.Web.ASPxGridView.GridViewCommandColumn colDel = (DevExpress.Web.ASPxGridView.GridViewCommandColumn)gridUser.Columns["删除"];
                    if (!UserHandle.ValidationHandle(PermissionTag.Add))
                    {
                        btnAddUser.Visible = false;
                    }
                    if (!UserHandle.ValidationHandle(PermissionTag.Edit))
                    {
                        gridUser.Columns["编辑"].Visible = false;
                    }
                    if (!UserHandle.ValidationHandle(PermissionTag.Delete))
                    {
                        gridUser.Columns["删除"].Visible = false;
                    }
                    if (!UserHandle.ValidationHandle(PermissionTag.Search))
                    {
                        btnSearch.Visible = false;
                    }
                    if (!UserHandle.ValidationHandle(PermissionTag.BatchEditRole))
                    {
                        btnEditRole.Visible = false;
                    }
                    if (!UserHandle.ValidationHandle(PermissionTag.BatchEditUsergroup))
                    {
                        btnEditUsergroup.Visible = false;
                    }
                    if (!UserHandle.ValidationHandle(PermissionTag.ResetPassword))
                    {
                        gridUser.Columns["密码重置"].Visible = false;
                    }
                    if (!UserHandle.ValidationHandle(PermissionTag.ImportUser))
                    {
                        btnBatchAddUser.Visible = false;
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
        BindOrder();
    }
    protected void btnAddUser_Click(object sender, EventArgs e)
    {
        JSHelper.OpenWebFormSize("AddUser.aspx", 880, 750, 10, 200, this);
    }
    public DataSet d_GetTreeList(string column, string regex)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append("select NAME,ID");
        strSql.Append(" FROM GETDEPTTREE ");
        if (column.Trim() != "" && regex.Trim() != "")
        {
            strSql.Append(string.Format(" where REGEXP_LIKE({0},'{1}') and deptlevel='正科级'", column, regex));
        }
        return OracleHelper.Query(strSql.ToString());
    }
    protected void Page_PreRenderComplete(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddlRole.Items.Insert(0, new ListItem("--全部--", "-1"));
            ddlDept.Items.Insert(0, new ListItem("--全部--", "-1"));
            ddlUserGroup.Items.Insert(0, new ListItem("--全部--", "-1"));
            ddlKQ.Items.Insert(0, new ListItem("--全部--", "-1"));

        }
    }

    protected void InitRole(string where)
    {
        ddlRole.DataSource = role.GetVRoleList(where, "");
        ddlRole.DataTextField = "ROLENAME";
        ddlRole.DataValueField = "ROLEID";
        ddlRole.DataBind();

    }
    protected void InitUserGroup(string where)
    {
        ddlUserGroup.DataSource = ug.GetUserGroupList(where);
        ddlUserGroup.DataTextField = "USERGROUPNAME";
        ddlUserGroup.DataValueField = "USERGROUPID";
        ddlUserGroup.DataBind();

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
                //BindDll(ddlDept, "deptnumber like'%00000' and deptname like'%公司'", "DEPTNAME", "DEPTNUMBER");
                BindDll(ddlDept, "(substr(deptnumber,1,2)='23' or substr(deptnumber,1,2)='24' or substr(deptnumber,1,2)='13'  or substr(deptnumber,1,2)='55' or substr(deptnumber,1,4)='5503' or substr(deptnumber,1,4)='5504' or substr(deptnumber,1,4)='5513' or substr(deptnumber,1,4)='5564') and substr(deptnumber,5,5)='00000'", "DEPTNAME", "DEPTNUMBER");
                break;
            case 1:
                strWhere += " and userid != 1";
                //BindDll(ddlDept, "deptnumber like'%00000' and deptname like'%公司'", "DEPTNAME", "DEPTNUMBER");
                BindDll(ddlDept, "(substr(deptnumber,1,2)='23' or substr(deptnumber,1,2)='24' or substr(deptnumber,1,2)='13'  or substr(deptnumber,1,2)='55' or substr(deptnumber,1,4)='5503' or substr(deptnumber,1,4)='5504' or substr(deptnumber,1,4)='5513' or substr(deptnumber,1,4)='5564') and substr(deptnumber,5,5)='00000'", "DEPTNAME", "DEPTNUMBER");
                break;
            case 2:
                strWhere += " and userid != 1 and DEPTNUMBER=" + SessionBox.GetUserSession().DeptNumber;
                //BindDll(ddlDept, "deptnumber like'%00000' and deptname like'%公司'", "DEPTNAME", "DEPTNUMBER");
                BindDll(ddlDept, "(substr(deptnumber,1,2)='23' or substr(deptnumber,1,2)='24' or substr(deptnumber,1,2)='13'  or substr(deptnumber,1,2)='55' or substr(deptnumber,1,4)='5503' or substr(deptnumber,1,4)='5504' or substr(deptnumber,1,4)='5513' or substr(deptnumber,1,4)='5564') and substr(deptnumber,5,5)='00000'", "DEPTNAME", "DEPTNUMBER");
                ddlDept.SelectedValue = SessionBox.GetUserSession().DeptNumber;
                BindDll(ddlKQ, "ID", ddlDept.SelectedItem.Value.Remove(4), "NAME", "ID");
                ddlDept.Enabled = false;
                break;
            default:
                strWhere += " and userid != 1 and DEPTNUMBER=" + SessionBox.GetUserSession().DeptNumber;
                BindDll(ddlDept, "(substr(deptnumber,1,2)='23' or substr(deptnumber,1,2)='24' or substr(deptnumber,1,2)='13'  or substr(deptnumber,1,2)='55' or substr(deptnumber,1,4)='5503' or substr(deptnumber,1,4)='5504' or substr(deptnumber,1,4)='5513' or substr(deptnumber,1,4)='5564') and substr(deptnumber,5,5)='00000'", "DEPTNAME", "DEPTNUMBER");
                ddlDept.SelectedValue = SessionBox.GetUserSession().DeptNumber;
                BindDll(ddlKQ, "ID", ddlDept.SelectedItem.Value.Remove(4), "NAME", "ID");
                ddlDept.Enabled = false;
                break;
        }
        //string strWhere = "1=1";
        //if (SessionBox.GetUserSession().CurrentRole[0].ToString().Split(',')[0] == "31")
        //{
        //    //Session["WhereUser"] = " ";
        //}
        //else if (SessionBox.GetUserSession().CurrentRole[0].ToString().Split(',')[0] == "2" || SessionBox.GetUserSession().CurrentRole[0].ToString().Split(',')[0] == "46")
        //{
        //    //Session["WhereUser"] = "userid != 1";
        //    //Session["WhereRole"] = "roleid NOT in(31)";
        //    strWhere += " and userid != 1";
        //}
        //else
        //{
        //    //Session["WhereRole"] = "roleid NOT in(2,31,46)";
        //    //Session["WhereUserGroup"] = "usergroupid not in(2,3,23)";
        //    //Session["WhereUser"] = "userid != 1 and DEPTNUMBER=" + SessionBox.GetUserSession().DeptNumber;
        //    strWhere += " and userid != 1 and DEPTNUMBER=" + SessionBox.GetUserSession().DeptNumber;
        //    ddlDept.SelectedValue = SessionBox.GetUserSession().DeptNumber;
        //    ddlDept.Enabled = false;
        //}
        Session["WhereRole"] = "ROLESTATUS=1";
        var ds = bll.GetUserList2(strWhere, " ORDER BY CreateTime DESC");
        ObjectDataSource1.SelectParameters["strWhere"].DefaultValue = strWhere;
        ObjectDataSource1.SelectParameters["strOrder"].DefaultValue = " ORDER BY CreateTime DESC";
        if (ds.Tables[0].Rows.Count == 0)
            GridViewMsg.InnerText = "无记录";
        else
            GridViewMsg.InnerText = "共有" + ds.Tables[0].Rows.Count + "条记录";
        gridUser.DataSourceID = ObjectDataSource1.ID;
        //gvUserList.DataSource = ds;
        gridUser.DataBind();
        gridUser.KeyFieldName = "USERID";
    }
    protected void BindOrder()
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
                strWhere += " and userid != 1";
                break;
            case 2:
                strWhere += " and userid != 1 and DEPTNUMBER=" + SessionBox.GetUserSession().DeptNumber;
                ddlDept.SelectedValue = SessionBox.GetUserSession().DeptNumber;
                ddlDept.Enabled = false;
                break;
            default:
                strWhere += " and userid != 1 and DEPTNUMBER=" + SessionBox.GetUserSession().DeptNumber;
                ddlDept.SelectedValue = SessionBox.GetUserSession().DeptNumber;
                ddlDept.Enabled = false;
                break;
        }

        if (ddlUserGroup.SelectedValue != "-1")
        {
            strWhere += " and USERGROUPID=" + ddlUserGroup.SelectedValue;
        }
        if (ddlRole.SelectedValue != "-1")
        {
            strWhere += " and ROLENAME like '%" + ddlRole.SelectedItem.Text + "%'";
        }
        if (ddlDept.SelectedValue != "-1")
        {
            strWhere += " and DEPTNUMBER=" + ddlDept.SelectedValue;
        }
        if (ddlKQ.SelectedValue != "-1")
        {
            strWhere += " and DEPTID in (select deptnumber from department start with deptnumber = " + ddlKQ.SelectedValue + " connect by prior deptnumber = fatherid)";
        }
        //if (ddlIsOnline.SelectedValue != "-1")
        //{
        //    strWhere += " and isonline=" + ddlIsOnline.SelectedValue;
        //}
        if (ddlUserStatus.SelectedValue != "-1")
        {
            strWhere += " and userstatus=" + ddlUserStatus.SelectedValue;
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
        var ds = bll.GetUserList2(strWhere, " ORDER BY CreateTime DESC");
        ObjectDataSource1.SelectParameters["strWhere"].DefaultValue = strWhere;
        ObjectDataSource1.SelectParameters["strOrder"].DefaultValue = " ORDER BY CreateTime DESC";
        if (ds.Tables[0].Rows.Count == 0)
            GridViewMsg.InnerText = "无记录";
        else
            GridViewMsg.InnerText = "共有" + ds.Tables[0].Rows.Count + "条记录";
        gridUser.DataSourceID = ObjectDataSource1.ID;
        //gvUserList.DataSource = ds;
        gridUser.DataBind();
        gridUser.KeyFieldName = "USERID";
    }
    protected void gridUser_CustomColumnDisplayText(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewColumnDisplayTextEventArgs e)
    {
        if (e.Column.FieldName == "USERSTATUS")
        {
            e.DisplayText = UserHandle.ReturnState(TypeParse.StrToInt(e.GetFieldValue("USERSTATUS"), 0));
        }
        //if (e.Column.FieldName == "ISONLINE")
        //{
        //    e.DisplayText = e.GetFieldValue("ISONLINE") == "1" ? "在线" : "离线";
        //}
    }
    protected void gridUser_PageIndexChanged(object sender, EventArgs e)
    {
        BindOrder();
    }
    protected void gridUser_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableRowEventArgs e)
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
    protected void btnEditRole_Click(object sender, EventArgs e)
    {
        if (gridUser.Selection.Count <= 0)
        {
            JSHelper.Alert("请至少选择一个用户！", this);
        }
        else
        {
            if (Session["UserIDList"] != null)
            {
                Session.Remove("UserIDList");
            }
            List<object> keyValues = gridUser.GetSelectedFieldValues("USERID");
            Session["UserIDList"] = keyValues;
            JSHelper.ShowModalDialogWindow("EditRole.aspx", 580, 380, 300, 450, this);
        }
    }
    protected void btnEditUsergroup_Click(object sender, EventArgs e)
    {
        if (gridUser.Selection.Count <= 0)
        {
            JSHelper.Alert("请至少选择一个用户！", this);
        }
        else
        {
            if (Session["UserIDList"] != null)
            {
                Session.Remove("UserIDList");
            }
            List<object> keyValues = gridUser.GetSelectedFieldValues("USERID");
            Session["UserIDList"] = keyValues;
            JSHelper.ShowModalDialogWindow("EditUserGroup.aspx", 300, 200, 300, 450, this);
        }
    }
    protected void BindDll(DropDownList ddl, string column, string regex, string txtField, string valField)
    {
        ddl.DataSource = d_GetTreeList(column, regex);
        //---------------------------------------------------------------
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
        /*
         * if (ddlDept.SelectedValue != "-1")
        {
            BindDll(ddlKQ, "ID", ddlDept.SelectedItem.Value.Remove(4), "NAME", "ID");
            ddlKQ.Items.Insert(0, new ListItem("--全部--", "-1"));
        }
         * */
        if (ddlDept.SelectedValue != "-1")
        {
            //BindDll(ddlKQ, "ID", ddlDept.SelectedItem.Value.Remove(4), "NAME", "ID");
            DataSet tem = new DataSet();
            tem = d_GetTreeList("ID", ddlDept.SelectedItem.Value.Remove(4));
            foreach (DataRow dr in tem.Tables[0].Rows)
            {
                ddlKQ.Items.Insert(0, new ListItem(dr["NAME"].ToString(), dr["ID"].ToString()));
            }
            ddlKQ.Items.Insert(0, new ListItem("--全部--", "-1"));
        }
        else
        {
            ddlKQ.Items.Clear();
            ddlKQ.Items.Insert(0, new ListItem("--全部--", "-1"));
        }
    }
    protected void btnBatchAddUser_Click(object sender, EventArgs e)
    {
        pnlAddPsn.Visible = true;
    }
    protected void btnSure_Click(object sender, EventArgs e)
    {
        if (upctrlPsn.UploadedFiles[0].IsValid && upctrlPsn.HasFile)
        {
            commonFunc.SaveFile(upctrlPsn, this);
            string savePath = commonFunc.GetSavePath(upctrlPsn, this);
            Aspose.Cells.Workbook xlsPsn = new Aspose.Cells.Workbook(savePath);
            //xlsPsn.Open(savePath);
            Aspose.Cells.Cells cellsPsn = xlsPsn.Worksheets[0].Cells;
            var dtTemp = cellsPsn.ExportDataTable(0, 0, cellsPsn.MaxDataRow + 1, cellsPsn.MaxDataColumn + 1);
            User usersCrud = new User();
            SF_User user = new SF_User();
            if (ddlUserGroup.SelectedValue != "-1" && ddlRole.SelectedValue != "-1")
            {
                int usercount = 0;
                int usercount2 = 0;
                string userName = "";
                user.PassWord = SecurityEncryption.MD5("123456", 32);
                int roleID = 0;
                roleID = Convert.ToInt32(ddlRole.SelectedValue);
                user.UserGroupID = Convert.ToInt32(ddlUserGroup.SelectedValue);
                for (int i = 1; i < dtTemp.Rows.Count; i++)
                {
                    user.UserName = dtTemp.Rows[i][0].ToString().Trim();
                    user.PersonNumber = dtTemp.Rows[i][0].ToString().Trim();
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
                        if (cellsPsn.Columns.Count >= 3 && dtTemp.Rows[i][3].ToString().Trim() != "")
                        {
                            SF_User user2 = usersCrud.GetUserModel(user.UserName);
                            user2.UserName = dtTemp.Rows[i][3].ToString().Trim();
                            int n2 = (int)usersCrud.UpdateUser(user2);
                            if (n2 >= 1)
                            {
                                usercount2++;
                            }
                        }
                        userName += user.UserName + ",";
                    }
                }
                commonFunc.DeleteFile(savePath);
                JSHelper.Alert(string.Format("共添加{0}个用户！共更新{1}个用户！", usercount, usercount2), this);
            }
            else
            {
                JSHelper.Alert("初始化信息请填写完整！", this);
            }
        }

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        pnlAddPsn.Visible = false;
    }
}
