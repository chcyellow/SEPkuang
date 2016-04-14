<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CloseMovePlan.aspx.cs" Inherits="YSNewProcess_CloseMovePlan" %>

<%@ Register assembly="Coolite.Ext.Web" namespace="Coolite.Ext.Web" tagprefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        var template = '<span style="color:{0};"><b>{1}</b><img src="{2}" height="22px" width="22px" /></span>';

        var change = function(value) {
            var color, url;
            if (value.toString().replace(/(^\s*)|(\s*$)/g, "") == '已走动') {
                color = 'green';
                url = '../Images/yzd.gif';
            }
            else if (value.toString().replace(/(^\s*)|(\s*$)/g, "") == '未走动') {
                color = '#cc0000';
                url = '../Images/wzd.gif';
            }
            else {
                color = 'red';
                url = '../Images/wzd.gif';
            }
            return String.format(template, color, value, url);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN" />
    <ext:Store ID="Store1" runat="server" >
        <Reader>
            <ext:JsonReader ReaderID="ID">
                <Fields>
                    <ext:RecordField Name="ID" />
                    <ext:RecordField Name="PersonID" Type="Int" />
                    <ext:RecordField Name="Name" />
                    <ext:RecordField Name="DeptName" />
                    <ext:RecordField Name="PlaceName" />
                    <ext:RecordField Name="StartTime" Type="Date" DateFormat="Y-m-dTh:i:s" />
                    <ext:RecordField Name="EndTime" Type="Date" DateFormat="Y-m-dTh:i:s" />
                   <%-- <ext:RecordField Name="MoveStartTime" Type="Date" DateFormat="Y-m-dTh:i:s" />
                    <ext:RecordField Name="MoveEndTime" Type="Date" DateFormat="Y-m-dTh:i:s" />  --%>                               
                    <ext:RecordField Name="MoveState" />
                    <ext:RecordField Name="Closeremarks" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    
    <ext:Store ID="DeptStore" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="Deptnumber">
                <Fields>
                    <ext:RecordField Name="Deptnumber"/>
                    <ext:RecordField Name="Deptname" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    
    <ext:Store ID="PersStore" runat="server" AutoLoad="false" OnRefreshData="PersRefresh">
        <Reader>
            <ext:JsonReader ReaderID="Personnumber">
                <Fields>
                    <ext:RecordField Name="Personnumber" />
                    <ext:RecordField Name="Name" />
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
                            Title="走动计划闭合"
                            Collapsible="false">
                            <ColumnModel ID="ColumnModel1" runat="server">
		                        <Columns>
		                            <ext:Column ColumnID="ID" Header="编号" Width="70" DataIndex="PersonID" >
                                    </ext:Column>
                                    <ext:Column Header="走动人员" Width="80" Sortable="true" DataIndex="Name" >
                                    </ext:Column>
                                    <ext:Column Header="部门" Width="100" Sortable="true" DataIndex="DeptName" >
                                    </ext:Column>
                                    <ext:Column Header="走动地点" Width="100" Sortable="true" DataIndex="PlaceName" >
                                    </ext:Column>
                                    <ext:Column Header="计划开始" Width="80" Sortable="true" DataIndex="StartTime" >
                                    <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" /></ext:Column>
                                    <ext:Column Header="计划截止" Width="80" Sortable="true" DataIndex="EndTime" >
                                    <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" /></ext:Column>
                                    <%--<ext:Column Header="走动开始" Width="80" Sortable="true" DataIndex="MoveStartTime" >
                                    <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d h:i')" /></ext:Column>
                                    <ext:Column Header="走动结束" Width="80" Sortable="true" DataIndex="MoveEndTime" >
                                    <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d h:i')" /></ext:Column>--%>
                                    <ext:Column Header="闭合原因" Width="100" Sortable="true" DataIndex="Closeremarks" />
                                    <ext:Column Header="走动状态" Width="185" Sortable="true" DataIndex="MoveState" >
                                        <Renderer Fn="change" />
                                    </ext:Column>
		                        </Columns>
                            </ColumnModel>
                            <Plugins>
                                <ext:GridFilters runat="server" ID="GridFilters1" Local="true">
                                     <Filters>
                                           <ext:StringFilter DataIndex="PersonID" />
                                           <ext:StringFilter DataIndex="Name" />
                                           <ext:StringFilter DataIndex="DeptName" />
                                           
                                           <ext:StringFilter DataIndex="PlaceName" />
                                           <ext:DateFilter DataIndex="StartTime">
                                                <DatePickerOptions runat="server" TodayText="Now" />
                                           </ext:DateFilter>
                                           <ext:DateFilter DataIndex="EndTime">
                                                <DatePickerOptions runat="server" TodayText="Now" />
                                           </ext:DateFilter>
                                           <ext:ListFilter DataIndex="MoveState" Options="未走动,走动中,已走动" />
                                     </Filters>
                                 </ext:GridFilters>
                            </Plugins>
                            <LoadMask ShowMask="true" Msg="数据加载中..." />
                            <BottomBar>
                                <ext:PagingToolBar ID="PagingToolBar1" runat="server" PageSize="15">
                                    <Items>
                                        <ext:ToolbarSeparator />
                                        <ext:Label runat="server" StyleSpec="color:red;" Text="提醒：当前日期如果是本月前3天则允许闭合上月计划！" />
                                    </Items>
                                </ext:PagingToolBar>
                             </BottomBar>
                            
                            <SelectionModel>
                                    <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" SingleSelect="false" runat="server" />                   
                            </SelectionModel>
                            <View>
                                <ext:GridView ForceFit="true" />
                            </View>
                           <TopBar>
                                <ext:Toolbar runat="server" ID="tb1">
                                    <Items>
                                        <ext:Label ID="Label1" runat="server" Text="单位:" StyleSpec="color:blue;" />
                                        <ext:ComboBox ID="cbbforcheckDept" runat="server"
                                            DisplayField="Deptname" 
                                            ValueField="Deptnumber"
                                            Width="180px" 
                                            StoreID="DeptStore" Editable="false">
                                            <Listeners>
                                                <Select Handler="#{fb_zrr}.clearValue(); #{PersStore}.reload();" />
                                            </Listeners>
                                        </ext:ComboBox>
                                        
                                        <ext:Label ID="Label2" runat="server" Text="走动人员:" StyleSpec="color:blue;" />
                                        <ext:ComboBox 
                                                    ID="fb_zrr" 
                                                    runat="server"
                                                    DisplayField="Name" 
                                                    ValueField="Personnumber"
                                                    Width="200px"
                                                    StoreID="PersStore"
                                                    EmptyText="没有待选人员"
                                                    Disabled="true" 
                                                    >
                                                </ext:ComboBox>
                                                
                                        <ext:ToolbarSeparator />
                                        <ext:ToolbarButton ID="btnSearch" runat="server" Icon="Zoom" Text="查询">
                                            <Listeners>
                                                <Click Handler="Coolite.AjaxMethods.btnSearch_Click();" />
                                            </Listeners>
                                        </ext:ToolbarButton>
                                        <ext:ToolbarSeparator />
                                        <ext:Button runat="server"  ID="btn_fcfk" Icon="FolderUser" Text="闭合走动">
                                            <Listeners>
                                                <Click Handler="Coolite.AjaxMethods.CloseMP();" />
                                            </Listeners>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                         </ext:GridPanel>
                    </Center>
                </ext:BorderLayout>
            </Body>
        </ext:ViewPort>
    </form>
</body>
</html>
