<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FinePersonSelect.aspx.cs" Inherits="YSNewProcess_FinePersonSelect" %>

<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" CleanResourceUrl="false" Locale="zh-CN" />
    <ext:Store ID="FkrenStore" runat="server">
        <Reader>
            <ext:JsonReader ReaderID="Personnumber">
                <Fields>
                    <ext:RecordField Name="Personnumber" />
                    <ext:RecordField Name="Name" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:FormPanel ID="FormPanel1" runat="server" BodyStyle="padding:5px;" ButtonAlign="Right" LabelWidth="65"
        Frame="true" Header="false" AutoHeight="true">
        <Defaults>
            <ext:Parameter Name="Anchor" Value="93%" Mode="Value" />
        </Defaults>
        <TopBar>
            <ext:Toolbar runat="server" ID="tb_1">
                <Items>
                    <ext:Label runat="server" ID="lblmsg_1" StyleSpec="color:red;" Html="罚款说明：提交后只保存不做处罚，<p>处罚确认后才能使处罚生效！" />
                </Items>
            </ext:Toolbar>
        </TopBar>
        <Body>
            <ext:FormLayout ID="FormLayout1" runat="server">
            </ext:FormLayout>
        </Body>
        <Buttons>
            <ext:Button ID="btnSubmit" runat="server" Icon="Disk" Text="提 交">
                <AjaxEvents>
                    <Click OnEvent="SubmitPerson" Method="POST">
                        <EventMask Msg="数据处理中.." ShowMask="true" />
                        <ExtraParams />
                    </Click>
                </AjaxEvents>
            </ext:Button>
            <ext:Button ID="Button1" runat="server" Icon="DiskMagnify" Text="处罚确认">
                <Listeners>
                    <Click Handler="Coolite.AjaxMethods.FineSure();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:FormPanel>
    </form>
</body>
</html>
