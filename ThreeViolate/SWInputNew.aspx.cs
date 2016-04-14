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

public partial class ThreeViolate_SWInputNew : System.Web.UI.Page
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            baseset();
            Gridload();
            btnEdit.Disabled = true;
            btnDelete.Disabled = true;

            //绑定排查人
            pcPersonLoad();

            //高级查询
            BindStore(int.Parse(PublicMethod.ReadXmlReturnNode("ZY", this)), TypeStore);
            BindStore(int.Parse(PublicMethod.ReadXmlReturnNode("FXLX", this)), FXStore);
            bindGLDX(PublicMethod.ReadXmlReturnNode("REN", this) + "," + PublicMethod.ReadXmlReturnNode("GUAN", this), GLRYStore);
        }
    }

    private void pcPersonLoad()
    {
        var person = from pl in dc.Vgetpl
                     where pl.Moduletag == "HiddenDanage_HDprocess" && pl.Operatortag == "YH_fcfk" && pl.Unitid == SessionBox.GetUserSession().DeptNumber
                     select new
                     {
                         pl.Personnumber,
                         pl.Name
                     };
        PCpersonStore.DataSource = person;
        PCpersonStore.DataBind();
        if (person.Where(p => p.Personnumber == SessionBox.GetUserSession().PersonNumber).Count() > 0)
        {
            cbbPCperson.SelectedItem.Value = SessionBox.GetUserSession().PersonNumber;
        }
    }

    private void baseset()
    {
        
        //初始化排查时间
        dfPCtime.Value = System.DateTime.Today;


        //初始化录入人
        //var per = dc.Person.First(p => p.Personnumber == SessionBox.GetUserSession().PersonNumber);
        //cbbPCperson.Items.Clear();
        //cbbPCperson.Items.Add(new Coolite.Ext.Web.ListItem(per.Name, per.Personnumber));
        //cbbPCperson.SelectedIndex = 0;

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
            case "personStore":
                var person = from r in dc.Person
                             from d in dc.Department
                             where r.Deptid == d.Deptnumber && r.Maindeptid == SessionBox.GetUserSession().DeptNumber
                             && (r.Pinyin.ToLower().Contains(py.ToLower()) || r.Lightnumber.ToLower().Contains(py.ToLower()) || r.Name.Contains(py.Trim()))
                             select new
                             {
                                 pernID = r.Personid,
                                 pernName = r.Name,
                                 pernLightNumber = r.Lightnumber,
                                 persNnmber = r.Personnumber,
                                 DeptName = d.Deptname
                             };
                personStore.DataSource = person;
                personStore.DataBind();
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
            case "yhStore":
                var hazard = from ha in dc.Hazards
                             from ore in dc.HazardsOre
                             from p in dc.Process
                             from w in dc.Worktasks
                             where ha.HNumber == ore.HNumber && ha.Processid == p.Processid && p.Worktaskid == w.Worktaskid
                             && ore.Deptnumber == SessionBox.GetUserSession().DeptNumber && ore.HBjw == "引用"// && ore.YhSwBjw=="隐患" 
                              && (ore.SwPinyin.ToLower().Contains(py.ToLower()) || ore.SwBm.Contains(py.Trim()))
                             select new
                             {
                                 yhNumber = ha.HNumber,
                                 yhContent = ore.HBm,
                                 Gzrwname = w.Worktask,
                                 Gxname = p.Name
                             };
                //var hazard = from ha in dc.Hazards
                //             from ore in dc.HazardsOre
                //             where ha.HNumber == ore.HNumber && ore.Deptnumber == SessionBox.GetUserSession().DeptNumber && ore.HBjw == "引用" //&& ore.YhSwBjw == "三违"
                //                 //&& (ore.HFPinyin.ToLower().Contains(py.ToLower()) || ore.HBm.Contains(py.Trim()))
                //             && (ore.SwPinyin.ToLower().Contains(py.ToLower()) || ore.SwBm.Contains(py.Trim()))
                //             select new
                //             {
                //                 yhNumber = ha.HNumber,
                //                 yhContent = ore.SwBm
                //             };
                yhStore.DataSource = hazard;
                yhStore.DataBind();
                break;
            case "PCpersonStore":
                var person1 = from pl in dc.Vgetpl
                             where pl.Moduletag == "HiddenDanage_HDprocess" && pl.Operatortag == "YH_fcfk" && pl.Unitid == SessionBox.GetUserSession().DeptNumber
                             && (dc.F_PINYIN(pl.Name).ToLower().Contains(py.ToLower()) || pl.Name.Contains(py.Trim()))
                             select new
                             {
                                 pl.Personnumber,
                                 pl.Name
                             };
                PCpersonStore.DataSource = person1;
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

    [AjaxMethod]
    public void SelectLoad()
    {
        var yh = dc.Gethazardusing.First(p => p.HNumber == cbbSWcontent.SelectedItem.Value);
        string msg = "危 险 源：" + yh.HContent;
        msg += "\r\n辨识单元：" + yh.Zyname;
        msg += "\r\n工作任务：" + yh.Gzrwname;
        msg += "\r\n工    序：" + yh.Gxname;
        msg += "\r\n风险类型：" + yh.Fxlx;
        msg += "\r\n风险等级：" + yh.Fclevel;
        msg += "\r\n事故类型：" + yh.Sglx;
        msg += "\r\n三违级别：" + yh.Swlevel;
        try
        {
            msg += "\r\n三违积分：" + yh.SwScores;
        }
        catch
        {
            msg += "\r\n三违积分：无";
        }
        TextArea2.Text = msg;
    }

    private void Gridload()
    {
        //绑定三违录入信息
        var data = from sw in dc.Swinput
                   from ha in dc.Hazards
                   from hao in dc.HazardsOre
                   from p in dc.Process
                   from w in dc.Worktasks
                   from cs in dc.CsBaseinfoset
                   from cs1 in dc.CsBaseinfoset
                   from pl in dc.Place
                   from per1 in dc.Person
                   from per2 in dc.Person
                   where sw.Swnumber.Trim() == ha.HNumber && sw.Placeid == pl.Placeid && ha.HNumber == hao.HNumber
                   && sw.Pcpersonid==per1.Personnumber && sw.Swpersonid==per2.Personnumber
                   && ha.Processid == p.Processid && p.Worktaskid == w.Worktaskid && w.Professionalid == cs.Infoid && hao.HBjw == "引用"
                   && hao.SwLevelid==cs1.Infoid
                   && (sw.Inputpersonid == SessionBox.GetUserSession().PersonNumber || sw.Pcpersonid==SessionBox.GetUserSession().PersonNumber)
                   orderby sw.Intime descending
                   select new
                   {
                       ID = sw.Id,
                       PlaceName = pl.Placename,
                       SWContent = hao.SwBm,
                       BanCi = sw.Banci,
                       Levelname=cs1.Infoname,
                       //Name = per.Name,
                       PCTime = sw.Pctime,
                       PCname =per1.Name,
                       Name=per2.Name,
                       Isend=sw.Isend.Value==1?true:false,
                       sw.Remarks
                   };
        SWinputStore.DataSource = data;
        SWinputStore.DataBind();
    }

    protected void RowClick(object sender, AjaxEventArgs e)//流程处理
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            var sw = dc.Swinput.First(p => p.Id == decimal.Parse(sm.SelectedRow.RecordID.Trim()));
            try
            {
                if (sw.Isend.Value == 1)
                {
                    cbbplace.SelectedItem.Value = "";
                    cbbSWcontent.SelectedItem.Value = "";
                    cbbSwperson.SelectedItem.Value = "";
                    btnEdit.Disabled = true;
                    btnDelete.Disabled = true;
                    TextArea2.Text = "";
                    TextArea1.Text = "";
                    return;
                }
                else
                {
                    detailload();
                    SelectLoad();
                }
            }
            catch
            {
                detailload();
                SelectLoad();
            }
        }
    }

    private void detailload()
    {
         RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
         var sw = dc.Swinput.First(p => p.Id == decimal.Parse(sm.SelectedRow.RecordID.Trim()));
         if (sm.SelectedRows.Count > 0)
         {
             cbbBc.SelectedItem.Value = sw.Banci;
             dfPCtime.Value = sw.Pctime;
             TextArea1.Text = sw.Remarks;
             
             cbbPCperson.SelectedItem.Value = sw.Pcpersonid;
             //地点
             var place = from pl in dc.Place
                         where pl.Placeid == sw.Placeid
                         select new
                         {
                             placID = pl.Placeid,
                             placName = pl.Placename
                         };
             placeStore.DataSource = place;
             placeStore.DataBind();
             cbbplace.SelectedItem.Value = sw.Placeid.ToString();

             //三违人员
             var person = from r in dc.Person
                          from d in dc.Department
                          where r.Personnumber == sw.Swpersonid && r.Deptid == d.Deptnumber
                          select new
                          {
                              pernID = r.Personid,
                              pernName = r.Name,
                              pernLightNumber = "",
                              persNnmber = r.Personnumber,
                              DeptName = d.Deptname
                          };
             personStore.DataSource = person;
             personStore.DataBind();
             cbbSwperson.SelectedItem.Value = sw.Swpersonid;
             //三违
             //var yh = from ha in dc.Hazards
             //         from ore in dc.HazardsOre
             //         where ha.HNumber == ore.HNumber && ore.Deptnumber == SessionBox.GetUserSession().DeptNumber && ore.HBjw == "引用" //&& ore.YhSwBjw == "隐患"
             //         && ha.HNumber == sw.Swnumber
             //         select new
             //         {
             //             yhNumber = ha.HNumber,
             //             yhContent = ore.HBm
             //         };
             //修改多列显示
             var yh = from hd in dc.Gethazardusing
                      where hd.HNumber == sw.Swnumber
                      select new
                      {
                          yhNumber = hd.HNumber,
                          yhContent = hd.HBm,
                          hd.Gzrwname,
                          hd.Gxname
                      };
             yhStore.DataSource = yh;
             yhStore.DataBind();
             cbbSWcontent.SelectedItem.Value = sw.Swnumber;
             TextArea1.Text = sw.Remarks;
             btnEdit.Disabled = false;
             btnDelete.Disabled = false;
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
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            var sw = dc.Swinput.First(p => p.Id == decimal.Parse(sm.SelectedRow.RecordID.Trim()));
            dc.Swinput.DeleteOnSubmit(sw);
            dc.SubmitChanges();
            Ext.Msg.Alert("提示", "删除成功!").Show();
            btnEdit.Disabled = true;
            btnDelete.Disabled = true;
            Gridload();
            cbbSwperson.SelectedItem.Value = "";
            cbbplace.SelectedItem.Value = "";
            cbbSWcontent.SelectedItem.Value = "";
        }
    }
    [AjaxMethod]
    public void AddClick(string action)
    {
        if (cbbBc.SelectedIndex == -1 || cbbplace.SelectedIndex == -1 || cbbSWcontent.SelectedIndex == -1 || cbbPCperson.SelectedIndex == -1)
        {
            Ext.Msg.Alert("提示", "请填写完整信息!").Show();
            return;
        }
        if (action == "new")
        {
            try
            {
                Swinput sw = new Swinput
                {
                    Banci = cbbBc.SelectedItem.Value.Trim(),
                    Inputpersonid = SessionBox.GetUserSession().PersonNumber,//cbbPCperson.SelectedItem.Value,
                    Intime = System.DateTime.Now,
                    Pctime = Convert.ToDateTime(dfPCtime.Value),
                    Pcpersonid = cbbPCperson.SelectedItem.Value,
                    Placeid = int.Parse(cbbplace.SelectedItem.Value),
                    Swpersonid = cbbSwperson.SelectedItem.Value,
                    Swnumber = cbbSWcontent.SelectedItem.Value.Trim(),
                    Maindeptid = SessionBox.GetUserSession().DeptNumber,
                    Remarks=TextArea1.Text
                };

                dc.Swinput.Insert(sw);
                dc.SubmitChanges();
                Ext.Msg.Alert("提示", "保存成功!").Show();

                //更新走动计划
                PublicCode.setUpdateMoveState(cbbPCperson.SelectedItem.Value.Trim(), Convert.ToDateTime(dfPCtime.Value), decimal.Parse(cbbplace.SelectedItem.Value.Trim()));
                Gridload();

                //清空三违信息
                cbbSWcontent.SelectedIndex = -1;
                TextArea2.Text = "";
            }
            catch
            {
                Ext.Msg.Alert("提示", "保存失败，请稍候重试!").Show();
            }
        }
        else
        {
            RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count > 0)
            {
                try
                {
                    var sw = dc.Swinput.First(p => p.Id == decimal.Parse(sm.SelectedRow.RecordID.Trim()));
                    sw.Banci = cbbBc.SelectedItem.Value.Trim();
                    sw.Pctime = Convert.ToDateTime(dfPCtime.Value);
                    sw.Pcpersonid = cbbPCperson.SelectedItem.Value;
                    sw.Placeid = int.Parse(cbbplace.SelectedItem.Value);
                    sw.Swpersonid = cbbSwperson.SelectedItem.Value;
                    sw.Swnumber = cbbSWcontent.SelectedItem.Value.Trim();
                    sw.Remarks = TextArea1.Text;
                    dc.SubmitChanges();
                    Ext.Msg.Alert("提示", "修改成功!").Show();
                    Gridload();
                    //清空三违信息
                    cbbSWcontent.SelectedIndex = -1;
                    TextArea2.Text = "";
                }
                catch
                {
                    Ext.Msg.Alert("提示", "保存失败，请稍候重试!").Show();
                }
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
                     hd.SwBm,
                     hd.Swlevel,
                     Scores = hd.SwScores.GetValueOrDefault(0)
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
                 where y.SwBm.Contains(tfSyhms.Text.Trim())
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
            //var yh = from hd in dc.Gethazardusing
            //         where hd.HNumber == sm.SelectedRow.RecordID && hd.Deptnumber == SessionBox.GetUserSession().DeptNumber
            //         select new
            //         {
            //             yhNumber = hd.HNumber,
            //             yhContent = hd.SwBm
            //         };
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
            cbbSWcontent.SelectedItem.Value = sm.SelectedRow.RecordID;
            SearchBLWindow.Hide();
            Ext.DoScript("#{cbbSWcontent}.triggers[0].show();");
            SelectLoad();
        }
    }
    #endregion

    #region 新增三违
    //新增三违申请
    protected void btnApplyClick(object sender, AjaxEventArgs e)
    {
        this.df_date.SelectedDate = System.DateTime.Today;

        Window2.Show();
    }

    protected void Save(object sender, AjaxEventArgs e)
    {
        if (ta_Context.Text == "")
        {
            Ext.Msg.Alert("提示", "三违内容输入不可为空!").Show();
            return;
        }
        var yhC = new Yscollect
        {
            Yhcontent = ta_Context.Text.Trim(),
            Personnumber = SessionBox.GetUserSession().PersonNumber,
            Intime = df_date.SelectedDate,
            Remark = "",
            Maindept = SessionBox.GetUserSession().DeptNumber,
            Ysbz = "三违",
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
    #endregion

}
