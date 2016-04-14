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

public partial class GSSG_AccidentQuery : System.Web.UI.Page
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        bindINFO();
        if (!Ext.IsAjaxRequest)
        {

            Gridload();
            Hidden1.Value = "-1";
         
        }

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

    #region 明细模块
    [AjaxMethod]
    public void DetailLoad()//加载明细信息
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        BasePanel.Disabled = !SetSWbase(sm.SelectedRows[0].RecordID.Trim());

    }
    #endregion
    private bool SetSWbase(string ID)//加载事故基本信息
    {
        try
        {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        var sg = dc.Accidentcase.First(p => p.Id == decimal.Parse(ID));
        if (sm.SelectedRows.Count > 0)
        {


            TextField1.Text = sg.Accidentname;
            sgdj.SelectedItem.Value = sg.AccidentLevelid.ToString();
            sglx.SelectedItem.Value = sg.AccidentTypeid.ToString();

            NumberField1.Text = sg.Deathnumber.ToString();
            NumberField2.Text = sg.Zsnumber.ToString();
            NumberField3.Text = sg.Qsnumber.ToString();
            NumberField4.Text = sg.ZjLoss.ToString();
            NumberField5.Text = sg.JjLoss.ToString();
            sf.SelectedItem.Value = sg.Provinces.ToString();
            TextField2.Text = sg.Orename;
            kjlx.SelectedItem.Value = sg.OreTypeid.ToString();
            wsdj.SelectedItem.Value = sg.WsLevelid.ToString();
            jt.SelectedItem.Value = sg.Jtgs.ToString();
            Happentime.Value = sg.Happendate;
            sgfenxi.Text = sg.Sgfx;
            sgjg.Text = sg.Sgjg;
        }


        return true;
        }
        catch
        {
            return false;
        }
    }

    [AjaxMethod]
    public void Gridload()
    {
        //绑定事故信息
        var data = from sg in dc.Accidentcase
                   from sf in dc.CsBaseinfoset
                   from jt in dc.CsBaseinfoset
                   from sglx in dc.CsBaseinfoset
                   from sgdj in dc.CsBaseinfoset
                   from wsdj in dc.CsBaseinfoset
                   from kjlx in dc.CsBaseinfoset
                   from inper in dc.Person
                   where sg.Provinces == sf.Infoid && sg.Jtgs == jt.Infoid && sg.AccidentTypeid == sglx.Infoid && sg.AccidentLevelid == sgdj.Infoid && sg.WsLevelid == wsdj.Infoid && sg.OreTypeid == kjlx.Infoid && inper.Personnumber == sg.Inpersonnumber 
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

                       sf = sf.Infoname,
                       jt = jt.Infoname,
                       sglx = sglx.Infoname,
                       sgdj = sgdj.Infoname,
                       wsdj = wsdj.Infoname,
                       kjlx = kjlx.Infoname,

                       sfid = sf.Infoid,
                       jtid = jt.Infoid,
                       sglxid = sglx.Infoid,
                       sgdjid = sgdj.Infoid,
                       wsdjid = wsdj.Infoid,
                       kjlxid = kjlx.Infoid,


                       sg.Happendate,
                       inper.Name,
                       sg.Indate

                   };
        if (!df_begin.IsNull && !df_end.IsNull)
        {
            data = data.Where(p => p.Happendate >= df_begin.SelectedDate && p.Happendate <= df_end.SelectedDate);
        }
        if (sgName.Text != "")
        {
            data = data.Where(p => p.Accidentname == sgName.Text);
        }
        if (kjName.Text != "")
        {
            data = data.Where(p => p.Orename == kjName.Text);
        }
        if (sf1.SelectedIndex > -1)
        {
            data = data.Where(p => p.sfid == int.Parse(sf1.SelectedItem.Value));
        }
        if (jt1.SelectedIndex > -1)
        {
            data = data.Where(p => p.jtid == int.Parse(jt1.SelectedItem.Value));
        }
        if (sglx1.SelectedIndex > -1)
        {
            data = data.Where(p => p.sglxid == int.Parse(sglx1.SelectedItem.Value));
        }
        if (sgdj1.SelectedIndex > -1)
        {
            data = data.Where(p => p.sgdjid == int.Parse(sgdj1.SelectedItem.Value));
        }
        if (wsdj1.SelectedIndex > -1)
        {
            data = data.Where(p => p.wsdjid == int.Parse(wsdj1.SelectedItem.Value));
        }
        if (kjlx1.SelectedIndex > -1)
        {
            data = data.Where(p => p.kjlxid == int.Parse(kjlx1.SelectedItem.Value));
        }

        if (swNumber.Text != "" && swNumber1.Text != "")
        {
            data = data.Where(p => p.Deathnumber >= int.Parse(swNumber.Text) && p.Deathnumber <= int.Parse(swNumber1.Text));
        }
        if (zsNumber.Text != "" && zsNumber1.Text != "")
        {
            data = data.Where(p => p.Zsnumber >= int.Parse(zsNumber.Text) && p.Zsnumber <= int.Parse(zsNumber1.Text));
        }
        if (qsNumber.Text != "" && qsNumber1.Text != "")
        {
            data = data.Where(p => p.Qsnumber >= int.Parse(qsNumber.Text) && p.Qsnumber <= int.Parse(qsNumber1.Text));
        }
        if (zjjjss.Text != "" && zjjjss1.Text != "")
        {
            data = data.Where(p => p.ZjLoss >= int.Parse(zjjjss.Text) && p.ZjLoss <= int.Parse(zjjjss1.Text));
        }
        if (jjjjss.Text != "" && jjjjss1.Text != "")
        {
            data = data.Where(p => p.JjLoss >= int.Parse(jjjjss.Text) && p.JjLoss <= int.Parse(jjjjss1.Text));
        }

        SGStore.DataSource = data;
        SGStore.DataBind();

    }
    [AjaxMethod]
    public void Search()
    {
        //SearchLoad();
        Gridload();
        Window1.Hide();
        GridPanel1.Show();
    }

    [AjaxMethod]
    public void ClearSearch()//清除条件
    {
        df_begin.Clear();
        df_end.Clear();
        sgName.Text = "";
        kjName.Text = "";
        sf1.SelectedItem.Value = null;
        jt1.SelectedItem.Value = null;
        sglx1.SelectedItem.Value = null;
        sgdj1.SelectedItem.Value = null;
        wsdj1.SelectedItem.Value = null;
        kjlx1.SelectedItem.Value = null;
        swNumber.Text = "";
        swNumber1.Text = "";
        zsNumber.Text = "" ;
        zsNumber1.Text = "";
        qsNumber.Text = "" ;
        qsNumber1.Text = "";
        zjjjss.Text = "" ;
        zjjjss1.Text = "";
        jjjjss.Text = "" ;
        jjjjss1.Text = "";
    }
}
