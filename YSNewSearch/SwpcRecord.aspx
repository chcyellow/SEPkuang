<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SwpcRecord.aspx.cs" Inherits="YSNewSearch_SwpcRecord" %>

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
                    <ext:RecordField Name="Name" />
                    <ext:RecordField Name="Count" />
                    <ext:RecordField Name="Fsw" />
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

     <ext:Store ID="PersStore" runat="server" AutoLoad="false" OnRefreshData="PersRefresh">
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
                        StoreID="SWStore"
                        StripeRows="true"
                        Title="月度反三违档案"
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
                                        <Listeners>
                                            <Select Handler="#{fb_zrr}.clearValue(); #{PersStore}.reload();" />
                                        </Listeners>
                                    </ext:ComboBox>
                                    
                                    <ext:Label ID="Label2" runat="server" Text="三违人员:" StyleSpec="color:blue;" />
                                    <ext:ComboBox 
                                                ID="fb_zrr" 
                                                runat="server"
                                                DisplayField="Name" 
                                                ValueField="Personnumber"
                                                Width="200px"
                                                StoreID="PersStore"
                                                EmptyText="没有待选人员"
                                                Disabled="true" 
                                                >
                                            </ext:ComboBox>
                                            <ext:Label ID="Label3" runat="server" Text="查询类型:" StyleSpec="color:blue;" />
                                    <ext:ComboBox 
                                                ID="cbb_kind" 
                                                runat="server"
                                                Width="200px" SelectedIndex="0" Editable="false"
                                                >
                                                <Items>
                                                    <ext:ListItem Text="三违排查人员" Value="1" />
                                                    <ext:ListItem Text="带指标人员" Value="0" />
                                                    
                                                </Items>
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
                                <ext:Column Header="反三违人员" DataIndex="Name" />
                                <ext:Column Header="反三违数量" DataIndex="Count" />
                                <ext:Column Header="指标/月数" DataIndex="Fsw" />
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
