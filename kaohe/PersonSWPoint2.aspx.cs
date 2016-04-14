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

public partial class kaohe_PersonSWPoint2 : BasePage
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
            }
            else if (SessionBox.GetUserSession().rolelevel.Contains("1") && (!SessionBox.GetUserSession().rolelevel.Contains("2")))
            {
                bindAllORE();
                OREcbox.Value = "241700000";
            }
            else
            {
                bindAllORE();
                OREcbox.Value = SessionBox.GetUserSession().DeptNumber;
            }
        }

        bindByRole();



    }

    //根据角色绑定考核信息
    private void bindByRole()
    {
        if (SessionBox.GetUserSession().rolelevel.Contains("0"))
        {
            Bind(OREcbox.Value.ToString().Trim(), txtName.Text.Trim());
        }
        else if (SessionBox.GetUserSession().rolelevel.Contains("1") && (!SessionBox.GetUserSession().rolelevel.Contains("2")))
        {

            Bind(OREcbox.Value.ToString().Trim(), txtName.Text.Trim());

        }
        else
        {
            //ASPxLabel1.Visible = false;
            //OREcbox.Visible = false;  
            OREcbox.Value = SessionBox.GetUserSession().DeptNumber;
            OREcbox.Enabled = false;

            Bind(SessionBox.GetUserSession().DeptNumber, txtName.Text.Trim());

        }

    }


    private void Bind(string deptnm,string psn)
    {

        DataSet ds = GetKaoHeInfo.GetPersonSWPoint(deteedit.Date, ASPxDateEdit1.Date, deptnm,psn);
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

    protected void ASPxButton2_Click(object sender, EventArgs e)
    {
        bindByRole();
    }
    protected void ASPxButton1_Click(object sender, EventArgs e)
    {

        ASPxGridView1.SettingsText.Title = "" + deteedit.Date.ToString("yyyy/MM/dd") + "到" + ASPxDateEdit1.Date.ToString("yyyy/MM/dd") + "个人三违积分表";
        bindByRole();
        ASPxGridViewExporter1.WriteXlsToResponse("" + deteedit.Date.ToString("yyyy/MM/dd") + "到" + ASPxDateEdit1.Date.ToString("yyyy/MM/dd") + "个人三违积分表");
    }
    //protected void ASPxGridView1_CustomUnboundColumnData(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewColumnDataEventArgs e)
    //{
        
    //    if (e.Column.FieldName == "Total")
    //    {
    //        if ((int)e.GetListSourceFieldValue("三违总数") <= 1)
    //        {
    //            decimal price1 = (decimal)e.GetListSourceFieldValue("KCP");
    //            decimal price2 = (decimal)e.GetListSourceFieldValue("ZCP");
    //            e.Value = price1 + price2;
    //        }
    //        else if ((int)e.GetListSourceFieldValue("三违总数") == 2)
    //        {

    //        }
    //        else if ((int)e.GetListSourceFieldValue("三违总数") == 3)
    //        {

    //        }
    //        else
    //        {

    //        }
            
    //    }
    //}

    protected void ASPxGridView1_BeforeColumnSortingGrouping(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
    {
        bindByRole();
    }
    protected void ASPxGridView1_PageIndexChanged(object sender, EventArgs e)
    {
        bindByRole();
    }
}
