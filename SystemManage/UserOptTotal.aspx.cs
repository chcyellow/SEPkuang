using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxEditors;
using GhtnTech.SecurityFramework;
using GhtnTech.SecurityFramework.BLL;
using GhtnTech.SecurityFramework.Model;
using GhtnTech.SecurityFramework.Utility;
using GhtnTech.SEP.DBUtility;
public partial class SystemManage_UserOptTotal : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!SessionBox.CheckUserSession())
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                dateBegin.Date = DateTime.Parse(System.DateTime.Now.Year + "-" + System.DateTime.Now.Month + "-01");
                dateEnd.Date = System.DateTime.Today;
                InitData();
            }
        }
    }
    protected void InitData()
    {
        GetDeptData();
        GetKQData();
        GetTotalData();
    }
    private void GetDeptData()
    {

        Util.BindDll(cboMainDept, "(substr(deptnumber,1,2)='23' or substr(deptnumber,1,2)='24' or substr(deptnumber,1,2)='13'  or substr(deptnumber,1,3)='552' or substr(deptnumber,1,4)='5503') and substr(deptnumber,5,5)='00000'", "DEPTNAME", "DEPTNUMBER");
        cboMainDept.Items.Insert(0, new ListEditItem("--全部--", "-1"));
        if (!SessionBox.GetUserSession().rolelevel.Contains("1") && !SessionBox.GetUserSession().rolelevel.Contains("0"))
        {
            cboMainDept.Value = SessionBox.GetUserSession().DeptNumber;
            cboMainDept.Enabled = false;
        }
        else
        {
            cboMainDept.Value = "-1";
        }
    }

    private void GetKQData()
    {
        if (cboMainDept.Value.ToString() != "-1")
        {
            Util.BindDll(cboDept, "ID", cboMainDept.Value.ToString().Remove(4), "NAME", "ID");
            cboDept.Items.Insert(0, new ListEditItem("--全部--", "-1"));
            cboDept.Value = "-1";
        }
        else
        {
            cboDept.Items.Clear();
            cboDept.Items.Insert(0, new ListEditItem("--全部--", "-1"));
            cboDept.Value = "-1";
        }
    }
    private void GetTotalData()
    {
        if (SessionBox.GetUserSession().rolelevel.Contains("0"))
        {
            Bind(cboMainDept.Value.ToString().Trim(), cboDept.Value.ToString().Trim(), txtName.Text.Trim(),txtUser.Text.Trim());
            //Bind(cboMainDept.Value.ToString().Trim() == "-1" ? "" : cboMainDept.Value.ToString().Trim(), cboDept.Value.ToString().Trim() == "-1" ? "" : cboDept.Value.ToString().Trim(), txtName.Text.Trim());
        }
        else if (SessionBox.GetUserSession().rolelevel.Contains("1") && (!SessionBox.GetUserSession().rolelevel.Contains("2")))
        {
            Bind(cboMainDept.Value.ToString().Trim(), cboDept.Value.ToString().Trim(), txtName.Text.Trim(), txtUser.Text.Trim());
            //Bind(cboMainDept.Value.ToString().Trim() == "-1" ? null : cboMainDept.Value.ToString().Trim(), cboDept.Value.ToString().Trim() == "-1" ? null : cboDept.Value.ToString().Trim(), txtName.Text.Trim() == "" ? null : txtName.Text.Trim());

        }
        else
        {
            cboMainDept.Value = SessionBox.GetUserSession().DeptNumber;
            cboMainDept.Enabled = false;

            Bind(SessionBox.GetUserSession().DeptNumber.Trim(), cboDept.Value.ToString().Trim(), txtName.Text.Trim(), txtUser.Text.Trim());

        }
    }
    private void Bind(string maindept, string deptnm, string psn)
    {
        DataSet ds = GetKaoHeInfo.GetUserOptTotal(dateBegin.Date, dateEnd.Date, maindept, deptnm, psn);
        gvUserOptTotal.DataSource = ds;
        gvUserOptTotal.DataBind();
    }
    private void Bind(string maindept, string deptnm, string psn,string username)
    {
        DataSet ds = GetUserOptTotal(dateBegin.Date, dateEnd.Date, maindept, deptnm, psn,username);
        gvUserOptTotal.DataSource = ds;
        gvUserOptTotal.DataBind();
    }
    protected void cboMainDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetKQData();
    }
    protected void btnSure_Click(object sender, EventArgs e)
    {
        GetTotalData();
    }
    protected void gvUserOptTotal_PageIndexChanged(object sender, EventArgs e)
    {
        GetTotalData();
    }
    protected void gvUserOptTotal_BeforeColumnSortingGrouping(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
    {
        GetTotalData();
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        GetTotalData();
        gridExport.WriteXlsToResponse();
    }

    
    private DataSet GetUserOptTotal(DateTime dateBegin, DateTime dateEnd, string maindept, string deptnm, string psn, string username)
    {
        string strSql = string.Format("select r.username,r.personnumber,r.name,r.deptnumber,r.deptname,r.maindeptid,r.maindept,nvl(w.activepage,'无') activepage,nvl(w.pageCount,0) pageCount from (select distinct u.userid,u.username,u.personnumber,p.name,kq.deptnumber deptnumber,kq.deptname deptname,dept.deptnumber maindeptid,dept.deptname maindept from sf_user u join person p on u.personnumber = p.personnumber left join department kq on p.areadeptid=kq.deptnumber left join department dept on p.maindeptid=dept.deptnumber) r left join ( SELECT username,activepage,count(vuserlog.activepage) pageCount FROM vuserlog where ActiveTime between to_date('{0}','YYYY-MM-DD') and to_date('{1}','YYYY-MM-DD') and activetype='浏览' group by  vuserlog.username,activepage) w on r.username = w.username where r.username !='yu'", dateBegin.ToShortDateString(), dateEnd.ToShortDateString());
        if (maindept != "-1")
        {
            strSql += string.Format(" and r.maindeptid='{0}'", maindept);
        }
        if (deptnm != "-1")
        {
            strSql += string.Format(" and r.deptnumber='{0}'", deptnm);
        }
        if (psn != "")
        {
            strSql += string.Format(" and r.name like'%{0}%'", psn);
        }
        if (username != "")
        {
            strSql += string.Format(" and r.username like'%{0}%'", username);
        }
        return OracleHelper.Query(strSql);
    }
    
}
