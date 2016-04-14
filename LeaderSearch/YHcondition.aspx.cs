using System;
using System.Collections;
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
using System.Xml.Linq;

public partial class LeaderSearch_YHcondition : BasePage
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            storeload();
        }
    }

    protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)
    {
        storeload();
    }


 
    protected void RowClick(object sender, AjaxEventArgs e)//流程处理
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        if (sm.SelectedRows.Count > 0)
        {
            btn_detail.Disabled = false;
        }
        else
        {
            btn_detail.Disabled = true;
        }
    }
    private void storeload()
    {
        //if (!SessionBox.GetUserSession().rolelevel.Contains("0") && !SessionBox.GetUserSession().rolelevel.Contains("1"))
        //{
        //    Ext.Msg.Alert("提示", "你没有权利查看此功能!");
        //    return;
        //}
        var query = from a in dc.Getyhinput
                    from m in dc.NyhinputMore//多人排查模块
                    from pp in dc.Person     //
                    from d in dc.Department 
                    from p in dc.Place
                    where a.Unitid == d.Deptnumber && a.Placeid==p.Placeid 
                    && a.Yhputinid==m.Yhputinid && m.Personid==pp.Personnumber //
                    select
                        new
                        {
                            a.Deptid,
                            p.Pareasid,
                            a.Placeid,
                            a.Banci,
                            a.Deptname,
                            a.Intime,
                            pp.Personnumber,//
                            pp.Name,        //
                            a.Pctime,
                            a.Placename,
                            a.Remarks,
                            a.Yhcontent,
                            Yhlevel=a.Levelname,
                            Yhtype=a.Typename,
                            a.Yhputinid,
                            a.Status,
                            Maindeptid=a.Unitid,
                            Maindeptname=d.Deptname,
                            a.Jctype
                        };

        if (!string.IsNullOrEmpty(Request["begin"]))
        {
            query = query.Where(p => (p.Pctime >= DateTime.Parse(this.Request["begin"].Trim()) && p.Pctime <= DateTime.Parse(this.Request["end"].Trim())));
        }
        else
        {
            GridPanel1.Title = "隐患信息";
        }
        if (!SessionBox.GetUserSession().rolelevel.Contains("0") && !SessionBox.GetUserSession().rolelevel.Contains("1"))
        {
            query = query.Where(p => (p.Maindeptid == SessionBox.GetUserSession().DeptNumber));
        }
        if (!string.IsNullOrEmpty(Request["DeptID"]))
        {
            query = query.Where(p => (p.Deptid == this.Request["DeptID"].Trim()));
        }
        if (!string.IsNullOrEmpty(Request["MainDeptID"]))
        {
            query = query.Where(p => (p.Maindeptid == this.Request["MainDeptID"].Trim()));
        }
        if (!string.IsNullOrEmpty(Request["status"]))
        {
            if (Request["status"].Trim() == "0" || Request["status"].Trim() == "1")
            {
                query = query.Where(p => (p.Status.Trim() == "复查通过" || p.Status.Trim() == "现场整改" ? 1 : 0) == int.Parse(this.Request["status"].Trim()));
            }
            if (Request["status"].Trim() == "2" || Request["status"].Trim() == "3")
            {
                query = query.Where(p => (p.Status.Trim() == "现场整改" ? 3 : 2) == int.Parse(this.Request["status"].Trim()));
            }
            if (Request["status"].Trim() == "4" || Request["status"].Trim() == "5" || Request["status"].Trim() == "6")
            {
                query = query.Where(p => (p.Jctype == int.Parse(this.Request["status"].Trim())-4));
            }
        }
        if(!string.IsNullOrEmpty(Request["PAreasID"]))
        {
            query = query.Where(p => p.Pareasid == int.Parse(this.Request["PAreasID"].Trim()));
        }
        if (!string.IsNullOrEmpty(Request["PlaceID"]))
        {
            query = query.Where(p => p.Placeid == int.Parse(this.Request["PlaceID"].Trim()));
        }
        if (!string.IsNullOrEmpty(Request["jctype"]))
        {
            query = query.Where(p => p.Jctype == int.Parse(this.Request["jctype"].Trim()));
        }
        if (!string.IsNullOrEmpty(Request["PCperson"]))
        {
            query = query.Where(p => p.Personnumber == this.Request["PCperson"].Trim());
        }
        if (!string.IsNullOrEmpty(Request["YHLevel"]))
        {
            query = query.Where(p => p.Yhlevel.Trim() == this.Request["YHLevel"].Trim());
            GridPanel1.Title = this.Request["YHLevel"].Trim()+"级隐患信息";
        }
        Store1.DataSource = query;
        Store1.DataBind();
    }

    [AjaxMethod]
    public void DetailLoad()//加载明细信息
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        string url = string.Format("../YSNewSearch/YhView.aspx?Yhid={0}", sm.SelectedRows[0].RecordID.Trim());
        Ext.DoScript("#{DetailWindow}.load('" + url + "');#{DetailWindow}.show();");
    }

    //[AjaxMethod]
    //public void DetailLoad()//加载明细信息
    //{
    //    //Response.Redirect("YHDetail.aspx");
    //    RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
    //    var input = dc.Yhinput.First(p => p.Yhputinid == Convert.ToInt32(sm.SelectedRows[0].RecordID.Trim()));
    //    int i = 0;
    //    switch (input.Status.Trim())
    //    {

    //        case "新增":
    //        case "提交审批":
    //        case "现场整改":
    //            i = 1;
    //            break;
    //        case "隐患未整改":
    //        case "隐患整改中":
    //        case "逾期未整改":
    //        case "逾期整改未完成":
    //            i = 2;
    //            break;
    //        case "隐患已整改":
    //            i = 3;
    //            break;
    //        case "复查通过":
    //        case "复查未通过":
    //            i = 4;
    //            break;
    //    }
    //    string id = sm.SelectedRows[0].RecordID.Trim();
    //    string s_url;
    //    s_url = "YHDetail.aspx?i=" + i + "&&id=" + id + "";
    //    Ext.DoScript("openwindow('" + s_url + "','MyFrm','610','400')");
    //}

    protected void ToExcel(object sender, EventArgs e)//导出报表
    {
        string json = GridData.Value.ToString();
        foreach (var r in GridPanel1.ColumnModel.Columns)
        {
            json = json.Replace("\"Name\"", "\"排查人\"");
            json = json.Replace("\"INTime\"", "\"录入时间\"");
            json = json.Replace("\"PCTime\"", "\"排查时间\"");
            json = json.Replace("\"" + r.DataIndex.Trim() + "\"", "\"" + r.Header + "\"");
        }
        json = json.Replace("T00:00:00", "");

        string strFileName = "隐患信息查询报表";
        Response.Clear();
        Response.Buffer = true;
        Response.Charset = "GB2312";

        StoreSubmitDataEventArgs eSubmit = new StoreSubmitDataEventArgs(json, null);
        XmlNode xml = eSubmit.Xml;

        this.Response.Clear();
        this.Response.ContentType = "application nd.ms-excel";
        Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(strFileName) + ".xls");
        XslCompiledTransform xtExcel = new XslCompiledTransform();
        xtExcel.Load(Server.MapPath("ExcelYH.xsl"));
        xtExcel.Transform(xml, null, this.Response.OutputStream);
        this.Response.End();
    }
}  
