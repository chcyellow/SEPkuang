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

public partial class HiddenDanage_YHInputNew : System.Web.UI.Page
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            baseset();
            Gridload();
            Hidden1.Value = "-1";
            btnEdit.Disabled = true;
            btnDelete.Disabled = true;
            //绑定排查人
            pcPersonLoad();
            //高级查询
            BindStore(int.Parse(PublicMethod.ReadXmlReturnNode("ZY", this)), TypeStore);
            BindStore(int.Parse(PublicMethod.ReadXmlReturnNode("FXLX", this)), FXStore);
            bindGLDX(PublicMethod.ReadXmlReturnNode("REN", this) + "," + PublicMethod.ReadXmlReturnNode("GUAN", this), GLRYStore);

            BuildTree();
        }
    }

    private void pcPersonLoad()
    {
        var person = (from pl in dc.Vgetpl
                     where pl.Moduletag == "HiddenDanage_HDprocess" && pl.Operatortag == "YH_fcfk" && pl.Unitid == SessionBox.GetUserSession().DeptNumber
                     && pl.Personnumber == SessionBox.GetUserSession().PersonNumber
                     select new
                     {
                         pl.Personnumber,
                         pl.Name,
                         pl.Deptname
                     }).Distinct();
        PCpersonStore.DataSource = person;
        PCpersonStore.DataBind();
        if (person.Where(p => p.Personnumber == SessionBox.GetUserSession().PersonNumber).Count() > 0)
        {
            cbbPerson.SelectedItem.Value = SessionBox.GetUserSession().PersonNumber;
            var per=person.Where(p => p.Personnumber == SessionBox.GetUserSession().PersonNumber);
            //tfPerson.Text = per.First().Name;
            //hdnPerson.SetValue(SessionBox.GetUserSession().PersonNumber);
            SelectedStore.DataSource = per;
            SelectedStore.DataBind();
            //string js=string.Format("PersonSelector.add({0},{1});", "GridPanel3", "[new Ext.data.Record({Personnumber:'" + per.Personnumber + "',Name:'" + per.Name + "',Deptname:'" + per.Deptname + "'})]");
            //Ext.DoScript(js);
        }
    }

    private void baseset()
    {
        
        //初始化排查时间
        dfPCtime.Value = System.DateTime.Today;

        //初始化录入人
        //var per=dc.Person.First(p=>p.Personnumber==SessionBox.GetUserSession().PersonNumber);
        //cbbPerson.Items.Clear();
        //cbbPerson.Items.Add(new Coolite.Ext.Web.ListItem(per.Name,per.Personnumber));
        //cbbPerson.SelectedIndex = 0;
        
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
                var dept = from d in dc.Department
                           where d.Deptnumber.Substring(0, 4) == SessionBox.GetUserSession().DeptNumber.Substring(0, 4)
                           && d.Deptnumber.Substring(7) == "00"
                           && (dc.F_PINYIN(d.Deptname).ToLower().Contains(py.ToLower()) || d.Deptname.Contains(py.Trim()))
                           select new
                           {
                               deptID = d.Deptnumber,
                               deptName = d.Deptname
                           };
                deptStore.DataSource = dept;
                deptStore.DataBind();
                break;
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
            case "yhStore"://修改多列显示
                //var hazard = from hd in dc.Gethazardusing
                //             from ore in dc.HazardsOre
                //         where hd.HNumber==ore.HNumber && hd.Deptnumber == SessionBox.GetUserSession().DeptNumber
                //         && (ore.HFPinyin.ToLower().Contains(py.ToLower()) || ore.HBm.Contains(py.Trim()))
                //         select new
                //         {
                //             yhNumber = hd.HNumber,
                //             yhContent = hd.HBm,
                //             hd.Gzrwname
                //         };
                var hazard = from ha in dc.Hazards
                             from ore in dc.HazardsOre
                             from p in dc.Process
                             from w in dc.Worktasks
                             where ha.HNumber == ore.HNumber && ha.Processid == p.Processid && p.Worktaskid == w.Worktaskid
                             && ore.Deptnumber == SessionBox.GetUserSession().DeptNumber && ore.HBjw == "引用"// && ore.YhSwBjw=="隐患" 
                             && (ore.HFPinyin.ToLower().Contains(py.ToLower()) || ore.HBm.Contains(py.Trim()))
                             select new
                             {
                                 yhNumber = ha.HNumber,
                                 yhContent = ore.HBm,
                                 Gzrwname = w.Worktask,
                                 Gxname=p.Name
                             };
                yhStore.DataSource = hazard;
                yhStore.DataBind();
                break;
            case "PCpersonStore":
                var person = from pl in dc.Vgetpl
                             where pl.Moduletag == "HiddenDanage_HDprocess" && pl.Operatortag == "YH_fcfk" && pl.Unitid == SessionBox.GetUserSession().DeptNumber
                             && (dc.F_PINYIN(pl.Name).ToLower().Contains(py.ToLower()) || pl.Name.Contains(py.Trim()))
                             select new
                             {
                                 pl.Personnumber,
                                 pl.Name
                             };
                PCpersonStore.DataSource = person;
                PCpersonStore.DataBind();
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

    private void BuildTree()
    {
        tpPerson.Root.Clear();
        var dept = dc.Department.First(p => p.Deptnumber == SessionBox.GetUserSession().DeptNumber);
        Coolite.Ext.Web.TreeNode root = new Coolite.Ext.Web.TreeNode(dept.Deptnumber, dept.Deptname,Icon.UserHome);
        tpPerson.Root.Add(root);
        var per = (from v in dc.Vgetpl
                  where v.Operatortag == "YH_fcfk" && v.Moduletag == "HiddenDanage_HDprocess" && v.Unitid == SessionBox.GetUserSession().DeptNumber
                  select new
                  {
                      v.Deptnumber,
                      v.Deptname
                  }).Distinct().OrderBy(p=>p.Deptname);
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
        //var dept = dc.Department.Where(p => p.Fatherid == deptid);
        //foreach (var r in dept)
        //{
        //    AsyncTreeNode asyncNode = new AsyncTreeNode(r.Deptnumber, r.Deptname);
        //    nodes.Add(asyncNode);
        //}
        var per = from p in dc.Person
                  from v in dc.Vgetpl
                  where p.Personnumber == v.Personnumber && p.Areadeptid == deptid && v.Operatortag == "YH_fcfk" && v.Moduletag == "HiddenDanage_HDprocess"
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

    #endregion

    protected void SubmitData(object sender, StoreSubmitDataEventArgs e)
    {
        //string json = e.Json;
        XmlNode xml = e.Xml;
        XmlNode rxml = xml.SelectSingleNode("records");
        XmlNodeList uRecords = rxml.SelectNodes("record");
        if (uRecords.Count > 0)
        {
            string strPer="";
            string strName = "";
            int i = 0;
            foreach (XmlNode record in uRecords)
            {
                if (record != null)
                {
                    strPer += record.SelectSingleNode("Personnumber").InnerText.Trim()+",";
                    strName += record.SelectSingleNode("Name").InnerText.Trim()+",";
                    i++;
                }
            }
            var per = from p in dc.Person
                      where p.Personnumber == SessionBox.GetUserSession().PersonNumber
                      select new
                          {
                              Personnumber = strPer.Substring(0, strPer.Length - 1),
                              Name=strName.Substring(0, strName.Length - 1)
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
        var per = from ym in dc.YhinputMore
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
        var yh = dc.Gethazardusing.First(p => p.HNumber == cbbyh.SelectedItem.Value);
        string msg = "危 险 源：" + yh.HContent;
        msg += "\r\n辨识单元：" + yh.Zyname;
        msg += "\r\n工作任务：" + yh.Gzrwname;
        msg += "\r\n工    序：" + yh.Gxname;
        msg += "\r\n风险类型：" + yh.Fxlx;
        msg += "\r\n风险等级：" + yh.Fclevel;
        msg += "\r\n事故类型：" + yh.Sglx;
        msg += "\r\n隐患级别：" + yh.Yhswlevel;
        try
        {
            msg += "\r\n隐患积分：" + yh.Scores;
        }
        catch
        {
            msg += "\r\n隐患积分：无";
        }
        TextArea2.Text = msg;
    }

    private void Gridload()
    {
        //绑定隐患录入信息
        var data = from yh in dc.Yhinput
                   from ha in dc.Hazards
                   from hao in dc.HazardsOre
                   from p in dc.Process
                   from w in dc.Worktasks
                   from cs in dc.CsBaseinfoset
                   from d in dc.Department
                   from pl in dc.Place
                   where yh.Yhnumber == ha.HNumber && yh.Deptid == d.Deptnumber && yh.Placeid == pl.Placeid && ha.HNumber == hao.HNumber 
                   && ha.Processid == p.Processid && p.Worktaskid == w.Worktaskid && w.Professionalid == cs.Infoid && hao.HBjw == "引用"
                   && yh.Inputpersonid == SessionBox.GetUserSession().PersonNumber
                   && yh.Intime >=System.DateTime.Today.AddDays(-7)//查询最近7天的信息
                   orderby yh.Intime descending
                   select new
                   {
                       YHPutinID = yh.Yhputinid,
                       DeptName = d.Deptname,
                       PlaceName = pl.Placename,
                       YHContent = hao.HBm,
                       Remarks = yh.Remarks,
                       BanCi = yh.Banci,
                       //Name = per.Name,
                       PCTime = yh.Pctime,
                       YHType = cs.Infoname,
                       Status = yh.Status
                   };
        YHputinStore.DataSource = data;
        YHputinStore.DataBind();
    }

    protected void RowClick(object sender, AjaxEventArgs e)//流程处理
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            var data = dc.Yhinput.First(p => p.Yhputinid == decimal.Parse(sm.SelectedRow.RecordID.Trim()));
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
                        where pl.Placeid == data.Placeid
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
            cbbyh.Items.Clear();
            //修改多列显示
            var yh = from hd in dc.Gethazardusing
                         where hd.HNumber == data.Yhnumber
                         select new
                         {
                             yhNumber = hd.HNumber,
                             yhContent = hd.HBm,
                             hd.Gzrwname,
                             hd.Gxname
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
            cbbyh.SelectedItem.Value = data.Yhnumber;
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
        var yh = dc.Yhinput.First(p => p.Yhputinid == decimal.Parse(Hidden1.Value.ToString().Trim()));
        dc.Yhinput.DeleteOnSubmit(yh);
        dc.SubmitChanges();
        //删除人员列表
        var ymmore = dc.YhinputMore.Where(p => p.Yhputinid == decimal.Parse(Hidden1.Value.ToString().Trim()));
        dc.YhinputMore.DeleteAllOnSubmit(ymmore);
        dc.SubmitChanges();
        Hidden1.Value = "-1";
        Ext.Msg.Alert("提示", "删除成功!").Show();
        btnEdit.Disabled = true;
        btnDelete.Disabled = true;
        Gridload();
        cbbDept.SelectedItem.Value = "";
        cbbplace.SelectedItem.Value = "";
        cbbyh.SelectedItem.Value = "";
        TextArea1.Text = "";
    }
    //下面的可以优化
    private void AddYhMorePerson(decimal Yhid, string[] person)
    {
        var ymall = dc.YhinputMore.Where(p => p.Yhputinid == Yhid && !person.Contains(p.Personid));//所有不在人员列表的数据
        dc.YhinputMore.DeleteAllOnSubmit(ymall);
        dc.SubmitChanges();
        foreach (string per in person)
        {
            if (dc.YhinputMore.Where(p => p.Yhputinid == Yhid && p.Personid == per).Count() == 0)
            {
                DBSCMDataContext db = new DBSCMDataContext();
                YhinputMore ym = new YhinputMore
                {
                    Personid = per,
                    Yhputinid = Yhid
                };
                db.YhinputMore.InsertOnSubmit(ym);
                db.SubmitChanges();
                //更新走动计划
                PublicCode.setUpdateMoveState(per, Convert.ToDateTime(dfPCtime.Value), decimal.Parse(cbbplace.SelectedItem.Value.Trim()));
            }
        }
        
        
    }
    [AjaxMethod]
    public void AddClick(string action)
    {
        if (cbbBc.SelectedIndex == -1 || cbbDept.SelectedIndex == -1 || cbbplace.SelectedIndex == -1 || cbbyh.SelectedIndex == -1 || dfPCtime.SelectedValue == null || cbbPerson.SelectedIndex==-1)//hdnPerson.Value.ToString().Trim()=="")
        {
            Ext.Msg.Alert("提示", "请填写完整信息!").Show();
            return;
        }
        string[] pergroup = cbbPerson.SelectedItem.Value.Trim().Split(',');//hdnPerson.Value.ToString().Split(',');//排查人数组
        if (action=="new")
        {
            
            try
            {
                var yho = dc.Yhinput.Where(
                    p => 
                        p.Yhnumber == cbbyh.SelectedItem.Value.Trim() 
                        && p.Deptid == cbbDept.SelectedItem.Value.Trim() 
                        && p.Placeid == int.Parse(cbbplace.SelectedItem.Value.Trim()) 
                    && new string[]{"新增","提交审批","隐患未整改","逾期未整改"}.Contains(p.Status));
                if (yho.Count() > 0)
                {
                    AddYhMorePerson(yho.First().Yhputinid, pergroup);
                    Ext.Msg.Alert("提示", "保存成功!").Show();
                }
                else
                {
                    Yhinput yh = new Yhinput();
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
                    yh.Yhnumber = cbbyh.SelectedItem.Value.Trim();
                    yh.Maindeptid = SessionBox.GetUserSession().DeptNumber;
                    dc.Yhinput.Insert(yh);
                    dc.SubmitChanges();
                    AddYhMorePerson(dc.Yhinput.First(p => p.Intime == dt && p.Inputpersonid == SessionBox.GetUserSession().PersonNumber).Yhputinid, pergroup);
                    Ext.Msg.Alert("提示", "保存成功!").Show();
                }
                Gridload();
                //清空隐患信息
                cbbyh.SelectedItem.Value = "";
                TextArea2.Text = "";
            }
            catch
            {
                Ext.Msg.Alert("提示", "保存失败，请稍候重试!").Show();
            }
        }
        else
        {
            var yh = dc.Yhinput.First(p => p.Yhputinid == decimal.Parse(Hidden1.Value.ToString().Trim()));
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
                yh.Yhnumber = cbbyh.SelectedItem.Value.Trim();
                //yh.Maindeptid = SessionBox.GetUserSession().DeptNumber;
                //dc.Yhinput.Insert(yh);
                dc.SubmitChanges();
                AddYhMorePerson(decimal.Parse(Hidden1.Value.ToString().Trim()), pergroup);
                Ext.Msg.Alert("提示", "修改成功!").Show();
                Gridload();
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
        var query = from yh in dc.Yhview
                    where yh.Status == "隐患已整改" && yh.Placeid == int.Parse(cbbplace.SelectedItem.Value.Trim())
                    orderby yh.Intime descending
                    select new
                    {
                        YHPutinID = yh.Yhputinid,
                        DeptName = yh.Deptname,
                        PlaceName = yh.Placename,
                        YHContent = yh.HBm,
                        Remarks = yh.Remarks,
                        BanCi = yh.Banci,
                        PCTime = yh.Pctime,
                        YHType = yh.Zyname,
                        Status = yh.Status,
                        YHLevel = yh.Levelname,
                        INTime = yh.Intime,
                        Name = yh.Personname
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


                var yinhuanFC = new Yinhuanreview
                {
                    Yhputinid = Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()),
                    Reviewopinion = fcfk_fcyj.Value.ToString(),
                    Reviewstate = fcfk_fcqk.SelectedItem.Value.Trim(),
                    Fctime = DateTime.Now,
                    Personid = SessionBox.GetUserSession().PersonNumber
                };
                Yhinput Input = dc.Yhinput.First(c => c.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
                Input.Status = fcfk_fcqk.SelectedItem.Value.Trim();
                dc.Yinhuanreview.InsertOnSubmit(yinhuanFC);
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
}
