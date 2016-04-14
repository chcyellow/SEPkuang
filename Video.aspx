<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html> 
<head>
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" >
<script type="text/javascript" src="wt.js"></script>
<script type="text/javascript" src="m.js"></script>
<script type="text/javascript" src="sf.js"></script>
<link rel="stylesheet" type="text/css" media="screen" href="m.css" />
<script> 
	// try{
		//document.execCommand('BackgroundImageCache', false, true);
		//}
//catch(e) {}
</script>
<script> 
var gdomready=0;
var gopenall=0;
var gsld;
var gslda;
var gsldb;
var gsldc;
var gsldd;
//var gslde;
var gca=0;
var gcb=0;
var gcc=0;
var gcd=0;
var gwid=0;
var gcid=-1;
var gptz=0;
var gxh=0;
var gxz=0;
var gxs=0;
var gxj=0;
var gdj=0;
var gdg=0;
var gfmu1=0;
var gfmnudj=0;
var gomnudj=null;
var gfmnuopen=0;
var gomnuopen=null;
var i=0;
var j=0;
var glhight=0;
var grhight=287;
var gcam=-1;
var HashCookie = new Hash.Cookie('DhWebCookie',{duration: 30});
var settings = {
	username:'',
	talktype:'1',
	logintype:'0',
	openall:'1'
}
 
function iniocx(){
;
}
function showlogin(){
	$('l').style.top="0px";
}
function ld()
{
    var ip = '10.36.13.108';
	//var ip = location.hostname;
	var username = $("username").value;
	var password = $("password").value;
	var logintype= $("logintype").value;	
	if(gcam==-1)
	{
		var r=ocx.LoginDeviceEx(ip,0,username,password,logintype);
		if (r==1){
			chkdev();
			resize();
			getcl();
			getdjl();
			//if (settings['talktype'] != '0') 
			ocx.SetDeviceMode(0,settings['talktype']);
			$('password').value="";
			$('l').style.display="none";
			$('m').style.top="0px";
			settings['username'] = username;
			settings['logintype'] = logintype;
			
			savesetting();
		}
	}
	else 
	{
		var r=ocx.LoginDeviceEx(ip,0,'op','1234',logintype);
		if (r==1){
			chkdev();
			resize();
			getcl();
			getdjl();
			ocx.SetDeviceMode(0,settings['talktype']);
			$('l').style.display="none";
			$('m').style.top="0px";
			ca($('c' + gcam),gcam);
		}
		
	}
}
function lo()
{
	if (ocx.LogoutDevice() >=0)
	{	
		loeft();
	}
}
function loeft()
{
		$('m').style.top="-10000px";
		$('l').style.display="";
		closemu();
		closemnudj();
		if (gdj==1){
			gdj=0;
			$('xdj').innerText=tl('AudioTalk');
		}
		if (gopenall==1){
			gopenall=0;
			$('xac').innerText=tl('W_ACO');
		}	
		ocx.CloseLocalPlay();
}
function ca(o,ch)
{
	if ($(o).hasClass('cl1')){
		if (ocx.ConnectRealVideo(ch,1)){
			;
		}
	}
	else{
		if (!ocx.DisConnectRealVideo(ch)){
			;
		}
	}
	closemu();
}
 
function cptz(){
	if (gptz==0) 
	{
		gptz=1;
		ocx.ControlPtz(51,0,0,0,0); 
	}
	else{
		gptz=0;
		ocx.ControlPtz(51,0,0,0,1);
	}
	setptzs();	
}
 
function setptzs(){
	if (gdomready==1){
		if (gptz==0){
			if ($('cptz').hasClass('y51')){
				$('cptz').removeClass($('cptz').className);
				$('cptz').addClass('y5');
			}
		}
		else{
			$('cptz').removeClass($('cptz').className);
			$('cptz').addClass('y51');
		}
	}
 
}
function cxh(o,ts,tb){
	if (gxh==0) 
	{		
		if (ocx.ControlPtz(13,$('pv').value,0,76,0))
		{
			gxh=1;
			o.innerText=ts;
		}
	}
	else{		
		if (ocx.ControlPtz(13,$('pv').value,0,96,0))
		{
			gxh=0;
			o.innerText=tb;
		}
	}
}
function cxz(o,ts,tb){
	if (gxz==0) 
	{		
		if (ocx.ControlPtz(39,0,0,0,0))
		{
			gxz=1;
			o.innerText=ts;
		}
	}
	else{		
		if (ocx.ControlPtz(40,0,0,0,0))
		{
			gxz=0;
			o.innerText=tb;
		}	
	}
}
function cxs(o,ts,tb){
	if (gxs==0) 
	{		
		if (ocx.ControlPtz(43,0,0,0,0))
		{
			gxs=1;
			o.innerText=ts;
		}
	}
	else{		
		if (ocx.ControlPtz(44,0,0,0,0))
		{
			gxs=0;
			o.innerText=tb;
		}
	}
}
 
function cxj(o,ts,tb){
	if (gxj==0)
	{		
		if (ocx.ControlPtz(47,$('pv').value,0,0,0))
		{
			gxj=1;
			o.innerText=ts;
		}
	}
	else{		
		if (ocx.ControlPtz(48,$('pv').value,0,0,0))
		{
			gxj=0;
			o.innerText=tb;
		}
	}
}
 
function cdj(o,ts,tb){
	var ret;
	if (gdj==0)
	{	
		if (ocx.ControlTalking(1))
		{
			gdj=1;
			o.innerText=ts;
		}
	}
	else{		
		if (ocx.ControlTalking(0))
		{
			gdj=0;
			o.innerText=tb;
		}
	}
}
 
function cdg(o,ts,tb){
	if (gdg==0)
	{	
		if (ocx.ControlPtz(14,1,0,0,0))
		{
			gdg=1;
			o.innerText=ts;
		}
	}
	else{		
		if (ocx.ControlPtz(14,0,0,0,0))
		{
			gdg=0;
			o.innerText=tb;
		}
	}	
}
 
