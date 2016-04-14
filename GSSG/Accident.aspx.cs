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

public partial class GSSG_Accident : System.Web.UI.Page
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        bindINFO();
        if (!Ext.IsAjaxRequest)
        {

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
        var per = dc.Person.First(p => p.Personnumber == SessionBox.GetUserSession().PersonNumber);
        cbbPerson.Items.Clear();
        cbbPerson.Items.Add(new Coolite.Ext.Web.ListItem(per.Name, per.Personnumber));
        cbbPerson.SelectedIndex = 0;

    }
    //绑定基础信息
    private void bindINFO()
    {
        int sfID = int.Parse(PublicMethod.ReadXmlReturnNode("SSSF", this));
        string oracletext = "select * from CS_BASEINFOSET WHERE FID= " + sfID + "  ";
        sfStore.DataSource = OracleHelper.Query(oracletext);
        sfStore.DataBind();


        int jtID = int.Parse(PublicMethod.ReadXmlReturnNode("SSJTGS", this));
        string a = "select * from CS_BASEINFOSET WHERE FID= " + jtID + "  ";
        jtStore.DataSource = OracleHelper.Query(a);
        jtStore.DataBind();

        int kjlxID = int.Parse(PublicMethod.ReadXmlReturnNode("KJLX", this));
        string b = "select * from CS_BASEINFOSET WHERE FID= " + kjlxID + "  ";
        kjlxStore.DataSource = OracleHelper.Query(b);
        kjlxStore.DataBind();

        int wsdjID = int.Parse(PublicMethod.ReadXmlReturnNode("WSDJ", this));
        string c = "select * from CS_BASEINFOSET WHERE FID= " + wsdjID + "  ";
        wsdjStore.DataSource = OracleHelper.Query(c);
        wsdjStore.DataBind();

        int sglxID = int.Parse(PublicMethod.ReadXmlReturnNode("SGLX", this));
        string d = "select * from CS_BASEINFOSET WHERE FID= " + sglxID + "  ";
        sglxStore.DataSource = OracleHelper.Query(d);
        sglxStore.DataBind();

        int sgdjID = int.Parse(PublicMethod.ReadXmlReturnNode("SGDJ", this));
        string e = "select * from CS_BASEINFOSET WHERE FID= " + sgdjID + "  ";
        sgdjStore.DataSource = OracleHelper.Query(e);
        sgdjStore.DataBind();
    }



   




    private void Gridload()
    {
        //绑定工伤信息
        var data = from sg in dc.Accidentcase
                   from sf in dc.CsBaseinfoset
                   from jt in dc.CsBaseinfoset
                   from sglx in dc.CsBaseinfoset
                   from sgdj in dc.CsBaseinfoset
                   from wsdj in dc.CsBaseinfoset
                   from kjlx in dc.CsBaseinfoset
                   from inper in dc.Person
                   where sg.Provinces == sf.Infoid && sg.Jtgs == jt.Infoid && sg.AccidentTypeid == sglx.Infoid && sg.AccidentLevelid == sgdj.Infoid && sg.WsLevelid == wsdj.Infoid && sg.OreTypeid == kjlx.Infoid && inper.Personnumber == sg.Inpersonnumber && sg.Inpersonnumber == SessionBox.GetUserSession().PersonNumber
                   select new
                   {
                       sg.Id,
                       sg.Accidentname,
                       sg.Orename,
                       sg.Deathnumber,
                       sg.Zsnumber,
                       sg.Qsnumber,
                       sg.ZjLoss,
                       sg.JjLoss,
                      
                       sf=sf.Infoname,
                       jt=jt.Infoname,
                       sglx=sglx.Infoname,
                       sgdj=sgdj.Infoname,
                       wsdj=wsdj.Infoname,
                       kjlx=kjlx.Infoname,
                       sg.Happendate,
                       inper.Name,
                       sg.Indate,
                       sg.Sgfx


                   };
        SGStore.DataSource = data;
        SGStore.DataBind();

    }


    protected void RowClick(object sender, AjaxEventArgs e)
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            var gs = dc.Accidentcase.First(p => p.Id == decimal.Parse(sm.SelectedRow.RecordID.Trim()));
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
        var sg = dc.Accidentcase.First(p => p.Id == decimal.Parse(sm.SelectedRow.RecordID.Trim()));
        if (sm.SelectedRows.Count > 0)
        {


            sgName.Text = sg.Accidentname;
            sgdj.SelectedItem.Value = sg.AccidentLevelid.ToString();
            sglx.SelectedItem.Value = sg.AccidentTypeid.ToString();

            swNumber.Text = sg.Deathnumber.ToString();
            zsNumber.Text = sg.Zsnumber.ToString();
            qsNumber.Text = sg.Qsnumber.ToString();
            zjjjss.Text = sg.ZjLoss.ToString();
            jjjjss.Text = sg.JjLoss.ToString();
            sf.SelectedItem.Value = sg.Provinces.ToString();
            kjName.Text = sg.Orename;
            kjlx.SelectedItem.Value = sg.OreTypeid.ToString();
            wsdj.SelectedItem.Value = sg.WsLevelid.ToString();
            jt.SelectedItem.Value = sg.Jtgs.ToString();
            Happentime.Value = sg.Happendate;
            sgfenxi.Text = sg.Sgfx;
            sgjg.Text = sg.Sgjg;


            //gsss.Text = gs.GsFact;

            //// GsLevelid = int.Parse( gsdj.SelectedItem.Value),
            //Happentime.Value = gs.Happendate;
            //// Placeid = int.Parse( cbbplace.SelectedItem.Value),
            //grkf.Text = gs.PointsPer.ToString().Trim();
            //grfk.Text = gs.FinePer.ToString().Trim();
            //bmkf.Text = gs.PointsDept.ToString().Trim();
            //bmfk.Text = gs.FineDept.ToString().Trim();
            //cbbPerson.SelectedItem.Value = SessionBox.GetUserSession().PersonNumber;



            ////地点
            //var place = from pl in dc.Place
            //            where pl.Placeid == gs.Placeid
            //            select new
            //            {
            //                placID = pl.Placeid,
            //                placName = pl.Placename
            //            };
            //placeStore.DataSource = place;
            //placeStore.DataBind();
            //cbbplace.SelectedItem.Value = gs.Placeid.ToString();
            ////工伤等级

            //var gsdj = from dj in dc.CsBaseinfoset
            //           where dj.Infoid == gs.GsLevelid
            //           select new
            //           {
            //               INFOID = dj.Infoid,
            //               INFONAME = dj.Infoname
            //           };
            //gsdjStore.DataSource = gsdj;
            //gsdjStore.DataBind();

            ////工伤人员
            //var person = from r in dc.Person
            //             from d in dc.Department
            //             where r.Personnumber == gs.Personnumber && r.Deptid == d.Deptnumber
            //             select new
            //             {
            //                 pernID = r.Personid,
            //                 pernName = r.Name,
            //                 pernLightNumber = "",
            //                 persNnmber = r.Personnumber,
            //                 DeptName = d.Deptname
            //             };
            //personStore.DataSource = person;
            //personStore.DataBind();

            //cbbGsperson.SelectedItem.Value = gs.Personnumber;

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
            var gs = dc.Accidentcase.First(p => p.Id == decimal.Parse(sm.SelectedRow.RecordID.Trim()));
            dc.Accidentcase.DeleteOnSubmit(gs);
            dc.SubmitChanges();
            Ext.Msg.Alert("提示", "删除成功!").Show();
            btnEdit.Disabled = true;
            btnDelete.Disabled = true;
            Gridload();
            sgName.Text="";
            sgdj.SelectedItem.Value="";
            sglx.SelectedItem.Value="";

            swNumber.Text="";
            zsNumber.Text="";
            qsNumber.Text="";
            zjjjss.Text="";
            jjjjss.Text="";
            sf.SelectedItem.Value="";
            kjName.Text="";
            kjlx.SelectedItem.Value="";
            wsdj.SelectedItem.Value="";
            jt.SelectedItem.Value = "";
        }
    }
    [AjaxMethod]
    public void AddClick(string action)
    {
        if (sgdj.SelectedIndex == -1 || sglx.SelectedIndex == -1 || kjlx.SelectedIndex == -1 || sf.SelectedIndex == -1 || wsdj.SelectedIndex == -1 || sgName.Text == "")
        {
            Ext.Msg.Alert("提示", "请填写完整信息!").Show();
            return;
        }
        if (action == "new")
        {
            try
            {
                Accidentcase gs = new Accidentcase
                {
                   Accidentname = sgName.Text,
                   AccidentLevelid = int.Parse(sgdj.SelectedItem.Value),
                   AccidentTypeid = int.Parse(sglx.SelectedItem.Value),

                   Deathnumber = int.Parse( swNumber.Text),
                   Zsnumber = int.Parse( zsNumber.Text),
                   Qsnumber = int.Parse( qsNumber.Text),
                   ZjLoss = int.Parse( zjjjss.Text ),
                   JjLoss = int.Parse( jjjjss.Text),
                   Provinces = int.Parse(sf.SelectedItem.Value),
                   Orename = kjName.Text,
                   OreTypeid = int.Parse(kjlx.SelectedItem.Value),
                   WsLevelid = int.Parse(wsdj.SelectedItem.Value),
                   Jtgs = int.Parse(jt.SelectedItem.Value),
                   Inpersonnumber = SessionBox.GetUserSession().PersonNumber,
                   Happendate = Convert.ToDateTime(Happentime.Value),
                   Indate = System.DateTime.Now,
                   Sgfx = sgfenxi.Text,
                   Sgjg = sgjg.Text

                };
                dc.Accidentcase.Insert(gs);
                dc.SubmitChanges();
                Ext.Msg.Alert("提示", "保存成功!").Show();
                Gridload();
                //清空事故名称
                sgName.Text = "";

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
                   var sg = dc.Accidentcase.First(p => p.Id == decimal.Parse(sm.SelectedRow.RecordID.Trim()));

                   sg.Accidentname = sgName.Text;
                   sg.AccidentLevelid = int.Parse(sgdj.SelectedItem.Value);
                   sg.AccidentTypeid = int.Parse(sglx.SelectedItem.Value);

                   sg.Deathnumber = int.Parse( swNumber.Text);
                   sg.Zsnumber = int.Parse( zsNumber.Text);
                   sg.Qsnumber = int.Parse( qsNumber.Text);
                   sg.ZjLoss = int.Parse( zjjjss.Text );
                   sg.JjLoss = int.Parse( jjjjss.Text);
                   sg.Provinces = int.Parse(sf.SelectedItem.Value);
                   sg.Orename = kjName.Text;
                   sg.OreTypeid = int.Parse(kjlx.SelectedItem.Value);
                   sg.WsLevelid = int.Parse(wsdj.SelectedItem.Value);
                   sg.Jtgs = int.Parse(jt.SelectedItem.Value);
                   sg.Happendate = Convert.ToDateTime(Happentime.Value);
                   sg.Sgfx = sgfenxi.Text;
                   sg.Sgjg = sgjg.Text;


                    //gs.Personnumber = cbbGsperson.SelectedItem.Value;
                    //gs.GsFact = gsss.Text;
                    //gs.GsLevelid = int.Parse(gsdj.SelectedItem.Value);
                    //gs.Happendate = Convert.ToDateTime(Happentime.Value);
                    //gs.Placeid = int.Parse(cbbplace.SelectedItem.Value);
                    //gs.PointsPer = int.Parse(grkf.Text);
                    //gs.FinePer = int.Parse(grfk.Text);
                    //gs.PointsDept = int.Parse(bmkf.Text);
                    //gs.FineDept = int.Parse(bmfk.Text);


                    dc.SubmitChanges();
                    Ext.Msg.Alert("提示", "修改成功!").Show();
                    Gridload();
                    //清空事故名称
                    sgName.Text = "";
                }
                catch
                {
                    Ext.Msg.Alert("提示", "保存失败，请稍候重试!").Show();
                }
            }
        }
    }
}
