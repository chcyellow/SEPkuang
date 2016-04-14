<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sep_search.aspx.cs" Inherits="Search_Sep_search" %>

<%@ Register Assembly="DevExpress.Web.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxHiddenField" TagPrefix="dxhf" %>

<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dxwtl" %>

<%@ Register Assembly="DevExpress.Web.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dxm" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dxtc" %>
<%@ Register Assembly="DevExpress.Web.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxClasses" tagprefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dxpc" %>
<%@ Register Assembly="DevExpress.Web.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dxrp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../Css/Table.css" rel="stylesheet" type="text/css" />
    <title></title>
    <script type="text/javascript">
        var postponedCallbackValue = null;
        function OnbtnBaseClick(s, e) {
            if (CallbackPanel.InCallback())
                postponedCallbackValue = "base";
            else
                CallbackPanel.PerformCallback("base");
        }

        function OnbtnQuickClick(s, e) {
            if (CallbackPanel.InCallback())
                postponedCallbackValue = "quick";
            else
                CallbackPanel.PerformCallback("quick");
        }

        function OnbtnCSClick(s, e) {
            if (CallbackPanel.InCallback())
                postponedCallbackValue = "cs";
            else
                CallbackPanel.PerformCallback("cs");
        }

        function OnEndCallback(s, e) {
            if (postponedCallbackValue != null) {
                CallbackPanel.PerformCallback(postponedCallbackValue);
                postponedCallbackValue = null;
            }

        }
        function OnMoreInfoClick(element, keyValue) {
            //            popup.ShowAtElement(element);
            popup.Show();
            cpPopup.PerformCallback(keyValue);
        } 
</script>
</head>
<body>
    <form id="form1" runat="server">
    <dxpc:ASPxPopupControl ID="popup" ClientInstanceName="popup" runat="server" 
        AllowDragging="True" PopupHorizontalAlign="OutsideLeft" HeaderText="危险源详情" 
        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" 
        ImageFolder="~/App_Themes/Aqua/{0}/">
        <ContentStyle VerticalAlign="Top">
        </ContentStyle>
        <SizeGripImage Height="12px" Width="12px" />
        <ContentCollection><dxpc:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
            <dxcp:ASPxCallbackPanel ID="cpPopup" ClientInstanceName="cpPopup" runat="server" Width="650px" OnCallback="cpPopup_Callback">
                <PanelCollection>
                    <dxp:PanelContent ID="PanelContent1" runat="server">
                        <asp:Literal ID="litText" runat="server" Text=""></asp:Literal>
                    </dxp:PanelContent>
                </PanelCollection>
            </dxcp:ASPxCallbackPanel>
        </dxpc:PopupControlContentControl></ContentCollection>
        <CloseButtonImage Height="16px" Width="17px" />
    </dxpc:ASPxPopupControl>
    <dxtc:ASPxPageControl SkinId="None" Width="100%" EnableViewState="false" 
        ID="ASPxPageControl2" runat="server" ActiveTabIndex="0" 
        EnableDefaultAppearance="False" TabSpacing="0px" 
        EnableHierarchyRecreation="True">
