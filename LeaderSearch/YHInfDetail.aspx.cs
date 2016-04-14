using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Coolite.Ext.Web;
using GhtnTech.SEP.DAL;
using GhtnTech.SecurityFramework.BLL;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.Linq;
using System.Web.Script.Serialization;

public partial class LeaderSearch_YHInfDetail : BasePage
{
    DBSCMDataContext dc = new DBSCMDataContext();
    //StoreRefreshDataEventArgs srdea = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!Ext.IsAjaxRequest)
            {
                //初始化模块权限
                //UserHandle.InitModule(this.PageTag);
                ////是否有浏览权限
                //if (UserHandle.ValidationHandle(PermissionTag.Browse))
                //{
                    //Ext.DoScript("#{Store1}.reload();");
                    SearchLoad();
                    //personload();
                    //storeload();//第一次不加载
                //}

            }
        }

    }
    protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)
    {
        //if (srdea == null)
        //{
        //    srdea = e;
        //}
        //storeload();
        Search();
    }

    private void personload()
    {
        //var q = dc.Person2.Where(p => p.Posid == p.Posid);
        //绑定人员
        var person = from r in dc.Person
                     from d in dc.Department
                     where r.Deptid == d.Deptnumber && d.Fatherid == SessionBox.GetUserSession().DeptNumber
                     select new
                     {
                         PersonID = r.Personnumber,
                         r.Name
                     };
        personStore.DataSource = person;
        personStore.DataBind();
    }

    #region 单击每行-根据不同状态获取权限
    protected void RowClick(object sender, AjaxEventArgs e)//流程处理
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            btn_detail.Disabled = false;
        }
        else
        {
            btn_detail.Disabled = true;
        }
    }
    #endregion

    #region 初始化页面-数据绑定
    private void storeload()//执行查询
    {
        if (SessionBox.GetUserSession().rolelevel.Contains("1") || SessionBox.GetUserSession().rolelevel.Contains("0"))
        {
            var data = from yh in dc.Getyhinput
                       from d in dc.Department 
                       where yh.Unitid==d.Deptnumber
                       orderby yh.Levelname, yh.Intime descending
                       select new
                       {
                           YHPutinID = yh.Yhputinid,
                           yh.Deptid,
                           DeptName = yh.Deptname,
                           PlaceName = yh.Placename,
                           YHContent = yh.Yhcontent,
                           Remarks = "",
                           BanCi = yh.Banci,
                           Name = yh.Name,
                           PCTime = yh.Pctime,
                           Zyid=yh.Typeid,
                           YHType = yh.Typename,
                           yh.Personid,
                           Maindeptid=yh.Unitid,
                           Maindeptname=d.Deptname,
                           yh.Levelid,
                           yh.Levelname,
                           Status = yh.Status
                       };

            if (df_begin.SelectedDate <= df_end.SelectedDate && df_begin.SelectedValue != null)
            {
                data = data.Where(p => p.PCTime <= df_end.SelectedDate && p.PCTime >= df_begin.SelectedDate);
            }
            if (cbb_person.SelectedIndex > -1)
            {
                data = data.Where(p => p.Personid == cbb_person.SelectedItem.Value);
            }
            if (nf_ID.Text.Trim() != "")
            {
                data = data.Where(p => p.YHPutinID == decimal.Parse(nf_ID.Text.Trim()));
            }
            if (cbb_part.SelectedIndex > -1)
            {
                data = data.Where(p => p.Deptid == cbb_part.SelectedItem.Value.Trim());
            }
            if (cbb_kind.SelectedIndex > -1)
            {
                data = data.Where(p => p.Zyid == decimal.Parse(cbb_kind.SelectedItem.Value.Trim()));
            }
            if (cbb_status.SelectedIndex > -1)
            {
                data = data.Where(p => p.Status == cbb_status.SelectedItem.Value.Trim());
            }
            if (cbbUnit.SelectedIndex > -1)
            {
                data = data.Where(p => p.Maindeptid == cbbUnit.SelectedItem.Value);
            }

            Store1.DataSource = data;
            Store1.DataBind();
            btn_detail.Disabled = true;
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            sm.SelectedRows.Clear();
            sm.UpdateSelection();
        }
        else
        {
            //var data = GetSafeInfo.GetYHView(SessionBox.GetUserSession().DeptNumber, df_begin.SelectedDate, df_end.SelectedDate);
            var data = from yh in dc.Getyhinput
                       from d in dc.Department
                       orderby d.Deptname, yh.Levelname, yh.Status, yh.Intime descending
                       where  yh.Unitid == d.Deptnumber //yh.Unitid== SessionBox.GetUserSession().DeptName.Trim() &&
                       select new
                       {
                           YHPutinID = yh.Yhputinid,
                           yh.Deptid,
                           DeptName = yh.Deptname,
                           PlaceName = yh.Placename,
                           YHContent = yh.Yhcontent,
                           Remarks = "",
                           BanCi = yh.Banci,
                           Name = yh.Name,
                           PCTime = yh.Pctime,
                           Zyid = yh.Typeid,
                           YHType = yh.Typename,
                           yh.Personid,
                           Maindeptid = yh.Unitid,
                           Maindeptname = d.Deptname,
                           yh.Levelid,
                           yh.Levelname,
                           Status = yh.Status
                       };

            if (df_begin.SelectedDate <= df_end.SelectedDate && df_begin.SelectedValue != null)
            {
                data = data.Where(p => p.PCTime <= df_end.SelectedDate && p.PCTime >= df_begin.SelectedDate);
            }
            if (cbb_person.SelectedIndex > -1)
            {
                data = data.Where(p => p.Personid == cbb_person.SelectedItem.Value);
            }
            if (nf_ID.Text.Trim() != "")
            {
                data = data.Where(p => p.YHPutinID == decimal.Parse(nf_ID.Text.Trim()));
            }
            if (cbb_part.SelectedIndex > -1)
            {
                data = data.Where(p => p.Deptid == cbb_part.SelectedItem.Value.Trim());
            }
            if (cbb_kind.SelectedIndex > -1)
            {
                data = data.Where(p => p.Zyid == decimal.Parse(cbb_kind.SelectedItem.Value.Trim()));
            }
            if (cbb_status.SelectedIndex > -1)
            {
                data = data.Where(p => p.Status == cbb_status.SelectedItem.Value.Trim());
            }
            if (cbbUnit.SelectedIndex > -1)
            {
                data = data.Where(p => p.Maindeptid == cbbUnit.SelectedItem.Value);
            }
            Store1.DataSource = data;
            Store1.DataBind();
            btn_detail.Disabled = true;
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            sm.SelectedRows.Clear();
            sm.UpdateSelection();
        }
        
    }

    #endregion

    #region 查询窗口操作

    protected void PersonRefresh(object sender, StoreRefreshDataEventArgs e)//查询人员刷新
    {
        var q = dc.Person.Where(p => p.Posid == Convert.ToInt32(cbb_position.SelectedItem.Value) && p.Maindeptid == cbbUnit.SelectedItem.Value);
        Store2.DataSource = q;
        Store2.DataBind();
        cbb_person.Disabled = q.Count() > 0 ? false : true;
    }

    private void SearchLoad()//查询窗口初始化
    {
        //初始化日期
        df_begin.SelectedDate = System.DateTime.Today;
        df_end.SelectedDate = System.DateTime.Today;
        //df_begin.SelectedDate = System.DateTime.Today;//.AddDays(-1);
        //df_end.SelectedDate = System.DateTime.Today;
        #region 初始化职务
        var pos = from p in dc.Position
                  from per in dc.Person
                  where p.Posid == per.Posid && per.Maindeptid == SessionBox.GetUserSession().DeptNumber
                  select new
                  {
                      p.Posid,
                      p.Posname
                  };
        Store3.DataSource = pos;
        Store3.DataBind();
        #endregion

        #region 初始化部门
        //var dep = from d in dc.Department
        //          where d.Fatherid == SessionBox.GetUserSession().DeptNumber
        //          select new
        //          {
        //              d.Deptname,
        //              Deptid = d.Deptnumber
        //          };
        //DeptStore.DataSource = dep;
        //DeptStore.DataBind();

        var dep = from d in dc.Department
                  from p in dc.Person
                  where d.Deptnumber == p.Areadeptid && p.Maindeptid == SessionBox.GetUserSession().DeptNumber
                  select new
                  {
                      d.Deptname,
                      Deptid = d.Deptnumber
                  };
        DeptStore.DataSource = dep;
        DeptStore.DataBind();

        #endregion

        #region 初始化单位
        if (SessionBox.GetUserSession().rolelevel.Trim().IndexOf("1") > -1)
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
            cbbUnit.Disabled = false;

            cbb_position.Disabled = true;
            cbb_person.Disabled = true;
            cbb_part.Disabled = true;
            cbb_status.Disabled = true;
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

        //#region 初始化级别
        //var lavel = from l in dc.Yinhuanlevel
        //            select new
        //                {
        //                    l.YHLevelID,
        //                    l.YHLevel
        //                };
        //LevelStore.DataSource = lavel;
        //LevelStore.DataBind();
        //#endregion

        #region 初始化类型
        int zyID = int.Parse(PublicMethod.ReadXmlReturnNode("ZY", this));

        var type = from t in dc.CsBaseinfoset
                   where t.Fid == zyID
                   select new
                   {
                       YHTypeID = t.Infoid,
                       YHType = t.Infoname
                   };
        TypeStore.DataSource = type;
        TypeStore.DataBind();
        #endregion

        #region 初始化状态
        StatusStore.DataBind();
        #endregion
    }

    [AjaxMethod]
    public void ClearSearch()//清除条件
    {
        df_begin.Clear();
        df_end.Clear();
        cbb_position.SelectedItem.Value = null;
        cbb_person.SelectedItem.Value = null;
        cbb_person.Disabled = true;
        nf_ID.Clear();
        cbb_part.SelectedItem.Value = null;
        cbb_kind.SelectedItem.Value = null;
        cbb_status.SelectedItem.Value = null;
        if (SessionBox.GetUserSession().rolelevel.Trim().IndexOf("1") > -1)
        {
            cbbUnit.SelectedItem.Value = null;
        }
    }
    [AjaxMethod]
    public void Search()//点击查询按钮
    {
        //storeload();
        //Ext.DoScript("#{Store1}.reload();");
        //Ext.DoScript("#{Store1}.load({params:{start:0,limit:" + PagingToolBar1.PageSize + "}});");
        
        
        storeload();
        Window1.Hide();
        //GridPanel1.Show();
    }
    #endregion

    #region 隐患信息查询绑定
    [AjaxMethod]
    public void DetailLoad()//加载明细信息
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        string url = string.Format("../YSNewSearch/YhView.aspx?Yhid={0}", sm.SelectedRows[0].RecordID.Trim());
        Ext.DoScript("#{DetailWindow}.load('" + url + "');#{DetailWindow}.show();");
    }

   

    #endregion

}
