<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditModule.aspx.cs" Inherits="SystemManage_EditModule" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>模块管理</title>
    <base target="_self"/>
    <link href="../Style/FontStyle.css" rel="stylesheet" type="text/css" media="screen"/>
    <link href="../Style/PopWindows.css" rel="stylesheet" type="text/css" media="screen"/>
    <link href="../Style/Page.css" rel="stylesheet" type="text/css" /> 
</head>
<body>
    <form id="form1" runat="server">
    <table borderColor="#fffbec" cellSpacing="0" cellPadding="0" width="100%" align="center" bgColor="#fffbec" border="1" frame="void">
        <tbody>
        <tr align="left">
        <td height="20">你现在的位置是：系统管理->模块管理</td></tr>
        </tbody>
    </table>
    <div>
    <table style=" width:100%;">
    <tr>
    <td style=" width:40%;text-align:right;">
    <table>
    <tr>
    <td style=" text-align:right;">所属分组：</td>
    <td>
        <dxe:ASPxComboBox ID="cboModuleGroup" runat="server">
        </dxe:ASPxComboBox>
    </td>
    </tr>
    <tr>
    <td style=" text-align:right;">模块名称：</td>
    <td>
        <dxe:ASPxTextBox ID="txtModule" runat="server" Width="170px">
        </dxe:ASPxTextBox>
    </td>
    </tr>
    <tr>
    <td style=" text-align:right;">模块标识：</td>
    <td>
        <dxe:ASPxTextBox ID="txtModuleTag" runat="server" Width="170px" ReadOnly="True" 
            NullText="自动生成模块标识">
            <NullTextStyle ForeColor="#CC3300">
            </NullTextStyle>
        </dxe:ASPxTextBox>
    </td>
    </tr>
    <tr>
    <td style=" text-align:right;">链接地址：</td>
    <td>
        <dxe:ASPxTextBox ID="txtUrl" runat="server" Width="170px">
        </dxe:ASPxTextBox>
    </td>
    </tr>
    <tr>
    <td style=" text-align:right;">排列顺序：</td>
    <td>
        <dxe:ASPxTextBox ID="txtOrder" runat="server" Width="170px">
        </dxe:ASPxTextBox>
    </td>
    </tr>
    <tr>
    <td style=" text-align:right;">模块描述：</td>
    <td>
        <dxe:ASPxMemo ID="txtAbout" runat="server" Height="71px" Width="170px">
        </dxe:ASPxMemo>
    </td>
    </tr>
    <tr>
    <td style=" text-align:right;">状态：</td>
    <td>
        <asp:RadioButtonList ID="radStatus" runat="server" RepeatDirection="Horizontal">
            <asp:ListItem Value="1" Selected="True">启用</asp:ListItem>
            <asp:ListItem Value="2">关闭</asp:ListItem>
        </asp:RadioButtonList>
    </td>
    </tr>
    </table>
    </td>
    <td>
    <table width="100%" border="0" cellpadding="0" cellspacing="1" bgcolor="#CCCCCC">
                <tr>
                    <td width="200" height="30" bgcolor="#CCCCCC">
                        <div align="center">
                            已拥有的功能</div>
                    </td>
                    <td width="100" bgcolor="#CCCCCC">
                        <div align="center" style="color: #FFFFFF;">
                        </div>
                    </td>
                    <td width="200" bgcolor="#CCCCCC">
                        <div align="center">
                            可选择的功能</div>
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF">
                        <div align="center" style="margin: 10px;">
                            <asp:ListBox ID="lstSelectedOpt" runat="server" Style="width: 160px;" SelectionMode="Multiple"
                                Height="200px" AutoPostBack="True" 
                                onselectedindexchanged="lstSelectedOpt_SelectedIndexChanged"></asp:ListBox>
                            <asp:TextBox ID="txtOldOperator" runat="server" Style="display: none;"></asp:TextBox>
                            <asp:TextBox ID="txtTRole" runat="server" Style="display: none;"></asp:TextBox>
                        </div>
                    </td>
                    <td bgcolor="#FFFFFF">
                        <div align="center">
                            <asp:Button ID="btnAdd" runat="server" Text="&lt;增加"  Width="50px" 
                                onclick="btnAdd_Click"/><br />
                            <br />
                            <asp:Button ID="btnRemove" runat="server" Text="删除&gt;" Width="50px" 
                                onclick="btnRemove_Click"/> <br />
                            <br />
                            按下<b>Ctrl</b>键<br />
                            可以多选                         </div>
                    </td>
                    <td bgcolor="#FFFFFF">
                        <div align="center" style="margin: 10px;">
                                <asp:ListBox ID="lstOpt" runat="server" Style="width: 160px;" SelectionMode="Multiple"
                                Height="200px" AutoPostBack="True" 
                                onselectedindexchanged="lstOpt_SelectedIndexChanged"></asp:ListBox>
                        </div>
                    </td>
                </tr>
            </table>
    </td>
    </tr>
    </table>
    
    </div>
    <table style="width:100%;">
    <tr>
    <td style=" text-align:right;">
        <dxe:ASPxButton ID="btnSave" runat="server" Text="保 存" onclick="btnSave_Click">
        </dxe:ASPxButton>
    </td><td>
        <dxe:ASPxButton ID="btnCancel" runat="server" Text="返 回" onclick="btnCancel_Click">
        </dxe:ASPxButton>
    </td>
    </tr>
    </table>
    </form>
</body>
</html>
