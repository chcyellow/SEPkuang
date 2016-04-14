using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using System.Data;
using GhtnTech.SEP.DAL;
using GhtnTech.SecurityFramework.BLL;

public partial class BaseManage_PersonW : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["maindeptid"] = SessionBox.GetUserSession().DeptNumber;
        Session["PosDept"] = SessionBox.GetUserSession().DeptNumber;
    }
    protected void ASPxButton1_Click(object sender, EventArgs e)
    {
            GridView.SettingsText.Title = "外委人员名称表";
            ASPxGridViewExporter1.WriteXlsToResponse("外委人员名称表");
    }
}
