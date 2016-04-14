<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DeptYHPoint2.aspx.cs" Inherits="kaohe_DeptYHPoint2" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2.Export, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView.Export" TagPrefix="dxwgv" %>

<%@ Register assembly="DevExpress.Web.ASPxGridView.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dxwgv" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>



<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>
<%@ Register assembly="DevExpress.Web.ASPxGridView.v9.2" namespace="DevExpress.Web.ASPxGridView" tagprefix="dxwgv" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>单位隐患积分</title>
</head>
<body>
    <form id="form1" runat="server" style="text-align:center">
   <table runat="server" id ="table">
   <tr >
    <td> 
       <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="选择矿：">
       </dxe:ASPxLabel>
        </td>
   <td> <dxe:ASPxComboBox ID="OREcbox" runat="server" TextField="deptname" 
           ValueField="deptnumber" AutoPostBack="True" 
           onselectedindexchanged="OREcbox_SelectedIndexChanged">
       </dxe:ASPxComboBox>
        </td>
        <td> 
       <dxe:ASPxLabel ID="lblKequ" runat="server" Text="选择科区：">
       </dxe:ASPxLabel>
        </td>
   <td> <dxe:ASPxComboBox ID="cboKequ" runat="server" TextField="deptname" ValueField="deptnumber">
       </dxe:ASPxComboBox>
        </td>
   <td> 选择日期： </td>
   <td><dxe:ASPxDateEdit runat ="server" ID="deteedit"  ></dxe:ASPxDateEdit></td>
   <td>到</td>
   <td><dxe:ASPxDateEdit runat ="server" ID="ASPxDateEdit1"  ></dxe:ASPxDateEdit></td>
   <td>
       <dxe:ASPxButton ID="ASPxButton2" runat="server" OnClick="ASPxButton2_Click" 
            Text="确定">
        </dxe:ASPxButton></td>
   <td>
       <dxe:ASPxButton ID="ASPxButton1" runat="server" OnClick="ASPxButton1_Click" 
            Text="导出到excel">
        </dxe:ASPxButton></td>
   </tr>
    </table>
        <dxwgv:ASPxGridView ID="ASPxGridView1" runat="server" 
            AutoGenerateColumns="False"
            CssFilePath="~/App_Themes/Aqua/{0}/styles.css" 
            CssPostfix="Aqua" 
           Width="100%" 
        oncustomunboundcolumndata="ASPxGridView1_CustomUnboundColumnData" 
        onbeforecolumnsortinggrouping="ASPxGridView1_BeforeColumnSortingGrouping" onpageindexchanged="ASPxGridView1_PageIndexChanged" 
            >
            <Styles CssFilePath="~/App_Themes/Aqua/{0}/styles.css" 
                CssPostfix="Aqua">
            </Styles>
            <Settings ShowTitlePanel="true" />
            <SettingsText Title="单位隐患积分" />
            <Columns >
            
            <dxwgv:GridViewDataTextColumn Caption="单位" FieldName="DEPTNAME" VisibleIndex="2" Width="20%" >  
            <HeaderStyle HorizontalAlign="Center" />
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="A级" FieldName="A级" VisibleIndex="3"> 
            <HeaderStyle HorizontalAlign="Center" /> 
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="B级" FieldName="B级" VisibleIndex="4">  
            <HeaderStyle HorizontalAlign="Center" />
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="C级" FieldName="C级" VisibleIndex="5">  
            <HeaderStyle HorizontalAlign="Center" />
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="D级" FieldName="D级" VisibleIndex="6">  
            <HeaderStyle HorizontalAlign="Center" />
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="隐患总数" FieldName="隐患总数" VisibleIndex="7" 
                    SortIndex="1" SortOrder="Descending">  
            <HeaderStyle HorizontalAlign="Center" />
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="矿查总积分" FieldName="矿查总积分" VisibleIndex="8" >  
            <HeaderStyle HorizontalAlign="Center" />
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="自查总积分" FieldName="自查总积分" VisibleIndex="9" >  
            <HeaderStyle HorizontalAlign="Center" />
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn FieldName="总积分" Caption="总积分" VisibleIndex="34" 
                    UnboundType="Decimal" SortIndex="0" SortOrder="Descending" >
                 <Settings AllowSort="True" />
                 <HeaderStyle HorizontalAlign="Center" />
                 <FooterCellStyle ForeColor="Brown" />          
             </dxwgv:GridViewDataTextColumn>
            </Columns>
            <SettingsLoadingPanel Text="" />
            <SettingsPager PageSize="20">
                <AllButton Text="All">
                    <Image Height="19px" Width="27px" />
                </AllButton>
                <FirstPageButton>
                    <Image Height="19px" Width="23px" />
                </FirstPageButton>
                <LastPageButton>
                    <Image Height="19px" Width="23px" />
                </LastPageButton>
                <NextPageButton >
                    <Image Height="19px" Width="19px" />
                </NextPageButton>
                <PrevPageButton >
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
        <dxwgv:ASPxGridViewExporter ID="ASPxGridViewExporter1" ExportedRowType="All" runat="server" GridViewID="ASPxGridView1">
        </dxwgv:ASPxGridViewExporter>
    

   
    </form>
</body>
</html>
