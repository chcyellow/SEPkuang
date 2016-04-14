<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkINJURY.aspx.cs" Inherits="GSSG_WorkINJURY" %>

<%@ Register assembly="Coolite.Ext.Web" namespace="Coolite.Ext.Web" tagprefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript">
        var qtip = function(v, p) {//单元格提示
            //v : value , p : cell
            p.attr = 'ext:qtitle="" ext:qtip="' + v + '"';
            return v;
        }

        var template = '<span style="color:{0};">{1}</span>';

        var change = function(value) {
            var color;
            if (value == '新增')
                color = 'red';
            else
                color = 'green';
            return String.format(template, color, value);
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
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN" CleanResourceUrl="false" />
        <ext:Hidden ID="GridData" runat="server" />
        <ext:Store ID="personStore" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="pernID" />
                        <ext:RecordField Name="pernName" />
                        <ext:RecordField Name="persNnmber" />
                        <ext:RecordField Name="pernLightNumber" />
                        <ext:RecordField Name="DeptName" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="placeStore" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="placID" />
                        <ext:RecordField Name="placName" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="sglxStore" runat="server">
            <Reader>
            <ext:JsonReader ReaderID="INFOID">
                <Fields>
                    <ext:RecordField Name="INFOID" />
                    <ext:RecordField Name="INFONAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
        </ext:Store>
        <ext:Store ID="gsdjStore" runat="server">
            <Reader>
            <ext:JsonReader ReaderID="INFOID">
                <Fields>
                    <ext:RecordField Name="INFOID" />
                    <ext:RecordField Name="INFONAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
        </ext:Store>
        
        <ext:Store ID="ssbwStore" runat="server">
            <Reader>
            <ext:JsonReader ReaderID="INFOID">
                <Fields>
                    <ext:RecordField Name="INFOID" />
                    <ext:RecordField Name="INFONAME" />
                </Fields>
            </ext:JsonReader>
        </Reader>
        </ext:Store>
        
        <ext:Store ID="GSStore" runat="server">
        <Reader>
                <ext:JsonReader ReaderID="Id">
                    <Fields>
                        <ext:RecordField Name="Id"/> 
                        <ext:RecordField Name="Name" />
                        <ext:RecordField Name="Deptname" />
                        <%--<ext:RecordField Name="Postname" />--%>
                       
                        <ext:RecordField Name="Placename" />
                        <ext:RecordField Name="Happendate" Type="Date" DateFormat="Y-m-dTh:i:s" />
                        <ext:RecordField Name="Indate" Type="Date" DateFormat="Y-m-dTh:i:s" />
                        <ext:RecordField Name="Infoname"/>  
                        <ext:RecordField Name="GsFact"/>  
                        
                         <ext:RecordField Name="PointsPer"/>  
                        <ext:RecordField Name="FinePer"/>   
                        
                         <ext:RecordField Name="PointsDept"/>  
                        <ext:RecordField Name="FineDept"/>  
                         <ext:RecordField Name="inPersonName"/> 
                         
                         
                          <ext:RecordField Name="sgleixing"/>  
                        <ext:RecordField Name="Banci"/>   
                        
                         <ext:RecordField Name="Injurysite"/>  
                        <ext:RecordField Name="Reason"/>  
                         <ext:RecordField Name="PointsBz"/>                                                                                                                           
                    </Fields>
                   
                </ext:JsonReader>
            </Reader>
        </ext:Store>
    <ext:Hidden ID="Hidden1" runat="server">
    </ext:Hidden>
    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                <North>
                    <ext:FormPanel 
            ID="FormPanel1" 
            runat="server" 
            Title="工伤录入系统"
            Frame="true" Height="330"
            BodyStyle="padding:5px;" 
            ButtonAlign="Center" style="font-size: 20px">
            <Body>
                <ext:ColumnLayout ID="ColumnLayout1" runat="server">
                    <ext:LayoutColumn ColumnWidth=".5">
                        <ext:Panel ID="Panel1" runat="server" Border="false" Header="false">
                            <Defaults>
                                <ext:Parameter Name="AllowBlank" Value="false" Mode="Raw" />
                                <ext:Parameter Name="MsgTarget" Value="side" />
                            </Defaults>
                            <Body>
                                <ext:FormLayout ID="FormLayout1" runat="server" LabelAlign="Left" LabelWidth="70" >
                                    <ext:Anchor Horizontal="92%">
                                        <ext:ComboBox 
                                            ID="cbbGsperson"
                                            runat="server" 
                                            StoreID="personStore"
                                            DisplayField="pernName" 
                                            ValueField="persNnmber"
                                            LoadingText="Searching..." 
                                            Width="570"
                                            ListWidth="350"
                                            PageSize="10"
                                            HideTrigger="true"
                                            FieldLabel="受伤人员<font color='red'>*</font>"
                                            ItemSelector="div.search-item"        
                                            > 
                                            <Template ID="Template1" runat="server">
                                                

                                               <tpl for=".">
                                                  <div class="search-item">
                                                     <h3><span>姓名:{pernName}</span><span>灯号：{pernLightNumber}</span>单位：{DeptName}</h3>
                                                     
                                                  </div>
                                               </tpl>

                                            </Template>
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
                                    </ext:Anchor>
                                    
                                    <ext:Anchor Horizontal="92%">
                                        
                                        <ext:ComboBox 
                                            ID="sglx"
                                            runat="server" 
                                            StoreID="sglxStore"
                                            DisplayField="INFONAME" 
                                            ValueField="INFOID"
                                            LoadingText="Searching..." 
                                            FieldLabel="事故类型<font color='red'>*</font>"
                                                 
                                            >
                                        </ext:ComboBox>
                                    </ext:Anchor>
                                    
                                    <ext:Anchor Horizontal="92%">
                                        
                                        <ext:ComboBox 
                                            ID="gsdj"
                                            runat="server" 
                                            StoreID="gsdjStore"
                                            DisplayField="INFONAME" 
                                            ValueField="INFOID"
                                            LoadingText="Searching..." 
                                            FieldLabel="工伤等级<font color='red'>*</font>"
                                                 
                                            >
                                        </ext:ComboBox>
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        
                                        <ext:ComboBox 
                                            ID="ssbw"
                                            runat="server" 
                                            StoreID="ssbwStore"
                                            DisplayField="INFONAME" 
                                            ValueField="INFOID"
                                            LoadingText="Searching..." 
                                            FieldLabel="受伤部位<font color='red'>*</font>"
                                                 
                                            >
                                        </ext:ComboBox>
                                    </ext:Anchor>
                                    
                                    <%--<ext:Anchor Horizontal="92%">
                                        <ext:TextField ID="ssbw" runat = "server" FieldLabel="受伤部位"></ext:TextField>
                                    </ext:Anchor>--%>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:TextArea ID="gsss" runat="server" FieldLabel="工伤事实<font color='red'>*</font>" Height="60">
                                        </ext:TextArea>
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:TextArea ID="zjyy" runat="server" FieldLabel="直接原因<font color='red'>*</font>" Height="60">
                                        </ext:TextArea>
                                    </ext:Anchor> 
                                </ext:FormLayout>
                            </Body>
                        </ext:Panel>
                    </ext:LayoutColumn>
                    <ext:LayoutColumn ColumnWidth=".5">
                        <ext:Panel ID="Panel2" runat="server" Border="false">
                            <Defaults>
                                
                                <ext:Parameter Name="MsgTarget" Value="side" />
                            </Defaults>
                            <Body>
                                <ext:FormLayout ID="FormLayout2" runat="server" LabelAlign="Left" LabelWidth="70">
                                   <ext:Anchor Horizontal="92%">
                                        
                                        <ext:ComboBox 
                                            ID="cbbplace"
                                            runat="server" 
                                            StoreID="placeStore"
                                            DisplayField="placName" 
                                            ValueField="placID"
                                            LoadingText="Searching..." 
                                            Width="570"
                                            PageSize="10"
                                            HideTrigger="true"
                                            FieldLabel="发生地点<font color='red'>*</font>"
                                            ItemSelector="div.search-item"        
                                            >
                                            <Template ID="Template3" runat="server">
                                            
                                               <tpl for=".">
                                                  <div class="search-item">
                                                     <span>{placName}</span>
                                                     
                                                  </div>
                                               </tpl>
                                            </Template>
                                            <Listeners>
                                                <Render Fn="function(f) {
                                                            f.el.on('keyup', function(e) {
                                                             if(window.event.keyCode==38 || window.event.keyCode==40 || window.event.keyCode==13){
                                                                return;
                                                             }
                                                             Coolite.AjaxMethods.PYsearch(f.getRawValue(), 'placeStore');
                                                            });
                                                            }
                                                            " />
                                            </Listeners>
                                        </ext:ComboBox>
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:DateField ID="Happentime" FieldLabel="发生时间<font color='red'>*</font>" runat="server" Vtype="daterange">
                                        </ext:DateField>
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                             <ext:ComboBox 
                                                ID="cbbBc"
                                                runat="server" 
                                                Editable="false"
                                                FieldLabel="发生班次<font color='red'>*</font>" SelectedIndex="0">
                                                <Items>
                                                <ext:ListItem Text="早班" Value="早班" />
                                                 <ext:ListItem Text="中班" Value="中班" />
                                                  <ext:ListItem Text="夜班" Value="夜班" />
                                                  
                                                </Items>
                                              
                                            </ext:ComboBox>
                                        </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:NumberField ID="grkf" runat="server" FieldLabel="个人扣分" Width="260px" />
                                    </ext:Anchor>
                                    <%--<ext:Anchor Horizontal="92%">
                                        
                                        <ext:NumberField ID="grfk" runat="server" FieldLabel="个人罚款" Width="260px"  Visible="false"/>
                                    </ext:Anchor>--%>
                                    <ext:Anchor Horizontal="92%">
                                        
                                        <ext:NumberField ID="bzkf" runat="server" FieldLabel="班组扣分" Width="260px" />
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                    
                                      
                                        <ext:NumberField ID="bmkf" runat="server" FieldLabel="科区扣分" Width="260px" />
                                    </ext:Anchor>
                                    <ext:Anchor Horizontal="92%">
                                        
                                        <ext:NumberField ID="bmfk" runat="server" FieldLabel="部门罚款" Width="260px" />
                                    </ext:Anchor>
                                    <%--<ext:Anchor Horizontal="92%">
                                        <ext:ComboBox 
                                            ID="cbbPerson"
                                            runat="server"
                                            ReadOnly="true"
                                            PageSize="10"
                                            FieldLabel="录入人员"        
                                            HideTrigger="true">
                                        </ext:ComboBox>
                                    </ext:Anchor>--%>
                                    <ext:Anchor Horizontal="92%">
                                        <ext:TextArea ID="bz" runat="server" FieldLabel="备注" Height="45">
                                        </ext:TextArea>
                                    </ext:Anchor>
                                </ext:FormLayout>
                            </Body>
                        </ext:Panel>
                    </ext:LayoutColumn>
                </ext:ColumnLayout>
            </Body>
            <Buttons>
                <ext:Button runat="server" ID="btnAdd" Text="提交" Icon="Add">
                    <Listeners>
                        <Click Handler="Coolite.AjaxMethods.AddClick('new');" />
                    </Listeners>
                </ext:Button>
                <ext:Button runat="server" ID="btnEdit" Text="修改" Icon="Disk">
                    <Listeners>
                        <Click Handler="Coolite.AjaxMethods.AddClick('edit');" />
                    </Listeners>
                </ext:Button>
                <ext:Button runat="server" ID="btnDelete" Text="删除" Icon="Delete">
                    <Listeners>
                        <Click Handler="Coolite.AjaxMethods.ClearClick();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
        </ext:FormPanel>
                </North>
                <Center>
                    <ext:GridPanel ID="GridPanel1" runat="server"  Title="工伤信息" StoreID="GSStore" AutoScroll="true" StripeRows="true">
            <ColumnModel ID="ColumnModel1" runat="server" >
                <Columns>
                    <ext:Column  Header="编号" DataIndex="Id" >
                    </ext:Column>
                     <ext:Column  Header="部门" Sortable="true" Align="Center"
                        DataIndex="Deptname" >
                    </ext:Column>
                    <ext:Column  Header="受伤人员" Sortable="true" Align="Center" 
                        DataIndex="Name" >
                    </ext:Column>
                   
                    <%--<ext:Column  Header="岗位" Sortable="true" Align="Center"
                        DataIndex="Postname" >
                    </ext:Column>--%>
                       <ext:Column Header="工伤等级" Sortable="true"  DataIndex="Infoname" Align="Center">
                    </ext:Column>
                    <ext:Column Header="工伤事实" Sortable="true" DataIndex="GsFact" Align="Center">
                        <Renderer Fn="qtip" />
                    </ext:Column>
                    <%--<ext:Column Header="直接原因" Sortable="true" DataIndex="Reason" Align="Center">
                        <Renderer Fn="qtip" />
                    </ext:Column>--%>
                     <%--<ext:Column Header="受伤部位" Sortable="true" DataIndex="Injurysite" Align="Center">
                        <Renderer Fn="qtip" />
                    </ext:Column>--%>
                 
                     <%--<ext:Column Header="事故类型" Sortable="true"  DataIndex="sgleixing" Align="Center">
                    </ext:Column>--%>
                    <ext:Column  Header="发生时间" Sortable="true" Align="Center"
                        DataIndex="Happendate" >
                        <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" />
                    </ext:Column>
                    <ext:Column  Header="发生地点" Sortable="true" Align="Center"
                        DataIndex="Placename" >
                    </ext:Column>
                    <%--<ext:Column  Header="发生班次" Sortable="true" Align="Center"
                        DataIndex="Banci" >
                    </ext:Column>--%>
                    <ext:Column  Header="员工扣分" Sortable="true" Align="Center"
                        DataIndex="PointsPer" >
                    </ext:Column>
                    <%--<ext:Column  Header="班组扣分" Sortable="true" Align="Center"
                        DataIndex="PointsBz" >
                    </ext:Column>--%>
                    <%--<ext:Column  Header="员工罚款" Sortable="true" Align="Center" Hidden="true"
                        DataIndex="FinePer" >
                    </ext:Column>--%>
                    <ext:Column  Header="科区扣分" Sortable="true" Align="Center"
                        DataIndex="PointsDept" >
                    </ext:Column>
                    <ext:Column  Header="部门罚款" Sortable="true" Align="Center"
                        DataIndex="FineDept" >
                    </ext:Column>
                    <ext:Column  Header="录入人员" Sortable="true" Align="Center"
                        DataIndex="inPersonName" >
                    </ext:Column>
                   <%-- <ext:Column  Header="录入时间" Sortable="true" Align="Center"
                        DataIndex="Indate" >
                        <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" />
                    </ext:Column>--%>
                </Columns> 
            </ColumnModel>
            <View>
                <ext:GridView ForceFit="true" />
            </View>
               
            <LoadMask ShowMask="true" />
            
            <BottomBar>
                <ext:PagingToolBar ID="PagingToolBar1" runat="server" PageSize="10" StoreID="GSStore" />
            </BottomBar>
        <SelectionModel>
                <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" SingleSelect="true" runat="server" />                   
        </SelectionModel>
        <AjaxEvents>
            <Click OnEvent="RowClick"></Click>
        </AjaxEvents>
        </ext:GridPanel> 
                </Center>
            </ext:BorderLayout>
        </Body>
    </ext:ViewPort>
    </form>
</body>
</html>

