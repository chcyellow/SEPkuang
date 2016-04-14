<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserManage.aspx.cs" Inherits="SystemManage_UserManage" %>

<%@ Register Assembly="DevExpress.Web.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxUploadControl" TagPrefix="dxuc" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dxwgv" %>

<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>用户管理</title>
    <link href="../Style/FontStyle.css" rel="stylesheet" type="text/css" media="screen"/>
    <link href="../Style/PopWindows.css" rel="stylesheet" type="text/css" media="screen"/>
     <link href="../Style/Page.css" rel="stylesheet" type="text/css" />
     <script type="text/javascript">
        function PopSendingWin(e, psnNumber, userName, userid) {
        var para = "id=" + escape(psnNumber) + "&name=" + escape(userName) + "&userid=" + escape(userid); 
        var url = "PopInitPwd.aspx?" + para;
        window.showModalDialog(url,"","dialogHeight:150px;dialogWidth:200px;dialogLeft:400px;dialogTop:250px;scroll:auto");
    }
    </script>     
</head>
<body>
    <form id="form1" runat="server">
    
    <table borderColor="#fffbec" cellSpacing="0" cellPadding="0" width="100%" align="center" bgColor="#fffbec" border="1" frame="void">
        <tbody>
        <tr align="left">
        <td height="20">你现在的位置是：系统管理->用户管理</td></tr>
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
                    用户：</td>
                <td>
                    <asp:TextBox ID="txtUser" runat="server" Width="100px"></asp:TextBox>
                    </td>
                <td style=" text-align:right;">
                    分组：</td>
                <td>
                    <asp:DropDownList ID="ddlUserGroup" runat="server" Width="100px">
                    </asp:DropDownList>
                    <%--DataSourceID="odsUserGroup" DataTextField="USERGROUPNAME" DataValueField="USERGROUPID"--%>
                </td>
                <td style=" text-align:right;">
                    角色：
                </td>
                <td>
                    <%--<asp:DropDownList ID="ddlRole" runat="server" DataSourceID="odsRole" 
                        DataTextField="ROLENAME" DataValueField="ROLEID" Width="120px">
                    </asp:DropDownList>--%>
                    <asp:DropDownList ID="ddlRole" runat="server"  Width="120px">
                    </asp:DropDownList>
                    
                </td>
                <td style=" text-align:right;">用户状态：</td>
                <td>
                    <asp:DropDownList ID="ddlUserStatus" runat="server" Width="150px">
                        <asp:ListItem Value="-1">--全部--</asp:ListItem>
                        <asp:ListItem Value="1">启用</asp:ListItem>
                        <asp:ListItem Value="2">锁定</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <%--<td style=" text-align:right;">
                 在线状态：   
                </td>
                <td>
                    <asp:DropDownList ID="ddlIsOnline" runat="server">
                        <asp:ListItem Value="-1">--全部--</asp:ListItem>
                        <asp:ListItem Value="1">在线</asp:ListItem>
                        <asp:ListItem Value="2">离线</asp:ListItem>
                    </asp:DropDownList>
                </td>--%>
            </tr>
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
                    单位：
                </td>
                <td>
                    <asp:DropDownList ID="ddlDept" runat="server" Width="120px" AutoPostBack="True" 
                        onselectedindexchanged="ddlDept_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td style=" text-align:right;">
                    科区：
                </td>
                <td>
                    <asp:DropDownList ID="ddlKQ" runat="server" Width="150px">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
            </td>
            <td>
                <asp:Button ID="btnSearch" runat="server" Text="查询" onclick="btnSearch_Click" 
                    Height="40px" Width="40px" />
            </td>
            <td>
                <asp:Button ID="btnAddUser" runat="server" Text="新增用户" 
                    onclick="btnAddUser_Click" />
            </td>
            <td>
                <asp:Button ID="btnBatchAddUser" runat="server" Text="导入用户" 
                    onclick="btnBatchAddUser_Click" />
            </td>
            <td>
                <asp:Button ID="btnEditRole" runat="server" Text="修改角色" 
                    onclick="btnEditRole_Click" /></td>
            <td>
                <asp:Button ID="btnEditUsergroup" runat="server" Text="修改用户组" 
                    onclick="btnEditUsergroup_Click" /></td>
                    
            </tr>
        </table>
        <asp:Panel ID="pnlAddPsn" runat="server" Width="100%" Visible="False">
		<table border="0" cellspacing="0" cellpadding="0" width="100%">
			<tr>
			<td>选择Excel文件：</td>
				<td>
                    <dxuc:aspxuploadcontrol id="upctrlPsn" runat="server" Width="400px">
                        <ValidationSettings AllowedContentTypes="application/vnd.ms-excel" FileDoesNotExistErrorText="文件不存在！"
                            GeneralErrorText="文件上传失败！" MaxFileSize="500000" MaxFileSizeErrorText="文件不要大于500k！"
                            NotAllowedContentTypeErrorText="请上传正确的Excel文件格式(xls)">
                        </ValidationSettings>
                    </dxuc:aspxuploadcontrol>
				</td>
				<td>
                    <asp:Button ID="btnSure" runat="server" Text="确  定" CssClass="btn_2k3" OnClientClick="Loading(true);" OnClick="btnSure_Click" />
                </td>
                <td>
                    <asp:Button ID="btnCancel" runat="server" Text="取  消" CausesValidation="False" CssClass="btn_2k3" OnClick="btnCancel_Click" />
                </td>
			</tr>
		</table>
    </asp:Panel>
    </fieldset>
    <div>
         <dxwgv:ASPxGridView ID="gridUser" runat="server" ClientInstanceName="gridUser" 
             CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" 
             Width="100%" AutoGenerateColumns="False" DataSourceID="ObjectDataSource1" 
             KeyFieldName="USERID" 
             oncustomcolumndisplaytext="gridUser_CustomColumnDisplayText" 
             onhtmlrowprepared="gridUser_HtmlRowPrepared" 
             onpageindexchanged="gridUser_PageIndexChanged">
             <SettingsBehavior ConfirmDelete="True" />
             <Styles CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua">
             </Styles>
             <SettingsLoadingPanel Text="" />
             <SettingsPager>
                 <AllButton>
                     <Image Height="19px" Width="27px" />
                 </AllButton>
                 <FirstPageButton>
                     <Image Height="19px" Width="23px" />
                 </FirstPageButton>
                 <LastPageButton>
                     <Image Height="19px" Width="23px" />
                 </LastPageButton>
                 <NextPageButton>
                     <Image Height="19px" Width="19px" />
                 </NextPageButton>
                 <PrevPageButton>
                     <Image Height="19px" Width="19px" />
                 </PrevPageButton>
             </SettingsPager>
             <Images ImageFolder="~/App_Themes/Aqua/{0}/">
                 <CollapsedButton Height="15px" 
                     Url="~/App_Themes/Aqua/GridView/gvCollapsedButton.png" Width="15px" />
                 <ExpandedButton Height="15px" 
                     Url="~/App_Themes/Aqua/GridView/gvExpandedButton.png" Width="15px" />
                 <DetailCollapsedButton Height="15px" 
                     Url="~/App_Themes/Aqua/GridView/gvDetailCollapsedButton.png" Width="15px" />
                 <DetailExpandedButton Height="15px" 
                     Url="~/App_Themes/Aqua/GridView/gvDetailExpandedButton.png" Width="15px" />
                 <HeaderFilter Height="19px" Url="~/App_Themes/Aqua/GridView/gvHeaderFilter.png" 
                     Width="19px" />
                 <HeaderActiveFilter Height="19px" 
                     Url="~/App_Themes/Aqua/GridView/gvHeaderFilterActive.png" Width="19px" />
                 <HeaderSortDown Height="5px" 
                     Url="~/App_Themes/Aqua/GridView/gvHeaderSortDown.png" Width="7px" />
                 <HeaderSortUp Height="5px" Url="~/App_Themes/Aqua/GridView/gvHeaderSortUp.png" 
                     Width="7px" />
                 <FilterRowButton Height="13px" Width="13px" />
                 <WindowResizer Height="13px" Url="~/App_Themes/Aqua/GridView/WindowResizer.png" 
                     Width="13px" />
             </Images>
             <Columns>
                 <dxwgv:GridViewCommandColumn VisibleIndex="0" ShowSelectCheckbox="True">
                     <HeaderStyle HorizontalAlign="Center" />
                    <HeaderTemplate>
                        <input id="chkSelectedAll" type="checkbox" onclick="gridUser.SelectAllRowsOnPage(this.checked);" style="vertical-align:middle;" title="选择或不选本页所有的人员" />
                    </HeaderTemplate>
                    <ClearFilterButton Visible="True">
                    </ClearFilterButton>
                </dxwgv:GridViewCommandColumn>
                 <dxwgv:GridViewDataTextColumn Caption="用户编码" FieldName="USERID" 
                     VisibleIndex="0" Visible="False">
                 </dxwgv:GridViewDataTextColumn>
                 <dxwgv:GridViewDataTextColumn Caption="用户" FieldName="USERNAME" 
                     VisibleIndex="1">
                 </dxwgv:GridViewDataTextColumn>
                 <dxwgv:GridViewDataTextColumn FieldName="USERGROUPID" Visible="False" 
                     VisibleIndex="2">
                 </dxwgv:GridViewDataTextColumn>
                 <dxwgv:GridViewDataTextColumn Caption="用户组" FieldName="USERGROUPNAME" 
                     VisibleIndex="2">
                 </dxwgv:GridViewDataTextColumn>
                 <dxwgv:GridViewDataTextColumn Caption="角色" FieldName="ROLENAME" 
                     VisibleIndex="3">
                 </dxwgv:GridViewDataTextColumn>
                 <dxwgv:GridViewDataTextColumn Caption="人员工号" FieldName="PERSONNUMBER" 
                     VisibleIndex="4">
                 </dxwgv:GridViewDataTextColumn>
                 <dxwgv:GridViewDataTextColumn Caption="姓名" FieldName="NAME" VisibleIndex="5">
                 </dxwgv:GridViewDataTextColumn>
                 <dxwgv:GridViewDataTextColumn FieldName="DEPTNUMBER" Visible="False" 
                     VisibleIndex="7">
                 </dxwgv:GridViewDataTextColumn>
                 <dxwgv:GridViewDataTextColumn Caption="单位" FieldName="DEPTNAME" 
                     VisibleIndex="6">
                 </dxwgv:GridViewDataTextColumn>
                 <dxwgv:GridViewDataTextColumn Caption="科区" FieldName="KQNAME" VisibleIndex="7">
                 </dxwgv:GridViewDataTextColumn>
                 <dxwgv:GridViewDataTextColumn Caption="在线状态" FieldName="ISONLINE" 
                     VisibleIndex="7" Visible="False">
                 </dxwgv:GridViewDataTextColumn>
                 <dxwgv:GridViewDataTextColumn Caption="最后一次登录时间" FieldName="LASTLOGINTIME" 
                     VisibleIndex="8">
                 </dxwgv:GridViewDataTextColumn>
                 <dxwgv:GridViewDataDateColumn Caption="创建时间" FieldName="CREATETIME" 
                     VisibleIndex="9">
                 </dxwgv:GridViewDataDateColumn>
                 <dxwgv:GridViewDataTextColumn Caption="用户描述" FieldName="ABOUT" 
                     VisibleIndex="10">
                 </dxwgv:GridViewDataTextColumn>
                 <dxwgv:GridViewDataTextColumn Caption="用户状态" FieldName="USERSTATUS" 
                     VisibleIndex="11">
                 </dxwgv:GridViewDataTextColumn>
                 <dxwgv:GridViewDataHyperLinkColumn Caption="编辑" VisibleIndex="12">
                     <DataItemTemplate>
                        <asp:HyperLink ID="link_view" runat="server" NavigateUrl='<%# Eval("USERID", "EditUser.aspx?uid={0}") %>'
                            Text="编辑"></asp:HyperLink>
                    </DataItemTemplate>
                 </dxwgv:GridViewDataHyperLinkColumn>
                 <dxwgv:GridViewDataHyperLinkColumn Caption="密码重置" VisibleIndex="12">
                    <DataItemTemplate>
                            <a href="javascript:void(0);" onclick="PopSendingWin(event, '<%# Eval("PERSONNUMBER") %>', '<%# Eval("USERNAME") %>','<%# Eval("USERID") %>')">密码重置</a>
                    </DataItemTemplate>
                </dxwgv:GridViewDataHyperLinkColumn>
                 <dxwgv:GridViewCommandColumn Caption="删除" VisibleIndex="13">
                     <DeleteButton Text="删除" Visible="True">
                     </DeleteButton>
                     <ClearFilterButton Visible="True">
                     </ClearFilterButton>
                 </dxwgv:GridViewCommandColumn>
             </Columns>
             <StylesEditors>
                 <ProgressBar Height="25px">
                 </ProgressBar>
             </StylesEditors>
             <ImagesEditors>
                 <CalendarFastNavPrevYear Height="19px" 
                     Url="~/App_Themes/Aqua/Editors/edtCalendarFNPrevYear.png" Width="19px" />
                 <CalendarFastNavNextYear Height="19px" 
                     Url="~/App_Themes/Aqua/Editors/edtCalendarFNNextYear.png" Width="19px" />
                 <DropDownEditDropDown Height="7px" 
                     Url="~/App_Themes/Aqua/Editors/edtDropDown.png" 
                     UrlDisabled="~/App_Themes/Aqua/Editors/edtDropDownDisabled.png" 
                     UrlHottracked="~/App_Themes/Aqua/Editors/edtDropDownHottracked.png" 
                     Width="9px" />
                 <SpinEditIncrement Height="7px" 
                     Url="~/App_Themes/Aqua/Editors/edtSpinEditIncrementImage.png" 
                     UrlDisabled="~/App_Themes/Aqua/Editors/edtSpinEditIncrementDisabledImage.png" 
                     UrlHottracked="~/App_Themes/Aqua/Editors/edtSpinEditIncrementHottrackedImage.png" 
                     UrlPressed="~/App_Themes/Aqua/Editors/edtSpinEditIncrementHottrackedImage.png" 
                     Width="7px" />
                 <SpinEditDecrement Height="7px" 
                     Url="~/App_Themes/Aqua/Editors/edtSpinEditDecrementImage.png" 
                     UrlDisabled="~/App_Themes/Aqua/Editors/edtSpinEditDecrementDisabledImage.png" 
                     UrlHottracked="~/App_Themes/Aqua/Editors/edtSpinEditDecrementHottrackedImage.png" 
                     UrlPressed="~/App_Themes/Aqua/Editors/edtSpinEditDecrementHottrackedImage.png" 
                     Width="7px" />
                 <SpinEditLargeIncrement Height="9px" 
                     Url="~/App_Themes/Aqua/Editors/edtSpinEditLargeIncImage.png" 
                     UrlDisabled="~/App_Themes/Aqua/Editors/edtSpinEditLargeIncDisabledImage.png" 
                     UrlHottracked="~/App_Themes/Aqua/Editors/edtSpinEditLargeIncHottrackedImage.png" 
                     UrlPressed="~/App_Themes/Aqua/Editors/edtSpinEditLargeIncHottrackedImage.png" 
                     Width="7px" />
                 <SpinEditLargeDecrement Height="9px" 
                     Url="~/App_Themes/Aqua/Editors/edtSpinEditLargeDecImage.png" 
                     UrlDisabled="~/App_Themes/Aqua/Editors/edtSpinEditLargeDecDisabledImage.png" 
                     UrlHottracked="~/App_Themes/Aqua/Editors/edtSpinEditLargeDecHottrackedImage.png" 
                     UrlPressed="~/App_Themes/Aqua/Editors/edtSpinEditLargeDecHottrackedImage.png" 
                     Width="7px" />
             </ImagesEditors>
        </dxwgv:ASPxGridView>
    </div>
    <div id="GridViewMsg" style="padding: 5px;" runat="server" />
    <div>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
            DataObjectTypeName="GhtnTech.SecurityFramework.Model.SF_User" 
            DeleteMethod="DeleteUser" InsertMethod="CreateUser" SelectMethod="GetUserList2" 
            TypeName="GhtnTech.SecurityFramework.User" UpdateMethod="UpdateUser">
            <SelectParameters>
                <asp:SessionParameter DefaultValue=" " Name="strWhere" SessionField="WhereUser" 
                    Type="String" />
                <asp:SessionParameter DefaultValue=" " Name="strOrder" SessionField="OrderUser" 
                    Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <%--<asp:ObjectDataSource ID="odsUserGroup" runat="server" 
         SelectMethod="GetUserGroupList" TypeName="GhtnTech.SecurityFramework.UserGroup">
         <SelectParameters>
             <asp:SessionParameter DefaultValue=" " Name="strWhere" 
                 SessionField="WhereUserGroup" Type="String" />
         </SelectParameters>
     </asp:ObjectDataSource>
     <asp:ObjectDataSource ID="odsRole" runat="server" SelectMethod="GetVRoleList" 
         TypeName="GhtnTech.SecurityFramework.Role">
         <SelectParameters>
             <asp:SessionParameter DefaultValue=" " Name="strWhere" SessionField="WhereRole" 
                 Type="String" />
             <asp:SessionParameter DefaultValue=" " Name="strOrder" 
                 SessionField="WhereOrder" Type="String" />
         </SelectParameters>
     </asp:ObjectDataSource>--%>
     <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" 
         SelectMethod="GetDept" TypeName="DepartmentBLL">
         <SelectParameters>
             <asp:SessionParameter DefaultValue=" " Name="deptID" SessionField="WhereDeptID" 
                 Type="String" />
         </SelectParameters>
     </asp:ObjectDataSource>
    </div>
    </form>
</body>
</html>