function sldtopos(sld,step){
	sld.knob.setStyle('left', sld.toPosition(step));
}
function txreset(step){
	setcolorsv(1,step);
	setcolorsv(2,step);
	setcolorsv(3,step);
	setcolorsv(4,step);
	sldtopos(gslda,step);
	sldtopos(gsldb,step);
	sldtopos(gsldc,step);
	sldtopos(gsldd,step);
	setcolors();
}
 
function getcolors(){
	var colors="";
	colors=ocx.GetColor();
	var t= new Array();
	if (colors !="")
		t=colors.split(',');
	sldtopos(gslda,parseInt(t[0]));
	sldtopos(gsldb,parseInt(t[1]));
	sldtopos(gsldc,parseInt(t[2]));
	sldtopos(gsldd,parseInt(t[3]));
	setcolorsv(1,parseInt(t[0]));
	setcolorsv(2,parseInt(t[1]));
	setcolorsv(3,parseInt(t[2]));
	setcolorsv(4,parseInt(t[3]));
}
function setcolorsv(f,v){
		switch (f)
		{
			case 1: gca=v;
					$('ska').title=v;
					break;
			case 2: gcb=v;
					$('skb').title=v;
					break;
			case 3: gcc=v;
					$('skc').title=v;
					break;
			case 4: gcd=v;
					$('skd').title=v;
					break;
		}	
}
function setcolors(){
	ocx.SetColor(0,gca,gcb,gcc,gcd);
}
 
function showmu(o,cid){
	if (gcid==cid){
		closemu();
	}
	else{
		gcid=cid;
		$('cmu').setProperty('title', cid); 
		$('cmu').injectAfter($(o).getParent());
		$('cmu').setStyle('height', 35);	
	}
}
function closemu(){
	gcid=-1;
	$('cmu').injectAfter($('cl')); 
	$('cmu').setStyle('height', '0'); 	
}
function onmu(f){
	var cid=$('cmu').getProperty('title'); 
	ocx.ConnectRealVideo(parseInt(cid),f)
	closemu();
}
 
function tl(s){
	var ret;
	ret=ocx.Translate(s);
	return ret;
}
 
function chkdev(){
	var strhtm="";
	var strhtmopen="";
	var sret="";
	
	sret=ocx.GetDevConfig(1);
	//if (sret.substring(0,1)=="1") 
	strhtm="<a id='xzml' href='javascript:;' onclick='onmu(1)' >"+ tl('MainStream') +"</a>";
	strhtmopen="<li><a href='javascript:;'  class='cdj1' onclick='onmnuopen(this,1)'>" + tl('MainStream')  + "</a></li>";
	if (sret.substring(1,2)=="1")
	{ 
		strhtm=strhtm+"<a id='xfml' href='javascript:;' onclick='onmu(2)'>"+ tl('SecondStream') +"</a>";
		strhtmopen=strhtmopen+"<li><a href='javascript:;'  class='cdj1' onclick='onmnuopen(this,2)'>" + tl('SecondStream')  + "</a></li>";
	}
	$('cmu').setHTML(strhtm);
	$("mnuopenl").innerHTML=strhtmopen;
 
 
	sret=ocx.GetDevConfig(2);
	if (sret=='1')
	{
		$('xkl').style.display="";
	}
 
	sret=ocx.GetDevConfig(7);
	if (sret=='1')
	{
		$('xhf').style.display="";
	}
 
	sret=ocx.GetDevConfig(8);
	if (sret=='1')
	{
		$('xfmq').style.display="";
	}
 
	
}
function savesetting(){
	HashCookie.extend(settings);
}
function getsetting(){
	if (HashCookie.get('username')) {
		settings['username'] = HashCookie.get('username');
	} else {
		settings['username'] = '';
	}
	
	if (HashCookie.get('talktype')) {
		settings['talktype'] = HashCookie.get('talktype');
	} else {
		settings['talktype'] = '1';
	}
	if (HashCookie.get('logintype')) {
		settings['logintype'] = HashCookie.get('logintype');
	} else {
		settings['logintype'] = '0';
	}
 
	if (HashCookie.get('openall')) {
		settings['openall'] = HashCookie.get('openall');
	} else {
		settings['openall'] = '1';
	}
	
	$('username').setProperty('value', settings['username']); 
	$('logintype').setProperty('value',settings['logintype']); 
}
 
function reps(str){
	var strReg1 =/"/g;
	var strReg2=/'/g;
	var strReg3=/</g;
	var strReg4=/>/g;
	var strReg5=/&/g;
	var ret=str.replace(strReg5, "&amp;");
 	ret=ret.replace(strReg1, "&quot;");
	ret=ret.replace(strReg2, "&acute;");
	ret=ret.replace(strReg3, "&lt;");
	ret=ret.replace(strReg4, "&gt;");
	return ret;
}
 
function showmu1(){
	if (gfmu1==0)
	{	
		gfmu1=1;
		$('smu1').style.display="";			
	}
	else{
		closemu1();	
	}	
}
function closemu1(){
	gfmu1=0;
	$('smu1').style.display="none";
}
function onmu1(v){
	$('ps').value=v;
	closemu1();
}
 
function showmnudj(){
	if (gfmnudj==0)
	{	
		gfmnudj=1;
		$('mnudj').style.display="";			
	}
	else{
		closemnudj();	
	}	
}
function closemnudj(){
	gfmnudj=0;
	$('mnudj').style.display="none";
}
function onmnudj(o,v){
	if (gomnudj !=null)
	{
		gomnudj.removeClass(gomnudj.className);
		gomnudj.addClass('cdj1');
	}
	gomnudj=$(o);
	gomnudj.removeClass(gomnudj.className);
	gomnudj.addClass('cdj2');	
	if (v=='0') v='1';
	ocx.SetDeviceMode(0,v);
	cdj($('xdj'),tl('StopTalk'),tl('AudioTalk'));
	closemnudj();
	settings['talktype'] = v;
	savesetting();
}
 
 
 
