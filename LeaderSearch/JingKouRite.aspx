<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JingKouRite.aspx.cs" Inherits="LeaderSearch_JingKouRite" %>
<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>井口礼仪</title>
<style>  
#warp {   
  position: absolute;   
  width:100%;   
  height:100%;   
  left:45%;   
  top:40%;   
  margin-left:-250px;   
  margin-top:-100px;   
  border: 0;   
}   
</style>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" CleanResourceUrl="false">
    </ext:ScriptManager>
    <div id="warp"> 
    <table   width= "100% "   HEIGHT= "100% " border= "0 "   cellspacing= "0 "   cellpadding= "0 "> 
    <tr> 
        <td ALIGN= "center "   VALIGN= "middle "> 
        <table   width= "50% "   border= "1 "   align= "center "   cellpadding= "0 " cellspacing= "0 " style=" font-size:x-large;"> 
                <tr>
    			<td><ext:HyperLink ID="HyperLink1" runat="server" Text="岱河矿" Icon="Zoom" NavigateUrl="http://10.36.13.108/" Target="_blank"  /></td>
    			<td><ext:HyperLink ID="HyperLink2" runat="server" Text="祁南矿" Icon="Zoom" NavigateUrl="http://10.36.13.108/" Target="_blank" Disabled ="true"  /></td>
    			<td><ext:HyperLink ID="HyperLink13" runat="server" Text="朱仙庄矿" Icon="Zoom" NavigateUrl="http://10.36.13.108/" Target="_blank" Disabled ="true" /></td>
    		</tr>
   			<tr>
    			<td><ext:HyperLink ID="HyperLink12" runat="server" Text="卢岭矿" Icon="Zoom" NavigateUrl="http://10.36.13.108/" Target="_blank" Disabled ="true"/></td>
    			<td><ext:HyperLink ID="HyperLink11" runat="server" Text="石台矿" Icon="Zoom" NavigateUrl="http://10.36.13.108/" Target="_blank" Disabled ="true"/></td>
    			<td><ext:HyperLink ID="HyperLink10" runat="server" Text="杨庄矿" Icon="Zoom" NavigateUrl="http://10.36.13.108/" Target="_blank" Disabled ="true"/></td>
    		</tr>
    		<tr>
    			<td><ext:HyperLink ID="HyperLink9" runat="server" Text="童亭矿" Icon="Zoom" NavigateUrl="http://10.36.13.108/" Target="_blank" Disabled ="true"/></td>
    			<td><ext:HyperLink ID="HyperLink8" runat="server" Text="海孜矿" Icon="Zoom" NavigateUrl="http://10.36.13.108/" Target="_blank" Disabled ="true"/></td>
    			<td><ext:HyperLink ID="HyperLink7" runat="server" Text="孙疃矿" Icon="Zoom" NavigateUrl="http://10.36.13.108/" Target="_blank" Disabled ="true"/></td>
    		</tr>
    		<tr>
    			<td><ext:HyperLink ID="HyperLink6" runat="server" Text="朱庄矿" Icon="Zoom" NavigateUrl="http://10.36.13.108/" Target="_blank" Disabled ="true"/></td>
    			<td><ext:HyperLink ID="HyperLink5" runat="server" Text="临涣矿" Icon="Zoom" NavigateUrl="http://10.36.13.108/" Target="_blank" Disabled ="true"/></td>
    			<td><ext:HyperLink ID="HyperLink4" runat="server" Text="袁庄矿" Icon="Zoom" NavigateUrl="http://10.36.13.108/" Target="_blank" Disabled ="true"/></td>
    		</tr>
    		<tr>
    			<td><ext:HyperLink ID="HyperLink3" runat="server" Text="桃园矿" Icon="Zoom" NavigateUrl="http://10.36.13.108/" Target="_blank" Disabled ="true"/></td>
    			<td><ext:HyperLink ID="HyperLink14" runat="server" Text="朔里矿" Icon="Zoom" NavigateUrl="http://10.36.13.108/" Target="_blank" Disabled ="true"/></td>
    			<td><ext:HyperLink ID="HyperLink15" runat="server" Text="许疃煤矿" Icon="Zoom" NavigateUrl="http://10.36.13.108/" Target="_blank" Disabled ="true"/></td>
    		</tr>
    		<tr>
    			<td><ext:HyperLink ID="HyperLink16" runat="server" Text="涡北煤矿" Icon="Zoom" NavigateUrl="http://10.36.13.108/" Target="_blank" Disabled ="true"/></td>
    			<td><ext:HyperLink ID="HyperLink17" runat="server" Text="刘店煤矿" Icon="Zoom" NavigateUrl="http://10.36.13.108/" Target="_blank" Disabled ="true"/></td>
    			<td><ext:HyperLink ID="HyperLink18" runat="server" Text="杨柳煤矿" Icon="Zoom" NavigateUrl="http://10.36.13.108/" Target="_blank" Disabled ="true"/></td>
    		</tr>
    		<tr>
    			<td></td>
    			<td></td>
    			<td></td>
    		</tr>
            </table> 
        </td> 
    </tr> 
</table> 
    </div>
    </form>
</body>
</html>
