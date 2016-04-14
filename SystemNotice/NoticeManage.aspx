<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NoticeManage.aspx.cs" Inherits="SystemNotice_NoticeManage" %>

<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../style/examples.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .x-form-group .x-form-group-header-text {
        	background-color: #dfe8f6;
        }
        
        .x-label-text {
            font-weight: bold;
            font-size: 11px;
        }
        .x-textfeild-style  
        {
            BORDER-RIGHT: #000000 0px solid; 
            BORDER-TOP: #000000 0px solid; 
            BORDER-LEFT: #000000 0px solid; 
            BORDER-BOTTOM: #000000 1px solid
        }
    </style>
    <script type="text/javascript">
        var qtip = function(v, p) {//单元格提示
            //v : value , p : cell
            p.attr = 'ext:qtitle="" ext:qtip="' + v + '"';
            return v;
        }
    </script>
    <script type="text/javascript">
        var template = '<span style="color:{0};">{1}</span>';

        var changeColor = function(value) {
            var color;
            if (value == '编辑')
                color = 'green';
            else if (value == '已发布')
                color = 'red';
            else
                color = 'black';
            return String.format(template, color, value);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" CleanResourceUrl="false" Locale="zh-CN" />
    <%--列表数据--%>
    <ext:Hidden ID="NidLoad" runat="server" />
    <ext:Store ID="Store1" runat="server" OnRefreshData="MyData_Refresh">
        <Reader>
            <ext:JsonReader ReaderID="Nid">
                <Fields>
                    <ext:RecordField Name="Nid" Type="Int" />
                    <ext:RecordField Name="Ntitle" />
                    <ext:RecordField Name="Nmessage" />
                    <ext:RecordField Name="Pname" />
                    <ext:RecordField Name="Pdept" />
                    <ext:RecordField Name="Pdate" Type="Date" DateFormat="Y-m-dTh:i:s" />
                    <ext:RecordField Name="Nstatus" />
                </Fields>
            </ext:JsonReader>
        </Reader>              
    </ext:Store>
    <%--发布对象列表--%>
    <ext:Store ID="FBDXStore" runat="server" >
        <Reader>
            <ext:JsonReader ReaderID="Deptnumber">
                <Fields>
                    <ext:RecordField Name="Deptnumber" />
                    <ext:RecordField Name="Deptname" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Viewport ID="Viewport1" runat="server">
        <Body>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                <Center>
                    <ext:GridPanel 
                        ID="GridPanel1" 
                        runat="server"
                        StoreID="Store1"
                        StripeRows="true"
                        Title="公告信息"
                        Icon="Mail"
                        AutoExpandColumn="NIid" 
                        Collapsible="false" 
                        Width="810px"
                        >
                        <ColumnModel ID="ColumnModel1" runat="server">
		                    <Columns>
                                <ext:Column Header="编号" Width="70" DataIndex="Nid" />
                                <ext:Column Header="标题" Width="100" DataIndex="Ntitle" />
                                <ext:Column ColumnID="NIid" Header="公告内容" Width="120" Sortable="false" DataIndex="Nmessage">
                                    <Renderer Fn="qtip" />
                                </ext:Column>
                                <ext:Column Header="发布人员" Width="70" DataIndex="Pname" />
                                <ext:Column Header="发布单位" Width="70" DataIndex="Pdept" />
                                <ext:Column Header="发布时间" Width="70" Sortable="true" DataIndex="Pdate" >
                                    <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" />
                                </ext:Column>
                                <ext:Column Header="状态" Width="70" Sortable="true" DataIndex="Nstatus">
                                    <Renderer Fn="changeColor" />
                                </ext:Column>
		                    </Columns>
                        </ColumnModel>
                        <LoadMask ShowMask="true" />
                        <BottomBar>
                            <ext:PagingToolbar runat="server" ID="pageToolBar" StoreID="Store1"></ext:PagingToolbar>
                        </BottomBar>
                        <SelectionModel>
                            <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" SingleSelect="true" runat="server" />                   
                        </SelectionModel>
                        <AjaxEvents>
                            <Click OnEvent="RowClick"></Click>
                        </AjaxEvents>
                        <TopBar>
                            <ext:Toolbar ID="Toolbar1" runat="server">
                                <Items>
                                    <ext:Label ID="Label1" runat="server" Text="时间选择:">
                                    </ext:Label>
                                    <ext:DateField ID="dfBegin" runat="server" Width="100" Vtype="daterange">
                                    </ext:DateField>
                                    <ext:Label ID="Label2" runat="server" Text="---">
                                    </ext:Label>
                                    <ext:DateField ID="dfEnd" runat="server" Width="100" Vtype="daterange"> 
                                    </ext:DateField>
                                    <ext:ToolbarSeparator />
                                    <ext:Button ID="btn_Sel" runat="server" Text="查询" Icon="Zoom">
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.storeload();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:ToolbarFill />
                                    <ext:Button runat="server" ID="btn_Chexiao" Icon="FolderBug" Text="撤销发布" Disabled="true" >
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.isCxfb();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server" ID="btn_FaBu" Icon="FolderWrench" Text="发布">
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.FBDXset();#{Window1}.show();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:ToolbarSeparator />
                                    <ext:Button runat="server" ID="btn_Delete" Icon="FolderDelete" Text="删除">
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.Delete();" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                     </ext:GridPanel>
                </Center>
                <East Collapsible="true" Split="true">
                    <ext:Panel runat="server" ID="pnlEdit" Title="明细" Icon="EmailEdit" Width="250" BodyStyle="background-color:#F0F4FB;" >
                        <Body>
                            <br /><span class="x-label-text">公告标题:</span>
                            <br /><ext:TextField ID="tf_Title" runat="server" Cls="x-textfeild-style" Width="240px" />
                            <br /><br /><span class="x-label-text">公告内容:</span>
                            <br /><ext:TextArea ID="ta_Message" runat="server" Width="240px" Height="220px" />
                            <br /><br /><span class="x-label-text">附件:</span>
                            <br /><ext:FileUploadField ID="BasicField" Width="250" runat="server" EmptyText="请选择文件" ButtonText="浏览" Icon="Attach" />
                            <ext:Panel runat="server" ID="pnlAnnex" Width="250" AutoHeight="true" Title="">
                                <Buttons>
                                    <ext:LinkButton runat="server" ID="lbtn_Again" Text="重新上传">
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.Again();" />
                                        </Listeners>
                                    </ext:LinkButton>
                                </Buttons>
                            </ext:Panel>
                        </Body>
                        <TopBar>
                            <ext:Toolbar runat="server">
                                <Items>
                                    <ext:ToolbarFill />
                                    <ext:Button runat="server" ID="btn_S" Icon="Add" Text="保存">
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.Save();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:ToolbarSeparator />
                                    <ext:Button runat="server" ID="btn_D" Icon="Cancel" Text="清空">
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.Cancel();" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                    </ext:Panel>
                </East>
            </ext:BorderLayout>
        </Body>
    </ext:Viewport>
    <ext:Window 
        ID="Window1" 
        runat="server" 
        BodyStyle="padding:5px;" 
        ButtonAlign="Right"
        Frame="true" 
        Title="公告发布"
        Width="100px" Height="400px"
        Modal="true" AutoScroll="true"
        ShowOnLoad="false" Icon="FolderWrench" 
        >
        <Body>
            <ext:BorderLayout runat="server" ID="borderlayoutwindow">
                <Center>
                    <ext:GridPanel 
                        ID="GridPanel2" 
                        runat="server" 
                        StoreID="FBDXStore" 
                        Height="200"
                        StripeRows="true"
                        Header="false"
                        Collapsible="false" AutoScroll="true"
                        >
                        <ColumnModel ID="ColumnModel2" runat="server">
                            <Columns>
                                <ext:Column Header="发布对象" DataIndex="Deptname"/>
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:CheckboxSelectionModel ID="CheckboxSelectionModel2" runat="server" SingleSelect="false" />
                        </SelectionModel>
                    </ext:GridPanel>
                </Center>
            </ext:BorderLayout>
        </Body>
        <Buttons>
            <ext:Button ID="Button3" runat="server" Icon="Accept" Text="发布确认">
                <Listeners>
                    <Click Handler="Coolite.AjaxMethods.Fabu();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    </form>
</body>
</html>
