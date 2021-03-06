﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkINJURYquery.aspx.cs" Inherits="GSSG_WorkINJURYquery" %>
<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../Style/examples.css" rel="stylesheet" type="text/css" />
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
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN" CleanResourceUrl="false" />
        <ext:Hidden ID="GridData" runat="server" />
        <ext:Store ID="personStore" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="pernID" />
                        <ext:RecordField Name="pernName" />
                        <ext:RecordField Name="persNnmber" />
                        <ext:RecordField Name="pernLightNumber" />
                        <ext:RecordField Name="DeptName" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
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
        <ext:Store ID="gsdjStore" runat="server">
            <Reader>
            <ext:JsonReader ReaderID="INFOID">
                <Fields>
                    <ext:RecordField Name="INFOID" />
                    <ext:RecordField Name="INFONAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
        </ext:Store>
        
        <ext:Store ID="GSStore" runat="server">
        <Reader>
                <ext:JsonReader ReaderID="Id">
                    <Fields>
                        <ext:RecordField Name="Id"/> 
                        <ext:RecordField Name="Name" />
                        <ext:RecordField Name="Deptname" />
                        <%--<ext:RecordField Name="Postname" />--%>
                       
                        <ext:RecordField Name="Placename" />
                        <ext:RecordField Name="Happendate" Type="Date" DateFormat="Y-m-dTh:i:s" />
                        <ext:RecordField Name="Indate" Type="Date" DateFormat="Y-m-dTh:i:s" />
                        <ext:RecordField Name="Infoname"/>  
                        <ext:RecordField Name="GsFact"/>  
                        
                         <ext:RecordField Name="PointsPer"/>  
                        <ext:RecordField Name="FinePer"/>   
                        
                         <ext:RecordField Name="PointsDept"/>  
                        <ext:RecordField Name="FineDept"/>  
                         <ext:RecordField Name="inPersonName"/>                                                                                                                           
                    </Fields>
                   
                </ext:JsonReader>
            </Reader>
        </ext:Store>
    <ext:Hidden ID="Hidden1" runat="server">
    </ext:Hidden>
    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                
                <Center>
                    <ext:GridPanel ID="GridPanel1" runat="server"  Title="工伤信息" StoreID="GSStore" AutoScroll="true" StripeRows="true">
            <ColumnModel ID="ColumnModel1" runat="server" >
                <Columns>
                    <ext:Column  Header="编号" DataIndex="Id" >
                    </ext:Column>
                      <ext:Column  Header="部门" Sortable="true" Align="Center"
                        DataIndex="Deptname" >
                    </ext:Column>
                    <ext:Column  Header="受伤人员" Sortable="true" Align="Center" 
                        DataIndex="Name" >
                    </ext:Column>
                    <ext:Column Header="工伤等级" Sortable="true"  DataIndex="Infoname" Align="Center">
                    </ext:Column>
                    <%--<ext:Column  Header="岗位" Sortable="true" Align="Center"
                        DataIndex="Postname" >
                    </ext:Column>--%>
                    <ext:Column Header="工伤事实" Sortable="true" DataIndex="GsFact" Align="Center">
                        <Renderer Fn="qtip" />
                    </ext:Column>
                    <ext:Column  Header="发生时间" Sortable="true" Align="Center"
                        DataIndex="Happendate" >
                        <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" />
                    </ext:Column>
                    <ext:Column  Header="发生地点" Sortable="true" Align="Center"
                        DataIndex="Placename" >
                    </ext:Column>
                    <ext:Column  Header="员工扣分" Sortable="true" Align="Center"
                        DataIndex="PointsPer" >
                    </ext:Column>
                    <ext:Column  Header="员工罚款" Sortable="true" Align="Center"
                        DataIndex="FinePer" >
                    </ext:Column>
                    <ext:Column  Header="部门扣分" Sortable="true" Align="Center"
                        DataIndex="PointsDept" >
                    </ext:Column>
                    <ext:Column  Header="部门罚款" Sortable="true" Align="Center"
                        DataIndex="FineDept" >
                    </ext:Column>
                    <ext:Column  Header="录入人员" Sortable="true" Align="Center"
                        DataIndex="inPersonName" >
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
                        <%--<ext:Button runat="server" ID="btn_detail" Icon="FolderFind" Text="查看明细" >
                            <Listeners>
                                <Click Handler="Coolite.AjaxMethods.DetailLoad();#{DetailWindow}.show();" />
                            </Listeners>
                        </ext:Button>--%>
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
                <ext:PagingToolBar ID="PagingToolBar1" runat="server" PageSize="10" StoreID="GSStore" />
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
                            <Defaults>
                                <ext:Parameter Name="MsgTarget" Value="side" />
                            </Defaults>
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
                                        <ext:ComboBox 
                                            ID="cbbGsperson"
                                            runat="server" 
                                            StoreID="personStore"
                                            DisplayField="pernName" 
                                            ValueField="persNnmber"
                                            LoadingText="Searching..." 
                                            Width="570"
                                            ListWidth="350"
                                            PageSize="10"
                                            HideTrigger="true"
                                            FieldLabel="受伤人员<font color='red'>*</font>"
                                            ItemSelector="div.search-item"        
                                            > 
                                            <Template ID="Template1" runat="server">
                                                

                                               <tpl for=".">
                                                  <div class="search-item">
                                                     <h3><span>姓名:{pernName}</span><span>灯号：{pernLightNumber}</span>单位：{DeptName}</h3>
                                                     
                                                  </div>
                                               </tpl>

                                            </Template>
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
                                    <ext:Anchor Horizontal="92%">
                                        
                                        <ext:ComboBox 
                                            ID="cbbplace"
                                            runat="server" 
                                            StoreID="placeStore"
                                            DisplayField="placName" 
                                            ValueField="placID"
                                            LoadingText="Searching..." 
                                            Width="570"
                                            PageSize="10"
                                            HideTrigger="true"
                                            FieldLabel="发生地点<font color='red'>*</font>"
                                            ItemSelector="div.search-item"        
                                            >
                                            <Template ID="Template3" runat="server">
                                            
                                               <tpl for=".">
                                                  <div class="search-item">
                                                     <span>{placName}</span>
                                                     
                                                  </div>
                                               </tpl>
                                            </Template>
                                            <Listeners>
                                                <Render Fn="function(f) {
                                                            f.el.on('keyup', function(e) {
                                                             if(window.event.keyCode==38 || window.event.keyCode==40 || window.event.keyCode==13){
                                                                return;
                                                             }
                                                             Coolite.AjaxMethods.PYsearch(f.getRawValue(), 'placeStore');
                                                            });
                                                            }
                                                            " />
                                            </Listeners>
                                        </ext:ComboBox>
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
                                            ID="gsdj"
                                            runat="server" 
                                            StoreID="gsdjStore"
                                            DisplayField="INFONAME" 
                                            ValueField="INFOID"
                                            LoadingText="Searching..." 
                                            FieldLabel="工伤等级<font color='red'>*</font>"
                                                 
                                            >
                                        </ext:ComboBox>
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:TextArea ID="gsss" runat="server" FieldLabel="工伤事实" Height="30px">
                                        </ext:TextArea>
                                    </ext:Anchor>
                                  
                                    <%--<ext:Anchor Horizontal="92%">
                                        <ext:ComboBox 
                                            ID="cbbPerson"
                                            runat="server"
                                            ReadOnly="true"
                                            PageSize="10"
                                            FieldLabel="录入人员"        
                                            HideTrigger="true">
                                        </ext:ComboBox>
                                    </ext:Anchor>--%>
                                    
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
            Title="工伤明细信息" 
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
                        Title="工伤基本信息" 
                        FormGroup="true" 
                        Width="580px"
                        >
                        <Body>
                            <table style="width:100%;">
                                <tr>
                                    <td style="width:33%;">
                                        <span class="x-label-text">发生部门:</span>
                                        <ext:TextField ID="lbl_deptname" runat="server" Cls="x-textfeild-style" />
                                    </td>
                                    <td style="width:34%;">
                                        <span class="x-label-text">受伤人员:</span>
                                        <ext:TextField ID="lbl_Name" runat="server" Cls="x-textfeild-style" />
                                    </td>
                                    <td style="width:34%;">
                                        <span class="x-label-text">工伤等级:</span>
                                        <ext:TextField ID="lbl_ggdj" runat="server" Cls="x-textfeild-style" />
                                    </td>
                                </tr>
                                <tr>
                                 <td style="width:33%;">
                                        <span class="x-label-text">录入人员:</span>
                                        <ext:TextField ID="lbl_inputname" runat="server" Cls="x-textfeild-style" />
                                    </td>
                                   
                                    <td style="width:34%;">
                                        <span class="x-label-text">发生时间:</span>
                                        <ext:TextField ID="lbl_happydate" runat="server" Cls="x-textfeild-style" />
                                    </td>
                                    <td style="width:34%;">
                                        <span class="x-label-text">发生地点:</span>
                                        <ext:TextField ID="lbl_place" runat="server" Cls="x-textfeild-style" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:33%;">
                                        <span class="x-label-text">员工扣分:</span>
                                        <ext:TextField ID="lbl_ygkf" runat="server" Cls="x-textfeild-style" />
                                    </td>
                                    <td style="width:34%;">
                                        <span class="x-label-text">科区扣分:</span>
                                        <ext:TextField ID="lbl_kqkf" runat="server" Cls="x-textfeild-style" />
                                    </td>
                                    <td style="width:34%;">
                                        <span class="x-label-text">部门罚款:</span>
                                        <ext:TextField ID="lbl_bmfk" runat="server" Cls="x-textfeild-style" />
                                    </td>
                                </tr>
                                <tr>
                                     <td style="width:33%;">
                                        <span class="x-label-text">工伤描述:</span>
                                        <ext:TextArea ID="lbl_ggms" runat="server" Cls="x-textfeild-style" Width="300px" />
                                    </td>
                                    <td style="width:33%;">
                                        <span class="x-label-text">备注:</span>
                                        <ext:TextArea ID="bz" runat="server" Cls="x-textfeild-style" Width="300px" />
                                    </td>
                                     <td colspan="2">
                                        <span class="x-label-text">录入时间:</span>
                                        <ext:TextField ID="lbl_inputdate" runat="server"  />
                                    </td>    
                                </tr> 
                            </table>
                        </Body>
                    </ext:Panel>
                    
                </ext:ContainerLayout>
            </Body>
        </ext:Window>
    </form>
</body>
</html>
