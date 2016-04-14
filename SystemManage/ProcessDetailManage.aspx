﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProcessDetailManage.aspx.cs" Inherits="SystemManage_ProcessDetailManage" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dxwgv" %>

<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>流程项管理</title>
    <link href="../Style/FontStyle.css" rel="stylesheet" type="text/css" media="screen"/>
    <link href="../Style/PopWindows.css" rel="stylesheet" type="text/css" media="screen"/>
    <link href="../Style/Page.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <table borderColor="#fffbec" cellSpacing="0" cellPadding="0" width="100%" align="center" bgColor="#fffbec" border="1" frame="void">
        <tbody>
        <tr align="left">
        <td height="20">你现在的位置是：系统管理->流程项管理</td></tr>
        </tbody>
    </table>
    <div>
    <dxwgv:ASPxGridView ID="girdConfDetail" runat="server" 
                        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" 
                        Width="100%" AutoGenerateColumns="False" DataSourceID="ObjectDataSource2" 
                        KeyFieldName="CONFIGDETAILID">
                        <SettingsBehavior ConfirmDelete="True" />
                        <Styles CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua">
                        </Styles>
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
                        <Images ImageFolder="~/App_Themes/Aqua/{0}/">
                            <CollapsedButton Height="15px" 
                                Url="~/App_Themes/Aqua/GridView/gvCollapsedButton.png" Width="15px" />
                            <ExpandedButton Height="15px" 
                                Url="~/App_Themes/Aqua/GridView/gvExpandedButton.png" Width="15px" />
                            <DetailCollapsedButton Height="15px" 
                                Url="~/App_Themes/Aqua/GridView/gvDetailCollapsedButton.png" Width="15px" />
                            <DetailExpandedButton Height="15px" 
                                Url="~/App_Themes/Aqua/GridView/gvDetailExpandedButton.png" Width="15px" />
                            <HeaderFilter Height="19px" Url="~/App_Themes/Aqua/GridView/gvHeaderFilter.png" 
                                Width="19px" />
                            <HeaderActiveFilter Height="19px" 
                                Url="~/App_Themes/Aqua/GridView/gvHeaderFilterActive.png" Width="19px" />
                            <HeaderSortDown Height="5px" 
                                Url="~/App_Themes/Aqua/GridView/gvHeaderSortDown.png" Width="7px" />
                            <HeaderSortUp Height="5px" Url="~/App_Themes/Aqua/GridView/gvHeaderSortUp.png" 
                                Width="7px" />
                            <FilterRowButton Height="13px" Width="13px" />
                            <WindowResizer Height="13px" Url="~/App_Themes/Aqua/GridView/WindowResizer.png" 
                                Width="13px" />
                        </Images>
                        <Columns>
                            <dxwgv:GridViewDataTextColumn Caption="流程项编码" FieldName="CONFIGDETAILID" 
                                VisibleIndex="0" ReadOnly="True">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataComboBoxColumn Caption="所属流程" FieldName="ITEMID" 
                                VisibleIndex="1">
                                <PropertiesComboBox DataSourceID="odsConfig" TextField="ITEMNAME" 
                                    ValueField="ITEMID" ValueType="System.String">
                                </PropertiesComboBox>
                            </dxwgv:GridViewDataComboBoxColumn>
                            <dxwgv:GridViewDataTextColumn Caption="流程项名称" FieldName="CONFIGDETAILNAME" 
                                VisibleIndex="2">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="标识" FieldName="CONFIGDETAILTAG" 
                                VisibleIndex="3">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataComboBoxColumn Caption="角色" FieldName="ROLEID" 
                                VisibleIndex="4">
                                <PropertiesComboBox DataSourceID="odsRole" TextField="ROLENAME" 
                                    ValueField="ROLEID" EnableSynchronization="False" 
                                    EnableIncrementalFiltering="True">
                                </PropertiesComboBox>
                            </dxwgv:GridViewDataComboBoxColumn>
                            <dxwgv:GridViewDataTextColumn Caption="描述" FieldName="ABOUT" 
                                VisibleIndex="5">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataDateColumn Caption="创建时间" FieldName="CREATETIME" 
                                VisibleIndex="6">
                                <PropertiesDateEdit DisplayFormatString="">
                                </PropertiesDateEdit>
                            </dxwgv:GridViewDataDateColumn>
                            <dxwgv:GridViewCommandColumn Caption="操作" VisibleIndex="7">
                                <EditButton Visible="True">
                                </EditButton>
                                <NewButton Visible="True">
                                </NewButton>
                                <DeleteButton Visible="True">
                                </DeleteButton>
                            </dxwgv:GridViewCommandColumn>
                        </Columns>
                        <StylesEditors>
                            <ProgressBar Height="25px">
                            </ProgressBar>
                        </StylesEditors>
                        <ImagesEditors>
                            <CalendarFastNavPrevYear Height="19px" 
                                Url="~/App_Themes/Aqua/Editors/edtCalendarFNPrevYear.png" Width="19px" />
                            <CalendarFastNavNextYear Height="19px" 
                                Url="~/App_Themes/Aqua/Editors/edtCalendarFNNextYear.png" Width="19px" />
                            <DropDownEditDropDown Height="7px" 
                                Url="~/App_Themes/Aqua/Editors/edtDropDown.png" 
                                UrlDisabled="~/App_Themes/Aqua/Editors/edtDropDownDisabled.png" 
                                UrlHottracked="~/App_Themes/Aqua/Editors/edtDropDownHottracked.png" 
                                Width="9px" />
                            <SpinEditIncrement Height="7px" 
                                Url="~/App_Themes/Aqua/Editors/edtSpinEditIncrementImage.png" 
                                UrlDisabled="~/App_Themes/Aqua/Editors/edtSpinEditIncrementDisabledImage.png" 
                                UrlHottracked="~/App_Themes/Aqua/Editors/edtSpinEditIncrementHottrackedImage.png" 
                                UrlPressed="~/App_Themes/Aqua/Editors/edtSpinEditIncrementHottrackedImage.png" 
                                Width="7px" />
                            <SpinEditDecrement Height="7px" 
                                Url="~/App_Themes/Aqua/Editors/edtSpinEditDecrementImage.png" 
                                UrlDisabled="~/App_Themes/Aqua/Editors/edtSpinEditDecrementDisabledImage.png" 
                                UrlHottracked="~/App_Themes/Aqua/Editors/edtSpinEditDecrementHottrackedImage.png" 
                                UrlPressed="~/App_Themes/Aqua/Editors/edtSpinEditDecrementHottrackedImage.png" 
                                Width="7px" />
                            <SpinEditLargeIncrement Height="9px" 
                                Url="~/App_Themes/Aqua/Editors/edtSpinEditLargeIncImage.png" 
                                UrlDisabled="~/App_Themes/Aqua/Editors/edtSpinEditLargeIncDisabledImage.png" 
                                UrlHottracked="~/App_Themes/Aqua/Editors/edtSpinEditLargeIncHottrackedImage.png" 
                                UrlPressed="~/App_Themes/Aqua/Editors/edtSpinEditLargeIncHottrackedImage.png" 
                                Width="7px" />
                            <SpinEditLargeDecrement Height="9px" 
                                Url="~/App_Themes/Aqua/Editors/edtSpinEditLargeDecImage.png" 
                                UrlDisabled="~/App_Themes/Aqua/Editors/edtSpinEditLargeDecDisabledImage.png" 
                                UrlHottracked="~/App_Themes/Aqua/Editors/edtSpinEditLargeDecHottrackedImage.png" 
                                UrlPressed="~/App_Themes/Aqua/Editors/edtSpinEditLargeDecHottrackedImage.png" 
                                Width="7px" />
                        </ImagesEditors>
                    </dxwgv:ASPxGridView>
    </div>
    <div>
    <asp:ObjectDataSource ID="odsConfig" runat="server" 
            DataObjectTypeName="GhtnTech.SecurityFramework.Model.SF_Configuration" 
            DeleteMethod="DeleteItem" InsertMethod="CreateItem" SelectMethod="GetItemList" 
            TypeName="GhtnTech.SecurityFramework.Configuration" UpdateMethod="UpdateItem">
            <SelectParameters>
                <asp:SessionParameter DefaultValue=" " Name="strWhere" 
                    SessionField="WhereConfig" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" 
            DataObjectTypeName="GhtnTech.SecurityFramework.Model.SF_ConfigDetail" 
            DeleteMethod="Delete" InsertMethod="Add" SelectMethod="GetList" 
            TypeName="GhtnTech.SecurityFramework.ConfigDetail" UpdateMethod="Update">
            <DeleteParameters>
                <asp:Parameter Name="CONFIGDETAILID" Type="Decimal" />
            </DeleteParameters>
            <SelectParameters>
                <asp:SessionParameter DefaultValue=" " Name="strWhere" 
                    SessionField="WhereConfDetail" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="odsRole" runat="server" SelectMethod="GetRoleList" 
            TypeName="GhtnTech.SecurityFramework.Role">
            <SelectParameters>
                <asp:Parameter DefaultValue="roleid != 31" Name="strWhere" Type="String" />
                <asp:Parameter DefaultValue=" " Name="strOrder" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
    </form>
</body>
</html>
