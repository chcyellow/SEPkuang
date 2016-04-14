<%@ Page Language="C#" AutoEventWireup="true" CodeFile="YHPointSet.aspx.cs" Inherits="SafeCheckSet_YHPointSet" %>
<%@ Register Assembly="ALinq.Web, Version=1.0.8.0, Culture=neutral, PublicKeyToken=2b23f34316d38f3a"
    Namespace="ALinq.Web.Controls" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dxwgv" %>

<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>隐患积分设置</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <dxwgv:ASPxGridView ID="gvYHPointSet" runat="server" 
            CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
            CssPostfix="Office2003_Blue" Width="100%" AutoGenerateColumns="False" 
            DataSourceID="adsYHPointSet" KeyFieldName="Levelid;Deptnumber" 
            onrowinserting="gvYHPointSet_RowInserting" 
            onrowupdating="gvYHPointSet_RowUpdating">
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
                <dxwgv:GridViewDataComboBoxColumn Caption="隐患级别" FieldName="Levelid" 
                    VisibleIndex="0">
                    <PropertiesComboBox DataSourceID="adsYHLevel" TextField="Infoname" 
                        ValueField="Infoid" ValueType="System.String">
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataTextColumn Caption="单位" FieldName="Deptnumber" 
                    VisibleIndex="1" Visible="False">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="矿查积分" FieldName="Kcscore" 
                    VisibleIndex="1">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="自查积分" FieldName="Zcscore" 
                    VisibleIndex="2">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="复查积分" FieldName="Dwscore" 
                    VisibleIndex="3">
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
    <div>
    <cc1:ALinqDataSource ID="adsYHPointSet" runat="server" 
            ContextTypeName="GhtnTech.SEP.DAL.DBSCMDataContext" TableName="Yhscore" 
            EnableViewState="False" EnableDelete="true" EnableInsert="true" EnableUpdate="true">
        </cc1:ALinqDataSource>
        <cc1:ALinqDataSource ID="adsYHLevel" runat="server" 
            ContextTypeName="GhtnTech.SEP.DAL.DBSCMDataContext" TableName="CsBaseinfoset" 
            EnableViewState="False" Select="new (Infoid, Infoname, Fid)" 
            Where="Fid == @Fid">
            <WhereParameters>
                <asp:Parameter DefaultValue="41" Name="Fid" Type="Decimal" />
            </WhereParameters>
        </cc1:ALinqDataSource>
    </div>
    </form>
</body>
</html>
