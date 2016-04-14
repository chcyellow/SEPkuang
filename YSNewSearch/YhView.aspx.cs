using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using GhtnTech.SEP.DAL;

public partial class YSNewSearch_YhView : System.Web.UI.Page
{
    DBSCMDataContext dc = new DBSCMDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack && !string.IsNullOrEmpty(Request.QueryString["Yhid"]))
        {

            SetHtml(decimal.Parse(Request.QueryString["Yhid"]));
        }
    }

    private void SetHtml(decimal strYh)
    {
        var input = dc.Nyhinput.First(p => p.Yhputinid == strYh);
        string table = "";
        int i = 0;
        switch (input.Status.Trim())
        {

            case "新增":
            case "提交审批":
            case "现场整改":
                i = 1;
                break;
            case "隐患未整改":
            case "隐患整改中":
            case "逾期未整改":
            case "逾期整改未完成":
                i = 2;
                break;
            case "隐患已整改":
                i = 3;
                break;
            case "复查通过":
            case "复查未通过":
                i = 4;
                break;
        }
        if (i > 0)
        {
            table += SetYHbase(strYh);
        }
        i--;
        if (i > 0)
        {
            table += SetYHZG(strYh);
        }
        i--;
        if (i > 0)
        {
            table += SetYHZGFK(strYh);
        }
        i--;
        if (i > 0)
        {
            table += SetYHFCFK(strYh);
        }
        try
        {
            if (input.Isfine.Value == 1)
            {
                table += SetFine(strYh);
            }
        }
        catch
        {
        }
        table += SetHistroyYQ(strYh);
        lmsg.Text = table;
    }
    private string td = "<td class=\"lbr3 lpa title\" width=\"13%\">{0}:</td><td class=\"lbr3 lpa \" width=\"20%\" {2}>{1}</td>";

    private string SetYHbase(decimal strYh)
    {
        string note = "<B><font color=\"blue\">隐患基本信息</font></B><HR>";
        var data = from yh in dc.Getyhinput
                   where yh.Yhputinid == strYh
                   select new
                   {
                       yh.Yhputinid,
                       yh.Deptname,
                       yh.Placename,
                       HBm = yh.Yhcontent,
                       yh.Remarks,
                       yh.Banci,
                       Personname = yh.Name,
                       yh.Pctime,
                       yh.Levelname,
                       Zyname = yh.Typename,
                       yh.Status,
                       yh.Jctype,
                       yh.Isfine
                   };
        if (data.Count() > 0)
        {
            var yh = data.First();
            note += "<table width=\"99%\" cellspacing=\"0\" cellpadding=\"0\">";

            note += "<tr>";
            note += string.Format(td, "隐患编号", yh.Yhputinid, "");
            note += string.Format(td, "隐患部门", yh.Deptname, "");
            note += string.Format(td, "隐患地点", yh.Placename, "");
            note += "</tr>";
            //note += "<tr>";
            //note += string.Format(td, "发生班次", yh.Banci, "");
            //note += string.Format(td, "排查人员", yh.Personname, "");
            //note += string.Format(td, "排查时间", yh.Pctime.Value.ToString("yyyy年MM月dd日"), "");
            //note += "</tr>";
            note += "<tr>";
            note += string.Format(td, "隐患级别", yh.Levelname, "");
            note += string.Format(td, "隐患类型", yh.Zyname, "");
            note += string.Format(td, "当前状态", yh.Status, "");
            note += "</tr>";
            note += "<tr>";
            note += string.Format(td, "隐患内容", yh.HBm, "colspan=\"5\"");
            note += "</tr>";
            //note += "<tr>";
            //note += string.Format(td, "备注信息", string.IsNullOrEmpty(yh.Remarks) ? "" : yh.Remarks, "colspan=\"5\"");
            //note += "</tr>";

            note += "</table><br>";

            note += "<B><font color=\"blue\">隐患排查信息</font></B><HR>";

            var pc = from m in dc.NyhinputMore
                     from p in dc.Person
                     where m.Personid == p.Personnumber && m.Yhputinid == strYh
                     select new
                     {
                         p.Name,
                         m.Pctime,
                         m.Banci,
                         m.Remarks,
                         m.Jctype
                     };
            string[] jc = new string[] { "矿查", "自查", "上级排查" };
            if (pc.Count() > 0)
            {
                note += "<table width=\"99%\" cellspacing=\"0\" cellpadding=\"0\">";
                note += "<tr>";
                note += "<td class=\"lbr3 lpa\" width=\"15%\" style=\"font-weight: bold\">排查人员</td>";
                note += "<td class=\"lbr3 lpa\" width=\"20%\" style=\"font-weight: bold\">排查时间</td>";
                note += "<td class=\"lbr3 lpa\" width=\"15%\" style=\"font-weight: bold\">排查班次</td>";
                note += "<td class=\"lbr3 lpa\" width=\"20%\" style=\"font-weight: bold\">检查类型</td>";
                note += "<td class=\"lbr3 lpa\" width=\"30%\" style=\"font-weight: bold\">备注信息</td>";
                note += "</tr>";
                foreach (var r in pc)
                {
                    note += "<tr>";
                    note += string.Format("<td class=\"lbr3 lpa \">{0}</td>", r.Name);
                    note += string.Format("<td class=\"lbr3 lpa \">{0}</td>", r.Pctime.Value.ToString("yyyy年MM月dd日"));
                    note += string.Format("<td class=\"lbr3 lpa \">{0}</td>", r.Banci);
                    note += string.Format("<td class=\"lbr3 lpa \">{0}</td>", jc[Convert.ToInt32(r.Jctype.Value)]);
                    note += string.Format("<td class=\"lbr3 lpa \">{0}</td>", string.IsNullOrEmpty(r.Remarks) ? "" : r.Remarks);
                    note += "</tr>";
                }
                note += "</table><br>";
            }
            return note;
        }
        return "";
    }

    private string SetYHZG(decimal strYh)
    {
        string note = "<B><font color=\"blue\">隐患整改信息</font></B><HR>";
        var data = from yh in dc.Nyinhuancheck
                   from d in dc.Department
                   where yh.Responsibledept == d.Deptnumber && yh.Yhputinid == strYh
                   select new
                   {
                       yh.Measures,
                       yh.Instructions,
                       d.Deptname,
                       yh.Instrtime,
                       yh.Reclimit,
                       yh.Banci
                   };
        if (data.Count() > 0)
        {
            var yh = data.First();
            note += "<table width=\"99%\" cellspacing=\"0\" cellpadding=\"0\">";

            note += "<tr>";
            note += string.Format(td, "整改措施", yh.Measures, "colspan=\"5\"");
            note += "</tr>";
            note += "<tr>";
            note += string.Format(td, "领导批示", string.IsNullOrEmpty(yh.Instructions) ? "" : yh.Instructions, "colspan=\"5\"");
            note += "</tr>";

            note += "<tr>";
            note += string.Format(td, "批示时间", yh.Instrtime.Value.ToString("yyyy年MM月dd日"), "");
            note += string.Format(td, "责任单位", yh.Deptname, "");
            note += string.Format(td, "整改期限", yh.Reclimit.Value.ToString("yyyy年MM月dd日") + yh.Banci, "");
            note += "</tr>";

            note += "</table><br>";
            return note;
        }
        return "";
    }

    private string SetYHZGFK(decimal strYh)
    {
        string note = "<B><font color=\"blue\">整改反馈信息</font></B><HR>";
        var data = from yh in dc.Nyinhuanrectification
                   from p1 in dc.Person
                   from p2 in dc.Person
                   where yh.Recpersonid == p1.Personnumber && yh.Yanshouid == p2.Personnumber && yh.Yhputinid == strYh
                   select new
                   {
                       RECPERSON=p1.Name,
                       yh.Rectime,
                       yh.Recstate,
                       Yanshou=p2.Name,
                       yh.Zgbanci
                   };

        if (data.Count() > 0)
        {
            var yh = data.First();
            note += "<table width=\"99%\" cellspacing=\"0\" cellpadding=\"0\">";

            note += "<tr>";
            note += string.Format(td, "整改情况", yh.Recstate, "");
            note += string.Format(td, "整改时间", yh.Rectime.Value.ToString("yyyy年MM月dd日"), "");
            note += string.Format(td, "整改班次", yh.Zgbanci, "");
            note += "</tr>";
            note += "<tr>";
            note += string.Format(td, "整 改 人", yh.RECPERSON, "");
            note += string.Format(td, "验 收 人", yh.Yanshou, "colspan=\"3\"");
            note += "</tr>";

            note += "</table><br>";
            return note;
        }
        return "";
    }

    private string SetYHFCFK(decimal strYh)
    {
        string note = "<B><font color=\"blue\">复查反馈信息</font></B><HR>";
        var data = from r in dc.Nyinhuanreview
                   from p in dc.Person
                   where r.Personid == p.Personnumber && r.Yhputinid == strYh
                   select new
                   {
                       r.Reviewopinion,
                       r.Fctime,
                       r.Reviewstate,
                       p.Name
                   };
        if (data.Count() > 0)
        {
            var yh = data.First();
            note += "<table width=\"99%\" cellspacing=\"0\" cellpadding=\"0\">";

            note += "<tr>";
            note += string.Format(td, "复查意见", yh.Reviewopinion, "colspan=\"5\"");
            note += "</tr>";
            note += "<tr>";
            note += string.Format(td, "复 查 人", yh.Name, "");
            note += string.Format(td, "复查时间", yh.Fctime.Value.ToString("yyyy年MM月dd日"), "");
            note += string.Format(td, "复查情况", yh.Reviewstate, "");
            note += "</tr>";

            note += "</table><br>";
            return note;
        }
        return "";
    }

    private string SetFine(decimal strYh)
    {
        string note = "<B><font color=\"blue\">处罚信息</font></B><HR>";
        string msg = "";
        note += "<table width=\"99%\" cellspacing=\"0\" cellpadding=\"0\">";

        var fk = from nd in dc.Nyhfinedetail
                 from l in dc.Objectset
                 from p in dc.Person
                 from yh in dc.Nyhinput
                 from f in dc.Fineset
                 where nd.Pid == l.Pid && nd.Personnumber == p.Personnumber && nd.Yinputid == strYh
                 && nd.Yinputid == yh.Yhputinid && yh.Levelid == f.Levelid && nd.Pid == f.Pid
                 orderby l.Pname 
                 select new
                 {
                     l.Pname,
                     p.Name,
                     fine = yh.Jctype == 0 ? f.Kcfine : f.Zcfine
                 };

        foreach (var r in fk)
        {
            msg += "<b>" + r.Pname + "：</b>" + r.Name + "(" + r.fine + "元)";
        }
        note += "<tr>";
        note += string.Format(td, "处罚明细", msg == "" ? "无" : msg, "colspan=\"5\"");
        note += "</tr>";
        note += "<tr>";
        note += string.Format(td, "共计罚款", fk.Sum(p => p.fine).ToString() + "元", "colspan=\"5\"");
        note += "</tr>";

        note += "<tr>";
        note += string.Format(td, "", "", "").Replace(":","");
        note += string.Format(td, "", "", "").Replace(":", "");
        note += string.Format(td, "", "", "").Replace(":", "");
        note += "</tr>";

        note += "</table><br>";
        return note;
    }

    private string SetHistroyYQ(decimal strYh)
    {
        var data = from a in dc.Nyhaction
                   from h in dc.Nyhhistory
                   where a.Actionid == h.Actionid && a.Yhputinid == strYh && a.Historystatus == "逾期未整改"
                   orderby h.Reclimit
                   select new
                   {
                       a.Happendate,
                       a.Newstauts,
                       //发布
                       h.Reclimit,
                       h.Banci,
                       h.Checktime,
                       //整改
                       h.Rectime,
                       h.Zgbanci
                   };
        if (data.Count() > 0)
        {
            string note = "<B><font color=\"orange\">历史逾期信息</font></B><HR>";
            note += "<table width=\"99%\" cellspacing=\"0\" cellpadding=\"0\">";
            note += "<tr>";
            note += "<td class=\"lbr3 lpa\" width=\"30%\" style=\"font-weight: bold\">发布时间</td>";
            note += "<td class=\"lbr3 lpa\" width=\"35%\" style=\"font-weight: bold\">整改期限</td>";
            note += "<td class=\"lbr3 lpa\" width=\"35%\" style=\"font-weight: bold\">整改日期</td>";
            note += "</tr>";
            foreach (var r in data)
            {
                note += "<tr>";
                note += string.Format("<td class=\"lbr3 lpa \">{0}</td>", r.Checktime.Value.ToString("yyyy年MM月dd日"));
                note += string.Format("<td class=\"lbr3 lpa \">{0}</td>", r.Reclimit.Value.ToString("yyyy年MM月dd日")+r.Banci);
                note += string.Format("<td class=\"lbr3 lpa \">{0}</td>", r.Rectime == null ? "无" : r.Rectime.Value.ToString("yyyy年MM月dd日") + r.Zgbanci);
                note += "</tr>";
            }
            note += "</table><br>";
            return note;
        }
        else
        {
            return "";
        }
    }
}