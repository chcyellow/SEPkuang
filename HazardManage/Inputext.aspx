<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Inputext.aspx.cs" Inherits="HazardManage_Inputext" %>

<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
    .x-tree-node-anchor span span, div.thumb-wrap div h4 span {
    margin-left: 8px;
    padding-left: 25px;
    background: transparent url(../Images/new.gif) no-repeat 0px 3px;
}

.x-tree-node-expanded .x-tree-node-anchor span span{
    background: transparent url(../Images/newgray.gif) no-repeat 0px 3px;
}

    </style>
    <script type="text/javascript">
        var template = '<span style="color:{0};">{1}</span>';

        var change = function(value) {
            var color;
            if (value=='已发布') {
                color = 'red';
            }
            else {
                color = 'green';
            }
            return String.format(template, color, value);
        }
        var header = function(value) {
            var color;
            if (value == '新增危险源') {
                color = 'red';
            }
            else if (value == '通用危险源') {
                color = 'orange';
            }
            else {
                color = 'green';
            }
            return String.format(template, color, value);
        }

        var qtip = function(v, p) {//单元格提示
            //v : value , p : cell
            p.attr = 'ext:qtitle="" ext:qtip="' + v + '"';
            return v;
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
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN" CleanResourceUrl="false">
    </ext:ScriptManager>
    <ext:Store ID="hazardsStore" runat="server" GroupField="ISGROUP" >
        <Reader>
            <ext:JsonReader ReaderID="HAZARDSID">
                <Fields>
                    <ext:RecordField Name="HAZARDSID" Type="Int" />
                    <ext:RecordField Name="H_CONTENT" />
                    <ext:RecordField Name="FXLX" />
                    <ext:RecordField Name="H_CONSEQUENCES" />
                    <ext:RecordField Name="SGLX" />
                    <ext:RecordField Name="ISPASS" />
                    <ext:RecordField Name="ISGROUP" />
                    <ext:RecordField Name="ISFROM" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="bmStore1" runat="server" >
        <Reader>
            <ext:JsonReader ReaderID="INFOID">
                <Fields>
                    <ext:RecordField Name="INFOID" />
                    <ext:RecordField Name="INFONAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="bmStore2" runat="server" >
        <Reader>
            <ext:JsonReader ReaderID="INFOID">
                <Fields>
                    <ext:RecordField Name="INFOID" />
                    <ext:RecordField Name="INFONAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="bmStore3" runat="server" >
        <Reader>
            <ext:JsonReader ReaderID="INFOID">
                <Fields>
                    <ext:RecordField Name="INFOID" />
                    <ext:RecordField Name="INFONAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
     <ext:Store ID="TJDXStore" runat="server" >
        <Reader>
            <ext:JsonReader ReaderID="ROLEID">
                <Fields>
                    <ext:RecordField Name="ROLEID" />
                    <ext:RecordField Name="ROLENAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store1" runat="server" >
        <Reader>
            <ext:JsonReader ReaderID="M_OBJECTID">
                <Fields>
                    <ext:RecordField Name="M_OBJECTID" />
                    <ext:RecordField Name="NAME" />
                    <ext:RecordField Name="MO_F_PINYIN" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store2" runat="server" >
        <Reader>
            <ext:JsonReader ReaderID="M_OBJECTID">
                <Fields>
                    <ext:RecordField Name="M_OBJECTID" />
                    <ext:RecordField Name="NAME" />
                    <ext:RecordField Name="MO_F_PINYIN" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store3" runat="server" >
        <Reader>
            <ext:JsonReader ReaderID="M_OBJECTID">
                <Fields>
                    <ext:RecordField Name="M_OBJECTID" />
                    <ext:RecordField Name="NAME" />
                    <ext:RecordField Name="MO_F_PINYIN" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store4" runat="server" >
        <Reader>
            <ext:JsonReader ReaderID="M_OBJECTID">
                <Fields>
                    <ext:RecordField Name="M_OBJECTID" />
                    <ext:RecordField Name="NAME" />
                    <ext:RecordField Name="MO_F_PINYIN" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    
    <ext:Menu ID="ZYmenu" runat="server"> 
        <Items>
             <ext:MenuItem ID="miAddGZRW" runat="server" Text="添加工作任务" Icon="Add">
               <Listeners>
                     <Click Handler="Coolite.AjaxMethods.maddGZRW(this.parentMenu.node==null?'-1':this.parentMenu.node.id);" />                    
                 </Listeners>    
              </ext:MenuItem>
        </Items>
    </ext:Menu>
    
    <ext:Menu ID="GZRWmenu" runat="server"> 
        <Items>
             <ext:MenuItem ID="miAddGX" runat="server" Text="添加工序" Icon="Add">
                 <Listeners>
                     <Click Handler="Coolite.AjaxMethods.maddGX(this.parentMenu.node==null?'-1':this.parentMenu.node.id);" />                    
                 </Listeners>    
              </ext:MenuItem>
              <ext:MenuItem ID="miEditGZRW" runat="server" Text="修改工作任务" Icon="Anchor">
                 <Listeners>
                     <Click Handler="Coolite.AjaxMethods.meditGZRW(this.parentMenu.node==null?'-1':this.parentMenu.node.id);" />                    
                 </Listeners>    
              </ext:MenuItem>
              <ext:MenuItem ID="miDelGZRW" runat="server" Text="删除工作任务" Icon="Delete">
                 <Listeners>
                     <Click Handler="Coolite.AjaxMethods.mdelGZRW(this.parentMenu.node==null?'-1':this.parentMenu.node.id);" />                    
                 </Listeners>    
              </ext:MenuItem>
        </Items>
    </ext:Menu>
    
    <ext:Menu ID="GXmenu" runat="server"> 
        <Items>
             <ext:MenuItem ID="miEditGX" runat="server" Text="修改工序" Icon="Anchor">
                 <Listeners>
                     <Click Handler="Coolite.AjaxMethods.meditGX(this.parentMenu.node==null?'-1':this.parentMenu.node.id);" />                    
                 </Listeners>    
              </ext:MenuItem>
              <ext:MenuItem ID="miDelGX" runat="server" Text="删除工序" Icon="Delete">
                 <Listeners>
                     <Click Handler="Coolite.AjaxMethods.mdelGX(this.parentMenu.node==null?'-1':this.parentMenu.node.id);" />                    
                 </Listeners>    
              </ext:MenuItem>
        </Items>
    </ext:Menu>
    
    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                <West Collapsible="true" Split="true">
                    <ext:TreePanel ID="tpZY" runat="server" Width="200" Title="辨识单元" RootVisible="false" AutoScroll="true" >
                         <Listeners>
                            <ContextMenu Handler="
                            e.preventDefault();node.select();
                            switch(node.id.substr(0,1)){
                            case 'z':
                                #{ZYmenu}.node=node;
                                #{ZYmenu}.showAt(e.getXY());
                                break;
                            case 'w':
                            case 'p':
                                break;
                            case 'a':
                                #{GZRWmenu}.node=node;
                                #{GZRWmenu}.showAt(e.getXY());
                                break;
                            case 'b':
                                #{GXmenu}.node=node;
                                #{GXmenu}.showAt(e.getXY());
                                break;
                                }
                            "/>

                            <BeforeLoad Fn="nodeLoad" />
                        </Listeners>
                    </ext:TreePanel>
                </West>
                <Center>
                    <ext:Panel ID="Panel7" runat="server">
                        <Body>
                            <ext:BorderLayout ID="BorderLayout2" runat="server">
                                <North>
                                    <ext:GridPanel 
                                        ID="GridPanel1" 
                                        runat="server" 
                                        StoreID="hazardsStore" 
                                        Height="200"
                                        StripeRows="true"
                                        Title="危险源信息"
                                        Icon="BrickMagnify"
                                        Collapsible="false" Width="440"
                                        >
                                        <ColumnModel ID="ColumnModel1" runat="server">
		                                    <Columns>
		                                        <ext:Column Header="危险源" DataIndex="H_CONTENT" Groupable="false" Width="100" />
		                                        <ext:Column Header="风险类型" DataIndex="FXLX" Groupable="false" Width="40" />
                                                <ext:Column Header="风险后果及描述" DataIndex="H_CONSEQUENCES" Groupable="false" Width="200">
                                                    <Renderer Fn="qtip" />
                                                </ext:Column>
                                                <ext:Column Header="事故类型" DataIndex="SGLX" Groupable="false" Width="50" />
                                                <ext:Column Header="状态" DataIndex="ISPASS" Groupable="false" Width="50">
                                                    <Renderer Fn="change" />
                                                </ext:Column>
                                                <ext:Column Header="组" DataIndex="ISGROUP">
                                                    <Renderer Fn="header" />
                                                </ext:Column>
		                                    </Columns>
                                        </ColumnModel>
                                        <Plugins>
                                            <ext:GridFilters runat="server" ID="GridFilters1" FiltersText="过滤项">
                                                <Filters>
                                                    <ext:StringFilter DataIndex="H_CONTENT" />
                                                    <ext:StringFilter DataIndex="FXLX" />
                                                    <ext:StringFilter DataIndex="H_CONSEQUENCES" />
                                                    <ext:StringFilter DataIndex="SGLX" />
                                                </Filters>
                                            </ext:GridFilters>
                                        </Plugins>

                                        <SelectionModel>
                                            <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" runat="server" SingleSelect="true" />
                                        </SelectionModel>
                                        <BottomBar>
                                            <ext:Toolbar ID="tbbottom" runat="server">
                                                <Items>
                                                    <ext:Button 
                                                        ID="btnToggleGroups" 
                                                        runat="server" 
                                                        Text="展开/合并组"
                                                        Icon="TableSort"
                                                        Style="margin-left: 6px;"
                                                        AutoPostBack="false">
                                                        <Listeners>
                                                            <Click Handler="#{GridPanel1}.getView().toggleAllGroups();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:ToolbarSeparator />
                                                    <ext:Button 
                                                        ID="btnAdd" 
                                                        runat="server" 
                                                        Text="新增"
                                                        Icon="Add"
                                                        Style="margin-left: 6px;"
                                                        Disabled="true">
                                                        <Listeners>
                                                            <Click Handler="Coolite.AjaxMethods.btnAdd_Click();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:ToolbarSeparator />
                                                    <ext:Button runat="server" ID="btn_Save" Icon="Disk" Text="保存" Disabled="true">
                                                        <Listeners>
                                                            <Click Handler="Coolite.AjaxMethods.btnSave_Click();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:Button runat="server" ID="btnApprove" Icon="AwardStarGold1" Text="提交" Disabled="true">
                                                        <Listeners>
                                                            <Click Handler="#{Window1}.show();" />
                                                            <%--<Click Handler="Coolite.AjaxMethods.TJ();" />--%>
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:Button runat="server" ID="btnDelete" Icon="Delete" Text="删除" Disabled="true">
                                                        <Listeners>
                                                            <Click Handler="Coolite.AjaxMethods.Delete();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:ToolbarSeparator runat="server" ID="ts1" />
                                                    <ext:Button runat="server" ID="btnPYes" Icon="Accept" Text="审批" Disabled="true">
                                                        <Listeners>
                                                            <Click Handler="Coolite.AjaxMethods.btnSH();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:ToolbarSeparator runat="server" ID="ts2" />
                                                    <ext:Button runat="server" ID="btnPublish" Icon="Server" Text="发布" Disabled="true">
                                                        <Listeners>
                                                            <Click Handler="Coolite.AjaxMethods.Publish();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                        </BottomBar>
                                        <View>
                                            <ext:GroupingView  
                                                ID="GroupingView1"
                                                Collapsible="false"
                                                HideGroupedColumn="true"
                                                runat="server" 
                                                ForceFit="true"
                                                GroupTextTpl='{text} ({[values.rs.length]} {["条"]})'
                                                EnableRowBody="true">
                                            </ext:GroupingView>

                                        </View>
                                        <AjaxEvents>
                                            <Click OnEvent="RowClick">
                                                <ExtraParams>
                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel1}.getRowsValues())" Mode="Raw" />
                                                </ExtraParams>
                                            </Click>
                                        </AjaxEvents>
                                    </ext:GridPanel>
                                </North>
                                <Center>
                                    <ext:Panel ID="Panel1" runat="server" Height="300" AutoScroll="true">
                                        <Body>
                                            <ext:ColumnLayout ID="ColumnLayout1" runat="server">
                                                <ext:LayoutColumn ColumnWidth=".3">
                                                    <ext:Panel ID="Panel2" runat="server" Border="false" Header="false">
                                                        <Defaults>
                                                            <ext:Parameter Name="anchor" Value="95%" Mode="Value" />
                                                            <ext:Parameter Name="msgTarget" Value="side" Mode="Value" />
                                                        </Defaults>
                                                        <Body>
                                                            <ext:FormLayout ID="FormLayout1" runat="server" LabelWidth="65">
                                                                <ext:Anchor Horizontal="95%">
                                                                    <ext:ComboBox ID="extACCIDENT_TYPENUMBER" runat="server" FieldLabel="事故类型<font color='red'>*</font>" ValueField="INFOID" DisplayField="INFONAME" StoreID="bmStore1" AllowBlank="false" BlankText="必填项">
                                                                    </ext:ComboBox>
                                                                </ext:Anchor>
                                                                <ext:Anchor Horizontal="95%">
                                                                    <ext:ComboBox ID="extM_OBJECTNUMBER" runat="server" FieldLabel="管理对象" ValueField="M_OBJECTID" DisplayField="NAME" StoreID="Store1" MinChars="1">
                                                                        <Listeners>
                                                                            <Render Fn="function(f) {
                                                                                f.el.on('keyup', function(e) {
                                                                                Coolite.AjaxMethods.PYchange(f.getRawValue(),'Store1');
                                                                                f.setValue(f.getRawValue());
                                                                                });
                                                                                }
                                                                                " />
                                                                        </Listeners>
                                                                    </ext:ComboBox>
                                                                </ext:Anchor>
                                                                <ext:Anchor Horizontal="95%">
                                                                    <ext:TextArea ID="extH_CONTENT" runat="server" FieldLabel="危险源描述<font color='red'>*</font>" AllowBlank="false" BlankText="必填项">
                                                                    </ext:TextArea>
                                                                </ext:Anchor>
                                                                <ext:Anchor Horizontal="95%">
                                                                    <ext:TextArea ID="extM_STANDARDS" runat="server" FieldLabel="管理标准<font color='red'>*</font>" AllowBlank="false" BlankText="必填项">
                                                                    </ext:TextArea>
                                                                </ext:Anchor>
                                                                <ext:Anchor Horizontal="95%">
                                                                    <ext:ComboBox ID="extPROCESSINGMODE" runat="server" FieldLabel="处理方式" SelectedIndex="2">
                                                                        <Items>
                                                                            <ext:ListItem Text="集团公司处理" Value="集团公司处理" />
                                                                            <ext:ListItem Text="矿领导处理" Value="矿领导处理" />
                                                                            <ext:ListItem Text="安监处处理" Value="安监处处理" />
                                                                            <ext:ListItem Text="科区处理" Value="科区处理" />
                                                                        </Items>
                                                                    </ext:ComboBox>
                                                                </ext:Anchor>
                                                            </ext:FormLayout>
                                                        </Body>
                                                    </ext:Panel>
                                                </ext:LayoutColumn>
                                                <ext:LayoutColumn ColumnWidth=".3">
                                                <ext:Panel ID="Panel3" runat="server" Border="false">
                                                     <Defaults>
                                                        <ext:Parameter Name="anchor" Value="95%" Mode="Value" />
                                                        <ext:Parameter Name="msgTarget" Value="side" Mode="Value" />
                                                    </Defaults>
                                                    <Body>
                                                        <ext:FormLayout ID="FormLayout2" runat="server" LabelWidth="65">
                                                            <ext:Anchor>
                                                                    <ext:ComboBox ID="extRISK_TYPESNUMBER" runat="server" FieldLabel="风险类型<font color='red'>*</font>"  ValueField="INFOID" DisplayField="INFONAME" StoreID="bmStore3" AllowBlank="false" BlankText="必填项">
                                                                        <Listeners>
                                                                            <Change Handler=" Coolite.AjaxMethods.GLDXLoad();" />
                                                                        </Listeners>
                                                                    </ext:ComboBox>
                                                                </ext:Anchor>
                                                                <ext:Anchor>
                                                                    <ext:ComboBox ID="extM_PERSONNUMBER" runat="server" FieldLabel="管理人员"  ValueField="M_OBJECTID" DisplayField="NAME" StoreID="Store2" MinChars="1">
                                                                        <Listeners>
                                                                            <Render Fn="function(f) {
                                                                                f.el.on('keyup', function(e) {
                                                                                Coolite.AjaxMethods.PYchange(f.getRawValue(),'Store2');
                                                                                f.setValue(f.getRawValue());
                                                                                });
                                                                                }
                                                                                " />
                                                                        </Listeners>
                                                                    </ext:ComboBox>
                                                                </ext:Anchor>
                                                                <ext:Anchor>
                                                                    <ext:TextArea ID="extH_CONSEQUENCES" runat="server" FieldLabel="后果描述<font color='red'>*</font>" AllowBlank="false" BlankText="必填项">
                                                                    </ext:TextArea>
                                                                </ext:Anchor>
                                                                <ext:Anchor>
                                                                    <ext:TextArea ID="extM_MEASURES" runat="server" FieldLabel="管理措施<font color='red'>*</font>" AllowBlank="false" BlankText="必填项">
                                                                    </ext:TextArea>
                                                                </ext:Anchor>
                                                                <ext:Anchor>
                                                                    <ext:TextArea ID="extPUNISHMENTSTANDARD" runat="server" FieldLabel="处罚标准" Height="23" Regex="^[1-9]\d*$" RegexText="必须为数字">
                                                                    </ext:TextArea>
                                                                </ext:Anchor>
                                                        </ext:FormLayout>
                                                    </Body>
                                                </ext:Panel>
                                            </ext:LayoutColumn>
                                            <ext:LayoutColumn ColumnWidth=".4">
                                                <ext:Panel ID="Panel4" runat="server" Border="false">
                                                    <Defaults>
                                                        <ext:Parameter Name="anchor" Value="95%" Mode="Value" />
                                                        <ext:Parameter Name="msgTarget" Value="side" Mode="Value" />
                                                    </Defaults>
                                                    <Body>
                                                        <ext:FormLayout ID="FormLayout3" runat="server" LabelWidth="65">
                                                            <ext:Anchor Horizontal="95%">
                                                                    <ext:ComboBox ID="extRISK_EVELNUMBER" runat="server" FieldLabel="风险等级" ValueField="INFOID" DisplayField="INFONAME" StoreID="bmStore2">
                                                                    </ext:ComboBox>
                                                                </ext:Anchor>
                                                                <ext:Anchor Horizontal="95%">
                                                                    <ext:ComboBox ID="extDIRECTLYRESPONSIBLEPERSONSNUMB" runat="server" FieldLabel="直接责任人"  ValueField="M_OBJECTID" DisplayField="NAME" StoreID="Store3" MinChars="1">
                                                                        <Listeners>
                                                                            <Render Fn="function(f) {
                                                                                f.el.on('keyup', function(e) {
                                                                                Coolite.AjaxMethods.PYchange(f.getRawValue(),'Store3');
                                                                                f.setValue(f.getRawValue());
                                                                                });
                                                                                }
                                                                                " />
                                                                        </Listeners>
                                                                    </ext:ComboBox>
                                                                </ext:Anchor>
                                                                <ext:Anchor Horizontal="95%">
                                                                    <ext:ComboBox ID="extREGULATORYPARTNERSNUMBER" runat="server" FieldLabel="监督责任人"  ValueField="M_OBJECTID" DisplayField="NAME" StoreID="Store4" MinChars="1">
                                                                        <Listeners>
                                                                            <Render Fn="function(f) {
                                                                                f.el.on('keyup', function(e) {
                                                                                Coolite.AjaxMethods.PYchange(f.getRawValue(),'Store4');
                                                                                f.setValue(f.getRawValue());
                                                                                });
                                                                                }
                                                                                " />
                                                                        </Listeners>
                                                                    </ext:ComboBox>
                                                                </ext:Anchor>
                                                                <ext:Anchor Horizontal="95%">
                                                                    <ext:TextArea ID="extREGULATORYMEASURES" runat="server" FieldLabel="监督措施<font color='red'>*</font>" AllowBlank="false" BlankText="必填项">
                                                                    </ext:TextArea>
                                                                </ext:Anchor>
                                                                <ext:Anchor Horizontal="95%">
                                                                    <ext:TextArea ID="extNOTE" runat="server" FieldLabel="备注">
                                                                    </ext:TextArea>
                                                                </ext:Anchor>
                                                                <ext:Anchor Horizontal="95%">
                                                                    <ext:Hidden ID="Hidden1" runat="server">
                                                                    </ext:Hidden>
                                                                </ext:Anchor>
                                                                <ext:Anchor Horizontal="95%">
                                                                    <ext:Hidden ID="Hidden2" runat="server">
                                                                    </ext:Hidden>
                                                                </ext:Anchor>
                                                        </ext:FormLayout>
                                                    </Body>
                                                </ext:Panel>
                                            </ext:LayoutColumn>
                                        </ext:ColumnLayout>
                                        </Body>
                                    </ext:Panel>
                                </Center>
                            </ext:BorderLayout>
                        </Body>
                    </ext:Panel>
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
        Title="危险源提交"
        Width="100px" Height="400px"
        Modal="true" AutoScroll="true"
        ShowOnLoad="false"
        >
        <Body>
            <ext:BorderLayout runat="server" ID="borderlayoutwindow">
                <Center>
                    <ext:GridPanel 
                        ID="GridPanel2" 
                        runat="server" 
                        StoreID="TJDXStore" 
                        Height="200"
                        StripeRows="true"
                        Header="false"
                        Collapsible="false" AutoScroll="true"
                        >
                        <ColumnModel ID="ColumnModel2" runat="server">
                            <Columns>
                                <ext:Column Header="提交对象" DataIndex="ROLENAME"/>
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:CheckboxSelectionModel ID="CheckboxSelectionModel2" runat="server" SingleSelect="false" />
                        </SelectionModel>
                    </ext:GridPanel>
                </Center>
            </ext:BorderLayout>
        </Body>
        <Buttons>
            <ext:Button ID="Button3" runat="server" Icon="Accept" Text="提交">
                <Listeners>
                    <Click Handler="Coolite.AjaxMethods.TJ();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    <ext:Window 
        ID="SPWindow" 
        runat="server" 
        BodyStyle="padding:5px;" 
        ButtonAlign="Right"
        Frame="true" 
        Title="危险源审批"
        Width="400px" Height="150px"
        Modal="true" AutoScroll="true"
        ShowOnLoad="false"
        >
        <Body>
            <ext:FormLayout ID="FormLayout5" runat="server" LabelWidth="60" LabelSeparator="">
                <ext:Anchor Horizontal="95%">
                    <ext:TextArea ID="taSPYJ" runat="server" FieldLabel="审批意见">
                    </ext:TextArea>
                </ext:Anchor>
             </ext:FormLayout>
        </Body>
        <Buttons>
            <ext:Button ID="Button1" runat="server" Icon="Accept" Text="审批通过">
                <Listeners>
                    <Click Handler="Coolite.AjaxMethods.SP('T');" />
                </Listeners>
            </ext:Button>
            <ext:Button ID="Button2" runat="server" Icon="Cross" Text="审批不通过">
                <Listeners>
                    <Click Handler="Coolite.AjaxMethods.SP('F');" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    </form>
</body>
</html>
