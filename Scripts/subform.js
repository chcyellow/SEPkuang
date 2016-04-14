var winfrm = null;
function openwindow(url, winName, width, height) {
    xposition = 0; yposition = 0;

    if ((parseInt(navigator.appVersion) >= 4)) {
        xposition = (screen.width - width) / 2 - 8;
        yposition = (screen.height - height) / 2 - 31;
    }
    theproperty = "width=" + width + ","
	                        + "height=" + height + ","
	                        + "location=no,"
	                        + "menubar=no,"
	                        + "scrollbars=yes,"
	                        + "status=no,"
	                        + "resizable=no,"
	                        + "titlebar=no,"
	                        + "toolbar=no,"
	                        + "hotkeys=no,"
	                        + "screenx=" + xposition + "," //仅适用于Netscape
	                        + "screeny=" + yposition + "," //仅适用于Netscape
	                        + "left=" + xposition + "," //IE
	                        + "top=" + yposition; //IE 
    winfrm = window.open(url, winName, theproperty);
}
function closewindow() {
    if (winfrm != null && winfrm.open)
        winfrm.close();
}