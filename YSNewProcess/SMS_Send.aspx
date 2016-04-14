<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SMS_Send.aspx.cs" Inherits="YSNewProcess_SMS_Send" %>

<%@ Register assembly="Coolite.Ext.Web" namespace="Coolite.Ext.Web" tagprefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        var qtip = function (v, p) {//单元格提示
            //v : value , p : cell
            p.attr = 'ext:qtitle="" ext:qtip="' + v + '"';
            return v;
        }

        function nodeLoad(node) {
            Coolite.AjaxMethods.NodeLoad(node.id, {
                success: function (result) {
                    var data = eval("(" + result + ")");
                    node.loadNodes(data);
                },

                failure: function (errorMsg) {
                    Ext.Msg.alert('Failure', errorMsg);
                }
            });
        }

        var PersonSelector = {
            add: function (destination, records) {
                PersonChooserDialog.body.mask('数据处理中...');
                try {
                    if (destination.id == 'GridPanel3') {
                        for (var i = 0; i < records.length; i++) {
                            destination.addRecord(records[i].data);
                        }
                    }
                }
                catch (e) {
                    Ext.Msg.alert('提示', '已添加的人员！');
                }
                PersonChooserDialog.body.unmask();
            },

            remove: function (source) {
                source.deleteSelected();
            },

            removeAll: function (source) {
                source.store.removeAll();
            }
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN" CleanResourceUrl="false" />
    <ext:Store ID="SelectedStore" runat="server" OnSubmitData="SubmitData">
        <Reader>
            <ext:JsonReader ReaderID="Personnumber">
                <Fields>
                    <ext:RecordField Name="Personnumber" />
                    <ext:RecordField Name="Name" />
                    <ext:RecordField Name="Deptname" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:BorderLayout ID="BorderLayout1" runat="server">
                    <Center>
                        <ext:Panel 
                            ID="PersonChooserDialog" 
                            runat="server"
                            Icon="User"
                            Header="false"
                            Modal="true" ShowOnLoad="false"        
                            BodyStyle="padding:5px;"
                            BodyBorder="false"
                            >
                            <TopBar>
                                <ext:Toolbar runat="server" ID="Toolbar2" Height="50">
                                    <Items>
                                        <ext:Label runat="server" ID="lbl1" Text="短信内容:" />
                                        <ext:TextArea runat="server" ID="tfMSG" Height="45" Width ="300" />
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <Body>        
                                <ext:ColumnLayout ID="ColumnLayout2" runat="server" FitHeight="true">
                                    <ext:LayoutColumn ColumnWidth="0.5">
                                        <ext:TreePanel ID="tpPerson" runat="server" Header="false" AutoScroll="true">
                                            <Root>
                                               <ext:TreeNode NodeID="-1" Text="人" />
                                            </Root>
                                            <Listeners>
                                                <BeforeLoad Fn="nodeLoad" />
                                            </Listeners>
                                        </ext:TreePanel>
                                    </ext:LayoutColumn>
                                    <ext:LayoutColumn>
                                        <ext:Panel ID="Panel4" runat="server" Width="35" BodyStyle="background-color: transparent;" Border="false">
                                            <Body>
                                                <ext:AnchorLayout ID="AnchorLayout2" runat="server">
                                                    <ext:Anchor Vertical="40%">
                                                        <ext:Panel ID="Panel5" runat="server" Border="false" BodyStyle="background-color: transparent;" />
                                                    </ext:Anchor>
                                                    <ext:Anchor>
                                                        <ext:Panel ID="Panel6" runat="server" Border="false" BodyStyle="padding:5px;background-color: transparent;">
                                                            <Body>
                                                                <ext:Button ID="Button1" runat="server" Icon="ResultsetNext" StyleSpec="margin-bottom:2px;">
                                                                    <Listeners>
                                                                        <Click Handler="
                                                                        var node=#{tpPerson}.getSelectionModel().getSelectedNode();
                                                                        try{
                                                                        if(node.leaf){
                                                                        PersonSelector.add(GridPanel3,
                                                                        [new Ext.data.Record({
                                                                        Personnumber:node.id ,
                                                                        Name:node.text,
                                                                        Deptname:node.attributes.data
                                                                        })]);
                                                                        }
                                                                        }catch(e){
                                                                        ;
                                                                        }" />
                                                                    </Listeners>
                                                                    <ToolTips>
                                                                        <ext:ToolTip ID="ToolTip1" runat="server" Title="提示" Html="添加左侧人员结构中选中的人" />
                                                                    </ToolTips>
                                                                </ext:Button>
                                                                <ext:Button ID="Button5" runat="server" Icon="ResultsetLast" StyleSpec="margin-bottom:2px;" Disabled="true">
                                                                    <Listeners>
                                                                        <Click Handler="PersonSelector.addAll();" />
                                                                    </Listeners>
                                                                    <ToolTips>
                                                                        <ext:ToolTip ID="ToolTip2" runat="server" Title="提示" Html="全部添加" />
                                                                    </ToolTips>
                                                                </ext:Button>
                                                                <ext:Button ID="btnRemove" runat="server" Icon="ResultsetPrevious" StyleSpec="margin-bottom:2px;">
                                                                    <Listeners>
                                                                        <Click Handler="PersonSelector.remove( GridPanel3);" />
                                                                    </Listeners>
                                                                    <ToolTips>
                                                                        <ext:ToolTip ID="ToolTip3" runat="server" Title="提示" Html="移除右侧选中人员" />
                                                                    </ToolTips>
                                                                </ext:Button>
                                                                <ext:Button ID="btnRemoveAll" runat="server" Icon="ResultsetFirst" StyleSpec="margin-bottom:2px;">
                                                                    <Listeners>
                                                                        <Click Handler="PersonSelector.removeAll(GridPanel3);" />
                                                                    </Listeners>
                                                                    <ToolTips>
                                                                        <ext:ToolTip ID="ToolTip4" runat="server" Title="提示" Html="移除右侧全部人员" />
                                                                    </ToolTips>
                                                                </ext:Button>
                                                            </Body>
                                                        </ext:Panel>
                                                    </ext:Anchor>
                                                </ext:AnchorLayout>
                                            </Body>
                                        </ext:Panel>
                                    </ext:LayoutColumn>
                                    <ext:LayoutColumn ColumnWidth="0.5">
                                        <ext:GridPanel 
                                            runat="server" 
                                            ID="GridPanel3" 
                                            EnableDragDrop="false"
                                            AutoExpandColumn="SelectPer" 
                                            StoreID="SelectedStore">
                                            <Listeners>
                                            </Listeners>
                                            <ColumnModel ID="ColumnModel4" runat="server">
                                                <Columns>
                                                    <ext:Column  Header="选中人员" DataIndex="Name" Sortable="true" Width="90" />  
                                                    <ext:Column ColumnID="SelectPer" Header="单位" DataIndex="Deptname" />                   
                                                </Columns>
                                            </ColumnModel>
                                            <SelectionModel>
                                                <ext:CheckboxSelectionModel runat="server" ID="CheckboxSelectionModel3" />
                                                <%--<ext:RowSelectionModel ID="RowSelectionModel2" runat="server" />--%>
                                            </SelectionModel>  
                                            <SaveMask ShowMask="true" />
                                        </ext:GridPanel>
                                    </ext:LayoutColumn>
                                </ext:ColumnLayout>                
                            </Body>
                            <Buttons>
                                <ext:Button runat="server" ID="OkBtn" Text="确 定" Icon="Disk">
                                    <Listeners>
                                        <Click Handler="var node=#{tpPerson}.getSelectionModel().getSelectedNode();
                                                                        try{
                                                                        if(node.leaf){
                                                                            #{GridPanel2}.submitData();
                                                                        }
                                                                        }catch(e){
                                                                        ;
                                                                        }" /><%-- Coolite.AjaxMethods.DataCheck();" />--%>
                                    </Listeners>
                                </ext:Button>
                            </Buttons>
                        </ext:Panel>
                    </Center>
                </ext:BorderLayout>
            </Body>
        </ext:ViewPort>
    </form>
</body>
</html>
