<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TablePrint.aspx.cs" Inherits="PAR_TablePrint" %>

<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/Table.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function CloseWin() {
            var ua = navigator.userAgent
            var ie = navigator.appName == "Microsoft Internet Explorer" ? true : false
            if (ie) {
                var IEversion = parseFloat(ua.substring(ua.indexOf("MSIE ") + 5, ua.indexOf(";", ua.indexOf("MSIE "))))
                if (IEversion < 5.5) {
                    var str = '<object id=noTipClose classid="clsid:ADB880A6-D8FF-11CF-9377-00AA003B7A11">'
                    str += '<param name="Command" value="Close"></object>';
                    document.body.insertAdjacentHTML("beforeEnd", str);
                    document.all.noTipClose.Click();
                }
                else {
                    var str = '<object id="WebBrowser" width=0 height=0 classid="CLSID:8856F961-340A-11D0-A96B-00C04FD705A2"></object>'
                    document.body.insertAdjacentHTML("beforeEnd", str);
                    document.all.WebBrowser.ExecWB(45, 1);
                }
            }
            else {
                window.close()
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN" CleanResourceUrl="false" />
    <ext:Panel ID="Panel1" runat="server" Border="false">
    </ext:Panel>
    </form>
</body>
</html>