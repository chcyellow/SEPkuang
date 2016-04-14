using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Coolite.Ext.Web;
using GhtnTech.SEP.DAL;
using GhtnTech.SecurityFramework.BLL;

public partial class PAR_Kind : System.Web.UI.Page
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            Ext.DoScript("refreshTree(#{tpkind});");
        }
    }

    #region 分类相关代码

    #region 构建刷新树

    private void TreeBuild(Coolite.Ext.Web.TreeNodeCollection nodes, decimal pid)
    {
        var kind = dc.ParKind.Where(p => p.Fid == pid && p.Usingdept == SessionBox.GetUserSession().DeptNumber);
        foreach (var r in kind)
        {
            Coolite.Ext.Web.TreeNode asyncNode = new Coolite.Ext.Web.TreeNode();
            asyncNode.Text = r.Pkindname + "(" + r.Fullscore.ToString()+")";
            asyncNode.NodeID = r.Pkindid.ToString();
            asyncNode.Qtip = "分值:" + r.Fullscore.ToString();
            
            nodes.Add(asyncNode);
            if (dc.ParKind.Where(p => p.Fid == r.Pkindid).Count() > 0)
            {
                TreeBuild(asyncNode.Nodes, r.Pkindid);
            }
            else
            {
                asyncNode.Listeners.Click.Handler = string.Format("Coolite.AjaxMethods.GVLoad({0});", r.Pkindid.ToString());
                asyncNode.Leaf = true;
            }
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
        root.NodeID = "考核管理";
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

    #region 考核项目初始化

    [AjaxMethod]
    public void GVLoad(int id)
    {
        hdnKindid.SetValue(id.ToString());
        var item = from i in dc.ParItem
                   where i.Pkindid == id && i.Status == "1"
                   orderby i.Sort
                   select new
                   {
                       i.Fullscore,
                       i.Indate,
                       i.Itemid,
                       Itemname=i.Itemname+"("+i.Fullscore+")",
                       i.Pkindid,
                       i.Sort,
                       i.Status,
                   };
        ItemStore.DataSource = item;
        ItemStore.DataBind();
        ItemControlSet();

        JeomSet(-1);
    }

    private void ItemControlSet()
    {
        btnItemAdd.Disabled = false;
        btnItemEdit.Disabled = true;
        btnItemDel.Disabled = true;

        btnAddJoemNew.Disabled = false;
        btnJoemNew.Disabled = true;
    }

    #endregion

    #region 明细界面初始化
    [AjaxMethod]//win　显示
    public void readInfo(string id, string action)
    {
        if (id == "-1" && action != "new")
        {
            Ext.Msg.Alert("提示", "请在类别上右键！").Show();
            return;
        }
        switch (action)
        {
            case "add":
            case "new":
                Ext.DoScript("#{FormPanel1}.getForm().reset();");
                hdnTreeParentID.SetValue(id);
                hdnID.SetValue("");
                Window1.Show();
                break;
            case "edit":
                formset(id);
                break;
        }
    }

    private void formset(string id)
    {
        var data = dc.ParKind.Where(p => p.Pkindid == decimal.Parse(id));
        if (data.Count() > 0)
        {
            hdnID.SetValue(id);
            hdnTreeParentID.SetValue(data.First().Fid.ToString());
            tfPKINDNAME.Text = data.First().Pkindname;
            nfFULLSCORE.Text = data.First().Fullscore.ToString();
            cbbRATE.SelectedItem.Value = data.First().Rate;
            cbbCHECKWAY.SelectedItem.Value = data.First().Checkway;
            Window1.Show();
        }
        else
        {
            Ext.Msg.Alert("提示", "未查询到数据，请刷新后重试！").Show();
        }
    }
    #endregion

    #region 数据提交

    protected void btnUpdateClick(object sender, AjaxEventArgs e)
    {
        if (hdnID.Value.ToString().Trim() == "")
        {
            ParKind pk = new ParKind
            {
                Fid = decimal.Parse(hdnTreeParentID.Value.ToString()),
                Pkindname = tfPKINDNAME.Text,
                Fullscore = decimal.Parse(nfFULLSCORE.Text),
                Rate = cbbRATE.SelectedItem.Value,
                Checkway = cbbCHECKWAY.SelectedItem.Value,
                Indate = System.DateTime.Today,
                Status = "1",
                Usingdept = SessionBox.GetUserSession().DeptNumber
            };
            dc.ParKind.InsertOnSubmit(pk);
            dc.SubmitChanges();
            Ext.Msg.Alert("提示", "新增成功！").Show();
        }
        else
        {
            var pk = dc.ParKind.First(p => p.Pkindid == decimal.Parse(hdnID.Value.ToString()));
            pk.Pkindname = tfPKINDNAME.Text;
            pk.Fullscore = decimal.Parse(nfFULLSCORE.Text);
            pk.Rate = cbbRATE.SelectedItem.Value;
            pk.Checkway = cbbCHECKWAY.SelectedItem.Value;
            dc.SubmitChanges();
            Ext.Msg.Alert("提示", "修改成功！").Show();
        }
        Ext.DoScript("refreshTree(#{tpkind});");
    }
    #endregion

    #region 删除操作
    [AjaxMethod]//确定、删除
    public void del(string id, string action)
    {
        if (id == "-1")
        {
            Ext.Msg.Alert("提示", "请在类别上右键！").Show();
            return;
        }
        switch (action)
        {
            case "del":
                Ext.Msg.Confirm("提示", "是否确定删除?", new MessageBox.ButtonsConfig
                {
                    Yes = new MessageBox.ButtonConfig
                    {
                        Handler = "Coolite.AjaxMethods.TreeNote_Del(" + id + ");",
                        Text = "确 定"
                    },
                    No = new MessageBox.ButtonConfig
                    {
                        Text = "取 消"
                    }
                }).Show();
                break;
        }
    }

    [AjaxMethod]
    public void TreeNote_Del(string id)
    {
        var pk = dc.ParKind.First(p => p.Pkindid == decimal.Parse(id));
        dc.ParKind.DeleteOnSubmit(pk);
        dc.SubmitChanges();
        Ext.Msg.Alert("提示", "删除成功!").Show();
        Ext.DoScript("refreshTree(#{tpkind});");

    }
    #endregion

    #endregion

    #region 考核项目相关代码

    #region 控件设置
    protected void ItemRowClick(object sender, AjaxEventArgs e)//单击行事件
    {
        RowSelectionModel sm = gpItem.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            btnItemEdit.Disabled = false;
            JeomSet(int.Parse(sm.SelectedRow.RecordID));
            if (dc.ParJeomcriterion.Where(p => p.Itemid == decimal.Parse(sm.SelectedRow.RecordID)).Count() > 0)
            {
                btnItemDel.Disabled = true;
            }
            else
            {
                btnItemDel.Disabled = false;
            }
            btnJoemNew.Disabled = false;
        }
        else
        {
            btnItemEdit.Disabled = true;
            btnItemDel.Disabled = true;

            btnJoemNew.Disabled = true;
        }
    }
    #endregion

    #region 考核标准初始化
    private void JeomSet(int id)
    {
        if (id > -1)
        {
            var data = (from j in dc.ParJeomcriterion
                        where j.Itemid == id && j.Status == "1"
                        orderby j.Sort
                        select new
                        {
                            Jcid = "j" + j.Jcid,
                            j.Jccontent,
                            j.Indate,
                            j.Maxscore,
                            j.Minscore,
                            Kind = "基本内容"
                        }).Concat(
                       from a in dc.ParAddjeomcriterion
                       where a.Pkindid == decimal.Parse(hdnKindid.Value.ToString()) && a.Status == "1"
                       orderby a.Sort
                       select new
                       {
                           Jcid = "a" + a.Ajcid,
                           Jccontent = a.Ajccontent,
                           a.Indate,
                           a.Maxscore,
                           a.Minscore,
                           Kind = "附加内容"
                       }
                           );
            JeomStore.DataSource = data;
            JeomStore.DataBind();
        }
        else
        {
            var data = from a in dc.ParAddjeomcriterion
                       where a.Pkindid == decimal.Parse(hdnKindid.Value.ToString()) && a.Status == "1"
                       orderby a.Sort
                       select new
                       {
                           Jcid = "a" + a.Ajcid,
                           Jccontent = a.Ajccontent,
                           a.Indate,
                           a.Maxscore,
                           a.Minscore,
                           Kind = "附加内容"
                       };
            JeomStore.DataSource = data;
            JeomStore.DataBind();
        }
    }
    #endregion

    #region 明细界面初始化
    [AjaxMethod]//win　显示
    public void ItemInfo(string action)
    {
        if(action=="new")
        {
            Ext.DoScript("#{fpItem}.getForm().reset();");
            hdnItemid.SetValue("");
        }
        if (action == "edit")
        {
            RowSelectionModel sm = gpItem.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count > 0)
            {
                hdnItemid.SetValue(sm.SelectedRow.RecordID);
                var item = dc.ParItem.First(p => p.Itemid == decimal.Parse(sm.SelectedRow.RecordID));
                tfItemname.Text = item.Itemname;
                nfitemFullscore.Text = item.Fullscore.ToString();
            }
        }
        ItemWindow.Show();
    }

    #endregion

    #region 数据提交

    protected void btnItemUpdateClick(object sender, AjaxEventArgs e)
    {
        var item = dc.ParItem.Where(p => p.Pkindid == decimal.Parse(hdnKindid.Value.ToString()));
        var kind = dc.ParKind.First(p => p.Pkindid == decimal.Parse(hdnKindid.Value.ToString()));
        
        if (hdnItemid.Value.ToString().Trim() == "")
        {
            if (kind.Fullscore.Value < item.Sum(p => p.Fullscore).Value + decimal.Parse(nfitemFullscore.Text))
            {
                Ext.Msg.Alert("提示", "分值越界！").Show();
                return;
            }
            ParItem pk = new ParItem
            {
                Itemname = tfItemname.Text,
                Fullscore = decimal.Parse(nfitemFullscore.Text),
                Indate = System.DateTime.Today,
                Pkindid = decimal.Parse(hdnKindid.Value.ToString()),
                Sort = item.Max(p => p.Sort).Value + 1,
                Status = "1"
            };
            dc.ParItem.InsertOnSubmit(pk);
            dc.SubmitChanges();
            Ext.Msg.Alert("提示", "新增成功！").Show();
        }
        else
        {
            var pk = dc.ParItem.First(p => p.Itemid == decimal.Parse(hdnKindid.Value.ToString()));
            if (kind.Fullscore.Value < item.Sum(p => p.Fullscore).Value + decimal.Parse(nfitemFullscore.Text) - pk.Fullscore.Value)
            {
                Ext.Msg.Alert("提示", "分值越界！").Show();
                return;
            }
            pk.Itemname = tfItemname.Text;
            pk.Fullscore = decimal.Parse(nfitemFullscore.Text);
            dc.SubmitChanges();
            Ext.Msg.Alert("提示", "修改成功！").Show();
        }
        GVLoad(int.Parse(hdnKindid.Value.ToString()));
    }
    #endregion

    #region 删除操作
    [AjaxMethod]//确定、删除
    public void Itemdel()
    {
        RowSelectionModel sm = gpItem.SelectionModel.Primary as RowSelectionModel;
        Ext.Msg.Confirm("提示", "是否确定删除选中项目?", new MessageBox.ButtonsConfig
        {
            Yes = new MessageBox.ButtonConfig
            {
                Handler = "Coolite.AjaxMethods.Item_Del(" + sm.SelectedRow.RecordID + ");",
                Text = "确 定"
            },
            No = new MessageBox.ButtonConfig
            {
                Text = "取 消"
            }
        }).Show();
    }

    [AjaxMethod]
    public void Item_Del(int id)
    {
        var pk = dc.ParItem.First(p => p.Itemid == id);
        dc.ParItem.DeleteOnSubmit(pk);
        dc.SubmitChanges();
        Ext.Msg.Alert("提示", "删除成功!").Show();
        GVLoad(int.Parse(hdnKindid.Value.ToString()));
    }
    #endregion

    #endregion

    #region 考核标准相关代码

    #region 控件设置

    protected void JoemRowClick(object sender, AjaxEventArgs e)//单击行事件
    {
        RowSelectionModel sm = gpJoem.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            btnJoemUpdate.Disabled = false;
            btnJoemDel.Disabled = false;
        }
        else
        {
            btnJoemUpdate.Disabled = true;
            btnJoemDel.Disabled = true;
        }
    }
    #endregion

    #region 明细界面初始化
    [AjaxMethod]//win　显示
    public void JoemInfo(string action)
    {
        if (action == "new")
        {
            Ext.DoScript("#{fpJoem}.getForm().reset();");
            hdnJoemid.SetValue("-1");
        }
        if (action == "addnew")
        {
            Ext.DoScript("#{fpJoem}.getForm().reset();");
            hdnJoemid.SetValue("-2");
        }
        if (action == "edit")
        {
            RowSelectionModel sm = gpJoem.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count > 0)
            {
                hdnJoemid.SetValue(sm.SelectedRow.RecordID);
                if (sm.SelectedRow.RecordID.Trim().Substring(0, 1) == "j")
                {
                    var joem = dc.ParJeomcriterion.First(p => p.Jcid == decimal.Parse(sm.SelectedRow.RecordID.Trim().Substring(1)));
                    tfJccontent.Text = joem.Jccontent;
                    nfJoemMinscore.Text = joem.Minscore.ToString();
                    nfJoemMaxscore.Text = joem.Maxscore.ToString();
                }
                else
                {
                    var joem = dc.ParAddjeomcriterion.First(p => p.Ajcid == decimal.Parse(sm.SelectedRow.RecordID.Trim().Substring(1)));
                    tfJccontent.Text = joem.Ajccontent;
                    nfJoemMinscore.Text = joem.Minscore.ToString();
                    nfJoemMaxscore.Text = joem.Maxscore.ToString();
                }
            }
        }
        JoemWindow.Show();
    }

    #endregion

    #region 数据提交

    protected void btnJoemUpdateClick(object sender, AjaxEventArgs e)
    {
        RowSelectionModel smItem = gpItem.SelectionModel.Primary as RowSelectionModel;
        switch(hdnJoemid.Value.ToString().Trim())
        {
            case "-1":
                ParJeomcriterion pj=new ParJeomcriterion
                {
                    Jccontent=tfJccontent.Text,
                    Maxscore=decimal.Parse( nfJoemMaxscore.Text),
                    Minscore=decimal.Parse(nfJoemMinscore.Text),
                    Itemid=decimal.Parse(smItem.SelectedRow.RecordID),
                    Sort=dc.ParJeomcriterion.Where(p=>p.Itemid==decimal.Parse(smItem.SelectedRow.RecordID)).Max(p=>p.Sort)+1,
                    Status="1",
                    Indate=System.DateTime.Today
                };
                dc.ParJeomcriterion.InsertOnSubmit(pj);
                dc.SubmitChanges();
                Ext.Msg.Alert("提示", "新增成功！").Show();
                break;
            case "-2":
                ParAddjeomcriterion pa=new ParAddjeomcriterion
                {
                    Ajccontent=tfJccontent.Text,
                    Maxscore=decimal.Parse( nfJoemMaxscore.Text),
                    Minscore=decimal.Parse(nfJoemMinscore.Text),
                    Pkindid=decimal.Parse(hdnKindid.Value.ToString()),
                    Sort=dc.ParAddjeomcriterion.Where(p=>p.Pkindid==decimal.Parse(hdnKindid.Value.ToString())).Max(p=>p.Sort)+1,
                    Status="1",
                    Indate=System.DateTime.Today
                };
                dc.ParAddjeomcriterion.InsertOnSubmit(pa);
                dc.SubmitChanges();
                Ext.Msg.Alert("提示", "新增成功！").Show();
                break;
            default:
                if(hdnJoemid.Value.ToString().Trim().Substring(0,1)=="j")
                {
                    var pja=dc.ParJeomcriterion.First(p=>p.Jcid==decimal.Parse(hdnJoemid.Value.ToString().Trim().Substring(1)));
                    pja.Jccontent=tfJccontent.Text;
                    pja.Minscore=decimal.Parse(nfJoemMinscore.Text);
                    pja.Maxscore=decimal.Parse( nfJoemMaxscore.Text);
                    dc.SubmitChanges();
                    Ext.Msg.Alert("提示", "修改成功！").Show();
                }
                else
                {
                    var paa=dc.ParAddjeomcriterion.First(p=>p.Ajcid==decimal.Parse(hdnJoemid.Value.ToString().Trim().Substring(1)));
                    paa.Ajccontent=tfJccontent.Text;
                    paa.Minscore=decimal.Parse(nfJoemMinscore.Text);
                    paa.Maxscore=decimal.Parse( nfJoemMaxscore.Text);
                    dc.SubmitChanges();
                    Ext.Msg.Alert("提示", "修改成功！").Show();
                }
                break;

        }
        if (smItem.SelectedRows.Count > 0)
        {
            JeomSet(int.Parse(smItem.SelectedRow.RecordID));
        }
        else
        {
            JeomSet(-1);
        }
    }
    #endregion

    #region 删除操作
    [AjaxMethod]//确定、删除
    public void Joemdel()
    {
        RowSelectionModel sm = gpJoem.SelectionModel.Primary as RowSelectionModel;
        Ext.Msg.Confirm("提示", "是否确定删除选中项目?", new MessageBox.ButtonsConfig
        {
            Yes = new MessageBox.ButtonConfig
            {
                Handler = "Coolite.AjaxMethods.Joem_Del('" + sm.SelectedRow.RecordID + "');",
                Text = "确 定"
            },
            No = new MessageBox.ButtonConfig
            {
                Text = "取 消"
            }
        }).Show();
    }

    [AjaxMethod]
    public void Joem_Del(string id)
    {
        if (id.Substring(0, 1) == "j")
        {
            var pja = dc.ParJeomcriterion.First(p => p.Jcid == decimal.Parse(id.Trim().Substring(1)));
            dc.ParJeomcriterion.DeleteOnSubmit(pja);
            dc.SubmitChanges();
        }
        else
        {
            var paa = dc.ParAddjeomcriterion.First(p => p.Ajcid == decimal.Parse(id.ToString().Trim().Substring(1)));
            dc.ParAddjeomcriterion.DeleteOnSubmit(paa);
            dc.SubmitChanges();
        }
        Ext.Msg.Alert("提示", "删除成功!").Show();
        RowSelectionModel smItem = gpItem.SelectionModel.Primary as RowSelectionModel;
        JeomSet(int.Parse(smItem.SelectedRow.RecordID));
    }
    #endregion

    #endregion

}
