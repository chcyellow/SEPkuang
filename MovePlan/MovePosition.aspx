<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MovePosition.aspx.cs" Inherits="MovePlan_MovePosition" %>

<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Style/examples.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        var saveData = function () {
        GridData.setValue(Ext.encode(GridPanel1.getRowsValues(false)));
        }
    </script>
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
    <div style=" text-align:center">
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN" CleanResourceUrl="false">
    </ext:ScriptManager>
    <ext:Hidden ID="GridData" runat="server" />
    <ext:Store ID="MoveStore" runat="server" OnRefreshData="MyData_Refresh">
        <Reader>
            <ext:JsonReader ReaderID="ID">
                <Fields>
                    <ext:RecordField Name="ID" />
                    <ext:RecordField Name="Name" />
                    <ext:RecordField Name="DeptName" />
                    <ext:RecordField Name="PosName" />
                    <ext:RecordField Name="PlaceName" />
                    <ext:RecordField Name="StartTime" Type="Date" DateFormat="Y-m-dTh:i:s" />
                    <ext:RecordField Name="EndTime" Type="Date" DateFormat="Y-m-dTh:i:s" />
                    <ext:RecordField Name="MoveStartTime" Type="Date" DateFormat="Y-m-dTh:i:s" />
                    <ext:RecordField Name="MoveEndTime" Type="Date" DateFormat="Y-m-dTh:i:s" />
                    <ext:RecordField Name="MoveState" />
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
    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                <Center>
    
    <ext:GridPanel 
        ID="GridPanel1" 
        runat="server"
        StoreID="MoveStore"
        StripeRows="true"
        Title="干部走动情况"
        AutoExpandColumn="NIid" 
        Collapsible="false"
        Width="910px">
        <ColumnModel ID="ColumnModel1" runat="server">
		    <Columns>
		        <ext:Column ColumnID="NIid" Header="计划编号" Width="70" Sortable="true" DataIndex="ID" />
                <ext:Column Header="走动人员" Width="70" Sortable="true" DataIndex="Name" />
                <ext:Column Header="人员部门" Width="70" Sortable="true" DataIndex="DeptName" />
                <ext:Column Header="人员职务" Width="70" Sortable="true" DataIndex="PosName" />
                <ext:Column Header="走动地点" Width="180" Sortable="true" DataIndex="PlaceName" />
                <ext:Column Header="计划开始" Width="100" Sortable="true" DataIndex="StartTime" >
                    <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" />
                </ext:Column>
                <ext:Column Header="计划截止" Width="100" Sortable="true" DataIndex="EndTime" >
                    <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" />
                </ext:Column>
                <%--<ext:Column Header="走动开始" Width="100" Sortable="true" DataIndex="MoveStartTime" >
                    <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" />
                </ext:Column>
                <ext:Column Header="走动结束" Width="100" Sortable="true" DataIndex="MoveEndTime" >
                    <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" />
                </ext:Column>--%>
                <ext:Column Header="走动状态" Width="100" Sortable="true" DataIndex="MoveState">
                    <Renderer Fn="change" />
                </ext:Column>
		    </Columns>
        </ColumnModel>
        <LoadMask ShowMask="true" Msg="数据加载中..." />
        <BottomBar>
            <ext:PagingToolBar ID="PagingToolBar1" runat="server" PageSize="15" />
        </BottomBar>
        <TopBar>
            <ext:Toolbar runat="server" ID="tb1">
                <Items>
                    <ext:Button runat="server" ID="btn_search" Icon="FolderMagnify" Text="条件查询" >
                        <Listeners>
                            <Click Handler="#{FormWindow}.show();" />
                        </Listeners>
                    </ext:Button>
                    <ext:ToolbarFill />
                </Items>
            </ext:Toolbar>
        </TopBar>
        <View>
            <ext:GridView ForceFit="true" />
        </View>
        <SelectionModel>
            <ext:RowSelectionModel SingleSelect="true" />
        </SelectionModel>
        <Listeners>
            <BeforeShow Fn="function(el) { el.setHeight(Ext.getBody().getViewSize().height - 5); }" />
        </Listeners>
     </ext:GridPanel>
                </Center>
            </ext:BorderLayout>
        </Body>
    </ext:ViewPort>
     <%--查询条件--%>
    <ext:Window 
        ID="FormWindow" 
        runat="server" 
        BodyStyle="padding:5px;" 
        ButtonAlign="Right"
        Frame="true" 
        Title="请选择查询条件"
        AutoHeight="true"
        Width="300px"
        Modal="true"
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
    </div>
    </form>
</body>
</html>
