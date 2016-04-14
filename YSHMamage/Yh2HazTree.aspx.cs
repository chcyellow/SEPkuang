using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Coolite.Ext.Web;
using GhtnTech.SEP.DAL;
using System.Xml;
using System.Xml.Xsl;

public partial class YSHMamage_Yh2HazTree : System.Web.UI.Page
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            BuildTreeYh();
            BuildTreeHaz();
        }
    }

    #region 隐患树

    private void BuildTreeYh()
    {
        tpYh.Root.Clear();
        var type = from t in dc.CsBaseinfoset
                   where t.Fid == int.Parse(PublicMethod.ReadXmlReturnNode("ZY", this))
                   orderby t.Infoid ascending
                   select new
                   {

                       t.Infoid,
                       t.Infoname
                   };
        var lavel = from c in dc.CsBaseinfoset
                    where c.Fid == int.Parse(PublicMethod.ReadXmlReturnNode("YHJB", this))
                    orderby c.Infoid
                    select new
                    {
                        c.Infoid,
                        c.Infoname
                    };
        Coolite.Ext.Web.TreeNode root = new Coolite.Ext.Web.TreeNode();
        root.NodeID = "-1";
        root.Text = "隐患信息";
        tpYh.Root.Add(root);
        foreach (var r in type)
        {
            Coolite.Ext.Web.TreeNode asyncNode = new Coolite.Ext.Web.TreeNode();
            asyncNode.NodeID = r.Infoid.ToString();
            asyncNode.Text = r.Infoname;
            //asyncNode.Icon = Icon.UserEarth;
            foreach (var i in lavel)
            {
                AsyncTreeNode asyncc = new AsyncTreeNode(i.Infoid.ToString(), i.Infoname);
                //asyncc.Icon = Icon.UserEarth;
                asyncc.CustomAttributes.Add(new ConfigItem("data", r.Infoid.ToString(), ParameterMode.Value));
                asyncNode.Nodes.Add(asyncc);
            }
            root.Nodes.Add(asyncNode);
        }  
    }

    [AjaxMethod]
    public string NodeLoadYh(string nodeID,string TypeId)
    {
        Coolite.Ext.Web.TreeNodeCollection nodes = new Coolite.Ext.Web.TreeNodeCollection();
        AddYh(nodes, decimal.Parse(nodeID), decimal.Parse(TypeId));
        return nodes.ToJson();
    }

    private void AddYh(Coolite.Ext.Web.TreeNodeCollection nodes, decimal LevelId, decimal TypeId)
    {
         var data = from yh in dc.Yhbase
                     where  yh.Levelid == LevelId && yh.Typeid == TypeId
                   select new
                   {
                       yh.Yhid,
                       yh.Yhnumber,
                       yh.Yhcontent
                   };
         foreach (var r in data)
         {
             Coolite.Ext.Web.TreeNode asyncNode = new Coolite.Ext.Web.TreeNode();
             asyncNode.Text = r.Yhcontent;//"["+r.Yhnumber+"]"
             asyncNode.Qtip = r.Yhcontent;
             asyncNode.NodeID = r.Yhid.ToString();
             //asyncNode.Icon = Icon.User;
             //asyncNode.CustomAttributes.Add(new ConfigItem("Type", r.Typename, ParameterMode.Value));
             //asyncNode.CustomAttributes.Add(new ConfigItem("Level", r.Levelname, ParameterMode.Value));
             asyncNode.Listeners.Click.Handler = "Coolite.AjaxMethods.LoadYh2Haz(" + r.Yhid.ToString() + ",1)";
             //asyncNode.Listeners.DblClick.Handler = string.Format("PersonSelector.add({0},{1});", "GridPanel3", "[new Ext.data.Record({Personnumber:'" + r.Personnumber + "',Name:'" + r.Name + "',Deptname:'" + r.Deptname + "'})]");
             asyncNode.Leaf = true;
             nodes.Add(asyncNode);
         }
    }

    #endregion

    #region 危险源树

    private void BuildTreeHaz()
    {
        tpHaz.Root.Clear();
        Coolite.Ext.Web.TreeNode root = new Coolite.Ext.Web.TreeNode();
        root.NodeID = "-1";
        root.Text = "辨识单元";
        tpHaz.Root.Add(root);
        var type = from t in dc.CsBaseinfoset
                   where t.Fid == int.Parse(PublicMethod.ReadXmlReturnNode("ZY", this))
                   orderby t.Infoid ascending
                   select new
                   {

                       t.Infoid,
                       t.Infoname
                   };
        foreach (var r in type)
        {
            AsyncTreeNode asyncNode = new AsyncTreeNode();
            asyncNode.Text = r.Infoname;
            asyncNode.NodeID = "z" + r.Infoid.ToString();
            root.Nodes.Add(asyncNode);
        }
    }

    [AjaxMethod]
    public string NodeLoadHaz(string nodeID)
    {
        Coolite.Ext.Web.TreeNodeCollection nodes = new Coolite.Ext.Web.TreeNodeCollection();
        switch (nodeID.Substring(0, 1))
        {
            case "z":
                AddGZRW(nodes, nodeID.Substring(1));
                break;
            case "w":
                AddGX(nodes, nodeID.Substring(1));
                break;
            case "p":
                AddHaz(nodes, nodeID.Substring(1));
                break;
        }

        string a = nodes.ToJson();

        return nodes.ToJson();
    }

    private void AddGZRW(Coolite.Ext.Web.TreeNodeCollection nodes, string pid)
    {
        var gzrw = dc.Worktasks.Where(p => p.Professionalid ==decimal.Parse( pid));
        foreach (var r in gzrw)
        {
            AsyncTreeNode asyncNode = new AsyncTreeNode();
            asyncNode.Text = r.Worktask;
            asyncNode.NodeID = "w" + r.Worktaskid.ToString();
            nodes.Add(asyncNode);
        }
    }

    private void AddGX(Coolite.Ext.Web.TreeNodeCollection nodes, string pid)
    {
        var gx = dc.Process.Where(p => p.Worktaskid == decimal.Parse(pid)).OrderBy(p=>p.Serialnumber);
        foreach (var r in gx)
        {
            Coolite.Ext.Web.TreeNode asyncNode = new Coolite.Ext.Web.TreeNode();
            asyncNode.Text = r.Name;
            asyncNode.NodeID = "p" + r.Processid.ToString();
            //asyncNode.Listeners.Click.Handler = string.Format("Coolite.AjaxMethods.GVLoad('{0}','F');", r["PROCESSID"].ToString().Trim());
            //asyncNode.Leaf = true;
            nodes.Add(asyncNode);
        }
    }

    private void AddHaz(Coolite.Ext.Web.TreeNodeCollection nodes, string pid)
    {
        var gx = dc.Hazards.Where(p => p.Processid == decimal.Parse(pid));
        foreach (var r in gx)
        {
            Coolite.Ext.Web.TreeNode asyncNode = new Coolite.Ext.Web.TreeNode();
            asyncNode.Text = r.HContent;
            asyncNode.Qtip = r.HContent;
            asyncNode.NodeID = r.Hazardsid.ToString();
            asyncNode.Listeners.Click.Handler = "Coolite.AjaxMethods.LoadYh2Haz(" + r.Hazardsid.ToString() + ",1)";
            //asyncNode.Listeners.Click.Handler = string.Format("Coolite.AjaxMethods.GVLoad('{0}','F');", r["PROCESSID"].ToString().Trim());
            asyncNode.Leaf = true;
            nodes.Add(asyncNode);
        }
    }

    #endregion

    protected void SubmitData(object sender, StoreSubmitDataEventArgs e)
    {
        XmlNode xml = e.Xml;
        XmlNode rxml = xml.SelectSingleNode("records");
        XmlNodeList uRecords = rxml.SelectNodes("record");
        if (uRecords.Count > 0)
        {
            decimal[] Yh = new decimal[uRecords.Count]; int i = 0;
            foreach (XmlNode record in uRecords)
            {
                if (record != null)
                {
                    Yh[i] = decimal.Parse(record.SelectSingleNode("Yhid").InnerText.Trim());
                    i++;
                }
            }
            var deldata = dc.Yhmatchup.Where(p => Yh.Contains(p.Yhid));
            dc.Yhmatchup.DeleteAllOnSubmit(deldata);
            dc.SubmitChanges();
            foreach (XmlNode record in uRecords)
            {
                if (record != null)
                {
                    DBSCMDataContext db = new DBSCMDataContext();
                    Yhmatchup ym = new Yhmatchup
                    {
                        Yhid = decimal.Parse(record.SelectSingleNode("Yhid").InnerText.Trim()),
                        Hazardsid = decimal.Parse(record.SelectSingleNode("Hazardsid").InnerText.Trim())
                    };
                    db.Yhmatchup.InsertOnSubmit(ym);
                    db.SubmitChanges();
                }
            }
            Ext.Msg.Alert("提示", "对应成功！").Show();
        }
        else
        {
            Ext.Msg.Alert("提示", "请选择需要对应的信息！").Show();
        }
    }

    [AjaxMethod]
    public void LoadYh2Haz(decimal pid, int type)
    {
        var haz = from ym in dc.Yhmatchup
                  from h in dc.Hazards
                  from y in dc.Yhbase
                  where ym.Hazardsid == h.Hazardsid && ym.Yhid == y.Yhid
                  select new
                  {
                      y.Yhid,
                      y.Yhcontent,
                      h.Hazardsid,
                      h.HContent
                  };
        if (type == 1)
        {
            haz = haz.Where(p => p.Yhid == pid);
        }
        else
        {
            haz = haz.Where(p => p.Hazardsid == pid);
        }
        Store2.DataSource = haz;
        Store2.DataBind();
    }
}