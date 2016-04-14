using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Coolite.Ext.Web;
using GhtnTech.SEP.DAL;
using GhtnTech.SecurityFramework.BLL;

public partial class CHARGETABLE_CT_manager : BasePage
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            //初始化模块权限
            UserHandle.InitModule(this.PageTag);
            //是否有浏览权限
            if (UserHandle.ValidationHandle(PermissionTag.Browse))
            {
                storeload();
                btnLookRoute.Disabled = true;
                if (!UserHandle.ValidationHandle(PermissionTag.Add))
                {
                    btnNew.Visible = false;
                    Button4.Visible = false;
                    btnPlaceAdd.Visible = false;
                    
                }
                if (!UserHandle.ValidationHandle(PermissionTag.Delete))
                {
                    btnPlaceDel.Visible = false;
                    btnDel.Visible = false;
                }
                if(!UserHandle.ValidationHandle(PermissionTag.Move))
                {
                    btnUp.Visible = false;
                    btnDown.Visible = false;
                }
                //如果拥有处理个人信息权限
                if (UserHandle.ValidationHandle(PermissionTag.PersonalOnly))
                {
                    cbbPerson.Disabled = true;
                }
                BaseSet();
            }

        }
    }

    private void BaseSet()
    {
        HBBLL hb = new HBBLL();
       
        var data = hb.GetLeaderRole(SessionBox.GetUserSession().DeptNumber, 431);
        var q = from p in data
                select new
                {
                    Personnumber = p.personnumber,
                    Name = p.name
                };
        perStore.DataSource = q;
        perStore.DataBind();
        if (SessionBox.GetUserSession().Role.Contains("11,矿级管理员"))
        {
            //cbbPerson.SelectedItem.Value = SessionBox.GetUserSession().PersonNumber;
            if (SessionBox.GetUserSession().Role.Contains("431,副总及以上领导"))
            {
                cbbPerson.SelectedItem.Value = SessionBox.GetUserSession().PersonNumber;
                cbbPerson.Disabled = false;
            }
            else
            {
                cbbPerson.SelectedIndex = 0;
            }
        }
        else
        {
            cbbPerson.Items.Add(new Coolite.Ext.Web.ListItem(SessionBox.GetUserSession().Name, SessionBox.GetUserSession().PersonNumber));
            cbbPerson.SelectedItem.Value = SessionBox.GetUserSession().PersonNumber;
            cbbPerson.Disabled = true;
        }
        df_begin.MinDate = System.DateTime.Today;
        df_begin.SelectedDate = df_begin.MinDate;
        //cx_date.SelectedDate = System.DateTime.Today;
        //var q = from p in dc.Person
        //        where p.Personnumber==SessionBox.GetUserSession().PersonNumber
        //        select new
        //        {
        //            p.Personnumber,
        //            p.Name
        //        };
        //perStore.DataSource = q;
        //perStore.DataBind();
        

        DeptStore.DataSource = PublicCode.GetMaindept("");
        DeptStore.DataBind();
        if (SessionBox.GetUserSession().rolelevel.Contains("1") || SessionBox.GetUserSession().rolelevel.Contains("0"))
        {
            cbbDept.Items.Insert(0, new Coolite.Ext.Web.ListItem("--全部--", "-1"));
            cbbDept.SelectedItem.Value = "-1";
            cbbDept.Disabled = false;
        }
        else
        {
            cbbDept.SelectedItem.Value = SessionBox.GetUserSession().DeptNumber;
            cbbDept.Disabled = true;
        }
        //HBBLL hb = new HBBLL();

        //var data2 = hb.GetYPPT(SessionBox.GetUserSession().PersonNumber, SessionBox.GetUserSession().DeptNumber, "个人");
        //var pt = from r in data2
        //         select new
        //         {
        //             Id = r.Id,
        //             Name = r.Name,
        //             Level = r.TLevel
        //         };
        //placeTemplateStore.DataSource = pt;
        //placeTemplateStore.DataBind();
        //cboPlaceTemplate.Items.Insert(0, new Coolite.Ext.Web.ListItem("--不使用模板--", "-1"));
        cx_date.SelectedValue = DateTime.Today;
    }

    protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)
    {
        storeload();
    }

    private void storeload()//执行查询
    {
        var data = from ct in dc.YChargetable
                   from p in dc.Person
                   from d in dc.Department
                   from d1 in dc.Department
                   where ct.Cperson == p.Personnumber && p.Deptid == d.Deptnumber && ct.Maindept==d1.Deptnumber
                   //&& ct.Maindept == SessionBox.GetUserSession().DeptNumber
                   orderby ct.Cdate descending
                   select new
                       {
                           ct.Recordtime,
                           ct.Cdate,
                           ct.Cbanci,
                           ct.Cperson,
                           ct.Maindept,
                           ct.Rperson,
                           ct.Status,
                           p.Name,
                           d.Deptname,
                           Maindeptname=d1.Deptname,
                           ct.Id
                       };
        if (SessionBox.GetUserSession().rolelevel.Contains("1") || SessionBox.GetUserSession().rolelevel.Contains("0"))
        {
            
        }
        else
        {
            //初始化模块权限
            UserHandle.InitModule(this.PageTag);
            //如果拥有处理个人信息权限
            if (UserHandle.ValidationHandle(PermissionTag.PersonalOnly))
            {
                data = data.Where(p => p.Cperson == SessionBox.GetUserSession().PersonNumber);
            }
            else if (UserHandle.ValidationHandle(PermissionTag.Deptall))
            {
                data = data.Where(p => p.Maindept == SessionBox.GetUserSession().DeptNumber);
            }
            else
            {
                data = data.Where(p => false);
            }
        }
        if (cbbDept.SelectedIndex >0 )
        {
            data = data.Where(p => p.Maindept == cbbDept.SelectedItem.Value);
        }
        if (cx_date.SelectedValue != null)
        {
            data = data.Where(p => p.Cdate == cx_date.SelectedDate);
        }
        if (cx_banci.SelectedIndex != -1)
        {
            data = data.Where(p => p.Cbanci == cx_banci.SelectedItem.Value);
        }
        
        Store1.DataSource = data;
        Store1.DataBind();
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        sm.SelectedRows.Clear();
        sm.UpdateSelection();

        btnPlan.Disabled = true;
        btnDel.Disabled = true;
    }

    protected void RowClick(object sender, AjaxEventArgs e)
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            var data = (from ct in dc.YChargetable
                        where ct.Id == decimal.Parse(sm.SelectedRecordID)
                        select ct).First();
            btnPlan.Disabled = false;
            //UserHandle.InitModule(this.PageTag);
            btnDel.Disabled=data.Cdate.Value < System.DateTime.Today;
        }
        else
        {
            btnPlan.Disabled = true;
        }
        
    }

    protected void RowClick1(object sender, AjaxEventArgs e)
    {
        RowSelectionModel sm = this.GridPanel2.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0 && sm.SelectedRows.Count<2)
        {
            btnPlaceDel.Disabled = false;
            btnUp.Disabled = false;
            btnDown.Disabled = false;
        }
        else if (sm.SelectedRows.Count > 1)
        {
            btnPlaceDel.Disabled = false;
            btnDown.Disabled = true;
            btnUp.Disabled = true;
            
        }
        else
        {
            btnPlaceDel.Disabled = true;
            btnDown.Disabled = true;
            btnUp.Disabled = true;
        }
    }

    #region 带班维护

    [AjaxMethod]
    public string BaseSave()
    {
        
        if (df_begin.SelectedValue == null || cbbBc.SelectedIndex == -1 || cbbPerson.SelectedIndex == -1)
        {
            //Ext.Msg.Alert("提示", "请填写完整信息!").Show();
            return "请填写完整信息!";
        }
        if (df_begin.SelectedDate < System.DateTime.Today)
        {
            //Ext.Msg.Alert("提示", "只能制定本日及以后的计划!").Show();
            return "只能制定本日及以后的计划!";
        }
        if (dc.YChargetable.Where(p => p.Cdate == df_begin.SelectedDate && p.Cbanci == cbbBc.SelectedItem.Value && p.Cperson == cbbPerson.SelectedItem.Value).Count() > 0)
        {
            //Ext.Msg.Alert("提示", "已添加的计划!").Show();
           return "已添加的计划!";
        }
        if (cboPlaceTemplate.SelectedIndex <= -1)
        {
            YChargetable ct = new YChargetable
            {
                Recordtime = System.DateTime.Now,
                Rperson = SessionBox.GetUserSession().PersonNumber,
                Cdate = df_begin.SelectedDate,
                Cbanci = cbbBc.SelectedItem.Value,
                Cperson = cbbPerson.SelectedItem.Value,
                Maindept = SessionBox.GetUserSession().DeptNumber,
                Status = 1
            };
            dc.YChargetable.InsertOnSubmit(ct);
            dc.SubmitChanges();
            //RecordAction(ct.Id, "新增");
            //Ext.Msg.Alert("提示", "新增成功!").Show();
            Ext.DoScript("#{Store1}.reload();");
            return "新增成功!";
        }
        else if (cboPlaceTemplate.SelectedItem.Value != "-1")
        {
            HBBLL hb = new HBBLL();
            if (hb.HaveMoveTemplate(int.Parse(cboPlaceTemplate.SelectedItem.Value)) > 0)
            {
                DateTime dt = System.DateTime.Now;
                YChargetable ct = new YChargetable
                {
                    Recordtime = dt,
                    Rperson = SessionBox.GetUserSession().PersonNumber,
                    Cdate = df_begin.SelectedDate,
                    Cbanci = cbbBc.SelectedItem.Value,
                    Cperson = cbbPerson.SelectedItem.Value,
                    Maindept = SessionBox.GetUserSession().DeptNumber,
                    Status = 1
                };
                dc.YChargetable.InsertOnSubmit(ct);
                dc.SubmitChanges();
                foreach (YPPTDetail pt in hb.GetYPPTDetail(int.Parse(cboPlaceTemplate.SelectedItem.Value), "", SessionBox.GetUserSession().DeptNumber, ""))
                {
                    YPlanplace pp = new YPlanplace
                    {
                        Ctid = ct.Id,
                        Recordtime = dt,
                        Placeid = decimal.Parse(pt.PlaceId.ToString()),
                        Moveorder = decimal.Parse(pt.MoveOrder.ToString())
                    };
                    hb.AddYPlanplace(pp.Recordtime, pp.Placeid, pp.Moveorder.Value, pp.Ctid);
                    //dc.YPlanplace.InsertOnSubmit(pp);
                    //dc.SubmitChanges();

                }
                //RecordAction(ct.Id, "新增");
                //RecordAction(ct.Id, "添加地点");
                //Ext.Msg.Alert("提示", "新增成功!").Show();
                //storeload();
                Ext.DoScript("#{Store1}.reload();");
                return "新增成功!";
            }
            else
            {
                //Ext.Msg.Alert("提示", "请确保模板里有走动线路!").Show();
                return "请确保模板里有走动线路!";
            }
        }
        else
        {
            //Ext.Msg.Alert("提示", "未知错误!").Show(); 
            return "未知错误！";
        }
    }

    [AjaxMethod]
    public void DelmsgShow()
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count == 0)
        {
            Ext.Msg.Alert("提示", "请选择需删除的带班信息!").Show();
            return;
        }
        var data = (from ct in dc.YChargetable
                    where ct.Id == decimal.Parse(sm.SelectedRecordID)
                    select ct).First();
        if(data.Cdate.Value<System.DateTime.Today)
        {
            Ext.Msg.Alert("提示", "不能删除历史带班信息!").Show();
            return;
        }
        Ext.Msg.Confirm("提示", "是否确定删除选中带班信息?", new MessageBox.ButtonsConfig
        {
            Yes = new MessageBox.ButtonConfig
            {
                Handler = "Coolite.AjaxMethods.SuerDelete();",
                Text = "确 定"
            },
            No = new MessageBox.ButtonConfig
            {
                Text = "取 消"
            }
        }).Show();
    }

    [AjaxMethod]
    public void SuerDelete()
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        var detail = dc.YPlanplace.Where(p => p.Ctid == Decimal.Parse(sm.SelectedRecordID));
        var ct = dc.YChargetable.First(p => p.Id == Decimal.Parse(sm.SelectedRecordID));
        RecordAction(ct.Id, "删除");
        dc.YPlanplace.DeleteAllOnSubmit(detail);
        dc.YChargetable.DeleteOnSubmit(ct);
        dc.SubmitChanges();
        Ext.Msg.Alert("提示", "删除成功!").Show();
        storeload();
    }

    #endregion

    #region 行走路线管理

    [AjaxMethod]
    public void PlanLoad()
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            var ct = dc.YChargetable.First(p => p.Id == int.Parse(sm.SelectedRecordID));
            if (ct.Cdate.Value < System.DateTime.Today || SessionBox.GetUserSession().rolelevel.Contains("1") || SessionBox.GetUserSession().rolelevel.Contains("0"))
            {
                tb2.Hidden = true;
            }
            else
            {
                tb2.Hidden = false;
                btnPlaceAdd.Disabled = false;
                lblDetail.Text = "带班人员：" + dc.Person.First(p => p.Personnumber == ct.Cperson).Name + ";带班班次:" + ct.Cdate.Value.ToString("yyyy-MM-dd") + ct.Cbanci;
            }
            PlanPlaceStoreLoad(Decimal.Parse(sm.SelectedRecordID));
            DetailWindow.Show();
        }
    }

    private void PlanPlaceStoreLoad(Decimal dt)
    {
        var detail = from pp in dc.YPlanplace
                     from p in dc.Place
                     where pp.Placeid == p.Placeid && pp.Ctid == dt
                     orderby pp.Moveorder
                     select new
                     {
                         pp.Recordtime,
                         pp.Placeid,
                         pp.Moveorder,
                         p.Placename,
                         pp.Id,
                         pp.Ctid
                     };
        Store2.DataSource = detail;
        Store2.DataBind();
        btnPlaceDel.Disabled = true;

    }

    [AjaxMethod]
    public void PlaceAdd()
    {
        if (cbbplace.SelectedIndex == -1)
        {
            Ext.Msg.Alert("提示", "请选择地点!").Show();
            return;
        }
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        var detail = dc.YPlanplace.Where(p => p.Ctid == Decimal.Parse(sm.SelectedRecordID)).OrderByDescending(p => p.Moveorder);
        decimal index = 1;
        if (detail.Count() > 0)
        {
            //foreach (var r in detail)
            //{
            //    index = r.Moveorder.Value + 1;
            //    break;
            //}
            index = (from d in detail
                     select d.Moveorder).Max().Value + 1;
        }
        YPlanplace pp = new YPlanplace
        {
            
            Recordtime = DateTime.Now,
            Placeid = decimal.Parse(cbbplace.SelectedItem.Value),
            Moveorder = index,
            Ctid = Decimal.Parse(sm.SelectedRecordID)
        };
        //dc.YPlanplace.InsertOnSubmit(pp);
        try
        {
            HBBLL hb = new HBBLL();
            hb.AddYPlanplace(pp.Recordtime, pp.Placeid, pp.Moveorder.Value, pp.Ctid);
            //dc.SubmitChanges();
        }
        catch (Exception ex)
        {
            Ext.Msg.Alert("错误", ex.Message).Show();
            return;
        }
        RecordAction(Decimal.Parse(sm.SelectedRecordID), "添加地点");
        cbbplace.Value = null;
        Ext.Msg.Alert("提示", "添加成功!").Show();
        PlanPlaceStoreLoad(Decimal.Parse(sm.SelectedRecordID));

    }

    [AjaxMethod]
    public void PlaceDel()
    {
        RowSelectionModel sm = this.GridPanel2.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            RowSelectionModel sm1 = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            foreach (var r in sm.SelectedRows)
            {
                DBSCMDataContext db = new DBSCMDataContext();
                var pp = db.YPlanplace.First(p => p.Ctid == Decimal.Parse(sm1.SelectedRecordID) && p.Id == decimal.Parse(r.RecordID));
                db.YPlanplace.DeleteOnSubmit(pp);
                db.SubmitChanges();
            }
            ReSortPlanPlace(Decimal.Parse(sm1.SelectedRecordID));
            RecordAction(Decimal.Parse(sm1.SelectedRecordID), "删除地点");
            Ext.Msg.Alert("提示", "删除成功!").Show();
            PlanPlaceStoreLoad(Decimal.Parse(sm1.SelectedRecordID));
        }
        else
        {
            Ext.Msg.Alert("提示", "没有选择需要删除信息!").Show();
        }
    }
    [AjaxMethod]
    public void PlaceUp()
    {
        RowSelectionModel sm = this.GridPanel2.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count ==1 )
        {
            RowSelectionModel sm1 = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            
            
            //DBSCMDataContext db = new DBSCMDataContext();
            var pp = dc.YPlanplace.First(p => p.Ctid == Decimal.Parse(sm1.SelectedRecordID) && p.Id == decimal.Parse(sm.SelectedRows[0].RecordID));
            HBBLL hb = new HBBLL();
            hb.MoveUp2(pp.Id, pp.Moveorder.Value, pp.Ctid);
            
            PlanPlaceStoreLoad(Decimal.Parse(sm1.SelectedRecordID));
        }
    }
    [AjaxMethod]
    public void PlaceDown()
    {
        RowSelectionModel sm = this.GridPanel2.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count == 1)
        {
            RowSelectionModel sm1 = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;


            //DBSCMDataContext db = new DBSCMDataContext();
            var pp = dc.YPlanplace.First(p => p.Ctid == Decimal.Parse(sm1.SelectedRecordID) && p.Id == decimal.Parse(sm.SelectedRows[0].RecordID));
            HBBLL hb = new HBBLL();
            hb.MoveDown2(pp.Id, pp.Moveorder.Value, pp.Ctid);

            PlanPlaceStoreLoad(Decimal.Parse(sm1.SelectedRecordID));
        }
    }
    private void ReSortPlanPlace(Decimal dt)
    {
        var detail = dc.YPlanplace.Where(p => p.Ctid == dt).OrderBy(p => p.Moveorder);
        decimal index = 1;
        foreach (var r in detail)
        {
            r.Moveorder = index;
            index++;
        }
        dc.SubmitChanges();
    }
    #endregion

    #region 拼音检索

    [AjaxMethod]
    public void PYsearch(string py, string store)
    {
        HBBLL hb = new HBBLL();
        switch (store.Trim())
        {
            //case "perStore":
            //    var data = hb.GetLeaderRole(py, SessionBox.GetUserSession().DeptNumber, 431);
            //    var q = from p in data
            //            select new
            //            {
            //                Personnumber = p.personnumber,
            //                Name = p.name
            //            };
            //    perStore.DataSource = q;
            //    perStore.DataBind();
            //    break;
            case "placeStore":
                var place = from pl in dc.Place
                            where pl.Maindeptid == SessionBox.GetUserSession().DeptNumber && pl.Placestatus == 1
                            && (dc.F_PINYIN(pl.Placename) + pl.Placename).ToLower().Contains(py.ToLower())
                            select new
                            {
                                pl.Placeid,
                                pl.Placename
                            };
                placeStore.DataSource = place;
                placeStore.DataBind();
                break;
        }
    }
    //[AjaxMethod]
    //public void PYsearch(string py, string store)
    //{
    //    
    //    switch (store.Trim())
    //    {
    //        case "perStore":
    //            var q = from p in dc.Person
    //                    where p.Maindeptid == SessionBox.GetUserSession().DeptNumber
    //                    && (p.Pinyin + p.Name + p.Personnumber).ToLower().Contains(py.ToLower()) && p.Personstatus == 1
    //                    select new
    //                    {
    //                        p.Personnumber,
    //                        p.Name
    //                    };
    //            perStore.DataSource = q;
    //            perStore.DataBind();
    //            break;
    //        case "placeStore":
    //            var place = from pl in dc.Place
    //                        where pl.Maindeptid == SessionBox.GetUserSession().DeptNumber && pl.Placestatus == 1
    //                        && (dc.F_PINYIN(pl.Placename) + pl.Placename).ToLower().Contains(py.ToLower())
    //                        select new
    //                        {
    //                            pl.Placeid,
    //                            pl.Placename
    //                        };
    //            placeStore.DataSource = place;
    //            placeStore.DataBind();
    //            break;
    //    }
    //}
    #endregion
    protected void CitiesRefresh(object sender, StoreRefreshDataEventArgs e)
    {
        HBBLL hb = new HBBLL();

        var data2 = hb.GetYPPT(cbbPerson.SelectedItem.Value, SessionBox.GetUserSession().DeptNumber);
        var pt = from r in data2
                 select new
                 {
                     Id = r.Id,
                     Name = r.TLevel=="矿级" ? string.Format("{0}({1})",r.Name,r.TLevel) : r.Name,
                     Level = r.TLevel
                 };
        placeTemplateStore.DataSource = pt;
        placeTemplateStore.DataBind();
        //cboPlaceTemplate.Items.Insert(0, new Coolite.Ext.Web.ListItem("--不使用模板--", "-1"));
        
    }

    protected void YPPTDetailStore_RefershData(object sender, StoreRefreshDataEventArgs e)
    {
        if (cboPlaceTemplate.SelectedIndex > 0)
        {

            if (cboPlaceTemplate.SelectedItem.Value != "-1")
            {
                HBBLL hb = new HBBLL();

                var data2 = hb.GetYPPTDetail(int.Parse(cboPlaceTemplate.SelectedItem.Value), "", SessionBox.GetUserSession().DeptNumber, "");

                this.YPPTDetailStore.DataSource = data2;
                this.YPPTDetailStore.DataBind();
            }
        }

    }
    [AjaxMethod]
    public void Changed()//处别选择下拉事件
    {
        if (cboPlaceTemplate.SelectedIndex > 0)
        {
            Ext.DoScript("#{YPPTDetailStore}.reload();");
            btnLookRoute.Disabled = false;
        }
        else
        {
            btnLookRoute.Disabled = true;
        }

    }
    #region 记录操作

    private void RecordAction(Decimal recordtime, string action)
    {
        var ct = dc.YChargetable.First(p => p.Id== recordtime);
        YCtAction ya = new YCtAction
        {
            Arecordtime = System.DateTime.Now,
            Recordtime = ct.Recordtime,
            Cdate = ct.Cdate,
            Cbanci = ct.Cbanci,
            Cperson = ct.Cperson,
            Aperson = SessionBox.GetUserSession().PersonNumber,
            Action = action
        };
        dc.YCtAction.InsertOnSubmit(ya);
        dc.SubmitChanges();
    }

    #endregion
}