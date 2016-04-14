<%@ Page Language="C#" AutoEventWireup="true" CodeFile="YHSmsSet.aspx.cs" Inherits="SafeCheckSet_YHSmsSet" %>
<%@ Register Assembly="ALinq.Web, Version=1.0.8.0, Culture=neutral, PublicKeyToken=2b23f34316d38f3a"
    Namespace="ALinq.Web.Controls" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dxwgv" %>

<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>
<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>隐患短信发送设置</title>
  
    <script type="text/javascript">
        function PopSendingWin() {
            var url = "AddSmsReceiver.aspx";
            window.showModalDialog(url, window, "dialogHeight:750px;dialogWidth:880px;dialogLeft:200px;dialogTop:10px;scroll:auto");
    }
    </script> 
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN" CleanResourceUrl="false" />
    <table borderColor="#fffbec" cellSpacing="0" cellPadding="0" width="100%" align="center" bgColor="#fffbec" border="1" frame="void">
        <tbody>
        <tr align="left">
        <td height="20">你现在的位置是：安全考核设置->隐患短信设置</td></tr>
        </tbody>
    </table>
    <fieldset id="Fieldset1" class="Fieldsetbox mbox pbox">
        <legend class="fsTitle">功能操作</legend>
        <table>
            <tr>
            <td>
<%--                <asp:Button ID="btnAddPsn" runat="server" Text="添加人员" 
                    onclientclick="PopSendingWin();return false;" />--%>
                    <ext:Button runat="server" ID="btnAdd" Text="添加人员"> 
                    <Listeners>
                        <Click Handler="parent.window.loadnewpage('添加人员','SafeCheckSet/AddSmsReceiver.aspx');" />
                    <%--<Click Handler="parent.window.Window1.reload();parent.window.Window1.show();" />--%>
                    <%--#{Window1}.reload();#{Window1}.show();--%>
                    </Listeners>
                    </ext:Button>  
             </td>
                    
            </tr>
        </table>
        
    </fieldset>
    <ext:Window ID="Window1"  ShowOnLoad="false" 
    BodyStyle="padding:0pc" runat="server"   BodyBorder="false" CloseAction="Hide"
    Collapsible="false" Frame="true" Modal="true" Width="880"  Height="600" AutoScroll="true" Resizable="false"
     Title="添加人员">
        <AutoLoad Mode="IFrame" Url="AddSmsReceiver.aspx" />
         <LoadMask ShowMask="true" Msg="数据加载中...." />
         <Listeners>
            <%--<BeforeShow Fn="function(el) { el.setHeight(Ext.getBody().getViewSize().height-20);el.setWidth(Ext.getBody().getViewSize().width-2); }" />--%>
            <BeforeHide Handler="window.location.reload();"/>
         </Listeners>
    </ext:Window>
    <div>
        <dxwgv:ASPxGridView ID="gvYHSmsSet" runat="server" 
            CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
            CssPostfix="Office2003_Blue" Width="100%" AutoGenerateColumns="False" 
            KeyFieldName="YHSMSSETID" onrowdeleting="gvYHSmsSet_RowDeleting">
            <SettingsBehavior ConfirmDelete="True" />
            <Styles CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
                CssPostfix="Office2003_Blue">
                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                </Header>
                <LoadingPanel ImageSpacing="10px">
                </LoadingPanel>
            </Styles>
            <Images ImageFolder="~/App_Themes/Office2003Blue/{0}/">
                <CollapsedButton Height="12px" 
                    Url="~/App_Themes/Office2003Blue/GridView/gvCollapsedButton.png" Width="11px" />
                <ExpandedButton Height="12px" 
                    Url="~/App_Themes/Office2003Blue/GridView/gvExpandedButton.png" Width="11px" />
                <DetailCollapsedButton Height="12px" 
                    Url="~/App_Themes/Office2003Blue/GridView/gvCollapsedButton.png" Width="11px" />
                <DetailExpandedButton Height="12px" 
                    Url="~/App_Themes/Office2003Blue/GridView/gvExpandedButton.png" Width="11px" />
                <FilterRowButton Height="13px" Width="13px" />
            </Images>
            <Columns>
                <dxwgv:GridViewDataTextColumn Caption="自动编号" FieldName="YHSMSSETID" 
                    VisibleIndex="0" Visible="False">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="隐患级别" FieldName="INFONAME" 
                    VisibleIndex="0">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="人员" FieldName="NAME" VisibleIndex="2">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="部门" FieldName="DEPTNAME" VisibleIndex="1">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewCommandColumn Caption="操作" VisibleIndex="4">
                    <DeleteButton Visible="True">
                    </DeleteButton>
                </dxwgv:GridViewCommandColumn>
            </Columns>
            <Settings ShowGroupPanel="True" />
            <StylesEditors>
                <ProgressBar Height="25px">
                </ProgressBar>
            </StylesEditors>
        </dxwgv:ASPxGridView>
    </div>
    <cc1:ALinqDataSource ID="adsYHSmsPSet" runat="server" 
            ContextTypeName="GhtnTech.SEP.DAL.DBSCMDataContext" TableName="Yhsmsset" 
            EnableViewState="False" EnableDelete="true">
        </cc1:ALinqDataSource>
         <cc1:ALinqDataSource ID="adsYHLevel" runat="server" 
            ContextTypeName="GhtnTech.SEP.DAL.DBSCMDataContext" TableName="CsBaseinfoset" 
            EnableViewState="False" Select="new (Infoid, Infoname, Fid)" 
        Where="Fid == @Fid" >
             <WhereParameters>
                 <asp:Parameter DefaultValue="41" Name="Fid" Type="Decimal" />
             </WhereParameters>
        </cc1:ALinqDataSource>
         <cc1:ALinqDataSource ID="adsPsn" runat="server" 
            ContextTypeName="GhtnTech.SEP.DAL.DBSCMDataContext" TableName="Person" 
            EnableViewState="False" Select="new (Personnumber, Name)" >
        </cc1:ALinqDataSource>
        
    </form>
</body>
</html>
