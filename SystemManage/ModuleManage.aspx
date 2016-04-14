<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ModuleManage.aspx.cs" Inherits="SystemManage_ModuleManage" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dxwgv" %>

<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dxwtl" %>

<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>模块管理</title>
    <link href="../Style/FontStyle.css" rel="stylesheet" type="text/css" media="screen"/>
    <link href="../Style/PopWindows.css" rel="stylesheet" type="text/css" media="screen"/>
    <link href="../Style/Page.css" rel="stylesheet" type="text/css" /> 
    <script type="text/javascript">
        function PopSendingWin(e, id) {
            var para = "mid=" + escape(id);
        var url = "EditModule.aspx?" + para;
        window.showModalDialog(url,"","dialogHeight:680px;dialogWidth:700px;dialogLeft:200px;dialogTop:10px;scroll:auto");
    }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <%--页面标题--%>
    <table borderColor="#fffbec" cellSpacing="0" cellPadding="0" width="100%" align="center" bgColor="#fffbec" border="1" frame="void">
        <tbody>
        <tr align="left">
        <td height="20">你现在的位置是：系统管理->模块管理</td></tr>
        </tbody>
    </table>
	<div> 
		<fieldset style="width: 99%" align="center">
        <legend><font size="2">功能操作区</font></legend>
        <div>
            <table width="100%">
                <tr>
                    <td style="width:60%;"> 
                        <table style="border-collapse:collapse"  borderColor="#A0CEF8" cellspacing="0" width="100%" align="left" border="1" bgcolor="#eef3ff">
                            <tbody>
                                <tr>
                                    <td>
                                        <div align="right" style="font-size:12px;">模块分组:</div>
                                    </td>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboModuleGroup" runat="server" 
                                            DataSourceID="odsModuleGroup" TextField="MODULEGROUPNAME" 
                                            ValueField="MODULEGROUPID" ValueType="System.String">
                                        </dxe:ASPxComboBox>
                                    </td>
                                    <td>
                                        <div align="right" style="font-size:12px;">模块名称:</div>
                                    </td>
                                    <td>
                                        <dxe:ASPxTextBox ID="txtModuleName" runat="server" Width="124px" Height="18px">
                                        </dxe:ASPxTextBox>
                                    </td>
                                    <td>
                                        <div align="right" style="font-size:12px;">模块标志:</div>
                                    </td>
                                    <td>
                                        <dxe:ASPxTextBox ID="txtModuleTag" runat="server" Width="123px" Height="18px">
                                        </dxe:ASPxTextBox>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                    <td>
                        <dxe:ASPxButton ID="btnSearch" runat="server" Text="查询" Width="60px" 
                            onclick="btnSearch_Click">
                        </dxe:ASPxButton>
                    </td>
                    <td>
                        <dxe:ASPxButton ID="btnAddModule" runat="server" Text="添加模块" Width="100px" 
                            onclick="btnAddModule_Click">
                        </dxe:ASPxButton>
                    </td>
                </tr>
            </table>
        </div>
    </fieldset>
    <%--查询结果集--%>
    <fieldset style="width: 99%" align="center">
        <legend><font size="2">模块信息</font></legend>
        <div>
            <dxwgv:ASPxGridView ID="ASPxGridView1" runat="server" Width="99%" 
                CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" 
                AutoGenerateColumns="False" DataSourceID="odsModule" 
                KeyFieldName="MODULEID" 
                oncustomcolumndisplaytext="ASPxGridView1_CustomColumnDisplayText">
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
                    <dxwgv:GridViewDataTextColumn Caption="模块编码" FieldName="MODULEID" 
                        VisibleIndex="0" ReadOnly="True" Visible="False">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataComboBoxColumn Caption="模块组" FieldName="MODULEGROUPID" 
                        VisibleIndex="0">
                        <PropertiesComboBox DataSourceID="odsModuleGroup" TextField="MODULEGROUPNAME" 
                            ValueField="MODULEGROUPID" ValueType="System.String">
                        </PropertiesComboBox>
                    </dxwgv:GridViewDataComboBoxColumn>
                    <dxwgv:GridViewDataTextColumn Caption="模块名称" FieldName="MODULENAME" 
                        VisibleIndex="1">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="模块标志" FieldName="MODULETAG" 
                        VisibleIndex="2">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="链接地址" FieldName="MODULEURL" 
                        VisibleIndex="3">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="模块排序" FieldName="MODULEORDER" 
                        VisibleIndex="4">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="模块描述" FieldName="ABOUT" VisibleIndex="5">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="模块状态" FieldName="STATUS" 
                        VisibleIndex="6">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataHyperLinkColumn Caption="详情" VisibleIndex="7" 
                        Visible="False">
                        <DataItemTemplate>
                            <asp:HyperLink ID="lnkDetail" runat="server" Text="查看"></asp:HyperLink>
                        </DataItemTemplate>
                    </dxwgv:GridViewDataHyperLinkColumn>
                    <dxwgv:GridViewDataHyperLinkColumn Caption="编辑" VisibleIndex="7">
                        <DataItemTemplate>
                            <asp:HyperLink ID="lnkEdit" runat="server" Text="编辑" NavigateUrl='<%# Eval("MODULEID", "EditModule.aspx?mid={0}") %>'></asp:HyperLink>
                            <%--<a href="javascript:void(0);" onclick="PopSendingWin(event, <%# Eval("MODULEID") %>)">编辑</a>--%>
                        </DataItemTemplate>
                    </dxwgv:GridViewDataHyperLinkColumn>
                    <dxwgv:GridViewCommandColumn Caption="删除" VisibleIndex="8">
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
            <asp:ObjectDataSource ID="odsModuleGroup" runat="server" 
                SelectMethod="GetModuleGroupList" TypeName="GhtnTech.SecurityFramework.Module">
                <SelectParameters>
                    <asp:SessionParameter DefaultValue=" " Name="strWhere" 
                        SessionField="WhereModule" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:ObjectDataSource ID="odsModule" runat="server" 
                SelectMethod="GetModuleList" TypeName="GhtnTech.SecurityFramework.Module" 
                DataObjectTypeName="GhtnTech.SecurityFramework.Model.SF_Module" 
                DeleteMethod="DeleteModule">
                <SelectParameters>
                    <asp:SessionParameter DefaultValue=" " Name="strWhere" 
                        SessionField="WhereModule" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </fieldset>
	</div> 
    </form>
</body>
</html>
