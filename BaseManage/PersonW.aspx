<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PersonW.aspx.cs" Inherits="BaseManage_PersonW" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dxwgv" %>

<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>

<%@ Register assembly="ALinq.Web, Version=1.0.8.0, Culture=neutral, PublicKeyToken=2b23f34316d38f3a" namespace="ALinq.Web.Controls" tagprefix="cc2" %>

<%@ Register assembly="DevExpress.Web.ASPxGridView.v9.2.Export, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView.Export" tagprefix="dxwgv1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style=" text-align: center">
  <table runat="server" id ="table">
   <tr > 
   <td align="right">
       <dxe:ASPxButton ID="ASPxButton1" runat="server" OnClick="ASPxButton1_Click" 
            Text="导出到excel" style="text-align: right">
        </dxe:ASPxButton></td>
   </tr>
    </table>
    <dxwgv:ASPxGridView ID="GridView" ClientInstanceName="GridView" runat="server" 
        AutoGenerateColumns="False"  DataSourceID="odsPerson" KeyFieldName="PERSONID" 
        Width="100%" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" 
        CssPostfix="Aqua">
         <Columns>
             <dxwgv:GridViewDataTextColumn FieldName="PERSONNUMBER" Caption="工号" 
                 VisibleIndex="0" ReadOnly="True">
                <EditFormSettings Caption="工号" CaptionLocation="Near" />
             </dxwgv:GridViewDataTextColumn>
             <dxwgv:GridViewDataTextColumn FieldName="NAME" Caption="姓名" VisibleIndex="1" 
                 Width="100px">
                 <PropertiesTextEdit Width="100px">
                 </PropertiesTextEdit>
                <EditFormSettings Caption="姓名" CaptionLocation="Near" />
             </dxwgv:GridViewDataTextColumn>
             <dxwgv:GridViewDataComboBoxColumn Caption="性别" FieldName="SEX" VisibleIndex="2">
                <PropertiesComboBox ValueType="System.String">
                    <Items>
                        <dxe:ListEditItem Text="男" Value="男"></dxe:ListEditItem>
                        <dxe:ListEditItem Text="女" Value="女"></dxe:ListEditItem>
                    </Items>
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
             <dxwgv:GridViewDataTextColumn FieldName="TEL" Caption="电话" VisibleIndex="3" 
                 Width="100px">
                 <PropertiesTextEdit Width="100px">
                 </PropertiesTextEdit>
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
                 VisibleIndex="5">
                 <PropertiesComboBox DataSourceID="odsDept" TextField="DEPTNAME" 
                     ValueField="DEPTNUMBER" EnableSynchronization="False" 
                     EnableIncrementalFiltering="True">
                 </PropertiesComboBox>
             </dxwgv:GridViewDataComboBoxColumn>
             <dxwgv:GridViewDataTextColumn FieldName="LIGHTNUMBER" Caption="灯号" 
                 VisibleIndex="6" Width="100px">
                 <PropertiesTextEdit Width="100px">
                 </PropertiesTextEdit>
                <EditFormSettings Caption="灯号" CaptionLocation="Near" />
             </dxwgv:GridViewDataTextColumn>
             <dxwgv:GridViewDataComboBoxColumn FieldName="POSTID" Caption="岗位" 
                 Visible="False">
                 <PropertiesComboBox DataSourceID="adsPost" TextField="Postname" 
                     ValueField="Postid" EnableSynchronization="False" 
                     EnableIncrementalFiltering="True">
                 </PropertiesComboBox>
             </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewCommandColumn VisibleIndex="7" Caption="操作">
                    <EditButton Visible="True" Text="修改">
                    </EditButton>
                    <NewButton Visible="True" Text="新增">
                    </NewButton>
                    <DeleteButton Visible="True" Text="删除">
                    </DeleteButton>
                </dxwgv:GridViewCommandColumn>
         </Columns>
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
         <TotalSummary>
             <dxwgv:ASPxSummaryItem FieldName="PosName" SummaryType="Count" DisplayFormat="累计信息数：{0}条" />
         </TotalSummary>
         <Settings ShowFilterRow="True" ShowFooter="True" ShowGroupPanel="True"/>
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
        <SettingsEditing EditFormColumnCount="2" Mode="Inline" 
             PopupEditFormWidth="450px" />
        <Settings ShowTitlePanel="true" />
        <SettingsText Title="人员管理" />
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
    
        <dxwgv1:ASPxGridViewExporter ID="ASPxGridViewExporter1" ExportedRowType="All" 
            runat="server" GridViewID="GridView">
        </dxwgv1:ASPxGridViewExporter>
    

   
    <cc2:ALinqDataSource ID="adsPerson" runat="server" 
            ContextTypeName="GhtnTech.SEP.DAL.DBSCMDataContext" TableName="Person" EnableUpdate="true" StoreOriginalValuesInViewState="True">
    </cc2:ALinqDataSource>
    <cc2:ALinqDataSource ID="adsPosition" runat="server" 
            ContextTypeName="GhtnTech.SEP.DAL.DBSCMDataContext" TableName="Position" 
            Where="Maindeptid == @Maindeptid" >
        <WhereParameters>
            <asp:SessionParameter DefaultValue=" " Name="Maindeptid" SessionField="PosDept" 
                Type="String" />
        </WhereParameters>
    </cc2:ALinqDataSource>
    <cc2:ALinqDataSource ID="adsDept" runat="server" 
            ContextTypeName="GhtnTech.SEP.DAL.DBSCMDataContext" TableName="Department" >
    </cc2:ALinqDataSource>
        
    <cc2:ALinqDataSource ID="adsPost" runat="server" 
            ContextTypeName="GhtnTech.SEP.DAL.DBSCMDataContext" TableName="Post">
    </cc2:ALinqDataSource>
    <asp:ObjectDataSource ID="odsPerson" runat="server" SelectMethod="GetPersonWW" 
            TypeName="GhtnTech.SEP.OraclDAL.PersonWW" DataObjectTypeName="Person" 
            UpdateMethod="UpdatePerWW" DeleteMethod="DeletePerWW" 
            InsertMethod="CreatePerWW">
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="odsDept" runat="server" SelectMethod="GetDeptWW" 
            TypeName="GhtnTech.SEP.OraclDAL.DeptWW">
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="odsPos" runat="server"></asp:ObjectDataSource>
    </div>
    </form>
</body>
</html>

