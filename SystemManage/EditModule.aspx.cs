using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GhtnTech.SecurityFramework;
using GhtnTech.SecurityFramework.Model;
using GhtnTech.SecurityFramework.BLL;
using GhtnTech.SecurityFramework.Utility;
public partial class SystemManage_EditModule : BasePage
{
    Module mbll = new Module();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
            //UserHandle.InitModule(this.PageTag);
            //if (!UserHandle.ValidationHandle(PermissionTag.Grant))
            //{
            //    Session["ErrorNum"] = "0";
            //    Response.Redirect("~/Error.aspx");
            //}
            //else
            //{
            if (Request.QueryString["mid"] != null)
            {
                GetModule(int.Parse(Request.QueryString["mid"].ToString()));
            }
            else
            {
                GetModuleGroup();
            }
                BindOpt();
            //}
        }
        btnAdd.Enabled = lstOpt.SelectedIndex >= 0 ? true : false;
        btnRemove.Enabled = lstSelectedOpt.SelectedIndex >= 0 ? true : false;
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        MoveItems(true);// true since we add
    }
    protected void btnRemove_Click(object sender, EventArgs e)
    {
        MoveItems(false); // false since we remove
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtModule.Text != "")
        {
            SF_Module m = new SF_Module();
            m.ModuleGroupID = Convert.ToDecimal(cboModuleGroup.SelectedItem.Value.ToString());
            m.ModuleName = txtModule.Text.Trim();
            m.ModuleTag = txtUrl.Text.Trim().Replace("/", "_").Remove(txtUrl.Text.LastIndexOf("."));
            m.ModuleOrder = Convert.ToDecimal(txtOrder.Text);
            m.ModuleUrl = txtUrl.Text.Trim();
            m.About = txtAbout.Text.Trim();
            m.Status = radStatus.SelectedValue;
            if (Request.QueryString["mid"] != null)
            {
                m.ModuleID = int.Parse(Request.QueryString["mid"].ToString()); 
                switch (Convert.ToInt32(mbll.UpdateModule(m)))
                {
                    case 1:
                        List<string> lst = new List<string>();
                        foreach (ListItem item in lstOpt.Items)
                        {
                            string s = string.Empty;
                            s = Request.QueryString["mid"].ToString() + "|" + item.Value + "|0";
                            lst.Add(s);
                        }
                        foreach (ListItem item in lstSelectedOpt.Items)
                        {
                            string s = string.Empty;
                            if (txtOldOperator.Text != "")
                            {
                                string[] oldOperator = (txtOldOperator.Text.Remove(txtOldOperator.Text.LastIndexOf("|"))).Split('|');
                                if (!oldOperator.Contains(item.Value))
                                {
                                    s = Request.QueryString["mid"].ToString() + "|" + item.Value + "|1";
                                    lst.Add(s);
                                }
                            }
                            else
                            {
                                s = Request.QueryString["mid"].ToString() + "|" + item.Value + "|1";
                                lst.Add(s);
                            }
                        }
                        if (mbll.UpdateOperatorList(lst))
                        {
                            JSHelper.Alert("更新成功！", this);
                        }
                        else
                        {
                            JSHelper.Alert("更新操作失败！", this);
                        }
                        break;
                    case 2:
                        JSHelper.Alert("标识已经存在，请重新定义标识！", this);
                        break;
                    default:
                        JSHelper.Alert("更新模块失败！", this);
                        break;
                }
            }
            else
            {
                if (!mbll.ModuleExists(txtModuleTag.Text.Trim()))
                {
                    int MID = (int)mbll.CreateModule(m);//返回模块ID;
                    if (MID != 0)//添加OK
                    {
                        List<string> lst = new List<string>();//建立事务列表
                        foreach (ListItem item in lstSelectedOpt.Items)
                        {
                            string s = string.Empty;
                            if (txtOldOperator.Text != "")
                            {
                                string[] oldOperator = (txtOldOperator.Text.Remove(txtOldOperator.Text.LastIndexOf("|"))).Split('|');
                                if (!oldOperator.Contains(item.Value))
                                {
                                    s = MID.ToString() + "|" + item.Value;
                                    lst.Add(s);
                                }
                            }
                            else
                            {
                                s = MID.ToString() + "|" + item.Value;
                                lst.Add(s);
                            }
                        }
                        //权限加入是否成功！
                        if (mbll.CreateOperatorList(lst))
                        {
                            JSHelper.AlertAndRefreshParentWin("添加成功！", "ModuleManage.aspx", this);
                        }
                        else
                        {
                            JSHelper.Alert("添加操作失败!",this);
                        }
                    }
                    else
                    {
                        JSHelper.Alert("标识已存在,请更换后重试!",this);
                    }
                }
                else
                {
                    JSHelper.Alert("添加操作失败!",this);
                }
            }
        }
        else
        {
            JSHelper.Alert("请填写完整！", this);
        }
    }
    
    private void MoveItems(bool isAdd)
    {

        if (isAdd)// means if you add items to the right box
        {

            for (int i = lstOpt.Items.Count - 1; i >= 0; i--)
            {

                if (lstOpt.Items[i].Selected)
                {

                    lstSelectedOpt.Items.Add(lstOpt.Items[i]);

                    lstSelectedOpt.ClearSelection();

                    lstOpt.Items.Remove(lstOpt.Items[i]);

                }

            }

        }
        else // means if you remove items from the right box and add it back to the left box
        {

            for (int i = lstSelectedOpt.Items.Count - 1; i >= 0; i--)
            {

                if (lstSelectedOpt.Items[i].Selected)
                {

                    lstOpt.Items.Add(lstSelectedOpt.Items[i]);

                    lstOpt.ClearSelection();

                    lstSelectedOpt.Items.Remove(lstSelectedOpt.Items[i]);

                }

            }

        }

    }

    private void BindOpt()
    {
        Operator obll = new Operator();
        lstOpt.Items.Clear();
        string strwhere = "";
        if (Request.QueryString["mid"] != null)
        {
            Module mbll = new Module();
            var mlist = mbll.GetModuleOptArray(int.Parse(Request.QueryString["mid"].ToString()));
            if (mlist.Count > 0)
            {
                foreach (var m in mlist)
                {
                    string[] r = m.ToString().Split(',');
                    lstSelectedOpt.Items.Add(new ListItem(r[1], r[0]));
                    strwhere += "'" +r[0] +"'"+ ",";
                    txtOldOperator.Text += r[0] + "|";
                }
            }
        }
        if (strwhere != "")
        {
            strwhere = "not OPERATORTAG in(" + strwhere.Substring(0, strwhere.Length - 1) + ")";
        }
        var optlist = obll.GetOperatorList(strwhere, "").Tables[0];
        lstOpt.DataSource = optlist;
        lstOpt.DataTextField = "OPERATORNAME";
        lstOpt.DataValueField = "OPERATORTAG";
        lstOpt.DataBind();

    }
    protected void lstOpt_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnAdd.Enabled = lstOpt.SelectedIndex >= 0 ? true : false;
    }
    protected void lstSelectedOpt_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnRemove.Enabled = lstSelectedOpt.SelectedIndex >= 0 ? true : false;
    }

    private void GetModule(int mid)
    {
        
        var m = mbll.GetModuleModel(mid);
        var mg = mbll.GetModuleGroupList("").Tables[0];
        cboModuleGroup.DataSource = mg;
        cboModuleGroup.TextField = "MODULEGROUPNAME";
        cboModuleGroup.ValueField = "MODULEGROUPID";
        cboModuleGroup.DataBind();
        cboModuleGroup.Value = m.ModuleGroupID.ToString();
        txtModule.Text = m.ModuleName;
        txtModuleTag.Text = m.ModuleTag;
        txtOrder.Text = m.ModuleOrder.ToString();
        txtUrl.Text = m.ModuleUrl;
        txtAbout.Text = m.About;
        radStatus.SelectedValue = m.Status == "1" ? "1" : "2";

    }
    private void GetModuleGroup()
    {
        var mg = mbll.GetModuleGroupList("").Tables[0];
        cboModuleGroup.DataSource = mg;
        cboModuleGroup.TextField = "MODULEGROUPNAME";
        cboModuleGroup.ValueField = "MODULEGROUPID";
        cboModuleGroup.DataBind();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["mid"] != null)
        {
            Response.Redirect("ModuleManage.aspx");
        }
        else
        {
            JSHelper.RefreshParent("ModuleManage.aspx", this);
        }
    }
}
