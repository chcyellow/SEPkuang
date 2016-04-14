using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Coolite.Ext.Web;
using GhtnTech.SEP.DAL;
using GhtnTech.SecurityFramework.BLL;

public partial class YSNewProcess_SWLearn_object : System.Web.UI.Page
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            Ext.DoScript("refreshTree(#{tpkind});");
        }
    }

    #region 构建刷新树

    private void TreeBuild(Coolite.Ext.Web.TreeNodeCollection nodes, decimal pid)
    {
        var lavel = from c in dc.CsBaseinfoset
                    where c.Fid == int.Parse(PublicMethod.ReadXmlReturnNode("SWJB", this))
                    select new
                    {
                        c.Infoid,
                        c.Infoname
                    };
        foreach (var r in lavel)
        {
            Coolite.Ext.Web.TreeNode asyncNode = new Coolite.Ext.Web.TreeNode();
            asyncNode.Text = r.Infoname;
            asyncNode.NodeID = r.Infoid.ToString();

            nodes.Add(asyncNode);
            asyncNode.Listeners.Click.Handler = string.Format("Coolite.AjaxMethods.GVLoad({0});", r.Infoid.ToString());
            asyncNode.Leaf = true;
        }
    }

    private Coolite.Ext.Web.TreeNodeCollection LoadTree(Coolite.Ext.Web.TreeNodeCollection nodes)
    {
        if (nodes == null)
        {
            nodes = new Coolite.Ext.Web.TreeNodeCollection();
        }//根节点为null时

        tpkind.Root.Clear();
        Coolite.Ext.Web.TreeNode root = new Coolite.Ext.Web.TreeNode();
        root.Text = "-1";
        root.NodeID = "三违分类";
        tpkind.Root.Add(root);
        TreeBuild(root.Nodes, -1);
        return nodes;
    }

    [AjaxMethod]
    public string RefreshMenu()
    {
        Coolite.Ext.Web.TreeNodeCollection nodes = LoadTree(tpkind.Root);
        return nodes.ToJson();
    }

    #endregion

    [AjaxMethod]
    public void GVLoad(int id)
    {
        hdnKindid.SetValue(id.ToString());
        var item = from i in dc.Swlearn
                   where i.Levelid == id && i.Deptnumber == SessionBox.GetUserSession().DeptNumber
                   //orderby i.Sort
                   select new
                   {
                       i.Lname,
                       i.Lid,
                       i.Intime,
                       i.Nstatus
                   };
        ItemStore.DataSource = item;
        ItemStore.DataBind();

        btnJoemNew.Disabled = false;
        //ItemControlSet();

        //JeomSet(-1);
    }

    protected void JoemRowClick(object sender, AjaxEventArgs e)//单击行事件
    {
        ControlSet();
    }

    private void ControlSet()
    {
        RowSelectionModel sm = gpJoem.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            switch (int.Parse(dc.Swlearn.First(p => p.Lid == decimal.Parse(sm.SelectedRow.RecordID)).Nstatus.ToString()))
            {
                case 0:
                    btnJoemUpdate.Disabled = false;
                    btnJoemDel.Disabled = false;
                    btnJoemPublic.Disabled = false;
                    break;
                case 1:
                    btnJoemUpdate.Disabled = true;
                    btnJoemDel.Disabled = false;
                    btnJoemPublic.Disabled = true;
                    break;
                case 2:
                    btnJoemUpdate.Disabled = true;
                    btnJoemDel.Disabled = true;
                    btnJoemPublic.Disabled = false;
                    break;
            }
        }
        else
        {
            btnJoemUpdate.Disabled = true;
            btnJoemDel.Disabled = true;
            btnJoemPublic.Disabled = true;
        }
    }

    [AjaxMethod]
    public void JoemInfo(string Action)
    {
        string value = "";
        if (Action == "edit")
        {
             RowSelectionModel sm = gpJoem.SelectionModel.Primary as RowSelectionModel;
             value = dc.Swlearn.First(p => p.Lid == decimal.Parse(sm.SelectedRow.RecordID)).Lname;
        }
        Ext.Msg.Show(new MessageBox.Config
        {
            Title = "学习项目维护",
            Message = "学习项目名称：",
            Width = 300,
            Value = value,
            ButtonsConfig = new MessageBox.ButtonsConfig
            {
                Yes = new MessageBox.ButtonConfig
                {
                    Handler = "Coolite.AjaxMethods.SaveSWLearn(text,'" + Action + "')",
                    Text = "保存"
                },
                No = new MessageBox.ButtonConfig
                {
                    Text = "取消"
                }
            },
            Buttons = MessageBox.Button.YESNO,
            Multiline = true,
            AnimEl = btnJoemUpdate.ClientID,
        });
    }

    [AjaxMethod]
    public void SaveSWLearn(string lname, string act)
    {
        if (act == "new")
        {
            Swlearn l = new Swlearn
            {
                Deptnumber = SessionBox.GetUserSession().DeptNumber,
                Intime = System.DateTime.Today,
                Levelid = int.Parse(hdnKindid.Value.ToString()),
                Lname = lname,
                Nstatus = 0
            };
            dc.Swlearn.InsertOnSubmit(l);
            dc.SubmitChanges();
        }
        else
        {
            RowSelectionModel sm = gpJoem.SelectionModel.Primary as RowSelectionModel;
            var l = dc.Swlearn.First(p => p.Lid == decimal.Parse(sm.SelectedRow.RecordID));
            l.Lname = lname;
            dc.SubmitChanges();
        }
        Ext.Msg.Alert("提示", "保存成功！").Show();
        GVLoad(int.Parse(hdnKindid.Value.ToString()));
        ControlSet();
    }

    [AjaxMethod]
    public void JoemAction(int action)
    {
        string msg = "是否确认" + (action == 0 ? "作废" : "启用") + "选中项目?";
        Ext.Msg.Confirm("提示", msg, new MessageBox.ButtonsConfig
        {
            Yes = new MessageBox.ButtonConfig
            {
                Handler = "Coolite.AjaxMethods.UpdateSWLearn(" + action + ");",
                Text = "确 定"
            },
            No = new MessageBox.ButtonConfig
            {
                Text = "取 消"
            }
        }).Show();
    }

    [AjaxMethod]
    public void UpdateSWLearn(int action)
    {
        RowSelectionModel sm = gpJoem.SelectionModel.Primary as RowSelectionModel;
        var l = dc.Swlearn.First(p => p.Lid == decimal.Parse(sm.SelectedRow.RecordID));
        l.Nstatus = (action == 0 ? 2 : 1);
        dc.SubmitChanges();
        Ext.Msg.Alert("提示", "操作成功！").Show();
        GVLoad(int.Parse(hdnKindid.Value.ToString()));
        ControlSet();
    }
}