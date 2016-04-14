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

public partial class YSNewProcess_SWProcess : BasePage
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e) //已改
    {
        if (!Ext.IsAjaxRequest)
        {
            if (!SessionBox.CheckUserSession())
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {

                UserHandle.InitModule(this.PageTag);//初始化此模块的权限。
                if (UserHandle.ValidationHandle(PermissionTag.Browse))//是否有浏览权限
                {
                    if (!UserHandle.ValidationHandle(PermissionTag.SW_xxcl))//三违处理权限
                    {
                        Button1.Visible = false;
                        //Button2.Visible = false;
                    }
                    if (!UserHandle.ValidationHandle(PermissionTag.Delete))//三违删除权限
                    {
                        Button2.Visible = false;
                    }
                }
                SearchLoad();
                try
                {
                    string[] strgroup = Request.QueryString["SWIDgroup"].Trim().Split(',');
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
                    var data = from sw in dc.Nswinput
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
                                   PCTime = sw.Pctime,
                                   PCname = per1.Name,
                                   Name = per2.Name,
                                   Ispublic = sw.Ispublic == 1,
                                   Isend = sw.Isend.Value == 1,
                                   sw.Jctype,
                                   sw.Remarks,
                                   sw.Intime  //袁一矿提出报表添加报送时间
                               };
                        
                    Store1.DataSource = data;//dc.GetSWbyIDgroup(Request["SWIDgroup"].Trim());
                    Store1.DataBind();
                    btn_detail.Disabled = true;
                    Button1.Disabled = true;
                    Button2.Disabled = true;
                }
                catch
                {
                    storeload();
                }
            }
        }
    }
    protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)
    {
        storeload();
    }
    private void storeload()//执行查询 已改
    {
        UserHandle.InitModule(this.PageTag);//初始化此模块的权限。
        var data = from sw in dc.Nswinput
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
                       sw.Swpersonid,
                       sw.Intime  //袁一矿提出报表添加报送时间
                   };
        if (UserHandle.ValidationHandle(PermissionTag.SW_xxcl))//三违处理权限
        {
            data = data.Where(p => p.Dwid == SessionBox.GetUserSession().DeptNumber);
        }
        else if (UserHandle.ValidationHandle(PermissionTag.Browse))//是否有浏览权限
        {
            data = data.Where(p => p.Pcpersonid == SessionBox.GetUserSession().PersonNumber || p.Swpersonid == SessionBox.GetUserSession().PersonNumber);
        }
        else
        {
            data = data.Where(p => p.Dwid == "-2");
        }
        if (!df_begin.IsNull)
        {
            data = data.Where(p => p.Pctime >= df_begin.SelectedDate);
        }
        if (!df_end.IsNull)
        {
            data = data.Where(p => p.Pctime <= df_end.SelectedDate);
        }
        if (cbb_part.SelectedIndex > -1)
        {
            data = data.Where(p => p.Kqid == cbb_part.SelectedItem.Value.Trim());
        }
        if (cbb_lavel.SelectedIndex > -1)
        {
            data = data.Where(p => p.Levelid == decimal.Parse(cbb_lavel.SelectedItem.Value.Trim()));
        }

        Store1.DataSource = data;
        Store1.DataBind();
        btn_detail.Disabled = true;
        Button1.Disabled = true;
        Button2.Disabled = true;
    }

    protected void RowClick(object sender, AjaxEventArgs e)//单击每行响应事件 已改
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            btn_detail.Disabled = false;
            Button2.Disabled = false;
            //DBSCMDataContext dc = new DBSCMDataContext();
            var Input = dc.Nswinput.First(p => p.Id == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
            try
            {
                Button1.Disabled = Input.Isend.Value == 1 ? true : false;
                //Button2.Disabled = Input.Isend.Value == 1 ? true : false;
            }
            catch
            {
                Button1.Disabled = false;
            }
        }
        else
        {
            btn_detail.Disabled = true;
            Button1.Disabled = true;
            Button2.Disabled = true;
        }
    }

    private void SearchLoad()//查询窗口初始化 已改
    {
        //DBSCMDataContext dc = new DBSCMDataContext();
        //初始化日期
        //df_begin.SelectedDate = System.DateTime.Today.AddDays(-6);
        //df_end.SelectedDate = System.DateTime.Today;
        //初始化部门
        var dept = from d in dc.Department
                   where d.Deptnumber.Substring(0, 4) == SessionBox.GetUserSession().DeptNumber.Substring(0, 4)
                   && d.Deptnumber.Substring(7) == "00"
                   && d.Deptlevel == "正科级" && d.Deptstatus == "1"
                   orderby d.Deptname
                   select new
                   {
                       d.Deptnumber,
                       d.Deptname
                   };
        DeptStore.DataSource = dept.Distinct();
        DeptStore.DataBind();
        //初始化级别
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
    }

    [AjaxMethod]
    public void Search()
    {
        storeload();
        Window1.Hide();
        GridPanel1.Show();
    }

    [AjaxMethod]
    public void delshow()//作废三违信息
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            Ext.Msg.Confirm("提示", "是否确定作废选中三违?", new MessageBox.ButtonsConfig
            {
                Yes = new MessageBox.ButtonConfig
                {
                    Handler = "Coolite.AjaxMethods.maddGZRW('" + sm.SelectedRows[0].RecordID.Trim() + "');",
                    Text = "确 定"
                },
                No = new MessageBox.ButtonConfig
                {
                    Text = "取 消"
                }
            }).Show();
        }
    }

    [AjaxMethod]
    public void maddGZRW(string id)
    {
        if (id == "-1")
        {
            return;
        }
        Ext.Msg.Show(new MessageBox.Config
        {
            Title = "三违信息作废",
            Message = "请在下面录入作废原因：",
            Width = 300,
            ButtonsConfig = new MessageBox.ButtonsConfig
            {
                Yes = new MessageBox.ButtonConfig
                {
                    Handler = "Coolite.AjaxMethods.AgStatus(text,'" + id + "')",
                    Text = "保存"
                },
                No = new MessageBox.ButtonConfig
                {
                    Text = "取消"
                }
            },
            Buttons = MessageBox.Button.YESNO,
            Multiline = true,
            AnimEl = Button2.ClientID,
            //Fn = new JFunction { Fn = "showResultText" }
        });

    }

    [AjaxMethod]
    public void AgStatus(string reason, string ID)
    {
        var view = dc.Nswinput.Where(p => p.Id == decimal.Parse(ID));
        if (view.Count() > 0)
        {
            NswinputNullify sn = new NswinputNullify
            {
                Id = view.First().Id,
                Banci = view.First().Banci,
                Fine = view.First().Fine,
                Inputpersonid = view.First().Inputpersonid,
                Intime = view.First().Intime,
                Isadministrativepenalty = view.First().Isadministrativepenalty,
                Isend = view.First().Isend,
                Ispublic = view.First().Ispublic,
                Maindeptid = view.First().Maindeptid,
                Nullify = reason,
                Pcpersonid = view.First().Pcpersonid,
                Pctime = view.First().Pctime,
                Placeid = view.First().Placeid,
                Remarks = view.First().Remarks,
                Swnumber = view.First().Swid.ToString(),
                Swpersonid = view.First().Swpersonid,
                Islearn = view.First().Islearn
            };
            dc.NswinputNullify.InsertOnSubmit(sn);
            //dc.SubmitChanges();
            var sw = dc.Nswinput.First(p => p.Id == decimal.Parse(ID));
            dc.Nswinput.DeleteOnSubmit(sw);
            try
            {
                dc.SubmitChanges();
            }
            catch (Exception e)
            {
                string a = e.ToString();
            }
            Ext.DoScript("#{Store1}.reload();");
            Ext.Msg.Alert("提示", "作废成功!").Show();
        }

    }

    [AjaxMethod]
    public void DetailLoad_cl()//加载部分明细信息 已改
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        var data = dc.Getsanwei.First(p => p.Id == decimal.Parse(sm.SelectedRows[0].RecordID.Trim()));
        if (fb_YES.Checked == true)
        {
            lbl_Fine.Text = Decimal.Round(data.Fine.Value, 2).ToString();
            lbl_Fine.Disabled = false;
        }
        else if (fb_YES.Checked == false)
        {
            lbl_Fine.Text = "";
            lbl_Fine.Disabled = true;
        }
        ckb_IsSms.Checked = false;
    }


    #region 明细模块
    [AjaxMethod]
    public void DetailLoad()//加载明细信息
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        BasePanel.Disabled = !SetSWbase(sm.SelectedRows[0].RecordID.Trim());
        //CLPanel.Disabled = !SetSWbase_cl(sm.SelectedRows[0].RecordID.Trim());
        //CLPanel.Collapsed = true;
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
                            Placename=ga.Placename==null?"已删除":ga.Placename,
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

    //private bool SetSWbase_cl(string ID)//加载三违处理信息 已改
    //{
    //    var data = (from sw in dc.Nswinput
    //                from ha in dc.Getswandhazusing
    //                from pl in dc.Place
    //                from per1 in dc.Person
    //                from per2 in dc.Person
    //                from dep in dc.Department
    //                where sw.Swid == ha.Swid && sw.Placeid == pl.Placeid
    //                && sw.Pcpersonid == per1.Personnumber && sw.Swpersonid == per2.Personnumber
    //                && ha.Deptnumber == SessionBox.GetUserSession().DeptNumber
    //                && per2.Deptid == dep.Deptnumber
    //                && sw.Id == decimal.Parse(ID)
    //                orderby sw.Intime descending
    //                select new
    //                {
    //                    sw.Id,
    //                    Swperson = per2.Name,
    //                    Kqid = dep.Deptnumber,
    //                    Kqname = dep.Deptname,
    //                    ha.Swcontent,
    //                    ha.Levelid,
    //                    ha.Levelname,
    //                    pl.Placename,
    //                    sw.Banci,
    //                    Pcname = per1.Name,
    //                    sw.Pctime,
    //                    PCname = per1.Name,
    //                    Name = per2.Name,
    //                    Ispublic = sw.Ispublic == 1,
    //                    Isend = sw.Isend.Value == 1,
    //                    sw.Remarks,
    //                    Dwid = sw.Maindeptid,
    //                    sw.Pcpersonid,
    //                    sw.Swpersonid,
    //                    sw.Fine,
    //                    sw.Isadministrativepenalty,
    //                }).First();
    //    //三违处理信息
    //    try
    //    {
    //        if (data.Isend)
    //        {
    //            //1.罚款信息绑定
    //            //try
    //            //{
    //            //    if (data.Fine == 0)
    //            //    {
    //            //        lbl_Isfak.Text = "否";
    //            //        lbl_SWFine.Text = "";
    //            //    }
    //            //    else
    //            //    {
    //            //        lbl_Isfak.Text = "是";
    //            //        lbl_SWFine.Text = Decimal.Round(data.Fine.Value, 2).ToString();
    //            //    }
    //            //}
    //            //catch
    //            //{

    //            //    lbl_Isfak.Text = "否";
    //            //    lbl_SWFine.Text = "";
    //            //}
    //            //2.行政处罚信息绑定
    //            if (data.Isadministrativepenalty.Value == 1)
    //            {
    //                lbl_IsAp.Text = "是";
    //            }
    //            else
    //            {
    //                lbl_IsAp.Text = "否";
    //            }
    //            //3.是否公布信息绑定
    //            if (data.Ispublic)
    //            {
    //                lbl_Isamp.Text = "已公布";
    //            }
    //            else
    //            {
    //                lbl_Isamp.Text = "未公布";
    //            }
    //            //4.三违积分绑定
    //            //try
    //            //{
    //            //    lbl_SWPoints.Text = data.Scores.Value.ToString();
    //            //}
    //            //catch
    //            //{
    //            //    lbl_SWPoints.Text = "0";
    //            //}

    //            return true;
    //        }
    //        else
    //            return false;
    //    }
    //    catch
    //    {
    //        return false;
    //    }
    //}



    #endregion

    #region 信息处理模块

    [AjaxMethod]
    public void SWpublish() //已改
    {
        //保存三违信息确认的数据库信息
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        //DBSCMDataContext dc = new DBSCMDataContext();
        var SWcheck = dc.Nswinput.Where(p => p.Id == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
        foreach (var r in SWcheck)
        {
            //r.ID = Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim());
            r.Fine = lbl_Fine.Text.Trim() == "" ? 0 : Convert.ToDecimal(lbl_Fine.Text.Trim());
            r.Ispublic = Radio3.Checked ? 1 : 0;
            r.Isadministrativepenalty = Radio1.Checked ? 1 : 0;
            r.Isend = 1;
            r.Islearn = Radio5.Checked ? 1 : 0;
            try
            {
                dc.SubmitChanges();
            }
            catch (Exception er)
            {
                Ext.Msg.Alert("", er.ToString()).Show();
            }
        }
        //短信提醒三违人员
        if (ckb_IsSms.Checked == true)
        {
            ////获取发送者账号信息
            //string user = "";
            //user = SessionBox.GetUserSession().LoginName;
            ////获取手机号码
            //var per = dc.Person.Where(p => p.Name == tf_name.Text.Trim());
            //string tel = "";
            //foreach (var r in per)
            //{
            //    tel = r.Tel.Trim();
            //}
            ////获取发送的短信内容
            //string content = "";
            //if (ta_smsText.Text == "")
            //{
            //    content = "祁南矿干部走动式巡查管理系统提示您：您有一条三违信息已经确认！";
            //}
            //else
            //{
            //    content = ta_smsText.Text.Trim();
            //}
            ////发送短信
            //Sms sms = new Sms();
            //sms.SendSms(GhtnTech.Utility.ExtensionHelper.SmsType.HiddenTroubleTips, user, content, tel);
        }
        Ext.Msg.Alert("提示", "该三违信息已确认!").Show();
        ckb_IsSms.Checked = false;
        storeload();
        Window2.Hide();
    }

    [AjaxMethod]
    public void IsSms()//处理三违信息短信提醒确认
    {
        //if (ckb_IsSms.Checked == true)
        //{
        //    RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        //    var swinput = from sw in dc.SanWweiInput
        //                  from p in dc.Person
        //                  where sw.SWPersonID==p.PersonID && sw.ID==Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim())
        //                  select new 
        //                  {
        //                      p.Name
        //                  };
        //    foreach (var r in swinput)
        //    {
        //        tf_name.Text = r.Name;
        //    }
        //    tf_name.Disabled = true;
        //    ta_smsText.Disabled = false;
        //}
        //else if (ckb_IsSms.Checked == false)
        //{
        //    tf_name.Disabled = true;
        //    ta_smsText.Disabled = true;
        //    tf_name.Text = "";
        //    ta_smsText.Text = "";
        //}
    }

    #endregion

    [AjaxMethod]
    public void SelectPerson()
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        string url = string.Format("FinePersonSelect.aspx?Post={0}&Mod={1}&Win={2}", sm.SelectedRow.RecordID, "sw", FineWin.ClientID);
        Ext.DoScript("#{FineWin}.load('" + url + "');#{FineWin}.show();");
    }

    #region 导出报表
    protected void ToExcel(object sender, EventArgs e)//导出报表
    {
        string json = GridDataExcel.Value.ToString();
        foreach (var r in GridPanel1.ColumnModel.Columns)
        {
            json = json.Replace("\"" + r.DataIndex.Trim() + "\"", "\"" + r.Header + "\"");
        }
        json = json.Replace("T00:00:00", "");
        json = json.Replace("\"Ispublic\":false", "\"是否发布\":\"否\"");
        json = json.Replace("\"Ispublic\":true", "\"是否发布\":\"是\"");
        json = json.Replace("\"是否闭合\":true", "\"是否闭合\":\"是\"");
        json = json.Replace("\"是否闭合\":false", "\"是否闭合\":\"否\"");

        string strFileName = "三违信息报表";
        Response.Clear();
        Response.Buffer = true;
        Response.Charset = "GB2312";

        StoreSubmitDataEventArgs eSubmit = new StoreSubmitDataEventArgs(json, null);
        XmlNode xml = eSubmit.Xml;

        this.Response.Clear();
        this.Response.ContentType = "application nd.ms-excel";
        Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(strFileName) + ".xls");
        XslCompiledTransform xtExcel = new XslCompiledTransform();
        xtExcel.Load(Server.MapPath("ExcelSW.xsl"));
        xtExcel.Transform(xml, null, this.Response.OutputStream);
        this.Response.End();
    }
    #endregion
}
