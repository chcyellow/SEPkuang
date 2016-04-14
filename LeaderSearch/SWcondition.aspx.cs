using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Xsl;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Coolite.Ext.Web;
using GhtnTech.SEP.DAL;
using GhtnTech.SecurityFramework.BLL;

public partial class LeaderSearch_SWcondition : BasePage
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            storeload();
        }
    }
    protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)
    {
        storeload();
    }
    private void storeload()//执行查询
    {
        var query = from a in dc.Nswinput
                    from s in dc.Getswandhazusing
                    from md in dc.Department
                    from sp in dc.Person
                    from sd in dc.Department
                    from pp in dc.Person
                    from pd in dc.Department
                    join pl in dc.Place on a.Placeid equals pl.Placeid into gg
                    from ga in gg.DefaultIfEmpty()
                    where a.Swid == s.Swid && a.Maindeptid==s.Deptnumber
                    && a.Maindeptid == md.Deptnumber 
                    && a.Swpersonid == sp.Personnumber && sp.Areadeptid == sd.Deptnumber 
                    && a.Pcpersonid == pp.Personnumber && pp.Deptid == pd.Deptnumber
                    orderby a.Pctime descending
                    select new
                    {
                        a.Id,
                        a.Maindeptid,
                        Maindeptname = md.Deptname,
                        Deptid = sd.Deptnumber,
                        Pareasid=ga.Pareasid==null?-1:ga.Pareasid,
                        a.Placeid,
                        a.Banci,
                        a.Intime,
                        a.Pctime,
                        s.Swcontent,
                        Placename=ga.Placename==null?"已删除":ga.Placename,
                        Swpoints = s.Kcscore,
                        Swlevel = s.Levelname,
                        a.Pcpersonid,
                        Pcname = pp.Name,
                        Pcdeptname = pd.Deptname,
                        a.Swpersonid,
                        Swperson = sp.Name,
                        sd.Deptname,
                        Isend = a.Isend == 1 ? true : false,
                        Ispublic = a.Ispublic == 1 ? true : false
                    };
        if (!SessionBox.GetUserSession().rolelevel.Contains("0") && !SessionBox.GetUserSession().rolelevel.Contains("1"))
        {
            query = query.Where(p => (p.Maindeptid == SessionBox.GetUserSession().DeptNumber));
        }
        if (!string.IsNullOrEmpty(Request["begin"]))
        {
            DateTime bg = DateTime.Parse(Request["begin"].Trim());
            DateTime end = DateTime.Parse(Request["end"].Trim());
            query = query.Where(p => p.Pctime >= bg && p.Pctime <= end);
        }
        if (!string.IsNullOrEmpty(Request["DeptID"]))
        {
            query = query.Where(p => (p.Deptid == this.Request["DeptID"].Trim()));
        }
        if (!string.IsNullOrEmpty(Request["MainDeptID"]))
        {
            query = query.Where(p => (p.Maindeptid == this.Request["MainDeptID"].Trim()));
        }
        if (!string.IsNullOrEmpty(Request["PAreasID"]))
        {
            query = query.Where(p => p.Pareasid == int.Parse(this.Request["PAreasID"].Trim()));
        }
        if (!string.IsNullOrEmpty(Request["PlaceID"]))
        {
            query = query.Where(p => p.Placeid == int.Parse(this.Request["PlaceID"].Trim()));
        }
        if (!string.IsNullOrEmpty(Request["PCperson"]))
        {
            query = query.Where(p => p.Pcpersonid == this.Request["PCperson"].Trim());
        }
        if (!string.IsNullOrEmpty(Request["SWperson"]))
        {
            query = query.Where(p => p.Swpersonid == this.Request["SWperson"].Trim());
        }
        if (!string.IsNullOrEmpty(Request["SWLevel"]))
        {
            query = query.Where(p => p.Swlevel.Trim() == this.Request["SWLevel"].Trim());
            GridPanel1.Title = this.Request["SWLevel"].Trim() + "级别‘三违’信息";
        }
        Store1.DataSource = query;
        Store1.DataBind();
    }

    protected void ToExcel(object sender, EventArgs e)//导出报表
    {
        string json = GridData.Value.ToString();
        foreach (var r in GridPanel1.ColumnModel.Columns)
        {
            json = json.Replace("\"Name\"", "\"排查人\"");
            //json = json.Replace("\"INTime\"", "\"录入时间\"");
            json = json.Replace("\"PCTime\"", "\"排查时间\"");
            json = json.Replace("\"" + r.DataIndex.Trim() + "\"", "\"" + r.Header + "\"");
        }
        json = json.Replace("T00:00:00", "");

        string strFileName = "三违信息查询报表";
        Response.Clear();
        Response.Buffer = true;
        Response.Charset = "GB2312";

        StoreSubmitDataEventArgs eSubmit = new StoreSubmitDataEventArgs(json, null);
        XmlNode xml = eSubmit.Xml;

        this.Response.Clear();
        this.Response.ContentType = "application nd.ms-excel";
        Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(strFileName) + ".xls");
        XslCompiledTransform xtExcel = new XslCompiledTransform();
        xtExcel.Load(Server.MapPath("ExcelSW.xsl"));
        xtExcel.Transform(xml, null, this.Response.OutputStream);
        this.Response.End();
    }

}