function showmnuopen(){
	if (gfmnuopen==0)
	{	
		gfmnuopen=1;
		$('mnuopen').style.display="";			
	}
	else{
		closemnuopen();	
	}	
}
function closemnuopen(){
	gfmnuopen=0;
	$('mnuopen').style.display="none";
}
function onmnuopen(o,v){
	if (gomnuopen !=null)
	{
		gomnuopen.removeClass(gomnuopen.className);
		gomnuopen.addClass('cdj1');
	}
	gomnuopen=$(o);
	gomnuopen.removeClass(gomnuopen.className);
	gomnuopen.addClass('cdj2');	
	
	
	closemnuopen();
	settings['openall'] = v;
	savesetting();
	gopenall=0;
	openall($('xac'),tl('W_ACC'),tl('W_ACO'))
}
 
 
function getcl(){	
	var t= new Array();
	var ts=new Array();
	var shtml="";
	var strsplita=String.fromCharCode(9);
	var strsplitb=String.fromCharCode(16);
	var sc;
	
	sc=ocx.GetChannelName();
	
	if (sc !=""){
		sc=sc.substr(0, sc.length-1);
		t=sc.split(strsplita);
		
		for (var i =0; i<t.length;i++ ){
			ts=t[i].split(strsplitb);			
			ts[1]=reps(ts[1]);			
			
			shtml+="<li title='" + ts[1]+ "' ><div id='c" + ts[0] + "' class='cl1' onclick='ca(this," + ts[0] +")' >" + ts[1] + "</div><a class='ca1' href='javascript:;' onclick='showmu(this," + ts[0] + ")'></a></li>"
			t[i]=ts;
		
		}
		$("cl").innerHTML=shtml;
		var ls = $$('#cl li');
		ls.each(function(element) { 
			var fx = new Fx.Styles(element, {duration:100, wait:false}); 
	
			element.addEvent('mouseenter', function(){
				fx.start({
					'opacity':1
				});
			}); 
			element.addEvent('mouseleave', function(){
				fx.start({
					'opacity':0.001
				});
			}); 
		});
	}
}
 
function getdjl(){	
	var t= new Array();
	var ts=new Array();
	var shtml="";
	var strsplita=String.fromCharCode(9);
	var strsplitb=String.fromCharCode(16);
	var sc="";
	sc=ocx.GetDevConfig(3);
	if (sc !=""){
		sc=sc.substr(0, sc.length-1);
		t=sc.split(strsplita);
		
		for (var i =0; i<t.length;i++ ){
			ts=t[i].split(strsplitb);
			ts[1]=reps(ts[1]);
//			if (ts[1] != "G711u")
//			{
				shtml+="<li><a href='javascript:;'  class='cdj1' onclick='onmnudj(this," + ts[0]+ ")'>" + ts[1] + "</a></li>";
//			}
			t[i]=ts;
		}
		$("mnudjl").innerHTML=shtml;
	}
}
 
function rfc(){
	var t= new Array();
	var ts=new Array();
	var shtml="";
	var strsplita=String.fromCharCode(9);
	var strsplitb=String.fromCharCode(16);
	var sc;
	sc=ocx.GetChannelName();
	if (sc !=""){
		sc=sc.substr(0, sc.length-1);
		t=sc.split(strsplita);
		for (var i =0; i<t.length;i++ ){
			ts=t[i].split(strsplitb);
			//ts[1]=reps(ts[1]);
			var temp='c' + ts[0];
			$(temp).setText(ts[1]);
			t[i]=ts;
		}
	}	
}
 
 
 
function reboot(){
	var ret;
	
	if (confirm(tl('w_rebootconfirm'))){	
		ret=ocx.Restart();
		if (ret==0)
		{
			alert(tl('w_rebootfail'));
		}
		else{
			ocx.LogoutDevice();
			loeft();
		}
	}
	
}
 
function openall(o,ts,tb){
	var ret;
	//alert(settings['openall']);
	if (gopenall==0)
	{	
		if (ocx.ConnectAllChannelEx(settings['openall']))
		{
			gopenall=1;
			o.innerText=ts;
		}
	}
	else{		
		if (ocx.DisConnectAllChannel())
		{
			gopenall=0;
			o.innerText=tb;
		}
	}
}
 
function toggleDisplay(obj){
	if (obj.getStyle('display')=='none'){
		obj.setStyle('display','');
	}
	else{
		obj.setStyle('display','none');
	}
}
 
function closebeep(){
	ocx.BeepAlarmControl(0);	
}
 
function limitPs(){
	var inpt=$('ps').value;
	$('ps').value = inpt.replace(/[^\d]/g,'');
	if(inpt=='0')$('ps').value =1;
	else if(inpt=='9') $('ps').value =8;
}
 
function limitPv(){
	$('pv').value = $('pv').value.replace(/[^\d]/g,'');
	var inpt=$('pv').value;
	if(inpt.length >= 3 && (inpt-0)>255) $('pv').value=255;
	else if(inpt!='') $('pv').value=inpt-0;
}
 
