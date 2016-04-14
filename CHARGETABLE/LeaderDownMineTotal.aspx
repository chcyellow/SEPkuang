<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LeaderDownMineTotal.aspx.cs" Inherits="CHARGETABLE_LeaderDownMineTotal" %>
<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>副总以上领导下井人员情况汇总表</title>
    <script type="text/javascript">
        var qtip = function (v, p) {//单元格提示
            //v : value , p : cell
            p.attr = 'ext:qtitle="" ext:qtip="' + v + '"';
            return v;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN" CleanResourceUrl="false" />
    <div>
    <ext:Store ID="Store1" runat="server" OnRefreshData="MyData_Refresh" AutoLoad="true">
    <Proxy>
                <ext:DataSourceProxy>
                </ext:DataSourceProxy>
         </Proxy>
        <Reader>
            <ext:JsonReader ReaderID="PersonNumber">
                <Fields>
                    <ext:RecordField Name="MainDeptNumber" />
                    <ext:RecordField Name="MainDeptName" />
                    <ext:RecordField Name="PersonNumber" />
                    <ext:RecordField Name="PersonName" />
                    <ext:RecordField Name="PostID" />
                    <ext:RecordField Name="PostName" />
                    <ext:RecordField Name="DaiBanTotal" />
                    <ext:RecordField Name="DaiBanZao" />
                    <ext:RecordField Name="DaiBanZhong" />
                    <ext:RecordField Name="DaiBanYe" />
                    <ext:RecordField Name="DownMineTotal" />                  
                    <ext:RecordField Name="About" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="DownDetailStore" runat="server" >
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="MainDeptNumber" />
                    <ext:RecordField Name="MainDeptName" />
                    <ext:RecordField Name="PersonNumber" />
                    <ext:RecordField Name="PersonName" />
                    <ext:RecordField Name="PostID" />
                    <ext:RecordField Name="PostName" />
                    <ext:RecordField Name="DownTime"  Type="Date" DateFormat="Y-m-dTh:i:s"/>
                    <ext:RecordField Name="UpTime" Type="Date" DateFormat="Y-m-dTh:i:s"/>
                    <ext:RecordField Name="Move" />
                    <ext:RecordField Name="MM" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="DBDetailStore" runat="server" >
        <Reader>
            <ext:JsonReader>
                <Fields>
                    <ext:RecordField Name="MainDeptNumber" />
                    <ext:RecordField Name="MainDeptName" />
                    <ext:RecordField Name="PersonNumber" />
                    <ext:RecordField Name="PersonName" />
                    <ext:RecordField Name="PostID" />
                    <ext:RecordField Name="PostName" />
                    <ext:RecordField Name="DBDate" Type="Date" DateFormat="Y-m-dTh:i:s" />
                    <ext:RecordField Name="BanCi" />
                    <ext:RecordField Name="Move" />
                    <ext:RecordField Name="MM" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="perStore" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="Personnumber">
                <Fields>
                    <ext:RecordField Name="Personnumber" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="DeptStore" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="Deptnumber">
                <Fields>
                    <ext:RecordField Name="Deptnumber" />
                    <ext:RecordField Name="Deptname" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="PostStore" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="PosID">
                <Fields>
                    <ext:RecordField Name="PosID" />
                    <ext:RecordField Name="PosName" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    </div>
     <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                <Center>
                    <%--数据列表--%>
                    <ext:GridPanel ID="GridPanel1" runat="server" StoreID="Store1" Height="300" StripeRows="true"
                        Title="各矿副总以上领导下井情况汇总表" Collapsible="false">
                        <ColumnModel ID="ColumnModel1" runat="server">
                            <Columns>
                                
                                <ext:Column Header="单位ID" Hidden="true" DataIndex="MainDeptNumber" />
                                <ext:Column Header="单位" Width="200" Sortable="true" DataIndex="MainDeptName" />
                                <ext:Column Header="人员ID" Hidden="true" DataIndex="PersonNumber" />
                                <ext:Column Header="姓名" Width="150" Sortable="true" DataIndex="PersonName" />
                                <ext:Column Header="职务ID" Hidden="true" DataIndex="PostID" />
                                <ext:Column Header="职务" Width="200" Sortable="true" DataIndex="PostName" />
                                <ext:Column Header="下井数" Width="100" Sortable="true" DataIndex="DownMineTotal" Css="cursor:pointer;" />
                                <ext:Column Header="带班合计" Width="100" Sortable="true" DataIndex="DaiBanTotal" Css="cursor:pointer;" />
                                <ext:Column Header="中班" Width="100" DataIndex="DaiBanZhong" Css="cursor:pointer;" />
                                <ext:Column Header="早班" Width="100" DataIndex="DaiBanZao" Css="cursor:pointer;" />
                                <ext:Column Header="夜班" Width="100" DataIndex="DaiBanYe" Css="cursor:pointer;" />
                                <ext:Column Header="备注" Width="100"  DataIndex="About" />
                            </Columns>
                        </ColumnModel>
                        <LoadMask ShowMask="true" Msg="数据加载中..." />
                        <BottomBar>
                            <ext:PagingToolbar ID="PagingToolBar1" runat="server" PageSize="15" StoreID="Store1" />
                        </BottomBar>
                        <TopBar>
                            <ext:Toolbar runat="server" ID="tb1">
                                <Items>
                                    <ext:Label runat="server" ID="lbldw" Text="单位:" />
                                    <ext:ComboBox ID="cbbDept" runat="server" StoreID="DeptStore" DisplayField="Deptname"
                                        ValueField="Deptnumber" FieldLabel="单位" Editable="false" AllowBlank="false">
                                        <Listeners>
                              <Select Handler="Coolite.AjaxMethods.Changed();" />
                            </Listeners>
                                    </ext:ComboBox>
                                    <ext:Label runat="server" ID="Label1" Text="职务:" />
                                    <ext:ComboBox ID="cboPost" runat="server" StoreID="PostStore" DisplayField="PosName"
                                        ValueField="PosID" FieldLabel="职务">
                                    </ext:ComboBox>
                                    <ext:Label runat="server" ID="lblName" Text="姓名:" />
                                    <ext:TextField runat="server" ID="txtName" EmptyText="多人请用逗号（，）隔开" Width="200"></ext:TextField>
                                    <ext:Label runat="server" ID="lblYear" Text="年份：" ></ext:Label>
                                    <ext:ComboBox runat="server" ID="cboYear" Width="60"></ext:ComboBox>
                                    <ext:Label runat="server" ID="lblMonth" Text="月份："></ext:Label>
                                    <ext:ComboBox runat="server" ID="cboMonth" Width="60">
                                    <Items>
                                    <ext:ListItem Value="1" Text="1" />
                                    <ext:ListItem Value="2" Text="2" />
                                    <ext:ListItem Value="3" Text="3" />
                                    <ext:ListItem Value="4" Text="4" />
                                    <ext:ListItem Value="5" Text="5" />
                                    <ext:ListItem Value="6" Text="6" />
                                    <ext:ListItem Value="7" Text="7" />
                                    <ext:ListItem Value="8" Text="8" />
                                    <ext:ListItem Value="9" Text="9" />
                                    <ext:ListItem Value="10" Text="10" />
                                    <ext:ListItem Value="11" Text="11" />
                                    <ext:ListItem Value="12" Text="12" />
                                    </Items>
                                    
                                    </ext:ComboBox>
                                    <ext:ToolbarSeparator />
                                    <ext:Button runat="server" ID="btn_search" Icon="Find" Text="查询">
                                        <Listeners>
                                            <Click Handler="#{Store1}.reload();" />
                                        </Listeners>
                                    </ext:Button>
                                    
                                    <ext:ToolbarFill />
                                    <ext:Button runat="server" ID="btnExport" Icon="PageExcel" Text="导出" Hidden = "true">
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.ExportXls();" />
                                        </Listeners>
                                    </ext:Button>
                                    
                                </Items>
                                
                            </ext:Toolbar>
                        </TopBar>
                        <SelectionModel>
                            <ext:CellSelectionModel ID="CellSelectionModel1" runat="server">
                                <AjaxEvents>
                                    <CellSelect OnEvent="Cell_Click" />                        
                                </AjaxEvents>
                            </ext:CellSelectionModel>
                        </SelectionModel>
                        
                    </ext:GridPanel>
                </Center>
            </ext:BorderLayout>
        </Body>
    </ext:ViewPort>
    <ext:Window ID="winDown" runat="server" BodyStyle="padding:6px;" Frame="true"
        Title="下井详情" Height="400" Resizable="false" Width="1000px" ShowOnLoad="false"
        Modal="true">
        <Body>
            <ext:FitLayout runat="server" ID="fitl1">
                <ext:GridPanel ID="GridPanel2" runat="server" StoreID="DownDetailStore" AutoExpandColumn="cold1"
                    Height="380" Header="false">
                    <ColumnModel ID="ColumnModel2" runat="server">
                        <Columns>
                            <ext:Column Header="单位"  Width="100" DataIndex="MainDeptName"  />
                            <ext:Column Header="姓名" Width="50"  DataIndex="PersonName" />
                            <ext:Column Header="职务" Width="100" DataIndex="PostName"  />
                            <ext:Column Header="入井时间"  Width="150" DataIndex="DownTime" >
                            <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d H:i:s')" />
                            </ext:Column>
                            <ext:Column Header="升井时间" Width="150" DataIndex="UpTime" >
                            <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d H:i:s')" />
                            </ext:Column>
                            <ext:Column Header="走动轨迹" Width="300" ColumnID="cold1" DataIndex="Move"  >
                            <Renderer Fn="qtip" />
                            </ext:Column>
                        </Columns>
                    </ColumnModel>
                    <LoadMask ShowMask="true" Msg="数据加载中..." /> 
                </ext:GridPanel>
            </ext:FitLayout>
        </Body>
    </ext:Window>
    <ext:Window ID="winDaiBan" runat="server" BodyStyle="padding:6px;" Frame="true"
        Title="带班详情" Height="400" Resizable="false" Width="1000px" ShowOnLoad="false"
        Modal="true">
        <Body>
            <ext:FitLayout runat="server" ID="FitLayout1">
                <ext:GridPanel ID="GridPanel3" runat="server" StoreID="DBDetailStore" AutoExpandColumn="cold1"
                    Height="380" Header="false">
                    <ColumnModel ID="ColumnModel3" runat="server">
                        <Columns>
                            <ext:Column Header="单位"  Width="100" DataIndex="MainDeptName"  />
                            <ext:Column Header="姓名" Width="50"  DataIndex="PersonName" />
                            <ext:Column Header="职务" Width="100" DataIndex="PostName"  />
                            <ext:Column Header="带班日期"  Width="100" DataIndex="DBDate"  >
                                <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" />
                            </ext:Column>
                            <ext:Column Header="班次" Width="50"  DataIndex="BanCi" />
                            <ext:Column Header="计划走动地点" ColumnID="cold1" Width="380" DataIndex="Move" >
                            <Renderer Fn="qtip" />
                            </ext:Column>
                        </Columns>
                    </ColumnModel>
                    <LoadMask ShowMask="true" Msg="数据加载中..." />
                </ext:GridPanel>
            </ext:FitLayout>
        </Body>
    </ext:Window>
    </form>
</body>
</html>
