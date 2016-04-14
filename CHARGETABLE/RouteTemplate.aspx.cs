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
public partial class CHARGETABLE_RouteTemplate : BasePage
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            UpdateStatus();
        }
    }
    [AjaxMethod]
    public void AddTemplate()
    {
        TID.Value = "";
        txtTName.Text = "";
        if (SessionBox.GetUserSession().Role.Contains("11,矿级管理员"))
        {
            rKuangJi.Disabled = false;
            rKuangJi.Checked = true;
        }
        else
        {
            rKuangJi.Disabled = true;
            rGeRen.Checked = true;
        }
        Window1.Show();
    }
    [AjaxMethod]
    public void UpdateTemplate()
    {
        if (SessionBox.GetUserSession().Role.Contains("矿级管理员"))
        {
            rKuangJi.Enabled = true;
            rKuangJi.Checked = true;
        }
        else
        {
            rKuangJi.Enabled = false;
            rGeRen.Checked = true;
        }
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            TID.SetValue(sm.SelectedRows[0].RecordID.Trim());
            txtTName.Text = "";
            Window1.Show();
        }
        else
        {
            Ext.Msg.Alert("提示", "请选择要修改的模板!").Show();
        }
    }
    [AjaxMethod]
    public void DeleteTemplate()
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count == 0)
        {
            Ext.Msg.Alert("提示", "请选择需删除的模板!").Show();
            return;
        }

        Ext.Msg.Confirm("提示", "是否确定删除选中模板?", new MessageBox.ButtonsConfig
        {
            Yes = new MessageBox.ButtonConfig
            {
                Handler = "Coolite.AjaxMethods.SuerDeleteT('" + sm.SelectedRows[0].RecordID.Trim() + "');",
                Text = "确 定"
            },
            No = new MessageBox.ButtonConfig
            {
                Text = "取 消"
            }
        }).Show();
    }
    [AjaxMethod]
    public void SuerDeleteT(string id)
    {
        HBBLL hb = new HBBLL();
        hb.DeleteYPPT(int.Parse(id));
        Ext.Msg.Alert("提示", "删除成功!").Show();
        Ext.DoScript("#{placeTemplateStore}.reload();");

    }

    [AjaxMethod]
    public void RouteMan()
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            HBBLL hb = new HBBLL();
            TID.SetValue(sm.SelectedRows[0].RecordID.Trim());
            TID.Value = sm.SelectedRows[0].RecordID.Trim();
            TID.Text = sm.SelectedRows[0].RecordID.Trim();

            string id = TID.Value.ToString();
            id = TID.Text;
            lblDetail.Text = hb.GetYPPTNameByID(int.Parse(id));
            
            DetailWindow.Show();
            UpdateStatus1();
            Ext.DoScript("#{YPPTDetailStore}.reload();");
        }
        else
        {
            Ext.Msg.Alert("提示", "请选择一个模板!").Show();
        }
    }
    [AjaxMethod]
    public void PlaceUp()
    {
        RowSelectionModel sm = this.GridPanel2.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count == 1)
        {
            HBBLL hb = new HBBLL();
            int id = int.Parse(sm.SelectedRows[0].RecordID.Trim());
            hb.MoveUp(id, hb.GetMoveOrderByID(id), int.Parse(TID.Value.ToString()));
            Ext.DoScript("#{YPPTDetailStore}.reload();");
        }
    }
    [AjaxMethod]
    public void PlaceDown()
    {
        RowSelectionModel sm = this.GridPanel2.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count == 1)
        {
            HBBLL hb = new HBBLL();
            int id = int.Parse(sm.SelectedRows[0].RecordID.Trim());
            hb.MoveDown(id, hb.GetMoveOrderByID(id), int.Parse(TID.Value.ToString()));
            Ext.DoScript("#{YPPTDetailStore}.reload();");
        }
    }
    [AjaxMethod]
    public void PlaceDel()
    {
        RowSelectionModel sm = this.GridPanel2.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count == 0)
        {
            Ext.Msg.Alert("提示", "请选择需删除的地点!").Show();
            return;
        }
        
        Ext.Msg.Confirm("提示", "是否确定删除选中地点?", new MessageBox.ButtonsConfig
        {
            Yes = new MessageBox.ButtonConfig
            {
                Handler = "Coolite.AjaxMethods.SuerDelete();",
                Text = "确 定"
            },
            No = new MessageBox.ButtonConfig
            {
                Text = "取 消"
            }
        }).Show();
    }
    [AjaxMethod]
    public void SuerDelete()
    {
        RowSelectionModel sm = this.GridPanel2.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            List<int> ids = new List<int>();
            foreach (var item in sm.SelectedRows)
            {
                ids.Add(Convert.ToInt32(item.RecordID));
            }
            HBBLL hb = new HBBLL();
            hb.DeleteYPPTDetail(ids);
            hb.UpdateYPPTDetailOrder(int.Parse(TID.Value.ToString()));
            Ext.Msg.Alert("提示", "删除成功!").Show();
            Ext.DoScript("#{YPPTDetailStore}.reload();");
        }
        
    }
    [AjaxMethod]
    public void SaveTName()
    {
        if (txtTName.Text.Trim() != "")
        {
            HBBLL hb = new HBBLL();
            if (TID.Value.ToString() == "")
            {
                hb.AddYPPT(txtTName.Text.Trim(), rKuangJi.Checked==true?rKuangJi.BoxLabel:rGeRen.BoxLabel, SessionBox.GetUserSession().PersonNumber, SessionBox.GetUserSession().DeptNumber);
                Ext.Msg.Alert("提示", "新增成功!").Show();
                Ext.DoScript("#{placeTemplateStore}.reload();");
            }
            else
            {
                hb.UpdateYPPT(txtTName.Text.Trim(), int.Parse(TID.Value.ToString()));
                Ext.Msg.Alert("提示", "修改成功!").Show();
                Ext.DoScript("#{placeTemplateStore}.reload();");
            }
        }
    }
    [AjaxMethod]
    public void BaseSave()
    {
        if (cbbplace.SelectedIndex == -1)
        {
            Ext.Msg.Alert("提示", "请选择地点!").Show();
            return;
        }
        if (TDID.Value.ToString() == "")
        {
            HBBLL hb = new HBBLL();
            hb.AddYPPTDetail(int.Parse(TID.Value.ToString()), int.Parse(cbbplace.SelectedItem.Value), hb.GetMaxMoveOrder(int.Parse(TID.Value.ToString())) + 1);
            cbbplace.Value = null;
            Ext.Msg.Alert("提示", "新增成功!").Show();
            Ext.DoScript("#{YPPTDetailStore}.reload();");
        }
        else
        {
            HBBLL hb = new HBBLL();
            hb.UpdateYPPTDetail(int.Parse(cbbplace.SelectedItem.Value),int.Parse(TDID.Value.ToString()));
            cbbplace.Value = null;
            Ext.Msg.Alert("提示", "修改成功!").Show();
            Ext.DoScript("#{YPPTDetailStore}.reload();");

        }
    }
    [AjaxMethod]
    public void PlaceAdd()
    {
        TDID.Value = "";
        Window2.Show();
    }
    [AjaxMethod]
    public void PlaceUpdate()
    {
        RowSelectionModel sm = this.GridPanel2.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count == 1)
        {
            TDID.SetValue(sm.SelectedRows[0].RecordID.Trim());          
            Window2.Show();
        }
        else
        {
            Ext.Msg.Alert("提示", "请只选择一个要修改的地点!").Show();
        }
    }
    protected void placeTemplateStore_RefershData(object sender, StoreRefreshDataEventArgs e)
    {
        
        HBBLL hb = new HBBLL();

        var data2 = hb.GetYPPT(SessionBox.GetUserSession().PersonNumber, SessionBox.GetUserSession().DeptNumber);

        this.placeTemplateStore.DataSource = data2;
        this.placeTemplateStore.DataBind();
        UpdateStatus();
       
    }
    protected void YPPTDetailStore_RefershData(object sender, StoreRefreshDataEventArgs e)
    {

        if (TID.Value.ToString() != "")
        {
            HBBLL hb = new HBBLL();

            var data2 = hb.GetYPPTDetail(int.Parse(TID.Value.ToString()), "", SessionBox.GetUserSession().DeptNumber, "");

            this.YPPTDetailStore.DataSource = data2;
            this.YPPTDetailStore.DataBind();
            UpdateStatus1();
        }

    }
    protected void RowClick(object sender, AjaxEventArgs e)
    {
        UpdateStatus();

    }
    protected void RowClick1(object sender, AjaxEventArgs e)
    {
        UpdateStatus1();

    }
    private void UpdateStatus()
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            HBBLL hb = new HBBLL();
            YPPT data = hb.GetYPPTByID(int.Parse(sm.SelectedRows[0].RecordID));
            if (data.CreatePersonNumber == SessionBox.GetUserSession().PersonNumber)
            {
                btnDel.Disabled = false;
                btnDetail.Disabled = false;
                btnPlan.Disabled = false;
            }
            else
            {
                btnPlan.Disabled = true;
                btnDetail.Disabled = false;
                btnDel.Disabled = true;
            }
            
        }
        else
        {
            btnPlan.Disabled = true;
            btnDetail.Disabled = true;
            btnDel.Disabled = true;
        }
    }
    private void UpdateStatus1()
    {
        RowSelectionModel sm = this.GridPanel2.SelectionModel.Primary as RowSelectionModel;
        HBBLL hb = new HBBLL();
        var data = hb.GetYPPTByID(int.Parse(TID.Value.ToString()));
        if (data.CreatePersonNumber == SessionBox.GetUserSession().PersonNumber)
        {
            switch (sm.SelectedRows.Count)
            {
                case 0:
                    btnPlaceUpdate.Disabled = true;
                    btnPlaceDel.Disabled = true;
                    btnDown.Disabled = true;
                    btnUp.Disabled = true;
                    break;
                case 1:
                    btnPlaceUpdate.Disabled = false;
                    btnPlaceDel.Disabled = false;
                    btnDown.Disabled = false;
                    btnUp.Disabled = false;
                    break;
                default:
                    btnPlaceUpdate.Disabled = true;
                    btnPlaceDel.Disabled = false;
                    btnDown.Disabled = true;
                    btnUp.Disabled = true;
                    break;
            }
        }
        else
        {
            btnPlaceAdd.Disabled = true;
            btnPlaceUpdate.Disabled = true;
            btnPlaceDel.Disabled = true;
            btnDown.Disabled = true;
            btnUp.Disabled = true;
        }
        
    }
    #region 拼音检索

    [AjaxMethod]
    public void PYsearch(string py, string store)
    {
        switch (store.Trim())
        {
            case "placeStore":
                var place = from pl in dc.Place
                            where pl.Maindeptid == SessionBox.GetUserSession().DeptNumber && pl.Placestatus == 1
                            && (dc.F_PINYIN(pl.Placename) + pl.Placename).ToLower().Contains(py.ToLower())
                            select new
                            {
                                pl.Placeid,
                                pl.Placename
                            };
                placeStore.DataSource = place;
                placeStore.DataBind();
                break;
        }
    }

    #endregion
}