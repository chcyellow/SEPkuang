<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CT_manager.aspx.cs" Inherits="CHARGETABLE_CT_manager" %>

<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN" CleanResourceUrl="false" />
    <%--列表数据--%>
    <ext:Store ID="Store1" runat="server" OnRefreshData="MyData_Refresh" AutoLoad="true">
    <Proxy>
                <ext:DataSourceProxy>
                </ext:DataSourceProxy>
         </Proxy>
        <Reader>
            <ext:JsonReader ReaderID="Id">
                <Fields>
                    <ext:RecordField Name="Recordtime" Type="Date" DateFormat="Y-m-dTh:i:s" />
                    <ext:RecordField Name="Cdate" Type="Date" DateFormat="Y-m-dTh:i:s" />
                    <ext:RecordField Name="Cbanci" />
                    <ext:RecordField Name="Name" />
                    <ext:RecordField Name="Deptname" />
                    <ext:RecordField Name="Maindeptname" />
                    <ext:RecordField Name="Id" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store2" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="Id">
                <Fields>
                    <ext:RecordField Name="Recordtime" Type="Date" DateFormat="Y-m-dTh:i:s" />
                    <ext:RecordField Name="Placeid" />
                    <ext:RecordField Name="Placename" />
                    <ext:RecordField Name="Moveorder" />
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Ctid" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="placeStore" runat="server">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="Placeid" />
                    <ext:RecordField Name="Placename" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="placeTemplateStore" runat="server"  AutoLoad="false" OnRefreshData="CitiesRefresh">
        <Proxy>
                <ext:DataSourceProxy>
                </ext:DataSourceProxy>
         </Proxy>
        <Reader>
            <ext:JsonReader ReaderID="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                    <ext:RecordField Name="Level" />
                </Fields>
            </ext:JsonReader>
        </Reader>
        <Listeners>
            <Load Handler="#{cboPlaceTemplate}.insertItem(0, '--不使用模板--', -1);#{cboPlaceTemplate}.setValue(-1);" />
        </Listeners>
    </ext:Store>
    <ext:Store ID="perStore" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="Personnumber">
                <Fields>
                    <ext:RecordField Name="Personnumber" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="DeptStore" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="Deptnumber">
                <Fields>
                    <ext:RecordField Name="Deptnumber" />
                    <ext:RecordField Name="Deptname" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="YPPTDetailStore" runat="server"  OnRefreshData="YPPTDetailStore_RefershData" >
    <Proxy>
        <ext:DataSourceProxy>
        </ext:DataSourceProxy>
         </Proxy>
        <Reader>
            <ext:JsonReader  ReaderID="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="PptId" />
                    <ext:RecordField Name="Name" />
                    <ext:RecordField Name="PlaceId" />
                    <ext:RecordField Name="PlaceName" />
                    <ext:RecordField Name="MoveOrder" SortDir="ASC" />
                    <ext:RecordField Name="TLevel" />
                    <ext:RecordField Name="CreatePersonNumber" />
                    <ext:RecordField Name="CreatePersonName" />
                    <ext:RecordField Name="CreateMainDeptNumber" />
                    <ext:RecordField Name="CreateMainDeptName" />
                    <ext:RecordField Name="CreateTime"  Type="Date" DateFormat="Y-m-dTh:i:s"/>
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                <Center>
                    <%--数据列表--%>
                    <ext:GridPanel ID="GridPanel1" runat="server" StoreID="Store1" Height="300" StripeRows="true"
                        Title="领导跟带班情况" Collapsible="false">
                        <ColumnModel ID="ColumnModel1" runat="server">
                            <Columns>
                                <ext:Column Header="日期" Width="150" DataIndex="Cdate">
                                    <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" />
                                </ext:Column>
                                <ext:Column Header="班次" Width="100" DataIndex="Cbanci" />
                                <ext:Column Header="人员" Width="100" DataIndex="Name" />
                                <ext:Column Header="人员部门" Width="200" DataIndex="Deptname" />
                                <ext:Column Header="单位" Width="150" Sortable="true" DataIndex="Maindeptname" />
                                <ext:Column Header="录入时间" Width="200" DataIndex="Recordtime">
                                    <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d H:i:s')" />
                                </ext:Column>
                                <ext:Column Header="编号"  DataIndex="Id" Hidden="true" />
                            </Columns>
                        </ColumnModel>
                        <LoadMask ShowMask="true" Msg="数据加载中..." />
                        <BottomBar>
                            <ext:PagingToolbar ID="PagingToolBar1" runat="server" PageSize="15" StoreID="Store1" />
                        </BottomBar>
                        <TopBar>
                            <ext:Toolbar runat="server" ID="tb1">
                                <Items>
                                    <ext:Label runat="server" ID="lbldw" Text="单位:" />
                                    <ext:ComboBox ID="cbbDept" runat="server" StoreID="DeptStore" DisplayField="Deptname"
                                        ValueField="Deptnumber" FieldLabel="单位">
                                    </ext:ComboBox>
                                    <ext:Label runat="server" ID="Label1" Text="带班时间:" />
                                    <ext:DateField ID="cx_date" runat="server" Format="yyyy-MM-dd" Width="100" Vtype="daterange" />
                                    <ext:ComboBox ID="cx_banci" runat="server" Editable="false" >
                                        <Items>
                                            <ext:ListItem Text="早班" Value="早班" />
                                            <ext:ListItem Text="中班" Value="中班" />
                                            <ext:ListItem Text="夜班" Value="夜班" />
                                        </Items>
                                    </ext:ComboBox>
                                    <ext:Button runat="server" ID="btn_search" Icon="FolderMagnify" Text="查询">
                                        <Listeners>
                                            <Click Handler="#{Store1}.reload();" />
                                        </Listeners>
                                    </ext:Button>
                                    <%--<ext:Button runat="server" ID="btn_detail" Icon="FolderFind" Text="查看明细">
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.DetailLoad();" />
                                        </Listeners>
                                    </ext:Button>--%>
                                    <ext:ToolbarSeparator />
                                    <ext:Button runat="server" ID="btnNew" Icon="Add" Text="新增带班">
                                        <Listeners>
                                            <Click Handler="#{Window1}.show();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server" ID="btnPlan" Icon="ChartLine" Text="行走路线">
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.PlanLoad();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:ToolbarFill />
                                    <ext:Button runat="server" ID="btnDel" Icon="Delete" Text="删除带班">
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.DelmsgShow();" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <SelectionModel>
                            <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" SingleSelect="true" runat="server" />
                        </SelectionModel>
                        <AjaxEvents>
                            <Click OnEvent="RowClick">
                            </Click>
                        </AjaxEvents>
                    </ext:GridPanel>
                </Center>
            </ext:BorderLayout>
        </Body>
    </ext:ViewPort>
    <ext:Window ID="DetailWindow" runat="server" BodyStyle="padding:6px;" Frame="true"
        Title="计划行走路线" Height="390" Resizable="false" Width="660px" ShowOnLoad="false"
        Modal="true">
        <Body>
            <ext:FitLayout runat="server" ID="fitl1">
                <ext:GridPanel ID="GridPanel2" runat="server" StoreID="Store2" AutoExpandColumn="cold1"
                    Height="300" StripeRows="true" Header="false">
                    <ColumnModel ID="ColumnModel2" runat="server">
                        <Columns>
                            <ext:Column Header="走动地点" ColumnID="cold1" DataIndex="Placename" />
                            <ext:Column Header="走动次序" Width="70" DataIndex="Moveorder" />
                            <ext:Column Header="编号"  DataIndex="Id" Hidden="true" />
                        </Columns>
                    </ColumnModel>
                    <LoadMask ShowMask="true" Msg="数据加载中..." />
                    <TopBar>
                        <ext:Toolbar runat="server" ID="tb2">
                            <Items>
                                <ext:Label runat="server" ID="lblDetail" Text="" />
                                <ext:ToolbarSeparator />
                                <ext:ComboBox ID="cbbplace" runat="server" StoreID="placeStore" DisplayField="Placename"
                                    ValueField="Placeid" FieldLabel="地点" HideTrigger="true">
                                    <Listeners>
                                        <Render Fn="function(f) {
                                                            f.el.on('keyup', function(e) {
                                                            if(window.event.keyCode==38 || window.event.keyCode==40 || window.event.keyCode==13){
                                                                return;
                                                             }
                                                             Coolite.AjaxMethods.PYsearch(f.getRawValue(), 'placeStore');
                                                            });
                                                            }
                                                            " Delay="250" />
                                    </Listeners>
                                </ext:ComboBox>
                                <ext:Button runat="server" ID="btnPlaceAdd" Icon="Add" Text="添加地点">
                                    <Listeners>
                                        <Click Handler="Coolite.AjaxMethods.PlaceAdd();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:ToolbarSeparator />
                                <ext:Button runat="server" ID="btnPlaceDel" Icon="Delete" Text="删除选中">
                                    <Listeners>
                                        <Click Handler="Coolite.AjaxMethods.PlaceDel();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:ToolbarSeparator />
                                <ext:Button runat="server" ID="btnUp" Icon="ArrowUp">
                                    <Listeners>
                                        <Click Handler="Coolite.AjaxMethods.PlaceUp();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button runat="server" ID="btnDown" Icon="ArrowDown">
                                    <Listeners>
                                        <Click Handler="Coolite.AjaxMethods.PlaceDown();" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <SelectionModel>
                        <ext:CheckboxSelectionModel ID="CheckboxSelectionModel2" runat="server" />
                    </SelectionModel>
                    <AjaxEvents>
                        <Click OnEvent="RowClick1">
                        </Click>
                    </AjaxEvents>
                </ext:GridPanel>
            </ext:FitLayout>
        </Body>
    </ext:Window>
    <ext:Window ID="Window1" runat="server" BodyStyle="padding:5px;" ButtonAlign="Right"
        Frame="true" Title="新增带班" AutoHeight="true" Width="300px" Modal="true" ShowOnLoad="false">
        <Body>
            <ext:FormLayout ID="FormLayout2" runat="server" LabelWidth="60">
                <ext:Anchor Horizontal="95%">
                    <ext:DateField ID="df_begin" runat="server" Format="yyyy-MM-dd" FieldLabel="日期<font color='red'>*</font>"
                        Width="100" Vtype="daterange" />
                </ext:Anchor>
                <ext:Anchor Horizontal="95%">
                    <ext:ComboBox ID="cbbBc" runat="server" Editable="false" FieldLabel="班次<font color='red'>*</font>"
                        SelectedIndex="0">
                        <Items>
                            <ext:ListItem Text="早班" Value="早班" />
                            <ext:ListItem Text="中班" Value="中班" />
                            <ext:ListItem Text="夜班" Value="夜班" />
                        </Items>
                    </ext:ComboBox>
                </ext:Anchor>
                <ext:Anchor Horizontal="95%">
                    <ext:ComboBox ID="cbbPerson" runat="server" PageSize="10" FieldLabel="人员<font color='red'>*</font>"
                            AllowBlank="false" StoreID="perStore" DisplayField="Name" ValueField="Personnumber"
                             Editable="false">
                            <Listeners>
                              <Select Handler="#{placeTemplateStore}.reload();" />
                            </Listeners>
                        </ext:ComboBox>
                    <%--<ext:ComboBox ID="cbbPerson" runat="server" PageSize="10" FieldLabel="人员<font color='red'>*</font>"
                        AllowBlank="false" StoreID="perStore" DisplayField="Name" ValueField="Personnumber"
                        HideTrigger="true">
                        <Listeners>
                            <Render Fn="function(f) {
                                                            f.el.on('keyup', function(e) {
                                                            if(window.event.keyCode==38 || window.event.keyCode==40 || window.event.keyCode==13){
                                                                return;
                                                             }
                                                            Coolite.AjaxMethods.PYsearch(f.getRawValue(), 'perStore');
                                                           
                                                            });
                                                            }
                                                            " />
                          <Select Handler="#{cboPlaceTemplate}.clearValue(); #{placeTemplateStore}.reload();" />
                        </Listeners>
                    </ext:ComboBox>--%>
                </ext:Anchor>
                <ext:Anchor Horizontal="95%">
                
                 <ext:ComboBox ID="cboPlaceTemplate" runat="server" Editable="false" FieldLabel="带班模板" StoreID="placeTemplateStore"  ValueField="Id" DisplayField="Name" 
                    EmptyText="--不使用模板--"  
                    ValueNotFoundText="--不使用模板--"
                    ForceSelection="true" >
                    <Listeners>
                    <Select Handler="Coolite.AjaxMethods.Changed();" /> 
                            </Listeners>
                    </ext:ComboBox>               
                </ext:Anchor >
                
            </ext:FormLayout>
        </Body>
        <Buttons>
        
            <ext:Button runat="server" ID="btnLookRoute" Text="查看线路">
            <Listeners>
                                            <Click Handler="#{Window2}.show();" />
                                        </Listeners>
            </ext:Button>
                
            <ext:Button ID="Button4" runat="server" Icon="Disk" Text="确 定">
                <Listeners>
                    <Click Handler="Coolite.AjaxMethods.BaseSave( {
                    success: function(result) {

                        Ext.Msg.alert('提示', result);

                    },
                    eventMask: {

                        showMask: true,

                        minDelay: 250

                    }

                });" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    <ext:Window ID="Window2" runat="server" BodyStyle="padding:6px;" Frame="true"
        Title="模板行走路线" Height="390" Resizable="false" Width="660px" ShowOnLoad="false"
        Modal="true">
        <Body>
            <ext:FitLayout runat="server" ID="FitLayout1">
                <ext:GridPanel ID="GridPanel3" runat="server" StoreID="YPPTDetailStore" AutoExpandColumn="cold1"
                    Height="300" StripeRows="true" Header="false">
                    <ColumnModel ID="ColumnModel3" runat="server">
                        <Columns>
                            <ext:Column Header="走动地点ID"  DataIndex="PlaceId" Hidden="true" />
                            <ext:Column Header="走动地点" ColumnID="cold1" DataIndex="PlaceName" />
                            <ext:Column Header="走动次序" Width="70" DataIndex="MoveOrder"  />
                        </Columns>
                    </ColumnModel>
                    <LoadMask ShowMask="true" Msg="数据加载中..." />
                </ext:GridPanel>
            </ext:FitLayout>
        </Body>
    </ext:Window>
    </form>
</body>
</html>
