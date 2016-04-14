<%@ Page Language="C#" AutoEventWireup="true" CodeFile="YHInputNew.aspx.cs" Inherits="HiddenDanage_YHInputNew" %>

<%@ Register assembly="Coolite.Ext.Web" namespace="Coolite.Ext.Web" tagprefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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

        function nodeLoad(node) {
            Coolite.AjaxMethods.NodeLoad(node.id, {
                success: function(result) {
                    var data = eval("(" + result + ")");
                    node.loadNodes(data);
                },

                failure: function(errorMsg) {
                    Ext.Msg.alert('Failure', errorMsg);
                }
            });
        }

        var PersonSelector = {
            add: function(destination, records) {
                PersonChooserDialog.body.mask('数据处理中...');
                try {
                    if (destination.id == 'GridPanel3') {
                        for (var i = 0; i < records.length; i++) {
                            destination.addRecord(records[i].data);
                        }
                    }
                }
                catch (e) {
                    Ext.Msg.alert('提示', '已添加的人员！');
                }
                PersonChooserDialog.body.unmask();
            },

            remove: function(source) {
                source.deleteSelected();
            },

            removeAll: function(source) {
                source.store.removeAll();
            }
        };
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
        <%--<ext:Store ID="personStore" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="pernID" />
                        <ext:RecordField Name="pernName" />
                        <ext:RecordField Name="persNnmber" />
                        <ext:RecordField Name="pernLightNumber" />
                        <ext:RecordField Name="pyall" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>--%>
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
        <ext:Store ID="YHputinStore" runat="server">
        <Reader>
                <ext:JsonReader ReaderID="YHPutinID">
                    <Fields>
                        <ext:RecordField Name="YHPutinID"/> 
                        <ext:RecordField Name="DeptName"/> 
                        <ext:RecordField Name="YHContent" />
                        <ext:RecordField Name="PlaceName" />
                        <ext:RecordField Name="YHType" />
                        <%--<ext:RecordField Name="Name" /> --%>      
                        <ext:RecordField Name="BanCi" />
                        <%--<ext:RecordField Name="INTime" Type="Date" DateFormat="Y-m-dTh:i:s" />--%>
                        <ext:RecordField Name="PCTime" Type="Date" DateFormat="Y-m-dTh:i:s" />
                        <ext:RecordField Name="Remarks"/>  
                        <ext:RecordField Name="Status"/>                                                                                                                          
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
        
        <ext:Store ID="Store6" runat="server">
        <Reader>
                <ext:JsonReader ReaderID="YHPutinID">
                    <Fields>
                        <ext:RecordField Name="BanCi" />
                        <ext:RecordField Name="INTime" />
                        <ext:RecordField Name="PCTime" Type="Date" DateFormat="Y-m-dTh:i:s" />
                        <ext:RecordField Name="YHContent" />
                        <ext:RecordField Name="PlaceName" />
                        <ext:RecordField Name="YHLevel" />
                        <ext:RecordField Name="YHType" />
                        <ext:RecordField Name="Name" />
                        <ext:RecordField Name="DeptName"/>  
                        <ext:RecordField Name="Remarks"/> 
                        <ext:RecordField Name="YHPutinID"/> 
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
                    <ext:RecordField Name="HBm" />
                    <ext:RecordField Name="Yhswlevel" />
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
    <%--排查人选择--%>
    <ext:Store ID="SelectedStore" runat="server" OnSubmitData="SubmitData">
            <Reader>
                <ext:JsonReader ReaderID="Personnumber">
                    <Fields>
                        <ext:RecordField Name="Personnumber" />
                        <ext:RecordField Name="Name" />
                        <ext:RecordField Name="Deptname" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
    <ext:Hidden ID="Hidden1" runat="server">
    </ext:Hidden>
    
    <%--<ext:Hidden ID="hdnPerson" runat="server" />--%>
    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                <North>
                    <ext:FormPanel 
            ID="FormPanel1" 
            runat="server" 
            Title="隐患录入系统"
            Frame="true" Height="255"
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
                                            ID="cbbDept"
                                            runat="server" 
                                            StoreID="deptStore"
                                            DisplayField="deptName" 
                                            ValueField="deptID"
                                            HideTrigger="true"
                                            PageSize="10"
                                            FieldLabel="隐患部门<font color='red'>*</font>"
                                            ItemSelector="div.search-item"     
                                            > 
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
                                                            Coolite.AjaxMethods.PYsearch(f.getRawValue(), 'deptStore');
                                                           
                                                            });
                                                            }
                                                            " />
                                            </Listeners>
                                        </ext:ComboBox>
                                    </ext:Anchor>
                                       <ext:Anchor Horizontal="92%">
                                        <%--<ext:ComboBox 
                                            ID="cbbyh"
                                            runat="server" 
                                            StoreID="yhStore"
                                            DisplayField="yhContent" 
                                            ValueField="yhNumber"
                                            LoadingText="Searching..." 
                                            MinHeight="300" MaxHeight="400"
                                            ListWidth="600"
                                            FieldLabel="隐患内容<font color='red'>*</font>"
                                            ItemSelector="div.search-item" HideTrigger="true"       
                                            >
                                            <Template ID="Template2" runat="server">
                                               <tpl for=".">
                                                  <div class="search-item" ext:qtip="{yhContent}">
                                                     <span>{yhContent}</span>
                                                  </div>
                                               </tpl>
                                            </Template>
                                            <Listeners>
                                                <Select Handler="Coolite.AjaxMethods.SelectLoad();" />
                                                <Render Fn="function(f) {
                                                            f.el.on('keyup', function(e) {
                                                            if(window.event.keyCode==38 || window.event.keyCode==40 || window.event.keyCode==13){
                                                                return;
                                                             }
                                                             Coolite.AjaxMethods.PYsearch(f.getRawValue(), 'yhStore');
                                                            });
                                                            }
                                                            " />
                                            </Listeners>
                                        </ext:ComboBox>--%>
                                        <ext:ComboBox 
                                            ID="cbbyh"
                                            runat="server" 
                                            StoreID="yhStore"
                                            DisplayField="yhContent" 
                                            ValueField="yhNumber"
                                            LoadingText="Searching..." 
                                            MinHeight="300" MaxHeight="400"
                                            ListWidth="600" Width="500" 
                                            ForceSelection="true"
                                            FieldLabel="隐患内容<font color='red'>*</font>"
                                            ItemSelector="tr.list-item"    
                                            >
                                            <%--<Template ID="Template2" runat="server">
                                               <tpl for=".">
                                                  <div class="search-item" ext:qtip="{yhContent}">
                                                     <span>{yhContent}</span>
                                                  </div>
                                               </tpl>
                                            </Template>--%>
                                            <Template ID="Template5" runat="server">
                                                <Html>
					                                <tpl for=".">
						                                <tpl if="[xindex] == 1">
							                                <table class="cbStates-list">
								                                <tr>
									                                <th>隐患内容</th>
									                                <th>工作任务</th>
									                                <th>工序</th>
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
                                        <ext:TextArea ID="TextArea2" runat="server" FieldLabel="危险源内容描述" Height="95" ReadOnly="true">
                                        </ext:TextArea>
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:DateField ID="dfPCtime" FieldLabel="排查时间" runat="server" Vtype="daterange">
                                        </ext:DateField>
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
                                        <ext:ComboBox 
                                            ID="cbbplace"
                                            runat="server" 
                                            StoreID="placeStore"
                                            DisplayField="placName" 
                                            ValueField="placID"
                                            AllowBlank="false"
                                            PageSize="10"
                                            FieldLabel="排查地点<font color='red'>*</font>"
                                            ItemSelector="div.search-item" HideTrigger="true" 
                                            >
                                            <Template ID="Template3" runat="server">
                                               <tpl for=".">
                                                  <div class="search-item" ext:qtip="{yhContent}">
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
                                                            " Delay="1000" />
                                            </Listeners>
                                        </ext:ComboBox>
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <%--<ext:TriggerField 
                                            ID="tfPerson" 
                                            FieldLabel="排查人员<font color='red'>*</font>"  
                                            ReadOnly="true"
                                            runat="server" 
                                            EmptyText="点击按钮选择 -->">
                                            <Listeners>
                                                <TriggerClick Handler="#{PersonChooserDialog}.show();" />
                                            </Listeners>
                                        </ext:TriggerField>--%>

                                        <ext:ComboBox 
                                            ID="cbbPerson"
                                            runat="server"
                                            PageSize="10"
                                            FieldLabel="排查人员<font color='red'>*</font>"        
                                            AllowBlank="false"
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
                                            <Triggers>
                                                <ext:FieldTrigger Icon="Clear" Qtip="清除内容" /><%--HideTrigger="true"--%>
                                                <ext:FieldTrigger Icon="Ellipsis" Qtip="多人排查" />
                                            </Triggers>
                                            <Listeners>
                                                <Select Handler="this.triggers[0].show();" />
                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); }else if(index==1){#{PersonChooserDialog}.show();}" />
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
                                        <ext:ComboBox 
                                            ID="cbbBc"
                                            runat="server" Editable="false" 
                                            FieldLabel="发生班次<font color='red'>*</font>" SelectedIndex="0">
                                            <Items>
                                                <ext:ListItem Text="早班" Value="早班" />
                                                <ext:ListItem Text="中班" Value="中班" />
                                                <ext:ListItem Text="夜班" Value="夜班" />
                                            </Items>
                                        </ext:ComboBox>
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:TextArea ID="TextArea1" runat="server" FieldLabel="备注">
                                        </ext:TextArea>
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:ComboBox 
                                            ID="cbbStatus"
                                            runat="server" 
                                            AllowBlank="false" Editable="false"
                                            FieldLabel="现场整改<font color='red'>*</font>" SelectedIndex="1">
                                            <Items>
                                                <ext:ListItem Text="是" Value="现场整改" />
                                                <ext:ListItem Text="否" Value="新增" />
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
                <ext:Button ID="Button2" runat="server" Text="复查" Icon="ZoomIn" >
                    <AjaxEvents>
                        <Click OnEvent="btnFCFKClick"></Click>
                    </AjaxEvents>
                </ext:Button>
                <ext:Button ID="btn_apply" runat="server" Text="新增隐患申请" Icon="Add" >
                    <AjaxEvents>
                        <Click OnEvent="btnApplyClick"></Click>
                    </AjaxEvents>
                </ext:Button>
            </Buttons>
        </ext:FormPanel>
                </North>
                <Center>
                    <ext:GridPanel ID="GridPanel1" runat="server"  Title="隐患信息" StoreID="YHputinStore" AutoScroll="true" StripeRows="true" AutoExpandColumn="yhcontent">
            <ColumnModel ID="ColumnModel1" runat="server" >
                <Columns>
                    <ext:Column Header="编号" DataIndex="YHPutinID" Width="70" >
                    </ext:Column>
                    <ext:Column  Header="部门" Sortable="true" Width="90"
                        DataIndex="DeptName" >
                    </ext:Column>
                    <ext:Column ColumnID="yhcontent" Header="隐患内容" Sortable="true" DataIndex="YHContent">
                        <Renderer Fn="qtip" />
                    </ext:Column>
                    <ext:Column Header="地点" Sortable="true"  DataIndex="PlaceName" Width="120">
                    </ext:Column>
                    <ext:Column  Header="班次" Sortable="true" Width="50"
                        DataIndex="BanCi" >
                    </ext:Column>
                    <ext:Column  Header="类型" Sortable="true" Width="50"
                        DataIndex="YHType" >
                    </ext:Column>
                    <ext:Column Header="排查时间" Width="70" Sortable="true" DataIndex="PCTime" >
                        <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" />
                    </ext:Column>
                    <ext:Column  Header="备注" Sortable="true" Width="250"
                        DataIndex="Remarks" >
                    </ext:Column>
                    <ext:Column  Header="状态" Sortable="true" Align="Center" Width="90"
                        DataIndex="Status" >
                        <Renderer Fn="change" />
                    </ext:Column>
                </Columns>
            </ColumnModel>
            <%--<View>
                <ext:GridView ForceFit="true" />
            </View>--%>
            <LoadMask ShowMask="true" />
            <BottomBar>
                <ext:PagingToolBar ID="PagingToolBar1" runat="server" PageSize="10" StoreID="YHputinStore" />
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
                                <ext:Column Header="隐患描述" Width="150" Sortable="false" DataIndex="HBm">
                                    <Renderer Fn="qtip" />
                                </ext:Column>
                                <ext:Column Header="隐患级别" Width="70" Sortable="true" DataIndex="Yhswlevel" >
                                </ext:Column>
                                 <ext:Column Header="隐患积分" Width="70" Sortable="true" DataIndex="Scores" >
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
    
    <ext:Window ID="Window1"  runat="server" 
            BodyStyle="padding:5px;" 
            ButtonAlign="Right"
            Frame="true" 
            Title="隐患复查反馈"
            AutoHeight="true"
            Width="685px"
            Modal="true"
            ShowOnLoad="false">
            <Body>
            <ext:Hidden ID="hdnid" runat="server">
            </ext:Hidden>
            <ext:Hidden ID="Hidden2" runat="server">
            </ext:Hidden>
            <ext:FormLayout ID="FormLayout3" runat="server" LabelWidth="60">
            <ext:Anchor Horizontal="100%">
                <ext:TabPanel ID="TabPanel1" runat="server" ActiveTabIndex="0"  Height="300px">
                    <Tabs>
                        <ext:Tab ID="Tab1" runat="server" Title="隐患信息">
                            <Body>
                                <ext:AnchorLayout ID="AnchorLayout1" Horizontal="100%" runat="server">
                                <ext:Anchor Horizontal="100%">
                                <ext:GridPanel 
                                ID="GridPanel2" 
                                runat="server"
                                StoreID="Store6"
                                StripeRows="true"
                                AutoExpandColumn="YHPutinID" 
                                Collapsible="false"
                                Width="660px"
                                Height="272px">
                                <ColumnModel ID="ColumnModel3" runat="server">
		                            <Columns>
		                                <ext:Column ColumnID="YHPutinID" Header="编号" Width="70" DataIndex="YHPutinID" >
                                        </ext:Column>
                                        <ext:Column Header="内容" Width="170" Sortable="true" DataIndex="YHContent" >
                                            <Renderer Fn="qtip" />
                                        </ext:Column>
                                        <ext:Column Header="部门" Width="60" Sortable="true" DataIndex="DeptName" >
                                        </ext:Column>
                                        <ext:Column Header="地点" Width="100" Sortable="true" DataIndex="PlaceName" >
                                        </ext:Column>
                                        <ext:Column Header="排查人" Width="60" Sortable="true" DataIndex="Name" >
                                        </ext:Column>
                                        <ext:Column Header="排查时间" Width="80" Sortable="true" DataIndex="PCTime" >
                                        <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" /></ext:Column>
                                        <ext:Column Header="类型" Width="60" Sortable="true" DataIndex="YHType" >
                                        </ext:Column>
                                        <ext:Column Header="级别" Width="60" Sortable="true" DataIndex="YHLevel" >
                                        </ext:Column>
		                            </Columns>
                                </ColumnModel>
                                <LoadMask ShowMask="true" />
                                <BottomBar>
                                    <ext:PagingToolBar ID="PagingToolBar2" runat="server" PageSize="10" />
                                 </BottomBar>
                                <SelectionModel>
                                        <ext:CheckboxSelectionModel ID="CheckboxSelectionModel2" SingleSelect="true" runat="server" />                   
                                </SelectionModel>
                                <AjaxEvents>
                                    <Click OnEvent="FCRowClick"></Click>
                                </AjaxEvents>
                            </ext:GridPanel>
                            </ext:Anchor>  
                            </ext:AnchorLayout>
                            </Body>
                        </ext:Tab>
                        <ext:Tab ID="Tab2" runat="server" Title="复查意见" Width="300" Height="240">
                        <Body>
                        <table style="width:100%; height: 150px;">                          
                            <tr>
                                 <td>
                                    <span>复查意见:</span>
                                    
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <ext:TextArea ID="fcfk_fcyj" runat="server" Width="300px" Height="60px" MinChars="1" />
                                </td>
                            </tr>
                           
                            <tr> 
                                <td style="width:30%"><span>复查情况:</span></td>
                            </tr>                           
                            <tr>
                                <td style="text-align:left; width:300px" >                                                                 
                                    <ext:ComboBox ID="fcfk_fcqk" runat="server" Width="300">
                                        <Items>
                                            <ext:ListItem Text="复查通过" Value="复查通过" />
                                            <ext:ListItem Text="复查未通过" Value="复查未通过" />
                                        </Items>
                                    </ext:ComboBox>
                                </td>
                            </tr>
                        </table>
                        </Body>
                            <Buttons>
                                <ext:Button ID="Button3" Text="提交" runat="server">
                                     <AjaxEvents>
                                        <Click OnEvent="btnFCsubmitClick"></Click>
                                    </AjaxEvents>
                                </ext:Button>
                            </Buttons>
                            <Buttons>
                                <ext:Button ID="Button4" Text="返回" runat="server">
                                     <AjaxEvents>
                                        <Click OnEvent="btnReturnClick"></Click>
                                    </AjaxEvents>
                                </ext:Button>
                            </Buttons>
                        </ext:Tab>
                    </Tabs>
                </ext:TabPanel>     
                </ext:Anchor>
            </ext:FormLayout>
            </Body>
        </ext:Window>
     
    <ext:Window ID="Window2"  runat="server" 
            BodyStyle="padding:5px;" 
            ButtonAlign="Right"
            Frame="true" 
            Title="新增隐患申请"
            AutoHeight="true"
            Width="400"
            Modal="true"
            ShowOnLoad="false"
            X="100"
            Y="60">
            <Body>
                <ext:Panel runat="server" ID="panelapply" Height="200">
                <Body>
                <table style="width:100%">
                    <tr>
                        <td align="right" style="width:20%">
                            <ext:Label runat="server" ID="lbl_Context" Text="隐患内容" />
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
        ID="PersonChooserDialog" 
        runat="server"
        Icon="User"
        Title="人员选择"
        Height="553"
        Width="700"
        Modal="true" ShowOnLoad="false"        
        BodyStyle="padding:5px;"
        BodyBorder="false"
        >
        <Body>        
            <ext:ColumnLayout ID="ColumnLayout2" runat="server" FitHeight="true">
                <ext:LayoutColumn ColumnWidth="0.5">
                    <ext:TreePanel ID="tpPerson" runat="server" Header="false" AutoScroll="true">
                        <Root>
                           <ext:TreeNode NodeID="-1" Text="人" />
                        </Root>
                        <Listeners>
                            <BeforeLoad Fn="nodeLoad" />
                        </Listeners>
                    </ext:TreePanel>
                </ext:LayoutColumn>
                <ext:LayoutColumn>
                    <ext:Panel ID="Panel4" runat="server" Width="35" BodyStyle="background-color: transparent;" Border="false">
                        <Body>
                            <ext:AnchorLayout ID="AnchorLayout2" runat="server">
                                <ext:Anchor Vertical="40%">
                                    <ext:Panel ID="Panel5" runat="server" Border="false" BodyStyle="background-color: transparent;" />
                                </ext:Anchor>
                                <ext:Anchor>
                                    <ext:Panel ID="Panel6" runat="server" Border="false" BodyStyle="padding:5px;background-color: transparent;">
                                        <Body>
                                            <ext:Button ID="Button1" runat="server" Icon="ResultsetNext" StyleSpec="margin-bottom:2px;">
                                                <Listeners>
                                                    <Click Handler="
                                                    var node=#{tpPerson}.getSelectionModel().getSelectedNode();
                                                    try{
                                                    if(node.leaf){
                                                    PersonSelector.add(GridPanel3,
                                                    [new Ext.data.Record({
                                                    Personnumber:node.id ,
                                                    Name:node.text,
                                                    Deptname:node.attributes.data
                                                    })]);
                                                    }
                                                    }catch(e){
                                                    ;
                                                    }" />
                                                </Listeners>
                                                <ToolTips>
                                                    <ext:ToolTip ID="ToolTip1" runat="server" Title="提示" Html="添加左侧人员结构中选中的人" />
                                                </ToolTips>
                                            </ext:Button>
                                            <ext:Button ID="Button5" runat="server" Icon="ResultsetLast" StyleSpec="margin-bottom:2px;" Disabled="true">
                                                <Listeners>
                                                    <Click Handler="PersonSelector.addAll();" />
                                                </Listeners>
                                                <ToolTips>
                                                    <ext:ToolTip ID="ToolTip2" runat="server" Title="提示" Html="全部添加" />
                                                </ToolTips>
                                            </ext:Button>
                                            <ext:Button ID="btnRemove" runat="server" Icon="ResultsetPrevious" StyleSpec="margin-bottom:2px;">
                                                <Listeners>
                                                    <Click Handler="PersonSelector.remove( GridPanel3);" />
                                                </Listeners>
                                                <ToolTips>
                                                    <ext:ToolTip ID="ToolTip3" runat="server" Title="提示" Html="移除右侧选中人员" />
                                                </ToolTips>
                                            </ext:Button>
                                            <ext:Button ID="btnRemoveAll" runat="server" Icon="ResultsetFirst" StyleSpec="margin-bottom:2px;">
                                                <Listeners>
                                                    <Click Handler="PersonSelector.removeAll(GridPanel3);" />
                                                </Listeners>
                                                <ToolTips>
                                                    <ext:ToolTip ID="ToolTip4" runat="server" Title="提示" Html="移除右侧全部人员" />
                                                </ToolTips>
                                            </ext:Button>
                                        </Body>
                                    </ext:Panel>
                                </ext:Anchor>
                            </ext:AnchorLayout>
                        </Body>
                    </ext:Panel>
                </ext:LayoutColumn>
                <ext:LayoutColumn ColumnWidth="0.5">
                    <ext:GridPanel 
                        runat="server" 
                        ID="GridPanel3" 
                        EnableDragDrop="false"
                        AutoExpandColumn="SelectPer" 
                        StoreID="SelectedStore">
                        <Listeners>
                        </Listeners>
                        <ColumnModel ID="ColumnModel4" runat="server">
                            <Columns>
                                <ext:Column  Header="选中人员" DataIndex="Name" Sortable="true" Width="90" />  
                                <ext:Column ColumnID="SelectPer" Header="单位" DataIndex="Deptname" />                   
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:CheckboxSelectionModel runat="server" ID="CheckboxSelectionModel3" />
                            <%--<ext:RowSelectionModel ID="RowSelectionModel2" runat="server" />--%>
                        </SelectionModel>  
                        <SaveMask ShowMask="true" />
                    </ext:GridPanel>
                </ext:LayoutColumn>
            </ext:ColumnLayout>                
        </Body>
        <Buttons>
            <ext:Button runat="server" ID="OkBtn" Text="确 定" Icon="Disk">
                <Listeners>
                    <Click Handler="#{GridPanel3}.submitData();;#{PersonChooserDialog}.hide();" />
                </Listeners>
            </ext:Button>
            <ext:Button runat="server" ID="CancelBtn" Text="取 消" Icon="Cancel">
                <Listeners>
                    <Click Handler="#{PersonChooserDialog}.hide();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    </form>
</body>
</html>
