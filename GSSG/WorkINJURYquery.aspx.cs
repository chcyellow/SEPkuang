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

public partial class GSSG_WorkINJURYquery : System.Web.UI.Page
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        bindGSDJ();
        if (!Ext.IsAjaxRequest)
        {

            
            Gridload();
            Hidden1.Value = "-1";
          //  btnQuery.Disabled = true;
            
        }
    }

    //绑定工伤等级
    private void bindGSDJ()
    {
        int gsdjID = int.Parse(PublicMethod.ReadXmlReturnNode("GSDJ", this));
        //string oracletext = "select * from CS_BASEINFOSET where INFOID!= " + zyID + " start with INFOID= " + zyID + "  connect by prior INFOID = FID order by INFOID asc";
        string oracletext = "select * from CS_BASEINFOSET WHERE FID= " + gsdjID + "  ";
        gsdjStore.DataSource = OracleHelper.Query(oracletext);
        gsdjStore.DataBind();
    }



    //[AjaxMethod]
    //public void PYsearch(string py, string store)
    //{
    //    if (py.Trim() == "")
    //    {
    //        return;
    //    }
    //    switch (store.Trim())
    //    {
    //        case "personStore":
    //            var person = from r in dc.Person
    //                         from d in dc.Department
    //                         where r.Deptid == d.Deptnumber && r.Maindeptid == SessionBox.GetUserSession().DeptNumber
    //                         && (r.Pinyin.ToLower().Contains(py.ToLower()) || r.Lightnumber.ToLower().Contains(py.ToLower()) || r.Name.Contains(py.Trim()))
    //                         select new
    //                         {
    //                             pernID = r.Personid,
    //                             pernName = r.Name,
    //                             pernLightNumber = r.Lightnumber,
    //                             persNnmber = r.Personnumber,
    //                             DeptName = d.Deptname
    //                         };
    //            personStore.DataSource = person;
    //            personStore.DataBind();
    //            break;
    //        case "placeStore":
    //            var place = from pl in dc.Place
    //                        where pl.Maindeptid == SessionBox.GetUserSession().DeptNumber
    //                        && (dc.F_PINYIN(pl.Placename).ToLower().Contains(py.ToLower()) || pl.Placename.Contains(py.Trim()))
    //                        select new
    //                        {
    //                            placID = pl.Placeid,
    //                            placName = pl.Placename
    //                        };
    //            placeStore.DataSource = place;
    //            placeStore.DataBind();
    //            break;


    //    }
    //}
    #region 明细模块
    [AjaxMethod]
    public void DetailLoad()//加载明细信息
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        BasePanel.Disabled = !SetSWbase(sm.SelectedRows[0].RecordID.Trim());

    }
    #endregion
    private bool SetSWbase(string ID)//加载工伤基本信息
    {
        try
        {
            var data = from gs in dc.Workinjury
                       from per in dc.Person
                       from dept in dc.Department
                       // from pos in dc.Post
                       from pl in dc.Place
                       from gslevel in dc.CsBaseinfoset
                       from inper in dc.Person
                       where gs.Personnumber == per.Personnumber && gs.Placeid == pl.Placeid && gs.GsLevelid == gslevel.Infoid && per.Areadeptid == dept.Deptnumber && inper.Personnumber == gs.Inpersonnumber && per.Maindeptid == SessionBox.GetUserSession().DeptNumber &&  gs.Id.ToString().Trim() == ID
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
                           per.Personnumber,
                           gs.Placeid,
                           gs.GsLevelid,
                           gs.Remarks

                       };
            foreach (var gs in data)
            {
                lbl_deptname.Text = gs.Deptname.ToString();
                lbl_Name.Text = gs.Name.ToString();
                lbl_ggdj.Text = gs.Infoname.ToString();
                lbl_ggms.Text = gs.GsFact.ToString();
                lbl_happydate.Text = gs.Happendate.Value.ToString();
                lbl_place.Text = gs.Placename.ToString();
                lbl_ygkf.Text = gs.PointsPer.ToString();
                lbl_kqkf.Text = gs.PointsDept.ToString();
                lbl_bmfk.Text = gs.FineDept.ToString();
                lbl_inputname.Text = gs.Indate.ToString();
                lbl_inputname.Text = gs.inPersonName.ToString();
                if (!string.IsNullOrEmpty(gs.Remarks))
                    bz.Text = gs.Remarks;
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
        //绑定工伤信息
        var data = from gs in dc.Workinjury
                   from per in dc.Person
                   from dept in dc.Department
                   // from pos in dc.Post
                   from pl in dc.Place
                   from gslevel in dc.CsBaseinfoset
                   from inper in dc.Person
                   where gs.Personnumber == per.Personnumber && gs.Placeid == pl.Placeid && gs.GsLevelid == gslevel.Infoid && per.Areadeptid == dept.Deptnumber && inper.Personnumber == gs.Inpersonnumber && per.Maindeptid == SessionBox.GetUserSession().DeptNumber 
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
                       per.Personnumber,
                       gs.Placeid,
                       gs.GsLevelid

                      
                   };
        if (!df_begin.IsNull && !df_end.IsNull)
        {
            data = data.Where(p => p.Happendate >= df_begin.SelectedDate && p.Happendate <= df_end.SelectedDate);
        }
        if (cbbGsperson.SelectedIndex > -1)
        {
            data = data.Where(p => p.Personnumber == cbbGsperson.SelectedItem.Value);
 
        }
        if (cbbplace.SelectedIndex > -1)
        {
            data = data.Where(p => p.Placeid == int.Parse( cbbplace.SelectedItem.Value));
 
        }
        if (gsdj.SelectedIndex > -1)
        {
            data = data.Where(p => p.GsLevelid == int.Parse(gsdj.SelectedItem.Value));
 
        }
        if (gsss.Text != "")
        {
            data = data.Where(p => p.GsFact.Contains(gsss.Text));
 
        }

        GSStore.DataSource = data;
        GSStore.DataBind();

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
        cbbGsperson.SelectedItem.Value = null;
        gsdj.SelectedItem.Value = null;

        cbbplace.SelectedItem.Value = null;
       
        gsss.Text = "";
       
    }
}
