using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Coolite.Ext.Web;
using GhtnTech.SEP.DAL;
using System.Xml;
using GhtnTech.SEP.DBUtility;

public partial class SystemManage_Movegblevel : System.Web.UI.Page
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            BuildTree();
            StoreBind();
        }
    }

    #region 绑定树

    private void BuildTree()
    {
        tpPerson.Root.Clear();
        Coolite.Ext.Web.TreeNode root = new Coolite.Ext.Web.TreeNode("-1", "全局职务", Icon.UserHome);
        tpPerson.Root.Add(root);
        var per = (from v in dc.Position
                   select new
                   {
                       v.Posname
                   }).Distinct().OrderBy(p => p.Posname);
        int i = 0;
        foreach (var r in per)
        {
            Coolite.Ext.Web.TreeNode asyncNode = new Coolite.Ext.Web.TreeNode();
            asyncNode.Text = r.Posname;
            asyncNode.NodeID = "w" + i.ToString(); 
            asyncNode.Icon = Icon.User;
            asyncNode.Listeners.DblClick.Handler = string.Format("PersonSelector.add({0},{1});", "GridPanel3", "[new Ext.data.Record({Name:'" + r.Posname + "'})]");
            asyncNode.Leaf = true;
            root.Nodes.Add(asyncNode);
            i++;
        }
    }

    #endregion

    [AjaxMethod]
    public void StoreBind()
    {
        var per = (from v in dc.Position where v.Movegblevel==cbbStatus.SelectedItem.Value
                   select new
                   {
                       Name=v.Posname
                   }).Distinct().OrderBy(p => p.Name);
        SelectedStore.DataSource = per;
        SelectedStore.DataBind();
    }

    protected void SubmitData(object sender, StoreSubmitDataEventArgs e)
    {
        //string json = e.Json;
        XmlNode xml = e.Xml;
        XmlNode rxml = xml.SelectSingleNode("records");
        XmlNodeList uRecords = rxml.SelectNodes("record");
        if (uRecords.Count > 0)
        {
            string strName = "";
            foreach (XmlNode record in uRecords)
            {
                if (record != null)
                {
                    strName += "'"+record.SelectSingleNode("Name").InnerText.Trim() + "',";
                }
            }
            if (strName.Length > 0)
            {
                strName = strName.Substring(0, strName.Length - 1);
                string oracletext = string.Format("update position t set t.movegblevel='' where t.movegblevel='{0}'", cbbStatus.SelectedItem.Value);
                OracleHelper.Query(oracletext);
                oracletext = string.Format("update position t set t.movegblevel='{0}' where t.posname in ({1})", cbbStatus.SelectedItem.Value, strName);
                OracleHelper.Query(oracletext);
                Ext.Msg.Alert("提示", "设置成功!").Show();
            }
        }
        else
        {
            Ext.Msg.Alert("提示", "请天假需要设置的职务!").Show();
        }
    }
}