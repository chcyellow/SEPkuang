<%@ Page Language="C#" AutoEventWireup="true" CodeFile="YHBaseSet.aspx.cs" Inherits="YSHMamage_YHBaseSet" %>

<%@ Register assembly="Coolite.Ext.Web" namespace="Coolite.Ext.Web" tagprefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
    
        var qtip = function(v, p) {//单元格提示
            //v : value , p : cell
            p.attr = 'ext:qtitle="" ext:qtip="' + v + '"';
            return v;
        }

        var template = '<span style="color:{0};">{1}</span>';
        var change = function(value) {
            var color; var valu
            if (value == '1') {
                color = 'red'; valu = '新增';
            }
            else if (value == '2') {
                color = 'orange'; valu = '发布';
            }
            else if (value == '3') {
                color = 'gray'; valu = '作废';
            }
            return String.format(template, color, valu);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN" CleanResourceUrl="false" />
    
    <ext:Store ID="YHStore" runat="server" >
        <Reader>
            <ext:JsonReader ReaderID="Yhid">
                <Fields>
                    <ext:RecordField Name="Yhid" Type="Int" />
                    <ext:RecordField Name="Yhnumber" />
                    <ext:RecordField Name="Yhcontent" />
                    <ext:RecordField Name="Levelid" />
                    <ext:RecordField Name="Levelname" />
                    <ext:RecordField Name="Typeid" />
                    <ext:RecordField Name="Typename" />
                    <ext:RecordField Name="Conpyfirst" />
                    <ext:RecordField Name="Intime" Type="Date" DateFormat="Y-m-dTh:i:s" />
                    <ext:RecordField Name="Nstatus" />
                    <ext:RecordField Name="Sglxid" />
                    <ext:RecordField Name="Sglxname" />
                    <ext:RecordField Name="Deptname" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    
    <ext:Store ID="LevelStore" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="YHLevelID">
                <Fields>
                    <ext:RecordField Name="YHLevelID" />
                    <ext:RecordField Name="YHLevel" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
     <ext:Store ID="TypeStore" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="YHTypeID">
                <Fields>
                    <ext:RecordField Name="YHTypeID" />
                    <ext:RecordField Name="YHType" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    
    <ext:Store ID="SGStore" runat="server" >
        <Reader>
            <ext:JsonReader ReaderID="Infoid">
                <Fields>
                    <ext:RecordField Name="Infoid" />
                    <ext:RecordField Name="Infoname" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    
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
                        Title="标准隐患信息"
                        >
                        <TopBar>
                            <ext:Toolbar ID="Toolbar2" runat="server">
                                <Items>
                                    <ext:Button runat="server" ID="btn_search" Icon="Zoom" Text="条件查询" >
                                        <Listeners>
                                            <Click Handler="#{Window1}.show();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:ToolbarSeparator />
                                    <ext:Button runat="server" ID="Button1" Icon="Add" Text="新增" >
                                        <Listeners>
                                            <Click Handler="#{FormPanel1}.getForm().reset();Coolite.AjaxMethods.detailDet('-1');#{Window2}.show();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server" ID="Button2" Icon="FolderEdit" Text="修改" Disabled="true" >
                                        <Listeners>
                                            <Click Handler="#{Window2}.show();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server" ID="Button3" Icon="Delete" Text="作废" Disabled="true" >
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.yhdel();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:ToolbarSeparator />
                                    <ext:Button runat="server" ID="btnpublish" Icon="Disk" Text="发布" Disabled="true" >
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.yhpublish();" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <ColumnModel ID="ColumnModel1" runat="server">
	                        <Columns>
	                            <ext:Column Header="编号" Width="100" Sortable="true" DataIndex="Yhnumber" />
	                            <ext:Column Header="专业" Width="100" Sortable="true" DataIndex="Typename" />
	                            <ext:Column Header="级别" Width="100" Sortable="true" DataIndex="Levelname" />
	                            <ext:Column Header="事故类型" Width="100" Sortable="true" DataIndex="Sglxname" />
                                <ext:Column ColumnID="detail" Header="描述" Width="170" Sortable="true" DataIndex="Yhcontent">
                                    <Renderer Fn="qtip" />
                                </ext:Column>
                                
                                <ext:Column Header="入库时间" Width="80" Sortable="true" DataIndex="Intime" >
                                    <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" />
                                </ext:Column>
                                <ext:Column Header="当前状态" Width="60" Sortable="true" DataIndex="Nstatus">
                                    <Renderer Fn="change" />
                                </ext:Column>
                                <ext:Column Header="提交单位" Width="100" Sortable="true" DataIndex="Deptname" />
	                        </Columns>
                        </ColumnModel>
                        <LoadMask ShowMask="true" />
                        <BottomBar>
                            <ext:PagingToolBar ID="PagingToolBar1" runat="server" PageSize="20" />
                        </BottomBar>
                        <SelectionModel>
                             <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                <Listeners>
                                    <RowSelect Handler="#{FormPanel1}.getForm().loadRecord(record);Coolite.AjaxMethods.detailDet(record.data.Yhid);" />
                                </Listeners>
                            </ext:RowSelectionModel>                 
                        </SelectionModel>
                        <AjaxEvents>
                            <Click OnEvent="RowClick"></Click>
                        </AjaxEvents>
                    </ext:GridPanel>
                </Center>
            </ext:BorderLayout>
        </Body>
    </ext:ViewPort>
    <%--查询条件--%>
     <ext:Window 
        ID="Window1" 
        runat="server" 
        BodyStyle="padding:5px;" 
        ButtonAlign="Right"
        Frame="true" 
        Title="请选择查询条件"
        AutoHeight="true"
        Width="300px"
        Modal="true"
        ShowOnLoad="false"
        X="100" Y="60">
        <Body>
            <ext:FormPanel 
                    ID="FormPanel2" 
                    runat="server"
                    BodyStyle="padding:1px;"
                    BodyBorder="false"
                    Header="false">
                    <Body>
            <ext:FormLayout ID="FormLayout2" runat="server" LabelWidth="60">
                <ext:Anchor Horizontal="95%">
                    <ext:MultiField ID="mf_datecheck" runat="server" FieldLabel="入库时间">
                        <Fields>
                            <ext:DateField ID="df_begin" runat="server" Format="yyyy-MM-dd" Note="起始日期" Width="100" Vtype="daterange">
                                <Listeners>
                                    <Render Handler="this.endDateField = '#{df_end}'" />
                                </Listeners>
                            </ext:DateField>
                            <ext:DateField ID="df_end" runat="server" Format="yyyy-MM-dd" Note="截止日期" Width="100" Vtype="daterange" >
                                <Listeners>
                                    <Render Handler="this.startDateField = '#{df_begin}'" />
                                </Listeners>
                            </ext:DateField>
                        </Fields>
                    </ext:MultiField>
                </ext:Anchor>
                <ext:Anchor Horizontal="95%">
                    <ext:ComboBox 
                        ID="cbb_lavel" 
                        runat="server"  FieldLabel="级别" 
                        EmptyText="请选择级别.."
                        DisplayField="YHLevel" 
                        ValueField="YHLevelID" 
                        StoreID="LevelStore"
                        Mode="Local"
                        Width="100"
                        Editable="false"
                        >
                    </ext:ComboBox>
                 </ext:Anchor>
                <ext:Anchor Horizontal="95%">
                    <ext:ComboBox 
                        ID="cbb_kind"  FieldLabel="专业"
                        runat="server"  
                        EmptyText="请选择专业.."
                        DisplayField="YHType" 
                        ValueField="YHTypeID" 
                        StoreID="TypeStore"
                        Editable="false"
                        >
                    </ext:ComboBox>
                </ext:Anchor>
                <ext:Anchor Horizontal="95%">
                    <ext:ComboBox 
                        ID="cbb_SG"
                        FieldLabel="事故类型"
                        runat="server"  
                        EmptyText="请选择类型.."
                        DisplayField="Infoname" 
                        ValueField="Infoid" 
                        StoreID="SGStore"
                        Editable="false"
                        >
                    </ext:ComboBox>
                </ext:Anchor>
                <ext:Anchor Horizontal="95%">
                    <ext:ComboBox 
                        ID="cbb_status" 
                        runat="server" 
                        FieldLabel="状态" 
                        EmptyText="请选择状态.."
                        Editable="false"
                        >
                        <Items>
                            <ext:ListItem Text="新增" Value="1" />
                            <ext:ListItem Text="发布" Value="2" />
                            <ext:ListItem Text="作废" Value="3" />
                        </Items>
                    </ext:ComboBox>
                </ext:Anchor>
            </ext:FormLayout>
            </Body>
            </ext:FormPanel>
        </Body>
        <Buttons>
            <ext:Button ID="btn_Clear" runat="server" Icon="Cancel" Text="清除条件">
                <Listeners>
                    <Click Handler="#{FormPanel2}.getForm().reset();" />
                </Listeners>
            </ext:Button>
            <ext:Button ID="btnDoSearch" runat="server" Icon="Zoom" Text="查 询">
                <Listeners>
                    <Click Handler="Coolite.AjaxMethods.Search();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    
    <ext:Window ID="Window2"  
        ShowOnLoad="false" 
        BodyStyle="padding:0pc" 
        runat="server"
        BodyBorder="false"
        Collapsible="false"
        Frame="false"
        Width="350"
        Modal="true"
        AutoHeight="true"
        Title="隐患信息">
            <LoadMask ShowMask="true" />
            <Body>
                <ext:FormPanel 
                    ID="FormPanel1" 
                    runat="server"
                    BodyStyle="padding:1px;"
                    ButtonAlign="Center"
                    Frame="true"
                    BodyBorder="false"
                    MonitorValid="true"
                    Header="false">
                    <Defaults >
                        <ext:Parameter Name="anchor" Value="95%" Mode="Value" />
                        <ext:Parameter Name="msgTarget" Value="side" Mode="Value" />
                        <ext:Parameter Name="AllowBlank" Value="false" Mode="Raw" />
                    </Defaults>
                    <Listeners>
                        <ClientValidation Handler="#{btnUpdate}.setDisabled(!valid);" />
                    </Listeners>
                    <Body>
                        <ext:FormLayout ID="FormLayout1" runat="server"   LabelAlign="Left" LabelWidth="60">
                            <ext:Anchor>
                               <ext:Hidden ID="hdnID" runat="server" Text="-1" /> 
                            </ext:Anchor>
                            <ext:Anchor>
                               <ext:ComboBox 
                                    ID="cbbTypeid"
                                    FieldLabel="专业"
                                    runat="server"  
                                    DataIndex="Typeid"
                                    EmptyText="请选择专业.."
                                    DisplayField="YHType" 
                                    ValueField="YHTypeID" 
                                    StoreID="TypeStore"
                                    Editable="false"
                                    >
                                </ext:ComboBox> 
                            </ext:Anchor>
                            <ext:Anchor>
                               <ext:ComboBox 
                                    ID="cbbLevelid" 
                                    runat="server"
                                    FieldLabel="级别" 
                                    DataIndex="Levelid"
                                    EmptyText="请选择级别.."
                                    DisplayField="YHLevel" 
                                    ValueField="YHLevelID" 
                                    StoreID="LevelStore"
                                    Width="100"
                                    Editable="false"
                                    >
                                </ext:ComboBox>
                            </ext:Anchor>
                            <ext:Anchor Horizontal="95%">
                                <ext:ComboBox 
                                    ID="cbbSglxid"  FieldLabel="事故类型"
                                    runat="server"  
                                    DataIndex="Sglxid"
                                    EmptyText="请选择类型.."
                                    DisplayField="Infoname" 
                                    ValueField="Infoid" 
                                    StoreID="SGStore"
                                    Editable="false"
                                    >
                                </ext:ComboBox>
                            </ext:Anchor>
                            <ext:Anchor>
                                <ext:TextArea ID="tfYhcontent" runat="server" FieldLabel="描述" DataIndex="Yhcontent" Height="75px" BlankText="描述不能为空" />
                            </ext:Anchor>                        
                      </ext:FormLayout>
                    </Body>
                    <Buttons>
                        <ext:Button ID="btnUpdate" runat="server" Icon="Add" Text="更新">
                            <Listeners>
                                <Click Handler="if(!#{tfYhcontent}.validate()){Ext.Msg.alert('提示','描述不能为空！'); return false;}" />
                            </Listeners>
                            <AjaxEvents>
                                <Click  OnEvent="btnUpdateClick">
                                    <EventMask CustomTarget="={#{tfYhcontent}.body}"  Target="CustomTarget" ShowMask="true" MinDelay="20" />
                                </Click>
                            </AjaxEvents>
                        </ext:Button>
                         <ext:Button ID="Button4" runat="server" Icon="Cancel" Text="取消">
                            <Listeners>
                                <Click Handler="#{Window2}.hide(null);" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
                     
            </Body>

        </ext:Window>
    </form>
</body>
</html>
