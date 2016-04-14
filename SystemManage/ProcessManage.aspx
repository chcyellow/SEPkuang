<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProcessManage.aspx.cs" Inherits="SystemManage_ProcessManage" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dxwgv" %>

<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>流程管理</title>
    <link href="../Style/FontStyle.css" rel="stylesheet" type="text/css" media="screen"/>
    <link href="../Style/PopWindows.css" rel="stylesheet" type="text/css" media="screen"/>
    <link href="../Style/Page.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

//    function OpenWindows2(url,wname,w,h,x,y,parameters){

//     w = w || 630;

//     h = h || 497;

//     x = (window.screen.width - w) / 2;

//     y = (window.screen.height - h) / 2;

//     if(!parameters){parameters = ',menubar=no,toolbar=no,location=no,directories=no,status=no,scrollbars=no,resizable=no';};

//     myWin = window.open(url,wname,'width='+w+',height='+h+',screenX='+x+',screenY='+y+',top='+y+',left='+x+parameters);

//}
//function ChooseRole_onclick() {

//    OpenWindows2('ChooseRole.aspx')

//}

        function OnModuleChanged(cmbModule) {
            grid.GetEditor("ITEMTAG").PerformCallback(cmbModule.GetValue().toString());
        }
</script>

<%--<script type="text/javascript">//下面这个事件将赋给ImageButton1来弹出子窗体。

function ChooseSKU_onclick()

{

OpenWindows2('ChooseSku.aspx')

}

