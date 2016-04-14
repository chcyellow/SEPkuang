using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GhtnTech.SecurityFramework.BLL;
public partial class SafeCheckSet_PostFSWSet : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!SessionBox.CheckUserSession())
        {
            Response.Redirect("~/Login.aspx");
        }
        else
        {
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
                adsPosition.Where = "Maindeptid == \"" + SessionBox.GetUserSession().DeptNumber + "\"";
                adsPFSWSet.Where = "Maindeptid == \"" + SessionBox.GetUserSession().DeptNumber + "\"";
            }
        }
    }
    protected void gvPFSWSet_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
    {

        e.NewValues["Maindeptid"] = SessionBox.GetUserSession().DeptNumber;
    }
}
