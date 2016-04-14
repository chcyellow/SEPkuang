using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GhtnTech.SEP.DAL;

public partial class SystemNotice_FileDown : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            DBSCMDataContext dc = new DBSCMDataContext();
            var data = dc.Sysnotice.Single(p => p.Nid == int.Parse(Request["Nid"]));
            string strPhyPath = Server.MapPath(data.Nfileaddress.Trim());
            PublicMethod.FileDown(this, strPhyPath, data.Nfilename);
            // Response.Redirect(data.AnnexUrl.Trim());
        }
    }
}
