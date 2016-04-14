<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChooseRole.aspx.cs" Inherits="SystemManage_ChooseRole" %>
<%@ Register assembly="DevExpress.Web.ASPxGridView.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dxwgv" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>

<%@ Register assembly="DevExpress.Web.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPanel" tagprefix="dxp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>选择角色</title>
    <base target="_self"/>
     <meta http-equiv="Pragma" content="no-cache" />   
     <meta http-equiv="Cache-Control" content="no-cache" />   
     <meta http-equiv="Expires" content="0" />
     <link href="../Style/FontStyle.css" rel="stylesheet" type="text/css" media="screen"/>
    <link href="../Style/PopWindows.css" rel="stylesheet" type="text/css" media="screen"/>
     <link href="../Style/Page.css" rel="stylesheet" type="text/css" />  
    <script type="text/javascript"><!--
        function OnGridSelectionChanged() {
            var counter = document.getElementById("selCount");
            if(counter != null) setInnerText(counter, xgvPerson.GetSelectedRowCount().toString());
            xgvPerson.GetSelectedFieldValues('ROLENAME', OnGridSelectionComplete);
        }
        function OnGridSelectionComplete(values) {
            xlstSelected.BeginUpdate();
            xlstSelected.ClearItems();
            for(var i = 0; i < values.length; i ++) {
                xlstSelected.AddItem(values[i]);
            }
            xlstSelected.EndUpdate();
        }
        function setInnerText(element, text) { 
            if(typeof element.textContent != 'undefined') { 
                element.textContent = text; 
            } 
            else if (typeof element.innerText != 'undefined') { 
                element.innerText = text; 
            } 
            else if (typeof element.removeChild != 'undefined') { 
                while (element.hasChildNodes()) { 
                    element.removeChild(element.lastChild); 
                } 
                element.appendChild(document.createTextNode(text)); 
            } 
        }
//--></script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="diviframe"> 
 <div id="divOuter"> 
 <div id="divCaption"> 
 <div id="divCaption_Left" class="Manager13b"> 
     角色信息
 </div> 
</div> 
<div id="panMain" class="Manager13">
  <table style="width:100%"><tbody>
      <tr>
      <td style="width: 160px; height: 26px;">已选择的角色：</td>
      <td style="height: 26px">
          <input id="btnSelectAll" onclick="xgvPerson.SelectRows();" type="button" value="选择所有" class="btn_2k3" />
          <input id="btnUnselectAll" onclick="xgvPerson.UnselectRows();" type="button" value="取消全选" class="btn_2k3" />
      </td>
      </tr>
      <tr>
       <td style="vertical-align:top; width: 160px;">
         <dxe:ASPxListBox ID="xlstSelected" runat="server" 
               ClientInstanceName="xlstSelected" 
               CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" 
               ImageFolder="~/App_Themes/Aqua/{0}/" Width="100%" Height="300px" Rows="10" 
               LoadingPanelText="">
             <ValidationSettings>
                 <ErrorImage Height="14px" Url="~/App_Themes/Aqua/Editors/edtError.png" 
                     Width="14px" />
                 <ErrorFrameStyle ImageSpacing="4px">
                     <ErrorTextPaddings PaddingLeft="4px" />
                 </ErrorFrameStyle>
             </ValidationSettings>
           </dxe:ASPxListBox>
         <p></p>
           已选择角色数： <a id="selCount" type="text/plain">0</a>
       </td>
       <td style="vertical-align:top">
           <dxwgv:aspxgridview id="xgvPerson" runat="server" autogeneratecolumns="False" clientinstancename="xgvPerson"
               cssfilepath="~/App_Themes/Aqua/{0}/styles.css" csspostfix="Aqua"
               datasourceid="ObjectDataSource1" keyfieldname="ROLEID" width="100%"><Columns>
