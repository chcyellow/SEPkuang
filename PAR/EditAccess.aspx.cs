using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Coolite.Ext.Web;
using System.Xml;
using System.Text;
using GhtnTech.SEP.DAL;
using GhtnTech.SecurityFramework.BLL;

public partial class PAR_EditAccess : System.Web.UI.Page
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            BaseSet();//初始化基本控件

            if (Request.QueryString["Type"] == "new")
            {
                SetKind(decimal.Parse(Request.QueryString["Pkindid"]));
                dfDate.SelectedDate = System.DateTime.Today;
                cbbCheckDept.SelectedItem.Value = dc.Vgetpl.First(p => p.Personnumber == SessionBox.GetUserSession().PersonNumber).Deptnumber;

                var q = from p in dc.Person
                        where p.Areadeptid == cbbCheckDept.SelectedItem.Value
                        select new
                        {
                            p.Personnumber,
                            p.Name
                        };
                KHRStore.DataSource = q;
                KHRStore.DataBind();
                //cbb_khr.Disabled = q.Count() > 0 ? false : true;
                cbb_khr.SelectedItem.Value = SessionBox.GetUserSession().PersonNumber;

                GVLoad(decimal.Parse(Request.QueryString["Pkindid"]));
            }
            if (Request.QueryString["Type"] == "edit" || Request.QueryString["Type"] == "view")
            {
                var result = dc.ParResult.First(p => p.Rid == decimal.Parse(Request.QueryString["Rid"]));
                bool bl=SetKind(result.Pkindid);
                cbbCheckDept.SelectedItem.Value = result.Checkdept;
                cbbforcheckDept.SelectedItem.Value = result.Checkfordept;
                var q = from p in dc.Person
                        where p.Areadeptid == result.Checkfordept
                        select new
                        {
                            p.Personnumber,
                            p.Name
                        };
                PersStore.DataSource = q;
                PersStore.DataBind();
                //fb_zrr.Disabled = q.Count() > 0 ? false : true;
                //fb_zrr.EmptyText = q.Count() > 0 ? "请选择责任人" : "没有待选人员";
                var x = from p in dc.Person
                        where p.Areadeptid == result.Checkdept
                        select new
                        {
                            p.Personnumber,
                            p.Name
                        };
                KHRStore.DataSource = x;
                KHRStore.DataBind();
                cbb_khr.Disabled = x.Count() > 0 ? false : true;
                try
                {
                    cbb_khr.SelectedItem.Value = result.Khperson;
                }
                catch
                {
                }
                dfDate.SelectedDate = result.Checkdate.Value;
                cbbplace.SelectedItem.Value = result.Placeid;
                try
                {
                    fb_zrr.SelectedItem.Value = result.Bkhperson;
                }
                catch
                {
                }
                if (bl)
                {
                    cbbBanci.SelectedItem.Value = result.Banci;
                }
                GVLoad(Request.QueryString["Rid"]);
            }
            if (Request.QueryString["Type"] == "view")
            {
                btnSave.Visible = false;
            }
        }
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
            case "PersStore":
                if (cbbforcheckDept.SelectedIndex == -1)
                {
                    return;
                }
                var q = from p in dc.Person
                        where p.Areadeptid == cbbforcheckDept.SelectedItem.Value
                        && (dc.F_PINYIN(p.Name).ToLower().Contains(py.ToLower()) || p.Name.Contains(py.Trim()))
                        select new
                        {
                            p.Personnumber,
                            p.Name
                        };
                PersStore.DataSource = q;
                PersStore.DataBind();
                break;
        }
    }

    protected void KHRRefresh(object sender, StoreRefreshDataEventArgs e)
    {
        var q = from p in dc.Person
                where p.Areadeptid == cbbCheckDept.SelectedItem.Value
                select new
                {
                    p.Personnumber,
                    p.Name
                };
        KHRStore.DataSource = q;
        KHRStore.DataBind();
        cbb_khr.Disabled = q.Count() > 0 ? false : true;
        //fb_zrr.EmptyText = q.Count() > 0 ? "请选择责任人" : "没有待选人员";

    }

    private void BaseSet()
    {
        var dept = PublicCode.GetKQdept( SessionBox.GetUserSession().DeptNumber);
        DeptStore.DataSource = dept;
        DeptStore.DataBind();

        var place = from pl in dc.Place
                    where pl.Maindeptid == SessionBox.GetUserSession().DeptNumber
                    orderby pl.Placename ascending
                    select new
                    {
                        placID = pl.Placeid,
                        placName = pl.Placename
                    };
        placeStore.DataSource = place;
        placeStore.DataBind();
    }

    private bool SetKind(decimal kindid)
    {
        var kind = dc.ParKind.First(p => p.Pkindid == kindid);
        cbbKind.Items.Clear();
        cbbKind.Items.Add(new Coolite.Ext.Web.ListItem(kind.Pkindname, kind.Pkindid.ToString()));
        cbbKind.SelectedIndex = 0;
        cbbKind.Disabled = true;

        cbbBanci.Visible = kind.Rate == "每班一次" ? true : false;

        return kind.Rate == "每班一次";
    }

    private void GVLoad(decimal kindid)
    {
        var data = (from j in dc.ParJeomcriterion
                    from i in dc.ParItem
                    where j.Itemid == i.Itemid && j.Status == "1" && i.Pkindid==kindid
                    orderby i.Sort,j.Sort
                    select new
                    {
                        Rdid=-1,
                        Jcid = "j" + j.Jcid,
                        j.Jccontent,
                        j.Minscore,
                        j.Maxscore,
                        Jeom = "",
                        Remark = "",
                        Kind = i.Itemname
                    }).Concat(
                       from a in dc.ParAddjeomcriterion
                       where a.Pkindid == kindid && a.Status == "1"
                       orderby a.Sort
                       select new
                       {
                           Rdid=-1,
                           Jcid = "a" + a.Ajcid,
                           Jccontent = a.Ajccontent,
                           a.Minscore,
                           a.Maxscore,
                           Jeom = "",
                           Remark = "",
                           Kind = "附加内容"
                       }
                           );
        JeomStore.DataSource = data;
        JeomStore.DataBind();
    }

    private void GVLoad(string Rid)
    {
        var data=dc.ViewParresultdetail.Where(p=>p.Rid==decimal.Parse(Rid));
        JeomStore.DataSource = data;
        JeomStore.DataBind();
    }

    protected void Cell_Click(object sender, AjaxEventArgs e)
    {
        //CellSelectionModel sm = gpEdit.SelectionModel.Primary as CellSelectionModel;
        //int row = sm.SelectedCell.RowIndex;
        //int col = sm.SelectedCell.ColIndex;

        //Ext.DoScript("#{gpEdit}.getView().focusRow("+row+");#{gpEdit}.startEditing("+row+", "+col+");");
    }

    #region 数据保存

    protected void SaveHandler(object sender, BeforeStoreChangedEventArgs e)
    {
        if (cbbCheckDept.SelectedIndex == -1 || cbbforcheckDept.SelectedIndex == -1)
        {
            Ext.Msg.Alert("提示", "请填写完整信息!").Show();
            return;
        }
        XmlNode xml = e.DataHandler.XmlData;
        XmlNode updated = xml.SelectSingleNode("records/Updated");
        if (Check(updated))
        {
            string type = Request.QueryString["Type"].Trim();
            decimal strrid = 0;
            if (type == "new")
            {
                ParResult pr = new ParResult
                {
                    Checkdate = dfDate.SelectedDate,
                    Checkdept = cbbCheckDept.SelectedItem.Value,
                    Checkfordept = cbbforcheckDept.SelectedItem.Value,
                    Pkindid = decimal.Parse(Request.QueryString["Pkindid"]),
                   
                    Rstatus = "1",
                    Total = 0
                };
                try
                {
                    pr.Banci = cbbBanci.SelectedItem.Value;
                }
                catch
                {
                }
                if (fb_zrr.SelectedIndex > -1)
                {
                    pr.Bkhperson = fb_zrr.SelectedItem.Value;
                }
                if (cbb_khr.SelectedIndex > -1)
                {
                    pr.Khperson = cbb_khr.SelectedItem.Value;
                }
                if(cbbplace.SelectedIndex>-1)
                {
                    pr.Placeid = cbbplace.SelectedItem.Value;
                }
                dc.ParResult.InsertOnSubmit(pr);
                dc.SubmitChanges();
                strrid = dc.ParResult.Max(p => p.Rid);
            }
            else
            {
                var result = dc.ParResult.First(p => p.Rid == decimal.Parse(Request.QueryString["Rid"]));
                result.Checkfordept = cbbforcheckDept.SelectedItem.Value;
                result.Placeid = cbbplace.SelectedItem.Value;
                result.Checkdate = dfDate.SelectedDate;
                if (fb_zrr.SelectedIndex > -1)
                {
                    result.Bkhperson = fb_zrr.SelectedItem.Value;
                }
                if (cbb_khr.SelectedIndex > -1)
                {
                    result.Khperson = cbb_khr.SelectedItem.Value;
                }
                dc.SubmitChanges();
                strrid = result.Rid;
            }
            if (updated != null)
            {

                XmlNodeList uRecords = updated.SelectNodes("record");
                decimal total = 0;
                if (uRecords.Count > 0)
                {
                    foreach (XmlNode record in uRecords)
                    {
                        if (record != null)
                        {
                            total += record.SelectSingleNode("Jeom").InnerText.Trim() == "" ? 0 : decimal.Parse(record.SelectSingleNode("Jeom").InnerText);
                            if (type == "new")
                            {
                                if (record.SelectSingleNode("Jcid").InnerText.Trim().Substring(0, 1) == "j")
                                {
                                    AddDetail(record, strrid);
                                }
                                else
                                {
                                    AddRAdetail(record, strrid);
                                }
                            }
                            else
                            {
                                if (record.SelectSingleNode("Jcid").InnerText.Trim().Substring(0, 1) == "j")
                                {
                                    if (record.SelectSingleNode("Rdid").InnerText.Trim() == "-1")
                                        AddDetail(record, strrid);
                                    else
                                        UpdateDetail(record);
                                }
                                else
                                {
                                    if (record.SelectSingleNode("Rdid").InnerText.Trim() == "-1")
                                        AddRAdetail(record, strrid);
                                    else
                                        UpdateRADetail(record);
                                }
                            }
                        }
                    }
                    var res = dc.ParResult.First(p => p.Rid == strrid);
                    res.Total = dc.ParKind.First(p => p.Pkindid == res.Pkindid).Fullscore.Value - total;
                    dc.SubmitChanges();

                }
               
                e.Cancel = true;

                GVLoad(strrid.ToString());
            }
            Ext.Msg.Alert("提示", "保存成功").Show();
        }
        else
        {
            Ext.Msg.Alert("提示", "扣分超过最大值").Show();
        }
    }

    private void AddDetail(XmlNode data,decimal rid)
    {
        ParResultDetail rd = new ParResultDetail
                            {
                                Jcid = decimal.Parse(data.SelectSingleNode("Jcid").InnerText.Trim().Substring(1)),
                                Jeom = data.SelectSingleNode("Jeom").InnerText.Trim() == "" ? 0 : decimal.Parse(data.SelectSingleNode("Jeom").InnerText),
                                Remark = data.SelectSingleNode("Remark").InnerText.Trim(),
                                Rid=rid
                            };
        DBSCMDataContext dc1 = new DBSCMDataContext();
        dc1.ParResultDetail.InsertOnSubmit(rd);
        dc1.SubmitChanges();
    }

    private void UpdateDetail(XmlNode data)
    {
        DBSCMDataContext dc1 = new DBSCMDataContext();
        var rd = dc1.ParResultDetail.First(p => p.Rdid == decimal.Parse(data.SelectSingleNode("Rdid").InnerText));
        rd.Jeom = data.SelectSingleNode("Jeom").InnerText.Trim() == "" ? 0 : decimal.Parse(data.SelectSingleNode("Jeom").InnerText);
        rd.Remark = data.SelectSingleNode("Remark").InnerText.Trim();
        dc1.SubmitChanges();
    }

    private void AddRAdetail(XmlNode data, decimal rid)
    {
        ParResultAdddetail ra = new ParResultAdddetail
        {
            Ajcid = decimal.Parse(data.SelectSingleNode("Jcid").InnerText.Trim().Substring(1)),
            Jeom = data.SelectSingleNode("Jeom").InnerText.Trim() == "" ? 0 : decimal.Parse(data.SelectSingleNode("Jeom").InnerText),
            Remark = data.SelectSingleNode("Remark").InnerText.Trim(),
            Rid = rid
        };
        DBSCMDataContext dc1 = new DBSCMDataContext();
        dc1.ParResultAdddetail.InsertOnSubmit(ra);
        dc1.SubmitChanges();
    }

    private void UpdateRADetail(XmlNode data)
    {
        DBSCMDataContext dc1 = new DBSCMDataContext();
        var rd = dc1.ParResultAdddetail.First(p => p.Ardid == decimal.Parse(data.SelectSingleNode("Rdid").InnerText));
        rd.Jeom = data.SelectSingleNode("Jeom").InnerText.Trim() == "" ? 0 : decimal.Parse(data.SelectSingleNode("Jeom").InnerText);
        rd.Remark = data.SelectSingleNode("Remark").InnerText.Trim();
        dc1.SubmitChanges();
    }

    private bool Check(XmlNode node)
    {
        if (node != null)
        {
            XmlNodeList uRecords = node.SelectNodes("record");
            decimal all = 0;
            foreach (XmlNode record in uRecords)
            {
                if (record != null)
                {
                    string Jeom=record.SelectSingleNode("Jeom").InnerText.Trim();
                    if (Jeom != "" && Jeom != "0")
                    {
                        //decimal min = decimal.Parse(record.SelectSingleNode("Minscore").InnerText);
                        //decimal max = decimal.Parse(record.SelectSingleNode("Maxscore").InnerText);
                        //if (decimal.Parse(Jeom) < min || decimal.Parse(Jeom) > max)
                        //{
                        //    return false;
                        //}
                        //else
                        //{
                        all += decimal.Parse(Jeom);
                        //}
                    }
                }
            }

            if (dc.ParKind.First(p => p.Pkindid == decimal.Parse(cbbKind.SelectedItem.Value)).Fullscore.Value > all)
            {
                return true;
            }

            return false;
        }

        return true;
    }

    #endregion
}
