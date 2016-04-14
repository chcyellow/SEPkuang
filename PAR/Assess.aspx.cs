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
using System.Xml.Xsl;

public partial class PAR_Assess : System.Web.UI.Page
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            Ext.DoScript("refreshTree(#{tpkind});");

            dfBeginDate.SelectedDate = System.DateTime.Today.AddDays(-7);
            dfEndDate.SelectedDate = System.DateTime.Today;
        }
    }

    #region 构建刷新树

    private void TreeBuild(Coolite.Ext.Web.TreeNodeCollection nodes, decimal pid)
    {
        var kind = dc.ParKind.Where(p => p.Fid == pid && p.Usingdept == SessionBox.GetUserSession().DeptNumber);
        foreach (var r in kind)
        {
            Coolite.Ext.Web.TreeNode asyncNode = new Coolite.Ext.Web.TreeNode();
            asyncNode.Text = r.Pkindname + "(" + r.Fullscore.ToString() + ")";
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

    [AjaxMethod]
    public void GVLoad(decimal kindid)
    {
        hdnKindid.SetValue(kindid.ToString());
        btnSearch.Disabled = true;
        btnNew.Disabled = false;
        //List<string> Role = SessionBox.GetUserSession().Role;
        //bool bl = false;
        //foreach (var r in Role)
        //{
        //    if (r.ToString().Split(',')[0] == "11")
        //    {
        //        bl = true;
        //        break;
        //    }
        //}
        //setnew(!bl);
        LoadData(kindid);
    }

    //private void setnew(bool bl)
    //{
    //    btnNew.Disabled = bl;
    //}
    [AjaxMethod]
    public void PYsearch(string py, string store)
    {
        if (py.Trim() == "")
        {
            return;
        }
        switch (store.Trim())
        {
            case "placeStore":
                var place = from pl in dc.Place
                            where pl.Maindeptid == SessionBox.GetUserSession().DeptNumber
                            && (dc.F_PINYIN(pl.Placename).ToLower().Contains(py.ToLower()) || pl.Placename.Contains(py.Trim()))
                            select new
                            {
                                placID = pl.Placeid,
                                placName = pl.Placename
                            };
                placeStore.DataSource = place;
                placeStore.DataBind();
                break;
            case "PCpersonStore":
                var q = from p in dc.Person
                        where p.Maindeptid == SessionBox.GetUserSession().DeptNumber
                        && (dc.F_PINYIN(p.Name).ToLower().Contains(py.ToLower()) || p.Name.Contains(py.Trim()))
                        select new
                        {
                            p.Personnumber,
                            p.Name
                        };
                PCpersonStore.DataSource = q.Distinct();
                PCpersonStore.DataBind();
                break;
        }
    }
    private void LoadData(decimal id)
    {
        
        if (dfBeginDate.SelectedDate > dfEndDate.SelectedDate)
        {
            Ext.Msg.Alert("提示", "日期选择有误!").Show();
            return;
        }
        var data = from r in dc.ParResult
                   from k in dc.ParKind
                   from d in dc.Department
                   from d2 in dc.Department
                   join e in dc.Person on r.Bkhperson equals e.Personnumber into gg
                   from g in gg.DefaultIfEmpty()
                   join f in dc.Person on r.Khperson equals f.Personnumber into kk
                   from kkk in kk.DefaultIfEmpty()
                   where r.Pkindid == k.Pkindid && r.Checkdept == d.Deptnumber && r.Checkfordept == d2.Deptnumber
                   && r.Pkindid == id
                   && r.Checkdate >= dfBeginDate.SelectedDate && r.Checkdate <= dfEndDate.SelectedDate
                   select new
                   {
                       k.Pkindname,
                       r.Checkdate,
                       CheckDeptName = d.Deptname,
                       CheckForDeptName = d2.Deptname,
                       r.Bkhperson,
                       r.Rid,
                       r.Rstatus,
                       r.Khperson,
                       r.Total,
                       r.Placeid,
                       Personname = g.Name != null ? g.Name : "无",
                       KHname = kkk.Name != null ? kkk.Name : "无"
                   };
        if (cbbPerson.SelectedIndex > -1)
        {
            data = data.Where(p => p.Khperson == cbbPerson.SelectedItem.Value);
        }
        if (cbbplace.SelectedIndex > -1)
        {
            data = data.Where(p => p.Placeid == cbbplace.SelectedItem.Value);
        }

        JeomStore.DataSource = data;
        JeomStore.DataBind();

    }

    protected void RowClick(object sender, AjaxEventArgs e)//流程处理
    {
        RowSelectionModel sm = gpEdit.SelectionModel.Primary as RowSelectionModel;
        
        if (sm.SelectedRows.Count > 0)
        {
            List<string> Role = SessionBox.GetUserSession().Role;
            bool bl = false;
            foreach (var r in Role)
            {
                if (r.ToString().Split(',')[0] == "11")
                {
                    bl = true;
                    break;
                }
            }
            if (bl)//矿级管理员可以编辑
            {
                btnEdit.Disabled = false;
                btnDel.Disabled = false;
            }
            else
            {
                btnEdit.Disabled = true;
                btnDel.Disabled = true;
            }
            btnView.Disabled = false;
        }
        else
        {
            btnEdit.Disabled = true;
            btnDel.Disabled = true;
            btnView.Disabled = true;
        }
    }

    [AjaxMethod]
    public void btnNew_Click()
    {
        Ext.DoScript("#{Window1}.load('EditAccess.aspx?type=new&Pkindid=" + hdnKindid.Value.ToString() + "');#{Window1}.show();");
    }

    [AjaxMethod]
    public void btnEdit_Click()
    {
        RowSelectionModel sm = gpEdit.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            Ext.DoScript("#{Window1}.load('EditAccess.aspx?type=edit&Rid=" + sm.SelectedRow.RecordID + "');#{Window1}.show();");
        }
    }

    [AjaxMethod]
    public void btnView_Click()
    {
        RowSelectionModel sm = gpEdit.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            Ext.DoScript("#{Window1}.load('EditAccess.aspx?type=view&Rid=" + sm.SelectedRow.RecordID + "');#{Window1}.show();");
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
        var r = dc.ParResult.First(p => p.Rid == id);
        var rj = dc.ParResultDetail.Where(p => p.Rid == id);
        var ra = dc.ParResultAdddetail.Where(p => p.Rid == id);
        dc.ParResult.DeleteOnSubmit(r);
        dc.ParResultDetail.DeleteAllOnSubmit(rj);
        dc.ParResultAdddetail.DeleteAllOnSubmit(ra);
        dc.SubmitChanges();
        Ext.Msg.Alert("提示", "删除成功!").Show();
        LoadData(int.Parse(hdnKindid.Value.ToString()));
    }

    [AjaxMethod]
    public void btnSearch_Click()
    {
        LoadData(int.Parse(hdnKindid.Value.ToString()));
    }

    protected void ToExcel(object sender, EventArgs e)//导出报表
    {
        string json = GridDataExcel.Value.ToString();
        foreach (var r in gpEdit.ColumnModel.Columns)
        {
            json = json.Replace("\"" + r.DataIndex.Trim() + "\"", "\"" + r.Header + "\"");
        }
        json = json.Replace("T00:00:00", "");
        json = json.Replace("Rid", "编号");
        //json = json.Replace("\"检查方式\":1", "\"检查方式\":\"自查\"");
        //json = json.Replace("\"处罚确认\":1", "\"处罚确认\":\"是\"");
        //json = json.Replace("\"处罚确认\":0", "\"处罚确认\":\"否\"");

        string strFileName = "风险预控考核报表";
        Response.Clear();
        Response.Buffer = true;
        Response.Charset = "GB2312";

        StoreSubmitDataEventArgs eSubmit = new StoreSubmitDataEventArgs(json, null);
        XmlNode xml = eSubmit.Xml;

        this.Response.Clear();
        this.Response.ContentType = "application nd.ms-excel";
        Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(strFileName) + ".xls");
        XslCompiledTransform xtExcel = new XslCompiledTransform();
        xtExcel.Load(Server.MapPath("ExcelAssess.xsl"));
        xtExcel.Transform(xml, null, this.Response.OutputStream);
        this.Response.End();
    }
}
