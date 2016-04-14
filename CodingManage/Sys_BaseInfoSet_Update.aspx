<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sys_BaseInfoSet_Update.aspx.cs" Inherits="CodingManage_Sys_BaseInfoSet_Update" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dxwtl" %>
    
<%@ Register Assembly="DevExpress.Web.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
    
<%@ Register Assembly="DevExpress.Web.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dxp" %>
    
<%@ Register Assembly="DevExpress.Web.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dxrp" %>
    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <%--页面标题--%>
    <table borderColor="#fffbec" cellSpacing="0" cellPadding="0" width="100%" align="center" bgColor="#fffbec" border="1" frame="void">
        <tbody>
        <%--<tr align="center" valign="bottom">
            <td height="40" colspan="2">编码基础信息设置维护</td>
        </tr>--%>
        <tr>
            <td height="15" style="width:85%;"  align="right">
                <dxe:ASPxLabel ID="lbl_Depart" runat="server" Text="维护单位：">
                </dxe:ASPxLabel>
            </td>
            <td height="15" style="width:15%;" align="left">
                <dxe:ASPxLabel ID="lbl_DepartName" runat="server" Text="">
                </dxe:ASPxLabel>
            </td>
        </tr>
        </tbody>
    </table>
    
    <%--信息维护--%>
    <fieldset style="width: 99%" align="center">
        <legend><font size="2">维护区域</font></legend>
        <div>
            <dxcp:ASPxCallbackPanel ID="backPanel" runat="server" ClientInstanceName="CallbackPanel" OnCallback="backPanel_Callback" Width="100%" HideContentOnCallback="False">
                <PanelCollection>
                    <dxp:PanelContent ID="PanelContent1" runat="server">
                        <dxrp:ASPxRoundPanel runat="server" ID="RoundPanel" Width="100%" EnableTheming="False" ShowHeader="false" ShowDefaultImages="False">
                            <PanelCollection>
                                <dxp:PanelContent ID="PanelContent2" runat="server">
                                    <dxwtl:ASPxTreeList ID="InfoTree" runat="server" 
                                        DataSourceID="ObjectDataSource1" KeyFieldName="INFOID" ParentFieldName="FID"
                                        AutoGenerateColumns="False" Width="100%" 
                                        OnNodeValidating="InfoTree_NodeValidating" 
                                        OnHtmlDataCellPrepared ="InfoTree_HtmlDataCellPrepared" 
                                        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua">
                                        <SettingsPager AlwaysShowPager="True" Mode="ShowPager" PageSize="12">
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
                                        <SettingsText ConfirmDelete="您确认要删除/禁用吗?" LoadingPanelText=""  />
                                        <SettingsBehavior AllowFocusedNode="true" AutoExpandAllNodes="True" ExpandCollapseAction="NodeDblClick" AllowSort="false" AllowDragDrop="false" />
                                        <SettingsLoadingPanel Text="" />
                                        <SettingsEditing Mode="EditFormAndDisplayNode" EditFormColumnCount="3" />
                                        <Columns>
                                            <dxwtl:TreeListTextColumn FieldName="INFOID" Caption="自增主键" VisibleIndex="0"  Visible="false">
                                            </dxwtl:TreeListTextColumn>
                                            <dxwtl:TreeListTextColumn Caption="名称" VisibleIndex="1" FieldName="INFONAME">
                                            </dxwtl:TreeListTextColumn>
                                            <dxwtl:TreeListTextColumn Caption="编码" VisibleIndex="2" FieldName="INFOCODE">
                                            </dxwtl:TreeListTextColumn>
                                            <dxwtl:TreeListTextColumn Caption="父节点" FieldName="FID" Visible="False" 
                                                VisibleIndex="3">
                                            </dxwtl:TreeListTextColumn>
                                            <dxwtl:TreeListComboBoxColumn Caption="下属字段类型" FieldName="CODINGTYPE" 
                                                VisibleIndex="4">
                                                <PropertiesComboBox ValueType="System.String">
                                                    <Items>
                                                        <dxe:ListEditItem Selected="True" Text="字符型" Value="STRING" />
                                                        <dxe:ListEditItem Text="数值型" Value="NUMBERIC" />
                                                    </Items>
                                                </PropertiesComboBox>
                                            </dxwtl:TreeListComboBoxColumn>
                                            <dxwtl:TreeListTextColumn Caption="下属字段长度" VisibleIndex="5"
                                                FieldName="CODINGL">
                                            </dxwtl:TreeListTextColumn>
                                            <dxwtl:TreeListDateTimeColumn Caption="编制日期" VisibleIndex="6" FieldName="PDAY">
                                            </dxwtl:TreeListDateTimeColumn>
                                            <dxwtl:TreeListTextColumn FieldName="STATUS" VisibleIndex="7" Caption="状态" ReadOnly="true">
                                                <PropertiesTextEdit NullText="编辑"></PropertiesTextEdit>
                                            </dxwtl:TreeListTextColumn>
                                            <dxwtl:TreeListCommandColumn VisibleIndex="8" ShowNewButtonInHeader="True" Caption="操作">
                                                <EditButton Visible="true" Text="修改">
                                                </EditButton>
                                                <NewButton Visible="True" Text="新增">
                                                </NewButton>
                                                <DeleteButton Visible="True" Text="删除/禁用">
                                                </DeleteButton>
                                            </dxwtl:TreeListCommandColumn>
                                        </Columns>
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
        DataObjectTypeName="GhtnTech.SEP.Model.CS_BaseInfoSet" 
        DeleteMethod="DeleteBaseInfoSet" InsertMethod="CreateBaseInfoSet" 
        SelectMethod="GetBaseInfoSetList" 
        TypeName="GhtnTech.SEP.OraclDAL.DALCS_BaseInfoSet" 
        UpdateMethod="UpdateRulesTreeKind">
        <SelectParameters>
            <asp:SessionParameter DefaultValue=" " Name="strWhere" SessionField="strWhere" 
                Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <%--操作区域--%>
    <fieldset style="width: 99%" align="center">
        <legend><font size="2">操作区域</font></legend>
        <table width="100%" style="height:30px;">
            <tr>
                <td style="width:100px;" align="right">
                    <dxe:ASPxButton ID="btn_fabu" runat="server" Text="信息发布" AutoPostBack="False" 
                        ClientInstanceName="btn_fabu" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" 
                        CssPostfix="Aqua">
                        <ClientSideEvents Click="function(s,e){
                                if(confirm('您确认要进行信息发布吗？'))
                                {
                                    CallbackPanel.PerformCallback('fabu');
                                }
                                else
                                {
                                    alert('信息未发布！');
                                }
                            }"
                            />
                    </dxe:ASPxButton>
                </td>
                <td><div align="left" style="padding-top:5px;"><font size="2" color="red"><b>◆重要提示</b>：信息发布功能只能针对本单位下编辑状态的信息进行发布！</font></div></td>
            </tr>
        </table>
    </fieldset>
    <%--注意事项--%>
    <fieldset style="width: 99%" align="center">
        <legend><font size="2" color="#ff6600">友情提示</font></legend>
        <div align="left" style="padding-top:5px;"><font size="2"><br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;◆编码基础信息设置将以树的形态生成；集团公司及各基层单位只能对本单位的编码信息设置进行维护，其余操作均无效；</font></div>
        <div align="left" style="padding-top:5px;"><font size="2">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;◆对根节点新增、修改时，字段属性和长度是根据填写内容，系统自动默认；</font></div>
        <div align="left" style="padding-top:5px;"><font size="2">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;◆对子节点新增、修改时，填写的下属字段属性和长度是对本节点的所有第一级下属节点进行定义；</font></div>
        <div align="left" style="padding-top:5px;"><font size="2">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;◆维护完毕编码基础信息设置后，各单位可以对本单位的信息进行发布，方可正常使用！<br /></font></div>
    </fieldset>
    </form>
</body>
</html>
