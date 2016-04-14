using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using System.Data;
using GhtnTech.SEP.DAL;
using GhtnTech.SecurityFramework.BLL;

public partial class BaseManage_PersonManage3 : BasePage
{
    DBSCMDataContext db = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!SessionBox.CheckUserSession())
        {
            Response.Redirect("~/Login.aspx");
        }
        else
        {
            List<string> lstRole = new List<string>();
            lstRole.Add("2");
            lstRole.Add("46");
            if (SessionBox.GetUserSession().CurrentRole[0].ToString().Split(',')[0] == "31")
            {
                //var data = from p in db.Person
                //           select p;
                //GridView.DataSource = data;
                //GridView.DataBind();
                //GridView.KeyFieldName = "Personid";
            }
            else if (lstRole.Contains(SessionBox.GetUserSession().CurrentRole[0].ToString().Split(',')[0]))
            {
                //var data = from p in db.Person
                //           select p;
                //GridView.DataSource = data;
                //GridView.DataBind();
                //GridView.KeyFieldName = "Personid";
            }
            else
            {
                //var data = from p in db.Person
                //           where p.Maindeptid == SessionBox.GetUserSession().DeptNumber
                //           select p;
                //GridView.DataSource = data;
                //GridView.DataBind();
                //GridView.KeyFieldName = "Personid";
                //adsPerson.Where = "Maindeptid == \"" + SessionBox.GetUserSession().DeptNumber + "\"";
                //adsDept.Where = "Deptnumber.StartsWith(\"" + SessionBox.GetUserSession().DeptNumber.Remove(4) + "\")";
                //adsPosition.Where = "Maindeptid == \"" + SessionBox.GetUserSession().DeptNumber + "\"";
                Session["maindeptid"] = SessionBox.GetUserSession().DeptNumber;
                Session["deptid"] = SessionBox.GetUserSession().DeptNumber.Remove(4);
                Session["PosDept"] = SessionBox.GetUserSession().DeptNumber;
            }
            //初始化模块权限
            UserHandle.InitModule(this.PageTag);
            //是否有浏览权限
            if (UserHandle.ValidationHandle(PermissionTag.Browse))
            {
                GridViewCommandColumn colEdit = (GridViewCommandColumn)GridView.Columns["操作"];
                if (!UserHandle.ValidationHandle(PermissionTag.Edit))
                {
                    colEdit.EditButton.Visible = false;
                }
            }
        }
            
    }

    //添加-隐患字母编码
    //protected void LinqDataSource1_Inserting(object sender, LinqDataSourceInsertEventArgs e)
    //{
    //    Person person = (Person)e.NewObject;
    //    person.pinyin = db.f_GetPy(person.Name);
    //}
    //修改
    //protected void LinqDataSource1_Updating(object sender, LinqDataSourceUpdateEventArgs e)
    //{
    //    Person person = (Person)e.NewObject;
    //    person.pinyin = db.f_GetPy(person.Name);
    //}
    //删除

    //protected void GridView_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
    //{
    //    if (!GridView.IsNewRowEditing)
    //    {
    //        ((GridViewDataColumn)GridView.Columns["PersonNumber"]).ReadOnly = true;
    //    }
    //}
}
