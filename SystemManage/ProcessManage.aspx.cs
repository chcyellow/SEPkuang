using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxClasses;
using System.Reflection;
using System.Runtime.Remoting;
public partial class SystemManage_ProcessManage : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {

        
    }
    protected void gvProcess_CustomColumnDisplayText(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewColumnDisplayTextEventArgs e)
    {
        if (e.Column.FieldName == "STATUS")
        {
            e.DisplayText = e.Value.ToString()=="1" ? "激活" : "关闭";
        }
    }
    //protected void lnkGetValue_Click(object sender, EventArgs e)
    //{
    //    string lst = "";
    //    lst = (string)Session["GetRole"];
    //    TextBox memo = gvProcess.FindEditFormTemplateControl("lblRole") as TextBox;
    //    memo.Text = lst.Remove(lst.LastIndexOf(","));
    //    Session.Contents.Remove("GetRole");
    //}
    //protected void gvProcess_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
    //{
    //    TextBox txtItemName = gvProcess.FindEditFormTemplateControl("txtItemName") as TextBox;
    //    TextBox memo = gvProcess.FindEditFormTemplateControl("lblRole") as TextBox;
    //    TextBox txtItemTag = gvProcess.FindEditFormTemplateControl("txtItemTag") as TextBox;
    //    TextBox txtAbout = gvProcess.FindEditFormTemplateControl("txtAbout") as TextBox;
    //    RadioButtonList radStatus = gvProcess.FindEditFormTemplateControl("radStatus") as RadioButtonList;
    //    e.NewValues["ITEMNAME"] = txtItemName.Text;
    //    e.NewValues["ITEMVALUE"] = memo.Text;
    //    e.NewValues["ITEMTAG"] = txtItemTag.Text;
    //    e.NewValues["ABOUT"] = txtAbout.Text;
    //    e.NewValues["STATUS"] = radStatus.SelectedItem.Value;
    //}
    //protected void gvProcess_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
    //{

    //}
    //protected void gvProcess_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
    //{
    //    TextBox txtItemName = gvProcess.FindEditFormTemplateControl("txtItemName") as TextBox;
    //    TextBox memo = gvProcess.FindEditFormTemplateControl("lblRole") as TextBox;
    //    TextBox txtItemTag = gvProcess.FindEditFormTemplateControl("txtItemTag") as TextBox;
    //    TextBox txtAbout = gvProcess.FindEditFormTemplateControl("txtAbout") as TextBox;
    //    RadioButtonList radStatus = gvProcess.FindEditFormTemplateControl("radStatus") as RadioButtonList;
    //    e.NewValues["ITEMNAME"] = txtItemName.Text;
    //    e.NewValues["ITEMVALUE"] = memo.Text;
    //    e.NewValues["ITEMTAG"] = txtItemTag.Text;
    //    e.NewValues["ABOUT"] = txtAbout.Text;
    //    e.NewValues["STATUS"] = radStatus.SelectedItem.Value;
    //}
    protected void girdConfDetail_BeforePerformDataSelect(object sender, EventArgs e)
    {
        Session["WhereConfDetail"] = "ITEMID=" + (sender as DevExpress.Web.ASPxGridView.ASPxGridView).GetMasterRowKeyValue().ToString();
    }
    //protected void gvProcess_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
    //{
    //    //if (!gvProcess.IsEditing || e.Column.FieldName != "ITEMTAG") return;
    //    //if (e.KeyValue == DBNull.Value || e.KeyValue == null) return;
    //    //object val = gvProcess.GetRowValuesByKeyValue(e.KeyValue, "ITEMVALUE");
    //    //if (val == DBNull.Value) return;
    //    //string moduleTag = (string)val;

    //    //DevExpress.Web.ASPxEditors.ASPxComboBox combo = e.Editor as DevExpress.Web.ASPxEditors.ASPxComboBox;
    //    //FillModuleControlsCombo(combo, moduleTag);

    //    //combo.Callback += new DevExpress.Web.ASPxClasses.CallbackEventHandlerBase(cmbModuleControls_OnCallback);
    //}
    //protected void FillModuleControlsCombo(DevExpress.Web.ASPxEditors.ASPxComboBox cmb, string moduleTag)
    //{
    //    if (string.IsNullOrEmpty(moduleTag)) return;

    //    List<string> cities = GetModuleControls(moduleTag);
    //    cmb.Items.Clear();
    //    foreach (string city in cities)
    //        cmb.Items.Add(city);
    //}
    //List<string> GetModuleControls(string moduleTag)
    //{
    //    List<string> list = new List<string>();
    //    odsModule.SelectParameters[0].DefaultValue = moduleTag;
    //    //DataView view = (DataView)AccessDataSourceCities.Select(DataSourceSelectArguments.Empty);
    //    //for (int i = 0; i < view.Count; i++)
    //    //{
    //    //    list.Add((string)view[i][0]);
    //    //}
    //    System.Web.UI.Page p = (System.Web.UI.Page)CreateObject2(moduleTag);
    //    foreach (Control c in p.Controls)
    //    {
    //        list.Add(c.ID);
    //    }   
    //    return list;
    //}

    //private void cmbModuleControls_OnCallback(object source, CallbackEventArgsBase e)
    //{
    //    FillModuleControlsCombo(source as DevExpress.Web.ASPxEditors.ASPxComboBox, e.Parameter);
    //}
    //public static object CreateObject(string classname)
    //{
    //    return Assembly.Load("SEP_deploy").CreateInstance(classname);
    //}
    //public static object CreateObject2(string classname)
    //{
    //    Assembly assembly = Assembly.Load("SEP_deploy");
    //    Type type = assembly.GetType(classname);

    //    object obj = Activator.CreateInstance(type);
    //    return obj;
    //}
    //public static object CreateObject3(string classname)
    //{

    //    Assembly oa = Assembly.LoadFrom(HttpContext.Current.Server.MapPath("~/Script/SEP_deploy.dll"));
    //    Type ot = oa.GetType(classname);

    //    //ObjectHandle oh = Activator.CreateInstance(oa.FullName, ot.FullName);
    //    //System.Web.UI.Page p1 = oh.Unwrap() as System.Web.UI.Page;

    //    object obj3 = AppDomain.CurrentDomain.CreateInstanceAndUnwrap(oa.FullName, ot.FullName);
    //    System.Web.UI.Page p2 = obj3 as System.Web.UI.Page;

    //    //object obj5 = AppDomain.CurrentDomain.CreateInstance(oa.FullName, ot.FullName);
    //    //System.Web.UI.Page p3 = oh.Unwrap() as System.Web.UI.Page;
    //    return p2;
    //}
}
