<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HDprocess.aspx.cs" Inherits="HiddenDanage_HDprocess" %>

<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../style/examples.css" rel="stylesheet" type="text/css" />
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
    <script language="javascript" type="text/javascript">
        var saveData = function() {
            GridData.setValue(Ext.encode(GridPanel1.getRowsValues(false)));
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style=" text-align:center">
    <ext:ScriptManager ID="ScriptManager1" runat="server" CleanResourceUrl="false" Locale="zh-CN" />
    <ext:Hidden ID="GridData" runat="server" />
    <%--列表数据--%>
    <ext:Store ID="Store1" runat="server" OnRefreshData="MyData_Refresh">
    
        <Reader>
            <ext:JsonReader ReaderID="Yhputinid">
                <Fields>
                    <ext:RecordField Name="Yhputinid" Type="Int" />
                    <ext:RecordField Name="Deptname" />
                    <ext:RecordField Name="Placename" />
                    <ext:RecordField Name="Banci" />
                    <ext:RecordField Name="HBm" />
                    <ext:RecordField Name="Personname" />
                    <ext:RecordField Name="Pctime" Type="Date" DateFormat="Y-m-dTh:i:s" />
                    <%--<ext:RecordField Name="Intime" Type="Date" DateFormat="Y-m-dTh:i:s" />--%>
                    <ext:RecordField Name="Levelname" />
                    <ext:RecordField Name="Zyname" />
                    <ext:RecordField Name="Remarks" />
                    <ext:RecordField Name="Status" />
                </Fields>
                 
            </ext:JsonReader>
            
        </Reader>              
    </ext:Store>
    <%--隐患地点--%>
    <ext:Store ID="placeStore" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="placID" />
                        <ext:RecordField Name="placName" />
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
            <Load Handler="#{cbb_person}.setValue(#{cbb_person}.store.getAt(0).get('Personnumber'));" />
        </Listeners>
    </ext:Store>
    <%--隐患提交处理时发送短信人员处理--%>
    <ext:Store ID="perStore" runat="server" AutoLoad="false" OnRefreshData="perRefresh">
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
        <%--<Listeners>
            <Load Handler="#{refer_Rname}.setValue(#{refer_Rname}.store.getAt(0).get('Personnumber'));" />
        </Listeners>--%>
    </ext:Store>
    <%--隐患提交处理时责任人员处理--%>
    <ext:Store ID="PersStore" runat="server" AutoLoad="false" OnRefreshData="PersRefresh">
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
        <%--<Listeners>
            <Load Handler="#{fb_zrr}.setValue(#{fb_zrr}.store.getAt(0).get('Personnumber'));" />
        </Listeners>--%>
    </ext:Store>
    <ext:Store ID="persoStore" runat="server" AutoLoad="false" OnRefreshData="PersRefresh1">
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
            <Load Handler="#{yq_zrr}.setValue(#{yq_zrr}.store.getAt(0).get('Personnumber'));" />
        </Listeners>
    </ext:Store>
    <ext:Store ID="personStore" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="Personnumber">
                <Fields>
                    <ext:RecordField Name="Personnumber" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="FBBigleader" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="Personnumber">
                <Fields>
                    <ext:RecordField Name="Personnumber" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <%--隐患提交领导-人员处理--%>
    <ext:Store ID="FBleader" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="Personnumber">
                <Fields>
                    <ext:RecordField Name="Personnumber" />
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
            <ext:JsonReader ReaderID="Deptnumber">
                <Fields>
                    <ext:RecordField Name="Deptnumber"/>
                    <ext:RecordField Name="Deptname" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    
    <ext:Store ID="LevelStore" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="YHLevelID">
                <Fields>
                    <ext:RecordField Name="YHLevelID" />
                    <ext:RecordField Name="YHLevel" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
     <ext:Store ID="TypeStore" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="YHTypeID">
                <Fields>
                    <ext:RecordField Name="YHTypeID" />
                    <ext:RecordField Name="YHType" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <%--获取状态--%>
    <asp:XmlDataSource ID="XmlDataSource1" runat="server" DataFile="Status.xml"
            TransformFile="Status.xsl" >
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
    <%--数据列表--%>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:BorderLayout ID="BorderLayout1" runat="server">
                    <Center>
                        <ext:GridPanel 
        ID="GridPanel1" 
        runat="server"
        StoreID="Store1"
        StripeRows="true"
        Title="隐患信息"
        AutoExpandColumn="NIid" 
        Collapsible="false" 
        Width="810px"
        >
        <ColumnModel ID="ColumnModel1" runat="server">
		    <Columns>
                <ext:Column Header="隐患编号" Width="100" DataIndex="Yhputinid" />
                <ext:Column Header="隐患部门" Width="70" DataIndex="Deptname" />
                <ext:Column Header="隐患地点" Width="70" DataIndex="Placename" />
                <ext:Column Header="班次" Width="70" DataIndex="Banci" />
                <ext:Column ColumnID="NIid" Header="隐患内容" Width="120" Sortable="false" DataIndex="HBm">
                    <Renderer Fn="qtip" />
                </ext:Column>
                <ext:Column Header="排查人员" Width="70" DataIndex="Personname" />
                <ext:Column Header="排查时间" Width="70" Sortable="true" DataIndex="Pctime" >
                    <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" />
                </ext:Column>
                <ext:Column Header="隐患级别" Width="70" Sortable="true" DataIndex="Levelname" >
                <Renderer Fn="changeColor" />
                </ext:Column>
                 
                <ext:Column Header="隐患类型" Width="70" Sortable="true" DataIndex="Zyname" />
                <ext:Column Header="备注" Width="70" Sortable="true" DataIndex="Remarks" Hidden="true" />
                <ext:Column Header="隐患状态" Width="100" Sortable="true" DataIndex="Status">
                    <Renderer Fn="change" />
                </ext:Column>
		    </Columns>
        </ColumnModel>
        <Plugins>
            <ext:GridFilters runat="server" ID="GridFilters1" Local="true">
                 <Filters>
                       <ext:StringFilter DataIndex="Yhputinid" />
                       <ext:StringFilter DataIndex="Personname" />
                       <ext:StringFilter DataIndex="Placename" />
                       <ext:DateFilter DataIndex="StartTime">
                            <DatePickerOptions runat="server" TodayText="Now" />
                       </ext:DateFilter>
                       <ext:DateFilter DataIndex="EndTime">
                            <DatePickerOptions runat="server" TodayText="Now" />
                       </ext:DateFilter>
                 </Filters>
             </ext:GridFilters>
        </Plugins>
        <LoadMask ShowMask="true" />
        <BottomBar>
         <ext:PagingToolbar runat="server" ID="pageToolBar" StoreID="Store1"></ext:PagingToolbar>
       </BottomBar>

        <TopBar>
            <ext:Toolbar runat="server" ID="tb1">
                <Items>
                    <ext:Button runat="server" ID="btn_search" Icon="FolderMagnify" Text="条件查询" >
                        <Listeners>
                            <Click Handler="#{Window1}.show();" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button runat="server" ID="btn_detail" Icon="FolderFind" Text="查看明细" >
                        <Listeners>
                            <Click Handler="Coolite.AjaxMethods.DetailLoad();#{DetailWindow}.show();" />
                        </Listeners>
                    </ext:Button>
                    <ext:ToolbarSeparator />
                    <ext:Button runat="server" ID="btn_publish" Icon="FolderUp" Text="发布/提交">
                        <Listeners>
                            <Click Handler="Coolite.AjaxMethods.detailFine();Coolite.AjaxMethods.LoadHazard();#{Window2}.show();#{Tab1}.show();" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button runat="server" ID="btn_zgfk" Icon="FolderUser" Text="整改反馈">
                        <Listeners>
                            <Click Handler="Coolite.AjaxMethods.detailzgr();#{Window3}.show();" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button runat="server" ID="btn_fcfk" Icon="FolderUser" Text="复查反馈">
                        <Listeners>
                            <Click Handler="Coolite.AjaxMethods.detailfcr();#{Window4}.show();" />
                        </Listeners>
                    </ext:Button>
                    <ext:ToolbarSeparator runat="server" ID="ts2" />
                    <ext:Button runat="server" ID="Button5" Icon="FolderTable" Text="闭合新增隐患">
                        <Listeners>
                            <Click Handler="Coolite.AjaxMethods.YHycfkLoad1();#{Window6}.show();" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button runat="server" ID="Button8" Icon="FolderTable" Text="分级流程处理">
                        <Listeners>
                            <Click Handler="Coolite.AjaxMethods.yqDataLoad();#{YQWindow}.show();" />
                        </Listeners>
                    </ext:Button>
                    <ext:ToolbarSeparator runat="server" ID="ts3" />
                    <ext:Button runat="server" ID="btn_action" Icon="FolderTable" Text="异常流程处理">
                        <Listeners>
                            <Click Handler="Coolite.AjaxMethods.YHycfkLoad();#{Window5}.show();" />
                        </Listeners>
                    </ext:Button>
                    <ext:ToolbarFill />
                    <%--<ext:Button runat="server" ID="btn_AgStatus" Icon="FolderDelete" Text="状态置不受理" Visible="false">
                        <Listeners>
                            <Click Handler="Coolite.AjaxMethods.delshow();" />
                        </Listeners>
                    </ext:Button>--%>
                    <ext:Button runat="server" ID="btn_AgStatus" Icon="FolderDelete" Text="删除">
                        <Listeners>
                            <Click Handler="Coolite.AjaxMethods.delshow();" />
                        </Listeners>
                    </ext:Button>
                    <ext:ToolbarSeparator runat="server" ID="ts4" />
                    <ext:Button ID="btn_print" runat="server" Text="导出报表" Icon="PageExcel" AutoPostBack="true" OnClick="ToExcel">
                        <Listeners>
                            <Click Fn="saveData" />
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
            <DblClick OnEvent="DblClick"></DblClick>
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
                                <Listeners>
                                    <Render Handler="this.endDateField = '#{df_end}'" />
                                </Listeners>
                            </ext:DateField>
                            <ext:DateField ID="df_end" runat="server" Format="yyyy-MM-dd" Note="截止日期" Width="100" Vtype="daterange" >
                                <Listeners>
                                    <Render Handler="this.startDateField = '#{df_begin}'" />
                                </Listeners>
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
                                EmptyText="请先选择职务.."
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
                                ValueField="Deptnumber" 
                                StoreID="DeptStore"
                                TypeAhead="true" 
                                Mode="Local"
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
                    <ext:MultiField ID="MultiField3" runat="server" FieldLabel="级别/类型">
                        <Fields>
                            <ext:ComboBox 
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
                            </ext:ComboBox>
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
                        </Fields>
                    </ext:MultiField>
                </ext:Anchor>
                <ext:Anchor Horizontal="59%">
                    <ext:MultiField ID="MultiField4" runat="server" FieldLabel="状态/地点">
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
                                ID="cbb_place" 
                                runat="server" 
                                FieldLabel="隐患状态" 
                                EmptyText="请选择地点.."
                                StoreID="placeStore"
                                DisplayField="placName" 
                                ValueField="placID"
                                TypeAhead="true" 
                                Mode="Local"
                                ForceSelection="true" 
                                TriggerAction="All"
                                Width="100"
                                Editable="false"
                                >
                            </ext:ComboBox>
                        </Fields>
                    </ext:MultiField>
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
    <%--明细窗口/Modal去掉了--%>
    <ext:Window 
        ID="DetailWindow" 
        runat="server" 
        BodyStyle="padding:6px;" 
        ButtonAlign="Right"
        Frame="true" 
        Title="隐患明细信息"
        AutoHeight="true"
        Width="600px"
        ShowOnLoad="false"
        Y="1">
        <Tools>
            <ext:Tool Type="Refresh" Handler="Coolite.AjaxMethods.DetailLoad();" />
        </Tools>
        <LoadMask Msg="信息正在加载，请稍候...." ShowMask="true" />
        <Body>
            <ext:ContainerLayout ID="ContainerLayout1" runat="server">
                <ext:Panel 
                    ID="BasePanel" 
                    runat="server" 
                    Title="隐患基本信息" 
                    AutoHeight="true" 
                    FormGroup="true"
                    Width="580px"
                    >
                    <Body>
                        <table style="width:100%;">
                            <tr>
                                <td style="width:33%;">
                                    <span class="x-label-text">隐患编号:</span>
                                    <ext:TextField ID="lbl_YHPutinID" runat="server" Cls="x-textfeild-style" />
                                </td>
                                <td style="width:34%;">
                                    <span class="x-label-text">隐患部门:</span>
                                    <ext:TextField ID="lbl_DeptName" runat="server" Cls="x-textfeild-style" />
                                </td>
                                <td style="width:33%;">
                                    <span class="x-label-text">隐患地点:</span>
                                    <ext:TextField ID="lbl_PlaceName" runat="server" Cls="x-textfeild-style" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width:33%;">
                                    <span class="x-label-text">发生班次:</span>
                                    <ext:TextField ID="lbl_BanCi" runat="server" Cls="x-textfeild-style" />
                                </td>
                                <td style="width:34%;">
                                    <span class="x-label-text">排查人员:</span>
                                    <ext:TextField ID="lbl_rName" runat="server" Cls="x-textfeild-style" />
                                </td>
                                <td style="width:33%;">
                                    <span class="x-label-text">排查时间:</span>
                                    <ext:TextField ID="lbl_PCTime" runat="server" Cls="x-textfeild-style" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width:33%;">
                                    <span class="x-label-text">隐患级别:</span>
                                    <ext:TextField ID="lbl_YHLevel" runat="server" Cls="x-textfeild-style" />
                                </td>
                                <td style="width:34%;">
                                    <span class="x-label-text">隐患类型:</span>
                                    <ext:TextField ID="lbl_YHType" runat="server" Cls="x-textfeild-style" />
                                </td>
                                <td style="width:33%;">
                                    <span class="x-label-text">当前状态:</span>
                                    <ext:TextField ID="lbl_Status" runat="server" Cls="x-textfeild-style" />
                                </td>
                            </tr>
                            <tr>
                                 <td colspan="3">
                                    <span class="x-label-text">隐患内容:</span>
                                    <ext:TextArea ID="lbl_YHContent" runat="server" Width="500px" Height="40px" />
                                </td>
                            </tr>
                            <tr>
                                 <td colspan="3">
                                    <span class="x-label-text">备注信息:</span>
                                    <ext:TextArea ID="lbl_Remarks" runat="server" Width="500px" Height="40px" />
                                </td>
                            </tr> 
                        </table>
                    </Body>
                </ext:Panel>
                <ext:Panel 
                    ID="Panel1" 
                    runat="server" 
                    Title="隐患整改信息" 
                    AutoHeight="true" 
                    FormGroup="true"
                    Width="580px"
                    Collapsed="True">
                    <Body>
                        <table style="width:100%;">
                            <tr>
                                 <td colspan="3">
                                    <span class="x-label-text">整改措施:</span>
                                    <ext:TextArea ID="zg_Measures" runat="server" Width="500px" Height="40px" />
                                </td>
                            </tr>
                            <tr>
                                 <td colspan="3">
                                    <span class="x-label-text">领导批示:</span>
                                    <ext:TextArea ID="zg_Instructions" runat="server" Width="500px" Height="40px" />
                                </td>
                            </tr> 
                            <tr>
                                <td style="width:33%;">
                                    <span class="x-label-text">批示时间:</span>
                                    <ext:TextField ID="zg_InstrTime" runat="server" Cls="x-textfeild-style" />
                                </td>
                                <td style="width:34%;">
                                    <span class="x-label-text">责任单位:</span>
                                    <ext:TextField ID="zg_zrdw" runat="server" Cls="x-textfeild-style" />
                                </td>
                                <td style="width:33%;">
                                    <span class="x-label-text">责任人:</span>
                                    <ext:TextField ID="zg_PersonID" runat="server" Cls="x-textfeild-style" />
                                </td>
                                
                            </tr>
                            <tr>
                                <td style="width:33%;">
                                    <span class="x-label-text">整改期限:</span>
                                    <ext:TextField ID="zg_RecLimit" runat="server" Cls="x-textfeild-style" />
                                </td>
                                <td style="width:34%;">
                                    <span class="x-label-text">整改班次:</span>
                                    <ext:TextField ID="zg_BanCi" runat="server" Cls="x-textfeild-style" />
                                </td>
                                <td style="width:33%;">
                                    <span class="x-label-text">共计罚款:</span>
                                    <ext:TextField ID="zg_IsFine" runat="server" Cls="x-textfeild-style" />
                                </td>
                            </tr>
                        </table>
                    </Body>
                </ext:Panel>
                <ext:Panel 
                    ID="ZGPanel" 
                    runat="server" 
                    Title="整改反馈信息" 
                    AutoHeight="true" 
                    FormGroup="true"
                    Collapsed="True">
                    <Body>
                        <table style="width:100%;">
                            <tr>
                                <td style="width:33%;">
                                    <span class="x-label-text">整改情况:</span>
                                    <ext:TextField ID="zfk_RecState" runat="server" Cls="x-textfeild-style" />
                                </td>
                                <td style="width:34%;">
                                    <span class="x-label-text">整改时间:</span>
                                    <ext:TextField ID="zfk_RecTime" runat="server" Cls="x-textfeild-style" />
                                </td>
                                <td style="width:33%;">
                                    <span class="x-label-text">整改班次:</span>
                                    <ext:TextField ID="zfk_bc" runat="server" Cls="x-textfeild-style" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width:33%;">
                                    <span class="x-label-text">整 改 人:</span>
                                    <ext:TextField ID="zfk_RecPersonID" runat="server" Cls="x-textfeild-style" />
                                </td>
                                 <td colspan="3">
                                    <span class="x-label-text">验 收 人:</span>
                                    <ext:TextField ID="zfk_yanshouName" runat="server" Cls="x-textfeild-style" />
                                </td>
                            </tr>
                        </table>
                    </Body>
                </ext:Panel>
                <ext:Panel 
                    ID="FCPanel" 
                    runat="server" 
                    Title="复查反馈信息" 
                    AutoHeight="true" 
                    FormGroup="true"
                    Collapsed="True">
                    <Body>
                        <table style="width:100%;">
                            <tr>
                                 <td colspan="3">
                                    <span class="x-label-text">复查意见:</span>
                                    <ext:TextArea ID="ffk_ReviewOpinion" runat="server" Width="500px" Height="40px" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width:33%;">
                                    <span class="x-label-text">复 查 人:</span>
                                    <ext:TextField ID="ffk_PersonID" runat="server" Cls="x-textfeild-style" />
                                </td>
                                <td style="width:34%;">
                                    <span class="x-label-text">复查时间:</span>
                                    <ext:TextField ID="ffk_FCTime" runat="server" Cls="x-textfeild-style" />
                                </td>
                                <td style="width:33%;">
                                    <span class="x-label-text">复查情况:</span>
                                    <ext:TextField ID="ffk_ReviewState" runat="server" Cls="x-textfeild-style" />
                                </td>
                            </tr>
                        </table>
                    </Body>
                </ext:Panel>
            </ext:ContainerLayout>
        </Body>
    </ext:Window>
    <%--发布窗口/Modal去掉了--%>
    <ext:Window 
        ID="Window2" 
        runat="server" 
        BodyStyle="padding:5px;" 
        ButtonAlign="Right"
        Frame="true" 
        Title="隐患整改信息管理"
        Height="500px"
        Width="570px"
        ShowOnLoad="false"
        >
        <Body>
        <ext:FitLayout ID="FitLayout1" runat="server">
                <ext:TabPanel 
                    ID="tp_father" 
                    runat="server" 
                    ActiveTabIndex="1" 
                    TabPosition="Top"
                    Height="280px"
                    Title="Title">
                    <Tabs>
                        <ext:Tab ID="Tab1" runat="server" Title="隐患发布">
                            <Body>
                                <table style="width:570px;">
                                    <tr>
                                        <td colspan="2">
                                            <span class="x-label-text">整改措施:</span>
                                            <ext:TextArea ID="fb_zgcs" runat="server" Height="120px" Width="530px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:50%;">
                                            <span class="x-label-text">责任单位:</span>
                                            <ext:ComboBox 
                                                ID="fb_zrdw" 
                                                runat="server"
                                                
                                                Width="240px" 
                                                >
                                                <Listeners>
                                                    <Select Handler="#{fb_zrr}.clearValue(); #{PersStore}.reload();" />
                                                </Listeners>
                                            </ext:ComboBox>
                                        </td>
                                        <td style="width:50%;">
                                            <span class="x-label-text">责任人:</span>
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
                                                <Listeners>
                                                    <Render Fn="function(f) {
                                                                f.el.on('keyup', function(e) {
                                                                 if(window.event.keyCode==38 || window.event.keyCode==40 || window.event.keyCode==13){
                                                                    return;
                                                                 }
                                                                 Coolite.AjaxMethods.PYsearch(f.getRawValue(), 'PersStore');
                                                                });
                                                                }
                                                                " />
                                                </Listeners>
                                            </ext:ComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:50%;">
                                            <span class="x-label-text">整改期限:</span>
                                            <ext:DateField ID="fb_zgqx" runat="server" Width="260px" Vtype="daterange">  
                                            </ext:DateField>
                                        </td>
                                        <td style="width:50%;">
                                            <span class="x-label-text">班 次:</span>
                                            <ext:ComboBox 
                                                ID="fb_bc" 
                                                runat="server" 
                                                Width="200px"
                                                >
                                                <Items>
                                                    <ext:ListItem Text="早班" Value="早班" />
                                                    <ext:ListItem Text="中班" Value="中班" />
                                                    <ext:ListItem Text="夜班" Value="夜班" />
                                                </Items>
                                            </ext:ComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                         <td colspan="2">
                                            <span class="x-label-text">领导批示:</span>
                                            <ext:TextArea ID="fb_ldps" runat="server" Height="60px" Width="530px" />
                                        </td>
                                    </tr>
                                    <tr>
                                         <td style="width:50%;">
                                            <span class="x-label-text">罚款金额:</span>
                                            <ext:NumberField ID="fb_fkje" runat="server" FieldLabel="罚款金额" Width="260px" />
                                        </td>
                                        <td style="width:50%;">
                                            <span class="x-label-text"> </span>
                                            <ext:Checkbox ID="fb_isfine" runat="server" BoxLabel="确认罚款" Width="260px" Checked="true" >
                                                <Listeners>
                                                    <Check Handler="Coolite.AjaxMethods.IsFine();" />
                                                </Listeners>
                                            </ext:Checkbox>
                                        </td>
                                    </tr> 
                                    <tr>
                                    <td style="width:50%;">
                                            <span class="x-label-text">短信提醒:</span>
                                            <ext:TextArea ID="txtSms" runat="server" FieldLabel="短信提醒" EmptyText="若没有填写具体内容,系统将默认发送信息!" Disabled="true" Height="40px" Width="180px" />
                                        </td>
                                        <td style="width:50%;">
                                            <span class="x-label-text"> </span>
                                            <ext:Checkbox ID="chkIsSend" runat="server" BoxLabel="确认发送" Width="260px" Checked="false" >
                                                <Listeners>
                                                    <Check Handler="Coolite.AjaxMethods.IsSend();" />
                                                </Listeners>
                                            </ext:Checkbox>
                                        </td>
                                    </tr>
                                </table>
                            </Body>
                            <Buttons>
                                <ext:Button ID="btn_fb" runat="server" Text="发 布">
                                    <Listeners>
                                        <Click Handler="Coolite.AjaxMethods.YHpublish();" />
                                    </Listeners>
                                </ext:Button>
                            </Buttons>
                        </ext:Tab>
                        <ext:Tab ID="Tab2" runat="server" Title="提交职能科室">
                            <Body>
                                <table width="380px">
                                    <tr>
                                        <td style="width:100px">
                                            <span class="x-label-text">科室选择:</span>
                                        </td>
                                        <td style="width:260px">
                                            <ext:ComboBox 
                                                ID="refer_dept" 
                                                runat="server"
                                                >
                                                <Listeners>
                                                    <Select Handler="#{refer_Rname}.clearValue(); #{perStore}.reload();" />
                                                </Listeners>
                                            </ext:ComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                    </tr>
                                    <tr>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="width:100px">
                                            <ext:Checkbox runat="server" ID="ckb_isSms" Checked="false" BoxLabel="短信提醒">
                                                <Listeners>
                                                    <Check Handler="Coolite.AjaxMethods.IsSms();" />
                                                </Listeners>
                                            </ext:Checkbox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:100px">
                                            <ext:Label runat="server" ID="lbl_sms" Text="短信发送至:" />
                                        </td>
                                        <td style="width:260px">
                                            <ext:ComboBox 
                                                ID="refer_Rname" 
                                                runat="server"
                                                DisplayField="Name" 
                                                ValueField="Personnumber" 
                                                StoreID="perStore" Disabled="true">
                                            </ext:ComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:100px">
                                            <ext:Label runat="server" ID="lbl_smsText" Text="编辑短信内容:" />
                                        </td>
                                        <td style="width:260px">
                                            <ext:TextArea ID="ta_smsText" runat="server" Height="80px" Width="165px" EmptyText="若没有填写具体内容,系统将默认发送信息!" Disabled="true" />
                                        </td>
                                    </tr>
                                </table>
                            </Body>
                            <Buttons>
                                <ext:Button ID="btn_RD" runat="server" Text="提 交">
                                    <Listeners>
                                        <Click Handler="Coolite.AjaxMethods.ReferToDept();" />
                                    </Listeners>
                                </ext:Button>
                            </Buttons>
                        </ext:Tab>
                        <ext:Tab ID="Tab3" runat="server" Title="提交领导">
                            <Body>
                                <table width="380px">
                                    <tr>
                                        <td style="width:100px">
                                            <span class="x-label-text">领导选择:</span>
                                        </td>
                                        <td style="width:260px">
                                            <ext:ComboBox 
                                                ID="refer_lead" 
                                                runat="server"
                                                DisplayField="Name" 
                                                ValueField="Personnumber" 
                                                StoreID="FBBigleader">
                                            </ext:ComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                    </tr>
                                    <tr>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="width:100px">
                                            <ext:Checkbox runat="server" ID="ckb_IsSmss" Checked="false" BoxLabel="短信提醒">
                                                <Listeners>
                                                    <Check Handler="Coolite.AjaxMethods.isSmss();" />
                                                </Listeners>
                                            </ext:Checkbox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:100px">
                                            <ext:Label runat="server" ID="lbl_SmsTexts" Text="编辑短信内容:" />
                                        </td>
                                        <td style="width:260px">
                                            <ext:TextArea ID="ta_SmsTexts" runat="server" Height="80px" Width="165px" EmptyText="若没有填写具体内容,系统将默认发送信息!" Disabled="true" />
                                        </td>
                                    </tr>
                                </table>
                            </Body>
                            <Buttons>
                                <ext:Button ID="btn_RL" runat="server" Text="提 交">
                                    <Listeners>
                                        <Click Handler="Coolite.AjaxMethods.ReferToLeader();" />
                                    </Listeners>
                                </ext:Button>
                            </Buttons>
                        </ext:Tab>
                    </Tabs>
                </ext:TabPanel>
            </ext:FitLayout>           
        </Body> 
    </ext:Window>
    <%--整改反馈--%>
    <ext:Window 
        ID="Window3" 
        runat="server" 
        BodyStyle="padding:5px;" 
        ButtonAlign="Right"
        Frame="true" 
        Title="隐患整改反馈"
        AutoHeight="true"
        Width="400px"
        Modal="true"
        ShowOnLoad="false"
        >
        <Body>
            <ext:FormLayout ID="FormLayout10" runat="server" LabelWidth="60">
                <ext:Anchor Horizontal="95%">
                    <ext:ComboBox ID="zgfk_zgqk" runat="server" FieldLabel="整改情况">
                        <Items>
                            <ext:ListItem Text="隐患已整改" Value="隐患已整改" />
                            <ext:ListItem Text="隐患未整改" Value="隐患未整改" />
                        </Items>
                    </ext:ComboBox>
                </ext:Anchor>
                <ext:Anchor Horizontal="95%">
                    <ext:ComboBox 
                        ID="zgfk_zgr" 
                        runat="server" 
                        FieldLabel="整改人"
                        DisplayField="Name" 
                        ValueField="Personnumber" 
                        StoreID="personStore">
                    </ext:ComboBox>
                </ext:Anchor>
                <ext:Anchor Horizontal="95%">
                    <ext:ComboBox 
                        ID="zgfk_ysr" 
                        runat="server" 
                        FieldLabel="验收人"
                        DisplayField="Name" 
                        ValueField="Personnumber" 
                        StoreID="personStore">
                        <Listeners>
                            <Render Fn="function(f) {
                                        f.el.on('keyup', function(e) {
                                         if(window.event.keyCode==38 || window.event.keyCode==40 || window.event.keyCode==13){
                                            return;
                                         }
                                         Coolite.AjaxMethods.PYsearch(f.getRawValue(), 'personStore');
                                        });
                                        }
                                        " />
                        </Listeners>
                    </ext:ComboBox>
                </ext:Anchor>
                <ext:Anchor Horizontal="95%">
                    <ext:DateField ID="zgfk_zgrq" runat="server" FieldLabel="整改日期">
                    </ext:DateField>
                </ext:Anchor>
                <ext:Anchor Horizontal="95%">
                    <ext:ComboBox 
                        ID="zgfk_bc" 
                        runat="server"
                        FieldLabel="班次"
                        >
                        <Items>
                            <ext:ListItem Text="早班" Value="早班" />
                            <ext:ListItem Text="中班" Value="中班" />
                            <ext:ListItem Text="夜班" Value="夜班" />
                        </Items>
                    </ext:ComboBox>
                </ext:Anchor>
            </ext:FormLayout>
        </Body>
        <Buttons>
            <ext:Button ID="btn_zfk" runat="server" Text="提 交">
                <Listeners>
                    <Click Handler="Coolite.AjaxMethods.YHzgfk();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    <%--复查反馈--%>
    <ext:Window 
        ID="Window4" 
        runat="server" 
        BodyStyle="padding:5px;" 
        ButtonAlign="Right"
        Frame="true" 
        Title="隐患复查反馈"
        AutoHeight="true"
        Width="400px"
        Modal="true"
        ShowOnLoad="false"
        >
        <Body>
            <ext:FormLayout ID="FormLayout3" runat="server" LabelWidth="60">
                <ext:Anchor Horizontal="95%">
                    <ext:TextArea ID="fcfk_fcyj" runat="server" Height="40px" FieldLabel="复查意见" />
                </ext:Anchor>
                <ext:Anchor Horizontal="95%">
                    <ext:ComboBox 
                        ID="fcfk_fcr" 
                        runat="server" 
                        FieldLabel="复查人"
                        DisplayField="Name" 
                        ValueField="Personnumber" 
                        StoreID="personStore">
                    </ext:ComboBox>
                </ext:Anchor>
                <ext:Anchor Horizontal="95%">
                    <ext:ComboBox ID="fcfk_fcqk" runat="server" FieldLabel="复查情况">
                        <Items>
                            <ext:ListItem Text="复查通过" Value="复查通过" />
                            <ext:ListItem Text="复查未通过" Value="复查未通过" />
                        </Items>
                    </ext:ComboBox>
                </ext:Anchor>
            </ext:FormLayout>
        </Body>
        <Buttons>
            <ext:Button ID="Button9" runat="server" Text="提 交">
                <Listeners>
                    <Click Handler="Coolite.AjaxMethods.YHfcfk();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    <%--异常流程处理--%>
    <ext:Window 
        ID="Window5" 
        runat="server" 
        BodyStyle="padding:5px;" 
        ButtonAlign="Right"
        Frame="true" 
        Title="异常流程处理"
        AutoHeight="true"
        Width="400px"
        Modal="true"
        ShowOnLoad="false"
        >
        <Body>
            <ext:FormLayout ID="FormLayout4" runat="server" LabelWidth="60">
                <ext:Anchor Horizontal="95%">
                    <ext:TextField ID="yc_YHID" runat="server" FieldLabel="隐患编号" Disabled="true">
                    </ext:TextField>
                </ext:Anchor>
                <ext:Anchor Horizontal="95%">
                    <ext:ComboBox 
                        ID="yc_nowstatus" 
                        runat="server" 
                        FieldLabel="当前状态"
                        DisplayField="Value" 
                        ValueField="Value" 
                        StoreID="StatusStore"
                        Disabled="true">
                    </ext:ComboBox>
                </ext:Anchor>
                <ext:Anchor Horizontal="95%">
                    <ext:ComboBox 
                        ID="yc_newstatus" 
                        runat="server" 
                        FieldLabel="变更状态"
                        DisplayField="Value" 
                        ValueField="Value" 
                        StoreID="StatusStore">
                    </ext:ComboBox>
                </ext:Anchor>
                <ext:Anchor Horizontal="95%">
                    <ext:TextArea ID="yc_detail" runat="server" Height="40px" FieldLabel="变更事由" />
                </ext:Anchor>
                <ext:Anchor Horizontal="95%">
                    <ext:NumberField ID="yc_fine" runat="server" FieldLabel="罚款金额" Cls="x-textfeild-style" EmptyText="不罚输入0或不输入" />
                </ext:Anchor>
            </ext:FormLayout>
        </Body>
        <Buttons>
            <ext:Button ID="Button1" runat="server" Text="提 交">
                <Listeners>
                    <Click Handler="Coolite.AjaxMethods.YHycfk();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    <ext:Window 
        ID="Window6" 
        runat="server" 
        BodyStyle="padding:5px;" 
        ButtonAlign="Right"
        Frame="true" 
        Title="闭合新增隐患"
        AutoHeight="true"
        Width="400px"
        Modal="true"
        ShowOnLoad="false"
        >
        <Body>
            <ext:FormLayout ID="FormLayout1" runat="server" LabelWidth="60">
                <ext:Anchor Horizontal="95%">
                    <ext:TextField ID="TextField1" runat="server" FieldLabel="隐患编号" Disabled="true">
                    </ext:TextField>
                </ext:Anchor>
                <ext:Anchor Horizontal="95%">
                    <ext:ComboBox 
                        ID="ComboBox1" 
                        runat="server" 
                        FieldLabel="当前状态"
                        DisplayField="Value" 
                        ValueField="Value" 
                        StoreID="StatusStore"
                        Disabled="true">
                    </ext:ComboBox>
                </ext:Anchor>
                <ext:Anchor Horizontal="95%">
                    <ext:ComboBox 
                        ID="ComboBox2" 
                        runat="server" 
                        FieldLabel="变更状态"
                        DisplayField="Value" 
                        ValueField="Value" 
                        StoreID="StatusStore">
                    </ext:ComboBox>
                </ext:Anchor>

            </ext:FormLayout>
        </Body>
        <Buttons>
            <ext:Button ID="Button2" runat="server" Text="提 交">
                <Listeners>
                    <Click Handler="Coolite.AjaxMethods.YHycfk1();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
     <%--逾期未走动流程处理--%>
    <ext:Window 
        ID="YQWindow" 
        runat="server" 
        BodyStyle="padding:5px;" 
        ButtonAlign="Right"
        Frame="true" 
        Title="分级流程处理"
        Height="500px"
        Width="450px"
        ShowOnLoad="false"
        >
        <Body>
        <ext:FitLayout ID="FitLayout2" runat="server">
            <ext:Panel ID="Panel2" runat="server" Width="400">
                <Body>
                    <table style="width:385px;">
                        <tr>
                            <td colspan="2">
                                <span class="x-label-text">整改措施:</span>
                                <ext:TextArea ID="yq_zgcs" runat="server" Height="40px" Width="390px" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width:60%;">
                                <span class="x-label-text">责任单位:</span>
                                <ext:ComboBox 
                                    ID="yq_zrdw" 
                                    runat="server"
                                    DisplayField="Deptname" 
                                    ValueField="Deptnumber"
                                    Width="180px" 
                                    StoreID="DeptStore">
                                    <Listeners>
                                        <Select Handler="#{yq_zrr}.clearValue(); #{persoStore}.reload();" />
                                    </Listeners>
                                </ext:ComboBox>
                            </td>
                            <td style="width:40%;">
                                <span class="x-label-text">责任人:</span>
                                <ext:ComboBox 
                                    ID="yq_zrr" 
                                    runat="server"
                                    DisplayField="Name" 
                                    ValueField="Personnumber"
                                    Width="180px"
                                    StoreID="persoStore"
                                    EmptyText="没有待选人员"
                                    Disabled="true" 
                                    Editable="false">
                                </ext:ComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:60%;">
                                <span class="x-label-text">整改期限:</span>
                                <ext:DateField ID="yq_zgqx" runat="server" Width="180px" />
                            </td>
                            <td style="width:40%;">
                                <span class="x-label-text">班 次:</span>
                                <ext:ComboBox 
                                    ID="yq_bc" 
                                    runat="server" 
                                    Width="180px"
                                    >
                                    <Items>
                                        <ext:ListItem Text="早班" Value="早班" />
                                        <ext:ListItem Text="中班" Value="中班" />
                                        <ext:ListItem Text="夜班" Value="夜班" />
                                    </Items>
                                </ext:ComboBox>
                            </td>
                        </tr>
                        <tr>
                             <td colspan="2">
                                <span class="x-label-text">领导批示:</span>
                                <ext:TextArea ID="yq_ldps" runat="server" Height="40px" Width="390px" />
                            </td>
                        </tr>
                        <tr>
                             <td style="width:60%;">
                                <span class="x-label-text">罚款金额:</span>
                                <ext:NumberField ID="yq_fkje" runat="server" FieldLabel="罚款金额" Width="180px" />
                            </td>
                            <td style="width:40%;">
                                <span class="x-label-text"> </span>
                                <ext:Checkbox ID="Checkbox1" runat="server" BoxLabel="确认罚款" Width="180px" Checked="true" >
                                    <Listeners>
                                        <Check Handler="Coolite.AjaxMethods.yqIsFine();" />
                                    </Listeners>
                                </ext:Checkbox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <hr />
                                <ext:Label runat="server" ID="lblmsgtext" />
                                <hr />
                            </td>
                            
                        </tr>
                        <tr>
                            <td>
                                 <span class="x-label-text"><ext:Checkbox ID="Checkbox2" runat="server" BoxLabel="<font color='red'>提醒责任人</font>" Width="110px" Checked="true">
                                    <Listeners>
                                        <Check Handler="Coolite.AjaxMethods.yqzrr();" />
                                    </Listeners>
                                </ext:Checkbox></span>
                            </td>
                            <td>
                                <span class="x-label-text">短信内容:</span>
                                <ext:TextArea ID="tazrrmassage" runat="server" Height="30px" Width="200px" EmptyText="若没有填写具体内容,系统将默认发送信息!" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="x-label-text"><ext:Checkbox ID="Checkbox3" runat="server" BoxLabel="<font color='red'>提醒分管领导</font>" Width="110px" Checked="true">
                                    <Listeners>
                                        <Check Handler="Coolite.AjaxMethods.yqfgld();" />
                                    </Listeners>
                                </ext:Checkbox></span>
                                <ext:ComboBox 
                                    ID="yq_fgld" 
                                    runat="server"
                                    DisplayField="Name" 
                                    ValueField="Personnumber"
                                    Width="180px"
                                    StoreID="FBleader"
                                    EmptyText="请选择人员"
                                    Editable="false">
                                </ext:ComboBox>
                            </td>
                            <td>
                                <span class="x-label-text">短信内容:</span>
                                <ext:TextArea ID="tafgldmassamge" runat="server" Height="30px" Width="200px" EmptyText="若没有填写具体内容,系统将默认发送信息!" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="x-label-text"><ext:Checkbox ID="Checkbox4" runat="server" BoxLabel="<font color='red'>提醒主要领导</font>" Width="110px">
                                    <Listeners>
                                        <Check Handler="Coolite.AjaxMethods.yqdld();" />
                                    </Listeners>
                                </ext:Checkbox></span>
                                <ext:ComboBox 
                                    ID="yq_dld" 
                                    runat="server"
                                    DisplayField="Name" 
                                    ValueField="Personnumber"
                                    Width="180px"
                                    StoreID="FBBigleader"
                                    EmptyText="请选择人员"
                                    Disabled="true" 
                                    Editable="false">
                                </ext:ComboBox>
                            </td>
                            <td>
                                <span class="x-label-text">短信内容:</span>
                                <ext:TextArea ID="tadldmassage" runat="server" Height="30px" Width="200px" EmptyText="若没有填写具体内容,系统将默认发送信息!" />
                            </td>
                        </tr>
                    </table>
                </Body>
                <Buttons>
                    <ext:Button ID="Button6" runat="server" Text="发 布">
                        <Listeners>
                            <Click Handler="Coolite.AjaxMethods.yqYHpublish();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:Panel>
            </ext:FitLayout>           
        </Body> 
    </ext:Window>
    </div>
    </form>
</body>
</html>
