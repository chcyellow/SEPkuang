using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using GhtnTech.SecurityFramework.BLL;

public partial class BaseManage_PositionManage : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (!SessionBox.CheckUserSession())
            {
                Response.Redirect("~/Login.aspx");
            }
            
            //初始化模块权限
            UserHandle.InitModule(this.PageTag);
            //是否有浏览权限
            if (UserHandle.ValidationHandle(PermissionTag.Browse))
            {
                GridViewCommandColumn colEdit = (GridViewCommandColumn)GridViewPos.Columns["操作"];
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
            }
        }

        List<string> lstRole = new List<string>();
        lstRole.Add("2");
        lstRole.Add("46");
        if (SessionBox.GetUserSession().CurrentRole[0].ToString().Split(',')[0] == "31")
        {
        }
        else if (lstRole.Contains(SessionBox.GetUserSession().CurrentRole[0].ToString().Split(',')[0]))
        {
        }
        else
        {
            ALinqDataSource1.Where = "Maindeptid == \"" + SessionBox.GetUserSession().DeptNumber + "\"";

        }
    }
    protected void GridViewPos_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
    {
        e.NewValues["Maindeptid"] = SessionBox.GetUserSession().DeptNumber;
    }
}
