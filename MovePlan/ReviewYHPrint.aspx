<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReviewYHPrint.aspx.cs" Inherits="MovePlan_ReviewYHPrint" %>

<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        var CountrySelector = {
            add: function(source, destination) {
                source = source || GridPanel1;
                destination = destination || GridPanel2;
                if (source.hasSelection()) {
                    destination.store.add(source.selModel.getSelections());
                    source.deleteSelected();
                }
            },
            addAll: function(source, destination) {
                source = source || GridPanel1;
                destination = destination || GridPanel2;
                destination.store.add(source.store.getRange());
                source.store.removeAll();
            },
            addByName: function(name) {
                if (!Ext.isEmpty(name)) {
                    var result = Store1.getById(name);
                    if (!Ext.isEmpty(result)) {
                        GridPanel2.store.add(result);
                        GridPanel1.store.remove(result);
                    }
                }
            },
            addByNames: function(name) {
                for (var i = 0; i < name.length; i++) {
                    this.addByName(name[i]);
                }
            },
            remove: function(source, destination) {
                this.add(destination, source);
            },
            removeAll: function(source, destination) {
                this.addAll(destination, source);
            }
        };

        var saveData = function() {
            GridData.setValue(Ext.encode(GridPanel3.getRowsValues(false)));
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN" CleanResourceUrl="false" />
    <ext:Store runat="server" ID="Store1">
            <SortInfo Field="Placename" Direction="ASC" />            
            <Reader>
                <ext:JsonReader ReaderID="Placeid">
                    <Fields>
                        <ext:RecordField Name="Placeid" Type="Int" />
                        <ext:RecordField Name="Placename" />  
                        <ext:RecordField Name="py" /> 
                        <ext:RecordField Name="Pareasname" />                      
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        
        <ext:Store runat="server" ID="Store2" OnSubmitData="SubmitData">
            <Reader>
                <ext:JsonReader ReaderID="Placeid">
                    <Fields>
                        <ext:RecordField Name="Placeid" />
                        <ext:RecordField Name="Placename" />  
                        <ext:RecordField Name="py" /> 
                        <ext:RecordField Name="Pareasname" />                      
                    </Fields>
                </ext:JsonReader>
            </Reader>         
        </ext:Store>
        
        <ext:Store ID="YHStore" runat="server">
        <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="DeptName"/> 
                        <ext:RecordField Name="YHContent" />
                        <ext:RecordField Name="PlaceName" />
                        <ext:RecordField Name="YHLevel" />
                        <ext:RecordField Name="YHType" />
                        <ext:RecordField Name="Name" />
                        <ext:RecordField Name="BanCi" />                       
                        <ext:RecordField Name="PCTime" Type="Date" DateFormat="Y-m-dTh:i:s" />   
                      </Fields> 
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        
        <ext:Window 
            ID="Window1" 
            runat="server" 
            ShowOnLoad="false" Modal="true"
            Height="553"
            Width="700"
            Icon="Plugin"
            Title="走动地点选择"
            BodyStyle="padding:5px;"
            BodyBorder="false">
            <TopBar>
                <ext:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <ext:Button ID="Button7" runat="server" Text="加载待走动地点" Icon="Add">
                            <Listeners>
                                <Click Handler="Coolite.AjaxMethods.LoadPlanPlace();" />
                            </Listeners>
                        </ext:Button>
                    </Items>
                </ext:Toolbar>
            </TopBar>
            <Body>
                <ext:ColumnLayout ID="ColumnLayout1" runat="server" FitHeight="true">
                    <ext:LayoutColumn ColumnWidth="0.5">
                       <ext:GridPanel 
                            runat="server" 
                            ID="GridPanel1" Title="地点列表"
                            EnableDragDrop="true"
                            AutoExpandColumn="Country"
                            StoreID="Store1">
                            <ColumnModel ID="ColumnModel1" runat="server">
	                            <Columns>
                                    <ext:Column ColumnID="Country" Header="地点名称" DataIndex="Placename" Sortable="true" /> 
                                    <ext:Column Header="片区" DataIndex="Pareasname" Sortable="true" />                  
	                            </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" runat="server" />
                            </SelectionModel> 
                            <Plugins>
                                <ext:GridFilters ID="GridFilters1" runat="server" Local="true">
                                    <Filters>
                                        <ext:StringFilter DataIndex="Name" />
                                    </Filters>
                                </ext:GridFilters>
                            </Plugins>
                        </ext:GridPanel>
                    </ext:LayoutColumn>
                    <ext:LayoutColumn>
                        <ext:Panel ID="pnlAction" runat="server" Width="35" BodyStyle="background-color: transparent;" Border="false">
                            <Body>
                                <ext:AnchorLayout ID="AnchorLayout1" runat="server">
                                    <ext:Anchor Vertical="40%">
                                        <ext:Panel ID="Panel1" runat="server" Border="false" BodyStyle="background-color: transparent;" />
                                    </ext:Anchor>
                                    <ext:Anchor>
                                        <ext:Panel ID="Panel2" runat="server" Border="false" BodyStyle="padding:5px;background-color: transparent;">
                                            <Body>
                                                <ext:Button ID="Button1" runat="server" Icon="ResultsetNext" StyleSpec="margin-bottom:2px;">
                                                    <Listeners>
                                                        <Click Handler="CountrySelector.add();" />
                                                    </Listeners>
                                                    <ToolTips>
                                                        <ext:ToolTip ID="ToolTip1" runat="server" Title="Add" Html="Add Selected Rows" />
                                                    </ToolTips>
                                                </ext:Button>
                                                <ext:Button ID="Button2" runat="server" Icon="ResultsetLast" StyleSpec="margin-bottom:2px;">
                                                    <Listeners>
                                                        <Click Handler="CountrySelector.addAll();" />
                                                    </Listeners>
                                                    <ToolTips>
                                                        <ext:ToolTip ID="ToolTip2" runat="server" Title="Add all" Html="Add All Rows" />
                                                    </ToolTips>
                                                </ext:Button>
                                                <ext:Button ID="Button3" runat="server" Icon="ResultsetPrevious" StyleSpec="margin-bottom:2px;">
                                                    <Listeners>
                                                        <Click Handler="CountrySelector.remove(GridPanel1, GridPanel2);" />
                                                    </Listeners>
                                                    <ToolTips>
                                                        <ext:ToolTip ID="ToolTip3" runat="server" Title="Remove" Html="Remove Selected Rows" />
                                                    </ToolTips>
                                                </ext:Button>
                                                <ext:Button ID="Button4" runat="server" Icon="ResultsetFirst" StyleSpec="margin-bottom:2px;">
                                                    <Listeners>
                                                        <Click Handler="CountrySelector.removeAll(GridPanel1, GridPanel2);" />
                                                    </Listeners>
                                                    <ToolTips>
                                                        <ext:ToolTip ID="ToolTip4" runat="server" Title="Remove all" Html="Remove All Rows" />
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
                            ID="GridPanel2" 
                            EnableDragDrop="false"
                            AutoExpandColumn="Country" Title="已选地点" 
                            StoreID="Store2">
                            <Listeners>
                            </Listeners>
                            <ColumnModel ID="ColumnModel2" runat="server">
	                            <Columns>
                                    <ext:Column ColumnID="Country" Header="已选地点" DataIndex="Placename" Sortable="true" /> 
                                    <ext:Column Header="片区" DataIndex="Pareasname" Sortable="true" />                   
	                            </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:CheckboxSelectionModel ID="CheckboxSelectionModel2" runat="server" />
                            </SelectionModel>  
                            <SaveMask ShowMask="true" />
                        </ext:GridPanel>
                    </ext:LayoutColumn>
                </ext:ColumnLayout>              
            </Body>  
            <Buttons>
                <ext:Button ID="Button5" runat="server" Text="确 定" Icon="Disk">
                    <Listeners>
                        <Click Handler="#{GridPanel2}.submitData();#{Window1}.hide();" />
                    </Listeners>
                </ext:Button>
                <ext:Button ID="Button6" runat="server" Text="取 消" Icon="Cancel">
                    <Listeners>
                        <Click Handler="#{Window1}.hide();" /><%--CountrySelector.removeAll(GridPanel1, GridPanel2);--%>
                    </Listeners>
                </ext:Button>
            </Buttons>      
        </ext:Window>
        <ext:Hidden ID="GridData" runat="server" />
        <ext:ViewPort ID="ViewPort1" runat="server">
            <Body>
                <ext:BorderLayout ID="BorderLayout1" runat="server">
                    <Center>
                        <ext:GridPanel 
                            ID="GridPanel3" 
                            runat="server"
                            StoreID="YHStore"
                            StripeRows="true"
                            Collapsible="false"
                            AutoExpandColumn="detail"
                            Title="待复查隐患信息"
                            >
                            <TopBar>
                                <ext:Toolbar ID="Toolbar2" runat="server">
                                    <Items>
                                        <ext:Button runat="server" ID="btn_search" Icon="FolderMagnify" Text="地点选择" >
                                            <Listeners>
                                                <Click Handler="#{Window1}.show();" />
                                            </Listeners>
                                        </ext:Button>
                                        <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                                        <ext:Button ID="btnPrint" runat="server" Text="打印报表" Icon="Printer" AutoPostBack="true" OnClick="ToPrint" Disabled="true">
                                            <Listeners>
                                                <Click Fn="saveData" />
                                            </Listeners>
                                        </ext:Button>
                                        <ext:ToolbarSeparator />
                                        <ext:Button ID="btnExcel" runat="server" Text="导出报表" Icon="PageExcel" AutoPostBack="true" OnClick="ToExcel" Disabled="true">
                                            <Listeners>
                                                <Click Fn="saveData" />
                                            </Listeners>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <ColumnModel ID="ColumnModel3" runat="server">
		                        <Columns>
                                    <ext:Column ColumnID="detail" Header="内容" Width="170" Sortable="true" DataIndex="YHContent" >
                                    </ext:Column>
                                    <ext:Column Header="部门" Width="100" Sortable="true" DataIndex="DeptName" >
                                    </ext:Column>
                                    <ext:Column Header="地点" Width="150" Sortable="true" DataIndex="PlaceName" >
                                    </ext:Column>
		                            <ext:Column  Header="班次" Width="70" DataIndex="BanCi" >
                                    </ext:Column>
                                    <ext:Column Header="排查人" Width="60" Sortable="true" DataIndex="Name" >
                                    </ext:Column>
                                    <ext:Column Header="排查时间" Width="80" Sortable="true" DataIndex="PCTime" >
                                    <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" /></ext:Column>
                                    <ext:Column Header="类型" Width="60" Sortable="true" DataIndex="YHType" >
                                    </ext:Column>
                                    <ext:Column Header="级别" Width="60" Sortable="true" DataIndex="YHLevel" >
                                    </ext:Column>
		                        </Columns>
                            </ColumnModel>
                            <LoadMask ShowMask="true" />
                            <BottomBar>
                                <ext:PagingToolBar ID="PagingToolBar2" runat="server" PageSize="20" />
                            </BottomBar>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="RowSelectionModel1" SingleSelect="true" runat="server" />                   
                            </SelectionModel>
                        </ext:GridPanel>
                    </Center>
                </ext:BorderLayout>
            </Body>
        </ext:ViewPort>

    </form>
</body>
</html>
