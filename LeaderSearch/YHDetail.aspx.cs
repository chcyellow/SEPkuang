using System;
using System.Collections;
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
using System.Xml.Linq;
using System.Data;
public partial class YHDetail : BasePage
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        DetailLoad();
    }
    public void DetailLoad()//加载明细信息
    {
        int i = int.Parse( Request.QueryString["i"].ToString());
        
        string id =  Request.QueryString["id"].ToString();
        var input = dc.Nyhinput.First(p => p.Yhputinid == Convert.ToInt32(id));
        if (i > 0)
        {
            SetYHbase(id);
        }
        BasePanel.Disabled = i > 0 ? false : true; i--;
        if (i > 0)
        {
            SetYHZG(id);
        }
        Panel1.Disabled = i > 0 ? false : true; i--;
        if (i > 0)
        {
            SetYHZGFK(id);
        }
        ZGPanel.Disabled = i > 0 ? false : true; i--;
        if (i > 0)
        {
            SetYHFCFK(id);
        }
        FCPanel.Disabled = i > 0 ? false : true; i--;
        try
        {
            CFPanel.Disabled = input.Isfine.Value == 1 ? false : true;
        }
        catch
        {
            CFPanel.Disabled = true;
        }
        Panel1.Collapsed = true;
        ZGPanel.Collapsed = true;
        FCPanel.Collapsed = true;
        CFPanel.Collapsed = true;
    }

    private void SetYHbase(string ID)//加载隐患基本信息
    {
        var data = from yh in dc.Getyhinput
                   where yh.Yhputinid == decimal.Parse(ID)
                   select new
                   {
                       yh.Yhputinid,
                       yh.Deptname,
                       yh.Placename,
                       HBm = yh.Yhcontent,
                       yh.Remarks,
                       yh.Banci,
                       Personname = yh.Name,
                       yh.Pctime,
                       yh.Levelname,
                       Zyname = yh.Typename,
                       yh.Status,
                       yh.Jctype,
                       yh.Isfine
                   };
        foreach (var r in data)
        {
            lbl_YHPutinID.Text = r.Yhputinid.ToString();
            lbl_DeptName.Text = r.Deptname.Trim();
            lbl_PlaceName.Text = r.Placename.Trim();
            lbl_YHContent.Text = r.HBm.Trim();
            try
            {
                lbl_Remarks.Text = r.Remarks.Trim();
            }
            catch
            {
                lbl_Remarks.Text = "";
            }
            lbl_BanCi.Text = r.Banci.Trim();
            lbl_rName.Text = r.Personname;
            lbl_PCTime.Text = r.Pctime.Value.ToString("yyyy年MM月dd日");
            lbl_YHLevel.Text = r.Levelname.Trim();
            lbl_YHType.Text = r.Zyname.Trim();
            lbl_Status.Text = r.Status.Trim();
        }
        //罚款信息
        string msg = "";
        if (data.First().Isfine.Value == 1)
        {
            var fk = from nd in dc.Nyhfinedetail
                     from l in dc.Objectset
                     from p in dc.Person
                     from yh in dc.Nyhinput
                     from f in dc.Fineset
                     where nd.Pid == l.Pid && nd.Personnumber == p.Personnumber && nd.Yinputid == decimal.Parse(ID)
                     && nd.Yinputid == yh.Yhputinid && yh.Levelid == f.Levelid && nd.Pid == f.Pid
                     select new
                     {
                         l.Pname,
                         p.Name,
                         fine = yh.Jctype == 0 ? f.Kcfine : f.Zcfine
                     };

            foreach (var r in fk)
            {
                msg += "<b>" + r.Pname + "：</b>" + r.Name + "(" + r.fine + "元)";
            }

            //zg_IsFine.Text = fk.Sum(p => p.fine).ToString() + "元";
        }
        lbl_fkxx.Html = msg == "" ? "无" : msg;
    }

    private void SetYHZG(string ID)//加载隐患整改信息
    {
        GhtnTech.SEP.OraclDAL.DALGetYHINFO yh = new GhtnTech.SEP.OraclDAL.DALGetYHINFO();
        DataTable dt = yh.GetNYHZGbyID(ID).Tables[0];
        foreach (DataRow r in dt.Rows)
        {
            zg_Measures.Text = r["MEASURES"].ToString().Trim();
            zg_Instructions.Text = r["INSTRUCTIONS"].ToString().Trim();
            zg_PersonID.Text = r["RNAME"].ToString().Trim();
            zg_zrdw.Text = r["DEPTNAME"].ToString().Trim();
            zg_InstrTime.Text = Convert.ToDateTime(r["INSTRTIME"]).ToString("yyyy年MM月dd日");
            //zg_IsFine.Text = r.IsFine.Value ? "是" : "否";
            //---------------------****---------------------
            zg_RecLimit.Text = Convert.ToDateTime(r["RECLIMIT"]).ToString("yyyy年MM月dd日");//
            zg_BanCi.Text = r["BANCI"].ToString().Trim();//
            //zg_ReviewLimit.Text = r.ReviewLimit.Value.ToString() + "天";
            //------------------------------------------
        }
        //var fine = dc.Nyhaction.Where(p => p.Yhputinid == Convert.ToInt32(ID)).Sum(f => f.Fine);
        //zg_IsFine.Text = Decimal.Round(fine.Value, 2).ToString() + "元";
    }

    private void SetYHZGFK(string ID)//加载隐患整改反馈信息
    {
        GhtnTech.SEP.OraclDAL.DALGetYHINFO yh = new GhtnTech.SEP.OraclDAL.DALGetYHINFO();
        DataTable dt = yh.GetNYHZGFKbyID(ID).Tables[0];
        foreach (DataRow r in dt.Rows)
        {
            //---------------------****---------------------
            //zfk_RecOpinion.Text = r.RecOpinion.Trim();
            zfk_RecPersonID.Text = r["RECPERSON"].ToString().Trim();
            zfk_RecTime.Text = Convert.ToDateTime(r["RECTIME"]).ToString("yyyy年MM月dd日");
            zfk_RecState.Text = r["RECSTATE"].ToString().Trim();
            zfk_yanshouName.Text = r["YANSHOUNAME"].ToString().Trim();
            zfk_bc.Text = r["ZGBANCI"].ToString().Trim();
            //zfk_ResponsibleID.Text = r.Responsible.Trim();
            //------------------------------------------
        }
    }

    private void SetYHFCFK(string ID)//加载隐患复查反馈信息
    {
        var data = from r in dc.Nyinhuanreview
                   from p in dc.Person
                   where r.Personid == p.Personnumber && r.Yhputinid == int.Parse(ID)
                   select new
                   {
                       r.Reviewopinion,
                       r.Fctime,
                       r.Reviewstate,
                       p.Name
                   };

        foreach (var r in data)
        {
            ffk_ReviewOpinion.Text = r.Reviewopinion.Trim();
            ffk_PersonID.Text = r.Name.Trim();
            ffk_FCTime.Text = r.Fctime.Value.ToString("yyyy年MM月dd日");
            ffk_ReviewState.Text = r.Reviewstate.Trim();
        }
    }
}
