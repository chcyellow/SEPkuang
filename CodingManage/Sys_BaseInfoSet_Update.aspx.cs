using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.Data;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxTreeList;
using GhtnTech.SEP.DBUtility;
using System.Data.OracleClient;
using DevExpress.Web.ASPxClasses;
using GhtnTech.SecurityFramework;
using GhtnTech.SecurityFramework.Model;
using GhtnTech.SecurityFramework.Utility;
using GhtnTech.SecurityFramework.BLL;
public partial class CodingManage_Sys_BaseInfoSet_Update : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!SessionBox.CheckUserSession())
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                //初始化模块权限
                UserHandle.InitModule(this.PageTag);
                //是否有浏览权限
                if (UserHandle.ValidationHandle(PermissionTag.Browse))
                {
                    TreeListCommandColumn colEdit = (TreeListCommandColumn)InfoTree.Columns["操作"];
                    if (!UserHandle.ValidationHandle(PermissionTag.Add))
                    {
                        colEdit.NewButton.Visible = false;
                    }
                    if (!UserHandle.ValidationHandle(PermissionTag.Edit))
                    {
                        colEdit.EditButton.Visible = false;
                    }
                    if (!UserHandle.ValidationHandle(PermissionTag.Delete))
                    {
                        colEdit.DeleteButton.Visible = false;
                    }
                }
            }
        }
        Session["strWhere"] = "";
        lbl_DepartName.Text = SessionBox.GetUserSession().DeptName;//需要加权限调用
        lbl_DepartName.ForeColor = System.Drawing.Color.Red;
    }

    protected void backPanel_Callback(object source, CallbackEventArgsBase e)
    {
        if (e.Parameter == "fabu")
        {
            UpdateStatus();
        }
    }
    private void UpdateStatus()
    {
        try
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update CS_BaseInfoSet set ");
            strSql.Append(" STATUS = '启用'");
            strSql.Append(" where STATUS = '编辑' and PDEPART='" + SessionBox.GetUserSession().DeptName + "'");//需要添加单位判断
            OracleHelper.Query(strSql.ToString());
        }
        catch
        {
            System.Windows.Forms.MessageBox.Show("发布过程出错，请核对信息明细！", "信息提示");
        }
    }

    #region 输入信息判断
    protected void InfoTree_NodeValidating(object sender, TreeListNodeValidationEventArgs e)
    {
        if (e.NewValues["INFOCODE"].ToString().Trim().Length < 1)
        {
            e.Errors["INFOCODE"] = "请输入编码！";
        }
        if (e.NewValues["INFOCODE"].ToString().Trim().Length > 0)
        {
            //GhtnTech.SEP.OraclDAL.DALCS_BaseInfoSet cb = new GhtnTech.SEP.OraclDAL.DALCS_BaseInfoSet();
            try
            {
                //DataSet ds = cb.GetBaseInfoSetList(" and INFOID=" + int.Parse(e.NewValues["FID"].ToString().Trim()));
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * FROM CS_BaseInfoSet");
                strSql.Append(" where INFOID=" + int.Parse(e.NewValues["FID"].ToString().Trim()));
                DataSet ds = OracleHelper.Query(strSql.ToString());
                //int A = e.NewValues["INFOCODE"].ToString().Trim().Length;
                //int b = int.Parse(ds.Tables[0].Rows[0]["CODINGL"].ToString().Trim());

                if (e.NewValues["INFOCODE"].ToString().Trim().Length != int.Parse(ds.Tables[0].Rows[0]["CODINGL"].ToString().Trim()))
                {
                    e.Errors["INFOCODE"] = "编码长度与设置不符合，请重新输入！";
                }
                if (ds.Tables[0].Rows[0]["CODINGTYPE"].ToString().Trim() == "NUMBERIC")
                {
                    try
                    {
                        Convert.ToInt32(e.NewValues["INFOCODE"].ToString().Trim());
                    }
                    catch
                    {
                        e.Errors["INFOCODE"] = "编码类型与设置不符合，请重新输入！";
                    }
                }
                if (ds.Tables[0].Rows[0]["CODINGTYPE"].ToString().Trim() == "STRING")
                {
                    try
                    {
                        Convert.ToInt32(e.NewValues["INFOCODE"].ToString().Trim());
                        e.Errors["INFOCODE"] = "编码类型与设置不符合，请重新输入！";
                    }
                    catch
                    {
                    }
                }

                StringBuilder strsSql = new StringBuilder();
                strsSql.Append("select * FROM CS_BaseInfoSet");
                strsSql.Append(" where FID=" + int.Parse(e.NewValues["FID"].ToString().Trim()));
                DataSet dsQC = OracleHelper.Query(strsSql.ToString());
                for (int i = 0; i < dsQC.Tables[0].Rows.Count; i++)
                {
                    if (e.NewValues["INFOCODE"].ToString().Trim() == dsQC.Tables[0].Rows[i]["INFOCODE"].ToString().Trim())
                    {
                        e.Errors["INFOCODE"] = "相同根节点下已经存在相同的编码，请重新输入！";
                    }
                }
            }
            catch
            {
            }
        }

        if (!IsStringValueNotEmpty(e.NewValues["INFONAME"]))
            e.Errors["INFONAME"] = "请输入信息！";

        if (!IsStringValueNotEmpty(e.NewValues["CODINGTYPE"]))
            e.Errors["CODINGTYPE"] = "请输入信息！";

        if (!IsStringValueNotEmpty(e.NewValues["CODINGL"]))
            e.Errors["CODINGL"] = "请输入信息！";

        if (!IsDateValid(e.NewValues["PDAY"]))
            e.Errors["PDAY"] = "请选择当天或当天之前的日期信息！";
    }
    bool IsStringValueNotEmpty(object value)
    {
        return value != null && value.ToString().Trim().Length > 0;
    }
    bool IsDateValid(object value)
    {
        if (value is DateTime)
            return ((DateTime)value) <= System.DateTime.Today;
        return false;
    }

    //private static string ReInt = @"^[+-]?(?:\d+)";
    //private static string ReUInt = @"^[+]?(?:\d+)";
    //private static string ReDouble = @"^[+-]?(?:\d+(\.\d*)?|\d*\.\d+?)";
    //private static string ReDate = @"^(?:(?!0000)[0-9]{4}([-/.]?)(?:(?:0?[1-9]|1[0-2])\1(?:0?[1-9]|1[0-9]|2[0-8])|(?:0?[13-9]|1[0-2])\1(?:29|30)|(?:0?[13578]|1[02])\1(?:31))|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)([-/.]?)0?2\2(?:29))";
    //private static string ReTime = @"^([0-9]|[0-1][0-9]|[2][0-3]):([0-9]|[0-5][0-9])(:([0-9]|[0-5][0-9]))?";
    //private static string EndFlag = @"\s*$";
    //public static bool RegexMatch(string input, string pattern)
    //{
    //    if (input == null || input.Trim().Length == 0)
    //        return false;
    //    return System.Text.RegularExpressions.Regex.IsMatch(input, pattern);
    //}

    //public static bool IsDouble(string str)
    //{
    //    return RegexMatch(str, ReDouble + EndFlag);
    //}

    //public static bool IsInt(string str)
    //{
    //    return RegexMatch(str, ReInt + EndFlag);
    //}

    //public static bool IsUInt(string str)
    //{
    //    return RegexMatch(str, ReUInt + EndFlag);
    //}

    #endregion

    #region 状态颜色显示
    protected void InfoTree_HtmlDataCellPrepared(object sender, TreeListHtmlDataCellEventArgs e)
    {
        if (e.Column.Caption == "状态")
        {
            string value = e.CellValue.ToString().Trim();
            if (value == "编辑")
                e.Cell.ForeColor = Color.Blue;
            if (value == "启用")
                e.Cell.ForeColor = Color.Red;
            if (value == "禁用")
                e.Cell.ForeColor = Color.Cornsilk;
        }
    }
    #endregion
}
