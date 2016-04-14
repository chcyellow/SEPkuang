using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using GhtnTech.SEP.DBUtility;

using Coolite.Ext.Web;
using GhtnTech.SEP.DAL;
using GhtnTech.SecurityFramework.BLL;

public partial class PAR_PAR_SZKS : System.Web.UI.Page
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        BuildTree();
    }

    #region 绑定树

    private void BuildTree()
    {
        tpZY.Root.Clear();
        Coolite.Ext.Web.TreeNode root = new Coolite.Ext.Web.TreeNode();
        root.Text = "专业";
        root.NodeID = "-1";
        tpZY.Root.Add(root);
        StringBuilder strSql = new StringBuilder();
        strSql.Append(string.Format("select * from CS_BASEINFOSET where FID={0} order by INFOID", PublicMethod.ReadXmlReturnNode("SZKSZY", this)));

        DataTable dt = OracleHelper.Query(strSql.ToString()).Tables[0];
        foreach (DataRow r in dt.Rows)
        {
            //Coolite.Ext.Web.TreeNode asyncNode = new Coolite.Ext.Web.TreeNode();
            AsyncTreeNode asyncNode = new AsyncTreeNode();
            asyncNode.Text = r["INFONAME"].ToString();
            asyncNode.NodeID = "z" + r["INFOID"].ToString();
            //AddGZRW_Temp(asyncNode.Nodes, r["INFOID"].ToString());
            //AddGZRW(asyncNode.Nodes, r["INFOID"].ToString());
            root.Nodes.Add(asyncNode);
        }
    }

    [AjaxMethod]
    public string NodeLoad(string nodeID)
    {
        Coolite.Ext.Web.TreeNodeCollection nodes = new Coolite.Ext.Web.TreeNodeCollection();
        switch (nodeID.Substring(0, 1))
        {
            case "z":
                AddPost(nodes, nodeID.Substring(1));
                break;
        }

        string a = nodes.ToJson();

        return nodes.ToJson();
    }

    private void AddPost(Coolite.Ext.Web.TreeNodeCollection nodes, string p)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append(string.Format("select * from post where zyid={0}", p));

        DataTable dt = OracleHelper.Query(strSql.ToString()).Tables[0];
        foreach (DataRow r in dt.Rows)
        {
            //AsyncTreeNode asyncNode = new AsyncTreeNode();
            //asyncNode.Text = r["WORKTASK"].ToString();
            //asyncNode.NodeID = "w" + r["WORKTASKID"].ToString();
            //nodes.Add(asyncNode);
            Coolite.Ext.Web.TreeNode asyncNode = new Coolite.Ext.Web.TreeNode();
            asyncNode.Text = r["POSTNAME"].ToString();
            asyncNode.NodeID = "w" + r["POSTID"].ToString();
            asyncNode.Listeners.Click.Handler = string.Format("Coolite.AjaxMethods.GVLoad({0});", r["POSTID"].ToString().Trim());
            asyncNode.Leaf = true;
            nodes.Add(asyncNode);
        }
    }

    #endregion

    [AjaxMethod]
    public void GVLoad(decimal id)
    {
        var data = dc.Szksknowledge.Where(p => p.Postid == id && p.Maindept == SessionBox.GetUserSession().DeptNumber);
        PanelEditor.CompleteEdit();

        if (data.Count()>0)
        {
            Panel1.Html = data.First().Kcontent;
        }
        else
        {
            Panel1.Html = "";
        }
        btnEdit.Disabled = false;
        hdnPostid.Value = id.ToString();
        
        btnSave.Disabled = true;

    }

    [AjaxMethod]
    public void DataSave()
    {
        var data = dc.Szksknowledge.Where(p => p.Postid == decimal.Parse(hdnPostid.Value.ToString()) && p.Maindept == SessionBox.GetUserSession().DeptNumber);
        if (data.Count() > 0)
        {
            data.First().Kcontent = HtmlEditor1.Value.ToString();
            dc.SubmitChanges();
            Ext.Msg.Alert("提示", "修改成功!").Show();
        }
        else
        {
            Szksknowledge sk = new Szksknowledge
            {
                Kcontent = HtmlEditor1.Value.ToString(),
                Maindept = SessionBox.GetUserSession().DeptNumber,//"241700000",
                Postid = decimal.Parse(hdnPostid.Value.ToString()),
                Status = "1",
                Remarks = "",
                Zyid = dc.Post.First(p=>p.Postid==decimal.Parse(hdnPostid.Value.ToString())).Zyid
            };
            dc.Szksknowledge.InsertOnSubmit(sk);
            dc.SubmitChanges();
            Ext.Msg.Alert("提示", "新增成功!").Show();
        }
    }


}
