<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlaceAreas.aspx.cs" Inherits="BaseManage_PlaceAreas" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dxwgv" %>

<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>

<%@ Register assembly="ALinq.Web, Version=1.0.8.0, Culture=neutral, PublicKeyToken=2b23f34316d38f3a" namespace="ALinq.Web.Controls" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>地点区域维护</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style=" text-align:center">
        <dxwgv:ASPxGridView ID="gvPlace" runat="server" AutoGenerateColumns="False" 
            CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
            CssPostfix="Office2003_Blue" DataSourceID="ALinqDataSource1" 
             Width="100%" EnableViewState="False" KeyFieldName="Pareasid" 
            onrowinserting="gvPlace_RowInserting">
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
                <dxwgv:GridViewDataTextColumn FieldName="Pareasid" VisibleIndex="0" 
                    ReadOnly="True" Caption="地点区域ID(自动生成)" Width="5%">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="Pareasname" VisibleIndex="1" 
                    Caption="地点区域名称">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataComboBoxColumn Caption="状态" FieldName="Status" 
                    VisibleIndex="2">
                    <PropertiesComboBox ValueType="System.String">
                        <Items>
                            <dxe:ListEditItem Text="工作状态" Value="工作状态"></dxe:ListEditItem>
                            <dxe:ListEditItem Text="非工作状态" Value="非工作状态"></dxe:ListEditItem>
                        </Items>
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataTextColumn Caption="单位" FieldName="Maindeptid" 
                    Visible="False" VisibleIndex="3">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewCommandColumn VisibleIndex="3" Caption="编辑">
                    <EditButton Visible="True">
                    </EditButton>
                    <NewButton Visible="True">
                    </NewButton>
                    <DeleteButton Visible="True">
                    </DeleteButton>
                </dxwgv:GridViewCommandColumn>
            </Columns>
            <Settings ShowGroupPanel="True" />
            <StylesEditors>
                <ProgressBar Height="25px">
                </ProgressBar>
            </StylesEditors>
        </dxwgv:ASPxGridView>
    </div>
    <div>
        <cc1:ALinqDataSource ID="ALinqDataSource1" runat="server" 
            ContextTypeName="GhtnTech.SEP.DAL.DBSCMDataContext" TableName="Placeareas" EnableDelete="True" EnableInsert="True" EnableUpdate="True" EnableViewState="False">
        </cc1:ALinqDataSource>
    </div>
    </form>
</body>
</html>
