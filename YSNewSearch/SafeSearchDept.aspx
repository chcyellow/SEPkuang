<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SafeSearchDept.aspx.cs" Inherits="YSNewSearch_SafeSearchDept" %>

<%@ Register assembly="Coolite.Ext.Web" namespace="Coolite.Ext.Web" tagprefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN" CleanResourceUrl="false" />
    <ext:Store ID="SWStore" runat="server">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="Deptname" />
                    <ext:RecordField Name="Yx" />
                    <ext:RecordField Name="Hg" />
                    <ext:RecordField Name="Bhg" />
                    <ext:RecordField Name="Yxrate" />
                    <ext:RecordField Name="Hgrate" />
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
    
     <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                <Center>
                    <ext:GridPanel 
                        ID="gpEdit" 
                        runat="server" 
                        StoreID="SWStore"
                        StripeRows="true"
                        Title="单位考核统计"
                        Icon="BrickMagnify"
                        Collapsible="false"
                        >
                        <TopBar>
                            <ext:Toolbar ID="Toolbar3" runat="server">
                                <Items>
                                    <ext:Label ID="Label1" runat="server" Text="单位:" StyleSpec="color:blue;" />
                                    <ext:ComboBox ID="cbbforcheckDept" runat="server"
                                    DisplayField="Deptname" 
                                    ValueField="Deptnumber"
                                    Width="180px" 
                                    StoreID="DeptStore" Editable="false">
                                    </ext:ComboBox>
                                    <ext:ToolbarSeparator />
                                    <ext:ToolbarButton ID="btnSearch" runat="server" Icon="Zoom" Text="查询">
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.btnSearch_Click();" />
                                        </Listeners>
                                    </ext:ToolbarButton>
                                </Items>
                            </ext:Toolbar>
                          </TopBar>
                        <ColumnModel ID="ColumnModel1" runat="server">
                            <Columns>
                                <ext:Column Header="单位" DataIndex="Deptname" />
                                <ext:Column Header="优秀" DataIndex="Yx" />
                                <ext:Column Header="合格" DataIndex="Hg" />
                                <ext:Column Header="不合格" DataIndex="Bhg" />
                                <ext:Column Header="优秀率" DataIndex="Yxrate" />
                                <ext:Column Header="合格率" DataIndex="Hgrate" />
                            </Columns>
                        </ColumnModel>
                        
                        <View>
                            <ext:GridView ForceFit="true" />
                        </View>
                        <SelectionModel>
                            <ext:RowSelectionModel runat="server" ID="rsmid" SingleSelect="true" />
                        </SelectionModel>
                        <BottomBar>
                            <ext:PagingToolbar runat="server" ID="pageToolBar">
                            </ext:PagingToolbar>
                        </BottomBar>
                    </ext:GridPanel>
                </Center>
            </ext:BorderLayout>
        </Body>
    </ext:ViewPort>
    </form>
</body>
</html>
