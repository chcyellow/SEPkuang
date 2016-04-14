<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AllSummary.aspx.cs" Inherits="LeaderSearch_AllSummary" %>

<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style=" text-align:center">
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN" CleanResourceUrl="false" />
    <table width="100%">
        <tr>
            <td width="100%">
                <ext:Panel ID="Panel1" runat="server" Height="200" Title="年度安全状况图表">
                    <AutoLoad Url="TotalChart.aspx?height=150&width=700&year=2011" Mode="IFrame" ShowMask="true" MaskMsg="正在加载图表...." />
                </ext:Panel>
            </td>
        </tr>
        <tr>
            <td width="100%">
                <ext:Panel ID="Panel7" runat="server" Title="全矿单位安全状况" Height="300">
                     <AutoLoad Url="SearchByDept.aspx?head=0" Mode="IFrame" ShowMask="true" MaskMsg="正在加载数据...." />
                </ext:Panel>
            </td>
        </tr>
        <tr>
            <td width="100%">
                <ext:Panel ID="Panel3" runat="server" Height="250" Title="全矿采区安全状况" Visible="false">
                    <AutoLoad Url="SearchByPlace.aspx?head=0" Mode="IFrame" ShowMask="true" MaskMsg="正在加载数据...." />
                </ext:Panel>
            </td>
        </tr>
    </table>
    </div>
    </form>
</body>
</html>
