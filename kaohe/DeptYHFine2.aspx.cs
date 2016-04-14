using System;
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
using DevExpress.Web.ASPxEditors;
public partial class kaohe_DeptYHFine2 : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            deteedit.Date = DateTime.Parse(System.DateTime.Now.Year + "-" + System.DateTime.Now.Month + "-01");
            ASPxDateEdit1.Date = System.DateTime.Today;

            if (SessionBox.GetUserSession().rolelevel.Contains("0"))
            {
                //OREcbox.Value = "241700000";
                bindAllORE();
                OREcbox.Value = "241700000";
                GetKQData();
            }
            else if (SessionBox.GetUserSession().rolelevel.Contains("1") && (!SessionBox.GetUserSession().rolelevel.Contains("2")))
            {
                bindAllORE();
                OREcbox.Value = "241700000";
                GetKQData();
            }
            else
            {
                bindAllORE();
                OREcbox.Value = SessionBox.GetUserSession().DeptNumber;
                GetKQData();
            }

        }

        bindByRole();



    }

    //根据角色绑定考核信息
    private void bindByRole()
    {
        if (SessionBox.GetUserSession().rolelevel.Contains("0"))
        {
            Bind(OREcbox.Value.ToString().Trim(),"");
        }
        else if (SessionBox.GetUserSession().rolelevel.Contains("1") && (!SessionBox.GetUserSession().rolelevel.Contains("2")))
        {
            if (cboKequ.Value != "-1")
            {

                Bind(OREcbox.Value.ToString().Trim(),cboKequ.Value.ToString());
            }
            else
            {
                Bind(OREcbox.Value.ToString().Trim(),"");
            }
        }
        else
        {
            //ASPxLabel1.Visible = false;
            //OREcbox.Visible = false;  
            OREcbox.Value = SessionBox.GetUserSession().DeptNumber;
            OREcbox.Enabled = false;
            if (cboKequ.Value != "-1")
            {
                Bind(SessionBox.GetUserSession().DeptNumber,cboKequ.Value.ToString());
            }
            else
            {
                Bind(SessionBox.GetUserSession().DeptNumber,"");
            }
        }

    }


    private void Bind(string deptnm,string kequ)
    {
        DataSet ds = GetKaoHeInfo.GetDeptYHFine(deteedit.Date, ASPxDateEdit1.Date, deptnm,kequ);
        ASPxGridView1.DataSource = ds;
        ASPxGridView1.DataBind();
    }
    //绑定全部矿
    private void bindAllORE()
    {
        //string strsql = "select * from department t where t.deptname like '%矿' and t.deptnumber like'%00000' order by deptname";
        //DataSet ds = OracleHelper.Query(strsql);
        //OREcbox.DataSource = ds;
        //OREcbox.DataBind();
        Util.BindDll(OREcbox, "(substr(deptnumber,1,2)='23' or substr(deptnumber,1,2)='24' or substr(deptnumber,1,2)='13'  or substr(deptnumber,1,3)='552' or substr(deptnumber,1,4)='5503') and substr(deptnumber,5,5)='00000'", "DEPTNAME", "DEPTNUMBER");

    }
    private void GetKQData()
    {
        if (OREcbox.SelectedIndex !=-1)
        {
            Util.BindDll(cboKequ, "ID", OREcbox.Value.ToString().Remove(4), "NAME", "ID");
            cboKequ.Items.Insert(0, new ListEditItem("--全部--", "-1"));
            cboKequ.Value = "-1";
        }
        else
        {
            cboKequ.Items.Clear();
            cboKequ.Items.Insert(0, new ListEditItem("--全部--", "-1"));
            cboKequ.Value = "-1";
        }
    }
    protected void ASPxButton2_Click(object sender, EventArgs e)
    {
        bindByRole();
    }
    protected void ASPxButton1_Click(object sender, EventArgs e)
    {

        ASPxGridView1.SettingsText.Title = "" + deteedit.Date.ToString("yyyy/MM/dd") + "到" + ASPxDateEdit1.Date.ToString("yyyy/MM/dd") + "单位隐患积分表";
        bindByRole();
        ASPxGridViewExporter1.WriteXlsToResponse("" + deteedit.Date.ToString("yyyy/MM/dd") + "到" + ASPxDateEdit1.Date.ToString("yyyy/MM/dd") + "单位隐患积分表");
    }
    protected void OREcbox_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetKQData();
    }
    protected void ASPxGridView1_PageIndexChanged(object sender, EventArgs e)
    {
        bindByRole();
    }
    protected void ASPxGridView1_CustomUnboundColumnData(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewColumnDataEventArgs e)
    {
        if (e.Column.FieldName == "Total")
        {
            decimal price1 = (decimal)e.GetListSourceFieldValue("KCF");
            decimal price2 = (decimal)e.GetListSourceFieldValue("ZCF");
            e.Value = price1 + price2;
        }
    }
    protected void ASPxGridView1_BeforeColumnSortingGrouping(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
    {
        bindByRole();
    }
}
