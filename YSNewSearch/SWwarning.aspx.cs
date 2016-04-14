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

public partial class YSNewSearch_SWwarning : System.Web.UI.Page
{

    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            StoreLoad();
            DeptStore.DataSource = PublicCode.GetKQdept(SessionBox.GetUserSession().DeptNumber);
            DeptStore.DataBind();
            //获取三违预警设置
            //hdnScore.SetValue(PublicCode.GetSWMaxScoreSet(SessionBox.GetUserSession().DeptNumber));
            hdnScore.Value = PublicCode.GetSWMaxScoreSet(SessionBox.GetUserSession().DeptNumber).ToString();
            hdnCount.Value = PublicCode.GetSWMaxCountSet(SessionBox.GetUserSession().DeptNumber).ToString();

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
        fb_zrr.EmptyText = q.Count() > 0 ? "请选择三违人员" : "没有待选人员";

    }

    private void StoreLoad()
    {
        //var data = from sw in dc.Nswinput
        //           from per in dc.Person
        //           from dep in dc.Department
        //           from sb in dc.Swbase
        //           from s in dc.Swscore
        //           where sw.Swpersonid == per.Personnumber && per.Areadeptid == dep.Deptnumber && sw.Swid == sb.Swid && sb.Levelid == s.Levelid
        //           && sw.Pctime >= DateTime.Parse(System.DateTime.Today.Year + "-01-01") && sw.Pctime <= DateTime.Parse(System.DateTime.Today.Year + "-12-31")
        //           && sw.Maindeptid == SessionBox.GetUserSession().DeptNumber && sw.Isend==1
        //           select new
        //           {
        //               per.Personnumber,
        //               per.Name,
        //               dep.Deptnumber,
        //               dep.Deptname,
        //               s.Kcscore
        //           };
        //if (cbbforcheckDept.SelectedIndex > -1)
        //{
        //    data = data.Where(p => p.Deptnumber == cbbforcheckDept.SelectedItem.Value);
        //}
        //if (fb_zrr.SelectedIndex > -1)
        //{
        //    data = data.Where(p => p.Personnumber == fb_zrr.SelectedItem.Value);
        //}
        //var group = from p in data
        //            group p by new
        //            {
        //                p.Deptname,
        //                p.Name
        //            }
        //                into g
        //                select new
        //                {
        //                    g.Key.Deptname,
        //                    g.Key.Name,
        //                    Score = g.Sum(p => p.Kcscore),
        //                    Count = g.Count()
        //                };
        //SWStore.DataSource = group.Where(p => p.Score > 11 || p.Count > 1);
        //SWStore.DataBind();

        DataSet ds = GetKaoHeInfo.GetPersonSWPoint(DateTime.Parse(System.DateTime.Today.Year + "-01-01"), DateTime.Parse(System.DateTime.Today.Year + "-12-31"), SessionBox.GetUserSession().DeptNumber, "");
        System.Data.DataView dv = new System.Data.DataView(ds.Tables[0]);
        string filter = "";
        if (cbbforcheckDept.SelectedIndex > -1)
        {
            filter += "DEPTNUMBER='" + cbbforcheckDept.SelectedItem.Value + "'";
        }
        if (fb_zrr.SelectedItem.Value!="")
        {
            if (filter != "")
            {
                filter += " and ";
            }
            filter += "PERSONNUMBER='" + fb_zrr.SelectedItem.Value + "'";
        }
        dv.RowFilter = filter;
        ds.Tables.Clear();
        ds.Tables.Add(dv.ToTable());
        SWStore.DataSource = ds;
        SWStore.DataBind();
    }

    [AjaxMethod]
    public void btnSearch_Click()
    {
        StoreLoad();
    }

    [AjaxMethod]
    public void DetailLoad()
    {
        RowSelectionModel sm = this.gpEdit.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            string url = "";
            Window1.Width = 890;
            Window1.Height = 400;
            Window1.Title = "三违明细信息";
            url = string.Format("../LeaderSearch/SWcondition.aspx?begin={0}&end={1}&SWperson={2}", DateTime.Parse(System.DateTime.Today.Year + "-01-01"), DateTime.Parse(System.DateTime.Today.Year + "-12-31"), sm.SelectedRow.RecordID);
            Ext.DoScript("#{Window1}.load('" + url + "');");
            Window1.Show();
        }
    }
}
