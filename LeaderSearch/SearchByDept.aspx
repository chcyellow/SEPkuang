<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SearchByDept.aspx.cs" Inherits="LeaderSearch_SearchByDept" %>

<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript">
        var template = '<span style="color:{0};"><b>{1}</b></span>';

        var change = function(value) {
        var color;
            if (value>= 0)
                color = '#cc0000';
            
            return String.format(template, color, value);
        }
    </script>
    <script type="text/javascript">
        var template = '<span style="color:{0};"><b>{1}</b></span>';

        var change1 = function(value) {
            var color;
            if (value >= 0)
                color = 'green';

            return String.format(template, color, value);
        }
    </script>
    <style lang="zh-cn" type="text/css" >
        .hand
        {
            cursor: pointer;
        }
    </style>
</head>
<body style=" text-align:center;">
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN" CleanResourceUrl="false">
    </ext:ScriptManager>
    <ext:Store ID="Store1" runat="server" OnRefreshData="MyData_Refresh">
        <Reader>
            <ext:JsonReader ReaderID="DEPTID">
                <Fields>
                    <ext:RecordField Name="MAINDEPTID"/>
                    <ext:RecordField Name="MAINDEPTNAME" />
                    <ext:RecordField Name="DEPTID" />
                    <ext:RecordField Name="DEPTNAME" />
                    <ext:RecordField Name="YHYZG" Type="Int" />
                    <ext:RecordField Name="YHWZG" Type="Int" />
                    <ext:RecordField Name="YHALL" Type="Int" />
                    <ext:RecordField Name="SWALL" Type="Int"  />
                    <ext:RecordField Name="YZD" Type="Int" />
                    <ext:RecordField Name="WZD" Type="Int" />
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
                <Center>
    <ext:GridPanel 
        ID="GridPanel1" 
        runat="server"
        StoreID="Store1"
        StripeRows="true"
        Title="全矿单位安全状况"
        Collapsible="false" 
        Width="710px"
        Height="400px" AutoScroll="true">
        <ColumnModel ID="ColumnModel1" runat="server">
		    <Columns>
		    <ext:Column Header="单位" DataIndex="MAINDEPTNAME" Resizable="false" />
                <ext:Column Header="部门" DataIndex="DEPTNAME" Resizable="false" />
                <ext:Column Header="隐患数量" DataIndex="YHALL" Resizable="false" Css="cursor:pointer;" />
                <ext:Column Header="已闭合隐患" DataIndex="YHYZG" Resizable="false" Css="cursor:pointer;" >
                <Renderer Fn="change1" />
                </ext:Column>
                
                <ext:Column Header="未闭合隐患" DataIndex="YHWZG" Resizable="false" Css="cursor:pointer;" >
                <Renderer  Fn="change"  />
                </ext:Column>
                <ext:Column Header="三违信息" DataIndex="SWALL" Resizable="false" Css="cursor:pointer;" />
                <ext:Column Header="已走动次数" DataIndex="YZD" Resizable="false" Css="cursor:pointer;" >
                <Renderer Fn="change1" />
                </ext:Column>
                <ext:Column Header="未走动次数" DataIndex="WZD" Resizable="false" Css="cursor:pointer;" >
                <Renderer Fn="change" />
                </ext:Column>
		    </Columns>
        </ColumnModel>
        <LoadMask ShowMask="true" />
        <%--<BottomBar>
            <ext:PagingToolBar ID="PagingToolBar1" runat="server" PageSize="15" />
         </BottomBar>--%>
        <TopBar>
            <ext:Toolbar runat="server" ID="tb1">
                <Items>
                    <ext:Label ID="Label1" runat="server" Text="时间选择:">
                    </ext:Label>
                    <ext:DateField ID="dfBegin" runat="server" Width="100" Vtype="daterange">
                    </ext:DateField>
                    <ext:Label ID="Label2" runat="server" Text="---">
                    </ext:Label>
                    <ext:DateField ID="dfEnd" runat="server" Width="100" Vtype="daterange"> 
                    </ext:DateField>
                    <ext:ToolbarSeparator />
                    
                    <ext:Label ID="Label4" runat="server" Text="单位：">
                    </ext:Label>
           
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
                       <ext:ToolbarSeparator />
                    <ext:Button ID="Button2" runat="server" Text="查询" Icon="Zoom">
                        <Listeners>
                            <Click Handler="Coolite.AjaxMethods.storebind();" />
                        </Listeners>
                    </ext:Button>
                    <ext:ToolbarFill />
                    <ext:Button ID="Button1" runat="server" Icon="PageExcel" Disabled="true">
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </TopBar>
        <SelectionModel>
            <ext:CellSelectionModel ID="CellSelectionModel1" runat="server">
                <AjaxEvents>
                    <CellSelect OnEvent="Cell_Click" />                        
                </AjaxEvents>
            </ext:CellSelectionModel>
        </SelectionModel>
        <View>
            <ext:GridView ForceFit="true" />
        </View>
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
        Resizable="false"
        CloseAction="Hide">
        <AutoLoad Url="" Mode="IFrame" ShowMask="true" MaskMsg="正在加载数据..." />
        <%--<Listeners>
            <BeforeShow Fn="function(el) { el.setHeight(Ext.getBody().getViewSize().height - 20); }" />
        </Listeners>--%>
    </ext:Window>
    </form>
</body>
</html>
