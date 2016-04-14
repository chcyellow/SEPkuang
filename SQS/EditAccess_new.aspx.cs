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

public partial class SQS_EditAccess_new : System.Web.UI.Page
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            BaseSet();

            if (Request.QueryString["Type"] == "new")
            {
                SetKind(decimal.Parse(Request.QueryString["Pkindid"]));
                dfDate.SelectedDate = System.DateTime.Today;
                cbbCheckDept.SelectedItem.Value = dc.Vgetpl.First(p => p.Personnumber == SessionBox.GetUserSession().PersonNumber).Deptnumber;

                GVLoad(decimal.Parse(Request.QueryString["Pkindid"]));
            }
            if (Request.QueryString["Type"] == "edit")
            {
                hdnId.SetValue(Request.QueryString["Rid"]);
                string a = hdnId.Value.ToString();
                hdnId.Value = Request.QueryString["Rid"];
                var result = dc.SqsResult.First(p => p.Rid == decimal.Parse(Request.QueryString["Rid"]));
                SetKind(result.Did.Value);
                cbbCheckDept.SelectedItem.Value = result.Checkdept;
                cbbforcheckDept.SelectedItem.Value = result.Checkfordept;
                dfDate.SelectedDate = result.Checkdate.Value;
                tfplace.Text = result.Placeid;
                cbbXZ.SelectedItem.Value = result.Checkway;
                GVLoad(Request.QueryString["Rid"], result.Did.Value);
            }
        }
    }

    private void SetKind(decimal kindid)
    {
        var kind = dc.SqsDomain.First(p => p.Did == kindid);
        cbbKind.Items.Clear();
        cbbKind.Items.Add(new Coolite.Ext.Web.ListItem(kind.Dname, kind.Did.ToString()));
        cbbKind.SelectedIndex = 0;
        cbbKind.Disabled = true;
    }

    private void BaseSet()
    {
        var dept = from d in dc.Department
                   where d.Deptnumber.Substring(0, 4) == SessionBox.GetUserSession().DeptNumber.Substring(0, 4)
                   && d.Deptlevel=="正科级"
                   orderby d.Deptname
                   select new
                   {
                       d.Deptnumber,
                       d.Deptname
                   };
        if (dept.Count() > 0)
        {
            DeptStore.DataSource = dept;
            DeptStore.DataBind();
        }
        else
        {
            var de = from d in dc.Department
                     where d.Deptnumber.Substring(4) == "00000"
                     select new
                     {
                         d.Deptnumber,
                         d.Deptname
                     };
            DeptStore.DataSource = de;
            DeptStore.DataBind();
        }

        dfDate.SelectedDate = System.DateTime.Today;
    }

    private void GVLoad(decimal kindid)
    {

        var n = from a in dc.SqsEssentialcondition
                where a.Did==kindid
                select new
                {
                    a.Ecid,
                    a.Content,
                    isCheck = false
                };

        ESSENTIALCONDITIONStore.DataSource = n;
        ESSENTIALCONDITIONStore.DataBind();

        var d = from a in dc.SqsDemotion
                where a.Did==kindid
                select new
                {
                    a.Deid,
                    a.Content,
                    isCheck = false
                };
        DEMOTIONStore.DataSource = d;
        DEMOTIONStore.DataBind();

        var data = from j in dc.SqsJeomcriterion
                   from i in dc.SqsKind
                   where j.Pkindid == i.Pkindid && i.Did == kindid && j.Status == "1"
                   orderby j.Sort
                   select new
                   {
                       j.Jcid,
                       Jccontent = j.Jccontent,
                       j.Score,
                       Means = j.Means,
                       Jeom = j.Score,
                       Remark = "",
                       Kind = i.Pkindname
                   };
        JeomStore.DataSource = data;
        JeomStore.DataBind();
    }

    private void GVLoad(string Rid,decimal kind)
    {

        var n = from e in dc.SqsEssentialcondition
                join r in dc.SqsEssentialconditiondetail.Where(p=>p.Rid==decimal.Parse(Rid)) on e.Ecid equals r.Ecid into gg
                from g in gg.DefaultIfEmpty()
                where e.Did == kind
                select new
                {
                    e.Ecid,
                    e.Content,
                    isCheck = g.Ecid != null
                };

        ESSENTIALCONDITIONStore.DataSource = n;
        ESSENTIALCONDITIONStore.DataBind();

        var d = from a in dc.SqsDemotion
                join r in dc.SqsDemotiondetail.Where(p=>p.Rid== decimal.Parse(Rid)) on a.Deid equals r.Deid into gg
                from g in gg.DefaultIfEmpty()
                where a.Did == kind
                select new
                {
                    a.Deid,
                    a.Content,
                    isCheck = g.Deid != null
                };
        DEMOTIONStore.DataSource = d;
        DEMOTIONStore.DataBind();

        var data = from j in dc.SqsJeomcriterion
                   join i in dc.SqsKind on j.Pkindid equals i.Pkindid
                   join r in dc.SqsResultDetail.Where(p=>p.Rid == decimal.Parse(Rid)) on j.Jcid equals r.Jcid into gg
                   from g in gg.DefaultIfEmpty()
                   where i.Did == kind
                   orderby j.Sort
                   select new
                   {
                       j.Jcid,
                       j.Jccontent,
                       j.Score,
                       Means = j.Means,
                       Jeom = g==null?j.Score:g.Jeom,
                       Remark = g==null?"":g.Remark,
                       Kind = i.Pkindname
                   };
        JeomStore.DataSource = data;
        JeomStore.DataBind();
    }

    [AjaxMethod]
    public void BaseSave()
    {
        if (cbbCheckDept.SelectedIndex == -1 || cbbforcheckDept.SelectedIndex == -1 || tfplace.Text.Trim() == "" || cbbXZ.SelectedIndex == -1)
        {
            Ext.Msg.Alert("提示", "请填写完整信息!").Show();
            return;
        }
        string type = Request.QueryString["Type"].Trim();
        decimal strrid = 0;
        if (hdnId.Value.ToString().Trim() == "")
        {
            SqsResult pr = new SqsResult
            {
                Checkdate = dfDate.SelectedDate,
                Checkdept = cbbCheckDept.SelectedItem.Value,
                Checkfordept = cbbforcheckDept.SelectedItem.Value,
                Did = decimal.Parse(Request.QueryString["Pkindid"]),
                Checkway = cbbXZ.SelectedItem.Value,
                Placeid = tfplace.Text,
                Rstatus = "1",
                Total = 0
            };
            dc.SqsResult.InsertOnSubmit(pr);
            dc.SubmitChanges();
            strrid = dc.SqsResult.Max(p => p.Rid);
            hdnId.SetValue(strrid.ToString());
        }
        else
        {
            var result = dc.SqsResult.First(p => p.Rid == decimal.Parse(hdnId.Value.ToString()));
            result.Checkfordept = cbbforcheckDept.SelectedItem.Value;
            result.Placeid = tfplace.Text;
            result.Checkdate = dfDate.SelectedDate;
            result.Checkway = cbbXZ.SelectedItem.Value;
            dc.SubmitChanges();
            strrid = result.Rid;
        }
        //Ext.Msg.Alert("提示", "1").Show();
        Ext.DoScript("#{gpESSENTIALCONDITION}.save();");
        Ext.DoScript("#{gpDEMOTION}.save();#{gpJeom}.save();");
        Ext.DoScript("#{gpJeom}.save();");

        Ext.Msg.Alert("提示", "保存成功!").Show();

        //var res = dc.SqsResult.First(p => p.Rid == decimal.Parse(hdnId.Value.ToString()));
        //decimal total = dc.SqsResultDetail.Where(p => p.Rid == decimal.Parse(hdnId.Value.ToString())).Sum(p => p.Jeom).Value;
        //var d = dc.SqsDemotiondetail.Where(p => p.Rid == decimal.Parse(hdnId.Value.ToString()));
        //if (total - d.Count() * 10 < 80)
        //{
        //    total = 70 + total % 10 - d.Count() * 5;
        //}
    }

    protected void SaveN(object sender, BeforeStoreChangedEventArgs e)
    {
        XmlNode xml = e.DataHandler.XmlData;
        XmlNode updated = xml.SelectSingleNode("records/Updated");
        if (updated != null)
        {

            XmlNodeList uRecords = updated.SelectNodes("record");
            if (uRecords.Count > 0)
            {
                foreach (XmlNode record in uRecords)
                {
                    if (record != null)
                    {
                        DBSCMDataContext dc1 = new DBSCMDataContext();
                        var rd = dc1.SqsEssentialconditiondetail.Where(p => p.Ecid == decimal.Parse(record.SelectSingleNode("Ecid").InnerText) && p.Rid == decimal.Parse(hdnId.Value.ToString()));
                        if (record.SelectSingleNode("isCheck").InnerText.Trim() == "false")
                        {
                            dc1.SqsEssentialconditiondetail.DeleteOnSubmit(rd.First());
                            dc1.SubmitChanges();
                        }
                        else
                        {
                            if (rd.Count() == 0)
                            {
                                SqsEssentialconditiondetail ra = new SqsEssentialconditiondetail
                                {
                                    Ecid = decimal.Parse(record.SelectSingleNode("Ecid").InnerText),
                                    Rid = decimal.Parse(hdnId.Value.ToString())
                                };
                                dc1.SqsEssentialconditiondetail.InsertOnSubmit(ra);
                                dc1.SubmitChanges();
                            }
                        }
                    }
                }
            }
        }
    }

    protected void SaveD(object sender, BeforeStoreChangedEventArgs e)
    {
        XmlNode xml = e.DataHandler.XmlData;
        XmlNode updated = xml.SelectSingleNode("records/Updated");
        if (updated != null)
        {

            XmlNodeList uRecords = updated.SelectNodes("record");
            if (uRecords.Count > 0)
            {
                foreach (XmlNode record in uRecords)
                {
                    if (record != null)
                    {
                        DBSCMDataContext dc1 = new DBSCMDataContext();
                        var rd = dc1.SqsDemotiondetail.Where(p => p.Deid == decimal.Parse(record.SelectSingleNode("Deid").InnerText) && p.Rid == decimal.Parse(hdnId.Value.ToString()));
                        if (record.SelectSingleNode("isCheck").InnerText.Trim() == "false")
                        {
                            dc1.SqsDemotiondetail.DeleteOnSubmit(rd.First());
                            dc1.SubmitChanges();
                        }
                        else
                        {
                            if (rd.Count() == 0)
                            {
                                SqsDemotiondetail ra = new SqsDemotiondetail
                                {
                                    Deid = decimal.Parse(record.SelectSingleNode("Deid").InnerText),
                                    Rid = decimal.Parse(hdnId.Value.ToString())
                                };
                                dc1.SqsDemotiondetail.InsertOnSubmit(ra);
                                dc1.SubmitChanges();
                            }
                        }
                    }
                }
            }
        }
    }

    protected void SaveJ(object sender, BeforeStoreChangedEventArgs e)
    {
        XmlNode xml = e.DataHandler.XmlData;
        XmlNode updated = xml.SelectSingleNode("records/Updated");
        if (updated != null)
        {

            XmlNodeList uRecords = updated.SelectNodes("record");
            if (uRecords.Count > 0)
            {
                foreach (XmlNode record in uRecords)
                {
                    if (record != null)
                    {
                        DBSCMDataContext dc1 = new DBSCMDataContext();
                        var rd = dc1.SqsResultDetail.Where(p => p.Jcid == decimal.Parse(record.SelectSingleNode("Jcid").InnerText) && p.Rid == decimal.Parse(hdnId.Value.ToString()));
                        if (rd.Count() == 0)
                        {
                            SqsResultDetail rk = new SqsResultDetail
                            {
                                Jcid = decimal.Parse(record.SelectSingleNode("Jcid").InnerText.Trim().Substring(1)),
                                Jeom = record.SelectSingleNode("Jeom").InnerText.Trim() == "" ? 0 : decimal.Parse(record.SelectSingleNode("Jeom").InnerText),
                                Remark = record.SelectSingleNode("Remark").InnerText.Trim(),
                                Rid = decimal.Parse(hdnId.Value.ToString())
                            };
                            dc1.SqsResultDetail.InsertOnSubmit(rk);
                            dc1.SubmitChanges();
                        }
                        else
                        {
                            var r = rd.First();
                            r.Jeom = record.SelectSingleNode("Jeom").InnerText.Trim() == "" ? 0 : decimal.Parse(record.SelectSingleNode("Jeom").InnerText);
                            r.Remark = record.SelectSingleNode("Remark").InnerText.Trim();
                            dc1.SubmitChanges();
                        }
                    }
                }
                e.Cancel = true;
            }
        }
    }
}
