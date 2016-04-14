<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OperationManage.aspx.cs" Inherits="SystemManage_OperationManage" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dxwgv" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>功能列表</title>
    <link href="../Style/FontStyle.css" rel="stylesheet" type="text/css" media="screen"/>
    <link href="../Style/PopWindows.css" rel="stylesheet" type="text/css" media="screen"/>
    <link href="../Style/Page.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <table borderColor="#fffbec" cellSpacing="0" cellPadding="0" width="100%" align="center" bgColor="#fffbec" border="1" frame="void">
        <tbody>
        <tr align="left">
        <td height="20">你现在的位置是：系统管理->功能管理</td></tr>
        </tbody>
    </table>
    <div>
        <dxwgv:ASPxGridView ID="gridOperator" runat="server" 
            AutoGenerateColumns="False" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" 
            CssPostfix="Aqua" DataSourceID="ObjectDataSource1" KeyFieldName="OPERATORID" 
            Width="100%">
            <SettingsBehavior ConfirmDelete="True" />
            <styles cssfilepath="~/App_Themes/Aqua/{0}/styles.css" csspostfix="Aqua">
            </styles>
            <SettingsLoadingPanel Text="" />
            <settingspager>
                <allbutton>
                    <Image Height="19px" Width="27px" />
                </allbutton>
                <firstpagebutton>
                    <Image Height="19px" Width="23px" />
                </firstpagebutton>
                <lastpagebutton>
                    <Image Height="19px" Width="23px" />
                </lastpagebutton>
                <nextpagebutton>
                    <Image Height="19px" Width="19px" />
                </nextpagebutton>
                <prevpagebutton>
                    <Image Height="19px" Width="19px" />
                </prevpagebutton>
            </settingspager>
            <images imagefolder="~/App_Themes/Aqua/{0}/">
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
            </images>
            <SettingsEditing Mode="Inline" />
            <Columns>
                <dxwgv:GridViewDataTextColumn Caption="功能编码" FieldName="OPERATORID" 
                    ReadOnly="True" VisibleIndex="0" Visible="False">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="功能名称" FieldName="OPERATORNAME" 
                    VisibleIndex="0">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="功能标志" FieldName="OPERATORTAG" 
                    VisibleIndex="1">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="功能描述" FieldName="ABOUT" VisibleIndex="2">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="排序" FieldName="OPERATORORDER" 
                    VisibleIndex="3">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="状态" FieldName="STATUS" Visible="False" 
                    VisibleIndex="5">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewCommandColumn Caption="操作" VisibleIndex="4">
                    <EditButton Visible="True">
                    </EditButton>
                    <NewButton Visible="True">
                    </NewButton>
                    <DeleteButton Visible="True">
                    </DeleteButton>
                </dxwgv:GridViewCommandColumn>
            </Columns>
            <styleseditors>
                <progressbar height="25px">
                </progressbar>
            </styleseditors>
            <imageseditors>
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
            </imageseditors>
        </dxwgv:ASPxGridView>
    </div>
    <div>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
            DataObjectTypeName="GhtnTech.SecurityFramework.Model.SF_Operator" 
            DeleteMethod="DeleteOperator" InsertMethod="CreateOperator" 
            SelectMethod="GetOperatorList" TypeName="GhtnTech.SecurityFramework.Operator" 
            UpdateMethod="UpdateOperator">
            <SelectParameters>
                <asp:SessionParameter DefaultValue=" " Name="strWhere" 
                    SessionField="WhereOperator" Type="String" />
                <asp:SessionParameter DefaultValue=" " Name="strOrder" 
                    SessionField="OrderOperator" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
    </form>
</body>
</html>
