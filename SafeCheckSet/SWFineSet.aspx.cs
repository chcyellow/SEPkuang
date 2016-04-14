using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GhtnTech.SecurityFramework.BLL;
public partial class SafeCheckSet_SWFineSet : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!SessionBox.CheckUserSession())
        {
            Response.Redirect("~/Login.aspx");
        }
        else
        {
            adsSWFineSet.Where = "Deptnumber ==\"" + SessionBox.GetUserSession().DeptNumber + "\"";
            adsPersonType.Where = "Deptnumber ==\"" + SessionBox.GetUserSession().DeptNumber + "\"";
        }
        //adsPosition.Where = "Maindeptid == \"" + SessionBox.GetUserSession().DeptNumber + "\"";
        //adsDept.Where = "Deptnumber.StartsWith(\"" + SessionBox.GetUserSession().DeptNumber.Remove(4) + "\")";
    }
    protected void gvSWFineSet_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
    {
        e.NewValues["Deptnumber"] = SessionBox.GetUserSession().DeptNumber;
        e.NewValues["Usingtime"] = DateTime.Now;
    }
    protected void gvSWFineSet_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
    {
        e.NewValues["Usingtime"] = DateTime.Now;
    }
}
