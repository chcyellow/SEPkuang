using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GhtnTech.SecurityFramework;
using GhtnTech.SecurityFramework.Model;
using GhtnTech.SecurityFramework.BLL;
using GhtnTech.SecurityFramework.Utility;
using GhtnTech.SEP.DBUtility;
public partial class SafeCheckSet_YHSmsSet : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!SessionBox.CheckUserSession())
        {
            Response.Redirect("~/Login.aspx");
        }
        else
        {

            InitData(string.Format("maindept='{0}'", SessionBox.GetUserSession().DeptNumber));
            //gvYHSmsSet.GroupBy(gvYHSmsSet.Columns["INFONAME"]);
            //gvYHSmsSet.ExpandAll();
            
        }
    }

    internal void InitData(string where)
    {
        string strSql = "";
        if(where == "")
        {
            strSql = "select YHSMSSETID,INFONAME,NAME,DEPTNAME from yhsmsreceiver";
        }
        else
        {
            strSql = "select YHSMSSETID,INFONAME,NAME,DEPTNAME from yhsmsreceiver where " + where;
        }

        gvYHSmsSet.DataSource = OracleHelper.Query(strSql);
        gvYHSmsSet.DataBind();
    }

    private void DeleteData(object id)
    {
        string strSql = "delete from yhsmsset where yhsmssetid=" + id;
        OracleHelper.ExecuteSql(strSql);
    }
    protected void gvYHSmsSet_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
    {
        DeleteData(e.Keys[0]);
        e.Cancel = true;
        InitData(string.Format("maindept='{0}'" , SessionBox.GetUserSession().DeptNumber));
    }
}
