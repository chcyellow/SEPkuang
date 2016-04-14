// JScript 文件

function AutoResizeIframe()
{
    {
        //document.getElementById("mainFrame").style.height=document.frames["mainFrame"].document.body.scrollHeight+1+"px";
        document.getElementById("mainFrame").style.height = document.getElementById("mainContent").style.height;
        document.getElementById("mainFrame").style.width = document.getElementById("mainContent").style.width;
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
    window.parent.frames['mainFrame'].location.href = "main.aspx";
}
function navigateUrl_Index()
{
    window.parent.frames['mainFrame'].location.href = "main.aspx";
}
function navigateUrl_Refresh()
{
    window.parent.frames['mainFrame'].location.href = window.parent.frames['mainFrame'].location.href;  
}
function navigateUrl_help()
{
    window.parent.frames['mainFrame'].location.href = "Help/Help.htm";
}
function navigateUrl_exit()
{
    if(!confirm("您确定要退出吗？")) return;
    window.location.href("logout.aspx");
}