using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GhtnTech.SecurityFramework;
using GhtnTech.SecurityFramework.BLL;
using GhtnTech.SecurityFramework.Model;
using GhtnTech.SecurityFramework.Utility;
using GhtnTech.SEP.DAL;
using DevExpress.Web.ASPxEditors;
using System.Reflection;
/// <summary>
///Util 的摘要说明
/// </summary>
public  class Util
{

    public static void BindDll(ASPxComboBox ddl, string column, string regex, string txtField, string valField)
    {
        DepartmentBll dept = new DepartmentBll();
        ddl.DataSource = dept.GetTreeList(column, regex);
        ddl.TextField = txtField;
        ddl.ValueField = valField;
        ddl.DataBind();
    }
    public static void BindDll(ASPxComboBox ddl, string where, string txtField, string valField)
    {
        DepartmentBll dept = new DepartmentBll();
        ddl.DataSource = dept.GetList2(where);
        ddl.TextField = txtField;
        ddl.ValueField = valField;
        ddl.DataBind();
    }
    public static DataTable LinqQueryToDataTable<T>(IEnumerable<T> query)
    {
        DataTable tbl = new DataTable();
        PropertyInfo[] props = null;
        foreach (T item in query)
        {
            if (props == null) //尚未初始化
            {
                Type t = item.GetType();
                props = t.GetProperties();
                foreach (PropertyInfo pi in props)
                {
                    Type colType = pi.PropertyType;
                    //針對Nullable<>特別處理
                    if (colType.IsGenericType && colType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        colType = colType.GetGenericArguments()[0];
                    }
                    //建立欄位
                    tbl.Columns.Add(pi.Name, colType);
                }
            }
            DataRow row = tbl.NewRow();
            foreach (PropertyInfo pi in props)
            {
                row[pi.Name] = pi.GetValue(item, null) ?? DBNull.Value;
            }
            tbl.Rows.Add(row);
        }
        return tbl;
    }
}
