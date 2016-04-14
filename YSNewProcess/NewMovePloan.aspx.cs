using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Coolite.Ext.Web;
using GhtnTech.SEP.DAL;
using GhtnTech.SecurityFramework.BLL;

using System.Xml;
using System.Xml.Xsl;

public partial class YSNewProcess_NewMovePloan : System.Web.UI.Page
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            //初始化日期
            df_end.SelectedValue = System.DateTime.Today.AddMonths(1).AddDays(-System.DateTime.Today.Day);
            df_begin.SelectedValue = System.DateTime.Today.AddDays(1 - System.DateTime.Today.Day);
            storeload();
        }
    }

    protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)
    {
        storeload();
    }

    #region 数据加载
    private void storeload()
    {
        var data = from m in dc.VMoveplan
                   from k in dc.Moveplan
                   where m.Maindept == SessionBox.GetUserSession().DeptNumber && m.Id==k.Id
                   select new
                   {
                       m.Name,
                       m.Placeid,
                       m.Placename,
                       m.Deptname,
                       m.Posid,
                       m.Posname,
                       m.Id,
                       m.Personid,
                       m.Starttime,
                       m.Endtime,
                       m.Movestarttime,
                       m.Movestate,
                       Closeremarks = (k.Closeremarks == "录入隐患闭合" ? "" : k.Closeremarks)
                   };
        if (!df_begin.IsNull)
        {
            data = data.Where(p => p.Starttime >= df_begin.SelectedDate.Date);
        }
        if (!df_end.IsNull)
        {
            data = data.Where(p => p.Starttime <= df_end.SelectedDate.Date);
        }
        if (cbb_person.SelectedIndex > -1)
        {
            data = data.Where(p => p.Personid == cbb_person.SelectedItem.Value.Trim());
        }
        if (cbb_place.SelectedIndex > -1)
        {
            data = data.Where(p => p.Placeid == Decimal.Parse(cbb_place.SelectedItem.Value.Trim()));
        }
        if (cbb_zhiwu.SelectedIndex > -1)
        {
            data = data.Where(p => p.Posid == Decimal.Parse(cbb_zhiwu.SelectedItem.Value.Trim()));
        }

        //var data = from a in dc.Moveplan
        //            from b in dc.Person
        //            from c in dc.Place
        //            from d in dc.Department
        //            from e in dc.Position
        //            where a.Personid == b.Personnumber && a.Placeid == c.Placeid && b.Deptid == d.Deptnumber && b.Posid==e.Posid &&
        //            a.Maindept == SessionBox.GetUserSession().DeptNumber && a.Starttime >= start && a.Starttime <= end
        //            select new
        //            {
        //                b.Name,
        //                c.Placename,
        //                d.Deptname,
        //                e.Posname,
        //                a.Id,
        //                a.Personid,
        //                a.Starttime,
        //                a.Endtime,
        //                a.Movestate
        //            };

        Store1.DataSource = data;
        Store1.DataBind();
        btn_delete.Disabled = true;
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        sm.SelectedRows.Clear();
        sm.UpdateSelection();
    }

    #endregion

    #region 计划复制

    [AjaxMethod]
    public void CopyPlan()
    {
        Ext.Msg.Confirm("提示", "是否确认完全复制上月计划作为本月计划?", new MessageBox.ButtonsConfig
        {
            Yes = new MessageBox.ButtonConfig
            {
                Handler = "Coolite.AjaxMethods.PlanCopy();",
                Text = "确 定"
            },
            No = new MessageBox.ButtonConfig
            {
                Text = "取 消"
            }
        }).Show();
    }

    [AjaxMethod]
    public void PlanCopy()
    {
        var mp = dc.Moveplan.Where(p => p.Starttime >= System.DateTime.Today.AddMonths(-1).AddDays(1 - System.DateTime.Today.Day)
            && p.Starttime <= System.DateTime.Today.AddDays(-System.DateTime.Today.Day)
            && p.Maindept == SessionBox.GetUserSession().DeptNumber
            );
        foreach (var r in mp)
        {

            Moveplan mp1 = new Moveplan
            {
                Personid = r.Personid,
                Placeid = r.Placeid,
                Starttime = r.Starttime.Value.AddMonths(1),
                Endtime = r.Endtime.Value.AddMonths(1),
                Movestate = "未走动",
                Maindept = r.Maindept
            };
            dc.Moveplan.InsertOnSubmit(mp1);
            dc.SubmitChanges();
        }
        Ext.Msg.Alert("提示", "复制完成，共计添加" + mp.Count().ToString() + "条计划！").Show();
        storeload();
    }

    #endregion

    #region 删除计划

    [AjaxMethod]
    public void delshow()
    {
        Ext.Msg.Confirm("提示", "已走动的计划系统不予删除，是否确定?", new MessageBox.ButtonsConfig
        {
            Yes = new MessageBox.ButtonConfig
            {
                Handler = "Coolite.AjaxMethods.Detail_Del();",
                Text = "确 定"
            },
            No = new MessageBox.ButtonConfig
            {
                Text = "取 消"
            }
        }).Show();
    }
    [AjaxMethod]
    public void Detail_Del()
    {
        CheckboxSelectionModel sm = this.GridPanel1.SelectionModel.Primary as CheckboxSelectionModel;
        if (sm.SelectedRows.Count() > 0)
        {
            foreach (var r in sm.SelectedRows)
            {
                DBSCMDataContext dc = new DBSCMDataContext();
                var mp1 = dc.Moveplan.Where(p => p.Id == Convert.ToInt32(r.RecordID) && p.Movestate != "已走动");
                if (mp1.Count() > 0)
                {
                    dc.Moveplan.DeleteAllOnSubmit(mp1);
                    dc.SubmitChanges();
                }
            }
            storeload();
            Ext.Msg.Alert("提示", "删除成功!").Show();
        }
    }

    #endregion

    #region 选择行事件
    protected void RowClick(object sender, AjaxEventArgs e)
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            //btnpostion.Disabled = false;
            btn_delete.Disabled = false;
        }
        else
        {
            //btnpostion.Disabled = true;
            btn_delete.Disabled = true;
        }
    }

    #endregion

    #region 导出报表
    protected void ToExcel(object sender, EventArgs e)//导出报表
    {
        string json = GridDataExcel.Value.ToString();
        foreach (var r in GridPanel1.ColumnModel.Columns)
        {
            json = json.Replace("\"" + r.DataIndex.Trim() + "\"", "\"" + r.Header + "\"");
        }
        json = json.Replace("T00:00:00", "");
        //json = json.Replace("\"检查方式\":0", "\"检查方式\":\"矿查\"");
        //json = json.Replace("\"检查方式\":1", "\"检查方式\":\"自查\"");
        //json = json.Replace("\"处罚确认\":1", "\"处罚确认\":\"是\"");
        //json = json.Replace("\"处罚确认\":0", "\"处罚确认\":\"否\"");

        string strFileName = "走动信息报表";
        Response.Clear();
        Response.Buffer = true;
        Response.Charset = "GB2312";

        StoreSubmitDataEventArgs eSubmit = new StoreSubmitDataEventArgs(json, null);
        XmlNode xml = eSubmit.Xml;

        this.Response.Clear();
        this.Response.ContentType = "application nd.ms-excel";
        Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(strFileName) + ".xls");
        XslCompiledTransform xtExcel = new XslCompiledTransform();
        xtExcel.Load(Server.MapPath("ExcelMP.xsl"));
        xtExcel.Transform(xml, null, this.Response.OutputStream);
        this.Response.End();
    }
    #endregion

    #region 查询窗口

    [AjaxMethod]
    public void SearchLoad()//查询窗口初始化
    {
        //初始化地点
        var dep = from d in dc.Place
                  where d.Maindeptid == SessionBox.GetUserSession().DeptNumber
                  select new
                  {
                      PlaceName = d.Placename,
                      PlaceID = d.Placeid
                  };
        Store4.DataSource = dep;
        Store4.DataBind();
        //初始化人员职务
        //需要添加权限判断-判断是否为走动干部进入
        var pos = from p in dc.Position
                  from per in dc.Person
                  where p.Posid == per.Posid && p.Maindeptid == per.Maindeptid && per.Maindeptid == SessionBox.GetUserSession().DeptNumber
                  select new
                  {
                      PosID = p.Posid,
                      PosName = p.Posname
                  };
        PosStore.DataSource = pos;
        PosStore.DataBind();
    }

    [AjaxMethod]
    public void ClearSearch()//清除条件
    {
        df_begin.Clear();
        df_end.Clear();
        cbb_zhiwu.SelectedItem.Value = null;
        cbb_person.SelectedItem.Value = null;
        cbb_place.SelectedItem.Value = null;
    }
    [AjaxMethod]
    public void Search()
    {
        FormWindow.Hide();
        storeload();
    }

    protected void PersonRefresh(object sender, StoreRefreshDataEventArgs e)
    {
        //需要添加权限判断-判断是否为走动干部进入

        //var q = dc.Person.Where(p => p.Posid == Convert.ToInt32(cbb_zhiwu.SelectedItem.Value) && p.Maindeptid == SessionBox.GetUserSession().DeptNumber);
        var q = from p in dc.Person
                where p.Posid == Convert.ToInt32(cbb_zhiwu.SelectedItem.Value) && p.Maindeptid == SessionBox.GetUserSession().DeptNumber
                orderby p.Name ascending
                select new
                {
                    p.Personnumber,
                    p.Name
                };
        PersonStore.DataSource = q;
        PersonStore.DataBind();
        cbb_person.Disabled = q.Count() > 0 ? false : true;
    }
    #endregion

    [AjaxMethod]
    public void PostionShow(string command, decimal Id)
    {
        try
        {
            var mp1 = dc.Moveplan.First(p => p.Id == Id);
            if (mp1.Movestate != "已走动")
            {
                Ext.Msg.Alert("提示", "尚未走动，不能查看").Show();
                return;
            }
            if (mp1.Movestarttime == null)
            {
                Ext.Msg.Alert("提示", "手动闭合计划，不能查看").Show();
                return;
            }

            string historysql = string.Format("SELECT short_name, stateflag,person_name, entertime_mine, outtime_mine, trackblock, station_mark FROM v_jykj_history where remark2='{0}' and entertime_mine>='{1}' and entertime_mine<='{2}' order by entertime_mine desc;",
                mp1.Personid.Trim().PadLeft(7, '0'),
                mp1.Movestarttime.Value.ToString("yyyy-MM-dd") + " 00:00:00",
                mp1.Movestarttime.Value.ToString("yyyy-MM-dd") + " 23:59:59");
            PostGreSQLHelper pgh = new PostGreSQLHelper();

            string msg = "<B><font color=\"orange\">" + mp1.Movestarttime.Value.ToString("yyyy年MM月dd日") + "下井信息</font></B><HR>";
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

            SearchBLWindow.Html = msg;
            //SearchBLStore.DataSource = pgh.ExecuteQuery(historysql).Tables[0];
            //SearchBLStore.DataBind();
            SearchBLWindow.Show();

        }
        catch 
        {
            Ext.Msg.Alert("出错提醒", "查询超时或没有信息！").Show();
        }
    }
}
