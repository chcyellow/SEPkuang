<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserGroupManage.aspx.cs" Inherits="SystemManage_UserGroupManage" %>

<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dxwtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dxwgv" %>

<%@ Register Assembly="DevExpress.Web.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dxrp" %>

<%@ Register assembly="DevExpress.Web.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPanel" tagprefix="dxp" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>用户组管理</title>
    <link href="../Style/FontStyle.css" rel="stylesheet" type="text/css" media="screen"/>
    <link href="../Style/PopWindows.css" rel="stylesheet" type="text/css" media="screen"/>
    <link href="../Style/Page.css" rel="stylesheet" type="text/css" /> 
</head>
<body>
    <form id="form1" runat="server">
    <table borderColor="#fffbec" cellSpacing="0" cellPadding="0" width="100%" align="center" bgColor="#fffbec" border="1" frame="void">
        <tbody>
        <tr align="left">
        <td height="20">你现在的位置是：系统管理->用户组管理</td></tr>
        </tbody>
    </table>
    <div>
        <dxwtl:ASPxTreeList ID="treeUserGroup" runat="server" 
            AutoGenerateColumns="False" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" 
            CssPostfix="Aqua" DataSourceID="ObjectDataSource1" 
            KeyFieldName="USERGROUPID" ParentFieldName="PARENTUSERGROUPID">
            <Styles CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua">
                <CustomizationWindowContent VerticalAlign="Top">
                </CustomizationWindowContent>
            </Styles>
            <Images ImageFolder="~/App_Themes/Aqua/{0}/">
                <CollapsedButton Height="15px" 
                    Url="~/App_Themes/Aqua/TreeList/CollapsedButton.png" Width="15px" />
                <ExpandedButton Height="15px" 
                    Url="~/App_Themes/Aqua/TreeList/ExpandedButton.png" Width="15px" />
                <SortAscending Height="5px" Url="~/App_Themes/Aqua/TreeList/SortAsc.png" 
                    Width="7px" />
                <SortDescending Height="5px" Url="~/App_Themes/Aqua/TreeList/SortDesc.png" 
                    Width="7px" />
                <CustomizationWindowClose Height="16px" Width="17px" />
            </Images>
            <SettingsText LoadingPanelText="" />
            <SettingsLoadingPanel Text="" />
            <SettingsPager>
                <AllButton>
                    <Image Height="19px" Width="27px" />
                </AllButton>
                <FirstPageButton>
                    <Image Height="19px" Width="23px" />
                </FirstPageButton>
                <LastPageButton>
                    <Image Height="19px" Width="23px" />
                </LastPageButton>
                <NextPageButton>
                    <Image Height="19px" Width="19px" />
                </NextPageButton>
                <PrevPageButton>
                    <Image Height="19px" Width="19px" />
                </PrevPageButton>
            </SettingsPager>
            <SettingsBehavior AllowFocusedNode="True" />
            <Columns>
                <dxwtl:TreeListTextColumn Caption="用户组编码" FieldName="USERGROUPID" 
                    Visible="False" VisibleIndex="0">
                </dxwtl:TreeListTextColumn>
                <dxwtl:TreeListTextColumn Caption="用户组" FieldName="USERGROUPNAME" SortIndex="0" 
                    SortOrder="Ascending" VisibleIndex="0">
            <CellStyle><Paddings Padding="1" /></CellStyle>
            <EditCellStyle><Paddings Padding="1" /></EditCellStyle>
            <DataCellTemplate>
                <table width="100%" cellpadding="0" cellspacing="0"><tr><td style="width:20px">
                    <dxe:ASPxImage ID="ASPxImage1" runat="server" IsPng="true" Width="21" Height="21" 
                        ImageUrl='<%# GetNodeGlyph(Container) %>' ImageAlign="Top" />
                </td><td>
                    <%# Container.Text %>
                </td></tr></table>
            </DataCellTemplate>
            <EditCellTemplate>
                <table width="100%" cellpadding="0" cellspacing="0"><tr><td style="width: 20px">
                    <dxe:ASPxImage ID="ASPxImage1" runat="server" IsPng="true" Width="21" Height="21" 
                        ImageUrl='<%# GetNodeGlyph(Container) %>' ImageAlign="Top" />
                </td><td>
                    <dxe:ASPxTextBox runat="server" ID="ed"  Text='<%# Bind("USERGROUPNAME") %>' Width="80px">
                    </dxe:ASPxTextBox>
                </td></tr></table>                            
            </EditCellTemplate>                        
        </dxwtl:TreeListTextColumn>
                <dxwtl:TreeListTextColumn Caption="父用户组" FieldName="PARENTUSERGROUPID" 
                    Visible="False" VisibleIndex="2">
                </dxwtl:TreeListTextColumn>
                <dxwtl:TreeListCommandColumn Caption="操作" ShowNewButtonInHeader="True" 
                    VisibleIndex="1">
                    <EditButton Visible="True">
                    </EditButton>
                    <NewButton Visible="True">
                    </NewButton>
                    <DeleteButton Visible="True">
                    </DeleteButton>
                </dxwtl:TreeListCommandColumn>
            </Columns>
        </dxwtl:ASPxTreeList>
    </div>
    <div>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
            DataObjectTypeName="GhtnTech.SecurityFramework.Model.SF_UserGroup" 
            DeleteMethod="DeleteUserGroup" InsertMethod="CreateUserGroup" 
            SelectMethod="GetUserGroupList" TypeName="GhtnTech.SecurityFramework.UserGroup" 
            UpdateMethod="UpdateUserGroup">
            <SelectParameters>
                <asp:SessionParameter DefaultValue=" " Name="strWhere" 
                    SessionField="WhereUserGroup" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
    </form>
</body>
</html>
