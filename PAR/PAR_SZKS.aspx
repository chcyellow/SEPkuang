<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PAR_SZKS.aspx.cs" Inherits="PAR_PAR_SZKS" %>

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
    <ext:Hidden runat="server" ID="hdnPostid" />
    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                <West Collapsible="true" Split="true">
                    <ext:TreePanel ID="tpZY" runat="server" Width="200" Title="专业" RootVisible="false" AutoScroll="true" >
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
                        Title="手指口述内容">            
                        <TopBar>
                            <ext:Toolbar ID="Toolbar1" runat="server">
                                <Items>
                                    <ext:ToolbarButton ID="btnEdit" runat="server" Text="编辑" Icon="Pencil" Disabled="true">
                                        <Listeners>
                                            <Click Handler="el.setDisabled(true);#{btnSave}.setDisabled(false);#{PanelEditor}.startEdit(#{Panel1}.getBody());" />
                                        </Listeners>
                                    </ext:ToolbarButton>
                                    <ext:ToolbarButton ID="btnSave" runat="server" Text="保存" Icon="Disk" Disabled="true">
                                        <Listeners>
                                            <Click Handler="el.setDisabled(true);#{btnEdit}.setDisabled(false);#{PanelEditor}.completeEdit();Coolite.AjaxMethods.DataSave();" />
                                        </Listeners>
                                    </ext:ToolbarButton>    
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <Listeners>
                            <Deactivate Handler="#{btnEdit}.setDisabled(true);#{btnSave}.setDisabled(true);#{PanelEditor}.completeEdit();" />
                        </Listeners>
                    </ext:Panel>
                            
                </Center>
            </ext:BorderLayout>
        </Body>
    </ext:ViewPort>
    <ext:Editor 
        ID="PanelEditor" 
        runat="server"
        AutoSize="Fit"
        Shadow="None">
        <Field>
            <ext:HtmlEditor ID="HtmlEditor1" runat="server" />
        </Field>
    </ext:Editor>
    </form>
</body>
</html>
