<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SWLearn.aspx.cs" Inherits="YSNewProcess_SWLearn" %>

<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
        .x-form-group .x-form-group-header-text {
        	background-color: #dfe8f6;
        }
        
        .x-label-text {
            font-weight: bold;
            font-size: 11px;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN" CleanResourceUrl="false" />
    <ext:Store ID="Store1" runat="server" OnRefreshData="MyData_Refresh">
        <Reader>
            <ext:JsonReader ReaderID="Swid">
                <Fields>
                    <ext:RecordField Name="Swid" Type="Int" />
                    <ext:RecordField Name="Kqname" />
                    <ext:RecordField Name="Swperson" />
                    <ext:RecordField Name="Levelname" />
                    <ext:RecordField Name="Stime" Type="Date" DateFormat="Y-m-dTh:i:s" />
                    <ext:RecordField Name="Etime" Type="Date" DateFormat="Y-m-dTh:i:s" />
                    <ext:RecordField Name="Isfinish" Type="Boolean" />
                    <ext:RecordField Name="Result" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    
    <ext:Store ID="SelectedStore" runat="server">
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
    
    <ext:Store ID="LearnStore" runat="server" OnBeforeStoreChanged="SaveD">
        <Reader>
            <ext:JsonReader ReaderID="Lid">
                <Fields>
                    <ext:RecordField Name="Lid"/>
                    <ext:RecordField Name="Lname" />
                    <ext:RecordField Name="Remark" />
                    <ext:RecordField Name="isCheck" Type="Boolean" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    
    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                <West Collapsible="true" Split="true">
                    <ext:GridPanel 
                        runat="server" 
                        ID="gpPer" Title="待进班人员" 
                        AutoExpandColumn="SelectPer" Width="250"
                        StoreID="SelectedStore">
                        <Listeners>
                        </Listeners>
                        <ColumnModel ID="ColumnModel4" runat="server">
                            <Columns>
                                <ext:Column  Header="姓名" DataIndex="Name" Sortable="true" Width="70" />  
                                <ext:Column ColumnID="SelectPer" Header="单位" DataIndex="Deptname" />                   
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:CheckboxSelectionModel runat="server" ID="CheckboxSelectionModel3" />
                        </SelectionModel>  
                        <SaveMask ShowMask="true" />
                        <TopBar>
                            <ext:Toolbar runat="server">
                                <Items>
                                    <ext:Label ID="Label1" runat="server" Text="进班日期：" />
                                    <ext:DateField ID="dfInban" runat="server" />
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <Buttons>
                            <ext:Button ID="btnInban" Text="进班" runat="server" >
                                <Listeners>
                                    <Click Handler="Coolite.AjaxMethods.BanAddPerson();" />
                                </Listeners>
                            </ext:Button>
                            <ext:Button ID="btnoutqueue" Text="移除" runat="server" >
                                <Listeners>
                                    <Click Handler="Coolite.AjaxMethods.isBanoutqueue();" />
                                </Listeners>
                            </ext:Button>
                        </Buttons>
                    </ext:GridPanel>
                </West>
                <Center>
                    <ext:GridPanel 
                        ID="GridPanel1" 
                        runat="server"
                        StoreID="Store1"
                        StripeRows="true"
                        Title="三违学习班人员情况" AutoExpandColumn="detail"
                        Collapsible="false">
                        <ColumnModel ID="ColumnModel1" runat="server">
		                    <Columns>
		                        <ext:Column Header="部门" Width="150" DataIndex="Kqname" />
                                <ext:Column Header="三违人员" Width="70" DataIndex="Swperson" />
                                <ext:Column Header="三违级别" Width="70" Sortable="true" DataIndex="Levelname" >
                                    <Renderer Fn="change" />
                                </ext:Column>
                                <ext:Column Header="进班时间" Width="70" Sortable="true" DataIndex="Stime" >
                                    <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" />
                                </ext:Column>
                                <ext:Column Header="毕业时间" Width="70" Sortable="true" DataIndex="Etime" >
                                    <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" />
                                </ext:Column>
                                <ext:Column ColumnID="detail" Header="考核结果" Width="120" Sortable="false" DataIndex="Result" />
                                <ext:Column Header="是否毕业" Width="75" Sortable="true" DataIndex="Isfinish">
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
                                    <ext:Button runat="server" ID="btn_search" Icon="FolderMagnify" Text="条件查询" Disabled="true" >
                                        <Listeners>
                                            <Click Handler="#{Window1}.show();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server" ID="btn_detail" Icon="FolderFind" Text="查看明细" Disabled="true" >
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.DetailLoad(0);" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server" ID="btnsw" Icon="FolderFind" Text="三违信息" Disabled="true">
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.SWLoad();#{DetailWindow}.show();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:ToolbarSeparator />
                                    <ext:Button runat="server" ID="Button1" Icon="FolderUp" Text="学习进度" Disabled="true" >
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.DetailLoad(1);" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server" ID="btnBY" Icon="Accept" Text="批量毕业" Disabled="true" >
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.DetailLoad(2);" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server" ID="btnOutClass" Icon="FolderError" Text="退班" Disabled="true" >
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.isOutClass();" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <SelectionModel>
                            <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" runat="server" />                   
                        </SelectionModel>
                        <AjaxEvents>
                            <Click OnEvent="RowClick"></Click>
                        </AjaxEvents>
                     </ext:GridPanel>
                </Center>
            </ext:BorderLayout>
        </Body>
    </ext:ViewPort>
    
    <ext:Window 
        ID="ActionWindow" 
        runat="server" 
        BodyStyle="padding:6px;" 
        ButtonAlign="Right"
        Frame="true" 
        Title="学习明细"
        Height="390" Resizable="false"
        Width="600px"
        ShowOnLoad="false" Modal="true"
        >
        <LoadMask Msg="信息正在加载，请稍候...." ShowMask="true" />
        <Body>
            <ext:ContainerLayout ID="ContainerLayout1" runat="server">
                <ext:Panel ID="Panel2" runat="server" AutoHeight="true" Title="学习流程" Width="575" FormGroup="true" Collapsible="false">
                    <Body>
                        <ext:GridPanel 
                            ID="gpDetail" 
                            runat="server" 
                            StoreID="LearnStore"
                            StripeRows="true" Border="false"
                            Header="false" Width="575" Height="190" AutoScroll="true"
                            Collapsible="false" AutoExpandColumn="jcontent" ClicksToEdit="1"
                            >
                            <ColumnModel ID="ColumnModel2" runat="server">
                               <Columns>
                                   <ext:RowNumbererColumn />
                                   <ext:Column ColumnID="jcontent" Header="学习项目" DataIndex="Lname">
                                   </ext:Column>
                                   <ext:Column Header="备注" DataIndex="Remark" Width="200">
                                      <Editor>
                                          <ext:TextArea ID="TextArea1" runat="server">
                                          </ext:TextArea>
                                      </Editor>
                                   </ext:Column>
                                   <ext:CheckColumn Width="40" Header="操作" DataIndex="isCheck" Editable="true" />
                               </Columns>
                           </ColumnModel>
                           <SaveMask ShowMask="true" Msg="正在保存数据，请稍候..." />
                        </ext:GridPanel>
                    </Body>
                </ext:Panel>
                
                <ext:Panel ID="Panel1" runat="server" AutoHeight="true" Title="考核结果" Width="575" FormGroup="true" Collapsible="false">
                    <Body>
                        <ext:TextArea runat="server" ID="tfResult" Height="75" Width="575" />
                    </Body>
                </ext:Panel>
            </ext:ContainerLayout>
        </Body>
        <Buttons>
            <ext:Button runat="server" ID="btnSave" Text="保存" Icon="Disk">
                <Listeners>
                    <Click Handler="#{gpDetail}.save();" />
                </Listeners>
            </ext:Button>
            <ext:Button runat="server" ID="btnSure" Text="毕业" Icon="Table">
                <Listeners>
                    <Click Handler="Coolite.AjaxMethods.isfini();" />
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
            Title="三违明细信息" 
            AutoHeight="true" 
            Width="600px" 
            Modal="true" 
            ShowOnLoad="false" 
            X="100" Y="60">
            <Tools>
                <ext:Tool Type="Refresh" Handler="Coolite.AjaxMethods.SWLoad();" />
            </Tools>
            <LoadMask Msg="信息正在加载，请稍候...." ShowMask="true" />
            <Body>
                <ext:ContainerLayout ID="ContainerLayout2" runat="server">
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
                    ID="CFPanel" 
                    runat="server" 
                    Title="处罚信息" 
                    AutoHeight="true" 
                    FormGroup="true"
                    Collapsed="True">
                    <Body>
                        <table style="width:100%;">
                            <tr>
                                 <td colspan="3">
                                     <ext:Label ID="lbl_fkxx" runat="server" Width="500px" Height="40px" />
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
