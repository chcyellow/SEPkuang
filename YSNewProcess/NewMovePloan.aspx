<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NewMovePloan.aspx.cs" Inherits="YSNewProcess_NewMovePloan" %>

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

        var shownothing = function (value) {
            return '<span />';;
        }

        var prepare = function (grid, command, record, row, col, value) {
            //debugger;
            if (value != "已走动" && command.command == "Edit") {
                command.hidden = true;
                command.hideMode = "visibility";
            }
        }

    </script>    
    <script language="javascript" type="text/javascript">
        var saveData = function () {
            GridDataExcel.setValue(Ext.encode(GridPanel1.getRowsValues(false)));
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN" CleanResourceUrl="false" />
    <ext:Store ID="Store1" runat="server" OnRefreshData="MyData_Refresh">
        <Reader>
            <ext:JsonReader ReaderID="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Personid" Type="Int" />
                    <ext:RecordField Name="Name" />
                    <ext:RecordField Name="Deptname" />
                    <ext:RecordField Name="Posname" />
                    <ext:RecordField Name="Placename" />
                    <ext:RecordField Name="Starttime" Type="Date" DateFormat="Y-m-dTh:i:s" />
                    <ext:RecordField Name="Endtime" Type="Date" DateFormat="Y-m-dTh:i:s" />
                    <ext:RecordField Name="Movestarttime" Type="Date" DateFormat="Y-m-dTh:i:s" />
                    <ext:RecordField Name="Movestate" />
                    <ext:RecordField Name="Closeremarks" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>

    <ext:Store ID="PersonStore" runat="server" AutoLoad="false" OnRefreshData="PersonRefresh">
        <AjaxEventConfig>
            <EventMask ShowMask="false" />
        </AjaxEventConfig>
        <Reader>
            <ext:JsonReader ReaderID="Personnumber">
                <Fields>
                    <ext:RecordField Name="Personnumber" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="PosStore" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="PosID">
                <Fields>
                    <ext:RecordField Name="PosID" Type="Int" />
                    <ext:RecordField Name="PosName" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store4" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="PlaceID">
                <Fields>
                    <ext:RecordField Name="PlaceID" Type="Int" />
                    <ext:RecordField Name="PlaceName" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>

    <%--<ext:Store ID="SearchBLStore" runat="server">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="person_name" />
                    <ext:RecordField Name="entertime_mine" Type="Date" DateFormat="Y-m-dTh:i:s" />
                    <ext:RecordField Name="outtime_mine" Type="Date" DateFormat="Y-m-dTh:i:s" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>--%>

    <ext:Hidden ID="GridDataExcel" runat="server" />
    
    <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:BorderLayout ID="BorderLayout1" runat="server">
                    <Center>
                        <ext:GridPanel 
                            ID="GridPanel1" 
                            runat="server"
                            StoreID="Store1"
                            StripeRows="true"
                            Title="走动计划"
                            Collapsible="false" 
                            Width="810px"
                            >
                            <ColumnModel ID="ColumnModel1" runat="server">
		                        <Columns>
                                    <ext:Column ColumnID="NIid" Header="编号" Width="50" DataIndex="Personid" />
                                    <ext:Column Header="走动人员" Width="80" Sortable="true" DataIndex="Name" />
                                    <ext:Column Header="部门" Width="100" Sortable="true" DataIndex="Deptname" />
                                    <ext:Column Header="职务" Width="150" Sortable="true" DataIndex="Posname" />
                                    <ext:Column Header="走动地点" Width="200" Sortable="true" DataIndex="Placename" />
                                    <ext:Column Header="计划开始" Width="80" Sortable="true" DataIndex="Starttime" >
                                        <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" />
                                    </ext:Column>
                                    <ext:Column Header="计划截止" Width="80" Sortable="true" DataIndex="Endtime" >
                                        <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" />
                                    </ext:Column>
                                    <ext:Column Header="走动时间" Width="80" Sortable="true" DataIndex="Movestarttime" >
                                        <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" />
                                    </ext:Column>
                                    <ext:Column Header="走动状态" Width="150" Sortable="true" DataIndex="Movestate" >
                                        <Renderer Fn="change" />
                                    </ext:Column>
                                    <ext:Column Width="60" Header="走动轨迹" DataIndex="Movestate">
                                        <Renderer Fn="shownothing" />
                                        <Commands>
                                            <ext:ImageCommand Icon="Zoom" CommandName="Edit">
                                                 <ToolTip Text="走动轨迹" />
                                            </ext:ImageCommand>
                                        </Commands>
                                        <PrepareCommand Fn="prepare" />
                                    </ext:Column>
                                    <ext:Column Header="闭合原因" Width="100" Sortable="true" DataIndex="Closeremarks" />
		                        </Columns>
                            </ColumnModel>
                            <LoadMask ShowMask="true" />
                            <BottomBar>
                                <ext:PagingToolbar runat="server" ID="pageToolBar" StoreID="Store1" />
                            </BottomBar>

                             <TopBar>
                                <ext:Toolbar runat="server" ID="tb1">
                                    <Items>
                                        <ext:Button runat="server" ID="btn_search" Icon="FolderMagnify" Text="条件查询" >
                                            <Listeners>
                                                <Click Handler="Coolite.AjaxMethods.SearchLoad();#{FormWindow}.show();" />
                                            </Listeners>
                                        </ext:Button>
                                        <ext:ToolbarSeparator />
                                        <ext:Button runat="server" ID="ben_plan" Icon="Add" Text="新增计划" >
                                            <Listeners>
                                                <%--<Click Handler="#{FormPanel1}.show();" />--%>
                                                <Click Handler="parent.window.loadnewpage('新增走动计划','YSNewProcess/MovePlanCreate.aspx');" />
                                            </Listeners>
                                        </ext:Button>
                                        <ext:Button runat="server" ID="btn_delete" Icon="ReportDelete" Text="删除" Disabled="true">
                                            <Listeners>
                                                <Click Handler="Coolite.AjaxMethods.delshow();" />
                                            </Listeners>
                                        </ext:Button>
                                        <ext:ToolbarSeparator />
                                        <ext:Button runat="server" ID="Button1" Icon="Add" Text="复制上月计划" >
                                            <Listeners>
                                                <%--<Click Handler="#{FormPanel1}.show();" />--%>
                                                <Click Handler="Coolite.AjaxMethods.CopyPlan();" />
                                            </Listeners>
                                        </ext:Button>
                                        <%-- <ext:ToolbarSeparator />
                                        <ext:Button runat="server" ID="btnpostion" Icon="UserEarth" Text="走动信息情况" Disabled="true">
                                            <Listeners>
                                                <Click Handler="Coolite.AjaxMethods.PostionShow();" />
                                            </Listeners>
                                        </ext:Button>--%>
                                        <ext:ToolbarFill />
                                        <ext:Button ID="btn_print" runat="server" Text="导出报表" Icon="PageExcel" AutoPostBack="true" OnClick="ToExcel">
                                            <Listeners>
                                                <Click Fn="saveData" />
                                            </Listeners>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <SelectionModel>
                                <ext:CheckboxSelectionModel runat="server" ID="CheckboxSelectionModel1" />          
                            </SelectionModel>
                            <AjaxEvents>
                                <Click OnEvent="RowClick"></Click>
                            </AjaxEvents>
                            <Listeners>
                                <Command Handler="Coolite.AjaxMethods.PostionShow(command,record.data.Id);" />
                            </Listeners>
                         </ext:GridPanel>
                    </Center>
                </ext:BorderLayout>
            </Body>
        </ext:ViewPort>
        
        <ext:Window 
        ID="FormWindow" 
        runat="server" 
        BodyStyle="padding:5px;" 
        ButtonAlign="Right"
        Frame="true" 
        Title="请选择查询条件"
        Height="200px"
        Width="300px"
        Modal="true" AutoScroll="true"
        ShowOnLoad="false"
        X="100" Y="60">
        <Body>
            <ext:FormLayout ID="FormLayout1" runat="server" LabelWidth="60">
                <ext:Anchor Horizontal="95%">
                    <ext:MultiField ID="mf_datecheck" runat="server" FieldLabel="排查时间">
                        <Fields>
                            <ext:DateField ID="df_begin" runat="server" Format="yyyy-MM-dd" Note="起始日期" Width="100" Vtype="daterange">
                                <%--<Listeners>
                                    <Render Handler="this.endDateField = '#{df_end}'" />
                                </Listeners>--%>
                            </ext:DateField>
                            <ext:DateField ID="df_end" runat="server" Format="yyyy-MM-dd" Note="截止日期" Width="100" Vtype="daterange" >
                                <%--<Listeners>
                                    <Render Handler="this.startDateField = '#{df_begin}'" />
                                </Listeners>--%>
                            </ext:DateField>
                        </Fields>
                    </ext:MultiField>
                </ext:Anchor>
                <ext:Anchor Horizontal="95%">
                    <ext:ComboBox 
                    ID="cbb_zhiwu" 
                    runat="server" 
                    FieldLabel="职务" 
                    EmptyText="请选择职务...."
                    DisplayField="PosName" 
                    ValueField="PosID" 
                    StoreID="PosStore" 
                    Editable="false" 
                    TypeAhead="true" 
                    Mode="Local"
                    ForceSelection="true" 
                    TriggerAction="All" 
                    SelectOnFocus="true"
                    >
                        <Listeners>
                            <Select Handler="#{cbb_person}.clearValue(); #{PersonStore}.reload();" />
                        </Listeners>
                    </ext:ComboBox>
                </ext:Anchor>
                <ext:Anchor Horizontal="95%">
                    <ext:ComboBox 
                        ID="cbb_person" 
                        runat="server" 
                        FieldLabel="走动人员" 
                        EmptyText="请选择人员...."
                        DisplayField="Name" 
                        ValueField="Personnumber" 
                        StoreID="PersonStore"
                        TypeAhead="true" 
                        Mode="Local"
                        ForceSelection="true" 
                        TriggerAction="All"
                        >
                    </ext:ComboBox>
                </ext:Anchor>
                <ext:Anchor Horizontal="95%">
                    <ext:ComboBox 
                        ID="cbb_place" 
                        runat="server" 
                        FieldLabel="走动地点" 
                        EmptyText="请选择点...."
                        DisplayField="PlaceName" 
                        ValueField="PlaceID" 
                        StoreID="Store4"
                        TypeAhead="true" 
                        Mode="Local"
                        ForceSelection="true" 
                        TriggerAction="All"
                        >
                    </ext:ComboBox>
                </ext:Anchor>
            </ext:FormLayout>
        </Body>
        <Buttons>
                <ext:Button ID="Button4" runat="server" Icon="Cancel" Text="清除条件">
                    <Listeners>
                        <Click Handler="Coolite.AjaxMethods.ClearSearch();" />
                    </Listeners>
                </ext:Button>
                <ext:Button ID="Button3" runat="server" Icon="Zoom" Text="查 询">
                    <Listeners>
                        <Click Handler="Coolite.AjaxMethods.Search();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
    </ext:Window>

    <ext:Window 
        ID="SearchBLWindow" 
        runat="server" 
        BodyStyle="padding:5px;" 
        ButtonAlign="Right"
        Frame="true" 
        Title="人员下井信息"
        Height="400px"
        Width="600px"
        Modal="true" Icon="UserEarth" Resizable="false"
        ShowOnLoad="false"
        >
        <%--<Body>
            <ext:BorderLayout runat="server" ID="SearchBL">
                <Center>
                    <ext:GridPanel ID="gpSearchBL" runat="server" StoreID="SearchBLStore">
                        <ColumnModel ID="ColumnModel2" runat="server">
		                    <Columns>
                                <ext:Column Header="姓名" Width="90" DataIndex="person_name" />
                                <ext:Column Header="下井时间" Width="150" DataIndex="entertime_mine">
                                    <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d h:i:s')" />
                                </ext:Column>
                                <ext:Column Header="上井时间" Width="150" DataIndex="outtime_mine">
                                    <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d h:i:s')" />
                                </ext:Column>
		                    </Columns>
                        </ColumnModel>
                        <LoadMask Msg="数据加载中，请稍候..." ShowMask="true" />
                        <SelectionModel>
                            <ext:RowSelectionModel runat="server" ID="SearchBLrsm" SingleSelect="true" />
                        </SelectionModel>
                    </ext:GridPanel>
                </Center>
            </ext:BorderLayout>
        </Body>--%>
        <LoadMask Msg="数据加载中，请稍候..." ShowMask="true" />
    </ext:Window>
        
    </form>
</body>
</html>
