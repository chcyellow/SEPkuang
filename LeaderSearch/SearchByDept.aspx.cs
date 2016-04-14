using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GhtnTech.SEP.DAL;
using GhtnTech.SecurityFramework.BLL;
using Coolite.Ext.Web;
using System.Data;
public partial class LeaderSearch_SearchByDept : BasePage
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_PreRenderComplete(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cbbUnit.Items.Insert(0, new Coolite.Ext.Web.ListItem("--全部--", "-1"));
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request["head"]))
            {
                GridPanel1.TopBar[0].Visible = false;
                if (Request["head"].Trim() == "0")
                {
                    GridPanel1.Header = false;
                    GridPanel1.Height = 160;
                }
                if (Request["head"].Trim() == "1")
                {
                    GridPanel1.Height = 300;
                }
                Window1.Listeners.BeforeShow.Fn = "function(el) { el.setHeight(Ext.getBody().getViewSize().height-20);el.setWidth(Ext.getBody().getViewSize().width-20); }";
            }
            dfBegin.SelectedDate = System.DateTime.Today.AddDays(1 - System.DateTime.Today.Day);
            dfEnd.SelectedDate = System.DateTime.Today;
            dfBegin.MaxDate = System.DateTime.Today;
            dfEnd.MaxDate = System.DateTime.Today;
            #region 初始化单位
            DBSCMDataContext dc = new DBSCMDataContext();
            if (SessionBox.GetUserSession().rolelevel.Contains("1") || SessionBox.GetUserSession().rolelevel.Contains("0"))
            {
                var dept = from d in dc.Department
                           where d.Deptnumber.Substring(4) == "00000" && d.Deptname.EndsWith("矿")
                           select new
                           {
                               d.Deptname,
                               Deptid = d.Deptnumber
                           };
                UnitStore.DataSource = dept;
                UnitStore.DataBind();
                cbbUnit.SelectedItem.Value = "-1";
                cbbUnit.Disabled = false;

            }
            else
            {
                var dept = from d in dc.Department
                           where d.Deptnumber == SessionBox.GetUserSession().DeptNumber
                           select new
                           {
                               d.Deptname,
                               Deptid = d.Deptnumber
                           };
                UnitStore.DataSource = dept;
                UnitStore.DataBind();
                cbbUnit.SelectedItem.Value = SessionBox.GetUserSession().DeptNumber;
                cbbUnit.Disabled = true;
            }
            #endregion
            storebind();

        }
    }

    protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)
    {
        storebind();
    }

    [AjaxMethod]
    public void storebind()
    {
        if (dfBegin.SelectedDate > dfEnd.SelectedDate)
        {
            Ext.Msg.Alert("提示", "请选择正确日期").Show();
            return;
        }
       // DataSet ds = new DataSet();
        //var data = ds;
        //var datayh = (from sw in dc.Getyhinput
        //            where
        //            sw.Pctime >= dfBegin.SelectedDate && sw.Pctime <= dfEnd.SelectedDate
        //            select sw).ToList();
        //var groupyh = from sw in datayh
        //            group sw by new{ 
        //                sw.Deptid,
        //                sw.Deptname,
        //                sw.Unitid
        //            }
        //                into g
        //            select new
        //            {
        //                g.Key.Deptid,
        //                g.Key.Deptname,
        //                g.Key.Unitid,
        //                Total = g.Count(),
        //                Pass = g.Count(p => p.Status == "复查通过"),
        //                NPass = g.Count(p => p.Status != "复查通过")
        //            };
        //var groupsw= from sw in dc.Nswinput
        //             group sw by sw.
        if (SessionBox.GetUserSession().rolelevel.Contains("1") || SessionBox.GetUserSession().rolelevel.Contains("0"))
        {
            if (cbbUnit.SelectedIndex > -1 && cbbUnit.SelectedItem.Value!="-1")
            {
                var data = GetSafeInfo.GetAllSafetyCountByDept(cbbUnit.SelectedItem.Value, dfBegin.SelectedDate, dfEnd.SelectedDate);
                Store1.DataSource = data;
                Store1.DataBind();
            }
            else
            {
                var data = GetSafeInfo.GetAllSafetyCountByDept("", dfBegin.SelectedDate, dfEnd.SelectedDate);
                Store1.DataSource = data;
                Store1.DataBind();
            }
        }
        else
        {
            var data = GetSafeInfo.GetAllSafetyCountByDept(SessionBox.GetUserSession().DeptNumber, dfBegin.SelectedDate, dfEnd.SelectedDate);
            Store1.DataSource = data;
            Store1.DataBind();
        }
        
    }

    private void baseSearch(string maindept, DateTime begindate, DateTime enddate)
    {
        var yhdata = (from yh in dc.Getyhinput
                     where yh.Pctime >= begindate && yh.Pctime <= enddate
                     group yh by new
                     {
                         yh.Unitid,
                         yh.Deptid
                     }
                         into g
                         select new
                         {
                             g.Key.Unitid,
                             g.Key.Deptid,
                             YHALL = g.Count(),
                             YHYZG = g.Count(p => p.Status == "复查通过" || p.Status == "现场整改"),
                             YHWZG = g.Count(p => p.Status != "复查通过" && p.Status != "现场整改")
                         }).ToList();
        var swdata = (from yh in
                         (from sw in dc.Nswinput
                          from p in dc.Person
                          where sw.Pctime >= begindate && sw.Pctime <= enddate && sw.Swpersonid == p.Personnumber
                          select new
                          {
                              Unitid = sw.Maindeptid,
                              Deptid = p.Areadeptid
                          }
                          )
                     group yh by new
                       {
                           yh.Unitid,
                           yh.Deptid
                       }
                         into g
                         select new
                         {
                             g.Key.Unitid,
                             g.Key.Deptid,
                             SWALL = g.Count()
                         }).ToList();
        var zddata = (from yh in
                         (from mp in dc.Moveplan
                          from p in dc.Person
                          where mp.Starttime >= begindate && mp.Starttime <= enddate && mp.Personid == p.Personnumber
                          select new
                                 {
                                     Unitid = mp.Maindept,
                                     Deptid = p.Areadeptid,
                                     mp.Movestate
                                 }
                          )
                     group yh by new
                         {
                             yh.Unitid,
                             yh.Deptid
                         }
                         into g
                         select new
                         {
                             g.Key.Unitid,
                             g.Key.Deptid,
                             YZD = g.Count(p => p.Movestate == "已走动"),
                             WZD = g.Count(p => p.Movestate != "已走动")
                         }).ToList();
        var group2 = (
            (from y in yhdata
             select new
             {
                 y.Unitid,
                 y.Deptid
             }).Union(
                  from y in swdata
                  select new
                  {
                      y.Unitid,
                      y.Deptid
                  }).Union(
                  from y in zddata
                  select new
                  {
                      y.Unitid,
                      y.Deptid
                  })).Distinct();
        var group1 = (from g in group2
                     from d in dc.Department
                     from d1 in dc.Department
                     where g.Unitid == d.Deptnumber && g.Deptid == d1.Deptnumber
                     select new
                     {
                         DEPTID = g.Deptid,
                         MAINDEPTID = g.Unitid,
                         MAINDEPTNAME = d.Deptname,
                         DEPTNAME = d1.Deptname
                     }).ToList();
        var data = from g in group1
                   join yh in yhdata on g.DEPTID equals yh.Deptid into gyh
                   from gg in gyh.DefaultIfEmpty()
                   select new
                   {
                       g.DEPTID,
                       g.MAINDEPTID,
                       g.MAINDEPTNAME,
                       g.DEPTNAME,
                       YHALL = gg == null ? 0 : gg.YHALL,
                       YHYZG = gg == null ? 0 : gg.YHYZG,
                       YHWZG = gg == null ? 0 : gg.YHWZG,
                   };
        var data1 = from g in data
                   join yh in swdata on g.DEPTID equals yh.Deptid into gyh
                   from gg in gyh.DefaultIfEmpty()
                   select new
                   {
                       g.DEPTID,
                       g.MAINDEPTID,
                       g.MAINDEPTNAME,
                       g.DEPTNAME,
                       g.YHALL ,
                       g.YHYZG,
                       g.YHWZG,
                       SWALL = gg == null ? 0 : gg.SWALL
                   };
        var data2 = from g in data1
                    join yh in zddata on g.DEPTID equals yh.Deptid into gyh
                    from gg in gyh.DefaultIfEmpty()
                    select new
                    {
                        g.DEPTID,
                        g.MAINDEPTID,
                        g.MAINDEPTNAME,
                        g.DEPTNAME,
                        g.YHALL,
                        g.YHYZG,
                        g.YHWZG,
                        g.SWALL,
                        YZD = gg == null ? 0 : gg.YZD,
                        WZD = gg == null ? 0 : gg.WZD
                    };
        if (maindept != "")
        {
            data2 = data2.Where(p => p.MAINDEPTID == maindept);
        }
        Store1.DataSource = data2;
        Store1.DataBind();


    }

    protected void Cell_Click(object sender, AjaxEventArgs e)
    {
        CellSelectionModel sm = this.GridPanel1.SelectionModel.Primary as CellSelectionModel;
        if (sm.SelectedCell.ColIndex == 0 || sm.SelectedCell.Value.Trim() == "0" || sm.SelectedCell.ColIndex == 1)
            return;
        Window1.Title = dc.Department.First(p => p.Deptnumber == sm.SelectedCell.RecordID).Deptname.Trim() + "---";
        string url = "";
        switch (sm.SelectedCell.Name.Trim())
        {
            case "YHALL":
                Window1.Width = 840;
                Window1.Height = 422;
                Window1.Title += "所有隐患";
                url = string.Format("YHcondition.aspx?DeptID={0}&begin={1}&end={2}", sm.SelectedCell.RecordID.Trim(), dfBegin.SelectedDate.ToString("yyyy-MM-dd"), dfEnd.SelectedDate.ToString("yyyy-MM-dd"));
                break;
            case "YHYZG":
                Window1.Width = 840;
                Window1.Height = 422;
                Window1.Title += "已闭合隐患";
                url = string.Format("YHcondition.aspx?DeptID={0}&begin={1}&end={2}&status={3}", sm.SelectedCell.RecordID.Trim(), dfBegin.SelectedDate.ToString("yyyy-MM-dd"), dfEnd.SelectedDate.ToString("yyyy-MM-dd"), "1");
                break;
            case "YHWZG":
                Window1.Width = 840;
                Window1.Height = 422;
                Window1.Title += "未闭合隐患";
                url = string.Format("YHcondition.aspx?DeptID={0}&begin={1}&end={2}&status={3}", sm.SelectedCell.RecordID.Trim(), dfBegin.SelectedDate.ToString("yyyy-MM-dd"), dfEnd.SelectedDate.ToString("yyyy-MM-dd"), "0");
                break;
            case "SWALL":
                Window1.Width = 890;
                Window1.Height = 400;
                Window1.Title += "三违信息";
                url = string.Format("SWcondition.aspx?DeptID={0}&begin={1}&end={2}", sm.SelectedCell.RecordID.Trim(), dfBegin.SelectedDate.ToString("yyyy-MM-dd"), dfEnd.SelectedDate.ToString("yyyy-MM-dd"));
                break;
            case "YZD":
                Window1.Width = 840;
                Window1.Height = 422;
                Window1.Title += "已走动信息";
                url = string.Format("MPcondition.aspx?DeptID={0}&begin={1}&end={2}&status={3}", sm.SelectedCell.RecordID.Trim(), dfBegin.SelectedDate.ToString("yyyy-MM-dd"), dfEnd.SelectedDate.ToString("yyyy-MM-dd"), "1");
                break;
            case "WZD":
                Window1.Width = 840;
                Window1.Height = 422;
                Window1.Title += "未走动信息";
                url = string.Format("MPcondition.aspx?DeptID={0}&begin={1}&end={2}&status={3}", sm.SelectedCell.RecordID.Trim(), dfBegin.SelectedDate.ToString("yyyy-MM-dd"), dfEnd.SelectedDate.ToString("yyyy-MM-dd"), "0");
                break;
        }
        //url=Server.HtmlEncode(url);
        Ext.DoScript("#{Window1}.load('" + url + "');");
        Window1.Show();
    }

}
