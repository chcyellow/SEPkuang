using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DevExpress.Web.ASPxTreeList;
using DevExpress.Web.ASPxEditors;
using System.Data;
using GhtnTech.SEP.DBUtility;
using System.Data.OracleClient;
using System.Text;
using GhtnTech.SecurityFramework.BLL;


public partial class HazardManage_Quote : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        GhtnTech.SEP.OraclDAL.DALUnionTree ut = new GhtnTech.SEP.OraclDAL.DALUnionTree();
        treeList.DataSource = ut.GetUnionTreeBase(PublicMethod.ReadXmlReturnNode("ZY", this));
        treeList.DataBind();
        if (!IsPostBack)
        {
            agvDATA.GroupBy(agvDATA.Columns["ISGROUP"]);
            agvDATA.EndUpdate();
            agvDATA.ExpandAll();

            //Bindcombox(int.Parse(PublicMethod.ReadXmlReturnNode("PL", this)), ASPxComboBox15);
            //Bindcombox(int.Parse(PublicMethod.ReadXmlReturnNode("SS", this)), ASPxComboBox16);
            Bindcombox(int.Parse(PublicMethod.ReadXmlReturnNode("SGLX", this)), ASPxComboBox7);
            Bindcombox(int.Parse(PublicMethod.ReadXmlReturnNode("FXDJ", this)), ASPxComboBox9);

            bindFXLX();

            bindGLDX(8, ASPxComboBox11);
            bindGLDX(8, ASPxComboBox12);
            bindGLDX(8, ASPxComboBox13);

            bindGLDX(8, ASPxComboBox10);


            btnwork.Enabled = false;

            KEnableSet();
        }
    }

    private void baseSet(string ID)
    {
        string oracletext = "select * from HAZARDS where HAZARDSID = " + ID + " ";
        DataTable dt = OracleHelper.Query(oracletext).Tables[0];
        ASPxMemo3.Text = dt.Rows[0]["M_STANDARDS"].ToString();//
        ASPxMemo4.Text = dt.Rows[0]["M_MEASURES"].ToString();//
        ASPxMemo5.Text = dt.Rows[0]["REGULATORYMEASURES"].ToString();//
        ASPxMemo6.Text = dt.Rows[0]["H_CONTENT"].ToString();//
        ASPxMemo7.Text = dt.Rows[0]["H_CONSEQUENCES"].ToString();//

        ASPxComboBox7.Value = dt.Rows[0]["ACCIDENT_TYPENUMBER"].ToString();//
        ASPxComboBox9.Value = dt.Rows[0]["RISK_EVELNUMBER"].ToString();//
        ASPxComboBox8.Value = dt.Rows[0]["RISK_TYPESNUMBER"].ToString();//
        ASPxComboBox10.Value = dt.Rows[0]["M_OBJECTNUMBER"].ToString();//
        ASPxComboBox11.Value = dt.Rows[0]["M_PERSONNUMBER"].ToString();//
        ASPxComboBox12.Value = dt.Rows[0]["REGULATORYPARTNERSNUMBER"].ToString();//
        ASPxComboBox13.Value = dt.Rows[0]["DIRECTLYRESPONSIBLEPERSONSNUMB"].ToString();//
    }

    private void EnableSet()
    {
        foreach (Control c in basetable.Controls)
        {
            foreach (Control c1 in c.Controls)
            {
                foreach (Control c2 in c1.Controls)
                {
                    string name = c2.GetType().Name;
                    if (name == "ASPxMemo")
                    {
                        ASPxMemo am = (ASPxMemo)c2;
                        am.Enabled = false;
                    }
                    if (name == "ASPxComboBox")
                    {
                        ASPxComboBox am = (ASPxComboBox)c2;
                        am.Enabled = false;
                    }
                }
            }
        }
        ASPxComboBox10.Enabled = false;
    }

    private void Bindcombox(int fid, ASPxComboBox comx)
    {

        string oracletext = "select * from CS_BASEINFOSET where FID = " + fid + " ";
        comx.DataSource = OracleHelper.Query(oracletext);
        comx.DataBind();


    }

    //绑定风险类型

    private void bindFXLX()
    {
        int zyID = int.Parse(PublicMethod.ReadXmlReturnNode("FXLX", this));
        string oracletext = "select * from CS_BASEINFOSET where INFOID!= " + zyID + " start with INFOID= " + zyID + "  connect by prior INFOID = FID order by INFOID asc";
        ASPxComboBox8.DataSource = OracleHelper.Query(oracletext);
        ASPxComboBox8.DataBind();

    }
    //绑定管理对象
    private void bindGLDX(int id, ASPxComboBox comb)
    {

        string oracletext = "select * from M_OBJECT where RISK_TYPESID = " + id + "";

        comb.DataSource = OracleHelper.Query(oracletext);
        comb.DataBind();


    }

    private void KEnableSet()
    {
        ASPxMemo3.Enabled = false;
        ASPxMemo4.Enabled = false;
        ASPxMemo5.Enabled = false;
        ASPxMemo6.Enabled = false;
        ASPxMemo7.Enabled = false;
        ASPxComboBox7.Enabled = false;
        ASPxComboBox8.Enabled = false;
        //ASPxComboBox15.Enabled = false;
        //ASPxComboBox16.Enabled = false;
    }

    protected void acpGridview_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
    {
        TreeListNode node = treeList.FindNodeByKeyValue(e.Parameter.Trim());
        if (node.HasChildren)
        {
            return;
        }

        if (e.Parameter.Trim().Substring(0, 1).ToLower() == "w" )//标准工作任务
        {
            //工作任务
            return;
        }
        if (e.Parameter.Trim() == "uptate")
        {
            yy();
        }

        ObjectDataSource1.SelectParameters["strWhere"].DefaultValue = " and HAZARDS.PROCESSID='" + e.Parameter.Trim().Substring(1) + "'";
        ObjectDataSource1.SelectParameters["DeptNumber"].DefaultValue = "000000000";
        agvDATA.DataSourceID = "ObjectDataSource1";
        ObjectDataSource1.DataBind();

        agvDATA.ExpandAll();
    }

    private void yy()
    {
        string oracletext = "select * from HAZARDS where HAZARDSID = " + agvDATA.GetSelectedFieldValues("HAZARDSID")[0].ToString().Trim() + " ";
        DataTable dt = OracleHelper.Query(oracletext).Tables[0];
        //附属表信息

        StringBuilder strSql1 = new StringBuilder();
        strSql1.Append("insert into HAZARDS_ORE(");
        strSql1.Append("RISK_EVELNUMBER,M_OBJECTNUMBER,M_PERSONNUMBER,DIRECTLYRESPONSIBLEPERSONSNUMB,REGULATORYPARTNERSNUMBER,DATEINPUT,PERSONID,ISPASS,DEPTNUMBER,H_NUMBER)");
        strSql1.Append(" values(");
        strSql1.Append(":RISK_EVELNUMBER,:M_OBJECTNUMBER,:M_PERSONNUMBER,:DIRECTLYRESPONSIBLEPERSONSNUMB,:REGULATORYPARTNERSNUMBER,:DATEINPUT,:PERSONID,:ISPASS,:DEPTNUMBER,:H_NUMBER)");
        OracleParameter[] parameters1 = {
                    new OracleParameter(":RISK_EVELNUMBER",OracleType.Number,9),//风险等级
                    new OracleParameter(":M_OBJECTNUMBER", OracleType.Number,9),//管理对象
                    new OracleParameter(":M_PERSONNUMBER", OracleType.Number,9),//管理人员
                    new OracleParameter(":DIRECTLYRESPONSIBLEPERSONSNUMB",OracleType.Number,9),//直接责任人

					new OracleParameter(":REGULATORYPARTNERSNUMBER", OracleType.Number,9),//监管责任人
                    //new OracleParameter(":FREQUENCYNUMBER",OracleType.Number,9),//频率

                    //new OracleParameter(":LOSSNUMBER", OracleType.Number,9),//损失
					new OracleParameter(":DATEINPUT", OracleType.DateTime),//录入时间
                    //new OracleParameter(":PERSONID", OracleType.Number,9),//录入人
                    new OracleParameter(":PERSONID", OracleType.VarChar,20),//录入人
                    new OracleParameter(":ISPASS",OracleType.VarChar,10),//状态

                    new OracleParameter(":DEPTNUMBER", OracleType.VarChar,30),//引用单位

                    new OracleParameter(":H_NUMBER",OracleType.VarChar,100)//编码
                    };

        parameters1[0].Value = int.Parse(ASPxComboBox9.SelectedItem.Value.ToString());
        try
        {
            parameters1[1].Value = int.Parse(ASPxComboBox10.SelectedItem.Value.ToString());
        }
        catch { }
        parameters1[2].Value = int.Parse(ASPxComboBox11.SelectedItem.Value.ToString());
        parameters1[3].Value = int.Parse(ASPxComboBox13.SelectedItem.Value.ToString());

        parameters1[4].Value = int.Parse(ASPxComboBox12.SelectedItem.Value.ToString());
        //parameters1[5].Value = int.Parse(ASPxComboBox15.SelectedItem.Value.ToString());

        //parameters1[6].Value = int.Parse(ASPxComboBox16.SelectedItem.Value.ToString());
        parameters1[5].Value = DateTime.Now;
        try
        {
            oracletext = "select PERSONID from PERSON where PERSONNUMBER = " + SessionBox.GetUserSession().PersonNumber + " ";
            parameters1[6].Value = OracleHelper.GetSingle(oracletext).ToString();
        }
        catch
        {
            parameters1[6].Value = 434;
        }
        parameters1[7].Value = "引用";

        try
        {
            parameters1[8].Value = SessionBox.GetUserSession().DeptNumber;
        }
        catch
        {
            parameters1[8].Value = "000000000";
        }
        parameters1[9].Value = dt.Rows[0]["H_NUMBER"].ToString().Trim();

        OracleHelper.ExecuteSql(strSql1.ToString(), parameters1);
    }

    protected void agvDATA_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableDataCellEventArgs e)
    {
        if (e.DataColumn.Caption.Trim() == "状态")
        {
            if (e.CellValue.ToString().Trim() == "已发布")
            {
                e.Cell.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                e.Cell.ForeColor = System.Drawing.Color.Green;
            }
        }
    }
    protected void acpAction_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
    {
        if (e.Parameter.Trim() == "Z")
        {
            if (agvDATA.Selection.Count > 0)
            {
                btnwork.Enabled = true;
            }
            else
            {
                btnwork.Enabled = false;
            }
            return;
        }
        TreeListNode node = treeList.FindNodeByKeyValue(e.Parameter.Trim());
        if (node.HasChildren)
        {
            btnwork.Enabled = false;
            return;
        }

        if (e.Parameter.Trim().Substring(0, 1).ToLower() == "w")//标准工作任务
        {
            btnwork.Enabled = true;
            return;
        }
        else
        {
            btnwork.Enabled = false;
        }
    }
    protected void acpInfo_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
    {
        if (e.Parameter.Trim() == "Z")//引用
        {
            if (agvDATA.Selection.Count > 0)
            {
                //ASPxHiddenField1.Set("key", agvDATA.GetSelectedFieldValues("HAZARDSID")[0].ToString().Trim());
                baseSet(agvDATA.GetSelectedFieldValues("HAZARDSID")[0].ToString().Trim());
            }
        }
    }
}
