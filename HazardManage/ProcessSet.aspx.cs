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
using GhtnTech.SecurityFramework;
using GhtnTech.SecurityFramework.BLL;
using GhtnTech.SecurityFramework.Utility;
using DevExpress.Web.ASPxGridView;
using GhtnTech.SEP.DBUtility;
public partial class HazardManage_ProcessSet : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        bind();
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
                    GridViewCommandColumn colEdit = (GridViewCommandColumn)ASPxGridView2.Columns["操作"];
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
                    if (!UserHandle.ValidationHandle(PermissionTag.Move))
                    {
                        colEdit.CustomButtons["sy"].Visibility = GridViewCustomButtonVisibility.Invisible;
                        colEdit.CustomButtons["xy"].Visibility = GridViewCustomButtonVisibility.Invisible;
                    }
                    
                }
            }

        }
    }

    //绑定专业
    private void bind()
    {
        int zyID = int.Parse(PublicMethod.ReadXmlReturnNode("ZY", this));
        //string oracletext = "select * from CS_BASEINFOSET where INFOID!= " + zyID + " start with INFOID= " + zyID + "  connect by prior INFOID = FID order by INFOID asc";
        string oracletext = "select * from CS_BASEINFOSET start with INFOID= " + zyID + "  connect by prior INFOID = FID order by INFOID asc";
        treeList.DataSource = OracleHelper.Query(oracletext);
        treeList.DataBind();
    }
    protected void ASPxCallbackPanel1_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
    {
        string key = e.Parameter.Trim();
        Session["zyID"] = key;
        TreeListNode node = treeList.FindNodeByKeyValue(key);
        if (node.ChildNodes.Count>0)
        {
            return;
        }
        //if (node.HasChildren)
        //{
        //    return;
        //}
        int gzrwID = int.Parse(key);

        string oracletext = "select * FROM WORKTASKS where PROFESSIONALID = "+key+" ";

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
