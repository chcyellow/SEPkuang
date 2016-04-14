<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SWInput.aspx.cs" Inherits="YSNewProcess_SWInput" %>

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
    </script>
    <script type="text/javascript">
        var template = '<span style="color:{0};">{1}</span>';

        var SWchange = function(value) {
            var color;
            if (!value)
                color = '#cc0000';
            else
                color = 'black';
            return String.format(template, color, value ? '是' : '否');
        }
    </script>
    <script type="text/javascript">
        var template = '<span style="color:{0};"><b>{1}</b></span>';

        var change = function(value) {
            var color;
            if (value.toString().replace(/(^\s*)|(\s*$)/g, "") == '严重')
                color = '#cc0000';
            else if (value.toString().replace(/(^\s*)|(\s*$)/g, "") == '一般')
                color = '#FF9900';
            else
                color = 'blue';

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
        
         .cbStates-list 
        {
            width: 600px;
            font: 11px tahoma,arial,helvetica,sans-serif;
        }
        
        .cbStates-list th {
            font-weight: bold;
        }
        
        .cbStates-list td, .cbStates-list th {
            padding: 3px;
        }
    </style>
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
        <ext:Store ID="deptStore" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="deptID" />
                        <ext:RecordField Name="deptName" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="yhStore" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="yhContent" />
                        <ext:RecordField Name="yhNumber" />
                        <ext:RecordField Name="Gzrwname" />
                        <ext:RecordField Name="Gxname" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
         <ext:Store ID="SWinputStore" runat="server">
        <Reader>
              
                <ext:JsonReader ReaderID="ID">
                    <Fields>
                        <ext:RecordField Name="ID" />
                        <ext:RecordField Name="SWContent" />
                        <ext:RecordField Name="PlaceName" />
                        <ext:RecordField Name="Levelname" />
                        <ext:RecordField Name="PCname" />
                        <ext:RecordField Name="Name" />
                        <ext:RecordField Name="DeptName"/>
                        <ext:RecordField Name="BanCi" />
                        <ext:RecordField Name="PCTime" Type="Date" DateFormat="Y-m-dTh:i:s" />  
                        <ext:RecordField Name="Isend" Type="Boolean" />
                        <ext:RecordField Name="Remarks" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="PCpersonStore" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="Name" />
                        <ext:RecordField Name="Personnumber" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <%-- 以下store高级查询用--%>
        <ext:Store ID="SearchBLStore" runat="server" OnRefreshData="SearchBLStoreRefresh">
        <Reader>
            <ext:JsonReader ReaderID="HNumber">
                <Fields>
                    <ext:RecordField Name="HNumber" />
                    <ext:RecordField Name="Zyid" />
                    <ext:RecordField Name="Zyname" />
                    <ext:RecordField Name="Gzrwname" />
                    <ext:RecordField Name="Gxname" />
                    <ext:RecordField Name="Gldxid" />
                    <ext:RecordField Name="Gldxname" />
                    <ext:RecordField Name="Fxlxid" />
                    <ext:RecordField Name="Fxlx" />
                    <ext:RecordField Name="Glrnid" />
                    <ext:RecordField Name="Glrn" />
                    <ext:RecordField Name="HContent" />
                    <ext:RecordField Name="HConsequences" />
                    <ext:RecordField Name="MStandards" />
                    <ext:RecordField Name="SwBm" />
                    <ext:RecordField Name="Swlevel" />
                    <ext:RecordField Name="Scores" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="FXStore" runat="server" >
        <Reader>
            <ext:JsonReader ReaderID="INFOID">
                <Fields>
                    <ext:RecordField Name="INFOID" />
                    <ext:RecordField Name="INFONAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="TypeStore" runat="server" >
        <Reader>
            <ext:JsonReader ReaderID="INFOID">
                <Fields>
                    <ext:RecordField Name="INFOID" />
                    <ext:RecordField Name="INFONAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="WorkTaskStore" runat="server" >
        <Reader>
            <ext:JsonReader ReaderID="Worktaskid">
                <Fields>
                    <ext:RecordField Name="Worktaskid" />
                    <ext:RecordField Name="Worktask" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="GLDXStore" runat="server" >
        <Reader>
            <ext:JsonReader ReaderID="INFOID">
                <Fields>
                    <ext:RecordField Name="INFOID" />
                    <ext:RecordField Name="INFONAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="GLRYStore" runat="server" >
        <Reader>
            <ext:JsonReader ReaderID="INFOID">
                <Fields>
                    <ext:RecordField Name="INFOID" />
                    <ext:RecordField Name="INFONAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
        <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                <North>
                    <ext:FormPanel 
            ID="FormPanel1" 
            runat="server" 
            Title="三违录入系统"
             Height="230"
            Frame="true" 
            
            BodyStyle="padding:5px;" 
            ButtonAlign="Center" style="font-size: 20px">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout1" runat="server">
                    <ext:LayoutColumn ColumnWidth=".5">
                        <ext:Panel ID="Panel1" runat="server" Border="false" Header="false">
                            <Defaults>
                                <ext:Parameter Name="AllowBlank" Value="false" Mode="Raw" />
                                <ext:Parameter Name="MsgTarget" Value="side" />
                            </Defaults>
                            <Body>
                                <ext:FormLayout ID="FormLayout1" runat="server" LabelAlign="Left" LabelWidth="70" >
                                   
                                    <ext:Anchor Horizontal="92%">
                                        
                                        <ext:ComboBox 
                                            ID="cbbSwperson"
                                            runat="server" 
                                            StoreID="personStore"
                                            DisplayField="pernName" 
                                            ValueField="persNnmber"
                                            LoadingText="Searching..." 
                                            Width="570"
                                            ListWidth="600"
                                            
                                            HideTrigger="true"
                                            FieldLabel="三违人员<font color='red'>*</font>"
                                            ItemSelector="tr.list-item"
                                            ForceSelection="true"         
                                            > 
                                            <Template ID="Template1" runat="server">
                                                <Html>
					                                <tpl for=".">
						                                <tpl if="[xindex] == 1">
							                                <table class="cbStates-list">
								                                <tr>
									                                <th>工号</th>
									                                <th>姓名</th>
									                                <th>灯号</th>
									                                <th>单位</th>
								                                </tr>
						                                </tpl>
						                                <tr class="list-item">
							                                <td width="100px">{persNnmber}</td>
							                                <td width="100px">{pernName}</td>
							                                <td width="100px">{pernLightNumber}</td>
							                                <td width="150px">{DeptName}</td>
						                                </tr>
						                                <tpl if="[xcount-xindex]==0">
							                                </table>
						                                </tpl>
					                                </tpl>
				                                </Html>
                                            </Template>
                                            <%--<Template ID="Template1" runat="server">
                                                

                                               <tpl for=".">
                                                  <div class="search-item">
                                                     <h3><span>{persNnmber}</span><span>{pernName}</span><span>{pernLightNumber}</span>{DeptName}</h3>
                                                     
                                                  </div>
                                               </tpl>

                                            </Template>--%>
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
                                                ID="cbbBc"
                                                runat="server" 
                                                Editable="false"
                                                FieldLabel="发生班次<font color='red'>*</font>" SelectedIndex="0">
                                                <Items>
                                                <ext:ListItem Text="早班" Value="早班" />
                                                 <ext:ListItem Text="中班" Value="中班" />
                                                  <ext:ListItem Text="夜班" Value="夜班" />
                                                  
                                                </Items>
                                              
                                            </ext:ComboBox>
                                        </ext:Anchor>
                                           <ext:Anchor Horizontal="92%">
                                            
                                            <ext:ComboBox 
                                            ID="cbbSWcontent"
                                            runat="server" 
                                            StoreID="yhStore"
                                            DisplayField="yhContent" 
                                            ValueField="yhNumber"
                                            LoadingText="Searching..." 
                                            Width="570" MaxHeight="400"
                                            ListWidth="600"
                                            FieldLabel="三违描述<font color='red'>*</font>" HideTrigger="true"
                                            ItemSelector="tr.list-item"
                                            ForceSelection="true"        
                                            >
                                            <Template ID="Template5" runat="server">
                                                <Html>
					                                <tpl for=".">
						                                <tpl if="[xindex] == 1">
							                                <table class="cbStates-list">
								                                <tr>
									                                <th>三违内容</th>
									                                <th>级别</th>
									                                <th>专业</th>
								                                </tr>
						                                </tpl>
						                                <tr class="list-item">
							                                <td style="padding:3px 0px;" ext:qtip="{yhContent}" width="330px">{yhContent}</td>
							                                <td width="120px">{Gzrwname}</td>
							                                <td width="150px">{Gxname}</td>
						                                </tr>
						                                <tpl if="[xcount-xindex]==0">
							                                </table>
						                                </tpl>
					                                </tpl>
				                                </Html>
                                            </Template>
                                            <Triggers>
                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除内容" />
                                                <ext:FieldTrigger Icon="Ellipsis" Qtip="高级查询" />
                                            </Triggers>
                                            <Listeners>
                                                <Select Handler="Coolite.AjaxMethods.SelectLoad();this.triggers[0].show();" />
                                                <Render Fn="function(f) {
                                                            f.el.on('keyup', function(e) {
                                                            if(window.event.keyCode==38 || window.event.keyCode==40 || window.event.keyCode==13){
                                                                return;
                                                             }
                                                             Coolite.AjaxMethods.PYsearch(f.getRawValue(), 'yhStore');
                                                            });
                                                            }
                                                            " />
                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide();#{TextArea2}.setValue(''); }else if(index==1){#{SearchBLWindow}.show();}" />

                                            </Listeners>
                                        </ext:ComboBox>
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:TextArea ID="TextArea2" runat="server" FieldLabel="危险源内容描述" ReadOnly="true" AllowBlank="true">
                                        </ext:TextArea>
                                    </ext:Anchor>
                                    
                                </ext:FormLayout>
                            </Body>
                            </ext:Panel>
                    </ext:LayoutColumn>
                    <ext:LayoutColumn ColumnWidth=".5">
                        <ext:Panel ID="Panel2" runat="server" Border="false">
                            <Defaults>

                                <ext:Parameter Name="AllowBlank" Value="false" Mode="Raw"  />
                                <ext:Parameter Name="MsgTarget" Value="side" />
                            </Defaults>
                            <Body>
                                <ext:FormLayout ID="FormLayout2" runat="server" LabelAlign="Left" LabelWidth="70">
                                   
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
                                            FieldLabel="排查地点<font color='red'>*</font>"
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
                                       
                                    <ext:Anchor Horizontal="92%">
                                        <ext:DateField ID="dfPCtime" FieldLabel="排查时间<font color='red'>*</font>" runat="server" Vtype="daterange">                                  
                                        </ext:DateField>   
                                        
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                            
                                        <ext:ComboBox 
                                            ID="cbbPCperson"
                                            runat="server"
                                            PageSize="10"
                                            FieldLabel="排查人员<font color='red'>*</font>"        
                                            HideTrigger="true"
                                            StoreID="PCpersonStore"
                                            DisplayField="Name" 
                                            ValueField="Personnumber"
                                            ItemSelector="div.search-item"     
                                            > 
                                            <Template ID="Template4" runat="server">
                                               <tpl for=".">
                                                  <div class="search-item">
                                                     <span>{Name}</span>
                                                  </div>
                                               </tpl>
                                            </Template>
                                            <Listeners>
                                                <Render Fn="function(f) {
                                                            f.el.on('keyup', function(e) {
                                                            if(window.event.keyCode==38 || window.event.keyCode==40 || window.event.keyCode==13){
                                                                return;
                                                             }
                                                            Coolite.AjaxMethods.PYsearch(f.getRawValue(), 'PCpersonStore');
                                                           
                                                            });
                                                            }
                                                            " />
                                            </Listeners>
                                        </ext:ComboBox>
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:TextArea ID="TextArea1" runat="server" FieldLabel="备注" AllowBlank="true" Height="40">
                                        </ext:TextArea>
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:ComboBox 
                                            ID="cbbJctype"
                                            runat="server" 
                                            AllowBlank="false" Editable="false"
                                            FieldLabel="检查方式<font color='red'>*</font>" SelectedIndex="0">
                                            <Items>
                                                <ext:ListItem Text="矿查" Value="0" />
                                                <ext:ListItem Text="自查" Value="1" />
                                            </Items>
                                        </ext:ComboBox>
                                    </ext:Anchor>
                                </ext:FormLayout>
                            </Body>
                        </ext:Panel>
                    </ext:LayoutColumn>
                </ext:ColumnLayout>
            </Body>
            <Buttons>
                <ext:Button runat="server" ID="btnAdd" Text="提交" Icon="Add">
                    <Listeners>
                        <Click Handler="Coolite.AjaxMethods.AddClick('new');" />
                    </Listeners>
                </ext:Button>
                <ext:Button runat="server" ID="btnEdit" Text="修改" Icon="Disk">
                    <Listeners>
                        <Click Handler="Coolite.AjaxMethods.AddClick('edit');" />
                    </Listeners>
                </ext:Button>
                <ext:Button runat="server" ID="btnDelete" Text="删除" Icon="Delete">
                    <Listeners>
                        <Click Handler="Coolite.AjaxMethods.ClearClick();" />
                    </Listeners>
                </ext:Button>
                <ext:Button ID="btn_apply" runat="server" Text="新增三违申请" Icon="Add" >
                    <AjaxEvents>
                        <Click OnEvent="btnApplyClick"></Click>
                    </AjaxEvents>
                </ext:Button>

            </Buttons>
            
        </ext:FormPanel>
                </North>
                <Center>
                    <ext:GridPanel ID="GridPanel1" runat="server"  Title="三违信息" StoreID="SWinputStore" AutoScroll="true" StripeRows="true" AutoExpandColumn="remarkcol">
                    <ColumnModel ID="ColumnModel1" runat="server" >
                        <Columns>
                        <ext:Column Header="编号" DataIndex="ID" />
                            <ext:Column  Header="班次" Sortable="true" DataIndex="BanCi" />
                           
                            <ext:Column Header="排查时间" Sortable="true" DataIndex="PCTime">
                                <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" />   
                            </ext:Column>
                            <ext:Column Header="三违内容" Sortable="true" DataIndex="SWContent" Width="190">
                                <Renderer Fn="qtip" />
                            </ext:Column>
                            <ext:Column Header="三违级别" Sortable="true" DataIndex="Levelname" >
                                <Renderer Fn="change" />
                            </ext:Column>
                            <ext:Column Header="地点" Sortable="true"  DataIndex="PlaceName" />
                             <ext:Column  Header="排查人员"  Sortable="true" DataIndex="PCname" />
                             <ext:Column  Header="三违人员" Sortable="true"  DataIndex="Name" />
                             <ext:Column Header="备注" Sortable="true" DataIndex="Remarks" ColumnID="remarkcol">
                                <Renderer Fn="qtip" />
                            </ext:Column>
                            <ext:Column Header="是否闭合" Sortable="true" DataIndex="Isend">
                                <Renderer Fn="SWchange" />
                            </ext:Column>
                        </Columns>
                        
                    </ColumnModel>
                     <%--<View>
                        <ext:GridView ForceFit="true" />
                    </View>--%>
                 
                    <LoadMask ShowMask="true" />
                <BottomBar>
                    <ext:PagingToolBar ID="PagingToolBar1" runat="server" PageSize="10" StoreID="SWinputStore" />
                </BottomBar>
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
     <%--高级查询窗口--%>
    <ext:Window 
        ID="SearchBLWindow" 
        runat="server" 
        BodyStyle="padding:5px;" 
        ButtonAlign="Right"
        Frame="true" 
        Title="高级查询"
        Height="400px"
        Width="600px"
        Modal="true" Icon="Zoom" Resizable="false"
        ShowOnLoad="false"
        >
        <Body>
            <ext:BorderLayout runat="server" ID="SearchBL">
                <North>
                    <ext:Panel ID="Panel3" runat="server" Height="90" Header="false">
                        <Body>
                            <table width="100%">
                                <tr>
                                    <td width="80px">
                                        辨识单元：
                                    </td>
                                    <td>
                                        <ext:ComboBox ID="cbbSbsdy" runat="server" Width="120" EmptyText="请选择辨识单元.."
                                            DisplayField="INFONAME" 
                                            ValueField="INFOID" 
                                            StoreID="TypeStore"
                                            TypeAhead="true" 
                                            Mode="Local"
                                            ForceSelection="true" 
                                            TriggerAction="All" Editable="false">
                                            <Listeners>
                                                <Select Handler=" Coolite.AjaxMethods.GZRWLoad();" />
                                            </Listeners>
                                        </ext:ComboBox>
                                    </td>
                                    <td width="80px">
                                        工作任务：
                                    </td>
                                    <td>
                                        <ext:ComboBox ID="cbbSgzrw" runat="server" Width="120" EmptyText="请输入拼音检索.."
                                            DisplayField="Worktask" 
                                            ValueField="Worktaskid" 
                                            StoreID="WorkTaskStore"
                                            ListWidth="230"
                                            Disabled="true" HideTrigger="true">
                                            <Listeners>
                                                <Render Fn="function(f) {
                                                            f.el.on('keyup', function(e) {
                                                            if(window.event.keyCode==38 || window.event.keyCode==40 || window.event.keyCode==13){
                                                                return;
                                                             }
                                                             Coolite.AjaxMethods.PYsearch(f.getRawValue(), 'WorkTaskStore');
                                                            });
                                                            }
                                                            " Delay="1000" />
                                            </Listeners>
                                        </ext:ComboBox>
                                    </td>
                                    <td width="80px">
                                        风险类型：
                                    </td>
                                    <td>
                                        <ext:ComboBox ID="cbbSfxlx" runat="server" Width="120" EmptyText="请选择风险类型.."
                                            DisplayField="INFONAME" 
                                            ValueField="INFOID" 
                                            StoreID="FXStore"
                                            TypeAhead="true" 
                                            Mode="Local"
                                            ForceSelection="true" 
                                            TriggerAction="All" Editable="false">
                                            <Listeners>
                                                <Select Handler=" Coolite.AjaxMethods.GLDXLoad();" />
                                            </Listeners>
                                        </ext:ComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="80px">
                                        管理对象：
                                    </td>
                                    <td>
                                        <ext:ComboBox ID="cbbSgldx" runat="server" Width="120" DisplayField="INFONAME" 
                                            ValueField="INFOID" 
                                            StoreID="GLDXStore"
                                            TypeAhead="true" 
                                            Mode="Local"
                                            ForceSelection="true" 
                                            TriggerAction="All" Editable="false">
                                        </ext:ComboBox>
                                    </td>
                                    <td>
                                        管理人员：
                                    </td>
                                    <td>
                                        <ext:ComboBox ID="cbbSglry" runat="server" Width="120" DisplayField="INFONAME" 
                                            ValueField="INFOID" 
                                            StoreID="GLRYStore"
                                            TypeAhead="true" 
                                            Mode="Local"
                                            ForceSelection="true" 
                                            TriggerAction="All" Editable="false">
                                        </ext:ComboBox>
                                    </td>
                                    <td>
                                        隐患描述：
                                    </td>
                                    <td>
                                        <ext:TextField ID="tfSyhms" runat="server" Width="120" EmptyText="请输入隐患关键字">
                                        </ext:TextField>
                                    </td>
                                </tr>
                            </table>
                            <%--<ext:FormLayout runat="server" ID="SearchFL">
                                <ext:Anchor>
                                    <ext:ComboBox ID="ComboBox1" runat="server" FieldLabel="辨识单元">
                                    </ext:ComboBox>
                                </ext:Anchor>
                                <ext:Anchor>
                                    <ext:ComboBox ID="ComboBox2" runat="server" FieldLabel="管理对象">
                                    </ext:ComboBox>
                                </ext:Anchor>
                            </ext:FormLayout>--%>
                        </Body>
                        <Buttons>
                            <ext:Button runat="server" ID="btnSearchBL" Text="查询" Icon="Zoom">
                                <Listeners>
                                    <%--<Click Handler="Coolite.AjaxMethods.SearchBLLoad();" />--%>
                                    <Click Handler="#{SearchBLStore}.reload();" />
                                </Listeners>
                            </ext:Button>
                            <ext:Button runat="server" ID="btnClearBL" Text="清除条件" Icon="Delete">
                                <Listeners>
                                    <Click Handler="Coolite.AjaxMethods.ClearBL();" />
                                </Listeners>
                            </ext:Button>
                        </Buttons>
                    </ext:Panel>
                </North>
                <Center>
                    <ext:GridPanel ID="gpSearchBL" runat="server" StoreID="SearchBLStore">
                        <ColumnModel ID="ColumnModel2" runat="server">
		                    <Columns>
                                <ext:Column Header="危险源编码" Width="90" DataIndex="HNumber" />
                                <ext:Column Header="辨识单元" Width="70" DataIndex="Zyname" />
                                <ext:Column Header="工作任务" Width="120" DataIndex="Gzrwname">
                                    <Renderer Fn="qtip" />
                                </ext:Column>
                                <ext:Column Header="工序" Width="120" DataIndex="Gxname">
                                    <Renderer Fn="qtip" />
                                </ext:Column>
                                <ext:Column Header="风险类型" Width="70" DataIndex="Fxlx" />
                                <ext:Column Header="三违描述" Width="150" Sortable="false" DataIndex="SwBm">
                                    <Renderer Fn="qtip" />
                                </ext:Column>
                                <ext:Column Header="三违级别" Width="70" Sortable="true" DataIndex="Swlevel" >
                                    <Renderer Fn="change" />
                                </ext:Column>
                                 <ext:Column Header="三违积分" Width="70" Sortable="true" DataIndex="Scores" >
                                </ext:Column>
                                <ext:Column Header="管理措施" Width="150" Sortable="true" DataIndex="HConsequences">
                                    <Renderer Fn="qtip" />
                                </ext:Column>
                                <ext:Column Header="管理标准" Width="150" Sortable="true" DataIndex="MStandards">
                                    <Renderer Fn="qtip" />
                                </ext:Column>
		                    </Columns>
                        </ColumnModel>
                        <LoadMask Msg="数据加载中，请稍候..." ShowMask="true" />
                        <AjaxEvents>
                            <DblClick OnEvent="SearchBLDblClick" />
                        </AjaxEvents>
                        <SelectionModel>
                            <ext:RowSelectionModel runat="server" ID="SearchBLrsm" SingleSelect="true" />
                        </SelectionModel>
                        <BottomBar>
                            <ext:PagingToolbar runat="server" ID="SearchBLptb" PageSize="10">
                                <Items>
                                    <ext:ToolbarSeparator />
                                    <ext:Label ID="lblsMSG" runat="server" Text="提示：双击行选择" StyleSpec="color:red;" />
                                </Items>
                            </ext:PagingToolbar>
                        </BottomBar>
                    </ext:GridPanel>
                </Center>
            </ext:BorderLayout>
        </Body>
    </ext:Window>
    
    <ext:Window ID="Window2"  runat="server" 
            BodyStyle="padding:5px;" 
            ButtonAlign="Right"
            Frame="true" 
            Title="新增三违申请"
            AutoHeight="true"
            Width="400"
            Modal="true"
            ShowOnLoad="false"
            X="100"
            Y="60">
            <Body>
                <ext:Panel runat="server" ID="panelapply">
                <Body>
                <table style="width:100%">
                    <tr>
                        <td align="right" style="width:20%">
                            <ext:Label runat="server" ID="lbl_Context" Text="三违内容" />
                        </td>
                        <td align="left" style="width:80%">
                            <ext:TextArea runat="server" ID="ta_Context" Text="" Height="100" Width="250" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width:20%">
                            <ext:Label runat="server" ID="lbl_date" Text="申请日期" />
                        </td>
                        <td align="left" style="width:80%">
                            <ext:DateField runat="server" ID="df_date" />
                        </td>
                    </tr>
                </table>
                </Body>
                <Buttons>
                    <ext:Button runat="server" ID="btn_Save" Text="保存/申请">
                        <AjaxEvents>
                            <Click OnEvent="Save"></Click>
                        </AjaxEvents>
                    </ext:Button>
                </Buttons>
                <Buttons>
                    <ext:Button runat="server" ID="btn_Cancel" Text="关闭">
                        <AjaxEvents>
                            <Click OnEvent="Cancel"></Click>
                        </AjaxEvents>
                    </ext:Button>
                </Buttons>
                </ext:Panel>
            </body>
        </ext:Window>
        
        <ext:Window 
        ID="FineWin" 
        runat="server"
        Icon="User"
        Title="罚款人员选择"
        Width="325"
        Modal="true"
        ShowOnLoad="false" AutoHeight="true"
        Resizable="false"
        CloseAction="Hide">
        <AutoLoad Url="" Mode="IFrame" ShowMask="true" MaskMsg="正在加载数据..." />
    </ext:Window>
    </form>
</body>
</html>