</script>--%>
</head>
<body>
    <form id="form1" runat="server">
    <table borderColor="#fffbec" cellSpacing="0" cellPadding="0" width="100%" align="center" bgColor="#fffbec" border="1" frame="void">
        <tbody>
        <tr align="left">
        <td height="20">你现在的位置是：系统管理->流程管理</td></tr>
        </tbody>
    </table>
    <div>
        <dxwgv:ASPxGridView ID="gvProcess" ClientInstanceName="grid" runat="server" 
            CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" 
            Width="100%" AutoGenerateColumns="False" DataSourceID="ObjectDataSource1" 
            KeyFieldName="ITEMID" oncustomcolumndisplaytext="gvProcess_CustomColumnDisplayText">
            <SettingsBehavior ConfirmDelete="True" />
            <SettingsEditing Mode="Inline" />
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
                <dxwgv:GridViewDataTextColumn Caption="流程编码" FieldName="ITEMID" ReadOnly="True" 
                    VisibleIndex="0">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="流程名称" FieldName="ITEMNAME" 
                    VisibleIndex="1">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataComboBoxColumn Caption="流程处理页面" FieldName="ITEMVALUE" 
                    VisibleIndex="2">
                    <PropertiesComboBox DataSourceID="odsModule" TextField="MODULENAME" EnableSynchronization="False"
                    EnableIncrementalFiltering="True"
                        ValueField="MODULETAG" ValueType="System.String">
                        <%--<ClientSideEvents SelectedIndexChanged="function(s, e) { OnModuleChanged(s); }"></ClientSideEvents>--%>
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataTextColumn Caption="流程处理控件" FieldName="ITEMTAG" 
                    VisibleIndex="3">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="流程处理说明" FieldName="ABOUT" 
                    VisibleIndex="4">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataComboBoxColumn Caption="状态" FieldName="STATUS" 
                    VisibleIndex="5">
                    <PropertiesComboBox ValueType="System.String">
                        <Items>
                            <dxe:ListEditItem Selected="True" Text="激活" Value="1" />
                            <dxe:ListEditItem Text="关闭" Value="2" />
                        </Items>
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewCommandColumn Caption="操作" VisibleIndex="6">
                    <DeleteButton Visible="True">
                    </DeleteButton>
                    <EditButton Visible="True">
                    </EditButton>
                    <NewButton Visible="True">
                    </NewButton>
                </dxwgv:GridViewCommandColumn>
            </Columns>
            <Templates>
                <DetailRow>
                    <dxwgv:ASPxGridView ID="girdConfDetail" runat="server" 
                        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" 
                        Width="100%" AutoGenerateColumns="False" DataSourceID="ObjectDataSource2" 
                        onbeforeperformdataselect="girdConfDetail_BeforePerformDataSelect" 
                        KeyFieldName="CONFIGDETAILID">
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
                            <dxwgv:GridViewDataTextColumn Caption="流程项编码" FieldName="ITEMID" 
                                VisibleIndex="0">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="流程项名称" FieldName="CONFIGDETAILNAME" 
                                VisibleIndex="1">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="标识" FieldName="CONFIGDETAILTAG" 
                                VisibleIndex="2">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="流程编码" FieldName="CONFIGURATIONID" 
                                VisibleIndex="3" Visible="False">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataComboBoxColumn Caption="角色" FieldName="ROLEID" 
                                VisibleIndex="3">
                                <PropertiesComboBox DataSourceID="odsRole" TextField="ROLENAME" 
                                    ValueField="ROLEID" EnableSynchronization="False" 
                                    EnableIncrementalFiltering="True">
                                </PropertiesComboBox>
                            </dxwgv:GridViewDataComboBoxColumn>
                            <dxwgv:GridViewDataTextColumn Caption="描述" FieldName="ABOUT" VisibleIndex="4">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataDateColumn Caption="创建时间" FieldName="CREATETIME" 
                                VisibleIndex="5">
                                <PropertiesDateEdit DisplayFormatString="">
                                </PropertiesDateEdit>
                            </dxwgv:GridViewDataDateColumn>
                        </Columns>
                        <StylesEditors>
                            <ProgressBar Height="25px">
                            </ProgressBar>
                        </StylesEditors>
                        <SettingsDetail IsDetailGrid="True" />
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
                </DetailRow>
             <%--<EditForm>
             <div style="padding:4px 4px 3px 4px">
                <table>
                <tr>
                <td style="text-align:right">流程编码：</td><td><asp:Label ID="lblItemID" runat="server" Text='<%# Eval("ITEMID") %>'></asp:Label></td>
                <td style="text-align:right">流程名称：</td><td><asp:TextBox ID="txtItemName" Text='<%# Eval("ITEMNAME") %>' runat="server"></asp:TextBox></td>
                <td style="text-align:right">流程角色：</td><td style="text-align:left; cursor:pointer"><asp:TextBox ID="lblRole" runat="server" Text='<%# Eval("ITEMVALUE") %>'></asp:TextBox><asp:ImageButton runat="server" ID="ibtnChooseRole" ImageUrl="~/Images/yu/user.png" OnClientClick="ChooseRole_onclick() " ToolTip="选择角色"></asp:ImageButton></td>
                </tr>
                <tr>
                <td style="text-align:right">流程标识：</td><td><asp:TextBox ID="txtItemTag" Text='<%# Eval("ITEMTAG") %>' runat="server"></asp:TextBox></td>
                <td>流程处理描述：</td><td><asp:TextBox ID="txtAbout" runat="server" 
                        Text='<%# Eval("ABOUT") %>' TextMode="MultiLine"></asp:TextBox></td>
                <td style="text-align:right">状态：</td><td style="text-align:left">
                    <asp:RadioButtonList ID="radStatus" runat="server" RepeatDirection="Horizontal" >
                        <asp:ListItem Value="1">激活</asp:ListItem>
                        <asp:ListItem Value="2">关闭</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                </tr>
                </table>
                </div>
             <div style="text-align:right; padding:2px 2px 2px 2px">

                 <dxwgv:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton" runat="server"></dxwgv:ASPxGridViewTemplateReplacement>

                 <dxwgv:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton" runat="server"></dxwgv:ASPxGridViewTemplateReplacement>

             </div>
             </EditForm>--%>
            </Templates>
            <StylesEditors>
                <ProgressBar Height="25px">
                </ProgressBar>
            </StylesEditors>
            <SettingsDetail ShowDetailRow="True" />
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
    <div>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
            DataObjectTypeName="GhtnTech.SecurityFramework.Model.SF_Configuration" 
            DeleteMethod="DeleteItem" InsertMethod="CreateItem" SelectMethod="GetItemList" 
            TypeName="GhtnTech.SecurityFramework.Configuration" UpdateMethod="UpdateItem">
            <SelectParameters>
                <asp:SessionParameter DefaultValue=" " Name="strWhere" 
                    SessionField="WhereConfig" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" 
            DataObjectTypeName="GhtnTech.SecurityFramework.Model.SF_ConfigDetail" 
             SelectMethod="GetList" 
            TypeName="GhtnTech.SecurityFramework.ConfigDetail">
            <SelectParameters>
                <asp:SessionParameter DefaultValue=" " Name="strWhere" 
                    SessionField="WhereConfDetail" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="odsRole" runat="server" SelectMethod="GetRoleList" 
            TypeName="GhtnTech.SecurityFramework.Role">
            <SelectParameters>
                <asp:Parameter DefaultValue=" " Name="strWhere" Type="String" />
                <asp:Parameter DefaultValue=" " Name="strOrder" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="odsModule" runat="server" 
            SelectMethod="GetModuleList" TypeName="GhtnTech.SecurityFramework.Module">
            <SelectParameters>
                <asp:SessionParameter DefaultValue=" " Name="strWhere" 
                    SessionField="WhereModule" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <br />
        <%--<asp:LinkButton ID="lnkGetValue" runat="server" onclick="lnkGetValue_Click"></asp:LinkButton>--%>
    </div>
    </form>
</body>
</html>
