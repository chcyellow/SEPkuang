using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GhtnTech.SecurityFramework.BLL;
public partial class Error : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string s = string.Empty;
        if (!SessionBox.CheckUserSession())
        {
            s = @"您还未登录！   请<a href='login.aspx'>登录</a>";
        }
        else
        {
            switch (int.Parse(Session["ErrorNum"].ToString()))
            {
                case 0:
                    s = "您无权进入！  <a href='#'onclick='javascript:history.go(-1);'>返回</a>";
                    break;
                case 1:
                    break;
                case 2:
                    s = @"此用户已在别处登陆，你被强行退出！   请<a href='login.aspx'>登录</a>";
                    break;
            }
        }

        strinfo.InnerHtml = "<ul><li>" + s + "</li></ul>";
    }
}
