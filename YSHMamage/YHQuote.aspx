<%@ Page Language="C#" AutoEventWireup="true" CodeFile="YHQuote.aspx.cs" Inherits="YSHMamage_YHQuote" %>

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

        var template = '<span style="color:{0};">{1}</span>';
        var change = function (value) {
            var color; var valu;
            if (value == '2') {
                color = 'red'; valu = '未引用';
            }
            else if (value == '1') {
                color = 'orange'; valu = '引用';
            }
            else if (value == '0') {
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
                        Title="隐患引用信息"
                        >
                        <TopBar>
                            <ext:Toolbar ID="Toolbar2" runat="server">
                                <Items>
                                    <ext:Label runat="server" ID="lbl1" Text="专业:" />
                                    <ext:ComboBox 
                                        ID="cbb_kind"
                                        runat="server"  
                                        EmptyText="请选择专业.."
                                        DisplayField="YHType" 
                                        ValueField="YHTypeID" 
                                        StoreID="TypeStore"
                                        Editable="false"
                                        />
                                    <ext:Label runat="server" ID="Label1" Text="级别:" />
                                    <ext:ComboBox 
                                        ID="cbb_lavel" 
                                        runat="server" 
                                        EmptyText="请选择级别.."
                                        DisplayField="YHLevel" 
                                        ValueField="YHLevelID" 
                                        StoreID="LevelStore"
                                        Mode="Local"
                                        Width="100"
                                        Editable="false"
                                        >
                                    </ext:ComboBox>
                                    <ext:ToolbarSeparator />
                                    <ext:Button runat="server" ID="btn_search" Icon="Zoom" Text="查询" >
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.Search();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:ToolbarSeparator />
                                    <ext:Button runat="server" ID="btnquote" Icon="FolderAdd" Text="引用" Disabled="true" >
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.yhquote();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server" ID="btndel" Icon="Delete" Text="作废" Disabled="true" >
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.yhdel();" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button runat="server" ID="btnAction" Icon="FolderEdit" Text="修改" Disabled="true" >
                                        <Listeners>
                                            <Click Handler="#{Window2}.show();" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <ColumnModel ID="ColumnModel1" runat="server">
	                        <Columns>
	                            <ext:Column Header="编号" Width="100" Sortable="true" DataIndex="Yhnumber" />
                                <ext:Column ColumnID="detail" Header="描述" Width="170" Sortable="true" DataIndex="Yhcontent">
                                    <Renderer Fn="qtip" />
                                </ext:Column>
                                <ext:Column Header="级别" Width="100" Sortable="true" DataIndex="Levelname" />
                                <ext:Column Header="专业" Width="100" Sortable="true" DataIndex="Typename" />
                                <ext:Column Header="引用时间" Width="80" Sortable="true" DataIndex="Intime" >
                                    <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" />
                                </ext:Column>
                                <ext:Column Header="当前状态" Width="60" Sortable="true" DataIndex="Nstatus">
                                    <Renderer Fn="change" />
                                </ext:Column>
	                        </Columns>
                        </ColumnModel>
                        <LoadMask ShowMask="true" />
                        <BottomBar>
                            <ext:PagingToolBar ID="PagingToolBar1" runat="server" PageSize="20" />
                        </BottomBar>
                        <SelectionModel>
                             <ext:CheckboxSelectionModel ID="RowSelectionModel3" runat="server" SingleSelect="true">
                                <Listeners>
                                    <RowSelect Handler="#{FormPanel1}.getForm().loadRecord(record);Coolite.AjaxMethods.detailDet(record.data.Yhid);" />
                                </Listeners>
                             </ext:CheckboxSelectionModel>         
                        </SelectionModel>
                        <AjaxEvents>
                            <Click OnEvent="RowClick"></Click>
                        </AjaxEvents>
                    </ext:GridPanel>
                </Center>
            </ext:BorderLayout>
        </Body>
    </ext:ViewPort>

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
                                <ext:TextField ID="TextArea1" runat="server" FieldLabel="编号" DataIndex="Yhnumber" BlankText="编号不能为空" Disabled="true" />
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
                            <ext:Anchor>
                                <ext:TextArea ID="tfYhcontent" runat="server" FieldLabel="描述" DataIndex="Yhcontent" Height="75px" BlankText="描述不能为空" Disabled="true" />
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
