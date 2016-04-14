using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;
using System.Xml.Linq;
using DevExpress.Web.ASPxTreeList;
using GhtnTech.SecurityFramework.BLL;


public partial class Search_Sep_search : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //登录用户判断显示不同信息

            //string dept = SessionBox.GetUserSession().DeptNumber;
            //if (dept.Substring(5, 5) == "00000")
            //{
            //    cbbFW.Text = "矿级";
            //    ObjectDataSource1.SelectParameters["strWhere"].DefaultValue = " and hazards_ore.DEPTNUMBER='" + SessionBox.GetUserSession().DeptNumber + "'";
            //    ASPxGridView1.DataSourceID = "ObjectDataSource1";
            //    ObjectDataSource1.DataBind();
            //}
            //工作任务
            cbbGZway.SelectedIndex = 1;
            //工序
            cbbGXway.SelectedIndex = 1;
            //时间
            deBegin.Date = System.DateTime.Today;
            deEnd.Date = System.DateTime.Today;
        }

        pagebind();
        
        if (IsCallback)
        {
            
        }
    }

    #region  数据初始化
    private void pagebind()
    {
        //绑定专业
        string ID = PublicMethod.ReadXmlReturnNode("ZY", this);
        lblZY.Text = GhtnTech.SEP.OraclDAL.DALCS_BaseGet.GetBAseSetName(ID) + "：";
        ObjectDataSource2.SelectParameters["ID"].DefaultValue = ID ;
        ObjectDataSource2.SelectParameters["strWhere"].DefaultValue = " and INFOID!='" + ID + "'";
        DevExpress.Web.ASPxTreeList.ASPxTreeList tl = cbbZY.FindControl("ASPxTreeList1") as DevExpress.Web.ASPxTreeList.ASPxTreeList;
        tl.DataSourceID = "ObjectDataSource2";
        tl.DataBind();
        //绑定单位
        ObjectDataSource3.SelectParameters["DEPTNUMBER"].DefaultValue = SessionBox.GetUserSession().DeptNumber;
        cbbDW.DataSourceID = "ObjectDataSource3";
        cbbDW.DataBind();
        //风险等级
        ID = PublicMethod.ReadXmlReturnNode("FXDJ", this);
        lblFXlevel.Text = GhtnTech.SEP.OraclDAL.DALCS_BaseGet.GetBAseSetName(ID) + "：";
        ObjectDataSource5.SelectParameters["ID"].DefaultValue = ID;
        ObjectDataSource5.SelectParameters["strWhere"].DefaultValue = " and INFOID!='" + ID + "'";
        cbbFXlevel.DataSourceID = "ObjectDataSource5";
        cbbFXlevel.DataBind();
        //风险类型
        ID = PublicMethod.ReadXmlReturnNode("FXLX", this);
        lblSG.Text = GhtnTech.SEP.OraclDAL.DALCS_BaseGet.GetBAseSetName(ID) + "：";
        ObjectDataSource4.SelectParameters["ID"].DefaultValue = ID;
        ObjectDataSource4.SelectParameters["strWhere"].DefaultValue = " and INFOID!='" + ID + "'";
        cbbFXkind.DataSourceID = "ObjectDataSource4";
        cbbFXkind.DataBind();
        //事故类型
        ID = PublicMethod.ReadXmlReturnNode("SGLX", this);
        lblSG.Text = GhtnTech.SEP.OraclDAL.DALCS_BaseGet.GetBAseSetName(ID) + "：";
        ObjectDataSource6.SelectParameters["ID"].DefaultValue = ID;
        ObjectDataSource6.SelectParameters["strWhere"].DefaultValue = " and INFOID!='" + ID + "'";
        cbbSG.DataSourceID = "ObjectDataSource6";
        cbbSG.DataBind();
        
    }
    #endregion

    protected void ASPxCallbackPanel1_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
    {
        if (e.Parameter.Trim() == "quick")
        {
            if (tbQuick.Text.Trim() != "")
            {
                ObjectDataSource1.SelectParameters["strWhere"].DefaultValue = " and H_CONTENT like '%" + tbQuick.Text.Trim() + "%'";
            }
            else
            {
                ObjectDataSource1.SelectParameters["strWhere"].DefaultValue = " and 1=1";
            }
            ASPxGridView1.DataSourceID = "ObjectDataSource1";
            ObjectDataSource1.DataBind();
        }
        if (e.Parameter.Trim() == "base")
        {
            string strwhere = "";
            if (deBegin.Date <= deEnd.Date)
            {
                strwhere += " and to_char(HAZARDS.OPENINGTIME,'yyyy-mm-dd') between '" + deBegin.Date.ToString("yyyy-MM-dd") + "' and '" + deEnd.Date.ToString("yyyy-MM-dd") + "'";
            }
            if (cbbZY.Text.Trim() != "")
            {
                strwhere += " and a.infoid=" + ASPxHiddenField1.Get("ZYkey").ToString();
            }
            if (cbbDW.SelectedIndex > -1)
            {
                //strwhere+=" and "
            }
            if (cbbFXkind.SelectedIndex > -1)
            {
                strwhere += " and HAZARDS.Risk_Typesnumber=" + cbbFXkind.SelectedItem.Value.ToString();
            }
            if (cbbFXlevel.SelectedIndex > -1)
            {
                strwhere += " and c.infoid=" + cbbFXlevel.SelectedItem.Value.ToString();
            }
            if (cbbSG.SelectedIndex > -1)
            {
                strwhere += " and HAZARDS.Accident_Typenumber=" + cbbSG.SelectedItem.Value.ToString();
            }
            if (tbGZ.Text.Trim() != "")
            {
                strwhere += " and b.WORKTASK" + (cbbGZway.SelectedIndex == 0 ? "='" : " like '%") + tbGZ.Text.Trim() + (cbbGZway.SelectedIndex == 0 ? "'" : "%'");
            }
            if (tbGX.Text.Trim() != "")
            {
                strwhere += " and PROCESS.NAME" + (cbbGXway.SelectedIndex == 0 ? "='" : " like '%") + tbGX.Text.Trim() + (cbbGXway.SelectedIndex == 0 ? "'" : "%'");
            }
            if (tbBM.Text.Trim() != "" && tbBMchild.Text.Trim() != "")
            {
                strwhere += " and (HAZARDS.H_NUMBER like '%" + tbBM.Text.Trim() + "%'" + (cbbBMchild.SelectedIndex == 0 ? " and" : " or") + " HAZARDS.H_NUMBER like '%" + tbBMchild.Text.Trim() + "%')";
            }
            if (tbBM.Text.Trim() != "" && tbBMchild.Text.Trim() == "")
            {
                strwhere += " and HAZARDS.H_NUMBER like '%" + tbBM.Text.Trim() + "%'";
            }
            if (tbBM.Text.Trim() == "" && tbBMchild.Text.Trim() != "")
            {
                strwhere += " and HAZARDS.H_NUMBER like '%" + tbBMchild.Text.Trim() + "%'";
            }

            if (tbWXY.Text.Trim() != "" && tbWXYchild.Text.Trim() != "")
            {
                strwhere += " and (H_CONTENT like '%" + tbWXY.Text.Trim() + "%'" + (cbbWXYchild.SelectedIndex == 0 ? " and" : " or") + " H_CONTENT like '%" + tbWXYchild.Text.Trim() + "%')";
            }
            if (tbWXY.Text.Trim() != "" && tbWXYchild.Text.Trim() == "")
            {
                strwhere += " and H_CONTENT like '%" + tbWXY.Text.Trim() + "%'";
            }
            if (tbWXY.Text.Trim() == "" && tbWXYchild.Text.Trim() != "")
            {
                strwhere += " and H_CONTENT like '%" + tbWXYchild.Text.Trim() + "%'";
            }

            if (tbHG.Text.Trim() != "" && tbHGchild.Text.Trim() != "")
            {
                strwhere += " and (H_CONSEQUENCES like '%" + tbHG.Text.Trim() + "%'" + (cbbHGchild.SelectedIndex == 0 ? " and" : " or") + " H_CONSEQUENCES like '%" + tbHGchild.Text.Trim() + "%')";
            }
            if (tbHG.Text.Trim() != "" && tbHGchild.Text.Trim() == "")
            {
                strwhere += " and H_CONSEQUENCES like '%" + tbHG.Text.Trim() + "%'";
            }
            if (tbHG.Text.Trim() == "" && tbHGchild.Text.Trim() != "")
            {
                strwhere += " and H_CONSEQUENCES like '%" + tbHGchild.Text.Trim() + "%'";
            }

            ObjectDataSource1.SelectParameters["strWhere"].DefaultValue = strwhere;
            ASPxGridView1.DataSourceID = "ObjectDataSource1";
            ObjectDataSource1.DataBind();
        }
        if (e.Parameter.Trim() == "cs")
        {
            ObjectDataSource1.SelectParameters["strWhere"].DefaultValue = " and M_STANDARDS like '%" + tbCSsearch.Text.Trim() + "%' or M_MEASURES like '%" + tbCSsearch.Text.Trim() + "%'";
            ASPxGridView1.DataSourceID = "ObjectDataSource1";
            ObjectDataSource1.DataBind();
        }
    }

    protected void treeList_CustomDataCallback(object sender, TreeListCustomDataCallbackEventArgs e)
    {
        var treelist = sender as DevExpress.Web.ASPxTreeList.ASPxTreeList;
        string key = e.Argument.ToString();
        TreeListNode node = treelist.FindNodeByKeyValue(key);
        e.Result = node["INFONAME"].ToString();
    }

    protected void cpPopup_Callback(object source, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
    {
        ASPxHiddenField1.Set("ID", e.Parameter);
        litText.Text = GetNotes(e.Parameter);
    }

    private string GetNotes(string ID)
    {
        GhtnTech.SEP.OraclDAL.DALHAZARDSall da = new GhtnTech.SEP.OraclDAL.DALHAZARDSall();
        DataTable dt = da.GetDALHAZARDSall(" and HAZARDSID=" + ID, "").Tables[0];

        #region table画数据排列
        string note = "<table width=\"99%\" cellspacing=\"0\" cellpadding=\"0\">";

        note += "<tr>";
        note += string.Format("<td class=\"lbr3 lpa title\" width=\"17%\">危险源编码:</td><td class=\"lbr3 lpa \" colspan=\"3\">{0}</td>", dt.Rows[0]["H_NUMBER"].ToString());
        note += "</tr>";

        note += "<tr>";
        note += string.Format("<td class=\"lbr3 lpa title\" width=\"17%\">辨识单元:</td><td class=\"lbr3 lpa \" width=\"30%\">{0}</td>", dt.Rows[0]["ZYNAME"].ToString());
        note += string.Format("<td class=\"lbr3 lpa title\" width=\"13%\">工作任务:</td><td class=\"lbr3 lpa \" width=\"40%\">{0}</td>", dt.Rows[0]["GZRWNAME"].ToString());
        note += "</tr>";

        note += "<tr>";
        note += string.Format("<td class=\"lbr3 lpa title\" width=\"17%\">工序:</td><td class=\"lbr3 lpa \" width=\"30%\">{0}</td>", dt.Rows[0]["GXNAME"].ToString());
        note += string.Format("<td class=\"lbr3 lpa title\" width=\"13%\">危险源:</td><td class=\"lbr3 lpa \" width=\"40%\">{0}</td>", dt.Rows[0]["H_CONTENT"].ToString());
        note += "</tr>";

        note += "<tr>";
        note += string.Format("<td class=\"lbr3 lpa title\" width=\"17%\">风险后果及描述:</td><td class=\"lbr3 lpa \" colspan=\"3\">{0}</td>", dt.Rows[0]["H_CONSEQUENCES"].ToString());
        note += "</tr>";

        note += "<tr>";
        note += string.Format("<td class=\"lbr3 lpa title\" width=\"17%\">风险等级:</td><td class=\"lbr3 lpa \" width=\"35%\">{0}</td>", dt.Rows[0]["FCLEVEL"].ToString());
        note += string.Format("<td class=\"lbr3 lpa title\" width=\"13%\">风险类型:</td><td class=\"lbr3 lpa \" width=\"35%\">{0}</td>", dt.Rows[0]["FXLX"].ToString());
        note += "</tr>";

        note += "<tr>";
        note += string.Format("<td class=\"lbr3 lpa title\" width=\"17%\">事故类型：</td><td class=\"lbr3 lpa \" width=\"35%\">{0}</td>", dt.Rows[0]["SGLX"].ToString());
        note += string.Format("<td class=\"lbr3 lpa title\" width=\"13%\">直接责任人:</td><td class=\"lbr3 lpa \" width=\"35%\">{0}</td>", dt.Rows[0]["ZJZZR"].ToString());
        note += "</tr>";

        note += "<tr>";
        note += string.Format("<td class=\"lbr3 lpa title\" width=\"17%\">管理对象:</td><td class=\"lbr3 lpa \" width=\"35%\">{0}</td>", dt.Rows[0]["GLDXNAME"].ToString());
        note += string.Format("<td class=\"lbr3 lpa title\" width=\"13%\">管理人员:</td><td class=\"lbr3 lpa \" width=\"35%\">{0}</td>", dt.Rows[0]["GLRN"].ToString());
        note += "</tr>";

        note += "<tr>";
        note += string.Format("<td class=\"lbr3 lpa title\" width=\"13%\">管理标准:</td><td class=\"lbr3 lpa \" colspan=\"3\">{0}</td>", dt.Rows[0]["M_STANDARDS"].ToString());
        note += "</tr>";

        note += "<tr>";
        note += string.Format("<td class=\"lbr3 lpa title\" width=\"17%\">管理措施:</td><td class=\"lbr3 lpa \" colspan=\"3\">{0}</td>", dt.Rows[0]["M_MEASURES"].ToString());
        note += "</tr>";

        note += "<tr>";
        note += string.Format("<td class=\"lbr3 lpa title\" width=\"13%\">监管人员:</td><td class=\"lbr3 lpa \" width=\"35%\">{0}</td>", dt.Rows[0]["JGZZR"].ToString());
        note += string.Format("<td class=\"lbr3 lpa title\" width=\"17%\">监管措施:</td><td class=\"lbr3 lpa \" width=\"35%\">{0}</td>", dt.Rows[0]["REGULATORYMEASURES"].ToString());
        note += "</tr>";

        note += "<tr>";
        note += string.Format("<td class=\"lbr3 lpa title\" width=\"13%\">处罚标准:</td><td class=\"lbr3 lpa \" width=\"35%\">{0}</td>", dt.Rows[0]["PUNISHMENTSTANDARD"].ToString());
        note += string.Format("<td class=\"lbr3 lpa title\" width=\"17%\">处理方式:</td><td class=\"lbr3 lpa \" width=\"35%\">{0}</td>", dt.Rows[0]["PROCESSINGMODE"].ToString());
        note += "</tr>";

        note += "<tr>";
        note += string.Format("<td class=\"lbr3 lpa title\" width=\"13%\">别名:</td><td class=\"lbr3 lpa \" colspan=\"3\">{0}</td>", dt.Rows[0]["H_BM"].ToString());
        note += "</tr>";

        note += "<tr>";
        note += string.Format("<td class=\"lbr3 lpa title\" width=\"13%\">备注:</td><td class=\"lbr3 lpa \" width=\"35%\">{0}</td>", dt.Rows[0]["NOTE"].ToString());
        note += string.Format("<td class=\"lbr3 lpa title\" width=\"17%\">当前状态:</td><td class=\"lbr3 lpa \" width=\"35%\"><font color=\"{1}\">{0}</font></td>", dt.Rows[0]["ISPASS"].ToString(), "Red");
        note += "</tr>";

        note += "</table>";
        #endregion

        return note;
    }
    protected void ASPxGridView1_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableDataCellEventArgs e)
    {
        if (e.DataColumn.Caption == "状态")
        {
            if (e.CellValue.ToString().Trim() == "通用")
            {
                e.Cell.ForeColor = System.Drawing.Color.Red;
            }
            if (e.CellValue.ToString().Trim() == "引用")
            {
                e.Cell.ForeColor = System.Drawing.Color.DarkOrange;
            }
        }
    }
}
