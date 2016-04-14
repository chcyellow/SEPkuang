using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GhtnTech.SecurityFramework;
using GhtnTech.SecurityFramework.BLL;
using GhtnTech.SecurityFramework.Utility;
public partial class SystemManage_EditUserGroup : BasePage
{
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
                List<string> lstRole = new List<string>();
                lstRole.Add("2");
                lstRole.Add("46");
                if (SessionBox.GetUserSession().CurrentRole[0].ToString().Split(',')[0] == "31")
                {
                    Session["WhereUsergroup"] = " ";
                }
                else if (lstRole.Contains(SessionBox.GetUserSession().CurrentRole[0].ToString().Split(',')[0]))
                {
                    Session["WhereUsergroup"] = " ";
                }
                else
                {
                    Session["WhereUsergroup"] = "Usergroupid NOT in(2,3,23)";

                }
            }
        }
    }
    protected void btnSure_Click(object sender, EventArgs e)
    {
        User ull = new User();
        if (ull.UpdateUserGroup((List<object>)Session["UserIDList"], Convert.ToInt32(ddlUserGroup.SelectedValue)))
        {
            JSHelper.AlertAndCloseModalWin("更新成功！", this);
        }
        else
        {
            JSHelper.Alert("更新失败！", this);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        JSHelper.CloseModalWindow(this);
    }
}
