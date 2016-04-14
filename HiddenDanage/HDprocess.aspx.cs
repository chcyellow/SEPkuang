using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Coolite.Ext.Web;
using GhtnTech.SEP.DAL;
using GhtnTech.SecurityFramework.BLL;
using System.Data;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.Linq;
using System.Web.Script.Serialization;
/* 说明：
 * LimitsLoad()在配好权限后要重写
 * storeload(),SearchLoad()可改
 * YHpublish()需取登陆人编号
 * 复查不通过转入新流程为写，库没定
 
 */

public partial class HiddenDanage_HDprocess : BasePage
{
    DBSCMDataContext dc = new DBSCMDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            LimitsLoad();//加载权限
            AutoDoSth();//自动处理
            SearchLoad();//加载查询数据
            
            personload();//加载人员信息
            leaderload();//加载矿领导
            //设置初始属性
            btn_detail.Disabled = true;
            btn_zgfk.Disabled = true;
            btn_fcfk.Disabled = true;
            btn_publish.Disabled = true;
            btn_action.Disabled = true;
            Button5.Disabled = true;
            ComboBox2.SelectedItem.Value = "现场整改";
            ComboBox2.Disabled = true;

            btn_AgStatus.Disabled = true;

            fb_zgqx.MinDate = System.DateTime.Today;//.AddDays(1);

            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            sm.SelectedRows.Clear();
            sm.UpdateSelection();

            //加载Grid数据
            Session["TempSet"] = "True";//初次加载不要太多数据
            storeload();


