using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Coolite.Ext.Web;
using GhtnTech.SEP.DAL;
using GhtnTech.SecurityFramework.BLL;
using System.Data;
using GhtnTech.SEP.DBUtility;

public partial class YSNewSearch_PersonSafeRecord : System.Web.UI.Page
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            BaseLoad();
            StoreLoad();
        }
    }

    private void BaseLoad()
    {
        //单位
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

    protected void PersRefresh(object sender, StoreRefreshDataEventArgs e)
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
        fb_zrr.EmptyText = q.Count() > 0 ? "请选择人员" : "没有待选人员";

    }

    private void StoreLoad()
    {
        //var data = ((from yh in dc.Nyhinput
        //            from yc in dc.Nyinhuancheck
        //            from pr in dc.Person
        //            where yh.Yhputinid == yc.Yhputinid && yc.Responsibleid==pr.Personnumber
        //            select new
        //            {
        //                yh.Maindeptid,
        //                pr.Areadeptid,
        //                pr.Personnumber,
        //                yh.Pctime,
        //                Kind = "yh"
        //            }).Union(
        //         from sw in dc.Nswinput
        //         from pr in dc.Person
        //         where sw.Swpersonid==pr.Personnumber
        //         select new
        //         {
        //             sw.Maindeptid,
        //             pr.Areadeptid,
        //             pr.Personnumber,
        //             sw.Pctime,
        //             Kind = "sw"
        //         }).Union(
        //         from z in dc.Moveplan
        //         from pr in dc.Person
        //         where z.Personid==pr.Personnumber
        //         select new
        //         {
        //             Maindeptid=z.Maindept,
        //             pr.Areadeptid,
        //             pr.Personnumber,
        //             Pctime = z.Starttime,
        //             Kind = "zd"
        //         }).Union(
        //         from g in dc.Workinjury
        //         from pr in dc.Person
        //         where g.Personnumber==pr.Personnumber
        //         select new
        //         {
        //             g.Maindeptid,
        //             pr.Areadeptid,
        //             pr.Personnumber,
        //             Pctime = g.Indate,
        //             Kind = "gs"
        //         })).ToList();
        //if (fb_zrr.SelectedIndex > -1)
        //{
        //    data = data.Where(p => p.Personnumber == fb_zrr.SelectedItem.Value).ToList();
        //}
        //if (cbbforcheckDept.SelectedIndex > -1)
        //{
        //    data = data.Where(p => p.Areadeptid == cbbforcheckDept.SelectedItem.Value).ToList();
        //}
        //data = data.Where(p => p.Pctime >= new DateTime(System.DateTime.Today.Year, 1, 1) && p.Maindeptid == SessionBox.GetUserSession().DeptNumber).ToList();

        //var gro = from g in data
        //          group g by new
        //          {
        //              g.Maindeptid,
        //              g.Areadeptid,
        //              g.Personnumber
        //          }
        //              into gg
        //              select new
        //              {
        //                  gg.Key.Maindeptid,
        //                  gg.Key.Areadeptid,
        //                  gg.Key.Personnumber,
        //                  Yh = gg.Count(p => p.Kind == "yh"),
        //                  Sw = gg.Count(p => p.Kind == "sw"),
        //                  Zd = gg.Count(p => p.Kind == "zd"),
        //                  Gs = gg.Count(p => p.Kind == "gs")
        //              };
        //var gro1 = from g in gro
        //           from d in dc.Department
        //           from pr in dc.Person
        //           where g.Areadeptid == d.Deptnumber && g.Personnumber == pr.Personnumber
        //           select new
        //           {
        //               g.Personnumber,
        //               d.Deptname,
        //               pr.Name,
        //               g.Yh,
        //               g.Sw,
        //               g.Zd,
        //               g.Gs
        //           };
        DataStore.DataSource = GetUserLoginTotal(new DateTime(System.DateTime.Today.Year, 1, 1), System.DateTime.Today, SessionBox.GetUserSession().DeptNumber, cbbforcheckDept.SelectedIndex > -1 ? cbbforcheckDept.SelectedItem.Value : "-1", fb_zrr.SelectedIndex > -1 ? fb_zrr.SelectedItem.Value : "");
        DataStore.DataBind();

    }

    [AjaxMethod]
    public void btnSearch_Click()
    {
        StoreLoad();
    }

    protected void RowClick(object sender, AjaxEventArgs e)
    {
        RowSelectionModel sm = gpEdit.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            ToolbarButton1.Disabled = false;
        }
        else
        {
            ToolbarButton1.Disabled = true;
        }
    }

    [AjaxMethod]
    public void DetailShow()
    {
        RowSelectionModel sm = gpEdit.SelectionModel.Primary as RowSelectionModel;
        string perid = sm.SelectedRow.RecordID;
        //var data =
        //    (from yh in dc.Nyhinput
        //     from yc in dc.Nyinhuancheck
        //     from y in dc.Yhbase
        //     where yh.Yhputinid == yc.Yhputinid && yh.Yhid == y.Yhid && yc.Responsibleid == perid
        //     select new
        //     {
        //         Content = y.Yhcontent,
        //         yh.Pctime,
        //         Kind = "yh"
        //     }).Union(
        //     from sw in dc.Nswinput
        //     from s in dc.Swbase
        //     where sw.Swid == s.Swid && sw.Swpersonid == perid
        //     select new
        //     {
        //         Content = s.Swcontent,
        //         sw.Pctime,
        //         Kind = "sw"
        //     }).Union(
        //     from z in dc.Moveplan
        //     from d in dc.Place
        //     where z.Personid == perid && z.Placeid==d.Placeid
        //     select new
        //     {
        //         Content=d.Placename+",走动情况:"+z.Movestate,
        //         Pctime = z.Starttime,
        //         Kind = "zd"
        //     }).Union(
        //     from g in dc.Workinjury
        //     where g.Personnumber == perid
        //     select new
        //     {
        //         Content=g.GsFact,
        //         Pctime = g.Indate,
        //         Kind = "gs"
        //     });
        //data = data.Where(p => p.Pctime >= new DateTime(System.DateTime.Today.Year, 1, 1));
        DetailStore.DataSource = GetUserLoginDetail(new DateTime(System.DateTime.Today.Year, 1, 1), System.DateTime.Today, perid);
        DetailStore.DataBind();
        DetailWindow.Show();

    }

    private DataSet GetUserLoginTotal(DateTime dateBegin, DateTime dateEnd, string maindept, string deptnm, string psnid)
    {
        string strSql = string.Format("select b.*,p.name,d.deptname,d.deptnumber,p.maindeptid from (select a.Personnumber,sum(case when a.kind='yh' then 1 else 0 end) yh,sum(case when a.kind='sw' then 1 else 0 end) sw,sum(case when a.kind='zd' then 1 else 0 end) zd,sum(case when a.kind='gs' then 1 else 0 end) gs from ((select nc.responsibleid Personnumber,ny.pctime,'yh' kind from Nyhinput ny inner join Nyinhuancheck nc on ny.yhputinid=nc.yhputinid and nc.responsibleid is not null)union all(select Swpersonid Personnumber,Pctime,'sw' kind from Nswinput)union all(select Personid Personnumber,Starttime Pctime,'zd' kind from Moveplan)union all( select Personnumber,Indate Pctime,'gs' kind from Workinjury ) ) a where a.pctime between to_date('{0}','YYYY-MM-DD') and to_date('{1}','YYYY-MM-DD') group by a.Personnumber ) b inner join person p on b.Personnumber=p.personnumber inner join department d on p.areadeptid=d.deptnumber where 1=1", dateBegin.ToShortDateString(), dateEnd.ToShortDateString());
        if (maindept != "-1")
        {
            strSql += string.Format(" and p.maindeptid='{0}'", maindept);
        }
        if (deptnm != "-1")
        {
            strSql += string.Format(" and d.deptnumber='{0}'", deptnm);
        }
        if (psnid != "")
        {
            strSql += string.Format(" and b.Personnumber='{0}'", psnid);
        }
        return OracleHelper.Query(strSql);
    }

    private DataSet GetUserLoginDetail(DateTime dateBegin, DateTime dateEnd, string psnid)
    {
        string strSql = string.Format("select a.* from ((select nc.responsibleid Personnumber,yb.yhcontent Content,ny.pctime,'隐患' kind from Nyhinput ny inner join Nyinhuancheck nc on ny.yhputinid=nc.yhputinid  inner join yhbase yb on ny.yhid=yb.yhid)union all(select Nswinput.Swpersonid Personnumber,sb.swcontent Content,Pctime,'三违' kind from Nswinput inner join Swbase sb on Nswinput.Swid=sb.swid)union all(select Moveplan.Personid Personnumber,place.placename||'走动情况:'||Moveplan.Movestate Content,Starttime Pctime,'走动情况' kind from Moveplan inner join Place on Moveplan.Placeid=Place.Placeid)union all(select Workinjury.Personnumber,Workinjury.Gs_Fact Content,Indate Pctime,'工伤信息' kind from Workinjury ) ) a  where a.pctime between to_date('{0}','YYYY-MM-DD') and to_date('{1}','YYYY-MM-DD') and a.Personnumber='{2}'", dateBegin.ToShortDateString(), dateEnd.ToShortDateString(),psnid);
        return OracleHelper.Query(strSql);
    }
}