function inilanguage(){
	$('xyhm').setText(tl('User Name')+":");
	$('xmm').setText(tl('Password')+":");
	$('lbt').setText(tl('Login'));
	showlogin();
	$('xtc').setText(tl('Logout'));
	//$('xzml').setText(tl('MainStream'));
	//$('xfml').setText(tl('SecondStream'));
	$('xdj').setText(tl('AudioTalk'));
	$('xhf').setText(tl('OpenRec'));
	$('xbb').setText(tl('Zoom'));
	$('xbj').setText(tl('Focus'));
	$('xgq').setText(tl('Iris'));
	$('xbc').setText(tl('w_Step'));	
	$('xz').setText(tl('w_SetValue'));
	$('xtld').setProperty('title', tl('Bright')); 
	$('xtdbd').setProperty('title', tl('Cont')); 
	$('xtbhd').setProperty('title', tl('Sat')); 
	$('xtsd').setProperty('title', tl('Hue')); 
	$('xcz').setText(tl('Reset'));
	$('xztlj').setText(tl('PicturePath'));
	$('xlxlj').setText(tl('RecordPath'));
	$('xztlj').setProperty('title', tl('SetPicturePath')); 
	$('xlxlj').setProperty('title', tl('SetRecordPath')); 
	$('taba1').setText(tl('Img Config'));
	$('taba2').setText(tl('Other Config'));
	$('xyzd').setText(tl('Preset'));
	$('xdjxh').setText(tl('Auto-Tour'));
	$('xspxz').setText(tl('Auto-Pan'));
	$('xxs').setText(tl('Auto-Scan'));
	$('xxuj').setText(tl('Pattern'));
	$('xfzk').setText(tl('AUX ON'));
	$('xfzg').setText(tl('AUX OFF'));
	$('xytsz').setText(tl('w_PTZsetting'));	
	$('xkl').setText(tl('W_Burning'));
	//$('xkl').setProperty('title', tl('w_ptjrtitle'));
	$('xsxt').setText(tl('w_refresh'));
	$('xsxt').setProperty('title', tl('w_trefresh'));
	$('xcqsb').setText(tl('w_reboot'));
	$('xcqsb').setProperty('title', tl('w_reboottitle'));
	$('xac').setText(tl('W_ACO'));
	$('xac').setProperty('title', tl('W_ACO_T'));
	
	$('xp1').setProperty('title', tl('W_P1'));
	$('xp2').setProperty('title', tl('W_P2'));
	$('xp3').setProperty('title', tl('W_P3'));
	$('xp4').setProperty('title', tl('W_P4'));
	$('xp5').setProperty('title', tl('W_P5'));
	
	$('xdlfs').setText(tl('type'));
	$('xdlfszb').setText(tl('w_muticast'));
	$('xladv').setText(tl('C_ADVANCED.'));
	$('xfmq').setText(tl('W_CLOSEBEEP'));
	
	
	inilanguage_ex();
}
function resize(){
	var mbbw;
	var mbbh;
	wwidth =document.documentElement.clientWidth;
	wheight=document.documentElement.clientHeight;
	//if(wwidth<1030) $('m').setStyle('width', 1030);
//	else $('m').setStyle('width', wwidth);
	if (wheight<600) wheight=600;
	//ma:31;mc:18;plc:33;dra,b,c:24
	mbbh=wheight - (31+18+33+24+3+8);
	glhight=mbbh+33;
	grhight=mbbh+33-263-$('yt21').offsetHeight;
	mbbw=mbbh *64/51;
	$('mb').setStyle('width', mbbw+(145*2+8));
	$('mbb').setStyle('width', mbbw);//yzt 0508
	$('mbb').setStyle('height', mbbh);
	$('mbal').setStyle('height', glhight);
	$('yt3').setStyle('height', grhight);
}
 
window.addEvent('resize',function(){
	resize();
});
 
window.addEvent('domready',function(){
	gsld = new Slider($('sa'), $('sk'), {
	steps:1000,
	onComplete:function(step){ocx.SetPlayPos(step);}
	});
	
	gslda = new Slider($('saa'), $('ska'), {
	steps:128,
	onChange:function(step){setcolorsv(1,step);setcolors();}
	});
 
	gsldb = new Slider($('sab'), $('skb'), {
	steps:128,
	onChange:function(step){setcolorsv(2,step);setcolors();}
	});
	
	gsldc = new Slider($('sac'), $('skc'), {
	steps:128,
	onChange:function(step){setcolorsv(3,step);setcolors();}
	});
	
	gsldd = new Slider($('sad'), $('skd'), {
	steps:128,
	onChange:function(step){setcolorsv(4,step);setcolors();}
	});
 
	//gslde = new Slider($('sae'), $('ske'), {
	//steps:100,
	//onChange:function(step){;}
	//});
//menu
	var kwicks = $$('#kwick .kwick');
	var fx = new Fx.Elements(kwicks, {wait: false, duration: 300, transition:Fx.Transitions.Back.easeOut});
	kwicks.each(function(kwick, i){
		kwick.addEvent('mouseenter', function(e){
			var obj = {};
			obj[i] = {
				'width': [kwick.getStyle('width').toInt(), 155]
			};
			kwicks.each(function(other, j){
				if (other != kwick){
					var w = other.getStyle('width').toInt();
					if (w != 80) obj[j] = {'width': [w,80]};
				}
			});
			fx.start(obj);
		});
	});
	
	$('kwick').addEvent('mouseleave', function(e){
		var obj = {};
		kwicks.each(function(other, j){
			obj[j] = {'width': [other.getStyle('width').toInt(), 95]};
		});
		fx.start(obj);
	});
 
//
	var container = $('mb');
	var drop = $('drb');
	var mba=$('mba');
	var mbb=$('mbb');
	var mbc=$('mbc');
	var dropFx = drop.effect('opacity', {wait: false}); // wait is needed so that to toggle the effect,
	var bedra= $('dra');
	var bedrc= $('drc');
/*	
bedra.addEvent('mousedown', function(e) {
		e = new Event(e).stop();			
		var clone = this.clone()
			.setStyles(this.getCoordinates()) // this returns an object with left/top/bottom/right, so its perfect
			.setStyles({'opacity': 0.7, 'position': 'absolute'})
			.addEvent('emptydrop', function() {this.remove();drop.removeEvents();})						
			.inject(document.body);	
		drop.addEvents({
			'drop': function() {
				drop.removeEvents();
				clone.remove();							
				dropFx.start('1');											
				if (mbc.getStyle('left') !='0px')
				{
					mbc.setStyle('left','0');
					mbc.injectBefore(mba);
					mbb.injectBefore(mba);
				}
				else{
					mbc.setStyle('left','120');							
					mba.injectBefore(mbc);
					mbb.injectBefore(mbc);
				}
			},
			'over': function() {
				dropFx.start('0.7');
			},
			'leave': function() {
				dropFx.start('1');
			}						
		});
		var drag = clone.makeDraggable({
			'container':container,
			'droppables': [drop]
		}); // this returns the dragged element
		drag.start(e); // start the event manual	
	});
 
 
	bedrc.addEvent('mousedown', function(e) {
		e = new Event(e).stop();			
		var clone = this.clone()
			.setStyles(this.getCoordinates()) // this returns an object with left/top/bottom/right, so its perfect
			.setStyles({'opacity': 0.7, 'position': 'absolute'})
			.addEvent('emptydrop', function() {this.remove();drop.removeEvents();})						
			.inject(document.body);	
		drop.addEvents({
			'drop': function() {
				drop.removeEvents();
				clone.remove();							
				dropFx.start('1');											
				if (mbc.getStyle('left') !='0px')
				{
					mbc.setStyle('left','0');
					mbc.injectBefore(mba);
					mbb.injectBefore(mba);
				}
				else{
					mbc.setStyle('left','120');							
					mba.injectBefore(mbc);
					mbb.injectBefore(mbc);
				}
			},
			'over': function() {
				dropFx.start('0.7');
			},
			'leave': function() {
				dropFx.start('1');
			}						
		});
		var drag = clone.makeDraggable({
			'container':container,
			'droppables': [drop]
		}); // this returns the dragged element
		drag.start(e); // start the event manual	
	});
*/
//
	var hCyt21 = new Fx.Style('yt21', 'height',{duration:500});
	var hCyt3 = new Fx.Style('yt3', 'height',{duration:500});
	
	$('ayt22').addEvent('click', function(e){
		new Event(e).stop();
		if (this.hasClass('y1'))
		{
			this.removeClass(this.className);
			this.addClass('y2');
			hCyt21.start(0,160);
			
			hCyt3.start(grhight,grhight-160);
			grhight=grhight-160
		}
		else{
			this.removeClass(this.className);
			this.addClass('y1');
			hCyt21.start(160,0);
			hCyt3.start(grhight,grhight+160);
			grhight=grhight+160
		}
	});
 
	var taba1=$('taba1');
	var taba2=$('taba2');
	taba1.addEvent('click', function(e){
		new Event(e).stop();
		if (this.hasClass('t3'))
		{
			this.removeClass(this.className);
			this.addClass('t1');			
			taba2.removeClass(taba2.className);
			taba2.addClass('t2');
			$('yt3t1').style.display="";			
			$('yt3t2').style.display="none";			
		}
	});
	
	taba2.addEvent('click', function(e){
		new Event(e).stop();
		if (this.hasClass('t2'))
		{
			taba1.removeClass(taba2.className);
			taba1.addClass('t3');			
			this.removeClass(this.className);
			this.addClass('t4');
			$('yt3t1').style.display="none";			
			$('yt3t2').style.display="";			
		}
	});
	
//
	document.addEvent('keydown', function(event) {
		event = new Event(event);
		if (event.key == 'up') ocx.ControlPtz(0,0,$('ps').value,0,0);
		if (event.key == 'left') ocx.ControlPtz(2,0,$('ps').value,0,0);
		if (event.key == 'right') ocx.ControlPtz(3,0,$('ps').value,0,0);
		if (event.key == 'down') ocx.ControlPtz(1,0,$('ps').value,0,0);
	});
	document.addEvent('keyup', function(event) {
		event = new Event(event);
		if (event.key == 'up') ocx.ControlPtz(0,0,$('ps').value,0,1);
		if (event.key == 'left') ocx.ControlPtz(2,0,$('ps').value,0,1);
		if (event.key == 'right') ocx.ControlPtz(3,0,$('ps').value,0,1);
		if (event.key == 'down') ocx.ControlPtz(1,0,$('ps').value,0,1);
	});
 
	inilanguage();
	getsetting();
	
	gdomready=1;
});
</script>
 
