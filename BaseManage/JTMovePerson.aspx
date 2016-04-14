<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JTMovePerson.aspx.cs" Inherits="BaseManage_JTMovePerson" %>

<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
                vlu = '本科室';
            }
            else if (value == '2') {
                color = 'blue';
                vlu = '本单位';
            }
            else if (value == '3') {
                color = 'green';
                vlu = '全局';
            }
            else {
                color = 'gray';
                vlu = '未设定';
            }
            return String.format(template, color, vlu);
        }
    </script>

     <style type="text/css">
        .search-item {
            font: normal 12px tahoma, arial, helvetica, sans-serif;
            padding: 3px 10px 3px 10px;
            border: 1px solid #fff;
            font-weight:bold; 
            border-bottom: 1px solid #eeeeee;
            white-space: normal;
            color: #555;
        }
        
        .search-item h3 {
            display: block;
            font: inherit;
            
            color: #222;
        }

        .search-item h3 span {
            float: left;
             
            margin: 0 0 5px 5px;
            width: 100px;
            display: block;
            clear: none;
        } 
        
        p { width: 650px; }
        
        .ext-ie .x-form-text { position: static !important; }
        
        .cbStates-list 
        {
            width: 600px;
            font: 11px tahoma,arial,helvetica,sans-serif;
        }
        
        .cbStates-list th {
            font-weight: bold;
        }
        
        .cbStates-list td, .cbStates-list th {
            padding: 3px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN" CleanResourceUrl="false" />
    <ext:Store ID="ItemStore" runat="server" >
        <Reader>
            <ext:JsonReader ReaderID="Personnumber">
                <Fields>
                    <ext:RecordField Name="Personnumber" />
                    <ext:RecordField Name="Name" />
                    <ext:RecordField Name="Deptname" />
                    <ext:RecordField Name="Posname" />
                    <ext:RecordField Name="Visualfield" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>

    <ext:Store ID="DeptStore" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="Deptnumber">
                <Fields>
                    <ext:RecordField Name="Deptnumber" />
                    <ext:RecordField Name="Deptname" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>

    <ext:Store ID="personStore" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="Name" />
                        <ext:RecordField Name="Personnumber" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>

    <ext:Hidden ID="hdnKindid" runat="server" Text="0"/>

    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                <West Collapsible="true" Split="true">
                    <ext:TreePanel ID="tpkind" runat="server" Title="部门信息" Border="false" Width="380" RootVisible="false">
                        <TopBar>
                            <ext:Toolbar runat="server">
                                <Items>
                                    <ext:ComboBox 
                                            ID="cbbDept"
                                            runat="server" 
                                            StoreID="DeptStore"
                                            DisplayField="Deptname" 
                                            ValueField="Deptnumber"
                                            HideTrigger="true"
                                            > 
                                            <Listeners>
                                                <Render Fn="function(f) {
                                                            f.el.on('keyup', function(e) {
                                                            if(window.event.keyCode==38 || window.event.keyCode==40 || window.event.keyCode==13){
                                                                return;
                                                             }
                                                            Coolite.AjaxMethods.PYsearch(f.getRawValue(), 'DeptStore');
                                                           
                                                            });
                                                            }
                                                            " />
                                            </Listeners>
                                        </ext:ComboBox>
                                        <ext:ToolbarSeparator />
                                        <ext:ToolbarButton ID="btnAddDept" runat="server" Icon="Add" Text="添加">
                                            <Listeners>
                                                <Click Handler="Coolite.AjaxMethods.AddDept();" />
                                            </Listeners>
                                        </ext:ToolbarButton>
                                        <ext:ToolbarButton ID="btnDelDept" runat="server" Icon="Delete" Text="删除" >
                                            <Listeners>
                                                <Click Handler="Coolite.AjaxMethods.issureDelDept();" />
                                            </Listeners>
                                        </ext:ToolbarButton>
                                        <ext:ToolbarSeparator />
                                        <ext:ToolbarButton ID="btnRefresh" runat="server" Text="刷新" Icon="ArrowRefresh">
                                            <Listeners>
                                                <Click Handler="refreshTree(#{tpkind});" /> 
                                            </Listeners>
                                        </ext:ToolbarButton>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <Root>
                            <ext:TreeNode NodeID="-1" Text="部门" />
                        </Root>
                    </ext:TreePanel>
                </West>
                <Center>
                    <ext:GridPanel 
                        ID="gpJoem" 
                        runat="server" 
                        StoreID="ItemStore"
                        StripeRows="true"
                        Title="走动人员"
                        Icon="BrickMagnify"
                        Collapsible="false"
                        >
                        <ColumnModel ID="ColumnModel1" runat="server">
                            <Columns>
                                <ext:Column Header="姓名" DataIndex="Name" Width="100" />
                                <ext:Column Header="单位" DataIndex="Deptname" Width="100" />
                                <ext:Column Header="职务" DataIndex="Posname" Width="100" />
                                <ext:Column Header="可见范围" DataIndex="Visualfield" Width="80" >
                                    <Renderer Fn="change" />
                                </ext:Column>
                            </Columns>
                        </ColumnModel>
                        <TopBar>
                            <ext:Toolbar ID="Toolbar3" runat="server">
                                <Items>
                                    <ext:ComboBox 
                                        ID="cbbPerson"
                                        runat="server"    
                                        StoreID="personStore"
                                        DisplayField="Name" 
                                        ValueField="Personnumber"
                                        HideTrigger="true"    
                                        > 
                                        <Listeners>
                                            <Render Fn="function(f) {
                                                        f.el.on('keyup', function(e) {
                                                        if(window.event.keyCode==38 || window.event.keyCode==40 || window.event.keyCode==13){
                                                            return;
                                                            }
                                                        Coolite.AjaxMethods.PYsearch(f.getRawValue(), 'personStore');
                                                           
                                                        });
                                                        }
                                                        " />
                                        </Listeners>
                                    </ext:ComboBox>
                                    <ext:ToolbarSeparator />
                                    <ext:ToolbarButton ID="btnJoemNew" runat="server" Icon="Add" Text="新增人员" Disabled="true">
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.PersonAdd();" />
                                        </Listeners>
                                    </ext:ToolbarButton>
                                    <ext:ToolbarButton ID="btnJoemDel" runat="server" Icon="Delete" Text="删除" Disabled="true">
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.issurePersonDel();" />
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