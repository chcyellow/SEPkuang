using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GhtnTech.SecurityFramework;
using GhtnTech.SecurityFramework.BLL;
using GhtnTech.SecurityFramework.Utility;
public partial class SystemManage_PasswordEdit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSure_Click(object sender, EventArgs e)
    {
        User bllUser = new User();
        int id = 0;
        try
        {
            id = (int)SessionBox.GetUserSession().LoginId;
        }
        catch (Exception ex)
        {
            JSHelper.AlertAndRedirect("请登录后操作！\n"+ex.Message, "../Login.aspx", this);
        }
        if (bllUser.VerifyPassword(id, SecurityEncryption.MD5(txtOldPwd.Text, 32)))
        {
            if (bllUser.ChangePassword(id, SecurityEncryption.MD5(txtPwd2.Text, 32)))
            {
                JSHelper.AlertAndRedirect("密码修改成功，下次登录请用新密码！", "../Main.aspx", this);
            }
        }
        else
        {
            JSHelper.Alert("原密码输入不正确！", this);
        }
    }
}
