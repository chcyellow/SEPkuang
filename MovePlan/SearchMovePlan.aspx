<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SearchMovePlan.aspx.cs" Inherits="SearchMovePlan" %>
<%@ Register assembly="Coolite.Ext.Web" namespace="Coolite.Ext.Web" tagprefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
        
        .ext-ie .x-form-text { position: static !important;}
    </style>
    <script language="javascript" type="text/javascript">
        var saveData = function () {
            GridData.setValue(Ext.encode(GridPanel2.getRowsValues(false)));
          
        } 
        var saveDataPL = function () {
        GridDataPL.setValue(Ext.encode(GridPanel1.getRowsValues(false)));
        } 
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style=" text-align:center">
        <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN">
        </ext:ScriptManager>
        <ext:Hidden ID="GridData" runat="server" />
        <ext:Hidden ID="GridDataPL" runat="server" />
        <ext:Store ID="Store3" runat="server" AutoLoad="false">
            <Proxy>
                <ext:HttpProxy Method="POST" Url="../CallsProcess/SearchPern.ashx" />
            </Proxy>
            <Reader>
                <ext:JsonReader Root="plants" TotalProperty="totalCount" >
                    <Fields>
                        <ext:RecordField Name="pernID" />
                        <ext:RecordField Name="pernName" />
                        <ext:RecordField Name="persNnmber" />
                        <ext:RecordField Name="pernLightNumber" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="Store1" runat="server" >
        <Reader>
            <ext:JsonReader ReaderID="ID">
                <Fields>
                    <ext:RecordField Name="ID" />
                    <ext:RecordField Name="PersonID" Type="Int" />
                    <ext:RecordField Name="Name" />
                    <ext:RecordField Name="DeptName" />
                    <ext:RecordField Name="PlaceName" />
                    <ext:RecordField Name="StartTime" Type="Date" DateFormat="Y-m-dTh:i:s" />
                    <ext:RecordField Name="EndTime" Type="Date" DateFormat="Y-m-dTh:i:s" />
                    <ext:RecordField Name="MoveStartTime" Type="Date" DateFormat="Y-m-dTh:i:s" />
                    <ext:RecordField Name="MoveEndTime" Type="Date" DateFormat="Y-m-dTh:i:s" />                                 
                    <ext:RecordField Name="MoveState" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store2" runat="server">
        <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="DeptName"/> 
                        <ext:RecordField Name="YHContent" />
                        <ext:RecordField Name="PlaceName" />
                        <ext:RecordField Name="YHLevel" />
                        <ext:RecordField Name="YHType" />
                        <ext:RecordField Name="Name" />
                        <ext:RecordField Name="BanCi" />                       
                        <ext:RecordField Name="PCTime" Type="Date" DateFormat="Y-m-dTh:i:s" />   
                      </Fields> 
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <table>
        <tr>
        
        
        <td>
        <ext:Button ID="Button5" runat="server" Text="查看"  >
                    <AjaxEvents>
                        <Click OnEvent="Button4_Click"></Click>
                    </AjaxEvents>
                </ext:Button>
        </td>
        </tr>
        </table>
        
    <ext:GridPanel 
        ID="GridPanel1" 
        runat="server"
        StoreID="Store1"
        StripeRows="true"
        Title="走动计划"
        Collapsible="false"
        Width="745px"
        AutoHeight="true">
        <ColumnModel ID="ColumnModel1" runat="server">
		    <Columns>
		        <ext:Column ColumnID="ID" Header="编号" Width="70" DataIndex="PersonID" >
                </ext:Column>
                <ext:Column Header="走动人员" Width="80" Sortable="true" DataIndex="Name" >
                </ext:Column>
                <ext:Column Header="部门" Width="100" Sortable="true" DataIndex="DeptName" >
                </ext:Column>
                <ext:Column Header="走动地点" Width="100" Sortable="true" DataIndex="PlaceName" >
                </ext:Column>
                <ext:Column Header="计划开始" Width="80" Sortable="true" DataIndex="StartTime" >
                <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" /></ext:Column>
                <ext:Column Header="计划截止" Width="80" Sortable="true" DataIndex="EndTime" >
                <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" /></ext:Column>
                <ext:Column Header="走动开始" Width="80" Sortable="true" DataIndex="MoveStartTime" >
                <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d h:i')" /></ext:Column>
                <ext:Column Header="走动结束" Width="80" Sortable="true" DataIndex="MoveEndTime" >
                <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d h:i')" /></ext:Column>
                <ext:Column Header="走动状态" Width="185" Sortable="true" DataIndex="MoveState" >
                </ext:Column>
		    </Columns>
        </ColumnModel>
        <Plugins>
            <ext:GridFilters runat="server" ID="GridFilters1" Local="true">
                 <Filters>
                       <ext:StringFilter DataIndex="PersonID" />
                       <ext:StringFilter DataIndex="Name" />
                       <ext:StringFilter DataIndex="DeptName" />
                       
                       <ext:StringFilter DataIndex="PlaceName" />
                       <ext:DateFilter DataIndex="StartTime">
                            <DatePickerOptions runat="server" TodayText="Now" />
                       </ext:DateFilter>
                       <ext:DateFilter DataIndex="EndTime">
                            <DatePickerOptions runat="server" TodayText="Now" />
                       </ext:DateFilter>
                       <ext:ListFilter DataIndex="MoveState" Options="未走动,走动中,已走动" />
                 </Filters>
             </ext:GridFilters>
        </Plugins>
        <LoadMask ShowMask="true" Msg="数据加载中..." />
        <BottomBar>
            <ext:PagingToolBar ID="PagingToolBar1" runat="server" PageSize="15" />
         </BottomBar>
        
        <SelectionModel>
                <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" SingleSelect="true" runat="server" />                   
        </SelectionModel>
        <View>
            <ext:GridView ForceFit="true" />
        </View>
        <AjaxEvents>
            <Click OnEvent="RowClick"></Click>
        </AjaxEvents>
       <TopBar>
            <ext:Toolbar runat="server" ID="tb1">
                <Items>
                <ext:ToolbarSeparator />
               <%-- <ext:Button ID="Button1" runat="server" Text="走动开始"  >
                    <AjaxEvents>
                        <Click OnEvent="Button1_Click"></Click>
                    </AjaxEvents>
                </ext:Button>
                <ext:Button ID="Button2" runat="server" Text="走动结束">
                    <AjaxEvents>
                        <Click OnEvent="Button2_Click"></Click>
                    </AjaxEvents>
                </ext:Button>--%>
                <ext:Button runat="server"  ID="btn_fcfk" Icon="FolderUser" Text="查看详情" Disabled="true">
                    <AjaxEvents>
                        <Click OnEvent="Button3_Click"></Click>
                    </AjaxEvents>
                </ext:Button>
                <ext:ToolbarFill />
                <ext:Button ID="Button4" runat="server" Text="导出报表" Icon="PageExcel" AutoPostBack="true" OnClick="ToExcelPL" Disabled="true">
                    <Listeners>
                        <Click Fn="saveDataPL" />
                    </Listeners>
                </ext:Button>
                <ext:ToolbarSeparator /> 
                </Items>
            </ext:Toolbar>
        </TopBar>
     </ext:GridPanel>
        <ext:Window ID="Window1"  runat="server" 
        BodyStyle="padding:5px;" 
        ButtonAlign="Right"
        Frame="true" 
        Title="待复查隐患信息详情"
        AutoHeight="true"
        Width="685px"
        Modal="true"
        ShowOnLoad="false"
        X="60"
        Y="100">
            <TopBar>
                <ext:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                        <ext:Button ID="Button3" runat="server" Text="导出报表" Icon="PageExcel" AutoPostBack="true" OnClick="ToExcel" Disabled="true">
                            <Listeners>
                                <Click Fn="saveData" />
                            </Listeners>
                        </ext:Button>
                    </Items>
                </ext:Toolbar>
            </TopBar>
            <Body>
            <ext:Hidden ID="hdnid" runat="server">
            </ext:Hidden>
            <ext:Hidden ID="Hidden1" runat="server">
            </ext:Hidden>
            <ext:FormLayout ID="FormLayout3" runat="server" LabelWidth="60">
            <ext:Anchor Horizontal="100%">
                <ext:GridPanel 
                    ID="GridPanel2" 
                    runat="server"
                    StoreID="Store2"
                    StripeRows="true"
                    
                    Collapsible="false"
                    Width="660px"
                    AutoHeight="true">
                    <ColumnModel ID="ColumnModel2" runat="server">
		                <Columns>
                            <ext:Column Header="内容" Width="170" Sortable="true" DataIndex="YHContent" >
                            </ext:Column>
                            <ext:Column Header="部门" Width="60" Sortable="true" DataIndex="DeptName" >
                            </ext:Column>
                            <ext:Column Header="地点" Width="100" Sortable="true" DataIndex="PlaceName" >
                            </ext:Column>
		                    <ext:Column  Header="班次" Width="70" DataIndex="BanCi" >
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
                        <ext:PagingToolBar ID="PagingToolBar2" runat="server" PageSize="9" />
                    </BottomBar>
                    <SelectionModel>
                            <ext:CheckboxSelectionModel ID="CheckboxSelectionModel2" SingleSelect="true" runat="server" />                   
                    </SelectionModel>
                    <AjaxEvents>
                        <Click OnEvent="RowClick1"></Click>
                    </AjaxEvents>
                </ext:GridPanel>
             </ext:Anchor>
            </ext:FormLayout>
            </Body>
        </ext:Window>
    </div>
    </form>
</body>
</html>
