<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sys_BaseInfoSet.aspx.cs" Inherits="CodingManage_Sys_BaseInfoSet" %>

<%@ Register Assembly="DevExpress.Web.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
    
<%@ Register Assembly="DevExpress.Web.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dxrp" %>

<%@ Register Assembly="DevExpress.Web.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dxp" %>

<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dxwtl" %>

<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="background-color:#eef3ff;">
    <form id="form1" runat="server">
    
    <%--页面标题--%>
    <%--<table borderColor="#fffbec" cellSpacing="0" cellPadding="0" width="100%" align="center" bgColor="#fffbec" border="1" frame="void">
        <tbody>
        <tr align="center">
        <td height="40">编码基础信息设置查询</td></tr>
        </tbody>
    </table>--%>

    <%--查询模块--%>
    <fieldset style="width: 99%" align="center">
        <legend><font size="2">查询条件</font></legend>
        <div>
            <table width="480px">
                <tr>
                    <td valign="middle" style="width:30px;">
                        <dxe:ASPxImage ID="ASPxImage1" runat="server" 
                            ImageUrl="~/Images/Sel.jpg" Height="50px">
                        </dxe:ASPxImage>
                    </td>
                    <td style="width:450px;"> 
                        <table style="border-collapse:collapse"  borderColor="#A0CEF8" cellspacing="0" width="520px" align="left" border="1" bgcolor="#eef3ff">
                            <tbody>
                                <tr>
                                    <td width="75px">
                                        <div align="right" style="font-size:12px;">属性名称:</div>
                                    </td>
                                    <%--<td width="150px">--%>
                                    <td colspan="3">
                                        <div align="left">
                                            <dxe:ASPxTextBox ID="tbNodeName" runat="server" Width="140px">
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </td>
                                    <%--<td width="75px">
                                        <div align="right" style="font-size:12px;">编制单位:</div>
                                    </td>
                                    <td width="150px">
                                        <div align="left">
                                            <dxe:ASPxComboBox ID="cbbPDepart" runat="server" Width="140px">
                                            </dxe:ASPxComboBox>
                                        </div>
                                    </td>--%>
                                </tr><tr>
                                    <td width="75px">
                                        <div align="right" style="font-size:12px;">编制日期:</div>
                                    </td>
                                    <td width="150px">
                                        <div align="left">
                                            <dxe:ASPxDateEdit ID="deDay" runat="server" Width="140px">
                                            </dxe:ASPxDateEdit>
                                        </div>
                                    </td>
                                    <td width="75px">
                                        <div align="right" style="font-size:12px;">状态:</div>
                                    </td>
                                    <td width="150px">
                                        <div align="left">
                                            <dxe:ASPxComboBox ID="cbbStatus" runat="server" Width="140px" 
                                                ValueType="System.String">
                                                <Items>
                                                    <dxe:ListEditItem Text="编辑" Value="编辑" />
                                                    <dxe:ListEditItem Text="启用" Value="启用" />
                                                    <dxe:ListEditItem Text="禁用" Value="禁用" />
                                                </Items>
                                            </dxe:ASPxComboBox>
                                        </div>
                                    </td>
                               </tr>
                            </tbody>
                        </table>
                    </td>
                    <td valign="bottom">
                        <dxe:ASPxButton ID="btnSel" ClientInstanceName="btnSel" runat="server" 
                            Text="检索信息设置" Width="100px" AutoPostBack="False" 
                            CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua">
                            <ClientSideEvents Click="function(s,e){
                                CallbackPanel.PerformCallback('Sel');
                            }"
                            />    
                        </dxe:ASPxButton>
                    </td>
                </tr>
            </table>
        </div>
    </fieldset>
    
    <%--查询结果集--%>
    <fieldset style="width: 99%" align="center">
        <legend><font size="2">查询结果集</font></legend>
        <div>
            <dxcp:ASPxCallbackPanel ID="backPanel" runat="server" ClientInstanceName="CallbackPanel" OnCallback="backPanel_Callback" Width="100%" HideContentOnCallback="False">
                <PanelCollection>
                    <dxp:PanelContent runat="server">
                        <dxrp:ASPxRoundPanel runat="server" ID="RoundPanel" Width="100%" EnableTheming="False" ShowHeader="false" ShowDefaultImages="False">
                            <PanelCollection>
                                <dxp:PanelContent ID="PanelContent1" runat="server">
                                    <dxwtl:ASPxTreeList ID="InfoTree" runat="server" 
                                        DataSourceID="ObjectDataSource1" KeyFieldName="INFOID" ParentFieldName="FID"
                                        AutoGenerateColumns="False" Width="100%" 
                                        OnHtmlDataCellPrepared ="InfoTree_HtmlDataCellPrepared" 
                                        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua">
                                        <SettingsPager AlwaysShowPager="True" Mode="ShowPager" PageSize="15">
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
                                        <Settings GridLines="Vertical" />
                                        <%--<Settings ShowColumnHeaders="False" />--%>

                                        <SettingsBehavior AllowFocusedNode="true" AutoExpandAllNodes="True" ExpandCollapseAction="NodeDblClick" AllowSort="false" AllowDragDrop="false" />
                                        <Columns>
                                            <dxwtl:TreeListTextColumn Caption="名称" VisibleIndex="0" FieldName="INFONAME">
                                            </dxwtl:TreeListTextColumn>
                                            <dxwtl:TreeListTextColumn Caption="编码" VisibleIndex="1" FieldName="INFOCODE">
                                            </dxwtl:TreeListTextColumn>
                                            <dxwtl:TreeListTextColumn Caption="编制单位" VisibleIndex="2" FieldName="DEPTNAME">
                                            </dxwtl:TreeListTextColumn>
                                            <dxwtl:TreeListTextColumn Caption="编制人员" VisibleIndex="3" FieldName="NAME">
                                            </dxwtl:TreeListTextColumn>
                                            <dxwtl:TreeListDateTimeColumn Caption="编制日期" VisibleIndex="4" FieldName="PDAY">
                                            </dxwtl:TreeListDateTimeColumn>
                                            <dxwtl:TreeListTextColumn Caption="启用人员" VisibleIndex="5" FieldName="PPNAME">
                                            </dxwtl:TreeListTextColumn>
                                            <dxwtl:TreeListDateTimeColumn Caption="启用日期" VisibleIndex="6" FieldName="SDAY">
                                            </dxwtl:TreeListDateTimeColumn>
                                            <dxwtl:TreeListTextColumn Caption="状态" VisibleIndex="7" FieldName="STATUS">
                                            </dxwtl:TreeListTextColumn>
                                        </Columns>
                                        <SettingsLoadingPanel Text="" />
                                        <SettingsText LoadingPanelText="" />
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
                                        <Styles CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua">
                                            <CustomizationWindowContent VerticalAlign="Top">
                                            </CustomizationWindowContent>
                                        </Styles>
                                    </dxwtl:ASPxTreeList>
                                </dxp:PanelContent>
                            </PanelCollection>
                        </dxrp:ASPxRoundPanel>
                    </dxp:PanelContent>
                </PanelCollection>
            </dxcp:ASPxCallbackPanel>
        </div>
    </fieldset>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
        SelectMethod="GetBaseInfoSetList" 
        TypeName="GhtnTech.SEP.OraclDAL.DALCS_BaseInfoSet">
        <SelectParameters>
            <asp:SessionParameter Name="strWhere" SessionField="strWhere" Type="String" 
                DefaultValue=" " />
        </SelectParameters>
    </asp:ObjectDataSource>
    <%--友情提示--%>
    <fieldset style="width: 99%" align="center">
        <legend><font size="2" color="#ff6600">友情提示</font></legend>
        <div align="left" style="padding-top:5px;"><font size="2"><br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;◆选择编制日期时，可以查询编制日期当天及之前的信息集合！<br /></font></div>
    </fieldset>
    </form>
</body>
</html>
