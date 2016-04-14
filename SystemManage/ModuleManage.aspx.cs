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
public partial class SystemManage_ModuleManage : BasePage
{
    Module mbll = new Module();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Page_PreRenderComplete(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cboModuleGroup.Items.Insert(0, new DevExpress.Web.ASPxEditors.ListEditItem("--全部--", -1));
            cboModuleGroup.SelectedIndex = 0;
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
                    GridViewCommandColumn colDel = (GridViewCommandColumn)ASPxGridView1.Columns["删除"];
                    GridViewDataHyperLinkColumn colEdit = (GridViewDataHyperLinkColumn)ASPxGridView1.Columns["编辑"];
                    if (!UserHandle.ValidationHandle(PermissionTag.Add))
                    {
                        btnAddModule.Visible = false;
                    }
                    if (!UserHandle.ValidationHandle(PermissionTag.Edit))
                    {
                        colEdit.Visible = false;
                    }
                    if (!UserHandle.ValidationHandle(PermissionTag.Delete))
                    {
                        colDel.Visible = false;
                    }
                }
            }
        }

    }
    protected void btnAddModule_Click(object sender, EventArgs e)
    {
        JSHelper.OpenWebFormSize("EditModule.aspx", 700, 680, 10, 200, this);
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindModule();
    }
    protected void BindModule()
    {
        string where = "1=1";
        if(cboModuleGroup.SelectedIndex != -1)
        {
            if (cboModuleGroup.SelectedIndex != 0)
            {
                where += " and modulegroupid=" + cboModuleGroup.SelectedItem.Value;
            }
        }
        if (txtModuleName.Text.Trim() != "")
        {
            where += " and modulename like'%" + txtModuleName.Text.Trim() + "%'";
        }
        if (txtModuleTag.Text.Trim() != "")
        {
            where += " and moduletag like'%" + txtModuleTag.Text.Trim() + "%'";
        }
        //var ds = mbll.GetModuleList(where);
        odsModule.SelectParameters["strWhere"].DefaultValue = where;
        ASPxGridView1.DataSourceID = odsModule.ID;
        ASPxGridView1.DataBind();
        ASPxGridView1.KeyFieldName = "MODULEID";

    }
    protected void ASPxGridView1_CustomColumnDisplayText(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewColumnDisplayTextEventArgs e)
    {
        if (e.Column.FieldName == "STATUS")
        {
            e.DisplayText = e.Value.ToString()=="1" ? "启用" : "禁用";
        }
    }
}
