using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Coolite.Ext.Web;
using GhtnTech.SEP.DAL;
using GhtnTech.SecurityFramework.BLL;
using System.Xml;

public partial class YSNewProcess_SMS_Management : System.Web.UI.Page
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            StoreLoad();
        }
    }

    private void StoreLoad()
    {
        decimal lvl = 0;
        if (SessionBox.GetUserSession().rolelevel.Contains("1"))
        {
            lvl = 1;
        }
        if (SessionBox.GetUserSession().rolelevel.Contains("2"))
        {
            lvl = 2;
        }
        var data = from pr in dc.SmsProject
                   join m in dc.SmsManagement.Where(p => p.Deptnumber == SessionBox.GetUserSession().DeptNumber) on pr.Coding equals m.Coding into gg
                   from g in gg.DefaultIfEmpty()
                   where pr.Flevel == lvl
                   select new
                   {
                       pr.Functionname,
                       pr.Coding,
                       pr.Flevel,
                       Mid = g.Mid == null ? -1 : g.Mid,
                       isCheck = g.Sendset != null
                   };
        ManagementStore.DataSource = data.OrderByDescending(p=>p.Functionname);
        ManagementStore.DataBind();
    }

    protected void SaveD(object sender, BeforeStoreChangedEventArgs e)
    {
        try
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
                            if (record.SelectSingleNode("Mid").InnerText.Trim() == "-1")
                            {
                                if (record.SelectSingleNode("isCheck").InnerText.Trim() == "true")
                                {
                                    SmsManagement ma = new SmsManagement
                                    {
                                        Coding = record.SelectSingleNode("Coding").InnerText,
                                        Deptnumber = SessionBox.GetUserSession().DeptNumber,
                                        Sendset = 1
                                    };
                                    dc1.SmsManagement.InsertOnSubmit(ma);
                                    dc1.SubmitChanges();
                                }
                            }
                            else
                            {
                                var ma = dc1.SmsManagement.First(p => p.Mid == decimal.Parse(record.SelectSingleNode("Mid").InnerText.Trim()));
                                if (record.SelectSingleNode("isCheck").InnerText.Trim() == "true")
                                {
                                    ma.Sendset = 1;
                                }
                                else
                                {
                                    dc1.SmsManagement.DeleteOnSubmit(ma);
                                }
                                dc1.SubmitChanges();
                            }
                        }
                    }
                    e.Cancel = true;
                    StoreLoad();
                    Ext.Msg.Alert("提示", "保存成功!").Show();

                }

            }
        }
        catch
        {
            Ext.Msg.Alert("提示", "数据处理异常，请刷新页面重新操作!").Show();
        }
    }
}