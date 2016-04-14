using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxTreeList;
using GhtnTech.SecurityFramework.BLL;
using GhtnTech.SEP.DAL;
public partial class BaseManage_DepartInfo : BasePage
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
                List<string> lstRole = new List<string>();
                lstRole.Add("2");
                lstRole.Add("46");
                if (SessionBox.GetUserSession().CurrentRole[0].ToString().Split(',')[0] == "31")
                {
                    var data = from dept in dc.Department
                               select new
                               {
                                   Deptname = dept.Deptname,
                                   Fatherid = dept.Fatherid,

                                   Deptnumber = dept.Deptnumber
                               };
                    DepTreeList.DataSource = data;
                    DepTreeList.DataBind();
                }
                else if (lstRole.Contains(SessionBox.GetUserSession().CurrentRole[0].ToString().Split(',')[0]))
                {
                    var data = from dept in dc.Department
                              select new
                              {
                                  Deptname = dept.Deptname,
                                  Fatherid = dept.Fatherid,

                                  Deptnumber = dept.Deptnumber
                              };
                    DepTreeList.DataSource = data;
                    DepTreeList.DataBind();
                }
                else
                {
                    var data = from dept in dc.Department
                               where dept.Deptnumber.StartsWith(SessionBox.GetUserSession().DeptNumber.Remove(4))
                               select new
                               {
                                   Deptname = dept.Deptname,
                                   Fatherid = dept.Fatherid,
                                   Deptnumber = dept.Deptnumber
                               };
                    DepTreeList.DataSource = data;
                    DepTreeList.DataBind();
                }
            }
        
    }
}
