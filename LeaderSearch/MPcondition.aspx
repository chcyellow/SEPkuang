<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MPcondition.aspx.cs" Inherits="LeaderSearch_MPcondition" %>

<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../Style/examples.css" rel="stylesheet" type="text/css" />
   
</head>
<body>
    <form id="form1" runat="server">
    <div style=" text-align:center">
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN">
        </ext:ScriptManager>
        <ext:Store ID="Store1" runat="server" >
        <Reader>
            <ext:JsonReader ReaderID="ID">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Personid"/>
                    <ext:RecordField Name="Name" />
                    <ext:RecordField Name="Deptname" />
                    <ext:RecordField Name="Placename" />
                    <ext:RecordField Name="Starttime" Type="Date" DateFormat="Y-m-dTh:i:s" />
                    <ext:RecordField Name="Endtime" Type="Date" DateFormat="Y-m-dTh:i:s" />
                    <ext:RecordField Name="Movestarttime" Type="Date" DateFormat="Y-m-dTh:i:s" />
                    <ext:RecordField Name="Moveendtime" Type="Date" DateFormat="Y-m-dTh:i:s" />                                     
                    <ext:RecordField Name="Movestate" />
                    <ext:RecordField Name="Maindeptid" />
                    <ext:RecordField Name="Maindeptname" />
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
        AutoExpandColumn="ID" 
        Collapsible="false"
        Width="745px"
        Height="400px" AutoScroll="true">
        <ColumnModel ID="ColumnModel1" runat="server">
		    <Columns>
		        <ext:Column ColumnID="ID" Header="编号" Width="70" DataIndex="Id" >
                </ext:Column>
                <ext:Column Header="单位" Width="100" Sortable="true" DataIndex="Maindeptname" >
                </ext:Column>
                <ext:Column Header="部门" Width="100" Sortable="true" DataIndex="Deptname" >
                </ext:Column>
                <ext:Column Header="走动人员" Width="80" Sortable="true" DataIndex="Name" >
                </ext:Column>
                <ext:Column Header="走动地点" Width="100" Sortable="true" DataIndex="Placename" >
                </ext:Column>
                <ext:Column Header="计划开始" Width="80" Sortable="true" DataIndex="Starttime" >
                <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" /></ext:Column>
                <ext:Column Header="计划截止" Width="80" Sortable="true" DataIndex="Endtime" >
                <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" /></ext:Column>
                <ext:Column Header="走动开始" Width="80" Sortable="true" DataIndex="Movestarttime" Hidden="true" >
                <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d h:i')" /></ext:Column>
                <ext:Column Header="走动结束" Width="80" Sortable="true" DataIndex="Moveendtime" Hidden="true">
                <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d h:i')" /></ext:Column>
                <ext:Column Header="走动状态" Width="130" Sortable="true" DataIndex="Movestate" >
                </ext:Column>
		    </Columns>
        </ColumnModel>
     </ext:GridPanel>
     </Center>
            </ext:BorderLayout>
        </Body>
    </ext:ViewPort> 
    </div>
    </form>
</body>
</html>

