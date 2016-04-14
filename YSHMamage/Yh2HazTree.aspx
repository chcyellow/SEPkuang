<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Yh2HazTree.aspx.cs" Inherits="YSHMamage_Yh2HazTree" %>

<%@ Register assembly="Coolite.Ext.Web" namespace="Coolite.Ext.Web" tagprefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">

        function nodeLoadYh(node) {
            Coolite.AjaxMethods.NodeLoadYh(node.id,node.attributes.data, {
                success: function (result) {
                    var data = eval("(" + result + ")");
                    node.loadNodes(data);
                },

                failure: function (errorMsg) {
                    Ext.Msg.alert('Failure', errorMsg);
                }
            });
        }
        function nodeLoadHaz(node) {
            Coolite.AjaxMethods.NodeLoadHaz(node.id, {
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
                    if (destination.id == 'GridPanel2') {
                        for (var i = 0; i < records.length; i++) {
                            destination.addRecord(records[i].data);
                        }
                    }
                }
                catch (e) {
                    Ext.Msg.alert('提示', '已添加的信息！');
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

      <ext:Store runat="server" ID="Store2" OnSubmitData="SubmitData">
            <Reader>
                <ext:JsonReader ReaderID="Yhid">
                    <Fields>
                        <ext:RecordField Name="Yhcontent" />
                        <ext:RecordField Name="Hazardsid" />  
                        <ext:RecordField Name="HContent" />                      
                    </Fields>
                </ext:JsonReader>
            </Reader>         
        </ext:Store>

     <ext:Window 
        ID="PersonChooserDialog" 
        runat="server"
        Icon="Table"
        Title="隐患与危险源信息对应"
        Height="553"
        Width="750"
        Modal="true"        
        BodyStyle="padding:5px;"
        BodyBorder="false"
        >
        <Body>        
            <ext:ColumnLayout ID="ColumnLayout2" runat="server" FitHeight="true">
                <ext:LayoutColumn ColumnWidth="0.3">
                    <ext:TreePanel ID="tpYh" runat="server" Header="false" AutoScroll="true">
                        <Root>
                           <ext:TreeNode NodeID="-1" Text="隐患" />
                        </Root>
                        <Listeners>
                            <BeforeLoad Fn="nodeLoadYh" />
                            <DblClick Handler="
                            var nodeYh=#{tpYh}.getSelectionModel().getSelectedNode();
                            var nodeHaz=#{tpHaz}.getSelectionModel().getSelectedNode();
                                try{
                                if(nodeYh.leaf){
                                    if(nodeHaz.leaf){
                                        PersonSelector.add(GridPanel2,
                                        [new Ext.data.Record({
                                        Yhid:nodeYh.id ,
                                        Yhcontent:nodeYh.text,
                                        Hazardsid:nodeHaz.id,
                                        HContent:nodeHaz.text
                                        })]);
                                    }
                                  }
                                }catch(e){
                                ;
                                }" />
                        </Listeners>
                    </ext:TreePanel>
                </ext:LayoutColumn>
                <ext:LayoutColumn ColumnWidth="0.4">
                    <ext:GridPanel 
                        runat="server" 
                        ID="GridPanel2" 
                        EnableDragDrop="false"
                        AutoExpandColumn="SelectPer"
                        StoreID="Store2">
                        <Listeners>
                        </Listeners>
                        <ColumnModel ID="ColumnModel2" runat="server">
                            <Columns>
                                <ext:Column  Header="隐患信息" DataIndex="Yhcontent" Sortable="true" Width="150" />  
                                <ext:Column ColumnID="SelectPer" Header="危险源信息" DataIndex="HContent" />                
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:CheckboxSelectionModel runat="server" ID="CheckboxSelectionModel1" />
                        </SelectionModel>  
                        
                        <SaveMask ShowMask="true" />
                    </ext:GridPanel>
                </ext:LayoutColumn>
                <ext:LayoutColumn ColumnWidth="0.3">
                    <ext:TreePanel ID="tpHaz" runat="server" Header="false" AutoScroll="true">
                        <Root>
                           <ext:TreeNode NodeID="-1" Text="危险源" />
                        </Root>
                        <Listeners>
                            <BeforeLoad Fn="nodeLoadHaz" />
                            <DblClick Handler="
                            var nodeYh=#{tpYh}.getSelectionModel().getSelectedNode();
                            var nodeHaz=#{tpHaz}.getSelectionModel().getSelectedNode();
                                try{
                                if(nodeYh.leaf){
                                    if(nodeHaz.leaf){
                                        PersonSelector.add(GridPanel2,
                                        [new Ext.data.Record({
                                        Yhid:nodeYh.id ,
                                        Yhcontent:nodeYh.text,
                                        Hazardsid:nodeHaz.id,
                                        HContent:nodeHaz.text
                                        })]);
                                    }
                                  }
                                }catch(e){
                                ;
                                }" />
                        </Listeners>
                    </ext:TreePanel>
                </ext:LayoutColumn>
                
            </ext:ColumnLayout>                
        </Body>

        <Buttons>
            <ext:Button runat="server" ID="OkBtn" Text="确 定" Icon="Disk">
                <Listeners>
                    <Click Handler="#{GridPanel2}.submitData();" />
                </Listeners>
            </ext:Button>
            <ext:Button runat="server" ID="CancelBtn" Text="删 除" Icon="Cancel">
                <Listeners>
                    <Click Handler="PersonSelector.remove( GridPanel2);" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    </form>
</body>
</html>
