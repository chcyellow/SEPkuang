using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Coolite.Ext.Web;
using GhtnTech.SecurityFramework.BLL;
using GhtnTech.SEP.DAL;

public partial class MovePlan_Movemodels : BasePage
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            //baseLoad();
            BaseStroeLoad();
        }
    }
    protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)
    {
        BaseStroeLoad();
    }

    [AjaxMethod]
    public void PYsearch(string py, string store)
    {
        switch (store.Trim())
        {
            case "DepartmentSearchStore":
                var dept = from d in dc.Department
                           where d.Deptnumber.Substring(0, 4) == SessionBox.GetUserSession().DeptNumber.Substring(0, 4)
                           && d.Deptnumber.Substring(7) == "00"
                           && dc.F_PINYIN(d.Deptname).ToLower().Contains(py.ToLower())
                           select new
                           {
                               deptID = d.Deptnumber,
                               deptName = d.Deptname
                           };
                DepartmentSearchStore.DataSource = dept;
                DepartmentSearchStore.DataBind();
                break;
            case "PlaceSearchStore":
                var place = from pl in dc.Place
                            where pl.Maindeptid == SessionBox.GetUserSession().DeptNumber
                            && dc.F_PINYIN(pl.Placename).ToLower().Contains(py.ToLower())
                            select new
                            {
                                placID = pl.Placeid,
                                placName = pl.Placename
                                //,
                                //pyall = dc.F_PINYIN(pl.Placename)
                            };
                PlaceSearchStore.DataSource = place;
                PlaceSearchStore.DataBind();
                break;
        }
    }

    private void baseLoad()
    {
        //绑定部门--没有矿级信息
        var dep = from d in dc.Department
                  where d.Deptnumber != SessionBox.GetUserSession().DeptNumber && d.Deptnumber.Substring(0, 4) == SessionBox.GetUserSession().DeptNumber.Substring(0, 4)
                  && d.Deptnumber.Substring(7) == "00"
                  select new
                  {
                      deptID = d.Deptnumber,
                      deptName = d.Deptname
                      //,
                      //pyall = dc.F_PINYIN(d.Deptname)
                  };
        DepartmentSearchStore.DataSource = dep;
        DepartmentSearchStore.DataBind();

        //初始化地点
        var place = from pl in dc.Place
                    where pl.Maindeptid == SessionBox.GetUserSession().DeptNumber
                    select new
                    {
                        placID = pl.Placeid,
                        placName = pl.Placename
                        //,
                        //pyall = dc.F_PINYIN(pl.Placename)
                    };
        PlaceSearchStore.DataSource = place;
        PlaceSearchStore.DataBind();
    }

    private void BaseStroeLoad()
    {
        var data = from m in dc.VMovefrequency
                where m.Maindept == SessionBox.GetUserSession().DeptNumber
                select new
                {
                    FrequencyID = m.Frequencyid,
                    DeptName = m.Deptname,
                    PlaceName = m.Placename,
                    PosName = m.Posname,
                    Frequency = m.Frequency
                };
        #region 直接linq查询-数据搜索速度慢，先改成上述视图
        //var data = from m in dc.Movefrequency
        //           from pos in dc.Position
        //           from pl in dc.Place
        //           from d in dc.Department
        //           where m.Posid == pos.Posid && m.Placeid == pl.Placeid && m.Deptid == d.Deptnumber && m.Maindept==SessionBox.GetUserSession().DeptNumber
        //           select new
        //           {
        //               FrequencyID = m.Frequencyid,
        //               DeptName = d.Deptname,
        //               PlaceName = pl.Placename,
        //               PosName = pos.Posname,
        //               Frequency = m.Frequency
        //           };
        #endregion
        Store1.DataSource = data;
        Store1.DataBind();
        btn_update.Disabled = true;
        btn_delete.Disabled = true;
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        sm.SelectedRows.Clear();
        sm.UpdateSelection();
    }
    protected void RowClick(object sender, AjaxEventArgs e)
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            btn_update.Disabled = false;
            btn_delete.Disabled = false;
        }
        else
        {
            btn_update.Disabled = true;
            btn_delete.Disabled = true;
        }
    }

    [AjaxMethod]
    public void Updatedata()
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        var data = dc.Movefrequency.Where(p => p.Frequencyid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
        foreach (var r in data)
        {
            //r.DeptID = Convert.ToInt32(up_dep.SelectedItem.Value);
            //r.PosID = Convert.ToInt32(up_pos.SelectedItem.Value);
            //r.PlaceID = Convert.ToInt32(up_pla.SelectedItem.Value);
            r.Frequency = Convert.ToInt32(up_fun.Text.Trim());
        }
        dc.SubmitChanges();
        BaseStroeLoad();
        Ext.Msg.Alert("提示", "保存成功!").Show();
    }

    [AjaxMethod]
    public void LoadData()
    {
        baseLoad();
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        var data = dc.Movefrequency.Where(p => p.Frequencyid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
        foreach (var r in data)
        {
            up_dep.SetValue(r.Deptid);
            up_pos.SetValue(r.Posid.Value.ToString());
            up_pla.SetValue(r.Placeid.Value.ToString());
            up_fun.Text = r.Frequency.Value.ToString();
        }
        Window2.Show();
    }

    [AjaxMethod]
    public void Savedata()
    {
        if (cbb_dep.SelectedIndex == -1 || cbb_pos.SelectedIndex == -1 || cbb_pla.SelectedIndex == -1 || nf_Frequency.IsNull)
        {
            Ext.Msg.Alert("提示", "请选择完整信息").Show();
            return;
        }
        var check = from m in dc.Movefrequency
                    from p in dc.Place
                    where m.Placeid == p.Placeid && p.Placeid == Convert.ToInt32(cbb_pla.SelectedItem.Value.Trim()) && m.Posid == Convert.ToInt32(cbb_pos.SelectedItem.Value.Trim()) && m.Deptid == cbb_dep.SelectedItem.Value.Trim()
                    select m;
        if (check.Count() == 0)
        {
            SaveNewData();
            BaseStroeLoad();
            Ext.Msg.Alert("提示", "生成成功!").Show();
        }
        else
        {
            Ext.Msg.Confirm("提示", "该条件下模板已存在，是否删除后重新生成?", new MessageBox.ButtonsConfig
            {
                Yes = new MessageBox.ButtonConfig
                {
                    Handler = "Coolite.AjaxMethods.DelData();",
                    Text = "确 定"
                },
                No = new MessageBox.ButtonConfig
                {
                    Text = "取 消"
                }
            }).Show();
        }
    }
    [AjaxMethod]
    public void DelData()
    {
        var check = from m in dc.Movefrequency
                    from p in dc.Place
                    where m.Placeid == p.Placeid && p.Placeid == Convert.ToInt32(cbb_pla.SelectedItem.Value.Trim()) && m.Posid == Convert.ToInt32(cbb_pos.SelectedItem.Value.Trim()) && m.Deptid == cbb_dep.SelectedItem.Value.Trim()
                    select m;
        dc.Movefrequency.DeleteAllOnSubmit(check);
        dc.SubmitChanges();
        SaveNewData();
        BaseStroeLoad();
        Ext.Msg.Alert("提示", "生成成功!").Show();
    }

    [AjaxMethod]
    public void delshow()
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        Ext.Msg.Confirm("提示", "是否确定删除?", new MessageBox.ButtonsConfig
        {
            Yes = new MessageBox.ButtonConfig
            {
                Handler = "Coolite.AjaxMethods.Detail_Del('" + sm.SelectedRows[0].RecordID.Trim() + "');",
                Text = "确 定"
            },
            No = new MessageBox.ButtonConfig
            {
                Text = "取 消"
            }
        }).Show();
    }
    [AjaxMethod]
    public void Detail_Del(string ID)
    {
        var mf = dc.Movefrequency.Where(p => p.Frequencyid == Convert.ToInt32(ID));
        dc.Movefrequency.DeleteAllOnSubmit(mf);
        dc.SubmitChanges();
        BaseStroeLoad();
        Ext.Msg.Alert("提示", "删除成功!").Show();
    }

    private void SaveNewData()
    {
        var group = dc.Place.Where(p => p.Placeid == Convert.ToInt32(cbb_pla.SelectedItem.Value.Trim()));
        foreach (var r in group)
        {
            Movefrequency mf = new Movefrequency
            {
                Deptid = cbb_dep.SelectedItem.Value.Trim(),
                Posid = Convert.ToInt32(cbb_pos.SelectedItem.Value.Trim()),
                Placeid = r.Placeid,
                Frequency = Convert.ToInt32(nf_Frequency.Text.Trim()),
                Maindept = SessionBox.GetUserSession().DeptNumber
            };
            dc.Movefrequency.InsertOnSubmit(mf);
            dc.SubmitChanges();
        }
    }

    protected void PosSearchRefresh(object sender, StoreRefreshDataEventArgs e)//发布/提交选择责任部门人员刷新
    {
        var pos = (from position in dc.Position
                   from person in dc.Person
                   where position.Posid == person.Posid && person.Areadeptid == cbb_dep.SelectedItem.Value && person.Maindeptid == SessionBox.GetUserSession().DeptNumber
                   select new
                   {
                       PosID = position.Posid,
                       PosName = position.Posname
                   }).Distinct();
        PosSearchStore.DataSource = pos;
        PosSearchStore.DataBind();
        cbb_pos.Disabled = pos.Count() > 0 ? false : true;
    }
}
