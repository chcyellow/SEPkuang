<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserLoginTotal.aspx.cs" Inherits="SystemManage_UserLoginTotal" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2.Export, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView.Export" TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dxwgv" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>用户登录统计</title>
</head>
<body>
    <form id="form1" runat="server">
    <table runat="server" id ="table">
   <tr >
   <td>
   <table>
   <tr><td> 
       <asp:Label ID="Label1" runat="server" Text="选择矿："></asp:Label>
        </td>
   <td> 
       <dxe:ASPxComboBox ID="cboMainDept" runat="server" AutoPostBack="True" 
           onselectedindexchanged="cboMainDept_SelectedIndexChanged">
       </dxe:ASPxComboBox>
        </td>
        <td> 
            <asp:Label ID="Label2" runat="server" Text="选择科区："></asp:Label>
        </td>
        <td>
            <dxe:ASPxComboBox ID="cboDept" runat="server">
            </dxe:ASPxComboBox>
        </td>
        <td> 
            <asp:Label ID="Label3" runat="server" Text="姓名："></asp:Label>
        </td>
        <td>
        <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
        </td></tr>
   <tr>
   <td>
       <asp:Label ID="Label4" runat="server" Text="选择日期："></asp:Label></td>
       <td>
           <dxe:ASPxDateEdit ID="dateBegin" runat="server">
           </dxe:ASPxDateEdit>
       </td>
       <td>
           <asp:Label ID="Label5" runat="server" Text="到"></asp:Label>
       </td>
       <td><dxe:ASPxDateEdit ID="dateEnd" runat="server">
           </dxe:ASPxDateEdit></td>
           <td>
               <asp:Label ID="Label6" runat="server" Text="用户："></asp:Label></td>
           <td>
               <asp:TextBox ID="txtUser" runat="server"></asp:TextBox></td>
   </tr>
   </table>
   </td>
   
   
   <td>
       <asp:Button ID="btnSure" runat="server" Text="确定" onclick="btnSure_Click" />
       </td>
   <td>
       <asp:Button ID="btnExport" runat="server" Text="导出" onclick="btnExport_Click" />
       </td>
   </tr>
    </table>
    <div>
        <dxwgv:ASPxGridView ID="gvUserOptTotal" runat="server" Width="100%" 
            CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
            CssPostfix="Office2003_Blue" AutoGenerateColumns="False" 
            onbeforecolumnsortinggrouping="gvUserOptTotal_BeforeColumnSortingGrouping" 
            onpageindexchanged="gvUserOptTotal_PageIndexChanged">
            <Styles CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
                CssPostfix="Office2003_Blue">
                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                </Header>
                <LoadingPanel ImageSpacing="10px">
                </LoadingPanel>
            </Styles>
            <Images ImageFolder="~/App_Themes/Office2003Blue/{0}/">
                <CollapsedButton Height="12px" 
                    Url="~/App_Themes/Office2003Blue/GridView/gvCollapsedButton.png" Width="11px" />
                <ExpandedButton Height="12px" 
                    Url="~/App_Themes/Office2003Blue/GridView/gvExpandedButton.png" Width="11px" />
                <DetailCollapsedButton Height="12px" 
                    Url="~/App_Themes/Office2003Blue/GridView/gvCollapsedButton.png" Width="11px" />
                <DetailExpandedButton Height="12px" 
                    Url="~/App_Themes/Office2003Blue/GridView/gvExpandedButton.png" Width="11px" />
                <FilterRowButton Height="13px" Width="13px" />
            </Images>
            <Columns>
                <dxwgv:GridViewDataTextColumn Caption="单位名称" FieldName="MAINDEPT" 
                    VisibleIndex="0" SortIndex="1" SortOrder="Ascending">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="科区名称" FieldName="DEPTNAME" 
                    VisibleIndex="1">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="用户名" FieldName="USERNAME" 
                    VisibleIndex="2">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="人员编码" FieldName="PERSONNUMBER" 
                    VisibleIndex="3" Visible="False">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="姓名" FieldName="NAME" 
                    VisibleIndex="3">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="单位编码" FieldName="MAINDEPTID" 
                    VisibleIndex="3" Visible="False">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="科区编码" FieldName="DEPTNUMBER" 
                    VisibleIndex="4" Visible="False">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="登录统计" FieldName="LOGINCOUNT" 
                    SortIndex="0" SortOrder="Descending" VisibleIndex="4">
                </dxwgv:GridViewDataTextColumn>
            </Columns>
            <StylesEditors>
                <ProgressBar Height="25px">
                </ProgressBar>
            </StylesEditors>
        </dxwgv:ASPxGridView>
    </div>
    <div>
        <dxwgv:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="gvUserOptTotal">
        </dxwgv:ASPxGridViewExporter>
    </div>
    </form>
</body>
</html>
