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
using System.Xml;
using System.Xml.Xsl;
using System.Xml.Linq;
using System.Web.Script.Serialization;

public partial class YSNewProcess_YHProcess : BasePage
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
            Button8.Disabled = true;
            btnFineSet.Disabled = true;
            btnBulkClose.Disabled = true;
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
                   && (d.Deptlevel=="正科级" || d.Deptnumber.Substring(7, 2)=="00")
                   orderby d.Deptname
                   select new
                   {
                       d.Deptnumber,
                       d.Deptname
                   };
        fb_zrdw.Items.Clear();
        foreach (var r in dept)
        {
            fb_zrdw.Items.Add(new Coolite.Ext.Web.ListItem(r.Deptname, r.Deptnumber));
        }
    }

    [AjaxMethod]
    public void zrbdLoad()
    {
        var dept = from d in dc.Department
                   where d.Fatherid==fb_zrdw.SelectedItem.Value
                   orderby d.Deptname
                   select new
                   {
                       d.Deptnumber,
                       d.Deptname
                   };
        zrbdStore.DataSource = dept;
        zrbdStore.DataBind();
        //cbb_zrbd.Items.Clear();
        //foreach (var r in dept)
        //{
        //    cbb_zrbd.Items.Add(new Coolite.Ext.Web.ListItem(r.Deptname, r.Deptnumber));
        //}
        cbb_zrbd.SelectedItem.Value = null;
        Ext.DoScript("#{fb_zrr}.clearValue(); #{PersStore}.reload();");
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
            case "FkrenStore":
                var q2 = from p in dc.Person
                         where p.Maindeptid == SessionBox.GetUserSession().DeptNumber && p.Pinyin.ToLower().Contains(py.ToLower())
                         select new
                         {
                             p.Personnumber,
                             p.Name
                         };
                FkrenStore.DataSource = q2;
                FkrenStore.DataBind();
                break;
        }
    }
    //根据隐患状态绑定隐患信息--无用
    private void BindYHbyState(string stateYH)
    {
        var yhInfo = from a in dc.Yhview
                     where a.Status == stateYH
                     select a;
        Store1.DataSource = yhInfo;
        Store1.DataBind();
    }

    private void BeferRefresh()
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        sm.SelectedRows.Clear();
        sm.UpdateSelection();
        Ext.DoScript("#{Store1}.reload();");
        
    }

    protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)//已改
    {
        storeload();
    }

    private void AutoDoSth()//自动处理流程 已改
    {
        GhtnTech.SEP.OraclDAL.DALGetYHINFO yh = new GhtnTech.SEP.OraclDAL.DALGetYHINFO();
        yh.AutoSetNYHStatus();
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
                if (!UserHandle.ValidationHandle(PermissionTag.YH_fbtj) && !UserHandle.ValidationHandle(PermissionTag.YH_zgfk))//填报处罚信息
                {
                    btnFineSet.Visible = false;
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
                where pl.Moduletag == this.PageTag && pl.Operatortag == PermissionTag.YH_kldsh && pl.Unitid == SessionBox.GetUserSession().DeptNumber
                select new
                {
                    pl.Personnumber,
                    pl.Name
                };

        FBBigleader.DataSource = q;
        FBBigleader.DataBind();
        //矿领导
        var leader = from pl in dc.Vgetpl
                     from a in dc.Person 
                     where pl.Moduletag == this.PageTag && pl.Operatortag == PermissionTag.YH_kldsh && pl.Unitid == SessionBox.GetUserSession().DeptNumber
                     && pl.Personnumber==a.Personnumber
                     orderby a.Positionname.StartsWith("矿") descending, a.Positionname.StartsWith("党") descending, a.Personnumber ascending
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
                    break;

            }
        }
    }

    #region 单击每行-根据不同状态获取权限 已改

    protected void RowClick(object sender, AjaxEventArgs e)//流程处理
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0 && sm.SelectedRows.Count < 2)
        {
            btn_detail.Disabled = false;
            btn_action.Disabled = true;
            Button5.Disabled = true;
            btn_AgStatus.Disabled = false;
            btnBulkClose.Disabled = true;
            var Input = dc.Nyhinput.Where(p => p.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));

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
                        if (row.Refer != null)
                        {
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
                if (row.Isfine != null)
                {
                    if (row.Isfine == 1)
                    {
                        btnFineSet.Disabled = true;
                    }
                    else
                    {
                        btnFineSet.Disabled = false;
                    }
                }
                else
                {
                    btnFineSet.Disabled = false;
                }
            }
        }
        else if (sm.SelectedRows.Count > 1)
        {
            btn_zgfk.Disabled = true;
            btn_fcfk.Disabled = true;
            btn_detail.Disabled = true;
            btn_publish.Disabled = true;
            btn_AgStatus.Disabled = true;
            Button5.Disabled = true;
            Button8.Disabled = true;
            btnFineSet.Disabled = true;
            foreach (var item in sm.SelectedRows)
            {
                var y = dc.Nyhinput.Where(p => p.Yhputinid == Convert.ToInt32(item.RecordID.Trim())).First();
                if (y.Status != "新增")
                {
                    btnBulkClose.Disabled = true;
                    break;
                }
                else
                {
                    btnBulkClose.Disabled = false;
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
            btnFineSet.Disabled = true;
            btnBulkClose.Disabled = true;
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
            var data = from yh in dc.Getyhinput
                       where yh.Unitid == SessionBox.GetUserSession().DeptNumber
                              && dgroup.Contains(yh.Yhputinid)
                       select new
                       {
                           yh.Yhputinid,
                           yh.Deptname,
                           yh.Placename,
                           yh.Fcpersonname,
                           HBm = yh.Yhcontent,
                           yh.Remarks,
                           yh.Banci,
                           Personname = yh.Name,
                           yh.Pctime,
                           yh.Levelname,
                           Zyname = yh.Typename,
                           yh.Status,yh.Jctype,yh.Isfine
                       };
            Store1.DataSource = data;//dc.GetYHbyIDgroup(Request["YHIDgroup"].Trim());
            Store1.DataBind();
        }
        else if (Request.QueryString["DeptID"] != null)
        {
            var data = from yh in dc.Getyhinput
                       where yh.Unitid == SessionBox.GetUserSession().DeptNumber
                       && yh.Deptid == Request["DeptID"].Trim()
                       && yh.Status == "现场整改"
                       select new
                       {
                           yh.Yhputinid,
                           yh.Deptname,
                           yh.Placename,
                           yh.Fcpersonname,
                           HBm = yh.Yhcontent,
                           yh.Remarks,
                           yh.Banci,
                           Personname = yh.Name,
                           yh.Pctime,
                           yh.Levelname,
                           Zyname = yh.Typename,
                           yh.Status,yh.Jctype,yh.Isfine
                       };
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
            var data = from yh in dc.Getyhinput
                       where yh.Unitid == SessionBox.GetUserSession().DeptNumber
                       && yh.Deptid == Request["DeptID1"].Trim()
                       && yh.Status == "逾期未整改"
                       select new
                       {
                           yh.Yhputinid,
                           yh.Deptname,
                           yh.Placename,
                           yh.Fcpersonname,
                           HBm = yh.Yhcontent,
                           yh.Remarks,
                           yh.Banci,
                           Personname = yh.Name,
                           yh.Pctime,
                           yh.Levelname,
                           Zyname = yh.Typename,
                           yh.Status,yh.Jctype,yh.Isfine
                       };
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
            var data = from yh in dc.Getyhinput
                       where yh.Unitid == SessionBox.GetUserSession().DeptNumber
                       && yh.Status == Request["YHState"].ToString().Trim()
                       select new
                       {
                           yh.Yhputinid,
                           yh.Deptname,
                           yh.Placename,
                           yh.Fcpersonname,
                           HBm = yh.Yhcontent,
                           yh.Remarks,
                           yh.Banci,
                           Personname = yh.Name,
                           yh.Pctime,
                           yh.Levelname,
                           Zyname = yh.Typename,
                           yh.Status,yh.Jctype,yh.Isfine
                       };
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
            var yhdata = from yh in dc.Getyhinput
                       where false // yh.Unitid == SessionBox.GetUserSession().DeptNumber
                       select new
                       {
                           yh.Yhputinid,
                           yh.Deptname,
                           yh.Placename,
                           yh.Fcpersonname,
                           HBm = yh.Yhcontent,
                           yh.Remarks,
                           yh.Banci,
                           Personname = yh.Name,
                           yh.Pctime,
                           yh.Levelname,
                           Zyname = yh.Typename,
                           yh.Status,yh.Jctype,yh.Isfine
                       };
            if (UserHandle.ValidationHandle(PermissionTag.YH_kldsh))//拥有领导审核权限--矿领导
            {
                yhdata = yhdata.Union(
                    from yh in dc.Getyhinput
                    where yh.Unitid == SessionBox.GetUserSession().DeptNumber
                    && yh.Status == "提交审批" && yh.Refer == 1 && yh.Referid == SessionBox.GetUserSession().PersonNumber
                    select new
                    {
                        yh.Yhputinid,
                        yh.Deptname,
                        yh.Placename,
                        yh.Fcpersonname,
                        HBm = yh.Yhcontent,
                        yh.Remarks,
                        yh.Banci,
                        Personname = yh.Name,
                        yh.Pctime,
                        yh.Levelname,
                        Zyname = yh.Typename,
                        yh.Status,yh.Jctype,yh.Isfine
                    }
                       );


            }
            if (UserHandle.ValidationHandle(PermissionTag.Browse))//拥有浏览权限--所有人
            {
                yhdata = yhdata.Union(
                    from yh in dc.Getyhinput
                       where yh.Unitid == SessionBox.GetUserSession().DeptNumber
                       && (yh.Status == "复查通过" || yh.Status == "现场整改")
                       select new
                       {
                           yh.Yhputinid,
                           yh.Deptname,
                           yh.Placename,
                           yh.Fcpersonname,
                           HBm = yh.Yhcontent,
                           yh.Remarks,
                           yh.Banci,
                           Personname = yh.Name,
                           yh.Pctime,
                           yh.Levelname,
                           Zyname = yh.Typename,
                           yh.Status,yh.Jctype,yh.Isfine
                       }
                );
            }

            if (UserHandle.ValidationHandle(PermissionTag.YH_fcfk))//拥有复查权限--走动干部
            {
                yhdata = yhdata.Union(
                    from yh in dc.Getyhinput
                       where yh.Unitid == SessionBox.GetUserSession().DeptNumber
                       &&  yh.Status == "隐患已整改"
                       select new
                       {
                           yh.Yhputinid,
                           yh.Deptname,
                           yh.Placename,
                           yh.Fcpersonname,
                           HBm = yh.Yhcontent,
                           yh.Remarks,
                           yh.Banci,
                           Personname = yh.Name,
                           yh.Pctime,
                           yh.Levelname,
                           Zyname = yh.Typename,
                           yh.Status,yh.Jctype,yh.Isfine
                       }
                );
            }


            if (UserHandle.ValidationHandle(PermissionTag.YH_zgfk))//拥有整改反馈权限--责任部门
            {
                yhdata = yhdata.Union(
                    from yh in dc.Getyhinput
                    from yc in dc.Nyinhuancheck
                    where yh.Unitid == SessionBox.GetUserSession().DeptNumber
                    && yh.Yhputinid == yc.Yhputinid && yh.Status == "隐患未整改" && yc.Responsibledept == GetDeptIDViaPersonNumber(SessionBox.GetUserSession().PersonNumber)
                    select new
                    {
                        yh.Yhputinid,
                        yh.Deptname,
                        yh.Placename,
                        yh.Fcpersonname,
                        HBm = yh.Yhcontent,
                        yh.Remarks,
                        yh.Banci,
                        Personname = yh.Name,
                        yh.Pctime,
                        yh.Levelname,
                        Zyname = yh.Typename,
                        yh.Status,yh.Jctype,yh.Isfine
                    }
                );
            }

            if (UserHandle.ValidationHandle(PermissionTag.YH_znkssh))//拥有职能科室审核权限--职能科室
            {
                yhdata = yhdata.Union(
                    from yh in dc.Getyhinput
                    where yh.Unitid == SessionBox.GetUserSession().DeptNumber
                    && yh.Status == "提交审批"
                 && yh.Refer == 2 && yh.Referid == GetDeptIDViaPersonNumber(SessionBox.GetUserSession().PersonNumber)
                    select new
                    {
                        yh.Yhputinid,
                        yh.Deptname,
                        yh.Placename,
                        yh.Fcpersonname,
                        HBm = yh.Yhcontent,
                        yh.Remarks,
                        yh.Banci,
                        Personname = yh.Name,
                        yh.Pctime,
                        yh.Levelname,
                        Zyname = yh.Typename,
                        yh.Status,yh.Jctype,yh.Isfine
                    }
                );
            }

            if (UserHandle.ValidationHandle(PermissionTag.YH_fbtj) || UserHandle.ValidationHandle(PermissionTag.YH_bhxz))//拥有发布权限或闭合新增隐患权限--安监人员
            {
                yhdata = yhdata.Union(
                    from yh in dc.Getyhinput
                    where yh.Unitid == SessionBox.GetUserSession().DeptNumber
                    && yh.Status == "新增"
                    select new
                    {
                        yh.Yhputinid,
                        yh.Deptname,
                        yh.Placename,
                        yh.Fcpersonname,
                        HBm = yh.Yhcontent,
                        yh.Remarks,
                        yh.Banci,
                        Personname = yh.Name,
                        yh.Pctime,
                        yh.Levelname,
                        Zyname = yh.Typename,
                        yh.Status,yh.Jctype,yh.Isfine
                    }
                );
            }

            if (UserHandle.ValidationHandle(PermissionTag.YH_fjpccl))//拥有分级排查权限--安监人员
            {
                yhdata = yhdata.Union(
                    from yh in dc.Getyhinput
                    where yh.Unitid == SessionBox.GetUserSession().DeptNumber
                    && new string[] { "逾期整改未完成", "逾期未整改", "复查未通过" }.Contains(yh.Status)
                    select new
                    {
                        yh.Yhputinid,
                        yh.Deptname,
                        yh.Placename,
                        yh.Fcpersonname,
                        HBm = yh.Yhcontent,
                        yh.Remarks,
                        yh.Banci,
                        Personname = yh.Name,
                        yh.Pctime,
                        yh.Levelname,
                        Zyname = yh.Typename,
                        yh.Status,yh.Jctype,yh.Isfine
                    }
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
                    yhdata = from yh in dc.Getyhinput
                             where yh.Unitid == SessionBox.GetUserSession().DeptNumber
                             select new
                             {
                                 yh.Yhputinid,
                                 yh.Deptname,
                                 yh.Placename,
                                 yh.Fcpersonname,
                                 HBm = yh.Yhcontent,
                                 yh.Remarks,
                                 yh.Banci,
                                 Personname = yh.Name,
                                 yh.Pctime,
                                 yh.Levelname,
                                 Zyname = yh.Typename,
                                 yh.Status,yh.Jctype,yh.Isfine
                             };
                }
            }

            if (Session["TempSet"].ToString().Trim() == "True")//初始登陆过滤
            {
                yhdata = yhdata.Where(p => p.Status != "现场整改" && p.Status != "复查通过");
            }

            //yhdata = yhdata.Where(p => p.Maindeptid == SessionBox.GetUserSession().DeptNumber);//只处理本矿的


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
            
            if (cbb_part.SelectedIndex > -1)
            {
                yhdata = yhdata.Where(p => p.Deptname == cbb_part.SelectedItem.Text.Trim());
            }
            if (cbb_lavel.SelectedIndex > -1)
            {
                yhdata = yhdata.Where(p => p.Levelname == cbb_lavel.SelectedItem.Text.Trim());
            }
            if (cbb_kind.SelectedIndex > -1)
            {
                yhdata = yhdata.Where(p => p.Zyname == cbb_kind.SelectedItem.Text.Trim());
            }
            if (cbb_status.SelectedIndex > -1)
            {
                if (cbb_status.SelectedItem.Value == "历史逾期")
                {
                    yhdata = (from y in yhdata
                             from a in dc.Nyhaction
                             where y.Yhputinid == a.Yhputinid && a.Historystatus == "逾期未整改"
                             select y).Distinct();
                }
                else
                {
                    yhdata = yhdata.Where(p => p.Status == cbb_status.SelectedItem.Value.Trim());
                }
            }

            if (cbb_place.SelectedIndex > -1)
            {
                yhdata = yhdata.Where(p => p.Placename == cbb_place.SelectedItem.Text.Trim());
            }

            if (cbb_jctype.SelectedIndex > -1)
            {
                yhdata = yhdata.Where(p => p.Jctype == decimal.Parse(cbb_jctype.SelectedItem.Value));
            }

            //多人排查查询多加附表过滤条件
            if (cbb_person.SelectedIndex > -1)
            {
                yhdata = from y in yhdata
                         from m in dc.NyhinputMore
                         where y.Yhputinid == m.Yhputinid && m.Personid == cbb_person.SelectedItem.Value
                         select y;
            }

            Store1.DataSource = yhdata.OrderByDescending(p => p.Pctime);
            Store1.DataBind();

            //btn_detail.Disabled = true;
            //btn_zgfk.Disabled = true;
            //btn_fcfk.Disabled = true;
            //btn_publish.Disabled = true;
            //btn_action.Disabled = true;
            //Button5.Disabled = true;
            //Button8.Disabled = true;
            
        }
    }

    [AjaxMethod]
    public void LoadHazard()//加载管理标准
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        var info = from yh in dc.Nyhinput
                   from ha in dc.Getyhandhazusing
                   where yh.Yhid == ha.Yhid && yh.Yhputinid == decimal.Parse(sm.SelectedRows[0].RecordID)
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

        var q = dc.Person.Where(p => p.Posid == Convert.ToInt32(cbb_position.SelectedItem.Value));
        Store2.DataSource = q;
        Store2.DataBind();
        cbb_person.Disabled = q.Count() > 0 ? false : true;
    }

    protected void perRefresh(object sender, StoreRefreshDataEventArgs e)//发送短信人员刷新--修改科区内选择人员
    {
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
        #region 初始化职务
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
        DeptStore.DataSource = PublicCode.GetKQdept(SessionBox.GetUserSession().DeptNumber);
        DeptStore.DataBind();
        #endregion

        #region 初始化级别

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
        //BeferRefresh();
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
        string url = string.Format("../YSNewSearch/YhView.aspx?Yhid={0}",sm.SelectedRows[0].RecordID.Trim());
        Ext.DoScript("#{DetailWindow}.load('" + url + "');#{DetailWindow}.show();");
        //var input = dc.Nyhinput.First(p => p.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
        //int i = 0;
        //switch (input.Status.Trim())
        //{

        //    case "新增":
        //    case "提交审批":
        //    case "现场整改":
        //        i = 1;
        //        break;
        //    case "隐患未整改":
        //    case "隐患整改中":
        //    case "逾期未整改":
        //    case "逾期整改未完成":
        //        i = 2;
        //        break;
        //    case "隐患已整改":
        //        i = 3;
        //        break;
        //    case "复查通过":
        //    case "复查未通过":
        //        i = 4;
        //        break;
        //}
        //if (i > 0)
        //{
        //    SetYHbase(sm.SelectedRows[0].RecordID.Trim());
        //}
        //BasePanel.Disabled = i > 0 ? false : true; i--;
        //if (i > 0)
        //{
        //    SetYHZG(sm.SelectedRows[0].RecordID.Trim());
        //}
        //Panel1.Disabled = i > 0 ? false : true; i--;
        //if (i > 0)
        //{
        //    SetYHZGFK(sm.SelectedRows[0].RecordID.Trim());
        //}
        //ZGPanel.Disabled = i > 0 ? false : true; i--;
        //if (i > 0)
        //{
        //    SetYHFCFK(sm.SelectedRows[0].RecordID.Trim());
        //}
        //FCPanel.Disabled = i > 0 ? false : true; i--;
        //try
        //{
        //    CFPanel.Disabled = input.Isfine.Value == 1 ? false : true;
        //}
        //catch
        //{
        //    CFPanel.Disabled = true;
        //}
        //Panel1.Collapsed = true;
        //ZGPanel.Collapsed = true;
        //FCPanel.Collapsed = true;
        //CFPanel.Collapsed = true;
    }


    [AjaxMethod]
    public void detailFine()//绑定隐患罚款金额
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        fb_zrdw.SelectedItem.Value = dc.Nyhinput.First(p=>p.Yhputinid==Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim())).Deptid.ToString();
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
        zrbdLoad();
        //var dataFine = from yh in dc.Nyhinput
        //               from y in dc.Yhbase
        //               from f in dc.Fineset
        //               where yh.Yhid==y.Yhid && y.Levelid==f.Levelid && yh.Maindeptid==f.Deptnumber
        //                   && yh.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim())
        //               select new
        //               {
        //                   f.Kcfine,//自定义处罚标准
        //                   yh.Deptid,
        //               };
        //foreach (var r in dataFine)
        //{
        //    //try
        //    //{
        //    //    fb_fkje.Text = Decimal.Round(r.Kcfine.Value, 2).ToString();
        //    //}
        //    //catch
        //    //{
        //    //    fb_fkje.Text = "0.00";
        //    //}
            
        //}
    }

    [AjaxMethod]
    public void detailzgr()//绑定隐患整改人员
    {
        try
        {
            personStore.DataSource=from p in dc.Person
                         where p.Maindeptid == SessionBox.GetUserSession().DeptNumber
                         select new
                         {
                             p.Personnumber,
                             p.Name
                         };
            personStore.DataBind();
            zgfk_zgr.SelectedItem.Value = SessionBox.GetUserSession().PersonNumber;
            //zgfk_zgr.Disabled = true;
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
    //[AjaxMethod]
    //public void YHpublish()//发布隐患
    protected void YHpublish(object sender, AjaxEventArgs e)
    {
        //---------------------****---------------------
        if (fb_zgcs.Text.Trim() == "" || fb_zgqx.SelectedValue == null || fb_bc.SelectedIndex == -1 || fb_zrdw.SelectedIndex == -1)
        {
            Ext.Msg.Alert("提示", "请填写完整信息!").Show();
            return;
        }
        if (fb_zgqx.SelectedDate < System.DateTime.Today)
        {
            Ext.Msg.Alert("提示", "整改期限不应小于当前日期!").Show();
            return;
        }
        //if (fb_isfine.Checked)
        //{
        //    if (fb_fkje.Text.Trim() == "" || fb_fkje.Text.Trim().IndexOf("-") != -1)
        //    {
        //        Ext.Msg.Alert("提示", "金额填写有误!").Show();
        //        return;
        //    }
        //}
        //------------------------------------------
        //if (fb_zgqx.Text.Trim().IndexOf("-") != -1 || fb_fcqx.Text.Trim().IndexOf("-") != -1)
        //{
        //    Ext.Msg.Alert("提示", "期限不能为负!").Show();
        //    return;
        //}
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        var Input = dc.Nyhinput.Where(p => p.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
        foreach (var row in Input)
        {
            if (row.Status.Trim() != "提交审批" && row.Status.Trim() != "新增")
            {
                Ext.Msg.Alert("提示", "当前状态不允许该操作!").Show();
                return;
            }
        }

        var YHcheck = dc.Nyinhuancheck.Where(p => p.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));

        //获取整改截至时间
        string time = "00:00:00";
        switch (fb_bc.SelectedItem.Value.Trim())
        {
            case "早班":
                time = PublicMethod.ReadXmlReturnNode("ZBSJ", this);
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
            Nyinhuancheck yhc = new Nyinhuancheck
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
                //Isfine = fb_isfine.Checked ? 1 : 0//
            };
            if (fb_zrr.SelectedIndex != -1)//由于说是有可能责任人不填先把它放到外面
            {
                yhc.Responsibleid = fb_zrr.SelectedItem.Value.Trim();
            }
            if (cbb_zrbd.SelectedIndex > -1)
            {
                yhc.Responsiblebd = cbb_zrbd.SelectedItem.Value;
            }
            //------------------------------------------
            dc.Nyinhuancheck.InsertOnSubmit(yhc);
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
                //r.Isfine = fb_isfine.Checked ? 1 : 0;
                if (fb_zrr.SelectedIndex != -1)//
                {
                    r.Responsibleid = fb_zrr.SelectedItem.Value.Trim();
                }
                if (cbb_zrbd.SelectedIndex > -1)
                {
                    r.Responsiblebd = cbb_zrbd.SelectedItem.Value;
                }
                //------------------------------------------
                dc.SubmitChanges();
            }
        }
        //以发布责任单位作为原始单位
        try
        {
            var changeinput = Input.First();
            if (!string.IsNullOrEmpty(fb_zrdw.SelectedItem.Value))
            {
                changeinput.Deptid = fb_zrdw.SelectedItem.Value.Trim();
            }
            changeinput.Status = "隐患未整改";
            dc.SubmitChanges();
        }
        catch
        {
            var changeinput = Input.First();
            changeinput.Status = "隐患未整改";
            dc.SubmitChanges();
        }

        var amount = from f in dc.Fineset
                     from y in dc.Nyhinput
                     from d in dc.Nyhfinedetail
                     where f.Levelid == y.Levelid && y.Yhputinid == d.Yinputid && f.Pid == d.Pid && y.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim())
                     select new
                     {
                         f.Kcfine,
                         f.Zcfine
                     };
        Nyhaction act = new Nyhaction
        {
            Detail = fb_zgcs.Text.Trim(),
            Historystatus = "新增/提交审批",
            Newstauts = "隐患未整改",
            Yhputinid = Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()),
            Fine = amount.Sum(p => p.Kcfine),
            Happendate = System.DateTime.Today
        };
        dc.Nyhaction.InsertOnSubmit(act);
        dc.SubmitChanges();
        //出现status为null的现象 代码修改到上面try中
        //foreach (var row in Input)
        //{
        //    row.Refer = 0;
        //    row.Status = "隐患未整改";
        //}
        //dc.SubmitChanges();

        SMS_Send ss = new SMS_Send();
        //短信提醒责任人有待处理隐患信息
        
        if (chkIsSend.Checked == true && fb_zrr.SelectedIndex>-1)
        {
            string smsstr = txtSms.Text.Trim() == "" ? ss.GetYHSMSmsg(Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim())) : txtSms.Text.Trim();
            Sms.Send(fb_zrr.SelectedItem.Value.Trim(), smsstr, SmsType.HiddenTroubleTips, sm.SelectedRows[0].RecordID.Trim(), "-支撑平台!");//"您有一条待处理的隐患。"
        }

        //自动短信
        string msg = "短信发送失败！";
		try
		{
            msg = ss.SendSMS(Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()), "yhfb", this);
		}
		catch
		{
		}
        Ext.Msg.Alert("提示", "发布成功!" + msg).Show();

        BeferRefresh();
        //storeload();
        Window2.Hide();
        //if (IsHandSend.Checked)
        //{
        //    Ext.DoScript("parent.window.loadnewpage('短信发送','YSNewProcess/SMS_Send.aspx?Yhid='" + sm.SelectedRows[0].RecordID.Trim() + ");");
        //}
    }

    protected void PersRefresh(object sender, StoreRefreshDataEventArgs e)//发布/提交选择责任部门人员刷新
    {
        if (cbb_zrbd.SelectedIndex == -1)
        {
            var q = from p in dc.Person
                    where p.Areadeptid == fb_zrdw.SelectedItem.Value
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
        else
        {
            var q = from p in dc.Person
                    where p.Deptid == cbb_zrbd.SelectedItem.Value
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
    }

    //[AjaxMethod]
    //public void IsFine()//确认是否罚款
    //{
    //    if (fb_isfine.Checked == true)
    //    {
    //        detailFine();
    //        fb_fkje.Disabled = false;
    //    }
    //    else if (fb_isfine.Checked == false)
    //    {
    //        fb_fkje.Text = "";
    //        fb_fkje.Disabled = true;
    //    }
    //}
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
        var yhin = dc.Nyhinput.First(p => p.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
        yhin.Refer = 1;
        if (!string.IsNullOrEmpty(refer_lead.SelectedItem.Value) && refer_lead.SelectedIndex >= 0)
        {
            yhin.Referid = refer_lead.SelectedItem.Value;
        }
        else
        {
            Ext.Msg.Alert("系统提示", "请选择具体领导!");
            return;
        }
        yhin.Status = "提交审批";
        dc.SubmitChanges();
        //短信提醒提交的领导有待处理隐患信息
        string msg = "提交成功!";
        if (ckb_IsSmss.Checked == true)
        {
            //发送短信
             SMS_Send ss = new SMS_Send();
             string result = Sms.Send(refer_lead.SelectedItem.Value.Trim(), ta_SmsTexts.Text.Trim() == "" ? ss.GetYHSMSmsg(Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim())) : ta_SmsTexts.Text.Trim(), SmsType.HiddenTroubleTips, sm.SelectedRows[0].RecordID.Trim(), "-支撑平台!");
            msg += (result == "1" ? "短信发送成功" : "短信发送失败!");
        }
        Ext.Msg.Alert("提示", msg).Show();
        BeferRefresh();
        //storeload();
        Window2.Hide();
    }

    [AjaxMethod]
    public void ReferToDept()//提交职能科室--
    {
        //保存提交职能科室的数据库信息
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        var yhin = dc.Nyhinput.First(p => p.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
        yhin.Refer = 2;
        if (!string.IsNullOrEmpty(refer_dept.SelectedItem.Value))
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
            SMS_Send ss = new SMS_Send();
            Sms.Send(refer_Rname.SelectedItem.Value.Trim(), ta_smsText.Text.Trim() == "" ? ss.GetYHSMSmsg(Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim())) : ta_smsText.Text.Trim(), SmsType.HiddenTroubleTips, sm.SelectedRows[0].RecordID.Trim(), "-安全生产体系支撑平台!");

        }
        Ext.Msg.Alert("提示", "提交成功!").Show();
        // storeload();
        BeferRefresh();
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
        var Input = dc.Nyhinput.Where(p => p.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
        foreach (var row in Input)
        {
            if (row.Status.Trim() != "隐患未整改" && row.Status.Trim() != "隐患整改中")
            {
                Ext.Msg.Alert("提示", "当前状态不允许该操作!").Show();
                return;
            }
        }
        var Rectification = dc.Nyinhuanrectification.Where(p => p.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
        if (Rectification.Count() == 0)
        {
            Nyinhuanrectification yhr = new Nyinhuanrectification
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
            dc.Nyinhuanrectification.InsertOnSubmit(yhr);
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
        BeferRefresh();
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
        var Input = dc.Nyhinput.Where(p => p.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
        foreach (var row in Input)
        {
            if (row.Status.Trim() != "隐患已整改")
            {
                Ext.Msg.Alert("提示", "当前状态不允许该操作!").Show();
                return;
            }
        }
        var Review = dc.Nyinhuanreview.Where(p => p.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
        if (Review.Count() == 0)
        {
            Nyinhuanreview yhr = new Nyinhuanreview
            {
                Yhputinid = Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()),
                Personid = fcfk_fcr.SelectedItem.Value.Trim(),
                Fctime = System.DateTime.Today,
                Reviewstate = fcfk_fcqk.SelectedItem.Value.Trim(),
                Reviewopinion = fcfk_fcyj.Text.Trim()
            };
            dc.Nyinhuanreview.InsertOnSubmit(yhr);
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
        BeferRefresh();
        Window4.Hide();
    }

    [AjaxMethod]
    public void YHycfkLoad()//异常反馈载入
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        var Input = dc.Nyhinput.Where(p => p.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
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
        var Input = dc.Nyhinput.Where(p => p.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
        foreach (var row in Input)
        {
            TextField1.Text = row.Yhputinid.ToString();
            ComboBox1.SetValue(row.Status.Trim());
        }
    }
    [AjaxMethod]
    public void YHycfkLoad2()//批量闭合隐患
    {
        TextField2.Text = "";
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        foreach (var item in sm.SelectedRows)
        {
            TextField2.Text += item.RecordID.Trim() + "|";
        }
        ComboBox3.SetValue("新增");
        ComboBox4.SetValue("现场整改");
        ComboBox4.Disabled = true;
        //var Input = dc.Nyhinput.Where(p => p.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
        //foreach (var row in Input)
        //{
        //    TextField2.Text = row.Yhputinid.ToString();
        //    ComboBox3.SetValue(row.Status.Trim());
        //}
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
        var q = dc.Nyhinput.Where(p => p.Yhputinid == Convert.ToInt32(TextField1.Text.Trim()));
        foreach (var r in q)
        {
            r.Status = ComboBox2.SelectedItem.Value;
        }
        dc.SubmitChanges();
        //storeload();
        BeferRefresh();
        Window6.Hide();
        Ext.Msg.Alert("提示", "保存成功!").Show();
    }
    [AjaxMethod]
    public void YHycfk2()//批量闭合隐患
    {
        if (ComboBox3.SelectedItem.Value.Trim() == ComboBox4.SelectedItem.Value.Trim() || ComboBox4.SelectedItem.Value != "现场整改")
        {
            Ext.Msg.Alert("提示", "请检查信息准确性!").Show();
            return;
        }

        //更改主表信息
        foreach (var s in TextField2.Text.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
        {
            var q = dc.Nyhinput.Where(p => p.Yhputinid == Convert.ToInt32(s)).First();
            q.Status = ComboBox4.SelectedItem.Value;
        }
        dc.SubmitChanges();
        BeferRefresh();
        Window7.Hide();
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
        Nyhaction act = new Nyhaction
        {
            Yhputinid = Convert.ToInt32(yc_YHID.Text.Trim()),
            Historystatus = yc_nowstatus.SelectedItem.Value.Trim(),
            Newstauts = yc_newstatus.SelectedItem.Value.Trim(),
            Detail = yc_detail.Text.Trim(),
            Fine = yc_fine.IsNull ? 0 : Convert.ToDecimal(yc_fine.Text.Trim()),
            Happendate = System.DateTime.Today
        };
        //记录罚款历史
        dc.Nyhaction.InsertOnSubmit(act);
        dc.SubmitChanges();
        //记录历史
        RecordHistory(act.Actionid, Convert.ToInt32(yc_YHID.Text.Trim()));
        //更改主表信息
        var q = dc.Nyhinput.Where(p => p.Yhputinid == Convert.ToInt32(yc_YHID.Text.Trim()));
        foreach (var r in q)
        {
            r.Status = yc_newstatus.SelectedItem.Value;
        }
        dc.SubmitChanges();
        //storeload();
        BeferRefresh();
        Window5.Hide();
        Ext.Msg.Alert("提示", "保存成功!").Show();
    }

    private void RecordHistory(Decimal act, int YHid)//将以前记录转为历史
    {
        //隐患信息
        var input = dc.Nyhinput.First(p => p.Yhputinid == YHid);
        //整改信息
        var check = dc.Nyinhuancheck.First(p => p.Yhputinid == YHid);
        //整改反馈
        var rec = dc.Nyinhuanrectification.Where(p => p.Yhputinid == YHid);
        //复查反馈
        var rev = dc.Nyinhuanreview.Where(p => p.Yhputinid == YHid);
        //合并历史信息
        Nyhhistory his = new Nyhhistory();
        his.Actionid = act;
        his.Deptid = input.Deptid;
        his.Placeid = input.Placeid;
        his.Banci = check.Banci; //input.Banci;作为整改期限班次用
        his.Personid = input.Personid;
        his.Pctime = input.Pctime;
        his.Intime = input.Intime;
        his.Yhid = input.Yhid;
        his.Remarks = input.Remarks;
        his.Status = input.Status;
        his.Measures = check.Measures;
        his.Instructions = check.Instructions;
        his.Personid1 = check.Personid;
        his.Instrtime = check.Instrtime;
        his.Reclimit = check.Reclimit;
        his.Reviewlimit = check.Reviewlimit;
        his.Checktime = check.Checktime;
        his.Responsibleid = check.Responsibleid;
        his.Isfine = check.Isfine;
        his.Maindeptid = input.Maindeptid;
        his.Responsibledept = check.Responsibledept;
        //---------------------****---------------------
        if (rec.Count() > 0)
        {
            //his.RecOpinion=rec.First().RecOpinion;

            his.Recpersonid = rec.First().Recpersonid;

            his.Rectime = rec.First().Rectime;
            his.Recstate = rec.First().Recstate;
            his.Yanshouid = rec.First().Yanshouid;
            his.Zgbanci = rec.First().Zgbanci;
        }
        if (rev.Count() > 0)
        {
            his.Personid2 = rev.First().Personid;
            his.Fctime = rev.First().Fctime;
            his.Reviewstate = rev.First().Reviewstate;
            his.Reviewopinion = rev.First().Reviewopinion;
        }
        DBSCMDataContext db = new DBSCMDataContext();
        db.Nyhhistory.InsertOnSubmit(his);
        db.SubmitChanges();
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
        //var view = dc.Nyinhuanreview.Where(p => p.Yhputinid == decimal.Parse(ID));
        //if (view.Count() > 0)
        //    dc.Nyinhuanreview.DeleteOnSubmit(view.First());

        //var RECTIFICATION = dc.Nyinhuanrectification.Where(p => p.Yhputinid == decimal.Parse(ID));
        //if (RECTIFICATION.Count() > 0)
        //    dc.Nyinhuanrectification.DeleteOnSubmit(RECTIFICATION.First());

        //var check = dc.Nyinhuancheck.Where(p => p.Yhputinid == decimal.Parse(ID));
        //if (check.Count() > 0)
        //    dc.Nyinhuancheck.DeleteOnSubmit(check.First());

        //var more = dc.NyhinputMore.Where(p => p.Yhputinid == decimal.Parse(ID));
        //if (more.Count() > 0)
        //    dc.NyhinputMore.DeleteOnSubmit(more.First());

        //var action = dc.Nyhaction.Where(p => p.Yhputinid == Convert.ToInt32(ID));
        //if (action.Count() > 0)
        //    dc.Nyhaction.DeleteOnSubmit(action.First());

        //var yhinput = dc.Nyhinput.Where(p => p.Yhputinid == Convert.ToInt32(ID));
        //if (yhinput.Count() > 0)
        //{
        //    dc.Nyhinput.DeleteOnSubmit(yhinput.First());
        //}
        //var mp1 = dc.Yhinput.Where(p => p.Yhputinid == Convert.ToInt32(ID));
        //foreach (var r in mp1)
        //{
        //    r.Status = "系统不受理";
        //}
        var yhinput = dc.Nyhinput.First(p => p.Yhputinid == Convert.ToInt32(ID));
        yhinput.Status = "作废";
        dc.SubmitChanges();
        //storeload();
        BeferRefresh();
        Ext.Msg.Alert("提示", "删除成功!").Show();
    }
    #endregion

    #region 逾期未整改流程处理 已改

    [AjaxMethod]
    public void yqDataLoad()
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        var YHcheck = dc.Nyinhuancheck.Single(p => p.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
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
        var dataFine = from yh in dc.Nyhinput
                       from y in dc.Yhbase
                       from f in dc.Fineset
                       from yhc in dc.Nyinhuancheck
                       where yh.Yhid == y.Yhid && y.Levelid == f.Levelid && yh.Maindeptid == f.Deptnumber && yh.Yhputinid == yhc.Yhputinid
                           && yh.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim())
                       select new
                       {
                           f.Kcfine,//自定义处罚标准
                           yhc.Responsibleid
                       };
        foreach (var r in dataFine)
        {
            //try
            //{
            //    yq_fkje.Text = Decimal.Round(r.Kcfine.Value, 2).ToString();
            //}
            //catch
            //{
            //    yq_fkje.Text = "0.00";
            //}
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
        try
        {
            Checkbox1.Checked = YHcheck.Isfine.Value == 1 ? true : false;
        }
        catch
        {
            Checkbox1.Checked = true;
        }

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
        var YHinput = dc.Nyhinput.Single(p => p.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
        var record = from act in dc.Nyhaction
                     from h in dc.Nyhhistory
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
                Checkbox1.BoxLabel="确认罚款，第一次逾期加罚一倍";//yq_fkje.Text = (Decimal.Parse(yq_fkje.Text.Trim()) * 2).ToString();//第一次翻两倍
                break;
            default:
                Checkbox4.Disabled = false;
                Checkbox4.Checked = true;
                yq_dld.Disabled = false;
                tadldmassage.Disabled = false;
                Checkbox1.BoxLabel="确认罚款，多次逾期加罚四倍";//yq_fkje.Text = (Decimal.Parse(yq_fkje.Text.Trim()) * 4).ToString();//多于一次翻四倍
                break;
        }
        lblmsgtext.Html = string.Format("<b>当前隐患第{0}次{1}</b>", (record.Count() + 1).ToString(), YHinput.Status.Trim());//消息提醒
    }

    //[AjaxMethod]
    //public void yqIsFine()//确认是否罚款
    //{
    //    if (Checkbox1.Checked == true)
    //    {
    //        yqdetailFine();
    //        yq_fkje.Disabled = false;
    //    }
    //    else if (Checkbox1.Checked == false)
    //    {
    //        yq_fkje.Text = "";
    //        yq_fkje.Disabled = true;
    //    }
    //}

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

        yq_zrdw.SelectedItem.Value = dc.Nyhinput.First(p=>p.Yhputinid==Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim())).Deptid.ToString();
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
        //var dataFine = from yh in dc.Nyhinput
        //               from y in dc.Yhbase
        //               from f in dc.Fineset
        //               where yh.Yhid == y.Yhid && y.Levelid == f.Levelid && yh.Maindeptid == f.Deptnumber
        //                   && yh.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim())
        //               select new
        //               {
        //                   f.Kcfine,//自定义处罚标准
        //                   yh.Deptid
        //               };
        //foreach (var r in dataFine)
        //{
        //    //yq_fkje.Text = Decimal.Round(r.Kcfine.Value, 2).ToString();
            
        //}
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
    public void yqYHpublishSure()
    {
        if (Checkbox1.Checked)
        {
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            if (dc.Nyhfinedetail.Where(p => p.Yinputid == decimal.Parse(sm.SelectedRows[0].RecordID)).Count() == 0)
            {
                Ext.Msg.Confirm("提示", "当前没有设置罚款对象，罚款加倍无效，是否继续?", new MessageBox.ButtonsConfig
                {
                    Yes = new MessageBox.ButtonConfig
                    {
                        Handler = "Coolite.AjaxMethods.yqYHpublish();",
                        Text = "确 定"
                    },
                    No = new MessageBox.ButtonConfig
                    {
                        Text = "取 消"
                    }
                }).Show();
            }
            else
            {
                yqYHpublish();
            }
        }
        else
        {
            yqYHpublish();
        }
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
        //罚款信息
        decimal finesum = 0;
        if (Checkbox1.Checked)
        {
            var YHinput = dc.Nyhinput.Single(p => p.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
            var record = from a in dc.Nyhaction
                         from h in dc.Nyhhistory
                         where a.Actionid == h.Actionid
                         && a.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim())
                         && h.Status == YHinput.Status
                         select new
                         {
                             a.Actionid
                         };
            var fineset = dc.Nyhfinedetail.Where(p => p.Yinputid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
            if (fineset.Count() > 0)
            {
                if (record.Count() == 0)
                {
                    finesum = fineset.Sum(p => p.Fine).Value;
                    foreach (var r in fineset)
                    {
                        r.Fine = r.Fine * 2;
                    }
                    dc.SubmitChanges();

                }
                else
                {
                    finesum = fineset.Sum(p => p.Fine/3*4).Value;
                    foreach (var r in fineset)
                    {
                        r.Fine = r.Fine/3 * 7;
                    }
                    dc.SubmitChanges();
                }
            }
        }
        var Input = dc.Nyhinput.Where(p => p.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
        foreach (var row in Input)
        {
            if (row.Status.Trim() != "逾期未整改" && row.Status.Trim() != "复查未通过")
            {
                Ext.Msg.Alert("提示", "当前状态不允许该操作!").Show();
                return;
            }
        }
        //转为历史
        Nyhaction act = new Nyhaction
        {
            Yhputinid = Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()),
            Historystatus = dc.Nyhinput.First(p => p.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim())).Status.Trim(),//记录当前状态
            Newstauts = "隐患未整改",
            Detail = "",
            //Fineperson = Checkbox4.Checked ? yq_fgld.SelectedItem.Value.Trim() : yq_zrr.SelectedItem.Value.Trim(),//超过一次罚分管领导，否则罚责任人
            Fineperson =yq_zrr.SelectedItem.Value.Trim(),
            Fine = finesum,
            Happendate = System.DateTime.Today
        };
        //记录罚款历史
        dc.Nyhaction.InsertOnSubmit(act);
        dc.SubmitChanges();
        //记录历史
        RecordHistory(dc.Nyhaction.Where(p => p.Yhputinid == act.Yhputinid && p.Happendate == act.Happendate).OrderByDescending(p => p.Actionid).First().Actionid, Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));

        //保存新信息
        var YHcheck = dc.Nyinhuancheck.Single(p => p.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));

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
        YHcheck.Isfine = Checkbox1.Checked ? 1 : 0;
        if (yq_zrr.SelectedIndex != -1)//
        {
            YHcheck.Responsibleid = yq_zrr.SelectedItem.Value.Trim();
        }
        //------------------------------------------
        dc.SubmitChanges();
        var YHin = dc.Nyhinput.Single(p => p.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
        YHin.Status = "隐患未整改";
        dc.SubmitChanges();
        //发送短信

        SMS_Send ss = new SMS_Send();
		string msg = "短信发送失败！";
		try
		{
		msg = ss.SendSMS(YHin.Yhputinid, "yhyq", this);
		}
		catch
		{
		}

        //责任人
        if (Checkbox2.Checked == true)
        {
            if (yq_zrr.SelectedIndex == -1)
            {
                Ext.Msg.Alert("系统提示", "请选择该短信具体发送至某人!").Show();
                return;
            }
            Sms.Send(yq_zrr.SelectedItem.Value.Trim(), tazrrmassage.Text.Trim() == "" ? "当前你单位有一隐患需要你立即安排人员整改，详细信息请登陆GIC系统查看！" : tazrrmassage.Text.Trim(), SmsType.HiddenTroubleTips, sm.SelectedRows[0].RecordID.Trim(), "-支撑平台!");

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
            Sms.Send(yq_fgld.SelectedItem.Value.Trim(), tafgldmassamge.Text.Trim() == "" ? yq_zrdw.SelectedItem.Text.Trim() + "有隐患在规定时间内未整改，请督促其立即整改，详细信息请登陆GIC系统查看！" : tafgldmassamge.Text.Trim(), SmsType.HiddenTroubleTips, sm.SelectedRows[0].RecordID.Trim(), "-安全生产体系支撑平台!");
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
            Sms.Send(yq_dld.SelectedItem.Value.Trim(), tafgldmassamge.Text.Trim() == "" ? yq_zrdw.SelectedItem.Text.Trim() + "有隐患在规定时间内未整改，请督促其立即整改，详细信息请登陆GIC系统查看！" : tafgldmassamge.Text.Trim(), SmsType.HiddenTroubleTips, sm.SelectedRows[0].RecordID.Trim(), "-支撑平台!");

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
        Ext.Msg.Alert("提示", "发布成功!" + msg).Show();
        BeferRefresh();
        //storeload();
        YQWindow.Hide();
        //if (yqIshandSend.Checked) 
        //{
        //    Ext.DoScript("parent.window.loadnewpage('短信发送','YSNewProcess/SMS_Send.aspx?Yhid='" + sm.SelectedRows[0].RecordID.Trim() + ");");
        //}
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
        json = json.Replace("\"检查方式\":0","\"检查方式\":\"矿查\"");
        json = json.Replace("\"检查方式\":1", "\"检查方式\":\"自查\"");
        json = json.Replace("\"检查方式\":2", "\"检查方式\":\"上级排查\"");
        json = json.Replace("\"处罚\":1", "\"处罚\":\"是\"");
        json = json.Replace("\"处罚\":0", "\"处罚\":\"否\"");

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

    [AjaxMethod]
    public void SelectPerson()
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        string url = string.Format("FinePersonSelect.aspx?Post={0}&Mod={1}&Win={2}", sm.SelectedRow.RecordID, "yh", FineWin.ClientID);
        UserHandle.InitModule(this.PageTag);//初始化此模块的权限。
        if (UserHandle.ValidationHandle(PermissionTag.Browse))//是否有浏览权限
        {
            if (!UserHandle.ValidationHandle(PermissionTag.YH_fbtj))
            {
                url += "&Action=1";
            }
        }
        Ext.DoScript("#{FineWin}.load('" + url + "');#{FineWin}.show();");
    }
}
