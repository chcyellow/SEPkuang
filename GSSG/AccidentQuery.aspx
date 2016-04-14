﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccidentQuery.aspx.cs" Inherits="GSSG_AccidentQuery" %>
<%@ Register assembly="Coolite.Ext.Web" namespace="Coolite.Ext.Web" tagprefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript">
        var qtip = function(v, p) {//单元格提示
            //v : value , p : cell
            p.attr = 'ext:qtitle="" ext:qtip="' + v + '"';
            return v;
        }

        var template = '<span style="color:{0};">{1}</span>';

        var change = function(value) {
            var color;
            if (value == '新增')
                color = 'red';
            else
                color = 'green';
            return String.format(template, color, value);
        }
    </script>
        
    <style type="text/css">
        .search-item {
            font: normal 12px tahoma, arial, helvetica, sans-serif;
            padding: 3px 10px 3px 10px;
            border: 1px solid #fff;
            font-weight:bold; 
            border-bottom: 1px solid #eeeeee;
            white-space: normal;
            color: #555;
        }
        
        .search-item h3 {
            display: block;
            font: inherit;
            
            color: #222;
        }

        .search-item h3 span {
            float: left;
             
            margin: 0 0 5px 5px;
            width: 100px;
            display: block;
            clear: none;
        } 
        
        p { width: 650px; }
        
        .ext-ie .x-form-text { position: static !important; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN" CleanResourceUrl="false" />
        <ext:Hidden ID="GridData" runat="server" />
       
        
        <ext:Store ID="sglxStore" runat="server">
            <Reader>
            <ext:JsonReader ReaderID="INFOID">
                <Fields>
                    <ext:RecordField Name="INFOID" />
                    <ext:RecordField Name="INFONAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
        </ext:Store>
        <ext:Store ID="sgdjStore" runat="server">
            <Reader>
            <ext:JsonReader ReaderID="INFOID">
                <Fields>
                    <ext:RecordField Name="INFOID" />
                    <ext:RecordField Name="INFONAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
        </ext:Store>
        <ext:Store ID="sfStore" runat="server">
            <Reader>
            <ext:JsonReader ReaderID="INFOID">
                <Fields>
                    <ext:RecordField Name="INFOID" />
                    <ext:RecordField Name="INFONAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
        </ext:Store>
        <ext:Store ID="jtStore" runat="server">
            <Reader>
            <ext:JsonReader ReaderID="INFOID">
                <Fields>
                    <ext:RecordField Name="INFOID" />
                    <ext:RecordField Name="INFONAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
        </ext:Store>
        <ext:Store ID="kjlxStore" runat="server">
            <Reader>
            <ext:JsonReader ReaderID="INFOID">
                <Fields>
                    <ext:RecordField Name="INFOID" />
                    <ext:RecordField Name="INFONAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
        </ext:Store>
        <ext:Store ID="wsdjStore" runat="server">
            <Reader>
            <ext:JsonReader ReaderID="INFOID">
                <Fields>
                    <ext:RecordField Name="INFOID" />
                    <ext:RecordField Name="INFONAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
        </ext:Store>
        
        
        <ext:Store ID="SGStore" runat="server">
        <Reader>
                <ext:JsonReader ReaderID="Id">
                    <Fields>
                        <ext:RecordField Name="Id"/> 
                        <ext:RecordField Name="Name" />
                        <ext:RecordField Name="Accidentname" />
                   
                        <ext:RecordField Name="Orename" />
                        <ext:RecordField Name="Happendate" Type="Date" DateFormat="Y-m-dTh:i:s" />
                        <ext:RecordField Name="Indate" Type="Date" DateFormat="Y-m-dTh:i:s" />
                        
                        <ext:RecordField Name="Deathnumber"/>  
                        <ext:RecordField Name="Zsnumber"/>  
                         <ext:RecordField Name="Qsnumber"/>  
                        <ext:RecordField Name="ZjLoss"/>  
                          <ext:RecordField Name="JjLoss"/>    
                        
                         <ext:RecordField Name="sf"/>  
                        <ext:RecordField Name="jt"/>  
                         <ext:RecordField Name="sglx"/>     
                         <ext:RecordField Name="sgdj"/>  
                        <ext:RecordField Name="wsdj"/>  
                         <ext:RecordField Name="kjlx"/>                                                                                                                           
                    </Fields>
                  
                </ext:JsonReader>
            </Reader>
        </ext:Store>
    <ext:Hidden ID="Hidden1" runat="server">
    </ext:Hidden>
    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body >
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                
                <Center>
                    <ext:GridPanel ID="GridPanel1" runat="server"  Title="事故信息" StoreID="SGStore" AutoScroll="true" StripeRows="true">
            <ColumnModel ID="ColumnModel1" runat="server" >
                <Columns>
                    <ext:Column  Header="编号" DataIndex="Id" >
                    </ext:Column>
                    <ext:Column  Header="案例名称" Sortable="true" Align="Center" 
                        DataIndex="Accidentname" >
                    </ext:Column>
                    <ext:Column  Header="省份" Sortable="true" Align="Center"
                        DataIndex="sf" >
                    </ext:Column>
                   
                    <ext:Column Header="集团公司" Sortable="true" DataIndex="jt" Align="Center">
                       
                    </ext:Column>
                    <ext:Column Header="事故类型" Sortable="true"  DataIndex="sglx" Align="Center">
                    </ext:Column>
                    
                    <ext:Column  Header="事故等级" Sortable="true" Align="Center"
                        DataIndex="sgdj" >
                    </ext:Column>
                    <ext:Column  Header="瓦斯等级" Sortable="true" Align="Center"
                        DataIndex="wsdj" >
                    </ext:Column>
                    <ext:Column  Header="矿井类型" Sortable="true" Align="Center"
                        DataIndex="kjlx" >
                    </ext:Column>
                    <ext:Column  Header="矿井名称" Sortable="true" Align="Center"
                        DataIndex="Orename" >
                    </ext:Column>
                    
                    
                    
                     <ext:Column Header="死亡数量" Sortable="true"  DataIndex="Deathnumber" Align="Center">
                    </ext:Column>
                    
                    <ext:Column  Header="重伤数量" Sortable="true" Align="Center"
                        DataIndex="Zsnumber" >
                    </ext:Column>
                    <ext:Column  Header="轻伤数量" Sortable="true" Align="Center"
                        DataIndex="Qsnumber" >
                    </ext:Column>
                    <ext:Column  Header="直接经济损失" Sortable="true" Align="Center"
                        DataIndex="ZjLoss" >
                    </ext:Column>
                    <ext:Column  Header="间接经济损失" Sortable="true" Align="Center"
                        DataIndex="JjLoss" >
                    </ext:Column>
                    
                    
                    
                    <ext:Column  Header="发生时间" Sortable="true" Align="Center"
                        DataIndex="Happendate" >
                        <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" />
                    </ext:Column>
                    <ext:Column  Header="录入人员" Sortable="true" Align="Center"
                        DataIndex="Name" >
                    </ext:Column>
                  <%--  <ext:Column  Header="录入时间" Sortable="true" Align="Center"
                        DataIndex="Indate" >
                        <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" />
                    </ext:Column>--%>
                    
                </Columns> 
            </ColumnModel>
            <View>
                <ext:GridView ForceFit="true" />
            </View>
               
            <LoadMask ShowMask="true" />
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
                        <ext:ToolbarFill />
                        <%--<ext:Button ID="btn_print" runat="server" Text="导出报表" Icon="PageExcel" AutoPostBack="true" OnClick="ToExcel" Disabled="true">
                            <Listeners>
                                <Click Fn="saveData" />
                            </Listeners>
                        </ext:Button>--%>
                    </Items>
                </ext:Toolbar>
            </TopBar>
            <BottomBar>
                <ext:PagingToolBar ID="PagingToolBar1" runat="server" PageSize="10" StoreID="SGStore" />
            </BottomBar>
        <SelectionModel>
                <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" SingleSelect="true" runat="server" />                   
        </SelectionModel>
        
        </ext:GridPanel> 
        
                </Center>
            </ext:BorderLayout>
        </Body>
    </ext:ViewPort>
    <ext:Window 
            ID="Window1" 
            runat="server" 
            BodyStyle="padding:5px;" 
            ButtonAlign="Right"
            Frame="true" 
            Title="请选择查询条件"
            AutoHeight="true"
            Width="520px"
            Modal="true"
            ShowOnLoad="false"
            X="100" Y="60">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout1" runat="server">
                    <ext:LayoutColumn ColumnWidth=".5">
                        <ext:Panel ID="Panel1" runat="server" Border="false" Header="false">
                           
                            <Body>
                                <ext:FormLayout ID="FormLayout1" runat="server" LabelAlign="Left" LabelWidth="70" >
                                    <ext:Anchor Horizontal="92%">
                                <ext:DateField ID="df_begin" runat="server" Format="yyyy-MM-dd" FieldLabel="起始日期"  Vtype="daterange">
                                    <Listeners>
                                        <Render Handler="this.endDateField = '#{df_end}'" />
                                    </Listeners>
                                </ext:DateField>
                                </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:TextField ID="sgName" runat = "server" FieldLabel="事故名称"></ext:TextField>
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        
                                        <ext:ComboBox 
                                            ID="sglx1"
                                            runat="server" 
                                            StoreID="sglxStore"
                                            DisplayField="INFONAME" 
                                            ValueField="INFOID"
                                            LoadingText="Searching..." 
                                            FieldLabel="事故类型<font color='red'>*</font>"
                                                 
                                            >
                                        </ext:ComboBox>
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        
                                        <ext:ComboBox 
                                            ID="sgdj1"
                                            runat="server" 
                                            StoreID="sgdjStore"
                                            DisplayField="INFONAME" 
                                            ValueField="INFOID"
                                            LoadingText="Searching..." 
                                            FieldLabel="事故等级<font color='red'>*</font>"
                                                 
                                            >
                                        </ext:ComboBox>
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        
                                        <ext:ComboBox 
                                            ID="wsdj1"
                                            runat="server" 
                                            StoreID="wsdjStore"
                                            DisplayField="INFONAME" 
                                            ValueField="INFOID"
                                            LoadingText="Searching..." 
                                            FieldLabel="瓦斯等级<font color='red'>*</font>"
                                                 
                                            >
                                        </ext:ComboBox>
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:NumberField ID="swNumber" runat="server" FieldLabel="死亡人数" Width="260px" />
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:NumberField ID="zsNumber" runat="server" FieldLabel="重伤人数" Width="260px" />
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:NumberField ID="qsNumber" runat="server" FieldLabel="轻伤人数" Width="260px" />
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:NumberField ID="zjjjss" runat="server" FieldLabel="直接损失" Width="260px" />
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:NumberField ID="jjjjss" runat="server" FieldLabel="间接损失" Width="260px" />
                                    </ext:Anchor>
                                    
                                </ext:FormLayout>
                            </Body>
                        </ext:Panel>
                    </ext:LayoutColumn>
                    <ext:LayoutColumn ColumnWidth=".5">
                        <ext:Panel ID="Panel2" runat="server" Border="false">
                            <Defaults>
                                
                                <ext:Parameter Name="MsgTarget" Value="side" />
                            </Defaults>
                            <Body>
                                <ext:FormLayout ID="FormLayout2" runat="server" LabelAlign="Left" LabelWidth="70">
                                   
                                    <ext:Anchor Horizontal="92%">
                                        <ext:DateField ID="df_end" runat="server" Format="yyyy-MM-dd" FieldLabel="截止日期"  Vtype="daterange" >
                                    <Listeners>
                                        <Render Handler="this.startDateField = '#{df_begin}'" />
                                    </Listeners>
                                </ext:DateField>
                                    </ext:Anchor>
                                   <ext:Anchor Horizontal="92%">
                                        
                                        <ext:ComboBox 
                                            ID="sf1"
                                            runat="server" 
                                            StoreID="sfStore"
                                            DisplayField="INFONAME" 
                                            ValueField="INFOID"
                                            LoadingText="Searching..." 
                                            FieldLabel="所属省份<font color='red'>*</font>"
                                                 
                                            >
                                        </ext:ComboBox>
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        
                                        <ext:ComboBox 
                                            ID="jt1"
                                            runat="server" 
                                            StoreID="jtStore"
                                            DisplayField="INFONAME" 
                                            ValueField="INFOID"
                                            LoadingText="Searching..." 
                                            FieldLabel="所属集团<font color='red'>*</font>"
                                                 
                                            >
                                        </ext:ComboBox>
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:TextField ID="kjName" runat = "server" FieldLabel="矿井名称"></ext:TextField>
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        
                                        <ext:ComboBox 
                                            ID="kjlx1"
                                            runat="server" 
                                            StoreID="kjlxStore"
                                            DisplayField="INFONAME" 
                                            ValueField="INFOID"
                                            LoadingText="Searching..." 
                                            FieldLabel="矿井类型<font color='red'>*</font>"
                                                 
                                            >
                                        </ext:ComboBox>
                                    </ext:Anchor>
                                    
                                    <ext:Anchor Horizontal="92%">
                                        <ext:NumberField ID="swNumber1" runat="server" FieldLabel="至" Width="260px" />
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:NumberField ID="zsNumber1" runat="server" FieldLabel="至" Width="260px" />
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:NumberField ID="qsNumber1" runat="server" FieldLabel="至" Width="260px" />
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:NumberField ID="zjjjss1" runat="server" FieldLabel="至" Width="260px" />
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:NumberField ID="jjjjss1" runat="server" FieldLabel="至" Width="260px" />
                                    </ext:Anchor>
                                    
                                </ext:FormLayout>
                            </Body>
                        </ext:Panel>
                    </ext:LayoutColumn>
                </ext:ColumnLayout>
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
            ID="DetailWindow" 
            runat="server" 
            BodyStyle="padding:6px;" 
            ButtonAlign="Right" 
            Frame="true" 
            Title="事故明细信息" 
            AutoHeight="true" 
            Width="600px" 
            Modal="true" 
            ShowOnLoad="false" 
            X="100" Y="60">
            <Tools>
                <ext:Tool Type="Refresh" Handler="Coolite.AjaxMethods.DetailLoad();" />
            </Tools>
            <LoadMask Msg="信息正在加载，请稍候...." ShowMask="true" />
            <Body>
                <ext:ContainerLayout ID="ContainerLayout1" runat="server">
                    <ext:Panel 
                        ID="BasePanel" 
                        runat="server" 
                        Title="事故基本信息" 
                        FormGroup="true" 
                        Width="580px"
                        >
                        <Body>
                <ext:ColumnLayout ID="ColumnLayout2" runat="server">
                    <ext:LayoutColumn ColumnWidth=".5">
                        <ext:Panel ID="Panel3" runat="server" Border="false" Header="false">
                            <Defaults>
                                <%--<ext:Parameter Name="AllowBlank" Value="false" Mode="Raw" />--%>
                                <ext:Parameter Name="MsgTarget" Value="side" />
                            </Defaults>
                            <Body>
                                <ext:FormLayout ID="FormLayout3" runat="server" LabelAlign="Left" LabelWidth="70" >
                                    
                                    <ext:Anchor Horizontal="92%">
                                        <ext:TextField ID="TextField1" runat = "server" FieldLabel="事故名称" ></ext:TextField>
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        
                                        <ext:ComboBox  
                                            ID="sglx"
                                            runat="server" 
                                            StoreID="sglxStore"
                                            DisplayField="INFONAME" 
                                            ValueField="INFOID"
                                            LoadingText="Searching..." 
                                            FieldLabel="事故类型<font color='red'>*</font>"
                                                 
                                            >
                                        </ext:ComboBox>
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        
                                        <ext:ComboBox 
                                            ID="sgdj"
                                            runat="server" 
                                            StoreID="sgdjStore"
                                            DisplayField="INFONAME" 
                                            ValueField="INFOID"
                                            LoadingText="Searching..." 
                                            FieldLabel="事故等级<font color='red'>*</font>"
                                                 
                                            >
                                        </ext:ComboBox>
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:NumberField ID="NumberField1" runat="server" FieldLabel="死亡人数" Width="260px" />
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:NumberField ID="NumberField2" runat="server" FieldLabel="重伤人数" Width="260px" />
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:NumberField ID="NumberField3" runat="server" FieldLabel="轻伤人数" Width="260px" />
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:NumberField ID="NumberField4" runat="server" FieldLabel="直接损失" Width="260px" />
                                    </ext:Anchor>
                                    
                                    <ext:Anchor Horizontal="92%">
                                        <ext:TextArea ID="sgjg" runat="server" FieldLabel="事故经过" Height="60">
                                        </ext:TextArea>
                                    </ext:Anchor>
                                    
                                </ext:FormLayout>
                            </Body>
                        </ext:Panel>
                    </ext:LayoutColumn>
                    <ext:LayoutColumn ColumnWidth=".5">
                        <ext:Panel ID="Panel4" runat="server" Border="false">
                            <Defaults>
                                
                                <ext:Parameter Name="MsgTarget" Value="side" />
                            </Defaults>
                            <Body>
                                <ext:FormLayout ID="FormLayout4" runat="server" LabelAlign="Left" LabelWidth="70">
                                   
                                    <ext:Anchor Horizontal="92%">
                                        <ext:DateField ID="Happentime" FieldLabel="发生时间" runat="server" Vtype="daterange">
                                        </ext:DateField>
                                    </ext:Anchor>
                                   <ext:Anchor Horizontal="92%">
                                        
                                        <ext:ComboBox 
                                            ID="sf"
                                            runat="server" 
                                            StoreID="sfStore"
                                            DisplayField="INFONAME" 
                                            ValueField="INFOID"
                                            LoadingText="Searching..." 
                                            FieldLabel="所属省份<font color='red'>*</font>"
                                                 
                                            >
                                        </ext:ComboBox>
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        
                                        <ext:ComboBox 
                                            ID="jt"
                                            runat="server" 
                                            StoreID="jtStore"
                                            DisplayField="INFONAME" 
                                            ValueField="INFOID"
                                            LoadingText="Searching..." 
                                            FieldLabel="所属集团<font color='red'>*</font>"
                                                 
                                            >
                                        </ext:ComboBox>
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:TextField ID="TextField2" runat = "server" FieldLabel="矿井名称"></ext:TextField>
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        
                                        <ext:ComboBox 
                                            ID="kjlx"
                                            runat="server" 
                                            StoreID="kjlxStore"
                                            DisplayField="INFONAME" 
                                            ValueField="INFOID"
                                            LoadingText="Searching..." 
                                            FieldLabel="矿井类型<font color='red'>*</font>"
                                                 
                                            >
                                        </ext:ComboBox>
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        
                                        <ext:ComboBox 
                                            ID="wsdj"
                                            runat="server" 
                                            StoreID="wsdjStore"
                                            DisplayField="INFONAME" 
                                            ValueField="INFOID"
                                            LoadingText="Searching..." 
                                            FieldLabel="瓦斯等级<font color='red'>*</font>"
                                                 
                                            >
                                        </ext:ComboBox>
                                    </ext:Anchor>
                                   <%-- <ext:Anchor Horizontal="92%">
                                        <ext:ComboBox Visible="true"
                                            ID="cbbPerson"
                                            runat="server"
                                            ReadOnly="true"
                                            PageSize="10"
                                            FieldLabel="录入人员"        
                                            HideTrigger="true">
                                        </ext:ComboBox>
                                    </ext:Anchor>--%>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:NumberField ID="NumberField5" runat="server" FieldLabel="间接损失" Width="260px" />
                                    </ext:Anchor>
                                      <ext:Anchor Horizontal="92%">
                                        <ext:TextArea ID="sgfenxi" runat="server" FieldLabel="事故分析" Height="70">
                                        </ext:TextArea>
                                    </ext:Anchor>
                                </ext:FormLayout>
                            </Body>
                        </ext:Panel>
                    </ext:LayoutColumn>
                </ext:ColumnLayout>
            </Body>
                    </ext:Panel>
                    
                </ext:ContainerLayout>
            </Body>
        </ext:Window>
    </form>
</body>
</html>
