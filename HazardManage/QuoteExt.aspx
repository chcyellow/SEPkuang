<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QuoteExt.aspx.cs" Inherits="HazardManage_QuoteExt" %>

<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript">
        var template = '<span style="color:{0};">{1}</span>';
        var qtip = function(v, p) {//单元格提示
            //v : value , p : cell
            p.attr = 'ext:qtitle="" ext:qtip="' + v + '"';
            return v;
        }
        var header = function(value) {
            var color;
            if (value == '已引用') {
                color = 'red';
            }
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
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN" CleanResourceUrl="false">
    </ext:ScriptManager>
    <ext:Store ID="hazardsStore" runat="server" >
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
    <ext:Store ID="bmStore4" runat="server" >
        <Reader>
            <ext:JsonReader ReaderID="INFOID">
                <Fields>
                    <ext:RecordField Name="INFOID" />
                    <ext:RecordField Name="INFONAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="bmStore5" runat="server" >
        <Reader>
            <ext:JsonReader ReaderID="INFOID">
                <Fields>
                    <ext:RecordField Name="INFOID" />
                    <ext:RecordField Name="INFONAME" />
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
    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                <West Collapsible="true" Split="true">
                    <ext:TreePanel ID="tpZY" runat="server" Width="200" Title="辨识单元" RootVisible="false" AutoScroll="true" >
                        <Listeners>
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
                                        Collapsible="false" Width="490"
                                        >
                                        <ColumnModel ID="ColumnModel1" runat="server">
		                                    <Columns>
                                                <ext:Column Header="是否引用" DataIndex="ISGROUP" Groupable="false" Width="50">
                                                    <Renderer Fn="header" />
                                                </ext:Column>
		                                        <ext:Column Header="危险源" DataIndex="H_CONTENT" Groupable="false" Width="100" />
		                                        <ext:Column Header="风险类型" DataIndex="FXLX" Groupable="false" Width="40" />
                                                <ext:Column Header="风险后果及描述" DataIndex="H_CONSEQUENCES" Groupable="false" Width="200">
                                                    <Renderer Fn="qtip" />
                                                </ext:Column>
                                                <ext:Column Header="事故类型" DataIndex="SGLX" Groupable="false" Width="50" />
                                                <ext:Column Header="状态" DataIndex="ISPASS" Groupable="false" Width="50">
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
                                        <View>
                                            <ext:GridView ForceFit="true" />
                                        </View>
                                        <SelectionModel>
                                            <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" runat="server" SingleSelect="true" />
                                        </SelectionModel>
                                        <AjaxEvents>
                                            <Click OnEvent="RowClick">
                                                <ExtraParams>
                                                    <ext:Parameter Name="Values" Value="Ext.encode(#{GridPanel1}.getRowsValues())" Mode="Raw" />
                                                </ExtraParams>
                                            </Click>
                                        </AjaxEvents>
                                        <BottomBar>
                                            <ext:Toolbar runat="server" ID="tb1">
                                                <Items>
                                                    <ext:Button runat="server" ID="btn_Sure" Icon="Accept" Text="全部引用">
                                                        <Listeners>
                                                            <Click Handler="Coolite.AjaxMethods.btnSure_Click();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:Button runat="server" ID="btn_C" Icon="Cross" Text="全部取消">
                                                        <Listeners>
                                                            <%--<Click Handler="#{Window1}.show();" />--%>
                                                            <Click Handler="Coolite.AjaxMethods.btnC_Click();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:ToolbarSeparator />
                                                    <ext:Button runat="server" ID="btn_yy" Icon="Accept" Text="引用">
                                                        <Listeners>
                                                            <Click Handler="Coolite.AjaxMethods.btnyy_Click();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:Button runat="server" ID="btn_cancel" Icon="Cross" Text="取消引用">
                                                        <Listeners>
                                                            <Click Handler="Coolite.AjaxMethods.btncancel_Click();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:ToolbarSeparator />
                                                    <ext:Button runat="server" ID="btn_Save" Icon="Disk" Text="保存">
                                                        <Listeners>
                                                            <Click Handler="Coolite.AjaxMethods.btnSave_Click();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                        </BottomBar>
                                    </ext:GridPanel>
                                </North>
                                <Center>
                                    <ext:Panel ID="Panel1" runat="server" Height="300" AutoScroll="true">
                                        <Body>
                                            <ext:ColumnLayout ID="ColumnLayout1" runat="server">
                                                <ext:LayoutColumn ColumnWidth=".3">
                                                    <ext:Panel ID="Panel2" runat="server" Border="false" Header="false">
                                                        <Body>
                                                            <ext:FormLayout ID="FormLayout1" runat="server" LabelWidth="75">
                                                                <ext:Anchor Horizontal="95%">
                                                                    <ext:ComboBox ID="extRISK_TYPESNUMBER" runat="server" FieldLabel="风险类型"  ValueField="INFOID" DisplayField="INFONAME" StoreID="bmStore3" AllowBlank="false" BlankText="必填项" Disabled="true">
                                                                    </ext:ComboBox>
                                                                </ext:Anchor>
                                                                <ext:Anchor Horizontal="95%">
                                                                    <ext:ComboBox ID="extM_OBJECTNUMBER" runat="server" FieldLabel="管理对象<font color='red'>*</font>" ValueField="M_OBJECTID" DisplayField="NAME" StoreID="Store1" MinChars="1">
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
                                                                    <ext:ComboBox ID="extM_PERSONNUMBER" runat="server" FieldLabel="管理人员<font color='red'>*</font>"  ValueField="M_OBJECTID" DisplayField="NAME" StoreID="Store2" MinChars="1">
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
                                                                <ext:Anchor Horizontal="95%">
                                                                    <ext:ComboBox ID="extRISK_EVELNUMBER" runat="server" FieldLabel="风险等级<font color='red'>*</font>" ValueField="INFOID" DisplayField="INFONAME" StoreID="bmStore2">
                                                                    </ext:ComboBox>
                                                                </ext:Anchor>
                                                                <ext:Anchor Horizontal="95%">
                                                                    <ext:ComboBox ID="extDIRECTLYRESPONSIBLEPERSONSNUMB" runat="server" FieldLabel="直接责任人<font color='red'>*</font>"  ValueField="M_OBJECTID" DisplayField="NAME" StoreID="Store3" MinChars="1">
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
                                                                    <ext:ComboBox ID="extREGULATORYPARTNERSNUMBER" runat="server" FieldLabel="监督责任人<font color='red'>*</font>"  ValueField="M_OBJECTID" DisplayField="NAME" StoreID="Store4" MinChars="1">
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
                                                                    <ext:ComboBox ID="extPROCESSINGMODE" runat="server" FieldLabel="处理方式" SelectedIndex="2">
                                                                        <Items>
                                                                            <ext:ListItem Text="集团公司处理" Value="集团公司处理" />
                                                                            <ext:ListItem Text="矿领导处理" Value="矿领导处理" />
                                                                            <ext:ListItem Text="安监处处理" Value="安监处处理" />
                                                                            <ext:ListItem Text="科区处理" Value="科区处理" />
                                                                        </Items>
                                                                    </ext:ComboBox>
                                                                </ext:Anchor>
                                                                <ext:Anchor Horizontal="95%">
                                                                    <ext:TextArea ID="extREGULATORYMEASURES" runat="server" FieldLabel="监督措施" ReadOnly="true" Height="90">
                                                                    </ext:TextArea>
                                                                </ext:Anchor>
                                                                <ext:Anchor Horizontal="95%">
                                                                    <ext:TextArea ID="extNOTE" runat="server" FieldLabel="备注">
                                                                    </ext:TextArea>
                                                                </ext:Anchor>
                                                            </ext:FormLayout>
                                                        </Body>
                                                    </ext:Panel>
                                                </ext:LayoutColumn>
                                                <ext:LayoutColumn ColumnWidth=".3">
                                                <ext:Panel ID="Panel3" runat="server" Border="false">
                                                    <Body>
                                                        <ext:FormLayout ID="FormLayout2" runat="server" LabelWidth="65">
                                                                <ext:Anchor Horizontal="95%">
                                                                    <ext:TextArea ID="extM_STANDARDS" runat="server" FieldLabel="管理标准" ReadOnly="true" Height="150">
                                                                    </ext:TextArea>
                                                                </ext:Anchor>
                                                                <ext:Anchor Horizontal="95%">
                                                                    <ext:TextArea ID="extSW_BM" runat="server" FieldLabel="三违描述<font color='red'>*</font>" Height="90">
                                                                    </ext:TextArea>
                                                                </ext:Anchor>
                                                                <ext:Anchor Horizontal="95%">
                                                                    <ext:ComboBox ID="extSW_LEVELID" runat="server" FieldLabel="三违级别<font color='red'>*</font>" ValueField="INFOID" DisplayField="INFONAME" SelectedIndex="0" StoreID="bmStore5">
                                                                        
                                                                    </ext:ComboBox>
                                                                </ext:Anchor>
                                                                <ext:Anchor Horizontal="95%">
                                                                    <ext:TextArea ID="extSW_SCORES" runat="server" FieldLabel="三违积分<font color='red'>*</font>"  Regex="^[1-9]\d*$" RegexText="必须为数字" Height="23">
                                                                    </ext:TextArea>
                                                                </ext:Anchor>
                                                                <ext:Anchor Horizontal="95%">
                                                                    <ext:TextArea ID="extSW_PUNISHMENTSTANDARD" runat="server" FieldLabel="三违处罚标准<font color='red'>*</font>" Regex="^[1-9]\d*$" RegexText="必须为数字" Height="23">
                                                                    </ext:TextArea>
                                                                </ext:Anchor>
                                                        </ext:FormLayout>
                                                    </Body>
                                                </ext:Panel>
                                            </ext:LayoutColumn>
                                            <ext:LayoutColumn ColumnWidth=".3">
                                                <ext:Panel ID="Panel4" runat="server" Border="false">
                                                    <Body>
                                                        <ext:FormLayout ID="FormLayout3" runat="server" LabelWidth="65">
                                                                <ext:Anchor Horizontal="95%">
                                                                    <ext:TextArea ID="extM_MEASURES" runat="server" FieldLabel="管理措施" ReadOnly="true" Height="150">
                                                                    </ext:TextArea>
                                                                </ext:Anchor>
                                                                <ext:Anchor Horizontal="95%">
                                                                    <ext:TextArea ID="extH_BM" runat="server" FieldLabel="隐患描述<font color='red'>*</font>" Height="90">
                                                                    </ext:TextArea>
                                                                </ext:Anchor>
                                                                <ext:Anchor Horizontal="95%">
                                                                    <ext:ComboBox ID="extLEVELID" runat="server" FieldLabel="隐患级别<font color='red'>*</font>" ValueField="INFOID" DisplayField="INFONAME" SelectedIndex="0" StoreID="bmStore4">
                                                                        
                                                                    </ext:ComboBox>
                                                                </ext:Anchor>
                                                                <ext:Anchor Horizontal="95%">
                                                                    <ext:TextArea ID="extSCORES" runat="server" FieldLabel="隐患积分<font color='red'>*</font>"  Regex="^[1-9]\d*$" RegexText="必须为数字" Height="23">
                                                                    </ext:TextArea>
                                                                </ext:Anchor>
                                                                <ext:Anchor Horizontal="95%">
                                                                    <ext:TextArea ID="extPUNISHMENTSTANDARD" runat="server" FieldLabel="隐患处罚标准<font color='red'>*</font>" Regex="^[1-9]\d*$" RegexText="必须为数字" Height="23">
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
    </form>
</body>
</html>
