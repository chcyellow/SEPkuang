using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GhtnTech.SecurityFramework;
using GhtnTech.SecurityFramework.Model;
using GhtnTech.SecurityFramework.Utility;
using GhtnTech.SEP.DAL;
using GhtnTech.SecurityFramework.BLL;
public partial class SystemManage_PopInitPwd : System.Web.UI.Page
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblUser.Text = Request.QueryString["name"];
            lblName.Text = GetName(Request.QueryString["id"]);
            txtPwd.Text = "123456";
            btnSure.Focus();
        }
    }
    private string GetName(string psnNumber)
    {
        return (from p in dc.Person where p.Personnumber == psnNumber select p.Name).Single();
    }
    
    protected void btnSure_Click(object sender, EventArgs e)
    {
        User bllUser = new User();
        if (bllUser.ChangePassword(Convert.ToInt32(Request.QueryString["userid"]), SecurityEncryption.MD5(txtPwd.Text, 32)))
        {
            JSHelper.CloseWindow();
        }
    }
}
