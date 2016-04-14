using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Coolite.Ext.Web;
using GhtnTech.SEP.DAL;
using GhtnTech.SecurityFramework.BLL;

public partial class YSNewSearch_SafeSearchDept : System.Web.UI.Page
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            StoreLoad();
            var dept = from d in dc.Department
                       where d.Deptnumber.Substring(0, 4) == SessionBox.GetUserSession().DeptNumber.Substring(0, 4)
                       && d.Deptlevel == "正科级"
                       orderby d.Deptname
                       select new
                       {
                           d.Deptnumber,
                           d.Deptname
                       };
            DeptStore.DataSource = dept;
            DeptStore.DataBind();
        }
    }

    private void StoreLoad()
    {
        var data = (from r in dc.ParResult
                   from k in dc.ParKind
                   from d in dc.Department
                   from d2 in dc.Department
                   where r.Pkindid == k.Pkindid && r.Checkdept == d.Deptnumber && r.Checkfordept == d2.Deptnumber
                   && r.Checkdate >= System.DateTime.Today.AddDays(1 - System.DateTime.Today.Day) && r.Checkdate <= System.DateTime.Today
                   && k.Usingdept == SessionBox.GetUserSession().DeptNumber
                   select new
                   {
                       d2.Deptnumber,
                       d2.Deptname,
                       r.Total
                   }).ToList();
        if (cbbforcheckDept.SelectedIndex > -1)
        {
            data = data.Where(p => p.Deptnumber == cbbforcheckDept.SelectedItem.Value).ToList();
        }
        var group1 = from p in data
                    group p by new
                    {
                        p.Deptname,
                    }
                        into g
                        select new
                        {
                            g.Key.Deptname,
                            Yx = g.Count(p => p.Total >= 90),
                            Hg = g.Count(p => p.Total >= 70 && p.Total < 90),
                            Bhg = g.Count(p => p.Total < 70),
                            Total=g.Count()
                        };
        var group = from g in group1
                    select new
                    {
                        g.Deptname,
                        g.Yx,
                        g.Hg,
                        g.Bhg,
                        Yxrate = ((int)(g.Yx * 10000 / g.Total)) / 100,
                        Hgrate = ((int)((g.Hg + g.Yx) * 10000 / g.Total)) / 100
                    };
        SWStore.DataSource = group;
        SWStore.DataBind();
    }

    [AjaxMethod]
    public void btnSearch_Click()
    {
        StoreLoad();
    }
}
