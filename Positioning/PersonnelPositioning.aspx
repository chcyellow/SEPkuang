<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PersonnelPositioning.aspx.cs" Inherits="Positioning_PersonnelPositioning" %>

<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script language="javascript" type="text/javascript">
        var template = '<span style="color:{0};cursor: pointer;">{1}</span>';

        var change = function (value) {
            if (value == 0) {
                return String.format(template, 'red', '未入井');
            }
            else if (value == 1) {
                return String.format(template, 'blue', '已入井');
            }
            else {
                return String.format(template, 'green', '已出井');
            }
        }

        var qtip = function (v, p) {//单元格提示
            //v : value , p : cell
            p.attr = 'ext:qtitle="" ext:qtip="' + v + '"';
            return v;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" CleanResourceUrl="false" Locale="zh-CN" />
    <ext:Store ID="Store1" runat="server">
        <Reader>
            <ext:ArrayReader>
                <Fields>
                    <ext:RecordField Name="Deptname" Mapping="Deptname" />
                    <ext:RecordField Name="Card" Mapping="Card" />
                    <ext:RecordField Name="Name" Mapping="Name" />
                    <ext:RecordField Name="InTime" Mapping="InTime" />
                    <ext:RecordField Name="OutTime" Mapping="OutTime" />
                    <ext:RecordField Name="Status" Mapping="Status" Type="Int" />
                    <ext:RecordField Name="Detail" Mapping="Detail" />
                </Fields>
            </ext:ArrayReader>
        </Reader>
    </ext:Store>

    <ext:Store ID="UnitStore" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="Deptnumber">
                <Fields>
                    <ext:RecordField Name="Deptnumber" />
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
                        ID="GridPanel1" 
                        runat="server"
                        StoreID="Store1"
                        StripeRows="true"
                        Title="人员定位信息查询" 
                        AutoScroll="true"
                        Icon="UserStar"
                        >
                        <TopBar>
                            <ext:Toolbar runat="server" ID="tb1">
                                <Items>
                                    <ext:Label ID="Label4" runat="server" Text="单位：" />
                                    <ext:ComboBox 
                                        ID="cbbUnit" 
                                        runat="server"
                                        DisplayField="Deptname" 
                                        ValueField="Deptnumber"
                                        StoreID="UnitStore"
                                        Width="200"
                                        Editable="false"
                                        />
                                    <ext:ToolbarSeparator />
                                    <ext:Button ID="btnSearch" runat="server" Text="查询FTP" Icon="Zoom">
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.LoadFTPData();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button ID="Button1" runat="server" Text="查询本地" Icon="Zoom">
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.LoadlocalData();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:ToolbarSeparator />
                                    <ext:Button ID="btnShowDetail" runat="server" Text="查看明细" Icon="UserGo" Disabled="true" >
                                        <Listeners>
                                            <Click Handler="Ext.Msg.alert(GridPanel1.getSelectionModel().getSelected().data.Name+'--走动信息', GridPanel1.getSelectionModel().getSelected().data.Detail);" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <ColumnModel ID="ColumnModel1" runat="server">
		                    <Columns>
                                <ext:Column Header="单位" Width="150" DataIndex="Deptname" />
                                <ext:Column Header="姓名" Width="65" DataIndex="Name" />
                                <ext:Column Header="识别卡号" Width="175" DataIndex="Card" />
                                <ext:Column Header="下井时间" Width="130" DataIndex="InTime" />
                                <ext:Column Header="上井时间" Width="130" DataIndex="OutTime" />
                                <ext:Column Header="当前状态" Width="120" DataIndex="Status">
                                     <Renderer Fn="change" />
                                </ext:Column>
                                <%--<ext:Column Header="明细" Width="65" DataIndex="Detail">
                                    <Renderer Fn="qtip" />
                                </ext:Column>--%>
		                    </Columns>
                        </ColumnModel>
                         <SelectionModel>
                            <ext:RowSelectionModel runat="server" ID="RowSelectionModel1" SingleSelect="true" />
                        </SelectionModel>
                        <AjaxEvents>
                            <Click OnEvent="RowClick"></Click>
                        </AjaxEvents>
                        <LoadMask ShowMask="true" Msg="数据加载中..." />
                     </ext:GridPanel>
                </Center>
            </ext:BorderLayout>
        </Body>
    </ext:ViewPort>
    </form>
</body>
</html>
