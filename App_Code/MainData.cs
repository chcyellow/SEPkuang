using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GhtnTech.SEP.DBUtility;
using System.Data.OracleClient;
using GhtnTech.SecurityFramework.BLL;
using GhtnTech.SEP.DAL;
using System.Data;

/// <summary>
///MainData 的摘要说明
/// </summary>
public class MainData
{
	public MainData()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}
    public static void Getdata(List<WaitindDo> data, string kind, List<DataGroup> group)
    {
        if (data.Count > 0)
        {
            group.Add(new DataGroup { Kind = kind });
            foreach (var r in data)
            {
                string date = r.Date;
                string remark = r.Detail.Length > 11 ? string.Concat(r.Detail.Substring(0, 11), "....") : r.Detail;
                string url = string.Concat(r.Url, "?", r.Ziduan, "=", r.ID);
                string describe = r.Describe;
                DataGroup row = group[group.Count - 1];
                row.Details.Add(new { id = "e" + r.Ziduan + r.ID, date, remark, url, describe });
            }
        }
    }
    public class DataGroup
    {
        private List<object> detail;

        public string Kind { get; set; }

        public List<object> Details
        {
            get
            {
                if (this.detail == null)
                {
                    this.detail = new List<object>();
                }
                return detail;
            }
        }
    }
    public class WaitindDo
    {
        public string Date { get; set; }
        public string Detail { get; set; }
        public string Ziduan { get; set; }
        public string ID { get; set; }
        public string Url { get; set; }
        public string Describe { get; set; }
    }
    private const string NEW_MARKER = "<span>&nbsp;</span>";
    public static string MarkNew(string name, bool bl)
    {
        return bl ? name + NEW_MARKER : name;
    }

    /// <summary>
    /// 获取首页隐患信息--矿领导:待审批（已用）
    /// </summary>
    /// <param name="PerID">登陆账号人员ID</param>
    /// <param name="DepID">登陆账号部门ID</param>
    /// <returns>返回行数</returns>
    public static string[] getYHDSP_kld(string PerID, string DepID)
    {
        DBSCMDataContext dc = new DBSCMDataContext();
        int count = 0;
        string group = "";
        var q = from yh in dc.Getyhinput
                //from yhNum in dc.YinHuanNumber
                where //yh.YHNumber == yhNum.YHNumber && 
                yh.Status == "提交审批" && yh.Refer == 1 && yh.Referid == PerID
                select yh.Yhputinid;
        count = q.Count();
        foreach (var r in q)
        {
            group += r.ToString() + ",";
        }
        group = count > 0 ? group.Substring(0, group.Length - 1) : "";
        return new string[] { count.ToString(), group };
    }

    /// <summary>
    /// 获取首页隐患信息--职能科室:待审批（已用）
    /// </summary>
    /// <param name="PerID">登陆账号人员ID</param>
    /// <param name="DepID">登陆账号部门ID</param>
    /// <returns>返回行数</returns>
    public static string[] getYHDSP_znks(string PerID, string DepID)
    {
        DBSCMDataContext dc = new DBSCMDataContext();
        int count = 0;
        string group = "";
        var q = from yh in dc.Getyhinput
                //from yhNum in dc.YinHuanNumber
                where //yh.YHNumber == yhNum.YHNumber && 
                yh.Status == "提交审批" && yh.Refer == 2 && yh.Referid == DepID
                select yh.Yhputinid;
        count = q.Count();
        foreach (var r in q)
        {
            group += r.ToString() + ",";
        }
        group = count > 0 ? group.Substring(0, group.Length - 1) : "";
        return new string[] { count.ToString(), group };
    }

    /// <summary>
    /// 获取首页隐患信息--走动干部:待复查（已用）
    /// </summary>
    /// <param name="PerID">登陆账号人员ID</param>
    /// <param name="DepID">登陆账号部门ID</param>
    /// <returns>返回行数</returns>
    public static string[] getYHDFC_zdgb(string PerID, string DepID)
    {
        DBSCMDataContext dc = new DBSCMDataContext();
        int count = 0;
        string group = "";
        var q = from yh in dc.Getyhinput
                //from yhNum in dc.YinHuanNumber
                from MP in dc.Moveplan
                where //yh.YHNumber == yhNum.YHNumber  
                    //MP.Personid == PerID &&
                yh.Placeid == MP.Placeid && yh.Status == "隐患已整改" && yh.Unitid == DepID
                && MP.Starttime <= System.DateTime.Today && MP.Endtime >= System.DateTime.Today && MP.Movestate.Trim() == "未走动"
                && MP.Personid==PerID//add by xl
                select yh.Yhputinid;
        count = q.Count();
        foreach (var r in q)
        {
            group += r.ToString() + ",";
        }
        group = count > 0 ? group.Substring(0, group.Length - 1) : "";
        return new string[] { count.ToString(), group };
    }

    ///// <summary>
    ///// 获取首页隐患信息--责任部门:复查未通过
    ///// </summary>
    ///// <param name="PerID">登陆账号人员ID</param>
    ///// <param name="DepID">登陆账号部门ID</param>
    ///// <returns>返回行数</returns>
    ///// <summary>
    //public static string[] getYHFCWTG_zrbmf(string PerID, string DepID)
    //{
    //    DBSCMDataContext dc = new DBSCMDataContext();
    //    int count = 0;
    //    string group = "";
    //    var q = from yh in dc.Yhinput
    //            //from yhNum in dc.YinHuanNumber
    //            from yc in dc.Yinhuancheck
    //            where //yh.YHNumber == yhNum.YHNumber &&
    //            yc.Responsibledept == DepID &&
    //            yh.Yhputinid == yc.Yhputinid && yh.Status == "复查未通过"
    //            select yh.Yhputinid;
    //    count = q.Count();
    //    foreach (var r in q)
    //    {
    //        group += r.ToString() + ",";
    //    }
    //    group = count > 0 ? group.Substring(0, group.Length - 1) : "";
    //    return new string[] { count.ToString(), group };
    //}
    /// <summary>
    /// 获取首页隐患信息--责任部门:隐患未整改/整改反馈（已用）
    /// </summary>
    /// <param name="PerID">登陆账号人员ID</param>
    /// <param name="DepID">登陆账号部门ID</param>
    /// <returns>返回行数</returns>
    /// <summary>
    public static string[] getYHWZG_zrbmw(string PerID, string DepID)
    {
        DBSCMDataContext dc = new DBSCMDataContext();
        int count = 0;
        string group = "";
        var q = from yh in dc.Getyhinput
                //from yhNum in dc.YinHuanNumber
                from yc in dc.Nyinhuancheck
                where //yh.YHNumber == yhNum.YHNumber &&
                yc.Responsibledept == DepID &&
                yh.Yhputinid == yc.Yhputinid && yh.Status == "隐患未整改"
                select yh.Yhputinid;
        count = q.Count();
        foreach (var r in q)
        {
            group += r.ToString() + ",";
        }
        group = count > 0 ? group.Substring(0, group.Length - 1) : "";
        return new string[] { count.ToString(), group };
    }

    /// <summary>
    /// 获取首页现场整改隐患信息-（未用）
    /// </summary>
    /// <param name="PerID">登陆账号人员ID</param>
    /// <param name="DepID">登陆账号部门ID</param>
    /// <returns>返回行数</returns>
    public static string[] getYHinfoDXCZG( string DepID)
    {
        DBSCMDataContext dc = new DBSCMDataContext();
        int count = 0;
        string group = "";

        var q = from yh in dc.Getyhinput
                where yh.Status == "现场整改" && (yh.Pctime.Value.Year.ToString().Trim() + yh.Pctime.Value.Month.ToString().Trim()) == (DateTime.Now.Year.ToString().Trim() + DateTime.Now.Month.ToString().Trim()) && yh.Deptid == DepID
                select yh.Yhputinid;
        count = q.Count();
        foreach (var r in q)
        {
            group += r.ToString() + ",";
        }


        group = count > 0 ? group.Substring(0, group.Length - 1) : "";
        return new string[] { count.ToString(), group };
    }

    /// <summary>
    /// 获取首页责任部门逾期未整改隐患信息--（未用）
    /// </summary>

    /// <param name="PerID">登陆账号人员ID</param>
    /// <param name="DepID">登陆账号部门ID</param>
    /// <returns>返回行数</returns>

    public static string[] getYHinfoDYQ(string DepID)
    {
        DBSCMDataContext dc = new DBSCMDataContext();
        int count = 0;
        string group = "";

        var q = from yh in dc.Getyhinput
                where yh.Status == "逾期未整改" && yh.Deptid == DepID
                select yh.Yhputinid;
        count = q.Count();
        foreach (var r in q)
        {
            group += r.ToString() + ",";
        }


        group = count > 0 ? group.Substring(0, group.Length - 1) : "";
        return new string[] { count.ToString(), group };
    }

    /// <summary>
    /// 获取首页新增隐患信息--（已用）
    /// </summary>

    /// <param name="PerID">登陆账号人员ID</param>
    /// <param name="DepID">登陆账号部门ID</param>
    /// <returns>返回行数</returns>

    public static string[] getYHinfoD1(string DepID)
    {
        DBSCMDataContext dc = new DBSCMDataContext();
        int count = 0;
        string group = "";

        var q = from yh in dc.Getyhinput
                where yh.Status == "新增" && yh.Unitid == DepID
                select yh.Yhputinid;
        count = q.Count();
        foreach (var r in q)
        {
            group += r.ToString() + ",";
        }


        group = count > 0 ? group.Substring(0, group.Length - 1) : "";
        return new string[] { count.ToString(), group };
    }

    /// <summary>
    /// 获取首页隐患已整改信息--（未用）
    /// </summary>

    /// <param name="PerID">登陆账号人员ID</param>
    /// <param name="DepID">登陆账号部门ID</param>
    /// <returns>返回行数</returns>

    public static string[] getYHinfoD2(string PerID, string DepID)
    {
        DBSCMDataContext dc = new DBSCMDataContext();
        int count = 0;
        string group = "";

        var q = from yh in dc.Getyhinput
                where yh.Status == "隐患已整改"
                select yh.Yhputinid;
        count = q.Count();
        foreach (var r in q)
        {
            group += r.ToString() + ",";
        }


        group = count > 0 ? group.Substring(0, group.Length - 1) : "";
        return new string[] { count.ToString(), group };
    }

    /// <summary>
    /// 获取首页逾期未整改隐患信息--（已用）
    /// </summary>

    /// <param name="PerID">登陆账号人员ID</param>
    /// <param name="DepID">登陆账号部门ID</param>
    /// <returns>返回行数</returns>

    public static string[] getYHinfoD3(string DepID)
    {
        DBSCMDataContext dc = new DBSCMDataContext();
        int count = 0;
        string group = "";

        var q = from yh in dc.Getyhinput
                where yh.Status == "逾期未整改" && yh.Unitid == DepID
                select yh.Yhputinid;
        count = q.Count();
        foreach (var r in q)
        {
            group += r.ToString() + ",";
        }

        group = count > 0 ? group.Substring(0, group.Length - 1) : "";
        return new string[] { count.ToString(), group };
    }

    /// <summary>
    /// 获取首页逾期整改未完成信息--（已用）
    /// </summary>

    /// <param name="PerID">登陆账号人员ID</param>
    /// <param name="DepID">登陆账号部门ID</param>
    /// <returns>返回行数</returns>

    public static string[] getYHinfoD4(string DepID)
    {
        DBSCMDataContext dc = new DBSCMDataContext();
        int count = 0;
        string group = "";

        var q = from yh in dc.Getyhinput
                where yh.Status == "逾期整改未完成" && yh.Unitid == DepID
                select yh.Yhputinid;
        count = q.Count();
        foreach (var r in q)
        {
            group += r.ToString() + ",";
        }


        group = count > 0 ? group.Substring(0, group.Length - 1) : "";
        return new string[] { count.ToString(), group };
    }

    /// <summary>
    /// 获取首页复查未通过隐患信息--（已用）
    /// </summary>

    /// <param name="PerID">登陆账号人员ID</param>
    /// <param name="DepID">登陆账号部门ID</param>
    /// <returns>返回行数</returns>

    public static string[] getYHinfoD5(string DepID)
    {
        DBSCMDataContext dc = new DBSCMDataContext();
        int count = 0;
        string group = "";

        var q = from yh in dc.Getyhinput
                where yh.Status == "复查未通过" && yh.Unitid == DepID
                select yh.Yhputinid;
        count = q.Count();
        foreach (var r in q)
        {
            group += r.ToString() + ",";
        }


        group = count > 0 ? group.Substring(0, group.Length - 1) : "";
        return new string[] { count.ToString(), group };
    }

    /// <summary>
    /// 获取首页待办三违信息--（已用）
    /// </summary>
    /// <returns>返回行数</returns>

    public static string[] getSWinfoD()
    {
        DBSCMDataContext dc = new DBSCMDataContext();
        int count = 0;
        string group = "";
        var q = from a in dc.Nswinput
                //from b in dc.SanWeiNumber
                where //a.SWNumber == b.SWNumber && 
                a.Isend == 0
                select a.Id;
        count = q.Count();
        foreach (var r in q)
        {
            group += r.ToString() + ",";
        }
        group = count > 0 ? group.Substring(0, group.Length - 1) : "";
        return new string[] { count.ToString(), group };
    }
    /// <summary>
    /// 获取首页走动计划信息--（未用）
    /// </summary>
    /// <param name="Roleid">登陆账号角色</param>
    /// <param name="PerID">登陆账号人员ID</param>
    /// <returns>返回行数</returns>
    public static int getPLinfoD(string PerID)
    {
        DBSCMDataContext dc = new DBSCMDataContext();
        int count = 0;
        count = (from a in dc.Moveplan
                 where  (a.Starttime.Value<=DateTime.Today && DateTime.Today <=a.Endtime.Value ) && a.Movestate=="未走动"  && a.Personid== PerID 
                 select new
                 {
                     a.Id

                 }).Count();
        return count;
    }

    public static int getYujingSW(string deptnumber)//三违预警
    {
        //DataSet ds = GetKaoHeInfo.GetPersonSWPoint(DateTime.Parse(System.DateTime.Today.Year + "-01-01"), DateTime.Parse(System.DateTime.Today.Year + "-12-31"), deptnumber, "");
        //System.Data.DataView dv = new System.Data.DataView(ds.Tables[0]);
        //string filter = "三违总数>=" + PublicCode.GetSWMaxCountSet(deptnumber) + " or TOTAL>="+PublicCode.GetSWMaxScoreSet(deptnumber);
        //dv.RowFilter = filter;
        //ds.Tables.Clear();
        //ds.Tables.Add(dv.ToTable());
        //return ds.Tables[0].Rows.Count;

        DateTime today = DateTime.Today;
        DateTime dateTime = DateTime.Parse(string.Concat(today.Year, "-01-01"));
        today = DateTime.Today;
        DataSet personSWPoint = GetKaoHeInfo.GetPersonSWPoint(dateTime, DateTime.Parse(string.Concat(today.Year, "-12-31")), deptnumber, "");
        DataView dataViews = new DataView(personSWPoint.Tables[0]);
        object[] sWMaxCountSet = new object[] { "三违总数>=", PublicCode.GetSWMaxCountSet(deptnumber), " or TOTAL>=", PublicCode.GetSWMaxScoreSet(deptnumber) };
        dataViews.RowFilter = string.Concat(sWMaxCountSet);
        personSWPoint.Tables.Clear();
        personSWPoint.Tables.Add(dataViews.ToTable());
        return personSWPoint.Tables[0].Rows.Count;
        //DBSCMDataContext dc = new DBSCMDataContext();
        //var data = (from sw in dc.Nswinput
        //            from per in dc.Person
        //            from dep in dc.Department
        //            from sb in dc.Swbase
        //            from s in dc.Swscore
        //            where sw.Swpersonid == per.Personnumber && per.Areadeptid == dep.Deptnumber && sw.Swid == sb.Swid && sb.Levelid == s.Levelid
        //            && sw.Pctime >= DateTime.Parse(System.DateTime.Today.Year + "-01-01") && sw.Pctime <= DateTime.Parse(System.DateTime.Today.Year + "-12-31")
        //            && sw.Maindeptid == deptnumber && sw.Isend == 1
        //            select new
        //            {
        //                per.Personnumber,
        //                per.Name,
        //                dep.Deptnumber,
        //                dep.Deptname,
        //                s.Kcscore
        //            }).ToList();
        //var group = from p in data
        //            group p by new
        //            {
        //                p.Deptname,
        //                p.Name
        //            }
        //                into g
        //                select new
        //                {
        //                    g.Key.Deptname,
        //                    g.Key.Name,
        //                    Score = g.Sum(p => p.Kcscore),
        //                    Count = g.Count()
        //                };
        //return group.Count(p => p.Score >= 12 || p.Count >= 2);
        
    }

    public static int getNocloseUpYH(string deptnumber)//未闭合升级隐患
    {
        DBSCMDataContext dc = new DBSCMDataContext();
        var yh = dc.Getyhinput.Where(p => p.Unitid == deptnumber && p.Levelname.Length > 2 && p.Status != "复查通过" && p.Pctime > System.DateTime.Today.AddDays(1 - System.DateTime.Today.Day));
        return yh.Count();
    }
}
