<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Main.aspx.cs" Inherits="Main" %>

<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
     <link rel="stylesheet" type="text/css" href="css/List.css" />
     <link rel="stylesheet" type="text/css" href="css/main.css" />
   
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" CleanResourceUrl="false">
    </ext:ScriptManager>
    <div  id="floatTips" style="width: 99%">
    <table width="100%">
        <tr>
            <td width="50%">
                <ext:Panel ID="Portlet2" Title="待办事宜" runat="server" Height="500px" AutoScroll="true" />
            </td>
            <td width="50%">
                <ext:Panel ID="Portlet1" Title="最新动态" runat="server" Height="500px" AutoScroll="true" />
            </td>
        </tr>
    </table>
    </div>
    </form>
</body>
</html>
