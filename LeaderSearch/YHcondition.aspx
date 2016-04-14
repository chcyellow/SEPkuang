<%@ Page Language="C#" AutoEventWireup="true" CodeFile="YHcondition.aspx.cs" Inherits="LeaderSearch_YHcondition" %>

<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../Style/examples.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Scripts/subform.js"></script>
    <script language="javascript" type="text/javascript">
        var saveData = function() {
            GridData.setValue(Ext.encode(GridPanel1.getRowsValues(false)));
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

        var change = function(value) {
            var color;
            if (value == '新增')
                color = 'red';
            else if (value == '复查未通过' || value == '逾期未整改' || value == '逾期整改未完成')
                color = 'orange';
            else if (value == '现场整改')
                color = 'gray';
            else
                color = 'green';
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
    <ext:Hidden ID="GridData" runat="server" />
    <%--列表数据--%>
    <ext:Store ID="Store1" runat="server" OnRefreshData="MyData_Refresh">
        <Reader>
            <ext:JsonReader ReaderID="Yhputinid">
                <Fields>
                    <ext:RecordField Name="Yhputinid" Type="Int" />
                    <ext:RecordField Name="Maindeptname" />
                    <ext:RecordField Name="Deptname" />
                    <ext:RecordField Name="Placename" />
                    <ext:RecordField Name="Banci" />
                    <ext:RecordField Name="Yhcontent" />
                    <ext:RecordField Name="Name" />
                    <ext:RecordField Name="Pctime" Type="Date" DateFormat="Y-m-dTh:i:s" />
                    <ext:RecordField Name="Intime" Type="Date" DateFormat="Y-m-dTh:i:s" />
                    <ext:RecordField Name="Yhlevel" />
                    <ext:RecordField Name="Yhtype" />
                    <ext:RecordField Name="Status"  SortDir="ASC" SortType="AsText"/>
                </Fields>
            </ext:JsonReader>
        </Reader>
        <SortInfo Field="Status" />
    </ext:Store>
    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                <Center>
                    <%--数据列表--%>
    <ext:GridPanel 
        ID="GridPanel1" 
        runat="server"
        StoreID="Store1"
        StripeRows="true" 
        Collapsible="false" 
        Width="810px"
        Height="400px" AutoScroll="true">
        <ColumnModel ID="ColumnModel1" runat="server">
		    <Columns>
                <ext:Column ColumnID="NIid" Header="隐患编号" Width="100" DataIndex="Yhputinid" />
                <ext:Column Header="隐患单位" Width="70" DataIndex="Maindeptname" />
                <ext:Column Header="隐患部门" Width="70" DataIndex="Deptname" />
                <ext:Column Header="隐患地点" Width="70" DataIndex="Placename" />
                <ext:Column Header="班次" Width="70" DataIndex="BanCi" />
                <ext:Column Header="隐患内容" Width="120" Sortable="false" DataIndex="Yhcontent">
                    <Renderer Fn="qtip" />
                </ext:Column>
                <ext:Column Header="排查人员" Width="70" DataIndex="Name" />
                <ext:Column Header="排查时间" Width="70" Sortable="true" DataIndex="Pctime" >
                    <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" />
                </ext:Column>
                <ext:Column Header="隐患级别" Width="70" Sortable="true" DataIndex="Yhlevel" />
                <ext:Column Header="隐患类型" Width="70" Sortable="true" DataIndex="Yhtype" />
                <ext:Column Header="隐患状态" Width="100" Sortable="true" DataIndex="Status" >
                    <Renderer Fn="change" />
                </ext:Column>
		    </Columns>
        </ColumnModel>
        <Plugins>
            <ext:GridFilters runat="server" ID="GridFilters1" Local="true">
                 <Filters>
                       <ext:StringFilter DataIndex="ID" />
                       <ext:StringFilter DataIndex="Name" />
                       <ext:StringFilter DataIndex="Placename" />
                       <ext:DateFilter DataIndex="Starttime">
                            <DatePickerOptions runat="server" TodayText="Now" />
                       </ext:DateFilter>
                       <ext:DateFilter DataIndex="Endtime">
                            <DatePickerOptions runat="server" TodayText="Now" />
                       </ext:DateFilter>
                 </Filters>
             </ext:GridFilters>
        </Plugins>
        <LoadMask ShowMask="true" Msg="数据加载中..." />
        <%--<BottomBar>
            <ext:PagingToolBar ID="PagingToolBar1" runat="server" PageSize="15" />
         </BottomBar>--%>
        <TopBar>
            <ext:Toolbar runat="server" ID="tb1">
                <Items>
                    <ext:Button runat="server" ID="btn_detail" Icon="FolderFind" Text="查看明细" Disabled="true" >
                        <Listeners>
                            <Click Handler="Coolite.AjaxMethods.DetailLoad();" />
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
    </div>   

    <ext:Window 
        ID="DetailWindow" 
        runat="server" 
        BodyStyle="padding:6px;" 
        ButtonAlign="Right"
        Frame="true" 
        Title="隐患明细信息"
        Height="400" AutoScroll="true" Resizable="false"
        Width="750px"
        ShowOnLoad="false"
        Y="1">
        <Tools>
            <ext:Tool Type="Refresh" Handler="Coolite.AjaxMethods.DetailLoad();" />
        </Tools>
         <AutoLoad Mode="IFrame" MaskMsg="信息正在加载，请稍候...." ShowMask="true" />
        
    </ext:Window>
    </form>
</body>
</html>
