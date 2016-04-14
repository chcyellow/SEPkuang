using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GhtnTech.SecurityFramework;
using GhtnTech.SecurityFramework.BLL;
using GhtnTech.SecurityFramework.Model;
using GhtnTech.SecurityFramework.Utility;
using GhtnTech.SEP.DAL;
public partial class _Default : System.Web.UI.Page 
{
    PersonBll p = new PersonBll();
    
    DepartmentBll dept = new DepartmentBll();
    DBSCMDataContext dc = new DBSCMDataContext();
    static readonly int _pageSize = 10;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //BindGridView(AspNetPager1.StartRecordIndex, "person", _pageSize, "", "", "");
            //AspNetPager1.RecordCount = p.PersonCount();
            ////BindDll(ddlDept, "ID", @"24\d\d0{5}", "NAME", "ID");
            //if (!SessionBox.CheckUserSession())
            //{
            //    Response.Redirect("~/Login.aspx");
            //}
            //else
            //{
            //    InitData();
            //}
            InitData();
        }
        
    }
    private void BindGridView(int index, string table,int pageSize,string where, string column,string order)
    {
        DataTable dt = p.GetList(index, table, pageSize, where,column,order).Tables[0];
        int dtcount = dt.Rows.Count;
        gvPerson.DataSource = dt;
        gvPerson.DataBind();
        AspNetPager1.RecordCount = p.PersonCount();
        AspNetPager1.PageSize = pageSize;
        AspNetPager1.CustomInfoHTML = "记录总数：<font color=\"blue\"><b>" + AspNetPager1.RecordCount.ToString() + "</b></font>";
        AspNetPager1.CustomInfoHTML += " 总页数：<font color=\"blue\"><b>" + AspNetPager1.PageCount.ToString() + "</b></font>";
        AspNetPager1.CustomInfoHTML += " 当前页：<font color=\"red\"><b>" + AspNetPager1.CurrentPageIndex.ToString() + "</b></font>";
    }
    private void GetPersonData()
    {
        int index = 0;
        string table = "PERSON";
        int pageSize = _pageSize;
        string where = "1=1";
        string column = "PERSONID";
        string order = "ASC";
        if (AspNetPager1.CurrentPageIndex > 0)
        {
            index = AspNetPager1.CurrentPageIndex - 1;
        }
        if (txtPsnNo.Text.Trim() != "")
        {
            where += string.Format(" and PERSONNUMBER like'%{0}%'", txtPsnNo.Text.Trim());
        }
        if (txtLightNo.Text.Trim() != "")
        {
            where += string.Format(" and LIGHTNUMBER like'%{0}%'", txtLightNo.Text.Trim());
        }
        if (txtName.Text.Trim() != "")
        {
            where += string.Format(" and NAME like'%{0}%'", txtName.Text.Trim());
        }
        if (txtPhone.Text.Trim() != "")
        {
            where += string.Format(" and TEL like'%{0}%'", txtPhone.Text.Trim());
        }
        if (ddlDept.SelectedValue != "-1")
        {
            where += string.Format(" and MAINDEPTID='{0}'", ddlDept.SelectedValue.Trim());
        }
        if (ddlKQ.SelectedValue != "-1")
        {
            where += string.Format(" and DEPTID='{0}'", ddlKQ.SelectedValue.Trim());
        }
        if (ddlPos.SelectedValue != "-1")
        {
            where += string.Format(" and POSID={0}", ddlPos.SelectedValue.Trim());
        }
        BindGridView(index, table, pageSize, where, column, order);
        AspNetPager1.RecordCount = p.PersonCount();
    }

    protected void InitData()
    {
        GetDeptData();
        GetKQData();
        GetPos();
        GetPersonData();
    }
    private void GetPos()
    {
        if (ddlDept.SelectedValue != "-1")
        {
            var data = from pos in dc.Position
                      where pos.Maindeptid == ddlDept.SelectedValue
                      select new
                          {
                              id = pos.Posid,
                              name = pos.Posname
                          };
            ddlPos.DataSource = data;
            ddlPos.DataTextField = "name";
            ddlPos.DataValueField = "id";
            ddlPos.DataBind();
            ddlPos.Items.Insert(0, new ListItem("--全部--", "-1"));
        }
        else
        {
            ddlPos.Items.Clear();
            ddlPos.Items.Insert(0, new ListItem("--全部--", "-1"));
        }
    }
    private void GetDeptData()
    {
        BindDll(ddlDept, "ID", @"24\d\d0{5}", "NAME", "ID");
        ddlDept.Items.Insert(0, new ListItem("--全部--", "-1"));
        if (!SessionBox.GetUserSession().rolelevel.Contains("1"))
        {
            ddlDept.SelectedValue = SessionBox.GetUserSession().DeptNumber;
            ddlDept.Enabled = false;
        }
    }
    /// <summary>
    /// AspNetPager1   PageChanged事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void AspNetPager1_PageChanged(object sender, EventArgs e)
    {

        //BindGridView(AspNetPager1.CurrentPageIndex - 1, "person", _pageSize, "", "", "");
        GetPersonData();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {

        GetPersonData();
    }

    protected void BindDll(DropDownList ddl,string column,string regex,string txtField,string valField)
    {
        ddl.DataSource = dept.GetTreeList(column, regex);
        ddl.DataTextField = txtField;
        ddl.DataValueField = valField;
        ddl.DataBind();
    }
    protected void ddlDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetKQData();
        GetPos();
    }

    private void GetKQData()
    {
        if (ddlDept.SelectedValue != "-1")
        {
            BindDll(ddlKQ, "ID", ddlDept.SelectedItem.Value.Remove(4), "NAME", "ID");
            ddlKQ.Items.Insert(0, new ListItem("--全部--", "-1"));
        }
        else
        {
            ddlKQ.Items.Clear();
            ddlKQ.Items.Insert(0, new ListItem("--全部--", "-1"));
        }
    }
    protected void gvPerson_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvPerson.EditIndex = e.NewEditIndex;
        GetPersonData();
    }
    protected void gvPerson_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        SF_Person sfp = new SF_Person();
        sfp.LIGHTNUMBER = ((TextBox)gvPerson.Rows[e.RowIndex].FindControl("txtLightNo2")).Text.Trim();
        sfp.POSID = int.Parse(((DropDownList)gvPerson.Rows[e.RowIndex].FindControl("ddlPostion")).SelectedValue.Trim());
        sfp.TEL = ((TextBox)gvPerson.Rows[e.RowIndex].FindControl("txtPhone")).Text.Trim();
        if (!p.Update(sfp))
        {
            JSHelper.Alert("更新失败！",this);
        }
        gvPerson.EditIndex = -1;
        GetPersonData();
    }
    protected void gvPerson_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void gvPerson_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)//判定当前的行是否属于datarow类型的行
        {

            //当鼠标放上去的时候 先保存当前行的背景颜色 并给附一颜色
            e.Row.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#ffffcd',this.style.fontWeight='';");
            //当鼠标离开的时候 将背景颜色还原的以前的颜色
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor,this.style.fontWeight='';");

            Label lblPos = ((Label)e.Row.FindControl("lblPos"));
            Label hid_lblPos = ((Label)e.Row.FindControl("hid_lblPos"));
            Label maindept = ((Label)e.Row.FindControl("lblUnit"));
            Label lbldept = ((Label)e.Row.FindControl("lblDept"));
            if (ddlDept.SelectedValue != "-1")
            {
                var data = from pos in dc.Position
                           where pos.Maindeptid == ddlDept.SelectedValue
                           select new
                           {
                               id = pos.Posid,
                               name = pos.Posname
                           };
                foreach (var d in data)
                {
                    if (d.id.ToString() == hid_lblPos.Text)
                    {
                        lblPos.Text = d.name.Trim();
                    }
                }
                var data2 = dept.GetTreeList("ID", @"24\d\d0{5}");
                var data3 = dept.GetTreeList("ID", ddlDept.SelectedItem.Value.Remove(4));
                foreach (var d2 in data2.Tables[0].Select())
                {
                    if (d2["ID"].ToString() == maindept.Text)
                    {
                        maindept.Text = d2["name"].ToString();
                    }
                }
                foreach (var d3 in data3.Tables[0].Select())
                {
                    if (d3["ID"].ToString() == lbldept.Text)
                    {
                        lbldept.Text = d3["name"].ToString();
                    }
                }
            }
            else
            {
                var data = from pos in dc.Position
                           select new
                           {
                               id = pos.Posid,
                               name = pos.Posname
                           };
                foreach (var d in data)
                {
                    if (d.id.ToString() == hid_lblPos.Text)
                    {
                        lblPos.Text = d.name.Trim();
                    }
                }
                var data2 = dept.GetTreeList("ID", @"24\d\d0{5}");
                var data3 = dept.GetTreeList("ID", maindept.Text.Remove(4));
                foreach (var d2 in data2.Tables[0].Select())
                {
                    if (d2["ID"].ToString() == maindept.Text)
                    {
                        maindept.Text = d2["NAME"].ToString();
                    }
                }
                foreach (var d3 in data3.Tables[0].Select())
                {
                    if (d3["ID"].ToString() == lbldept.Text)
                    {
                        lbldept.Text = d3["NAME"].ToString();
                    }
                }
            }
            if ((e.Row.RowState & DataControlRowState.Edit) != 0)
            {
                DropDownList ddlP = (DropDownList)e.Row.FindControl("ddlPostion");
                Label hid_lblPos2 = ((Label)e.Row.FindControl("hid_lblPos"));
                
                if (ddlDept.SelectedValue != "-1")
                {
                    var data = from pos in dc.Position
                               where pos.Maindeptid == ddlDept.SelectedValue
                               select new
                               {
                                   id = pos.Posid,
                                   name = pos.Posname
                               };
                    ddlP.DataSource = data;
                    ddlP.DataTextField = "name";
                    ddlP.DataValueField = "id";
                    ddlP.DataBind();
                    ddlP.SelectedValue = hid_lblPos2.Text;
                }
                else
                {
                    var data = from pos in dc.Position
                               select new
                               {
                                   id = pos.Posid,
                                   name = pos.Posname
                               };
                    ddlP.DataSource = data;
                    ddlP.DataTextField = "name";
                    ddlP.DataValueField = "id";
                    ddlP.DataBind();
                    ddlP.SelectedValue = hid_lblPos2.Text;
                }
            }
        }
    }
    protected void gvPerson_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvPerson.EditIndex = -1;
        GetPersonData();
    }
    protected void btnCancelTxt_Click(object sender, EventArgs e)
    {
        txtLightNo.Text = "";
        txtName.Text = "";
        txtPhone.Text = "";
        txtPsnNo.Text = "";
        ddlPos.SelectedValue = "-1";
        ddlKQ.SelectedValue = "-1";
        ddlDept.SelectedValue = "-1";
        GetPersonData();
    }
}
