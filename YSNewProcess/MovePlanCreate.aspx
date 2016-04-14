<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MovePlanCreate.aspx.cs" Inherits="YSNewProcess_MovePlanCreate" %>

<%@ Register assembly="Coolite.Ext.Web" namespace="Coolite.Ext.Web" tagprefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
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

        function nodeLoadPlace(node) {
            Coolite.AjaxMethods.NodeLoadPlace(node.id, {
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
                    Ext.Msg.alert('提示', '已添加的地点！');
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
                <ext:JsonReader ReaderID="Placeid">
                    <Fields>
                        <ext:RecordField Name="Placeid" />
                        <ext:RecordField Name="Placename" />  
                        <ext:RecordField Name="Pareasname" />                      
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
                                <ext:Toolbar runat="server" ID="Toolbar2">
                                    <Items>
                                        <ext:Label runat="server" ID="lbl1" Text="时间范围:" />
                                        <ext:DateField runat="server" ID="dfbegin" />
                                        <ext:Label runat="server" ID="lbl2" Text="----" />
                                        <ext:DateField runat="server" ID="dfend" />
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <Body>        
                                <ext:ColumnLayout ID="ColumnLayout2" runat="server" FitHeight="true">
                                    <ext:LayoutColumn ColumnWidth="0.3">
                                        <ext:TreePanel ID="tpPerson" runat="server" Header="false" AutoScroll="true">
                                            <Root>
                                               <ext:TreeNode NodeID="-1" Text="人" />
                                            </Root>
                                            <Listeners>
                                                <BeforeLoad Fn="nodeLoad" />
                                            </Listeners>
                                        </ext:TreePanel>
                                    </ext:LayoutColumn>
                                    <ext:LayoutColumn ColumnWidth="0.3">
                                        <ext:TreePanel ID="tpPlace" runat="server" Header="false" AutoScroll="true">
                                            <Root>
                                               <ext:TreeNode NodeID="-1" Text="地点" />
                                            </Root>
                                            <Listeners>
                                                <BeforeLoad Fn="nodeLoadPlace" />
                                            </Listeners>
                                        </ext:TreePanel>
                                    </ext:LayoutColumn>
                                    <ext:LayoutColumn ColumnWidth="0.1">
                                        <ext:Panel ID="Panel6" runat="server" Border="false" BodyStyle="padding:5px;background-color: transparent;">
                                            <Body>
                                                <ext:Button ID="Button6" runat="server" Icon="ResultsetNext" StyleSpec="margin-bottom:2px;">
                                                    <Listeners>
                                                        <Click Handler="
                                                        var node=#{tpPlace}.getSelectionModel().getSelectedNode();
                                                        try{
                                                        if(node.leaf){
                                                        PersonSelector.add(GridPanel2,
                                                        [new Ext.data.Record({
                                                        Placeid:node.id ,
                                                        Placename:node.text,
                                                        Pareasname:node.attributes.data,
                                                        Zdcs:1
                                                        })]);
                                                        }
                                                        }catch(e){
                                                        ;
                                                        }" />
                                                    </Listeners>
                                                    <ToolTips>
                                                        <ext:ToolTip ID="ToolTip5" runat="server" Title="提示" Html="添加左侧地点结构中选中的地点" />
                                                    </ToolTips>
                                                </ext:Button>
                                                <ext:Button ID="Button7" runat="server" Icon="ResultsetLast" StyleSpec="margin-bottom:2px;" Disabled="true">
                                                    <Listeners>
                                                        <Click Handler="PersonSelector.addAll();" />
                                                    </Listeners>
                                                    <ToolTips>
                                                        <ext:ToolTip ID="ToolTip6" runat="server" Title="提示" Html="全部添加" />
                                                    </ToolTips>
                                                </ext:Button>
                                                <ext:Button ID="btnRemove" runat="server" Icon="ResultsetPrevious" StyleSpec="margin-bottom:2px;">
                                                    <Listeners>
                                                        <Click Handler="PersonSelector.remove( GridPanel2);" />
                                                    </Listeners>
                                                    <ToolTips>
                                                        <ext:ToolTip ID="ToolTip7" runat="server" Title="提示" Html="移除右侧选中地点" />
                                                    </ToolTips>
                                                </ext:Button>
                                                <ext:Button ID="btnRemoveAll" runat="server" Icon="ResultsetFirst" StyleSpec="margin-bottom:2px;">
                                                    <Listeners>
                                                        <Click Handler="PersonSelector.removeAll(GridPanel2);" />
                                                    </Listeners>
                                                    <ToolTips>
                                                        <ext:ToolTip ID="ToolTip8" runat="server" Title="提示" Html="移除右侧全部地点" />
                                                    </ToolTips>
                                                </ext:Button>
                                                <ext:Hidden ID="GridData" runat="server" />
                                                <ext:Hidden ID="selectedDept" runat="server" />
                                            </Body>
                                        </ext:Panel>
                                    </ext:LayoutColumn>
                                    <ext:LayoutColumn ColumnWidth="0.3">
                                        <ext:GridPanel 
                                            runat="server" 
                                            ID="GridPanel2" 
                                            EnableDragDrop="false"
                                            AutoExpandColumn="SelectPer" ClicksToEdit="1"
                                            StoreID="Store2">
                                            <ColumnModel ID="ColumnModel2" runat="server">
                                                <Columns>
                                                    <ext:Column  Header="已选地点" DataIndex="Placename" Sortable="true" Width="90" />  
                                                    <ext:Column ColumnID="SelectPer" Header="区域" DataIndex="Pareasname" /> 
                                                    <ext:Column Header="走动次数" DataIndex="Zdcs" Width="80">
                                                        <Editor>
                                                            <ext:NumberField runat="server" ID="nfJeom" />
                                                        </Editor>
                                                    </ext:Column>                  
                                                </Columns>
                                            </ColumnModel>
                                            <SelectionModel>
                                                <ext:CheckboxSelectionModel runat="server" ID="CheckboxSelectionModel1" />
                                            </SelectionModel>  
                                            <SaveMask ShowMask="true" Msg="正在保存数据，请稍候..." />
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
                                                                            GridData.setValue(node.id);
                                                                            selectedDept.setValue(-1);
                                                                            #{GridPanel2}.submitData();
                                                                        }
                                                                        else{
                                                                            GridData.setValue(-1);
                                                                            selectedDept.setValue(node.id);
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
