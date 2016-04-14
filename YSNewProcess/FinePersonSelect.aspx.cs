using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Coolite.Ext.Web;
using GhtnTech.SEP.DAL;
using GhtnTech.SecurityFramework.BLL;

public partial class YSNewProcess_FinePersonSelect : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["Action"]))
            {
                Button1.Visible = false;
            }
            if (!string.IsNullOrEmpty(Request.QueryString["Post"]))
            {
                CreateFKper(Request.QueryString["Post"], Request.QueryString["Mod"]);
            }
        }
    }

    private void CreateFKper(string pid,string mod)
    {
        DBSCMDataContext dc = new DBSCMDataContext();
        if (mod == "yh")
        {
            var data = from f in dc.Fineset
                       from l in dc.Objectset
                       from y in dc.Nyhinput
                       where f.Deptnumber == SessionBox.GetUserSession().DeptNumber && f.Pid == l.Pid && y.Levelid == f.Levelid && y.Yhputinid == decimal.Parse(pid)
                       select new
                       {
                           l.Pid,
                           l.Pname
                       };
            foreach (var r in data)
            {
                var yd = dc.Nyhfinedetail.Where(p => p.Yinputid == decimal.Parse(pid) && p.Pid == r.Pid);
                
                if (yd.Count() > 0)
                {
                    CreateCol(r.Pid, r.Pname, yd.First().Personnumber);
                }
                else if (r.Pname.Trim() == "责任人")
                {
                    var check = dc.Nyinhuancheck.Where(p => p.Yhputinid == decimal.Parse(pid));
                    try
                    {
                        CreateCol(r.Pid, r.Pname, check.Count() == 0 ? "" : check.First().Responsibleid);
                    }
                    catch
                    {
                        CreateCol(r.Pid, r.Pname, "");
                    }
                }
                else
                {
                    CreateCol(r.Pid, r.Pname, "");
                }
            }
        }
        else if(mod == "sw")
        {
            var data = from f in dc.Swfineset
                       from l in dc.Objectset
                       from y in dc.Nswinput
                       from sw in dc.Swbase
                       where f.Deptnumber == SessionBox.GetUserSession().DeptNumber && f.Pid == l.Pid && y.Swid == sw.Swid && sw.Levelid==f.Levelid && y.Id== decimal.Parse(pid)
                       select new
                       {
                           l.Pid,
                           l.Pname,
                           y.Swpersonid
                       };
            foreach (var r in data)
            {
                var yd = dc.Nswfinedetail.Where(p => p.Id == decimal.Parse(pid) && p.Pid == r.Pid);
                if (yd.Count() > 0)
                {
                    CreateCol(r.Pid, r.Pname, yd.First().Personnumber);
                }
                else if (r.Pname.Trim() == "责任人")
                {
                    CreateCol(r.Pid, r.Pname, r.Swpersonid);
                }
                else
                {
                    CreateCol(r.Pid, r.Pname, "");
                }
            }
        }
    }

    private void CreateCol(decimal pid,string name,string person)
    {
        ComboBox cbb = new ComboBox();
        cbb.StoreID = "FkrenStore";
        cbb.DisplayField = "Name";
        cbb.ValueField = "Personnumber";
        cbb.FieldLabel = name;
        cbb.HideTrigger = true;
        cbb.Listeners.Render.Fn = "function(f) {f.el.on('keyup', function(e) {if(window.event.keyCode==38 || window.event.keyCode==40 || window.event.keyCode==13){return;}Coolite.AjaxMethods.PYsearch(f.getRawValue(), 'FkrenStore');});}";
        cbb.ID = "cbbFk_" + pid;
        if (person != "")
        {
            DBSCMDataContext dc = new DBSCMDataContext();
            var per = dc.Person.First(p1 => p1.Personnumber == person);
            cbb.Items.Add(new Coolite.Ext.Web.ListItem(per.Name, per.Personnumber));
            cbb.SelectedItem.Value = person;
        }
        //cbb.Template.AddScript("<Html><tpl for=\".\"><tpl if=\"[xindex] == 1\"><table class=\"cbStates-list\"><tr><th>姓名</th><th>科区</th></tr></tpl><tr class=\"list-item\"><td style=\"padding:3px 0px;\" ext:qtip=\"{Name}\" width=\"80px\">{Name}</td><td width=\"120px\">{Name}</td></tr><tpl if=\"[xcount-xindex]==0\"></table></tpl></tpl></Html>");
        FormLayout1.Anchors.Add(cbb);
        Coolite.Ext.Web.Parameter p = new Coolite.Ext.Web.Parameter();
        p.Name = "fkat" + pid;
        p.Value = "#{cbbFk_" + pid + "}.getValue()";
        p.Mode = Coolite.Ext.Web.ParameterMode.Raw;
        btnSubmit.AjaxEvents.Click.ExtraParams.Add(p);
    }

    protected void SubmitPerson(object sender, AjaxEventArgs e)
    {
        //记录罚款信息
        if (Request.QueryString["Mod"] == "yh")
        {
            foreach (var r in e.ExtraParams)
            {
                if (r.Value.ToString().Trim() != "")
                {
                    string fkid = r.Name.Replace("fkat", "");
                    SaveYh(decimal.Parse(Request.QueryString["Post"]), decimal.Parse(fkid), r.Value.Trim());
                }
            }
        }
        else if(Request.QueryString["Mod"] == "sw")
        {
            foreach (var r in e.ExtraParams)
            {
                if (r.Value.ToString().Trim() != "")
                {
                    string fkid = r.Name.Replace("fkat", "");
                    SaveSw(decimal.Parse(Request.QueryString["Post"]), decimal.Parse(fkid), r.Value.Trim());
                }
            }
        }
        Ext.Msg.Alert("提示","保存成功！").Show();
        //AddSuccessAndClose(Request.QueryString["Win"]);
    }

    private void SaveYh(decimal yhid, decimal pid,string perid)
    {
        DBSCMDataContext db = new DBSCMDataContext();
        var fk = db.Nyhfinedetail.Where(p => p.Yinputid == yhid && p.Pid == pid);
        if (fk.Count() > 0)
        {
            var fd = fk.First();
            fd.Personnumber = perid;
            db.SubmitChanges();
        }
        else
        {
            var data = from f in db.Fineset
                       from y in db.Nyhinput
                       where f.Deptnumber == SessionBox.GetUserSession().DeptNumber && y.Levelid == f.Levelid && y.Yhputinid == yhid && f.Pid==pid
                       select new
                       {
                           f.Kcfine
                       };
            Nyhfinedetail nf = new Nyhfinedetail
            {
                Yinputid = yhid,
                Pid = pid,
                Personnumber = perid,
                Fine=data.First().Kcfine
            };
            db.Nyhfinedetail.InsertOnSubmit(nf);
            db.SubmitChanges();
        }
    }

    private void SaveSw(decimal swid, decimal pid, string perid)
    {
        DBSCMDataContext db = new DBSCMDataContext();
        var fk = db.Nswfinedetail.Where(p => p.Id == swid && p.Pid == pid);
        if (fk.Count() > 0)
        {
            var fd = fk.First();
            fd.Personnumber = perid;
            db.SubmitChanges();
        }
        else
        {
            var data = from f in db.Swfineset
                       from y in db.Nswinput
                       from sw in db.Swbase
                       where f.Deptnumber == SessionBox.GetUserSession().DeptNumber && y.Swid == sw.Swid && sw.Levelid == f.Levelid && y.Id == swid && f.Pid==pid
                       select new
                       {
                           y.Fine
                       };
            Nswfinedetail nf = new Nswfinedetail
            {
                Id=swid,
                Pid = pid,
                Personnumber = perid,
                Fine=data.First().Fine
            };
            db.Nswfinedetail.InsertOnSubmit(nf);
            db.SubmitChanges();
        }
    }

    [AjaxMethod]
    public void PYsearch(string py, string store)//拼音检索
    {
        switch (store.Trim())
        {
            case "FkrenStore":
                DBSCMDataContext dc = new DBSCMDataContext();
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

    public static void AddSuccessAndClose(string windowname) 
    { 
        string fn = @"function(but){if(but === 'ok'){parent.window." + windowname + ".hide();}}";
        Ext.Msg.Show(new MessageBox.Config
        {
            Title = "提示",
            Message = "保存成功",
            Buttons = MessageBox.Button.OK,
            Icon = MessageBox.Icon.INFO,
            Fn = new JFunction { Fn = fn }
        }
        ); 
    }
    [AjaxMethod]
    public void FineSure()
    {
        DBSCMDataContext db = new DBSCMDataContext();
        bool bl = false;
        if (Request.QueryString["Mod"].Trim() == "yh")
        {
            var detail = db.Nyhfinedetail.Where(p => p.Yinputid == decimal.Parse(Request.QueryString["Post"]));
            bl = detail.Count() == 0;
        }
        if (Request.QueryString["Mod"].Trim() == "sw" && !bl)
        {
            var detail = db.Nswfinedetail.Where(p => p.Id == decimal.Parse(Request.QueryString["Post"]));
            bl = detail.Count() == 0;
        }
        if (bl)
        {
            Ext.Msg.Confirm("提示", "当前没有提交任何人员，是否确认?", new MessageBox.ButtonsConfig
            {
                Yes = new MessageBox.ButtonConfig
                {
                    Handler = "Coolite.AjaxMethods.FineClose();",
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
            FineClose();
        }
        
    }

    [AjaxMethod]
    public void FineClose()
    {
        DBSCMDataContext db = new DBSCMDataContext();
        if (Request.QueryString["Mod"].Trim() == "yh")
        {
            var yh = db.Nyhinput.First(p => p.Yhputinid == decimal.Parse(Request.QueryString["Post"]));
            yh.Isfine = 1;
            db.SubmitChanges();
        }
        
        AddSuccessAndClose(Request.QueryString["Win"]);
    }
}
