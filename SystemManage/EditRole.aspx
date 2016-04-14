<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditRole.aspx.cs" Inherits="SystemManage_EditRole" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="pragma" content="no-cache">   
<meta http-equiv="Cache-Control" content="no-cache, must-revalidate">   
    <title>修改角色</title>
    <base target="_self">
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblRole" runat="server" Text="无用户信息传过来"></asp:Label>
    </div>
    <asp:Panel ID="pnlRole" runat="server">
                    <table width="500" border="0" cellpadding="0" cellspacing="1" bgcolor="#CCCCCC">
                <tr>
                    <td width="200" height="30" bgcolor="#CCCCCC">
                        <div align="center">
                            可选择的角色</div>
                    </td>
                    <td width="100" bgcolor="#CCCCCC">
                        <div align="center" style="color: #FFFFFF;">
                        </div>
                    </td>
                    <td width="200" bgcolor="#CCCCCC">
                        <div align="center">
                            已拥有的角色</div>
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF">
                        <div align="center" style="margin: 10px;">
                            <asp:ListBox ID="lstOldRole" runat="server" Style="width: 160px;" SelectionMode="Multiple"
                                Height="205px" AutoPostBack="True" 
                                onselectedindexchanged="lstOldRole_SelectedIndexChanged" 
                                DataSourceID="ObjectDataSource1" DataTextField="ROLENAME" 
                                DataValueField="ROLEID"></asp:ListBox>
                            <asp:TextBox ID="txtOldRole" runat="server" Style="display: none;"></asp:TextBox>
                            <asp:TextBox ID="txtTRole" runat="server" Style="display: none;"></asp:TextBox>
                        </div>
                    </td>
                    <td bgcolor="#FFFFFF">
                        <div align="center">
                            <asp:Button ID="btnAdd" runat="server" Text="增加>"  Width="50px" 
                                onclick="btnAdd_Click"/><br />
                            <br />
                            <asp:Button ID="btnRemove" runat="server" Text="<删除" Width="50px" 
                                onclick="btnRemove_Click"/> <br />
                            <br />
                            按下<b>Ctrl</b>键<br />
                            可以多选                         </div>
                    </td>
                    <td bgcolor="#FFFFFF">
                        <div align="center" style="margin: 10px;">
                            <asp:ListBox ID="lstSelectedRole" runat="server" Style="width: 160px;" SelectionMode="Multiple"
                                Height="205px" AutoPostBack="True" 
                                onselectedindexchanged="lstSelectedRole_SelectedIndexChanged"></asp:ListBox>
                        </div>
                    </td>
                </tr>
                <tr><td>
                    &nbsp;</td><td>
                    <asp:Button ID="btnSave" runat="server" Text="保存" onclick="btnSave_Click" />&nbsp;&nbsp;&nbsp; 
                        <asp:Button ID="btnClose" runat="server" Text="关闭" onclick="btnClose_Click" /></td><td></td></tr>
            </table>
                    </asp:Panel>
                    <div>
                        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
                            SelectMethod="GetRoleList" TypeName="GhtnTech.SecurityFramework.Role">
                            <SelectParameters>
                                <asp:SessionParameter DefaultValue=" " Name="strWhere" 
                                    SessionField="WhereRole" Type="String" />
                                <asp:Parameter DefaultValue=" " Name="strOrder" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </div>
    </form>
</body>
</html>
