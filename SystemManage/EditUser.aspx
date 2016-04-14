<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditUser.aspx.cs" Inherits="SystemManage_EditUser" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>编辑用户</title>
    <link href="../Style/Page.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 241px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Panel ID="TitlePanel" runat="server" CssClass="titlePadding">
        用户信息
    </asp:Panel>
    <div id="strinfo" runat="server" class="mbox pbox" visible="false">
    </div>
    <div class="gv">
        <table width="100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td class="style1">
                    <table border="0" cellpadding="0" cellspacing="5">
                        <tr>
                            <td class="leftLab">
                                用户名：
                            </td>
                            <td style="white-space: nowrap;">
                                <asp:TextBox ID="txtUserName" runat="server" class="inputbox" 
                                    Style="width: 150px;"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="leftLab">
                                所属分组：
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlUserGroup" runat="server" Style="width: 160px;">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="leftLab">
                                角色：
                            </td>
                            <td>
                                <asp:ListBox ID="lstRole" runat="server" 
                                    SelectionMode="Multiple" Width="120px"></asp:ListBox>
                                <asp:Button ID="btnEditRole" runat="server" Text="修改" 
                                    onclick="btnEditRole_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td class="leftLab">
                                个人编码：
                            </td>
                            <td>
                                <asp:TextBox ID="txtPsnNumber" runat="server" class="inputbox" 
                                    Style="width: 150px;" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="leftLab">
                                状态：
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlState" runat="server" Style="width: 160px;">
                                    <asp:ListItem Value="0">禁止登录</asp:ListItem>
                                    <asp:ListItem Value="1">允许登录</asp:ListItem>
                                </asp:DropDownList>                               
                            </td>
                        </tr>
                        <tr>
                            <td class="leftLab">
                                注册时间：
                            </td>
                            <td>
                                <asp:Label ID="lblRegTime" runat="server" Font-Bold="True" ForeColor="#FF6600"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            <asp:Label ID="uid" runat="server" Text="" style=" display:none;"></asp:Label>
                            </td>
                            <td><br />
                                <asp:Button ID="btnOK" CssClass="button" runat="server" Text="确认修改" 
                                    onclick="btnOK_Click" />
                                &nbsp;&nbsp;
                                <asp:Button ID="Button1" runat="server" CssClass="button" 
                                    onclick="Button1_Click" Text="返回" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
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
                                onselectedindexchanged="lstOldRole_SelectedIndexChanged"></asp:ListBox>
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
                        <asp:Button ID="btnClose" runat="server" Text="取消" onclick="btnClose_Click" /></td><td></td></tr>
            </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
