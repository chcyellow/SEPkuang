using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using GhtnTech.SEP.DAL;
using GhtnTech.SEP.DBUtility;
using System.Data;

/// <summary>
///SMS_Send 的摘要说明
/// </summary>
public class SMS_Send
{
    DBSCMDataContext dc = new DBSCMDataContext();
	public SMS_Send()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    public string SendSMS(decimal yhid, string type,System.Web.UI.Page page)
    {
        var YH=dc.Getyhinput.First(p => p.Yhputinid == yhid);
        var set = dc.SmsManagement.Where(p => p.Coding == type && p.Deptnumber == YH.Unitid);
        if (set.Count() > 0)//允许发短信
        {
            switch (type)
            {
                case "yhfb":
                    var smsd = dc.Yhsmsset.Where(p => p.Yhlevel == YH.Levelid && p.Maindept == YH.Unitid && p.Deptnumber == YH.Deptid);
                    List<string> per = new List<string>();
                    foreach (var r in smsd)
                    {
                        per.Add(r.Person);
                    }
                    return Sms.Send(per, GetYHSMSmsg(yhid), yhid.ToString(), SmsType.HiddenTroubleTips, "-支撑平台!");
                case "yhyq":
                    var record = from act in dc.Nyhaction
                                 from h in dc.Nyhhistory
                                 where act.Actionid == h.Actionid
                                 && act.Yhputinid == yhid
                                 && h.Status == YH.Status
                                 select new
                                 {
                                     act.Actionid
                                 };
                    string sql = "select u.personnumber,p.deptid from sf_userrole ur inner join sf_user u on ur.userid=u.userid inner join person p on u.personnumber=p.personnumber where ur.roleid={0} and p.deptid='{1}'";
                    if (record.Count() == 0)
                    {
                        List<string> per1 = new List<string>();
                        foreach (DataRow r in OracleHelper.Query(string.Format(sql, PublicMethod.ReadXmlReturnNode("T" + YH.Typeid.ToString(), page), YH.Unitid)).Tables[0].Rows)
                        {
                            per1.Add(r["PERSONNUMBER"].ToString().Trim());
                        }
                        return Sms.Send(per1, GetYHSMSmsg(yhid) + ",逾期未整改", yhid.ToString(), SmsType.HiddenTroubleTips, "-支撑平台!");
                    }
                    else
                    {
                        List<string> per1 = new List<string>();
                        foreach (DataRow r in OracleHelper.Query(string.Format(sql, PublicMethod.ReadXmlReturnNode("T0", page), YH.Unitid)).Tables[0].Rows)
                        {
                            per1.Add(r["PERSONNUMBER"].ToString().Trim());
                        }
                        return Sms.Send(per1, GetYHSMSmsg(yhid)+",逾期未整改", yhid.ToString(), SmsType.HiddenTroubleTips, "-支撑平台!");
                    }
            }
        }
       
        return "";
    }

    public string GetYHSMSmsg(decimal yhid)
    {
        
        var YH = dc.Getyhinput.First(p => p.Yhputinid == yhid);
        if (YH.Remarks != null)
        {
            string msg = YH.Pctime.Value.Day.ToString() + "日"
                + YH.Banci
                + YH.Deptname
                + YH.Placename
                + "发现安全隐患：" + (YH.Remarks.Trim() == "" ? YH.Yhcontent.Trim() : YH.Remarks.Trim());
            if (msg.Length > 250)
                return "您有一条待处理的隐患。";
            else
                return msg;
        }
        else
        {
            string msg = YH.Pctime.Value.Day.ToString() + "日"
                + YH.Banci
                + YH.Deptname
                + YH.Placename
                + "发现安全隐患：" + YH.Yhcontent.Trim();
            if (msg.Length > 250)
                return "您有一条待处理的隐患。";
            else
                return msg;
        }
    }
}