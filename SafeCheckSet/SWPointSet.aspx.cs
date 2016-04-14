using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GhtnTech.SecurityFramework.BLL;

using GhtnTech.SEP.DAL;
using GhtnTech.SecurityFramework.Utility;

public partial class SafeCheckSet_SWPointSet : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!SessionBox.CheckUserSession())
        {
            Response.Redirect("~/Login.aspx");
        }
        else
        {
            adsSWPointSet.Where = "Deptnumber ==\"" + SessionBox.GetUserSession().DeptNumber + "\"";
            if (!this.IsPostBack)
            {
                DBSCMDataContext db = new DBSCMDataContext();
                var warn = db.Swwarningset.Where(p => p.Deptnumber == SessionBox.GetUserSession().DeptNumber);
                if (warn.Count() > 0)
                {
                    txtScore.Text = warn.First().Maxscore.ToString();
                    txtCount.Text = warn.First().Maxcount.ToString();
                }
            }
        }
    }
    protected void gvSWPointSet_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
    {
        e.NewValues["Deptnumber"] = SessionBox.GetUserSession().DeptNumber;
        e.NewValues["Usingtime"] = DateTime.Now;
    }
    protected void gvSWPointSet_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
    {
        e.NewValues["Usingtime"] = DateTime.Now;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtScore.Text.Trim() == "" || txtCount.Text.Trim() == "")
        {
            JSHelper.Alert("请填写分值与次数!");
            return;
        }
        try
        {
            DBSCMDataContext db = new DBSCMDataContext();
            var warn = db.Swwarningset.Where(p => p.Deptnumber == SessionBox.GetUserSession().DeptNumber);
            if (warn.Count() > 0)
            {
                Swwarningset war = db.Swwarningset.First(p => p.Deptnumber == SessionBox.GetUserSession().DeptNumber);
                war.Maxcount = decimal.Parse(txtCount.Text.Trim());
                war.Maxscore = decimal.Parse(txtScore.Text.Trim());
                db.SubmitChanges();
            }
            else
            {
                Swwarningset war = new Swwarningset
                {
                    Deptnumber = SessionBox.GetUserSession().DeptNumber,
                    Maxcount = decimal.Parse(txtCount.Text.Trim()),
                    Maxscore = decimal.Parse(txtScore.Text.Trim()),
                    Intime=System.DateTime.Today
                };
                db.Swwarningset.InsertOnSubmit(war);
                db.SubmitChanges();
            }
            JSHelper.Alert("保存成功!");
        }
        catch
        {
            JSHelper.Alert("保存失败，请检查填写是否正确!");
        }
    }
}
