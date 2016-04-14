<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2,Version=9.2.6.0, Culture=neutral, PublicKeyToken=B88D1754D700E49A"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server"> 
    <title>淮北矿业集团安全生产体系支撑平台</title>
    <script type="text/javascript"> 
        if (window != top) 
            top.location.href = location.href; 
    </script>
</head>
<body style="text-align:center; MARGIN-RIGHT: auto; MARGIN-LEFT: auto;">
<form runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div align="center">
        <div style="width:1006px; height:606px;background-image:Url(Images/login.jpg);" align="center">
            <div style="width:150px; height:30px; position: relative; left: 275px; top: 313px;">
                <dxe:ASPxTextBox ID="txtUserName" runat="server" Width="170px">
                </dxe:ASPxTextBox>
            </div>
            <div style="width:150px; height:30px; position: relative; left: 275px; top: 323px;">
                <dxe:ASPxTextBox ID="txtPwd" runat="server" Width="170px" Password="True">
                </dxe:ASPxTextBox>
            </div>
            <div style="width:150px; height:30px; position: relative; left: 300px; top: 335px;">
                <asp:ImageButton ID="ibtnLogin" runat="server" 
                ImageUrl="~/Images/btn_login.gif" 
                onclick="ibtnLogin_Click"/>
            </div>
        </div>
    </div>

    </ContentTemplate>
</asp:UpdatePanel>

</form>
</body>
</html>
