<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PersonW_batch.aspx.cs" Inherits="BaseManage_PersonW_batch" %>

<%@ Register assembly="Coolite.Ext.Web" namespace="Coolite.Ext.Web" tagprefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN" CleanResourceUrl="false" />
    <ext:Store ID="Store1" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="工号">
                <Fields>
                    <ext:RecordField Name="工号" />
                    <ext:RecordField Name="姓名" />
                    <ext:RecordField Name="性别" />
                    <ext:RecordField Name="电话" />
                    <ext:RecordField Name="职务" />
                    <ext:RecordField Name="部门" />
                    <ext:RecordField Name="灯号" />
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
                        Title="批量上传" 
                        AutoScroll="true"
                        Icon="PhoneKey"
                        >
                        <TopBar>
                            <ext:Toolbar runat="server" ID="tb1">
                                <Items>
                                    <ext:Button ID="btnUpLoad" runat="server" Text="上传数据" Icon="Add" >
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.UpLoadFile();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:ToolbarSeparator />
                                    <ext:FileUploadField runat="server" ID="fufExcel" ButtonText="选择文件..." Icon="PageExcel" ButtonOnly="True" Width="75px" />
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <ColumnModel ID="ColumnModel1" runat="server">
		                    <Columns>
                                <ext:Column Header="工号" DataIndex="工号" />
                                <ext:Column Header="姓名" DataIndex="姓名" />
                                <ext:Column Header="性别" DataIndex="性别" />
                                <ext:Column Header="电话" DataIndex="电话" />
                                <ext:Column Header="职务" DataIndex="职务" />
                                <ext:Column Header="部门" DataIndex="部门" />
                                <ext:Column Header="灯号" DataIndex="灯号" />
		                    </Columns>
                        </ColumnModel>
                        <LoadMask ShowMask="true" Msg="数据加载中..." />
                     </ext:GridPanel>
                </Center>
            </ext:BorderLayout>
        </Body>
    </ext:ViewPort>
    </form>
</body>
</html>