<script language="javascript" for="ocx" event="ReturnWindInfo(wid,cid,ps,sit)"> 
	gptz=sit;
	gwid=wid;
	if (ps!=0)
	{
		if (ps==5 || ps==4){
			$('pbh').setStyle('display','none');
		}
		else{
			$('pbh').setStyle('display','');
		}
		getcolors();
	}
		setptzs();
	
</script>
 
<script language="javascript" for="ocx" event="ReturnPlayState(pos)"> 
	sldtopos(gsld,pos);
</script>
 
<script language="javascript" for="ocx" event="StateChanged(ci,ps,wid)"> 
	var oc;
	
	if (ci !=-1 && ci!=20){
		oc=$('c' + ci);
		if (ps==0){
			oc.removeClass(oc.className);
			oc.addClass('cl1');
		}
		if (ps==1){
   			//oc.removeClass('cl1');
   			oc.removeClass(oc.className);
   			oc.addClass('cl2');
		}
		if (ps==3){
			//oc.removeClass('cl1');
			oc.removeClass(oc.className);
			oc.addClass('cl3');
		}
		if(ps==6){
   			oc.removeClass(oc.className);
   			oc.addClass('cl2');
		}
 
	}
	if (wid == gwid){
		if (ps==3||ps==5){$('pbh').setStyle('display','none');}else{$('pbh').setStyle('display','');}
		getcolors();
	}
</script>
<script language="javascript" for="ocx" event="DeviceDisconnected(ip,pt)"> 
	loeft();
</script>
 
<script language="javascript" for="ocx" event="Talk(talkState,extendPara)"> 
	if (talkState==1){
		if (gdj==0) cdj($('xdj'),tl('StopTalk'),tl('AudioTalk'));	
	}
	else
	{
		if (gdj==1) cdj($('xdj'),tl('StopTalk'),tl('AudioTalk'));	
	}
	
</script>
 
<script language="javascript" for="ocx" event="DeviceChanged(strDevIp, lPort, lType)"> 
	if (lType == 1)
	{
		rfc();
	}
	if (lType==3)
	{
		lo();
		$('l').setStyle('display','none');
		window.location.href='http://' + strDevIp;
	}	
 
</script>
 
 
</head>
<body>
<div id='l'>
	<div id='lx'>
		<div id='lb'></div>
		<div id='la'>
			<div id='lal'></div>
			<div id='lar'></div>
			<div id='lalogo'></div>
			<div id='lainput'>
				<div><span id='xyhm'>username:</span><input type='text' maxlength='20' id='username' onKeyDown="javascript:if (event.keyCode==13) event.keyCode=9;"></div>
				<div><span id='xmm'>pass:</span><input type="password" maxlength="20" id="password" onKeyDown="javascript:if (event.keyCode==13) ld();"></div>
				<div style="display:none;"><span>IP:</span><input type="text" id="ip" onKeyDown="javascript:if (event.keyCode==13) event.keyCode=9;" value="10.7.4.27"></div>
				<div id='ladvxx' style="display:;"><span id='xdlfs'>type</span><select id="logintype" onKeyDown="javascript:if (event.keyCode==13) ld();"><option value="0">TCP</option><option value="4">UDP</option><option id='xdlfszb' value="3">w_muticast</option></select></div>
			</div>
			<div id='labt'><button id='lbt' onClick="ld()">l</button>
				<div style="position: relative; height:20px;width:88px; float:left; margin:8px 0 0 1px; overflow:hidden;display:none"><a  id='xladv' href="javascript:;" onclick="toggleDisplay($('ladvxx'))">adv</a></div>
			</div>
			
		</div>
		<div id='lc'></div>
	</div>
