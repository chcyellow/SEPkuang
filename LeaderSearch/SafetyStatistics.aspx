<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SafetyStatistics.aspx.cs" Inherits="LeaderSearch_SafetyStatistics" %>

<%@ Register assembly="Coolite.Ext.Web" namespace="Coolite.Ext.Web" tagprefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script language="javascript" type="text/javascript">
        var saveData = function () {
            GridData.setValue(Ext.encode(GridPanel1.getRowsValues(false)));
        }

        var template = '<span style="color:{0};cursor: pointer;">{1}</span>';

        var change = function (value) {
            return String.format(template, 'blue', value);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN" CleanResourceUrl="false" />
    <ext:Store ID="Store1" runat="server" OnRefreshData="MyData_Refresh" AutoLoad="false" >
    <Proxy>
        <ext:DataSourceProxy>
        </ext:DataSourceProxy>
         </Proxy>
        <Reader>
            <ext:JsonReader ReaderID="PERSONNUMBER">
                <Fields>
                    <ext:RecordField Name="MAINDEPT" />
                    <ext:RecordField Name="DEPTNAME" />
                    <ext:RecordField Name="NAME" />
                    <ext:RecordField Name="POSNAME" />
                    <ext:RecordField Name="MOVEGBLEVEL" />
                    <ext:RecordField Name="XJ" />
                    <ext:RecordField Name="YH" />
                    <ext:RecordField Name="YXC" />
                    <ext:RecordField Name="FXC" />
                    <ext:RecordField Name="YBH" />
                    <ext:RecordField Name="WBH" />
                    <ext:RecordField Name="SW" />
                    <ext:RecordField Name="LOGINCOUNT" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    
    <ext:Store ID="KQStore" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="Deptnumber">
                <Fields>
                    <ext:RecordField Name="Deptnumber" />
                    <ext:RecordField Name="Deptname" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="MainDeptStore" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="Deptnumber">
                <Fields>
                    <ext:RecordField Name="Deptnumber" />
                    <ext:RecordField Name="Deptname" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    
    <ext:Hidden ID="GridData" runat="server" />

    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                <Center>
                    <ext:GridPanel 
                        ID="GridPanel1" 
                        runat="server"
                        StoreID="Store1"
                        StripeRows="true"
                        Title="安全统计报表" 
                        AutoScroll="true"
                        Icon="Table"
                        >
                        <TopBar>
                            <ext:Toolbar runat="server" ID="tb1">
                                <Items>
                                    <ext:Label ID="Label1" runat="server" Text="开始日期：" />
                                    <ext:DateField ID="dfBegin" runat="server" />
                                    <ext:Label ID="Label2" runat="server" Text="截止日期：" />
                                    <ext:DateField ID="dfEnd" runat="server" />
                                    <ext:ToolbarSeparator />
                                    <ext:Label ID="Label3" runat="server" Text="单位：" />
                                     <ext:ComboBox 
                                        ID="cbbMianDept" 
                                        runat="server"
                                        DisplayField="Deptname" 
                                        ValueField="Deptnumber"
                                        StoreID="MainDeptStore"
                                        TypeAhead="true" 
                                        Mode="Local"
                                        ForceSelection="true" 
                                        TriggerAction="All"
                                        Width="100"
                                        Editable="false"
                                        />
                                    <ext:Label ID="Label4" runat="server" Text="部门：" />
                                    <ext:ComboBox 
                                        ID="cbbKQ" 
                                        runat="server"
                                        DisplayField="Deptname" 
                                        ValueField="Deptnumber"
                                        StoreID="KQStore"
                                        TypeAhead="true" 
                                        Mode="Local"
                                        ForceSelection="true" 
                                        TriggerAction="All"
                                        Width="100"
                                        Editable="false"
                                        />
                                    <ext:ToolbarSeparator />
                                    <ext:Button ID="btnSearch" runat="server" Text="查询" Icon="Zoom">
                                        <Listeners>
                                            <%--<Click Handler="Coolite.AjaxMethods.LoadData();" />--%>
                                            <Click Handler="#{Store1}.reload();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:ToolbarFill />
                                    <ext:Button ID="Button3" runat="server" Text="导出报表" Icon="PageExcel" AutoPostBack="true" OnClick="ToExcel">
                                        <Listeners>
                                            <Click Fn="saveData" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <ColumnModel ID="ColumnModel1" runat="server">
		                    <Columns>
                                <ext:Column Header="单位" Width="100" DataIndex="MAINDEPT" />
                                <ext:Column Header="部门" Width="150" DataIndex="DEPTNAME" />
                                <ext:Column Header="姓名" Width="65" DataIndex="NAME" />
                                <ext:Column Header="职务" Width="65" DataIndex="POSNAME" />
                                <ext:Column Header="职务级别" Width="65" DataIndex="MOVEGBLEVEL" />
                                <ext:Column Header="下井次数" Width="65" DataIndex="XJ">
                                    <Renderer Fn="change" />
                                </ext:Column>
                                <ext:Column Header="隐患数" Width="50" DataIndex="YH">
                                    <Renderer Fn="change" />
                                </ext:Column>
                                <ext:Column Header="现场整改" Width="65" DataIndex="YXC">
                                    <Renderer Fn="change" />
                                </ext:Column>
                                <ext:Column Header="非现场整改" Width="65" DataIndex="FXC">
                                    <Renderer Fn="change" />
                                </ext:Column>
                                <ext:Column Header="已闭合" Width="50" DataIndex="YBH">
                                    <Renderer Fn="change" />
                                </ext:Column>
                                <ext:Column Header="未闭合" Width="50" DataIndex="WBH">
                                    <Renderer Fn="change" />
                                </ext:Column>
                                <ext:Column Header="三违数" Width="50" DataIndex="SW">
                                    <Renderer Fn="change" />
                                </ext:Column>
                                <ext:Column Header="登陆次数" Width="65" DataIndex="LOGINCOUNT" />
		                    </Columns>
                        </ColumnModel>
                        <LoadMask ShowMask="true" Msg="数据加载中..." />
                        <SelectionModel>
                            <ext:CellSelectionModel ID="CellSelectionModel1" runat="server">
                                <AjaxEvents>
                                    <CellSelect OnEvent="Cell_Click" />                        
                                </AjaxEvents>
                            </ext:CellSelectionModel>
                        </SelectionModel>
                     </ext:GridPanel>
                </Center>
            </ext:BorderLayout>
        </Body>
    </ext:ViewPort>

    <ext:Window 
        ID="Window1" 
        runat="server"
        Maximizable="true" 
        Icon="Server" 
        Title="数据信息"
        Width="725"
        Modal="true"
        ShowOnLoad="false"
        Resizable="false" AutoScroll="true"
        CloseAction="Hide">
        <AutoLoad Url="" Mode="IFrame" ShowMask="true" MaskMsg="正在加载数据..." />
        <%--<Listeners>
            <BeforeShow Fn="function(el) { el.setHeight(Ext.getBody().getViewSize().height - 20); }" />
        </Listeners>--%>
    </ext:Window>
    </form>
</body>
</html>
