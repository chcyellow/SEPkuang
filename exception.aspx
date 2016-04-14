<%@ Page Language="C#" AutoEventWireup="true" CodeFile="exception.aspx.cs" Inherits="exception" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>异常页面</title>
    <style type="text/css">
    <!--
    <style type="text/css"> 
    <!--
    INPUT {
	    FONT-SIZE: 12px
    }
    TD {
	    FONT-SIZE: 12px
    }
    .p2 {
	    FONT-SIZE: 12px
    }
    .p6 {
	    FONT-SIZE: 12px; COLOR: #1b6ad8
    }
    A {
	    COLOR: #1b6ad8; TEXT-DECORATION: none
    }
    A:hover {
	    COLOR: red
    }
    .STYLE1 {color: #FF0000}
    --> 
    </style> 
</head>
<body>
    <form id="form1" runat="server">
    <table width="90%" border="0" align="center" cellpadding="5" cellspacing="0"> 
  <tr> 
    <td align="center"><img src="Images/404_error.gif" width="329" height="211" /></td> 
  </tr> 
  <tr> 
    <td align="left"><span class="STYLE1">出错原因：<asp:TextBox ID="txtError" runat="server" BorderStyle="Groove" ForeColor="Red" ReadOnly="True" TextMode="MultiLine" Width="100%" Rows="3"></asp:TextBox></span></td>
  </tr> 
  <tr> 
    <td align="left"><span class="STYLE1">我们很抱歉给您的使用带来不便，您可以把出错原因通过以下方式告知我们。谢谢！</span></td>  
  </tr>
  <tr> 
    <td style="font-size:14px; background-image:url(Images/bg_Blue.gif); color:Blue;">打电话：0561-4954701（免长途费) | 发邮件：<a href="mailto:513296549@gmail.com">513296549@qq.com</a></td> 
  </tr>  
  <tr> 
    <td align="center"><table cellspacing="0" cellpadding="0" width="480" border="0"> 
        <tr> 
          <td width="6"><Img height="26" src="Images/404_left.gif" width="7" /></td> 
          <td background="images/404_bg.gif"><div align="center"><font class="p6"><a href="Main.aspx" target="mainfrm">返回首页</a>　 　|　　 <a href="javascript:history.go(-1)">返回前一页</a>　 　|　　 <a href="javascript:window.close()">关闭本页</a></font> </div></td> 
          <td width="7"><Img src="Images/404_right.gif" width="7" height="26" /></td> 
        </tr> 
      </table>&nbsp;</td> 
  </tr> 
</table> 
    </form>
</body>
</html>
