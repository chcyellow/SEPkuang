using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using DevExpress.Web;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxTreeList;
using GhtnTech.SEP.DBUtility;
using GhtnTech.SecurityFramework.BLL;

public partial class CodingManage_Sys_BaseInfoSet : BasePage
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
                UserHandle.InitModule(this.PageTag);
                if (UserHandle.ValidationHandle(PermissionTag.Browse))//是否有浏览权限
                {

                }
                else
                {
                    Session["ErrorNum"] = "0";
                    Response.Redirect("~/Error.aspx");
                }
            }
        }
        Session["strWhere"] = "";
        //需要初始化单位
        //StringBuilder strSql = new StringBuilder();
        //strSql.Append("select * from Department");
        //DataSet ds = OracleHelper.Query(strSql.ToString());
        //cbbPDepart.DataSource = ds;
        //cbbPDepart.TextField = "DEPTNAME";
        //cbbPDepart.ValueField = "DEPTNUMBER";
        //cbbPDepart.DataBind();
    }

    private void BindTree()
    {
        if (tbNodeName.Text.Trim() != "")
        {
            Session["strWhere"] += " and INFONAME like '%" + tbNodeName.Text + "%'";
        }
        //if (cbbPDepart.SelectedIndex > -1)
        //{
        //    Session["strWhere"] += " and PDEPART = '" + cbbPDepart.SelectedItem.Text.Trim() + "'";
        //}
        if (deDay.Value != null)
        {
            Session["strWhere"] += " and PDAY <= to_date('" + deDay.Date + "','yyyy-mm-dd hh24:mi:ss')";
        }
        if (cbbStatus.SelectedIndex > -1)
        {
            Session["strWhere"] += " and STATUS='" + cbbStatus.SelectedItem.Text + "'";
        }
        InfoTree.DataBind();
    }

    protected void backPanel_Callback(object source, CallbackEventArgsBase e)
    {
        if (e.Parameter == "Sel")
        {
            BindTree();
        }
    }

    #region 状态颜色显示
    protected void InfoTree_HtmlDataCellPrepared(object sender, TreeListHtmlDataCellEventArgs e)
    {
        if (e.Column.Caption == "状态")
        {
            string value = e.CellValue.ToString().Trim();
            if (value == "编辑")
                e.Cell.ForeColor = Color.Blue;
            if(value == "启用")
                e.Cell.ForeColor = Color.Red;
            if (value == "禁用")
                e.Cell.ForeColor = Color.Cornsilk;
        }
    }
    #endregion
}
