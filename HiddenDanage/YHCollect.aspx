<%@ Page Language="C#" AutoEventWireup="true" CodeFile="YHCollect.aspx.cs" Inherits="HiddenDanage_YHCollect" %>

<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" CleanResourceUrl="false" Locale="zh-CN" />
    <%--列表数据--%>
    <ext:Store ID="Store1" runat="server" OnRefreshData="MyData_Refresh">
        <Reader>
            <ext:JsonReader ReaderID="Cid">
                <Fields>
                    <ext:RecordField Name="Cid" Type="Int" />
                    <ext:RecordField Name="YinHuanContent" />
                    <ext:RecordField Name="Ysbz" />
                    <ext:RecordField Name="Name" />
                    <ext:RecordField Name="InTime" Type="Date" DateFormat="Y-m-dTh:i:s" />
                    <ext:RecordField Name="DeptName" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <%--数据列表--%>
    <ext:GridPanel 
        ID="GridPanel1" 
        runat="server"
        StoreID="Store1"
        StripeRows="true"
        Title="新增隐患/三违信息" 
        Collapsible="false" 
        AutoExpandColumn="NIid"
        AutoHeight="true">
        <ColumnModel ID="ColumnModel1" runat="server">
		    <Columns>
                <ext:Column Header="编号"  DataIndex="Cid" Align="Center" />
                <ext:Column Header="新增隐患/三违内容" ColumnID="NIid" Width="500" DataIndex="YinHuanContent" Align="Center" />
                <ext:Column Header="标识" DataIndex="Ysbz" Align="Center" />
                <ext:Column Header="申请人员" DataIndex="Name" Align="Center" />
                <ext:Column Header="申请时间" Sortable="true" DataIndex="InTime" Align="Center" >
                    <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" />
                </ext:Column>
                <ext:Column Header="申请单位" DataIndex="DeptName" Align="Center" />
		    </Columns>
        </ColumnModel>
        <Plugins>
            <ext:GridFilters runat="server" ID="GridFilters1" Local="true">
                 <Filters>
                       <ext:StringFilter DataIndex="Cid" />
                       <ext:StringFilter DataIndex="YinHuanContent" />
                       <ext:StringFilter DataIndex="Name" />
                       <ext:DateFilter DataIndex="InTime">
                            <DatePickerOptions runat="server" TodayText="Now" />
                       </ext:DateFilter>
                       <ext:StringFilter DataIndex="DeptName" />
                 </Filters>
             </ext:GridFilters>
        </Plugins>
        <LoadMask ShowMask="true" />
        <BottomBar>
            <ext:PagingToolBar ID="PagingToolBar1" runat="server" PageSize="15" StoreID="Store1" />
         </BottomBar>
        <TopBar>
            <ext:Toolbar runat="server" ID="tb1">
                <Items>
                    <ext:Label ID="Label1" runat="server" Text="时间选择:">
                    </ext:Label>
                    <ext:DateField ID="dfBegin" runat="server" Width="100" Vtype="daterange">
                    </ext:DateField>
                    <ext:Label ID="Label2" runat="server" Text="---">
                    </ext:Label>
                    <ext:DateField ID="dfEnd" runat="server" Width="100" Vtype="daterange"> 
                    </ext:DateField>
                    <ext:ToolbarSeparator />
                    <ext:Button ID="Button2" runat="server" Text="查询" Icon="Zoom">
                        <Listeners>
                            <Click Handler="Coolite.AjaxMethods.storeload();" />
                        </Listeners>
                    </ext:Button>
                    <ext:ToolbarFill />
                    <ext:Button runat="server" ID="btn_ContentIn" Icon="FolderUp" Text="危险源辨识">
                        <Listeners>
                            <Click Handler="Coolite.AjaxMethods.ContentIn();" />
                        </Listeners>
                    </ext:Button>
                    <ext:ToolbarSeparator />
                    <ext:Button runat="server" ID="btn_ContentDel" Icon="FolderUser" Text="信息删除">
                        <Listeners>
                            <Click Handler="Coolite.AjaxMethods.ContentDel();" />
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
    </form>
</body>
</html>
