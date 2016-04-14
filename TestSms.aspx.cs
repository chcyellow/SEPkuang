using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GhtnTech.SecurityFramework.Utility;
public partial class TestSms : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (IsCallback)
        //{
        //    System.Threading.Thread.Sleep(50000);
        //}
    }
    protected void btnSend_Click(object sender, EventArgs e)
    {
        
        //if (txtPhone.Text != "" && txtSms.Text != "")
        //{
        //    string msg = Sms.Send(txtPhone.Text.Trim(), txtSms.Text.Trim(),SmsType.Other,"123","");
        //    if ( msg== "1")
        //    {
        //        JSHelper.Alert("发送成功！", this);
        //    }
        //    else
        //    {
        //        msg = @"发送失败！原因如下：\n" + msg;
        //        JSHelper.Alert(msg, this);
        //    }
  
        //}
        //else
        //{
        System.Threading.Thread.Sleep(5000);
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
            txtSms.Text += item.Value;
        }
        int count = 0;
        for (int i = 0; i < 1000000; i++)
        {
            count++;
        }
        JSHelper.Alert(@count.ToString(), this);
        //}
    }
    protected void btnSendAll_Click(object sender, EventArgs e)
    {
        List<string> lstPhone = txtPhone.Text.Split(',').ToList();
        string msg = Sms.Send(lstPhone, txtSms.Text.Trim());
        if (msg == "1")
        {
            JSHelper.Alert("发送成功！", this);
        }
        else
        {
            JSHelper.Alert(msg, this);
        }
    }
}
