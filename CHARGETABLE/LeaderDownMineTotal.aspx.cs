using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls; 
using Coolite.Ext.Web;
using GhtnTech.SEP.DAL;
using GhtnTech.SecurityFramework.BLL;
using System.Data;
using Aspose.Cells;
public partial class CHARGETABLE_LeaderDownMineTotal : BasePage
{
    static List<LeaderDownMine> ldm = new List<LeaderDownMine>();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Ext.IsAjaxRequest)
        {
            //初始化模块权限
            UserHandle.InitModule(this.PageTag);
            //是否有浏览权限
            if (UserHandle.ValidationHandle(PermissionTag.Browse))
            {
                int year = DateTime.Today.Year;
                for (int i = 2013; i <= year; i++)
                {
                    cboYear.Items.Add(new Coolite.Ext.Web.ListItem(i.ToString(), i.ToString()));
                }
                cboYear.SelectedItem.Value = DateTime.Today.Year.ToString();
                cboMonth.SelectedItem.Value = (DateTime.Today.Month - 1).ToString();
                InitDept();
                Changed();
            }
            else
            {
                Response.Redirect("~/Login.aspx");
            }
        }
    }
    [AjaxMethod]
    public void Changed()//处别选择下拉事件
    {
        InitPost(cbbDept.SelectedItem.Value);
        if (cbbDept.SelectedItem.Value == "-1")
        {
            cboPost.Disabled = true;
        }
        else
        {
            cboPost.Disabled = false;
        }

    }
    private void InitDept()
    {
        HBBLL hb = new HBBLL();
        var dept = hb.GetVPDept();
        var data = from d in dept
                   select new
                   {
                       Deptnumber = d.DeptNumber,
                       Deptname = d.DeptName
                   };
        DeptStore.DataSource = data;
        DeptStore.DataBind();
        if (SessionBox.GetUserSession().rolelevel.Contains("1") || SessionBox.GetUserSession().rolelevel.Contains("0"))
        {

            cbbDept.Items.Insert(0, new Coolite.Ext.Web.ListItem("--全部--", "-1"));
            cbbDept.SelectedItem.Value = "-1";
            cbbDept.Disabled = false;
        }
        else
        {
            cbbDept.SelectedItem.Value = SessionBox.GetUserSession().DeptNumber;
            cbbDept.Disabled = true;
        }
    }
    private void InitPost(string deptid)
    {
        HBBLL hb = new HBBLL();
        var post = hb.GetVPPost(deptid);
        var data = from p in post
                   select new
                   {
                       PosID = p.PosId,
                       PosName = p.PosName
                   };
        PostStore.DataSource = data;
        PostStore.DataBind();
        cboPost.Items.Insert(0, new Coolite.Ext.Web.ListItem("--全部--", "-1"));
        cboPost.SelectedItem.Value = "-1";
    }
    protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)
    {
        storeload();
    }
    private void storeload()
    {
        string person = "";
        if (txtName.Text.Trim() != "")
        {
            string[] ss = System.Text.RegularExpressions.Regex.Split(txtName.Text.Trim(), "\\s+|[，,]");

            foreach (var s in ss)
            {
                person = string.Format("'{0}',", s);
            }
            person = person.Substring(0, person.Length - 1);
        }
        HBBLL hb = new HBBLL();
        var vp = hb.GetVP(cbbDept.SelectedItem.Value, cboPost.SelectedItem.Value, person);
        var minetotal = hb.GetMineTotal(person, string.Format("{0}-{1}", cboYear.SelectedItem.Value, cboMonth.SelectedItem.Value.PadLeft(2, '0')));
        var daibantotal = hb.GetDaiBanTotal(person, string.Format("{0}-{1}", cboYear.SelectedItem.Value, cboMonth.SelectedItem.Value.PadLeft(2, '0')));
        var data = from v in vp 
                   join m in minetotal on v.PersonNumber equals m.PersonId into x
                   from v_m  in x.DefaultIfEmpty()
                   join d in daibantotal on v.PersonNumber equals d.PersonNumber into y
                   from v_d in y.DefaultIfEmpty()
                   select new LeaderDownMine
                   {
                        MainDeptNumber = v.DeptNumber, 
                        MainDeptName = v.DeptName,
                        PersonNumber = v.PersonNumber,
                        PersonName  = v.Name,
                        PostID  =v.PosId,
                        PostName  = v.PosName,
                        DownMineTotal = v_m==null?0:v_m.Count.Value,
                        DaiBanZao  =v_d==null?0:v_d.Zao.Value,
                        DaiBanZhong  =v_d==null?0:v_d.Zhong.Value,
                        DaiBanYe  = v_d==null?0:v_d.Ye.Value,
                        DaiBanTotal = (v_d == null ? 0 : v_d.Zao.Value) + (v_d == null ? 0 : v_d.Zhong.Value) + (v_d == null ? 0 : v_d.Ye.Value),
                        About = "" 

                   };
        ldm = data.ToList<LeaderDownMine>();
        Store1.DataSource = data;
        Store1.DataBind();
    }


    [AjaxMethod]
    public void ExportXls()
    {
        HBBLL hb = new HBBLL();
        DataTable dt = Util.LinqQueryToDataTable<LeaderDownMine>(ldm);
        dt.TableName = "tt";
        var s = Aspose.Cells.CellsHelper.GetVersion();
        WorkbookDesigner designer = new WorkbookDesigner();
        designer.Open(MapPath("tt.xls"));
        //数据源 
        designer.SetDataSource(dt);
        //报表标题 
        designer.SetDataSource("Title", string.Format("{0}副总以上领导下井人员情况汇总表",cbbDept.SelectedItem.Value=="-1"?"各矿":cbbDept.SelectedItem.Text));
        designer.SetDataSource("DownDate", string.Format("{0}年{1}月)", cboYear.SelectedItem.Value, cboMonth.SelectedItem.Value.PadLeft(2, '0')));
        
        designer.Process();

        designer.Save(string.Format("各矿副总以上领导下井人员情况汇总表{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmssffff")), SaveType.OpenInExcel, FileFormatType.Excel97To2003, Response);
        Response.Flush();
        Response.Close();
        designer = null;
        Response.End();
    }
    private void BindDBDetail(string mm, string personnumber, string banci)
    {
        HBBLL hb = new HBBLL();

        var data = hb.GetDBDetail(mm, personnumber, banci);
        DBDetailStore.DataSource = data;
        DBDetailStore.DataBind();
    }
    private void BindDownDetail(string mm, string personnumber)
    {
        HBBLL hb = new HBBLL();

        var data = hb.GetDownDetail(mm, personnumber);
        DownDetailStore.DataSource = data;
        DownDetailStore.DataBind();
    }
    
    protected void Cell_Click(object sender, AjaxEventArgs e)
    {
        CellSelectionModel sm = this.GridPanel1.SelectionModel.Primary as CellSelectionModel;
        string mm = string.Format("{0}-{1}", cboYear.SelectedItem.Value, cboMonth.SelectedItem.Value.PadLeft(2, '0'));
        if (sm.SelectedCell.ColIndex <= 5 || sm.SelectedCell.Value.Trim() == "0")
            return;
        
        switch (sm.SelectedCell.Name.Trim())
        {
            case "DownMineTotal":
                
                BindDownDetail(mm, sm.SelectedCell.RecordID.Trim());
                winDown.Show();
                break;
            case "DaiBanTotal":
                
                BindDBDetail(mm, sm.SelectedCell.RecordID.Trim(), "");
                winDaiBan.Show();
                break;
            case "DaiBanZhong":
                
                BindDBDetail(mm, sm.SelectedCell.RecordID.Trim(), "中班");
                winDaiBan.Show();
                break;
            case "DaiBanZao":
                
                BindDBDetail(mm, sm.SelectedCell.RecordID.Trim(), "早班");
                winDaiBan.Show();
                break;
            case "DaiBanYe":
                
                BindDBDetail(mm, sm.SelectedCell.RecordID.Trim(), "夜班");
                winDaiBan.Show();
                break;
            
        }

    }
}