using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GhtnTech.SecurityFramework;
using GhtnTech.SecurityFramework.BLL;
using GhtnTech.SecurityFramework.Utility;
using GhtnTech.SecurityFramework.Model;
using GhtnTech.SecurityFramework.Utility.ExtensionHelper;
public partial class SystemManage_EditUser : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            strinfo.InnerHtml = "";
            strinfo.Visible = false;
            if (!SessionBox.CheckUserSession())
            {
                Response.Redirect("~/login.aspx");
            }
            else
            {
                if (UserHandle.ValidationHandle(PermissionTag.Browse))
                {
                    //if (!UserHandle.ValidationHandle(PermissionTag.Edit))
                    //{
                    //    btnOK.Visible = false;
                    //    btnEditRole.Visible = false;
                    //    pnlRole.Visible = false;
                    //}
                    if (Request.QueryString["uid"] != null)
                    {
                        UserHandle.BindDropDownList(ddlUserGroup, 0);
                        GetUser(int.Parse(Request.QueryString["uid"].ToString()));
                        pnlRole.Visible = false;
                    }
                }
            }
        }
        
        btnAdd.Enabled = lstOldRole.SelectedIndex >= 0 ? true : false;
        btnRemove.Enabled = lstSelectedRole.SelectedIndex >= 0 ? true : false;
    }
    protected void GetUser(int id)
    {
        //UserGroup ugbll = new UserGroup();
        //ddlUserGroup.DataSource = ugbll.GetUserGroupList("").Tables[0];
        //ddlUserGroup.DataTextField = "USERGROUPNAME";
        //ddlUserGroup.DataValueField = "USERGROUPID";
        //ddlUserGroup.DataBind();
        User bll = new User();
        SF_User model = new SF_User();
        model = bll.GetUserModel(id);
        uid.Text = id.ToString();
        txtUserName.Text = model.UserName;
        ddlUserGroup.SelectedValue = model.UserGroupID.ToString();

        txtPsnNumber.Text = model.PersonNumber;
        ddlState.SelectedValue = model.UserStatus.ToString();
        lblRegTime.Text = model.CreateTime.ToString();
        List<string> rid = model.Role;
        for (int i = 0; i < rid.Count; i++)
        {
            string[] r = rid[i].ToString().Split(',');
            lstRole.Items.Add(new ListItem(r[1], r[0]));
        }
    }
    /// <summary>
    /// ！！废弃！！加载用户分组 
    /// </summary>
    /// <param name="UG_TID">分类上级ID</param>
    /// <param name="Depth">分类级别深度</param>
    protected void LoadGroupList(string UG_TID, int Depth, DataView dvList)
    {
        dvList.RowFilter = "PARENTUSERGROUPID=" + UG_TID;  //过滤父节点 
        foreach (DataRowView dv in dvList)
        {
            string depth = string.Empty;
            for (int i = 0; i < Depth; i++)
            {
                depth = depth + "-";
            }
            ddlUserGroup.Items.Add(new ListItem(depth + dv["USERGROUPNAME"].ToString(), dv["USERGROUPID"].ToString()));

            LoadGroupList(dv["UG_ID"].ToString(), int.Parse(dv["UG_Depth"].ToString()) + 1, dvList);  //递归 
        }
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        User bll = new User();
        SF_User model = new SF_User();
        model.UserID = int.Parse(uid.Text);
        model.UserName = txtUserName.Text;
        model.UserGroupID = int.Parse(ddlUserGroup.SelectedItem.Value);
        model.UserStatus = ddlState.SelectedItem.Value;

        if (bll.UpdateUser(model) >= 1)
        {
            strinfo.InnerHtml = JSHelper.ErrInfo("用户信息更新成功!");
            strinfo.Visible = true;
        }
        else
        {
            strinfo.InnerHtml = JSHelper.ErrInfo("用户信息更新失败!");
            strinfo.Visible = true;
        }
    }
    public void GetPersonInfo()
    {

    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        MoveItems(true);
    }
    protected void btnRemove_Click(object sender, EventArgs e)
    {
        MoveItems(false);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (lstSelectedRole.Items.Count > 0)
        {
            foreach (ListItem item in lstSelectedRole.Items)
            {
                txtTRole.Text += item.Value + ",";
            }
            User bll = new User();
            List<string> ar = new List<string>(); //添加表
            List<string> dr = new List<string>(); //删除表
            string s = txtTRole.Text;
            if (s != "")
            {
                string[] str = s.Substring(0, s.Length - 1).Split(',');
                string[] ostr = txtOldRole.Text.ToString().Split(',');
                for (int i = 0; i < str.Length; i++)
                {
                    if (!TypeParse.IsStringArray(str[i], ostr))
                    {
                        //不存在则添加到插入记录列表
                        ar.Add(uid.Text + "," + str[i]);
                    }
                }

                for (int i = 0; i < ostr.Length; i++)
                {
                    if (!TypeParse.IsStringArray(ostr[i], str))
                    {
                        //不存在则添加到删除记录列表
                        dr.Add(uid.Text + "," + ostr[i]);
                    }
                }
            }
            else
            {
                //如果提交角色为空则删除该用户的所有角色
                string[] ostr = txtOldRole.Text.ToString().Split(',');
                for (int i = 0; i < ostr.Length; i++)
                {
                    //不存在则添加到删除记录列表
                    dr.Add(uid.Text + "," + ostr[i]);
                }
            }

            try
            {
                if (ar.Count != 0)
                {
                    bll.AddUserRole(ar);
                }
                if (dr.Count != 0)
                {
                    bll.DeleteUserRole(dr);
                }
                pnlRole.Visible = false;
                lstRole.Items.Clear();
                foreach (ListItem item in lstSelectedRole.Items)
                {
                    lstRole.Items.Add(item);
                }
            }
            catch
            {
            }
        }
        else { JSHelper.Alert("请至少选择一个角色！", this); }
    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        pnlRole.Visible = false;
    }

    private void MoveItems(bool isAdd)
    {

        if (isAdd)// means if you add items to the right box
        {

            for (int i = lstOldRole.Items.Count - 1; i >= 0; i--)
            {

                if (lstOldRole.Items[i].Selected)
                {

                    lstSelectedRole.Items.Add(lstOldRole.Items[i]);

                    lstSelectedRole.ClearSelection();

                    lstOldRole.Items.Remove(lstOldRole.Items[i]);

                }

            }

        }
        else // means if you remove items from the right box and add it back to the left box
        {

            for (int i = lstSelectedRole.Items.Count - 1; i >= 0; i--)
            {

                if (lstSelectedRole.Items[i].Selected)
                {

                    lstOldRole.Items.Add(lstSelectedRole.Items[i]);

                    lstOldRole.ClearSelection();

                    lstSelectedRole.Items.Remove(lstSelectedRole.Items[i]);

                }

            }

        }

    }
    protected void BindRole(string userID)
    {
        User bll = new User();
        lstOldRole.Items.Clear();
        lstSelectedRole.Items.Clear();
        List<string> rid = bll.GetUserRoleArray(int.Parse(userID));
        string strwhere = "";
        for (int i = 0; i < rid.Count; i++)
        {
            string[] r = rid[i].ToString().Split(',');
            lstSelectedRole.Items.Add(new ListItem(r[1], r[0]));
            strwhere += r[0] + ",";
        }
        if (strwhere != "")
        {
            txtOldRole.Text = strwhere.Substring(0, strwhere.Length - 1);
            strwhere = "not RoleID in(" + txtOldRole.Text + ")";
        }

        Role bll2 = new Role();
        SF_Role r2 = bll2.GetRoleModel(decimal.Parse(SessionBox.GetUserSession().CurrentRole[0].ToString().Split(',')[0]));
        switch ((int)r2.LevelID)
        {
            case 0:
                lstOldRole.DataSource = bll2.GetRoleList(strwhere, "");
                lstOldRole.DataTextField = "RoleName";
                lstOldRole.DataValueField = "RoleID";
                lstOldRole.DataBind();
                break;
            case 1:
                lstOldRole.DataSource = bll2.GetRoleList(strwhere + " and levelid >= 1", "");
                lstOldRole.DataTextField = "RoleName";
                lstOldRole.DataValueField = "RoleID";
                lstOldRole.DataBind();
                break;
            case 2:
                lstOldRole.DataSource = bll2.GetRoleList(strwhere + string.Format(" and MAINDEPTID='{0}' or (maindeptid='{1}' and levelid>={2})", SessionBox.GetUserSession().DeptNumber, "000000000", r2.LevelID.ToString()), "");
                lstOldRole.DataTextField = "RoleName";
                lstOldRole.DataValueField = "RoleID";
                lstOldRole.DataBind();
                //Session["WhereRole"] = string.Format("MAINDEPTID='{0}' or (maindeptid='{1}' and levelid>={2})", SessionBox.GetUserSession().DeptNumber, "000000000", rolelevel.ToString());
                break;
            default:
                lstOldRole.DataSource = bll2.GetRoleList(strwhere + string.Format(" and MAINDEPTID='{0}' or (maindeptid='{1}' and levelid>={2})", SessionBox.GetUserSession().DeptNumber, "000000000", r2.LevelID.ToString()), "");
                lstOldRole.DataTextField = "RoleName";
                lstOldRole.DataValueField = "RoleID";
                lstOldRole.DataBind();
                //Session["WhereRole"] = string.Format("MAINDEPTID='{0}' or (maindeptid='{1}' and levelid>={2})", SessionBox.GetUserSession().DeptNumber, "000000000", rolelevel.ToString());
                break;
        }
        
    }
    protected void lstOldRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnAdd.Enabled = lstOldRole.SelectedIndex >= 0 ? true : false;
    }
    protected void lstSelectedRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnRemove.Enabled = lstSelectedRole.SelectedIndex >= 0 ? true : false;
    }
    protected void btnEditRole_Click(object sender, EventArgs e)
    {
        BindRole(uid.Text);
        btnAdd.Enabled = lstOldRole.SelectedIndex >= 0 ? true : false;
        btnRemove.Enabled = lstSelectedRole.SelectedIndex >= 0 ? true : false;
        pnlRole.Visible = true;
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("UserManage.aspx");
    }

    /*public void BindDropDownList(System.Web.UI.WebControls.DropDownList droplist, int ParentID)
    {
        UserGroup ugbll = new UserGroup();
        DataSet dsFlowType = ugbll.GetUserGroupList("");
        string NodeId = "USERGROUPID";
        string ParentId = "PARENTUSERGROUPID";
        string NodeName = "USERGROUPNAME";

        
        DataView dvTree = new DataView(dsFlowType.Tables[0]);

        //过滤ParentId,得到当前的所有子节点
        dvTree.RowFilter = ParentId + " = " + ParentID.ToString();
        foreach (DataRowView drv in dvTree)
        {
            //-----------------------------------------
            int depth = 0;
            int NodeID = Convert.ToInt32(drv[NodeId]); ;
            Depth(NodeID, ref depth);   //计算当前节点深度

            string blank = "";
            if (ParentID != 0)
            {
                for (int i = 1; i <= depth; i++)
                {
                    blank += "　";
                    //└├┗━┖┣┗
                }
            }
            //-----------------------------------------
            ListItem list = new ListItem();
            list.Text = blank + drv[NodeName].ToString().Trim();
            list.Value = drv[NodeId].ToString().Trim();
            droplist.Items.Add(list);       //***注意区别:根节点
            BindDropDownList(droplist, Int32.Parse(drv[NodeId].ToString().Trim()));    //递归

        }
    }*/
    //计算当前节点深度
    /*public int Depth(int NodeID, ref int depth)
    {
        string NodeId = "USERGROUPID";
        string ParentId = "PARENTUSERGROUPID";

        UserGroup ugbll = new UserGroup();
        DataSet dsFlowType = ugbll.GetUserGroupList("");
        DataView dvTree = new DataView(dsFlowType.Tables[0]);
        //过滤ParentId,得到当前的所有父节点
        dvTree.RowFilter = NodeId + " = " + NodeID.ToString();
        foreach (DataRowView drv in dvTree)
        {
            int ID = Convert.ToInt32(drv[ParentId]);
            if (ID != 0)
            {
                depth += 1;
                Depth(ID, ref depth);    //递归
            }
        }
        return depth;
    }*/

}
