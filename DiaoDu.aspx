<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DiaoDu.aspx.cs" Inherits="DiaoDu" %>
<%@ Register assembly="Coolite.Ext.Web" namespace="Coolite.Ext.Web" tagprefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>走动干部调度情况一览</title>
    <link href="chooser.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var template = '<span style="color:{0};"><b>{1}</b></span>';

        var change = function(value) {
            var color;
            if (value.toString().replace(/(^\s*)|(\s*$)/g, "") == '已走动')
                color = 'orange';
            else if (value.toString().replace(/(^\s*)|(\s*$)/g, "") == '未走动')
                color = '#cc0000';
            else
                color = 'red';
            return String.format(template, color, value);
        }
    </script>
    <script type="text/javascript">
        var selectionChanged = function(dv, nodes) {
            if (nodes.length > 0) {
                var id = nodes[0].id;
                Coolite.AjaxMethods.DetailLoad(id);
            }
        }
    </script>
    <script type="text/javascript">
        var selectionChanged1 = function(dv, nodes) {
            if (nodes.length > 0) {
                var id = nodes[0].id;
                Coolite.AjaxMethods.DetailLoad1(id);
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <ext:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" />
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN">
    </ext:ScriptManager>
    <ext:Store ID="Store4" runat="server" >
        <Reader>
            <ext:JsonReader ReaderID="PersonID">
                <Fields>
                    <ext:RecordField Name="PersonID" Type="Int" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store3" runat="server" >
        <Reader>
            <ext:JsonReader ReaderID="PersonID">
                <Fields>
                    <ext:RecordField Name="PersonID" Type="Int" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store1" runat="server" OnRefreshData="MyData_Refresh">
        <Reader>
            <ext:JsonReader ReaderID="PERSONNUMBER">
                <Fields>
                    <ext:RecordField Name="PERSONNUMBER" />
                    <ext:RecordField Name="NAME" />
                   <ext:RecordField Name="POSNAME" />
                    <ext:RecordField Name="KQ" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store2" runat="server" >
        <Reader>
            <ext:JsonReader ReaderID="ID">
                <Fields>
                    <ext:RecordField Name="ID" />
                    <ext:RecordField Name="PERSONNUMBER" />
                    <ext:RecordField Name="NAME" />
                    <ext:RecordField Name="KQ" />
                    <ext:RecordField Name="PLACENAME" />
                    <ext:RecordField Name="STARTTIME" Type="Date" DateFormat="Y-m-dTh:i:s" />
                    <ext:RecordField Name="ENDTIME" Type="Date" DateFormat="Y-m-dTh:i:s" />                 
                    <ext:RecordField Name="MOVESTATE" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="Store5" runat="server" >
        <Reader>
            <ext:JsonReader ReaderID="Deptnumber">
                <Fields>
                    <ext:RecordField Name="Deptnumber" />
                    <ext:RecordField Name="Deptname" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Panel ID="Panel3" runat="server" Height="55" Title="查询条件" Icon="Zoom">
        <Body>
            <table>
                <tr>
                    <td>
                        <ext:ComboBox ID="cbbDept" runat="server" Editable="false" DisplayField="Deptname" ValueField="Deptnumber" StoreID="Store5">
                        </ext:ComboBox>
                    </td>
                    <td>
                        <ext:Button ID="btnSearch" runat="server" Text="查询" Icon="Zoom">
                            <Listeners>
                                <Click Handler="Coolite.AjaxMethods.LoadData();" />
                            </Listeners>
                        </ext:Button>
                    </td>
                </tr>
            </table>
        </body>
    </ext:Panel>
    <ext:Panel ID="Panel1" runat="server" AutoScroll="true" Cls="img-chooser-view" Title="矿领导">
        <Body>
            <ext:ContainerLayout ID="ContainerLayout2" runat="server">
            <ext:DataView runat="server" ID="DataView1" StoreID="Store3"
                SingleSelect="true" 
                OverClass="x-view-over" 
                ItemSelector="div.thumb-wrap" 
                EmptyText="<div style='padding:10px;'>暂无数据</div>">
                <Template ID="Template2" runat="server">
                    <tpl for=".">
                        <div class="thumb-wrap" id="{PersonID}"><span>{Name}</span></div>
                    </tpl>
                </Template>
                <Listeners>
                    <SelectionChange Fn="selectionChanged" />
                </Listeners>                                
            </ext:DataView> 
            </ext:ContainerLayout>          
        </Body>
    </ext:Panel>
    <ext:Panel ID="Panel2" runat="server" AutoScroll="true" Cls="img-chooser-view" Title="中层领导">
        <Body>
            <ext:ContainerLayout ID="ContainerLayout1" runat="server">
            <ext:DataView runat="server" ID="ImageView" StoreID="Store4"
                SingleSelect="true" 
                OverClass="x-view-over" 
                ItemSelector="div.thumb-wrap" 
                EmptyText="<div style='padding:10px;'>暂无数据</div>">
                <Template ID="Template1" runat="server">
                    <tpl for=".">
                        <div class="thumb-wrap" id="{PersonID}"><span>{Name}</span></div>
                    </tpl>
                </Template>
                <Listeners>
                    <SelectionChange Fn="selectionChanged" />
                </Listeners>                                
            </ext:DataView> 
            </ext:ContainerLayout>          
        </Body>
    </ext:Panel>
<%--<ext:Panel ID="Portlet2" Title="矿领导" runat="server" AutoHeight="true" AutoScroll="true" />
--%><%--<ext:Panel ID="Panel1" Title="中层领导" runat="server" AutoHeight="true" AutoScroll="true" />--%>

    <ext:GridPanel 
        ID="GridPanel1" 
        runat="server"
        StoreID="Store1"
        StripeRows="true"
        Title="干部走动计划"
        AutoExpandColumn="PERSONNUMBER" 
        Collapsible="false"
        Width="670px"
        AutoHeight="true">
        <ColumnModel ID="ColumnModel1" runat="server">
		    <Columns>
		        <ext:Column ColumnID="PERSONNUMBER" Header="人员工号" Width="120" DataIndex="PERSONNUMBER" >
                </ext:Column>
                <ext:Column Header="走动人员" Width="150" Sortable="true" DataIndex="NAME" >
                </ext:Column>
                <ext:Column Header="部门" Width="200" Sortable="true" DataIndex="KQ" >
                </ext:Column>
                <ext:Column Header="职务" Width="200" Sortable="true" DataIndex="POSNAME" >
                </ext:Column>
		    </Columns>
        </ColumnModel>

        <SelectionModel>
            <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" SingleSelect="true" runat="server" />                   
        </SelectionModel>
        <AjaxEvents>
            <Click OnEvent="RowClick"></Click>
        </AjaxEvents>
       <TopBar>
            <ext:Toolbar runat="server" ID="tb1">
                <Items>
                <ext:ToolbarSeparator ID="ctl56" />

                <ext:Button runat="server"  ID="btn_fcfk" Icon="FolderUser" Text="查看详情" Disabled="true" >
                    <AjaxEvents>
                        <Click OnEvent="Button3_Click"></Click>
                    </AjaxEvents>
                </ext:Button>
                <ext:ToolbarFill ID="ctl57" />
                <ext:ToolbarSeparator ID="ctl58" /> 
                </Items>
            </ext:Toolbar>
        </TopBar>
        <LoadMask ShowMask="true" />
        <BottomBar>
            <ext:PagingToolBar ID="PagingToolBar1" runat="server" PageSize="15" />
         </BottomBar>
     </ext:GridPanel>
     <ext:Window ID="Window1"  runat="server" 
        BodyStyle="padding:5px;" 
        ButtonAlign="Right"
        Frame="true" 
        Title="走动计划详细"
        AutoHeight="true"
        Width="685px"
        Modal="true"
        ShowOnLoad="false"
        X="60"
        Y="100">
            <TopBar>
                <ext:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                        <ext:Label ID="lable" runat="server"></ext:Label>
                     </Items>
                </ext:Toolbar>
            </TopBar>
            <Body>
            <ext:Hidden ID="hdnid" runat="server">
            </ext:Hidden>
            <ext:Hidden ID="Hidden1" runat="server">
            </ext:Hidden>
            <ext:FormLayout ID="FormLayout3" runat="server" LabelWidth="60">
            <ext:Anchor Horizontal="100%">
                <ext:GridPanel 
                    ID="GridPanel2" 
                    runat="server"
                    StoreID="Store2"
                    StripeRows="true"
                    
                    Collapsible="false"
                    Width="660px"
                    AutoHeight="true">
            <ColumnModel ID="ColumnModel2" runat="server">
		    <Columns>
		        <ext:Column ColumnID="ID" Header="编号" Width="70" DataIndex="ID" >
                </ext:Column>
                <ext:Column Header="走动人员" Width="100" Sortable="true" DataIndex="NAME" >
                </ext:Column>
                <%--<ext:Column Header="部门" Width="100" Sortable="true" DataIndex="DeptName" >
                </ext:Column>--%>
                <ext:Column Header="走动地点" Width="150" Sortable="true" DataIndex="PLACENAME" >
                </ext:Column>
                <ext:Column Header="计划开始" Width="125" Sortable="true" DataIndex="STARTTIME" >
                <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" /></ext:Column>
                <ext:Column Header="计划截止" Width="125" Sortable="true" DataIndex="ENDTIME" >
                <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" /></ext:Column>
                <ext:Column Header="走动状态" Width="100" Sortable="true" DataIndex="MOVESTATE" >
                <Renderer Fn="change" />
                </ext:Column>
		    </Columns>
        </ColumnModel>
                    <LoadMask ShowMask="true" />
                    <BottomBar>
                        <ext:PagingToolBar ID="PagingToolBar2" runat="server" PageSize="15" />
                    </BottomBar>
                </ext:GridPanel>
             </ext:Anchor>
            </ext:FormLayout>
            </Body>
        </ext:Window>
    </div>
    </form>
</body>
</html>
