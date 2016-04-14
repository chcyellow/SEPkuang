<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Par_SaftyConfirm.aspx.cs" Inherits="PAR_Par_SaftyConfirm" %>

<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function nodeLoad(node) {
            Coolite.AjaxMethods.NodeLoad(node.id, {
                success: function(result) {
                    var data = eval("(" + result + ")");
                    node.loadNodes(data);
                },

                failure: function(errorMsg) {
                    Ext.Msg.alert('Failure', errorMsg);
                }
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN" CleanResourceUrl="false">
    </ext:ScriptManager>
    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                <West Collapsible="true" Split="true">
                    <ext:TreePanel ID="tpZY" runat="server" Width="200" Title="辨识单元" RootVisible="false" AutoScroll="true" >
                        <Listeners>
                            <BeforeLoad Fn="nodeLoad" />
                        </Listeners>
                    </ext:TreePanel>
                </West>
                <Center>
                    <ext:Panel 
                        ID="Panel1" 
                        runat="server" 
                        Width="610" 
                        Height="300" 
                        Html="" 
                        BodyStyle="padding:6px;" AutoScroll="true"
                        Title="安全确认内容">
                    </ext:Panel>
                </Center>
            </ext:BorderLayout>
        </Body>
    </ext:ViewPort>
    </form>
</body>
</html>
