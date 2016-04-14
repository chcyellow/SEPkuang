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
using System.Xml.Xsl;

public partial class MovePlan_ReviewYHPrint : System.Web.UI.Page
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            placeLoad();

            btnPrint.Visible = false;
        }
    }

    [AjaxMethod]
    public void LoadPlanPlace()
    {
        bindPlan();
    }

    private void placeLoad()
    {
        var place = from pl in dc.Place
                    join a in dc.Placeareas on pl.Pareasid equals a.Pareasid into bb
                    from ba in bb.DefaultIfEmpty()
                    where pl.Maindeptid == SessionBox.GetUserSession().DeptNumber 
                    select new
                    {
                        pl.Placeid,
                        pl.Placename,
                        py = dc.F_PINYIN(pl.Placename).ToLower(),
                        ba.Pareasname
                    };
        Store1.DataSource = place;
        Store1.DataBind();

        //bindPlan();
        bindYH(GetPlanPlace());
    }

    //绑定走动计划
    private void bindPlan()
    {
        var sqltext = (from m in dc.Moveplan
                      where m.Personid == SessionBox.GetUserSession().PersonNumber 
                      //&& (m.Starttime.Value <= DateTime.Parse("2011-05-03") && DateTime.Parse("2011-05-03") <= m.Endtime.Value)
                      && (m.Starttime.Value <= System.DateTime.Today && System.DateTime.Today <= m.Endtime.Value)
                      select new
                      {
                          m.Placeid
                      }).Distinct();
        if (sqltext.Count() > 0)
        {
            string group = "[";
            foreach (var r in sqltext)
            {
                group += r.Placeid + ",";
            }
            group = group.Substring(0, group.Length - 1) + "]";
            Ext.DoScript("CountrySelector.addByNames(" + group + ");");
        }
    }

    private decimal[] GetPlanPlace()
    {
        var sqltext = (from m in dc.Moveplan
                       where m.Personid == SessionBox.GetUserSession().PersonNumber 
                       //&& (m.Starttime.Value <= DateTime.Parse("2011-05-03") && DateTime.Parse("2011-05-03") <= m.Endtime.Value)
                       && (m.Starttime.Value <= System.DateTime.Today && System.DateTime.Today <= m.Endtime.Value)
                       select new
                       {
                           m.Placeid
                       }).Distinct();
        if (sqltext.Count() > 0)
        {
            decimal[] place = new decimal[sqltext.Count()]; int i = 0;
            foreach (var r in sqltext)
            {
                place[i]= r.Placeid.Value ;
                i++;
            }
            return place;
        }
        return new decimal[] { -1 };
    }

    //绑定待复查隐患
    private void bindYH(decimal[] place)
    {
        var query = from a in dc.Getyhinput
                    where a.Status == "隐患已整改" && place.Contains(a.Placeid.Value) && a.Unitid == SessionBox.GetUserSession().DeptNumber 
                    orderby a.Placename
                    select
                        new
                        {
                            BanCi = a.Banci,
                            DeptName = a.Deptname,
                            INTime = a.Intime,
                            a.Name,
                            PCTime = a.Pctime,
                            PlaceName = a.Placename,
                            Remarks = a.Remarks,
                            YHContent = a.Yhcontent,
                            YHLevel = a.Levelname,
                            YHType = a.Typename,
                            YHPutinID = a.Yhputinid
                        };
        YHStore.DataSource = query;
        YHStore.DataBind();
        btnExcel.Disabled = query.Count() > 0 ? false : true;
        btnPrint.Disabled = query.Count() > 0 ? false : true;

    }

    //导出报表
    protected void ToExcel(object sender, EventArgs e)
    {
        string json = GridData.Value.ToString();
        foreach (var r in GridPanel3.ColumnModel.Columns)
        {

            json = json.Replace("\"" + r.DataIndex.Trim() + "\"", "\"" + r.Header + "\"");
        }
        json = json.Replace("T00:00:00", "");

        string strFileName = "待复查隐患信息详情报表";
        Response.Clear();
        Response.Buffer = true;
        Response.Charset = "GB2312";

        StoreSubmitDataEventArgs eSubmit = new StoreSubmitDataEventArgs(json, null);
        XmlNode xml = eSubmit.Xml;

        this.Response.Clear();
        this.Response.ContentType = "application nd.ms-excel";
        Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(strFileName) + ".xls");
        XslCompiledTransform xtExcel = new XslCompiledTransform();
        xtExcel.Load(Server.MapPath("Excel.xsl"));
        xtExcel.Transform(xml, null, this.Response.OutputStream);
        this.Response.End();
    }

    //打印报表
    protected void ToPrint(object sender, EventArgs e)
    {
        string json = GridData.Value.ToString();
        foreach (var r in GridPanel3.ColumnModel.Columns)
        {

            json = json.Replace("\"" + r.DataIndex.Trim() + "\"", "\"" + r.Header + "\"");
        }
        json = json.Replace("T00:00:00", "");

        //string strFileName = "待复查隐患信息详情报表";
        //Response.Clear();
        //Response.Buffer = true;
        //Response.Charset = "GB2312";

        //StoreSubmitDataEventArgs eSubmit = new StoreSubmitDataEventArgs(json, null);
        //XmlNode xml = eSubmit.Xml;

        //this.Response.Clear();
        //this.Response.ContentType = "application nd.ms-excel";
        //Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(strFileName) + ".xls");
        //XslCompiledTransform xtExcel = new XslCompiledTransform();
        //xtExcel.Load(Server.MapPath("Excel.xsl"));
        //xtExcel.Transform(xml, null, this.Response.OutputStream);
        //this.Response.End();
    }

    protected void SubmitData(object sender, StoreSubmitDataEventArgs e)
    {
        //string json = e.Json;
        XmlNode xml = e.Xml;
        XmlNode rxml = xml.SelectSingleNode("records");
        XmlNodeList uRecords = rxml.SelectNodes("record");
        if (uRecords.Count > 0)
        {
            decimal[] place = new decimal[uRecords.Count]; int i = 0;
            foreach (XmlNode record in uRecords)
            {
                if (record != null)
                {
                    place[i] = decimal.Parse(record.SelectSingleNode("Placeid").InnerText.Trim());
                    i++;
                }
            }
            bindYH(place);
        }
        else
        {
            bindYH(new decimal[] { -1 });
        }
    }

}
