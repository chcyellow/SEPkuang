<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Movemodels.aspx.cs" Inherits="MovePlan_Movemodels" %>

<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/examples.css"rel="stylesheet" type="text/css" />
    <style type="text/css">
        .search-item {
            font: normal 11px tahoma, arial, helvetica, sans-serif;
            padding: 3px 10px 3px 10px;
            border: 1px solid #fff;
            border-bottom: 1px solid #eeeeee;
            white-space: normal;
            color: #555;
        }
        
        .search-item h3 {
            display: block;
            font: inherit;
            font-weight: bold;
            color: #222;
        }

        .search-item h3 span {
            float: right;
            font-weight: normal;
            margin: 0 0 5px 5px;
            width: 100px;
            display: block;
            clear: none;
        } 
        
        p { width: 650px; }
        
        .ext-ie .x-form-text { position: static !important; }
    </style>
    
    <ext:TokenScript ID="TokenScript1" runat="server">
        <script type="text/javascript">
            var applyFilter = function (py,store) {
                store.filterBy(getRecordFilter(py));
            }
            
            var clearFilter = function () {
            }

            var filterString = function (value, dataIndex, record) {
                var val = record.get(dataIndex);
                if (typeof val != "string") {
                    return value.length == 0;
                }
                return val.toLowerCase().indexOf(value.toLowerCase()) > -1;
            }

            var getRecordFilter = function (py) {
                var f = [];

                f.push({
                    filter: function(record) {                         
                        return filterString(py, 'pyall', record);
                    }
                });
                
                
                var len = f.length;
                return function(record) {
                    for (var i = 0; i < len; i++) {
                        if (!f[i].filter(record)) {
                            return false;
                        }
                    }
                    return true;
                };
            } 
        </script>
    </ext:TokenScript>
