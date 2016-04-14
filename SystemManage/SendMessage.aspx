<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SendMessage.aspx.cs" Inherits="SystemManage_SendMessage" %>

<%@ Register Assembly="DevExpress.Web.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxUploadControl" TagPrefix="dxuc" %>

<%@ Register Assembly="DevExpress.Web.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxLoadingPanel" TagPrefix="dxlp" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Css/iframe.css" rel="stylesheet" type="text/css" media="screen"/>
    <link href="../css/FontStyle.css" rel="stylesheet" type="text/css" media="screen"/>
    <link href="../Css/PopWindows.css" rel="stylesheet" type="text/css" media="screen"/>
    <script type="text/javascript" src="../Scripts/Calendar.js"></script>
    <script type="text/javascript">
    function DelConfirm()
    {
        var listbox = document.getElementById('xlstPerson');
        for(var i=0;i<listbox.options.length;i++)   
        if (listbox.options[i].selected) {
            return confirm("你确定要删除此项吗？");
        }
    }
    function ClearConfirm()
    {
        var listbox = document.getElementById('xlstPerson');
        if (listbox.options.length > 0) {
            return confirm("你确定要清空列表吗？");
        }
    }
    function divmin(id)
    {
        var div = document.getElementById(id);
        if(div.style.display=='none')
        {
            div.style.display='';
        }
        else
        {
            div.style.display='none';
        }
    }

    function divclose(id)
    {
        document.getElementById(id).style.display='none';
    }
    <!--   
// 这里都是公用函数，挺多的   
var   
// 获取元素   
$ = function(element) {   
 return (typeof(element) == 'object' ? element : document.getElementById(element));   
},   
// 判断浏览器   
brower = function() {   
 var ua = navigator.userAgent.toLowerCase();
    var os = new Object();
    os.isFirefox = ua.indexOf ('gecko') != -1;
    os.isOpera = ua.indexOf ('opera') != -1;
    os.isIE = !os.isOpera && ua.indexOf ('msie') != -1;
    os.isIE7 = os.isIE && ua.indexOf ('7.0') != -1;
    return os;

},   
// 事件操作(可保留原有事件)   
eventListeners = [],   
findEventListener = function(node, event, handler){   
 var i;   
 for (i in eventListeners){   
  if (eventListeners[i].node == node && eventListeners[i].event == event && eventListeners[i].handler == handler){   
   return i;   
  }   
 }   
 return null;   
},   
myAddEventListener = function(node, event, handler){   
 if (findEventListener(node, event, handler) != null){   
  return;   
 }   
 if (!node.addEventListener){   
  node.attachEvent('on' + event, handler);   
 }else{   
  node.addEventListener(event, handler, false);   
 }   
 eventListeners.push({node: node, event: event, handler: handler});   
},   
removeEventListenerIndex = function(index){   
 var eventListener = eventListeners[index];   
 delete eventListeners[index];   
 if (!eventListener.node.removeEventListener){   
  eventListener.node.detachEvent('on' + eventListener.event,   
  eventListener.handler);   
 }else{   
  eventListener.node.removeEventListener(eventListener.event,   
  eventListener.handler, false);   
 }   
},   
myRemoveEventListener = function(node, event, handler){   
 var index = findEventListener(node, event, handler);   
 if (index == null) return;   
 removeEventListenerIndex(index);   
},   
cleanupEventListeners = function(){   
 var i;   
 for (i = eventListeners.length; i > 0; i--){   
  if (eventListeners[i] != undefined){   
   removeEventListenerIndex(i);   
  }   
 }   
};   
-->  
        </script>
        <script language="javascript" type="text/javascript">  
