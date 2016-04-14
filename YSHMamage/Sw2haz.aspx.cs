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

public partial class YSHMamage_Sw2haz : System.Web.UI.Page
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            baseload();
            StoreLoad();
        }
    }

    private void baseload()
    {
        #region 初始化级别
        var lavel = from c in dc.CsBaseinfoset
                    where c.Fid == int.Parse(PublicMethod.ReadXmlReturnNode("SWJB", this))
                    select new
                    {
                        YHLevelID = c.Infoid,
                        YHLevel = c.Infoname
                    };
        LevelStore.DataSource = lavel;
        LevelStore.DataBind();
        #endregion

        #region 初始化类型
        var type = from t in dc.CsBaseinfoset
                   where t.Fid == int.Parse(PublicMethod.ReadXmlReturnNode("ZY", this))
                   orderby t.Infoid ascending
                   select new
                   {

                       YHTypeID = t.Infoid,
                       YHType = t.Infoname
                   };
        TypeStore.DataSource = type;
        TypeStore.DataBind();
        #endregion

        BindStore(int.Parse(PublicMethod.ReadXmlReturnNode("FXLX", this)), FXStore);
    }

    private void BindStore(int fid, Store store)
    {
        string oracletext = "select infoid,infoname from CS_BASEINFOSET where FID = " + fid + " order by infoid";
        store.DataSource = OracleHelper.Query(oracletext);
        store.DataBind();
    }


    private void StoreLoad()
    {
        var data = from yh in dc.Swbase
                   from c in dc.CsBaseinfoset
                   from d in dc.CsBaseinfoset
                   where yh.Levelid == c.Infoid && yh.Typeid == d.Infoid && yh.Nstatus == 2
                   select new
                   {
                       yh.Swid,
                       yh.Swnumber,
                       yh.Swcontent,
                       yh.Levelid,
                       Levelname = c.Infoname,
                       yh.Typeid,
                       Typename = d.Infoname,
                       yh.Conpyfirst,
                       yh.Intime,
                       yh.Nstatus,
                       y2h = dc.Shmatchup.Count(ym => ym.Swid == yh.Swid) == 0 ? "否" : "是"
                   };
        if (df_begin.SelectedValue != null)
        {
            data = data.Where(p => p.Intime >= df_begin.SelectedDate);
        }
        if (df_end.SelectedValue != null)
        {
            data = data.Where(p => p.Intime <= df_end.SelectedDate);
        }
        if (cbb_lavel.SelectedIndex > -1)
        {
            data = data.Where(p => p.Levelid == decimal.Parse(cbb_lavel.SelectedItem.Value));
        }
        if (cbb_kind.SelectedIndex > -1)
        {
            data = data.Where(p => p.Typeid == decimal.Parse(cbb_kind.SelectedItem.Value));
        }
        if (tf_Yhconcent.Text.Trim() != "")
        {
            data = data.Where(p => p.Swcontent.Contains(tf_Yhconcent.Text.Trim()));
        }
        //data = data.Where(p => p.y2h == "是");
        YHStore.DataSource = data;
        YHStore.DataBind();
    }

    [AjaxMethod]
    public void Search()
    {
        StoreLoad();
    }

    [AjaxMethod]
    public void hazLoad()
    {
        SearchBLLoad();
        hazDetailWindow.Show();
    }

    [AjaxMethod]
    public void RowClick()
    {
        RowSelectionModel sm = GridPanel3.SelectionModel.Primary as RowSelectionModel;
        btnAction.Disabled = (sm.SelectedRows.Count > 0);
    }

    [AjaxMethod]
    public void hazDel()
    {
        RowSelectionModel sm = gpSearchBL.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count == 0)
        {
            Ext.Msg.Alert("提示", "请选择删除危险源！").Show();
            return;
        }
        RowSelectionModel sm1 = GridPanel3.SelectionModel.Primary as RowSelectionModel;
        var yh = dc.Swbase.First(p => p.Swid == decimal.Parse(sm1.SelectedRow.RecordID));
        foreach (var r in sm.SelectedRows)
        {
            DBSCMDataContext dc1 = new DBSCMDataContext();
            var y2h = dc1.Shmatchup.First(p => p.Swid == yh.Swid && p.Hazardsid == decimal.Parse(r.RecordID));

            dc1.Shmatchup.DeleteOnSubmit(y2h);
            dc1.SubmitChanges();
        }
        Ext.Msg.Alert("提示", "删除成功，共计删除" + sm.SelectedRows.Count.ToString() + "条危险源!").Show();
        SearchBLLoad();
    }

    private void SearchBLLoad()
    {
        RowSelectionModel sm = GridPanel3.SelectionModel.Primary as RowSelectionModel;
        var haz = from ym in dc.Shmatchup
                  from h in dc.Hazards
                  from p in dc.Process
                  from w in dc.Worktasks
                  from a in dc.CsBaseinfoset
                  from b in dc.CsBaseinfoset
                 // from c in dc.CsBaseinfoset
                  where ym.Hazardsid == h.Hazardsid && ym.Swid == decimal.Parse(sm.SelectedRow.RecordID) &&
                  h.Processid == p.Processid && p.Worktaskid == w.Worktaskid && w.Professionalid == a.Infoid && h.RiskTypesnumber == b.Infoid //&& h.AccidentTypenumber == c.Infoid
                  select new
                  {
                      h.Hazardsid,
                      h.HNumber,
                      Gxname = p.Name,
                      Zyid = w.Professionalid,
                      Zyname = a.Infoname,
                      Gzrwname = w.Worktask,
                      Fxlxid = h.RiskTypesnumber,
                      Fxlx = b.Infoname,
                      h.HContent,
                      h.HConsequences,
                      h.MStandards
                  };
        y2hStore.DataSource = haz;
        y2hStore.DataBind();

    }

    protected void hazStoreRefresh(object sender, StoreRefreshDataEventArgs e)
    {
        hazdetailLoad();
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
    public void ClearBL()
    {
        RowSelectionModel sm = GridPanel3.SelectionModel.Primary as RowSelectionModel;
        var yh = dc.Yhbase.First(p => p.Yhid == decimal.Parse(sm.SelectedRow.RecordID));
        cbbSbsdy.SelectedItem.Value = yh.Typeid.ToString();
        cbbSfxlx.SelectedItem.Value = null;
        cbbSgzrw.SelectedItem.Value = null;
        tfSyhms.Text = "";
    }

    [AjaxMethod]
    public void hazdetailLoad()
    {

        var haz = from h in dc.Hazards
                  from p in dc.Process
                  from w in dc.Worktasks
                  from a in dc.CsBaseinfoset
                  from b in dc.CsBaseinfoset
                  //from c in dc.CsBaseinfoset
                  where w.Professionalid == decimal.Parse(cbbSbsdy.SelectedItem.Value) &&
                  h.Processid == p.Processid && p.Worktaskid == w.Worktaskid && w.Professionalid == a.Infoid && h.RiskTypesnumber == b.Infoid //&& h.AccidentTypenumber == c.Infoid
                  select new
                  {
                      h.Hazardsid,
                      h.HNumber,
                      Gxname = p.Name,
                      Zyid = w.Professionalid,
                      Zyname = a.Infoname,
                      Gzrwid = w.Worktaskid,
                      Gzrwname = w.Worktask,
                      Fxlxid = h.RiskTypesnumber,
                      Fxlx = b.Infoname,
                      h.HContent,
                      h.HConsequences,
                      h.MStandards
                  };
        if (cbbSgzrw.SelectedIndex > -1)
        {
            haz = haz.Where(p => p.Gzrwid == decimal.Parse(cbbSgzrw.SelectedItem.Value));
        }
        if (cbbSfxlx.SelectedIndex > -1)
        {
            haz = haz.Where(p => p.Fxlxid == decimal.Parse(cbbSfxlx.SelectedItem.Value));
        }
        if (tfSyhms.Text.Trim() != "")
        {
            haz = from y in haz
                  where y.HContent.Contains(tfSyhms.Text.Trim())
                  select y;
        }
        hazStore.DataSource = haz;
        hazStore.DataBind();
    }

    [AjaxMethod]
    public void AddHaz()
    {
        RowSelectionModel sm = GridPanel1.SelectionModel.Primary as RowSelectionModel;
        RowSelectionModel sm1 = GridPanel3.SelectionModel.Primary as RowSelectionModel;
        var yh = dc.Swbase.First(p => p.Swid == decimal.Parse(sm1.SelectedRow.RecordID));
        if (sm.SelectedRows.Count() > 0)
        {
            int i = 0;
            foreach (var r in sm.SelectedRows)
            {
                if (dc.Shmatchup.Count(p => p.Swid == yh.Swid && p.Hazardsid == decimal.Parse(r.RecordID)) == 0)
                {
                    Shmatchup ym = new Shmatchup
                    {
                        Swid = yh.Swid,
                        Hazardsid = decimal.Parse(r.RecordID)
                    };
                    DBSCMDataContext dc1 = new DBSCMDataContext();
                    dc1.Shmatchup.InsertOnSubmit(ym);
                    dc1.SubmitChanges();
                    i++;
                }
            }
            Ext.Msg.Alert("提示", "添加成功，共计添加" + i.ToString() + "条危险源!").Show();
            SearchBLLoad();
            hazWindow.Hide();
        }
    }
}
