using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Coolite.Ext.Web;
using GhtnTech.SEP.DAL;
using GhtnTech.SecurityFramework.BLL;

public partial class SQS_Domain : System.Web.UI.Page
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            BuildTree();
        }
    }

    #region 专业代码

    #region 树代码

    private void BuildTree()
    {
        tpkind.Root.Clear();
        Coolite.Ext.Web.TreeNode root = new Coolite.Ext.Web.TreeNode();
        root.Text = "-1";
        root.NodeID = "专业管理";
        tpkind.Root.Add(root);
        var dm = dc.SqsDomain.Where(p => p.Fid == -1 && p.Usingdept == SessionBox.GetUserSession().DeptNumber);
        foreach (var r in dm)
        {
            //Coolite.Ext.Web.TreeNode asyncNode = new Coolite.Ext.Web.TreeNode();
            AsyncTreeNode asyncNode = new AsyncTreeNode();
            asyncNode.Text = r.Dname;
            asyncNode.NodeID = r.Did.ToString();
            asyncNode.Qtip = "满分分值:" + r.Fullscore.ToString();
            root.Nodes.Add(asyncNode);
        }
    }

    [AjaxMethod]
    public string NodeLoad(string nodeID)
    {
        Coolite.Ext.Web.TreeNodeCollection nodes = new Coolite.Ext.Web.TreeNodeCollection();
        var dm = dc.SqsDomain.Where(p => p.Fid == decimal.Parse(nodeID));
        foreach (var r in dm)
        {
            //Coolite.Ext.Web.TreeNode asyncNode = new Coolite.Ext.Web.TreeNode();
            AsyncTreeNode asyncNode = new AsyncTreeNode();
            asyncNode.Text = r.Dname;
            asyncNode.NodeID = r.Did.ToString();
            asyncNode.Qtip = "满分分值:" + r.Fullscore.ToString()+",系数:"+r.Rate.ToString();
            if (dc.SqsDomain.Count(p => p.Fid == r.Did) == 0)
            {
                asyncNode.Listeners.Click.Handler = string.Format("Coolite.AjaxMethods.GVLoad({0});", r.Did.ToString());
                asyncNode.Leaf = true;
            }
            nodes.Add(asyncNode);
        }
        return nodes.ToJson();
    }

    #endregion

    #region 考核项目初始化

    [AjaxMethod]
    public void GVLoad(int id)
    {
        hdnKindid.SetValue(id.ToString());
        var item = from i in dc.SqsKind
                   where i.Did == id && i.Status == "1"
                   orderby i.Pkindid
                   select new
                   {
                       i.Fullscore,
                       i.Indate,
                       i.Pkindid,
                       Pkindname = i.Pkindname + "(满分：" + i.Fullscore + ",系数："+i.Rate+")",
                       i.Status,
                       i.Rate
                   };
        ItemStore.DataSource = item;
        ItemStore.DataBind();
        ItemControlSet();

        JeomSet(-1,id);
    }

    private void ItemControlSet()
    {
        btnItemAdd.Disabled = false;
        btnItemEdit.Disabled = true;
        btnItemDel.Disabled = true;

        btnAddJNew.Disabled = true;
        btnAddNNew.Disabled = false;
        banAddDNew.Disabled = false;
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
                hdnID.SetValue("");
                hdnTreeParentID.SetValue(id);
                Window1.Show();
                break;
            case "edit":
                formset(id);
                break;
        }
    }

    private void formset(string id)
    {
        var data = dc.SqsDomain.Where(p => p.Did == decimal.Parse(id));
        if (data.Count() > 0)
        {
            hdnID.SetValue(id);
            hdnTreeParentID.SetValue(data.First().Fid.ToString());
            tfDNAME.Text = data.First().Dname;
            nfFULLSCORE.Text = data.First().Fullscore.ToString();
            nfRATE.Text = data.First().Rate.ToString();
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
            SqsDomain pk = new SqsDomain
            {
                Fid = decimal.Parse(hdnTreeParentID.Value.ToString()),
                Dname = tfDNAME.Text,
                Fullscore = decimal.Parse(nfFULLSCORE.Text),
                Rate =decimal.Parse( nfRATE.Text),
                Indate = System.DateTime.Today,
                Status = "1",
                Usingdept = SessionBox.GetUserSession().DeptNumber
            };
            dc.SqsDomain.InsertOnSubmit(pk);
            dc.SubmitChanges();
            Ext.Msg.Alert("提示", "新增成功！").Show();
        }
        else
        {
            var pk = dc.SqsDomain.First(p => p.Did == decimal.Parse(hdnID.Value.ToString()));
            pk.Dname = tfDNAME.Text;
            pk.Fullscore = decimal.Parse(nfFULLSCORE.Text);
            pk.Rate = decimal.Parse( nfRATE.Text);
            dc.SubmitChanges();
            Ext.Msg.Alert("提示", "修改成功！").Show();
        }
        //Ext.DoScript("refreshTree(#{tpkind});");
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
        var pk = dc.SqsDomain.First(p => p.Did == decimal.Parse(id));
        dc.SqsDomain.DeleteOnSubmit(pk);
        dc.SubmitChanges();
        Ext.Msg.Alert("提示", "删除成功!").Show();
        //Ext.DoScript("refreshTree(#{tpkind});");

    }
    #endregion

    #endregion

    #region 考核项目分类相关代码

    #region 控件设置
    protected void ItemRowClick(object sender, AjaxEventArgs e)//单击行事件
    {
        RowSelectionModel sm = gpItem.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            btnItemEdit.Disabled = false;
            JeomSet(int.Parse(sm.SelectedRow.RecordID),-1);
            if (dc.ParJeomcriterion.Where(p => p.Itemid == decimal.Parse(sm.SelectedRow.RecordID)).Count() > 0)
            {
                btnItemDel.Disabled = true;
            }
            else
            {
                btnItemDel.Disabled = false;
            }
            btnAddJNew.Disabled = false;
        }
        else
        {
            btnItemEdit.Disabled = true;
            btnItemDel.Disabled = true;

            btnAddJNew.Disabled = true;
        }
    }
    #endregion

    #region 考核标准初始化
    private void JeomSet(int id ,int did)
    {
        if (id > -1)
        {
            var data = from j in dc.SqsJeomcriterion
                        where j.Pkindid == id && j.Status == "1"
                        orderby j.Sort
                        select new
                        {
                            Jcid = "j" + j.Jcid,
                            j.Jccontent,
                            j.Indate,
                            Score=" " +j.Score,
                            j.Means,
                            Kind = "基本内容"
                        };
                       // .Concat(
                       //from a in dc.SqsEssentialcondition
                       //where a.Did == decimal.Parse(hdnKindid.Value.ToString()) && a.Status == "1"
                       //select new
                       //{
                       //    Jcid = "n" + a.Ecid,
                       //    Jccontent = a.Content,
                       //    a.Indate,
                       //    Score="不达标",
                       //    Means = "不达标扣10分，得分不得超过不达标标准分",
                       //    Kind = "否决条件"
                       //}
                       //    ).Concat(
                       //from a in dc.SqsDemotion
                       //where a.Did == decimal.Parse(hdnKindid.Value.ToString()) && a.Status == "1"
                       //select new
                       //{
                       //    Jcid = "d" + a.Deid,
                       //    Jccontent = a.Content,
                       //    a.Indate,
                       //    Score = "降一级",
                       //    Means = "降一级扣5分，得分不得超过下一级的最高分",
                       //    Kind = "降级条件"
                       //}
                       //    );
            JeomStore.DataSource = data;
            JeomStore.DataBind();
        }
        else
        {
            var data = (
                       from a in dc.SqsEssentialcondition
                       where a.Did == did && a.Status == "1"
                       select new
                       {
                           Jcid = "n" + a.Ecid,
                           Jccontent = a.Content,
                           a.Indate,
                           Score = "不达标",
                           Means = "不达标扣10分，得分不得超过不达标标准分",
                           Kind = "否决条件"
                       }
                           ).Concat(
                       from a in dc.SqsDemotion
                       where a.Did == did && a.Status == "1"
                       select new
                       {
                           Jcid = "d" + a.Deid,
                           Jccontent = a.Content,
                           a.Indate,
                           Score = "降一级",
                           Means = "降一级扣5分，得分不得超过下一级的最高分",
                           Kind = "降级条件"
                       }
                           );
            JeomStore.DataSource = data;
            JeomStore.DataBind();
        }
    }
    #endregion

    #region 明细界面初始化
    [AjaxMethod]//win　显示
    public void ItemInfo(string action)
    {
        if (action == "new")
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
                var item = dc.SqsKind.First(p => p.Pkindid == decimal.Parse(sm.SelectedRow.RecordID));
                tfPKINDNAME.Text = item.Pkindname;
                nfitemFullscore.Text = item.Fullscore.ToString();
                nfitemRATE.Text = item.Rate.ToString();
            }
        }
        ItemWindow.Show();
    }

    #endregion

    #region 数据提交

    protected void btnItemUpdateClick(object sender, AjaxEventArgs e)
    {
        var item = dc.SqsKind.Where(p => p.Did == decimal.Parse(hdnKindid.Value.ToString()));
        var kind = dc.SqsDomain.First(p => p.Did == decimal.Parse(hdnKindid.Value.ToString()));

        if (hdnItemid.Value.ToString().Trim() == "")
        {
            if (item.Sum(p => p.Rate * p.Fullscore).Value + decimal.Parse(nfitemRATE.Text) * decimal.Parse(nfitemFullscore.Text) > kind.Fullscore.Value)
            {
                Ext.Msg.Alert("提示", "分值越界！").Show();
                return;
            }
            SqsKind pk = new SqsKind
            {
                Pkindname = tfPKINDNAME.Text,
                Fullscore = decimal.Parse(nfitemFullscore.Text),
                Indate = System.DateTime.Today,
                Did = decimal.Parse(hdnKindid.Value.ToString()),
                Rate=decimal.Parse(nfitemRATE.Text),
                Status = "1",
            };
            dc.SqsKind.InsertOnSubmit(pk);
            dc.SubmitChanges();
            Ext.Msg.Alert("提示", "新增成功！").Show();
        }
        else
        {
            RowSelectionModel sm = gpItem.SelectionModel.Primary as RowSelectionModel;
            var pk = dc.SqsKind.First(p => p.Pkindid == decimal.Parse(sm.SelectedRow.RecordID));
            if (item.Sum(p => p.Rate * p.Fullscore).Value + decimal.Parse(nfitemRATE.Text) * decimal.Parse(nfitemFullscore.Text) - pk.Rate.Value * pk.Fullscore.Value > kind.Fullscore.Value)
            {
                Ext.Msg.Alert("提示", "分值越界！").Show();
                return;
            }
            pk.Pkindname = tfPKINDNAME.Text;
            pk.Fullscore = decimal.Parse(nfitemFullscore.Text);
            pk.Rate = decimal.Parse(nfitemRATE.Text);
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
        var pk = dc.SqsKind.First(p => p.Pkindid == id);
        dc.SqsKind.DeleteOnSubmit(pk);
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
        if (action == "Jnew")
        {
            Ext.DoScript("#{fpJoem}.getForm().reset();");
            hdnJoemid.SetValue("-1");
            nfScore.Disabled = false;
            taMEANS.Disabled = false;
        }
        if (action == "Nnew")
        {
            Ext.DoScript("#{fpJoem}.getForm().reset();");
            hdnJoemid.SetValue("-2");
            nfScore.Disabled = true;
            taMEANS.Disabled = true;
            taMEANS.Text = "不达标扣10分，得分不得超过不达标标准分";
        }
        if (action == "Dnew")
        {
            Ext.DoScript("#{fpJoem}.getForm().reset();");
            hdnJoemid.SetValue("-3");
            nfScore.Disabled = true;
            taMEANS.Disabled = true;
            taMEANS.Text = "降一级扣5分，得分不得超过下一级的最高分";
        }
        if (action == "edit")
        {
            RowSelectionModel sm = gpJoem.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count > 0)
            {
                hdnJoemid.SetValue(sm.SelectedRow.RecordID);
                switch (sm.SelectedRow.RecordID.Trim().Substring(0, 1))
                {
                    case "j":
                        var joem = dc.SqsJeomcriterion.First(p => p.Jcid == decimal.Parse(sm.SelectedRow.RecordID.Trim().Substring(1)));
                        tfJccontent.Text = joem.Jccontent;
                        nfScore.Text = joem.Score.ToString();
                        taMEANS.Text = joem.Means;
                        break;
                    case "n":
                        var ec = dc.SqsEssentialcondition.First(p => p.Ecid == decimal.Parse(sm.SelectedRow.RecordID.Trim().Substring(1)));
                        tfJccontent.Text = ec.Content;
                        nfScore.Disabled = true;
                        taMEANS.Disabled = true;
                        taMEANS.Text = "不达标扣10分，得分不得超过不达标标准分";
                        break;
                    case "d":
                        var de = dc.SqsDemotion.First(p => p.Deid == decimal.Parse(sm.SelectedRow.RecordID.Trim().Substring(1)));
                        tfJccontent.Text = de.Content;
                        nfScore.Disabled = true;
                        taMEANS.Disabled = true;
                        taMEANS.Text = "降一级扣5分，得分不得超过下一级的最高分";
                        break;
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
        switch (hdnJoemid.Value.ToString().Trim())
        {
            case "-1":
                SqsJeomcriterion pj = new SqsJeomcriterion
                {
                    Jccontent = tfJccontent.Text,
                    Score=decimal.Parse(nfScore.Text),
                    Means=taMEANS.Text,
                    Pkindid = decimal.Parse(smItem.SelectedRow.RecordID),
                    Sort = dc.ParJeomcriterion.Where(p => p.Itemid == decimal.Parse(smItem.SelectedRow.RecordID)).Max(p => p.Sort) + 1,
                    Status = "1",
                    Indate = System.DateTime.Today
                };
                dc.SqsJeomcriterion.InsertOnSubmit(pj);
                dc.SubmitChanges();
                Ext.Msg.Alert("提示", "新增成功！").Show();
                break;
            case "-2":
                SqsEssentialcondition pa = new SqsEssentialcondition
                {
                    Content = tfJccontent.Text,
                    Did = decimal.Parse(hdnKindid.Value.ToString()),
                    Status = "1",
                    Indate = System.DateTime.Today
                };
                dc.SqsEssentialcondition.InsertOnSubmit(pa);
                dc.SubmitChanges();
                Ext.Msg.Alert("提示", "新增成功！").Show();
                break;
            case "-3":
                SqsDemotion pb = new SqsDemotion
                {
                    Content = tfJccontent.Text,
                    Did = decimal.Parse(hdnKindid.Value.ToString()),
                    Status = "1",
                    Indate = System.DateTime.Today
                };
                dc.SqsDemotion.InsertOnSubmit(pb);
                dc.SubmitChanges();
                Ext.Msg.Alert("提示", "新增成功！").Show();
                break;
            default:
                if (hdnJoemid.Value.ToString().Trim().Substring(0, 1) == "j")
                {
                    var pja = dc.SqsJeomcriterion.First(p => p.Jcid == decimal.Parse(hdnJoemid.Value.ToString().Trim().Substring(1)));
                    pja.Jccontent = tfJccontent.Text;
                    pja.Score = decimal.Parse(nfScore.Text);
                    pja.Means = taMEANS.Text;
                    dc.SubmitChanges();
                    Ext.Msg.Alert("提示", "修改成功！").Show();
                }
                else if (hdnJoemid.Value.ToString().Trim().Substring(0, 1) == "n")
                {
                    var paa = dc.SqsEssentialcondition.First(p => p.Ecid == decimal.Parse(hdnJoemid.Value.ToString().Trim().Substring(1)));
                    paa.Content = tfJccontent.Text;
                    dc.SubmitChanges();
                    Ext.Msg.Alert("提示", "修改成功！").Show();
                }
                else
                {
                    var pba = dc.SqsDemotion.First(p => p.Deid == decimal.Parse(hdnJoemid.Value.ToString().Trim().Substring(1)));
                    pba.Content = tfJccontent.Text;
                    dc.SubmitChanges();
                    Ext.Msg.Alert("提示", "修改成功！").Show();
                }
                break;

        }
        if (smItem.SelectedRows.Count > 0)
        {
            JeomSet(int.Parse(smItem.SelectedRow.RecordID),-1);
        }
        else
        {
            JeomSet(-1, int.Parse(hdnKindid.Value.ToString()));
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
            var pja = dc.SqsJeomcriterion.First(p => p.Jcid == decimal.Parse(id.Trim().Substring(1)));
            dc.SqsJeomcriterion.DeleteOnSubmit(pja);
            dc.SubmitChanges();
        }
        else if (id.Substring(0, 1) == "n")
        {
            var paa = dc.SqsEssentialcondition.First(p => p.Ecid == decimal.Parse(id.ToString().Trim().Substring(1)));
            dc.SqsEssentialcondition.DeleteOnSubmit(paa);
            dc.SubmitChanges();
        }
        else if (id.Substring(0, 1) == "d")
        {
            var paa = dc.SqsDemotion.First(p => p.Deid == decimal.Parse(id.ToString().Trim().Substring(1)));
            dc.SqsDemotion.DeleteOnSubmit(paa);
            dc.SubmitChanges();
        }
        Ext.Msg.Alert("提示", "删除成功!").Show();
        RowSelectionModel smItem = gpItem.SelectionModel.Primary as RowSelectionModel;
        JeomSet(int.Parse(smItem.SelectedRow.RecordID),-1);
    }
    #endregion

    #endregion
}
