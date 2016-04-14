<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MainFrame.aspx.cs" Inherits="mainframes" %>

<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>淮北矿业集团安全生产体系支撑平台</title>
    <script type="text/javascript" src="Scripts/Main.js"></script>
    <style type="text/css">
    .logo{
        background: #4c91d6 url(Images/backgroundheader.jpg) no-repeat;
    }
    .basic{
        background: #c5defb;
    }
    .west-panel .x-layout-collapsed-west{
            background: url(Images/collapsed-west.GIF) no-repeat center;
        }
    .windowEl
    {
        position:inherit !important;
        bottom:2px;
        right:2px;
    }
    
    </style>
    <script language="javascript" type="text/javascript">
        function openwin(x) {
            var str = x;
            window.open(str, "newwindow", "height=280, width=400,left=350,top=200, toolbar=no, menubar=no, scrollbars=no, resizable=no, location=no, status=no")
            return false;
        }

        function loadnewpage(title, url) {
            Coolite.AjaxMethods.Win1load(title, url);
        }
    </script> 
    <ext:TokenScript ID="TokenScript1" runat="server">
        <script type="text/javascript">
            var reload = function (msg, url) {
                #{pnlFrame}.load(url);
            }
        </script>
    </ext:TokenScript>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN" CleanResourceUrl="false">
    </ext:ScriptManager>
    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                <North Collapsible="True" Split="True">
                    <ext:Panel ID="Panel1" runat="server" Height="98" BaseCls="logo">
                        <BottomBar>
                            <ext:Toolbar ID="basicstatusbar" runat="server" Cls="basic">
                                <Items>
                                    <ext:ToolbarSeparator />
                                    <ext:Label ID="lblWelcome" runat="server" Text="欢迎您：" Icon="User" />
                                    <ext:Label ID="lblLoginName" runat="server" Text="" StyleSpec="color:red;" />
                                    <ext:ToolbarSeparator />
                                    <ext:ToolbarSeparator />
                                    <ext:Label ID="lblR" runat="server" Text="" Icon="UserSuitBlack" />
                                    <ext:ComboBox ID="cboRoleName" runat="server" Editable="false" Width="100">
                                        <Listeners>
                                            <Select Handler="Coolite.AjaxMethods.PageChange();" />
                                        </Listeners>
                                    </ext:ComboBox>
                                    <ext:Label ID="lblRoleName" runat="server" Text="" Icon="Group" />
                                    <ext:Label ID="lblRole" runat="server" Text="" StyleSpec="color:red;" />
                                    <ext:Label ID="Label1" runat="server" Text="当前服务器：" />
                                    <ext:Label ID="lblServerIP" runat="server" Text="" StyleSpec="color:red;" />
                                    <ext:ToolbarSpacer Width="150" />
                                    <ext:Label ID="lblClock" runat="server" />
                                    <ext:ToolbarFill />
                                    <ext:Button ID="Button1" runat="server" Text="首页" Icon="House">
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.homeload();#{winNotice}.show();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:ToolbarSeparator />
                                    <ext:Button ID="btnNotice" runat="server" Text="公告" Icon="Mail">
                                        <Listeners>
                                            <Click Handler="showMessage()" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:ToolbarSeparator />
                                    <ext:Button ID="btnRefresh" runat="server" Text="刷新" Icon="ArrowRefresh">
                                        <Listeners>
                                            <Click Handler="#{pnlFrame}.reload();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:ToolbarSeparator />
                                    <ext:Button ID="btnRefresh1" runat="server" Text="帮助" Icon="Help" />
                                    <ext:ToolbarSeparator />
                                    <ext:Button ID="btnRefresh2" runat="server" Text="退出" Icon="Cancel">
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.Cancel();" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                                <Listeners>
                                    <Render Fn="showClock" />
                                </Listeners>
                            </ext:Toolbar>
                        </BottomBar>
                    </ext:Panel>
                </North>
                <West Collapsible="true" Split="true">
                    <ext:Panel ID="pnlTree" runat="server" Title="系统导航" Width="150" CtCls="west-panel">
                    </ext:Panel>
                </West>
                <Center>
                    <ext:Panel ID="Panel7" runat="server">
                        <Body>
                            <ext:FitLayout ID="FitLayout2" runat="server">
                                <ext:Panel ID="pnlFrame" runat="server" Height="300" Header="false" Frame="true">
                                    <AutoLoad Url="main.aspx" Mode="IFrame" ShowMask="true" MaskMsg="正在加载数据，请稍候..." />
                                </ext:Panel>
                            </ext:FitLayout>
                        </Body>
                    </ext:Panel>
                </Center>
            </ext:BorderLayout>
        </Body>
    </ext:ViewPort>
    <ext:Window 
        ID="winNotice" runat="server" 
        Collapsible="false" Icon="Mail" 
        Title="公告" CloseAction="Hide" 
        Width="330" Height="200" 
        Resizable="false" Plain="true" 
        AnimCollapse="true" ButtonAlign="Center"
        >
        <Body>
            <ext:Panel runat="server" ID="NoticeDetail" Title="" Height="140" AutoScroll="true" BodyStyle="background-color:#CAD9EC;">
            </ext:Panel>
        </Body>
        <TopBar>
            <ext:Toolbar runat="server">
                <Items>
                    <ext:ToolbarFill />
                    <ext:Button ID="btn_more" runat="server" Text="更多>>">
                        <Listeners>
                            <Click Handler="#{pnlFrame}.load('SystemNotice/NoticeManage.aspx');#{winNotice}.hide();" />
                        </Listeners>
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </TopBar>
    </ext:Window>
    
    <ext:Window ID="Window1"  ShowOnLoad="false" 
    BodyStyle="padding:0pc" runat="server"   BodyBorder="false" CloseAction="Hide"
    Collapsible="false" Frame="true" Modal="true" Width="880"  Height="700" AutoScroll="true" Resizable="false" X="300" Y="-2"
     Title="添加人员">
        <AutoLoad Mode="IFrame" Url="SafeCheckSet/AddSmsReceiver.aspx" />
         <LoadMask ShowMask="true" Msg="数据加载中...." />
         <Listeners>
            <%--<BeforeShow Fn="function(el) { el.setHeight(Ext.getBody().getViewSize().height-20);el.setWidth(Ext.getBody().getViewSize().width-2); }" />--%>
            <BeforeHide Handler="#{pnlFrame}.reload();"/>
         </Listeners>
    </ext:Window>
    </form>
</body>
</html>