</div>
 
<div id="m">
	<div id="ma">
		<div id="maa"></div>
		<div id="mab"></div>
		<div id="mac"></div>
		<div id="mad"></div>
		<div id=kwick>
			<ul class=kwicks>
			  <li><span id='xlxcx' class="kwick b" onclick="ocx.ShowPlayback()">recq</span></li>
			  <li><span id='xbjsz' class="kwick c" onclick="ocx.ShowAlarm()">ac</span></li>
			  <li><span id='xxtpz' class="kwick a" onclick="ocx.ShowDeviceConfig()">sysc</span></li>
			  <li><span id='xgy' class="kwick d" onclick="showabout()">about</span></li>
			  <li><span id='xtc' class="kwick e" onclick="lo()">quit</span></li>
			</ul>
		</div>
		<div id="mae"></div>
	</div>
	
	<div id="mb" >
		<div id="mba" style="float:left;padding:0 0 0 5px;">
			<div  class="mpad">
				<div id='dra' class="drabc1">
					<div class="drabc2"></div>
					<div class="drabc3"></div>
					<div class="drabc4"></div>
				</div>
				<div id='mbal' style="height:533px;position:relative; overflow:hidden;">
					<div id='dcl'>
						<ul id='cl'></ul>
						<div id='cmu' title=''></div>
					</div>
					<div id='ddj' style="width:100%; height:100px;">
 
 
						<div style="padding:10px 0 0 4px; float:left"><a id='xac' class="cbt" href="javascript:;" onclick="openall(this,tl('W_ACC'),tl('W_ACO'))">open all</a><a href="javascript:;" class="cbtm" onclick="showmnuopen()"></a></div>
						
						<div style=" padding:10px 0 0 4px; float:left"><a id='xdj' class="cbt" href="javascript:;" onclick="cdj(this,tl('StopTalk'),tl('AudioTalk'))">st</a><a href="javascript:;" class="cbtm" onclick="showmnudj()"></a></div>
						
						<div id="mnudj" style="display:none">
							<ul id='mnudjl'></ul>
						</div>	
 
 
						<div id="mnuopen" style="display:none">
							<ul id='mnuopenl'></ul>
						</div>						
						
						<div style="padding:10px 0 0 4px; float:left"><a id='xhf' style="display:none" class="cbt" href="javascript:;" onclick="ocx.QuickOperation(0)">playback</a></div>
 
						<div style="padding:10px 0 0 4px; float:left"><a id='xsxt' class="cbt" href="javascript:;" onclick="rfc()">refresh</a></div>
						
						
						
						<div style="padding:10px 0 0 4px; float:left"><a id='xkl' style="display:none" class="cbt" href="javascript:;" onclick="ocx.ShowBurning()">kel</a></div>
						<!--
						<div style="margin:5px 0 0 4px;">
							<div style='width:21px; height:21px;background:url(yl.png);float:left' title='volue'></div>
							<div style="margin:4px 0 0 4px;float:left"><a class="sal" href="javascript:;" onclick="gslde.set(gslde.step-1)"></a></div>
							<div id='sae' style="width:70px;height:13px;margin:4px 0 0 4px;float:left;background:url(sas.png) repeat-x">
								<div id='ske' style="width:6px;height:13px;background:url(sks.png) no-repeat; cursor:pointer;"></div>
							</div>
							<div style="margin:4px 0 0 4px; float:left;"><a class="sar" href="javascript:;" onclick="gslde.set(gslde.step+1)"></a></div>
						</div>
						-->
					</div>
					<div style="padding:0 0 0 4px; float:left"><a id='xfmq' style="display:none" class="cbt" href="javascript:;" onclick="closebeep()">closebeep</a></div>
				</div>
			</div>
		</div>
		<div id="mbb" style="width:650px;height:500px;float:left; left:120px;padding:0 0 0 5px;">
			<div id="drb" class="drabc1">
				<div class="drabc2"></div>
				<div class="drabc3"></div>
				<div class="drabc4"></div>
			</div>
			<div id='pla' style="width:100%;height:100%;float:left;padding:0"><script type="text/javascript" src="olp.js"></script></div>			
			<div id='plc' style="width:100%;float:left;height:33px;background:url(plcb.png); ">
				<div style="width:7px;height:33px;float:left; background:url(plcbl.png);background-repeat:no-repeat;background-position:0 0;"></div>
				<div id='pbh' style="background:url(plcb.png);width:580px;height:33px;position:absolute;z-index:10;"></div>
				<div id='pb' style="height:26px;margin:3px 0 0 10px;position:absolute;z-index:9;">
					<div id='sa'>
						<div id='sk'></div>
					</div>
					<div id='pc' style="float:left;">
						<div style="margin:0 0 0 5px"><a id='xp1' class="p1" href="javascript:;" onclick="ocx.PlayVideo(1)" ></a></div>
						<div style="margin:0 0 0 10px"><a id='xp2' class="p2" href="javascript:;" onclick="ocx.PlayVideo(2)"></a></div>
						<div style="margin:0 0 0 10px"><a id='xp3' class="p3" href="javascript:;" onclick="ocx.PlayVideo(3)"></a></div>
						<div style="margin:0 0 0 10px"><a id='xp4' class="p4" href="javascript:;" onclick="ocx.PlayVideo(5)"></a></div>
						<div style="margin:0 0 0 10px"><a id='xp5' class="p5" href="javascript:;" onclick="ocx.PlayVideo(4)"></a></div>
						<!--<div style="margin:0 0 0 10px"><a class="p6" href="javascript:;"></a></div>
						<div style="margin:0 0 0 10px"><a class="p7" href="javascript:;"></a></div>-->
					</div>
				</div>
				<div style="width:7px;height:33px;float:right; background:url(plcbl.png);background-repeat:no-repeat;background-position:-7px 0;"></div>
			</div>
		</div>
		<div id="mbc" style="float:left;padding:0 0 0 5px;">
			<div  class="mpad">
				<div id='drc' class="drabc1">
					<div class="drabc2"></div>
					<div class="drabc3"></div>
					<div class="drabc4"></div>
				</div>
				<div id='yt' style="height:120px;position:relative;padding:0 10px 0 10px;">
					<div style="width:33px;height:26px;padding:14px 0 0 7px;"><a class="y1" href="javascript:;" onmousedown="ocx.ControlPtz(32,5,$('ps').value,0,0)" onmouseup="ocx.ControlPtz(32,5,$('ps').value,0,1)"></a></div>
					<div style="width:33px;height:26px;padding:14px 0 0 7px;"><a class="y2" href="javascript:;" onmousedown="ocx.ControlPtz(0,0,$('ps').value,0,0)" onmouseup="ocx.ControlPtz(0,0,$('ps').value,0,1)"></a></div>
					<div style="width:33px;height:26px;padding:14px 0 0 7px;"><a class="y3" href="javascript:;" onmousedown="ocx.ControlPtz(33,5,$('ps').value,0,0)" onmouseup="ocx.ControlPtz(33,5,$('ps').value,0,1)"></a></div>
					<div style="width:33px;height:33px;padding:7px 0 0 7px;"><a class="y4" href="javascript:;" onmousedown="ocx.ControlPtz(2,0,$('ps').value,0,0)" onmouseup="ocx.ControlPtz(2,0,$('ps').value,0,1)"></a></div>
					<div style="width:40px;height:40px;"><a id='cptz' class="y5" href="javascript:;" onclick="cptz()" ></a></div>
					<div style="width:33px;height:33px;padding:7px 0 0 7px;"><a class="y6" href="javascript:;" onmousedown="ocx.ControlPtz(3,0,$('ps').value,0,0)" onmouseup="ocx.ControlPtz(3,0,$('ps').value,0,1)"></a></div>
					<div style="width:33px;height:40px;padding:0 0 0 7px;"><a class="y7" href="javascript:;" onmousedown="ocx.ControlPtz(34,5,$('ps').value,0,0)" onmouseup="ocx.ControlPtz(34,5,$('ps').value,0,1)"></a></div>
					<div style="width:33px;height:40px;padding:0 0 0 7px;"><a class="y8" href="javascript:;" onmousedown="ocx.ControlPtz(1,0,$('ps').value,0,0)" onmouseup="ocx.ControlPtz(1,0,$('ps').value,0,1)"></a></div>
					<div style="width:33px;height:40px;padding:0 0 0 7px;"><a class="y9" href="javascript:;" onmousedown="ocx.ControlPtz(35,5,$('ps').value,0,0)" onmouseup="ocx.ControlPtz(35,5,$('ps').value,0,1)"></a></div>
				</div>
				<div style="width:132px;height:20px;padding:0 0 0 8px;position:relative;overflow:hidden;">
					<div id='xbc' class="divt" style="float:left;">(1-8):</div>
					<div style="width:53px;height:18px; background:url(ytabg.png); padding:0 0 0 3px;float:left;">
						<input class="inputyt" type="text" id="ps" value="5" maxlength="1" onkeyup="limitPs()" onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d]/g,''))">
						<a href="javascript:;" class="yta1" onclick="showmu1()"></a>
					</div>
				</div>
				<div  id="smu1"  style="display:none">
					<ul>
						<li ><a href='javascript:;' onclick='onmu1(1)'>1</a></li>
						<li ><a href='javascript:;' onclick='onmu1(2)'>2</a></li>
						<li ><a href='javascript:;' onclick='onmu1(3)'>3</a></li>
						<li ><a href='javascript:;' onclick='onmu1(4)'>4</a></li>
						<li ><a href='javascript:;' onclick='onmu1(5)'>5</a></li>
						<li ><a href='javascript:;' onclick='onmu1(6)'>6</a></li>
						<li ><a href='javascript:;' onclick='onmu1(7)'>7</a></li>
						<li ><a href='javascript:;' onclick='onmu1(8)'>8</a></li>
					</ul>
				</div>
				<div id='yt1' style="height:90px;position:relative; margin:0 5px 0 5px;">
					<div style=" padding:0 0 5px 14px;"><a class="y1" href="javascript:;"  onmousedown="ocx.ControlPtz(4,0,$('ps').value,0,0)" onmouseup="ocx.ControlPtz(4,0,$('ps').value,0,1)"></a></div>
						<div id='xbb' style="height:18px;width:50px;padding:8px 0 0 0; text-align:center;">bb</div>
					<div style=" padding:0 0 5px 0;"><a class="y2" href="javascript:;" onmousedown="ocx.ControlPtz(5,0,$('ps').value,0,0)" onmouseup="ocx.ControlPtz(5,0,$('ps').value,0,1)"></a></div>
					<div style=" padding:0 0 5px 14px;"><a class="y1" href="javascript:;" onmousedown="ocx.ControlPtz(6,0,$('ps').value,0,0)" onmouseup="ocx.ControlPtz(6,0,$('ps').value,0,1)"></a></div>
						<div id='xbj' style="height:18px;width:50px;padding:8px 0 0 0; text-align:center;">bj</div>						
					<div style=" padding:0 0 5px 0;"><a class="y2" href="javascript:;" onmousedown="ocx.ControlPtz(7,0,$('ps').value,0,0)" onmouseup="ocx.ControlPtz(7,0,$('ps').value,0,1)"></a></div>
					<div style=" padding:0 0 5px 14px;"><a class="y1" href="javascript:;" onmousedown="ocx.ControlPtz(8,0,$('ps').value,0,0)" onmouseup="ocx.ControlPtz(8,0,$('ps').value,0,1)"></a></div>
						<div id='xgq' style="height:18px;width:50px;padding:8px 0 0 0; text-align:center;">gq</div>						
					<div style=" padding:0 0 5px 0;"><a class="y2" href="javascript:;" onmousedown="ocx.ControlPtz(9,0,$('ps').value,0,0)" onmouseup="ocx.ControlPtz(9,0,$('ps').value,0,1)"></a></div>
				</div>
				<div id='yt2' style="height:100%;position:relative;padding:0 0 0 4px;float:left;">
					<div id='yt21' style=" height:0;overflow:hidden;float:left;">
