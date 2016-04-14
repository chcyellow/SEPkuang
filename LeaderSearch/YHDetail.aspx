<%@ Page Language="C#" AutoEventWireup="true" CodeFile="YHDetail.aspx.cs" Inherits="YHDetail" %>
<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>隐患明细信息</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style=" text-align:center">
    <ext:ScriptManager ID="ScriptManager1" runat="server" Locale="zh-CN" CleanResourceUrl="false" />
    
                <ext:Panel 
        ID="DetailWindow" 
        runat="server" 
        BodyStyle="padding:6px;" 
        ButtonAlign="Right"
        Frame="true" 
        Title="隐患明细信息"
        Height="400" AutoScroll="true" Resizable="false"
        Width="600px"
        ShowOnLoad="false"
        Y="1">
        <Tools>
            <ext:Tool Type="Refresh" Handler="Coolite.AjaxMethods.DetailLoad();" />
        </Tools>
        <LoadMask Msg="信息正在加载，请稍候...." ShowMask="true" />
        <Body>
            <ext:ContainerLayout ID="ContainerLayout1" runat="server">
                <ext:Panel 
                    ID="BasePanel" 
                    runat="server" 
                    Title="隐患基本信息" 
                    AutoHeight="true" 
                    FormGroup="true"
                    Width="580px"
                    >
                    <Body>
                        <table style="width:100%;">
                            <tr>
                                <td style="width:33%;">
                                    <span class="x-label-text">隐患编号:</span>
                                    <ext:TextField ID="lbl_YHPutinID" runat="server" Cls="x-textfeild-style" />
                                </td>
                                <td style="width:34%;">
                                    <span class="x-label-text">隐患部门:</span>
                                    <ext:TextField ID="lbl_DeptName" runat="server" Cls="x-textfeild-style" />
                                </td>
                                <td style="width:33%;">
                                    <span class="x-label-text">隐患地点:</span>
                                    <ext:TextField ID="lbl_PlaceName" runat="server" Cls="x-textfeild-style" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width:33%;">
                                    <span class="x-label-text">发生班次:</span>
                                    <ext:TextField ID="lbl_BanCi" runat="server" Cls="x-textfeild-style" />
                                </td>
                                <td style="width:34%;">
                                    <span class="x-label-text">排查人员:</span>
                                    <ext:TextField ID="lbl_rName" runat="server" Cls="x-textfeild-style" />
                                </td>
                                <td style="width:33%;">
                                    <span class="x-label-text">排查时间:</span>
                                    <ext:TextField ID="lbl_PCTime" runat="server" Cls="x-textfeild-style" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width:33%;">
                                    <span class="x-label-text">隐患级别:</span>
                                    <ext:TextField ID="lbl_YHLevel" runat="server" Cls="x-textfeild-style" />
                                </td>
                                <td style="width:34%;">
                                    <span class="x-label-text">隐患类型:</span>
                                    <ext:TextField ID="lbl_YHType" runat="server" Cls="x-textfeild-style" />
                                </td>
                                <td style="width:33%;">
                                    <span class="x-label-text">当前状态:</span>
                                    <ext:TextField ID="lbl_Status" runat="server" Cls="x-textfeild-style" />
                                </td>
                            </tr>
                            <tr>
                                 <td colspan="3">
                                    <span class="x-label-text">隐患内容:</span>
                                    <ext:TextArea ID="lbl_YHContent" runat="server" Width="500px" Height="40px" />
                                </td>
                            </tr>
                            <tr>
                                 <td colspan="3">
                                    <span class="x-label-text">备注信息:</span>
                                    <ext:TextArea ID="lbl_Remarks" runat="server" Width="500px" Height="40px" />
                                </td>
                            </tr>  
                        </table>
                    </Body>
                </ext:Panel>
                <ext:Panel 
                    ID="Panel1" 
                    runat="server" 
                    Title="隐患整改信息" 
                    AutoHeight="true" 
                    FormGroup="true"
                    Width="580px"
                    Collapsed="True">
                    <Body>
                        <table style="width:100%;">
                            <tr>
                                 <td colspan="3">
                                    <span class="x-label-text">整改措施:</span>
                                    <ext:TextArea ID="zg_Measures" runat="server" Width="500px" Height="40px" />
                                </td>
                            </tr>
                            <tr>
                                 <td colspan="3">
                                    <span class="x-label-text">领导批示:</span>
                                    <ext:TextArea ID="zg_Instructions" runat="server" Width="500px" Height="40px" />
                                </td>
                            </tr> 
                            <tr>
                                <td style="width:33%;">
                                    <span class="x-label-text">批示时间:</span>
                                    <ext:TextField ID="zg_InstrTime" runat="server" Cls="x-textfeild-style" />
                                </td>
                                <td style="width:34%;">
                                    <span class="x-label-text">责任单位:</span>
                                    <ext:TextField ID="zg_zrdw" runat="server" Cls="x-textfeild-style" />
                                </td>
                                <td style="width:33%;">
                                    <span class="x-label-text">责任人:</span>
                                    <ext:TextField ID="zg_PersonID" runat="server" Cls="x-textfeild-style" />
                                </td>
                                
                            </tr>
                            <tr>
                                <td style="width:33%;">
                                    <span class="x-label-text">整改期限:</span>
                                    <ext:TextField ID="zg_RecLimit" runat="server" Cls="x-textfeild-style" />
                                </td>
                                <td style="width:34%;">
                                    <span class="x-label-text">整改班次:</span>
                                    <ext:TextField ID="zg_BanCi" runat="server" Cls="x-textfeild-style" />
                                </td>
                                <td style="width:33%;">
                                   
                                </td>
                            </tr>
                        </table>
                    </Body>
                </ext:Panel>
                <ext:Panel 
                    ID="ZGPanel" 
                    runat="server" 
                    Title="整改反馈信息" 
                    AutoHeight="true" 
                    FormGroup="true"
                    Collapsed="True">
                    <Body>
                        <table style="width:100%;">
                            <tr>
                                <td style="width:33%;">
                                    <span class="x-label-text">整改情况:</span>
                                    <ext:TextField ID="zfk_RecState" runat="server" Cls="x-textfeild-style" />
                                </td>
                                <td style="width:34%;">
                                    <span class="x-label-text">整改时间:</span>
                                    <ext:TextField ID="zfk_RecTime" runat="server" Cls="x-textfeild-style" />
                                </td>
                                <td style="width:33%;">
                                    <span class="x-label-text">整改班次:</span>
                                    <ext:TextField ID="zfk_bc" runat="server" Cls="x-textfeild-style" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width:33%;">
                                    <span class="x-label-text">整 改 人:</span>
                                    <ext:TextField ID="zfk_RecPersonID" runat="server" Cls="x-textfeild-style" />
                                </td>
                                 <td colspan="3">
                                    <span class="x-label-text">验 收 人:</span>
                                    <ext:TextField ID="zfk_yanshouName" runat="server" Cls="x-textfeild-style" />
                                </td>
                            </tr>
                        </table>
                    </Body>
                </ext:Panel>
                <ext:Panel 
                    ID="FCPanel" 
                    runat="server" 
                    Title="复查反馈信息" 
                    AutoHeight="true" 
                    FormGroup="true"
                    Collapsed="True">
                    <Body>
                        <table style="width:100%;">
                            <tr>
                                 <td colspan="3">
                                    <span class="x-label-text">复查意见:</span>
                                    <ext:TextArea ID="ffk_ReviewOpinion" runat="server" Width="500px" Height="40px" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width:33%;">
                                    <span class="x-label-text">复 查 人:</span>
                                    <ext:TextField ID="ffk_PersonID" runat="server" Cls="x-textfeild-style" />
                                </td>
                                <td style="width:34%;">
                                    <span class="x-label-text">复查时间:</span>
                                    <ext:TextField ID="ffk_FCTime" runat="server" Cls="x-textfeild-style" />
                                </td>
                                <td style="width:33%;">
                                    <span class="x-label-text">复查情况:</span>
                                    <ext:TextField ID="ffk_ReviewState" runat="server" Cls="x-textfeild-style" />
                                </td>
                            </tr>
                        </table>
                    </Body>
                </ext:Panel>
                <ext:Panel 
                    ID="CFPanel" 
                    runat="server" 
                    Title="处罚信息" 
                    AutoHeight="true" 
                    FormGroup="true"
                    Collapsed="True">
                    <Body>
                        <table style="width:100%;">
                            <tr>
                                 <td colspan="3">
                                     <ext:Label ID="lbl_fkxx" runat="server" Width="500px" Height="40px" />
                                </td>
                            </tr> 
                        </table>
                    </Body>
                </ext:Panel>
            </ext:ContainerLayout>
        </Body>
    </ext:Panel>
          
    </div>
    </form>
</body>
</html>
