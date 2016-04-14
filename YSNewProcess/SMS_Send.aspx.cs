using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Coolite.Ext.Web;
using System.Xml;
using GhtnTech.SEP.DAL;
using GhtnTech.SecurityFramework.BLL;

public partial class YSNewProcess_SMS_Send : BasePage
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            BuildTree();
            var YH = dc.Getyhinput.First(p => p.Yhputinid == decimal.Parse(Request.QueryString["Yhid"]));
            tfMSG.Text= YH.Deptname + "在" + YH.Placename + "发生隐患:" + YH.Remarks.Trim() == "" ? YH.Yhcontent.Trim() : YH.Remarks.Trim();
        }
    }

    #region 绑定树

    private void BuildTree()
    {
        tpPerson.Root.Clear();
        var dept = dc.Department.First(p => p.Deptnumber == SessionBox.GetUserSession().DeptNumber);
        Coolite.Ext.Web.TreeNode root = new Coolite.Ext.Web.TreeNode(dept.Deptnumber, dept.Deptname, Icon.UserHome);
        tpPerson.Root.Add(root);
        var per = (from v in dc.Vgetpl
                   where v.Operatortag == "YH_fcfk" && v.Moduletag == "YSNewProcess_YHProcess" && v.Unitid == SessionBox.GetUserSession().DeptNumber
                   select new
                   {
                       v.Deptnumber,
                       v.Deptname
                   }).Distinct().OrderBy(p => p.Deptname);
        foreach (var r in per)
        {
            AsyncTreeNode asyncNode = new AsyncTreeNode(r.Deptnumber, r.Deptname);
            asyncNode.Icon = Icon.UserEarth;
            root.Nodes.Add(asyncNode);
        }
    }

    [AjaxMethod]
    public string NodeLoad(string nodeID)
    {
        Coolite.Ext.Web.TreeNodeCollection nodes = new Coolite.Ext.Web.TreeNodeCollection();
        AddDeptandPerson(nodes, nodeID);
        return nodes.ToJson();
    }

    private void AddDeptandPerson(Coolite.Ext.Web.TreeNodeCollection nodes, string deptid)
    {
        var father = dc.Department.First(p => p.Deptnumber == deptid);

        var per = (from p in dc.Person
                   from v in dc.Vgetpl
                   where p.Personnumber == v.Personnumber && p.Areadeptid == deptid && v.Operatortag == "YH_fcfk" && v.Moduletag == "HiddenDanage_HDprocess"
                   select new
                   {
                       p.Personnumber,
                       p.Name,
                       Deptname = father.Deptname
                   }).Distinct();
        foreach (var r in per)
        {
            Coolite.Ext.Web.TreeNode asyncNode = new Coolite.Ext.Web.TreeNode();
            asyncNode.Text = r.Name;
            asyncNode.NodeID = r.Personnumber;
            asyncNode.Icon = Icon.User;
            asyncNode.CustomAttributes.Add(new ConfigItem("data", r.Deptname, ParameterMode.Value));
            asyncNode.Listeners.DblClick.Handler = string.Format("PersonSelector.add({0},{1});", "GridPanel3", "[new Ext.data.Record({Personnumber:'" + r.Personnumber + "',Name:'" + r.Name + "',Deptname:'" + r.Deptname + "'})]");
            asyncNode.Leaf = true;
            nodes.Add(asyncNode);
        }
    }

    #endregion

    protected void SubmitData(object sender, StoreSubmitDataEventArgs e)
    {
        //string json = e.Json;
        XmlNode xml = e.Xml;
        XmlNode rxml = xml.SelectSingleNode("records");
        XmlNodeList uRecords = rxml.SelectNodes("record");
        if (uRecords.Count > 0)
        {
            List<string> per = new List<string>();
            foreach (XmlNode record in uRecords)
            {
                if (record != null)
                {
                    per.Add(record.SelectSingleNode("Personnumber").InnerText.Trim());
                }
            }
            try
            {
                string msg=Sms.Send(per, tfMSG.Text, Request.QueryString["Yhid"], SmsType.HiddenTroubleTips, "-安全生产体系支撑平台!");
                Ext.Msg.Alert("提示", "发送成功！"+msg).Show();
            }
            catch
            {
                Ext.Msg.Alert("提示", "信息发送失败，请稍候重试！").Show();
            }
        }
        else
        {
            Ext.Msg.Alert("提示", "请选择人员！").Show();
        }
    }
}