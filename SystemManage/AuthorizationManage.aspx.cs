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
public partial class SystemManage_AuthorizationManage : BasePage
{
    Operator AD = new Operator();
    Module Modulebll = new Module();
    Role Rolebll = new Role();
    int rolelevel = 0;
    string roledeptid = "";
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
                switch ((int)rolelevel)
                {
                    case 0:
                        Session["WhereRole"] = "ROLESTATUS=1";
                        break;
                    case 1:
                        Session["WhereRole"] = string.Format("levelid >={0} and ROLESTATUS=1", rolelevel);
                        break;
                    case 2:
                        Session["WhereRole"] = string.Format("MAINDEPTID='{0}' and ROLESTATUS=1", roledeptid);
                        break;
                    default:
                        Session["WhereRole"] = string.Format("MAINDEPTID='{0}' and ROLESTATUS=1", roledeptid);
                        break;
                }
                //if (rolelevel > 1)
                //{
                //    Session["WhereRole"] = string.Format("MAINDEPTID='{0}'", SessionBox.GetUserSession().DeptNumber);
                //}
                //List<string> lstRole = new List<string>();
                //lstRole.Add("2");
                //lstRole.Add("46");
                //if (SessionBox.GetUserSession().CurrentRole[0].ToString().Split(',')[0] == "31")
                //{
                //    Session["WhereRole"] = " ";
                //}
                //else if (lstRole.Contains(SessionBox.GetUserSession().CurrentRole[0].ToString().Split(',')[0]))
                //{
                //    Session["WhereRole"] = "roleid != 31";
                //}
                //else
                //{
                //    Session["WhereRole"] = "roleid NOT in(2,31,46)";

