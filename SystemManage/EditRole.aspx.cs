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
public partial class SystemManage_EditRole : BasePage
{
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
                        Session["WhereRole"] = string.Format("levelid >={0}", rolelevel);
                        break;
                    case 1:
                        Session["WhereRole"] = string.Format("levelid >={0}", rolelevel);
                        break;
                    case 2:
                        Session["WhereRole"] = string.Format("MAINDEPTID='{0}' or (maindeptid='{1}' and levelid>={2})", SessionBox.GetUserSession().DeptNumber, "000000000", rolelevel.ToString());
                        break;
                    default:
                        Session["WhereRole"] = string.Format("MAINDEPTID='{0}' or (maindeptid='{1}' and levelid>={2})", SessionBox.GetUserSession().DeptNumber, "000000000", rolelevel.ToString());
                        break;
                }
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
            }
        }
        if (Session["UserIDList"] != null)
        {
            lblRole.Text = "";
        }
        btnAdd.Enabled = lstOldRole.SelectedIndex >= 0 ? true : false;
        btnRemove.Enabled = lstSelectedRole.SelectedIndex >= 0 ? true : false;
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
            foreach (var uid in (List<object>)Session["UserIDList"])
            {
                txtTRole.Text = "";
                BindRole(uid.ToString());
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
                            ar.Add(uid.ToString() + "," + str[i]);
                        }
                    }

                    for (int i = 0; i < ostr.Length; i++)
                    {
                        if (!TypeParse.IsStringArray(ostr[i], str))
                        {
                            //不存在则添加到删除记录列表
                            dr.Add(uid.ToString() + "," + ostr[i]);
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
                        dr.Add(uid.ToString() + "," + ostr[i]);
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
                    JSHelper.AlertAndCloseModalWin("更新成功！", this);

                }
                catch
                {
                    JSHelper.Alert("角色修改失败！", this); 
                }
            }
        }
        else 
        { 
            JSHelper.Alert("请至少选择一个角色！", this); 
        }
    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        JSHelper.CloseModalWindow(this);
    }
    protected void lstOldRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnAdd.Enabled = lstOldRole.SelectedIndex >= 0 ? true : false;
    }
    protected void lstSelectedRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnRemove.Enabled = lstSelectedRole.SelectedIndex >= 0 ? true : false;
    }
    protected void BindRole(string userID)
    {
        txtOldRole.Text = "";
        User bll = new User();
        List<string> rid = bll.GetUserRoleArray(int.Parse(userID));
        string strwhere = "";
        for (int i = 0; i < rid.Count; i++)
        {
            string[] r = rid[i].ToString().Split(',');
            strwhere += r[0] + ",";
        }
        if (strwhere != "")
        {
            txtOldRole.Text = strwhere.Substring(0, strwhere.Length - 1);
        }

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
}
