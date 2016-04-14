<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Yh2Haz.aspx.cs" Inherits="YSHMamage_Yh2Haz" %>

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
            var color; var valu;
            if (value == '1') {
                color = 'red'; valu = '新增';
            }
            else if (value == '2') {
                color = 'orange'; valu = '发布';
            }
            else if (value == '3') {
                color = 'gray'; valu = '作废';
            }
            return String.format(template, color, valu);
        }

        var changecol = function(value) {
            var color;
            if (value == '否') 
                color = 'red';
            else
                color = 'orange';
            return String.format(template, color, value);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN" CleanResourceUrl="false" />
    
    <ext:Store ID="YHStore" runat="server" >
        <Reader>
            <ext:JsonReader ReaderID="Yhid">
                <Fields>
                    <ext:RecordField Name="Yhid" Type="Int" />
                    <ext:RecordField Name="Yhnumber" />
                    <ext:RecordField Name="Yhcontent" />
                    <ext:RecordField Name="Levelid" />
                    <ext:RecordField Name="Levelname" />
                    <ext:RecordField Name="Typeid" />
                    <ext:RecordField Name="Typename" />
                    <ext:RecordField Name="Conpyfirst" />
                    <ext:RecordField Name="Intime" Type="Date" DateFormat="Y-m-dTh:i:s" />
                    <ext:RecordField Name="Nstatus" />
                    <ext:RecordField Name="y2h" />
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
    
    <ext:Store ID="y2hStore" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="Hazardsid">
                <Fields>
                    <ext:RecordField Name="HNumber" />
                    <ext:RecordField Name="Zyid" />
                    <ext:RecordField Name="Zyname" />
                    <ext:RecordField Name="Gzrwname" />
                    <ext:RecordField Name="Gxname" />
                    <ext:RecordField Name="Fxlxid" />
                    <ext:RecordField Name="Fxlx" />
                    <ext:RecordField Name="HContent" />
                    <ext:RecordField Name="HConsequences" />
                    <ext:RecordField Name="MStandards" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    
    <ext:Store ID="hazStore" runat="server" OnRefreshData="hazStoreRefresh">
        <Reader>
            <ext:JsonReader ReaderID="Hazardsid">
                <Fields>
                    <ext:RecordField Name="HNumber" />
                    <ext:RecordField Name="Zyid" />
                    <ext:RecordField Name="Zyname" />
                    <ext:RecordField Name="Gzrwname" />
                    <ext:RecordField Name="Gxname" />
                    <ext:RecordField Name="Fxlxid" />
                    <ext:RecordField Name="Fxlx" />
                    <ext:RecordField Name="HContent" />
                    <ext:RecordField Name="HConsequences" />
                    <ext:RecordField Name="MStandards" />
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
    
     <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                <Center>
                    <ext:GridPanel 
                        ID="GridPanel3" 
                        runat="server"
                        StoreID="YHStore"
                        StripeRows="true"
                        Collapsible="false"
                        AutoExpandColumn="detail"
                        Title="隐患与危险源对应信息"
                        >
                        <TopBar>
                            <ext:Toolbar ID="Toolbar2" runat="server">
                                <Items>
                                    <ext:Button runat="server" ID="btn_search" Icon="Zoom" Text="条件查询" >
                                        <Listeners>
                                            <Click Handler="#{Window1}.show();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:ToolbarSeparator />
                                    <ext:Button runat="server" ID="btnAction" Icon="FolderEdit" Text="与危险源对应管理" Disabled="true" >
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.hazLoad();" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <ColumnModel ID="ColumnModel1" runat="server">
	                        <Columns>
	                            <ext:Column Header="编号" Width="100" Sortable="true" DataIndex="Yhnumber" />
                                <ext:Column ColumnID="detail" Header="描述" Width="170" Sortable="true" DataIndex="Yhcontent">
                                    <Renderer Fn="qtip" />
                                </ext:Column>
                                <ext:Column Header="级别" Width="100" Sortable="true" DataIndex="Levelname" />
                                <ext:Column Header="专业" Width="100" Sortable="true" DataIndex="Typename" />
                                <ext:Column Header="入库时间" Width="80" Sortable="true" DataIndex="Intime" >
                                    <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" />
                                </ext:Column>
                                <ext:Column Header="当前状态" Width="60" Sortable="true" DataIndex="Nstatus">
                                    <Renderer Fn="change" />
                                </ext:Column>
                                <ext:Column Header="与危险源对应" Width="100" Sortable="true" DataIndex="y2h">
                                    <Renderer Fn="changecol" />
                                </ext:Column>
	                        </Columns>
                        </ColumnModel>
                        <LoadMask ShowMask="true" />
                        <BottomBar>
                            <ext:PagingToolBar ID="PagingToolBar1" runat="server" PageSize="20" />
                        </BottomBar>
                        <SelectionModel>
                             <ext:RowSelectionModel ID="RowSelectionModel3" runat="server" SingleSelect="true">
                                <Listeners>
                                    <RowSelect Handler="Coolite.AjaxMethods.RowClick();" />
                                </Listeners>
                             </ext:RowSelectionModel>         
                        </SelectionModel>
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
            <ext:FormPanel 
                    ID="FormPanel2" 
                    runat="server"
                    BodyStyle="padding:1px;"
                    BodyBorder="false"
                    Header="false">
                    <Body>
            <ext:FormLayout ID="FormLayout2" runat="server" LabelWidth="60">
                <ext:Anchor Horizontal="95%">
                    <ext:MultiField ID="mf_datecheck" runat="server" FieldLabel="入库时间">
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
                    <ext:ComboBox 
                        ID="cbb_lavel" 
                        runat="server"  FieldLabel="级别" 
                        EmptyText="请选择级别.."
                        DisplayField="YHLevel" 
                        ValueField="YHLevelID" 
                        StoreID="LevelStore"
                        Mode="Local"
                        Width="100"
                        Editable="false"
                        >
                    </ext:ComboBox>
                 </ext:Anchor>
                <ext:Anchor Horizontal="95%">
                    <ext:ComboBox 
                        ID="cbb_kind"  FieldLabel="专业"
                        runat="server"  
                        EmptyText="请选择专业.."
                        DisplayField="YHType" 
                        ValueField="YHTypeID" 
                        StoreID="TypeStore"
                        Editable="false"
                        >
                    </ext:ComboBox>
                </ext:Anchor>
                <ext:Anchor Horizontal="95%">
                    <ext:TextField ID="tf_Yhconcent" runat="server" FieldLabel="隐患内容" EmptyText="请输入关键字">
                    </ext:TextField>
                </ext:Anchor>
            </ext:FormLayout>
            </Body>
            </ext:FormPanel>
        </Body>
        <Buttons>
            <ext:Button ID="btn_Clear" runat="server" Icon="Cancel" Text="清除条件">
                <Listeners>
                    <Click Handler="#{FormPanel2}.getForm().reset();" />
                </Listeners>
            </ext:Button>
            <ext:Button ID="btnDoSearch" runat="server" Icon="Zoom" Text="查 询">
                <Listeners>
                    <Click Handler="Coolite.AjaxMethods.Search();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    
    <ext:Window 
        ID="hazDetailWindow" 
        runat="server" 
        BodyStyle="padding:5px;" 
        ButtonAlign="Right"
        Frame="true" 
        Title="对应危险源信息"
        Height="400px"
        Width="600px"
        Modal="true" Icon="Zoom" Resizable="false"
        ShowOnLoad="false"
        >
        <Body>
            <ext:BorderLayout runat="server" ID="hazDetail">
                <Center>
                    <ext:GridPanel ID="gpSearchBL" runat="server" StoreID="y2hStore">
                        <TopBar>
                            <ext:Toolbar ID="Toolbar1" runat="server">
                                <Items>
                                    <ext:Button runat="server" ID="btnAdd" Icon="Add" Text="添加">
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.ClearBL();#{hazWindow}.show();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:ToolbarSeparator />
                                    <ext:Button runat="server" ID="btnDel" Icon="Delete" Text="删除">
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.hazDel();" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
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
                                <ext:Column Header="内容" Width="70" DataIndex="HContent" />
                                <ext:Column Header="风险类型" Width="70" DataIndex="Fxlx" />
                                <ext:Column Header="管理措施" Width="150" Sortable="true" DataIndex="HConsequences">
                                    <Renderer Fn="qtip" />
                                </ext:Column>
                                <ext:Column Header="管理标准" Width="150" Sortable="true" DataIndex="MStandards">
                                    <Renderer Fn="qtip" />
                                </ext:Column>
		                    </Columns>
                        </ColumnModel>
                        <LoadMask Msg="数据加载中，请稍候..." ShowMask="true" />
                        <SelectionModel>
                            <ext:CheckboxSelectionModel runat="server" ID="CheckboxSelectionModel1" SingleSelect="false" />
                        </SelectionModel>
                    </ext:GridPanel>
                </Center>
            </ext:BorderLayout>
        </Body>
    </ext:Window>
    
    <ext:Window 
        ID="hazWindow" 
        runat="server" 
        BodyStyle="padding:5px;" 
        ButtonAlign="Right"
        Frame="true" 
        Title="危险源信息"
        Height="400px"
        Width="600px"
        Modal="true" Resizable="false"
        ShowOnLoad="false"
        >
        <Body>
            <ext:BorderLayout runat="server" ID="BorderLayout2">
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
                                            DisplayField="YHType" 
                                            ValueField="YHTypeID" 
                                            StoreID="TypeStore"
                                            Disabled="true">
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
                                            HideTrigger="true">
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
                                        </ext:ComboBox>
                                    </td>
                                </tr>
                                <tr>

                                    <td>
                                        危险源内容：
                                    </td>
                                    <td colspan="5">
                                        <ext:TextField ID="tfSyhms" runat="server" Width="320" EmptyText="请输入危险源关键字">
                                        </ext:TextField>
                                    </td>
                                </tr>
                            </table>
                        </Body>
                        <Buttons>
                            <ext:Button runat="server" ID="btnSearchBL" Text="查询" Icon="Zoom">
                                <Listeners>
                                    <%--<Click Handler="Coolite.AjaxMethods.SearchBLLoad();" />--%>
                                    <Click Handler="#{hazStore}.reload();" />
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
                    <ext:GridPanel ID="GridPanel1" runat="server" StoreID="hazStore">
                        <ColumnModel ID="ColumnModel3" runat="server">
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
                                <ext:Column Header="内容" Width="150" Sortable="true" DataIndex="HContent">
                                    <Renderer Fn="qtip" />
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
                        <SelectionModel>
                            <ext:CheckboxSelectionModel runat="server" ID="CheckboxSelectionModel2" SingleSelect="false" />
                        </SelectionModel>
                        <Buttons>
                            <ext:Button runat="server" ID="btnSure" Icon="Disk" Text="确认">
                                <Listeners>
                                    <Click Handler="Coolite.AjaxMethods.AddHaz();" />
                                </Listeners>
                            </ext:Button>
                        </Buttons>
                    </ext:GridPanel>
                </Center>
            </ext:BorderLayout>
        </Body>
    </ext:Window>
    </form>
</body>
</html>