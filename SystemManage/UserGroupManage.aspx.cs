using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxTreeList;
using GhtnTech.SecurityFramework;
using GhtnTech.SecurityFramework.BLL;
using GhtnTech.SecurityFramework.Utility;
public partial class SystemManage_UserGroupManage : BasePage
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
                    TreeListCommandColumn colEdit = (TreeListCommandColumn)treeUserGroup.Columns["操作"];
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
    
    protected string GetNodeGlyph(TreeListDataCellTemplateContainer container)
    {
        string fmt = "~/Images/Demo/{0}.png";
        if (container.NodeKey == null)
            return string.Format(fmt, "usergroup32");
        //string name = container.GetValue(FileManagerHelper.FullPathName).ToString();
        //if (Directory.Exists(name))
        //{
        if (container.Expandable && container.Expanded)
            return string.Format(fmt, "usergroup");
        return string.Format(fmt, "usergroup32");
        //}

    }
}
