<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PositionManage.aspx.cs" Inherits="BaseManage_PositionManage" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dxwgv" %>

<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>

<%@ Register assembly="ALinq.Web, Version=1.0.8.0, Culture=neutral, PublicKeyToken=2b23f34316d38f3a" namespace="ALinq.Web.Controls" tagprefix="cc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style=" text-align:center">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <dxwgv:ASPxGridView ID="GridViewPos" ClientInstanceName="GridView" runat="server" 
        AutoGenerateColumns="False" DataSourceID="ALinqDataSource1" KeyFieldName="Posid" 
        Width="100%" CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
        CssPostfix="Office2003_Blue" onrowinserting="GridViewPos_RowInserting">
         <Columns>
             <dxwgv:GridViewDataTextColumn Caption="职务ID" FieldName="Posid" Visible="False" 
                 VisibleIndex="0">
             </dxwgv:GridViewDataTextColumn>
             <dxwgv:GridViewDataTextColumn FieldName="Posnumber" VisibleIndex="0" 
                 Width="20%" Caption="职务编码">
                <EditFormSettings VisibleIndex="0" Caption="职务编码" CaptionLocation="Near" />
             </dxwgv:GridViewDataTextColumn>
             <dxwgv:GridViewDataTextColumn FieldName="Posname" VisibleIndex="1" Width="30%" 
                 Caption="职务名称">
                <EditFormSettings VisibleIndex="1" Caption="职务名称" CaptionLocation="Near" />
             </dxwgv:GridViewDataTextColumn>
             <dxwgv:GridViewDataComboBoxColumn FieldName="Movegblevel" VisibleIndex="2" 
                 Width="30%" Caption="职务级别">
                <PropertiesComboBox ValueType="System.String">
                    <Items>
                        <dxe:ListEditItem Text="矿领导" Value="矿领导"></dxe:ListEditItem>
                        <dxe:ListEditItem Text="中层领导" Value="中层领导"></dxe:ListEditItem>
                        <dxe:ListEditItem Text="一般管技" Value="一般管技"></dxe:ListEditItem>
                        <dxe:ListEditItem Text="其他" Value="其他"></dxe:ListEditItem>
                    </Items>
                </PropertiesComboBox>
             </dxwgv:GridViewDataComboBoxColumn>
             <dxwgv:GridViewCommandColumn VisibleIndex="2" Caption="操作">
                <EditButton Visible="True" />
                <NewButton Visible="True" />
                <DeleteButton Visible="true" />
                <ClearFilterButton Visible="True" />
            </dxwgv:GridViewCommandColumn>
             <dxwgv:GridViewDataTextColumn Caption="单位" FieldName="Maindeptid" 
                 Visible="False" VisibleIndex="3">
             </dxwgv:GridViewDataTextColumn>
         </Columns>
         <TotalSummary>
             <dxwgv:ASPxSummaryItem FieldName="PosName" SummaryType="Count" DisplayFormat="累计信息数：{0}条" />
         </TotalSummary>
         <Settings ShowFilterRow="True" ShowFooter="True"/>
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
         <SettingsEditing EditFormColumnCount="4" Mode="Inline" 
             PopupEditFormWidth="450px" />
         <SettingsPager Mode="ShowPager">
         </SettingsPager>
         <Settings ShowTitlePanel="true" />
         <SettingsText Title="职务管理" />
         <Styles CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
            CssPostfix="Office2003_Blue">
             <LoadingPanel ImageSpacing="10px">
             </LoadingPanel>
             <Header ImageSpacing="5px" SortingImageSpacing="5px">
             </Header>
         </Styles>
         <StylesEditors>
             <ProgressBar Height="25px">
             </ProgressBar>
        </StylesEditors>
     </dxwgv:ASPxGridView>
        <cc2:ALinqDataSource ID="ALinqDataSource1" runat="server" 
            ContextTypeName="GhtnTech.SEP.DAL.DBSCMDataContext" TableName="Position" EnableDelete="True" EnableInsert="True" EnableUpdate="True" EnableViewState="False">
        </cc2:ALinqDataSource>
</div>
    </form>
</body>
</html>
