<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SWPointSet.aspx.cs" Inherits="SafeCheckSet_SWPointSet" %>
<%@ Register Assembly="ALinq.Web, Version=1.0.8.0, Culture=neutral, PublicKeyToken=2b23f34316d38f3a"
    Namespace="ALinq.Web.Controls" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dxwgv" %>

<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>三违积分设置</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="100%">
            <tr>
                <td width="65px">
                    <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="预警分值：">
                    </dxe:ASPxLabel>
                </td>
                <td width="105px">
                    <dxe:ASPxTextBox ID="txtScore" runat="server" Width="100px">
                    </dxe:ASPxTextBox>
                </td>
                <td width="65px">
                    <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="预警次数：">
                    </dxe:ASPxLabel>
                </td>
                <td width="105px">
                    <dxe:ASPxTextBox ID="txtCount" runat="server" Width="100px">
                    </dxe:ASPxTextBox>
                </td>
                <td>
                    <dxe:ASPxButton ID="btnSave" runat="server" Text="保存" onclick="btnSave_Click">
                    </dxe:ASPxButton>
                </td>
            </tr>
        </table>
        <dxwgv:ASPxGridView ID="gvSWPointSet" runat="server" 
            AutoGenerateColumns="False" 
            CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
            CssPostfix="Office2003_Blue" DataSourceID="adsSWPointSet" Width="100%" 
            KeyFieldName="Levelid;Deptnumber" 
            onrowinserting="gvSWPointSet_RowInserting" 
            onrowupdating="gvSWPointSet_RowUpdating">
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
                <dxwgv:GridViewDataTextColumn Caption="单位" FieldName="Deptnumber" 
                    VisibleIndex="1" Visible="False">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="矿查积分" FieldName="Kcscore" 
                    VisibleIndex="1">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="自查积分" FieldName="Zcscore" 
                    VisibleIndex="2">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="单位积分" FieldName="Dwscore" 
                    VisibleIndex="2" Visible="False">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="第二次三违积分" FieldName="Second" 
                    VisibleIndex="3">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="第三次三违积分" FieldName="Third" 
                    VisibleIndex="4">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataDateColumn Caption="录入时间" FieldName="Usingtime" 
                    VisibleIndex="6">
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
    <cc1:ALinqDataSource ID="adsSWPointSet" runat="server" 
            ContextTypeName="GhtnTech.SEP.DAL.DBSCMDataContext" TableName="Swscore" 
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
    </div>
    </form>
</body>
</html>
