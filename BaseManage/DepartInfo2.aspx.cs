using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxTreeList;
using GhtnTech.SecurityFramework;
using GhtnTech.SecurityFramework.BLL;
using GhtnTech.SecurityFramework.Model;
using GhtnTech.SecurityFramework.Utility;
using GhtnTech.SEP.DAL;
using GhtnTech.SEP.DBUtility;
public partial class BaseManage_DepartInfo2 : BasePage
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!SessionBox.CheckUserSession())
        {
            Response.Redirect("~/Login.aspx");
        }
        else
        {
             UserHandle.InitModule(this.PageTag);//初始化此模块的权限。
             if (UserHandle.ValidationHandle(PermissionTag.Browse))//是否有浏览权限
             {
                 InitData();
                 if (!UserHandle.ValidationHandle(PermissionTag.Edit))
                 {
                     btnDisable.Visible = false;
                     btnEnable.Visible = false;
                     DepTreeList.Columns["操作"].Visible = false;
                 }
             }
             else
             {
                 Session["ErrorNum"] = "0";
                 Response.Redirect("~/Error.aspx");
             }
        }

    }

    private void InitData()
    {
        //List<string> lstRole = new List<string>();
        //lstRole.Add("2");
        //lstRole.Add("46");
        //if (SessionBox.GetUserSession().CurrentRole[0].Split(',')[0] == "31")
        //{
        //    BindData("");
        //}
        //else if (lstRole.Contains(SessionBox.GetUserSession().CurrentRole[0].Split(',')[0]))
        //{
        //    BindData("");
        //}
        //else 
        if (SessionBox.GetUserSession().rolelevel.Trim().IndexOf("1") > -1)
        {
            BindData("");
        }
        else
        {
            BindData(string.Format("DEPTNUMBER like'{0}%'", SessionBox.GetUserSession().DeptNumber.Remove(4)));
        }
    }

    private void BindData(string where)
    {
        Session["FID"] = PublicMethod.ReadXmlReturnNode("ZY", this);
        DepartmentBll dept = new DepartmentBll();
        DepTreeList.DataSource = dept.GetList(where);
        DepTreeList.DataBind();
    }

    private void UpdateDept(object typeid, object deptstatus, object deptnumber)
    {
        string strSql = string.Empty;
        if (typeid != null)
        {
            strSql = string.Format("update department set typeid={0},deptstatus='{1}' where deptnumber='{2}'", typeid, deptstatus, deptnumber);
        }
        else
        {
            strSql = string.Format("update department set deptstatus='{0}' where deptnumber='{1}'", deptstatus, deptnumber);
        }
        OracleHelper.ExecuteSql(strSql);
    }
    protected void DepTreeList_NodeUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
    {
        UpdateDept(e.NewValues["TYPEID"],  e.NewValues["DEPTSTATUS"],e.OldValues["DEPTNUMBER"]);
        e.Cancel = true;
        (sender as DevExpress.Web.ASPxTreeList.ASPxTreeList).CancelEdit();
        InitData();
    }
    protected void btnEnable_Click(object sender, EventArgs e)
    {
        if (DepTreeList.SelectionCount <= 0)
        {
            JSHelper.Alert("请至少选择一条记录！", this);
        }
        else
        {
            List<string> list = new List<string>();//建立事务列表
            foreach (var item in DepTreeList.GetSelectedNodes())
            {
                list.Add(string.Format("update department set deptstatus='1' where deptnumber='{0}'", item.Key));
            }
            try
            {
                OracleHelper.ExecuteSqlTran(list);
                InitData();
                DepTreeList.UnselectAll();
                JSHelper.Alert("更新成功！", this);
            }
            catch (Exception ex)
            {
                JSHelper.Alert("出现异常，更新失败！请与管理员联系！", this);
            }
        }
    }
    protected void btnDisable_Click(object sender, EventArgs e)
    {
        if (DepTreeList.SelectionCount <= 0)
        {
            JSHelper.Alert("请至少选择一条记录！", this);
        }
        else
        {
            List<string> list = new List<string>();//建立事务列表
            foreach (var item in DepTreeList.GetSelectedNodes())
            {
                list.Add(string.Format("update department set deptstatus=' ' where deptnumber='{0}'", item.Key));
            }
            try
            {
                OracleHelper.ExecuteSqlTran(list);
                InitData();
                DepTreeList.UnselectAll();
                JSHelper.Alert("更新成功！", this);
            }
            catch (Exception ex)
            {
                JSHelper.Alert("出现异常，更新失败！请与管理员联系！", this);
            }
        }
    }

    protected void GetType()
    {
        Session["FID"] = PublicMethod.ReadXmlReturnNode("ZY", this);
        var type = from t in dc.CsBaseinfoset
                   where t.Fid == int.Parse(PublicMethod.ReadXmlReturnNode("ZY", this))
                   orderby t.Infoid ascending
                   select new
                   {

                       YHTypeID = t.Infoid,
                       YHType = t.Infoname
                   };
    }
}
