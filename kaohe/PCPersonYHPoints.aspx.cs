﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.OracleClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GhtnTech.SEP.DAL;
using GhtnTech.SecurityFramework.BLL;
using GhtnTech.SEP.DBUtility;

public partial class kaohe_PCPersonYHPoints : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            deteedit.Date = DateTime.Parse(System.DateTime.Now.Year + "-" + System.DateTime.Now.Month + "-01");
            ASPxDateEdit1.Date = System.DateTime.Today;

            if (SessionBox.GetUserSession().rolelevel.Contains("1") || SessionBox.GetUserSession().rolelevel.Contains("0"))
            {
                OREcbox.Value = "241700000";
            }
            else
            {
                ASPxLabel1.Text = "选择单位";
                
            }
            bindAllORE();
        }
        
        bindByRole();
       
     

    }

    //根据角色绑定考核信息
    private void bindByRole()
    {
        if (SessionBox.GetUserSession().rolelevel.Contains("1") || SessionBox.GetUserSession().rolelevel.Contains("0"))
        {
            Bind(OREcbox.Value.ToString().Trim(),"");
        }
        else
        {
            //ASPxLabel1.Visible = false;
            //OREcbox.Visible = false;
            if (OREcbox.SelectedIndex > -1)
            {
                Bind(SessionBox.GetUserSession().DeptNumber, OREcbox.Text.ToString().Trim());
            }
            else
            {
                Bind(SessionBox.GetUserSession().DeptNumber, "");
            }
        }

    }


    private void Bind(string unitnm, string deptname)
    {
        //string sql = "select * from GETALLYHSWCOUNTBYPER where unitid='" + SessionBox.GetUserSession().DeptNumber + "'";
        //DataSet ds = OracleHelper.Query(sql);
        DataSet ds = GetKaoHeInfo.getallyhswcountbyper(deteedit.Date, ASPxDateEdit1.Date, unitnm);
        if (deptname != "")
        {
            System.Data.DataView dv = new System.Data.DataView(ds.Tables[0]);
            //dv.RowFilter = "DEPTNUMBER='" + deptnm + "'";
            dv.RowFilter = "DEPTNAME='" + deptname + "'";
            ds.Tables.Clear();
            ds.Tables.Add(dv.ToTable());
        }
        ASPxGridView1.DataSource = ds;
        ASPxGridView1.DataBind();
    }
    //绑定全部矿
    private void bindAllORE()
    {
        if (SessionBox.GetUserSession().rolelevel.Contains("1") || SessionBox.GetUserSession().rolelevel.Contains("0"))
        {
            string strsql = "select * from department t where t.deptname like '%矿' and t.deptnumber like'%00000' order by deptname";
            DataSet ds = OracleHelper.Query(strsql);
            OREcbox.DataSource = ds;
            OREcbox.DataBind();
        }
        else
        {
            string strsql = "select * from department t where t.deptnumber like '2417_____' and (t.deptlevel='正科级' or t.deptnumber like '_______00') order by deptname desc";
            DataSet ds = OracleHelper.Query(strsql);
            OREcbox.DataSource = ds;
            OREcbox.DataBind();
        }

    }

    protected void ASPxButton2_Click(object sender, EventArgs e)
    {
        bindByRole();
    }
    protected void ASPxButton1_Click(object sender, EventArgs e)
    {

        ASPxGridView1.SettingsText.Title = "" + deteedit.Date.ToString("yyyy/MM/dd") + "到" + ASPxDateEdit1.Date.ToString("yyyy/MM/dd") + "个人安全排查积分表";
        bindByRole();
        ASPxGridViewExporter1.WriteXlsToResponse("" + deteedit.Date.ToString("yyyy/MM/dd") + "到" + ASPxDateEdit1.Date.ToString("yyyy/MM/dd") + "个人安全排查积分表");
    }

  
    
}
