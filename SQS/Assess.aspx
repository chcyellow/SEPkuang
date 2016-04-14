<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Assess.aspx.cs" Inherits="SQS_Assess" %>

<%@ Register assembly="Coolite.Ext.Web" namespace="Coolite.Ext.Web" tagprefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN" CleanResourceUrl="false" />
    <ext:Store ID="JeomStore" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="Rid">
                <Fields>
                    <ext:RecordField Name="Rid" />
                    <ext:RecordField Name="Dname" />
                    <ext:RecordField Name="Checkdate" Type="Date" DateFormat="Y-m-dTh:i:s" />
                    <ext:RecordField Name="CheckDeptName" />
                    <ext:RecordField Name="CheckForDeptName" />
                    <ext:RecordField Name="Placename" />
                    <ext:RecordField Name="Rstatus" />
                    <ext:RecordField Name="Total" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Hidden runat="server" ID="hdnKindid" />
    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                <West Collapsible="true" Split="true">
                    <ext:TreePanel ID="tpkind" runat="server" Title="考核类别" Border="false" Width="230" RootVisible="false">
                           <Root>
                               <ext:TreeNode NodeID="-1" Text="考核管理" />
                            </Root>
                    </ext:TreePanel>
                </West>
                <Center>
                    <%--<ext:Panel ID="Panel1" runat="server" Title="考核明细" Frame="true">
                        <AutoLoad Mode="IFrame" ShowMask="true" MaskMsg="正在加载数据，请稍候..." />
                    </ext:Panel>--%>
                    <ext:GridPanel 
                        ID="gpEdit" 
                        runat="server" 
                        StoreID="JeomStore"
                        StripeRows="true"
                        Title="考核结果"
                        Icon="BrickMagnify"
                        Collapsible="false"
                        >
                        <ColumnModel ID="ColumnModel1" runat="server">
                            <Columns>
                                <ext:Column Header="考核类型" DataIndex="Dname" />
                                <ext:Column Header="考核单位" DataIndex="CheckDeptName"/>
                                <ext:Column Header="被考核单位" DataIndex="CheckForDeptName" />
                                <ext:Column Header="考核地点" DataIndex="Placename" />
                                <ext:Column Header="得分" DataIndex="Total" />
                                <ext:Column Header="考核日期" Width="70" Sortable="true" DataIndex="Checkdate" >
                                    <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" />
                                </ext:Column>
                            </Columns>
                        </ColumnModel>
                        <TopBar>
                            <ext:Toolbar ID="Toolbar3" runat="server">
                                <Items>
                                    <ext:Label ID="Label1" runat="server" Text="考核时间" StyleSpec="color:blue;" />
                                    <ext:DateField ID="dfBeginDate" runat="server" />
                                    <ext:Label ID="Label2" runat="server" Text="----" StyleSpec="color:blue;" />
                                    <ext:DateField ID="dfEndDate" runat="server" />
                                    <ext:ToolbarSeparator />
                                    <ext:ToolbarButton ID="btnSearch" runat="server" Icon="Zoom" Text="查询" Disabled="true">
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.btnSearch_Click();" />
                                        </Listeners>
                                    </ext:ToolbarButton>
                                    <ext:ToolbarSeparator />
                                    <ext:ToolbarButton ID="btnNew" runat="server" Icon="Add" Text="新增" Disabled="true">
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.btnNew_Click();" />
                                        </Listeners>
                                    </ext:ToolbarButton>
                                    <ext:ToolbarButton ID="btnEdit" runat="server" Icon="FolderEdit" Text="修改" Disabled="true">
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.btnEdit_Click();" />
                                        </Listeners>
                                    </ext:ToolbarButton>
                                    <ext:ToolbarButton ID="btnDel" runat="server" Icon="Delete" Text="删除" Disabled="true">
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.btnDel_Click();" />
                                        </Listeners>
                                    </ext:ToolbarButton>
                                </Items>
                            </ext:Toolbar>
                          </TopBar>
                        <View>
                            <ext:GridView ForceFit="true" />
                        </View>
                        <SelectionModel>
                            <ext:RowSelectionModel runat="server" ID="rsmid" SingleSelect="true" />
                        </SelectionModel>
                        <AjaxEvents>
                            <Click OnEvent="RowClick"></Click>
                        </AjaxEvents>
                        <BottomBar>
                            <ext:PagingToolbar runat="server" ID="pageToolBar"></ext:PagingToolbar>
                        </BottomBar>
                    </ext:GridPanel>
                </Center>
            </ext:BorderLayout>
        </Body>
    </ext:ViewPort>
    <ext:Window ID="Window1"  ShowOnLoad="false" 
    BodyStyle="padding:0pc" runat="server"   BodyBorder="false"
    Collapsible="false" Frame="true" Modal="true" MinWidth="600" AutoScroll="true"
     Title="考核评分">
        <AutoLoad Mode="IFrame" />
         <LoadMask ShowMask="true" Msg="数据加载中...." />
         <Listeners>
            <BeforeShow Fn="function(el) { el.setHeight(Ext.getBody().getViewSize().height-20);el.setWidth(830); }" />
         </Listeners>
    </ext:Window>
    </form>
</body>
</html>
