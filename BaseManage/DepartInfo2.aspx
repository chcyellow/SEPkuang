<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DepartInfo2.aspx.cs" Inherits="BaseManage_DepartInfo2" %>

<%@ Register Assembly="ALinq.Web, Version=1.0.8.0, Culture=neutral, PublicKeyToken=2b23f34316d38f3a"
    Namespace="ALinq.Web.Controls" TagPrefix="cc2" %>

<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>
<%@ Register assembly="DevExpress.Web.ASPxTreeList.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxTreeList" tagprefix="dxwtl" %>

<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>
<%@ Register assembly="DevExpress.Web.ASPxTreeList.v9.2" namespace="DevExpress.Web.ASPxTreeList" tagprefix="dxwtl" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <table borderColor="#fffbec" cellSpacing="0" cellPadding="0" width="100%" align="center" bgColor="#fffbec" border="1" frame="void">
        <tbody>
        <tr align="left">
        <td height="20">你现在的位置是：基础信息->部门管理</td></tr>
        </tbody>
    </table>
    <fieldset id="Fieldset1" class="Fieldsetbox mbox pbox">
        <legend class="fsTitle">功能操作</legend>
        <table style=" float:right;">
            <tr>
            <td>
                <asp:Button ID="btnEnable" runat="server" Text="启用" onclick="btnEnable_Click" />
            </td>
            <td>
                <asp:Button ID="btnDisable" runat="server" Text="锁定" 
                    onclick="btnDisable_Click" />
                </td>
       
            </tr>
        </table>
        </fieldset>
    <div style=" text-align:center">
    
        <dxwtl:ASPxTreeList ID="DepTreeList" runat="server" 
            AutoGenerateColumns="False" 
            CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
            CssPostfix="Office2003_Blue" Width="100%"  
            KeyFieldName="DEPTNUMBER" ParentFieldName="FATHERID" 
            onnodeupdating="DepTreeList_NodeUpdating">
            <Styles CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
                CssPostfix="Office2003_Blue">
            </Styles>

<SettingsBehavior AllowFocusedNode="True" AllowDragDrop="False"></SettingsBehavior>

<SettingsSelection Enabled="True" AllowSelectAll="True"></SettingsSelection>

            <Images ImageFolder="~/App_Themes/Office2003Blue/{0}/">
                <CollapsedButton Height="11px" 
                    Url="~/App_Themes/Office2003Blue/TreeList/CollapsedButton.png" Width="11px" />
                <ExpandedButton Height="11px" 
                    Url="~/App_Themes/Office2003Blue/TreeList/ExpandedButton.png" Width="11px" />
                <CustomizationWindowClose Height="12px" Width="13px" />
<CollapsedButton Height="11px" Width="11px" Url="~/App_Themes/Office2003Blue/TreeList/CollapsedButton.png"></CollapsedButton>

<ExpandedButton Height="11px" Width="11px" Url="~/App_Themes/Office2003Blue/TreeList/ExpandedButton.png"></ExpandedButton>

<CustomizationWindowClose Height="12px" Width="13px"></CustomizationWindowClose>
            </Images>
            <SettingsSelection AllowSelectAll="True" Enabled="True" />
            <SettingsBehavior AllowDragDrop="False" AllowFocusedNode="True" />
            <Columns>
                <dxwtl:TreeListTextColumn Caption="部门名称" VisibleIndex="0" FieldName="DEPTNAME" 
                    ReadOnly="True">
                    <CellStyle HorizontalAlign="Left">
                    </CellStyle>
                </dxwtl:TreeListTextColumn>
                <dxwtl:TreeListComboBoxColumn Caption="专业" FieldName="TYPEID" VisibleIndex="1">
                    <PropertiesComboBox DataSourceID="adsType" 
                        TextField="Infoname" ValueField="Infoid">
                    
<ItemStyle HorizontalAlign="Center"></ItemStyle>
</PropertiesComboBox>
                </dxwtl:TreeListComboBoxColumn>
                <dxwtl:TreeListTextColumn Caption="父编码" FieldName="FATHERID" Visible="False" 
                    VisibleIndex="1" ReadOnly="True">
                </dxwtl:TreeListTextColumn>
                <dxwtl:TreeListTextColumn Caption="部门编码" VisibleIndex="2" 
                    FieldName="DEPTNUMBER" ReadOnly="True">
                    <CellStyle HorizontalAlign="Left">
                    </CellStyle>
                </dxwtl:TreeListTextColumn>
                <dxwtl:TreeListComboBoxColumn Caption="状态" FieldName="DEPTSTATUS" 
                    VisibleIndex="3">
                    <PropertiesComboBox ValueType="System.String">
                        <Items>
                            
<dxe:ListEditItem Text="启用" Value="1" />
                            
<dxe:ListEditItem Text="锁定" Value="0" />
                        
</Items>
                    
</PropertiesComboBox>
                </dxwtl:TreeListComboBoxColumn>
                <dxwtl:TreeListCommandColumn Caption="操作" VisibleIndex="4">
                    <EditButton Visible="True">
                    </EditButton>
                </dxwtl:TreeListCommandColumn>
            </Columns>
        </dxwtl:ASPxTreeList>
    
    </div>
    <cc2:ALinqDataSource ID="adsType" runat="server" 
            ContextTypeName="GhtnTech.SEP.DAL.DBSCMDataContext" 
        TableName="CsBaseinfoset" 
        Where="Fid == @Fid" Select="new (Infoid, Infoname, Fid)">
        <WhereParameters>
            <asp:SessionParameter DefaultValue="1" Name="Fid" SessionField="FID" 
                Type="Decimal" />
        </WhereParameters>
    </cc2:ALinqDataSource>
    </form>
</body>
</html>
