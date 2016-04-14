// JScript 文件

function AutoResizeIframe()
{
    {
        if (document.frames["mainfrm"].document.body.scrollHeight + 1 < 617) {
            document.getElementById("mainfrm").style.height = 617 + "px";
        }
        else {
            document.getElementById("mainfrm").style.height = document.frames["mainfrm"].document.body.scrollHeight + 1 + "px";
        }
        
        setTimeout('AutoResizeIframe()', 1000);
    }  

}
function reSizeAll()
{
    var div_Main = document.getElementById("container");
    var divNavigate = document.getElementById("divNavigate"); 
    var div_Top_Caption = document.getElementById("div_Top_Caption");
    divNavigate.style.width = div_Main.clientWidth - div_Top_Caption.clientWidth - 40 + "px";
}
function navigateUrl_StartPage()
{
    window.parent.frames['mainfrm'].location.href = "NetPing.aspx";
}
function navigateUrl_Index()
{
    window.parent.frames['mainfrm'].location.href = "Main.aspx";
}
function navigateUrl_Refresh()
{
    window.parent.frames['mainfrm'].location.href = window.parent.frames['mainfrm'].location.href;  
}
function navigateUrl_help()
{
    window.parent.frames['mainfrm'].location.href = "Help/Help.htm";
}
function navigateUrl_exit()
{
    if(!confirm("您确定要退出吗？")) return;
    window.location.href("logout.aspx");
}