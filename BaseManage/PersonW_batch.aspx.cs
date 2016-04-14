using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Coolite.Ext.Web;
using GhtnTech.SEP.DAL;
using System.Data.OleDb;
using System.Data;
using System.IO;

public partial class BaseManage_PersonW_batch : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [AjaxMethod]
    public void UpLoadFile()
    {
        if (fufExcel.HasFile)
        {
            if (fufExcel.FileName.Substring(fufExcel.FileName.LastIndexOf(".")+1).ToLower() == "xls")
            {
                Ext.DoScript("Ext.Msg.wait('正在加载你选择的文件...', '加载中');");
                string strPath = MapPath("~") + @"\SystemNotice\FileUpload\";
                string strOldFileName = fufExcel.FileName.Substring(fufExcel.FileName.LastIndexOf("\\") + 1);
                string strNewFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + fufExcel.FileName.Substring(fufExcel.FileName.LastIndexOf("."));
                fufExcel.PostedFile.SaveAs(strPath + strNewFileName);
                LoadData(strPath + strNewFileName, "Sheet1");
            }
            else
            {
                Ext.Msg.Alert("提示", "请选择Excel文件").Show();
            }
        }
        else
        {
            Ext.Msg.Alert("提示", "请选择Excel文件").Show();
        }

    }

    private void LoadData(string path, string sheetName)
    {
        DataSet ds = new DataSet();

        string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source= " + path + "; Extended Properties='Excel 8.0'";
        OleDbDataAdapter oada = new OleDbDataAdapter("select * from [" + sheetName + "$]", strConn);
        oada.Fill(ds);
        if (CheckExcel(ds.Tables[0]))
        {
            Store1.DataSource = ds.Tables[0];
            Store1.DataBind();
            DeleteExcel(path);
            Ext.Msg.Alert("提示", "加载完成！").Show();
        }
        else
        {
            DeleteExcel(path);
            Ext.Msg.Alert("提示", "你选择的文件系统无法识别！").Show();
        }
    }

    private bool CheckExcel(DataTable dt)
    {
        string[] columns = new string[] { "工号", "姓名", "性别", "电话", "职务", "部门", "灯号" };
        for (int i = 0; i < dt.Columns.Count; i++)
        {
            if (dt.Columns[i].ColumnName.Trim() == columns[i])
            {
                continue;
            }
            else
            {
                return false;
            }
        }
        return true;

    }

    private void DeleteExcel(string strPhyPath)
    {
        File.Delete(strPhyPath);
    }
}