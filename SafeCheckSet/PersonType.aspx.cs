using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GhtnTech.SecurityFramework.BLL;
public partial class SafeCheckSet_PersonType : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!SessionBox.CheckUserSession())
        {
            Response.Redirect("~/Login.aspx");
        }
        else
        {
            adsPersonType.Where = "Deptnumber ==\"" + SessionBox.GetUserSession().DeptNumber + "\"";
        }
    }
    protected void gvPersonType_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
    {
        e.NewValues["Deptnumber"] = SessionBox.GetUserSession().DeptNumber;
    }
}
