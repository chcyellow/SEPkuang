<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProcessSet.aspx.cs" Inherits="HazardManage_ProcessSet" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<%@ Register assembly="DevExpress.Web.ASPxGridView.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dxwgv" %>
  
<%@ Register TagPrefix="dxrp" Namespace="DevExpress.Web.ASPxRoundPanel" Assembly="DevExpress.Web.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
 
<%@ Register Assembly="DevExpress.Web.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
    
<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>

<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dxwtl" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <table style="border-collapse:collapse" bordercolor="#A0CEF8" cellspacing="0" width="1020px" border="1">
        <tr>
            <td style="width:180px;">
                <fieldset style="width: 178px" align="center">
                    <legend><font size="3" color="#ff6600">辨识单元</font></legend>
                    <div>
                        <dxwtl:ASPxTreeList ID="treeList" runat="server"
                            ClientInstanceName="treeList" Width="100%"  KeyFieldName="INFOID" ParentFieldName="FID" 
                            Height="400px" AutoGenerateColumns="False" 
                            CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" 
                            >
                            <Columns>
                                <dxwtl:TreeListDataColumn FieldName="INFONAME" Caption="辨识单元" VisibleIndex="0" />    
                            </Columns>
                            <Styles CssFilePath="~/App_Themes/Aqua/{0}/styles.css" 
                                CssPostfix="Aqua">
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
                            <Settings ShowColumnHeaders="False" />
                            <SettingsBehavior AutoExpandAllNodes="true" AllowFocusedNode="True" ExpandCollapseAction="NodeDblClick" />
                            <ClientSideEvents 
                                FocusedNodeChanged="function(s, e) { 
                                var key = treeList.GetFocusedNodeKey();
                                CallbackPanel.PerformCallback(key);
                                CallbackPanel2.PerformCallback('k'); 
                             }" />
                        </dxwtl:ASPxTreeList>
                    </div>
                </fieldset>
            </td>
            <td style="width:30px;"><img alt="" src="../Images/jt.gif" style="width:30px; height: 21px" /></td>
            <td style="width:280px;">
                <fieldset style="width: 278px" align="center">
                    <legend><font size="3" color="#ff6600">工作任务</font></legend>
                    <div>
                        <dxcp:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel1" Height="400px" Width="100%" ClientInstanceName="CallbackPanel" oncallback="ASPxCallbackPanel1_Callback">
                            <PanelCollection>
                                <dxrp:PanelContent ID="PanelContent2" runat="server">
                                    <dxe:ASPxListBox ID="ASPxListBox1" runat="server" ValueField="WORKTASKID" ClientInstanceName="ListBox"
                                         TextField="WORKTASK" Width="100%" Height="400px" 
                                         CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                                         CssPostfix="Glass" ImageFolder="~/App_Themes/Glass/{0}/" >
                                         <ClientSideEvents 
                                            SelectedIndexChanged="function(s, e) {
                                            var item = ListBox.GetSelectedItem();
                                            CallbackPanel2.PerformCallback(item.value); }" />
                                         <ValidationSettings>
                                             <ErrorImage Height="14px" Url="~/App_Themes/Glass/Editors/edtError.png" 
                                                 Width="14px" />
                                             <ErrorFrameStyle ImageSpacing="4px">
                                                 <ErrorTextPaddings PaddingLeft="4px" />
                                             </ErrorFrameStyle>
                                         </ValidationSettings>
                                     </dxe:ASPxListBox>
                                </dxrp:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                    </div>
                </fieldset>
            </td>
            <td style="width:30px;"><img alt="" src="../Images/jt.gif" style="width:30px; height: 21px" /></td>
            <td style="width:500px;">
                <fieldset style="width: 495px" align="center">
                    <legend><font size="3" color="#ff6600">工序</font></legend>
                    <div>
                        <dxcp:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel2" Height="400px" Width="100%" ClientInstanceName="CallbackPanel2" oncallback="ASPxCallbackPanel2_Callback">
                            <PanelCollection>
                                <dxrp:PanelContent ID="PanelContent1" runat="server">
                                    <dxwgv:ASPxGridView ID="ASPxGridView2" runat="server" 
                                         AutoGenerateColumns="False" ClientInstanceName="GridView" 
                                         CssFilePath="~/App_Themes/Aqua/{0}/styles.css" 
                                         CssPostfix="Aqua" 
                                         KeyFieldName="PROCESSID"  Width="490px"
                                         OnCustomButtonCallback="ASPxGridView2_CustomButtonCallback" 
                                         DataSourceID="ObjectDataSource1" Visible="false">
                                        <Columns>
                                            <dxwgv:GridViewDataTextColumn Caption="工序名称" FieldName="NAME" VisibleIndex="0" Width="260px" >
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="编码" FieldName="ISOMUX" VisibleIndex="1" ReadOnly="true" >
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewCommandColumn Caption="操作" VisibleIndex="1" Width="150px">
                                                <EditButton Text="编辑" Visible="True">
                                                </EditButton>
                                                <NewButton Text="新增" Visible="True">
                                                </NewButton>
                                                <DeleteButton Text="删除" Visible="True">
                                                </DeleteButton>
                                                <CustomButtons>
                                                    <dxwgv:GridViewCommandColumnCustomButton ID="sy" Text="上移">
                                                    </dxwgv:GridViewCommandColumnCustomButton>
                                                    <dxwgv:GridViewCommandColumnCustomButton ID="xy" Text="下移">
                                                    </dxwgv:GridViewCommandColumnCustomButton>
                                                </CustomButtons>
                                            </dxwgv:GridViewCommandColumn>
                                        </Columns>
                                        <Settings ShowVerticalScrollBar="True" VerticalScrollableHeight="330" />
                                        <SettingsPager AlwaysShowPager="true" PageSize="12"></SettingsPager>
                                        <SettingsEditing Mode="Inline" />
                                        <SettingsBehavior ConfirmDelete="true" />
                                        <SettingsText ConfirmDelete="确认要删除吗？" />
                                        <Images ImageFolder="~/App_Themes/Aqua/{0}/">
                                            <CollapsedButton Height="15px" 
                                                Url="~/App_Themes/Aqua/GridView/gvCollapsedButton.png" Width="15px" />
                                            <ExpandedButton Height="15px" 
                                                Url="~/App_Themes/Aqua/GridView/gvExpandedButton.png" Width="15px" />
                                            <DetailCollapsedButton Height="15px" 
                                                Url="~/App_Themes/Aqua/GridView/gvDetailCollapsedButton.png" 
                                                Width="15px" />
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
                                        <Styles CssFilePath="~/App_Themes/Aqua/{0}/styles.css" 
                                            CssPostfix="Aqua">
                                        </Styles>
                                        <StylesEditors>
                                            <ProgressBar Height="25px">
                                            </ProgressBar>
                                        </StylesEditors>
                                    </dxwgv:ASPxGridView>
                                </dxrp:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                    </div>
                </fieldset>
            </td>
        </tr>
    </table>
    
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
        DataObjectTypeName="GhtnTech.SEP.Model.PROCESS" 
        InsertMethod="CreateDALPROCESS" SelectMethod="GetDALPROCESS" 
    UpdateMethod="UpdateDALPROCESS" DeleteMethod="DeleteDALPROCESS"
        TypeName="GhtnTech.SEP.OraclDAL.DALPROCESS" 
    >
        <SelectParameters>
            <asp:Parameter Name="strWhere" Type="String" DefaultValue=" " />
        </SelectParameters>
    </asp:ObjectDataSource>
    </form>
</body>
</html>
