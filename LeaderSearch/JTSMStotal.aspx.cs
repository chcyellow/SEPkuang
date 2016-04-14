using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Coolite.Ext.Web;
using GhtnTech.SEP.DAL;

public partial class LeaderSearch_JTSMStotal : System.Web.UI.Page
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            dfBegin.SelectedDate = System.DateTime.Today.AddDays(1 - System.DateTime.Today.Day);
            dfEnd.SelectedDate = System.DateTime.Today;

            #region 初始化单位
            //KQStore.DataSource = PublicCode.GetMaindept("");
            //KQStore.DataBind();
            #endregion
            LoadData();
        }
    }

    [AjaxMethod]
    public void LoadData()
    {
        if (dfBegin.SelectedDate > dfEnd.SelectedDate)
        {
            Ext.Msg.Alert("提示", "日期选择有误!").Show();
            return;
        }
        var data = from t in dc.TblSmsendtask
                   from p in dc.Person
                   from m in dc.Department
                   where t.Destaddr == p.Tel && p.Maindeptid == m.Deptnumber
                   && t.Subtime >= dfBegin.SelectedDate && t.Subtime <= dfEnd.SelectedDate
                   select new
                   {
                       t.Smsid,
                       m.Deptnumber,
                       m.Deptname
                   };
        //if (cbbKQ.SelectedIndex > -1)
        //{
        //    data = data.Where(p => p.Deptnumber == cbbKQ.SelectedItem.Value);
        //}
        Store1.DataSource = from d in data
                     group d by new
                     {
                         d.Deptnumber,
                         d.Deptname
                     } into g
                     select new
                     {
                         g.Key.Deptnumber,
                         g.Key.Deptname,
                         SMScount = g.Count()
                     };
        Store1.DataBind();
    }

    protected void Cell_Click(object sender, AjaxEventArgs e)
    {
        if (dfBegin.SelectedDate > dfEnd.SelectedDate)
        {
            Ext.Msg.Alert("提示", "日期选择有误!").Show();
            return;
        }
        CellSelectionModel sm = this.GridPanel1.SelectionModel.Primary as CellSelectionModel;
        if (sm.SelectedCell.Value.Trim() == "0")
            return;
        if (sm.SelectedCell.Name.Trim() == "SMScount")
        {
            var data = from t in dc.TblSmsendtask
                       from p in dc.Person
                       where t.Destaddr == p.Tel
                       && t.Subtime >= dfBegin.SelectedDate && t.Subtime <= dfEnd.SelectedDate
                       && p.Maindeptid == sm.SelectedCell.RecordID.Trim()
                       select new
                       {
                           t.Smsid,
                           t.SmContent,
                           t.Sendtime,
                           p.Name
                       };
            DetailStore.DataSource = data.OrderByDescending(p=>p.Sendtime);
            DetailStore.DataBind();

            DetailWindow.Show();
        }
    }
}