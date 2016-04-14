<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Main_JTLeader.aspx.cs" Inherits="Main_JTLeader" %>

<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .STYLE1 {font-size: 36px; color: #000099; font-weight: bold; }
        .STYLE2 {font-size: 24px; color: #000099; font-weight: bold; }
    </style>
</head>
<body bgcolor="#C7DBF6">
    <form id="form1" runat="server">
    <ext:ScriptManager ID="ScriptManager1" runat="server" CleanResourceUrl="false">
    </ext:ScriptManager>
    <table width="100%" border="1" cellpadding="0" cellspacing="0" bordercolor="#CCCCCC">
        <tr><td height="30" align="left" bordercolor="#CCCCCC"><ext:Label ID="lab" runat="server"  Height="50px" AutoScroll="true"></ext:Label></td>
            <td align="right"><table><tr align="right">
            <td>
                <ext:HyperLink ID="HyperLink4" runat="server" Text="视频监控" Icon="Camera" NavigateUrl="http://10.1.10.109/" Target="_blank" />
            </td>
            <td>
                <ext:HyperLink ID="HyperLink3" runat="server" Text="井口礼仪" Icon="Zoom" NavigateUrl="LeaderSearch/JingKouRite.aspx" Target="_self" />
            </td>
                <td>
                <ext:HyperLink ID="HyperLink2" runat="server" Text="矿领导下井查询" Icon="Zoom" NavigateUrl="http://10.1.10.8/hxoffice/hbjc/(S(ssg55q45g430nin1dc3il2zr))/Xiajing/W_Queryzhiban.aspx?kuangbie=%bc%af%cd%c5%b9%ab%cb%be" Target="_blank" />
            </td>
            <td>
                <%--<ext:HyperLink ID="HyperLink1" runat="server" Text="机关领导下井查询" Icon="Zoom" NavigateUrl="http://10.1.10.8/hxoffice/hbjc/(S(ssg55q45g430nin1dc3il2zr))/Xiajing/W_JiGuanxiajing_query.aspx?kuangbie=%bc%af%cd%c5%b9%ab%cb%be" Target="_blank" />--%>
                <ext:HyperLink ID="HyperLink1" runat="server" Text="机关领导下井查询" Icon="Zoom" NavigateUrl="http://10.1.10.8/hxoffice/hbjc/(S(k20vve55w21lp445ntw0pnej))/lingdaoxiajing/JiGuanxiajing_query.aspx?kuangbie=%bc%af%cd%c5%b9%ab%cb%be" Target="_blank" />
            </td>
            </tr></table></td>
        </tr>
        <tr bordercolor="#CCCCCC">
            <td height="100" colspan="3" align="center" bordercolor="#CCCCCC">
                <span class="STYLE1">全局安全概况</span>
                <%--<img alt="" src="Images/pic/全局安全概况简述副本.jpg" />--%></td>
        </tr>
        <tr>
            <td width="50%" height="35" align="center" bordercolor="#CCCCCC" ><span class="STYLE2" >安全统计和报表</span></td>
            <td width="50%" height="35" align="center" bordercolor="#CCCCCC" ><span class="STYLE2" >全局安全动态</span></td>
        </tr>
        <tr>
            <td align="center" bordercolor="#CCCCCC" valign="top"><table width="100%" border="0">
              <%--<tr>
                <td width="50%" align="center"><img title="井下状况图" name="lddl_sp_r3_c3_r3_c3" 
                            src="Images/pic/lddl_sp_r3_c3_r3_c3.jpg" width="216" height="67" border="0" 
                            id="lddl_sp_r3_c3_r3_c3" alt="" onmouseover="this.style.cursor='hand'" onclick="document.location.href='./JingxiaMap/Map.aspx'" /></td>
                <td align="center"><img title="干部走动情况" name="lddl_sp_r3_c3_r4_c4" src="Images/pic/lddl_sp_r3_c3_r4_c4.jpg" width="217" height="68" border="0" id="lddl_sp_r3_c3_r4_c4" alt="" onmouseover="this.style.cursor='hand'" onclick="document.location.href='./LeaderSearch/MPInfDetail.aspx'" /></td>
              </tr>--%>
              <tr>
                <td align="center" style="height:100px;"><img title="全局安全概况" name="lddl_sp_r3_c3_r3_c2" src="Images/MLpic/全局安全概况.bmp" width="217" height="67" border="0" id="lddl_sp_r3_c3_r3_c2" alt="" onmouseover="this.style.cursor='hand'" onclick="document.location.href='./LeaderSearch/AllSearch.aspx'" /></td>
                <td align="center" style="height:100px;"><img title="干部走动排查情况报表" name="lddl_sp_r3_c3_r4_c3" src="Images/MLpic/走动和排查情况报表.bmp" width="216" height="68" border="0" id="Img2" alt="" onmouseover="this.style.cursor='hand'" onclick="document.location.href='./kaohe/PCPersonYHPoints.aspx'" /></td>
              </tr>
              <tr>
                <td align="center" style="height:100px;"><img title="机关排查统计" name="lddl_sp_r3_c3_r4_c4" src="Images/MLpic/机关排查统计.bmp" width="216" height="68" border="0" id="Img4" alt="" onmouseover="this.style.cursor='hand'" onclick="document.location.href='./LeaderSearch/JTYHtotalbyperson.aspx'" /></td>
                <td align="center" style="height:100px;"><img title="基层排查统计" name="lddl_sp_r3_c3_r4_c5" src="Images/MLpic/基层排查统计.bmp" width="216" height="68" border="0" id="Img5" alt="" onmouseover="this.style.cursor='hand'" onclick="document.location.href='./LeaderSearch/SafetyStatistics.aspx'" /></td>
              </tr>
              <tr>
                <td align="center" style="height:100px;"><img title="全局隐患统计" name="lddl_sp_r3_c3_r4_c2" src="Images/MLpic/全局隐患统计.bmp" width="217" height="68" border="0" id="lddl_sp_r3_c3_r4_c2" alt="" onmouseover="this.style.cursor='hand'" onclick="document.location.href='./LeaderSearch/YHInfDetail.aspx'" /></td>
                <td align="center" style="height:100px;"><img title="全局三违统计" name="lddl_sp_r3_c3_r4_c4" src="Images/MLpic/全局三违统计.bmp" width="217" height="68" border="0" id="Img3" alt="" onmouseover="this.style.cursor='hand'" onclick="document.location.href='./LeaderSearch/SWInfDetail.aspx'" /></td>
              </tr>
              <tr>
                <td align="center" style="height:100px;"><img title="危险源信息统计" name="lddl_sp_r3_c3_r4_c2" src="Images/MLpic/危险源信息统计.bmp" width="217" height="68" border="0" id="Img1" alt="" onmouseover="this.style.cursor='hand'" onclick="document.location.href='./Search/Sep_search.aspx'" /></td>
                <td align="center" style="height:100px;"><img title="干部走动统计" name="lddl_sp_r3_c3_r4_c3" src="Images/MLpic/干部走动统计.bmp" width="216" height="68" border="0" id="lddl_sp_r3_c3_r4_c3" alt="" onmouseover="this.style.cursor='hand'" onclick="document.location.href='DiaoDu.aspx'" /></td>
                
              </tr>
              
              <%--<tr>
                <td  colspan="2" align="left" >
                    <br /><br /><br /><br />
                </td>
              </tr>--%>
            </table></td>
        
            <td align="center" bordercolor="#CCCCCC"><table width="100%" border="0">
              <tr>
                <td width="50%" align="left"><ext:Panel runat="server" ID="DayPanel" Height="160px" Header="false" 
                            BaseCls="x-grid-record-gray">
                        </ext:Panel></td>
                <td><ext:Panel runat="server" ID="WeekPanel" Height="160px" Header="false" align="left"
                            BaseCls="x-grid-record-gray" >
                        </ext:Panel></td>
              </tr>
              <tr>
                <td height="15" align="left"><ext:Panel runat="server" ID="MonthPanel" Height="160px" Header="false" 
                            BaseCls="x-grid-record-gray">
                        </ext:Panel></td>
                <td align="left">
                <ext:Panel runat="server" ID="YearPanel" Height="160px" Header="false" 
                            BaseCls="x-grid-record-gray">
                        </ext:Panel></td>
              </tr></table></td>
        </tr>
    </table>
    </form>
</body>
</html>