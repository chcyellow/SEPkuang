<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LogManage.aspx.cs" Inherits="SystemManage_LogManage" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2.Export, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView.Export" TagPrefix="dxwgv" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dxwgv" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>日志管理</title>
    <link href="../Style/FontStyle.css" rel="stylesheet" type="text/css" media="screen"/>
    <link href="../Style/PopWindows.css" rel="stylesheet" type="text/css" media="screen"/>
     <link href="../Style/Page.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <table borderColor="#fffbec" cellSpacing="0" cellPadding="0" width="100%" align="center" bgColor="#fffbec" border="1" frame="void">
        <tbody>
        <tr align="left">
        <td height="20">你现在的位置是：系统管理->日志管理</td></tr>
        </tbody>
    </table>
    <fieldset id="Fieldset1" class="Fieldsetbox mbox pbox">
        <legend class="fsTitle">功能操作</legend>
        <table>
            <tr>
            <td>
            <table width="100%" border="0" cellpadding="0" cellspacing="5">
            <tr>
                <td style=" text-align:right;">
                    用户：</td>
                <td>
                    <asp:TextBox ID="txtUser" runat="server" Width="100px"></asp:TextBox>
                    </td>
                <td style=" text-align:right;">
                    IP：</td>
                <td>
                    <asp:TextBox ID="txtIP" runat="server" Width="100px"></asp:TextBox>
                    <%--DataSourceID="odsUserGroup" DataTextField="USERGROUPNAME" DataValueField="USERGROUPID"--%>
                </td>
                <td style=" text-align:right;">
                    单位：
                </td>
                <td>
                    <asp:DropDownList ID="ddlDept" runat="server" Width="150px" AutoPostBack="True" 
                        onselectedindexchanged="ddlDept_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td style=" text-align:right;">
                    起始时间：
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="dateBegin" runat="server" 
                        Spacing="0" Width="150px">
                    </dxe:ASPxDateEdit>
                </td>
                
                <%--<td style=" text-align:right;">
                 在线状态：   
                </td>
                <td>
                    <asp:DropDownList ID="ddlIsOnline" runat="server">
                        <asp:ListItem Value="-1">--全部--</asp:ListItem>
                        <asp:ListItem Value="1">在线</asp:ListItem>
                        <asp:ListItem Value="2">离线</asp:ListItem>
                    </asp:DropDownList>
                </td>--%>
            </tr>
            <tr>
            <td style=" text-align:right;">
                 工号：   
                </td>
                <td>
                    <asp:TextBox ID="txtPsnNo" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td style=" text-align:right;">
                 姓名：   
                </td>
                <td>
                    <asp:TextBox ID="txtName" runat="server" Width="100px"></asp:TextBox>
                </td>
                
                <td style=" text-align:right;">
                    科区：
                </td>
                <td>
                    <asp:DropDownList ID="ddlKQ" runat="server" Width="150px">
                    </asp:DropDownList>
                </td>
                <td style=" text-align:right;">结束时间：</td>
                <td>
                    <dxe:ASPxDateEdit ID="dateEnd" runat="server" 
                        Spacing="0" Width="150px">
                    </dxe:ASPxDateEdit>
                </td>
            </tr>
        </table>
            </td>
            <td>
                <asp:Button ID="btnSearch" runat="server" Text="查询" 
                    Height="40px" Width="40px" onclick="btnSearch_Click" />
            </td>
            <td>
                <asp:Button ID="btnExportLog" runat="server" Text="导出日志" 
                    onclick="btnExportLog_Click" />
            </td>
            <td>
                <asp:Button ID="btnBatchDelete" runat="server" Text="删除" 
                    onclick="btnBatchDelete_Click" />
            </td>                    
            </tr>
        </table>
    </fieldset>
    <div>
        <dxwgv:ASPxGridView ID="gvLogManage" runat="server" ClientInstanceName="gvLogManage"
            CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
            CssPostfix="Office2003_Blue" Width="100%" AutoGenerateColumns="False" 
            KeyFieldName="ID" onpageindexchanged="gvLogManage_PageIndexChanged" 
            onbeforecolumnsortinggrouping="gvLogManage_BeforeColumnSortingGrouping">
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
                <dxwgv:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0">
                    <HeaderStyle HorizontalAlign="Center" />
                    <HeaderTemplate>
                        <input id="chkSelectedAll" type="checkbox" onclick="gvLogManage.SelectAllRowsOnPage(this.checked);" style="vertical-align:middle;" title="选择或不选本页所有的日志记录" />
                    </HeaderTemplate>
                    <ClearFilterButton Visible="True">
                    </ClearFilterButton>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataTextColumn Caption="用户名" FieldName="USERNAME" 
                    VisibleIndex="1">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="工号" FieldName="PERSONNUMBER" 
                    VisibleIndex="2">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="姓名" FieldName="NAME" 
                    VisibleIndex="3">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="单位" FieldName="MAINDEPT" 
                    VisibleIndex="4">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="科区" FieldName="DEPTNAME" 
                    VisibleIndex="5">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="时间" FieldName="ACTIVETIME" 
                    VisibleIndex="6">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="IP地址" FieldName="IP" VisibleIndex="7">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="操作类型" FieldName="ACTIVETYPE" 
                    VisibleIndex="8">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="操作模块" FieldName="ACTIVEPAGE" 
                    VisibleIndex="9">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="描述" FieldName="ACTIVEDESCRIPTION" 
                    VisibleIndex="10">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="编号" FieldName="ID" Visible="False" 
                    VisibleIndex="10">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="DEPTNUMBER" Visible="False" 
                    VisibleIndex="10">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="MAINDEPTID" Visible="False" 
                    VisibleIndex="10">
                </dxwgv:GridViewDataTextColumn>
            </Columns>
            <StylesEditors>
                <ProgressBar Height="25px">
                </ProgressBar>
            </StylesEditors>
        </dxwgv:ASPxGridView>
    </div>
    <div>
        <dxwgv:ASPxGridViewExporter ID="expLog" runat="server" FileName="用户日志" 
            GridViewID="gvLogManage">
        </dxwgv:ASPxGridViewExporter>
    </div>
    </form>
</body>
</html>
