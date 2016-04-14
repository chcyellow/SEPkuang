using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Coolite.Ext.Web;
using GhtnTech.SecurityFramework.BLL;
using GhtnTech.SEP.DAL;
using System.Data;

public partial class Positioning_PositioningSearch : System.Web.UI.Page
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            #region 初始化单位
            MainDeptStore.DataSource = PublicCode.GetMaindept("");
            MainDeptStore.DataBind();
            
            if (SessionBox.GetUserSession().rolelevel.Contains("1") || SessionBox.GetUserSession().rolelevel.Contains("0"))
            {
                cbbKQ.Disabled = true;
                cbbPerson.Disabled = true;
            }
            else
            {
                cbbMianDept.SelectedItem.Value = SessionBox.GetUserSession().DeptNumber;
                cbbMianDept.Disabled = true;
                KQStore.DataSource = PublicCode.GetKQdept(SessionBox.GetUserSession().DeptNumber);
                KQStore.DataBind();
                cbbPerson.Disabled = true;
            }
            #endregion
        }
    }

    protected void PersRefresh(object sender, StoreRefreshDataEventArgs e)
    {
        if (cbbKQ.SelectedIndex > -1)
        {
            var q = from p in dc.Person
                    where p.Areadeptid == cbbKQ.SelectedItem.Value
                    select new
                    {
                        p.Personnumber,
                        p.Name
                    };
            PersStore.DataSource = q;
            PersStore.DataBind();
            cbbPerson.Disabled = q.Count() > 0 ? false : true;
        }
    }
    protected void KQRefresh(object sender, StoreRefreshDataEventArgs e)
    {
        if (cbbMianDept.SelectedIndex > -1)
        {
            KQStore.DataSource = PublicCode.GetKQdept(cbbMianDept.SelectedItem.Value);
            KQStore.DataBind();
            cbbKQ.Disabled = false;
        }
    }

    [AjaxMethod]
    public void LoadData()
    {
        if (cbbPerson.SelectedIndex == -1)
        {
            Ext.Msg.Alert("提示", "请选择人员！").Show();
            return;
        }
        //cbbPerson.SelectedItem.Value = "110932";

        string realsql = string.Format("SELECT short_name, stateflag,person_name, entertime_mine, outtime_mine, trackblock FROM v_jykj_real where remark2='{0}' order by entertime_mine desc limit 1;", cbbPerson.SelectedItem.Value.Trim().PadLeft(7,'0'));
        string historysql = string.Format("SELECT short_name, stateflag,person_name, entertime_mine, outtime_mine, trackblock, station_mark FROM v_jykj_history where remark2='{0}' order by entertime_mine desc limit 1;", cbbPerson.SelectedItem.Value.Trim().PadLeft(7,'0'));
        string msg = "";
        PostGreSQLHelper pgh = new PostGreSQLHelper();
        msg += "<B><font color=\"orange\">当前下井信息</font></B><HR>";
        DataTable dtreal = pgh.ExecuteQuery(realsql).Tables[0];
        if (dtreal.Rows.Count > 0)
        {
            msg += "当前状态：" + (dtreal.Rows[0]["stateflag"].ToString().Trim() == "1" ? "已入井" : "已出井");
            msg += "<p>";
            msg += "入井时间：" + dtreal.Rows[0]["entertime_mine"].ToString()+"<p>";
            if (dtreal.Rows[0]["outtime_mine"].ToString() == "1902/1/1 0:00:00")
                msg += "出井时间：xxxx/x/x x:xx:xx<p>";
            else
            msg += "出井时间：" + dtreal.Rows[0]["outtime_mine"].ToString() + "<p>";
            msg += "轨迹信息：<p>";
            if (dtreal.Rows[0]["trackblock"].ToString().Trim() == "")
            {
                msg += "无<p>";
            }
            else
            {
                string[] group = dtreal.Rows[0]["trackblock"].ToString().Trim().Split(',');
                foreach (var r in group)
                {
                    string[] row = r.Split('&');
                    msg += row[1] + "到达" + pgh.GetPointNameNote(row[0])+"<p>";
                }
            }
        }
        else
        {
            msg += "无<p>";
        }
        msg += "<B><font color=\"orange\">最近一次下井信息</font></B><HR>";
        DataTable dthistory = pgh.ExecuteQuery(historysql).Tables[0];
        if (dthistory.Rows.Count > 0)
        {
            msg += "历史状态：已出井<p>";
            msg += "入井时间：" + dthistory.Rows[0]["entertime_mine"].ToString() + "<p>";
            msg += "出井时间：" + dthistory.Rows[0]["outtime_mine"].ToString() + "<p>";
            msg += "轨迹信息：<p>";
            if (dthistory.Rows[0]["station_mark"].ToString().Trim() == "")
            {
                msg += "无<p>";
            }
            else
            {
                string[] group = dthistory.Rows[0]["trackblock"].ToString().Trim().Split(',');
                foreach (var r in group)
                {
                    string[] row = r.Split('&');
                    msg += row[1] + "到达" + pgh.GetPointNameNote(row[0]) + "<p>";
                }
                //msg += dthistory.Rows[0]["station_mark"].ToString().Trim().Replace(",", "<p>").Replace("*", "到达时间:");
            }
        }
        else
        {
            msg += "无";
        }

        panel1.Html = msg;
        //DetailStore.DataSource = pgh.ExecuteQuery("SELECT short_name, stateflag, substring(card_id from 10) as card_id, person_name, entertime_mine, outtime_mine, trackblock, station_mark FROM v_jykj_history limit 5;");
        //DetailStore.DataBind();
    }
}