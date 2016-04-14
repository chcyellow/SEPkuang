using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OracleClient;
using System.Xml;
using DevExpress.Web.ASPxTreeList;
using DevExpress.Web.ASPxEditors;
using GhtnTech.SEP.DBUtility;

public partial class HazardManage_ProcessSetINfo : System.Web.UI.Page
{

    private string deptnumber = "000000000";
    private string usernumber = "343";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    //绑定专业
    private void bind()
    {
       
         string zyID = Request.QueryString["zyID"].Trim();
            string oracletext = "select * from CS_BASEINFOSET where INFOID= " + zyID;
            ASPxComboBox1.DataSource = OracleHelper.Query(oracletext);
            ASPxComboBox1.DataBind();
            ASPxComboBox1.Value = zyID;
            ASPxComboBox1.Enabled = false;

            oracletext = "select * FROM WORKTASKS_TEMP where STATUS='保存' and PROFESSIONALID = " + ASPxComboBox1.SelectedItem.Value.ToString().Trim() + " ";
            ASPxListBox1.DataSource = OracleHelper.Query(oracletext);
            ASPxListBox1.DataBind();
            ASPxGridView2.Visible = true;
    }
    protected void ASPxCallbackPanel1_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
    {
        string oracletext = "";
        if (e.Parameter.Trim() == "i")
        {
            string msg="";
            if (ASPxTextBox1.Text.Trim() == "")
            {
                ASPxLabel2.Text = "请填写工作任务内容！";
                return;
            }
            oracletext = "select * FROM WORKTASKS_TEMP where STATUS='保存' and WORKTASK='"+ASPxTextBox1.Text.Trim()+"' and PROFESSIONALID = " + ASPxComboBox1.SelectedItem.Value.ToString().Trim() + " ";
            if (OracleHelper.Query(oracletext).Tables[0].Rows.Count > 0)
            {
                msg = "已经添加的工作任务";
            }
            else
            {
                oracletext = "select * FROM WORKTASKS where WORKTASK='" + ASPxTextBox1.Text.Trim() + "' and PROFESSIONALID = " + ASPxComboBox1.SelectedItem.Value.ToString().Trim() + " ";
                if (OracleHelper.Query(oracletext).Tables[0].Rows.Count > 0)
                {
                    msg = "标准库已经存在的工作任务";
                }
                else
                {
                    GhtnTech.SEP.OraclDAL.DALWORKTASKS_TEMP wt = new GhtnTech.SEP.OraclDAL.DALWORKTASKS_TEMP();
                    try
                    {
                        wt.InsertDALWORKTASKS_TEMP(ASPxTextBox1.Text.Trim(), int.Parse(ASPxComboBox1.SelectedItem.Value.ToString().Trim()), deptnumber, usernumber);
                        msg = "保存成功!";
                    }
                    catch
                    {
                        msg = "保存失败，请稍候重试!";
                    }
                }
            }

            ASPxLabel2.Text = msg;
        }
        oracletext = "select * FROM WORKTASKS_TEMP where STATUS='保存' and PROFESSIONALID = " + ASPxComboBox1.SelectedItem.Value.ToString().Trim() + " ";
        ASPxListBox1.DataSource = OracleHelper.Query(oracletext);
        ASPxListBox1.DataBind();
        ASPxGridView2.Visible = true;
    }

    protected void ASPxCallbackPanel2_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
    {
        string key = e.Parameter.Trim();
        if (key == "k")
        {
            Session["gzrwID"] = -1;
            ObjectDataSource1.SelectParameters["strWhere"].DefaultValue = "";
            ASPxGridView2.Visible = false;
        }
        else
        {
            Session["gzrwID"] = key;
            ObjectDataSource1.SelectParameters["strWhere"].DefaultValue = " and WORKTASKID = " + key + "";
            ASPxGridView2.Visible = true;
        }

    }
    protected void ASPxGridView2_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomButtonCallbackEventArgs e)
    {
        bool move = e.ButtonID == "sy" ? true : false;
        int PROCESSID = Convert.ToInt32(ASPxGridView2.GetRowValues(e.VisibleIndex, "PROCESSID"));
        
        GhtnTech.SEP.OraclDAL.DALPROCESS pro = new GhtnTech.SEP.OraclDAL.DALPROCESS();
        DataTable dt = pro.GetDALPROCESS(" and PROCESSID=" + PROCESSID.ToString()).Tables[0];
        int sort = Convert.ToInt32(dt.Rows[0]["SERIALNUMBER"]);
        int WORKTASKID = Convert.ToInt32(dt.Rows[0]["WORKTASKID"]);
        try
        {
            //修改后-删除某记录后也可进行排序
            DataSet ds = OracleHelper.Query("select SERIALNUMBER from (select SERIALNUMBER from PROCESS where WORKTASKID='" + WORKTASKID + "' order by SERIALNUMBER desc nulls last) where ROWNUM = 1");
            for (int i = 1; i <= int.Parse(ds.Tables[0].Rows[0]["SERIALNUMBER"].ToString()); i++)
            {
                int otherID = -1;
                try
                {
                    DataSet dsP=pro.GetDALPROCESS(" and WORKTASKID = " + WORKTASKID.ToString() + " and SERIALNUMBER = " + Convert.ToString(move ? sort - i : sort + i));
                    otherID = Convert.ToInt32(dsP.Tables[0].Rows[0]["PROCESSID"].ToString());
                }
                catch
                {
                    continue;
                }
                pro.UpdateDALPROCESS_SERIALNUMBER(PROCESSID, move ? sort - i : sort + i);
                pro.UpdateDALPROCESS_SERIALNUMBER(otherID, sort);
                break;
            }
            ObjectDataSource1.SelectParameters["strWhere"].DefaultValue = "and WORKTASKID = " + WORKTASKID + "";
            ASPxGridView2.DataSourceID = "ObjectDataSource1";
            ASPxGridView2.DataBind();

            //连号方可排序
            //int otherID = Convert.ToInt32(pro.GetDALPROCESS(" and WORKTASKID=" + WORKTASKID.ToString() + " and SERIALNUMBER=" + Convert.ToString(move ? sort - 1 : sort + 1)).Tables[0].Rows[0]["PROCESSID"]);
            //pro.UpdateDALPROCESS_SERIALNUMBER(PROCESSID, move ? sort - 1 : sort + 1);
            //pro.UpdateDALPROCESS_SERIALNUMBER(otherID, sort);
           
            //ObjectDataSource1.SelectParameters["strWhere"].DefaultValue = "and WORKTASKID = " + WORKTASKID + "";
            //ASPxGridView2.DataSourceID = "ObjectDataSource1";
            //ASPxGridView2.DataBind();
        }
        catch
        {
            string msg = move ? "当前是第一行，无法上移" : "当前是最后一行，无法下移";
            throw new Exception(msg);
        }
    }
}
