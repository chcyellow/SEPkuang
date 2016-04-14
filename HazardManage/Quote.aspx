<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Quote.aspx.cs" Inherits="HazardManage_Quote" %>

<%@ Register assembly="DevExpress.Web.ASPxGridView.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dxwgv" %>  <%@ Register TagPrefix="dxrp" Namespace="DevExpress.Web.ASPxRoundPanel" Assembly="DevExpress.Web.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %> <%@ Register Assembly="DevExpress.Web.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>    <%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %><%@ Register Assembly="DevExpress.Web.ASPxTreeList.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dxwtl" %>

<%@ Register Assembly="DevExpress.Web.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dxpc" %>
<%@ Register Assembly="DevExpress.Web.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" 
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dxp" %>

<%@ Register Assembly="DevExpress.Web.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxHiddenField" TagPrefix="dxhf" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function grid_SelectionChanged(s, e) {
            var values=s.GetSelectedFieldValues("HAZARDSID", GetSelectedFieldValuesCallback);
            Actioncallback.PerformCallback("Z");
            Infocallback.PerformCallback("Z");
        }
        function GetSelectedFieldValuesCallback(values) {
            if (values.length > 0) {
                    hf.Set('key', values[0]);
            }
            else {
                hf.Set('key', '-1');
            }
        } 

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <table class="clear lcol" width="100%" border="0" cellpadding="0" cellspacing="0">
	    <tr>
	        <td width="200px" id="MenuLeft" valign="top">
                <dxwtl:ASPxTreeList ID="treeList" runat="server"
                        ClientInstanceName="treeList" Width="100%"  KeyFieldName="INFOID" ParentFieldName="FID" 
                        AutoGenerateColumns="False" Height="550px" Border-BorderStyle="Solid"
                        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" 
                        >
                        <Columns>
                            <dxwtl:TreeListDataColumn FieldName="INFONAME" Caption="专业" VisibleIndex="0"  />    
                        </Columns>
                        <SettingsPager Mode="ShowPager" ShowNumericButtons="false" PageSize="22" />
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
                        <SettingsText LoadingPanelText="" />
                        <SettingsLoadingPanel Text="" />
                        <%--<Settings ShowColumnHeaders="False" />--%>
                        <SettingsBehavior AllowFocusedNode="True" ExpandCollapseAction="NodeDblClick" />
     
                        <ClientSideEvents 
                            FocusedNodeChanged="function(s, e) {
                            var key = treeList.GetFocusedNodeKey();
                            GVcallback.PerformCallback(key);
                            Actioncallback.PerformCallback(key);
                         }" />
                    </dxwtl:ASPxTreeList>
	        </td>
	        <td width="1px" vAlign="middle" onclick="switchSysBarLeft();"  
                id="switchPointLeft" title="关闭" 
                 background-repeat: no-repeat">
                <img id="imgleftright" src="../Images/resultset_previous.png" style= "cursor:hand" />
	        </td>
	        <td valign="top">
	           <table width="100%">
	           <tr>
	           <td>
	           
	           <dxcp:ASPxCallbackPanel runat="server" ID="acpGridview" Width="100%" ClientInstanceName="GVcallback" oncallback="acpGridview_Callback">
                     <PanelCollection>
                     <dxrp:PanelContent ID="PanelContent3" runat="server">
                     
                          <dxwgv:ASPxGridView ID="agvDATA" runat="server"  
                              DataSourceID="ObjectDataSource1" KeyFieldName="HAZARDSID"
                            AutoGenerateColumns="False" Width="100%" 
                              CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" 
                              OnHtmlDataCellPrepared="agvDATA_HtmlDataCellPrepared">
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
                            <Settings ShowTitlePanel="True" />
                            <SettingsText Title="危险源信息" />
                              <Styles CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua">
                              </Styles>
                            <Columns>
                                <dxwgv:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="10px" />
                                <dxwgv:GridViewDataTextColumn Caption="危险源" FieldName="H_CONTENT" Width="20%" VisibleIndex="1" />
                                <dxwgv:GridViewDataTextColumn Caption="风险类型" FieldName="FXLX" VisibleIndex="2" />
                                <dxwgv:GridViewDataTextColumn Caption="风险后果及描述" FieldName="H_CONSEQUENCES" Width="40%" VisibleIndex="3" />
                                <dxwgv:GridViewDataTextColumn Caption="事故类型" FieldName="SGLX" VisibleIndex="4" />
                                <dxwgv:GridViewDataTextColumn Caption="状态" FieldName="ISPASS" VisibleIndex="5" />
                                <dxwgv:GridViewDataTextColumn Caption="组" FieldName="ISGROUP" VisibleIndex="6" />
                            </Columns>
                            <SettingsBehavior AllowMultiSelection="false" />
                              <SettingsLoadingPanel Text="" />
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
                              <StylesEditors>
                                  <ProgressBar Height="25px">
                                  </ProgressBar>
                              </StylesEditors>
                              <ClientSideEvents SelectionChanged="grid_SelectionChanged" />
                        </dxwgv:ASPxGridView> 
                                      
                    </dxrp:PanelContent>
                    </PanelCollection>
                 </dxcp:ASPxCallbackPanel>
                 
                 <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
                       SelectMethod="GetHAZARDSForUsingView" 
                       TypeName="GhtnTech.SEP.OraclDAL.BaseTableGet">
                    <SelectParameters>
                        <asp:Parameter Name="strWhere" Type="String" DefaultValue=" and 1=0" />
                        <asp:Parameter Name="DeptNumber" Type="String" DefaultValue="0" />
                    </SelectParameters>
                </asp:ObjectDataSource>
	           
	           </td>
	           </tr>
	           <tr>
	           <td>
	           
	           <dxhf:ASPxHiddenField ID="ASPxHiddenField1" runat="server" ClientInstanceName="hf">
    </dxhf:ASPxHiddenField>
    <dxcp:ASPxCallbackPanel runat="server" ID="acpAction" Width="100%" ClientInstanceName="Actioncallback" oncallback="acpAction_Callback">
                     <PanelCollection>
                     <dxrp:PanelContent ID="PanelContent2" runat="server">
                     <hr />
                     <table width="100%">
                     <tr><td>
                            <dxe:ASPxButton ID="btnwork" runat="server" Text="引用" 
                                CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" AutoPostBack="false"
                                >
                                <ClientSideEvents Click="function(s, e){ GVcallback.PerformCallback('update'); }"/>
                            </dxe:ASPxButton>
                        </td>
                        <td>
                        </td>
                     </tr>
                     </table>
                     <hr />
                     </dxrp:PanelContent>
                    </PanelCollection>
                 </dxcp:ASPxCallbackPanel>

	           </td></tr>
	           <tr>
	            <td>
	            <dxcp:ASPxCallbackPanel runat="server" ID="acpInfo" Width="100%" ClientInstanceName="Infocallback" oncallback="acpInfo_Callback">
                     <PanelCollection>
                     <dxrp:PanelContent ID="PanelContent1" runat="server">
                        
                        <table runat="server" id="basetable">
                            <tr>
                            <td>
                            <dxe:ASPxLabel ID="ASPxLabel11" runat="server" Text="危险源描述">
                            </dxe:ASPxLabel>
                        </td>
                        <td>
                            <dxe:ASPxMemo ID="ASPxMemo6" runat="server" Height="75px" Width="210px">
                            </dxe:ASPxMemo>
                        </td>
                        <td>
                            <dxe:ASPxLabel ID="ASPxLabel10" runat="server" Text="后果描述">
                            </dxe:ASPxLabel>
                        </td>
                        <td>
                            <dxe:ASPxMemo ID="ASPxMemo7" runat="server" Height="75px" Width="209px">
                            </dxe:ASPxMemo>
                        </td>
                        
                        <td>
                            </td>
                        <td>
                            </td>
                    </tr>
                    <tr>
                        <td>
                            <dxe:ASPxLabel ID="ASPxLabel4" runat="server" Text="事故类型">
                            </dxe:ASPxLabel>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="ASPxComboBox7" runat="server" Height="23px" TextField="INFONAME" ValueField="INFOID"
                                ValueType="System.String" Width="205px" 
                                CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
                                CssPostfix="Office2003_Blue" ImageFolder="~/App_Themes/Office2003Blue/{0}/">
                                <ButtonStyle Width="13px">
                                </ButtonStyle>
                            </dxe:ASPxComboBox>
                        </td>
                        <td>
                            <dxe:ASPxLabel ID="ASPxLabel7" runat="server" Text="风险类型">
                            </dxe:ASPxLabel>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="ASPxComboBox8" runat="server" Height="23px" TextField="INFONAME" ValueField="INFOID" ClientInstanceName="comb"
                                ValueType="System.String" Width="205px" 
                                CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
                                CssPostfix="Office2003_Blue" ImageFolder="~/App_Themes/Office2003Blue/{0}/">
                                <%--<clientsideevents selectedindexchanged="function(s, e) {
              var item = comb.GetSelectedItem();
              CallbackPanel4.PerformCallback(item.value); }" />--%>
                                <ButtonStyle Width="13px">
                                </ButtonStyle>
                            </dxe:ASPxComboBox>
                        </td>
                        <td>
                            
                        </td>
                        <td>
                            
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dxe:ASPxLabel ID="ASPxLabel8" runat="server" Text="风险等级">
                            </dxe:ASPxLabel>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="ASPxComboBox9" runat="server" Height="23px" TextField="INFONAME" ValueField="INFOID"
                                ValueType="System.String" Width="205px" 
                                CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
                                CssPostfix="Office2003_Blue" ImageFolder="~/App_Themes/Office2003Blue/{0}/">
                                <ButtonStyle Width="13px">
                                </ButtonStyle>
                            </dxe:ASPxComboBox>
                        </td>
                        <td>
                            <dxe:ASPxLabel ID="ASPxLabel9" runat="server" Text="直接责任人">
                            </dxe:ASPxLabel>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="ASPxComboBox13" runat="server" Height="23px"  TextField="NAME" ValueField="M_OBJECTID"
                                ValueType="System.String" Width="205px" 
                                CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
                                CssPostfix="Office2003_Blue" ImageFolder="~/App_Themes/Office2003Blue/{0}/">
                                <ButtonStyle Width="13px">
                                </ButtonStyle>
                            </dxe:ASPxComboBox>
                            </td>
                            <td>
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <dxe:ASPxLabel ID="ASPxLabel12" runat="server" Text="管理对象">
                            </dxe:ASPxLabel>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="ASPxComboBox10" runat="server" Height="23px" TextField="NAME" ValueField="M_OBJECTID"
                                 Width="205px" 
                                CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
                                CssPostfix="Office2003_Blue" ImageFolder="~/App_Themes/Office2003Blue/{0}/">
                                <ButtonStyle Width="13px">
                                </ButtonStyle>
                            </dxe:ASPxComboBox>
                            </td>
                        <td>
                            <dxe:ASPxLabel ID="ASPxLabel13" runat="server" Text="管理人员">
                            </dxe:ASPxLabel>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="ASPxComboBox11" runat="server" Height="23px" TextField="NAME" ValueField="M_OBJECTID"
                                ValueType="System.String" Width="205px" 
                                CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
                                CssPostfix="Office2003_Blue" ImageFolder="~/App_Themes/Office2003Blue/{0}/">
                                <ButtonStyle Width="13px">
                                </ButtonStyle>
                            </dxe:ASPxComboBox>
                            </td>
                        <td>
                            <dxe:ASPxLabel ID="ASPxLabel14" runat="server" Text="监督责任人">
                            </dxe:ASPxLabel>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="ASPxComboBox12" runat="server" Height="23px" TextField="NAME" ValueField="M_OBJECTID"
                                ValueType="System.String" Width="205px" 
                                CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
                                CssPostfix="Office2003_Blue" ImageFolder="~/App_Themes/Office2003Blue/{0}/">
                                <ButtonStyle Width="13px">
                                </ButtonStyle>
                            </dxe:ASPxComboBox>
                            </td>
                    </tr>
                    <tr>
                        <td>
                            <dxe:ASPxLabel ID="ASPxLabel17" runat="server" Text="管理标准">
                            </dxe:ASPxLabel>
                        </td>
                        <td>
                            <dxe:ASPxMemo ID="ASPxMemo3" runat="server" Height="97px" Width="210px">
                            </dxe:ASPxMemo>
                        </td>
                        <td>
                            <dxe:ASPxLabel ID="ASPxLabel16" runat="server" Text="管理措施">
                            </dxe:ASPxLabel>
                        </td>
                        <td>
                            <dxe:ASPxMemo ID="ASPxMemo4" runat="server" Height="97px" Width="207px">
                            </dxe:ASPxMemo>
                        </td>
                        <td>
                            <dxe:ASPxLabel ID="ASPxLabel15" runat="server" Text="监督措施">
                            </dxe:ASPxLabel>
                        </td>
                        <td>
                            <dxe:ASPxMemo ID="ASPxMemo5" runat="server" Height="97px" Width="208px">
                            </dxe:ASPxMemo>
                        </td>
                    </tr>
                </table>
                <hr />
                 
                </dxrp:PanelContent>
                    </PanelCollection>
                 </dxcp:ASPxCallbackPanel>
	            </td>
	           </tr>
	           </table>
	        </td>
	    </tr>
	</table>
    </form>
</body>
</html>
