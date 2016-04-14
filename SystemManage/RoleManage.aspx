<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RoleManage.aspx.cs" Inherits="SystemManage_RoleManage" %>

<%@ Register Assembly="DevExpress.Web.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dxrp" %>

<%@ Register assembly="DevExpress.Web.ASPxGridView.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dxwgv" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>

<%@ Register assembly="DevExpress.Web.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPanel" tagprefix="dxp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>角色管理</title>
    <link href="../Style/FontStyle.css" rel="stylesheet" type="text/css" media="screen"/>
    <link href="../Style/PopWindows.css" rel="stylesheet" type="text/css" media="screen"/>
    <link href="../Style/Page.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    
    <%--页面标题--%>
    <table borderColor="#fffbec" cellSpacing="0" cellPadding="0" width="100%" align="center" bgColor="#fffbec" border="1" frame="void">
        <tbody>
        <tr align="left">
        <td height="20">你现在的位置是：系统管理->角色管理</td></tr>
        </tbody>
    </table>
    <dxrp:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="100%" 
        HeaderText="查询条件" BackColor="White" 
        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua">
        <TopEdge>
            <BackgroundImage ImageUrl="~/App_Themes/Aqua/Web/rpTopEdge.gif" 
                Repeat="RepeatX" VerticalPosition="Top" />
        </TopEdge>
        <LeftEdge>
            <BackgroundImage ImageUrl="~/App_Themes/Aqua/Web/rpLeftEdge.gif" 
                Repeat="RepeatY" VerticalPosition="Top" />
        </LeftEdge>
        <BottomRightCorner Height="7px" Url="~/App_Themes/Aqua/Web/rpBottomRight.png" 
            Width="7px" />
        <ContentPaddings Padding="14px" />
        <NoHeaderTopRightCorner Height="7px" 
            Url="~/App_Themes/Aqua/Web/rpNoHeaderTopRight.png" Width="7px" />
        <RightEdge>
            <BackgroundImage ImageUrl="~/App_Themes/Aqua/Web/rpRightEdge.gif" 
                Repeat="RepeatY" VerticalPosition="Top" />
        </RightEdge>
        <HeaderRightEdge>
            <BackgroundImage ImageUrl="~/App_Themes/Aqua/Web/rpHeaderBackground.gif" 
                Repeat="RepeatX" VerticalPosition="Top" />
        </HeaderRightEdge>
        <Border BorderColor="#AECAF0" BorderStyle="Solid" BorderWidth="1px" />
        <HeaderLeftEdge>
            <BackgroundImage ImageUrl="~/App_Themes/Aqua/Web/rpHeaderBackground.gif" 
                Repeat="RepeatX" VerticalPosition="Top" />
        </HeaderLeftEdge>
        <HeaderStyle BackColor="#E0EDFF">
        <BorderBottom BorderColor="#AECAF0" BorderStyle="Solid" BorderWidth="1px" />
        </HeaderStyle>
        <BottomEdge>
            <BackgroundImage ImageUrl="~/App_Themes/Aqua/Web/rpBottomEdge.gif" 
                Repeat="RepeatX" VerticalPosition="Bottom" />
        </BottomEdge>
        <TopRightCorner Height="7px" Url="~/App_Themes/Aqua/Web/rpTopRight.png" 
            Width="7px" />
        <HeaderContent>
            <BackgroundImage ImageUrl="~/App_Themes/Aqua/Web/rpHeaderBackground.gif" 
                Repeat="RepeatX" VerticalPosition="Top" />
        </HeaderContent>
        <NoHeaderTopEdge BackColor="White">
            <BackgroundImage ImageUrl="~/App_Themes/Aqua/Web/rpNoHeaderTopEdge.gif" 
                Repeat="RepeatX" VerticalPosition="Top" />
        </NoHeaderTopEdge>
        <NoHeaderTopLeftCorner Height="7px" 
            Url="~/App_Themes/Aqua/Web/rpNoHeaderTopLeft.png" Width="7px" />
        <PanelCollection>