<div class='cbtd'><div id='xz' class="divt">(0-255):</div><input type="text" class="inputyt" id="pv" value="1" maxlength="3" onkeyup="limitPv()" onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d]/g,''))"></div>
<div class='cbtd'><a id='xyzd' class="cbt" href="javascript:;" onclick="ocx.ControlPtz(10,0,$('pv').value,0,0)">yzd</a></div>
<div class='cbtd'><a id='xdjxh' class="cbt" href="javascript:;" onclick="cxh(this,tl('Stop'),tl('Auto-Tour'))">djxh</a></div>
<div class='cbtd'><a id='xspxz' class="cbt" href="javascript:;" onclick="cxz(this,tl('Stop'),tl('Auto-Pan'))">spxz</a></div>
<div class='cbtd'><a id='xxs' class="cbt" href="javascript:;" onclick="cxs(this,tl('Stop'),tl('Auto-Scan'))">xs</a></div>
<div class='cbtd'><a id='xxuj' class="cbt" href="javascript:;" onclick="cxj(this,tl('Stop'),tl('Pattern'))">xj</a></div>
<div class='cbtd'><a id='xfzk' class="cbt" href="javascript:;" onclick="ocx.ControlPtz(52,$('pv').value,0,0,0)">open</a></div>
<div class='cbtd'><a id='xfzg' class="cbt" href="javascript:;" onclick="ocx.ControlPtz(53,$('pv').value,0,0,0)">close</a></div>
<div class='cbtd'><a id='xytsz' class="cbt" href="javascript:;" onclick="ocx.ShowSetptz()">ptzsetup</a></div>
					</div>
					<div id='yt22' style="width:129px;height:9px;overflow:hidden; float:left;"><a id='ayt22' class="y1" href="javascript:;" ></a></div>
				</div>
			</div>
			
			<div id='taba' style="float:left;margin:3px 0 0 0; width:140px;">
				<div style="float:left"><a id='taba1' class="t1" href="javascript:;" >pic</a></div>
				<div style="float:left"><a id='taba2' class="t2" href="javascript:;" >other</a></div>
			</div>
			<div class="mpad" style="float:left;">
				<div id='yt3' style=" height:287px;position:relative; overflow:hidden; margin:0 0 0 4px;">
					<div id='yt3t1' style="height:100%;">
						<div id='yt3t1a'>
							<div style="margin:5px 0 0 0;">
								<div id='xtld' style='width:21px; height:21px; background:url(tx1.png);background-repeat:no-repeat;background-position:0 0;' title='ld'></div>
								<div style="margin:4px 0 0 4px;"><a class="sal" href="javascript:;" onclick="gslda.set(gca-1)"></a></div>
								<div id='saa' class="sax">
									<div id='ska' class="skx"></div>
								</div>
								<div style="margin:4px 0 0 4px;"><a class="sar" href="javascript:;" onclick="gslda.set(gca+1)"></a></div>
							</div>
							<div style="margin:5px 0 0 0;">
								<div id='xtdbd' style='width:21px; height:21px;background:url(tx1.png);background-repeat:no-repeat;background-position:-21px 0;' title='dbd'></div>
								<div style="margin:4px 0 0 4px;"><a class="sal" href="javascript:;" onclick="gsldb.set(gcb-1)"></a></div>
								<div id='sab' class="sax">
									<div id='skb' class="skx"></div>
								</div>
								<div style="margin:4px 0 0 4px;"><a class="sar" href="javascript:;" onclick="gsldb.set(gcb+1)"></a></div>
							</div>
							<div style="margin:5px 0 0 0;">
								<div id='xtbhd' style='width:21px; height:21px;background:url(tx1.png);background-repeat:no-repeat;background-position:-42px 0;' title='bhd'></div>
								<div style="margin:4px 0 0 4px;"><a class="sal" href="javascript:;" onclick="gsldc.set(gcc-1)"></a></div>
								<div id='sac' class="sax">
									<div id='skc' class="skx"></div>
								</div>
								<div style="margin:4px 0 0 4px;"><a class="sar" href="javascript:;" onclick="gsldc.set(gcc+1)"></a></div>
							</div>
							<div style="margin:5px 0 0 0;">
								<div id='xtsd' style='width:21px; height:21px;background:url(tx1.png);background-repeat:no-repeat;background-position:-63px 0;' title='sd'></div>
								<div style="margin:4px 0 0 4px;"><a class="sal" href="javascript:;" onclick="gsldd.set(gcd-1)"></a></div>
								<div id='sad' class="sax">
									<div id='skd' class="skx"></div>
								</div>
								<div style="margin:4px 0 0 4px;"><a class="sar" href="javascript:;" onclick="gsldd.set(gcd+1)"></a></div>
							</div>
						</div>
						<div style=" padding:5px 0 0 0;"><a id='xcz' class="cbt" href="javascript:;" onclick="txreset(64);" >reset</a></div>
					</div>
					<div id='yt3t2' style="height:100%;display:none;">
						<div class='cbtd'><a id='xztlj' class="cbt" href="javascript:;" onclick="ocx.SetConfigPath(1);" title="">cp</a></div>
						<div class='cbtd'><a id='xlxlj' class="cbt" href="javascript:;" onclick="ocx.SetConfigPath(2);" title="">rp</a></div>
						<div class='cbtd'><a id='xcqsb' class="cbt" href="javascript:;" onclick="reboot();" title="">reboot</a></div>
					</div>
				</div>
			</div>
		</div><!--mbc-->
	</div>
	<div id="mc" ><script type="text/javascript" src="ft.js"></script></div>
</div>
</body>
</html>
