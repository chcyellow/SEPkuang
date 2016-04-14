<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SWwarning.aspx.cs" Inherits="YSNewSearch_SWwarning" %>

<%@ Register assembly="Coolite.Ext.Web" namespace="Coolite.Ext.Web" tagprefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .x-grid3-body .x-grid3-td-Cost {
            background-color:#f1f2f4;
        }
        
        .x-grid3-summary-row .x-grid3-td-Cost {
            background-color:#e1e2e4;
        }
        
        .x-grid3-dirty-cell {
            background-image:none;
        }
        
        .x-grid3-summary-row {
            background:#F1F2F4 none repeat scroll 0 0;
            border-left:1px solid #FFFFFF;
            border-right:1px solid #FFFFFF;
            color:#333333;
        }
        
        .x-grid3-summary-row .x-grid3-cell-inner {
            font-weight:bold;
            padding-bottom:4px;
        }
        
        .x-grid3-cell-first .x-grid3-cell-inner {
            padding-left:16px;
        }
        
        .x-grid-hide-summary .x-grid3-summary-row {
            display:none;
        }
        
        .x-grid3-summary-msg {
            font-weight:bold;
            padding:4px 16px;
        }        
    </style>
    <script type="text/javascript">
        var template = '<span style="color:{0};"><b>{1}</b></span>';

        var change = function(value) {
        var color;
        if (value >= hdnScore.getValue())//Coolite.AjaxMethods.GetJfSet()
                color = 'red';
            else
                color = 'blue';
            return String.format(template, color, value);
        }

        var change2 = function(value) {
            var color;
            if (value >= hdnCount.getValue())//Coolite.AjaxMethods.GetJfSet()
                color = 'red';
            else
                color = 'blue';
            return String.format(template, color, value);
        }
    </script>


</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN" CleanResourceUrl="false" />
    <ext:Store ID="SWStore" runat="server"><%-- GroupField="Deptname">--%>
        <Reader>
            <ext:JsonReader ReaderID="PERSONNUMBER">
                <Fields>
                    <ext:RecordField Name="DEPTNAME" />
                    <ext:RecordField Name="PERSONNAME" />
                    <ext:RecordField Name="TOTAL"/>
                    <ext:RecordField Name="三违总数" />
                </Fields>
            </ext:JsonReader>
        </Reader>
        <SortInfo Field="TOTAL" Direction="DESC" />
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

    <ext:Hidden ID="hdnScore" runat="server" />
    <ext:Hidden ID="hdnCount" runat="server" />

    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                <Center>
                    <ext:GridPanel 
                        ID="gpEdit" 
                        runat="server" 
                        StoreID="SWStore"
                        StripeRows="true"
                        Title="年度三违预警"
                        Icon="BrickMagnify"
                        Collapsible="false"
                        >
                        <ColumnModel ID="ColumnModel1" runat="server">
                            <Columns>
                                <ext:Column Header="单位" DataIndex="DEPTNAME" />
                                 <ext:GroupingSummaryColumn
                                    ColumnID="Name"
                                    Header="三违人员"
                                    Sortable="true"
                                    DataIndex="PERSONNAME"
                                    Hideable="false"
                                    SummaryType="Count">
                                    <SummaryRenderer Handler="return  '(共计：' + value +')';" /> 
                                </ext:GroupingSummaryColumn>
                                <ext:GroupingSummaryColumn
                                    Width="100"
                                    ColumnID="Score"
                                    Header="年度积分"
                                    Sortable="true"
                                    DataIndex="TOTAL"
                                    SummaryType="Sum">
                                    <Renderer Fn="change" />
                                </ext:GroupingSummaryColumn>
                                <ext:GroupingSummaryColumn
                                    Width="100"
                                    ColumnID="Count"
                                    Header="三违次数"
                                    Sortable="true"
                                    DataIndex="三违总数"
                                    SummaryType="Sum">
                                    <Renderer Fn="change2" />
                                </ext:GroupingSummaryColumn>
                            </Columns>
                        </ColumnModel>
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
                                    <ext:ToolbarSeparator />
                                    <ext:ToolbarButton ID="btnSearch" runat="server" Icon="Zoom" Text="查询">
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.btnSearch_Click();" />
                                        </Listeners>
                                    </ext:ToolbarButton>
                                    <ext:ToolbarSeparator />
                                    <ext:ToolbarButton ID="btnswdetail" runat="server" Icon="Zoom" Text="三违信息">
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.DetailLoad();" />
                                        </Listeners>
                                    </ext:ToolbarButton>
                                </Items>
                            </ext:Toolbar>
                          </TopBar>
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
                            <ext:RowSelectionModel runat="server" ID="rsmid" SingleSelect="true" />
                        </SelectionModel>
                        <BottomBar>
                            <ext:PagingToolbar runat="server" ID="pageToolBar">
                                <Items>
                                    <ext:ToolbarSeparator />
                                    <ext:Button 
                                        ID="btnToggleGroups" 
                                        runat="server" 
                                        Text="展开/合并组"
                                        Icon="TableSort"
                                        Style="margin-left: 6px;"
                                        AutoPostBack="false">
                                        <Listeners>
                                            <Click Handler="#{gpEdit}.getView().toggleAllGroups();" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:PagingToolbar>
                        </BottomBar>
                    </ext:GridPanel>
                </Center>
            </ext:BorderLayout>
        </Body>
    </ext:ViewPort>

    <ext:Window 
        ID="Window1" 
        runat="server"
        Maximizable="true" 
        Icon="Server" 
        Title="数据信息"
        Width="725"
        Modal="true"
        ShowOnLoad="false"
        Resizable="false"
        CloseAction="Hide">
        <AutoLoad Url="" Mode="IFrame" ShowMask="true" MaskMsg="正在加载数据..." />
    </ext:Window>
    </form>
</body>
</html>
