using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GhtnTech.SecurityFramework;
using GhtnTech.SecurityFramework.Model;
using GhtnTech.SecurityFramework.BLL;
using GhtnTech.SEP.DAL;
using Coolite.Ext.Web;
using System.Data;
using System.Collections;

public partial class mainframes : BasePage
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!SessionBox.CheckUserSession())
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                //弹出公告
                winNotice.Hide();
                Ext.DoScript("showMessage();");
                GetNoticeInfo();

                if (!Ext.IsAjaxRequest)
                {
                    AutoDoSth();
                    //AutoUpdate();
                    lblLoginName.Text = SessionBox.GetUserSession().Name; //== "ghtntech" ? "国华天能管理员" : (from p in dc.Person where p.PersonNumber == SessionBox.GetUserSession().PersonNumber select p.Name).Single();
                    lblRoleName.Text = "所属单位：";
                    lblRole.Text = SessionBox.GetUserSession().DeptName;
                    List<string> Role = SessionBox.GetUserSession().Role;
                    if (Role.Count != 0)
                    {
                        lblR.Text = "您的身份：";
                        for (int i = 0; i < Role.Count; i++)
                        {
                            cboRoleName.Items.Add(new Coolite.Ext.Web.ListItem(Role[i].ToString().Split(',')[1], Role[i].ToString().Split(',')[0]));
                        }
                        //cboRoleName.SelectedIndex = 0;
                        cboRoleName.SelectedItem.Value = Role[0].ToString().Split(',')[0];
                        //cboRoleName.SelectedItem.Text = Role[0].ToString().Split(',')[1];
                        if (Role.Count > 1)
                        {
                            cboRoleName.Disabled = false;
                        }
                        else
                        {
                            cboRoleName.Disabled = true;
                        }
                    }
                    else
                    {
                        lblR.Text = "程序维护员";
                    }
                    InitMain();

                    lblServerIP.Text = System.Web.HttpContext.Current.Request.ServerVariables["Local_Addr"];
                }
            }
        }
          
    }
    [AjaxMethod]
    public void InitMain()
    {
        try
        {
            cboRoleName.SelectedItem.Value = SessionBox.GetUserSession().CurrentRole[0].ToString().Split(',')[0];

        }
        catch
        {
            List<string> al = new List<string>();
            al.Add(cboRoleName.SelectedItem.Value + "," + cboRoleName.SelectedItem.Text);
            SessionBox.GetUserSession().CurrentRole = al;
        }
        //if (SessionBox.GetUserSession().CurrentRole == null)
        //{
        //    List<string> al = new List<string>();
        //    al.Add(cboRoleName.SelectedItem.Value + "," + cboRoleName.SelectedItem.Text);
        //    SessionBox.GetUserSession().CurrentRole = al;
        //}
        //else
        //{
        //    cboRoleName.SelectedItem.Value = SessionBox.GetUserSession().CurrentRole[0].ToString().Split(',')[0];
        //}
        if (Request.Params["txtUrl"] != null && Request.Params["txtUrl"] != "-1")
        {
            Ext.DoScript("#{pnlFrame}.load('" + Request.Params["txtUrl"].ToString().Trim() + "');");
        }
        else
        {
            if (SessionBox.GetUserSession().CurrentRole[0].ToString().Split(',')[0] == "43")
            {
                Ext.DoScript("#{pnlFrame}.load('Main_Leader.aspx');");
            }
            else if (SessionBox.GetUserSession().CurrentRole[0].ToString().Split(',')[0] == "46")
            {
                Ext.DoScript("#{pnlFrame}.load('Main_JTLeader.aspx');");
            }
            else
            {
                Ext.DoScript("#{pnlFrame}.load('Main.aspx');");
            }
        }
        InitMenu();
    }
    [AjaxMethod]
    public void PageChange()
    {
        List<string> al = new List<string>();
        al.Add(cboRoleName.SelectedItem.Value + "," + cboRoleName.SelectedItem.Text);
        SessionBox.GetUserSession().CurrentRole = al;
        //Response.Redirect("MainFrame.aspx", true);
        if (SessionBox.GetUserSession().CurrentRole[0].ToString().Split(',')[0] == "43")
        {
            Ext.DoScript("#{pnlFrame}.load('Main_Leader.aspx');");
        }
        else if (SessionBox.GetUserSession().CurrentRole[0].ToString().Split(',')[0] == "46")
        {
            Ext.DoScript("#{pnlFrame}.load('Main_JTLeader.aspx');");
        }
        else
        {
            Ext.DoScript("#{pnlFrame}.load('Main.aspx');");
        }
    }
    /// <summary>
    /// 初始化菜单
    /// </summary>
    protected void InitMenu()
    {
        
        UserSession user = SessionBox.GetUserSession();
        Module modulesCrud = new Module();
        DataTable dtModuleType = modulesCrud.GetModuleGroupList("").Tables[0];  //取得所有数据得到DataTable 
        pnlTree.Items.Clear();
        Accordion accordion = new Accordion();
        if (dtModuleType.Rows.Count > 0)
        {
            for (int i = 0; i < dtModuleType.Rows.Count; i++)
            {
                TreePanel tree = new TreePanel();
                tree.AutoScroll = true;
                tree.Title = dtModuleType.Rows[i]["MODULEGROUPNAME"].ToString().Trim();
                tree.RootVisible = false;

                Coolite.Ext.Web.TreeNode root = new Coolite.Ext.Web.TreeNode();
                root.NodeID = dtModuleType.Rows[i]["MODULEGROUPID"].ToString();
                tree.Root.Add(root);

                DataTable dtModules = modulesCrud.GetModuleList("MODULEGROUPID=" + dtModuleType.Rows[i]["MODULEGROUPID"].ToString()).Tables[0];
                if (dtModules.Rows.Count > 0)
                {

                    for (int j = 0; j < dtModules.Rows.Count; j++)
                    {
                        bool b = UserHandle.ValidationModule(int.Parse(dtModules.Rows[j]["MODULEID"].ToString()), PermissionTag.Browse);
                        if (SessionBox.GetUserSession().Status == "1" && dtModules.Rows[j]["STATUS"].ToString().ToLower() == "1" && b)
                        {
                            if (dtModules.Rows[j]["MODULENAME"].ToString().Trim() == "发送短信" && (SessionBox.GetUserSession().PersonNumber != "68887" && SessionBox.GetUserSession().LoginName != "yu"))
                            {
                                continue;
                            }
                            Coolite.Ext.Web.TreeNode node = new Coolite.Ext.Web.TreeNode(dtModules.Rows[j]["MODULEID"].ToString(), dtModules.Rows[j]["MODULENAME"].ToString().Trim(), Icon.ApplicationViewList);
                            node.Qtip = dtModules.Rows[j]["MODULENAME"].ToString().Trim();
                            node.Listeners.Click.Handler = string.Format("reload('{0}', '{1}');", dtModules.Rows[j]["MODULENAME"].ToString().Trim(), dtModules.Rows[j]["MODULEURL"].ToString().Trim());
                            node.Expanded = false;
                            root.Nodes.Add(node);
                        }
                    }

                    if (root.Nodes.Count > 0)
                    {
                        accordion.Items.Add(tree);
                    }
                }
                if (accordion.Items.Count > 0)
                {
                    pnlTree.BodyControls.Add(accordion);
                }
            }
        }
    }

    #region 获取公告信息
    private void GetNoticeInfo()
    {
        string text = "";
        if (SessionBox.GetUserSession().rolelevel.Contains("1") || SessionBox.GetUserSession().rolelevel.Contains("0"))
        {
            var q = (from d in dc.Sysnotice
                     where d.Nstatus.Trim() == "已发布" && d.Edate >= System.DateTime.Today.AddDays(-7) && d.Edate <= System.DateTime.Today
                     && d.Edeptid == SessionBox.GetUserSession().DeptNumber
                     orderby d.Nstatus, d.Nid
                     select new
                     {
                         d.Nid,
                         d.Ntitle
                     }).Take(5);
            if (q.Count() > 0)
            {
                foreach (var r in q)
                {
                    string url = "SystemNotice/NoticeManageView.aspx?Nid=" + r.Nid.ToString().Trim();
                    text += "<br /><a href=\"#\" onclick=\"openwin('" + url + "')\">" + r.Ntitle.Trim() + "</a><br />";
                    //text += "<a href=\"SystemNotice/NoticeManageView.aspx?Nid=" + r.Nid.ToString().Trim() + "\" target=\"_bank\" >" + r.Ntitle.Trim() + "</a>";
                }
            }
            else
            {
                text = "<br />当前无最新公告！";
                winNotice.Hide();
            }
        }
        else
        {
            //var q = ((from d in dc.Sysnotice
            //          where d.Nstatus.Trim() == "已发布" && d.Edate >= System.DateTime.Today.AddDays(-7) && d.Edate <= System.DateTime.Today
            //          && d.Edeptid == SessionBox.GetUserSession().DeptNumber
            //          orderby d.Nstatus, d.Nid
            //          select new
            //          {
            //              d.Nid,
            //              d.Ntitle
            //          }).Concat(
            //        from n in dc.Sysnotice
            //        from nf in dc.Sysnoticefb
            //        where n.Nid == nf.Nid && n.Nstatus.Trim() == "已发布" && n.Edate >= System.DateTime.Today.AddDays(-7) && n.Edate <= System.DateTime.Today
            //        && nf.Deptid == SessionBox.GetUserSession().DeptNumber
            //        select new
            //        {
            //            n.Nid,
            //            n.Ntitle
            //        }
            //        )).Take(5);
            var q = ((from d in dc.Sysnotice
                      where d.Nstatus.Trim() == "已发布" && d.Edate >= System.DateTime.Today.AddDays(-7) && d.Edate <= System.DateTime.Today
                      && d.Edeptid == SessionBox.GetUserSession().DeptNumber
                      select new
                      {
                          d.Nid,
                          d.Ntitle,
                          d.Edate
                      }).Concat(
                    from n in dc.Sysnotice
                    from nf in dc.Sysnoticefb
                    where n.Nid == nf.Nid && n.Nstatus.Trim() == "已发布" && n.Edate >= System.DateTime.Today.AddDays(-7) && n.Edate <= System.DateTime.Today
                    && nf.Deptid == SessionBox.GetUserSession().DeptNumber
                    select new
                    {
                        n.Nid,
                        n.Ntitle,
                        n.Edate
                    }
                    )).OrderByDescending(p=>p.Edate).Take(5);
            if (q.Count() > 0)
            {
                foreach (var r in q)
                {
                    string url = "SystemNotice/NoticeManageView.aspx?Nid=" + r.Nid.ToString().Trim();
                    text += "<br /><a href=\"#\" onclick=\"openwin('" + url + "')\">" + r.Ntitle.Trim() + "</a><br />";
                    //text += string.Format("<li><span><a href={0}\" target=\"_bank\" ></a></span>{1}</li>", "SystemNotice/NoticeManageView.aspx?Nid=" + r.Nid, r.Ntitle.Trim());
                }
            }
            else
            {
                text = "<br />当前无最新公告！";
                winNotice.Hide();
            }
        }
        NoticeDetail.Html = text;
    }
    #endregion

    #region 首页
    [AjaxMethod]
    public void homeload()
    {
        if (SessionBox.GetUserSession().CurrentRole[0].ToString().Split(',')[0] == "43")
        {
            Ext.DoScript("#{pnlFrame}.load('Main_Leader.aspx');");
        }
        else if (SessionBox.GetUserSession().CurrentRole[0].ToString().Split(',')[0] == "46")
        {
            Ext.DoScript("#{pnlFrame}.load('Main_JTLeader.aspx');");
        }
        else
        {
            Ext.DoScript("#{pnlFrame}.load('Main.aspx');");
        }
    }
    #endregion

    #region  退出系统
    [AjaxMethod]
    public void Cancel()
    {
        Ext.Msg.Confirm("提示", "是否确认退出?", new MessageBox.ButtonsConfig
        {
            Yes = new MessageBox.ButtonConfig
            {
                Handler = "Coolite.AjaxMethods.Exit();",
                Text = "确 定"
            },
            No = new MessageBox.ButtonConfig
            {
                Text = "取 消"
            }
        }).Show();
    }
    [AjaxMethod]
    public void Exit()
    {
        this.Session.Clear();
        Response.Redirect("login.aspx", true);
    }
    #endregion

    private void AutoDoSth()//自动处理流程-干部走动
    {
        PublicMethod.AutoSetMovePlanStatus();
    }

    private void AutoUpdate()
    {
        //var lavel = from c in dc.CsBaseinfoset
        //            where c.Fid == int.Parse(PublicMethod.ReadXmlReturnNode("YHJB", this))
        //            select new
        //            {
        //                c.Infoid,
        //                c.Infoname
        //            };
        var yh = dc.Nyhinput.Where(p => p.Status == "逾期未整改");
        foreach (var r in yh)
        {
            if (r.Levelid > 42)
            {
                r.Levelid = r.Levelid.Value - 1;
            }
            r.Status = "新增"; 
            //自动短信
            var msgall = dc.Getyhinput.First(p => p.Yhputinid == r.Yhputinid);
            string msg12 = msgall.Deptname + "在" + msgall.Placename + "的" + msgall.Levelname + "级隐患因逾期未整改，级别升级!";

            var smsd = dc.Yhsmsset.Where(p => p.Yhlevel == msgall.Levelid && p.Maindept == msgall.Unitid && p.Deptnumber == msgall.Deptid);
            List<string> per = new List<string>();
            foreach (var n in smsd)
            {
                per.Add(n.Person);
            }
            string msg = Sms.Send(per, msg12, r.Yhputinid.ToString(), SmsType.HiddenTroubleTips, "-安全生产体系支撑平台!");
            //Ext.Msg.Alert("提示", "发布成功!" + msg).Show();
        }
        dc.SubmitChanges();
    }

    [AjaxMethod]
    public void Win1load(string title, string url)
    {
        Window1.Title = title == "" ? "添加人员" : title;
        Ext.DoScript("#{Window1}.load('" + url + "');");
        Window1.Show();
    }
}
