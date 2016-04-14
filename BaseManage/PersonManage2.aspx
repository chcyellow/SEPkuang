<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PersonManage2.aspx.cs" Inherits="BaseManage_PersonManage2" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dxwgv" %>

<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>

<%@ Register assembly="ALinq.Web, Version=1.0.8.0, Culture=neutral, PublicKeyToken=2b23f34316d38f3a" namespace="ALinq.Web.Controls" tagprefix="cc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>人员管理</title>
    <link href="../Style/FontStyle.css" rel="stylesheet" type="text/css" media="screen"/>
    <link href="../Style/PopWindows.css" rel="stylesheet" type="text/css" media="screen"/>
     <link href="../Style/Page.css" rel="stylesheet" type="text/css" /> 
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
                <asp:Button ID="btnSearch" runat="server" Text="查询" onclick="btnSearch_Click" 
                    Height="40px" Width="40px" />
            </td>
            <td>
                </td>
            <td>
                </td>
            <td>
                </td>
            </tr>
        </table>
        
    </fieldset>
    <div style=" text-align: center">
    <dxwgv:ASPxGridView ID="gvPerson" ClientInstanceName="gvPerson" runat="server" 
        AutoGenerateColumns="False" KeyFieldName="PERSONID" 
        Width="100%" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" 
        CssPostfix="Aqua" onstartrowediting="gvPerson_StartRowEditing">
         <Columns>
             <dxwgv:GridViewDataTextColumn FieldName="PERSONNUMBER" Caption="工号" 
                 VisibleIndex="0" ReadOnly="True">
                <EditFormSettings Caption="工号" CaptionLocation="Near" />
             </dxwgv:GridViewDataTextColumn>
             <dxwgv:GridViewDataTextColumn FieldName="NAME" Caption="姓名" VisibleIndex="1" 
                 ReadOnly="True">
                <EditFormSettings Caption="姓名" CaptionLocation="Near" />
             </dxwgv:GridViewDataTextColumn>
             <dxwgv:GridViewDataComboBoxColumn Caption="性别" FieldName="SEX" VisibleIndex="2" 
                 ReadOnly="True">
                <PropertiesComboBox ValueType="System.String">
                    <Items>
                        <dxe:ListEditItem Text="男" Value="男"></dxe:ListEditItem>
                        <dxe:ListEditItem Text="女" Value="女"></dxe:ListEditItem>
                    </Items>
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
             <dxwgv:GridViewDataTextColumn FieldName="TEL" Caption="电话" VisibleIndex="3">
                <EditFormSettings Caption="电话" CaptionLocation="Near" />
             </dxwgv:GridViewDataTextColumn>
             <dxwgv:GridViewDataComboBoxColumn FieldName="POSID" Caption="职务" 
                 VisibleIndex="4">
                 <PropertiesComboBox DataSourceID="adsPosition" TextField="Posname" 
                     ValueField="Posid" EnableSynchronization="False" 
                     EnableIncrementalFiltering="True">
                 </PropertiesComboBox>
             </dxwgv:GridViewDataComboBoxColumn>
             <dxwgv:GridViewDataComboBoxColumn FieldName="DEPTID" Caption="部门" 
                 ReadOnly="True" VisibleIndex="5">
                 <PropertiesComboBox DataSourceID="adsDept" TextField="Deptname" 
                     ValueField="Deptnumber" EnableSynchronization="False" 
                     EnableIncrementalFiltering="True">
                 </PropertiesComboBox>
             </dxwgv:GridViewDataComboBoxColumn>
             <dxwgv:GridViewDataTextColumn FieldName="LIGHTNUMBER" Caption="灯号" 
                 VisibleIndex="6">
                <EditFormSettings Caption="灯号" CaptionLocation="Near" />
             </dxwgv:GridViewDataTextColumn>
             <dxwgv:GridViewDataComboBoxColumn FieldName="POSTID" Caption="岗位" 
                 Visible="False">
                 <PropertiesComboBox DataSourceID="adsPost" TextField="Postname" 
                     ValueField="Postid" EnableSynchronization="False" 
                     EnableIncrementalFiltering="True">
                 </PropertiesComboBox>
             </dxwgv:GridViewDataComboBoxColumn>
             <dxwgv:GridViewDataComboBoxColumn Caption="单位" FieldName="MAINDEPTID" 
                 ReadOnly="True" VisibleIndex="7">
                 <PropertiesComboBox DataSourceID="adsDept" TextField="Deptname" 
                     ValueField="Deptnumber" EnableSynchronization="False" 
                     EnableIncrementalFiltering="True">
                 </PropertiesComboBox>
             </dxwgv:GridViewDataComboBoxColumn>
             <dxwgv:GridViewCommandColumn Caption="操作" VisibleIndex="8">
                <EditButton Visible="True" />
                <ClearFilterButton Visible="True">
                 </ClearFilterButton>
            </dxwgv:GridViewCommandColumn>
         </Columns>
         <Templates>
             <EditForm>
             <div style="padding:4px 4px 3px 4px">
                <table>
                <tr>
                <td style="text-align:right">电话：</td><td><asp:TextBox ID="txtPhone" Text='<%# Eval("TEL") %>' runat="server"></asp:TextBox></td>
                <td style="text-align:right">职务：</td><td>
                    <dxe:ASPxComboBox ID="cboPos" runat="server">
                    </dxe:ASPxComboBox>
                </td>
                <td style="text-align:right">灯号：</td><asp:TextBox ID="txtLightNO" runat="server" Text='<%# Eval("LIGHTNUMBER") %>'></asp:TextBox></td>
                </tr>
                </table>
                </div>
             <div style="text-align:right; padding:2px 2px 2px 2px">

                 <dxwgv:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton" runat="server"></dxwgv:ASPxGridViewTemplateReplacement>

                 <dxwgv:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton" runat="server"></dxwgv:ASPxGridViewTemplateReplacement>

             </div>
             </EditForm>
            </Templates>
         <SettingsLoadingPanel Text="" />
         <SettingsPager Mode="ShowAllRecords">
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
                 Url="~/App_Themes/Aqua/GridView/gvDetailCollapsedButton.png" 
                 Width="15px" />
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
        <SettingsEditing EditFormColumnCount="2" Mode="Inline" />
        <Styles CssFilePath="~/App_Themes/Aqua/{0}/styles.css" 
            CssPostfix="Aqua">
        </Styles>
         <Settings ShowGroupPanel="True" />
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
    <webdiyer:AspNetPager ID="AspNetPager1" CssClass="pages" 
            CurrentPageButtonClass="cpb"  runat="server" FirstPageText="首页" 
            LastPageText="尾页" NextPageText="下一页" PrevPageText="上一页"  AlwaysShow="true" 
            PageIndexBoxType="DropDownList" OnPageChanged="AspNetPager1_PageChanged"  
            ShowCustomInfoSection="left"  NumericButtonTextFormatString="{0}" 
            HorizontalAlign="Left" ></webdiyer:AspNetPager>
    <cc2:ALinqDataSource ID="adsPosition" runat="server" 
            ContextTypeName="GhtnTech.SEP.DAL.DBSCMDataContext" TableName="Position" >
    </cc2:ALinqDataSource>
    <cc2:ALinqDataSource ID="adsDept" runat="server" 
            ContextTypeName="GhtnTech.SEP.DAL.DBSCMDataContext" TableName="Department" >
    </cc2:ALinqDataSource>
        
    <cc2:ALinqDataSource ID="adsPost" runat="server" 
            ContextTypeName="GhtnTech.SEP.DAL.DBSCMDataContext" TableName="Post">
    </cc2:ALinqDataSource>
    </div>
    </form>
</body>
</html>
