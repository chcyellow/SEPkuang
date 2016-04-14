<%@ Page Language="C#" AutoEventWireup="true" CodeFile="KDSMSSearch.aspx.cs" Inherits="LeaderSearch_KDSMSSearch" %>

<%@ Register assembly="Coolite.Ext.Web" namespace="Coolite.Ext.Web" tagprefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script language="javascript" type="text/javascript">
        var template = '<span style="color:{0};cursor: pointer;">{1}</span>';

        var change = function (value) {
            return String.format(template, 'blue', value);
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
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN" CleanResourceUrl="false" />
    <ext:Store ID="DetailStore" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="Smsid">
                <Fields>
                    <ext:RecordField Name="SmContent" />
                    <ext:RecordField Name="Name" />
                    <ext:RecordField Name="Sendtime" Type="Date" DateFormat="Y-m-dTh:i:s" />
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
                        StoreID="DetailStore"
                        StripeRows="true"
                        Title="短信发送明细"
                        AutoExpandColumn="detail"
                        AutoScroll="true"
                        Icon="PhoneKey"
                        >
                        <TopBar>
                            <ext:Toolbar runat="server" ID="tb1">
                                <Items>
                                    <ext:Label ID="Label1" runat="server" Text="开始日期：" />
                                    <ext:DateField ID="dfBegin" runat="server" />
                                    <ext:Label ID="Label2" runat="server" Text="截止日期：" />
                                    <ext:DateField ID="dfEnd" runat="server" />
                                    <ext:ToolbarSeparator />
                                    <%--<ext:Label ID="Label4" runat="server" Text="单位：" />
                                    <ext:ComboBox 
                                        ID="cbbKQ" 
                                        runat="server"
                                        DisplayField="Deptname" 
                                        ValueField="Deptid"
                                        StoreID="KQStore"
                                        TypeAhead="true" 
                                        Mode="Local"
                                        ForceSelection="true" 
                                        TriggerAction="All"
                                        Width="100"
                                        Editable="false" Disabled="true"
                                        />
                                    <ext:ToolbarSeparator />--%>
                                    <ext:Button ID="btnSearch" runat="server" Text="查询" Icon="Zoom">
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.LoadData();" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <ColumnModel ID="ColumnModel2" runat="server">
                            <Columns>
                                <ext:Column Header="短信内容" ColumnID="detail" DataIndex="SmContent" Width="400">
                                    <Renderer Fn="qtip" />
                                </ext:Column>
                                <ext:Column Header="发送时间" DataIndex="Sendtime" Width="120">
                                    <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d h:i:s')" />
                                </ext:Column>
                                <ext:Column Header="收信人" DataIndex="Name" Width="50" />
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel runat="server" ID="RowSelectionModel1" SingleSelect="true" />
                        </SelectionModel>
                        <BottomBar>
                            <ext:PagingToolbar runat="server" ID="pageToolBar"></ext:PagingToolbar>
                        </BottomBar>
                        <LoadMask ShowMask="true" Msg="数据加载中..." />
                     </ext:GridPanel>
                </Center>
            </ext:BorderLayout>
        </Body>
    </ext:ViewPort>
    </form>
</body>
</html>
