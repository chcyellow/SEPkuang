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

public partial class YSNewProcess_MovePlanCreate : System.Web.UI.Page
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            //初始化时间
            dfbegin.SelectedDate = System.DateTime.Today.AddDays(1 - System.DateTime.Today.Day);
            dfend.SelectedDate = System.DateTime.Today.AddDays(1 - System.DateTime.Today.Day).AddMonths(1).AddDays(-1);
            dfbegin.MinDate = dfbegin.SelectedDate;
            //dfbegin.MaxDate = dfend.SelectedDate;
            dfend.MinDate = dfbegin.SelectedDate;
            BuildTree();
            BuildTreePlace();
        }
    }

    #region 新增计划
    [AjaxMethod]
    public void PlanCreate(decimal[,] place, string perid)
    {
        int count = 0;
        for (int i = 0; i < place.Length/2; i++)
        {
            int rate = (int)(((dfend.SelectedDate - dfbegin.SelectedDate).Days+1) / place[i,1]);
            //rate = rate == 0 ? 1 : rate;
            DateTime begin = dfbegin.SelectedDate;
            DateTime end = dfend.SelectedDate;
            int make = 0;
            for (; (begin <= end && make<place[i,1]); begin = begin.AddDays(rate))//begin.AddDays(rate) <= end
            {
                DBSCMDataContext dc1 = new DBSCMDataContext();
                Moveplan mp1 = new Moveplan
                {
                    Personid = perid,
                    Placeid = place[i, 0],
                    Starttime = begin,
                    Endtime = begin > end ? end : (begin.AddDays(rate) > end ? end : begin.AddDays(rate-1)),
                    Movestate = "未走动",
                    Maindept = SessionBox.GetUserSession().DeptNumber
                };
                dc1.Moveplan.InsertOnSubmit(mp1);
                dc1.SubmitChanges();
                count++;
                make++;
            }
        }        
        Ext.Msg.Alert("提示", "添加完成，共计添加" + count.ToString() + "条计划！").Show();
    }
    [AjaxMethod]
    public void PlanCreate(decimal[,] place, List<string> perids)
    {
        int count = 0;
        int countPsn = 0;
        foreach (var perid in perids)
        {
            count = 0;
            for (int i = 0; i < place.Length / 2; i++)
            {
                int rate = (int)(((dfend.SelectedDate - dfbegin.SelectedDate).Days + 1) / place[i, 1]);
                //rate = rate == 0 ? 1 : rate;
                DateTime begin = dfbegin.SelectedDate;
                DateTime end = dfend.SelectedDate;
                int make = 0;
                for (; (begin <= end && make < place[i, 1]); begin = begin.AddDays(rate))//begin.AddDays(rate) <= end
                {
                    DBSCMDataContext dc1 = new DBSCMDataContext();
                    Moveplan mp1 = new Moveplan
                    {
                        Personid = perid,
                        Placeid = place[i, 0],
                        Starttime = begin,
                        Endtime = begin > end ? end : (begin.AddDays(rate) > end ? end : begin.AddDays(rate - 1)),
                        Movestate = "未走动",
                        Maindept = SessionBox.GetUserSession().DeptNumber
                    };
                    dc1.Moveplan.InsertOnSubmit(mp1);
                    dc1.SubmitChanges();
                    count++;
                    make++;
                }
            }
            countPsn++;
        }
        Ext.Msg.Alert("提示", string.Format("添加完成，共添加{0}人。其中每人添加{1}条计划！",countPsn.ToString(),count.ToString())).Show();
    }
    #endregion

    #region 绑定人员树

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

    #region 绑定地点树

    private void BuildTreePlace()
    {
        tpPlace.Root.Clear();
        var dept = dc.Department.First(p => p.Deptnumber == SessionBox.GetUserSession().DeptNumber);
        Coolite.Ext.Web.TreeNode root = new Coolite.Ext.Web.TreeNode(dept.Deptnumber, dept.Deptname, Icon.HouseStar);
        tpPlace.Root.Add(root);
        var per = from v in dc.Placeareas
                  where v.Maindeptid == SessionBox.GetUserSession().DeptNumber
                  select new
                  {
                      v.Pareasid,
                      v.Pareasname
                  };
        foreach (var r in per)
        {
            AsyncTreeNode asyncNode = new AsyncTreeNode(r.Pareasid.ToString(), r.Pareasname);
            asyncNode.Icon = Icon.HouseKey;
            root.Nodes.Add(asyncNode);
        }
    }

    [AjaxMethod]
    public string NodeLoadPlace(string nodeID)
    {
        Coolite.Ext.Web.TreeNodeCollection nodes = new Coolite.Ext.Web.TreeNodeCollection();
        AddPlace(nodes, nodeID);
        return nodes.ToJson();
    }

    private void AddPlace(Coolite.Ext.Web.TreeNodeCollection nodes, string deptid)
    {
        var father = dc.Placeareas.First(p => p.Pareasid == decimal.Parse(deptid));

        var per = from p in dc.Place
                  where p.Pareasid == decimal.Parse(deptid) && p.Maindeptid == father.Maindeptid && p.Placestatus == 1//所有有效地点
                  select new
                  {
                      p.Placeid,
                      p.Placename,
                      father.Pareasname
                  };
        foreach (var r in per)
        {
            Coolite.Ext.Web.TreeNode asyncNode = new Coolite.Ext.Web.TreeNode();
            asyncNode.Text = r.Placename;
            asyncNode.NodeID = r.Placeid.ToString();
            asyncNode.Icon = Icon.House;
            asyncNode.CustomAttributes.Add(new ConfigItem("data", r.Pareasname, ParameterMode.Value));
            asyncNode.Listeners.DblClick.Handler = string.Format("PersonSelector.add({0},{1});", "GridPanel2", "[new Ext.data.Record({Placeid:'" + r.Placeid + "',Placename:'" + r.Placename + "',Pareasname:'" + r.Pareasname + "',Zdcs:1})]");
            asyncNode.Leaf = true;
            nodes.Add(asyncNode);
        }
    }

    #endregion

    protected void SubmitData(object sender, StoreSubmitDataEventArgs e)
    {
        if (dfbegin.SelectedDate > dfend.SelectedDate)
        {
            Ext.Msg.Alert("提示", "请重新选择时间！").Show();
            return;
        }
        if (GridData.Value == null || GridData.Value.ToString() == "-1")
        {
            if (selectedDept.Value == null || selectedDept.Value.ToString() == "-1" || selectedDept.Value.ToString() == SessionBox.GetUserSession().DeptNumber)
            {
                Ext.Msg.Alert("提示", "请选择人员或部门！").Show();
                return;
            }
            else
            {

                XmlNode xml = e.Xml;
                XmlNode rxml = xml.SelectSingleNode("records");
                XmlNodeList uRecords = rxml.SelectNodes("record");
                if (uRecords.Count > 0)
                {
                    List<string> pers = new List<string>();
                    var per = (from p in dc.Person
                               from v in dc.Vgetpl
                               where p.Personnumber == v.Personnumber && p.Areadeptid == selectedDept.Value.ToString() && v.Operatortag == "YH_fcfk" && v.Moduletag == "HiddenDanage_HDprocess"
                               select new
                               {
                                   p.Personnumber,
                                   p.Name
                               }).Distinct();
                    foreach (var r in per)
                    {
                        pers.Add(r.Personnumber);
                    }
                    decimal[,] place = new decimal[uRecords.Count, 2]; int i = 0;
                    foreach (XmlNode record in uRecords)
                    {
                        if (record != null)
                        {
                            place[i, 0] = decimal.Parse(record.SelectSingleNode("Placeid").InnerText.Trim());
                            place[i, 1] = decimal.Parse(record.SelectSingleNode("Zdcs").InnerText.Trim() == "" ? "1" : record.SelectSingleNode("Zdcs").InnerText.Trim());
                            i++;
                        }
                    }
                    PlanCreate(place, pers);
                }
                else
                {
                    Ext.Msg.Alert("提示", "请选择地点！").Show();
                }
            }
        }
        else
        {
            XmlNode xml = e.Xml;
            XmlNode rxml = xml.SelectSingleNode("records");
            XmlNodeList uRecords = rxml.SelectNodes("record");
            if (uRecords.Count > 0)
            {
                decimal[,] place = new decimal[uRecords.Count, 2]; int i = 0;
                foreach (XmlNode record in uRecords)
                {
                    if (record != null)
                    {
                        place[i, 0] = decimal.Parse(record.SelectSingleNode("Placeid").InnerText.Trim());
                        place[i, 1] = decimal.Parse(record.SelectSingleNode("Zdcs").InnerText.Trim() == "" ? "1" : record.SelectSingleNode("Zdcs").InnerText.Trim());
                        i++;
                    }
                }
                PlanCreate(place, GridData.Value.ToString());
            }
            else
            {
                Ext.Msg.Alert("提示", "请选择地点！").Show();
            }
        }
    }
}