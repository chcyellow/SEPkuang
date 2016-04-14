<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PopInitPwd.aspx.cs" Inherits="SystemManage_PopInitPwd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>初始化密码</title>
    <base target="_self"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table align="center">
    <tr>
    <td align="right">用户名：</td><td>
        <asp:Label ID="lblUser" runat="server" Text=""></asp:Label></td>
    </tr>
    <tr>
    <td align="right">姓名：</td><td>
        <asp:Label ID="lblName" runat="server" Text=""></asp:Label></td>
    </tr>
    <tr>
    <td align="right">密码：</td><td>
        <asp:TextBox ID="txtPwd" runat="server" Width="100px"></asp:TextBox></td>
    </tr>
    <tr><td>
        <asp:Button ID="btnSure" runat="server" Text="确定" onclick="btnSure_Click" /></td><td>
            <asp:Button ID="btnCancel" runat="server" Text="取消" 
                onclientclick="window.close();" /></td></tr>
    </table>
    </div>
    </form>
</body>
</html>
