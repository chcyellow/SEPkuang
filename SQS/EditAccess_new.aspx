<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditAccess_new.aspx.cs" Inherits="SQS_EditAccess_new" %>

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
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN" CleanResourceUrl="false" />
    <ext:Store ID="ESSENTIALCONDITIONStore" runat="server" OnBeforeStoreChanged="SaveN">
        <Reader>
            <ext:JsonReader ReaderID="Ecid">
                <Fields>
                    <ext:RecordField Name="Ecid"/>
                    <ext:RecordField Name="Content" />
                    <ext:RecordField Name="isCheck" Type="Boolean" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    
    <ext:Store ID="DEMOTIONStore" runat="server" OnBeforeStoreChanged="SaveD">
        <Reader>
            <ext:JsonReader ReaderID="Deid">
                <Fields>
                    <ext:RecordField Name="Deid"/>
                    <ext:RecordField Name="Content" />
                    <ext:RecordField Name="isCheck" Type="Boolean" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    
    <ext:Store ID="JeomStore" runat="server" GroupField="Kind" OnBeforeStoreChanged="SaveJ">
        <Reader>
            <ext:JsonReader ReaderID="Jcid">
                <Fields>
                    <ext:RecordField Name="Jcid" />
                    <ext:RecordField Name="Jccontent" />
                    <ext:RecordField Name="Score" />
                    <ext:RecordField Name="Means" />
                    <ext:RecordField Name="Jeom" />
                    <ext:RecordField Name="Remark" />
                    <ext:RecordField Name="Kind" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    
    <ext:Store ID="DeptStore" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="Deptnumber">
                <Fields>
                    <ext:RecordField Name="Deptnumber"/>
                    <ext:RecordField Name="Deptname" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Hidden runat="server" ID="hdnId" />
    <ext:Panel ID="Panel1" runat="server" AutoHeight="true" Title="基本信息" Width="800">
        <Body>
            <ext:ColumnLayout ID="ColumnLayout1" runat="server">
                    <ext:LayoutColumn ColumnWidth=".3">
                        <ext:Panel ID="Panel2" runat="server" Border="false" Header="false">
                            <Defaults>
                                <ext:Parameter Name="AllowBlank" Value="false" Mode="Raw" />
                                <ext:Parameter Name="MsgTarget" Value="side" />
                            </Defaults>
                            <Body>
                                <ext:FormLayout ID="FormLayout1" runat="server" LabelAlign="Left" LabelWidth="70" >
                                    <ext:Anchor Horizontal="92%">
                                        <ext:ComboBox ID="cbbforcheckDept" runat="server"
                                            DisplayField="Deptname" 
                                            ValueField="Deptnumber" FieldLabel="被考核单位"
                                            Width="180px" 
                                            StoreID="DeptStore" Editable="false" />
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:ComboBox ID="cbbKind" runat="server" FieldLabel="考核类型" />
                                    </ext:Anchor>
                                    </ext:FormLayout>
                            </Body>
                        </ext:Panel>
                    </ext:LayoutColumn>
                    <ext:LayoutColumn ColumnWidth=".3">
                        <ext:Panel ID="Panel3" runat="server" Border="false" Header="false">
                            <Defaults>
                                <ext:Parameter Name="AllowBlank" Value="false" Mode="Raw" />
                                <ext:Parameter Name="MsgTarget" Value="side" />
                            </Defaults>
                            <Body>
                                <ext:FormLayout ID="FormLayout2" runat="server" LabelAlign="Left" LabelWidth="70" >
                                    <ext:Anchor Horizontal="92%">
                                        <ext:DateField ID="dfDate" runat="server" FieldLabel="考核时间" />
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:ComboBox ID="cbbCheckDept" runat="server" DisplayField="Deptname" 
                                            ValueField="Deptnumber"
                                            StoreID="DeptStore" Disabled="true" FieldLabel="考核单位" Editable="false" />
                                    </ext:Anchor>
                                    </ext:FormLayout>
                            </Body>
                        </ext:Panel>
                    </ext:LayoutColumn>
                    <ext:LayoutColumn ColumnWidth=".4">
                        <ext:Panel ID="Panel4" runat="server" Border="false" Header="false">
                            <Defaults>
                                <ext:Parameter Name="AllowBlank" Value="false" Mode="Raw" />
                                <ext:Parameter Name="MsgTarget" Value="side" />
                            </Defaults>
                            <Body>
                                <ext:FormLayout ID="FormLayout3" runat="server" LabelAlign="Left" LabelWidth="70" >
                                    <ext:Anchor Horizontal="92%">
                                        <ext:TextField ID="tfplace" runat="server" FieldLabel="考核地点" />
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:ComboBox ID="cbbXZ" runat="server" FieldLabel="考核性质">
                                            <Items>
                                                <ext:ListItem Text="专业检查" Value="专业检查" />
                                                <ext:ListItem Text="动态检查" Value="动态检查" />
                                                <ext:ListItem Text="季度联合验收" Value="季度联合验收" />
                                            </Items>
                                        </ext:ComboBox>
                                    </ext:Anchor>
                                </ext:FormLayout>
                            </Body>
                        </ext:Panel>
                    </ext:LayoutColumn>
            </ext:ColumnLayout>
        </Body>
    </ext:Panel>
    
    <ext:GridPanel 
        ID="gpESSENTIALCONDITION" 
        runat="server" 
        StoreID="ESSENTIALCONDITIONStore"
        StripeRows="true"
        Title="否决条件" Width="800"
        Icon="Cross"
        Collapsible="false" AutoExpandColumn="jcontent" AutoHeight="true"
        >
        <TopBar>
            <ext:Toolbar ID="Toolbar3" runat="server">
                <Items>
                    <ext:Label ID="lbldmsg" runat="server" StyleSpec="color:red;" Text="考核期内出现下列情形之一的，安全质量标准化不达标扣10分，得分不得超过80分（不含80分）：" />
                </Items>
            </ext:Toolbar>
        </TopBar>
        <ColumnModel ID="ColumnModel1" runat="server">
           <Columns>
               <ext:RowNumbererColumn />
               <ext:Column ColumnID="jcontent" Header="否决条件" DataIndex="Content" Width="50">
                   <Renderer Fn="qtip" />
               </ext:Column>
               <ext:CheckColumn Width="40" Header="操作" DataIndex="isCheck" Editable="true" />
           </Columns>
       </ColumnModel>
       <SaveMask ShowMask="true" Msg="正在保存数据，请稍候..." />
    </ext:GridPanel>
    <ext:GridPanel 
        ID="gpDEMOTION" 
        runat="server" 
        StoreID="DEMOTIONStore"
        StripeRows="true"
        Title="降级条件" Width="800"
        Icon="ArrowDown"
        Collapsible="false" AutoExpandColumn="jcontent" AutoHeight="true"
        >
        <TopBar>
            <ext:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <ext:Label ID="Label1" runat="server" StyleSpec="color:red;" Text="考核期内矿井存在以下情形之一的，安全质量标准化降一级扣5分，得分不得超过下一级的最高分：" />
                </Items>
            </ext:Toolbar>
        </TopBar>
        <ColumnModel ID="ColumnModel2" runat="server">
           <Columns>
               <ext:RowNumbererColumn />
               <ext:Column ColumnID="jcontent" Header="降级条件" DataIndex="Content">
                   <Renderer Fn="qtip" />
               </ext:Column>
               <ext:CheckColumn Width="40" Header="操作" DataIndex="isCheck" Editable="true" />
           </Columns>
       </ColumnModel>
       <SaveMask ShowMask="true" Msg="正在保存数据，请稍候..." />
    </ext:GridPanel>
    <ext:GridPanel 
        ID="gpJeom"
        runat="server" 
        StoreID="JeomStore"
        StripeRows="true"
        Title="考核评分" Width="800"
        Icon="Table"
        Collapsible="false" AutoExpandColumn="jcontent" AutoHeight="true"
        >
        <ColumnModel ID="ColumnModel3" runat="server">
           <Columns>
               <ext:Column ColumnID="jcontent" Header="考核内容" DataIndex="Jccontent" Groupable="false" Width="50">
                   <Renderer Fn="qtip" />
               </ext:Column>
                <ext:Column Header="标准分值" DataIndex="Score" Groupable="false" Width="65" />
                <ext:Column Header="评分办法" DataIndex="Means" Groupable="false" Width="150">
                   <Renderer Fn="qtip" />
               </ext:Column>
                <ext:Column Header="扣分原因" DataIndex="Remark" Width="120" RightCommandAlign="False">
                    <Editor>
                        <ext:TextArea runat="server" ID="tfremark" Height="90" />
                    </Editor>
                </ext:Column>
                <ext:Column Header="实际得分" DataIndex="Jeom" Width="80">
                    <Editor>
                        <ext:NumberField runat="server" ID="nfJeom" />
                    </Editor>
                </ext:Column>
                <ext:Column Header="考核标准分类" DataIndex="Kind"/>
           </Columns>
       </ColumnModel>
       <SaveMask ShowMask="true" Msg="正在保存数据，请稍候..." />
       <View><%--StartCollapsed="true"--%>
            <ext:GroupingView  
                ID="GroupingView1"
                Collapsible="false"
                HideGroupedColumn="true"
                runat="server" 
                ShowGroupName="false"
                GroupTextTpl='{text} ({[values.rs.length]} {["条"]})'
                EnableRowBody="true">
            </ext:GroupingView>
        </View>
        <Buttons>
            <ext:Button ID="Button1" runat="server" Text="数据提交" Icon="Disk">
                <Listeners>
                    <Click Handler="Coolite.AjaxMethods.BaseSave();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:GridPanel>
    
    

    </form>
</body>
</html>
