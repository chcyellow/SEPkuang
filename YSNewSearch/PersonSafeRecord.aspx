<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PersonSafeRecord.aspx.cs" Inherits="YSNewSearch_PersonSafeRecord" %>

<%@ Register assembly="Coolite.Ext.Web" namespace="Coolite.Ext.Web" tagprefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN" CleanResourceUrl="false" />
    
    <ext:Store ID="DataStore" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="PERSONNUMBER">
                <Fields>
                    <ext:RecordField Name="DEPTNAME" />
                    <ext:RecordField Name="NAME" />
                    <ext:RecordField Name="YH" />
                    <ext:RecordField Name="SW" />
                    <ext:RecordField Name="ZD" />
                    <ext:RecordField Name="GS" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    
    <ext:Store ID="DetailStore" runat="server" GroupField="KIND">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="KIND" />
                    <ext:RecordField Name="CONTENT" />
                    <ext:RecordField Name="PCTIME" Type="Date" DateFormat="Y-m-dTh:i:s" />
                </Fields>
            </ext:JsonReader>
        </Reader>
        <SortInfo Field="PCTIME" Direction="DESC" />
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
                        StoreID="DataStore"
                        StripeRows="true"
                        Title="安全档案"
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
                                    
                                    <ext:Label ID="Label2" runat="server" Text="人员:" StyleSpec="color:blue;" />
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
                                    <ext:ToolbarSeparator />
                                    <ext:ToolbarButton ID="btnSearch" runat="server" Icon="Zoom" Text="查询">
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.btnSearch_Click();" />
                                        </Listeners>
                                    </ext:ToolbarButton>
                                    <ext:ToolbarSeparator />
                                    <ext:ToolbarButton ID="ToolbarButton1" runat="server" Icon="NoteGo" Text="查看明细" Disabled="true">
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.DetailShow();" />
                                        </Listeners>
                                    </ext:ToolbarButton>
                                </Items>
                            </ext:Toolbar>
                          </TopBar>
                        <ColumnModel ID="ColumnModel1" runat="server">
                            <Columns>
                                <ext:Column Header="单位" DataIndex="DEPTNAME" />
                                <ext:Column Header="姓名" DataIndex="NAME" />
                                <ext:Column Header="隐患" DataIndex="YH" />
                                <ext:Column Header="三违" DataIndex="SW" />
                                <ext:Column Header="走动情况" DataIndex="ZD" />
                                <ext:Column Header="工伤信息" DataIndex="GS" />
                                <%--<ext:CommandColumn Width="60" Header="查看明细">
                                    <Commands>
                                        <ext:GridCommand Icon="NoteEdit" CommandName="Edit" ToolTip-Text="查看明细" />
                                    </Commands>
                                </ext:CommandColumn>--%>
                                <%--<ext:ImageCommandColumn Header="查看明细" Width="60">
                                    <Commands>
                                        <ext:ImageCommand Icon="NoteGo" CommandName="Edit" ToolTip-Text="查看明细" />
                                    </Commands>
                                </ext:ImageCommandColumn>--%>
                            </Columns>
                        </ColumnModel>
                        
                        <View>
                            <ext:GridView ForceFit="true" />
                        </View>
                        <SelectionModel>
                            <ext:RowSelectionModel runat="server" ID="rsmid" SingleSelect="true" />
                        </SelectionModel>
                        <BottomBar>
                            <ext:PagingToolbar runat="server" ID="pageToolBar" />
                        </BottomBar>
                        <%--<Listeners>
                            <RowClick Handler="Coolite.AjaxMethods.RowClick();" />
                            <Command Handler="Coolite.AjaxMethods.DetailShow(command);" />
                        </Listeners>--%>
                        <AjaxEvents>
                            <Click OnEvent="RowClick"></Click>
                        </AjaxEvents>

                    </ext:GridPanel>
                </Center>
            </ext:BorderLayout>
        </Body>
    </ext:ViewPort>
    
    <ext:Window 
        ID="DetailWindow" 
        runat="server" 
        BodyStyle="padding:6px;" 
        ButtonAlign="Right"
        Frame="true" 
        Title="安全档案明细"
        Height="400" AutoScroll="true" Resizable="false"
        Width="600px"
        ShowOnLoad="false">
        <Tools>
            <ext:Tool Type="Toggle" Handler="#{GridPanel1}.getView().toggleAllGroups();" />
        </Tools>
        <LoadMask Msg="信息正在加载，请稍候...." ShowMask="true" />
        <Body>
            <ext:BorderLayout ID="BorderLayout2" runat="server">
                <Center>
                    <ext:GridPanel 
                        ID="GridPanel1" 
                        runat="server" 
                        StoreID="DetailStore"
                        StripeRows="true"
                        Header="false"
                        Collapsible="false"
                        >
                        <ColumnModel ID="ColumnModel2" runat="server">
                            <Columns>
                                <ext:Column Header="分类" DataIndex="KIND" Width="80" />
                                <ext:Column Header="内容" DataIndex="CONTENT" Width="400">
                                    <Renderer Fn="qtip" />
                                </ext:Column>
                                <ext:GroupingSummaryColumn
                                    ColumnID="Name"
                                    Header="时间"
                                    Sortable="true"
                                    DataIndex="PCTIME"
                                    Hideable="false"
                                    SummaryType="Count">
                                    <SummaryRenderer Handler="return  '<b>(共计：' + value +')</b>';" /> 
                                    <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" />
                                </ext:GroupingSummaryColumn>
                            </Columns>
                        </ColumnModel>
                        <View>
                            <%--<ext:GridView ForceFit="true" />--%>
                            <ext:GroupingView ID="GroupingView1"
                                runat="server"
                                ForceFit="true"
                                MarkDirty="false"
                                ShowGroupName="false"
                                EnableNoGroups="true"
                                HideGroupedColumn="true"
                                />
                        </View>
                        <Plugins>
                            <ext:GroupingSummary ID="GroupingSummary1" runat="server" />
                        </Plugins>
                        <SelectionModel>
                            <ext:RowSelectionModel runat="server" ID="RowSelectionModel1" SingleSelect="true" />
                        </SelectionModel>
                    </ext:GridPanel>
                </Center>
            </ext:BorderLayout>
        </Body>
    </ext:Window>
    </form>
</body>
</html>
