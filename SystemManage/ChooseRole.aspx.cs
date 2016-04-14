using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GhtnTech.SecurityFramework;
using GhtnTech.SecurityFramework.Model;
using GhtnTech.SecurityFramework.Utility;
using GhtnTech.SecurityFramework.BLL;
public partial class SystemManage_ChooseRole : System.Web.UI.Page
{
    Role Rolebll = new Role();
    int rolelevel = 0;
    string roledeptid = "";
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
                SF_Role r = Rolebll.GetRoleModel(decimal.Parse(SessionBox.GetUserSession().CurrentRole[0].ToString().Split(',')[0]));
                rolelevel = (int)r.LevelID;
                roledeptid = SessionBox.GetUserSession().DeptNumber;
                switch ((int)rolelevel)
                {
                    case 0:
                        Session["WhereRole"] = string.Format("levelid >={0}", rolelevel);
                        break;
                    case 1:
                        Session["WhereRole"] = string.Format("levelid >={0}", rolelevel);
                        break;
                    case 2:
                        Session["WhereRole"] = string.Format("MAINDEPTID='{0}' or (maindeptid='{1}' and levelid>={2})", SessionBox.GetUserSession().DeptNumber, "000000000", rolelevel.ToString());
                        break;
                    default:
                        Session["WhereRole"] = string.Format("MAINDEPTID='{0}' or (maindeptid='{1}' and levelid>={2})", SessionBox.GetUserSession().DeptNumber, "000000000", rolelevel.ToString());
                        break;
                }
                //if (rolelevel > 1)
                //{
                //    Session["WhereRole"] = string.Format("MAINDEPTID='{0}' or (maindeptid='{1}' and levelid>={2})", SessionBox.GetUserSession().DeptNumber, "000000000", rolelevel.ToString());
                //    //Session["WhereUserGroup"] = "usergroupid not in(2,3,23)";
                //    //Session["maindeptid"] = SessionBox.GetUserSession().DeptNumber;
                //    //Session["deptid"] = SessionBox.GetUserSession().DeptNumber.Remove(4);
                //}
                //List<string> lstRole = new List<string>();
                //lstRole.Add("2");
                //lstRole.Add("46");
                //if (SessionBox.GetUserSession().CurrentRole[0].ToString().Split(',')[0] == "31")
                //{
                //    Session["WhereRole"] = " ";
                //}
                //else if (lstRole.Contains(SessionBox.GetUserSession().CurrentRole[0].ToString().Split(',')[0]))
                //{
                //    Session["WhereRole"] = "roleid != 31";
                //}
                //else
                //{
                //    Session["WhereRole"] = "roleid NOT in(2,31,46)";

                //}
            }
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        JSHelper.RefreshParent("ProcessManage.aspx", this);
    }
    protected void btnAddPerson_Click(object sender, EventArgs e)
    {
        string str = "";
        for (int i = 0; i < xlstSelected.Items.Count; i++)
        {
            str += xlstSelected.Items[i].Value.ToString() + ","; 
        }
        Session["GetRole"] = str;
        JSHelper.ReturnToValue("lnkGetValue", this);
    }
}
