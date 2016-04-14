<%@ Page Language="C#" AutoEventWireup="true" CodeFile="YHChart.aspx.cs" Inherits="LeaderSearch_YHChart" %>

<%@ Register assembly="Coolite.Ext.Web" namespace="Coolite.Ext.Web" tagprefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
                    <ext:RecordField Name="Pass" />
                    <ext:RecordField Name="NPass" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    
    <ext:Store ID="UnitStore" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="DeptID">
                <Fields>
                    <ext:RecordField Name="Deptid" />
                    <ext:RecordField Name="Deptname" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    
    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                <North>
                    <ext:Panel ID="Panel1" runat="server" Height="55" Title="查询条件" Icon="Zoom">
                        <Body>
                            <table>
                                <tr>
                                    <td>
                                        <ext:Label ID="Label1" runat="server" Text="开始日期：">
                                        </ext:Label>
                                    </td>
                                    <td>
                                        <ext:DateField ID="dfBegin" runat="server">
                                        </ext:DateField>
                                    </td>
                                    <td>
                                        <ext:Label ID="Label2" runat="server" Text="截止日期：">
                                        </ext:Label>
                                    </td>
                                    <td>
                                        <ext:DateField ID="dfEnd" runat="server">
                                        </ext:DateField>
                                    </td>
                                    <td>
                                        <ext:Label ID="Label3" runat="server" Text="查询类型：">
                                        </ext:Label>
                                    </td>
                                    <td>
                                        <ext:ComboBox ID="cbbKind" runat="server" Editable="false" SelectedIndex="0">
                                            <Items>
                                                <ext:ListItem Text="隐患部门" Value="1" />
                                                <ext:ListItem Text="隐患地点" Value="2" />
                                            </Items>
                                        </ext:ComboBox>
                                    </td>
                                    <td>
                                        <ext:Label ID="Label4" runat="server" Text="单位：">
                                        </ext:Label>
                                    </td>
                                    <td>
                                        <ext:ComboBox 
                                            ID="cbbUnit" 
                                            runat="server" 
                                            EmptyText="请先选择单位.."
                                            DisplayField="Deptname" 
                                            ValueField="Deptid"
                                            StoreID="UnitStore"
                                            TypeAhead="true" 
                                            Mode="Local"
                                            ForceSelection="true" 
                                            TriggerAction="All"
                                            Width="100"
                                            Editable="false"
                                            ></ext:ComboBox>
                                    </td>
                                    <td>
                                        <ext:Button ID="btnSearch" runat="server" Text="查询" Icon="Zoom">
                                            <Listeners>
                                                <Click Handler="Coolite.AjaxMethods.LoadData();" />
                                            </Listeners>
                                        </ext:Button>
                                    </td>
                                </tr>
                            </table>
                        </Body>
                    </ext:Panel>
                </North>
                <South>
                    <ext:Panel ID="pnlChart" runat="server" Height="300" Title="隐患数据分析" Icon="ChartBar">
                    </ext:Panel>
                </South>
                <Center>
                    <ext:GridPanel 
                        ID="GridPanel1" 
                        runat="server"
                        StoreID="Store1"
                        StripeRows="true"
                        Title="隐患数据列表" 
                        AutoScroll="true"
                        Icon="Table"
                        >
                        <ColumnModel ID="ColumnModel1" runat="server">
		                    <Columns>
                                <ext:Column Header="隐患级别" Width="200" DataIndex="Key" />
                                <ext:Column Header="隐患数量" Width="100" DataIndex="Total" />
                                <ext:Column Header="已解决" Width="100" DataIndex="Pass" />
                                <ext:Column Header="未解决" Width="100" DataIndex="NPass" />
		                    </Columns>
                        </ColumnModel>
                        <LoadMask ShowMask="true" Msg="数据加载中..." />
                     </ext:GridPanel>
                </Center>
            </ext:BorderLayout>
        </Body>
    </ext:ViewPort>
    </form>
</body>
</html>
