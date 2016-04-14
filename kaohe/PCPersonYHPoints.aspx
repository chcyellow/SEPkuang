<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PCPersonYHPoints.aspx.cs" Inherits="kaohe_PCPersonYHPoints" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2.Export, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView.Export" TagPrefix="dxwgv" %>

<%@ Register assembly="DevExpress.Web.ASPxGridView.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dxwgv" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server" style="text-align:center">
    <table runat="server" id ="table">
   <tr >
   <td> 
       <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="选择矿：">
       </dxe:ASPxLabel>
        </td>
   <td> <dxe:ASPxComboBox ID="OREcbox" runat="server" TextField="deptname" ValueField="deptnumber">
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
     <h1></h1>
    <dxwgv:ASPxGridView ID="ASPxGridView1" runat="server" 
        AutoGenerateColumns="False"
        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" 
        CssPostfix="Aqua" 
        >
        
        <Styles CssFilePath="~/App_Themes/Aqua/{0}/styles.css" 
            CssPostfix="Aqua">
        </Styles>
        <SettingsLoadingPanel Text="" />
        <SettingsPager PageSize="20">
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
        <SettingsEditing EditFormColumnCount="2" Mode="PopupEditForm" PopupEditFormWidth="800px" />
        <Settings ShowTitlePanel="true" />
        <SettingsText Title="个人安全排查积分" />
        <Columns >
            
            <dxwgv:GridViewDataTextColumn Caption="姓名" FieldName="PERSONNAME" VisibleIndex="1" Width="80px" >  
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="单位" FieldName="DEPTNAME" VisibleIndex="2" Width="100px" >  
                <HeaderStyle HorizontalAlign="Center" />
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="A级" FieldName="A级" VisibleIndex="3" Width="50px">  
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="B级" FieldName="B级" VisibleIndex="4" Width="50px">  
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="C级" FieldName="C级" VisibleIndex="5" Width="50px">  
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="D级" FieldName="D级" VisibleIndex="6" Width="50px">  
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="排查隐患总数" FieldName="排查隐患总数" VisibleIndex="7" Width="50px">  
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="排查隐患积分" FieldName="排查隐患积分" VisibleIndex="8" 
                Width="50px" SortIndex="0" SortOrder="Descending">  
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="复查隐患总数" FieldName="复查隐患总数" VisibleIndex="9" Width="50px">  
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="复查隐患积分" FieldName="复查隐患积分" VisibleIndex="10" 
                Width="50px">  
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="严重" FieldName="严重" VisibleIndex="11">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="一般" FieldName="一般" VisibleIndex="12">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="轻微" FieldName="轻微" VisibleIndex="13">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="三违总数" FieldName="三违总数" VisibleIndex="14">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="三违积分" FieldName="三违积分" VisibleIndex="15">
            </dxwgv:GridViewDataTextColumn>
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
    <dxwgv:ASPxGridViewExporter ID="ASPxGridViewExporter1" ExportedRowType="All" runat="server" GridViewID="ASPxGridView1">
        </dxwgv:ASPxGridViewExporter>
    </form>
</body>
</html>
