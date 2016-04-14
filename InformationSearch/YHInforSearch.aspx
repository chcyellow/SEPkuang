<%@ Page Language="C#" AutoEventWireup="true" CodeFile="YHInforSearch.aspx.cs" Inherits="InformationSearch_YHInforSearch" %>

<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Style/examples.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        var saveData = function() {
            GridData.setValue(Ext.encode(GridPanel1.getRowsValues(false)));
        }
    </script>
    <style type="text/css">
        .x-form-group .x-form-group-header-text {
        	background-color: #dfe8f6;
        }
        
        .x-label-text {
            font-weight: bold;
            font-size: 11px;
        }
        .x-textfeild-style  
        {
            BORDER-RIGHT: #000000 0px solid; 
            BORDER-TOP: #000000 0px solid; 
            BORDER-LEFT: #000000 0px solid; 
            BORDER-BOTTOM: #000000 1px solid
        }
    </style>
    <script type="text/javascript">
        var template = '<span style="color:{0};">{1}</span>';

        var change = function(value) {
            var color;
            if (value == '新增')
                color = 'red';
            else if (value == '复查未通过' || value == '逾期未整改' || value == '逾期整改未完成')
                color = 'orange';
            else if (value == '现场整改')
                color = 'gray';
            else
                color = 'green';
            return String.format(template, color, value);
        }
    </script>
    <script type="text/javascript">
        var template = '<span style="color:{0};"><b>{1}</b></span>';

        var changeColor = function(value) {
            var color;
            if (value.toString().replace(/(^\s*)|(\s*$)/g, "") == 'A')
                color = '#cc0000';
            else if (value.toString().replace(/(^\s*)|(\s*$)/g, "") == 'B')
                color = '#FF9900';
            else if (value.toString().replace(/(^\s*)|(\s*$)/g, "") == 'C')
                color = 'black';
            else
                color = 'blue';
            return String.format(template, color, value);
        }
    </script>
    <script type="text/javascript">
        var qtip = function(v, p) {//单元格提示
            //v : value , p : cell
            p.attr = 'ext:qtitle="" ext:qtip="' + v + '"';
            return v;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style=" text-align:center">
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN" CleanResourceUrl="false" />
    <ext:Hidden ID="GridData" runat="server" />
    <ext:Hidden ID="hidBtn" runat="server" />
    <%--列表数据--%>
    <ext:Store ID="Store1" runat="server" OnRefreshData="MyData_Refresh" AutoLoad="false">
        <Proxy>
        <ext:DataSourceProxy>
        </ext:DataSourceProxy>
         </Proxy>
        <Reader>
            <ext:JsonReader ReaderID="YHPutinID">
                <Fields>
                    <ext:RecordField Name="YHPutinID" Type="Int" />
                    <ext:RecordField Name="DeptName" />
                    <ext:RecordField Name="Placename" />
                    <ext:RecordField Name="BanCi" />
                    <ext:RecordField Name="YHContent" />
                    <ext:RecordField Name="Name" />
                    <ext:RecordField Name="PCTime" Type="Date" DateFormat="Y-m-dTh:i:s" />
                    <ext:RecordField Name="YHType" />
                    <ext:RecordField Name="Maindeptname" />
                    <ext:RecordField Name="Levelname" />
                    <ext:RecordField Name="Status" />
                    <ext:RecordField Name="zgrId" />
                    <ext:RecordField Name="zgr" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <%--隐患查询职务-人员处理--%>
    <ext:Store ID="Store2" runat="server" AutoLoad="false" OnRefreshData="PersonRefresh">
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
        <Listeners>
            <Load Handler="#{cbb_person}.setValue(#{cbb_person}.store.getAt(0).get('Personid'));" />
        </Listeners>
    </ext:Store>
    <ext:Store ID="personStore" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="PersonID">
                <Fields>
                    <ext:RecordField Name="PersonID" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store3" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="Posid">
                <Fields>
                    <ext:RecordField Name="Posid" />
                    <ext:RecordField Name="Posname" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="DeptStore" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="DeptID">
                <Fields>
                    <ext:RecordField Name="Deptid" />
                    <ext:RecordField Name="Deptname" />
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
    <%--<ext:Store ID="LevelStore" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="YHLevelID">
                <Fields>
                    <ext:RecordField Name="YHLevelID" Type="Int" />
                    <ext:RecordField Name="YHLevel" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>--%>
     <ext:Store ID="TypeStore" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="YHTypeID">
                <Fields>
                    <ext:RecordField Name="YHTypeID" Type="Int" />
                    <ext:RecordField Name="YHType" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <%--获取状态--%>
    <asp:XmlDataSource ID="XmlDataSource1" runat="server" DataFile="../HiddenDanage/Status.xml"
            TransformFile="../HiddenDanage/Status.xsl" >
            </asp:XmlDataSource>
    <ext:Store runat="server" ID="StatusStore" DataSourceID="XmlDataSource1">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="Value" />
                    <ext:RecordField Name="Manage" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:BorderLayout ID="BorderLayout1" runat="server">
                    <Center>
                         <%--数据列表--%>
    <ext:GridPanel 
        ID="GridPanel1" 
        runat="server"
        StoreID="Store1" Height="300"
        StripeRows="true"
        Title="隐患信息综合查询"
        AutoExpandColumn="NIid" 
        Collapsible="false"
        >
        <ColumnModel ID="ColumnModel1" runat="server">
		    <Columns>
                <ext:Column Header="隐患编号" Width="100" DataIndex="YHPutinID" />
                <ext:Column Header="隐患部门" Width="70" DataIndex="DeptName" />
                <ext:Column Header="隐患地点" Width="70" DataIndex="Placename" />
                <ext:Column Header="班次" Width="70" DataIndex="BanCi" />
                <ext:Column ColumnID="NIid" Header="隐患内容" Sortable="false" DataIndex="YHContent">
                    <Renderer Fn="qtip" />
                </ext:Column>
                <ext:Column Header="排查人员" Width="70" DataIndex="Name" />
                <ext:Column Header="排查时间" Width="70" Sortable="true" DataIndex="PCTime" >
                    <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" />
                </ext:Column>
                <%--<ext:Column Header="隐患级别" Width="70" Sortable="true" DataIndex="YHLevel" >
                <Renderer Fn="changeColor" />
                </ext:Column>--%>
                <ext:Column Header="隐患类型" Width="70" Sortable="true" DataIndex="YHType" />
                <ext:Column Header="隐患级别" Width="70" Sortable="true" DataIndex="Levelname">
                    <Renderer Fn="changeColor" />
                </ext:Column>
                <ext:Column Header="隐患状态" Width="100" Sortable="true" DataIndex="Status">
                    <Renderer Fn="change" />
                </ext:Column>
                <ext:Column Header="隐患单位" Width="70" Sortable="true" DataIndex="Maindeptname" />
		    </Columns>
        </ColumnModel>
        <Plugins>
            <ext:GridFilters runat="server" ID="GridFilters1" Local="true">
                 <Filters>
                       <ext:StringFilter DataIndex="ID" />
                       <ext:StringFilter DataIndex="Name" />
                       <ext:StringFilter DataIndex="PlaceName" />
                       <ext:DateFilter DataIndex="StartTime">
                            <DatePickerOptions runat="server" TodayText="Now" />
                       </ext:DateFilter>
                       <ext:DateFilter DataIndex="EndTime">
                            <DatePickerOptions runat="server" TodayText="Now" />
                       </ext:DateFilter>
                 </Filters>
             </ext:GridFilters>
        </Plugins>
        <LoadMask ShowMask="true" Msg="数据加载中..." />
        <BottomBar>
            <ext:PagingToolBar ID="PagingToolBar1" runat="server" PageSize="15" StoreID="Store1" />
         </BottomBar>
        <TopBar>
            <ext:Toolbar runat="server" ID="tb1">
                <Items>
                    <ext:DateField ID="dateS" runat="server" Format="yyyy-MM-dd" FieldLabel="起始日期"  Width="100" Vtype="daterange">       
                    </ext:DateField>
                    <ext:DateField ID="dateE" runat="server" Format="yyyy-MM-dd" FieldLabel="截止日期" Width="100" Vtype="daterange" >
                     </ext:DateField>
                     <ext:ComboBox
                                ID="cboDept" 
                                runat="server"   
                                EmptyText="隐患部门.."
                                DisplayField="Deptname" 
                                ValueField="Deptid" 
                                StoreID="DeptStore"
                                TypeAhead="true" 
                                Mode="Local" ListWidth="200"
                                ForceSelection="true" 
                                TriggerAction="All"
                                Width="100"
                                Editable="false"
                                >
                     </ext:ComboBox>
                     <ext:TextField runat="server" ID="txtPerson" EmptyText="排查人员.." ></ext:TextField>
                     <ext:ComboBox 
                        ID="cboStatus" 
                        runat="server" 
                        FieldLabel="隐患状态" 
                        EmptyText="请选择状态.."
                        DisplayField="Value" 
                        ValueField="Value" 
                        StoreID="StatusStore"
                        TypeAhead="true" 
                        Mode="Local"
                        ForceSelection="true" 
                        TriggerAction="All"
                        Width="100"
                        Editable="false"
                        >
                        <Listeners>
                            <Select Handler="if(#{cboStatus}.getRawValue().toString() == '隐患已整改') {#{txtZGR}.setDisabled(false); } else { #{txtZGR}.clearValue();#{txtZGR}.setDisabled(true);}" />
                        </Listeners>
                    </ext:ComboBox>
                    <ext:TextField runat="server" ID="txtZGR" EmptyText="整改人.." ></ext:TextField>
                     <ext:Button runat="server" ID="btnSure" Icon="Find" Text="查询" >
                     
                       <Listeners>
                            <%--<Click Handler="Coolite.AjaxMethods.Search2({ eventMask: {

                                showMask: true,
                                msg: '正在查询，请稍后...',
                                minDelay: 500

                            }});" />--%>
                            <Click Handler="#{hidBtn}.setValue('1');#{Store1}.reload();" />
                       </Listeners>
                    </ext:Button>
                    <ext:ToolbarSeparator />
                    <ext:Button runat="server" ID="btn_search" Icon="FolderMagnify" Text="高级查询" >
                        <Listeners>
                            <Click Handler="#{Window1}.show();" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button runat="server" ID="btn_detail" Icon="FolderFind" Text="查看明细" >
                        <Listeners>
                            <Click Handler="Coolite.AjaxMethods.DetailLoad();" />
                        </Listeners>
                    </ext:Button>
                    <ext:ToolbarSeparator />
                    <%--<ext:ToolbarFill />
                    <ext:Button ID="btn_print" runat="server" Text="导出报表" Icon="PageExcel" AutoPostBack="true" OnClick="ToExcel" >
                        <Listeners>
                            <Click Fn="saveData" />
                        </Listeners>
                    </ext:Button>--%>
                </Items>
            </ext:Toolbar>
        </TopBar>
        <SelectionModel>
                <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" SingleSelect="true" runat="server" />                   
        </SelectionModel>
        <AjaxEvents>
            <Click OnEvent="RowClick"></Click>
        </AjaxEvents>
     </ext:GridPanel>
                    </Center>
                </ext:BorderLayout>
            </Body>
        </ext:ViewPort>
   
     <%--查询条件--%>
     <ext:Window 
        ID="Window1" 
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
            <ext:FormLayout ID="FormLayout2" runat="server" LabelWidth="60">
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
                <ext:MultiField ID="MultiField1" runat="server" FieldLabel="排查人">
                        <Fields>
                            <ext:ComboBox 
                                ID="cbb_position" 
                                runat="server" 
                                EmptyText="请选择职务.."
                                DisplayField="Posname" 
                                ValueField="Posid" 
                                StoreID="Store3" 
                                Editable="false" 
                                TypeAhead="true" 
                                Mode="Local"
                                ForceSelection="true" 
                                TriggerAction="All" 
                                SelectOnFocus="true"
                                Note="职务"
                                Width="100"
                                >
                                <Listeners>
                                    <Select Handler="#{cbb_person}.clearValue(); #{Store2}.reload();" />
                                </Listeners>
                            </ext:ComboBox>
                            <ext:ComboBox 
                                ID="cbb_person" 
                                runat="server" 
                                EmptyText="请先选择人员.."
                                DisplayField="Name" 
                                ValueField="Personnumber" 
                                StoreID="Store2"
                                TypeAhead="true" 
                                Mode="Local"
                                ForceSelection="true" 
                                TriggerAction="All"
                                Note="人员"
                                Width="100"
                                Editable="false"
                                >
                            </ext:ComboBox>
                        </Fields>
                    </ext:MultiField>
                    
                </ext:Anchor>
                <ext:Anchor Horizontal="95%">
                    <ext:MultiField ID="MultiField2" runat="server" FieldLabel="编号/部门">
                        <Fields>
                            <ext:NumberField ID="nf_ID" runat="server" EmptyText="请填写隐患编号.." Width="100" MsgTarget="Under" AllowDecimals="False" AllowNegative="False" />
                            <ext:ComboBox 
                                ID="cbb_part" 
                                runat="server"   
                                EmptyText="请选择部门.."
                                DisplayField="Deptname" 
                                ValueField="Deptid" 
                                StoreID="DeptStore"
                                TypeAhead="true" 
                                Mode="Local" ListWidth="200"
                                ForceSelection="true" 
                                TriggerAction="All"
                                Width="100"
                                Editable="false"
                                >
                            </ext:ComboBox>
                        </Fields>
                    </ext:MultiField>
                </ext:Anchor>
                <ext:Anchor Horizontal="95%">
                    <ext:MultiField ID="MultiField3" runat="server" FieldLabel="类型/单位">
                        <Fields>
                            <%--<ext:ComboBox 
                                ID="cbb_lavel" 
                                runat="server"  
                                EmptyText="请选择级别.."
                                DisplayField="YHLevel" 
                                ValueField="YHLevelID" 
                                StoreID="LevelStore"
                                Mode="Local"
                                Width="100"
                                Editable="false"
                                >
                            </ext:ComboBox>--%>
                            <ext:ComboBox 
                                ID="cbb_kind" 
                                runat="server"  
                                EmptyText="请选择类型.."
                                DisplayField="YHType" 
                                ValueField="YHTypeID" 
                                StoreID="TypeStore"
                                TypeAhead="true" 
                                Mode="Local"
                                ForceSelection="true" 
                                TriggerAction="All"
                                Width="100"
                                Editable="false"
                                >
                            </ext:ComboBox>
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
                        </Fields>
                    </ext:MultiField>
                </ext:Anchor>
                <ext:Anchor Horizontal="95%">
                    <ext:MultiField ID="MultiField4" runat="server" FieldLabel="状态/方式">
                        <Fields>
                            <ext:ComboBox 
                                ID="cbb_status" 
                                runat="server" 
                                FieldLabel="隐患状态" 
                                EmptyText="请选择状态.."
                                DisplayField="Value" 
                                ValueField="Value" 
                                StoreID="StatusStore"
                                TypeAhead="true" 
                                Mode="Local"
                                ForceSelection="true" 
                                TriggerAction="All"
                                Width="100"
                                Editable="false"
                                >
                            </ext:ComboBox>
                            <ext:ComboBox
                                ID="cbb_jctype" 
                                runat="server" 
                                FieldLabel="排查方式"
                                Editable="false"
                                Width="100"
                                >
                                <Items>
                                    <ext:ListItem Text="矿查" Value="0" />
                                    <ext:ListItem Text="自查" Value="1" />
                                    <ext:ListItem Text="上级排查" Value="2" />
                                </Items>
                            </ext:ComboBox>
                         </Fields>
                    </ext:MultiField>
                </ext:Anchor>
                <ext:Anchor Horizontal="95%">
                    <ext:ComboBox
                        ID="cbbBHstatus" 
                        runat="server" 
                        FieldLabel="闭合状态"
                        Editable="false"
                        Width="100"
                        >
                        <Items>
                            <ext:ListItem Text="未闭合" Value="0" />
                            <ext:ListItem Text="已闭合" Value="1" />
                        </Items>
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
                    <%--<Click Handler="Coolite.AjaxMethods.Search();" />--%>
                    <Click Handler="#{hidBtn}.setValue('2');#{Store1}.reload();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    <%--明细窗口--%>
    <ext:Window 
        ID="DetailWindow" 
        runat="server" 
        BodyStyle="padding:6px;" 
        ButtonAlign="Right"
        Frame="true" 
        Title="隐患明细信息"
        Height="400" AutoScroll="true" Resizable="false"
        Width="700px"
        ShowOnLoad="false"
        Y="1">
        <Tools>
            <ext:Tool Type="Refresh" Handler="Coolite.AjaxMethods.DetailLoad();" />
        </Tools>
         <AutoLoad Mode="IFrame" MaskMsg="信息正在加载，请稍候...." ShowMask="true" />
        
    </ext:Window>
    </div>
    </form>
</body>
</html>
