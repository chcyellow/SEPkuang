using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Reflection;
using GhtnTech.SecurityFramework;
using GhtnTech.SecurityFramework.Model;
using GhtnTech.SecurityFramework.Utility;
using GhtnTech.SecurityFramework.BLL;
using GhtnTech.SEP.DAL;
using System.Linq;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //if (string.IsNullOrEmpty(Request.QueryString["mark"]))
            //{
            //    DBSCMDataContext dc = new DBSCMDataContext();
            //    var mark = dc.Loginmark.First();
            //    //string serverAddr = "http://" + (mark.Lmark == 0 ? "zcpt.54321.cn/" : "10.1.10.74:81/") + "login.aspx?mark=" + mark.Lmark.ToString();
            //    string serverAddr = "http://{0}login.aspx?mark=" + mark.Lmark.ToString();
            //    string[] serverIp = new string[] { "zcpt.54321.cn/", "10.1.10.74:81/", "10.1.10.73/" };
            //    serverAddr = string.Format(serverAddr, serverIp[(int)(mark.Lmark)]);
            //    mark.Lmark = (mark.Lmark + 1) % 3;
            //    dc.SubmitChanges();
            //    if (Request.Params["txtName"] != null && Request.Params["txtPwd"] != null)
            //    {
            //        serverAddr += "&txtName=" + Request.Params["txtName"].ToString().Trim() + "&txtPwd=" + Request.Params["txtPwd"].ToString().Trim();
            //    }
            //    Response.Redirect(serverAddr, true);
            //}
            //LicHelper.LicHelper lic = new LicHelper.LicHelper();
            //if (lic.HasLic())
            //{
            if (Request.Params["txtName"] != null && Request.Params["txtPwd"] != null)
            {
                LoginVerify(Request.Params["txtName"].ToString().Trim(), SecurityEncryption.MD5(Request.Params["txtPwd"].ToString().Trim(), 32));
            }
            else
            {
                txtUserName.Focus();
            }
            //}
            //else
            //{
            //    //JSHelper.("UpdateLic.aspx",800,500,100,200,this);
            //    Response.Redirect("UpdateLic.aspx",true);
            //}

        }
    }
    protected void ibtnLogin_Click(object sender, ImageClickEventArgs e)
    {
        LoginVerify(txtUserName.Text.Trim(), SecurityEncryption.MD5(txtPwd.Text.Trim(), 32));
    }

    private void LoginVerify(string username, string pwd)
    {
        User userCrud = new User();
        SF_User user = new SF_User();
        if (userCrud.CheckLogin(username, pwd))
        {
            user = userCrud.GetUserModel(username);
            var ds = userCrud.GetUserList2(string.Format("USERNAME='{0}'", username), "").Tables[0].Select();
            if (ds[0]["USERSTATUS"].ToString() == "1")
            {
                //验证许可证
                //LicHelper.LicHelper lic = new LicHelper.LicHelper();
                //if (lic.CheckLicStatus())
                //{
                //string macid = lic.GetMachineCode();
                //if (lic.IsMachine(macid))
                //{
                //    if (lic.ContainDept(ds[0]["LEVELID"].ToString(), ds[0]["DEPTNAME"].ToString()))
                //    {

                List<string> lst = new List<string>();
                try
                {
                    userCrud.UpdateLoginTime(Convert.ToDecimal(ds[0]["USERID"]));
                }
                catch
                {
                }
                SessionBox.CreateUserSession(new UserSession(Convert.ToDecimal(ds[0]["USERID"]), ds[0]["USERNAME"].ToString(), user.Role, null, ds[0]["LEVELID"].ToString(), user.UserGroupID, user.PersonNumber, ds[0]["NAME"].ToString(), ds[0]["DEPTNUMBER"].ToString(), ds[0]["DEPTNAME"].ToString(), user.UserStatus));
                try
                {
                    GhtnTech.SecurityFramework.BLL.LogManager.WriteLog(SessionBox.GetUserSession().PersonNumber, SessionBox.GetUserSession().LoginName, GetUserIP(), DateTime.Now, GhtnTech.SecurityFramework.BLL.ActiveType.登录, "", "");
                }
                catch
                {
                }
                if (Request.Params["txtUrl"] != null)
                {
                    Response.Redirect("~/MainFrame.aspx?txtUrl=" + Request.Params["txtUrl"].ToString().Trim());
                }
                else
                {
                    Response.Redirect("~/MainFrame.aspx");
                }
                //    }
                //    else
                //    {
                //        JSHelper.AlertAndClose(UpdatePanel1, this, "您矿未获得授权。请联系管理员取得授权后再使用！");
                //    }
                //}
                //else
                //{
                //    JSHelper.AlertAndClose(UpdatePanel1, this, "您目前使用的服务器未获得授权。请联系管理员取得授权后再使用！");
                //}
                //}
                //else
                //{
                //    if (ds[0]["LEVELID"].ToString().Contains('1') || ds[0]["LEVELID"].ToString().Contains('0'))
                //    {
                //        //Response.Redirect("UpdateLic.aspx", true);
                //        //JSHelper.AlertAndRedirect(UpdatePanel1, "许可证已经过期！", "UpdateLic.aspx", this);
                //        JSHelper.ShowModalDialogWindow(UpdatePanel1,this,"UpdateLic.aspx",800,500,100,200);
                //    }
                //    else
                //    {
                //        JSHelper.AlertAndClose(UpdatePanel1, this, "许可证已经过期！请联系管理员取得更新授权后再使用！");
                //    }
                //}
                //

                //Response.Redirect("~/Default.aspx");

            }
            else
            {
                JSHelper.Alert(UpdatePanel1, this, "用户被锁定,请与管理员联系!");
            }
        }
        else
        {
            JSHelper.Alert(UpdatePanel1, this, "用户名或密码错误!");
        }
    }

    public string GetUserIP()
    {
        string userIP;
        if (Request.ServerVariables["HTTP_VIA"] == null)
        {
            userIP = Request.UserHostAddress;
        }
        else
        {
            userIP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        }
        return userIP;
    }


}