<!--   
/*======================================================   
 - statInput 输入限制统计   
 - By Mudoo 2008.5   
 - 长度超出_max的话就截取貌似没有更好的办法了   
======================================================*/   
function statInput(e, _max, _exp) {   
    e   = $(e);   
    _max  = parseInt(_max);   
    _max  = isNaN(_max) ? 0 : _max;   
    _exp_exp  = _exp==undefined ? {} : _exp;   
    
    if(e==null || _max==0) {   
        alert('statInput初始化失败！');   
         return;   
     }   
    
     var   
     // 浏览器   
     _brower  = brower();   
     // 输出对象   
     _objMax  = _exp._max==undefined ? null : $(_exp._max),   
     _objTotal = _exp._total==undefined ? null : $(_exp._total),   
     _objLeft = _exp._left==undefined ? null : $(_exp._left),   
     // 弹出提示   
     _hint  = _exp._hint==undefined ? null : _exp._hint;   
    
     // 初始统计   
    if(_objMax!=null) _objMax.innerHTML = _max;   
    if(_objTotal!=null) _objTotal.innerHTML = 0;   
    if(_objLeft!=null) _objLeft.innerHTML = 0;   
    
     // 设置监听事件   
    // 输入这个方法比较好.   
     // 但是Opera下中文输入跟粘贴不能正确统计相当BT的东西   
     // 如果不考虑Opera的话就用这个吧.否则就老老实实用计时器.   
     if(_brower.isIE) {   
          myAddEventListener(e, "propertychange", stat);   
     }else{   
         myAddEventListener(e, "input", stat);   
     }   
    /*   
    // 用计时器的话就什么浏览器都支持了.   
    var _intDo = null;   
     myAddEventListener(e, "focus", setListen);   
     myAddEventListener(e, "blur", remListen);   
     function setListen() {   
    _intDo = setInterval(stat, 10);   
    }   
    function remListen() {   
    clearInterval(_intDo);   
    }   
    */   
    
 // 统计函数   
 var _len, _olen, _lastRN, _sTop;   
 _olen = _len = 0;   
 function stat() {   
  _len = e.value.length;   
  if(_len==_olen) return;  // 防止用计时器监听时做无谓的牺牲   
  if(_len>_max) {   
   _sTop = e.scrollTop;   
   // 避免IE最后俩字符为'\r\n'.导致崩溃   
   _lastRN = (e.value.substr(_max-1, 2) == "\r\n");   
   e.value = e.value.substr(0, (_lastRN ? _max-1 : _max));   
   if(_hint==true) alert("悟空你也太调皮了，为师跟你说过，叫你不要输那么多字~~.");   
   // 解决FF老是跑回顶部   
   if(_brower.isFirefox) e.scrollTop = e.scrollHeight;   
  }   
  _olen = _len = e.value.length;   
     
  // 显示已输入字数   
  if(_objTotal!=null) _objTotal.innerHTML = _len;   
  // 显示剩余可输入字数   
  if(_objLeft!=null) _objLeft.innerHTML = (_max-_len)<0 ? 0 : (_max-_len);   
 }   
    
 stat();   
}   
-->  
        </script>
        <script language="javascript" type="text/javascript">  
<!--   
/*********************************************   
  - statInput 演示函数   
*********************************************/   
myAddEventListener(window, "load", testStatInput);   
function testStatInput(){   
 statInput('txtMsg', 210, {_max : 'stat_max', _total : 'stat_total', _left : 'stat_left', _hint : false});   
}   
-->
 </script> 
</head>
<body>
    <form id="sendMsgForm" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="header" class="Manager16b">
    发送短信</div>
<div id="diviframe"> 
 <div id="divOuter"> 
 <div id="divCaption"> 
 <div id="divCaption_Left" class="Manager13b"> 
      <img alt="" src="../images/msgtext.gif" id="imgCaption2" /> 
            短信内容
 </div> 
</div> 
<div id="divMain">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
<div class="banner">
<div class="b_left" style="width: 52%; font-size:small; color:Blue;">
    末尾发送者名称(可不填):<asp:TextBox ID="txtSender" runat="server" BorderStyle="Groove" Width="160px"></asp:TextBox></div>
<div class="b_center" style="width: 10%">
</div>
<div class="b_right">
    </div>
</div>
 </ContentTemplate>
 </asp:UpdatePanel>
