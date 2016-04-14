<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SWFineSet.aspx.cs" Inherits="SafeCheckSet_SWFineSet" %>
<%@ Register Assembly="ALinq.Web, Version=1.0.8.0, Culture=neutral, PublicKeyToken=2b23f34316d38f3a"
    Namespace="ALinq.Web.Controls" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dxwgv" %>

<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>三违罚款设置</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <dxwgv:ASPxGridView ID="gvSWFineSet" runat="server" AutoGenerateColumns="False" 
            CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
            CssPostfix="Office2003_Blue" DataSourceID="adsSWFineSet" 
            KeyFieldName="Levelid;Pid" Width="100%" 
            onrowinserting="gvSWFineSet_RowInserting" 
            onrowupdating="gvSWFineSet_RowUpdating">
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
                <dxwgv:GridViewDataComboBoxColumn Caption="三违级别" FieldName="Levelid" 
                    VisibleIndex="0">
                    <PropertiesComboBox DataSourceID="adsSWLevel" TextField="Infoname" 
                        ValueField="Infoid" ValueType="System.String">
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataComboBoxColumn Caption="人员对象" FieldName="Pid" 
                    VisibleIndex="1">
                    <PropertiesComboBox DataSourceID="adsPersonType" TextField="Pname" 
                        ValueField="Pid" ValueType="System.String">
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataTextColumn Caption="矿查罚款" FieldName="Kcfine" 
                    VisibleIndex="2">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="自查罚款" FieldName="Zcfine" 
                    VisibleIndex="3">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="单位" FieldName="Deptnumber" 
                    Visible="False" VisibleIndex="4">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataDateColumn Caption="录入时间" FieldName="Usingtime" 
                    VisibleIndex="4">
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewCommandColumn Caption="操作" VisibleIndex="5">
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
    <cc1:ALinqDataSource ID="adsSWFineSet" runat="server" 
            ContextTypeName="GhtnTech.SEP.DAL.DBSCMDataContext" TableName="Swfineset" 
            EnableViewState="False" EnableDelete="true" EnableInsert="true" EnableUpdate="true">
        </cc1:ALinqDataSource>
        <cc1:ALinqDataSource ID="adsSWLevel" runat="server" 
            ContextTypeName="GhtnTech.SEP.DAL.DBSCMDataContext" TableName="CsBaseinfoset" 
            EnableViewState="False" Select="new (Infoid, Infoname)" 
            Where="Fid == @Fid">
            <whereparameters>
                <asp:Parameter DefaultValue="46" Name="Fid" Type="Decimal" />
            </whereparameters>
        </cc1:ALinqDataSource>
        <cc1:ALinqDataSource ID="adsPersonType" runat="server" 
            ContextTypeName="GhtnTech.SEP.DAL.DBSCMDataContext" TableName="Objectset" 
            EnableViewState="False">
        </cc1:ALinqDataSource>
    </form>
</body>
</html>
