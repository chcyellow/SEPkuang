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

public partial class GSSG_WorkINJURY : System.Web.UI.Page
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!Ext.IsAjaxRequest)
        {
            bindGSDJ();
            baseset();
            Gridload();
            Hidden1.Value = "-1";
            btnEdit.Disabled = true;
            btnDelete.Disabled = true;
        }
    }

    private void baseset()
    {

        ////初始化排查时间
        //dfPCtime.Value = System.DateTime.Today;

        //初始化录入人
        //var per = dc.Person.First(p => p.Personnumber == SessionBox.GetUserSession().PersonNumber);
        //cbbPerson.Items.Clear();
        //cbbPerson.Items.Add(new Coolite.Ext.Web.ListItem(per.Name, per.Personnumber));
        //cbbPerson.SelectedIndex = 0;

    }
    //绑定工伤等级
    private void bindGSDJ()
    {
        //绑定工伤等级
        int gsdjID = int.Parse(PublicMethod.ReadXmlReturnNode("GSDJ", this));
        //string oracletext = "select * from CS_BASEINFOSET where INFOID!= " + zyID + " start with INFOID= " + zyID + "  connect by prior INFOID = FID order by INFOID asc";
        string oracletext = "select * from CS_BASEINFOSET WHERE FID= " + gsdjID + "  ";
        gsdjStore.DataSource = OracleHelper.Query(oracletext);
        gsdjStore.DataBind();

        //从基础表绑定事故类型
        int sglxID = int.Parse(PublicMethod.ReadXmlReturnNode("SGLX", this));
        string d = "select * from CS_BASEINFOSET WHERE FID= " + sglxID + "  ";
        sglxStore.DataSource = OracleHelper.Query(d);
        sglxStore.DataBind();

        //绑定受伤部位

        int ssbwID = int.Parse(PublicMethod.ReadXmlReturnNode("SSBW", this));
        string a = "select * from CS_BASEINFOSET WHERE FID= " + ssbwID + "  ";
        ssbwStore.DataSource = OracleHelper.Query(a);
        ssbwStore.DataBind();

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


        }
    }




    private void Gridload()
    {
        //绑定工伤信息
        var data = from gs in dc.Workinjury
                   from per in dc.Person
                   from dept in dc.Department
                   // from pos in dc.Post
                   from pl in dc.Place
                   from gslevel in dc.CsBaseinfoset
                   from sglx in dc.CsBaseinfoset
                   from inper in dc.Person
                   where gs.Personnumber == per.Personnumber && gs.Placeid == pl.Placeid && gs.GsLevelid == gslevel.Infoid && gs.AccidentTypeid == sglx.Infoid && per.Areadeptid == dept.Deptnumber && inper.Personnumber == gs.Inpersonnumber && gs.Inpersonnumber == SessionBox.GetUserSession().PersonNumber
                   select new
                   {
                       gs.Id,
                       per.Name,
                       dept.Deptname,
                       //   pos.Postname,
                       pl.Placename,
                       gslevel.Infoname,
                       gs.Happendate,
                       gs.GsFact,
                       gs.PointsPer,
                       gs.FinePer,
                       gs.PointsDept,
                       gs.FineDept,
                       inPersonName = inper.Name,
                       gs.Indate,
                       sgleixing = sglx.Infoname,
                       gs.Banci,
                       gs.Injurysite,
                       gs.Reason,
                       gs.PointsBz,

                   };
        GSStore.DataSource = data.OrderByDescending(p=>p.Indate);
        GSStore.DataBind();

    }


    protected void RowClick(object sender, AjaxEventArgs e)
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            var gs = dc.Workinjury.First(p => p.Id == decimal.Parse(sm.SelectedRow.RecordID.Trim()));
            try
            {

                detailload();
                // SelectLoad();

            }
            catch
            {
                detailload();
                //SelectLoad();
            }
        }

    }

    private void detailload()
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        var gs = dc.Workinjury.First(p => p.Id == decimal.Parse(sm.SelectedRow.RecordID.Trim()));
        if (sm.SelectedRows.Count > 0)
        {




            gsss.Text = gs.GsFact;

            // GsLevelid = int.Parse( gsdj.SelectedItem.Value),
            Happentime.Value = gs.Happendate;
            // Placeid = int.Parse( cbbplace.SelectedItem.Value),
            grkf.Text = gs.PointsPer.ToString().Trim();
           // grfk.Text = gs.FinePer.ToString().Trim();
            bmkf.Text = gs.PointsDept.ToString().Trim();
            bmfk.Text = gs.FineDept.ToString().Trim();
            //cbbPerson.SelectedItem.Value = SessionBox.GetUserSession().PersonNumber;
            sglx.SelectedItem.Value = gs.AccidentTypeid.ToString();//事故类型
            ssbw.SelectedItem.Value = gs.Injurysite.ToString();
            gsdj.SelectedItem.Value = gs.GsLevelid.ToString();
           // ssbw.Text = gs.Injurysite;
            cbbBc.SelectedItem.Value = gs.Banci.Trim();
            zjyy.Text = gs.Reason;
            bzkf.Text = gs.PointsBz.ToString().Trim();
            if (!string.IsNullOrEmpty(gs.Remarks))
                bz.Text = gs.Remarks.Trim();



            //地点
            var place = from pl in dc.Place
                        where pl.Placeid == gs.Placeid
                        select new
                        {
                            placID = pl.Placeid,
                            placName = pl.Placename
                        };
            placeStore.DataSource = place;
            placeStore.DataBind();
            cbbplace.SelectedItem.Value = gs.Placeid.ToString();
            //工伤等级

            //var gsdj = from dj in dc.CsBaseinfoset
            //           where dj.Infoid == gs.GsLevelid
            //           select new
            //           {
            //               INFOID = dj.Infoid,
            //               INFONAME = dj.Infoname
            //           };
            //gsdjStore.DataSource = gsdj;
            //gsdjStore.DataBind();


            //工伤人员
            var person = from r in dc.Person
                         from d in dc.Department
                         where r.Personnumber == gs.Personnumber && r.Deptid == d.Deptnumber
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

            cbbGsperson.SelectedItem.Value = gs.Personnumber;

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
            var gs = dc.Workinjury.First(p => p.Id == decimal.Parse(sm.SelectedRow.RecordID.Trim()));
            dc.Workinjury.DeleteOnSubmit(gs);
            dc.SubmitChanges();
            Ext.Msg.Alert("提示", "删除成功!").Show();
            btnEdit.Disabled = true;
            btnDelete.Disabled = true;
            Gridload();
            cbbGsperson.SelectedItem.Value = "";
            cbbplace.SelectedItem.Value = "";
            gsss.Text = "";

            Happentime.Value = "";
            grkf.Text = "";
          //  grfk.Text = "";
            bmkf.Text = "";
            bmfk.Text = "";
            bz.Text = "";
        }
    }
    [AjaxMethod]
    public void AddClick(string action)
    {
        if (cbbGsperson.SelectedIndex == -1 || sglx.SelectedIndex == -1 || gsdj.SelectedIndex == -1 || ssbw.SelectedIndex == -1 
            || zjyy.Text.Trim() == "" || gsss.Text == "" || cbbplace.SelectedIndex == -1 || Happentime.SelectedValue == null 
            || cbbBc.SelectedIndex == -1)
        {
            Ext.Msg.Alert("提示", "请填写完整信息!").Show();
            return;
        }
        if (action == "new")
        {
            try
            {
                Workinjury gs = new Workinjury
                {
                    Personnumber = cbbGsperson.SelectedItem.Value,
                    GsFact = gsss.Text,
                    GsLevelid = int.Parse(gsdj.SelectedItem.Value),
                    Happendate = Convert.ToDateTime(Happentime.Value),
                    Placeid = int.Parse(cbbplace.SelectedItem.Value),
                    //PointsPer = int.Parse(grkf.Text),
                    //PointsDept = int.Parse(bmkf.Text),
                    //FineDept = int.Parse(bmfk.Text),
                    Inpersonnumber = SessionBox.GetUserSession().PersonNumber,
                    Indate = System.DateTime.Now,
                    Maindeptid = SessionBox.GetUserSession().DeptNumber,

                    AccidentTypeid = int.Parse(sglx.SelectedItem.Value),//事故类型
                    Banci = cbbBc.SelectedItem.Value,//班次
                    Injurysite = int.Parse(ssbw.SelectedItem.Value),//受伤部位
                    Reason = zjyy.Text,//直接原因
                    //PointsBz = int.Parse(bzkf.Text),//班组扣分
                    Remarks=bz.Text.Trim()//备注信息--xl

                };
                if (grkf.Text.Trim() != "")
                {
                    gs.PointsPer = int.Parse(grkf.Text);
                }
                if (bmkf.Text != "")
                {
                    gs.PointsDept = int.Parse(bmkf.Text);
                }
                if (bmfk.Text != "")
                {
                    gs.FineDept = int.Parse(bmfk.Text);
                }
                if (bzkf.Text != "")
                {
                    gs.PointsBz = int.Parse(bzkf.Text);
                }
                dc.Workinjury.Insert(gs);
                dc.SubmitChanges();
                Ext.Msg.Alert("提示", "保存成功!").Show();
                Gridload();
                //清空工伤事实
                gsss.Text = "";
                zjyy.Text = "";
                bz.Text = "";

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
                    var gs = dc.Workinjury.First(p => p.Id == decimal.Parse(sm.SelectedRow.RecordID.Trim()));
                    gs.Personnumber = cbbGsperson.SelectedItem.Value;
                    gs.GsFact = gsss.Text;
                    gs.GsLevelid = int.Parse(gsdj.SelectedItem.Value);
                    gs.Happendate = Convert.ToDateTime(Happentime.Value);
                    gs.Placeid = int.Parse(cbbplace.SelectedItem.Value);
                    //gs.PointsPer = int.Parse(grkf.Text);
                    //gs.PointsDept = int.Parse(bmkf.Text);
                    //gs.FineDept = int.Parse(bmfk.Text);
                    if (grkf.Text.Trim() != "")
                    {
                        gs.PointsPer = int.Parse(grkf.Text);
                    }
                    else
                    {
                        gs.PointsPer = null;
                    }
                    if (bmkf.Text != "")
                    {
                        gs.PointsDept = int.Parse(bmkf.Text);
                    }
                    else
                    {
                        gs.PointsDept = null;
                    }
                    if (bmfk.Text != "")
                    {
                        gs.FineDept = int.Parse(bmfk.Text);
                    }
                    else
                    {
                        gs.FineDept = null;
                    }
                    if (bzkf.Text != "")
                    {
                        gs.PointsBz = int.Parse(bzkf.Text);
                    }
                    else
                    {
                        gs.PointsBz = null;
                    }

                    gs.AccidentTypeid = int.Parse(sglx.SelectedItem.Value);//事故类型
                    gs.Banci = cbbBc.SelectedItem.Value;//班次
                    gs.Injurysite = int.Parse(ssbw.SelectedItem.Value);//受伤部位
                    gs.Reason = zjyy.Text;//直接原因
                   // gs.PointsBz = int.Parse(bzkf.Text);//班组扣分
                    gs.Remarks = bz.Text.Trim();
                    dc.SubmitChanges();
                    Ext.Msg.Alert("提示", "修改成功!").Show();
                    Gridload();
                    //清空工伤事实
                    gsss.Text = "";
                    bz.Text = "";
                }
                catch
                {
                    Ext.Msg.Alert("提示", "保存失败，请稍候重试!").Show();
                }
            }
        }
    }
}
