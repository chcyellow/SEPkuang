<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SWcondition.aspx.cs" Inherits="LeaderSearch_SWcondition" %>

<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../Styles/examples.css" rel="stylesheet" type="text/css" />
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
        <ext:Store ID="Store1" runat="server" OnRefreshData="MyData_Refresh">
            <Reader>
                <ext:JsonReader ReaderID="Id">
                    <Fields>
                        <ext:RecordField Name="Id" Type="Int" />
                        <ext:RecordField Name="Maindeptname" />
                        <ext:RecordField Name="Swperson" />
                        <ext:RecordField Name="Deptname" />
                        <ext:RecordField Name="Swcontent" />
                        <ext:RecordField Name="Swlevel" />
                        <ext:RecordField Name="Placename" />
                        <ext:RecordField Name="Banci" />
                        <ext:RecordField Name="Pcname" />
                        <ext:RecordField Name="Pctime" Type="Date" DateFormat="Y-m-dTh:i:s" />
                        <ext:RecordField Name="Ispublic" Type="Boolean" />
                        <ext:RecordField Name="Isend" Type="Boolean" />
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
            Collapsible="false"
            Width="840px"
            Height="400px" AutoScroll="true">
            <ColumnModel ID="ColumnModel1" runat="server">
		        <Columns>
                    <ext:Column ColumnID="NIid" Header="三违编号" Width="80" DataIndex="Id" />
                    <ext:Column Header="单位" Width="70" DataIndex="Maindeptname" />
                    <ext:Column Header="部门" Width="70" DataIndex="Deptname" />
                    <ext:Column Header="三违人员" Width="70" DataIndex="Swperson" />
                    <ext:Column Header="三违内容" Width="120" Sortable="false" DataIndex="Swcontent">
                        <Renderer Fn="qtip" />
                    </ext:Column>
                    <ext:Column Header="三违级别" Width="70" Sortable="true" DataIndex="Swlevel" />
                    <ext:Column Header="三违地点" Width="70" DataIndex="Placename" />
                    <ext:Column Header="发生班次" Width="70" DataIndex="Banci" />
                    <ext:Column Header="排查人员" Width="70" DataIndex="Pcname" />
                    <ext:Column Header="排查时间" Width="70" Sortable="true" DataIndex="Pctime" >
                        <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" />
                    </ext:Column>
                    <ext:Column Header="是否公布" Width="75" Sortable="true" DataIndex="Ispublic">
                        <Renderer Handler="return value ? '已公布':'未公布';" />
                    </ext:Column>
                    <ext:Column Header="是否闭合" Width="75" Sortable="true" DataIndex="Isend">
                        <Renderer Handler="return value ? '是':'否';" />
                    </ext:Column>                
		        </Columns>
            </ColumnModel>
            <LoadMask ShowMask="true" Msg="数据加载中..." />
            <%--<BottomBar>
                <ext:PagingToolBar ID="PagingToolBar1" runat="server" PageSize="15" />
            </BottomBar>--%>
            <SelectionModel>
                <ext:RowSelectionModel runat="server" SingleSelect="true" ID="RowSelectionModel1" />                 
            </SelectionModel>
         </ext:GridPanel>
         </Center>
            </ext:BorderLayout>
        </Body>
    </ext:ViewPort> 
         
       
     </div>   
    </form>
</body>
</html>
