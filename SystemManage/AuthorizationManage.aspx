<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AuthorizationManage.aspx.cs" Inherits="SystemManage_AuthorizationManage" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dxwgv" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>授权管理</title>
    <link href="../Style/FontStyle.css" rel="stylesheet" type="text/css" media="screen"/>
    <link href="../Style/PopWindows.css" rel="stylesheet" type="text/css" media="screen"/>
    <link href="../Style/Page.css" rel="stylesheet" type="text/css" /> 
</head>
<body>
    <form id="form1" runat="server">
    <table borderColor="#fffbec" cellSpacing="0" cellPadding="0" width="100%" align="center" bgColor="#fffbec" border="1" frame="void">
        <tbody>
        <tr align="left">
        <td height="20">你现在的位置是：系统管理->授权管理</td></tr>
        </tbody>
    </table>
    <fieldset id="Fieldset1" class="Fieldsetbox mbox pbox">
        <legend class="fsTitle">选择角色</legend>
        <table>
            <tr>
            <td>
               <dxe:ASPxComboBox ID="cboRole" runat="server" DataSourceID="odsRole" 
                    TextField="ROLENAME" ValueField="ROLEID" ValueType="System.String" 
                    AutoPostBack="True" onselectedindexchanged="cboRole_SelectedIndexChanged">
                </dxe:ASPxComboBox>
                <asp:Label ID="Rid" runat="server" Text="" Style="display: none;"></asp:Label>
               </td>
            </tr>
        </table>
    </fieldset>
    <div>
    模块分组：<asp:DropDownList ID="ddlModuleGroup" runat="server"
                                AutoPostBack="True" DataSourceID="odsModuleGroup" 
            DataTextField="MODULEGROUPNAME" DataValueField="MODULEGROUPID" 
            Width="120px" onselectedindexchanged="ddlModuleGroup_SelectedIndexChanged">
                </asp:DropDownList>
    </div>
    <div>
        <asp:GridView ID="gvModuleOperator" runat="server" DataKeyNames="ModuleID" CssClass="Grid"
                            AllowSorting="True" AutoGenerateColumns="False" OnRowDataBound="gvModuleOperator_RowDataBound"
                            
            OnSelectedIndexChanging="gvModuleOperator_SelectedIndexChanging" Width="100%" 
            CellPadding="4" ForeColor="#333333" 
            GridLines="None">
                            <FooterStyle CssClass="GridFooter" BackColor="#507CD1" Font-Bold="True" 
                                ForeColor="White" />
                            <RowStyle CssClass="Row" BackColor="#EFF3FB" />
                            <Columns>
                                <asp:TemplateField HeaderText="模块名称">
                                    <ItemTemplate>
                                        <asp:Label ID="lblModuleID" runat="server" Text='<%# Eval("MODULEID")%>' Style="display: none"></asp:Label>
                                        <asp:Label ID="lblModuleName" runat="server" Text='<%# Eval("MODULENAME") %>'></asp:Label>
                                        <asp:Label ID="lblVerify" runat="server" Text="" Style="display: none"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="False" Width="15%" />
                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="功能列表">
                                    <ItemTemplate>
                                        <asp:CheckBoxList ID="chkAuthorityList" runat="server" RepeatDirection="Horizontal"
                                            RepeatLayout="Flow" RepeatColumns="10">
                                        </asp:CheckBoxList>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="False" />
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText=" 更新 ">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnUpdate" runat="server" CommandArgument='<%# Eval("MODULEID")%>'
                                            CausesValidation="False" CommandName="Select" Text="更新"></asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="False" Width="5%" />
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle CssClass="HeadingCell" BackColor="#507CD1" Font-Bold="True" 
                                ForeColor="White" />
                            <EditRowStyle BackColor="#2461BF" />
                            <AlternatingRowStyle BorderStyle="None" CssClass="AlternatingRow" 
                                BackColor="White" />
                        </asp:GridView>
    </div>
    <div style=" text-align:right;">
    <asp:Button ID="btnSave" runat="server" Text="保存所有" onclick="btnSave_Click" />
    </div>
    <div>
        <asp:ObjectDataSource ID="odsRole" runat="server" SelectMethod="GetVRoleList" 
            TypeName="GhtnTech.SecurityFramework.Role">
            <SelectParameters>
                <asp:SessionParameter DefaultValue=" " Name="strWhere" SessionField="WhereRole" 
                    Type="String" />
                <asp:SessionParameter DefaultValue=" " Name="strOrder" SessionField="OrderRole" 
                    Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="odsModule" runat="server" 
            SelectMethod="GetModuleList" TypeName="GhtnTech.SecurityFramework.Module">
            <SelectParameters>
                <asp:SessionParameter DefaultValue=" " Name="strWhere" 
                    SessionField="WhereModule" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="odsModuleGroup" runat="server" 
            SelectMethod="GetModuleGroupList" TypeName="GhtnTech.SecurityFramework.Module">
            <SelectParameters>
                <asp:SessionParameter DefaultValue=" " Name="strWhere" 
                    SessionField="WhereModuleGroup" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="odsOpt" runat="server"></asp:ObjectDataSource>
    </div>
    </form>
</body>
</html>
