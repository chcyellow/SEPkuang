using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GhtnTech.SecurityFramework;
using GhtnTech.SecurityFramework.BLL;
using GhtnTech.SecurityFramework.Utility;
using DevExpress.Web.ASPxGridView;
public partial class SystemManage_ModuleGroupManage : BasePage
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
                //初始化模块权限
                UserHandle.InitModule(this.PageTag);
                //是否有浏览权限
                if (UserHandle.ValidationHandle(PermissionTag.Browse))
                {
                    GridViewCommandColumn colEdit = (GridViewCommandColumn)gridModuleGroup.Columns["操作"];
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
        }
    }
    protected void gridModuleGroup_CustomColumnDisplayText(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewColumnDisplayTextEventArgs e)
    {
        if (e.Column.FieldName == "STATUS")
        {
            e.DisplayText = e.Value.ToString() == "1" ? "启用" : "禁用";
        }
    }
}
