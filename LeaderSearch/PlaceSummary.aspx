<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlaceSummary.aspx.cs" Inherits="LeaderSearch_PlaceSummary" %>

<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style=" text-align:center">
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN" CleanResourceUrl="false">
    </ext:ScriptManager>
    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                <West Split="true">
                    <ext:TreePanel ID="tpPlace" runat="server" Width="150" Title="地点列表" AutoScroll="true">
                    </ext:TreePanel>
                </West>
                <Center>
                    <ext:Panel ID="pnlDetail" runat="server">
                        <AutoLoad Url="" Mode="IFrame" ShowMask="true" MaskMsg="正在加载数据..." />
                    </ext:Panel>
                </Center>
            </ext:BorderLayout>
        </Body>
    </ext:ViewPort>
    </div>
    </form>
</body>
</html>
