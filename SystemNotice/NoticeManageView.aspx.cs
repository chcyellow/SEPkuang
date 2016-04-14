using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GhtnTech.SEP.DAL;
using Coolite.Ext.Web;

public partial class SystemNotice_NoticeManageView : System.Web.UI.Page
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            if (Request["Nid"].Trim() != "" || Request["Nid"].Trim() != null)
            {
                var data = dc.Sysnotice.First(p => p.Nid == decimal.Parse(Request["Nid"].Trim()));

                #region table画数据排列
                string note = "<table width=\"99%\" cellspacing=\"0\" cellpadding=\"0\">";

                note += "<tr>";
                note += string.Format("<td class=\"lbr3 lpa title\" width=\"30%\">公告编号:</td><td class=\"lbr3 lpa \" width=\"70%\">{0}</td>", data.Nid);
                note += "</tr>";

                note += "<tr>";
                note += string.Format("<td class=\"lbr3 lpa title\" width=\"30%\">公告标题:</td><td class=\"lbr3 lpa \" width=\"70%\">{0}</td>", data.Ntitle.Trim());
                note += "</tr>";

                note += "<tr>";
                note += string.Format("<td class=\"lbr3 lpa title\" width=\"30%\">公告内容:</td><td class=\"lbr3 lpa \" width=\"70%\">{0}</td>", data.Nmessage.Trim());
                note += "</tr>";

                note += "<tr>";
                try
                {
                    string strUrl = "<a href=\"FileDown.aspx?Nid=" + data.Nid.ToString().Trim() + "\" target=\"_bank\">" + data.Nfilename.Trim() + "</a>";
                    note += string.Format("<td class=\"lbr3 lpa title\" width=\"30%\">附件:</td><td class=\"lbr3 lpa \" width=\"70%\">{0}</td>", strUrl);
                }
                catch
                {
                    note += string.Format("<td class=\"lbr3 lpa title\" width=\"30%\">附件:</td><td class=\"lbr3 lpa \" width=\"70%\">{0}</td>", "无");
                }
                note += "</tr>";

                note += "<tr>";
                note += string.Format("<td class=\"lbr3 lpa title\" width=\"30%\">发布日期:</td><td class=\"lbr3 lpa \" width=\"70%\">{0}</td>", data.Pdate.Value.ToString("yyyy-MM-dd"));
                note += "</tr>";

                string perName = dc.Person.First(p => p.Personnumber == data.Pperid).Name;
                string deptName = dc.Department.First(p => p.Deptnumber == data.Pdeptid).Deptname;
                note += "<tr>";
                note += string.Format("<td class=\"lbr3 lpa title\" width=\"30%\">发布人：</td><td class=\"lbr3 lpa \" width=\"70%\">{0}</td>", perName.Trim());
                note += "</tr>";

                note += "<tr>";
                note += string.Format("<td class=\"lbr3 lpa title\" width=\"30%\">发布单位:</td><td class=\"lbr3 lpa \" width=\"70%\">{0}</td>", deptName.Trim());
                note += "</tr>";

                note += "</table>";
                #endregion

                PnlDetail.Html = note;
            }
        }
    }
}
