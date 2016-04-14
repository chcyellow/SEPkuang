using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using GhtnTech.SEP.DAL;
using GhtnTech.SecurityFramework.BLL;
using Coolite.Ext.Web;

public partial class YSNewProcess_CloseMovePlan : System.Web.UI.Page
{
    DBSCMDataContext db = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            BaseBind();
            bindPlan();
        }
    }

    private void BaseBind()
    {
        var dept = from d in db.Department
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

    protected void PersRefresh(object sender, StoreRefreshDataEventArgs e)//发布/提交选择责任部门人员刷新
    {
        var q = from p in db.Person
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

    //绑定走动计划
    private void bindPlan()
    {
        DateTime min = System.DateTime.Today.AddDays(1 - System.DateTime.Today.Day);
        DateTime max = System.DateTime.Today;
        if (System.DateTime.Today.Day < 4)//如果是本月前3天允许闭合上月计划
        {
            min = min.AddMonths(-1);
        }
        var sqltext = from a in db.Moveplan
                      from b in db.Person
                      from c in db.Place
                      from d in db.Department
                      where a.Personid == b.Personnumber && a.Placeid == c.Placeid && b.Areadeptid == d.Deptnumber
                      //&& a.Personid == SessionBox.GetUserSession().PersonNumber
                      && b.Maindeptid==SessionBox.GetUserSession().DeptNumber
&& ((a.Starttime.Value >= min && a.Starttime.Value <= max)
|| (a.Endtime.Value >= min && a.Endtime.Value <= max))
                      select new
                      {
                          ID = a.Id,
                          EndTime = a.Endtime,
                          StartTime = a.Starttime,
                          PersonID = a.Personid,
                          //MoveStartTime = a.Movestarttime,
                          //MoveEndTime = a.Moveendtime,
                          Name = b.Name,
                          PlaceName = c.Placename,
                          DeptName = d.Deptname,
                          MoveState = a.Movestate,
                          d.Deptnumber,
                          b.Personnumber,
                          a.Closeremarks
                      };
        if (cbbforcheckDept.SelectedIndex > -1)
        {
            sqltext = sqltext.Where(p => p.Deptnumber == cbbforcheckDept.SelectedItem.Value);
        }

        if (fb_zrr.SelectedIndex > -1)
        {
            sqltext = sqltext.Where(p => p.Personnumber == fb_zrr.SelectedItem.Value);
        }
        Store1.DataSource = sqltext;
        Store1.DataBind();
    }

    [AjaxMethod]
    public void btnSearch_Click()
    {
        bindPlan();
    }

    [AjaxMethod]
    public void CloseMP()
    {
        //RowSelectionModel sm = GridPanel1.SelectionModel.Primary as RowSelectionModel;
        CheckboxSelectionModel sm=GridPanel1.SelectionModel.Primary as CheckboxSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            Ext.Msg.Show(new MessageBox.Config
            {
                Title = "闭合选中计划",
                Message = "请在下面录入闭合计划原因：",
                Width = 300,
                ButtonsConfig = new MessageBox.ButtonsConfig
                {
                    Yes = new MessageBox.ButtonConfig
                    {
                        Handler = "Coolite.AjaxMethods.DoClose(text)",
                        Text = "保存"
                    },
                    No = new MessageBox.ButtonConfig
                    {
                        Text = "取消"
                    }
                },
                Buttons = MessageBox.Button.YESNO,
                Multiline = true,
                AnimEl = btn_fcfk.ClientID
                //Fn = new JFunction { Fn = "showResultText" }
            });
            //Ext.Msg.Confirm("提示", "是否确认闭合选中计划?", new MessageBox.ButtonsConfig
            //{
            //    Yes = new MessageBox.ButtonConfig
            //    {
            //        Handler = "Coolite.AjaxMethods.DoClose();",
            //        Text = "确 定"
            //    },
            //    No = new MessageBox.ButtonConfig
            //    {
            //        Text = "取 消"
            //    }
            //}).Show();
        }
        else
        {
            Ext.Msg.Alert("提示", "请选择走动计划!").Show();
        }
    }

    [AjaxMethod]
    public void DoClose(string text)
    {
        CheckboxSelectionModel sm = GridPanel1.SelectionModel.Primary as CheckboxSelectionModel;
        foreach (var r in sm.SelectedRows)
        {
            var mp = db.Moveplan.First(p => p.Id == Decimal.Parse(r.RecordID));
            mp.Closetype=1;
            mp.Closeremarks = text;
            mp.Movestate = "已走动";
            db.SubmitChanges();
        }
        bindPlan();
        Ext.Msg.Alert("提示", "共计闭合"+sm.SelectedRows.Count+"条走动计划!").Show();
    }
}
