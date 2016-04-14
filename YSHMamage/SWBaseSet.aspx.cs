using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Coolite.Ext.Web;
using GhtnTech.SEP.DAL;
using GhtnTech.SecurityFramework.BLL;

public partial class YSHMamage_SWBaseSet : System.Web.UI.Page
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

        #region 初始化事故类型
        var sg = from t in dc.CsBaseinfoset
                 where t.Fid == int.Parse(PublicMethod.ReadXmlReturnNode("SGLX", this))
                 orderby t.Infoid ascending
                 select new
                 {

                     t.Infoid,
                     t.Infoname
                 };
        SGStore.DataSource = sg;
        SGStore.DataBind();
        #endregion
    }

    private void StoreLoad()
    {
        var data1 = from yh in dc.Swbase
                   from c in dc.CsBaseinfoset
                   from d in dc.CsBaseinfoset
                   where yh.Levelid == c.Infoid && yh.Typeid == d.Infoid
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
                       yh.Sglxid
                   };

        var data = from d in data1
                   join e in dc.CsBaseinfoset on d.Sglxid equals e.Infoid into gg
                   from g in gg.DefaultIfEmpty()
                   select new
                   {
                       d.Swid,
                       d.Swnumber,
                       d.Swcontent,
                       d.Levelid,
                       d.Levelname,
                       d.Typeid,
                       d.Typename,
                       d.Conpyfirst,
                       d.Intime,
                       d.Nstatus,
                       d.Sglxid,
                       Sglxname = g.Infoname == null ? "" : g.Infoname
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
        if (cbb_status.SelectedIndex > -1)
        {
            data = data.Where(p => p.Nstatus == decimal.Parse(cbb_status.SelectedItem.Value));
        }
        if (cbb_SG.SelectedIndex > -1)
        {
            data = data.Where(p => p.Sglxid == decimal.Parse(cbb_SG.SelectedItem.Value));
        }
        YHStore.DataSource = data;
        YHStore.DataBind();
    }

    [AjaxMethod]
    public void Search()
    {
        StoreLoad();
    }

    [AjaxMethod]
    public void detailDet(string id)
    {
        hdnID.SetValue(id);
    }

    #region 数据提交

    protected void btnUpdateClick(object sender, AjaxEventArgs e)
    {
        if (tfYhcontent.Text.Trim() == "" || cbbLevelid.SelectedIndex == -1 || cbbTypeid.SelectedIndex == -1)
        {
            Ext.Msg.Alert("提示", "请填写完整信息!").Show();
            return;
        }
        if (hdnID.Value.ToString() == "-1")
        {
            Swbase yb = new Swbase
            {
                Swcontent = tfYhcontent.Text,
                Levelid = decimal.Parse(cbbLevelid.SelectedItem.Value),
                Typeid = decimal.Parse(cbbTypeid.SelectedItem.Value),
                Intime = System.DateTime.Now,
                Nstatus = 1,
                Sglxid=decimal.Parse(cbbSglxid.SelectedItem.Value)
                //Conpyfirst = dc.F_PINYIN(tfYhcontent.Text)
            };
            dc.Swbase.InsertOnSubmit(yb);
            dc.SubmitChanges();
            StoreLoad();
        }
        else
        {
            var yb = dc.Swbase.First(p => p.Swid == decimal.Parse(hdnID.Value.ToString()));
            yb.Swcontent = tfYhcontent.Text;
            yb.Typeid = decimal.Parse(cbbTypeid.SelectedItem.Value);
            yb.Levelid = decimal.Parse(cbbLevelid.SelectedItem.Value);
            yb.Sglxid = decimal.Parse(cbbSglxid.SelectedItem.Value);
            //yb.Conpyfirst = dc.F_PINYIN(tfYhcontent.Text).ToLower();
            dc.SubmitChanges();
            StoreLoad();
        }
        Ext.Msg.Alert("提示", "更新成功!").Show();
    }
    #endregion

    [AjaxMethod]
    public void yhpublish()
    {
        RowSelectionModel sm = GridPanel3.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            var yb = dc.Swbase.First(p => p.Swid == decimal.Parse(sm.SelectedRow.RecordID));
            yb.Nstatus = 2;
            yb.Swnumber = GetYhNumber(yb.Levelid.ToString(), yb.Typeid.ToString());
            dc.SubmitChanges();
            StoreLoad();
            Ext.Msg.Alert("提示", "发布成功!").Show();
        }
    }

    private string GetYhNumber(string jbid, string zyid)
    {
        return dc.CsBaseinfoset.First(p => p.Infoid == int.Parse(zyid)).Infocode + 
            dc.CsBaseinfoset.First(p => p.Infoid == decimal.Parse(jbid)).Infocode + 
            PublicMethod.GetYSNO(jbid, zyid);
    }

    [AjaxMethod]
    public void yhdel()
    {
        Ext.Msg.Confirm("提示", "是否确定删除?", new MessageBox.ButtonsConfig
        {
            Yes = new MessageBox.ButtonConfig
            {
                Handler = "Coolite.AjaxMethods.Dodel();",
                Text = "确 定"
            },
            No = new MessageBox.ButtonConfig
            {
                Text = "取 消"
            }
        }).Show();
    }

    [AjaxMethod]
    public void Dodel()
    {
        RowSelectionModel sm = GridPanel3.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            var yb = dc.Swbase.First(p => p.Swid == decimal.Parse(sm.SelectedRow.RecordID));
            if (yb.Nstatus.Value == 1)
            {
                dc.Swbase.DeleteOnSubmit(yb);
                dc.SubmitChanges();
            }
            else
            {
                yb.Nstatus = 3;
                dc.SubmitChanges();
            }
            StoreLoad();
            Ext.Msg.Alert("提示", "作废成功!").Show();
        }
    }

    protected void RowClick(object sender, AjaxEventArgs e)//流程处理
    {
        RowSelectionModel sm = GridPanel3.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            var yb = dc.Swbase.First(p => p.Swid == decimal.Parse(sm.SelectedRow.RecordID));
            switch (int.Parse(yb.Nstatus.ToString()))
            {
                case 1:
                    Button2.Disabled = false;
                    Button3.Disabled = false;
                    btnpublish.Disabled = false;
                    break;
                case 2:
                    Button2.Disabled = false;
                    Button3.Disabled = false;
                    btnpublish.Disabled = true;
                    break;
                case 3:
                    Button2.Disabled = true;
                    Button3.Disabled = true;
                    btnpublish.Disabled = true;
                    break;
            }
        }
        else
        {
            Button2.Disabled = true;
            Button3.Disabled = true;
            btnpublish.Disabled = true;
        }
    }
}