<%-- BeginRegion TabPages --%>
    <TabPages>
        <dxtc:TabPage Text="快速检索" Name="i">
            <ContentCollection><dxw:ContentControl runat="server">
                <table id="Table1" border="0" cellpadding="0" cellspacing="16" 
                    style="background-color: #FDFEFF; color: #333333; height: 50px; background-image: url(Images/Templates/ContentCenter.gif); background-repeat: repeat-x; background-position: left top;" 
                    width="100%">
                    <tr>
                        <td valign="top" width="20%">
                            <b>快速检索</b><br />
                            通过输入危险源关键字，迅速查找<br />
                        </td>
                        <td valign="top" width="15%">
                            <dxe:ASPxTextBox ID="tbQuick" runat="server" Width="170px" NullText="请输入检索词">
                            </dxe:ASPxTextBox>
                        </td>
                        <td valign="top" width="65%">
                            <dxe:ASPxButton ID="btnQuick" runat="server" Text="检索" AutoPostBack="false">
                                <ClientSideEvents Click="OnbtnQuickClick" />
                            </dxe:ASPxButton>
                        </td>
                    </tr>
                </table>
            </dxw:ContentControl></ContentCollection>
        </dxtc:TabPage>
        <dxtc:TabPage Text="标准检索" Name="ii">
            <ContentCollection><dxw:ContentControl runat="server">
                <table id="Table2" width="100%" border="0" cellpadding="0" cellspacing="3" style="background-color: #FDFEFF; color: #333333; height: 270px; background-image: url(Images/Templates/ContentCenter.gif); background-repeat: repeat-x; background-position: left top;">                                        
                    <tr>                                                
                        <td valign="top" width="20%">
                            <b>标准检索</b><br />
                            通过输入准确信息，精确查找<br />
                            <dxe:ASPxImage runat="server" ImageUrl="~/Search/Images/customer-3.gif" ID="ASPxImage1" Width="170" />
                        </td>
                        <td valign="top" width="80%">
                            <asp:Panel runat="server" GroupingText="检索范围控制条件" Width="100%">
                                <table width="100%">
                                    <tr>
                                        <td>
                                        <table>
                                        <tr>
                                            <td width="90px" align="right">发布时间：</td>
                                            <td>从</td>
                                            <td><dxe:ASPxDateEdit ID="deBegin" runat="server" Width="120px">
                                        </dxe:ASPxDateEdit></td>
                                            <td>到</td>
                                            <td><dxe:ASPxDateEdit ID="deEnd" runat="server" Width="120px">
                                        </dxe:ASPxDateEdit></td>
                                        <%--<td>
                                            <dxe:ASPxLabel ID="lblFW" runat="server" Text="区间：">
                                             </dxe:ASPxLabel>
                                        </td>
                                        <td>
                                            <dxe:ASPxComboBox ID="cbbFW" runat="server" Width="130px">
                                                <Items>
                                                    <dxe:ListEditItem Text="局级" Value="局级" />
                                                    <dxe:ListEditItem Text="矿级" Value="矿级" />
                                                </Items>
                                            </dxe:ASPxComboBox>
                                        </td>--%>
                                        <td>
                                            <dxe:ASPxLabel ID="lblDW" runat="server" Text="单位：">
                                             </dxe:ASPxLabel>
                                        </td>
                                        <td>
                                            <dxe:ASPxComboBox ID="cbbDW" runat="server" Width="130px" ValueField="DEPTNUMBER" TextField="DEPTNAME">
                                            </dxe:ASPxComboBox>
                                            <asp:ObjectDataSource ID="ObjectDataSource3" runat="server" 
                                                SelectMethod="GetDALDEPARTMENTtree" 
                                                TypeName="GhtnTech.SEP.OraclDAL.DALDEPARTMENTtree">
                                                <SelectParameters>
                                                    <asp:Parameter DefaultValue=" " Name="DEPTNUMBER" Type="String" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                        </td>
                                        </tr>
                                        </table>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td>
                                        <table>
                                        <tr>
                                             <td width="90px" align="right">
                                                 <dxe:ASPxLabel ID="lblZY" runat="server" Text="专业：">
                                                 </dxe:ASPxLabel>
                                                 </td>
                                            <td>
                                                <%--<dxe:ASPxComboBox ID="cbbZY" runat="server" Width="130px" ValueField="INFOCODE" TextField="INFONAME">
                                                </dxe:ASPxComboBox>--%>
                                                <dxhf:ASPxHiddenField ID="ASPxHiddenField1" runat="server" ClientInstanceName="hidden">
                                                </dxhf:ASPxHiddenField>
                                                <dxe:ASPxDropDownEdit ID="cbbZY" runat="server" Width="130px" ClientInstanceName="DropDownEdit" AllowUserInput="False">
                                                    <DropDownWindowStyle><Border BorderWidth="0px" /></DropDownWindowStyle>
                                                    <DropDownWindowTemplate>
                                                        <dxwtl:ASPxTreeList ID="ASPxTreeList1" runat="server" AutoGenerateColumns="False" 
                                                            KeyFieldName="INFOID" ParentFieldName="FID" ClientInstanceName="treeList" Width="190px"
                                                              Settings-GridLines="Vertical" OnCustomDataCallback="treeList_CustomDataCallback">
                                                            <Columns>
                                                                <dxwtl:TreeListDataColumn FieldName="INFOCODE" VisibleIndex="1" Caption="编码" Width="50px" /> 
                                                                <dxwtl:TreeListDataColumn FieldName="INFONAME" VisibleIndex="0" Caption="名称" Width="130px" />                        
                                                            </Columns>
                                                             <SettingsBehavior AllowFocusedNode="True" ExpandCollapseAction="NodeDblClick" />
                                                             <ClientSideEvents FocusedNodeChanged="function(s, e) {
                                                             s.Focus();
                                                             var key = treeList.GetFocusedNodeKey();
                                                             hidden.Set('ZYkey', key);
                                                             treeList.PerformCustomDataCallback(key); 
                                                             }" 
                                                             CustomDataCallback="function(s, e) { DropDownEdit.SetText(e.result);
                                                             DropDownEdit.HideDropDown(); }" 
                                                             />
                                                             <Border BorderStyle="Solid" />

                                                        </dxwtl:ASPxTreeList>
                                                    </DropDownWindowTemplate>
                                                </dxe:ASPxDropDownEdit>
                                                <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" SelectMethod="GetDALDEPARTMENTtree"
                                                    TypeName="GhtnTech.SEP.OraclDAL.DALCS_BaseGet">
                                                    <SelectParameters>
                                                        <asp:Parameter Name="ID" Type="String" DefaultValue=" " />
                                                        <asp:Parameter Name="strWhere" Type="String" DefaultValue="  " />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                            </td>
                                            <td width="90px" align="right"><dxe:ASPxLabel ID="lblFXkind" runat="server" Text="风险类型：">
                                                 </dxe:ASPxLabel></td>
                                            <td>
                                                <dxe:ASPxComboBox ID="cbbFXkind" runat="server" Width="105px" ValueField="INFOID" TextField="INFONAME">
                                                </dxe:ASPxComboBox>
                                                <asp:ObjectDataSource ID="ObjectDataSource4" runat="server" 
                                                    SelectMethod="GetDALDEPARTMENTtree" 
                                                    TypeName="GhtnTech.SEP.OraclDAL.DALCS_BaseGet">
                                                    <SelectParameters>
                                                        <asp:Parameter DefaultValue=" " Name="ID" Type="String" />
                                                        <asp:Parameter DefaultValue="  " Name="strWhere" Type="String" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                            </td>
                                            <td>
                                                <dxe:ASPxLabel ID="lblFXlevel" runat="server" Text="风险等级：">
                                                 </dxe:ASPxLabel>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox ID="cbbFXlevel" runat="server" Width="80px" ValueField="INFOID" TextField="INFONAME">
                                                </dxe:ASPxComboBox>
                                                <asp:ObjectDataSource ID="ObjectDataSource5" runat="server" 
                                                    SelectMethod="GetDALDEPARTMENTtree" 
                                                    TypeName="GhtnTech.SEP.OraclDAL.DALCS_BaseGet">
                                                    <SelectParameters>
                                                        <asp:Parameter DefaultValue=" " Name="ID" Type="String" />
                                                        <asp:Parameter DefaultValue="  " Name="strWhere" Type="String" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                            </td>
                                             <td>
                                                <dxe:ASPxLabel ID="lblSG" runat="server" Text="事故类型：">
                                                 </dxe:ASPxLabel>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox ID="cbbSG" runat="server" Width="80px" ValueField="INFOID" TextField="INFONAME">
                                                </dxe:ASPxComboBox>
                                                <asp:ObjectDataSource ID="ObjectDataSource6" runat="server" 
                                                    SelectMethod="GetDALDEPARTMENTtree" 
                                                    TypeName="GhtnTech.SEP.OraclDAL.DALCS_BaseGet">
                                                    <SelectParameters>
                                                        <asp:Parameter DefaultValue=" " Name="ID" Type="String" />
                                                        <asp:Parameter DefaultValue="  " Name="strWhere" Type="String" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                            </td>
                                        </tr>
                                        </table>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td>
                                        <table>
                                        <tr>
                                             <td width="90px" align="right"><dxe:ASPxLabel ID="lblGZ" runat="server" Text="工作任务：">
                                                 </dxe:ASPxLabel></td>
                                            <td>
                                                <dxe:ASPxTextBox ID="tbGZ" runat="server" Width="130px">
                                                </dxe:ASPxTextBox>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox ID="cbbGZway" runat="server" Width="56px">
                                                    <Items>
                                                        <dxe:ListEditItem Text="精确" Value="1" Selected="true" />
                                                        <dxe:ListEditItem Text="模糊" Value="0" />
                                                    </Items>
                                                </dxe:ASPxComboBox>
                                            </td>
                                            <td><dxe:ASPxLabel ID="lblGX" runat="server" Text="工序：">
                                                 </dxe:ASPxLabel></td>
                                            <td>
                                                <dxe:ASPxTextBox ID="tbGX" runat="server" Width="130px">
                                                </dxe:ASPxTextBox>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox ID="cbbGXway" runat="server" Width="56px">
                                                    <Items>
                                                        <dxe:ListEditItem Text="精确" Value="1" Selected="true" />
                                                        <dxe:ListEditItem Text="模糊" Value="0" />
                                                    </Items>
                                                </dxe:ASPxComboBox>
                                            </td>
                                        </tr>
                                        </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="Panel2" runat="server" GroupingText="危险源内容特征" Width="100%">
                                <table width="100%">
                                     <tr>
                                        <td>
                                        <table>
                                        <tr>
                                            <td width="56px"></td>
                                            <td>(</td>
                                             <td width="100px" align="right">编码：</td>
                                            <td>
                                                <dxe:ASPxTextBox ID="tbBM" runat="server" Width="130px">
                                                </dxe:ASPxTextBox>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox ID="cbbBMchild" runat="server" Width="56px">
                                                    <Items>
                                                        <dxe:ListEditItem Text="并含" Value="1" Selected="true" />
                                                        <dxe:ListEditItem Text="或含" Value="0" />
                                                    </Items>
                                                </dxe:ASPxComboBox>
                                            </td>
                                            <td>
                                            <dxe:ASPxTextBox ID="tbBMchild" runat="server" Width="130px">
                                                </dxe:ASPxTextBox>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox ID="cbbBMway" runat="server" Width="56px">
                                                    <Items>
                                                        <dxe:ListEditItem Text="精确" Value="1" Selected="true" />
                                                        <dxe:ListEditItem Text="模糊" Value="0" />
                                                    </Items>
                                                </dxe:ASPxComboBox>
                                            </td>
                                        </tr>
                                        </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        <table>
                                        <tr>
                                            <td>
                                                <dxe:ASPxComboBox ID="cbbGX1" runat="server" Width="56px">
                                                    <Items>
                                                        <dxe:ListEditItem Text="并且" Value="1" Selected="true" />
                                                        <dxe:ListEditItem Text="异或" Value="0" />
                                                    </Items>
                                                </dxe:ASPxComboBox>
                                            </td>
                                             <td>(</td>
                                             <td width="100px" align="right">危险源：</td>
                                            <td>
                                                <dxe:ASPxTextBox ID="tbWXY" runat="server" Width="130px">
                                                </dxe:ASPxTextBox>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox ID="cbbWXYchild" runat="server" Width="56px">
                                                    <Items>
                                                        <dxe:ListEditItem Text="并含" Value="1" Selected="true" />
                                                        <dxe:ListEditItem Text="或含" Value="0" />
                                                    </Items>
                                                </dxe:ASPxComboBox>
                                            </td>
                                            <td>
                                            <dxe:ASPxTextBox ID="tbWXYchild" runat="server" Width="130px">
                                                </dxe:ASPxTextBox>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox ID="cbbWXYway" runat="server" Width="56px">
                                                    <Items>
                                                        <dxe:ListEditItem Text="精确" Value="1" Selected="true" />
                                                        <dxe:ListEditItem Text="模糊" Value="0" />
                                                    </Items>
                                                </dxe:ASPxComboBox>
                                            </td>
                                        </tr>
                                        </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        <table>
                                        <tr>
                                            <td>
                                                <dxe:ASPxComboBox ID="cbbGX2" runat="server" Width="56px">
                                                    <Items>
                                                        <dxe:ListEditItem Text="并且" Value="1" Selected="true" />
                                                        <dxe:ListEditItem Text="异或" Value="0" />
                                                    </Items>
                                                </dxe:ASPxComboBox>
                                            </td>
                                            <td>(</td>
                                             <td width="100px" align="right">风险后果及描述：</td>
                                            <td>
                                                <dxe:ASPxTextBox ID="tbHG" runat="server" Width="130px">
                                                </dxe:ASPxTextBox>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox ID="cbbHGchild" runat="server" Width="56px">
                                                    <Items>
                                                        <dxe:ListEditItem Text="并含" Value="1" Selected="true" />
                                                        <dxe:ListEditItem Text="或含" Value="0" />
                                                    </Items>
                                                </dxe:ASPxComboBox>
                                            </td>
                                            <td>
                                            <dxe:ASPxTextBox ID="tbHGchild" runat="server" Width="130px">
                                                </dxe:ASPxTextBox>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox ID="cbbHGway" runat="server" Width="56px">
                                                    <Items>
                                                        <dxe:ListEditItem Text="精确" Value="1" Selected="true" />
                                                        <dxe:ListEditItem Text="模糊" Value="0" />
                                                    </Items>
                                                </dxe:ASPxComboBox>
                                            </td>
                                        </tr>
                                        </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="Panel7" runat="server" Width="100%">
                                <table Width="100%">
                                    <tr>
                                        <td align="right" Width="100%">
                                             <dxe:ASPxButton ID="btnBaseSearch" runat="server" Text="检索危险源" AutoPostBack="false" > 
                                                 <ClientSideEvents Click="OnbtnBaseClick" />
                                             </dxe:ASPxButton>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </dxw:ContentControl></ContentCollection>
        </dxtc:TabPage>
        <dxtc:TabPage Text="管理标准与管理措施检索" Name="v">
            <ContentCollection><dxw:ContentControl runat="server">
                <table id="Table5" width="100%" border="0" cellpadding="0" cellspacing="16" style="background-color: #FDFEFF; color: #333333; height: 50px; background-image: url(Images/Templates/ContentCenter.gif); background-repeat: repeat-x; background-position: left top;">
                    <tr>
                        <td valign="top" width="20%">
                            <b>管理标准与管理措施检索</b><br />
                            通过输入管理标准或管理措施关键字，查找相关危险源<br />
                        </td>
                        <td valign="top" width="15%">
                            <dxe:ASPxTextBox ID="tbCSsearch" runat="server" Width="170px" NullText="请输入检索词">
                            </dxe:ASPxTextBox>
                        </td>
                        <td valign="top" width="65%">
                            <dxe:ASPxButton ID="btnCSsearch" runat="server" Text="检索" AutoPostBack="false" > 
                                <ClientSideEvents Click="OnbtnCSClick" />
                            </dxe:ASPxButton>
                        </td>
                    </tr>
                </table>
            </dxw:ContentControl></ContentCollection>
        </dxtc:TabPage>
    </TabPages>
<%-- EndRegion --%>
<%-- BeginRegion Templates --%>
    <TabTemplate>
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td><asp:Image ID="Image2" runat="server" GenerateEmptyAlternateText="true" ImageUrl="Images/Templates/UnSelectedLeft.gif"/></td>
                <td style="white-space: nowrap; background-image:url(Images/Templates/UnSelectedCenter.gif);"><dxe:ASPxLabel ID="Label1" runat="server" Text="<%# Container.TabPage.Text %>" Font-Names="Tahoma" ForeColor="#333333" Font-Size="8pt" /></td>
                <td><asp:Image ID="Image5" runat="server" GenerateEmptyAlternateText="true" ImageUrl="Images/Templates/UnSelectedRight.gif"/></td>
            </tr>
        </table>
    </TabTemplate>
    <ActiveTabTemplate>
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td><asp:Image ID="Image7" runat="server" GenerateEmptyAlternateText="true" ImageUrl="Images/Templates/SelectedLeft.gif"/></td>
                <td style="white-space: nowrap; background-repeat: repeat-x; background-image:url(Images/Templates/SelectedCenter.gif);"><dxe:ASPxLabel ID="Label1" runat="server" Text="<%# Container.TabPage.Text %>" Font-Names="Tahoma" ForeColor="#333333" Font-Size="8pt" /></td>
                <td><asp:Image ID="Image6" runat="server" GenerateEmptyAlternateText="true" ImageUrl="Images/Templates/SelectedRight.gif"/></td>
            </tr>
        </table>
    </ActiveTabTemplate>
<%-- EndRegion --%>
    <Paddings Padding="0px" PaddingLeft="12px" />
    <TabStyle>
        <Paddings Padding="0px" />
    </TabStyle>
    <ContentStyle Font-Names="Tahoma" Font-Overline="False" Font-Size="11px">
        <Paddings Padding="0px" />
        <Border BorderColor="#6DA0E7" BorderStyle="Solid" BorderWidth="1px" />
    </ContentStyle>
</dxtc:ASPxPageControl>
     <dxcp:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel1" Height="450px" Width="99%" ClientInstanceName="CallbackPanel" oncallback="ASPxCallbackPanel1_Callback">
                     <ClientSideEvents EndCallback="OnEndCallback"></ClientSideEvents>
                     <PanelCollection>
                     <dxrp:PanelContent runat="server">


                <dxwgv:ASPxGridView ID="ASPxGridView1" runat="server"  DataSourceID="ObjectDataSource1" KeyFieldName="HAZARDSID"
                    AutoGenerateColumns="False" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" 
                    CssPostfix="Aqua" Width="100%" 
                             OnHtmlDataCellPrepared="ASPxGridView1_HtmlDataCellPrepared">
                    <Styles CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua">
                    </Styles>
                    <Settings ShowVerticalScrollBar="True" VerticalScrollableHeight="450" />
                    <SettingsLoadingPanel Text="数据加载中，请稍候...." />
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
                        <dxwgv:GridViewDataTextColumn Caption="辨识单元" FieldName="ZYNAME"
                            VisibleIndex="0">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="工作任务" FieldName="GZRWNAME" Width="10%"
                            VisibleIndex="1">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="工序" FieldName="GXNAME" Width="10%"
                            VisibleIndex="2">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="危险源" FieldName="H_CONTENT" Width="15%"
                            VisibleIndex="3">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="风险类型" FieldName="FXLX"
                            VisibleIndex="4">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="风险后果及描述" FieldName="H_CONSEQUENCES" Width="30%"
                            VisibleIndex="5">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="风险等级" FieldName="FCLEVEL"
                            VisibleIndex="6">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="事故类型" FieldName="SGLX"
                            VisibleIndex="7">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="状态" FieldName="H_BJW"
                            VisibleIndex="8">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataColumn Caption="操作" VisibleIndex="9">
                             <DataItemTemplate>
                                 <a href="javascript:void(0);" onclick="OnMoreInfoClick(this, '<%# Container.KeyValue %>')">详情</a>
                             </DataItemTemplate>
                         </dxwgv:GridViewDataColumn>
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
                    </dxrp:PanelContent>
                    </PanelCollection>
                 </dxcp:ASPxCallbackPanel>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetDALHAZARDSall"
            TypeName="GhtnTech.SEP.OraclDAL.DALHAZARDSall">
            <SelectParameters>
                <asp:Parameter Name="strWhere" Type="String" DefaultValue=" and 1=0" />
                <asp:Parameter Name="strInner" Type="String" DefaultValue=" " />
            </SelectParameters>
        </asp:ObjectDataSource>

    </form>
</body>
</html>