<div id="panMain" class="Manager13"> 
      <img alt="" src="../images/tip.gif" id="imgCaption" />短信内容最多可输入<span id="stat_max" class="b light" runat="server"></span>字，当前共<span id="stat_total" class="b light" runat="server"></span>字，还可输入<span id="stat_left" class="b light" runat="server"></span>字。
    <textarea name="txtMsg" id="txtMsg" rows="5" style="WIDTH: 95%; border-color: #4F93E3" runat="server"></textarea>
</div>
</div>
<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
<div id="divOuter2"> 
<div id="divCaption2"> 
<div id="divCaption_Left2" class="Manager13b"> 
            <img alt="" src="../images/msgperson.gif" id="ucRankArticles_imgCaption" align="absMiddle" /> 
            发送对象</div> 
</div> 
<div id="divMain2"> 
<div id="panMain2" class="Manager13"><img alt="" src="../images/tip.gif" />当前设定<dxe:aspxlabel
                    id="xlblPersonNumber" runat="server" forecolor="Blue" Text="0" ClientInstanceName="xlblPersonNumber" Font-Bold="True"></dxe:aspxlabel>个发送对象。&nbsp;<br />
    <asp:ListBox ID="xlstPerson" runat="server" Width="50%" Rows="5"></asp:ListBox></div>
<div class="banner">
<table>
<tr>
<%--<td><asp:Button ID="btnAddPerson" runat="server" CssClass="btn_2k3" Text="修改人员列表" 
        onclick="btnAddPerson_Click" /></td>--%>
<td><asp:Button ID="btnDel" runat="server" CssClass="btn_2k3" Text="删除选中对象" OnClick="btnDel_Click" OnClientClick="return DelConfirm();" />&nbsp;</div></td>
<td><asp:Button ID="btnClear" runat="server" CssClass="btn_2k3" Text="清空对象" OnClick="btnClear_Click" OnClientClick="return  ClearConfirm();" /></td>
</tr>
</table>
<div id="inputMan" class="Manager13" style="width: 685px">
    手动添加（多个号码之间用空格键隔开）：<asp:TextBox ID="txtPhoneNo" runat="server" CssClass="txt" Width="480px" onkeypress="if((event.keyCode < 48 && event.keyCode != 32) || event.keyCode > 57) event.returnValue = false;"></asp:TextBox>
    <asp:Button ID="AddPhoneNo" runat="server" CssClass="btn_2k3" Text="添加" OnClick="AddPhoneNo_Click" />(重复号码会自动过滤)
</div>   
 </div> 
</div>

<div class="banner">
<div class="b_left" id="left">
<asp:Button ID="btnSendMsg" runat="server" CssClass="btn_2k3" Text="发送消息" OnClick="btnSendMsg_Click" onclientclick="LoadingPanel.Show();"/>
    </div>
</div>
<div>
    <dxlp:ASPxLoadingPanel ID="LoadingPanel" runat="server" 
        ClientInstanceName="LoadingPanel" Modal="True" 
        CssFilePath="~/App_Themes/BlackGlass/{0}/styles.css" CssPostfix="BlackGlass" 
        ImageFolder="~/App_Themes/BlackGlass/{0}/" Text="发送中&amp;hellip;">
    </dxlp:ASPxLoadingPanel>
</div>
</ContentTemplate>
 </asp:UpdatePanel>
</div>
<table>
<tr>
<td>
导入号码：<dxuc:ASPxUploadControl ID="upctrlPsn" runat="server" Width="480px">
<ValidationSettings AllowedContentTypes="application/vnd.ms-excel" FileDoesNotExistErrorText="文件不存在！"
                            GeneralErrorText="文件上传失败！" MaxFileSize="500000" MaxFileSizeErrorText="文件不要大于500k！"
                            NotAllowedContentTypeErrorText="请上传正确的Excel文件格式(xls)">
                        </ValidationSettings>
    </dxuc:ASPxUploadControl>
</td>
<td>
    <asp:Button ID="btnUpdate" runat="server" CssClass="btn_2k3" Text="上传" onclick="btnUpdate_Click" onclientclick="LoadingPanel.Show();" /></td>
</tr>
</table>
</form>
</body>
</html>
