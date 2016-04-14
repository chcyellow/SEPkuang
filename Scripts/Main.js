var showMessage = function() {
    if (Ext.getCmp('btnNotice').text != '公告') {
        Ext.getCmp('btnNotice').setText('公告');
        winNotice.x = Ext.getBody().getWidth() - 332;
        winNotice.y = Ext.getBody().getHeight() - 232;
        winNotice.show();
    }
    else {
        Ext.getCmp('btnNotice').setText('公告');
        winNotice.x = Ext.getBody().getWidth() - 332;
        winNotice.y = Ext.getBody().getHeight() - 202;//252
        winNotice.show();
    }
}

var showClock = function() {
    Ext.TaskMgr.start
	({
		run: function() { Ext.fly(lblClock.getEl()).update(new Date().format('Y年m月d日 星期D G:i:s')); }, interval: 1000
	});
}