                //}
                //初始化模块权限
                UserHandle.InitModule(this.PageTag);
                if (UserHandle.ValidationHandle(PermissionTag.Browse))//是否有浏览权限
                {

                    BindModule();
                    if(!UserHandle.ValidationHandle(PermissionTag.Edit))
                    {
                        gvModuleOperator.Columns[2].Visible = false;
                    }
                }
                else
                {
                    Session["ErrorNum"] = "0";
                    Response.Redirect("~/Error.aspx");
                }
            }
        }
    }
    protected void gvModuleOperator_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            
            CheckBoxList AuthorityList = (CheckBoxList)e.Row.FindControl("chkAuthorityList");
            Label lblModuleID = (Label)e.Row.FindControl("lblModuleID");
            Label lblVerify = (Label)e.Row.FindControl("lblVerify");

            var ds = AD.GetOperatorList("", "order by OperatorOrder asc");
            var MALDS = Modulebll.GetOperatorList2(int.Parse(lblModuleID.Text));
            var RALDS = Rolebll.GetRoleOperatorList(int.Parse(Rid.Text), int.Parse(lblModuleID.Text));

            int n = ds.Tables[0].Rows.Count;//系统权限个数

            string[] vstate = new string[n];

            //获取系统配置的权限列表，如果模块没有该权限，则禁用该权限
            //for (int i = 0; i < n; i++)
            //{
            //    AuthorityList.Items.Add(new ListItem(ds.Tables[0].Rows[i]["OPERATORNAME"].ToString(), ds.Tables[0].Rows[i]["OPERATORTAG"].ToString()));
            //    AuthorityList.Items[i].Enabled = false;
            //    for (int k = 0; k < MALDS.Tables[0].Rows.Count; k++)
            //    {
            //        if (ds.Tables[0].Rows[i]["OPERATORTAG"].ToString() == MALDS.Tables[0].Rows[k]["OPERATORTAG"].ToString())
            //        {
            //            AuthorityList.Items[i].Enabled = true;
            //            break;
            //        }
            //    }
            //    vstate[i] = "0";//初始状态数组;
            //}
            //修改后
            for (int i = 0; i < MALDS.Tables[0].Rows.Count; i++)
            {
                AuthorityList.Items.Add(new ListItem(MALDS.Tables[0].Rows[i]["OPERATORNAME"].ToString(), MALDS.Tables[0].Rows[i]["OPERATORTAG"].ToString()));
                vstate[i] = "0";//初始状态数组;
            }
            
            AuthorityList.DataBind();

            //将模块权限付值
            for (int j = 0; j < RALDS.Tables[0].Rows.Count; j++)
            {
                for (int l = 0; l < AuthorityList.Items.Count; l++)
                {
                    if (RALDS.Tables[0].Rows[j]["OPERATORTAG"].ToString() == AuthorityList.Items[l].Value && RALDS.Tables[0].Rows[j]["STATUS"].ToString() == "1")
                    {
                        if (AuthorityList.Items[l].Enabled)
                            vstate[l] = "1";//权限存在
                        AuthorityList.Items[l].Selected = true;
                        break;
                    }
                }
            }

            lblVerify.Text = TypeParse.StringArrayToString(vstate, ',');
            if (!UserHandle.ValidationHandle(PermissionTag.Edit))//是否有编辑权限
            {
                AuthorityList.Enabled = false;
            }
        }
    }
    protected void gvModuleOperator_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        List<string> list = new List<string>();//建立事务列表
        Role Rolebll = new Role();
        CheckBoxList chkAuthorityList = (CheckBoxList)gvModuleOperator.Rows[e.NewSelectedIndex].Cells[1].FindControl("chkAuthorityList");
        Label lblVerify = (Label)gvModuleOperator.Rows[e.NewSelectedIndex].Cells[0].FindControl("lblVerify");
        string[] vstate = lblVerify.Text.Split(',');//获取原始状态

        for (int i = 0; i < chkAuthorityList.Items.Count; i++)
        {
            if (chkAuthorityList.Items[i].Enabled)
            {
                if (chkAuthorityList.Items[i].Selected)
                {
                    if (vstate[i] != "1")//检查数据有没有变化
                    {
                        string item = string.Empty;
                        item = item + Rid.Text + "|"
                            + gvModuleOperator.DataKeys[e.NewSelectedIndex].Values[0].ToString() + "|"
                            + chkAuthorityList.Items[i].Value + "|1";//设置为1，加入权限
                        list.Add(item);
                    }
                }
                else
                {
                    if (vstate[i] != "0")//检查数据有没有变化
                    {
                        string item = string.Empty;
                        item = item + Rid.Text + "|"
                            + gvModuleOperator.DataKeys[e.NewSelectedIndex].Values[0].ToString() + "|"
                            + chkAuthorityList.Items[i].Value + "|0";//设置为0，删除删除
                        list.Add(item);
                    }
                }
            }
        }

        if (Rolebll.UpdateRoleOperator(list))
        {
            BindModule();
            JSHelper.Alert("更新成功！",this);
        }
        else
        {
            JSHelper.Alert("更新失败！",this);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Role Rolebll = new Role();

        List<string> list = new List<string>();//建立事务列表
        for (int i = 0; i <= gvModuleOperator.Rows.Count - 1; i++)
        {
            CheckBoxList chkAuthorityList = (CheckBoxList)gvModuleOperator.Rows[i].Cells[1].FindControl("chkAuthorityList");
            Label lblVerify = (Label)gvModuleOperator.Rows[i].Cells[0].FindControl("lblVerify");
            string[] vstate = lblVerify.Text.Split(',');//获取原始状态

            for (int j = 0; j < chkAuthorityList.Items.Count; j++)
            {
                if (chkAuthorityList.Items[j].Enabled)
                {
                    if (chkAuthorityList.Items[j].Selected)
                    {
                        if (vstate[j] != "1")//检查数据有没有变化
                        {
                            string item = string.Empty;
                            item = item + Rid.Text + "|"
                                + gvModuleOperator.DataKeys[i].Values[0].ToString() + "|"
                                + chkAuthorityList.Items[j].Value + "|1";//设置为1，加入权限
                            list.Add(item);
                        }
                    }
                    else
                    {
                        if (vstate[j] != "0")//检查数据有没有变化
                        {
                            string item = string.Empty;
                            item = item + Rid.Text + "|"
                                + gvModuleOperator.DataKeys[i].Values[0].ToString() + "|"
                                + chkAuthorityList.Items[j].Value + "|0";//设置为0，删除删除
                            list.Add(item);
                        }
                    }
                }
            }
        }

        if (Rolebll.UpdateRoleOperator(list))
        {
            BindModule();
            JSHelper.Alert("设置成功！",this);

        }
        else
        {
            JSHelper.Alert("设置操作失败！",this);
        }
    }
    protected void cboRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        Rid.Text = cboRole.SelectedItem.Value.ToString();
        BindModule();
    }
    protected void ddlModuleGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindModule();
    }
    /// <summary>
    /// 获取模块权限列表
    /// </summary>
    protected void BindModule()
    {
        if (Rid.Text != "")
        {
            Module Mbll = new Module();
            if (SessionBox.GetUserSession().rolelevel.Contains("0"))
            {
                var ds = Mbll.GetModuleList2(ddlModuleGroup.SelectedValue);
                gvModuleOperator.DataSource = ds;
                gvModuleOperator.DataBind();

                if (ds.Tables[0].Rows.Count == 0)
                {
                    btnSave.Visible = false;
                }
                else
                {
                    if (UserHandle.ValidationHandle(PermissionTag.Edit))
                    {
                        btnSave.Visible = true;
                    }
                    else
                    {
                        btnSave.Visible = false;
                    }
                }
            }
            else
            {
                var ds = Mbll.GetModuleListWithRole(ddlModuleGroup.SelectedValue, SessionBox.GetUserSession().CurrentRole[0].ToString().Split(',')[0]);
                gvModuleOperator.DataSource = ds;
                gvModuleOperator.DataBind();

                if (ds.Tables[0].Rows.Count == 0)
                {
                    btnSave.Visible = false;
                }
                else
                {
                    if (UserHandle.ValidationHandle(PermissionTag.Edit))
                    {
                        btnSave.Visible = true;
                    }
                    else
                    {
                        btnSave.Visible = false;
                    }
                }
            }
            
        }
        else
        {
            btnSave.Visible = false;
            gvModuleOperator.DataSource = null;
            gvModuleOperator.DataBind();
        }
    }
}
