<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SWTreatment.aspx.cs" Inherits="ThreeViolate_SWTreatment" %>

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

        var SWchange = function(value) {
            var color;
            if (!value)
                color = '#cc0000';
            else
                color = 'black';
            return String.format(template, color, value?'是':'否');
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
        <ext:Store ID="Store1" runat="server" OnRefreshData="MyData_Refresh">
            <Reader>
                <ext:JsonReader ReaderID="Id">
                    <Fields>
                        <ext:RecordField Name="Id" Type="Int" />
                        <ext:RecordField Name="Swperson" />
                        <ext:RecordField Name="Kqname" />
                        <ext:RecordField Name="Swcontent" />
                        <ext:RecordField Name="Levelname" />
                        <ext:RecordField Name="Placename" />
                        <ext:RecordField Name="Banci" />
                        <ext:RecordField Name="Pcname" />
                        <ext:RecordField Name="Pctime" Type="Date" DateFormat="Y-m-dTh:i:s" />
                        <ext:RecordField Name="Ispublic" Type="Boolean" />
                        <ext:RecordField Name="Isend" Type="Boolean" />
                        <ext:RecordField Name="Remarks" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="DeptStore" runat="server">
            <Reader>
                <ext:JsonReader ReaderID="Deptnumber">
                    <Fields>
                        <ext:RecordField Name="Deptnumber" />
                        <ext:RecordField Name="Deptname" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="LevelStore" runat="server">
            <Reader>
                <ext:JsonReader ReaderID="SWLevelID">
                    <Fields>
                        <ext:RecordField Name="SWLevelID" />
                        <ext:RecordField Name="SWLevel" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:BorderLayout ID="BorderLayout1" runat="server">
                    <Center>
                        <ext:GridPanel 
            ID="GridPanel1" 
            runat="server"
            StoreID="Store1"
            StripeRows="true"
            Title="三违信息"
            AutoExpandColumn="NIid" 
            Collapsible="false"
            Width="840px">
            <ColumnModel ID="ColumnModel1" runat="server">
		        <Columns>
                    <ext:Column Header="三违编号" Width="80" DataIndex="Id" />
                    <ext:Column Header="三违人员" Width="70" DataIndex="Swperson" />
                    <ext:Column Header="部门" Width="70" DataIndex="Kqname" />
                    <ext:Column ColumnID="NIid" Header="三违内容" Width="120" Sortable="false" DataIndex="Swcontent">
                        <Renderer Fn="qtip" />
                    </ext:Column>
                    <ext:Column Header="三违级别" Width="70" Sortable="true" DataIndex="Levelname" >
                    <Renderer Fn="change" />
                    </ext:Column>
                    <ext:Column Header="三违地点" Width="70" DataIndex="Placename" />
                    <ext:Column Header="发生班次" Width="70" DataIndex="Banci" />
                    <ext:Column Header="排查人员" Width="70" DataIndex="Pcname" />
                    <ext:Column Header="排查时间" Width="70" Sortable="true" DataIndex="Pctime" >
                        <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" />
                    </ext:Column>
                    <ext:Column Header="备注" Width="120" Sortable="false" DataIndex="Remarks">
                        <Renderer Fn="qtip" />
                    </ext:Column>
                    <%--<ext:Column Header="是否公布" Width="75" Sortable="true" DataIndex="IsPublic">
                        <Renderer Handler="return value ? '是':'否';" />
                    </ext:Column>--%>
                    <ext:Column Header="是否闭合" Width="75" Sortable="true" DataIndex="Isend">
                        <Renderer Fn="SWchange" />
                    </ext:Column>                
		        </Columns>
            </ColumnModel>
            <LoadMask ShowMask="true" />
            <BottomBar>
                <ext:PagingToolBar ID="PagingToolBar1" runat="server" PageSize="15" />
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
                        <ext:Button runat="server" ID="Button1" Icon="FolderUp" Text="信息处理" >
                            <Listeners>
                                <Click Handler="Coolite.AjaxMethods.DetailLoad_cl();#{Window2}.show();" />
                            </Listeners>
                        </ext:Button>
                        <ext:ToolbarFill />
                        <ext:Button runat="server" ID="Button2" Icon="FolderDelete" Text="作废">
                            <Listeners>
                                <Click Handler="Coolite.AjaxMethods.delshow();" />
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
                        <ext:MultiField ID="MultiField2" runat="server" FieldLabel="三违部门">
                            <Fields>
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
                        <ext:MultiField ID="MultiField3" runat="server" FieldLabel="三违级别">
                            <Fields>
                                <ext:ComboBox 
                                    ID="cbb_lavel" 
                                    runat="server"  
                                    EmptyText="请选择级别.."
                                    DisplayField="SWLevel" 
                                    ValueField="SWLevelID" 
                                    StoreID="LevelStore"
                                    Mode="Local"
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
        <%--明细窗口--%>
        <ext:Window 
            ID="DetailWindow" 
            runat="server" 
            BodyStyle="padding:6px;" 
            ButtonAlign="Right" 
            Frame="true" 
            Title="三违明细信息" 
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
                        Title="三违基本信息" 
                        FormGroup="true" 
                        Width="580px"
                        >
                        <Body>
                            <table style="width:100%;">
                                <tr>
                                    <td style="width:33%;">
                                        <span class="x-label-text">三违编号:</span>
                                        <ext:TextField ID="lbl_SWPutinID" runat="server" Cls="x-textfeild-style" />
                                    </td>
                                    <td style="width:34%;">
                                        <span class="x-label-text">三违人员:</span>
                                        <ext:TextField ID="lbl_Name" runat="server" Cls="x-textfeild-style" />
                                    </td>
                                    <td style="width:34%;">
                                        <span class="x-label-text">三违部门:</span>
                                        <ext:TextField ID="lbl_DeptName" runat="server" Cls="x-textfeild-style" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:33%;">
                                        <span class="x-label-text">三违地点:</span>
                                        <ext:TextField ID="lbl_PlaceName" runat="server" Cls="x-textfeild-style" />
                                    </td>
                                    <td style="width:34%;">
                                        <span class="x-label-text">排查人员:</span>
                                        <ext:TextField ID="lbl_rName" runat="server" Cls="x-textfeild-style" />
                                    </td>
                                    <td style="width:34%;">
                                        <span class="x-label-text">排查时间:</span>
                                        <ext:TextField ID="lbl_PCTime" runat="server" Cls="x-textfeild-style" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:33%;">
                                        <span class="x-label-text">发生班次:</span>
                                        <ext:TextField ID="lbl_BanCi" runat="server" Cls="x-textfeild-style" />
                                    </td>
                                    <td style="width:34%;">
                                        <span class="x-label-text">三违级别:</span>
                                        <ext:TextField ID="lbl_SWLevel" runat="server" Cls="x-textfeild-style" />
                                    </td>
                                    <td style="width:34%;">
                                        <span class="x-label-text">是否闭合:</span>
                                        <ext:TextField ID="lbl_IsEnd" runat="server" Cls="x-textfeild-style" />
                                    </td>
                                </tr>
                                <tr>
                                     <td colspan="3">
                                        <span class="x-label-text">三违内容:</span>
                                        <ext:TextArea ID="lbl_SWContent" runat="server" Width="500px" Height="40px" />
                                    </td>
                                </tr> 
                                <tr>
                                     <td colspan="3">
                                        <span class="x-label-text">备注信息:</span>
                                        <ext:TextArea ID="lbldRemarks" runat="server" Width="500px" Height="40px" />
                                    </td>
                                </tr> 
                            </table>
                        </Body>
                    </ext:Panel>
                    <ext:Panel 
                        ID="CLPanel" 
                        runat="server"
                        Title="三违处理信息" 
                        FormGroup="true"
                        Width="580px" 
                        Collapsed="true"
                        >
                        <Body>
                            <table style="width:100%;">
                                <tr>
                                    <td style="width:45%;">
                                        <span class="x-label-text">该三违是否罚款:</span>
                                        <ext:TextField ID="lbl_Isfak" runat="server" Cls="x-textfeild-style" />
                                    </td>
                                    <td style="width:45%;">
                                        <span class="x-label-text">该三违罚款数为:</span>
                                        <ext:TextField ID="lbl_SWFine" runat="server" Cls="x-textfeild-style" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:45%;">
                                        <span class="x-label-text">该三违应罚积分:</span>
                                        <ext:TextField ID="lbl_SWPoints" runat="server" Cls="x-textfeild-style" />
                                    </td>
                                    <td style="width:45%;">
                                        <span class="x-label-text">是否已行政处罚:</span>
                                        <ext:TextField ID="lbl_IsAp" runat="server" Cls="x-textfeild-style" />
                                    </td>
                                 </tr>
                                 <tr>
                                    <td colspan="3">
                                        <span class="x-label-text">该三违是否公布:</span>
                                        <ext:TextField ID="lbl_Isamp" runat="server" Cls="x-textfeild-style" />
                                    </td>
                                </tr> 
                            </table>
                        </Body>
                    </ext:Panel>
                </ext:ContainerLayout>
            </Body>
        </ext:Window>
        <ext:Window 
            ID="Window2" 
            runat="server" 
            BodyStyle="padding:6px;" 
            ButtonAlign="Right"
            Frame="true" 
            Title="三违信息处理"
            AutoHeight="true"
            Width="600px"
            Modal="true"
            ShowOnLoad="false"
            X="100" Y="60" 
            >
            <Tools>
                <ext:Tool Type="Refresh" Handler="Coolite.AjaxMethods.DetailLoad_cl();" />
            </Tools>
            <LoadMask Msg="信息正在加载，请稍候...." ShowMask="true" />
            <Body>
            <ext:ContainerLayout ID="ContainerLayout2" runat="server">
                    <ext:Panel 
                        ID="fkPanel" 
                        runat="server" 
                        FormGroup="true"  
                        Title="三违罚款"
                        Width="580px">
                        <Body>
                            <table style="width:100%;">
                                <tr>
                                    <td style="width:20%;">
                                        <span class="x-label-text">是否罚款:</span>
                                    </td>
                                    <td style="width:30%;">
                                        <ext:RadioGroup ID="rg_model" runat="server" FieldLabel="是否罚款" Width="100px">
                                            <Items>
                                                <ext:Radio runat="server" ID="fb_YES" Checked="True" BoxLabel="是" AutoWidth="true" Width="20px">
                                                </ext:Radio>
                                                <ext:Radio runat="server" ID="fb_NO" BoxLabel="否" AutoWidth="true" Width="20px">
                                                </ext:Radio>
                                            </Items>
                                            <Listeners>
                                                <Change Handler="Coolite.AjaxMethods.DetailLoad_cl();" />
                                            </Listeners>
                                        </ext:RadioGroup>
                                    </td>
                                    <td style="width:50%;">
                                        <span class="x-label-text">罚款数:</span>
                                        <ext:TextField ID="lbl_Fine" runat="server" Cls="x-textfeild-style" />
                                    </td>
                                </tr>
                            </table>
                        </body>
                    </ext:Panel>
                    <ext:Panel 
                        ID="czcfPanel" 
                        runat="server" 
                        Title="行政处罚"
                        FormGroup="true" 
                        Width="580px">
                        <Body>
                            <table style="width:100%;">
                                <tr>
                                    <td style="width:20%;">
                                        <span class="x-label-text">是否行政处罚:</span>
                                    </td>
                                    <td style="width:30%;">
                                        <ext:RadioGroup ID="RadioGroup2" runat="server" FieldLabel="是否行政处罚" Width="100px">
                                            <Items>
                                                <ext:Radio runat="server" ID="Radio3" BoxLabel="是" AutoWidth="true" Width="20px">
                                                </ext:Radio>
                                                <ext:Radio runat="server" ID="Radio4" Checked="True" BoxLabel="否" AutoWidth="true" Width="20px">
                                                </ext:Radio>
                                            </Items>
                                        </ext:RadioGroup>
                                    </td>
                                    <td style="width:50%;"></td>
                                </tr>
                            </table>
                        </body>
                    </ext:Panel>
                    <ext:Panel 
                        ID="gbPanel" 
                        runat="server" 
                        Title="信息公布"
                        FormGroup="true" 
                        Width="580px">
                        <Body>
                            <table style="width:100%;">
                                <tr>
                                    <td style="width:20%;">
                                        <span class="x-label-text">信息是否公布:</span>
                                    </td>
                                    <td style="width:30%;">
                                        <ext:RadioGroup ID="RadioGroup1" runat="server" FieldLabel="是否行政处罚" Width="100px">
                                            <Items>
                                                <ext:Radio runat="server" ID="Radio1" BoxLabel="是" AutoWidth="true" Width="20px">
                                                </ext:Radio>
                                                <ext:Radio runat="server" ID="Radio2" Checked="True" BoxLabel="否" AutoWidth="true" Width="20px">
                                                </ext:Radio>
                                            </Items>
                                        </ext:RadioGroup>
                                    </td>
                                    <td style="width:50%;"></td>
                                </tr>
                            </table>
                        </body>
                    </ext:Panel>
                    <ext:Panel 
                        ID="xxPanel" 
                        runat="server" 
                        Title="短信提醒"
                        FormGroup="true" 
                        Width="580px">
                        <Body>
                            <table style="width:100%;">
                                <tr>
                                    <td colspan="2" style="width:20%;">
                                        <ext:Checkbox runat="server" ID="ckb_IsSms" Checked="false" BoxLabel="短信提醒">
                                            <Listeners>
                                                <Check Handler="Coolite.AjaxMethods.IsSms();" />
                                            </Listeners>
                                        </ext:Checkbox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:20%;">
                                        <ext:Label runat="server" ID="lbl_sms" Text="短信发送至:" />
                                    </td>
                                    <td style="width:80%;">
                                        <ext:TextField ID="tf_name" runat="server" Disabled="true" Cls="x-textfeild-style" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:20%;">
                                        <ext:Label runat="server" ID="lbl_smsText" Text="编辑短信内容:" />
                                    </td>
                                    <td style="width:80%;">
                                        <ext:TextArea ID="ta_smsText" runat="server" Height="80px" Width="200px" EmptyText="若没有填写具体内容,系统将默认发送信息!" Disabled="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:20%;">
                                        <ext:Button ID="btn_sure" runat="server" Text="确 认">
                                            <Listeners>
                                                <Click Handler="Coolite.AjaxMethods.SWpublish();" />
                                            </Listeners>
                                        </ext:Button>
                                    </td>
                                    <td style="width:80%;"></td>
                                </tr>
                            </table>
                        </body>
                    </ext:Panel>
                </ext:ContainerLayout>           
            </Body> 
        </ext:Window>
        </div>
    </form>
</body>
</html>
