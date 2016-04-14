<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditAccess.aspx.cs" Inherits="PAR_EditAccess" %>

<%@ Register assembly="Coolite.Ext.Web" namespace="Coolite.Ext.Web" tagprefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css" >
    .x-grid3-row td,.x-grid3-summary-row td    
    {
        
        border-left:1px solid #eceff6 !important;
     }
    </style>
    <%--line-height:18px;  
        vertical-align:top;
        border-right: 1px solid #eceff6 !important; 
        border-top: 0px solid #eceff6 !important;
        border-right:1px solid #eceff6 !important;--%>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN" CleanResourceUrl="false" />
    <ext:Store ID="JeomStore" runat="server" GroupField="Kind" OnBeforeStoreChanged="SaveHandler" >
        <Reader>
            <ext:JsonReader ReaderID="Jcid">
                <Fields>
                    <ext:RecordField Name="Jcid" />
                    <ext:RecordField Name="Jccontent" />
                    <ext:RecordField Name="Minscore" />
                    <ext:RecordField Name="Maxscore" />
                    <ext:RecordField Name="Jeom" />
                    <ext:RecordField Name="Remark" />
                    <ext:RecordField Name="Kind" />
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
     <ext:Store ID="PersStore" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="Personnumber">
                <Fields>
                    <ext:RecordField Name="Personnumber" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="KHRStore" runat="server" OnRefreshData="KHRRefresh">
        <Reader>
            <ext:JsonReader ReaderID="Personnumber">
                <Fields>
                    <ext:RecordField Name="Personnumber" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    
    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                <Center>
                    <ext:GridPanel 
                        ID="gpEdit" 
                        runat="server" 
                        StoreID="JeomStore"
                        StripeRows="true"
                        Header="false"
                        Icon="BrickMagnify"
                        Collapsible="false" AutoExpandColumn="jcontent"
                        >
                        <ColumnModel ID="ColumnModel1" runat="server">
                            <Columns>
                                <ext:Column ColumnID="jcontent" Header="考核内容" DataIndex="Jccontent" Groupable="false" Width="50" />
                                <ext:Column Header="最小扣分" DataIndex="Minscore" Groupable="false" Width="80" />
                                <ext:Column Header="最大扣分" DataIndex="Maxscore" Groupable="false" Width="80" />
                                <ext:Column Header="扣分分值" DataIndex="Jeom" Width="80">
                                    <Editor>
                                        <ext:NumberField runat="server" ID="nfJeom" />
                                    </Editor>
                                </ext:Column>
                                <ext:Column Header="扣分原因" DataIndex="Remark" Width="120" RightCommandAlign="False">
                                    <Editor>
                                        <ext:TextArea runat="server" ID="tfremark" Height="90" />
                                    </Editor>
                                </ext:Column>
                                <ext:Column Header="考核标准分类" DataIndex="Kind"/>
                            </Columns>
                        </ColumnModel>
                        <TopBar>
                            <ext:Toolbar ID="Toolbar3" runat="server">
                                <Items>
                                    <ext:ComboBox ID="cbbKind" runat="server" Note="<font color='blue'>考核类型</font>" NoteAlign="Top" Width="100px" />
                                    <ext:ToolbarSeparator />
                                    <ext:ComboBox ID="cbbCheckDept" runat="server" DisplayField="Deptname" 
                                        ValueField="Deptnumber"
                                        StoreID="DeptStore" Note="<font color='blue'>考核单位</font>" NoteAlign="Top" Editable="false" Width="100px" Disabled="true">
                                        <Listeners>
                                            <Select Handler="#{cbb_khr}.clearValue(); #{KHRStore}.reload();" />
                                        </Listeners>
                                        </ext:ComboBox>
                                    <ext:ComboBox 
                                        ID="cbb_khr" 
                                        runat="server" Note="<font color='blue'>考核人</font>" NoteAlign="Top"
                                        DisplayField="Name" 
                                        ValueField="Personnumber"
                                        Width="65px"
                                        StoreID="KHRStore"
                                        EmptyText="请选择人员"
                                        Disabled="true" 
                                        >
                                    </ext:ComboBox>
                                    <ext:ComboBox ID="cbbforcheckDept" runat="server"
                                    DisplayField="Deptname" 
                                    ValueField="Deptnumber"
                                    Width="100px"
                                    StoreID="DeptStore" Note="<font color='blue'>被考核单位</font>" NoteAlign="Top" Editable="false">
                                        <Listeners>
                                            <Select Handler="#{fb_zrr}.clearValue();" />
                                        </Listeners>
                                    </ext:ComboBox>
                                    <ext:ComboBox 
                                            ID="fb_zrr" 
                                            runat="server" Note="<font color='blue'>被考核人</font>" NoteAlign="Top"
                                            DisplayField="Name" 
                                            ValueField="Personnumber"
                                            Width="65px"
                                            StoreID="PersStore"
                                            EmptyText="拼音检索"
                                            HideTrigger="true"
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
                                    <ext:DateField ID="dfDate" runat="server" Note="<font color='blue'>考核时间</font>" NoteAlign="Top" Width="100px" Disabled="true" />
                                    <ext:ComboBox ID="cbbplace" runat="server"
                                    StoreID="placeStore"
                                    DisplayField="placName" HideTrigger="true" 
                                    ValueField="placID" Note="<font color='blue'>考核地点</font>" NoteAlign="Top">
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
                                    <ext:ComboBox ID="cbbBanci" runat="server" Note="<font color='blue'>考核班次</font>" NoteAlign="Top" SelectedIndex="0" Editable="false" Width="55">
                                        <Items>
                                            <ext:ListItem Text="夜班" Value="夜班" />
                                            <ext:ListItem Text="早班" Value="早班" />
                                            <ext:ListItem Text="中班" Value="中班" />
                                        </Items>
                                    </ext:ComboBox>
                                    <ext:ToolbarSeparator />
                                    <ext:ToolbarButton ID="btnSave" runat="server" Icon="Disk" Text="保存">
                                        <Listeners>
                                            <Click Handler="#{gpEdit}.save();" />
                                        </Listeners>
                                    </ext:ToolbarButton>
                                </Items>
                            </ext:Toolbar>
                          </TopBar>
                        <View><%--StartCollapsed="true"--%>
                            <ext:GroupingView  
                                ID="GroupingView1"
                                Collapsible="false"
                                HideGroupedColumn="true"
                                runat="server" 
                                ShowGroupName="false"
                                GroupTextTpl='{text} ({[values.rs.length]} {["条"]})'
                                EnableRowBody="true">
                            </ext:GroupingView>

                        </View>
                        <SelectionModel>
                            <ext:CellSelectionModel ID="CellSelectionModel1" runat="server">
                                <AjaxEvents>
                                    <CellSelect OnEvent="Cell_Click" />                        
                                </AjaxEvents>
                            </ext:CellSelectionModel>
                        </SelectionModel>
                        <SaveMask ShowMask="true" Msg="正在保存数据，请稍候..." />
                    </ext:GridPanel>
                </Center>
            </ext:BorderLayout>
        </Body>
    </ext:ViewPort>
    </form>
</body>
</html>
