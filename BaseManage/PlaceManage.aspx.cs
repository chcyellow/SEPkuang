using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using GhtnTech.SecurityFramework.BLL;

using GhtnTech.SEP.DAL;

//using System.Collections.Specialized;

public partial class BaseManage_PlaceManage : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!SessionBox.CheckUserSession())
            {
                Response.Redirect("~/Login.aspx");
            }
            
             //初始化模块权限
            UserHandle.InitModule(this.PageTag);
            //是否有浏览权限
            if (UserHandle.ValidationHandle(PermissionTag.Browse))
            {
                //gvPlace.GroupBy(gvPlace.Columns["Pareasid"]);
                GridViewCommandColumn colEdit = (GridViewCommandColumn)gvPlace.Columns["编辑"];
                if (!UserHandle.ValidationHandle(PermissionTag.Add))
                {
                    colEdit.NewButton.Visible = false;
                }
                if (!UserHandle.ValidationHandle(PermissionTag.Edit))
                {
                    colEdit.EditButton.Visible = false;
                }
                if (!UserHandle.ValidationHandle(PermissionTag.Delete))
                {
                    colEdit.DeleteButton.Visible = false;
                }
            }
        }
        List<string> lstRole = new List<string>();
        lstRole.Add("2");
        lstRole.Add("46");
        if (SessionBox.GetUserSession().CurrentRole[0].ToString().Split(',')[0] == "31")
        {

        }
        else if (lstRole.Contains(SessionBox.GetUserSession().CurrentRole[0].ToString().Split(',')[0]))
        {

        }
        else
        {
            adsPlace.Where = "Maindeptid == \"" + SessionBox.GetUserSession().DeptNumber + "\"";//&& Placestatus=1";
            adsPlaceAreas.Where = "Maindeptid == \"" + SessionBox.GetUserSession().DeptNumber + "\"";

        }
    }
    protected void gvPlace_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
    {
        e.NewValues["Maindeptid"] = SessionBox.GetUserSession().DeptNumber;
        //System.Nullable<decimal> gg = 1;
        //e.NewValues["Placestatus"] = gg;
        //DBSCMDataContext dc = new DBSCMDataContext();
        //Place p = new Place
        //{
        //    Maindeptid = SessionBox.GetUserSession().DeptNumber,
        //    Pareasid = decimal.Parse(e.NewValues["Pareasid"].ToString()),
        //    Placename = e.NewValues["Placename"].ToString(),
        //    Placestatus=1,
        //    Plid = decimal.Parse(e.NewValues["Plid"].ToString())
        //};
        //dc.Place.InsertOnSubmit(p);
        //dc.SubmitChanges();

        //e.Cancel = true;
    }

    protected void adsPlace_Deleting(object sender, LinqDataSourceDeleteEventArgs e)
    {
        DBSCMDataContext dc = new DBSCMDataContext();
    

        Place place=dc.Place.First(p=>p.Placeid==(e.OriginalObject as Place).Placeid);
        if (place.Placestatus == 1)
        {
            place.Placestatus = 0;
            dc.SubmitChanges();
        }

        //ListDictionary keyValues = new ListDictionary();
        //ListDictionary newValues = new ListDictionary();
        //ListDictionary oldValues = new ListDictionary();
        //keyValues.Add("Placeid", ((GhtnTech.SEP.DAL.Place)e.OriginalObject).Placeid;

        //oldValues.Add("ProductName", ((Label)DetailsView1.FindControl("NameLabel")).Text);
        //oldValues.Add("ProductCategory", ((Label)DetailsView1.FindControl("CategoryLabel")).Text);
        //oldValues.Add("Color", ((Label)DetailsView1.FindControl("ColorLabel")).Text);

        //newValues.Add("ProductName", "New Product");
        //newValues.Add("ProductCategory", "General");
        //newValues.Add("Color", "Not assigned");


        //adsPlace.Update(keyValues, newValues, oldValues);
        e.Cancel = true;
    }
    protected void Page_PreRenderComplete(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddlLevel.Items.Insert(0, new ListItem("--全部--", "-1"));
            ddlArea.Items.Insert(0, new ListItem("--全部--", "-1"));
            
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindOrder();
    }
    protected void gvPlace_PageIndexChanged(object sender, EventArgs e)
    {
        BindOrder();
    }
    protected void BindOrder()
    {
        string strWhere = "";
        List<string> lstRole = new List<string>();
        lstRole.Add("2");
        lstRole.Add("46");
        if (SessionBox.GetUserSession().CurrentRole[0].ToString().Split(',')[0] == "31")
        {

        }
        else if (lstRole.Contains(SessionBox.GetUserSession().CurrentRole[0].ToString().Split(',')[0]))
        {

        }
        else
        {
            strWhere += "Maindeptid == \"" + SessionBox.GetUserSession().DeptNumber + "\"";//&& Placestatus=1";
        }
        if (ddlArea.SelectedValue != "-1")
        {
            strWhere += " && Pareasid=" + ddlArea.SelectedValue;
        }
        if (ddlLevel.SelectedValue != "-1")
        {
            strWhere += " && Plid=" + ddlLevel.SelectedValue;
        }
        if (txtPlace.Text != "")
        {
            strWhere += string.Format(" && Placename.Contains(\"{0}\")", txtPlace.Text.Trim());
        }
        if (ddlStatus.SelectedValue != "-1")
        {
            strWhere += " && Placestatus=" + ddlStatus.SelectedValue;
        }
        adsPlace.Where = strWhere;
        adsPlaceAreas.Where = "Maindeptid == \"" + SessionBox.GetUserSession().DeptNumber + "\"";
        
    }
}
