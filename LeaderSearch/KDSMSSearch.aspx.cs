using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using Coolite.Ext.Web;
using GhtnTech.SEP.DAL;
using GhtnTech.SecurityFramework.BLL;

public partial class LeaderSearch_KDSMSSearch : System.Web.UI.Page
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            dfBegin.SelectedDate = System.DateTime.Today.AddDays(1 - System.DateTime.Today.Day);
            dfEnd.SelectedDate = System.DateTime.Today;

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
                   where t.Destaddr == p.Tel
                   && t.Subtime >= dfBegin.SelectedDate && t.Subtime <= dfEnd.SelectedDate
                   && p.Maindeptid == SessionBox.GetUserSession().DeptNumber
                   select new
                   {
                       t.Smsid,
                       t.SmContent,
                       t.Sendtime,
                       p.Name
                   };
        DetailStore.DataSource = data.OrderByDescending(p => p.Sendtime);
        DetailStore.DataBind();
    }

}