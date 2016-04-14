<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Style/FontStyle.css" rel="stylesheet" type="text/css" media="screen"/>
    <link href="../Style/PopWindows.css" rel="stylesheet" type="text/css" media="screen"/>
     <link href="../Style/Page.css" rel="stylesheet" type="text/css" />   
</head>
<body>
    <form id="form1" runat="server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>--%>
    <table borderColor="#fffbec" cellSpacing="0" cellPadding="0" width="100%" align="center" bgColor="#fffbec" border="1" frame="void">
        <tbody>
        <tr align="left">
        <td height="20">你现在的位置是：基础信息设置->人员管理</td></tr>
        </tbody>
    </table>
    <fieldset id="Fieldset1" class="Fieldsetbox mbox pbox">
        <legend class="fsTitle">功能操作</legend>
        <table>
            <tr>
            <td>
            <table width="100%" border="0" cellpadding="0" cellspacing="5">
            <tr>
            <td style=" text-align:right;">
                 工号：   
                </td>
                <td>
                    <asp:TextBox ID="txtPsnNo" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td style=" text-align:right;">
                 姓名：   
                </td>
                <td>
                    <asp:TextBox ID="txtName" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td style=" text-align:right;">
                 手机：   
                </td>
                <td>
                    <asp:TextBox ID="txtPhone" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td style=" text-align:right;">
                 灯号：   
                </td>
                <td>
                    <asp:TextBox ID="txtLightNo" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td style=" text-align:right;">
                 单位：
                </td>
                <td>
                    <asp:DropDownList ID="ddlDept" runat="server" Width="150px" AutoPostBack="True" 
                        onselectedindexchanged="ddlDept_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td style=" text-align:right;">
                 科区：
                </td>
                <td>
                    <asp:DropDownList ID="ddlKQ" runat="server" Width="100px">
                    </asp:DropDownList>
                </td>
                <td style=" text-align:right;">
                 职务：
                </td>
                <td>
                    <asp:DropDownList ID="ddlPos" runat="server" Width="100px">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
            </td>
            <td>
                <asp:Button ID="btnSearch" runat="server" Text="查询" onclick="btnSearch_Click" />
            </td>
            <td>
                <asp:Button ID="btnCancelTxt" runat="server" onclick="btnCancelTxt_Click" 
                    Text="清除条件" />
                </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            </tr>
        </table>
        
    </fieldset>
    
    <div>
    
        <asp:GridView ID="gvPerson" runat="server" AutoGenerateColumns="False" 
            BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" 
            CellPadding="3" GridLines="Horizontal" 
            onrowcancelingedit="gvPerson_RowCancelingEdit" 
            onrowcommand="gvPerson_RowCommand" onrowdatabound="gvPerson_RowDataBound" 
            onrowediting="gvPerson_RowEditing" onrowupdating="gvPerson_RowUpdating" 
            Width="100%">
            <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
            <Columns>
                <asp:BoundField DataField="PERSONID" HeaderText="人员编码" ReadOnly="True" 
                    Visible="False" />
                <asp:BoundField DataField="PERSONNUMBER" HeaderText="工号" ReadOnly="True" />
                <asp:BoundField DataField="NAME" HeaderText="姓名" ReadOnly="True" />
                <asp:BoundField DataField="SEX" HeaderText="性别" ReadOnly="True" />
                <asp:TemplateField HeaderText="手机">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtPhone" runat="server" Text='<%# Bind("TEL") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblPhone" runat="server" Text='<%# Bind("TEL") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="职务">
                    <EditItemTemplate>
                        <asp:Label ID="lblPos" runat="server" Text='<%# Bind("POSID") %>' style="display:none;"></asp:Label>
                        <asp:Label ID="hid_lblPos" runat="server" Text='<%# Bind("POSID") %>' style="display:none;"></asp:Label>
                        <asp:DropDownList ID="ddlPostion" runat="server" Width="100px">
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblPos" runat="server" Text='<%# Bind("POSID") %>'></asp:Label>
                        <asp:Label ID="hid_lblPos" runat="server" Text='<%# Bind("POSID") %>' style="display:none;"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="单位">
                    <EditItemTemplate>
                        <asp:Label ID="lblUnit" runat="server" Text='<%# Eval("MAINDEPTID") %>'></asp:Label>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblUnit" runat="server" Text='<%# Bind("MAINDEPTID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="灯号">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtLightNo2" runat="server" Text='<%# Bind("LIGHTNUMBER") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblLightNo" runat="server" Text='<%# Bind("LIGHTNUMBER") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="POSTID" HeaderText="工种" ReadOnly="True" 
                    Visible="False" />
                <asp:TemplateField HeaderText="部门">
                    <EditItemTemplate>
                        <asp:Label ID="lblDept" runat="server" Text='<%# Eval("DEPTID") %>'></asp:Label>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblDept" runat="server" Text='<%# Bind("DEPTID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作">
                    <EditItemTemplate>
                        <asp:LinkButton ID="btnUpdate" runat="server" CommandName="update">确定</asp:LinkButton>
                        &nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="btnCancel" runat="server" CausesValidation="False" 
                            CommandName="cancel">取消</asp:LinkButton>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="btnEdit" runat="server" 
                            CommandArgument='<%# Eval("PERSONNUMBER")%>' CommandName="edit">编辑</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
            <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" />
            <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
            <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
            <AlternatingRowStyle BackColor="#F7F7F7" />
        </asp:GridView>
    
    </div>
    <div>
    </div>
            <webdiyer:AspNetPager ID="AspNetPager1" CssClass="pages" 
            CurrentPageButtonClass="cpb"  runat="server" FirstPageText="首页" 
            LastPageText="尾页" NextPageText="下一页" PrevPageText="上一页"  AlwaysShow="true" 
            PageIndexBoxType="DropDownList" OnPageChanged="AspNetPager1_PageChanged"  
            ShowCustomInfoSection="left"  NumericButtonTextFormatString="{0}" 
            HorizontalAlign="Left" ></webdiyer:AspNetPager>
    </form>
    <p>
        </p>
</body>
</html>
