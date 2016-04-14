<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PositioningSearch.aspx.cs" Inherits="Positioning_PositioningSearch" %>

<%@ Register assembly="Coolite.Ext.Web" namespace="Coolite.Ext.Web" tagprefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN" CleanResourceUrl="false" />
    <ext:Store ID="KQStore" runat="server" OnRefreshData="KQRefresh">
        <Reader>
            <ext:JsonReader ReaderID="Deptnumber">
                <Fields>
                    <ext:RecordField Name="Deptnumber" />
                    <ext:RecordField Name="Deptname" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="MainDeptStore" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="Deptnumber">
                <Fields>
                    <ext:RecordField Name="Deptnumber" />
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
                    <ext:Panel 
                        runat="server"
                        ID="panel1"
                        Title="员工下井情况查询"
                        Icon="UserEarth">
                        <TopBar>
                            <ext:Toolbar runat="server" ID="Toolbar1">
                                <Items>
                                    <ext:Label ID="Label3" runat="server" Text="单位：" />
                                     <ext:ComboBox 
                                        ID="cbbMianDept" 
                                        runat="server"
                                        DisplayField="Deptname" 
                                        ValueField="Deptnumber"
                                        StoreID="MainDeptStore"
                                        Width="100"
                                        Editable="false">
                                        <Listeners>
                                            <Select Handler="#{cbbKQ}.clearValue();#{cbbPerson}.clearValue(); #{KQStore}.reload();" />
                                        </Listeners>
                                    </ext:ComboBox>
                                    <ext:Label ID="Label4" runat="server" Text="部门：" />
                                    <ext:ComboBox 
                                        ID="cbbKQ" 
                                        runat="server"
                                        DisplayField="Deptname" 
                                        ValueField="Deptnumber"
                                        StoreID="KQStore"
                                        Width="100"
                                        Editable="false">
                                        <Listeners>
                                            <Select Handler="#{cbbPerson}.clearValue(); #{PersStore}.reload();" />
                                        </Listeners>
                                    </ext:ComboBox>
                                    <ext:Label ID="Label1" runat="server" Text="人员：" />
                                    <ext:ComboBox 
                                            ID="cbbPerson" 
                                            runat="server"
                                            DisplayField="Name" 
                                            ValueField="Personnumber"
                                            Width="100px"
                                            StoreID="PersStore"
                                            Editable="false"
                                            >
                                        </ext:ComboBox>
                                    <ext:ToolbarSeparator />
                                    <ext:Button ID="Button1" runat="server" Text="查询" Icon="Zoom">
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.LoadData();" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <LoadMask ShowMask="true" Msg="数据加载中..." />
                    </ext:Panel>
                </Center>
            </ext:BorderLayout>
        </Body>
    </ext:ViewPort>
    </form>
</body>
</html>
