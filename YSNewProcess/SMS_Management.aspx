<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SMS_Management.aspx.cs" Inherits="YSNewProcess_SMS_Management" %>

<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN" CleanResourceUrl="false" />

    <ext:Store ID="ManagementStore" runat="server" OnBeforeStoreChanged="SaveD">
        <Reader>
            <ext:JsonReader ReaderID="Coding">
                <Fields>
                    <ext:RecordField Name="Coding"/>
                    <ext:RecordField Name="Functionname" />
                    <ext:RecordField Name="Flevel" />
                    <ext:RecordField Name="Mid" />
                    <ext:RecordField Name="isCheck" Type="Boolean" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>

    <ext:ViewPort ID="ViewPort1" runat="server">
        <Body>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                <Center>
                    <ext:GridPanel 
                            ID="gpDetail" 
                            runat="server" 
                            StoreID="ManagementStore"
                            StripeRows="true" Border="false"
                            Title="短信控制" Width="575" Height="190" AutoScroll="true"
                            Collapsible="false" AutoExpandColumn="jcontent" ClicksToEdit="1"
                            >
                            <ColumnModel ID="ColumnModel2" runat="server">
                               <Columns>
                                   <ext:RowNumbererColumn />
                                   <ext:Column ColumnID="jcontent" Header="功能信息" DataIndex="Functionname">
                                   </ext:Column>
                                   <ext:CheckColumn Width="40" Header="操作" DataIndex="isCheck" Editable="true" />
                               </Columns>
                           </ColumnModel>
                           <SaveMask ShowMask="true" Msg="正在保存数据，请稍候..." />
                           <Buttons>
                                <ext:Button runat="server" ID="btnSave" Text="保存" Icon="Disk">
                                    <Listeners>
                                        <Click Handler="#{gpDetail}.save();" />
                                    </Listeners>
                                </ext:Button>
                            </Buttons>
                        </ext:GridPanel>
                </Center>
            </ext:BorderLayout>
        </Body>
    </ext:ViewPort>
    </form>
</body>
</html>