<dxp:PanelContent runat="server">
<table>
<tr>
<td>角色名称：</td><td><dxe:ASPxTextBox ID="txtRoleName" runat="server" Width="170px"></dxe:ASPxTextBox></td>
<td>角色描述：</td><td><dxe:ASPxTextBox ID="txtRoleAbout" runat="server" Width="170px"></dxe:ASPxTextBox></td>
<td>
    <dxe:ASPxButton ID="btnSearch" runat="server" Text="查询" 
        OnClick="btnSearch_Click">
    </dxe:ASPxButton>
</td>
</tr>
</table>
    
</dxp:PanelContent>
</PanelCollection>
        <TopLeftCorner Height="7px" Url="~/App_Themes/Aqua/Web/rpTopLeft.png" 
            Width="7px" />
        <BottomLeftCorner Height="7px" Url="~/App_Themes/Aqua/Web/rpBottomLeft.png" 
            Width="7px" />
    </dxrp:ASPxRoundPanel>
    <div>
    <dxwgv:ASPxGridView ID="gridRole" runat="server" 
                CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" 
                DataSourceID="ObjectDataSource1" Width="100%" AutoGenerateColumns="False" 
                KeyFieldName="ROLEID" onrowinserting="gridRole_RowInserting" 
            oncustomcolumndisplaytext="gridRole_CustomColumnDisplayText">
                <SettingsBehavior ConfirmDelete="True" />
                <styles cssfilepath="~/App_Themes/Aqua/{0}/styles.css" csspostfix="Aqua">
                </styles>
                <settingsloadingpanel text="" />
                <settingspager>
                    <allbutton>
                        <image height="19px" width="27px" />
                    </allbutton>
                    <firstpagebutton>
                        <image height="19px" width="23px" />
                    </firstpagebutton>
                    <lastpagebutton>
                        <image height="19px" width="23px" />
                    </lastpagebutton>
                    <nextpagebutton>
                        <image height="19px" width="19px" />
                    </nextpagebutton>
                    <prevpagebutton>
                        <image height="19px" width="19px" />
                    </prevpagebutton>
                </settingspager>
                <images imagefolder="~/App_Themes/Aqua/{0}/">
                    <collapsedbutton height="15px" 
                        url="~/App_Themes/Aqua/GridView/gvCollapsedButton.png" width="15px" />
                    <expandedbutton height="15px" 
                        url="~/App_Themes/Aqua/GridView/gvExpandedButton.png" width="15px" />
                    <detailcollapsedbutton height="15px" 
                        url="~/App_Themes/Aqua/GridView/gvDetailCollapsedButton.png" width="15px" />
                    <detailexpandedbutton height="15px" 
                        url="~/App_Themes/Aqua/GridView/gvDetailExpandedButton.png" width="15px" />
                    <headerfilter height="19px" url="~/App_Themes/Aqua/GridView/gvHeaderFilter.png" 
                        width="19px" />
                    <headeractivefilter height="19px" 
                        url="~/App_Themes/Aqua/GridView/gvHeaderFilterActive.png" width="19px" />
                    <headersortdown height="5px" 
                        url="~/App_Themes/Aqua/GridView/gvHeaderSortDown.png" width="7px" />
                    <headersortup height="5px" url="~/App_Themes/Aqua/GridView/gvHeaderSortUp.png" 
                        width="7px" />
                    <filterrowbutton height="13px" width="13px" />
                    <windowresizer height="13px" url="~/App_Themes/Aqua/GridView/WindowResizer.png" 
                        width="13px" />
                </images>
                <SettingsEditing Mode="Inline" />
                <Columns>
                    <dxwgv:GridViewDataTextColumn Caption="角色编号" FieldName="ROLEID" 
                        VisibleIndex="0" ReadOnly="True" Visible="False">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="角色名称" FieldName="ROLENAME" 
                        VisibleIndex="0">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="角色描述" FieldName="ROLEABOUT" 
                        VisibleIndex="1">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataDateColumn Caption="创建时间" FieldName="CREATETIME" 
                        VisibleIndex="2">
                    </dxwgv:GridViewDataDateColumn>
                    <dxwgv:GridViewDataTextColumn Caption="创建单位" FieldName="MAINDEPTID" 
                        VisibleIndex="3" ReadOnly="True">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="角色状态" FieldName="ROLESTATUS" 
                        VisibleIndex="4" Visible="False">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewCommandColumn Caption="编辑" VisibleIndex="3">
                    <EditButton Visible="True">
                    </EditButton>
                    <NewButton Visible="True">
                    </NewButton>
                    <DeleteButton Visible="True">
                    </DeleteButton>
                </dxwgv:GridViewCommandColumn>
                    <dxwgv:GridViewDataTextColumn Caption="角色等级" FieldName="LEVELID" 
                        Visible="False" VisibleIndex="4">
                    </dxwgv:GridViewDataTextColumn>
                </Columns>
                <styleseditors>
                    <progressbar height="25px">
                    </progressbar>
                </styleseditors>
                <imageseditors>
                    <calendarfastnavprevyear height="19px" 
                        url="~/App_Themes/Aqua/Editors/edtCalendarFNPrevYear.png" width="19px" />
                    <calendarfastnavnextyear height="19px" 
                        url="~/App_Themes/Aqua/Editors/edtCalendarFNNextYear.png" width="19px" />
                    <dropdowneditdropdown height="7px" 
                        url="~/App_Themes/Aqua/Editors/edtDropDown.png" 
                        urldisabled="~/App_Themes/Aqua/Editors/edtDropDownDisabled.png" 
                        urlhottracked="~/App_Themes/Aqua/Editors/edtDropDownHottracked.png" 
                        width="9px" />
                    <spineditincrement height="7px" 
                        url="~/App_Themes/Aqua/Editors/edtSpinEditIncrementImage.png" 
                        urldisabled="~/App_Themes/Aqua/Editors/edtSpinEditIncrementDisabledImage.png" 
                        urlhottracked="~/App_Themes/Aqua/Editors/edtSpinEditIncrementHottrackedImage.png" 
                        urlpressed="~/App_Themes/Aqua/Editors/edtSpinEditIncrementHottrackedImage.png" 
                        width="7px" />
                    <spineditdecrement height="7px" 
                        url="~/App_Themes/Aqua/Editors/edtSpinEditDecrementImage.png" 
                        urldisabled="~/App_Themes/Aqua/Editors/edtSpinEditDecrementDisabledImage.png" 
                        urlhottracked="~/App_Themes/Aqua/Editors/edtSpinEditDecrementHottrackedImage.png" 
                        urlpressed="~/App_Themes/Aqua/Editors/edtSpinEditDecrementHottrackedImage.png" 
                        width="7px" />
                    <spineditlargeincrement height="9px" 
                        url="~/App_Themes/Aqua/Editors/edtSpinEditLargeIncImage.png" 
                        urldisabled="~/App_Themes/Aqua/Editors/edtSpinEditLargeIncDisabledImage.png" 
                        urlhottracked="~/App_Themes/Aqua/Editors/edtSpinEditLargeIncHottrackedImage.png" 
                        urlpressed="~/App_Themes/Aqua/Editors/edtSpinEditLargeIncHottrackedImage.png" 
                        width="7px" />
                    <spineditlargedecrement height="9px" 
                        url="~/App_Themes/Aqua/Editors/edtSpinEditLargeDecImage.png" 
                        urldisabled="~/App_Themes/Aqua/Editors/edtSpinEditLargeDecDisabledImage.png" 
                        urlhottracked="~/App_Themes/Aqua/Editors/edtSpinEditLargeDecHottrackedImage.png" 
                        urlpressed="~/App_Themes/Aqua/Editors/edtSpinEditLargeDecHottrackedImage.png" 
                        width="7px" />
                </imageseditors>
            </dxwgv:ASPxGridView>
</div>
    <div>
            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
                DataObjectTypeName="GhtnTech.SecurityFramework.Model.SF_Role" 
                DeleteMethod="DeleteRole" InsertMethod="CreateRole" SelectMethod="GetRoleList" 
                TypeName="GhtnTech.SecurityFramework.Role" UpdateMethod="UpdateRole">
                <SelectParameters>
                    <asp:SessionParameter DefaultValue=" " Name="strWhere" SessionField="WhereRole" 
                        Type="String" />
                    <asp:SessionParameter DefaultValue=" " Name="strOrder" SessionField="OrderRole" 
                        Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </form>
</body>
</html>
