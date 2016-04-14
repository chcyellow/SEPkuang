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

public partial class PAR_Par_SaftyConfirm : System.Web.UI.Page
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
        root.Text = "-1";
        root.NodeID = "辨识单元";
        tpZY.Root.Add(root);
        StringBuilder strSql = new StringBuilder();
        strSql.Append(string.Format("select * from CS_BASEINFOSET where FID={0} order by INFOID", PublicMethod.ReadXmlReturnNode("ZY", this)));

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
                AddGZRW(nodes, nodeID.Substring(1));
                break;
        }

        string a = nodes.ToJson();

        return nodes.ToJson();
    }

    private void AddGZRW(Coolite.Ext.Web.TreeNodeCollection nodes, string p)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append(string.Format("select * from worktasks where professionalid={0}", p));

        DataTable dt = OracleHelper.Query(strSql.ToString()).Tables[0];
        foreach (DataRow r in dt.Rows)
        {
            //AsyncTreeNode asyncNode = new AsyncTreeNode();
            //asyncNode.Text = r["WORKTASK"].ToString();
            //asyncNode.NodeID = "w" + r["WORKTASKID"].ToString();
            //nodes.Add(asyncNode);
            Coolite.Ext.Web.TreeNode asyncNode = new Coolite.Ext.Web.TreeNode();
            asyncNode.Text = r["WORKTASK"].ToString();
            asyncNode.NodeID = "w" + r["WORKTASKID"].ToString();
            asyncNode.Listeners.Click.Handler = string.Format("Coolite.AjaxMethods.GVLoad({0});", r["WORKTASKID"].ToString().Trim());
            asyncNode.Leaf = true;
            nodes.Add(asyncNode);
        }
    }

    #endregion

    [AjaxMethod]
    public void GVLoad(decimal workid)
    {
        Panel1.Html = Createtext(workid);
    }

    private string Createtext(decimal workid)
    {
        var wt = dc.Worktasks.First(p => p.Worktaskid == workid);
        var user = dc.Vgetpl.First(p => p.Personnumber == SessionBox.GetUserSession().PersonNumber);
        string text = string.Format("<P align=center><B>{0}风险预控安全确认</B></P><BR>",wt.Worktask);
        //text += string.Format("我是{0}<U>{2}</U>，现在进行{1}风险预控安全确认。<BR>", user.Deptname,wt.Worktask,user.Name);
        text += "<B>首先对本工作存在的危险源进行确认，本工作存在以下主要危险源：</B><BR>";
        var hz = from h in dc.Hazards
                 from gx in dc.Process
                 where h.Processid == gx.Processid && gx.Worktaskid == workid
                 select new
                 {
                     h.HContent,
                     h.HConsequences
                 };
        int Index = 1;
        string haz = "";
        string con = "";
        foreach (var r in hz)
        {
            haz += Index + "、" + r.HContent + "<BR>";
            con += Index + "、" + r.HConsequences + "<BR>";
            Index++;
        }

        text += haz;
        text += "<B>现在对以上危险源进行确认及风险描述：</B><BR>";
        text += con;
        text += "<B>" + wt.Worktask + "风险预控安全确认完毕。</B>";
        return text;
    }
}
