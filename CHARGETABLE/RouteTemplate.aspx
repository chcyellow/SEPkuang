<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RouteTemplate.aspx.cs" Inherits="CHARGETABLE_RouteTemplate" %>
<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>路线模板</title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN" CleanResourceUrl="false" />
    <ext:Hidden ID="TID" runat="server" Text="" />
    <ext:Hidden ID="TDID" runat="server" Text=""  />
    <div>
    <ext:Store ID="placeStore" runat="server">
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="Placeid" />
                    <ext:RecordField Name="Placename" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="placeTemplateStore" runat="server"  OnRefreshData="placeTemplateStore_RefershData" AutoLoad="true">
    <Proxy>
                <ext:DataSourceProxy>
                </ext:DataSourceProxy>
         </Proxy>
        <Reader>
            <ext:JsonReader  ReaderID="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="Name" />
                    <ext:RecordField Name="TLevel" />
                    <ext:RecordField Name="CreatePersonNumber" />
                    <ext:RecordField Name="CreatePersonName" />
                    <ext:RecordField Name="CreateMainDeptNumber" />
                    <ext:RecordField Name="CreateMainDeptName" />
                    <ext:RecordField Name="CreateTime"  Type="Date" DateFormat="Y-m-dTh:i:s"/>
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="YPPTDetailStore" runat="server"  OnRefreshData="YPPTDetailStore_RefershData" AutoLoad="true">
    <Proxy>
        <ext:DataSourceProxy>
        </ext:DataSourceProxy>
         </Proxy>
        <Reader>
            <ext:JsonReader  ReaderID="Id">
                <Fields>
                    <ext:RecordField Name="Id" />
                    <ext:RecordField Name="PptId" />
                    <ext:RecordField Name="Name" />
                    <ext:RecordField Name="PlaceId" />
                    <ext:RecordField Name="PlaceName" />
                    <ext:RecordField Name="MoveOrder" SortDir="ASC" />
                    <ext:RecordField Name="TLevel" />
                    <ext:RecordField Name="CreatePersonNumber" />
                    <ext:RecordField Name="CreatePersonName" />
                    <ext:RecordField Name="CreateMainDeptNumber" />
                    <ext:RecordField Name="CreateMainDeptName" />
                    <ext:RecordField Name="CreateTime"  Type="Date" DateFormat="Y-m-dTh:i:s"/>
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                <Center>
                    <%--数据列表--%>
                    <ext:GridPanel ID="GridPanel1" runat="server" StoreID="placeTemplateStore" Height="300" StripeRows="true"
                        Title="带班模板管理" Collapsible="false">
                        <ColumnModel ID="ColumnModel1" runat="server">
                            <Columns>
                                <ext:Column Header="Id" Width="150" DataIndex="Id" Hidden="true"> 
                                </ext:Column>
                                <ext:Column Header="名称" Width="100" DataIndex="Name" />
                                <ext:Column Header="级别" Width="100" Sortable="true" DataIndex="TLevel" />
                                <ext:Column Header="创建人编码" Width="150" Sortable="true" DataIndex="CreatePersonNumber" Hidden="true" />
                                <ext:Column Header="创建人" Width="150" Sortable="true" DataIndex="CreatePersonName" />
                                <ext:Column Header="创建单位编码" Width="200" Sortable="true" DataIndex="CreateMainDeptNumber" Hidden="true" />
                                <ext:Column Header="创建单位" Width="200" Sortable="true" DataIndex="CreateMainDeptName" />
                                <ext:Column Header="录入时间" Width="200" DataIndex="CreateTime">
                                    <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d H:i:s')" />
                                </ext:Column>
                            </Columns>
                        </ColumnModel>
                        <LoadMask ShowMask="true" Msg="数据加载中..." />
                        <BottomBar>
                            <ext:PagingToolbar ID="PagingToolBar1" runat="server" PageSize="15" StoreID="placeTemplateStore" />
                        </BottomBar>
                        <TopBar>
                            <ext:Toolbar runat="server" ID="tb1">
                                <Items>
                                    <ext:Button runat="server" ID="btnNew" Icon="Add" Text="新增模板">
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.AddTemplate()" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server" ID="btnPlan" Icon="BookEdit"  Text="修改模板">
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.UpdateTemplate();" />
                                        </Listeners>
                                    </ext:Button>
                                    
                                    <ext:Button runat="server" ID="btnDel" Icon="Delete" Text="删除模板">
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.DeleteTemplate();" />
                                        </Listeners>
                                    </ext:Button>
                                 
                                    <ext:ToolbarSeparator />
                                    <ext:Button runat="server" ID="btnDetail" Icon="ChartLine" Text="路线管理">
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.RouteMan();" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <SelectionModel>
                            <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" SingleSelect="true" runat="server" />
                        </SelectionModel>
                        <AjaxEvents>
                            <Click OnEvent="RowClick">
                            </Click>
                        </AjaxEvents>
                    </ext:GridPanel>
                </Center>
            </ext:BorderLayout>
        </Body>
    </ext:ViewPort>
    <ext:Window ID="Window1" runat="server" BodyStyle="padding:5px;" ButtonAlign="Right"
        Frame="true" Title="模板维护" AutoHeight="true" Width="300px" Modal="true" ShowOnLoad="false">
        <Body>
            <ext:FormLayout ID="FormLayout2" runat="server" LabelWidth="60">
                <ext:Anchor Horizontal="95%">
                    <ext:RadioGroup ID="rgLevel"
                    runat="server"
                    FieldLabel="级别"
                    Vertical="false">
                    <Items>
                        <ext:Radio ID="rGeRen" runat="server" BoxLabel="个人" InputValue="个人" />
                        <ext:Radio ID="rKuangJi" runat="server" BoxLabel="矿级" InputValue="矿级" />
                    </Items>
                </ext:RadioGroup>
                </ext:Anchor>
                <ext:Anchor Horizontal="95%">
                    <ext:TextField runat="server" ID="txtTName" FieldLabel="模板名称" ></ext:TextField>
                </ext:Anchor>
            </ext:FormLayout>
        </Body>
        <Buttons>
            <ext:Button ID="Button4" runat="server" Icon="Disk" Text="确 定">
                <Listeners>
                    <Click Handler="Coolite.AjaxMethods.SaveTName();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    <ext:Window ID="Window2" runat="server" BodyStyle="padding:5px;" ButtonAlign="Right"
        Frame="true" Title="地点维护" AutoHeight="true" Width="300px" Modal="true" ShowOnLoad="false">
        <Body>
            <ext:FormLayout ID="FormLayout1" runat="server" LabelWidth="60">
                
                <ext:Anchor Horizontal="95%">
                    <ext:ComboBox ID="cbbplace" runat="server" StoreID="placeStore" DisplayField="Placename"
                                    ValueField="Placeid" FieldLabel="地点" HideTrigger="true">
                                    <Listeners>
                                        <Render Fn="function(f) {
                                                            f.el.on('keyup', function(e) {
                                                            if(window.event.keyCode==38 || window.event.keyCode==40 || window.event.keyCode==13){
                                                                return;
                                                             }
                                                             Coolite.AjaxMethods.PYsearch(f.getRawValue(), 'placeStore');
                                                            });
                                                            }
                                                            " Delay="250" />
                                    </Listeners>
                                </ext:ComboBox>
                </ext:Anchor>
            </ext:FormLayout>
        </Body>
        <Buttons>
            <ext:Button ID="Button1" runat="server" Icon="Disk" Text="确 定">
                <Listeners>
                    <Click Handler="Coolite.AjaxMethods.BaseSave();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    <ext:Window ID="DetailWindow" runat="server" BodyStyle="padding:6px;" Frame="true"
        Title="模板行走路线" Height="390" Resizable="false" Width="660px" ShowOnLoad="false"
        Modal="true">
        <Body>
            <ext:FitLayout runat="server" ID="fitl1">
                <ext:GridPanel ID="GridPanel2" runat="server" StoreID="YPPTDetailStore" AutoExpandColumn="cold1"
                    Height="300" StripeRows="true" Header="false">
                    <ColumnModel ID="ColumnModel2" runat="server">
                        <Columns>
                            <ext:Column Header="走动地点ID"  DataIndex="PlaceId" Hidden="true" />
                            <ext:Column Header="走动地点" ColumnID="cold1" DataIndex="PlaceName" />
                            <ext:Column Header="走动次序" Width="70" DataIndex="MoveOrder"  />
                        </Columns>
                    </ColumnModel>
                    <LoadMask ShowMask="true" Msg="数据加载中..." />
                    <TopBar>
                        <ext:Toolbar runat="server" ID="tb2">
                            <Items>
                                <ext:Label runat="server" ID="Label1" Text="模板：" />
                                <ext:Label runat="server" ID="lblDetail" Text="" />
                                <ext:ToolbarSeparator />
                                
                                <ext:Button runat="server" ID="btnPlaceAdd" Icon="Add" Text="添加地点">
                                    <Listeners>
                                        <Click Handler="Coolite.AjaxMethods.PlaceAdd();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button runat="server" ID="btnPlaceUpdate" Icon="BookEdit" Text="修改地点">
                                    <Listeners>
                                        <Click Handler="Coolite.AjaxMethods.PlaceUpdate();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:ToolbarSeparator />
                                <ext:Button runat="server" ID="btnPlaceDel" Icon="Delete" Text="删除选中">
                                    <Listeners>
                                        <Click Handler="Coolite.AjaxMethods.PlaceDel();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:ToolbarSeparator />
                                <ext:Button runat="server" ID="btnUp" Icon="ArrowUp" Text="上移">
                                    <Listeners>
                                        <Click Handler="Coolite.AjaxMethods.PlaceUp();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button runat="server" ID="btnDown" Icon="ArrowDown" Text="下移">
                                    <Listeners>
                                        <Click Handler="Coolite.AjaxMethods.PlaceDown();" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <SelectionModel>
                        <ext:CheckboxSelectionModel ID="CheckboxSelectionModel2" SingleSelect="true" runat="server" />
                    </SelectionModel>
                    <AjaxEvents>
                        <Click OnEvent="RowClick1">
                        </Click>
                    </AjaxEvents>
                </ext:GridPanel>
            </ext:FitLayout>
        </Body>
    </ext:Window>
    </div>
    </form>
</body>
</html>
