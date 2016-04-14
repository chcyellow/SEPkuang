using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Xsl;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Coolite.Ext.Web;
using GhtnTech.SEP.DAL;
using GhtnTech.SecurityFramework.BLL;

public partial class InformationSearch_SWInforSearch : BasePage
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

                SearchLoad();
                storeload();
            }
        }
    }
    protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)
    {
        //storeload();
        Search();
    }
    private void storeload()//执行查询
    {
        //var predicate = PredicateBuilder.True<Getsanwei>();

        //if (!df_begin.IsNull)
        //{
        //    predicate = predicate.And(p => p.Pctime > df_begin.SelectedDate);
        //}
        //if (!df_end.IsNull)
        //{
        //    predicate = predicate.And(p => p.Pctime < df_begin.SelectedDate);
        //}
        //if (cbb_part.SelectedItem.Value != null)
        //{
        //    predicate = predicate.And(p => p.Kqname.Contains(cbb_part.SelectedItem.Text));
        //}
        //var sanwei = dc.Getsanwei.Where(predicate);

        if (df_begin.SelectedDate > df_end.SelectedDate)
        {
            Ext.Msg.Alert("提示", "日期选择有误!").Show();
            return;
        }

        var sanwei = from sw in dc.Getsanwei
                     where sw.Dwid == SessionBox.GetUserSession().DeptNumber
                     select new
                     {
                         ID=sw.Id,
                         sw.Swpersonid,
                         DeptNumber = sw.Kqid,
                         DeptName =sw.Kqname,
                         Name=sw.Swperson,
                         SWContent=sw.Swcontent,
                         sw.Levelid,
                         SWLevel=sw.Levelname,
                         PlaceName=sw.Placename,
                         BanCi=sw.Banci,
                         sw.Pcpersonid,
                         PCName=sw.Pcname,
                         PCTime=sw.Pctime,
                         Ispublic = sw.Ispublic == 1,
                         IsEnd = sw.Isend == 1,
                         sw.Remarks,
                         Dwid = sw.Dwid,
                         Dwname = sw.Dwname,
                     };
            
            
            //sanwei=from sw in dc.Nswinput
            //         from ha in dc.Getswandhazusing
            //         from per1 in dc.Person
            //         from per2 in dc.Person
            //         from dep in dc.Department
            //         from dep2 in dc.Department
            //         join pl in dc.Place on sw.Placeid equals pl.Placeid into gg
            //         from ga in gg.DefaultIfEmpty()
            //         where sw.Swid == ha.Swid
            //         && sw.Pcpersonid == per1.Personnumber && sw.Swpersonid == per2.Personnumber
            //         && ha.Deptnumber == SessionBox.GetUserSession().DeptNumber
            //         && per2.Deptid == dep.Deptnumber
            //         orderby sw.Intime descending
            //         select new
            //         {
            //             ID=sw.Id,
            //             Swpersonid=per2.Personnumber,
            //             //Swperson = per2.Name,
            //             DeptNumber = dep.Deptnumber,
            //             DeptName = dep.Deptname,
            //             Name = per2.Name,
            //             SWContent=ha.Swcontent,
            //             ha.Levelid,
            //             SWLevel=ha.Levelname,
            //             PlaceName = ga.Placename == null ? "已删除" : ga.Placename,
            //             BanCi = sw.Banci,
            //             sw.Pcpersonid,
            //             PCName = per1.Name,
            //             PCTime = sw.Pctime,
            //             Ispublic = sw.Ispublic == 1,
            //             IsEnd = sw.Isend == 1,
            //             sw.Remarks,
            //             Dwid = sw.Maindeptid,
            //             Dwname = dep2.Deptname,
            //         };

        if (!df_begin.IsNull && !df_end.IsNull)
        {
            sanwei = sanwei.Where(p => p.PCTime >= df_begin.SelectedDate && p.PCTime <= df_end.SelectedDate);
        }
        if (cbb_part.SelectedIndex > -1)
        {
            sanwei = sanwei.Where(p => p.DeptNumber.Substring(0, 7) == cbb_part.SelectedItem.Value.Substring(0, 7));
        }

        if (cbb_lavel.SelectedIndex > -1)
        {
            sanwei = sanwei.Where(p => p.Levelid == decimal.Parse(cbb_lavel.SelectedItem.Value));
        }
        if (cbbSWPerson.SelectedIndex > -1)
        {
            sanwei = sanwei.Where(p => p.Swpersonid == cbbSWPerson.SelectedItem.Value);
        }
        if (cbbPCPerson.SelectedIndex > -1)
        {
            sanwei = sanwei.Where(p => p.Pcpersonid == cbbPCPerson.SelectedItem.Value);
        }

        if (cbbUnit.SelectedIndex > -1)
        {
            sanwei = sanwei.Where(p => p.Dwid == cbbUnit.SelectedItem.Value);
            //if (SessionBox.GetUserSession().rolelevel.Trim().IndexOf("1") > -1)
            //{
            //    data = data.Where(p => p.Levelname == "A");
            //}
        }
        else if (SessionBox.GetUserSession().rolelevel.Trim().IndexOf("2") > -1)
        {
            sanwei = sanwei.Where(p => p.Dwid == SessionBox.GetUserSession().DeptNumber);
        }
        //    sanwei = from sw in dc.Getsanwei
        //             where sw.Dwname.Trim() == SessionBox.GetUserSession().DeptName.Trim() && sw.Pctime > df_begin.SelectedDate
        //                 select new
        //                 {
        //                     ID = sw.Swnumber,
        //                     Name = sw.Swperson,
        //                     DeptName = sw.Kqname,
        //                     SWContent = sw.Swcontent,
        //                     SWLevel = "",
        //                     PlaceName = sw.Placename,
        //                     BanCi = sw.Banci,
        //                     PCName = sw.Pcname,
        //                     PCTime = sw.Pctime,
        //                     IsPublic = sw.Ispublic == 1 ? true : false,
        //                     IsEnd = sw.Isend == 1 ? true : false
        //                 };
        //}
        //if (df_begin.IsNull && !df_end.IsNull)
        //{
        //    sanwei = from sw in dc.Getsanwei
        //             where sw.Dwname.Trim() == SessionBox.GetUserSession().DeptName.Trim() && sw.Pctime < df_end.SelectedDate
        //             select new
        //             {
        //                 ID = sw.Swnumber,
        //                 Name = sw.Swperson,
        //                 DeptName = sw.Kqname,
        //                 SWContent = sw.Swcontent,
        //                 SWLevel = "",
        //                 PlaceName = sw.Placename,
        //                 BanCi = sw.Banci,
        //                 PCName = sw.Pcname,
        //                 PCTime = sw.Pctime,
        //                 IsPublic = sw.Ispublic == 1 ? true : false,
        //                 IsEnd = sw.Isend == 1 ? true : false
        //             };
        //}
        //if (cbb_part.SelectedItem.Value != null)
        //{
        //    sanwei = from sw in dc.Getsanwei
        //             where sw.Dwname.Trim() == SessionBox.GetUserSession().DeptName.Trim() && sw.Kqname.Trim() == cbb_part.SelectedItem.Text.Trim()
        //             select new
        //             {
        //                 ID = sw.Swnumber,
        //                 Name = sw.Swperson,
        //                 DeptName = sw.Kqname,
        //                 SWContent = sw.Swcontent,
        //                 SWLevel = "",
        //                 PlaceName = sw.Placename,
        //                 BanCi = sw.Banci,
        //                 PCName = sw.Pcname,
        //                 PCTime = sw.Pctime,
        //                 IsPublic = sw.Ispublic == 1 ? true : false,
        //                 IsEnd = sw.Isend == 1 ? true : false
        //             };
        //}
        Store1.DataSource = sanwei;
        Store1.DataBind();
        btn_detail.Disabled = true;
        //"ID" Type="String" />
        //                <ext:RecordField Name="Name" />
        //                <ext:RecordField Name="DeptName" />
        //                <ext:RecordField Name="SWContent" />
        //                <%--<ext:RecordField Name="SWLevel" />--%>
        //                <ext:RecordField Name="PlaceName" />
        //                <ext:RecordField Name="BanCi" />
        //                <ext:RecordField Name="PCName" />
        //                <ext:RecordField Name="PCTime" Type="Date" DateFormat="Y-m-dTh:i:s" />
        //                <%--<ext:RecordField Name="IsPublic" Type="Boolean" />--%>
        //                <ext:RecordField Name="IsEnd" Type="Bo  
        //////不同角色进入获取不同数据
        //string pID = "";
        //var q = from p in dc.Person2
        //        where p.PersonNumber == SessionBox.GetUserSession().PersonNumber
        //        select new
        //        {
        //            p.PersonID
        //        };
        //foreach (var r in q)
        //{
        //    pID = r.PersonID.ToString();
        //}
        //switch (SessionBox.GetUserSession().CurrentRole[0].ToString().Substring(0, SessionBox.GetUserSession().CurrentRole[0].ToString().IndexOf(",")).Trim())
        //{
        //    //走动干部所排查的三违信息
        //    case "4":
        //        var data = dc.GetSWbysearch(
        //                        df_begin.IsNull ? "" : df_begin.SelectedDate.ToString("yyyy-MM-dd"),
        //                        df_end.IsNull ? "" : df_end.SelectedDate.ToString("yyyy-MM-dd"),
        //                        "",
        //                        cbb_part.SelectedItem.Value.Trim(),
        //                        cbb_lavel.SelectedItem.Value.Trim(),
        //                        "",
        //                        pID
        //                        );
        //        Store1.DataSource = data;
        //        btn_print.Disabled = dc.GetSWbysearch(
        //                        df_begin.IsNull ? "" : df_begin.SelectedDate.ToString("yyyy-MM-dd"),
        //                        df_end.IsNull ? "" : df_end.SelectedDate.ToString("yyyy-MM-dd"),
        //                        "",
        //                        cbb_part.SelectedItem.Value.Trim(),
        //                        cbb_lavel.SelectedItem.Value.Trim(),
        //                        "",
        //                        pID
        //                        ).Count() > 0 ? false : true;
        //        break;
        //    //（责任部门）三违人员的三违信息
        //    case "5":
        //        data = dc.GetSWbysearch(
        //                        df_begin.IsNull ? "" : df_begin.SelectedDate.ToString("yyyy-MM-dd"),
        //                        df_end.IsNull ? "" : df_end.SelectedDate.ToString("yyyy-MM-dd"),
        //                        "",
        //                        cbb_part.SelectedItem.Value.Trim(),
        //                        cbb_lavel.SelectedItem.Value.Trim(),
        //                        pID,
        //                        ""
        //                        );
        //        Store1.DataSource = data;
        //        btn_print.Disabled = dc.GetSWbysearch(
        //                        df_begin.IsNull ? "" : df_begin.SelectedDate.ToString("yyyy-MM-dd"),
        //                        df_end.IsNull ? "" : df_end.SelectedDate.ToString("yyyy-MM-dd"),
        //                        "",
        //                        cbb_part.SelectedItem.Value.Trim(),
        //                        cbb_lavel.SelectedItem.Value.Trim(),
        //                        pID,
        //                        ""
        //                        ).Count() > 0 ? false : true;
        //        break;
        //    default:
        //        data = dc.GetSWbysearch(
        //                        df_begin.IsNull ? "" : df_begin.SelectedDate.ToString("yyyy-MM-dd"),
        //                        df_end.IsNull ? "" : df_end.SelectedDate.ToString("yyyy-MM-dd"),
        //                        "",
        //                        cbb_part.SelectedItem.Value.Trim(),
        //                        cbb_lavel.SelectedItem.Value.Trim(),
        //                        "",
        //                        ""
        //                        );
        //        Store1.DataSource = data;
        //        btn_print.Disabled = dc.GetSWbysearch(
        //                        df_begin.IsNull ? "" : df_begin.SelectedDate.ToString("yyyy-MM-dd"),
        //                        df_end.IsNull ? "" : df_end.SelectedDate.ToString("yyyy-MM-dd"),
        //                        "",
        //                        cbb_part.SelectedItem.Value.Trim(),
        //                        cbb_lavel.SelectedItem.Value.Trim(),
        //                        "",
        //                        ""
        //                        ).Count() > 0 ? false : true;
        //        break;
        //}
        //Store1.DataBind();
        //btn_detail.Disabled = true;
    }

    protected void RowClick(object sender, AjaxEventArgs e)//单击每行响应事件
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

    private void SearchLoad()//查询窗口初始化
    {
        //初始化日期
        df_begin.SelectedDate = System.DateTime.Today.AddDays(-15);
        df_end.SelectedDate = System.DateTime.Today;
        #region 初始化部门
        DeptStore.DataSource = PublicCode.GetKQdept(SessionBox.GetUserSession().DeptNumber);
        DeptStore.DataBind();
        #endregion

        #region 初始化单位
        if (SessionBox.GetUserSession().rolelevel.Trim().IndexOf("1") > -1)
        {
            UnitStore.DataSource = PublicCode.GetMaindept("");
            UnitStore.DataBind();
            cbbUnit.Disabled = false;

            cbb_part.Disabled = true;
        }
        else
        {
            UnitStore.DataSource = PublicCode.GetMaindept(SessionBox.GetUserSession().DeptNumber);
            UnitStore.DataBind();
            cbbUnit.SelectedItem.Value = SessionBox.GetUserSession().DeptNumber;
            cbbUnit.Disabled = true;
        }
        #endregion
        //三违级别
        var lavel = from c in dc.CsBaseinfoset
                    where c.Fid == int.Parse(PublicMethod.ReadXmlReturnNode("SWJB", this))
                    select new
                    {
                        SWLevelID = c.Infoid,
                        SWLevel = c.Infoname
                    };
        LevelStore.DataSource = lavel;
        LevelStore.DataBind();
      
    }

    [AjaxMethod]
    public void ClearSearch()//清除条件
    {
        df_begin.Clear();
        df_end.Clear();
        cbb_part.SelectedItem.Value = null;
        cbb_lavel.SelectedItem.Value = null;
        cbbSWPerson.SelectedItem.Value = null;
        cbbPCPerson.SelectedItem.Value = null;
        //cbb_lavel.SelectedItem.Value = null;
        if (SessionBox.GetUserSession().rolelevel.Trim().IndexOf("1") > -1)
        {
            cbbUnit.SelectedItem.Value = null;
        }
    }

    [AjaxMethod]
    public void Search()
    {
        //SearchLoad();
        storeload();
        Window1.Hide();
        GridPanel1.Show();
    }


    #region 明细模块
    [AjaxMethod]
    public void DetailLoad()//加载明细信息
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        BasePanel.Disabled = !SetSWbase(sm.SelectedRows[0].RecordID.Trim());
        CFPanel.Collapsed = true;
    }

    private bool SetSWbase(string ID)//加载三违基本信息 已改
    {
        try
        {
            var data = (from sw in dc.Nswinput
                        from ha in dc.Getswandhazusing
                        from per1 in dc.Person
                        from per2 in dc.Person
                        from dep in dc.Department
                        join pl in dc.Place on sw.Placeid equals pl.Placeid into gg
                        from ga in gg.DefaultIfEmpty()
                        where sw.Swid == ha.Swid
                        && sw.Pcpersonid == per1.Personnumber && sw.Swpersonid == per2.Personnumber
                        && ha.Deptnumber == SessionBox.GetUserSession().DeptNumber
                        && per2.Deptid == dep.Deptnumber
                        && sw.Id == decimal.Parse(ID)
                        orderby sw.Intime descending
                        select new
                        {
                            sw.Id,
                            Swperson = per2.Name,
                            Kqid = dep.Deptnumber,
                            Kqname = dep.Deptname,
                            ha.Swcontent,
                            ha.Levelid,
                            ha.Levelname,
                            Placename = ga.Placename == null ? "已删除" : ga.Placename,
                            sw.Banci,
                            Pcname = per1.Name,
                            sw.Pctime,
                            PCname = per1.Name,
                            Name = per2.Name,
                            Ispublic = sw.Ispublic == 1,
                            Isend = sw.Isend.Value == 1,
                            sw.Remarks,
                            Dwid = sw.Maindeptid,
                            sw.Pcpersonid,
                            sw.Swpersonid
                        }).First();
            //三违基本信息
            //0.基本信息绑定
            lbl_SWPutinID.Text = data.Id.ToString();
            lbl_DeptName.Text = data.Kqname.Trim();
            lbl_PlaceName.Text = data.Placename.Trim();
            lbl_SWContent.Text = data.Swcontent.Trim();
            lbl_BanCi.Text = data.Banci.Trim();
            lbl_Name.Text = data.Swperson.Trim();
            lbl_rName.Text = data.Pcname.Trim();
            try
            {
                lbl_PCTime.Text = data.Pctime.Value.ToString("yyyy年MM月dd日");
            }
            catch
            {
                lbl_PCTime.Text = "无";
            }
            lbl_SWLevel.Text = data.Levelname.Trim();
            try
            {
                lbldRemarks.Text = data.Remarks;
            }
            catch
            {
                lbldRemarks.Text = "";
            }
            try
            {
                lbl_IsEnd.Text = data.Isend ? "是" : "否";
            }
            catch
            {
                lbl_IsEnd.Text = "否";
            }
            // 罚款信息
            string msg = "";
            //var fk = from nd in dc.Nswfinedetail
            //         from l in dc.Objectset
            //         from p in dc.Person
            //         where nd.Pid == l.Pid && nd.Personnumber == p.Personnumber && nd.Id == decimal.Parse(ID)
            //         select new
            //         {
            //             l.Pname,
            //             p.Name
            //         };

            //foreach (var r in fk)
            //{
            //    msg += "<b>" + r.Pname + "：</b>" + r.Name + " ";
            //}
            //lbl_fkxx.Html = msg == "" ? "无" : msg;

            var fk = from nd in dc.Nswfinedetail
                     from l in dc.Objectset
                     from p in dc.Person
                     from s1 in dc.Nswinput
                     from sw in dc.Swbase
                     from f in dc.Swfineset
                     where nd.Pid == l.Pid && nd.Personnumber == p.Personnumber && nd.Id == decimal.Parse(ID)
                     && nd.Id == s1.Id && s1.Swid == sw.Swid && s1.Maindeptid == f.Deptnumber && sw.Levelid == f.Levelid && nd.Pid == f.Pid
                     select new
                     {
                         l.Pname,
                         p.Name,
                         Fine = s1.Jctype == 0 ? f.Kcfine : f.Zcfine
                     };

            foreach (var r in fk)
            {
                msg += "<b>" + r.Pname + "：</b>" + r.Name + "(" + r.Fine + "元)";
            }
            lbl_fkxx.Html = msg == "" ? "无" : msg;
            return true;
        }
        catch
        {
            return false;
        }
    }

    #endregion

    [AjaxMethod]
    public void PYsearch(string py, string store)
    {
        if (py.Trim() == "")
        {
            return;
        }
        var person = from pl in dc.Person
                     where pl.Maindeptid==cbbUnit.SelectedItem.Value
                     && (dc.F_PINYIN(pl.Name).ToLower().Contains(py.ToLower()) || pl.Name.Contains(py.Trim()))
                     select new
                     {
                         pl.Personnumber,
                         pl.Name
                     };
        personStore.DataSource = person.Distinct();
        personStore.DataBind();
    }

    

}
