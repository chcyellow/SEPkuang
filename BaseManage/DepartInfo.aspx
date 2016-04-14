<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DepartInfo.aspx.cs" Inherits="BaseManage_DepartInfo" %>

<%@ Register Assembly="ALinq.Web, Version=1.0.8.0, Culture=neutral, PublicKeyToken=2b23f34316d38f3a"
    Namespace="ALinq.Web.Controls" TagPrefix="cc1" %>

<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>
<%@ Register assembly="DevExpress.Web.ASPxTreeList.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxTreeList" tagprefix="dxwtl" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style=" text-align:center">
    
        <dxwtl:ASPxTreeList ID="DepTreeList" runat="server" 
            AutoGenerateColumns="False" 
            CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
            CssPostfix="Office2003_Blue" Width="100%"  
            KeyFieldName="Deptnumber" ParentFieldName="Fatherid">
            <Styles CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
                CssPostfix="Office2003_Blue">
            </Styles>
            <Images ImageFolder="~/App_Themes/Office2003Blue/{0}/">
                <CollapsedButton Height="11px" 
                    Url="~/App_Themes/Office2003Blue/TreeList/CollapsedButton.png" Width="11px" />
                <ExpandedButton Height="11px" 
                    Url="~/App_Themes/Office2003Blue/TreeList/ExpandedButton.png" Width="11px" />
                <CustomizationWindowClose Height="12px" Width="13px" />
            </Images>
            <Columns>
                <dxwtl:TreeListTextColumn Caption="部门名称" VisibleIndex="0" FieldName="Deptname">
                    <CellStyle HorizontalAlign="Left">
                    </CellStyle>
                </dxwtl:TreeListTextColumn>
                <dxwtl:TreeListTextColumn Caption="父编码" FieldName="Fatherid" Visible="False" 
                    VisibleIndex="1">
                </dxwtl:TreeListTextColumn>
                <dxwtl:TreeListTextColumn Caption="部门编码" VisibleIndex="1" 
                    FieldName="Deptnumber">
                    <CellStyle HorizontalAlign="Left">
                    </CellStyle>
                </dxwtl:TreeListTextColumn>
            </Columns>
        </dxwtl:ASPxTreeList>
    
    </div>
    <cc1:ALinqDataSource ID="ALinqDataSource1" runat="server" 
        ContextTypeName="GhtnTech.SEP.DAL.DBSCMDataContext" TableName="Department" >
        
    </cc1:ALinqDataSource>
    </form>
</body>
</html>
