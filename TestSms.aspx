<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestSms.aspx.cs" Inherits="TestSms" %>

<%@ Register Assembly="DevExpress.Web.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxLoadingPanel" TagPrefix="dxlp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .Panel {
            border: Dashed 1px Gray;
            height: 125px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div> 
        <asp:Label ID="Label1" runat="server" Text="电话："></asp:Label><asp:TextBox ID="txtPhone" runat="server"></asp:TextBox>
    </div>
    <div>
        <asp:Label ID="Label2" runat="server" Text="短信内容："></asp:Label>
        <asp:TextBox ID="txtSms"
            runat="server" Height="57px" TextMode="MultiLine" Width="257px"></asp:TextBox>
    </div>
    <div>
        <asp:Button ID="btnSend" runat="server" Text="发送" onclick="btnSend_Click" 
            style="height: 26px" onclientclick="LoadingPanel.Show();" />
        <asp:Button ID="btnSendAll" runat="server" onclick="btnSendAll_Click" 
            Text="群发短信" />
    </div>
    <div>
    <dxlp:ASPxLoadingPanel ID="LoadingPanel" runat="server" 
        ClientInstanceName="LoadingPanel" Modal="True" 
        CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css" CssPostfix="BlackGlass" 
        ImageFolder="~/App_Themes/BlackGlass/{0}/" Text="发送中&amp;hellip;">
    </dxlp:ASPxLoadingPanel>
    </div>
    </form>
</body>
</html>