</head>
<body>
    <form id="form1" runat="server">
    <div style=" text-align:center">
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN" CleanResourceUrl="false">
    </ext:ScriptManager>
    <ext:Store ID="Store1" runat="server" OnRefreshData="MyData_Refresh">
        <Reader>
            <ext:JsonReader ReaderID="FrequencyID">
                <Fields>
                    <ext:RecordField Name="FrequencyID" />
                    <ext:RecordField Name="DeptName" />
                    <ext:RecordField Name="PosName" />
                    <ext:RecordField Name="PlaceName" />
                    <ext:RecordField Name="Frequency" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="PosSearchStore" runat="server" AutoLoad="false" OnRefreshData="PosSearchRefresh">
        <AjaxEventConfig>
            <EventMask ShowMask="false" />
        </AjaxEventConfig>
        <Reader>
            <ext:JsonReader ReaderID="PosID">
                <Fields>
                    <ext:RecordField Name="PosID" Type="Int" />
                    <ext:RecordField Name="PosName" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="DepartmentSearchStore" runat="server">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="deptID" />
                    <ext:RecordField Name="deptName" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="PlaceSearchStore" runat="server" AutoLoad="false">
            <Reader>
                <ext:JsonReader >
                    <Fields>
                        <ext:RecordField Name="placID" />
                        <ext:RecordField Name="placName" />
                       <%-- <ext:RecordField Name="pyall" />--%>
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
    <ext:Window 
        ID="Window1" 
        runat="server" 
        BodyStyle="padding:5px;" 
        ButtonAlign="Right"
        Frame="true" 
        Title="信息维护"
        AutoHeight="true"
        Width="275px"
        Modal="true"
        ShowOnLoad="false"
        X="140" Y="60">
        <Body>
            <ext:FormLayout ID="FormLayout2" runat="server" LabelWidth="65">
                <ext:Anchor Horizontal="95%">
                    <ext:ComboBox 
                        ID="cbb_dep"
                        runat="server" 
                        StoreID="DepartmentSearchStore"
                        DisplayField="deptName" 
                        ValueField="deptID"
                        TypeAhead="false"
                        LoadingText="数据查询中..."
                        BlankText="部门不能为空!" 
                        AllowBlank="false"
                        PageSize="15"
                        ListWidth="240"
                        HideTrigger="true"
                        FieldLabel="部门"
                        ItemSelector="div.search-item" MinChars="20"> 
                        <Template ID="Template1" runat="server">
                           <tpl for=".">
                               <div class="search-item">
                                 <span>{deptName}</span>
                              </div>
                           </tpl>
                        </Template>
                        <Listeners>
                            <Render Fn="function(f) {
                                f.el.on('keyup', function(e) {
                                  if(window.event.keyCode==38 || window.event.keyCode==40 || window.event.keyCode==13){
                                        return;
                                     }
                                    Coolite.AjaxMethods.PYsearch(f.getRawValue(), 'DepartmentSearchStore');
                                                           
                                });
                                }" />
                            <Select Handler="#{cbb_pos}.clearValue(); #{PosSearchStore}.reload();" />
                        </Listeners>
                    </ext:ComboBox>
                </ext:Anchor>
                <ext:Anchor Horizontal="95%">
                    <ext:ComboBox 
                    ID="cbb_pos" 
                    runat="server" 
                    FieldLabel="职务" 
                    BlankText="职务不能为空!" 
                    AllowBlank="false" 
                    EmptyText="请选择职务...."
                    DisplayField="PosName" 
                    ValueField="PosID" 
                    StoreID="PosSearchStore" 
                    Editable="false" 
                    TypeAhead="true" 
                    Mode="Local"
                    ForceSelection="true" 
                    TriggerAction="All" 
                    SelectOnFocus="true"
                    >
                    </ext:ComboBox>
                </ext:Anchor>
                <ext:Anchor Horizontal="95%">
                    <ext:ComboBox 
                        ID="cbb_pla"
                        runat="server" 
                        StoreID="PlaceSearchStore"
                        DisplayField="placName" 
                        ValueField="placID"
                        TypeAhead="false"
                        AllowBlank="false"
                        LoadingText="Searching..." 
                        Width="570"
                        PageSize="15"
                        ListWidth="240"
                        HideTrigger="true"
                        FieldLabel="走动地点"
                        ItemSelector="div.search-item" >
                        <Template ID="Template3" runat="server">
                           <tpl for=".">
                              <div class="search-item">
                                 <span>地点:{placName}</span>
                              </div>
                           </tpl>
                        </Template>
                        <Listeners>
                            <Render Fn="function(f) {
                                f.el.on('keyup', function(e) {
                                  if(window.event.keyCode==38 || window.event.keyCode==40 || window.event.keyCode==13){
                                        return;
                                     }
                                    Coolite.AjaxMethods.PYsearch(f.getRawValue(), 'PlaceSearchStore');
                                                           
                                });
                                }" />
                        </Listeners>
                    </ext:ComboBox>
                </ext:Anchor>
                <ext:Anchor Horizontal="95%">
                    <ext:NumberField 
                        ID="nf_Frequency" 
                        runat="server" 
                        FieldLabel="走动周期" 
                        AllowDecimals="False"
                        BlankText="周期不能为空!" 
                        AllowBlank="false" 
                        EmptyText="请输入整数天...">
                    </ext:NumberField>
                </ext:Anchor>
            </ext:FormLayout>
        </Body>
        <Buttons>
            <ext:Button ID="btn_Create" runat="server" Icon="Disk" Text="生成">
                <Listeners>
                    <Click Handler="Coolite.AjaxMethods.Savedata();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    <ext:Window 
        ID="Window2" 
        runat="server" 
        BodyStyle="padding:5px;" 
        ButtonAlign="Right"
        Frame="true" 
        Title="模板修改"
        AutoHeight="true"
        Width="275px"
        Modal="true"
        ShowOnLoad="false"
        X="160" Y="60">
        <Body>
            <ext:FormLayout ID="FormLayout1" runat="server" LabelWidth="65">
                <ext:Anchor Horizontal="95%">
                    <ext:ComboBox 
                    ID="up_dep" 
                    runat="server" 
                    FieldLabel="部门" 
                    BlankText="部门不能为空!" 
                    AllowBlank="false" 
                    EmptyText="请选择部门...."
                    DisplayField="deptName" 
                    ValueField="deptID" 
                    StoreID="DepartmentSearchStore" 
                    Editable="false" 
                    TypeAhead="true" 
                    Mode="Local"
                    Disabled="true"
                    ForceSelection="true" 
                    TriggerAction="All" 
                    SelectOnFocus="true"
                    >
                    </ext:ComboBox>
                </ext:Anchor>
                <ext:Anchor Horizontal="95%">
                    <ext:ComboBox 
                    ID="up_pos" 
                    runat="server" 
                    FieldLabel="职务" 
                    BlankText="职务不能为空!" 
                    AllowBlank="false" 
                    EmptyText="请选择职务...."
                    DisplayField="PosName" 
                    ValueField="PosID" 
                    StoreID="PosSearchStore" 
                    Editable="false" 
                    TypeAhead="true" 
                    Mode="Local"
                    Disabled="true"
                    ForceSelection="true" 
                    TriggerAction="All" 
                    SelectOnFocus="true"
                    >
                    </ext:ComboBox>
                </ext:Anchor>
                <ext:Anchor Horizontal="95%">
                    <ext:ComboBox 
                        ID="up_pla" 
                        runat="server" 
                        FieldLabel="地点" 
                        BlankText="地点不能为空!" 
                        AllowBlank="false" 
                        EmptyText="请选择地点...."
                        DisplayField="placName" 
                        ValueField="placID" 
                        StoreID="PlaceSearchStore"
                        TypeAhead="true" 
                        Mode="Local"
                        Disabled="true"
                        ForceSelection="true" 
                        TriggerAction="All"
                        >
                    </ext:ComboBox>
                </ext:Anchor>
                <ext:Anchor Horizontal="95%">
                    <ext:NumberField 
                        ID="up_fun" 
                        runat="server" 
                        FieldLabel="走动周期" 
                        AllowDecimals="False"
                        BlankText="周期不能为空!" 
                        AllowBlank="false" 
                        EmptyText="请输入整数天...">
                    </ext:NumberField>
                </ext:Anchor>
            </ext:FormLayout>
        </Body>
        <Buttons>
            <ext:Button ID="Button1" runat="server" Icon="Disk" Text="保存">
                <Listeners>
                    <Click Handler="Coolite.AjaxMethods.Updatedata();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                <Center>
                    <ext:GridPanel 
        ID="GridPanel1" 
        runat="server"
        StoreID="Store1"
        StripeRows="true"
        Title="走动周期模板维护"
        AutoExpandColumn="NIid" 
        Collapsible="false"
        Height="100">
        <ColumnModel ID="ColumnModel1" runat="server">
		    <Columns>
                <ext:Column ColumnID="NIid" Header="部门" Width="150" Sortable="true" DataIndex="DeptName" />
                <ext:Column Header="职务" Width="150" Sortable="true" DataIndex="PosName" />
                <ext:Column Header="走动地点" Width="150" Sortable="true" DataIndex="PlaceName" />
                <ext:Column Header="周期" Width="85" Sortable="true" DataIndex="Frequency" />
		    </Columns>
        </ColumnModel>
        <View>
            <ext:GridView ForceFit="true" />
        </View>
        <Plugins>
            <ext:GridFilters runat="server" ID="GridFilters1" Local="true">
                 <Filters>
                       <ext:StringFilter DataIndex="DeptName" />
                       <ext:StringFilter DataIndex="PosName" />
                       <ext:StringFilter DataIndex="PlaceName" />
                       <ext:NumericFilter DataIndex="Frequency" />
                 </Filters>
             </ext:GridFilters>
        </Plugins>
        <LoadMask ShowMask="true" Msg="数据加载中..." />
        <BottomBar>
            <ext:PagingToolBar ID="PagingToolBar1" runat="server" PageSize="15" />
         </BottomBar>
        <TopBar>
            <ext:Toolbar runat="server" ID="tb1">
                <Items>
                    <ext:Button runat="server" ID="ben_plan" Icon="Add" Text="生成走动周期" >
                        <Listeners>
                            <Click Handler="#{Window1}.show();" />
                        </Listeners>
                    </ext:Button>
                    <ext:ToolbarSeparator />
                    <ext:Button runat="server" ID="btn_update" Icon="ReportEdit" Text="修改" Disabled="true">
                        <Listeners>
                            <Click Handler="Coolite.AjaxMethods.LoadData();" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button runat="server" ID="btn_delete" Icon="ReportDelete" Text="删除" Disabled="true">
                        <Listeners>
                            <Click Handler="Coolite.AjaxMethods.delshow();" />
                        </Listeners>
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </TopBar>
        <SelectionModel>
                <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" SingleSelect="true" runat="server" />                   
        </SelectionModel>
        <AjaxEvents>
            <Click OnEvent="RowClick"></Click>
        </AjaxEvents>
        <Listeners>
            <BeforeShow Fn="function(el) { el.setHeight(Ext.getBody().getViewSize().height - 5); }" />
        </Listeners>
     </ext:GridPanel>
                </Center>
            </ext:BorderLayout>
        </Body>
    </ext:ViewPort>
    </div>
    </form>
</body>
</html>
