using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Coolite.Ext.Web;
using GhtnTech.SecurityFramework.BLL;
using System.Data;

public partial class Positioning_PersonnelPositioning : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            BaseLoad();
        }
    }

    private void BaseLoad()
    {
        UnitStore.DataSource = PublicCode.GetMaindept("");
        UnitStore.DataBind();
        if (SessionBox.GetUserSession().rolelevel.Trim().IndexOf("1") > -1)
        {
            cbbUnit.SelectedItem.Value = "241700000";
            cbbUnit.Disabled = false;
        }
        else
        {
            cbbUnit.SelectedItem.Value = SessionBox.GetUserSession().DeptNumber;
            cbbUnit.Disabled = true;
        }
    }

    [AjaxMethod]
    public void LoadFTPData()
    {
        if (cbbUnit.SelectedIndex == -1)
        {
            Ext.Msg.Alert("提示", "请选择单位").Show();
            return;
        }
        string strdata = DataReaderUtilFTP.readerFtpFile(
            PublicMethod.ReadXmlReturnNode("FTPServer", this), 
            PublicMethod.ReadXmlReturnNode("FTPUserName", this), 
            PublicMethod.ReadXmlReturnNode("FTPPWD", this), 
            PublicMethod.ReadXmlReturnNode("FTPFileName", this), 
            cbbUnit.SelectedItem.Value);
        string[] data = strdata.Split('\n');
        List<PersonnelPositioningEntity> ppes = new List<PersonnelPositioningEntity>();
        foreach (string r in data)
        {
            Codestr(r, ppes);
        }
        Store1.DataSource = ppes;
        Store1.DataBind();
    }

    [AjaxMethod]
    public void LoadlocalData()
    {
        if (cbbUnit.SelectedIndex == -1)
        {
            Ext.Msg.Alert("提示", "请选择单位").Show();
            return;
        }
        string strdata=DataReaderUtilFTP.readerLocalFile("C:\\20120622100746RYSS", cbbUnit.SelectedItem.Value);
        string[] data = strdata.Split('\n');
        List<PersonnelPositioningEntity> ppes = new List<PersonnelPositioningEntity>();
        foreach (string r in data)
        {
            Codestr(r, ppes);
        }
        Store1.DataSource = ppes;
        Store1.DataBind();
    }


    protected void RowClick(object sender, AjaxEventArgs e)
    {
        RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
        btnShowDetail.Disabled = sm.SelectedRows.Count == 0;
    }

    private void Codestr(string data, List<PersonnelPositioningEntity> ppes)
    {
        if (!string.IsNullOrEmpty(data))
        {
            string[] group = data.Split(';');
            if (group.Length >= 20)//源文件说明是共24个必填字段，但实际是最少20
            {
                PersonnelPositioningEntity ppe = new PersonnelPositioningEntity();
                ppe.Deptname = group[3];
                ppe.Card = group[7];
                ppe.Name = group[8];
                ppe.InTime = group[13];
                ppe.OutTime = group[14];
                ppe.Status = int.Parse(group[6]);
                string detail = "";
                for (int i = 15; i+4 < group.Length; i += 4)
                {
                    //detail += "<div style=\"white-space:nowrap;\">"+group[i + 3] + "到达" + group[i + 1] + "</div><HR>";
                    detail += group[i + 3] + "到达" + group[i + 1] + "<HR>";
                }
                ppe.Detail = detail;
                ppes.Add(ppe);
            }
        }
    }
}

public class PersonnelPositioningEntity
{
    private string _Deptname;
    private string _Card;
    private string _Name;
    private string _InTime;
    private string _OutTime;
    private int _Status;
    private string _Detail;

    public string Deptname
    {
        get
        {
            return this._Deptname;
        }
        set
        {
            this._Deptname = value;
        }
    }

    public string Card
    {
        get
        {
            return this._Card;
        }
        set
        {
            this._Card = value;
        }
    }

    public string Name
    {
        get
        {
            return this._Name;
        }
        set
        {
            this._Name = value;
        }
    }

    public string InTime
    {
        get
        {
            return this._InTime;
        }
        set
        {
            this._InTime = value;
        }
    }

    public string OutTime
    {
        get
        {
            return this._OutTime;
        }
        set
        {
            this._OutTime = value;
        }
    }

    public int Status
    {
        get
        {
            return this._Status;
        }
        set
        {
            this._Status = value;
        }
    }

    public string Detail
    {
        get
        {
            return this._Detail;
        }
        set
        {
            this._Detail = value;
        }
    }

}