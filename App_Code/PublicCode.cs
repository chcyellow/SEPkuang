using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using GhtnTech.SEP.DAL;
using System.Text;
using System.Data.OracleClient;
using GhtnTech.SEP.DBUtility;
using System.Data;

/// <summary>
///PublicCode 的摘要说明
/// </summary>
public class PublicCode
{
    public PublicCode()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }


    public static void setUpdateMoveState(string pcperson, DateTime pctime, decimal place)
    {
        //修改闭合时默认走动时间
        string sql = "update moveplan set MoveState='已走动',MOVESTARTTIME=to_date('{0}','yyyy-MM-dd'),MOVEENDTIME=to_date('{1}','yyyy-MM-dd'),Closeremarks='录入隐患闭合' where PersonID='{2}' and (to_date('{3}','yyyy-MM-dd') between StartTime and EndTime) ";
        //sql += "and (PlaceID='{2}' or PlaceID in (select PlaceID from Place where PAreasID in (select PAreasID from Place where PlaceID='{3}'))) and MoveState='未走动' ";
        sql += "and PlaceID='{4}' and MoveState='未走动' ";
        OracleHelper.ExecuteSql(string.Format(sql,pctime.ToString("yyyy-MM-dd"),pctime.ToString("yyyy-MM-dd"),pcperson,pctime.ToString("yyyy-MM-dd"),place));
        //DBSCMDataContext dc = new DBSCMDataContext();
        //var mp = from m in dc.Moveplan
        //         from p in dc.Place
        //         where m.Placeid == p.Placeid
        //            && m.Personid == pcperson
        //            && m.Starttime <= pctime
        //            && m.Endtime >= pctime
        //            && m.Movestate == "未走动"
        //            && p.Pareasid == dc.Place.First(k => k.Placeid == place).Pareasid
        //         select m;
        //foreach (var r in mp)
        //{
        //    r.Movestate = "已走动";
        //}
        //if (mp.Count() > 0)
        //{
        //    dc.SubmitChanges();
        //}
    }

    /// <summary>
    /// 获取矿下科级单位
    /// </summary>
    /// <param name="maindept">矿编号</param>
    /// <returns></returns>
    public static IOrderedQueryable<GhtnTech.SEP.DAL.Department> GetKQdept(string maindept)
    {
        DBSCMDataContext dc = new DBSCMDataContext();
        IOrderedQueryable<GhtnTech.SEP.DAL.Department> dept = dc.Department.Where(p => p.Deptnumber.Substring(0, 4) == maindept.Substring(0, 4) && (p.Deptlevel == "正科级") && p.Deptstatus == "1").OrderBy(p => p.Deptname);
                                                             //dc.Department.Where(p => p.Deptnumber.Substring(0, 4) == maindept.Substring(0, 4) && (p.Deptlevel == "正科级" || p.Deptnumber.Substring(7) == "00") && p.Deptstatus == "1").OrderBy(p => p.Deptname);
                   return dept;
    }
    public static string GetKQdeptNumber(string personnumber)
    {
        DBSCMDataContext dc = new DBSCMDataContext();
        return dc.Person.Where(p => p.Personnumber == personnumber).First().Areadeptid;
        
    }
    /// <summary>
    /// 获取矿级列表
    /// </summary>
    /// <param name="maindept">矿编号，为空获取所有</param>
    /// <returns></returns>
    public static IOrderedQueryable<GhtnTech.SEP.DAL.Department> GetMaindept(string maindept)
    {
        DBSCMDataContext dc = new DBSCMDataContext();
        IOrderedQueryable<GhtnTech.SEP.DAL.Department> dept = dc.Department.Where(p => p.Deptlevel == "正处级" && p.Deptstatus=="1").OrderBy(p => p.Deptname);
        if (maindept != "")
        {
            dept = dept.Where(p => p.Deptnumber == maindept).OrderBy(p => p.Deptname);
        }
        return dept;
    }

    public static int GetSWMaxScoreSet(string maindept)
    {
        DBSCMDataContext dc = new DBSCMDataContext();
        var w = dc.Swwarningset.Where(p => p.Deptnumber == maindept);
        if (w.Count() > 0)
        {
            return int.Parse(w.First().Maxscore.ToString());
        }
        else
        {
            return 12;
        }
    }

    public static int GetSWMaxCountSet(string maindept)
    {
        DBSCMDataContext dc = new DBSCMDataContext();
        var w = dc.Swwarningset.Where(p => p.Deptnumber == maindept);
        if (w.Count() > 0)
        {
            return int.Parse(w.First().Maxcount.ToString());
        }
        else
        {
            return 2;
        }
    }

    public static bool BugCreate(string maindept)
    {
        //if (maindept == "246300000")
        //{
        //    if (System.DateTime.Today > new DateTime(2012, 9, 22))
        //    {
        //        return true;
        //    }
        //}
        return false;

    }

    public static decimal GetKQrecord(string personnumber, DateTime pcdate, string pcbanci,string inputperson)
    {
        DBSCMDataContext dc = new DBSCMDataContext();
        var krs=dc.KqRecord.Where(p => p.Kqpnumber == personnumber && p.Kqtime == pcdate && p.Kqbenci==pcbanci);
        if (krs.Count() == 0)
        {
            string historysql = string.Format("SELECT short_name, stateflag,person_name, entertime_mine, outtime_mine, trackblock, station_mark FROM v_jykj_history where remark2='{0}' and outtime_mine>='{1}' order by outtime_mine desc;",
                    personnumber.PadLeft(7, '0'),
                    System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            PostGreSQLHelper pgh = new PostGreSQLHelper();
            DataTable dthistory = pgh.ExecuteQuery(historysql).Tables[0];
            if (dthistory.Rows.Count > 0)
            {
                KqRecord kr = new KqRecord
                {
                    Kqtime = pcdate,
                    Kqbenci = pcbanci,
                    Inputperson = inputperson,
                    Downtime = DateTime.Parse(dthistory.Rows[0]["entertime_mine"].ToString()),
                    Uptime = DateTime.Parse(dthistory.Rows[0]["outtime_mine"].ToString()),
                    Kqpnumber = personnumber,
                    Zdgj = dthistory.Rows[0]["trackblock"].ToString().Trim()
                };
                dc.KqRecord.InsertOnSubmit(kr);
                dc.SubmitChanges();
                return dc.KqRecord.First(p => p.Kqpnumber == personnumber && p.Downtime == kr.Downtime).Rjid;
            }
            else
            {
                return -1;
            }
        }
        else
        {
            if (krs.First().Uptime >= System.DateTime.Now.AddHours(-1))
            {
                return krs.First().Rjid;
            }
            else
            {
                return -1;
            }
        }
    }


    public static string GetBanci(DateTime dt)
    {
        var t1 = new TimeSpan(14, 0, 0);//早班结束时间
        var t2 = new TimeSpan(22, 0, 0);//中班结束时间
        var t3 = new TimeSpan(6, 0, 0);//夜班结束时间
        var c = dt.TimeOfDay;
        if (c >= t1 && c <= t2)
        {
            return "中班";
        }
        else if (c >= t3 && c < t1)
        {
            return "早班";

        }
        else if (c>t2 || c < t3)
        {
            return "夜班";
        }
        else
        {
            return "早班";
        }
    }
}
