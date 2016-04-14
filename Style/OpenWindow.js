function CloseWin() {
    window.open('YHInfoLogin.aspx?state=full', '登陆页', 'height=600, width=1010,top=84,left=0, toolbar=no, menubar=no, scrollbars=no, resizable=no,location=no,status=no');
    var ua = navigator.userAgent
    var ie = navigator.appName == "Microsoft Internet Explorer" ? true : false
    if (ie) {
        var IEversion = parseFloat(ua.substring(ua.indexOf("MSIE ") + 5, ua.indexOf(";", ua.indexOf("MSIE "))))
        if (IEversion < 5.5) {
            var str = '<object id=noTipClose classid="clsid:ADB880A6-D8FF-11CF-9377-00AA003B7A11">'
            str += '<param name="Command" value="Close"></object>';
            document.body.insertAdjacentHTML("beforeEnd", str);
            document.all.noTipClose.Click();
        }
        else {
            var str = '<object id="WebBrowser" width=0 height=0 classid="CLSID:8856F961-340A-11D0-A96B-00C04FD705A2"></object>'
            document.body.insertAdjacentHTML("beforeEnd", str);
            document.all.WebBrowser.ExecWB(45, 1);
        }
    }
    else {
        window.close()
    }
}