<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Main_Leader.aspx.cs" Inherits="Main_Leader" %>

<%@ Register Assembly="Coolite.Ext.Web" Namespace="Coolite.Ext.Web" TagPrefix="ext" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
        <tr><td height="30" colspan="3" align="left" bordercolor="#CCCCCC"><ext:Label ID="lab" runat="server"  Height="50px" AutoScroll="true"></ext:Label></td></tr>
        <tr bordercolor="#CCCCCC">
            <td height="100" colspan="3" align="center" bordercolor="#CCCCCC">
                <span class="STYLE1">ȫ��ȫ�ſ�</span>
                <%--<img alt="" src="Images/pic/ȫ��ȫ�ſ���������.jpg" />--%></td>
        </tr>
        <tr>
            <td width="50%" height="35" align="center" bordercolor="#CCCCCC" ><span class="STYLE2" >��ȫͳ�ƺͱ���</span></td>
            <td width="50%" height="35" align="center" bordercolor="#CCCCCC" ><span class="STYLE2" >ȫ��ȫ��̬</span></td>
        </tr>
        <tr>
            <td align="center" bordercolor="#CCCCCC" valign="top"><table width="100%" border="0">
              <%--<tr>
                <td width="50%" align="center"><img title="����״��ͼ" name="lddl_sp_r3_c3_r3_c3" 
                            src="Images/pic/lddl_sp_r3_c3_r3_c3.jpg" width="216" height="67" border="0" 
                            id="lddl_sp_r3_c3_r3_c3" alt="" onmouseover="this.style.cursor='hand'" onclick="document.location.href='./JingxiaMap/Map.aspx'" /></td>
                <td align="center"><img title="�ɲ��߶����" name="lddl_sp_r3_c3_r4_c4" src="Images/pic/lddl_sp_r3_c3_r4_c4.jpg" width="217" height="68" border="0" id="lddl_sp_r3_c3_r4_c4" alt="" onmouseover="this.style.cursor='hand'" onclick="document.location.href='./LeaderSearch/MPInfDetail.aspx'" /></td>
              </tr>--%>
              <tr>
                <td align="center" style="height:100px;"><img title="ȫ��ȫ�ſ�" name="lddl_sp_r3_c3_r3_c2" src="Images/MLpic/ȫ��ȫ�ſ�.bmp" width="217" height="67" border="0" id="lddl_sp_r3_c3_r3_c2" alt="" onmouseover="this.style.cursor='hand'" onclick="document.location.href='./LeaderSearch/AllSummary.aspx'" /></td>
                <td align="center" style="height:100px;"><img title="Σ��Դ��Ϣͳ��" name="lddl_sp_r3_c3_r4_c2" src="Images/MLpic/Σ��Դ��Ϣͳ��.bmp" width="217" height="68" border="0" id="Img1" alt="" onmouseover="this.style.cursor='hand'" onclick="document.location.href='./Search/Sep_search.aspx'" /></td>
              </tr>
              
              <tr>
                <td align="center" style="height:100px;"><img title="ȫ������ͳ��" name="lddl_sp_r3_c3_r4_c2" src="Images/MLpic/ȫ������ͳ��.bmp" width="217" height="68" border="0" id="lddl_sp_r3_c3_r4_c2" alt="" onmouseover="this.style.cursor='hand'" onclick="document.location.href='./InformationSearch/YHInforSearch.aspx'" /></td>
                <td align="center" style="height:100px;"><img title="ȫ����Υͳ��" name="lddl_sp_r3_c3_r4_c4" src="Images/MLpic/ȫ����Υͳ��.bmp" width="217" height="68" border="0" id="Img3" alt="" onmouseover="this.style.cursor='hand'" onclick="document.location.href='./InformationSearch/SWInforSearch.aspx'" /></td>
              </tr>
              <tr>
                <td align="center" style="height:100px;"><img title="�ɲ��߶�ͳ��" name="lddl_sp_r3_c3_r4_c3" src="Images/MLpic/�ɲ��߶�ͳ��.bmp" width="216" height="68" border="0" id="lddl_sp_r3_c3_r4_c3" alt="" onmouseover="this.style.cursor='hand'" onclick="document.location.href='DiaoDu.aspx'" /></td>
                <td align="center" style="height:100px;"><img title="�ɲ��߶��Ų��������" name="lddl_sp_r3_c3_r4_c3" src="Images/MLpic/�߶����Ų��������.bmp" width="216" height="68" border="0" id="Img2" alt="" onmouseover="this.style.cursor='hand'" onclick="document.location.href='./kaohe/PCPersonYHPoints.aspx'" /></td>
              </tr>
              <tr>
                <td  colspan="2" align="left" >
                    <br /><br /><br /><br />
                </td>
              </tr>
            </table></td>
        
            <td align="center" bordercolor="#CCCCCC"><table width="100%" border="0">
              <tr>
                <td width="50%" align="left"><ext:Panel runat="server" ID="DayPanel" Height="220px" Header="false" 
                            BaseCls="x-grid-record-gray">
                        </ext:Panel></td>
                <td><ext:Panel runat="server" ID="WeekPanel" Height="220px" Header="false" align="left"
                            BaseCls="x-grid-record-gray" >
                        </ext:Panel></td>
              </tr>
              <tr>
                <td height="15" align="left"><ext:Panel runat="server" ID="MonthPanel" Height="220px" Header="false" 
                            BaseCls="x-grid-record-gray">
                        </ext:Panel></td>
                <td align="left">
                <ext:Panel runat="server" ID="YearPanel" Height="220px" Header="false" 
                            BaseCls="x-grid-record-gray">
                        </ext:Panel></td>
              </tr></table></td>
        </tr>
    </table>
    </form>
</body>
</html>
