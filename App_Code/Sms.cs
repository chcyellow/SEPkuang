using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using GhtnTech.SEP.DAL;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
/// <summary>
///Sms 的摘要说明
/// </summary>
public class Sms
{
    public static string Send(string phone, string content)
    {
        if (VerifyNumber(phone))
        {
            DBSCMDataContext dc = new DBSCMDataContext();
            TblSmsendtask sms = new TblSmsendtask();
            sms.Count = 1;
            sms.Creatorid = "000";
            sms.Destaddr = phone;
            sms.Destaddrtype = 0;
            sms.Feecode = 0;
            sms.Feetype = "01";
            sms.Messageid = 0;
            sms.Msgid = "";
            sms.Needstatereport = 0;
            sms.Operationtype = "WAS";
            sms.Orgaddr = "1065730615390000";
            sms.Reserve1 = "";
            sms.Reserve2 = "";
            sms.Sendlevel = 0;
            sms.Sendstate = 0;
            sms.Sendtime = DateTime.Now.AddSeconds(2);
            sms.Sendtype = 1;
            sms.Serviceid = "MAH0510101";
            sms.SmContent = content;
            sms.Smsendednum = 0;
            sms.Smtype = 0;
            sms.Suboperationtype = 66;
            sms.Subtime = DateTime.Now;
            sms.Successid = 0;
            sms.Taskname = "";
            sms.Taskstatus = 0;
            sms.Trytimes = 3;
            string strSql = "INSERT INTO [tbl_SMSendTask]( [CreatorID], [TaskName], [SmSendedNum], [OperationType], [SuboperationType], [SendType], [OrgAddr], [DestAddr],  [SM_Content], [SendTime], [NeedStateReport], [ServiceID], [FeeType], [FeeCode], [MsgID], [SMType], [MessageID], [DestAddrType], [SubTime], [TaskStatus],  [SendLevel], [SendState], [TryTimes], [Count], [SuccessID], [Reserve1], [Reserve2]) values ('" + sms.Creatorid + "','" + sms.Taskname + "'," + sms.Smsendednum.Value.ToString() + ",'" + sms.Operationtype + "'," + sms.Suboperationtype.Value.ToString() + "," + sms.Sendtype.Value.ToString() + ",'" + sms.Orgaddr + "','" + sms.Destaddr + "','" + sms.SmContent + "','" + sms.Sendtime.Value + "'," + sms.Needstatereport.Value.ToString() + ",'" + sms.Serviceid + "','" + sms.Feetype + "'," + sms.Feecode.Value.ToString() + ",'" + sms.Msgid + "'," + sms.Smtype.Value.ToString() + "," + sms.Messageid.Value.ToString() + "," + sms.Destaddrtype.Value.ToString() + ",'" + sms.Subtime.Value + "'," + sms.Taskstatus.Value.ToString() + "," + sms.Sendlevel.Value.ToString() + "," + sms.Sendstate.Value.ToString() + "," + sms.Trytimes.Value.ToString() + "," + sms.Count.Value.ToString() + "," + sms.Successid.Value.ToString() + ",'" + sms.Reserve1 + "','" + sms.Reserve2 + "')";
            try
            {
                if (SQLHelper.ExecuteSql(strSql) >= 1)
                {
                    sms.Successid = 1;
                    dc.TblSmsendtask.Insert(sms);
                    dc.SubmitChanges();
                    return "1";
                }
                else
                {
                    dc.TblSmsendtask.Insert(sms);
                    dc.SubmitChanges();
                    return "0";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        else
        {
            return "非法的手机号码！";
        }
     }
    public static string Send(List<string> phone, string content)
    {
        string msg;
        string failmsg = "";
        int success = 0;
        int failure = 0;
        foreach (string s in phone)
        {
            msg = Send(s, content);
            if (msg == "1")
            {
                success++;
            }
            else if (msg == "0")
            {
                failure++;
            }
            else
            {
                failure++;
                failmsg += msg + "\n";
            }
        }
        if (failure > 0)
        {
            return "共有" + failure + "条短信发送失败！原因如下：\n" + failmsg;
        }
        else
        {
            return "1";
        }

    }
    /// <summary>
    /// 发送短信息
    /// </summary>
    /// <param name="personnumber">人员编码</param>
    /// <param name="content">短信内容</param>
    /// <param name="type">发送类型</param>
    /// <param name="msgid">隐患，三违，走动等ID信息。需要就写入。</param>
    /// <param name="sender">发送者</param>
    /// <returns>成功返回"1"</returns>
    public static string Send(string personnumber, string content, SmsType type, string msgid,string sender)
    {
        string phone = GetPhone(personnumber).Trim() == "" ? "0" : GetPhone(personnumber);
        if (VerifyNumber(phone))
        {
            DBSCMDataContext dc = new DBSCMDataContext();
            TblSmsendtask sms = new TblSmsendtask();
            sms.Count = 1;
            sms.Creatorid = "000";
            sms.Destaddr = phone;
            sms.Destaddrtype = 0;
            sms.Feecode = 0;
            sms.Feetype = "01";
            sms.Messageid = 0;
            sms.Msgid = "";
            sms.Needstatereport = 0;
            sms.Operationtype = "WAS";
            sms.Orgaddr = "1065730615390000";
            sms.Reserve1 = "";
            sms.Reserve2 = "";
            sms.Sendlevel = 0;
            sms.Sendstate = 0;
            sms.Sendtime = DateTime.Now.AddSeconds(2);
            sms.Sendtype = 1;
            sms.Serviceid = "MAH0510101";
            sms.SmContent = GetSmsType(type) + content +"\n"+ sender;
            sms.Smsendednum = 0;
            sms.Smtype = 0;
            sms.Suboperationtype = 66;
            sms.Subtime = DateTime.Now;
            sms.Successid = 0;
            sms.Taskname = "";
            sms.Taskstatus = 0;
            sms.Trytimes = 3;
            string strSql = "INSERT INTO [tbl_SMSendTask]( [CreatorID], [TaskName], [SmSendedNum], [OperationType], [SuboperationType], [SendType], [OrgAddr], [DestAddr],  [SM_Content], [SendTime], [NeedStateReport], [ServiceID], [FeeType], [FeeCode], [MsgID], [SMType], [MessageID], [DestAddrType], [SubTime], [TaskStatus],  [SendLevel], [SendState], [TryTimes], [Count], [SuccessID], [Reserve1], [Reserve2]) values ('" + sms.Creatorid + "','" + sms.Taskname + "'," + sms.Smsendednum.Value.ToString() + ",'" + sms.Operationtype + "'," + sms.Suboperationtype.Value.ToString() + "," + sms.Sendtype.Value.ToString() + ",'" + sms.Orgaddr + "','" + sms.Destaddr + "','" + sms.SmContent + "','" + sms.Sendtime.Value + "'," + sms.Needstatereport.Value.ToString() + ",'" + sms.Serviceid + "','" + sms.Feetype + "'," + sms.Feecode.Value.ToString() + ",'" + sms.Msgid + "'," + sms.Smtype.Value.ToString() + "," + sms.Messageid.Value.ToString() + "," + sms.Destaddrtype.Value.ToString() + ",'" + sms.Subtime.Value + "'," + sms.Taskstatus.Value.ToString() + "," + sms.Sendlevel.Value.ToString() + "," + sms.Sendstate.Value.ToString() + "," + sms.Trytimes.Value.ToString() + "," + sms.Count.Value.ToString() + "," + sms.Successid.Value.ToString() + ",'" + sms.Reserve1 + "','" + sms.Reserve2 + "')";
            try
            {
                if (SQLHelper.ExecuteSql(strSql) >= 1)
                {
                    sms.Successid = 1;
                    sms.Taskname = GetSmsType(type);
                    sms.Msgid = msgid;
                    dc.TblSmsendtask.Insert(sms);
                    dc.SubmitChanges();
                    return "1";
                }
                else
                {
                    dc.TblSmsendtask.Insert(sms);
                    dc.SubmitChanges();
                    return "0";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        else
        {
            return "非法的手机号码！";
        }
    }
    /// <summary>
    /// 群发短信息
    /// </summary>
    /// <param name="personnumber">人员编码列表</param>
    /// <param name="content">短信内容</param>
    /// <param name="type">发送类型</param>
    /// <param name="msgid">隐患，三违，走动等ID信息。需要就写入。</param>
    /// <param name="sender">发送者</param>
    /// <returns>成功返回"1"</returns>
    public static string Send(List<string> personnumber, string content, string msgid,SmsType type, string sender)
    {
        string msg;
        string failmsg = "";
        int success = 0;
        int failure = 0;
        foreach (string p in personnumber)
        {
            msg = Send(p, content, type, msgid,sender);
            if (msg == "1")
            {
                success++;
            }
            else if (msg == "0")
            {
                failure++;
            }
            else
            {
                failure++;
                failmsg += msg + "\n";
            }
        }
        if (failure > 0)
        {
            return "共有" + failure + "条短信发送失败！原因如下：\n" + failmsg;
        }
        else
        {
            return "1";
        }

    }

    public static string Send(string txt, ListBox lstPerson, string sender)
    {
        string phone = "";
        string errmsg = "";
        int failcount = 0;
        for (int i = 0; i < lstPerson.Items.Count; i++)
        {
            phone = VerifyNumber(lstPerson.Items[i].Text.Trim()) ? lstPerson.Items[i].Text.Trim() : GetPhoneViaName(lstPerson.Items[i].Text.Trim());
            if (VerifyNumber(phone))
            {
                DBSCMDataContext dc = new DBSCMDataContext();
                TblSmsendtask sms = new TblSmsendtask();
                sms.Count = 1;
                sms.Creatorid = "000";
                sms.Destaddr = phone;
                sms.Destaddrtype = 0;
                sms.Feecode = 0;
                sms.Feetype = "01";
                sms.Messageid = 0;
                sms.Msgid = "";
                sms.Needstatereport = 0;
                sms.Operationtype = "WAS";
                sms.Orgaddr = "1065730615390000";
                sms.Reserve1 = "";
                sms.Reserve2 = "";
                sms.Sendlevel = 0;
                sms.Sendstate = 0;
                sms.Sendtime = DateTime.Now.AddSeconds(2);
                sms.Sendtype = 1;
                sms.Serviceid = "MAH0510101";
                sms.SmContent = GetSmsType(SmsType.None) + txt + "\n" + sender;
                sms.Smsendednum = 0;
                sms.Smtype = 0;
                sms.Suboperationtype = 66;
                sms.Subtime = DateTime.Now;
                sms.Successid = 0;
                sms.Taskname = "";
                sms.Taskstatus = 0;
                sms.Trytimes = 3;
                string strSql = "INSERT INTO [tbl_SMSendTask]( [CreatorID], [TaskName], [SmSendedNum], [OperationType], [SuboperationType], [SendType], [OrgAddr], [DestAddr],  [SM_Content], [SendTime], [NeedStateReport], [ServiceID], [FeeType], [FeeCode], [MsgID], [SMType], [MessageID], [DestAddrType], [SubTime], [TaskStatus],  [SendLevel], [SendState], [TryTimes], [Count], [SuccessID], [Reserve1], [Reserve2]) values ('" + sms.Creatorid + "','" + sms.Taskname + "'," + sms.Smsendednum.Value.ToString() + ",'" + sms.Operationtype + "'," + sms.Suboperationtype.Value.ToString() + "," + sms.Sendtype.Value.ToString() + ",'" + sms.Orgaddr + "','" + sms.Destaddr + "','" + sms.SmContent + "','" + sms.Sendtime.Value + "'," + sms.Needstatereport.Value.ToString() + ",'" + sms.Serviceid + "','" + sms.Feetype + "'," + sms.Feecode.Value.ToString() + ",'" + sms.Msgid + "'," + sms.Smtype.Value.ToString() + "," + sms.Messageid.Value.ToString() + "," + sms.Destaddrtype.Value.ToString() + ",'" + sms.Subtime.Value + "'," + sms.Taskstatus.Value.ToString() + "," + sms.Sendlevel.Value.ToString() + "," + sms.Sendstate.Value.ToString() + "," + sms.Trytimes.Value.ToString() + "," + sms.Count.Value.ToString() + "," + sms.Successid.Value.ToString() + ",'" + sms.Reserve1 + "','" + sms.Reserve2 + "')";
                try
                {
                    if (SQLHelper.ExecuteSql(strSql) >= 1)
                    {
                        sms.Successid = 1;
                        sms.Taskname = GetSmsType(SmsType.Other);
                        dc.TblSmsendtask.Insert(sms);
                        dc.SubmitChanges();
                    }
                    else
                    {
                        dc.TblSmsendtask.Insert(sms);
                        dc.SubmitChanges();
                        failcount++;
                    }
                }
                catch (Exception ex)
                {
                    failcount++;
                    errmsg+= ex.Message+"\n";
                }
            }
            else
            {
                failcount++;
                errmsg += lstPerson.Items[i].Text.Trim() + "的手机号码非法！\n";
            }
        }
        if (failcount > 0 || errmsg != "")
        {
            return string.Format("共有{0}条短信未发送！原因如下：\n{1}", failcount, errmsg);
        }
        else
        {
            return "1";
        }
        
        
    }
    /// <summary>
    /// 验证字符串是否为手机号码或小灵通号码。
    /// </summary>
    /// <param name="number"></param>
    /// <returns>true or false</returns>
    private static bool VerifyNumber(string number)
    {
        Regex rgxNumber = new Regex(@"(^0(\d{3,4})?\d{7,8}$)|(^1\d{10}$)");
        return rgxNumber.IsMatch(number) ? true : false;
    }

    private static string GetPhone(string personnumber)
    {
        DBSCMDataContext dc = new DBSCMDataContext();
        var phone = from psn in dc.Person
                   where psn.Personnumber == personnumber
                   select psn.Tel;
        if (phone.Count() > 0)
        {
            try
            {
                return phone.First().Trim();
            }
            catch
            {
                return "0";
            }
        }
        else
        {
            return "0";
        }
    }
    private static string GetSmsType(SmsType type)
    {
        switch (type)
        {
            case SmsType.CheckTips:
                return "检查提醒：\n";
            case SmsType.HiddenTroubleTips:
                return "隐患提醒：\n";
            case SmsType.MoveTips:
                return "走动提醒：\n";
            case SmsType.NotifyAdmin:
                return "通知管理员：\n";
            case SmsType.NotifyLeader:
                return "通知领导：\n";
            case SmsType.None:
                return "";
            default:
                return "温馨提示：\n";

        }
    }

    private static string GetPhoneViaName(string name)
    {
        name = name.Replace(" ", "").Trim();
        DBSCMDataContext dc = new DBSCMDataContext();
        var phone = from psn in dc.Person
                    where psn.Name == name && psn.Maindeptid=="241700000"
                    select psn.Tel;
        if (phone.Count() > 0)
        {
            return phone.First();
        }
        else
        {
            return "0";
        }
    }
}
public enum SmsType : int
{
    MoveTips = 1, NotifyLeader, HiddenTroubleTips, NotifyAdmin, CheckTips,None,Other
}
