using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Xsl;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Coolite.Ext.Web;
using GhtnTech.SEP.DAL;
using GhtnTech.SecurityFramework.BLL;

public partial class LeaderSearch_MPcondition : BasePage
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
       Bind();
    }
    DBSCMDataContext db = new DBSCMDataContext();
    private void Bind()
    {
        var query = from a in db.ViewMoveplan
                      where 
                      a.Starttime >= DateTime.Parse(Request["begin"].Trim())
                      && a.Starttime <= DateTime.Parse(Request["end"].Trim())
                      && (a.Movestate.Trim() == "已走动" ? 1 : 0) == int.Parse(this.Request["status"].Trim())
                      select new
                      {
                          a.Id,
                          a.Deptid,
                          a.Pareasid,
                          a.Placeid,
                          a.Personid,
                          a.Name,
                          a.Deptname,
                          a.Placename,
                          a.Starttime,
                          a.Endtime,
                          a.Movestarttime,
                          a.Moveendtime,
                          a.Movestate,
                          a.Maindeptid,
                          a.Maindeptname
                      };
        if (!SessionBox.GetUserSession().rolelevel.Contains("0") && !SessionBox.GetUserSession().rolelevel.Contains("1"))
        {
            query = query.Where(p => (p.Maindeptid == SessionBox.GetUserSession().DeptNumber));
        }
        if (!string.IsNullOrEmpty(Request["DeptID"]))
        {
            query = query.Where(p => (p.Deptid == this.Request["DeptID"].Trim()));
        }
        if (!string.IsNullOrEmpty(Request["MainDeptID"]))
        {
            query = query.Where(p => (p.Maindeptid == this.Request["MainDeptID"].Trim()));
        }
        if (!string.IsNullOrEmpty(Request["PAreasID"]))
        {
            query = query.Where(p => p.Pareasid== int.Parse(this.Request["PAreasID"].Trim()));
        }
        if (!string.IsNullOrEmpty(Request["PlaceID"]))
        {
            query = query.Where(p => p.Placeid == int.Parse(this.Request["PlaceID"].Trim()));
        }
        Store1.DataSource = query;
        Store1.DataBind();
    }

}
