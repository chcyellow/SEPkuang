<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PostFSWSet.aspx.cs" Inherits="SafeCheckSet_PostFSWSet" %>
<%@ Register Assembly="ALinq.Web, Version=1.0.8.0, Culture=neutral, PublicKeyToken=2b23f34316d38f3a"
    Namespace="ALinq.Web.Controls" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dxwgv" %>

<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>职务带指标反三违设置</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <dxwgv:ASPxGridView ID="gvPFSWSet" runat="server" AutoGenerateColumns="False" 
            CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
            CssPostfix="Office2003_Blue" DataSourceID="adsPFSWSet" 
            KeyFieldName="Positionid;Swtypeid" Width="100%" 
            onrowinserting="gvPFSWSet_RowInserting">
            <SettingsBehavior ConfirmDelete="True" />
            <Styles CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
                CssPostfix="Office2003_Blue">
                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                </Header>
                <LoadingPanel ImageSpacing="10px">
                </LoadingPanel>
            </Styles>
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
            <SettingsEditing Mode="Inline" />
            <Columns>
                <dxwgv:GridViewDataComboBoxColumn Caption="职务" FieldName="Positionid" 
                    VisibleIndex="0">
                    <PropertiesComboBox DataSourceID="adsPosition" TextField="Posname" 
                        ValueField="Posid" ValueType="System.String">
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataComboBoxColumn Caption="三违级别" FieldName="Swtypeid" 
                    VisibleIndex="1">
                    <PropertiesComboBox DataSourceID="adsSWLevel" TextField="Infoname" 
                        ValueField="Infoid" ValueType="System.String">
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataTextColumn Caption="月数" FieldName="Month" VisibleIndex="2">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="个数" FieldName="Count" VisibleIndex="3">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="单位" FieldName="Maindeptid" 
                    VisibleIndex="4" Visible="False">
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
            <StylesEditors>
                <ProgressBar Height="25px">
                </ProgressBar>
            </StylesEditors>
        </dxwgv:ASPxGridView>
    </div>
    <div>
    <cc1:ALinqDataSource ID="adsPFSWSet" runat="server" 
            ContextTypeName="GhtnTech.SEP.DAL.DBSCMDataContext" TableName="Positionfswset" 
            EnableViewState="False" EnableDelete="true" EnableInsert="true" EnableUpdate="true">
        </cc1:ALinqDataSource>
        <cc1:ALinqDataSource ID="adsSWLevel" runat="server" 
            ContextTypeName="GhtnTech.SEP.DAL.DBSCMDataContext" TableName="CsBaseinfoset" 
            EnableViewState="False" Select="new (Infoid, Infoname, Fid)" 
            Where="Fid == @Fid" >
            <WhereParameters>
                <asp:Parameter DefaultValue="46" Name="Fid" Type="Decimal" />
            </WhereParameters>
        </cc1:ALinqDataSource>
        <cc1:ALinqDataSource ID="adsPosition" runat="server" 
            ContextTypeName="GhtnTech.SEP.DAL.DBSCMDataContext" TableName="Position" 
            EnableViewState="False" >
        </cc1:ALinqDataSource>
    </div>
    </form>
</body>
</html>
