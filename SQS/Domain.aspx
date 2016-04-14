<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Domain.aspx.cs" Inherits="SQS_Domain" %>

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
        
        function nodeLoad(node) {
            Coolite.AjaxMethods.NodeLoad(node.id, {
                success: function(result) {
                    var data = eval("(" + result + ")");
                    node.loadNodes(data);
                },

                failure: function(errorMsg) {
                    Ext.Msg.alert('Failure', errorMsg);
                }
            });            
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN" CleanResourceUrl="false" />
    
    <ext:Store ID="ItemStore" runat="server" >
        <Reader>
            <ext:JsonReader ReaderID="Pkindid">
                <Fields>
                    <ext:RecordField Name="Pkindid" Type="Int" />
                    <ext:RecordField Name="Pkindname" />
                    <ext:RecordField Name="Fullscore" />
                    <ext:RecordField Name="Rate" />
                    <ext:RecordField Name="Indate" />
                    <ext:RecordField Name="Status" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Store ID="JeomStore" runat="server" GroupField="Kind" >
        <Reader>
            <ext:JsonReader ReaderID="Jcid">
                <Fields>
                    <ext:RecordField Name="Jcid" />
                    <ext:RecordField Name="Jccontent" />
                    <ext:RecordField Name="Indate" Type="Date" DateFormat="Y-m-dTh:i:s"  />
                    <ext:RecordField Name="Score" />
                    <ext:RecordField Name="Means" />
                    <ext:RecordField Name="Kind" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    
    <ext:Menu ID="cmenu" runat="server"> 
        <Items>
             <ext:MenuItem ID="copyItems" runat="server" Text="添加子类" Icon="Add">
               <Listeners>
                     <Click Handler="Coolite.AjaxMethods.readInfo(this.parentMenu.node==null?'-1':this.parentMenu.node.id,'add');" />                    
                 </Listeners>    
              </ext:MenuItem>
             <ext:MenuItem ID="editItems" runat="server" Text="修改类别" Icon="Anchor">
              <Listeners>
                    <Click Handler="Coolite.AjaxMethods.readInfo(this.parentMenu.node==null?'-1':this.parentMenu.node.id,'edit');" />
              </Listeners>
             </ext:MenuItem>
             <ext:MenuItem ID="moveItems" runat="server" Text="删除类别" Icon="Delete">
                 <Listeners>
                      <Click Handler="Coolite.AjaxMethods.del(this.parentMenu.node==null?'-1':this.parentMenu.node.id,'del');" />
              </Listeners>
            </ext:MenuItem>
        </Items>
    </ext:Menu>
    
    <ext:Hidden ID="hdnKindid" runat="server" Text="0"/>
    
    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                <West Collapsible="true" Split="true">
                    <ext:TreePanel ID="tpkind" runat="server" Title="考核专业" Border="false" ContextMenuID="cmenu" Width="230" RootVisible="false">
                        <TopBar>
                            <ext:Toolbar ID="Toolbar1" runat="server">
                                <Items>
                                    <ext:ToolbarButton ID="ToolbarButton3" runat="server" Icon="Add" Text="添加专业分类">
                                        <Listeners>
                                            <Click Handler="Coolite.AjaxMethods.readInfo('-1','new');" />
                                        </Listeners>
                                    </ext:ToolbarButton>
                                     <ext:ToolbarFill/>
                                     <ext:ToolbarButton ID="ToolbarButton1" runat="server" Icon="ArrowNwNeSwSe">
                                        <Listeners>
                                            <Click Handler="#{tpkind}.root.expand(true);" />
                                        </Listeners>
                                        <ToolTips>
                                            <ext:ToolTip ID="ToolTip1" IDMode="Ignore" runat="server" Html="全部展开" />
                                        </ToolTips>
                                    </ext:ToolbarButton>
                                    <ext:ToolbarButton ID="ToolbarButton2" runat="server" Icon="ArrowInLonger">
                                        <Listeners>
                                            <Click Handler="#{tpkind}.root.collapse(true);" />
                                        </Listeners>
                                        <ToolTips>
                                            <ext:ToolTip ID="ToolTip2" IDMode="Ignore" runat="server" Html="全部合并" />
                                        </ToolTips>
                                    </ext:ToolbarButton>
                                </Items>
                            </ext:Toolbar>
                          </TopBar>
                            <Listeners> 
                                <ContextMenu  Handler="node.select();#{moveItems}.Disabled=!node.leaf;#{cmenu}.node=node;#{cmenu}.showAt(e.getPoint());" />
                                <BeforeLoad Fn="nodeLoad" />
                                 <%--<Click Handler="addTab(node);" />--%> 
                            </Listeners>
                           <Root>
                               <ext:TreeNode NodeID="-1" Text="专业管理" />
                            </Root>
                    </ext:TreePanel>

                </West>
                <Center>
                    <ext:Panel ID="Panel1" runat="server" Header="false">
                        <Body>
                            <ext:BorderLayout runat="server" ID="bl2">
                                <West>
                                    <ext:GridPanel 
                                        ID="gpItem" 
                                        runat="server"
                                        StripeRows="true"
                                        Title="考核项目" StoreID="ItemStore"
                                        Collapsible="false" Width="230"
                                        >
                                        <TopBar>
                                            <ext:Toolbar ID="Toolbar2" runat="server">
                                                <Items>
                                                    <ext:ToolbarButton ID="btnItemAdd" runat="server" Icon="Add" Text="新增" Disabled="true">
                                                        <Listeners>
                                                            <Click Handler="Coolite.AjaxMethods.ItemInfo('new');" />
                                                        </Listeners>
                                                    </ext:ToolbarButton>
                                                    <ext:ToolbarSeparator />
                                                    <ext:ToolbarButton ID="btnItemEdit" runat="server" Icon="FolderEdit" Text="修改" Disabled="true">
                                                        <Listeners>
                                                            <Click Handler="Coolite.AjaxMethods.ItemInfo('edit');" />
                                                        </Listeners>
                                                    </ext:ToolbarButton>
                                                    <ext:ToolbarButton ID="btnItemDel" runat="server" Icon="Delete" Text="删除" Disabled="true">
                                                        <Listeners>
                                                            <Click Handler="Coolite.AjaxMethods.Itemdel();" />
                                                        </Listeners>
                                                    </ext:ToolbarButton>
                                                </Items>
                                            </ext:Toolbar>
                                          </TopBar>
                                        <ColumnModel ID="ColumnModel2" runat="server">
                                            <Columns>
                                                <ext:Column Header="项目分类名称" DataIndex="Pkindname" Width="225" />
                                                <%--<ext:ImageCommandColumn Header="操作" Width="50">
                                                    <Commands>
                                                        <ext:ImageCommand Icon="ArrowUp" ToolTip-Text="上移" CommandName="moveup" />
                                                        <ext:ImageCommand Icon="ArrowDown" ToolTip-Text="下移" CommandName="movedown" />
                                                    </Commands>
                                                    <PrepareCommand Fn="prepareCommand" />
                                                </ext:ImageCommandColumn>--%>
                                            </Columns>
                                        </ColumnModel>
                                        <%--<Listeners>
                                            <Command Handler="Coolite.AjaxMethods.MoveItem(command, record.data.Common);" />
                                        </Listeners>--%>
                                        <AjaxEvents>
                                            <Click OnEvent="ItemRowClick" />
                                        </AjaxEvents>
                                        <LoadMask ShowMask="true" />
            
                                        <SelectionModel>
                                            <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                                <Listeners>
                                                    <RowSelect Handler="#{fpItem}.getForm().loadRecord(record);" />
                                                </Listeners>
                                            </ext:RowSelectionModel>
                                        </SelectionModel>

                                    </ext:GridPanel>
                                </West>
                                <Center>
                                    <ext:GridPanel 
                                        ID="gpJoem" 
                                        runat="server" 
                                        StoreID="JeomStore"
                                        StripeRows="true"
                                        Title="考核标准"
                                        Icon="BrickMagnify"
                                        Collapsible="false" AutoExpandColumn="jcontent"
                                        >
                                        <ColumnModel ID="ColumnModel1" runat="server">
                                            <Columns>
                                                <ext:Column ColumnID="jcontent" Header="考核内容" DataIndex="Jccontent" Groupable="false" Width="50" />
                                                <ext:Column Header="标准分值" DataIndex="Score" Groupable="false" Width="80" />
                                                <ext:Column Header="考核办法" DataIndex="Means" Groupable="false" Width="100" />
                                                <ext:Column Header="录入日期" DataIndex="Indate" Groupable="false" Width="80">
                                                    <Renderer Fn="Ext.util.Format.dateRenderer('Y-m-d')" />
                                                </ext:Column>
                                                <ext:Column Header="考核标准分类" DataIndex="Kind"/>
                                            </Columns>
                                        </ColumnModel>
                                        <TopBar>
                                            <ext:Toolbar ID="Toolbar3" runat="server">
                                                <Items>
                                                    <ext:ToolbarButton ID="btnAddJNew" runat="server" Icon="Add" Text="新增考核标准" Disabled="true">
                                                        <Listeners>
                                                            <Click Handler="Coolite.AjaxMethods.JoemInfo('Jnew');" />
                                                        </Listeners>
                                                    </ext:ToolbarButton>
                                                    <ext:ToolbarButton ID="btnAddNNew" runat="server" Icon="Add" Text="新增否决条件" Disabled="true">
                                                        <Listeners>
                                                            <Click Handler="Coolite.AjaxMethods.JoemInfo('Nnew');" />
                                                        </Listeners>
                                                    </ext:ToolbarButton>
                                                    <ext:ToolbarButton ID="banAddDNew" runat="server" Icon="Add" Text="新增降级条件" Disabled="true">
                                                        <Listeners>
                                                            <Click Handler="Coolite.AjaxMethods.JoemInfo('Dnew');" />
                                                        </Listeners>
                                                    </ext:ToolbarButton>
                                                    <ext:ToolbarSeparator />
                                                    <ext:ToolbarButton ID="btnJoemUpdate" runat="server" Icon="FolderEdit" Text="修改" Disabled="true">
                                                        <Listeners>
                                                            <Click Handler="Coolite.AjaxMethods.JoemInfo('edit');" />
                                                        </Listeners>
                                                    </ext:ToolbarButton>
                                                    <ext:ToolbarButton ID="btnJoemDel" runat="server" Icon="Delete" Text="删除" Disabled="true">
                                                        <Listeners>
                                                            <Click Handler="Coolite.AjaxMethods.Joemdel();" />
                                                        </Listeners>
                                                    </ext:ToolbarButton>
                                                </Items>
                                            </ext:Toolbar>
                                          </TopBar>
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
                    </ext:Panel>
                    
                    
                </Center>
            </ext:BorderLayout>
        </Body>
    </ext:ViewPort>
    
    <ext:Window ID="Window1"  ShowOnLoad="false" 
    BodyStyle="padding:0pc" runat="server"   BodyBorder="false"
    Collapsible="false" Frame="false" Width="350" Modal="true"
    AutoHeight="true"  Title="专业管理">
         <LoadMask ShowMask="true" />
        <Body>   
            <ext:Hidden ID="hdnID" runat="server" Text="0"/>
            <ext:Hidden ID="hdnTreeParentID" runat="server" Text="0"/>
            <ext:FormPanel ID="FormPanel1" runat="server" BodyStyle="padding:1px;" ButtonAlign="Center"
                Frame="true" BodyBorder="false"   MonitorValid="true"  Header="false">
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
                       <ext:TextField ID="tfDNAME" FieldLabel="专业名称"  BlankText="专业名称不能为空！" runat="server"/> 
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:NumberField ID="nfFULLSCORE" runat="server" FieldLabel="满分分值" MaxValue="100" MinValue="0" />
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:NumberField ID="nfRATE" runat="server" FieldLabel="分值系数" MaxValue="1" MinValue="0" DecimalPrecision="3" />
                    </ext:Anchor>                        
                  </ext:FormLayout>
                </Body>
                <Buttons>
                    <ext:Button ID="btnUpdate" runat="server" Icon="Add" Text="更新">
                        <Listeners>
                            <Click Handler="if(!#{tfDNAME}.validate()){Ext.Msg.alert('提示','分类名称不能为空！'); return false;}" />
                        </Listeners>
                        <AjaxEvents>
                            <Click  OnEvent="btnUpdateClick">
                                <EventMask CustomTarget="={#{Window1}.body}"  Target="CustomTarget" ShowMask="true" MinDelay="20" />
                            </Click>
                        </AjaxEvents>
                    </ext:Button>
                     <ext:Button ID="Button2" runat="server" Icon="Cancel" Text="取消">
                        <Listeners>
                            <Click Handler="#{FormPanel1}.getForm().reset();#{Window1}.hide(null);" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:FormPanel>
                 
        </Body>

    </ext:Window>
    
    <ext:Window ID="ItemWindow"  ShowOnLoad="false" 
        BodyStyle="padding:0pc" runat="server"   BodyBorder="false"
        Collapsible="false" Frame="false" Width="350" Modal="true"
        AutoHeight="true"  Title="考核分类管理">
        <LoadMask ShowMask="true" />
        <Body>   
            <ext:Hidden ID="hdnItemid" runat="server" Text="0"/>
            <ext:FormPanel ID="fpItem" runat="server" BodyStyle="padding:1px;" ButtonAlign="Center"
                Frame="true" BodyBorder="false"   MonitorValid="true"  Header="false">
                <Defaults >
                    <ext:Parameter Name="anchor" Value="95%" Mode="Value" />
                    <ext:Parameter Name="msgTarget" Value="side" Mode="Value" />
                    <ext:Parameter Name="AllowBlank" Value="false" Mode="Raw" />
                </Defaults>
                <Listeners>
                    <ClientValidation Handler="#{btnItemUpdate}.setDisabled(!valid);" />
                </Listeners>
                <Body>
                    <ext:FormLayout ID="FormLayout2" runat="server"   LabelAlign="Left" LabelWidth="60">
                    <ext:Anchor>
                       <ext:TextField ID="tfPKINDNAME" FieldLabel="分类名称" DataIndex="Itemname"  BlankText="分类名称不能为空！" runat="server"/> 
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:NumberField ID="nfitemFullscore" runat="server" FieldLabel="满分分值" DataIndex="Fullscore" MaxValue="100" MinValue="0" />
                    </ext:Anchor> 
                    <ext:Anchor>
                        <ext:NumberField ID="nfitemRATE" runat="server" FieldLabel="分值系数" MaxValue="1" MinValue="0" DecimalPrecision="3" />
                    </ext:Anchor>                    
                  </ext:FormLayout>
                </Body>
                <Buttons>
                    <ext:Button ID="btnItemUpdate" runat="server" Icon="Add" Text="更新">
                        <Listeners>
                            <Click Handler="if(!#{tfPKINDNAME}.validate()){Ext.Msg.alert('提示','分类名称不能为空！'); return false;}" />
                        </Listeners>
                        <AjaxEvents>
                            <Click  OnEvent="btnItemUpdateClick">
                                <EventMask CustomTarget="={#{ItemWindow}.body}"  Target="CustomTarget" ShowMask="true" MinDelay="20" />
                            </Click>
                        </AjaxEvents>
                    </ext:Button>
                     <ext:Button ID="btnItemReset" runat="server" Icon="Cancel" Text="取消">
                        <Listeners>
                            <Click Handler="#{fpItem}.getForm().reset();#{ItemWindow}.hide(null);" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:FormPanel>
                 
        </Body>

    </ext:Window>
    
    <ext:Window ID="JoemWindow"  ShowOnLoad="false" 
        BodyStyle="padding:0pc" runat="server"   BodyBorder="false"
        Collapsible="false" Frame="false" Width="350" Modal="true"
        AutoHeight="true"  Title="考核标准管理">
        <LoadMask ShowMask="true" />
        <Body>   
            <ext:Hidden ID="hdnJoemid" runat="server" Text="0"/>
            <ext:FormPanel ID="fpJoem" runat="server" BodyStyle="padding:1px;" ButtonAlign="Center"
                Frame="true" BodyBorder="false"   MonitorValid="true"  Header="false">
                <Defaults >
                    <ext:Parameter Name="anchor" Value="95%" Mode="Value" />
                    <ext:Parameter Name="msgTarget" Value="side" Mode="Value" />
                    <ext:Parameter Name="AllowBlank" Value="false" Mode="Raw" />
                </Defaults>
                <Listeners>
                    <ClientValidation Handler="#{btnJoemUpdateClick}.setDisabled(!valid);" />
                </Listeners>
                <Body>
                    <ext:FormLayout ID="FormLayout3" runat="server"   LabelAlign="Left" LabelWidth="60">
                    <ext:Anchor>
                       <ext:TextArea ID="tfJccontent" FieldLabel="考核内容" DataIndex="Jccontent"  BlankText="考核内容不能为空！" Width="65" runat="server"/> 
                    </ext:Anchor>
                    <ext:Anchor>
                        <ext:NumberField ID="nfScore" runat="server" FieldLabel="标准分值" MaxValue="100" MinValue="0" />
                    </ext:Anchor> 
                    <ext:Anchor>
                       <ext:TextArea ID="taMEANS" FieldLabel="评分办法" DataIndex="Means"  BlankText="评分办法不能为空！" Width="65" runat="server"/> 
                    </ext:Anchor>                   
                  </ext:FormLayout>
                </Body>
                <Buttons>
                    <ext:Button ID="benJoemUpdate" runat="server" Icon="Add" Text="更新">
                        <Listeners>
                            <Click Handler="if(!#{tfJccontent}.validate()){Ext.Msg.alert('提示','考核内容不能为空！'); return false;}" />
                        </Listeners>
                        <AjaxEvents>
                            <Click  OnEvent="btnJoemUpdateClick">
                                <EventMask CustomTarget="={#{JoemWindow}.body}"  Target="CustomTarget" ShowMask="true" MinDelay="20" />
                            </Click>
                        </AjaxEvents>
                    </ext:Button>
                     <ext:Button ID="Button3" runat="server" Icon="Cancel" Text="取消">
                        <Listeners>
                            <Click Handler="#{fpJoem}.getForm().reset();#{JoemWindow}.hide(null);" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:FormPanel>
                 
        </Body>

    </ext:Window>
    </form>
</body>
</html>
