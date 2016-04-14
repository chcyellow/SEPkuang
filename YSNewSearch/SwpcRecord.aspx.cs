using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using Coolite.Ext.Web;
using GhtnTech.SEP.DAL;
using GhtnTech.SecurityFramework.BLL;

public partial class YSNewSearch_SwpcRecord : System.Web.UI.Page
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            StoreLoad();
            var dept = from d in dc.Department
                       where d.Deptnumber.Substring(0, 4) == SessionBox.GetUserSession().DeptNumber.Substring(0, 4)
                       && d.Deptlevel == "正科级" && d.Deptstatus == "1"
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

    protected void PersRefresh(object sender, StoreRefreshDataEventArgs e)//发布/提交选择责任部门人员刷新
    {
        var q = from p in dc.Person
                where p.Areadeptid == cbbforcheckDept.SelectedItem.Value
                orderby p.Name
                select new
                {
                    p.Personnumber,
                    p.Name
                };
        PersStore.DataSource = q;
        PersStore.DataBind();
        fb_zrr.Disabled = q.Count() > 0 ? false : true;
        fb_zrr.EmptyText = q.Count() > 0 ? "请选择排查人员" : "没有待选人员";

    }

    private void StoreLoad()
    {
        var data = (from sw in dc.Nswinput
                   from per in dc.Person
                   from dep in dc.Department
                   where sw.Pcpersonid == per.Personnumber && per.Areadeptid == dep.Deptnumber
                   && sw.Pctime >= System.DateTime.Today.AddDays(1 - System.DateTime.Today.Day) && sw.Pctime <= System.DateTime.Today
                   && sw.Maindeptid == SessionBox.GetUserSession().DeptNumber && sw.Isend == 1
                   select new
                   {
                       per.Personnumber,
                       per.Name,
                       dep.Deptnumber,
                       dep.Deptname,
                   }).ToList();
        if (cbbforcheckDept.SelectedIndex > -1)
        {
            data = data.Where(p => p.Deptnumber == cbbforcheckDept.SelectedItem.Value).ToList();
        }
        if (fb_zrr.SelectedIndex > -1)
        {
            data = data.Where(p => p.Personnumber == fb_zrr.SelectedItem.Value).ToList();
        }
        var fsw = (from per in dc.Person
                  join f in dc.Positionfswset.Where(p => p.Maindeptid == SessionBox.GetUserSession().DeptNumber) on per.Posid equals f.Positionid into gg
                  where per.Maindeptid == SessionBox.GetUserSession().DeptNumber
                  from g in gg.DefaultIfEmpty()
                  select new
                  {
                      per.Personnumber,
                      Fsw = g == null ? "未设定" : (g.Count + "/" + g.Month)
                  }).ToList();
       
        var group1 = from p in data
                    group p by new
                    {
                        p.Deptname,
                        p.Personnumber,
                        p.Name
                    }
                        into g
                        select new
                        {
                            g.Key.Deptname,
                            g.Key.Name,
                            g.Key.Personnumber,
                            Count = g.Count()
                        };

        var pf = from p in group1
                 from f in fsw
                 where p.Personnumber == f.Personnumber
                 select new
                 {
                     p.Personnumber,
                     p.Name,
                     p.Deptname,
                     p.Count,
                     f.Fsw
                 };
        if (cbb_kind.SelectedIndex == 1)
        {
            pf = pf.Where(p => p.Fsw != "未设定");//过滤掉所有未设定人员--111012
        }
        SWStore.DataSource = pf;
        SWStore.DataBind();
    }

    [AjaxMethod]
    public void btnSearch_Click()
    {
        StoreLoad();
    }
}
