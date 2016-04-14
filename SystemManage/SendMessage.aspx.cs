
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
using GhtnTech.SEP.DAL;
public partial class SystemManage_SendMessage : BasePage
{
    DBSCMDataContext dc = new DBSCMDataContext();
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
                UserHandle.InitModule(this.PageTag);//初始化此模块的权限。
                if (UserHandle.ValidationHandle(PermissionTag.Browse))//是否有浏览权限
                {
                    txtSender.Text = "调度所";
                    GetPersonList();
                }
                else
                {
                    Session["ErrorNum"] = "0";
                    Response.Redirect("~/Error.aspx");
                }
            }
            
        }
    }
    protected void btnInsertMsg_Click(object sender, EventArgs e)
    {
        JSHelper.OpenWebFormSize(UpdatePanel1, this, "AddSafetyContent.aspx", 700, 680, 10, 200);
    }

    protected void btnSendMsg_Click(object sender, EventArgs e)
    {
        if (txtMsg.Value == "")
        {
            JSHelper.Alert(UpdatePanel2, this, "请输入你要发送的短信内容！");
        }
        else if (xlstPerson.Items.Count <= 0)
        {
            JSHelper.Alert(UpdatePanel2, this, "请输入你要发送的对象！");
        }
        
        else
        {

            string msg = Sms.Send(txtMsg.Value, xlstPerson, txtSender.Text.Trim());
            if (msg == "1")
            {
                txtMsg.Value = "";
                JSHelper.Alert(UpdatePanel2, this, "已将要发送的短信提交到服务器！");
            }
            else
            {
                JSHelper.Alert(UpdatePanel2, this, "已将要发送的短信提交到服务器！\n"+@msg);
            }
        }
        //}
    }
    protected void btnAddPerson_Click(object sender, EventArgs e)
    {
        JSHelper.OpenWebFormSize(UpdatePanel2, this, "AddPerson.aspx", 700, 680, 10, 200);
    }
    /// <summary>
    /// 隐藏控件，利用Session,用于得到子菜单返回的值。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkGetValue_Click(object sender, EventArgs e)
    {
        List<string> lst = new List<string>();
        lst = (List<string>)Session["GetPhoneID"];
        ListBox lstSelected = new ListBox();
        foreach (string str in lst)
        {
            lstSelected.Items.Add(str);
        }
        foreach (ListItem item in lstSelected.Items)
        {
            if (!xlstPerson.Items.Contains(item))
            {
                xlstPerson.Items.Add(item);
            }
        }

        xlblPersonNumber.Text = xlstPerson.Items.Count.ToString();
        Session.Contents.Remove("GetPhoneID");
    }

    protected void btnDel_Click(object sender, EventArgs e)
    {
        if (xlstPerson.Items.Count > 0 && xlstPerson.SelectedIndex != -1)
        {
            xlstPerson.Items.Remove(xlstPerson.SelectedItem);
            xlblPersonNumber.Text = xlstPerson.Items.Count.ToString();
        }
        else
        {
            JSHelper.Alert(UpdatePanel2, this, "请先选中要删除的对象！");
        }

    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        xlstPerson.Items.Clear();
        xlblPersonNumber.Text = xlstPerson.Items.Count.ToString();
    }

    protected void AddPhoneNo_Click(object sender, EventArgs e)
    {
        string errorNumber = "";
        if (txtPhoneNo.Text.Trim().Length > 0)
        {
            string[] strP = txtPhoneNo.Text.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            ListBox lstP = new ListBox();
            foreach (string str in strP)
            {
                lstP.Items.Add(str);
            }
            foreach (ListItem item in lstP.Items)
            {
                if (VerifyData.VerifyNumber(item.Text))
                {
                    if (!xlstPerson.Items.Contains(item))
                    {
                        xlstPerson.Items.Add(item);
                    }
                }
                else
                {
                    errorNumber += item.Text + "、";
                }

            }
            
            xlblPersonNumber.Text = xlstPerson.Items.Count.ToString();
            txtPhoneNo.Text = "";
            if (errorNumber.Length > 0)
            {
                JSHelper.Alert(UpdatePanel2, this, "下列号码非法：" + errorNumber.Substring(0, errorNumber.Length - 1) + @"\n注：小灵通要加区号，手机号码为11位。");
            }
        }
        else
        {
            JSHelper.Alert(UpdatePanel2, this, "请输入电话号码，多个号码之间用空格隔开！");
        }
    }
    
    protected void GetPersonList()
    {
        List<string> lst = XmlHelper.ReadXml(this, "App_Data", "person.xml");
        ListBox lstSelected = new ListBox();
        foreach (string str in lst)
        {
            if (str != "")
            {
                lstSelected.Items.Add(str.Trim());
            }
        }
        foreach (ListItem item in lstSelected.Items)
        {
            if (!xlstPerson.Items.Contains(item))
            {
                xlstPerson.Items.Add(item);
            }
        }

        xlblPersonNumber.Text = xlstPerson.Items.Count.ToString();
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (upctrlPsn.UploadedFiles[0].IsValid && upctrlPsn.HasFile)
        {
            commonFunc.SaveFile(upctrlPsn, this);
            string savePath = commonFunc.GetSavePath(upctrlPsn, this);
            Aspose.Cells.Workbook xlsPsn = new Aspose.Cells.Workbook(savePath);
            //xlsPsn.Open(savePath);
            Aspose.Cells.Cells cellsPsn = xlsPsn.Worksheets[0].Cells;
            var dtTemp = cellsPsn.ExportDataTable(0, 0, cellsPsn.MaxDataRow + 1, cellsPsn.MaxDataColumn + 1);
            ListBox lstUpdated = new ListBox();
            for (int i = 1; i < dtTemp.Rows.Count; i++)
            {
                if (commonFunc.VerifyNumber(dtTemp.Rows[i][0].ToString().Trim()))
                {
                    lstUpdated.Items.Add(dtTemp.Rows[i][0].ToString().Trim());
                }
            }
            foreach (ListItem item in lstUpdated.Items)
            {
                if (!xlstPerson.Items.Contains(item))
                {
                    xlstPerson.Items.Add(item);
                }
            }
            xlblPersonNumber.Text = xlstPerson.Items.Count.ToString();
            JSHelper.Alert(UpdatePanel2, this, "上传成功！");
        }
    }
}
