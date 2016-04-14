using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Coolite.Ext.Web;
using GhtnTech.SEP.DAL;
using GhtnTech.SecurityFramework.BLL;

public partial class SQS_Assess : System.Web.UI.Page
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            //Ext.DoScript("refreshTree(#{tpkind});");
            tpkind.Root.Clear();
            Coolite.Ext.Web.TreeNode root = new Coolite.Ext.Web.TreeNode();
            root.Text = "-1";
            root.NodeID = "专业管理";
            tpkind.Root.Add(root);
            TreeBuild(root.Nodes, -1);
            dfBeginDate.SelectedDate = System.DateTime.Today.AddDays(-7);
            dfEndDate.SelectedDate = System.DateTime.Today;
        }
    }

    #region 构建刷新树

    private void TreeBuild(Coolite.Ext.Web.TreeNodeCollection nodes, decimal pid)
    {
        var kind = dc.SqsDomain.Where(p => p.Fid == pid && p.Usingdept == SessionBox.GetUserSession().DeptNumber);
        foreach (var r in kind)
        {
            Coolite.Ext.Web.TreeNode asyncNode = new Coolite.Ext.Web.TreeNode();
            asyncNode.Text = r.Dname + "(" + r.Fullscore.ToString() + ")";
            asyncNode.NodeID = r.Did.ToString();
            asyncNode.Qtip = "满分分值:" + r.Fullscore.ToString() + ",系数:" + r.Rate.ToString();

            nodes.Add(asyncNode);
            if (dc.SqsDomain.Where(p => p.Fid == r.Did).Count() > 0)
            {
                TreeBuild(asyncNode.Nodes, r.Did);
            }
            else
            {
                asyncNode.Listeners.Click.Handler = string.Format("Coolite.AjaxMethods.GVLoad({0});", r.Did.ToString());
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
        root.NodeID = "专业管理";
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
    public void GVLoad(decimal kindid)
    {
        hdnKindid.SetValue(kindid.ToString());
        btnSearch.Disabled = false;
        btnNew.Disabled = false;
        //
        LoadData(kindid);
    }

    private void LoadData(decimal id)
    {

        if (dfBeginDate.SelectedDate > dfEndDate.SelectedDate)
        {
            Ext.Msg.Alert("提示", "日期选择有误!").Show();
            return;
        }
        var data = from r in dc.SqsResult
                   from k in dc.SqsDomain
                   from d in dc.Department
                   from d2 in dc.Department
                   where r.Did == k.Did && r.Checkdept == d.Deptnumber && r.Checkfordept == d2.Deptnumber
                   && r.Did == id
                   && r.Checkdate >= dfBeginDate.SelectedDate && r.Checkdate <= dfEndDate.SelectedDate
                   select new
                   {
                       k.Dname,
                       r.Checkdate,
                       CheckDeptName = d.Deptname,
                       CheckForDeptName = d2.Deptname,
                       Placename=r.Placeid,
                       r.Rid,
                       r.Rstatus,
                       r.Total
                   };
        JeomStore.DataSource = data;
        JeomStore.DataBind();

    }

    protected void RowClick(object sender, AjaxEventArgs e)//流程处理
    {
        RowSelectionModel sm = gpEdit.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            btnEdit.Disabled = false;
            btnDel.Disabled = false;
        }
        else
        {
            btnEdit.Disabled = true;
            btnDel.Disabled = true;
        }
    }

    [AjaxMethod]
    public void btnNew_Click()
    {
        Ext.DoScript("#{Window1}.load('EditAccess_new.aspx?type=new&Pkindid=" + hdnKindid.Value.ToString() + "');#{Window1}.show();");
    }

    [AjaxMethod]
    public void btnEdit_Click()
    {
        RowSelectionModel sm = gpEdit.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            Ext.DoScript("#{Window1}.load('EditAccess_new.aspx?type=edit&Rid=" + sm.SelectedRow.RecordID + "');#{Window1}.show();");
        }
    }

    [AjaxMethod]
    public void btnDel_Click()
    {
        RowSelectionModel sm = gpEdit.SelectionModel.Primary as RowSelectionModel;
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
        var r = dc.SqsResult.First(p => p.Rid == id);
        var rj = dc.SqsResultDetail.Where(p => p.Rid == id);
        var ra = dc.SqsEssentialconditiondetail.Where(p => p.Rid == id);
        var rb = dc.SqsDemotiondetail.Where(p => p.Rid == id);
        dc.SqsResult.DeleteOnSubmit(r);
        dc.SqsResultDetail.DeleteAllOnSubmit(rj);
        dc.SqsEssentialconditiondetail.DeleteAllOnSubmit(ra);
        dc.SqsDemotiondetail.DeleteAllOnSubmit(rb);
        dc.SubmitChanges();
        Ext.Msg.Alert("提示", "删除成功!").Show();
        LoadData(int.Parse(hdnKindid.Value.ToString()));
    }

    [AjaxMethod]
    public void btnSearch_Click()
    {
        LoadData(int.Parse(hdnKindid.Value.ToString()));
    }
}
