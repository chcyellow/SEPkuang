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

public partial class LeaderSearch_SafetyStatistics : BasePage
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            UserHandle.InitModule(this.PageTag);//初始化此模块的权限。
            if (UserHandle.ValidationHandle(PermissionTag.Browse))//是否有浏览权限
            {
                dfBegin.SelectedDate = System.DateTime.Today.AddDays(1 - System.DateTime.Today.Day);
                dfEnd.SelectedDate = System.DateTime.Today;
                //dfBegin.MaxDate = System.DateTime.Today;
                //dfEnd.MaxDate = System.DateTime.Today;

                #region 初始化单位
                MainDeptStore.DataSource = PublicCode.GetMaindept("");
                MainDeptStore.DataBind();
                KQStore.DataSource = PublicCode.GetKQdept(SessionBox.GetUserSession().DeptNumber);
                KQStore.DataBind();
                if (SessionBox.GetUserSession().rolelevel.Contains("1") || SessionBox.GetUserSession().rolelevel.Contains("0"))
                {
                    cbbKQ.Disabled = true;

                }
                else
                {
                    cbbMianDept.SelectedItem.Value = SessionBox.GetUserSession().DeptNumber;
                    cbbMianDept.Disabled = true;
                }

                #endregion
                //LoadData();
                if(UserHandle.ValidationHandle(PermissionTag.SearchAll))
                {
                    cbbMianDept.Disabled = false;
                    cbbKQ.Disabled = false;
                    btnSearch.Disabled = false;
                }
                else if (UserHandle.ValidationHandle(PermissionTag.SearchMainDept))
                {
                    cbbMianDept.SelectedItem.Value = SessionBox.GetUserSession().DeptNumber;
                    cbbMianDept.Disabled = true;
                    cbbKQ.Disabled = false;
                    btnSearch.Disabled = false;
                }
                else if (UserHandle.ValidationHandle(PermissionTag.SearchDept))
                {
                    cbbMianDept.SelectedItem.Value = SessionBox.GetUserSession().DeptNumber;
                    cbbMianDept.Disabled = true;
                    cbbKQ.SelectedItem.Value = PublicCode.GetKQdeptNumber(SessionBox.GetUserSession().PersonNumber);
                    cbbKQ.Disabled = true;
                    btnSearch.Disabled = false;
                    
                }
                else if (UserHandle.ValidationHandle(PermissionTag.SearchPersonal))
                {
                    cbbMianDept.SelectedItem.Value = SessionBox.GetUserSession().DeptNumber;
                    cbbMianDept.Disabled = true;
                    cbbKQ.SelectedItem.Value = PublicCode.GetKQdeptNumber(SessionBox.GetUserSession().PersonNumber);
                    cbbKQ.Disabled = true;
                    btnSearch.Disabled = false;
                }
                else
                {
                    btnSearch.Disabled = true;
                }

            }
            else
            {
                Session["ErrorNum"] = "0";
                Response.Redirect("~/Error.aspx");
            }
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
        bool bl=false;
        if (SessionBox.GetUserSession().rolelevel.Contains("1") || SessionBox.GetUserSession().rolelevel.Contains("0"))
        {
            bl = true;
        }
        Store1.DataSource=GetSafetyStatistics(dfBegin.SelectedDate, dfEnd.SelectedDate, cbbMianDept.SelectedIndex == -1 ? "-1" : cbbMianDept.SelectedItem.Value, cbbKQ.SelectedIndex == -1 ? "-1" : cbbKQ.SelectedItem.Value, bl);
        Store1.DataBind();
    }

    private DataSet GetSafetyStatistics(DateTime dateBegin, DateTime dateEnd, string maindept, string deptnm, bool isLeader)
    {
        string strSql = string.Format("select distinct dept.deptnumber maindeptid,dept.deptname maindept,kq.deptnumber deptnumber,kq.deptname deptname,u.username,p.personnumber,p.name,pos.posname,pos.MOVEGBLEVEL,nvl(yh.xj,0) xj,nvl(yh.yh,0) yh,nvl(yh.xc,0) YXC,nvl(yh.fxc,0) fxc,nvl(yh.ybh,0) ybh,nvl(yh.wbh,0) wbh,nvl(sw.sw,0) sw,nvl(dl.LoginCount,0) LoginCount from sf_user u inner join person p on u.personnumber = p.personnumber left join department kq on p.areadeptid=kq.deptnumber left join department dept on p.maindeptid=dept.deptnumber left join position pos on p.posid=pos.posid left join (select personid,count(xj) xj,sum(xj) yh ,sum(xc) xc,sum(fxc) fxc,sum(ybh) ybh,sum(wbh) wbh from ("+
            //修改隐患排查时间为排查人的个人排查时间
            "select nim.personid,nim.pctime,count(*) xj,sum(case when ni.status='现场整改' then 1 else 0 end) xc,sum(case when ni.status='现场整改' then 0 else 1 end) fxc,sum(case when ni.status='现场整改' or ni.status='复查通过' then 1 else 0 end) ybh,sum(case when ni.status='现场整改' or ni.status='复查通过' then 0 else 1 end) wbh from nyhinput ni inner join NYHINPUT_MORE nim on ni.yhputinid=nim.yhputinid  where ni.status not in ('新增','提交审批') and nim.pctime between to_date('{0}','YYYY-MM-DD') and to_date('{1}','YYYY-MM-DD') group by nim.personid,nim.pctime) group by personid ) yh on p.personnumber=yh.personid" +
            " left join (select PCPERSONID,count(*) sw from NSWINPUT where pctime between to_date('{2}','YYYY-MM-DD') and to_date('{3}','YYYY-MM-DD') group by PCPERSONID ) sw on p.personnumber=sw.PCPERSONID left join ( SELECT username,count(DISTINCT case when to_number(to_char(vuserlog.activetime,'hh24')) between 0 and 12 then TO_CHAR(vuserlog.activetime,'yyyy-mm-dd')||'上午' when to_number(to_char(vuserlog.activetime,'hh24')) between 12 and 24 then TO_CHAR(vuserlog.activetime,'yyyy-mm-dd')||'下午' end) LoginCount FROM vuserlog  where activetype='登录' and username !='yu' and vuserlog.activetime between to_date('{4}','YYYY-MM-DD') and to_date('{5}','YYYY-MM-DD') group by  vuserlog.username) dl on u.username = dl.username",
            dateBegin.ToString("yyyy-MM-dd"), dateEnd.ToString("yyyy-MM-dd"), dateBegin.ToString("yyyy-MM-dd"), dateEnd.ToString("yyyy-MM-dd"),dateBegin.ToString("yyyy-MM-dd"), dateEnd.ToString("yyyy-MM-dd"));
        strSql += " where 1=1";
        if (UserHandle.ValidationHandle(PermissionTag.SearchAll))
        {
            if (maindept != "-1")
            {
                strSql += string.Format(" and dept.deptnumber='{0}'", maindept);
            }
            if (deptnm != "-1")
            {
                strSql += string.Format(" and kq.deptnumber='{0}'", deptnm);
            }
            if (isLeader)
            {
                strSql += " and pos.movegblevel='矿领导'"; //or pos.posname like '%矿长' or pos.posname like '%副总')";
            }
        }
        else if (UserHandle.ValidationHandle(PermissionTag.SearchMainDept))
        {

            strSql += string.Format(" and dept.deptnumber='{0}'", SessionBox.GetUserSession().DeptNumber);
            
            if (deptnm != "-1")
            {
                strSql += string.Format(" and kq.deptnumber='{0}'", deptnm);
            }
            if (isLeader)
            {
                strSql += " and pos.movegblevel='矿领导'"; //or pos.posname like '%矿长' or pos.posname like '%副总')";
            }
        }
        else if (UserHandle.ValidationHandle(PermissionTag.SearchDept))
        {

            strSql += string.Format(" and dept.deptnumber='{0}'", SessionBox.GetUserSession().DeptNumber);

            strSql += string.Format(" and kq.deptnumber='{0}'", PublicCode.GetKQdeptNumber(SessionBox.GetUserSession().PersonNumber));
            
            if (isLeader)
            {
                strSql += " and pos.movegblevel='矿领导'"; //or pos.posname like '%矿长' or pos.posname like '%副总')";
            }
        }
        else if (UserHandle.ValidationHandle(PermissionTag.SearchPersonal))
        {
            strSql += string.Format(" and p.personnumber='{0}'", SessionBox.GetUserSession().PersonNumber);
        }
        else
        {
            return new DataSet();
        }
        
        return OracleHelper.Query(strSql);

    }

    protected void Cell_Click(object sender, AjaxEventArgs e)
    {
        CellSelectionModel sm = this.GridPanel1.SelectionModel.Primary as CellSelectionModel;
        if (sm.SelectedCell.ColIndex < 5 || sm.SelectedCell.Value.Trim() == "0")
            return;
        string url = "";
        switch (sm.SelectedCell.Name.Trim())
        {
            case "XJ":
                Window1.Width = 840;
                Window1.Height = 422;
                Window1.Title = "下井信息";
                Window1.Html = GetXJDate(sm.SelectedCell.RecordID.Trim());
                Window1.Show();
                //url = string.Format("YHcondition.aspx?MainDeptID={0}&begin={1}&end={2}", sm.SelectedCell.RecordID.Trim(), dfBegin.SelectedDate.ToString("yyyy-MM-dd"), dfEnd.SelectedDate.ToString("yyyy-MM-dd"));
                break;
            case "YH":
                Window1.Width = 840;
                Window1.Height = 422;
                Window1.Title += "隐患信息";
                url = string.Format("YHcondition.aspx?PCperson={0}&begin={1}&end={2}", sm.SelectedCell.RecordID.Trim(), dfBegin.SelectedDate.ToString("yyyy-MM-dd"), dfEnd.SelectedDate.ToString("yyyy-MM-dd"));
                Window1.Show();
                break;
            case "YXC":
                Window1.Width = 840;
                Window1.Height = 422;
                Window1.Title += "现场整改隐患信息";
                url = string.Format("YHcondition.aspx?PCperson={0}&begin={1}&end={2}&status=3", sm.SelectedCell.RecordID.Trim(), dfBegin.SelectedDate.ToString("yyyy-MM-dd"), dfEnd.SelectedDate.ToString("yyyy-MM-dd"));
                Window1.Show();
                break;
            case "FXC":
                Window1.Width = 840;
                Window1.Height = 422;
                Window1.Title += "非现场整改隐患信息";
                url = string.Format("YHcondition.aspx?PCperson={0}&begin={1}&end={2}&status=2", sm.SelectedCell.RecordID.Trim(), dfBegin.SelectedDate.ToString("yyyy-MM-dd"), dfEnd.SelectedDate.ToString("yyyy-MM-dd"));
                Window1.Show();
                break;
            case "YBH":
                Window1.Width = 840;
                Window1.Height = 422;
                Window1.Title += "已闭合隐患信息";
                url = string.Format("YHcondition.aspx?PCperson={0}&begin={1}&end={2}&status=1", sm.SelectedCell.RecordID.Trim(), dfBegin.SelectedDate.ToString("yyyy-MM-dd"), dfEnd.SelectedDate.ToString("yyyy-MM-dd"));
                Window1.Show();
                break;
            case "WBH":
                Window1.Width = 840;
                Window1.Height = 422;
                Window1.Title += "未闭合隐患信息";
                url = string.Format("YHcondition.aspx?PCperson={0}&begin={1}&end={2}&status=0", sm.SelectedCell.RecordID.Trim(), dfBegin.SelectedDate.ToString("yyyy-MM-dd"), dfEnd.SelectedDate.ToString("yyyy-MM-dd"));
                Window1.Show();
                break;
            case "SW":
                Window1.Width = 890;
                Window1.Height = 400;
                Window1.Title += "三违信息";
                url = string.Format("SWcondition.aspx?PCperson={0}&begin={1}&end={2}", sm.SelectedCell.RecordID.Trim(), dfBegin.SelectedDate.ToString("yyyy-MM-dd"), dfEnd.SelectedDate.ToString("yyyy-MM-dd"));
                Window1.Show();
                break;
            case "LOGINCOUNT":
                return;
        }
        if (url != "")
        {
            Window1.ClearContent();
            Ext.DoScript("#{Window1}.load('" + url + "');");
        }
       // Window1.Show();
    }

    private string GetXJDate(string Personnumber)
    {
        var data = from i in dc.Nyhinput
                   from m in dc.NyhinputMore
                   where i.Yhputinid == m.Yhputinid && m.Personid == Personnumber
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
                 }).OrderBy(p=>p.Pctime);
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