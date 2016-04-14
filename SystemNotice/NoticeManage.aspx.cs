using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Coolite.Ext.Web;
using GhtnTech.SEP.DAL;
using GhtnTech.SecurityFramework.BLL;
using System.IO;

public partial class SystemNotice_NoticeManage : BasePage
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
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
                if (!UserHandle.ValidationHandle(PermissionTag.Add))//新增
                {
                    btn_S.Visible = false;
                    btn_D.Visible = false;
                }
                if (!UserHandle.ValidationHandle(PermissionTag.Publish))//发布
                {
                    btn_FaBu.Visible = false;
                }
                if (!UserHandle.ValidationHandle(PermissionTag.Delete))//删除
                {
                    btn_Delete.Visible = false;
                }
            }

            if (!Ext.IsAjaxRequest)
            {
                NidLoad.Text = "";
                pnlAnnex.Disabled = true;
                pnlAnnex.Html = "请上传附件！";
                btn_Delete.Disabled = true;
                btn_FaBu.Disabled = true;
                dfBegin.SelectedDate = System.DateTime.Today.AddDays(-7);
                dfEnd.SelectedDate = System.DateTime.Today;
                //数据绑定
                storeload();
            }
        }
    }

    protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)
    {
        storeload();
    }

    [AjaxMethod]
    public void storeload()//数据初始化
    {
        if (dfBegin.SelectedDate > dfEnd.SelectedDate)
        {
            Ext.Msg.Alert("提示", "请选择正确日期").Show();
            return;
        }
        //角色判断-集团公司、矿俩级公告
        if (SessionBox.GetUserSession().rolelevel.Contains("1") || SessionBox.GetUserSession().rolelevel.Contains("0"))
        {
            var q = from d in dc.Sysnotice
                    where d.Edate >= dfBegin.SelectedDate && d.Edate <= dfEnd.SelectedDate
                    && d.Edeptid == SessionBox.GetUserSession().DeptNumber
                    orderby d.Nstatus, d.Nid
                    select new
                    {
                        d.Nid,
                        d.Ntitle,
                        d.Nmessage,
                        Pname = d.Pperid == "" || d.Pperid == null ? "" : dc.Person.Single(p => p.Personnumber == d.Pperid).Name,
                        Pdept = d.Pdeptid == "" || d.Pdeptid == null ? "" : dc.Department.Single(p => p.Deptnumber == d.Pdeptid).Deptname,
                        d.Pdate,
                        d.Nstatus
                    };
            Store1.DataSource = q;
            Store1.DataBind();
        }
        else
        {
            var q = (from d in dc.Sysnotice
                     where d.Edate >= dfBegin.SelectedDate && d.Edate <= dfEnd.SelectedDate
                     && d.Edeptid == SessionBox.GetUserSession().DeptNumber
                     orderby d.Nstatus, d.Nid
                     select new
                     {
                         d.Nid,
                         d.Ntitle,
                         d.Nmessage,
                         Pname = d.Pperid == "" || d.Pperid == null ? "" : dc.Person.Single(p => p.Personnumber == d.Pperid).Name,
                         Pdept = d.Pdeptid == "" || d.Pdeptid == null ? "" : dc.Department.Single(p => p.Deptnumber == d.Pdeptid).Deptname,
                         d.Pdate,
                         d.Nstatus
                     }).Concat(
                    from n in dc.Sysnotice
                    from nf in dc.Sysnoticefb
                    where n.Nid == nf.Nid && n.Edate >= dfBegin.SelectedDate && n.Edate <= dfEnd.SelectedDate
                    && nf.Deptid == SessionBox.GetUserSession().DeptNumber
                    select new
                    {
                        n.Nid,
                        n.Ntitle,
                        n.Nmessage,
                        Pname = n.Pperid == "" || n.Pperid == null ? "" : dc.Person.Single(p => p.Personnumber == n.Pperid).Name,
                        Pdept = n.Pdeptid == "" || n.Pdeptid == null ? "" : dc.Department.Single(p => p.Deptnumber == n.Pdeptid).Deptname,
                        n.Pdate,
                        n.Nstatus
                    }
                    );
            Store1.DataSource = q;
            Store1.DataBind();
        }
    }

    #region 单击行事件
    protected void RowClick(object sender, AjaxEventArgs e)//流程处理
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            //加载详情
            loadDetail();
            if (dc.Sysnotice.First(p => p.Nid == decimal.Parse(sm.SelectedRow.RecordID)).Nstatus.Trim() == "编辑")
            {
                btn_FaBu.Disabled = false;
                btn_Delete.Disabled = false;
                btn_S.Disabled = false;
                btn_D.Disabled = false;
                lbtn_Again.Disabled = false;
                btn_Chexiao.Disabled = true;
            }
            else
            {
                btn_FaBu.Disabled = true;
                btn_Delete.Disabled = true;
                btn_S.Disabled = true;
                btn_D.Disabled = false;
                lbtn_Again.Disabled = true;
                btn_Chexiao.Disabled = false;
            }
        }
        else
        {
            btn_FaBu.Disabled = true;
            btn_Delete.Disabled = true;
            btn_S.Disabled = false;
            btn_D.Disabled = false;
            lbtn_Again.Disabled = true;
            btn_Chexiao.Disabled = true;
        }
    }
    #endregion

    #region 单击行加载详情
    private void loadDetail()
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        NidLoad.Text = sm.SelectedRow.RecordID;

        var q = dc.Sysnotice.Where(p => p.Nid == decimal.Parse(sm.SelectedRow.RecordID));
        foreach (var r in q)
        {
            try
            {
                BasicField.Disabled = true;
                pnlAnnex.Disabled = false;
                pnlAnnex.Html = "<a href=\"FileDown.aspx?Nid=" + r.Nid.ToString().Trim() + "\" target=\"_bank\">" + r.Nfilename.Trim() + "</a>";
            }
            catch
            {
                BasicField.Disabled = false;
                pnlAnnex.Disabled = true;
                pnlAnnex.Html = "该公告没有附件！";
            }
            tf_Title.Text = r.Ntitle;
            ta_Message.Text = r.Nmessage;
        }
    }
    #endregion

    #region 原有附件，现在重新上传-设置
    [AjaxMethod]
    public void Again()
    {
        Ext.Msg.Confirm("提示", "重新上传将删除原有附件，是否确定重新上传?", new MessageBox.ButtonsConfig
        {
            Yes = new MessageBox.ButtonConfig
            {
                Handler = "Coolite.AjaxMethods.AgainDel();",
                Text = "确 定"
            },
            No = new MessageBox.ButtonConfig
            {
                Text = "取 消"
            }
        }).Show();
    }
    [AjaxMethod]
    public void AgainDel()
    {
        var q = dc.Sysnotice.First(p => p.Nid == decimal.Parse(NidLoad.Text.Trim()));
        if (q.Nfileaddress != "" || q.Nfileaddress != null)
        {
            //删除原有附件
            string strPhyPath = Server.MapPath(q.Nfileaddress.Trim());
            File.Delete(strPhyPath);
        }
        pnlAnnex.Disabled = true;
        BasicField.Disabled = false;
    }
    #endregion

    #region 保存
    [AjaxMethod]
    public void Save()
    {
        if (tf_Title.Text.Trim() == "")
        {
            Ext.Msg.Alert("提示", "请输入公告标题！").Show();
            return;
        }
        if (ta_Message.Text.Trim() == "")
        {
            Ext.Msg.Alert("提示", "请输入公告内容！").Show();
            return;
        }
        Ext.Msg.Confirm("提示", "是否确认保存?", new MessageBox.ButtonsConfig
        {
            Yes = new MessageBox.ButtonConfig
            {
                Handler = "Coolite.AjaxMethods.SaveDetail();",
                Text = "确 定"
            },
            No = new MessageBox.ButtonConfig
            {
                Text = "取 消"
            }
        }).Show();
    }

    [AjaxMethod]
    public void SaveDetail()
    {
        string strOldFileName = "";
        string strNewFileName = "";
        if (BasicField.HasFile)
        {
            string strPath = MapPath("~") + @"\SystemNotice\FileUpload\";
            strOldFileName = BasicField.FileName.Substring(BasicField.FileName.LastIndexOf("\\") + 1);
            strNewFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + BasicField.FileName.Substring(BasicField.FileName.LastIndexOf("."));
            BasicField.PostedFile.SaveAs(strPath + strNewFileName);
        }
        if (NidLoad.Text.Trim() == "")
        {
            Sysnotice notice = new Sysnotice()
            {
                Ntitle = tf_Title.Text.Trim(),
                Nmessage = ta_Message.Text.Trim(),
                Nfilename = strOldFileName,
                Nfileaddress = "../SystemNotice/FileUpload/" + strNewFileName,
                Eperid = SessionBox.GetUserSession().PersonNumber,
                Edeptid = SessionBox.GetUserSession().DeptNumber,
                Edate = System.DateTime.Today,
                Nstatus = "编辑"
            };
            dc.Sysnotice.InsertOnSubmit(notice);
            dc.SubmitChanges();
            Ext.Msg.Alert("提示", "保存成功！").Show();
        }
        else
        {
            var q = dc.Sysnotice.First(p => p.Nid == decimal.Parse(NidLoad.Text.Trim()));
            q.Ntitle = tf_Title.Text.Trim();
            q.Nmessage = ta_Message.Text.Trim();
            if (BasicField.HasFile)
            {
                q.Nfilename = strOldFileName;
                q.Nfileaddress = "../SystemNotice/FileUpload/" + strNewFileName;
            }
            q.Eperid = SessionBox.GetUserSession().PersonNumber;
            q.Edeptid = SessionBox.GetUserSession().DeptNumber;
            q.Edate = System.DateTime.Today;
            q.Nstatus = "编辑";
            dc.SubmitChanges();
            Ext.Msg.Alert("提示", "修改成功！").Show();
        }
        Cancel();
        storeload();
    }
    #endregion

    #region 取消
    [AjaxMethod]
    public void Cancel()
    {
        NidLoad.Text = "";
        tf_Title.Text = "";
        ta_Message.Text = "";
        BasicField.Reset();

        BasicField.Disabled = false;
        pnlAnnex.Disabled = true;
        btn_S.Disabled = false;
        pnlAnnex.Html = "请上传附件！";
    }
    #endregion

    #region 公告发布
    [AjaxMethod]
    public void FBDXset()//提交对象绑定
    {
        var dept = from d in dc.Department
                   where d.Deptnumber.Substring(4) == "00000" && d.Deptname.EndsWith("矿")
                   select new
                   {
                       d.Deptnumber,
                       d.Deptname
                   };
        FBDXStore.DataSource = dept;
        FBDXStore.DataBind();
    }
    [AjaxMethod]
    public void Fabu()
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            if (SessionBox.GetUserSession().rolelevel.Contains("1") || SessionBox.GetUserSession().rolelevel.Contains("0"))
            {//集团公司以上级别发布
                RowSelectionModel sm1 = this.GridPanel2.SelectionModel.Primary as RowSelectionModel;
                if (sm1.SelectedRows.Count == 0)
                {
                    Ext.Msg.Alert("提示", "请选择发布对象!").Show();
                    return;
                }
                else
                {
                    for (int i = 0; i < sm1.SelectedRows.Count; i++)
                    {
                        Sysnoticefb noticefb = new Sysnoticefb()
                        {
                            Nid = decimal.Parse(sm.SelectedRow.RecordID),
                            Deptid = sm1.SelectedRows[i].RecordID.Trim(),
                            Beizhu = "",
                            Status = "有效"
                        };
                        dc.Sysnoticefb.InsertOnSubmit(noticefb);
                        dc.SubmitChanges();
                    }
                }
                var q = dc.Sysnotice.Where(p => p.Nid == decimal.Parse(sm.SelectedRow.RecordID));
                foreach (var r in q)
                {
                    r.Pperid = SessionBox.GetUserSession().PersonNumber;
                    r.Pdeptid = SessionBox.GetUserSession().DeptNumber;
                    r.Pdate = System.DateTime.Today;
                    r.Nstatus = "已发布";
                }
                dc.SubmitChanges();

                Ext.Msg.Alert("提示", string.Format("发布成功，共发布给{0}个对象!", sm1.SelectedRows.Count)).Show();
                Window1.Hide();
                storeload();
            }
            else
            {//矿级别以下发布
                Ext.Msg.Confirm("提示", "是否确认发布此公告?", new MessageBox.ButtonsConfig
                {
                    Yes = new MessageBox.ButtonConfig
                    {
                        Handler = "Coolite.AjaxMethods.FabuDetail();",
                        Text = "确 定"
                    },
                    No = new MessageBox.ButtonConfig
                    {
                        Text = "取 消"
                    }
                }).Show();
            }
        }
    }

    [AjaxMethod]
    public void FabuDetail()//矿级发布操作
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        var q = dc.Sysnotice.First(p => p.Nid == decimal.Parse(sm.SelectedRow.RecordID));
        q.Pdate = System.DateTime.Today;
        q.Pperid = SessionBox.GetUserSession().PersonNumber;
        q.Pdeptid = SessionBox.GetUserSession().DeptNumber;
        q.Nstatus = "已发布";
        dc.SubmitChanges();
        Ext.Msg.Alert("提示", "发布成功！").Show();
        storeload();
    }
    #endregion

    #region 撤销发布

    [AjaxMethod]
    public void isCxfb()
    {
        Ext.Msg.Confirm("提示", "是否撤销发布此公告?", new MessageBox.ButtonsConfig
        {
            Yes = new MessageBox.ButtonConfig
            {
                Handler = "Coolite.AjaxMethods.Xcfb();",
                Text = "确 定"
            },
            No = new MessageBox.ButtonConfig
            {
                Text = "取 消"
            }
        }).Show();
    }

    [AjaxMethod]
    public void Xcfb()
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        var q = dc.Sysnotice.First(p => p.Nid == decimal.Parse(sm.SelectedRow.RecordID));
        q.Nstatus = "编辑";
        dc.SubmitChanges();
        Ext.Msg.Alert("提示", "撤销成功！").Show();
        storeload();
    }

    #endregion 

    #region 删除
    [AjaxMethod]
    public void Delete()
    {
        Ext.Msg.Confirm("提示", "是否确认删除此公告?", new MessageBox.ButtonsConfig
        {
            Yes = new MessageBox.ButtonConfig
            {
                Handler = "Coolite.AjaxMethods.DelDetail();",
                Text = "确 定"
            },
            No = new MessageBox.ButtonConfig
            {
                Text = "取 消"
            }
        }).Show();
    }

    [AjaxMethod]
    public void DelDetail()
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        var q = dc.Sysnotice.First(p => p.Nid == decimal.Parse(sm.SelectedRow.RecordID));
        try
        {//删除附件
            string strPhyPath = Server.MapPath(q.Nfileaddress.Trim());
            File.Delete(strPhyPath);
        }
        catch { }
        //删除记录
        dc.Sysnotice.DeleteOnSubmit(q);
        dc.SubmitChanges();
        Ext.Msg.Alert("提示", "删除成功！").Show();
        storeload();
    }
    #endregion
}
