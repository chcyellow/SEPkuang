<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TotalChart.aspx.cs" Inherits="LeaderSearch_TotalChart" %>

<%@ Register assembly="Coolite.Ext.Web" namespace="Coolite.Ext.Web" tagprefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script language="javascript" type="text/javascript" src="../FC/FusionCharts.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN" CleanResourceUrl="false" />
    <ext:Store ID="Store1" runat="server">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="Key" />
                    <ext:RecordField Name="Total" />
                    <ext:RecordField Name="Fine" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                <Center>
                    <ext:Panel ID="pnlChart" runat="server" Title="年度安全状况" Icon="ChartPie">
                        <TopBar>
                            <ext:Toolbar runat="server" ID="ddd">
                                <Items>
                                    <ext:Label ID="Label1" runat="server" Text="当前年份：">
                                    </ext:Label>
                                    <ext:ComboBox ID="cbbKind" runat="server" Editable="false" Disabled="true">
                                    </ext:ComboBox>
                                    <ext:ToolbarFill />
                                    <ext:HyperLink ID="HyperLink1" runat="server" Text="隐患明细图表" Icon="ChartBar" NavigateUrl="YHChart.aspx" />
                                    <ext:ToolbarSeparator />
                                    <ext:HyperLink ID="HyperLink2" runat="server" Text="三违明细图表" Icon="ChartBar" NavigateUrl="SWChart.aspx" />
                                    <ext:ToolbarSeparator />
                                    <ext:HyperLink ID="HyperLink3" runat="server" Text="走动明细图表" Icon="ChartBar" NavigateUrl="MPChart.aspx" />
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <Body>
                            <table style='width: 100%; height: 100%'>
                                <tr>
                                    <td width="50%">
                                        <ext:Panel ID="lblYH" runat="server" Header="false">
                                            <Listeners>
                                                <BeforeShow Fn="function(el) { el.setHeight(Ext.getBody().getViewSize().height - 20); }" />
                                            </Listeners>
                                        </ext:Panel>
                                    </td>
                                    <td width="50%">
                                        <ext:Panel ID="lblSW" runat="server" Header="false">
                                            <Listeners>
                                                <BeforeShow Fn="function(el) { el.setHeight(Ext.getBody().getViewSize().height - 20); }" />
                                            </Listeners>
                                        </ext:Panel>
                                    </td>
                                </tr>
                            </table>
                        </Body>
                    </ext:Panel>
                </Center>
            </ext:BorderLayout>
        </Body>
    </ext:ViewPort>
    </form>
</body>
</html>
