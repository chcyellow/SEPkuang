using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Coolite.Ext.Web;
using GhtnTech.SEP.DAL;
using GhtnTech.SecurityFramework.BLL;

public partial class PAR_TablePrint : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            if (string.IsNullOrEmpty(this.Request["Workid"]))
            {
                Ext.DoScript("CloseWin();");
                return;
            }
            string table = "";
            Panel1.Html = table;

            Ext.DoScript("window.print();");
            Ext.DoScript("CloseWin();");
        }
    }

    private void CreateHtml(decimal id)
    {
        DBSCMDataContext dc = new DBSCMDataContext();
        var wt = dc.Worktasks.First(p => p.Worktaskid == id);
        var user = dc.Vgetpl.First(p => p.Personnumber == SessionBox.GetUserSession().PersonNumber);
        string text = string.Format("<P align=center><B>{0}风险管理卡</B></P><BR>", wt.Worktask);
        text += string.Format("<P align=right><B>编号：{0}</B></P><BR>", wt.TaskNumber);
        text += "<B>首先对本工作存在的危险源进行确认，本工作存在以下主要危险源：</B><BR>";
        var hz = from h in dc.Hazards
                 from ho in dc.HazardsOre
                 from gx in dc.Process
                 where h.Processid == gx.Processid && gx.Worktaskid == id && h.HNumber == ho.HNumber && ho.Deptnumber == SessionBox.GetUserSession().DeptNumber && ho.HBjw == "引用"
                 select new
                 {
                     h.HContent,
                     h.HConsequences
                 };
        int Index = 1;
        string haz = "";
        string con = "";
        foreach (var r in hz)
        {
            haz += Index + "、" + r.HContent + "<BR>";
            con += Index + "、" + r.HConsequences + "<BR>";
            Index++;
        }

        text += haz;
        text += "<B>现在对以上危险源进行确认及风险描述：</B><BR>";
        text += con;
        text += "<B>" + wt.Worktask + "风险预控安全确认完毕。</B>";
    }
}
