<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditUserGroup.aspx.cs" Inherits="SystemManage_EditUserGroup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="pragma" content="no-cache">   
<meta http-equiv="Cache-Control" content="no-cache, must-revalidate">   

    <title>修改用户组</title>
    <base target="_self">

</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table>
    <tr>
    <td style=" text-align:right;">修改用户组为：</td><td>
        <asp:DropDownList ID="ddlUserGroup" runat="server" 
            DataSourceID="ObjectDataSource1" DataTextField="USERGROUPNAME" 
            DataValueField="USERGROUPID">
        </asp:DropDownList>
    </td>
    </tr>
    <tr>
    <td style=" text-align:right;">
        <asp:Button ID="btnSure" runat="server" Text="确定" onclick="btnSure_Click" /></td>
    <td>
        <asp:Button ID="btnCancel" runat="server" Text="关闭" onclick="btnCancel_Click" /></td>
    </tr>
    </table>
    </div>
    <div>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
            SelectMethod="GetUserGroupList" TypeName="GhtnTech.SecurityFramework.UserGroup">
            <SelectParameters>
                <asp:SessionParameter DefaultValue=" " Name="strWhere" 
                    SessionField="WhereUsergroup" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
    </form>
</body>
</html>
