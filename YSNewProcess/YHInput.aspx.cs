using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Coolite.Ext.Web;
using GhtnTech.SEP.DAL;
using GhtnTech.SecurityFramework.BLL;
using GhtnTech.SecurityFramework.DBUtility;
using GhtnTech.SEP.OraclDAL;
using System.Xml;

public partial class YSNewProcess_YHInput : System.Web.UI.Page
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            baseset();
            //Gridload();
            Hidden1.Value = "-1";
            btnEdit.Disabled = true;
            btnDelete.Disabled = true;
            //绑定排查人
            pcPersonLoad();
            //高级查询
            BindStore(int.Parse(PublicMethod.ReadXmlReturnNode("ZY", this)), TypeStore);
            BindStore(int.Parse(PublicMethod.ReadXmlReturnNode("FXLX", this)), FXStore);
            bindGLDX(PublicMethod.ReadXmlReturnNode("REN", this) + "," + PublicMethod.ReadXmlReturnNode("GUAN", this), GLRYStore);

            cbbBc.SelectedItem.Value = PublicCode.GetBanci(DateTime.Now);
        }
    }

   

    private void pcPersonLoad()
    {
        var person = (from pl in dc.Vgetpl
                      where pl.Moduletag == "YSNewProcess_YHProcess" && pl.Operatortag == "YH_fcfk" && pl.Unitid == SessionBox.GetUserSession().DeptNumber
                      && pl.Personnumber == SessionBox.GetUserSession().PersonNumber
                      select new
                      {
                          pl.Personnumber,
                          pl.Name,
                          pl.Deptname
                      }).Distinct();
        PCpersonStore.DataSource = person;
        PCpersonStore.DataBind();
        cbbPerson.SelectedItem.Value = SessionBox.GetUserSession().PersonNumber;
    }

    private void baseset()
    {

        //初始化排查时间
        //凌晨2点前录入算上一天中班
        if (System.DateTime.Now.Hour >= 2)
        {
            dfPCtime.Value = System.DateTime.Today;
        }
        else
        {
            dfPCtime.Value = System.DateTime.Today.AddDays(-1);
        }

    }


    [AjaxMethod]
    public void PYsearch(string py, string store)
    {
        if (py.Trim() == "")
        {
            return;
        }
        switch (store.Trim())
        {
            case "deptStore":
                if (PublicCode.BugCreate(SessionBox.GetUserSession().DeptNumber))
                {
                    string areadept = dc.Person.First(p => p.Personnumber == SessionBox.GetUserSession().PersonNumber).Areadeptid;
                    var dept = from d in dc.Department
                               where d.Deptnumber.Substring(0, 4) == SessionBox.GetUserSession().DeptNumber.Substring(0, 4)
                               && d.Deptnumber != areadept && d.Fatherid != areadept
                               && (dc.F_PINYIN(d.Deptname).ToLower().Contains(py.ToLower()) || d.Deptname.Contains(py.Trim()))
                               select new
                               {
                                   deptID = d.Deptnumber,
                                   deptName = d.Deptname
                               };
                    deptStore.DataSource = dept.Distinct();
                    deptStore.DataBind();
                }
                else
                {
                    var dept = from d in dc.Department
                               where d.Deptnumber.Substring(0, 4) == SessionBox.GetUserSession().DeptNumber.Substring(0, 4)
                               && (d.Deptnumber.Substring(7) == "00" || d.Deptlevel=="正科级") && d.Deptstatus=="1"
                               && (dc.F_PINYIN(d.Deptname).ToLower().Contains(py.ToLower()) || d.Deptname.Contains(py.Trim()))
                               select new
                               {
                                   deptID = d.Deptnumber,
                                   deptName = d.Deptname
                               };
                    deptStore.DataSource = dept.Distinct();
                    deptStore.DataBind();
                }
                break;
            case "placeStore":
                if (PublicCode.BugCreate(SessionBox.GetUserSession().DeptNumber))
                {
                    decimal areaid = (from p in dc.Person
                                      from d in dc.Department
                                      from a in dc.Placeareas
                                      where p.Personnumber == SessionBox.GetUserSession().PersonNumber
                                        && p.Areadeptid == d.Deptnumber && d.Deptname.Replace(" ", "") == a.Pareasname
                                      select a).First().Pareasid;
                    var place = from pl in dc.Place
                                where pl.Maindeptid == SessionBox.GetUserSession().DeptNumber
                                && pl.Pareasid != areaid && pl.Placestatus==1
                                && (dc.F_PINYIN(pl.Placename).ToLower().Contains(py.ToLower()) || pl.Placename.Contains(py.Trim()))
                                select new
                                {
                                    placID = pl.Placeid,
                                    placName = pl.Placename
                                };
                    placeStore.DataSource = place;
                    placeStore.DataBind();
                }
                else
                {
                    var place = from pl in dc.Place
                                where pl.Maindeptid == SessionBox.GetUserSession().DeptNumber && pl.Placestatus==1
                                && (dc.F_PINYIN(pl.Placename).ToLower().Contains(py.ToLower()) || pl.Placename.Contains(py.Trim()))
                                select new
                                {
                                    placID = pl.Placeid,
                                    placName = pl.Placename
                                };
                    placeStore.DataSource = place;
                    placeStore.DataBind();
                }
                break;
            case "yhStore"://修改多列显示
                var hazard = from yh in dc.Getyhandhazusing
                             where yh.Deptnumber == SessionBox.GetUserSession().DeptNumber
                             && (yh.Conpyfirst.ToLower().Contains(py.ToLower()) || yh.Yhcontent.Contains(py.Trim()) || yh.Yhnumber.ToLower().Contains(py.ToLower()))
                             select new
                             {
                                 yhNumber = yh.Yhid,
                                 yhContent=yh.Yhcontent,
                                 Gzrwname=yh.Levelname,
                                 Gxname=yh.Typename
                             };
                yhStore.DataSource = hazard;
                yhStore.DataBind();
                break;
            case "PCpersonStore":
                if (cbbJctype.SelectedItem.Value == "2")
                {
                    //局端排查人
                    var person = from p in dc.Person
                                 from d in dc.Department
                                 where p.Areadeptid == d.Deptnumber && p.Visualfield == 3 && p.Personstatus==1
                                 && (dc.F_PINYIN(p.Name).ToLower().Contains(py.ToLower()) || p.Name.Contains(py.Trim()))
                                 select new
                                 {
                                     p.Personnumber,
                                     p.Name,
                                     d.Deptname
                                 };
                    PCpersonStore.DataSource = person;
                    PCpersonStore.DataBind();
                }
                else
                {
                    var person = from pl in dc.Vgetpl
                                 from p in dc.Person
                                 where pl.Moduletag == "HiddenDanage_HDprocess" && pl.Operatortag == "YH_fcfk" && pl.Unitid == SessionBox.GetUserSession().DeptNumber
                                 && (dc.F_PINYIN(pl.Name).ToLower().Contains(py.ToLower()) || pl.Name.Contains(py.Trim()))
                                 && pl.Personnumber==p.Personnumber && p.Personstatus==1
                                 select new
                                 {
                                     pl.Personnumber,
                                     pl.Name
                                 };
                    PCpersonStore.DataSource = person.Distinct();
                    PCpersonStore.DataBind();
                }
                break;
            case "WorkTaskStore":
                var gzrw = dc.Worktasks.Where(
                    p => p.Professionalid == decimal.Parse(cbbSbsdy.SelectedItem.Value)
                    && dc.F_PINYIN(p.Worktask).ToLower().Contains(py.ToLower())
                    );
                WorkTaskStore.DataSource = gzrw;
                WorkTaskStore.DataBind();
                break;
        }
    }

    #region 绑定树

    private Coolite.Ext.Web.TreeNodeCollection LoadTree(Coolite.Ext.Web.TreeNodeCollection nodes)
    {
        if (nodes == null)
        {
            nodes = new Coolite.Ext.Web.TreeNodeCollection();
        }//根节点为null时

        tpPerson.Root.Clear();
        Coolite.Ext.Web.TreeNode root = new Coolite.Ext.Web.TreeNode();
        if (cbbJctype.SelectedItem.Value == "2")
        {
            var dept = dc.Department.First(p => p.Deptnumber == "000000000");
            root = new Coolite.Ext.Web.TreeNode(dept.Deptnumber, dept.Deptname, Icon.UserHome);
            tpPerson.Root.Add(root);
            var per = (from d in dc.Department
                       where //d.Deptnumber.Substring(0, 4) == "1303"
                       //&& (d.Deptnumber.Substring(7) == "00" || d.Deptlevel == "正科级")
                       //&& 
                       d.Visualfield == 3
                       select new
                       {
                           d.Deptnumber,
                           d.Deptname
                       }).OrderBy(p => p.Deptname);
            foreach (var r in per)
            {
                AsyncTreeNode asyncNode = new AsyncTreeNode(r.Deptnumber, r.Deptname);
                asyncNode.Icon = Icon.UserEarth;
                root.Nodes.Add(asyncNode);
            }

            //局端排查人
            var person = from p in dc.Person
                         from d in dc.Department
                         where p.Areadeptid == d.Deptnumber && p.Visualfield == 100//返回空结构
                         select new
                         {
                             p.Personnumber,
                             p.Name,
                             d.Deptname
                         };
            SelectedStore.DataSource = person;
            SelectedStore.DataBind();

        }
        else
        {
            var dept = dc.Department.First(p => p.Deptnumber == SessionBox.GetUserSession().DeptNumber);
            root = new Coolite.Ext.Web.TreeNode(dept.Deptnumber, dept.Deptname, Icon.UserHome);
            tpPerson.Root.Add(root);
            var per = (from v in dc.Vgetpl
                       where v.Operatortag == "YH_fcfk" && v.Moduletag == "HiddenDanage_HDprocess" && v.Unitid == SessionBox.GetUserSession().DeptNumber
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

            //排查人初始绑定
            var person = (from pl in dc.Vgetpl
                          where pl.Moduletag == "YSNewProcess_YHProcess" && pl.Operatortag == "YH_fcfk" && pl.Unitid == SessionBox.GetUserSession().DeptNumber
                          && pl.Personnumber == SessionBox.GetUserSession().PersonNumber
                          select new
                          {
                              pl.Personnumber,
                              pl.Name,
                              pl.Deptname
                          }).Distinct();
            if (person.Where(p => p.Personnumber == SessionBox.GetUserSession().PersonNumber).Count() > 0)
            {
                cbbPerson.SelectedItem.Value = SessionBox.GetUserSession().PersonNumber;
                var person1 = person.Where(p => p.Personnumber == SessionBox.GetUserSession().PersonNumber);
                SelectedStore.DataSource = person1;
                SelectedStore.DataBind();
            }
        }
        
        return nodes;
    }

    [AjaxMethod]
    public string RefreshMenu()
    {
        Coolite.Ext.Web.TreeNodeCollection nodes = LoadTree(tpPerson.Root);
        return nodes.ToJson();
    }

    //[AjaxMethod]
    //public void PersonDialogBuild()
    //{
    //    BuildTree();
    //    if (cbbJctype.SelectedItem.Value != "2")
    //    {
            
    //    }
    //}

    //private void BuildTree()
    //{
    //    tpPerson.Root.Clear();
    //    if (cbbJctype.SelectedItem.Value == "2")
    //    {
    //        var dept = dc.Department.First(p => p.Deptnumber == "000000000");
    //        Coolite.Ext.Web.TreeNode root = new Coolite.Ext.Web.TreeNode(dept.Deptnumber, dept.Deptname, Icon.UserHome);
    //        tpPerson.Root.Add(root);
    //        var per = (from d in dc.Department
    //                   where d.Deptnumber.Substring(0, 4) == "1303"
    //                   && (d.Deptnumber.Substring(7) == "00" || d.Deptlevel == "正科级")
    //                   && d.Visualfield == 3
    //                   select new
    //                   {
    //                       d.Deptnumber,
    //                       d.Deptname
    //                   }).OrderBy(p => p.Deptname);
    //        foreach (var r in per)
    //        {
    //            AsyncTreeNode asyncNode = new AsyncTreeNode(r.Deptnumber, r.Deptname);
    //            asyncNode.Icon = Icon.UserEarth;
    //            root.Nodes.Add(asyncNode);
    //        }
    //    }
    //    else
    //    {
    //        var dept = dc.Department.First(p => p.Deptnumber == SessionBox.GetUserSession().DeptNumber);
    //        Coolite.Ext.Web.TreeNode root = new Coolite.Ext.Web.TreeNode(dept.Deptnumber, dept.Deptname, Icon.UserHome);
    //        tpPerson.Root.Add(root);
    //        var per = (from v in dc.Vgetpl
    //                   where v.Operatortag == "YH_fcfk" && v.Moduletag == "HiddenDanage_HDprocess" && v.Unitid == SessionBox.GetUserSession().DeptNumber
    //                   select new
    //                   {
    //                       v.Deptnumber,
    //                       v.Deptname
    //                   }).Distinct().OrderBy(p => p.Deptname);
    //        foreach (var r in per)
    //        {
    //            AsyncTreeNode asyncNode = new AsyncTreeNode(r.Deptnumber, r.Deptname);
    //            asyncNode.Icon = Icon.UserEarth;
    //            root.Nodes.Add(asyncNode);
    //        }
    //    }
    //}

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
        if (cbbJctype.SelectedItem.Value == "2")
        {
            var per = from p in dc.Person
                      where p.Visualfield == 3 && (p.Areadeptid==deptid || p.Maindeptid==deptid)
                      select new
                      {
                          p.Personnumber,
                          p.Name,
                          Deptname = father.Deptname
                      };
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
        else
        {
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
            string strPer = "";
            string strName = "";
            int i = 0;
            foreach (XmlNode record in uRecords)
            {
                if (record != null)
                {
                    strPer += record.SelectSingleNode("Personnumber").InnerText.Trim() + ",";
                    strName += record.SelectSingleNode("Name").InnerText.Trim() + ",";
                    i++;
                }
            }
            var per = from p in dc.Person
                      where p.Personnumber == SessionBox.GetUserSession().PersonNumber
                      select new
                      {
                          Personnumber = strPer.Substring(0, strPer.Length - 1),
                          Name = strName.Substring(0, strName.Length - 1)
                      };
            PCpersonStore.DataSource = per;
            PCpersonStore.DataBind();
            cbbPerson.SelectedIndex = 0;
            //tfPerson.Text = strName.Substring(0, strName.Length - 1);
            //hdnPerson.SetValue(strPer.Substring(0, strPer.Length - 1));
        }
        else
        {
            cbbPerson.Items.Clear();
            //tfPerson.Text = "";
            //hdnPerson.SetValue("");
        }
    }

    private void GetPersonName(int Yhid)
    {
        var per = from ym in dc.NyhinputMore
                  from p in dc.Person
                  from d in dc.Department
                  where ym.Personid == p.Personnumber && p.Areadeptid == d.Deptnumber && ym.Yhputinid == Yhid
                  select new
                  {
                      p.Personnumber,
                      p.Name,
                      d.Deptname
                  };
        SelectedStore.DataSource = per;
        SelectedStore.DataBind();
        if (per.Count() > 0)
        {
            string strPer = "";
            string strName = "";
            foreach (var r in per)
            {
                strPer += r.Personnumber + ",";
                strName += r.Name + ",";
            }
            var person = from p in dc.Person
                         where p.Personnumber == SessionBox.GetUserSession().PersonNumber
                         select new
                         {
                             Personnumber = strPer.Substring(0, strPer.Length - 1),
                             Name = strName.Substring(0, strName.Length - 1)
                         };
            PCpersonStore.DataSource = person;
            PCpersonStore.DataBind();
            cbbPerson.SelectedIndex = 0;
            //tfPerson.Text = strName.Substring(0, strName.Length - 1);
            //hdnPerson.SetValue(strPer.Substring(0, strPer.Length - 1));
        }
        else
        {
            cbbPerson.Items.Clear();
            //tfPerson.Text = "";
            //hdnPerson.SetValue("");
        }
    }

    [AjaxMethod]
    public void SelectLoad()
    {
        var yh = dc.Getyhandhazusing.First(p => p.Yhid == decimal.Parse(cbbyh.SelectedItem.Value) && p.Deptnumber == SessionBox.GetUserSession().DeptNumber);
        string msg = "危 险 源：" + yh.HContent;
        msg += "\r\n辨识单元：" + yh.Typename;
        msg += "\r\n风险类型：" + yh.Fxlx;
        msg += "\r\n风险等级：" + yh.Fxlevel;
        msg += "\r\n事故类型：" + yh.Sglx;
        msg += "\r\n隐患级别：" + yh.Levelname;
        try
        {
            msg += "\r\n矿查积分：" + yh.Kcscore;
        }
        catch
        {
            msg += "\r\n矿查积分：无";
        }
        TextArea2.Text = msg;
    }

    private void Gridload()
    {
        //绑定隐患录入信息
        var data = from yh in dc.Getyhinput
                   from ny in dc.Nyhinput
                   from m in dc.NyhinputMore
                   where yh.Yhputinid==ny.Yhputinid && yh.Unitid == SessionBox.GetUserSession().DeptNumber
                   //多人排查 如果为排查人也可看到
                   && yh.Yhputinid==m.Yhputinid
                   && (ny.Inputpersonid == SessionBox.GetUserSession().PersonNumber || m.Personid==SessionBox.GetUserSession().PersonNumber)
                   && yh.Intime >= System.DateTime.Today.AddDays(-7)//查询最近7天的信息
                   orderby yh.Intime descending
                   select new
                   {
                       YHPutinID = yh.Yhputinid,
                       DeptName = yh.Deptname,
                       PlaceName = yh.Placename,
                       YHContent = yh.Yhcontent,
                       Remarks = yh.Remarks,
                       BanCi = yh.Banci,
                       Name = yh.Name,
                       INTime = yh.Intime,
                       PCTime = yh.Pctime,
                       YHType = yh.Typename,
                       Status = yh.Status
                   };
        if (data.Count() > 0)
        {
            YHputinStore.DataSource = data;
            YHputinStore.DataBind();
        }
    }

    protected void RowClick(object sender, AjaxEventArgs e)//流程处理
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            var data = dc.Nyhinput.First(p => p.Yhputinid == decimal.Parse(sm.SelectedRow.RecordID.Trim()));
            if (data.Status != "新增")
            {
                cbbDept.SelectedItem.Value = "";
                cbbplace.SelectedItem.Value = "";
                cbbyh.SelectedItem.Value = "";
                TextArea1.Text = "";
                btnEdit.Disabled = true;
                btnDelete.Disabled = true;
                TextArea2.Text = "";
                return;
            }
            cbbBc.SelectedItem.Value = data.Banci;
            //部门------------------------
            cbbDept.Items.Clear();
            var dept = from d in dc.Department
                       where d.Deptnumber == data.Deptid
                       select new
                       {
                           deptID = d.Deptnumber,
                           deptName = d.Deptname
                       };
            deptStore.DataSource = dept;
            deptStore.DataBind();
            cbbDept.SelectedItem.Value = data.Deptid;
            //cbbDept.SelectedItem.Text = dept.First().deptName;
            //------------------------
            //cbbPerson.SelectedItem.Value = data.Personid;
            GetPersonName(int.Parse(sm.SelectedRow.RecordID.Trim()));
            dfPCtime.Value = data.Pctime.Value;
            cbbplace.Items.Clear();
            //地点
            var place = from pl in dc.Place
                        where pl.Placeid == data.Placeid && pl.Placestatus==1
                        select new
                        {
                            placID = pl.Placeid,
                            placName = pl.Placename
                        };
            placeStore.DataSource = place;
            placeStore.DataBind();
            cbbplace.SelectedItem.Value = data.Placeid.ToString();
            //------------------------
            try
            {
                TextArea1.Text = data.Remarks.Trim();
            }
            catch
            {
                TextArea1.Text = "";
            }
            cbbStatus.SelectedItem.Value = data.Status;
            cbbJctype.SelectedItem.Value = data.Jctype.ToString();
            cbbyh.Items.Clear();
            //修改多列显示
            var yh = from hd in dc.Getyhandhazusing
                     where hd.Yhid == data.Yhid && hd.Deptnumber == SessionBox.GetUserSession().DeptNumber
                     select new
                     {
                         yhNumber = hd.Yhid,
                         yhContent = hd.Yhcontent,
                         Gzrwname = hd.Levelname,
                         Gxname = hd.Typename
                     };
            //var yh = from ha in dc.Hazards
            //         from ore in dc.HazardsOre
            //         where ha.HNumber == ore.HNumber && ore.Deptnumber == SessionBox.GetUserSession().DeptNumber && ore.HBjw == "引用" //&& ore.YhSwBjw == "隐患"
            //         && ha.HNumber==data.Yhnumber
            //         select new
            //         {
            //             yhNumber = ha.HNumber,
            //             yhContent = ore.HBm
            //         };
            yhStore.DataSource = yh;
            yhStore.DataBind();
            cbbyh.SelectedItem.Value = data.Yhid.ToString();
            Hidden1.Value = sm.SelectedRow.RecordID.Trim();
            btnEdit.Disabled = false;
            btnDelete.Disabled = false;

            SelectLoad();
        }
    }

    [AjaxMethod]
    public void ClearClick()
    {
        Ext.Msg.Confirm("提示", "是否确认删除?", new MessageBox.ButtonsConfig
        {
            Yes = new MessageBox.ButtonConfig
            {
                Handler = "Coolite.AjaxMethods.DoDel();",
                Text = "确 定"
            },
            No = new MessageBox.ButtonConfig
            {
                Text = "取 消"
            }
        }).Show();
    }
    [AjaxMethod]
    public void DoDel()
    {
        var yh = dc.Nyhinput.First(p => p.Yhputinid == decimal.Parse(Hidden1.Value.ToString().Trim()));
        dc.Nyhinput.DeleteOnSubmit(yh);
        dc.SubmitChanges();
        //删除人员列表
        var ymmore = dc.NyhinputMore.Where(p => p.Yhputinid == decimal.Parse(Hidden1.Value.ToString().Trim()));
        dc.NyhinputMore.DeleteAllOnSubmit(ymmore);
        dc.SubmitChanges();
        Hidden1.Value = "-1";
        Ext.Msg.Alert("提示", "删除成功!").Show();
        btnEdit.Disabled = true;
        btnDelete.Disabled = true;
        //Gridload();
        Ext.DoScript("#{YHputinStore}.reload();");
        cbbDept.SelectedItem.Value = "";
        cbbplace.SelectedItem.Value = "";
        cbbyh.SelectedItem.Value = "";
        TextArea1.Text = "";
    }
    //下面的可以优化
    private void AddYhMorePerson(decimal Yhid, string[] person)
    {
        //var ymall = dc.NyhinputMore.Where(p => p.Yhputinid == Yhid && !person.Contains(p.Personid));//所有不在人员列表的数据
        //dc.NyhinputMore.DeleteAllOnSubmit(ymall);
        //dc.SubmitChanges();
        string msg = "";
        foreach (string per in person)
        {
            //decimal rjid = PublicCode.GetKQrecord(per, dfPCtime.SelectedDate, cbbBc.SelectedItem.Value, SessionBox.GetUserSession().PersonNumber);
            //if (rjid == -1)
            //{
            //    msg += dc.Person.First(p => p.Personnumber == per).Name + ":为检测到一小时内出井记录!;";
            //}
            //else
            //{
            if (dc.NyhinputMore.Where(p => p.Yhputinid == Yhid && p.Personid == per).Count() == 0)
            {
                DBSCMDataContext db = new DBSCMDataContext();
                NyhinputMore ym = new NyhinputMore
                {//修正多人排查不同享检查信息
                    Personid = per,
                    Yhputinid = Yhid,
                    //--------------------------------
                    Pctime = dfPCtime.SelectedDate,
                    Banci = cbbBc.SelectedItem.Value,
                    Jctype = cbbJctype.SelectedIndex,
                    Remarks = TextArea1.Text.Trim()//,
                    //Rjid=rjid
                    //--------------------------------
                };
                db.NyhinputMore.InsertOnSubmit(ym);
                db.SubmitChanges();
                //msg += dc.Person.First(p => p.Personnumber == per).Name + ":添加成功!;";
                //更新入井信息
                //var rj = dc.KqRecord.First(p => p.Rjid == rjid);
                //rj.Inputyhcount = rj.Inputyhcount == null ? 1 : (rj.Inputyhcount.Value + 1);
                //rj.Status = 1;
                //dc.SubmitChanges();
                //更新走动计划
                PublicCode.setUpdateMoveState(per, Convert.ToDateTime(dfPCtime.Value), decimal.Parse(cbbplace.SelectedItem.Value.Trim()));
            }
            else
            {
                msg += dc.Person.First(p => p.Personnumber == per).Name + ":已添加过的人员!;";
            }
            //}
        }
        //WriteYHlog(Yhid, msg);
        //return msg;
    }

    private void WriteYHlog(decimal yhid, string log)
    {
        Nyhlog nl = new Nyhlog
        {
            Recordtime=System.DateTime.Now,
            Yhputinid=yhid,
            Log=log
        };
        dc.Nyhlog.InsertOnSubmit(nl);
        dc.SubmitChanges();
    }

    [AjaxMethod]
    public void AddClick(string action)
    {
        if (cbbBc.SelectedIndex == -1 || cbbDept.SelectedIndex == -1 || cbbplace.SelectedIndex == -1 || cbbyh.SelectedIndex == -1 || dfPCtime.SelectedValue == null || cbbPerson.SelectedIndex == -1)//hdnPerson.Value.ToString().Trim()=="")
        {
            Ext.Msg.Alert("提示", "请填写完整信息!").Show();
            return;
        }
        string[] pergroup = cbbPerson.SelectedItem.Value.Trim().Split(',');//hdnPerson.Value.ToString().Split(',');//排查人数组
        if (action == "new")
        {
            //获取各班次截止时间
            string time = "00:00:00";
            switch (cbbBc.SelectedItem.Value.Trim())
            {
                case "早班":
                    time = PublicMethod.ReadXmlReturnNode("ZBSJ", this);
                    break;
                case "中班":
                    time = PublicMethod.ReadXmlReturnNode("ZHBSJ", this);
                    break;
                case "夜班":
                    time = PublicMethod.ReadXmlReturnNode("WBSJ", this);
                    break;
            }
            //可录入时间为当班时间+4小时
            DateTime btime = DateTime.Parse(System.DateTime.Today.ToString("yyyy-MM-dd") + " " + time).AddHours(-10);//起始时间
            DateTime etime = DateTime.Parse(System.DateTime.Today.ToString("yyyy-MM-dd") + " " + time).AddHours(2);//截止时间
            if (btime.Day < System.DateTime.Today.Day)
            {
                btime=btime.AddDays(1);
            }
            if (etime.Day > System.DateTime.Today.Day)
            {
                etime = etime.AddDays(-1);
            }
            if (etime > btime)
            {
                if (System.DateTime.Now < btime || System.DateTime.Now > etime)
                {
                    Ext.Msg.Alert("提示", "不在当日" + cbbBc.SelectedItem.Value.Trim() + "隐患提交时间!").Show();
                    return;
                }
            }
            else
            {
                if (System.DateTime.Now < btime && System.DateTime.Now > etime)
                {
                    Ext.Msg.Alert("提示", "不在当日" + cbbBc.SelectedItem.Value.Trim() + "隐患提交时间!").Show();
                    return;
                }
            }
            try
            {
                var yho = dc.Nyhinput.Where(
                    p =>
                        p.Yhid ==decimal.Parse( cbbyh.SelectedItem.Value.Trim())
                        && p.Deptid == cbbDept.SelectedItem.Value.Trim()
                        && p.Placeid == int.Parse(cbbplace.SelectedItem.Value.Trim())
                    && new string[] { "新增", "提交审批", "隐患未整改", "逾期未整改" }.Contains(p.Status));
                if (yho.Count() > 0)
                {
                    AddYhMorePerson(yho.First().Yhputinid, pergroup);
                    
                    GhtnTech.SecurityFramework.BLL.LogManager.WriteLog(SessionBox.GetUserSession().PersonNumber, SessionBox.GetUserSession().LoginName, "", DateTime.Now, GhtnTech.SecurityFramework.BLL.ActiveType.录入隐患, yho.First().Yhputinid.ToString(), "");
                    Ext.Msg.Alert("提示", "执行隐患合并操作！并入隐患编号为：" + yho.First().Yhputinid).Show() ;//+ ";合并明细：<br>"+).Show();
                }
                else
                {
                    //现场整改隐患处理：同班下同一人判断为重复录入，多人或其他人录入并入隐患
                    var yho_s = dc.Nyhinput.Where(
                    p =>
                        p.Yhid == decimal.Parse(cbbyh.SelectedItem.Value.Trim())
                        && p.Deptid == cbbDept.SelectedItem.Value.Trim()
                        && p.Placeid == int.Parse(cbbplace.SelectedItem.Value.Trim())
                    && p.Status == "现场整改" && p.Banci == cbbBc.SelectedItem.Value.Trim() && p.Pctime == Convert.ToDateTime(dfPCtime.Value));
                    if (yho_s.Count() > 0)
                    {
                        if (pergroup.Length == 1 && pergroup[0] == yho_s.First().Personid)
                        {
                            Ext.Msg.Alert("提示", "不能重复录入隐患信息!").Show();
                            return;
                        }
                        else
                        {
                            AddYhMorePerson(yho.First().Yhputinid, pergroup);
                            
                            
                            GhtnTech.SecurityFramework.BLL.LogManager.WriteLog(SessionBox.GetUserSession().PersonNumber, SessionBox.GetUserSession().LoginName, "", DateTime.Now, GhtnTech.SecurityFramework.BLL.ActiveType.录入隐患, yho.First().Yhputinid.ToString(), "");
                            Ext.Msg.Alert("提示", "执行隐患合并操作！并入隐患编号为：" + yho.First().Yhputinid).Show();// + ";合并明细：<br>" + ).Show();
                            Ext.DoScript("#{YHputinStore}.reload();");
                            return;
                        }
                    }
                    var lel = dc.Getyhandhazusing.Where(p => p.Yhid == decimal.Parse(cbbyh.SelectedItem.Value.Trim()) && p.Deptnumber == SessionBox.GetUserSession().DeptNumber);
                    Nyhinput yh = new Nyhinput();
                    yh.Banci = cbbBc.SelectedItem.Value.Trim();
                    yh.Deptid = cbbDept.SelectedItem.Value.Trim();
                    yh.Inputpersonid = SessionBox.GetUserSession().PersonNumber;//cbbPerson.SelectedItem.Value.Trim();
                    DateTime dt = System.DateTime.Now;//当前时间 插入多人用
                    yh.Personid = pergroup[0];//cbbPerson.SelectedItem.Value.Trim();
                    yh.Intime = dt;
                    yh.Pctime = Convert.ToDateTime(dfPCtime.Value);
                    yh.Placeid = int.Parse(cbbplace.SelectedItem.Value.Trim());
                    yh.Remarks = TextArea1.Text.Trim();
                    yh.Status = cbbStatus.SelectedItem.Value.Trim();
                    yh.Yhid = decimal.Parse(cbbyh.SelectedItem.Value.Trim());
                    yh.Maindeptid = SessionBox.GetUserSession().DeptNumber;
                    yh.Jctype = cbbJctype.SelectedIndex;
                    if (lel.Count() > 0)
                    {
                        yh.Levelid = lel.First().Levelid;
                    }
                    //修正状态为null的问题
                    if (string.IsNullOrEmpty(yh.Status))
                    {
                        yh.Status = "现场整改";
                    }
                    dc.Nyhinput.Insert(yh);
                    dc.SubmitChanges();
                    
                    //if (cbbStatus.SelectedItem.Value.Trim() == "现场整改")
                    //{
                    //    string url = string.Format("FinePersonSelect.aspx?Post={0}&Mod={1}&Win={2}", dc.Nyhinput.First(p => p.Intime == dt && p.Inputpersonid == SessionBox.GetUserSession().PersonNumber).Yhputinid, "yh", FineWin.ClientID);
                    //    Ext.DoScript("#{FineWin}.load('" + url + "');#{FineWin}.show();");
                    //}
                    //else
                    //{
                    decimal yhinputid = dc.Nyhinput.First(p => p.Intime == dt && p.Inputpersonid == SessionBox.GetUserSession().PersonNumber).Yhputinid;
                     AddYhMorePerson(yhinputid, pergroup);
                    GhtnTech.SecurityFramework.BLL.LogManager.WriteLog(SessionBox.GetUserSession().PersonNumber, SessionBox.GetUserSession().LoginName, "", DateTime.Now, GhtnTech.SecurityFramework.BLL.ActiveType.录入隐患, yhinputid.ToString(), "");
                    Ext.Msg.Alert("提示", "保存成功!").Show();//排查人员添加明细:<br>"+).Show();
                    //}
                }
                //Gridload();
                Ext.DoScript("#{YHputinStore}.reload();");
                //清空隐患信息
                cbbyh.SelectedItem.Value = "";
                TextArea2.Text = "";
            }
            catch(Exception ex)
            {
                Ext.Msg.Alert("提示", string.Format("保存失败，请稍候重试!\n原因：{0}", ex.Message)).Show();
            }
        }
        else
        {
            var yh = dc.Nyhinput.First(p => p.Yhputinid == decimal.Parse(Hidden1.Value.ToString().Trim()));
            try
            {
                yh.Banci = cbbBc.SelectedItem.Value.Trim();
                yh.Deptid = cbbDept.SelectedItem.Value.Trim();
                yh.Inputpersonid = SessionBox.GetUserSession().PersonNumber; // cbbPerson.SelectedItem.Value.Trim();
                yh.Personid = pergroup[0];//cbbPerson.SelectedItem.Value.Trim();
                //yh.Intime = System.DateTime.Today;
                //yh.Pctime = Convert.ToDateTime(dfPCtime.Value);
                yh.Placeid = int.Parse(cbbplace.SelectedItem.Value.Trim());
                yh.Remarks = TextArea1.Text.Trim();
                yh.Status = cbbStatus.SelectedItem.Value.Trim();
                yh.Yhid = decimal.Parse(cbbyh.SelectedItem.Value.Trim());
                yh.Jctype = cbbJctype.SelectedIndex;
                //yh.Maindeptid = SessionBox.GetUserSession().DeptNumber;
                //dc.Yhinput.Insert(yh);
                dc.SubmitChanges();
                AddYhMorePerson(decimal.Parse(Hidden1.Value.ToString().Trim()), pergroup);
                Ext.Msg.Alert("提示", "修改成功!").Show();//排查人员添加明细:<br>" + ).Show();
                //Gridload();
                Ext.DoScript("#{YHputinStore}.reload();");
                //清空隐患信息
                cbbyh.SelectedIndex = -1;
                TextArea2.Text = "";
            }
            catch
            {
                Ext.Msg.Alert("提示", "保存失败，请稍候重试!").Show();
            }
        }
    }

    #region 高级查询
    private void BindStore(int fid, Store store)
    {
        string oracletext = "select infoid,infoname from CS_BASEINFOSET where FID = " + fid + " order by infoid";
        store.DataSource = OracleHelper.Query(oracletext);
        store.DataBind();
    }

    private void bindGLDX(string id, Store store)
    {
        string oracletext = "select M_OBJECTID infoid,NAME infoname from M_OBJECT where RISK_TYPESID in (" + id + ") order by M_OBJECTID";
        store.DataSource = OracleHelper.Query(oracletext);
        store.DataBind();
    }

    protected void SearchBLStoreRefresh(object sender, StoreRefreshDataEventArgs e)
    {
        SearchBLLoad();
    }

    [AjaxMethod]
    public void SearchBLLoad()
    {
        var yh = from hd in dc.Gethazardusing
                 where hd.Deptnumber == SessionBox.GetUserSession().DeptNumber
                 select new
                 {
                     hd.HNumber,
                     hd.Zyid,
                     hd.Zyname,
                     hd.Gzrwname,
                     hd.Gxname,
                     hd.Gldxid,
                     hd.Gldxname,
                     hd.Fxlxid,
                     hd.Fxlx,
                     hd.Glrnid,
                     hd.Glrn,
                     hd.HContent,
                     hd.HConsequences,//管理措施
                     hd.MStandards,//管理标准
                     hd.HBm,
                     hd.Yhswlevel,
                     Scores = hd.Scores.GetValueOrDefault(0)
                 };
        if (cbbSbsdy.SelectedIndex > -1)
        {
            yh = yh.Where(p => p.Zyid == decimal.Parse(cbbSbsdy.SelectedItem.Value));
        }
        if (cbbSgzrw.SelectedIndex > -1)//新增工作任务查询--存储过程没有id用名称检索
        {
            yh = yh.Where(p => p.Gzrwname == cbbSgzrw.SelectedItem.Text);
        }
        if (cbbSfxlx.SelectedIndex > -1)
        {
            yh = yh.Where(p => p.Fxlxid == decimal.Parse(cbbSfxlx.SelectedItem.Value));
        }
        if (cbbSgldx.SelectedIndex > -1)
        {
            yh = yh.Where(p => p.Gldxid == decimal.Parse(cbbSgldx.SelectedItem.Value));
        }
        if (cbbSglry.SelectedIndex > -1)
        {
            yh = yh.Where(p => p.Glrnid == decimal.Parse(cbbSglry.SelectedItem.Value));
        }
        if (tfSyhms.Text.Trim() != "")
        {
            yh = from y in yh
                 where y.HBm.Contains(tfSyhms.Text.Trim())
                 select y;
        }
        SearchBLStore.DataSource = yh;
        SearchBLStore.DataBind();

    }

    [AjaxMethod]
    public void ClearBL()
    {
        cbbSbsdy.SelectedItem.Value = null;
        cbbSfxlx.SelectedItem.Value = null;
        cbbSgldx.SelectedItem.Value = null;
        cbbSglry.SelectedItem.Value = null;
        cbbSgzrw.SelectedItem.Value = null;
        cbbSgzrw.Disabled = true;
        tfSyhms.Text = "";
    }

    [AjaxMethod]
    public void GLDXLoad()
    {
        if (cbbSfxlx.SelectedIndex > -1)
        {
            bindGLDX(cbbSfxlx.SelectedItem.Value, GLDXStore);
            cbbSgldx.SelectedItem.Value = null;
            cbbSgldx.Disabled = false;
        }
        else
        {
            cbbSgldx.SelectedItem.Value = null;
            cbbSgldx.Disabled = true;
        }
    }

    [AjaxMethod]
    public void GZRWLoad()
    {
        if (cbbSbsdy.SelectedIndex > -1)
        {
            //var gzrw = dc.Worktasks.Where(p => p.Professionalid == decimal.Parse(cbbSbsdy.SelectedItem.Value));
            //WorkTaskStore.DataSource = gzrw;
            //WorkTaskStore.DataBind();
            cbbSgzrw.Disabled = false;
        }
        else
        {
            cbbSgzrw.Disabled = true;
        }
        cbbSgzrw.SelectedItem.Value = null;
    }

    protected void SearchBLDblClick(object sender, AjaxEventArgs e)
    {
        RowSelectionModel sm = gpSearchBL.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            //修改多列显示
            var yh = from hd in dc.Gethazardusing
                     where hd.HNumber == sm.SelectedRow.RecordID && hd.Deptnumber == SessionBox.GetUserSession().DeptNumber
                     select new
                     {
                         yhNumber = hd.HNumber,
                         yhContent = hd.HBm,
                         hd.Gzrwname,
                         hd.Gxname
                     };
            yhStore.DataSource = yh;
            yhStore.DataBind();
            cbbyh.SelectedItem.Value = sm.SelectedRow.RecordID;
            SearchBLWindow.Hide();
            Ext.DoScript("#{cbbyh}.triggers[0].show();");
            SelectLoad();
        }
    }

    [AjaxMethod]
    public void TestBL(int index)
    {
        int a = cbbyh.Triggers.Count;
    }
    #endregion

    //复查反馈
    protected void btnFCFKClick(object sender, AjaxEventArgs e)
    {
        if (cbbplace.SelectedItem.Text == "")
        {
            Ext.Msg.Alert("提示", "地点不可为空!").Show();

        }
        else
        {

            Window1.Show();
            bindYH();
            Tab1.Show();
            Tab2.Disabled = true;
            Tab1.Disabled = false;

        }

    }

    //绑定待复查隐患
    private void bindYH()
    {

        //绑定隐患录入信息
        var query = from yh in dc.Nyhinput
                    from yu in dc.Getyhandhazusing
                    from d in dc.Department
                    from pl in dc.Place
                    from p in dc.Person
                    where yh.Yhid == yu.Yhid && yh.Deptid == d.Deptnumber && yh.Placeid == pl.Placeid && yu.Deptnumber == SessionBox.GetUserSession().DeptNumber
                    && yh.Status == "隐患已整改" && yh.Placeid == int.Parse(cbbplace.SelectedItem.Value.Trim()) && yh.Personid==p.Personnumber
                    orderby yh.Intime descending
                    select new
                    {
                        YHPutinID = yh.Yhputinid,
                        DeptName = d.Deptname,
                        PlaceName = pl.Placename,
                        YHContent = yu.Yhcontent,
                        Remarks = yh.Remarks,
                        BanCi = yh.Banci,
                        PCTime = yh.Pctime,
                        YHType = yu.Typename,
                        Status = yh.Status,
                        YHLevel = yu.Levelname,
                        INTime = yh.Intime,
                        Name = p.Name
                    };
        Store6.DataSource = query;
        Store6.DataBind();
    }

    protected void btnFCsubmitClick(object sender, AjaxEventArgs e)//提交复查意见
    {

        if (fcfk_fcyj.Value.ToString() == "" || fcfk_fcqk.SelectedItem.Value.Trim() == "")
        {
            Ext.Msg.Alert("提示", "输入不可为空!").Show();
        }
        else
        {
            //更新走动计划
            PublicCode.setUpdateMoveState(SessionBox.GetUserSession().PersonNumber, System.DateTime.Today, decimal.Parse(cbbplace.SelectedItem.Value.Trim()));
            this.Tab1.Show();
            this.Tab2.Disabled = true;
            this.Tab1.Disabled = false;

            RowSelectionModel sm = this.GridPanel2.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count > 0)
            {


                var yinhuanFC = new Nyinhuanreview
                {
                    Yhputinid = Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()),
                    Reviewopinion = fcfk_fcyj.Value.ToString(),
                    Reviewstate = fcfk_fcqk.SelectedItem.Value.Trim(),
                    Fctime = DateTime.Now,
                    Personid = SessionBox.GetUserSession().PersonNumber
                };
                Nyhinput Input = dc.Nyhinput.First(c => c.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
                Input.Status = fcfk_fcqk.SelectedItem.Value.Trim();
                dc.Nyinhuanreview.InsertOnSubmit(yinhuanFC);
                dc.SubmitChanges();
                bindYH();
                Ext.Msg.Alert("提示", "保存成功!").Show();
            }
        }

    }
    protected void btnReturnClick(object sender, AjaxEventArgs e)//返回
    {

        Tab1.Show();
        this.Tab2.Disabled = true;
        this.Tab1.Disabled = false;

    }

    protected void FCRowClick(object sender, AjaxEventArgs e)//选择待复查隐患
    {
        RowSelectionModel sm = this.GridPanel2.SelectionModel.Primary as RowSelectionModel;

        if (sm.SelectedRows.Count > 0)
        {
            fcfk_fcqk.SelectedIndex = 0;
            fcfk_fcyj.Value = "";
            fcfk_fcqk.SelectedItem.Value = "";
            this.Tab2.Disabled = false;
            this.Tab1.Disabled = true;
            this.Tab2.Show();
        }
        else
        {
            this.Tab2.Disabled = true;
        }
    }

    //新增隐患申请
    protected void btnApplyClick(object sender, AjaxEventArgs e)
    {
        this.df_date.SelectedDate = System.DateTime.Today;

        Window2.Show();
    }

    protected void Save(object sender, AjaxEventArgs e)
    {
        if (ta_Context.Text == "")
        {
            Ext.Msg.Alert("提示", "隐患内容输入不可为空!").Show();
            return;
        }
        var yhC = new Yscollect
        {
            Yhcontent = ta_Context.Text.Trim(),
            Personnumber = SessionBox.GetUserSession().PersonNumber,
            Intime = df_date.SelectedDate,
            Remark = "",
            Maindept = SessionBox.GetUserSession().DeptNumber,
            Ysbz = "隐患",
            Status = "是"
        };
        dc.Yscollect.InsertOnSubmit(yhC);
        dc.SubmitChanges();
        Ext.Msg.Alert("提示", "保存成功!").Show();
        this.ta_Context.Text = "";
    }

    protected void Cancel(object sender, AjaxEventArgs e)
    {
        Window2.Close();
    }
    protected void YHputinStore_RefershData(object sender, StoreRefreshDataEventArgs e)
    {
        int PageSize = this.pagecut.PageSize; //获取当前在页面中PagingToolBar 的PageSize的值
        int Count = 0;
        int CurPage = e.Start / PageSize + 1; //获取当前的页码是多少，也就是第几页
        HBBLL hb = new HBBLL();
        //绑定隐患录入信息
        var data2 = hb.GetYHInput2(SessionBox.GetUserSession().PersonNumber, SessionBox.GetUserSession().DeptNumber, ref Count);
        var data = from yh in data2
                   orderby yh.INTime descending
                   select new
                   {
                       YHPutinID = yh.YHPutinID,
                       DeptName = yh.DeptName,
                       PlaceName = yh.PlaceName,
                       YHContent = yh.YHContent,
                       Remarks = yh.Remarks,
                       BanCi = yh.BanCi,
                       Name = yh.Name,
                       INTime = yh.INTime,
                       PCTime = yh.PCTime,
                       YHType = yh.TypeName,
                       Status = yh.Status
                   };

        e.TotalCount = Count;
        if (e.TotalCount > 0)
        {
            this.YHputinStore.DataSource = data.Skip(e.Start <= 0 ? 0 : e.Start).Take(PageSize);//绑定数据
            this.YHputinStore.DataBind();
        }

    }
}
