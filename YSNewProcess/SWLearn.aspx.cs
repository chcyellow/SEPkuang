using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Coolite.Ext.Web;
using GhtnTech.SEP.DAL;
using GhtnTech.SecurityFramework.BLL;
using System.Xml;

public partial class YSNewProcess_SWLearn : System.Web.UI.Page
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            dfInban.SelectedDate = System.DateTime.Today;
            LearnStoreLoad();
            SelectedStoreLoad();
        }
    }

    protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)
    {
        LearnStoreLoad();
    }

    protected void RowClick(object sender, AjaxEventArgs e)//单击每行响应事件 已改
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0 &&sm.SelectedRows.Count < 2)
        {
            btn_detail.Disabled = false;
            btnsw.Disabled = false;
            //DBSCMDataContext dc = new DBSCMDataContext();
            var Input = dc.Swexamine.First(p => p.Swid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
            try
            {
                Button1.Disabled = Input.Isfinish.Value == 1 ? true : false;
                btnOutClass.Disabled = Input.Isfinish.Value == 1 ? true : false;
            }
            catch
            {
                Button1.Disabled = false;
                btnOutClass.Disabled = false;
            }
        }
        else if (sm.SelectedRows.Count > 1)
        {
            btn_detail.Disabled = true;
            btnsw.Disabled = true;
            Button1.Disabled = true;
            btnOutClass.Disabled = true;
            btnBY.Disabled = false;
        }
        else
        {
            btn_detail.Disabled = true;
            btnsw.Disabled = true;
            Button1.Disabled = true;
            btnOutClass.Disabled = true;
            btnBY.Disabled = true;
        }
    }

    [AjaxMethod]
    public void DetailLoad(int type)
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            var swe = (from se in dc.Swexamine
                      from sw in dc.Nswinput
                      from sb in dc.Swbase
                      where se.Swid==sw.Id && sw.Swid==sb.Swid && se.Swid== Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim())
                      select new 
                      {
                          se.Swid,
                          se.Result,
                          sb.Levelid,
                          sw.Maindeptid
                      }).First();
            var data = from l in dc.Swlearn
                       join r in dc.SwexamineDetail.Where(p => p.Swid == swe.Swid) on l.Lid equals r.Lid into gg
                       from g in gg.DefaultIfEmpty()
                       where l.Levelid == swe.Levelid && l.Deptnumber==swe.Maindeptid && l.Nstatus==1
                       select new
                       {
                           l.Lid,
                           l.Lname,
                           g.Remark,
                           isCheck = g.Swid != null
                       };
            LearnStore.DataSource = data;
            LearnStore.DataBind();
            try
            {
                tfResult.Text = swe.Result;
            }
            catch
            {
                tfResult.Text = "";
            }
            if (type == 1)
            {
                btnSave.Show();
                btnSure.Show();
            }
            else if (type == 2)
            {
                btnSave.Show();
                btnSure.Show();
            }
            else
            {
                btnSave.Hide();
                btnSure.Hide();
            }
            ActionWindow.Show();
        }
    }

    [AjaxMethod]
    public void BanAddPerson()
    {
        RowSelectionModel sm = gpPer.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            foreach (var r in sm.SelectedRows)
            { 
                DBSCMDataContext db = new DBSCMDataContext();
                var sw = db.Nswinput.First(p => p.Swpersonid == r.RecordID && p.Islearn == 1);
                Swexamine se = new Swexamine
                {
                    Swid = sw.Id,
                    Stime = dfInban.SelectedDate,
                    Isfinish = 0,
                    Result = " "
                };
                db.Swexamine.InsertOnSubmit(se);
                sw.Islearn = 2;
                db.SubmitChanges();
            }
            Ext.Msg.Alert("提示", "进班成功，共添加" + sm.SelectedRows.Count.ToString() + "人").Show();
            SelectedStoreLoad();
            sm.SelectedRows.Clear();
            sm.UpdateSelection();
            LearnStoreLoad();
            btn_detail.Disabled = true;
            btnsw.Disabled = true;
            Button1.Disabled = true;
        }
    }

    private void LearnStoreLoad()
    {
        var data= from s in dc.Swexamine
                  from sw in dc.Nswinput
                  from sh in dc.Getswandhazusing
                  from per in dc.Person
                  from dep in dc.Department
                  where s.Swid==sw.Id && sw.Swid == sh.Swid 
                  && sw.Swpersonid == per.Personnumber
                  && sh.Deptnumber == SessionBox.GetUserSession().DeptNumber
                  && per.Deptid == dep.Deptnumber
                  && sw.Maindeptid == SessionBox.GetUserSession().DeptNumber
                  orderby sw.Intime descending
                  select new
                  {
                      s.Swid,
                      Swperson = per.Name,
                      Kqname = dep.Deptname,
                      sh.Levelname,
                      s.Stime,
                      s.Etime,
                      s.Result,
                      Isfinish=s.Isfinish==1
                  };
        Store1.DataSource = data;
        Store1.DataBind();
    }

    private void SelectedStoreLoad()
    {
        var data=from sw in dc.Nswinput
                  from per in dc.Person
                  from dep in dc.Department
                  where sw.Swpersonid == per.Personnumber
                  && per.Deptid == dep.Deptnumber
                  && sw.Maindeptid == SessionBox.GetUserSession().DeptNumber
                  && sw.Islearn==1
                  orderby sw.Intime descending
                 select new
                 {
                     per.Personnumber,
                     per.Name,
                     dep.Deptname
                 };
        SelectedStore.DataSource = data;
        SelectedStore.DataBind();
    }

    protected void SaveD(object sender, BeforeStoreChangedEventArgs e)
    {
        XmlNode xml = e.DataHandler.XmlData;
        XmlNode updated = xml.SelectSingleNode("records/Updated");
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        if (updated != null)
        {
            XmlNodeList uRecords = updated.SelectNodes("record");
            if (uRecords.Count > 0)
            {
                foreach (XmlNode record in uRecords)
                {
                    if (record != null)
                    {
                        DBSCMDataContext dc1 = new DBSCMDataContext();
                        foreach (var item in sm.SelectedRows)
                        {
                            
                            var rd = dc1.SwexamineDetail.Where(p => p.Lid == decimal.Parse(record.SelectSingleNode("Lid").InnerText) && p.Swid == decimal.Parse(item.RecordID));
                            if (record.SelectSingleNode("isCheck").InnerText.Trim() == "false")
                            {
                                dc1.SwexamineDetail.DeleteOnSubmit(rd.First());
                                //dc1.SubmitChanges();
                            }
                            else
                            {
                                if (rd.Count() == 0)
                                {

                                    SwexamineDetail ra = new SwexamineDetail
                                    {
                                        Lid = decimal.Parse(record.SelectSingleNode("Lid").InnerText),
                                        Remark = record.SelectSingleNode("Remark").InnerText,
                                        Swid = decimal.Parse(item.RecordID)
                                    };
                                    dc1.SwexamineDetail.InsertOnSubmit(ra);
                                    
                                    //dc1.SubmitChanges();
                                }
                            }
                        }
                        dc1.SubmitChanges();
                    }
                }
            }
        }
        foreach (var item in sm.SelectedRows)
        {
            var se = dc.Swexamine.First(p => p.Swid == decimal.Parse(item.RecordID));
            se.Result = tfResult.Text;
        }
        dc.SubmitChanges();
        Ext.Msg.Alert("提示", "保存成功!").Show();
        LearnStoreLoad();
    }

    #region 毕业操作

    [AjaxMethod]
    public void isfini()
    {
        Ext.Msg.Confirm("提示", "毕业后将不能修改该数据，是否确定毕业?", new MessageBox.ButtonsConfig
        {
            Yes = new MessageBox.ButtonConfig
            {
                Handler = "Coolite.AjaxMethods.SureFinish();",
                Text = "确 定"
            },
            No = new MessageBox.ButtonConfig
            {
                Text = "取 消"
            }
        }).Show();
    }

    [AjaxMethod]
    public void SureFinish()
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            bool isfinish = false;
            foreach (var item in sm.SelectedRows)
            {
                var sd = dc.SwexamineDetail.Where(p => p.Swid == decimal.Parse(item.RecordID));
                if (sd.Count() > 0)
                {

                    var se = dc.Swexamine.First(p => p.Swid == decimal.Parse(item.RecordID));
                    if (se.Isfinish == 1)
                        continue;
                    se.Isfinish = 1;
                    se.Etime = System.DateTime.Today;
                    isfinish = true;
                    
                }
                else
                {
                    isfinish = false;
                    
                    break;
                }
            }
            if (isfinish)
            {
                dc.SubmitChanges();
                Ext.Msg.Alert("提示", "操作成功!").Show();
                LearnStoreLoad();
                ActionWindow.Hide();
            }
            else
            {
                Ext.Msg.Alert("提示", "至少要进行一个学习项目才能毕业!请先保存学习进度！").Show();
            }
        }
    }

    #endregion

    #region 退班操作

    [AjaxMethod]
    public void isOutClass()
    {
        Ext.Msg.Confirm("提示", "退班后将将清除该条数据，是否确定退班?", new MessageBox.ButtonsConfig
        {
            Yes = new MessageBox.ButtonConfig
            {
                Handler = "Coolite.AjaxMethods.SureOutClass();",
                Text = "确 定"
            },
            No = new MessageBox.ButtonConfig
            {
                Text = "取 消"
            }
        }).Show();
    }

    [AjaxMethod]
    public void SureOutClass()
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        var sd = dc.SwexamineDetail.Where(p => p.Swid == decimal.Parse(sm.SelectedRow.RecordID));
        var se = dc.Swexamine.First(p => p.Swid == decimal.Parse(sm.SelectedRow.RecordID));
        dc.SwexamineDetail.DeleteAllOnSubmit(sd);
        dc.Swexamine.DeleteOnSubmit(se);
        dc.SubmitChanges();
        try
        {
            var sw = dc.Nswinput.First(p => p.Id == se.Swid);
            sw.Islearn = 1;
            dc.SubmitChanges();
        }
        catch
        {
        }
        Ext.Msg.Alert("提示", "操作成功!").Show();
        LearnStoreLoad();
    }

    #endregion

    #region 移除待进班人员
    [AjaxMethod]
    public void isBanoutqueue()
    {
        Ext.Msg.Confirm("提示", "是否移除选中人员?", new MessageBox.ButtonsConfig
        {
            Yes = new MessageBox.ButtonConfig
            {
                Handler = "Coolite.AjaxMethods.Banoutqueue();",
                Text = "确 定"
            },
            No = new MessageBox.ButtonConfig
            {
                Text = "取 消"
            }
        }).Show();
    }

    [AjaxMethod]
    public void Banoutqueue()
    {
        RowSelectionModel sm = gpPer.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            foreach (var r in sm.SelectedRows)
            {
                DBSCMDataContext db = new DBSCMDataContext();
                var sw = db.Nswinput.First(p => p.Swpersonid == r.RecordID && p.Islearn == 1);
                sw.Islearn = 2;
                db.SubmitChanges();
            }
            Ext.Msg.Alert("提示", "移除成功，共移除" + sm.SelectedRows.Count.ToString() + "人").Show();
            SelectedStoreLoad();
            sm.SelectedRows.Clear();
            sm.UpdateSelection();
        }
    }

    #endregion

    #region 明细模块

    [AjaxMethod]
    public void SWLoad()//加载明细信息
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
                        where sw.Swid == ha.Swid && sw.Id==decimal.Parse(ID)
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
}
