using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Xsl;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Coolite.Ext.Web;
using GhtnTech.SecurityFramework.BLL;
using GhtnTech.SEP.DAL;

public partial class MovePlan_MovePosition : BasePage
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //初始化日期
            df_end.SelectedValue = System.DateTime.Today;
            df_begin.SelectedValue = System.DateTime.Today.AddDays(1 - System.DateTime.Today.Day);
            //df_begin.MaxDate = df_end.SelectedDate;
            //df_end.MinDate = df_begin.SelectedDate;
            SearchLoad();
            storeload();
            
        }
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

    protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)
    {
        storeload();
    }

    private void storeload()//数据绑定
    {
        //需要添加权限判断-判断是否为走动干部进入

        var data = from m in dc.VMoveplan
                where m.Maindept == SessionBox.GetUserSession().DeptNumber
                select new
                {
                    Name = m.Name,
                    PlaceName = m.Placename,
                    DeptName = m.Deptname,
                    PosName = m.Posname,
                    ID = m.Id,
                    PersonID = m.Personid,
                    StartTime = m.Starttime,
                    EndTime = m.Endtime,
                    MoveState = m.Movestate,
                    m.Posid,
                    m.Placeid
                };
        if (df_begin.SelectedDate > df_end.SelectedDate)
        {
            Ext.Msg.Alert("提示", "日期选择有误!").Show();
            return;
        }
        #region 直接linq查询-数据搜索速度慢，先改成上述视图
        //var data = from m in dc.Moveplan
        //           from p in dc.Person
        //           from pl in dc.Place
        //           from d in dc.Department
        //           from pos in dc.Position
        //           where m.Personid == p.Personnumber && m.Placeid == pl.Placeid && p.Deptid == d.Deptnumber && p.Posid == pos.Posid && m.Maindept == SessionBox.GetUserSession().DeptNumber
        //           && m.Starttime.Value >= (df_begin.IsNull ? m.Starttime.Value : df_begin.SelectedDate.Date) && m.Endtime.Value <= (df_end.IsNull ? m.Endtime.Value : df_end.SelectedDate.Date)
        //               //&& pos.Posid == (cbb_zhiwu.SelectedIndex == -1 ? pos.Posid : Convert.ToInt32(cbb_zhiwu.SelectedItem.Value))
        //           && p.Personnumber == (cbb_person.SelectedIndex == -1 ? p.Personnumber : cbb_person.SelectedItem.Value.Trim())
        //           //&& pl.Placeid == (cbb_place.SelectedIndex == -1 ? pl.Placeid : Decimal.Parse(cbb_place.SelectedItem.Value.Trim()))
        //           select new
        //           {
        //               Name = p.Name,
        //               PlaceName = pl.Placename,
        //               DeptName = d.Deptname,
        //               PosName = pos.Posname,
        //               ID = m.Id,
        //               PersonID = m.Personid,
        //               StartTime = m.Starttime,
        //               EndTime = m.Endtime,
        //               MoveState = m.Movestate,
        //               pos.Posid,
        //               pl.Placeid
        //           };
        #endregion
        if (!df_begin.IsNull)
        {
            data = data.Where(p => p.StartTime >= df_begin.SelectedDate.Date);
        }
        if (!df_end.IsNull)
        {
            data = data.Where(p => p.EndTime <= df_end.SelectedDate.Date);
        }
        if (cbb_person.SelectedIndex > -1)
        {
            data = data.Where(p => p.PersonID == cbb_person.SelectedItem.Value.Trim());
        }
        if (cbb_place.SelectedIndex > -1)
        {
            data = data.Where(p => p.Placeid == Decimal.Parse(cbb_place.SelectedItem.Value.Trim()));
        }
        if (cbb_zhiwu.SelectedIndex > -1)
        {
            data = data.Where(p => p.Posid == Decimal.Parse(cbb_zhiwu.SelectedItem.Value.Trim()));
        }
        MoveStore.DataSource = data;
        MoveStore.DataBind();
    }

    private void SearchLoad()//查询窗口初始化
    {
        DBSCMDataContext dc = new DBSCMDataContext();

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
                  where p.Posid == per.Posid && p.Maindeptid==per.Maindeptid && per.Maindeptid == SessionBox.GetUserSession().DeptNumber
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
        //SearchLoad();
        FormWindow.Hide();
        GridPanel1.Show();
        storeload();
        //ClearSearch();
    }
}
