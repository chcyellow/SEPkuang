<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SWLearn_object.aspx.cs" Inherits="YSNewProcess_SWLearn_object" %>

<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function refreshTree(tree) {
            tree.el.mask('数据加载中...', 'x-loading-mask');
            Coolite.AjaxMethods.RefreshMenu({
                success: function (result) {
                    var nodes = eval(result);
                    tree.root.ui.remove();
                    tree.initChildren(nodes);
                    tree.root.render();
                    tree.el.unmask();
                },
                failure: function (msg) {
                    tree.el.unmask();
                    Ext.Msg.alert('加载失败', '未能加载数据');
                }

            });
        }

        var template = '<span style="color:{0};">{1}</span>';

        var change = function (value) {
            var color; var vlu;
            if (value == '1') {
                color = 'orange';
                vlu = '启用';
            }
            else if (value == '2') {
                color = 'gray';
                vlu = '作废';
            }
            else {
                color = 'green';
                vlu = '编辑';
            }
            return String.format(template, color, vlu);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN" CleanResourceUrl="false" />
    <ext:Store ID="ItemStore" runat="server" >
        <Reader>
            <ext:JsonReader ReaderID="Lid">
                <Fields>
                    <ext:RecordField Name="Lid" Type="Int" />
                    <ext:RecordField Name="Lname" />
                    <ext:RecordField Name="Intime" Type="Date"  DateFormat="Y-m-dTh:i:s" />
                    <ext:RecordField Name="Nstatus" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>

    <ext:Hidden ID="hdnKindid" runat="server" Text="0"/>

    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                <West Collapsible="true" Split="true">
                    <ext:TreePanel ID="tpkind" runat="server" Title="三违级别" Border="false" Width="230" RootVisible="false">
                        <Root>
                               <ext:TreeNode NodeID="-1" Text="三违级别" />
                            </Root>
                    </ext:TreePanel>
                </West>
                <Center>
                    <ext:GridPanel 
                        ID="gpJoem" 
                        runat="server" 
                        StoreID="ItemStore"
                        StripeRows="true"
                        Title="学习项目"
                        Icon="BrickMagnify"
                        Collapsible="false" AutoExpandColumn="jcontent"
                        >
                        <ColumnModel ID="ColumnModel1" runat="server">
                            <Columns>
                                <ext:Column ColumnID="jcontent" Header="学习项目" DataIndex="Lname" Width="50" />
                                <ext:Column Header="录入日期" DataIndex="Intime" Width="80">
                                    <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" />
                                </ext:Column>
                                <ext:Column Header="状态" DataIndex="Nstatus" Width="80" >
                                    <Renderer Fn="change" />
                                </ext:Column>
                            </Columns>
                        </ColumnModel>
                        <TopBar>
                            <ext:Toolbar ID="Toolbar3" runat="server">
                                <Items>
                                    <ext:ToolbarButton ID="btnJoemNew" runat="server" Icon="Add" Text="新增项目" Disabled="true">
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.JoemInfo('new');" />
                                        </Listeners>
                                    </ext:ToolbarButton>
                                    <ext:ToolbarSeparator />
                                    <ext:ToolbarButton ID="btnJoemUpdate" runat="server" Icon="FolderEdit" Text="修改" Disabled="true">
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.JoemInfo('edit');" />
                                        </Listeners>
                                    </ext:ToolbarButton>
                                    <ext:ToolbarButton ID="btnJoemPublic" runat="server" Icon="Accept" Text="启用" Disabled="true">
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.JoemAction(1);" />
                                        </Listeners>
                                    </ext:ToolbarButton>
                                    <ext:ToolbarButton ID="btnJoemDel" runat="server" Icon="Delete" Text="作废" Disabled="true">
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.JoemAction(0);" />
                                        </Listeners>
                                    </ext:ToolbarButton>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <SelectionModel>
                            <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" runat="server" SingleSelect="true" />
                        </SelectionModel>
                        <AjaxEvents>
                            <Click OnEvent="JoemRowClick">
                            </Click>
                        </AjaxEvents>
                    </ext:GridPanel>
                </Center>
            </ext:BorderLayout>
        </Body>
    </ext:ViewPort>
    </form>
</body>
</html>
