using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Coolite.Ext.Web;
using GhtnTech.SEP.DAL;
using GhtnTech.SecurityFramework.BLL;

public partial class BaseManage_JTMovePerson : System.Web.UI.Page
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
        var dept = from c in dc.Department
                    where c.Visualfield == 3 //视野为3 全局
                    select new
                    {
                        c.Deptnumber,
                        c.Deptname
                    };
        foreach (var r in dept)
        {
            Coolite.Ext.Web.TreeNode asyncNode = new Coolite.Ext.Web.TreeNode();
            asyncNode.Text = r.Deptname;
            asyncNode.NodeID = r.Deptnumber.ToString();

            nodes.Add(asyncNode);
            asyncNode.Listeners.Click.Handler = string.Format("Coolite.AjaxMethods.GVLoad('{0}');", r.Deptnumber.ToString());
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
        root.NodeID = "走动部门";
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

    #region 部门操作

    [AjaxMethod]
    public void AddDept()
    {
        if (cbbDept.SelectedIndex == -1)
        {
            return;
        }
        if (SessionBox.GetUserSession().rolelevel.Contains("1") || SessionBox.GetUserSession().rolelevel.Contains("0"))
        {
            var dept = dc.Department.First(p => p.Deptnumber == cbbDept.SelectedItem.Value);
            if (dept.Visualfield == 3)
            {
                Ext.Msg.Alert("提示", "已添加的部门！").Show();
                return;
            }
            dept.Visualfield = 3;
            dc.SubmitChanges();
            Ext.Msg.Alert("提示", "保存成功！").Show();
            Ext.DoScript("refreshTree(#{tpkind});");
        }
        else
        {
            Ext.Msg.Alert("提示", "非局端用户，不能进行该项操作！").Show();
        }
    }

    [AjaxMethod]
    public void issureDelDept()
    {
        if (hdnKindid.Value.ToString() == "0" || hdnKindid.Value.ToString() == "")
        {
            Ext.Msg.Alert("提示", "请选择需要删除的部门！").Show();
            return;
        }
        Ext.Msg.Confirm("提示", "删除部门将同时删除该部门下的所有人员，是否确认？", new MessageBox.ButtonsConfig
        {
            Yes = new MessageBox.ButtonConfig
            {
                Handler = "Coolite.AjaxMethods.DeleteDept();",
                Text = "确 定"
            },
            No = new MessageBox.ButtonConfig
            {
                Text = "取 消"
            }
        }).Show();
    }

    [AjaxMethod]
    public void DeleteDept()
    {
        var dept = dc.Department.First(p => p.Deptnumber == hdnKindid.Value.ToString());
        var person = dc.Person.Where(p => p.Areadeptid == hdnKindid.Value.ToString());
        dept.Visualfield = 2;
        foreach (var p in person)
        {
            p.Visualfield = 2;
        }
        dc.SubmitChanges();
        Ext.Msg.Alert("提示", "删除成功！").Show();
        hdnKindid.SetValue("0");
        Ext.DoScript("refreshTree(#{tpkind});");
    }

    #endregion

    [AjaxMethod]
    public void PYsearch(string py, string store)
    {
        if (py.Trim() == "")
        {
            return;
        }
        switch (store.Trim())
        {
            case "DeptStore":
                var dept = from d in dc.Department
                           where (d.Deptnumber.Substring(0, 2) == "13" || d.Deptnumber.Substring(0, 4) == "2364")//1303为集团公司处级，2364为设备管理中心
                           && (d.Deptnumber.Substring(7) == "00" || d.Deptlevel == "正科级")
                           && (d.Visualfield != 3 || d.Visualfield == null)
                           && (dc.F_PINYIN(d.Deptname).ToLower().Contains(py.ToLower()) || d.Deptname.Contains(py.Trim()))
                           select new
                           {
                               d.Deptnumber,
                               d.Deptname
                           };
                DeptStore.DataSource = dept.Distinct();
                DeptStore.DataBind();
                break;
            case "personStore":
                var person = from pl in dc.Person
                             where (pl.Areadeptid == hdnKindid.Value.ToString() || pl.Maindeptid== hdnKindid.Value.ToString())
                             && (pl.Visualfield != 3 || pl.Visualfield==null)
                             && (dc.F_PINYIN(pl.Name).ToLower().Contains(py.ToLower()) || pl.Name.Contains(py.Trim()))
                             select new
                             {
                                 pl.Personnumber,
                                 pl.Name
                             };
                personStore.DataSource = person.Distinct();
                personStore.DataBind();
                break;
        }
    }

    [AjaxMethod]
    public void GVLoad(string id)
    {
        hdnKindid.SetValue(id.ToString());
        var item = from i in dc.Person
                   from d in dc.Department
                   join p in dc.Position on i.Personid equals p.Posid into gg
                   from tt in gg.DefaultIfEmpty()
                   where i.Deptid==d.Deptnumber
                   && i.Visualfield == 3 && (i.Areadeptid == id || i.Maindeptid==id)
                   //orderby i.Sort
                   select new
                   {
                       i.Name,
                       i.Personnumber,
                       Posname=tt.Posname==null?"":tt.Posname,
                       d.Deptname,
                       i.Visualfield
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
            btnJoemDel.Disabled = false;
        }
        else
        {
            btnJoemDel.Disabled = true;
        }
    }

    [AjaxMethod]
    public void PersonAdd()
    {
        if (cbbPerson.SelectedIndex == -1)
        {
            Ext.Msg.Alert("提示", "请选择需要添加的人员！").Show();
            return;
        }
        var person = dc.Person.First(p => p.Personnumber == cbbPerson.SelectedItem.Value);
        if (person.Visualfield==3)
        {
            Ext.Msg.Alert("提示", "已添加的人员！").Show();
            return;
        }
        person.Visualfield = 3;
        dc.SubmitChanges();
        Ext.Msg.Alert("提示", "保存成功！").Show();
        GVLoad(hdnKindid.Value.ToString());
        ControlSet();
    }


    [AjaxMethod]
    public void issurePersonDel()
    {
        Ext.Msg.Confirm("提示", "是否确认删除选中人员?", new MessageBox.ButtonsConfig
        {
            Yes = new MessageBox.ButtonConfig
            {
                Handler = "Coolite.AjaxMethods.PersonDel();",
                Text = "确 定"
            },
            No = new MessageBox.ButtonConfig
            {
                Text = "取 消"
            }
        }).Show();
    }

    [AjaxMethod]
    public void PersonDel()
    {
        RowSelectionModel sm = gpJoem.SelectionModel.Primary as RowSelectionModel;
        var l = dc.Person.First(p => p.Personnumber == sm.SelectedRow.RecordID);
        l.Visualfield = 2;
        dc.SubmitChanges();
        Ext.Msg.Alert("提示", "操作成功！").Show();
        GVLoad(hdnKindid.Value.ToString());
        ControlSet();
    }
}