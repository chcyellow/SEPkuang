using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GhtnTech.SEP.DAL;
using GhtnTech.SecurityFramework.BLL;
using Coolite.Ext.Web;

public partial class LeaderSearch_PlaceSummary : BasePage
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        string url = string.Format("{0}?begin={1}&end={2}&PAreasID={3}", Request["url"].Trim(), Request["begin"].Trim(), Request["end"].Trim(), Request["PAreasID"].Trim());
        if (!string.IsNullOrEmpty(this.Request["status"]))
        {
            url += "&status=" + this.Request["status"].Trim();
        }
        BuildTree(int.Parse(this.Request["PAreasID"]),url);
        Ext.DoScript("#{pnlDetail}.load('" + url + "');");
    }
    private void BuildTree(int PAreasID,string url)
    {
        tpPlace.Root.Clear();
        var area = dc.Placeareas.First(p => p.Pareasid == PAreasID);
        Coolite.Ext.Web.TreeNode root = new Coolite.Ext.Web.TreeNode(area.Pareasid.ToString(), area.Pareasname.Trim(), Icon.TransmitBlue);
        root.Qtip = area.Pareasname.Trim();
        root.Listeners.Click.Handler = "#{pnlDetail}.load('" + url + "');";
        tpPlace.Root.Add(root);
        var place = dc.Place.Where(p => p.Pareasid == PAreasID);
        foreach (var r in place)
        {
            Coolite.Ext.Web.TreeNode node = new Coolite.Ext.Web.TreeNode(r.Placeid.ToString(), r.Placename.Trim(), Icon.Package);
            node.Listeners.Click.Handler = "#{pnlDetail}.load('" + url + "&PlaceID=" + r.Placeid.ToString() + "');";
            node.Expanded = false;
            root.Nodes.Add(node);
        }
    }
}