            zrdwLoad();//添加本矿责任单位
            znksLoad();//添加本矿职能科室

        }  
    }

    private void zrdwLoad()
    {
        var dept = from d in dc.Department
                   where d.Deptnumber.Substring(0, 4) == SessionBox.GetUserSession().DeptNumber.Substring(0, 4)
                   //&& d.Deptnumber.Substring(7) == "00"
                   orderby d.Deptname
                   select new
                   {
                       d.Deptnumber,
                       d.Deptname
                   };
        //var dept = (from pl in dc.Vgetpl
        //            where pl.Moduletag == this.PageTag && pl.Operatortag == "YH_zgfk" && pl.Unitid==SessionBox.GetUserSession().DeptNumber
        //            select new
        //            {
        //                pl.Deptnumber,
        //                pl.Deptname
        //            }).Distinct();
        fb_zrdw.Items.Clear();
        foreach (var r in dept)
        {
            fb_zrdw.Items.Add(new Coolite.Ext.Web.ListItem(r.Deptname, r.Deptnumber));
        }
    }

    private void znksLoad()
    {
        var dept = (from pl in dc.Vgetpl
                    where pl.Moduletag == this.PageTag && pl.Operatortag == "YH_znkssh" && pl.Unitid == SessionBox.GetUserSession().DeptNumber
                    select new
                    {
                        pl.Deptnumber,
                        pl.Deptname
                    }).Distinct();
        refer_dept.Items.Clear();
        foreach (var r in dept)
        {
            refer_dept.Items.Add(new Coolite.Ext.Web.ListItem(r.Deptname, r.Deptnumber));
        }
    }

    [AjaxMethod]
    public void PYsearch(string py, string store)//拼音检索
    {
        //if (py.Trim() == "")
        //{
        //    return;
        //}
        switch (store.Trim())
        {
            case "PersStore":
                var q = from p in dc.Person
                        where p.Areadeptid == fb_zrdw.SelectedItem.Value && p.Pinyin.ToLower().Contains(py.ToLower()) 
                        select new
                        {
                            p.Personnumber,
                            p.Name
                        };
                PersStore.DataSource = q;
                PersStore.DataBind();
                break;
            case "personStore":
                var q1 = from p in dc.Person
                        where p.Maindeptid == SessionBox.GetUserSession().DeptNumber && p.Pinyin.ToLower().Contains(py.ToLower())
                        select new
                        {
                            p.Personnumber,
                            p.Name
                        };
                personStore.DataSource = q1;
                personStore.DataBind();
                break;
        }
    }
    //根据隐患状态绑定隐患信息
    private void BindYHbyState(string stateYH)
    {
        var yhInfo = from a in dc.Yhview
                      where a.Status == stateYH
                      select a;
        Store1.DataSource = yhInfo;
        Store1.DataBind();
    }

    protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)//已改
    {
        storeload();
    }

    private void AutoDoSth()//自动处理流程 已改
    {
        GhtnTech.SEP.OraclDAL.DALGetYHINFO yh = new GhtnTech.SEP.OraclDAL.DALGetYHINFO();
        yh.AutoSetYHStatus();
    }

    private void LimitsLoad()//账户进入读取权限--已改
    {
        ////Add by zhuyu 2010-01-30 20:32
        //if (!UserHandle.ValidationHandle(PermissionTag.Delete))
        //{
        //    btn_AgStatus.Visible = false;
        //}
        ////End 
        if (!SessionBox.CheckUserSession())
        {
            Response.Redirect("~/Login.aspx");
        }
        else
        {

            UserHandle.InitModule(this.PageTag);//初始化此模块的权限。
            if (UserHandle.ValidationHandle(PermissionTag.Browse))//是否有浏览权限
            {
                if (!UserHandle.ValidationHandle(PermissionTag.YH_fbtj) && !UserHandle.ValidationHandle(PermissionTag.YH_kldsh) && !UserHandle.ValidationHandle(PermissionTag.YH_znkssh))//发布提交、领导审核、职能科室审核
                {
                    btn_publish.Visible = false;
                }
                if (!UserHandle.ValidationHandle(PermissionTag.YH_zgfk))//整改反馈
                {
                    btn_zgfk.Visible = false;
                }
                if (!UserHandle.ValidationHandle(PermissionTag.YH_fcfk))//复查反馈
                {
                    btn_fcfk.Visible = false;
                }
                if (!UserHandle.ValidationHandle(PermissionTag.YH_bhxz))//闭合新增
                {
                    ts2.Visible = false;
                    Button5.Visible = false;
                }
                if (!UserHandle.ValidationHandle(PermissionTag.YH_fjpccl))//分级排查处理
                {
                    Button8.Visible = false;
                }
                if (!UserHandle.ValidationHandle(PermissionTag.YH_yclccl))//异常流程处理
                {
                    ts3.Visible = false;
                    btn_action.Visible = false;
                }
                if (!UserHandle.ValidationHandle(PermissionTag.Delete))//删除隐患权限--未改
                {
                    btn_AgStatus.Visible = false;
                }

                
            }
        }
        
    }

    private void personload()
    {
        //绑定人员
        var person = from r in dc.Person
                     where r.Maindeptid == SessionBox.GetUserSession().DeptNumber
                     select new
                     {
                         r.Personnumber,
                         r.Name
                     };
        personStore.DataSource = person;
        personStore.DataBind();
    }
    //提交领导时-绑定领导-已改--本矿领导
    private void leaderload()
    {
        //分管领导
        var q = from pl in dc.Vgetpl
                where pl.Moduletag == this.PageTag && pl.Operatortag == PermissionTag.YH_kldsh && pl.Unitid==SessionBox.GetUserSession().DeptNumber
                select new
                {
                    pl.Personnumber,
                    pl.Name
                };

        FBBigleader.DataSource = q;
        FBBigleader.DataBind();
        //矿领导
        var leader = from pl in dc.Vgetpl
                     where pl.Moduletag == this.PageTag && pl.Operatortag == PermissionTag.YH_kldsh && pl.Unitid == SessionBox.GetUserSession().DeptNumber
                     select new
                     {
                         pl.Personnumber,
                         pl.Name
                     };
        //var leader = from p in dc.Person
        //             from pos in dc.Position
        //             where p.Posid == pos.Posid && pos.Movegblevel == "大领导"
        //             select new
        //             {
        //                 p.Personnumber,
        //                 p.Name
        //             };
        FBBigleader.DataSource = leader;
        FBBigleader.DataBind();
    }

    protected void DblClick(object sender, AjaxEventArgs e)//双击每行弹出处理事件 无需修改
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            switch (Session["HD_act"].ToString().Trim())
            {
                case "1":
                    detailFine();
                    Window2.Show();
                    break;
                case "2":
                    detailzgr();
                    Window3.Show();
                    break;
                case "3":
                    detailfcr();
                    Window4.Show();
                    break;
                case "4":
                    YHycfkLoad();
                    Window5.Show();
                    break;
                case "":
                    DetailLoad();
                    DetailWindow.Show();
                    break;

            }
        }
    }

    #region 单击每行-根据不同状态获取权限 已改
    
    protected void RowClick(object sender, AjaxEventArgs e)//流程处理
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            btn_detail.Disabled = false;
            btn_action.Disabled = true;
            Button5.Disabled = true;
            btn_AgStatus.Disabled = false;
            var Input = dc.Yhinput.Where(p => p.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));

            UserHandle.InitModule(this.PageTag);//初始化此模块的权限。
            
            foreach (var row in Input)
            {
                switch (row.Status.Trim())
                {
                    case "新增":
                        btn_zgfk.Disabled = true;
                        btn_fcfk.Disabled = true;
                        Button5.Disabled = false;
                        btn_publish.Disabled = false;
                        Tab1.Disabled = false;
                        Tab2.Disabled = false;
                        Tab3.Disabled = false;
                        Button8.Disabled = true;
                        
                        break;
                    case "提交审批":
                        btn_zgfk.Disabled = true;
                        btn_fcfk.Disabled = true;
                        //btn_publish.Disabled = false;
                        Tab1.Disabled = false;
                        Tab2.Disabled = true;
                        Tab3.Disabled = true;
                        Button5.Disabled = true;
                        Button8.Disabled = true;
                        if (row.Refer.Value == 1)//提交领导
                        {
                            //判断是否拥有权限
                            btn_publish.Disabled = !UserHandle.ValidationHandle(PermissionTag.YH_kldsh);
                        }
                        else if (row.Refer.Value == 2)//提交职能部门
                        {
                            //判断是否拥有权限
                            btn_publish.Disabled = !UserHandle.ValidationHandle(PermissionTag.YH_znkssh);
                        }
                        break;
                    case "隐患未整改":
                    case "隐患整改中":
                        btn_zgfk.Disabled = false;
                        btn_fcfk.Disabled = true;
                        btn_publish.Disabled = true;
                        Button5.Disabled = true;
                        Button8.Disabled = true;
                        break;
                    case "隐患已整改":
                        btn_zgfk.Disabled = true;
                        btn_fcfk.Disabled = false;
                        btn_publish.Disabled = true;
                        Button5.Disabled = true;
                        Button8.Disabled = true;
                        break;
                    case "现场整改":
                    case "复查通过":
                        btn_zgfk.Disabled = true;
                        btn_fcfk.Disabled = true;
                        btn_publish.Disabled = true;
                        Button5.Disabled = true;
                        Button8.Disabled = true;
                        break;
                    case "逾期未整改":
                        Button8.Disabled = !UserHandle.ValidationHandle(PermissionTag.YH_fjpccl);//分级处理权限
                        btn_action.Disabled = false;
                        btn_zgfk.Disabled = true;
                        btn_fcfk.Disabled = true;
                        btn_publish.Disabled = true;
                        Button5.Disabled = true;
                        break;
                    case "逾期整改未完成":
                    case "复查未通过":
                        btn_action.Disabled = false;
                        btn_zgfk.Disabled = true;
                        btn_fcfk.Disabled = true;
                        btn_publish.Disabled = true;
                        Button5.Disabled = true;
                        Button8.Disabled = !UserHandle.ValidationHandle(PermissionTag.YH_fjpccl);//分级处理权限
                        break;
                    case "系统不受理":
                        btn_detail.Disabled = true;
                        btn_zgfk.Disabled = true;
                        btn_fcfk.Disabled = true;
                        btn_publish.Disabled = true;
                        btn_action.Disabled = true;
                        Button5.Disabled = true;
                        Button8.Disabled = true;
                        break;
                }
            }
        }
        else
        {
            btn_zgfk.Disabled = true;
            btn_fcfk.Disabled = true;
            btn_detail.Disabled = true;
            btn_publish.Disabled = true;
            btn_AgStatus.Disabled = true;
            Button5.Disabled = true;
            Button8.Disabled = true;
        }
        Session["HD_act"] = "";
        if (btn_publish.Visible == true && btn_publish.Disabled == false)
        {
            Session["HD_act"] = "1";
        }
        if (btn_zgfk.Visible == true && btn_zgfk.Disabled == false)
        {
            Session["HD_act"] = "2";
        }
        if (btn_fcfk.Visible == true && btn_fcfk.Disabled == false)
        {
            Session["HD_act"] = "3";
        }
        if (btn_action.Visible == true && btn_action.Disabled == false)
        {
            Session["HD_act"] = "4";
        }
    }
    #endregion

    #region 初始化页面-数据绑定 已改 
    private void storeload()//执行查询
    {

        if (Request.QueryString["YHIDgroup"] != null)
        {
            string[] strgroup = Request.QueryString["YHIDgroup"].Trim().Split(',');
            decimal[] dgroup;
            if (strgroup.Length == 0)
            {
                dgroup = new decimal[] { -1 };
            }
            else
            {
                dgroup = new decimal[strgroup.Length];
                for (int i = 0; i < strgroup.Length; i++)
                {
                    dgroup[i] = decimal.Parse(strgroup[i]);
                }
            }
            var data = from yh in dc.Yhview
                       where dgroup.Contains(yh.Yhputinid)
                       select yh;
            Store1.DataSource = data;//dc.GetYHbyIDgroup(Request["YHIDgroup"].Trim());
            Store1.DataBind();
        }
        else if (Request.QueryString["DeptID"] != null)
        {
            var data = from yh in dc.Yhview
                       where yh.Deptid == Request["DeptID"].Trim()
                       && yh.Status == "现场整改"
                       select yh;
            if (!df_begin.IsNull)
            {
                data = data.Where(p => p.Pctime >= df_begin.SelectedDate);
            }
            if (!df_end.IsNull)
            {
                data = data.Where(p => p.Pctime <= df_end.SelectedDate);
            }
            if (!nf_ID.IsNull)
            {
                data = data.Where(p => p.Yhputinid == decimal.Parse(nf_ID.Text.Trim()));
            }


            Store1.DataSource = data;
            Store1.DataBind();

        }
        else if (Request.QueryString["DeptID1"] != null)
        {
            var data = from yh in dc.Yhview
                       where yh.Deptid == Request["DeptID1"].Trim()
                       && yh.Status == "逾期未整改"
                       select yh;
            if (!df_begin.IsNull)
            {
                data = data.Where(p => p.Pctime >= df_begin.SelectedDate);
            }
            if (!df_end.IsNull)
            {
                data = data.Where(p => p.Pctime <= df_end.SelectedDate);
            }
            if (!nf_ID.IsNull)
            {
                data = data.Where(p => p.Yhputinid == decimal.Parse(nf_ID.Text.Trim()));
            }

            Store1.DataSource = data;
            Store1.DataBind();

        }
        else if (Request.QueryString["YHState"] != null)
        {
            var data = from yh in dc.Yhview
                       where yh.Status == Request["YHState"].ToString().Trim() && yh.Maindeptid==SessionBox.GetUserSession().DeptNumber
                       select yh;
            if (!df_begin.IsNull)
            {
                data = data.Where(p => p.Pctime >= df_begin.SelectedDate);
            }
            if (!df_end.IsNull)
            {
                data = data.Where(p => p.Pctime <= df_end.SelectedDate);
            }
            if (!nf_ID.IsNull)
            {
                data = data.Where(p => p.Yhputinid == decimal.Parse(nf_ID.Text.Trim()));
            }

            Store1.DataSource = data;
            Store1.DataBind();

        }
        else
        {
            UserHandle.InitModule(this.PageTag);//初始化此模块的权限。
            var yhdata = from yh in dc.Yhview where false select yh;
            if (UserHandle.ValidationHandle(PermissionTag.YH_kldsh))//拥有领导审核权限--矿领导
            {
                yhdata = yhdata.Union(
                    from yh in dc.Yhview
                    where yh.Status == "提交审批"
                    && yh.Refer == 1 && yh.Referid == SessionBox.GetUserSession().PersonNumber
                    select yh
                );

            }
            if (UserHandle.ValidationHandle(PermissionTag.Browse))//拥有浏览权限--所有人
            {
                yhdata = yhdata.Union(
                    from yh in dc.Yhview
                    where yh.Status == "复查通过" || yh.Status == "现场整改"
                    select yh
                );
            }

            if (UserHandle.ValidationHandle(PermissionTag.YH_fcfk))//拥有复查权限--走动干部
            {
                yhdata = yhdata.Union(
                    from yh in dc.Yhview
                    where yh.Status == "隐患已整改"
                    select yh
                );
            }


            if (UserHandle.ValidationHandle(PermissionTag.YH_zgfk))//拥有整改反馈权限--责任部门
            {
                yhdata = yhdata.Union(
                    from yh in dc.Yhview
                    from yc in dc.Yinhuancheck
                    where yh.Yhputinid==yc.Yhputinid && yh.Status == "隐患未整改" && yc.Responsibledept == GetDeptIDViaPersonNumber(SessionBox.GetUserSession().PersonNumber)
                    select yh
                );
            }

            if (UserHandle.ValidationHandle(PermissionTag.YH_znkssh))//拥有职能科室审核权限--职能科室
            {
                yhdata = yhdata.Union(
                    from yh in dc.Yhview
                    where yh.Status == "提交审批"
                    && yh.Refer == 2 && yh.Referid == GetDeptIDViaPersonNumber(SessionBox.GetUserSession().PersonNumber)
                    select yh
                );
            }

            if (UserHandle.ValidationHandle(PermissionTag.YH_fbtj) || UserHandle.ValidationHandle(PermissionTag.YH_bhxz))//拥有发布权限或闭合新增隐患权限--安监人员
            {
                yhdata = yhdata.Union(
                    from yh in dc.Yhview
                    where yh.Status == "新增"
                    select yh
                );
            }

            if (UserHandle.ValidationHandle(PermissionTag.YH_fjpccl))//拥有分级排查权限--安监人员
            {
                yhdata = yhdata.Union(
                    from yh in dc.Yhview
                    where new string[]{"逾期整改未完成","逾期未整改","复查未通过"}.Contains( yh.Status )
                    select yh
                );
            }

            if (UserHandle.ValidationHandle(PermissionTag.YH_yclccl))//拥有异常流程权限--安监人员
            {
                if (Session["TempSet"].ToString().Trim() == "True")//初始登陆过滤
                {
                    yhdata = yhdata.Where(p => p.Status != "隐患已整改" && p.Status != "提交审批");
                }
                else
                {
                    yhdata = from yh in dc.Yhview select yh;
                }
            }

            if (Session["TempSet"].ToString().Trim() == "True")//初始登陆过滤
            {
                yhdata = yhdata.Where(p => p.Status != "现场整改" && p.Status != "复查通过");
            }

            yhdata = yhdata.Where(p => p.Maindeptid == SessionBox.GetUserSession().DeptNumber);//只处理本矿的


            if (!df_begin.IsNull)
            {
                yhdata = yhdata.Where(p => p.Pctime >= df_begin.SelectedDate);
            }
            if (!df_end.IsNull)
            {
                yhdata = yhdata.Where(p => p.Pctime <= df_end.SelectedDate);
            }
            if (!nf_ID.IsNull)
            {
                yhdata = yhdata.Where(p => p.Yhputinid == decimal.Parse(nf_ID.Text.Trim()));
            }
            if (cbb_person.SelectedIndex > -1)
            {
                yhdata = yhdata.Where(p => p.Personid == cbb_person.SelectedItem.Value.Trim());
            }
            if (cbb_part.SelectedIndex > -1)
            {
                yhdata = yhdata.Where(p => p.Deptid == cbb_part.SelectedItem.Value.Trim());
            }
            if (cbb_lavel.SelectedIndex > -1)
            {
                yhdata = yhdata.Where(p => p.Levelid == decimal.Parse(cbb_lavel.SelectedItem.Value.Trim()));
            }
            if (cbb_kind.SelectedIndex > -1)
            {
                yhdata = yhdata.Where(p => p.Zyid == decimal.Parse(cbb_kind.SelectedItem.Value.Trim()));
            }
            if (cbb_status.SelectedIndex > -1)
            {
                yhdata = yhdata.Where(p => p.Status == cbb_status.SelectedItem.Value.Trim());
            }

            if (cbb_place.SelectedIndex > -1)
            {
                yhdata = yhdata.Where(p => p.Placeid == decimal.Parse(cbb_place.SelectedItem.Value.Trim()));
            }

            Store1.DataSource = yhdata.OrderByDescending(p => p.Pctime) ;
            Store1.DataBind();

            //btn_detail.Disabled = true;
            //btn_zgfk.Disabled = true;
            //btn_fcfk.Disabled = true;
            //btn_publish.Disabled = true;
            //btn_action.Disabled = true;
            //Button5.Disabled = true;
            //Button8.Disabled = true;
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            sm.SelectedRows.Clear();
            sm.UpdateSelection();
        }
    }

    [AjaxMethod]
    public void LoadHazard()//加载管理标准
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        var info=from yh in dc.Yhinput
                 from ha in dc.Hazards
                 where yh.Yhnumber==ha.HNumber && yh.Yhputinid==decimal.Parse( sm.SelectedRow.RecordID)
                 select new 
                 {
                     ha.MStandards
                 };
        fb_zgcs.Text = info.First().MStandards;
    }


    protected string GetDeptIDViaPersonNumber(string pn)//获取登陆用户单位ID
    {
        try
        {
            return (from psn in dc.Person where psn.Personnumber == pn select psn.Areadeptid).Single().Trim();
        }
        catch
        {
            return "";
        }
    }

    #endregion

    #region 查询窗口操作 已改

    protected void PersonRefresh(object sender, StoreRefreshDataEventArgs e)//查询人员刷新
    {
        
        ////ArrayList Role = (ArrayList)SessionBox.GetUserSession().RoleID;
        //ArrayList Role = (ArrayList)SessionBox.GetUserSession().CurrentRole;
        //string rolename = Role[0].ToString().Substring(Role[0].ToString().IndexOf(",") + 1).Trim();
        //if (rolename == "走动干部")
        //{
        //    var q = dc.Person.Where(p => p.PosID == Convert.ToInt32(cbb_position.SelectedItem.Value) && p.PersonNumber==SessionBox.GetUserSession().PersonNumber);
        //    Store2.DataSource = q;
        //    Store2.DataBind();
        //    cbb_person.Disabled = q.Count() > 0 ? false : true;
        //}
        //else
        //{
            var q = dc.Person.Where(p => p.Posid == Convert.ToInt32(cbb_position.SelectedItem.Value));
            Store2.DataSource = q;
            Store2.DataBind();
            cbb_person.Disabled = q.Count() > 0 ? false : true;
        //}
    }

    protected void perRefresh(object sender, StoreRefreshDataEventArgs e)//发送短信人员刷新--修改科区内选择人员
    {
        //var q = dc.Person.Where(p => p.Deptid == refer_dept.SelectedItem.Value);
        var q = from pl in dc.Vgetpl
                where pl.Moduletag == this.PageTag && pl.Operatortag == "YH_znkssh" && pl.Deptnumber == refer_dept.SelectedItem.Value
                select new
                {
                    pl.Personnumber,
                    pl.Name
                };
        perStore.DataSource = q;
        perStore.DataBind();
        refer_Rname.Disabled = q.Count() > 0 ? false : true;
    }

    private void SearchLoad()//查询窗口初始化
    {
        //初始化日期
        //df_begin.SelectedDate = System.DateTime.Today;
        //df_end.SelectedDate = System.DateTime.Today;

        //ArrayList Role = (ArrayList)SessionBox.GetUserSession().CurrentRole;
        //string rolename = Role[0].ToString().Substring(Role[0].ToString().IndexOf(",") + 1).Trim();
        //string perID = SessionBox.GetUserSession().PersonNumber;
        #region 初始化职务
        //if (rolename == "走动干部")
        //{
        //    var pos = from p in dc.Position
        //              from per in dc.Person
        //              where p.PosID == per.PosID && per.PersonNumber == perID
        //              select new
        //              {
        //                  p.PosID,
        //                  p.PosName
        //              };
        //    Store3.DataSource = pos;
        //}
        //else
        //{
            var pos = from p in dc.Position
                      where p.Maindeptid == SessionBox.GetUserSession().DeptNumber
                      select new
                      {
                          p.Posid,
                          p.Posname
                      };
            Store3.DataSource = pos;
        //}
        Store3.DataBind();
        #endregion

        #region 初始化部门
        var dept = from d in dc.Department
                   where d.Deptnumber.Substring(0, 4) == SessionBox.GetUserSession().DeptNumber.Substring(0, 4)
                   && d.Deptnumber.Substring(7) == "00"
                   orderby d.Deptname
                   select new
                   {
                       d.Deptnumber,
                       d.Deptname
                   };
        DeptStore.DataSource = dept;
        DeptStore.DataBind();
        #endregion

        #region 初始化级别
        //var lavel = from l in dc.YinHuanLevel
        //            select new
        //                {
        //                    l.YHLevelID,
        //                    l.YHLevel
        //                };
        var lavel = from c in dc.CsBaseinfoset
                    where c.Fid == int.Parse(PublicMethod.ReadXmlReturnNode("YHJB", this))
                    select new
                    {
                        YHLevelID = c.Infoid,
                        YHLevel = c.Infoname
                    };
        LevelStore.DataSource = lavel;
        LevelStore.DataBind();
        #endregion

        #region 初始化类型
        //var type = from t in dc.YinHuanType
        //           select new
        //               {
        //                   t.YHTypeID,
        //                   t.YHType
        //               };
        var type = from t in dc.CsBaseinfoset
                   where t.Fid == int.Parse(PublicMethod.ReadXmlReturnNode("ZY", this))
                   orderby t.Infoid ascending
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
        //List<YHStaList> data = new List<YHStaList>();

        ////判断不同角色进入绑定不同状态值
        //if (rolename == "矿领导" || rolename == "职能部门")
        //{
        //    var Status = from s in XElement.Load(Server.MapPath("") + @"\Status.xml").Elements("status")
        //                 where Convert.ToInt32(s.Element("manage").Value.Trim()) == 3
        //                 select s;
        //    foreach (var r in Status)
        //    {
        //        YHStaList YHSta = new YHStaList();
        //        YHSta.Mstatus = r.Element("value").Value;
        //        YHSta.Limits = Convert.ToInt32(r.Element("manage").Value.Trim());
        //        data.Add(YHSta);
        //    }
        //    StatusStore.DataSource = data;
        //    StatusStore.DataBind();
        //}
        //else if (rolename == "责任部门")
        //{
        //    var Status = from s in XElement.Load(Server.MapPath("") + @"\Status.xml").Elements("status")
        //                 where Convert.ToInt32(s.Element("manage").Value.Trim()) == 4
        //                 select s;
        //    foreach (var r in Status)
        //    {
        //        YHStaList YHSta = new YHStaList();
        //        YHSta.Mstatus = r.Element("value").Value;
        //        YHSta.Limits = Convert.ToInt32(r.Element("manage").Value.Trim());
        //        data.Add(YHSta);
        //    }
        //    StatusStore.DataSource = data;
        //    StatusStore.DataBind();
        //}
        //else if (rolename == "走动干部")
        //{
        //    var Status = from s in XElement.Load(Server.MapPath("") + @"\Status.xml").Elements("status")
        //                 where Convert.ToInt32(s.Element("manage").Value.Trim()) == 1
        //                 select s;
        //    foreach (var r in Status)
        //    {
        //        YHStaList YHSta = new YHStaList();
        //        YHSta.Mstatus = r.Element("value").Value;
        //        YHSta.Limits = Convert.ToInt32(r.Element("manage").Value.Trim());
        //        data.Add(YHSta);
        //    }
        //    StatusStore.DataSource = data;
        //    StatusStore.DataBind();
        //}
        //else
        //{
        //    var Status = from s in XElement.Load(Server.MapPath("") + @"\Status.xml").Elements("status")
        //                 select s;
        //    foreach (var r in Status)
        //    {
        //        YHStaList YHSta = new YHStaList();
        //        YHSta.Mstatus = r.Element("value").Value;
        //        YHSta.Limits = Convert.ToInt32(r.Element("manage").Value.Trim());
        //        data.Add(YHSta);
        //    }
        //    StatusStore.DataSource = data;
        //    StatusStore.DataBind();
        //}
        #endregion

        #region 初始化地点
        var place = from pl in dc.Place
                    where pl.Maindeptid == SessionBox.GetUserSession().DeptNumber
                    orderby pl.Placename ascending
                    select new
                    {
                        placID = pl.Placeid,
                        placName = pl.Placename
                    };
        placeStore.DataSource = place;
        placeStore.DataBind();
        #endregion
    }

    //public class YHStaList  //XML读取隐患状态使用
    //{
    //    public YHStaList(string mstatus, int limits)
    //    {
    //        Mstatus = mstatus;
    //        Limits = limits;
    //    }

    //    public YHStaList()
    //    {
    //    }

    //    public string Mstatus { get; set; }

    //    public int Limits { get; set; }

    //}

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
        cbb_lavel.SelectedItem.Value = null;
        cbb_kind.SelectedItem.Value = null;
        cbb_status.SelectedItem.Value = null;
    }
    [AjaxMethod]
    public void Search()//点击查询按钮
    {
        //Ext.DoScript("#{Store1}.reload();");
        Session["TempSet"] = "";
        storeload();
        //Ext.DoScript("#{Store1}.load({params:{start:0,limit:" + pageToolBar.PageSize + "}});");
        Window1.Hide();
        GridPanel1.Show();
    }
    #endregion

    #region 隐患信息查询绑定 已改
    [AjaxMethod]
    public void DetailLoad()//加载明细信息
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        var input = dc.Yhinput.First(p => p.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
        int i = 0;
        switch (input.Status.Trim())
        {
            
            case "新增":
            case "提交审批":
            case "现场整改":
                i = 1;
                break;
            case "隐患未整改":
            case "隐患整改中": 
            case "逾期未整改":
            case "逾期整改未完成":
                i = 2;
                break;
            case "隐患已整改":
                i = 3;
                break;
            case "复查通过":
            case "复查未通过":
                i = 4;
                break;
        }
        if (i > 0)
        {
            SetYHbase(sm.SelectedRows[0].RecordID.Trim());
        }
        BasePanel.Disabled = i > 0 ? false : true; i--;
        if (i > 0)
        {
            SetYHZG(sm.SelectedRows[0].RecordID.Trim());
        }
        Panel1.Disabled = i > 0 ? false : true; i--;
        if (i > 0)
        {
            SetYHZGFK(sm.SelectedRows[0].RecordID.Trim());
        }
        ZGPanel.Disabled = i > 0 ? false : true; i--;
        if (i > 0)
        {
            SetYHFCFK(sm.SelectedRows[0].RecordID.Trim());
        }
        FCPanel.Disabled = i > 0 ? false : true; i--;
        Panel1.Collapsed = true;
        ZGPanel.Collapsed = true;
        FCPanel.Collapsed = true;
    }

    private void SetYHbase(string ID)//加载隐患基本信息
    {
        var data = dc.Yhview.Where(p => p.Yhputinid == int.Parse(ID));
        foreach (var r in data)
        {
            lbl_YHPutinID.Text = r.Yhputinid.ToString();
            lbl_DeptName.Text = r.Deptname.Trim();
            lbl_PlaceName.Text = r.Placename.Trim();
            lbl_YHContent.Text = r.HBm.Trim();
            try
            {
                lbl_Remarks.Text = r.Remarks.Trim();
            }
            catch
            {
                lbl_Remarks.Text = "";
            }
            lbl_BanCi.Text = r.Banci.Trim();
            lbl_rName.Text = r.Inputpersonname.Trim();
            lbl_PCTime.Text = r.Pctime.Value.ToString("yyyy年MM月dd日");
            lbl_YHLevel.Text = r.Levelname.Trim();
            lbl_YHType.Text = r.Zyname.Trim();
            lbl_Status.Text = r.Status.Trim();
        }
    }

    private void SetYHZG(string ID)//加载隐患整改信息
    {
        GhtnTech.SEP.OraclDAL.DALGetYHINFO yh = new GhtnTech.SEP.OraclDAL.DALGetYHINFO();
        DataTable dt = yh.GetYHZGbyID(ID).Tables[0];
        foreach (DataRow r in dt.Rows)
        {
            zg_Measures.Text = r["MEASURES"].ToString().Trim();
            zg_Instructions.Text = r["INSTRUCTIONS"].ToString().Trim();
            zg_PersonID.Text = r["RNAME"].ToString().Trim();
            zg_zrdw.Text = r["DEPTNAME"].ToString().Trim();
            zg_InstrTime.Text = Convert.ToDateTime(r["INSTRTIME"]).ToString("yyyy年MM月dd日");
            //zg_IsFine.Text = r.IsFine.Value ? "是" : "否";
            //---------------------****---------------------
            zg_RecLimit.Text = Convert.ToDateTime(r["RECLIMIT"]).ToString("yyyy年MM月dd日");//
            zg_BanCi.Text = r["BANCI"].ToString().Trim();//
            //zg_ReviewLimit.Text = r.ReviewLimit.Value.ToString() + "天";
            //------------------------------------------
        }
        var fine = dc.Yhaction.Where(p => p.Yhputinid == Convert.ToInt32(ID)).Sum(f => f.Fine);
        zg_IsFine.Text = Decimal.Round(fine.Value, 2).ToString() + "元";
    }

    private void SetYHZGFK(string ID)//加载隐患整改反馈信息
    {
        GhtnTech.SEP.OraclDAL.DALGetYHINFO yh = new GhtnTech.SEP.OraclDAL.DALGetYHINFO();
        DataTable dt = yh.GetYHZGFKbyID(ID).Tables[0];
        foreach (DataRow r in dt.Rows)
        {
            //---------------------****---------------------
            //zfk_RecOpinion.Text = r.RecOpinion.Trim();
            zfk_RecPersonID.Text = r["RECPERSON"].ToString().Trim();
            zfk_RecTime.Text = Convert.ToDateTime(r["RECTIME"]).ToString("yyyy年MM月dd日");
            zfk_RecState.Text = r["RECSTATE"].ToString().Trim();
            zfk_yanshouName.Text = r["YANSHOUNAME"].ToString().Trim();
            zfk_bc.Text = r["ZGBANCI"].ToString().Trim();
            //zfk_ResponsibleID.Text = r.Responsible.Trim();
            //------------------------------------------
        }
    }

    private void SetYHFCFK(string ID)//加载隐患复查反馈信息
    {
        var data = from r in dc.Yinhuanreview
                   from p in dc.Person
                   where r.Personid == p.Personnumber && r.Yhputinid == int.Parse(ID)
                   select new
                   {
                       r.Reviewopinion,
                       r.Fctime,
                       r.Reviewstate,
                       p.Name
                   };
       
        foreach (var r in data)
        {
            ffk_ReviewOpinion.Text = r.Reviewopinion.Trim();
            ffk_PersonID.Text = r.Name.Trim();
            ffk_FCTime.Text = r.Fctime.Value.ToString("yyyy年MM月dd日");
            ffk_ReviewState.Text = r.Reviewstate.Trim();
        }
    }

    [AjaxMethod]
    public void detailFine()//绑定隐患罚款金额
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        var dataFine = from yh in dc.Yhinput
                       from yhN in dc.HazardsOre 
                       where yh.Yhnumber==yhN.HNumber && yh.Maindeptid==yhN.Deptnumber && yh.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim())
                       select new
                       {
                           yhN.Punishmentstandard,//自定义处罚标准
                           yh.Deptid,
                       };
        foreach (var r in dataFine)
        {
            try
            {
                fb_fkje.Text = Decimal.Round(Decimal.Parse(r.Punishmentstandard), 2).ToString();
            }
            catch
            {
                fb_fkje.Text = "0.00";
            }
            fb_zrdw.SelectedItem.Value = r.Deptid.ToString();
            var q = from p in dc.Person
                    //from u in dc.SF_Users
                    where //p.PersonNumber == u.PersonNumber && 
                    p.Areadeptid == fb_zrdw.SelectedItem.Value
                    select new
                    {
                        p.Personnumber,//这个地方改成人员编码 以前是id
                        p.Name
                    };
            PersStore.DataSource = q;
            PersStore.DataBind();
            fb_zrr.Disabled = q.Count() > 0 ? false : true;
        }
    }

    [AjaxMethod]
    public void detailzgr()//绑定隐患整改人员
    {
        try
        {
            zgfk_zgr.SelectedItem.Value = SessionBox.GetUserSession().PersonNumber;
            zgfk_zgr.Disabled = true;
        }
        catch
        {
        }
    }

    [AjaxMethod]
    public void detailfcr()//绑定隐患复查人员
    {
        try
        {
            fcfk_fcr.SelectedItem.Value = SessionBox.GetUserSession().PersonNumber;
            fcfk_fcr.Disabled = true;
        }
        catch
        {
        }
    }

    #endregion

    #region 隐患信息流程处理 已改
    [AjaxMethod]
    public void YHpublish()//发布隐患
    {
        //---------------------****---------------------
        if (fb_zgcs.Text.Trim() == "" || fb_zgqx.SelectedValue == null || fb_bc.SelectedIndex == -1 || fb_zrdw.SelectedIndex == -1)
        {
            Ext.Msg.Alert("提示", "请填写完整信息!").Show();
            return;
        }
        if (fb_isfine.Checked)
        {
            if (fb_fkje.Text.Trim() == "" || fb_fkje.Text.Trim().IndexOf("-") != -1)
            {
                Ext.Msg.Alert("提示", "金额填写有误!").Show();
                return;
            }
        }
        //------------------------------------------
        //if (fb_zgqx.Text.Trim().IndexOf("-") != -1 || fb_fcqx.Text.Trim().IndexOf("-") != -1)
        //{
        //    Ext.Msg.Alert("提示", "期限不能为负!").Show();
        //    return;
        //}
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        var Input = dc.Yhinput.Where(p => p.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
        foreach (var row in Input)
        {
            if (row.Status.Trim() != "提交审批" && row.Status.Trim() != "新增")
            {
                Ext.Msg.Alert("提示", "当前状态不允许该操作!").Show();
                return;
            }
        }

        var YHcheck = dc.Yinhuancheck.Where(p => p.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));

        //获取整改截至时间
        string time = "00:00:00";
        switch (fb_bc.SelectedItem.Value.Trim())
        {
            case "早班":
                time=PublicMethod.ReadXmlReturnNode("ZBSJ", this);
                break;
            case "中班":
                time = PublicMethod.ReadXmlReturnNode("ZHBSJ", this);
                break;
            case "夜班":
                time = PublicMethod.ReadXmlReturnNode("WBSJ", this);
                break;
        }

        DateTime rtime = DateTime.Parse(fb_zgqx.SelectedDate.ToString("yyyy-MM-dd") + " " + time);
        if (YHcheck.Count() == 0)
        {
            //---------------------****---------------------
            Yinhuancheck yhc = new Yinhuancheck
            {
                Yhputinid = Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()),
                Measures = fb_zgcs.Text.Trim(),
                Responsibledept = fb_zrdw.SelectedItem.Value.Trim(),//
                Instructions = fb_ldps.Text.Trim(),
                Personid = SessionBox.GetUserSession().PersonNumber,//获取session中帐号信息
                Instrtime = System.DateTime.Today,
                Reclimit = rtime,//fb_zgqx.SelectedDate,//
                Reviewlimit = 2,//整改期限未删除，暂时保留
                Checktime = System.DateTime.Today,
                Banci = fb_bc.SelectedItem.Value.Trim(),//
                Isfine = fb_isfine.Checked?1:0//
            };
            if (fb_zrr.SelectedIndex != -1)//由于说是有可能责任人不填先把它放到外面
            {
                yhc.Responsibleid = fb_zrr.SelectedItem.Value.Trim();
            }
            //------------------------------------------
            dc.Yinhuancheck.InsertOnSubmit(yhc);
            dc.SubmitChanges();
        }
        else
        {
            foreach (var r in YHcheck)
            {
                //--------------------****----------------------
                r.Measures = fb_zgcs.Text.Trim();
                r.Instructions = fb_ldps.Text.Trim();
                r.Personid = SessionBox.GetUserSession().PersonNumber;//获取session中帐号信息
                r.Reclimit = rtime;//fb_zgqx.SelectedDate;
                r.Reviewlimit = 2;//
                r.Checktime = System.DateTime.Today;
                r.Instrtime = System.DateTime.Today;
                r.Responsibledept = fb_zrdw.SelectedItem.Value.Trim();
                r.Banci = fb_bc.SelectedItem.Value.Trim();
                r.Isfine = fb_isfine.Checked?1:0;
                if (fb_zrr.SelectedIndex != -1)//
                {
                    r.Responsibleid = fb_zrr.SelectedItem.Value.Trim();
                }
                //------------------------------------------
                dc.SubmitChanges();
            }
        }
        Yhaction act = new Yhaction
        {
            Detail = fb_zgcs.Text.Trim(),
            Historystatus = "新增/提交审批",
            Newstauts = "隐患未整改",
            Yhputinid = Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()),
            Fine = fb_fkje.Text.Trim() == "" ? 0 : Convert.ToDecimal(fb_fkje.Text.Trim()),
            Happendate = System.DateTime.Today
        };
        dc.Yhaction.InsertOnSubmit(act);
        dc.SubmitChanges();
        foreach (var row in Input)
        {
            row.Refer = 0;
            row.Status = "隐患未整改";
        }
        dc.SubmitChanges();
        //短信提醒责任人有待处理隐患信息--没有短信猫
        if (chkIsSend.Checked == true)
        {
            //发送短信
            Sms.Send(fb_zrr.SelectedItem.Value.Trim(), txtSms.Text.Trim() == "" ? "您有一条待处理的隐患。" : txtSms.Text.Trim(), SmsType.HiddenTroubleTips, sm.SelectedRow.RecordID.Trim(), "-安全生产体系支撑平台!");
        }
        Ext.Msg.Alert("提示", "发布成功!").Show();
        Ext.DoScript("#{Store1}.reload();");
        //storeload();
        Window2.Hide();
    }

    protected void PersRefresh(object sender, StoreRefreshDataEventArgs e)//发布/提交选择责任部门人员刷新
    {
        //var q = dc.Person.Where(p => p.DeptID == Convert.ToInt32(fb_zrdw.SelectedItem.Value));
        var q = from p in dc.Person
                //from u in dc.SF_Users
                where //p.PersonNumber == u.PersonNumber && 
                //p.Deptid == fb_zrdw.SelectedItem.Value
                p.Areadeptid == fb_zrdw.SelectedItem.Value
                select new
                {
                    p.Personnumber,
                    p.Name
                };
        PersStore.DataSource = q;
        PersStore.DataBind();
        fb_zrr.Disabled = q.Count() > 0 ? false : true;
        fb_zrr.EmptyText = q.Count() > 0 ? "请选择责任人" : "没有待选人员";
    }

    [AjaxMethod]
    public void IsFine()//确认是否罚款
    {
        if (fb_isfine.Checked == true)
        {
            detailFine();
            fb_fkje.Disabled = false;
        }
        else if (fb_isfine.Checked == false)
        {
            fb_fkje.Text = "";
            fb_fkje.Disabled = true;
        }
    }
    [AjaxMethod]
    public void IsSend()//隐患提交短信提醒确认--
    {
        if (chkIsSend.Checked == true)
        {
            txtSms.Disabled = false;
        }
        else if (chkIsSend.Checked == false)
        {
            txtSms.Disabled = true;
        }
    }

    [AjaxMethod]
    public void IsSms()//提交职能科室短信提醒确认--
    {
        if (ckb_isSms.Checked == true)
        {
            refer_Rname.Disabled = false;
            ta_smsText.Disabled = false;
        }
        else if (ckb_isSms.Checked == false)
        {
            refer_Rname.Disabled = true;
            ta_smsText.Disabled = true;
            refer_Rname.SelectedItem.Value = null;
            ta_smsText.Text = "";
        }
    }

    [AjaxMethod]
    public void singletest()
    {
        Ext.Msg.Alert("", refer_Rname.SelectedItem.Text).Show();
    }

    [AjaxMethod]
    public void isSmss()//提交矿领导短信提醒确认--
    {
        if (ckb_IsSmss.Checked == true)
        {
            ta_SmsTexts.Disabled = false;
        }
        else if (ckb_IsSmss.Checked == false)
        {
            ta_SmsTexts.Disabled = true;
            ta_SmsTexts.Text = "";
        }
    }

    [AjaxMethod]
    public void ReferToLeader()//提交领导--
    {
        //保存提交领导的数据库信息
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        var yhin = dc.Yhinput.First(p => p.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
        yhin.Refer = 1;
        if (refer_lead.SelectedItem.Value != null)
        {
            yhin.Referid =refer_lead.SelectedItem.Value;
        }
        else
        {
            Ext.Msg.Alert("系统提示","请选择具体领导!");
            return;
        }
        yhin.Status = "提交审批";
        dc.SubmitChanges();
        //短信提醒提交的领导有待处理隐患信息
        string msg = "提交成功!";
        if (ckb_IsSmss.Checked == true)
        {
            //发送短信
            string result = Sms.Send(refer_lead.SelectedItem.Value.Trim(), ta_SmsTexts.Text.Trim() == "" ? "您有一条待处理的隐患。" : ta_SmsTexts.Text.Trim(), SmsType.HiddenTroubleTips, sm.SelectedRow.RecordID.Trim(), "-安全生产体系支撑平台!");
            msg+=(result=="1"?"短信发送成功":"短信发送失败!");
            //获取发送者账号信息
            //string user = "";
            //user = SessionBox.GetUserSession().LoginName;
            //获取手机号码
            //var per = dc.Person.Where(p => p.PersonID == Convert.ToInt32(refer_lead.SelectedItem.Value));
            //string tel = "";
            //foreach (var r in per)
            //{
            //    tel = r.Tel.Trim();
            //}
            //获取发送的短信内容
            //string content = "";
            //if (ta_SmsTexts.Text == "")
            //{
            //    content = "祁南矿干部走动式巡查管理系统提示您：您有一条待处理的隐患！";
            //}
            //else
            //{
            //    content = ta_SmsTexts.Text.Trim();
            //}
            //发送短信
            //Sms sms = new Sms();
            //sms.SendSms(GhtnTech.Utility.ExtensionHelper.SmsType.HiddenTroubleTips, user, content, tel);
        }
        Ext.Msg.Alert("提示", msg).Show();
        Ext.DoScript("#{Store1}.reload();");
        //storeload();
        Window2.Hide();
    }

    [AjaxMethod]
    public void ReferToDept()//提交职能科室--
    {
        //保存提交职能科室的数据库信息
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        var yhin = dc.Yhinput.First(p => p.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
        yhin.Refer = 2;
        if (refer_dept.SelectedItem.Value != null)
        {
            yhin.Referid = refer_dept.SelectedItem.Value;
        }
        else
        {
            Ext.Msg.Alert("系统提示", "请选择具体科室!");
            return;
        }
        yhin.Status = "提交审批";
        dc.SubmitChanges();
        //短信提醒职能科室有待处理隐患信息
        if (ckb_isSms.Checked == true)
        {
            Sms.Send(refer_Rname.SelectedItem.Value.Trim(), ta_smsText.Text.Trim() == "" ? "您有一条待处理的隐患。" : ta_smsText.Text.Trim(), SmsType.HiddenTroubleTips, sm.SelectedRow.RecordID.Trim(), "-安全生产体系支撑平台!");
            
        }
        //    if (refer_Rname.SelectedIndex == -1)
        //    {
        //        Ext.Msg.Alert("系统提示","请选择该短信具体发送至某人!");
        //        return;
        //    }
        //    //获取发送者账号信息
        //    string user = "";
        //    user = SessionBox.GetUserSession().LoginName;
        //    //获取手机号码
        //    var per = dc.Person.Where(p => p.PersonID == Convert.ToInt32(refer_Rname.SelectedItem.Value));
        //    string tel = "";
        //    foreach (var r in per)
        //    {
        //        tel = r.Tel.Trim();
        //    }
        //    //获取发送的短信内容
        //    string content = "";
        //    if (ta_smsText.Text == "")
        //    {
        //        content = "祁南矿干部走动式巡查管理系统提示您：您有一条待处理的隐患！";
        //    }
        //    else
        //    {
        //        content = ta_smsText.Text.Trim();
        //    }
        //    //发送短信
        //    Sms sms = new Sms();
        //    sms.SendSms(GhtnTech.Utility.ExtensionHelper.SmsType.HiddenTroubleTips, user, content, tel);

        //}
        Ext.Msg.Alert("提示", "提交成功!").Show();
       // storeload();
        Ext.DoScript("#{Store1}.reload();");
        Window2.Hide();
    }

    [AjaxMethod]
    public void YHzgfk()//整改反馈
    {
        //--------------------****----------------------
        if (zgfk_zgr.SelectedIndex == -1 || zgfk_ysr.SelectedIndex == -1 || zgfk_zgqk.SelectedIndex == -1 || zgfk_zgrq.SelectedValue == null || zgfk_bc.SelectedIndex == -1)
        {
            Ext.Msg.Alert("提示", "请填写完整信息!").Show();
            return;
        }
        //------------------------------------------
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        var Input = dc.Yhinput.Where(p => p.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
        foreach (var row in Input)
        {
            if (row.Status.Trim() != "隐患未整改" && row.Status.Trim() != "隐患整改中")
            {
                Ext.Msg.Alert("提示", "当前状态不允许该操作!").Show();
                return;
            }
        }
        var Rectification = dc.Yinhuanrectification.Where(p => p.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
        if (Rectification.Count() == 0)
        {
            Yinhuanrectification yhr = new Yinhuanrectification
            {
                //--------------------****----------------------
                Yhputinid = Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()),
                //RecOpinion = zgfk_zgyj.Text.Trim(),
                Recpersonid = zgfk_zgr.SelectedItem.Value.Trim(),
                //ResponsibleID = Convert.ToInt32(zgfk_zrr.SelectedItem.Value.Trim()),
                Rectime = System.DateTime.Today,
                Recstate = zgfk_zgqk.SelectedItem.Value.Trim(),
                Yanshouid = zgfk_ysr.SelectedItem.Value.Trim(),
                Zgbanci = zgfk_bc.SelectedItem.Value.Trim()
                //------------------------------------------
            };
            dc.Yinhuanrectification.InsertOnSubmit(yhr);
            dc.SubmitChanges();
        }
        else
        {
            foreach (var r in Rectification)
            {
                //--------------------****----------------------
                //r.RecOpinion = zgfk_zgyj.Text.Trim();
                r.Recpersonid = zgfk_zgr.SelectedItem.Value.Trim();
                r.Rectime = System.DateTime.Today;
                //r.ResponsibleID = Convert.ToInt32(zgfk_zrr.SelectedItem.Value.Trim());
                r.Recstate = zgfk_zgqk.SelectedItem.Value.Trim();
                r.Yanshouid = zgfk_ysr.SelectedItem.Value.Trim();
                r.Zgbanci = zgfk_bc.SelectedItem.Value.Trim();
                //------------------------------------------
                dc.SubmitChanges();
            }
        }
        foreach (var row in Input)
        {
            row.Status = zgfk_zgqk.SelectedItem.Value.Trim();
        }
        dc.SubmitChanges();
        Ext.Msg.Alert("提示", "反馈成功!").Show();
        //storeload();
        Ext.DoScript("#{Store1}.reload();");
        Window3.Hide();
    }

    [AjaxMethod]
    public void YHfcfk()//复查反馈
    {
        if (fcfk_fcyj.Text.Trim() == "" || fcfk_fcr.SelectedIndex == -1 || fcfk_fcqk.SelectedIndex == -1)
        {
            Ext.Msg.Alert("提示", "请填写完整信息!").Show();
            return;
        }
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        var Input = dc.Yhinput.Where(p => p.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
        foreach (var row in Input)
        {
            if (row.Status.Trim() != "隐患已整改")
            {
                Ext.Msg.Alert("提示", "当前状态不允许该操作!").Show();
                return;
            }
        }
        var Review = dc.Yinhuanreview.Where(p => p.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
        if (Review.Count() == 0)
        {
            Yinhuanreview yhr = new Yinhuanreview
            {
                Yhputinid = Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()),
                Personid = fcfk_fcr.SelectedItem.Value.Trim(),
                Fctime = System.DateTime.Today,
                Reviewstate = fcfk_fcqk.SelectedItem.Value.Trim(),
                Reviewopinion = fcfk_fcyj.Text.Trim()
            };
            dc.Yinhuanreview.InsertOnSubmit(yhr);
            dc.SubmitChanges();   
        }
        else
        {
            foreach (var r in Review)
            {
                r.Personid = fcfk_fcr.SelectedItem.Value.Trim();
                r.Fctime = System.DateTime.Today;
                r.Reviewstate = fcfk_fcqk.SelectedItem.Value.Trim();
                r.Reviewopinion = fcfk_fcyj.Text.Trim();
                dc.SubmitChanges();
            }
        }
        foreach (var row in Input)
        {
            row.Status = fcfk_fcqk.SelectedItem.Value.Trim();
        }
        //复查不通过转入新流程
        dc.SubmitChanges();
        Ext.Msg.Alert("提示", "反馈成功!").Show();
        //storeload();
        Ext.DoScript("#{Store1}.reload();");
        Window4.Hide();
    }

    [AjaxMethod]
    public void YHycfkLoad()//异常反馈载入
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        var Input = dc.Yhinput.Where(p => p.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
        foreach (var row in Input)
        {
            yc_YHID.Text = row.Yhputinid.ToString();
            yc_nowstatus.SetValue(row.Status.Trim());
        }
    }

    [AjaxMethod]
    public void YHycfkLoad1()//异常反馈载入
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        var Input = dc.Yhinput.Where(p => p.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
        foreach (var row in Input)
        {
            TextField1.Text = row.Yhputinid.ToString();
            ComboBox1.SetValue(row.Status.Trim());
        }
    }
    [AjaxMethod]
    public void YHycfk1()//异常反馈
    {
        if (ComboBox1.SelectedItem.Value.Trim() == ComboBox2.SelectedItem.Value.Trim())
        {
            Ext.Msg.Alert("提示", "请检查信息准确性!").Show();
            return;
        }
        //YHAction act = new YHAction
        //{
        //    YHPutinID = Convert.ToInt32(TextField1.Text.Trim()),
        //    HistoryStatus = ComboBox1.SelectedItem.Value.Trim(),
        //    NewStauts = ComboBox2.SelectedItem.Value.Trim(),
            
        //    HappenDate = System.DateTime.Today
        //};
        
        //更改主表信息
        var q = dc.Yhinput.Where(p => p.Yhputinid == Convert.ToInt32(TextField1.Text.Trim()));
        foreach (var r in q)
        {
            r.Status = ComboBox2.SelectedItem.Value;
        }
        dc.SubmitChanges();
        //storeload();
        Ext.DoScript("#{Store1}.reload();");
        Window6.Hide();
        Ext.Msg.Alert("提示", "保存成功!").Show();
    }

    [AjaxMethod]
    public void YHycfk()//异常反馈
    {
        if (yc_nowstatus.SelectedItem.Value.Trim() == yc_newstatus.SelectedItem.Value.Trim() || yc_detail.Text.Trim() == "")
        {
            Ext.Msg.Alert("提示", "请检查信息准确性!").Show();
            return;
        }
        Yhaction act = new Yhaction
        {
            Yhputinid = Convert.ToInt32(yc_YHID.Text.Trim()),
            Historystatus = yc_nowstatus.SelectedItem.Value.Trim(),
            Newstauts = yc_newstatus.SelectedItem.Value.Trim(),
            Detail = yc_detail.Text.Trim(),
            Fine = yc_fine.IsNull ? 0 : Convert.ToDecimal(yc_fine.Text.Trim()),
            Happendate = System.DateTime.Today
        };
        //记录罚款历史
        dc.Yhaction.InsertOnSubmit(act);
        dc.SubmitChanges();
        //记录历史
        RecordHistory(act.Actionid, Convert.ToInt32(yc_YHID.Text.Trim()));
        //更改主表信息
        var q = dc.Yhinput.Where(p => p.Yhputinid == Convert.ToInt32(yc_YHID.Text.Trim()));
        foreach (var r in q)
        {
            r.Status = yc_newstatus.SelectedItem.Value;
        }
        dc.SubmitChanges();
        //storeload();
        Ext.DoScript("#{Store1}.reload();");
        Window5.Hide();
        Ext.Msg.Alert("提示", "保存成功!").Show();
    }

    private void RecordHistory(Decimal act,int YHid)//将以前记录转为历史
    {
        //隐患信息
        var input = dc.Yhinput.First(p => p.Yhputinid == YHid);
        //整改信息
        var check = dc.Yinhuancheck.First(p => p.Yhputinid == YHid);
        //整改反馈
        var rec = dc.Yinhuanrectification.Where(p => p.Yhputinid == YHid);
        //复查反馈
        var rev = dc.Yinhuanreview.Where(p => p.Yhputinid == YHid);
        //合并历史信息
        Yhhistory his = new Yhhistory
        {
            Actionid = act,
            Deptid = input.Deptid,
            Placeid = input.Placeid,
            Banci = input.Banci,
            Personid = input.Personid,
            Pctime = input.Pctime,
            Intime = input.Intime,
            Yhnumber = input.Yhnumber,
            Remarks = input.Remarks,
            Status = input.Status,
            Measures = check.Measures,
            Instructions = check.Instructions,
            Personid1 = check.Personid,
            Instrtime = check.Instrtime,
            Reclimit = check.Reclimit,
            Reviewlimit = check.Reviewlimit,
            Checktime = check.Checktime,
            //---------------------****---------------------
            //RecOpinion=rec.RecOpinion,
            Recpersonid = rec.Count() > 0 ? rec.First().Recpersonid : null,
            Responsibleid = check.Responsibleid,//
            Rectime = rec.Count() > 0 ? rec.First().Rectime : null,
            Recstate = rec.Count() > 0 ? rec.First().Recstate : null,
            Personid2 = rev.Count() > 0 ? rev.First().Personid : null,
            Fctime = rev.Count() > 0 ? rev.First().Fctime : null,
            Reviewstate = rev.Count() > 0 ? rev.First().Reviewstate : null,
            Reviewopinion = rev.Count() > 0 ? rev.First().Reviewopinion : null,
            Isfine = check.Isfine,
            Responsibledept = check.Responsibledept,
            Yanshouid = rec.Count() > 0 ? rec.First().Yanshouid : null,
            Zgbanci = rec.Count() > 0 ? rec.First().Zgbanci : null,
            //------------------------------------------
            Maindeptid=input.Maindeptid
        };
        dc.Yhhistory.InsertOnSubmit(his);
        dc.SubmitChanges();
    }

    [AjaxMethod]
    public void delshow()//将隐患置状态为不受理
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        Ext.Msg.Confirm("提示", "是否确定删除选中隐患?", new MessageBox.ButtonsConfig
        {
            Yes = new MessageBox.ButtonConfig
            {
                Handler = "Coolite.AjaxMethods.AgStatus('" + sm.SelectedRows[0].RecordID.Trim() + "');",
                Text = "确 定"
            },
            No = new MessageBox.ButtonConfig
            {
                Text = "取 消"
            }
        }).Show();
    }

    [AjaxMethod]
    public void AgStatus(string ID)
    {
        var view = dc.Yinhuanreview.Where(p => p.Yhputinid == decimal.Parse(ID));
        if (view.Count() > 0)
            dc.Yinhuanreview.DeleteOnSubmit(view.First());

        var RECTIFICATION = dc.Yinhuanrectification.Where(p => p.Yhputinid == decimal.Parse(ID));
        if (RECTIFICATION.Count() > 0)
            dc.Yinhuanrectification.DeleteOnSubmit(RECTIFICATION.First());

        var check = dc.Yinhuancheck.Where(p => p.Yhputinid == decimal.Parse(ID));
        if (check.Count() > 0)
            dc.Yinhuancheck.DeleteOnSubmit(check.First());

        var more = dc.YhinputMore.Where(p => p.Yhputinid == decimal.Parse(ID));
        if (more.Count() > 0)
            dc.YhinputMore.DeleteOnSubmit(more.First());

        var action = dc.Yhaction.Where(p => p.Yhputinid == Convert.ToInt32(ID));
        if (action.Count() > 0)
            dc.Yhaction.DeleteOnSubmit(action.First());

        var yhinput = dc.Yhinput.Where(p => p.Yhputinid == Convert.ToInt32(ID));
        if (yhinput.Count() > 0)
        {
            dc.Yhinput.DeleteOnSubmit(yhinput.First());
        }
        //var mp1 = dc.Yhinput.Where(p => p.Yhputinid == Convert.ToInt32(ID));
        //foreach (var r in mp1)
        //{
        //    r.Status = "系统不受理";
        //}
        dc.SubmitChanges();
        //storeload();
        Ext.DoScript("#{Store1}.reload();");
        Ext.Msg.Alert("提示", "删除成功!").Show();
    }
    #endregion

    #region 逾期未走动流程处理 已改

    [AjaxMethod]
    public void yqDataLoad() 
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        var YHcheck = dc.Yinhuancheck.Single(p => p.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
        yq_zgcs.Text = YHcheck.Measures.Trim();
        try
        {
            yq_ldps.Text = YHcheck.Instructions.Trim();
        }
        catch
        {
            yq_ldps.Text = "";
        }
        yq_zrdw.SelectedItem.Value = YHcheck.Responsibledept;
        
        //责任单位、责任人、罚款金额、整改措施
        var dataFine = from yh in dc.Yhinput
                       from yhN in dc.HazardsOre 
                       from yhc in dc.Yinhuancheck
                       where yh.Yhnumber==yhN.HNumber && yh.Maindeptid==yhN.Deptnumber && yh.Yhputinid==yhc.Yhputinid
                       && yh.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim())
                       select new
                       {
                           yhN.Punishmentstandard,//自定义处罚标准
                           yhc.Responsibleid
                           //yh.Deptid,
                           //yh.Personid,
                           //yhc.Measures
                       };
        foreach (var r in dataFine)
        {
            try
            {
                yq_fkje.Text = Decimal.Round(Decimal.Parse(r.Punishmentstandard), 2).ToString();
            }
            catch
            {
                yq_fkje.Text = "0.00";
            }
            //yq_zrdw.SelectedItem.Value = r.Deptid.ToString();
            try
            {
                var q = from p in dc.Person
                        //from u in dc.SF_Users
                        where //p.PersonNumber == u.PersonNumber && 
                        p.Areadeptid == YHcheck.Responsibledept || p.Personnumber == r.Responsibleid.Trim()
                        select new
                        {
                            p.Personnumber,
                            p.Name
                        };
                persoStore.DataSource = q;
                persoStore.DataBind();
                yq_zrr.Disabled = q.Count() > 0 ? false : true;
                //if (q.Where(p => p.Personnumber == r.Responsibleid.Trim()).Count() == 0)
                //{
                //    var personsingle = dc.Person.First(p => p.Personnumber == r.Responsibleid.Trim());
                //    yq_zrr.Items.Add(new Coolite.Ext.Web.ListItem(personsingle.Name, personsingle.Personnumber));
                //}
                yq_zrr.SelectedItem.Value = r.Responsibleid.Trim();
            }
            catch
            {
                yq_zrr.SelectedItem.Value = null;
            }
            //yq_zgcs.Text = r.Measures;
        }
        //分管领导
        var fgld = from pl in dc.Vgetpl
                   where pl.Roleid == 43 && pl.Unitid == SessionBox.GetUserSession().DeptNumber
                   //pl.Moduletag == this.PageTag && pl.Operatortag == PermissionTag.YH_znkssh && pl.Unitid == SessionBox.GetUserSession().DeptNumber
                   select new
                   {
                       pl.Personnumber,
                       pl.Name
                   };

        FBleader.DataSource = fgld;
        FBleader.DataBind();
        //大领导
        var leader = from pl in dc.Vgetpl
                     where pl.Roleid == 43 && pl.Unitid == SessionBox.GetUserSession().DeptNumber
                     //pl.Moduletag == this.PageTag && pl.Operatortag == PermissionTag.YH_kldsh && pl.Unitid == SessionBox.GetUserSession().DeptNumber
                     select new
                     {
                         pl.Personnumber,
                         pl.Name
                     };
        FBBigleader.DataSource = leader;
        FBBigleader.DataBind();

        
        //yq_zrr.SelectedItem.Value = YHcheck.ResponsibleID.Value.ToString();
        yq_zgqx.SelectedDate = YHcheck.Reclimit.Value;
        yq_bc.SelectedItem.Value = YHcheck.Banci.Trim();
        Checkbox1.Checked = YHcheck.Isfine.Value==1?true:false;

        //var dataFine = from yh in dc.Yhinput
        //               from yhN in dc.HazardsOre
        //               where yh.Yhnumber == yhN.HNumber && yh.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim())
        //               select new
        //               {
        //                   yhN.Punishmentstandard//处罚标准
        //               };
        //try
        //{
        //    yq_fkje.Text = Decimal.Round(Decimal.Parse(dataFine.First().Punishmentstandard), 2).ToString();
        //}
        //catch
        //{
        //    yq_fkje.Text = "0";
        //}
        //try
        //{
        //    yq_fkje.Text = dc.YHAction.First(p => p.YHPutinID == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim())).fine.Value.ToString();
        //}
        //catch (Exception e)
        //{
        //    yq_fkje.Text = "0";
        //}
        var YHinput = dc.Yhinput.Single(p => p.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
        var record = from act in dc.Yhaction
                     from h in dc.Yhhistory
                     where act.Actionid == h.Actionid
                     && act.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim())
                     && h.Status == YHinput.Status
                     select new
                     {
                         act.Actionid
                     };
        switch (record.Count())
        {
            case 0:
                Checkbox4.Disabled = true;
                Checkbox4.Checked = false;
                yq_dld.Disabled = true;
                tadldmassage.Disabled = true;
                yq_fkje.Text = (Decimal.Parse(yq_fkje.Text.Trim()) * 2).ToString();//第一次翻两倍
                break;
            default:
                Checkbox4.Disabled = false;
                Checkbox4.Checked = true;
                yq_dld.Disabled = false;
                tadldmassage.Disabled = false;
                yq_fkje.Text = (Decimal.Parse(yq_fkje.Text.Trim()) * 4).ToString();//多于一次翻四倍
                break;
        }
        lblmsgtext.Html = string.Format("<b>当前隐患第{0}次{1}</b>", (record.Count()+1).ToString(), YHinput.Status.Trim());//消息提醒
    }

    [AjaxMethod]
    public void yqIsFine()//确认是否罚款
    {
        if (Checkbox1.Checked == true)
        {
            yqdetailFine();
            yq_fkje.Disabled = false;
        }
        else if (Checkbox1.Checked == false)
        {
            yq_fkje.Text = "";
            yq_fkje.Disabled = true;
        }
    }

    [AjaxMethod]
    public void yqzrr()//责任人--
    {
        if (Checkbox2.Checked == true)
        {
            tazrrmassage.Disabled = false;
            //加载短息内容
            tazrrmassage.Text = "";
        }
        else
        {
            tazrrmassage.Disabled = true;
            //清空短息内容
            tazrrmassage.Text = "";
        }
    }

    [AjaxMethod]
    public void yqfgld()//分管领导--
    {
        if (Checkbox3.Checked == true)
        {
            yq_fgld.Disabled = false;
            tafgldmassamge.Disabled = false;
            //加载短息内容
            tafgldmassamge.Text = "";
        }
        else
        {
            yq_fgld.Disabled = true; yq_fgld.SelectedIndex = -1;
            tafgldmassamge.Disabled = true;
            //清空短息内容
            tafgldmassamge.Text = "";
        }
    }

    [AjaxMethod]
    public void yqdld()//大领导--
    {
        if (Checkbox4.Checked == true)
        {
            yq_dld.Disabled = false;
            tadldmassage.Disabled = false;
            //加载短息内容
            tadldmassage.Text = "";
        }
        else
        {
            yq_dld.Disabled = true; yq_dld.SelectedIndex = -1;
            tadldmassage.Disabled = true;
            //清空短息内容
            tadldmassage.Text = "";
        }
    }


    [AjaxMethod]
    public void yqdetailFine()//绑定隐患罚款金额
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        var dataFine = from yh in dc.Yhinput
                       from yhN in dc.HazardsOre
                       where yh.Yhnumber == yhN.HNumber && yh.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim())
                       select new
                       {
                           yhN.Punishmentstandard,
                           yh.Deptid
                       };
        foreach (var r in dataFine)
        {
            yq_fkje.Text = Decimal.Round(Decimal.Parse(r.Punishmentstandard), 2).ToString();
            yq_zrdw.SelectedItem.Value = r.Deptid.ToString();
            Ext.DoScript("#{fb_zrr}.clearValue(); #{PersStore}.reload();");
            var q = from p in dc.Person
                    //from u in dc.SF_Users
                    where //p.PersonNumber == u.PersonNumber && 
                    p.Deptid == yq_zrdw.SelectedItem.Value
                    select new
                    {
                        p.Personnumber,
                        p.Name
                    };
            persoStore.DataSource = q;
            persoStore.DataBind();
            yq_zrr.Disabled = q.Count() > 0 ? false : true;
        }
    }
    protected void PersRefresh1(object sender, StoreRefreshDataEventArgs e)
    {
        //var q = dc.Person.Where(p => p.DeptID == Convert.ToInt32(fb_zrdw.SelectedItem.Value));
        var q = from p in dc.Person
                //from u in dc.SF_Users
                where //p.PersonNumber == u.PersonNumber && 
                p.Areadeptid == yq_zrdw.SelectedItem.Value
                select new
                {
                    p.Personnumber,
                    p.Name
                };
        persoStore.DataSource = q;
        persoStore.DataBind();
        yq_zrr.Disabled = q.Count() > 0 ? false : true;
    }

    [AjaxMethod]
    public void yqYHpublish()//发布逾期未整改隐患--
    {
        //---------------------****---------------------
        if (yq_zgcs.Text.Trim() == "" || yq_zgqx.SelectedValue == null || yq_bc.SelectedIndex == -1 || yq_zrdw.SelectedIndex == -1)
        {
            Ext.Msg.Alert("提示", "请填写完整信息!").Show();
            return;
        }

        if (yq_zgqx.SelectedDate < System.DateTime.Today)
        {
            Ext.Msg.Alert("提示", "请重新选择整改期限，并且日期应大于今天!").Show();
            return;
        }

        if (Checkbox1.Checked)
        {
            if (yq_fkje.Text.Trim() == "" || yq_fkje.Text.Trim().IndexOf("-") != -1)
            {
                Ext.Msg.Alert("提示", "金额填写有误!").Show();
                return;
            }
        }

        if (Checkbox3.Checked == true)
        {
            if (yq_fgld.SelectedIndex == -1)
            {
                Ext.Msg.Alert("系统提示", "请选择该短信具体发送至某分管领导!").Show();
                return;
            }
        }

        if (Checkbox4.Checked == true)
        {
            if (yq_dld.SelectedIndex == -1)
            {
                Ext.Msg.Alert("系统提示", "请选择该短信具体发送至某主要领导!").Show();
                return;
            }
        }

        //------------------------------------------
        //if (fb_zgqx.Text.Trim().IndexOf("-") != -1 || fb_fcqx.Text.Trim().IndexOf("-") != -1)
        //{
        //    Ext.Msg.Alert("提示", "期限不能为负!").Show();
        //    return;
        //}
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        var Input = dc.Yhinput.Where(p => p.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
        foreach (var row in Input)
        {
            if (row.Status.Trim() != "逾期未整改" && row.Status.Trim() != "复查未通过")
            {
                Ext.Msg.Alert("提示", "当前状态不允许该操作!").Show();
                return;
            }
        }
        //转为历史
        Yhaction act = new Yhaction
        {
            Yhputinid = Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()),
            Historystatus = dc.Yhinput.First(p => p.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim())).Status.Trim(),//记录当前状态
            Newstauts = "隐患未整改",
            Detail = "",
            Fineperson = Checkbox4.Checked ? yq_fgld.SelectedItem.Value.Trim() : yq_zrr.SelectedItem.Value.Trim(),//超过一次罚分管领导，否则罚责任人
            Fine = yq_fkje.IsNull ? 0 : Convert.ToDecimal(yq_fkje.Text.Trim()),
            Happendate = System.DateTime.Today
        };
        //记录罚款历史
        dc.Yhaction.InsertOnSubmit(act);
        dc.SubmitChanges();
        //记录历史
        RecordHistory(act.Actionid, Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));

        //保存新信息
        var YHcheck = dc.Yinhuancheck.Single(p => p.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));

        //--------------------****----------------------
        YHcheck.Measures = yq_zgcs.Text.Trim();
        YHcheck.Instructions = yq_ldps.Text.Trim();
        YHcheck.Personid = SessionBox.GetUserSession().PersonNumber;//获取session中帐号信息
        YHcheck.Reclimit = yq_zgqx.SelectedDate;
        YHcheck.Reviewlimit = 2;//
        YHcheck.Checktime = System.DateTime.Today;
        YHcheck.Instrtime = System.DateTime.Today;
        YHcheck.Responsibledept = yq_zrdw.SelectedItem.Value.Trim();
        YHcheck.Banci = yq_bc.SelectedItem.Value.Trim();
        YHcheck.Isfine = Checkbox1.Checked?1:0;
        if (yq_zrr.SelectedIndex != -1)//
        {
            YHcheck.Responsibleid = yq_zrr.SelectedItem.Value.Trim();
        }
        //------------------------------------------
        dc.SubmitChanges();
        var YHin = dc.Yhinput.Single(p => p.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
        YHin.Status = "隐患未整改";
        dc.SubmitChanges();
        //发送短信

        //责任人
        if (Checkbox2.Checked == true)
        {
            if (yq_zrr.SelectedIndex == -1)
            {
                Ext.Msg.Alert("系统提示", "请选择该短信具体发送至某人!").Show();
                return;
            }
            Sms.Send(yq_zrr.SelectedItem.Value.Trim(), tazrrmassage.Text.Trim() == "" ? "当前你单位有一隐患需要你立即安排人员整改，详细信息请登陆GIC系统查看！" : tazrrmassage.Text.Trim(), SmsType.HiddenTroubleTips, sm.SelectedRow.RecordID.Trim(), "-安全生产体系支撑平台!");
            
        }
        


        //    //获取手机号码
        //    var per = dc.Person.Where(p => p.PersonID == Convert.ToInt32(yq_zrr.SelectedItem.Value));
        //    string tel = "";
        //    foreach (var r in per)
        //    {
        //        tel = r.Tel.Trim();
        //    }
        //    //获取发送的短信内容
        //    string content = "";
        //    if (tazrrmassage.Text == "")
        //    {
        //        content = "当前你单位有一隐患需要你立即安排人员整改，详细信息请登陆GIC系统查看！";
        //    }
        //    else
        //    {
        //        content = tazrrmassage.Text.Trim();
        //    }
        //    SendMSG(tel, content, Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));

        //}

        //分管领导
        if (Checkbox3.Checked == true && yq_fgld.SelectedIndex != -1)
        {
            Sms.Send(yq_fgld.SelectedItem.Value.Trim(), tafgldmassamge.Text.Trim() == "" ? yq_zrdw.SelectedItem.Text.Trim() + "有隐患在规定时间内未整改，请督促其立即整改，详细信息请登陆GIC系统查看！" : tafgldmassamge.Text.Trim(), SmsType.HiddenTroubleTips, sm.SelectedRow.RecordID.Trim(), "-安全生产体系支撑平台!");
        }
        //    //获取手机号码
        //    var per = dc.Person.Where(p => p.PersonID == Convert.ToInt32(yq_fgld.SelectedItem.Value));
        //    string tel = "";
        //    foreach (var r in per)
        //    {
        //        tel = r.Tel.Trim();
        //    }
        //    //获取发送的短信内容
        //    string content = "";
        //    if (tafgldmassamge.Text == "")
        //    {
        //        content = yq_zrdw.SelectedItem.Text.Trim() + "有隐患在规定时间内未整改，请督促其立即整改，详细信息请登陆GIC系统查看！";
        //    }
        //    else
        //    {
        //        content = tafgldmassamge.Text.Trim();
        //    }
        //    SendMSG(tel, content, Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));

        //}
        //大领导
        if (Checkbox4.Checked == true && yq_dld.SelectedIndex != -1)
        {
            Sms.Send(yq_dld.SelectedItem.Value.Trim(), tafgldmassamge.Text.Trim() == "" ? yq_zrdw.SelectedItem.Text.Trim() + "有隐患在规定时间内未整改，请督促其立即整改，详细信息请登陆GIC系统查看！" : tafgldmassamge.Text.Trim(), SmsType.HiddenTroubleTips, sm.SelectedRow.RecordID.Trim(), "-安全生产体系支撑平台!");
            
        }
        //    //获取手机号码
        //    var per = dc.Person.Where(p => p.PersonID == Convert.ToInt32(yq_dld.SelectedItem.Value));
        //    string tel = "";
        //    foreach (var r in per)
        //    {
        //        tel = r.Tel.Trim();
        //    }
        //    //获取发送的短信内容
        //    string content = "";
        //    if (tadldmassage.Text == "")
        //    {
        //        content = yq_zrdw.SelectedItem.Text.Trim() + "有隐患在规定时间内未整改，请督促其立即整改，详细信息请登陆GIC系统查看！";
        //    }
        //    else
        //    {
        //        content = tadldmassage.Text.Trim();
        //    }
        //    SendMSG(tel, content, Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));

        //}
        Ext.Msg.Alert("提示", "发布成功!").Show();
        Ext.DoScript("#{Store1}.reload();");
        //storeload();
        YQWindow.Hide();
    }

   private void SendMSG(string Tel, string msg, int YHid)//--
    {
        //获取发送者账号信息
        //string user = "";
        //user = SessionBox.GetUserSession().LoginName;
        ////发送短信
        //Sms sms = new Sms();
        //sms.SendSms(YHid, user, msg, Tel);
    }
    #endregion

    #region 导出报表
   protected void ToExcel(object sender, EventArgs e)//导出报表
   {
       string json = GridData.Value.ToString();
       foreach (var r in GridPanel1.ColumnModel.Columns)
       {
           json = json.Replace("\"" + r.DataIndex.Trim() + "\"", "\"" + r.Header + "\"");
       }
       json = json.Replace("T00:00:00", "");

       string strFileName = "隐患信息报表";
       Response.Clear();
       Response.Buffer = true;
       Response.Charset = "GB2312";

       StoreSubmitDataEventArgs eSubmit = new StoreSubmitDataEventArgs(json, null);
       XmlNode xml = eSubmit.Xml;

       this.Response.Clear();
       this.Response.ContentType = "application nd.ms-excel";
       Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(strFileName) + ".xls");
       XslCompiledTransform xtExcel = new XslCompiledTransform();
       xtExcel.Load(Server.MapPath("ExcelYH.xsl"));
       xtExcel.Transform(xml, null, this.Response.OutputStream);
       this.Response.End();
   }
    #endregion
}
