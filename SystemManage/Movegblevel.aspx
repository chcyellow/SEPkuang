<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Movegblevel.aspx.cs" Inherits="SystemManage_Movegblevel" %>

<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
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
                    Ext.Msg.alert('提示', '已添加的职务！');
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
            <ext:JsonReader ReaderID="Name">
                <Fields>
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Window 
        ID="PersonChooserDialog" 
        runat="server"
        Icon="User"
        Title="职务类型设置"
        Height="553"
        Width="700"
        Modal="true" ShowOnLoad="true"        
        BodyStyle="padding:5px;"
        BodyBorder="false"
        >
        <TopBar>
            <ext:Toolbar runat="server" ID="Toolbar2">
                <Items>
                    <ext:Label runat="server" ID="lbl1" Text="职务类型:" />
                    <ext:ComboBox 
                        ID="cbbStatus"
                        runat="server" 
                        AllowBlank="false" Editable="false"
                        SelectedIndex="0">
                        <Items>
                            <ext:ListItem Text="矿领导" Value="矿领导" />
                            <ext:ListItem Text="中层领导" Value="中层领导" />
                        </Items>
                    </ext:ComboBox>
                    <ext:ToolbarSeparator />
                    <ext:Button runat="server" ID="btnSearch" Icon="Zoom" Text="查询">
                        <Listeners>
                            <Click Handler="Coolite.AjaxMethods.StoreBind();" />
                        </Listeners>
                    </ext:Button>
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
                                                    Name:node.text
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
                                <ext:Column ColumnID="SelectPer"  Header="选中职务" DataIndex="Name" Sortable="true" />                  
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
                    <Click Handler="#{GridPanel3}.submitData();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    </form>
</body>
</html>