<dxwgv:GridViewCommandColumn VisibleIndex="0" ShowSelectCheckbox="True">
    <HeaderStyle HorizontalAlign="Center" />
    <HeaderTemplate>
        <input id="chkSelectedAll" type="checkbox" onclick="xgvPerson.SelectAllRowsOnPage(this.checked);" style="vertical-align:middle;" title="选择或不选本页所有的角色" />
    </HeaderTemplate>
    <ClearFilterButton Visible="True">
    </ClearFilterButton>
</dxwgv:GridViewCommandColumn>
<dxwgv:GridViewDataTextColumn Caption="角色编号" FieldName="ROLEID" 
                        VisibleIndex="0" ReadOnly="True" Visible="False">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="角色名称" FieldName="ROLENAME" 
                        VisibleIndex="1">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="角色描述" FieldName="ROLEABOUT" 
                        VisibleIndex="2">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataDateColumn Caption="创建时间" FieldName="CREATETIME" 
                        VisibleIndex="3">
                    </dxwgv:GridViewDataDateColumn>
                    <dxwgv:GridViewDataTextColumn Caption="角色状态" FieldName="ROLESTATUS" 
                        VisibleIndex="4" Visible="False">
                    </dxwgv:GridViewDataTextColumn>
</Columns>

<Styles CssPostfix="Aqua" CssFilePath="~/App_Themes/Aqua/{0}/styles.css">
</Styles>

<Images ImageFolder="~/App_Themes/Aqua/{0}/">

<CollapsedButton Height="15px" Width="15px" 
        Url="~/App_Themes/Aqua/GridView/gvCollapsedButton.png"></CollapsedButton>

<ExpandedButton Height="15px" Width="15px" 
        Url="~/App_Themes/Aqua/GridView/gvExpandedButton.png"></ExpandedButton>

<DetailCollapsedButton Height="15px" Width="15px" 
        Url="~/App_Themes/Aqua/GridView/gvDetailCollapsedButton.png"></DetailCollapsedButton>

<DetailExpandedButton Height="15px" Width="15px" 
        Url="~/App_Themes/Aqua/GridView/gvDetailExpandedButton.png"></DetailExpandedButton>

    <HeaderFilter Height="19px" Url="~/App_Themes/Aqua/GridView/gvHeaderFilter.png" 
        Width="19px" />
    <HeaderActiveFilter Height="19px" 
        Url="~/App_Themes/Aqua/GridView/gvHeaderFilterActive.png" Width="19px" />
    <HeaderSortDown Height="5px" 
        Url="~/App_Themes/Aqua/GridView/gvHeaderSortDown.png" Width="7px" />
    <HeaderSortUp Height="5px" Url="~/App_Themes/Aqua/GridView/gvHeaderSortUp.png" 
        Width="7px" />

<FilterRowButton Height="13px" Width="13px"></FilterRowButton>
    <WindowResizer Height="13px" Url="~/App_Themes/Aqua/GridView/WindowResizer.png" 
        Width="13px" />
</Images>
               <ClientSideEvents SelectionChanged="function(s, e) {
	OnGridSelectionChanged();
}" />
               <Settings ShowGroupPanel="True" ShowFilterRow="True" />
               <SettingsText GroupPanel="把一列拖放到这里分组" EmptyDataRow="暂无数据！" />
               <SettingsLoadingPanel Text="" />
               <SettingsPager AlwaysShowPager="True">
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
</dxwgv:aspxgridview>
         
       </td>
      </tr>
     </tbody>
  </table>  
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
</div>
<table width="95%">
<tr>
<td align="center"><asp:Button ID="btnAddPerson" runat="server" CssClass="btn_2k3" 
        Text="添  加" OnClick="btnAddPerson_Click" 
        onclientclick="return confirm(&quot;你确定要添加这些角色吗？&quot;);" /></td>
<td align="center">
    <asp:Button ID="btnCancel" runat="server" CssClass="btn_2k3" 
        Text="关  闭" onclick="btnCancel_Click" /></td>
</tr></table>
</div>    
</form>
</body>
</html>
