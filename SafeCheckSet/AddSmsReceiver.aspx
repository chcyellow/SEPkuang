<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddSmsReceiver.aspx.cs" Inherits="SafeCheckSet_AddSmsReceiver" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dxwgv" %>
<%@ Register Assembly="ALinq.Web, Version=1.0.8.0, Culture=neutral, PublicKeyToken=2b23f34316d38f3a"
    Namespace="ALinq.Web.Controls" TagPrefix="cc1" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>添加短信接收者</title>
      <base target="_self"/>
     <meta http-equiv="Pragma" content="no-cache" />   
     <meta http-equiv="Cache-Control" content="no-cache" />   
     <meta http-equiv="Expires" content="0" />
     <link href="../Style/FontStyle.css" rel="stylesheet" type="text/css" media="screen"/>
    <link href="../Style/PopWindows2.css" rel="stylesheet" type="text/css" media="screen" /> 
    <script type="text/javascript"><!--
        function OnGridSelectionChanged() {
            var counter = document.getElementById("selCount");
            if(counter != null) setInnerText(counter, xgvPerson.GetSelectedRowCount().toString());
            xgvPerson.GetSelectedFieldValues('PERSONNUMBER', OnGridSelectionComplete);
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
    <fieldset id="Fieldset1" class="Fieldsetbox mbox pbox">
        <legend class="fsTitle">选择隐患信息</legend>
        <table width="100%" border="0" cellpadding="0" cellspacing="5">
            <tr>
                <td style=" text-align:right;">
                    隐患级别：</td>
                <td>
                    <dxe:ASPxComboBox ID="ddlYHLevel" runat="server" 
                        CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
                        CssPostfix="Office2003_Blue" ImageFolder="~/App_Themes/Office2003Blue/{0}/" 
                        ValueType="System.String" DataSourceID="adsYHLevel" TextField="Infoname" 
                        ValueField="Infoid">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxComboBox>
                </td>
                <td style=" text-align:right;">
                    隐患部门：</td>
                <td>
                    <dxe:ASPxComboBox ID="ddlYHDept" runat="server" 
                        CssFilePath="~/App_Themes/Office2003Blue/{0}/styles.css" 
                        CssPostfix="Office2003_Blue" ImageFolder="~/App_Themes/Office2003Blue/{0}/" >
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxComboBox>
                </td>
            </tr>
            
        </table>
    </fieldset>
    <div>
    <div id="diviframe"> 
 <div id="divOuter"> 
 <div id="divCaption"> 
 <div id="divCaption_Left" class="Manager13b"> 
     人员信息
 </div> 
</div> 
<div id="panMain" class="Manager13">
  <table style="width:100%"><tbody>
      <tr>
      <td style="width: 160px; height: 26px;"></td>
      <td style="height: 26px">
          <fieldset id="Fieldset2" class="Fieldsetbox mbox pbox">
        <legend class="fsTitle">查询操作</legend>
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
                <%--<td style=" text-align:right;">
                 手机：   
                </td>
                <td>
                    <asp:TextBox ID="txtPhone" runat="server" Width="80px"></asp:TextBox>
                </td>--%>
                <td style=" text-align:right;">
                 灯号：   
                </td>
                <td>
                    <asp:TextBox ID="txtLightNo" runat="server" Width="100px"></asp:TextBox>
                </td>
             </tr>
             <tr>
             <td style=" text-align:right;">
                 单位：
                </td>
                <td>
                    <asp:DropDownList ID="ddlDept" runat="server" Width="100px" AutoPostBack="True" 
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
                 性别：
                </td>
                <td>
                    <asp:DropDownList ID="ddlSex" runat="server" Width="100px">
                        <asp:ListItem Selected="True" Value="-1">--全部--</asp:ListItem>
                        <asp:ListItem>男</asp:ListItem>
                        <asp:ListItem>女</asp:ListItem>
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
                <asp:Button ID="btnCancelTxt" runat="server" onclick="btnCancelTxt_Click" 
                    Text="清除条件" />
                </td>
       
            </tr>
        </table>
        
    </fieldset>
      </td>
      </tr>
      <tr>
       <td style="vertical-align:top; width: 160px;">已选择的人员：
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
           已选择人数： <a id="selCount" type="text/plain">0</a>
       </td>
       <td style="vertical-align:top">
       <input id="btnSelectAll" onclick="xgvPerson.SelectRows();" type="button" value="选择所有" class="btn_2k3" />
          <input id="btnUnselectAll" onclick="xgvPerson.UnselectRows();" type="button" value="取消全选" class="btn_2k3" />
           <dxwgv:aspxgridview id="xgvPerson" runat="server" autogeneratecolumns="False" clientinstancename="xgvPerson"
               cssfilepath="~/App_Themes/Aqua/{0}/styles.css" csspostfix="Aqua" 
               keyfieldname="PERSONID" width="100%" 
               oncustomcolumndisplaytext="xgvPerson_CustomColumnDisplayText" 
               onhtmlrowprepared="xgvPerson_HtmlRowPrepared"><Columns>
<dxwgv:GridViewCommandColumn VisibleIndex="0" ShowSelectCheckbox="True">
    <HeaderTemplate>
        <input id="chkSelectedAll" type="checkbox" onclick="xgvPerson.SelectAllRowsOnPage(this.checked);" style="vertical-align:middle;" title="选择或不选本页所有的人员" />
    </HeaderTemplate>
</dxwgv:GridViewCommandColumn>
<dxwgv:GridViewDataTextColumn Caption="人员ID" FieldName="PERSONID" Visible="False"
                VisibleIndex="0">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn FieldName="PERSONNUMBER" Caption="工号" 
                 VisibleIndex="0">
                <EditFormSettings Caption="工号" CaptionLocation="Near" />
             </dxwgv:GridViewDataTextColumn>
             <dxwgv:GridViewDataTextColumn FieldName="NAME" Caption="姓名" VisibleIndex="1">
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
             <dxwgv:GridViewDataTextColumn FieldName="TEL" Caption="电话" Visible="False">
                <EditFormSettings Caption="电话" CaptionLocation="Near" />
             </dxwgv:GridViewDataTextColumn>
             <dxwgv:GridViewDataTextColumn FieldName="LIGHTNUMBER" Caption="灯号" VisibleIndex="6">
                <EditFormSettings Caption="灯号" CaptionLocation="Near" />
             </dxwgv:GridViewDataTextColumn>
              <dxwgv:GridViewDataTextColumn Caption="科区" FieldName="DEPTID" 
               VisibleIndex="7">
           </dxwgv:GridViewDataTextColumn>
           <dxwgv:GridViewDataTextColumn Caption="单位" FieldName="MAINDEPTID" 
               VisibleIndex="8">
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
               <SettingsText GroupPanel="把一列拖放到这里分组" EmptyDataRow="暂无数据！" />
               <SettingsLoadingPanel Text="" />
               <SettingsPager Mode="ShowAllRecords" Visible="False">
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
         <webdiyer:AspNetPager ID="AspNetPager1"  
            CurrentPageButtonClass="cpb"  runat="server" FirstPageText="首页" 
            LastPageText="尾页" NextPageText="下一页" PrevPageText="上一页"  AlwaysShow="true" 
            PageIndexBoxType="DropDownList" OnPageChanged="AspNetPager1_PageChanged"  
            ShowCustomInfoSection="left"  NumericButtonTextFormatString="{0}" 
            HorizontalAlign="Left" ></webdiyer:AspNetPager>
       </td>
      </tr>
     </tbody>
  </table>  
</div>
     <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
         SelectMethod="GetPersonByDept" TypeName="PersonBLL">
         <SelectParameters>
             <asp:SessionParameter DefaultValue=" " Name="deptid" SessionField="maindeptid" 
                 Type="String" />
         </SelectParameters>
     </asp:ObjectDataSource>
     <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" 
         SelectMethod="GetDept" TypeName="DepartmentBLL">
         <SelectParameters>
             <asp:SessionParameter DefaultValue=" " Name="deptID" SessionField="deptid" 
                 Type="String" />
         </SelectParameters>
     </asp:ObjectDataSource>
     <asp:ObjectDataSource ID="odsUserGroup" runat="server" 
         SelectMethod="GetUserGroupList" TypeName="GhtnTech.SecurityFramework.UserGroup">
         <SelectParameters>
             <asp:SessionParameter DefaultValue=" " Name="strWhere" 
                 SessionField="WhereUserGroup" Type="String" />
         </SelectParameters>
     </asp:ObjectDataSource>
     <asp:ObjectDataSource ID="odsRole" runat="server" SelectMethod="GetRoleList" 
         TypeName="GhtnTech.SecurityFramework.Role">
         <SelectParameters>
             <asp:SessionParameter DefaultValue="Roleid != 31" Name="strWhere" SessionField="WhereRole" 
                 Type="String" />
             <asp:SessionParameter DefaultValue=" " Name="strOrder" 
                 SessionField="WhereOrder" Type="String" />
         </SelectParameters>
     </asp:ObjectDataSource>
     <cc1:ALinqDataSource ID="adsPerson" runat="server" 
         ContextTypeName="GhtnTech.SEP.DAL.DBSCMDataContext" TableName="Person" >
         
     </cc1:ALinqDataSource>
     <cc1:ALinqDataSource ID="adsDept" runat="server" 
         ContextTypeName="GhtnTech.SEP.DAL.DBSCMDataContext" TableName="Department">
         
     </cc1:ALinqDataSource>
      <cc1:ALinqDataSource ID="adsYHLevel" runat="server" 
            ContextTypeName="GhtnTech.SEP.DAL.DBSCMDataContext" TableName="CsBaseinfoset" 
            EnableViewState="False" Select="new (Infoid, Infoname)" 
            Where="Fid == @Fid">
            <WhereParameters>
                <asp:Parameter DefaultValue="41" Name="Fid" Type="Decimal" />
            </WhereParameters>
        </cc1:ALinqDataSource>
</div>
<table width="95%">
<tr>
<td align="center"><asp:Button ID="btnAddPerson" runat="server" CssClass="btn_2k3" 
        Text="添  加" OnClick="btnAddPerson_Click" 
        onclientclick="return confirm(&quot;你确定要添加这些人员吗？&quot;);" /></td>
<td align="center">
    <asp:Button ID="btnCancel" runat="server" CssClass="btn_2k3" 
        Text="关  闭" onclick="btnCancel_Click" 
        onclientclick="parent.window.Window1.hide();" /></td>
</tr></table>
</div>
    </div>
    </form>
</body>
</html>
