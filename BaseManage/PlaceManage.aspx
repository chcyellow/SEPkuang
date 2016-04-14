<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlaceManage.aspx.cs" Inherits="BaseManage_PlaceManage" %>

<%@ Register Assembly="ALinq.Web, Version=1.0.8.0, Culture=neutral, PublicKeyToken=2b23f34316d38f3a"
    Namespace="ALinq.Web.Controls" TagPrefix="cc1" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dxwgv" %>

<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>

<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>
<%@ Register assembly="DevExpress.Web.ASPxGridView.v9.2" namespace="DevExpress.Web.ASPxGridView" tagprefix="dxwgv" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>地点编码维护</title>
</head>
<body>
    <form id="form1" runat="server">
    <fieldset id="Fieldset1" class="Fieldsetbox mbox pbox">
        <legend class="fsTitle">查询操作</legend>
        <table>
            <tr>
            <td>
            <table width="100%" border="0" cellpadding="0" cellspacing="5">
            <tr>
                <td style=" text-align:right;">
                    地点名称：</td>
                <td>
                    <asp:TextBox ID="txtPlace" runat="server" Width="100px"></asp:TextBox>
                    </td>
                <td style=" text-align:right;">
                    级别：</td>
                <td>
                    <asp:DropDownList ID="ddlLevel" runat="server" Width="100px" DataSourceID="adsPlaceLevel"
                     DataTextField="Plname" DataValueField="Plid">
                    </asp:DropDownList>
                    
                </td>
                <td style=" text-align:right;">
                    区域：
                </td>
                <td>
                    <asp:DropDownList ID="ddlArea" runat="server" DataSourceID="adsPlaceAreas" 
                        DataTextField="Pareasname" DataValueField="Pareasid" Width="120px">
                    </asp:DropDownList>
                    
                </td>
                <td style=" text-align:right;">
                    状态：
                </td>
                <td>
                    <asp:DropDownList ID="ddlStatus" runat="server" Width="120px">
                    <asp:ListItem Text="--全部--" Value="-1" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="启用" Value="1"></asp:ListItem>
                    <asp:ListItem Text="禁用" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                    
                </td>
            </tr>
        </table>
            </td>
            <td>
                <asp:Button ID="btnSearch" runat="server" Text="查询" onclick="btnSearch_Click" 
                    Height="40px" Width="40px" />
            </td>
   
            </tr>
        </table>
    </fieldset>
    <div style=" text-align:left">
        <dxwgv:ASPxGridView ID="gvPlace" runat="server" AutoGenerateColumns="False" 
            CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
            CssPostfix="Office2003_Blue" DataSourceID="adsPlace" 
             Width="100%" EnableViewState="False" KeyFieldName="Placeid" 
            onrowinserting="gvPlace_RowInserting"
            onpageindexchanged="gvPlace_PageIndexChanged">
            <Styles CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
                CssPostfix="Office2003_Blue">
                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                </Header>
                <LoadingPanel ImageSpacing="10px">
                </LoadingPanel>
            </Styles>
            <SettingsPager NumericButtonCount="20" PageSize="20">
            </SettingsPager>
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
                <dxwgv:GridViewDataTextColumn FieldName="Placeid" VisibleIndex="0" 
                    ReadOnly="True" Caption="地点ID(自动生成)" Width="5%">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="Placenumber" VisibleIndex="1" 
                    Caption="地点编码">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="Placename" VisibleIndex="2" 
                    Caption="地点名称">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataComboBoxColumn Caption="地点级别" FieldName="Plid" 
                    VisibleIndex="3">
                    <PropertiesComboBox ValueType="System.String" DataSourceID="adsPlaceLevel" 
                        TextField="Plname" ValueField="Plid">
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataComboBoxColumn Caption="地点区域" FieldName="Pareasid" 
                    VisibleIndex="3">
                    <PropertiesComboBox ValueType="System.String" DataSourceID="adsPlaceAreas" 
                        TextField="Pareasname" ValueField="Pareasid">
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataComboBoxColumn Caption="启用/禁用" FieldName="Placestatus" 
                    VisibleIndex="4">
                    <PropertiesComboBox ValueType="System.String">
                        <Items>
                            <dxe:ListEditItem Text="启用" Value="1"></dxe:ListEditItem>
                            <dxe:ListEditItem Text="禁用" Value="0"></dxe:ListEditItem>
                        </Items>
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewCommandColumn VisibleIndex="5" Caption="编辑">
                    <EditButton Visible="True">
                    </EditButton>
                    <NewButton Visible="True">
                    </NewButton>
                    <DeleteButton Visible="True">
                    </DeleteButton>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataTextColumn Caption="单位" FieldName="Maindeptid" 
                    Visible="False" VisibleIndex="6">
                </dxwgv:GridViewDataTextColumn>
                <%--<dxwgv:GridViewDataTextColumn Caption="是否可用" FieldName="Placestatus" 
                    Visible="False" VisibleIndex="7">
                </dxwgv:GridViewDataTextColumn>--%>
            </Columns>
            <Settings ShowGroupPanel="True" />
            <StylesEditors>
                <ProgressBar Height="25px">
                </ProgressBar>
            </StylesEditors>
        </dxwgv:ASPxGridView>
    </div>
    <div>
        <cc1:ALinqDataSource ID="adsPlace" runat="server" 
            ContextTypeName="GhtnTech.SEP.DAL.DBSCMDataContext" TableName="Place" 
            EnableViewState="False" EnableDelete="true" EnableInsert="true" 
            EnableUpdate="true" ondeleting="adsPlace_Deleting" >
            <SelectParameters>
                <asp:SessionParameter DefaultValue=" " Name="strWhere" SessionField="WhereUser" 
                    Type="String" />
                <asp:SessionParameter DefaultValue=" " Name="strOrder" SessionField="OrderUser" 
                    Type="String" />
            </SelectParameters>
        </cc1:ALinqDataSource>
        <cc1:ALinqDataSource ID="adsPlaceAreas" runat="server" 
            ContextTypeName="GhtnTech.SEP.DAL.DBSCMDataContext" TableName="Placeareas" EnableViewState="False" >
        </cc1:ALinqDataSource>
        <cc1:ALinqDataSource ID="adsPlaceLevel" runat="server" 
            ContextTypeName="GhtnTech.SEP.DAL.DBSCMDataContext" TableName="Placelevell">
        </cc1:ALinqDataSource>
    </div>
    </form>
</body>
</html>
