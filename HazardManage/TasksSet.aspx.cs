using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OracleClient;
using System.Xml;
using DevExpress.Web.ASPxTreeList;
using DevExpress.Web.ASPxEditors;
using GhtnTech.SecurityFramework;
using GhtnTech.SecurityFramework.BLL;
using GhtnTech.SecurityFramework.Utility;
using DevExpress.Web.ASPxGridView;
using GhtnTech.SEP.DBUtility;
public partial class HazardManage_TasksSet : BasePage
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
                    GridViewCommandColumn colEdit = (GridViewCommandColumn)ASPxGridView2.Columns["操作"];
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
            //bind();
        }
        bind();

    }
    protected void ASPxCallbackPanel2_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
    {
        string key = e.Parameter.Trim();
        Session["zyID"] = key;
        TreeListNode node = treeList.FindNodeByKeyValue(key);
        if (node.HasChildren)
        {
            ASPxGridView2.Visible = false;
            return;
        }
        ObjectDataSource1.SelectParameters["strWhere"].DefaultValue = "and PROFESSIONALID = " + key + "";
        ASPxGridView2.Visible = true;

    }
    //绑定专业
    private void bind()
    {
        int zyID = int.Parse(PublicMethod.ReadXmlReturnNode("ZY", this));
        //string oracletext = "select * from CS_BASEINFOSET where INFOID!= " + zyID + " start with INFOID= " + zyID + "  connect by prior INFOID = FID order by INFOID asc ";
        string oracletext = "select * from CS_BASEINFOSET start with INFOID= " + zyID + "  connect by prior INFOID = FID order by INFOID asc ";
        treeList.DataSource = OracleHelper.Query(oracletext);
        treeList.DataBind();
    }
}
