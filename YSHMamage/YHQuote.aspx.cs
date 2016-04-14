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

public partial class YSHMamage_YHQuote : System.Web.UI.Page
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

    #region 数据加载
    private void baseload()
    {
        #region 初始化级别
        var lavel = from c in dc.CsBaseinfoset
                    where c.Fid == int.Parse(PublicMethod.ReadXmlReturnNode("YHJB", this))
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

    }


    private void StoreLoad()
    {
        var datayh = from yh in dc.Yhbase
                   from d in dc.CsBaseinfoset
                   where yh.Typeid == d.Infoid && yh.Nstatus == 2
                   select new
                   {
                       yh.Yhid,
                       yh.Yhnumber,
                       yh.Yhcontent,
                       yh.Levelid,
                       yh.Typeid,
                       Typename = d.Infoname,
                       yh.Conpyfirst,
                       yh.Intime,
                       yh.Nstatus
                   };
        var data2 = from d in datayh
                   join yq in dc.Yhquote.Where(p => p.Deptnumber == SessionBox.GetUserSession().DeptNumber) on d.Yhid equals yq.Yhid into gg
                   from g in gg.DefaultIfEmpty()
                   select new
                   {
                       d.Yhid,
                       d.Yhnumber,
                       d.Yhcontent,
                       Levelid=g == null ?d.Levelid:g.Levelid,
                       d.Typeid,
                       d.Typename,
                       d.Conpyfirst,
                       Intime = g.Quotetime,
                       Nstatus = g == null ? 2 : g.Nstatus
                   };

        var data=from d in data2
                 from c in dc.CsBaseinfoset
                 where d.Levelid==c.Infoid
                 select new
                 {
                     d.Yhid,
                     d.Yhnumber,
                     d.Yhcontent,
                     d.Levelid,
                     Levelname = c.Infoname,
                     d.Typeid,
                     d.Typename,
                     d.Conpyfirst,
                     d.Intime,
                     d.Nstatus
                 };

        if (cbb_lavel.SelectedIndex > -1)
        {
            data = data.Where(p => p.Levelid == decimal.Parse(cbb_lavel.SelectedItem.Value));
        }
        if (cbb_kind.SelectedIndex > -1)
        {
            data = data.Where(p => p.Typeid == decimal.Parse(cbb_kind.SelectedItem.Value));
        }
        YHStore.DataSource = data;
        YHStore.DataBind();
    }
    #endregion

    [AjaxMethod]
    public void Search()
    {
        StoreLoad();
    }

    #region 点击行事件
    protected void RowClick(object sender, AjaxEventArgs e)
    {
        RowSelectionModel sm = GridPanel3.SelectionModel.Primary as RowSelectionModel;
        btnAction.Disabled = (sm.SelectedRows.Count > 0);
        if (sm.SelectedRows.Count > 0)
        {
            var yb = dc.Yhquote.Where(p => p.Yhid == decimal.Parse(sm.SelectedRow.RecordID) && p.Deptnumber == SessionBox.GetUserSession().DeptNumber);
            if (yb.Count() > 0)
            {
                if (yb.First().Nstatus == 1)
                {
                    btnquote.Disabled = true;
                    btndel.Disabled = false;
                    btnAction.Disabled = false;
                }
                else
                {
                    btnquote.Disabled = false;
                    btndel.Disabled = true;
                    btnAction.Disabled = true;
                }
            }
            else
            {
                btnquote.Disabled = false;
                btndel.Disabled = true;
                btnAction.Disabled = true;
            }
        }
        else
        {
            btnquote.Disabled = false;
            btndel.Disabled = true;
            btnAction.Disabled = true;
        }
    }

    [AjaxMethod]
    public void detailDet(string id)
    {
        hdnID.SetValue(id);
    }

    #endregion

    #region 修改
    protected void btnUpdateClick(object sender, AjaxEventArgs e)
    {
        if (tfYhcontent.Text.Trim() == "" || cbbLevelid.SelectedIndex == -1)
        {
            Ext.Msg.Alert("提示", "请填写完整信息!").Show();
            return;
        }
        try
        {
            if (hdnID.Value.ToString() != "-1")
            {
                var yb = dc.Yhquote.First(p => p.Yhid == decimal.Parse(hdnID.Value.ToString()) && p.Deptnumber == SessionBox.GetUserSession().DeptNumber);
                yb.Levelid = decimal.Parse(cbbLevelid.SelectedItem.Value);
                //yb.Conpyfirst = dc.F_PINYIN(tfYhcontent.Text).ToLower();
                dc.SubmitChanges();
                StoreLoad();
            }
            Ext.Msg.Alert("提示", "更新成功!").Show();
        }
        catch
        {
            Ext.Msg.Alert("提示", "请先引用!").Show();
        }
    }
    #endregion

    #region 作废
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
            var yb = dc.Yhquote.First(p => p.Yhid == decimal.Parse(sm.SelectedRow.RecordID) && p.Deptnumber == SessionBox.GetUserSession().DeptNumber);
            yb.Nstatus = 0;
            dc.SubmitChanges();
            StoreLoad();
            Ext.Msg.Alert("提示", "作废成功!").Show();
        }
    }
    #endregion

    #region 引用
    [AjaxMethod]
    public void yhquote()
    {
        Ext.Msg.Confirm("提示", "是否确定引用?", new MessageBox.ButtonsConfig
        {
            Yes = new MessageBox.ButtonConfig
            {
                Handler = "Coolite.AjaxMethods.Doquote();",
                Text = "确 定"
            },
            No = new MessageBox.ButtonConfig
            {
                Text = "取 消"
            }
        }).Show();
    }

    [AjaxMethod]
    public void Doquote()
    {
        RowSelectionModel sm = GridPanel3.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            var yb = dc.Yhquote.Where(p => p.Yhid == decimal.Parse(sm.SelectedRow.RecordID) && p.Deptnumber == SessionBox.GetUserSession().DeptNumber);
            if (yb.Count()>0)
            {
                yb.First().Nstatus = 1;
                dc.SubmitChanges();
                
            }
            else
            {
                var yh = dc.Yhbase.First(p => p.Yhid == decimal.Parse(sm.SelectedRow.RecordID));
                Yhquote yq = new Yhquote
                {
                    Yhid = decimal.Parse(sm.SelectedRow.RecordID),
                    Levelid = yh.Levelid,
                    Deptnumber = SessionBox.GetUserSession().DeptNumber,
                    Nstatus = 1,
                    Quotetime = System.DateTime.Today
                };
                dc.Yhquote.InsertOnSubmit(yq);
                dc.SubmitChanges();
            }
            StoreLoad();
            Ext.Msg.Alert("提示", "引用成功!").Show();
        }
    }
    #endregion
}