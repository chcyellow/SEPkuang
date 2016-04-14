<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PasswordEdit.aspx.cs" Inherits="SystemManage_PasswordEdit" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>修改密码</title>
    <link href="../Style/Page.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="strinfo" class="mbox pbox" style="display: none;">
    </div>
    <fieldset id="Fieldset1" class="Fieldsetbox mbox pbox">
        <legend class="fsTitle">修改登录密码</legend>
        <table width="526" border="0" cellpadding="0" cellspacing="5">
            <tr>
                <td width="87">
                    <div align="right" class="f14">
                        原&nbsp;密&nbsp;码</div>
                </td>
                <td width="161">
                    <asp:TextBox ID="txtOldPwd" runat="server" Width="150px" TextMode="Password"></asp:TextBox>
                </td>
                <td width="258" class="tcgray f14">
                    &nbsp;
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                        ControlToValidate="txtOldPwd" ErrorMessage="RequiredFieldValidator">必填！</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <div align="right" class="f14">
                        新&nbsp;密&nbsp;码</div>
                </td>
                <td>
                    <asp:TextBox ID="txtPwd1" runat="server" Width="150px" TextMode="Password"></asp:TextBox>
                </td>
                <td class="tcgray f14">
                    &nbsp;
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                        ControlToValidate="txtPwd1" ErrorMessage="RequiredFieldValidator">必填！</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <div align="right" class="f14">
                        再次输入密码</div>
                </td>
                <td>
                    <asp:TextBox ID="txtPwd2" runat="server" Width="150px" TextMode="Password"></asp:TextBox>
                </td>
                <td class="tcgray f14">
                    &nbsp;
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                        ControlToValidate="txtPwd2" Display="Dynamic" 
                        ErrorMessage="RequiredFieldValidator">必填！</asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator1" runat="server" 
                        ControlToCompare="txtPwd1" ControlToValidate="txtPwd2" Display="Dynamic" 
                        ErrorMessage="CompareValidator">两次输入密码不一致！</asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <div align="right">
                    </div>
                </td>
                <td>
                    <asp:Button ID="btnSure" runat="server" Text="确  定" CssClass="blueButton" 
                        onclick="btnSure_Click" BackColor="#6699FF" />
                  
                    <asp:Button ID="btnCancel" runat="server" Text="取  消" CssClass="blueButton" 
                        CausesValidation="False" BackColor="#6699FF" />
                </td>
                <td>
                    
                </td>
            </tr>
        </table>
    </fieldset>
    </form>
</body>
</html>
