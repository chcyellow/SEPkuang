using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using GhtnTech.SecurityFramework;
using GhtnTech.SecurityFramework.BLL;
using GhtnTech.SecurityFramework.Model;
using GhtnTech.SecurityFramework.Utility;
using GhtnTech.SEP.DBUtility;
using Coolite.Ext.Web;
using GhtnTech.SEP.DAL;

using System.Xml;
using System.Xml.Xsl;

public partial class LeaderSearch_JTYHtotalbyperson : System.Web.UI.Page
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            dfBegin.SelectedDate = System.DateTime.Today.AddDays(1 - System.DateTime.Today.Day);
            dfEnd.SelectedDate = System.DateTime.Today;
            //dfBegin.MaxDate = System.DateTime.Today;
            //dfEnd.MaxDate = System.DateTime.Today;

            #region 初始化单位
            var KQ = from d in dc.Department
                     where d.Visualfield==3
                     select new
                     {
                         d.Deptname,
                         Deptid = d.Deptnumber
                     };
            KQStore.DataSource = KQ;
            KQStore.DataBind();
            #endregion
            //LoadData();
        }
    }
    protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)
    {
        LoadData();
    }
    [AjaxMethod]
    public void LoadData()
    {
        if (dfBegin.SelectedDate > dfEnd.SelectedDate)
        {
            Ext.Msg.Alert("提示", "日期选择有误!").Show();
            return;
        }
        Store1.DataSource = GetSafetyStatistics(dfBegin.SelectedDate, dfEnd.SelectedDate, cbbKQ.SelectedIndex == -1 ? "-1" : cbbKQ.SelectedItem.Value);
        Store1.DataBind();
    }

    private DataSet GetSafetyStatistics(DateTime dateBegin, DateTime dateEnd,string deptnm)
    {
        string strSql = string.Format("select distinct dept.deptnumber maindeptid,dept.deptname maindept,kq.deptnumber deptnumber,kq.deptname deptname,u.username,p.personnumber,p.name,pos.posname,pos.MOVEGBLEVEL,nvl(yh.xj,0) xj,nvl(yh.yh,0) yh,nvl(sw.sw,0) sw,nvl(dl.LoginCount,0) LoginCount from person p left join sf_user u on p.personnumber = u.personnumber left join department kq on p.areadeptid=kq.deptnumber left join department dept on p.maindeptid=dept.deptnumber left join position pos on p.posid=pos.posid left join (select personid,count(xj) xj,sum(xj) yh from ("+
            //修改隐患排查时间为排查人的个人排查时间
            "select nim.personid,nim.pctime,count(*) xj from nyhinput ni inner join NYHINPUT_MORE nim on ni.yhputinid=nim.yhputinid  where nim.pctime between to_date('{0}','YYYY-MM-DD') and to_date('{1}','YYYY-MM-DD') and nim.jctype=2 group by nim.personid,nim.pctime) group by personid ) yh on p.personnumber=yh.personid"+
            " left join (select PCPERSONID,count(*) sw from NSWINPUT where pctime between to_date('{2}','YYYY-MM-DD') and to_date('{3}','YYYY-MM-DD') group by PCPERSONID ) sw on p.personnumber=sw.PCPERSONID left join ( SELECT username,count(DISTINCT case when to_number(to_char(vuserlog.activetime,'hh24')) between 0 and 12 then TO_CHAR(vuserlog.activetime,'yyyy-mm-dd')||'上午' when to_number(to_char(vuserlog.activetime,'hh24')) between 12 and 24 then TO_CHAR(vuserlog.activetime,'yyyy-mm-dd')||'下午' end) LoginCount FROM vuserlog  where activetype='登录' and username !='yu' and vuserlog.activetime between to_date('{4}','YYYY-MM-DD') and to_date('{5}','YYYY-MM-DD') group by  vuserlog.username) dl on u.username = dl.username",
            dateBegin.ToString("yyyy-MM-dd"), dateEnd.ToString("yyyy-MM-dd"), dateBegin.ToString("yyyy-MM-dd"), dateEnd.ToString("yyyy-MM-dd"), dateBegin.ToString("yyyy-MM-dd"), dateEnd.ToString("yyyy-MM-dd"));
        strSql += " where p.visualfield='3'";
        if (deptnm != "-1")
        {
            strSql += string.Format(" and (kq.deptnumber='{0}'  or p.maindeptid='{1}')", deptnm, deptnm);
        }
        return OracleHelper.Query(strSql);

    }

    protected void Cell_Click(object sender, AjaxEventArgs e)
    {
        try//为null时出错
        {
            CellSelectionModel sm = this.GridPanel1.SelectionModel.Primary as CellSelectionModel;
            if (sm.SelectedCell.ColIndex == 0 || sm.SelectedCell.Value.Trim() == "0")
                return;
            string url = "";
            switch (sm.SelectedCell.Name.Trim())
            {
                case "XJ":
                    Window1.Width = 840;
                    Window1.Height = 422;
                    Window1.Title = "下井信息";
                    Window1.Html = GetXJDate(sm.SelectedCell.RecordID.Trim());
                    //url = string.Format("YHcondition.aspx?MainDeptID={0}&begin={1}&end={2}", sm.SelectedCell.RecordID.Trim(), dfBegin.SelectedDate.ToString("yyyy-MM-dd"), dfEnd.SelectedDate.ToString("yyyy-MM-dd"));
                    Window1.Show();
                    break;
                case "YH":
                    Window1.Width = 840;
                    Window1.Height = 422;
                    Window1.Title += "隐患信息";
                    url = string.Format("YHcondition.aspx?PCperson={0}&begin={1}&end={2}&jctype={3}", sm.SelectedCell.RecordID.Trim(), dfBegin.SelectedDate.ToString("yyyy-MM-dd"), dfEnd.SelectedDate.ToString("yyyy-MM-dd"), "2");
                    Window1.ClearContent();
                    Ext.DoScript("#{Window1}.load('" + url + "');");
                    Window1.Show();
                    break;
            }
        }
        catch
        {
        }
        //if (url != "")
        //{
        //    Window1.ClearContent();
        //    Ext.DoScript("#{Window1}.load('" + url + "');");
        //}
        
    }

    private string GetXJDate(string Personnumber)
    {
        var data = from i in dc.Nyhinput
                   from m in dc.NyhinputMore
                   where i.Yhputinid == m.Yhputinid && m.Personid == Personnumber && i.Jctype==2
                   && m.Pctime >= dfBegin.SelectedDate && m.Pctime <= dfEnd.SelectedDate
                   select new
                   {
                       i.Yhputinid,
                       i.Banci,
                       m.Pctime
                   };
        var xj = (from d in data
                  group d by new
                  {
                      d.Pctime,
                      d.Banci
                  } into g
                  select new
                  {
                      g.Key.Pctime,
                      g.Key.Banci,
                      Yhcount = g.Count()
                  }).OrderBy(p => p.Pctime);
        if (xj.Count() > 0)
        {
            string table = "<table><tr><td width=\"100px\">下井时间</td><td width=\"100px\">班次</td><td>录入隐患</td><tr>";
            foreach (var r in xj)
            {
                table += "<tr><td>" + r.Pctime.Value.ToString("yyyy-MM-dd") + "</td><td>" + r.Banci + "</td><td>" + r.Yhcount + "</td></tr>";
            }
            table += "</table>";
            return table;
        }

        return "没有下井信息";
    }

    //导出报表
    protected void ToExcel(object sender, EventArgs e)
    {
        string json = GridData.Value.ToString();
        foreach (var r in GridPanel1.ColumnModel.Columns)
        {

            json = json.Replace("\"" + r.DataIndex.Trim() + "\"", "\"" + r.Header + "\"");
        }
        json = json.Replace("T00:00:00", "");
        json = json.Replace("PERSONNUMBER", "工号");

        string strFileName = "安全统计报表";
        Response.Clear();
        Response.Buffer = true;
        Response.Charset = "GB2312";

        StoreSubmitDataEventArgs eSubmit = new StoreSubmitDataEventArgs(json, null);
        XmlNode xml = eSubmit.Xml;

        this.Response.Clear();
        this.Response.ContentType = "application nd.ms-excel";
        Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(strFileName) + ".xls");
        XslCompiledTransform xtExcel = new XslCompiledTransform();
        xtExcel.Load(Server.MapPath("ExcelSafetyStatistics.xsl"));
        xtExcel.Transform(xml, null, this.Response.OutputStream);
        this.Response.End();
    }
}