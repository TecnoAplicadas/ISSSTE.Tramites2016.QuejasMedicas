//300x250
//defines global var for saving objects
var creative = {};
creative.count = 0;
/*
 * DON'T MODIFY!: Window onload handler. 
 */
function preInit() {
	setupDom();
	if (Enabler.isInitialized()) {
		init();
	} else {
		Enabler.addEventListener(studio.events.StudioEvent.INIT, init);
	}
}

/*
 * Initializes the ad components
 */
function setupDom() {
	creative.dom = {};
	creative.dom.mainContainer = document.getElementById('main-container');
	creative.dom.exit = document.getElementById('exit');
	creative.dom.img_2 = document.getElementById('img_2');
	creative.dom.img_3 = document.getElementById('img_3');
	creative.dom.img_4 = document.getElementById('img_4');
	creative.dom.img_5 = document.getElementById('img_5');
	creative.dom.txt_1 = document.getElementById('txt_1');
	creative.dom.txt_2 = document.getElementById('txt_2');
	creative.dom.txt_3 = document.getElementById('txt_3');
	creative.dom.txt_4 = document.getElementById('txt_4');
	creative.dom.txt_5 = document.getElementById('txt_5');
	creative.dom.txt_6 = document.getElementById('txt_6');
	creative.dom.txt_7 = document.getElementById('txt_7');
	creative.dom.cta = document.getElementById('cta');
	creative.dom.shimmer = document.getElementById('shimmer');
	//showing exit
	creative.dom.exit.style.display = "block";
	//defining a main timeline
	creative.dom.tl = new TimelineLite({paused:true});
	creative.dom.tl.add("init", 0);
	//presets
	creative.dom.tl.to(creative.dom.img_2, 0, {opacity: 0}, 0);
	creative.dom.tl.to(creative.dom.img_3, 0, {opacity: 0}, 0);
	creative.dom.tl.to(creative.dom.img_4, 0, {opacity: 0}, 0);
	creative.dom.tl.to(creative.dom.img_5, 0, {opacity: 0}, 0);
	creative.dom.tl.to(creative.dom.txt_1, 0, {opacity: 0}, 0);
	creative.dom.tl.to(creative.dom.txt_2, 0, {opacity: 0}, 0);
	creative.dom.tl.to(creative.dom.txt_3, 0, {opacity: 0}, 0);
	creative.dom.tl.to(creative.dom.txt_4, 0, {opacity: 0}, 0);
	creative.dom.tl.to(creative.dom.txt_5, 0, {opacity: 0}, 0);
	creative.dom.tl.to(creative.dom.txt_6, 0, {opacity: 0}, 0);
	creative.dom.tl.to(creative.dom.txt_7, 0, {opacity: 0}, 0); 
	creative.dom.tl.to(creative.dom.cta, 0, {opacity: 0}, 0);  
	creative.dom.tl.to(creative.dom.shimmer, 0, {opacity: 0, left: '-110%'}, 0);  
	//animation starts
	creative.dom.tl.add("start", 0);
	creative.dom.tl.to(creative.dom.txt_1, 0.45, 
		{opacity: 1, ease: Power1.easeInOut},
		"+=0");
	creative.dom.tl.to(creative.dom.txt_1, 0.45, 
		{opacity: 0, ease: Power1.easeInOut},
		"+=1.2");
	//step 1
	creative.dom.tl.fromTo(creative.dom.img_2, 0.35, 
		{opacity: 0, scale: 0.3},
		{opacity: 1, scale: 1, ease: Power1.easeInOut},
		"+=0");
	creative.dom.tl.fromTo(creative.dom.txt_2, 0.35, 
		{opacity: 0},
		{opacity: 1, ease: Power1.easeInOut},
		"-=0.2");
	creative.dom.tl.to(creative.dom.txt_2, 0.35, 
		{opacity: 0, ease: Power1.easeInOut},
		"+=0.3");
	//step 2
	creative.dom.tl.fromTo(creative.dom.img_3, 0.35, 
		{opacity: 0, scale: 0.3},
		{opacity: 1, scale: 1, ease: Power1.easeInOut},
		"+=0");
	creative.dom.tl.fromTo(creative.dom.txt_3, 0.35, 
		{opacity: 0},
		{opacity: 1, ease: Power1.easeInOut},
		"-=0.2");
	creative.dom.tl.to(creative.dom.txt_3, 0.35, 
		{opacity: 0, ease: Power1.easeInOut},
		"+=0.3");
	//step 3
	creative.dom.tl.fromTo(creative.dom.img_4, 0.35, 
		{opacity: 0, scale: 0.3},
		{opacity: 1, scale: 1, ease: Power1.easeInOut},
		"+=0");
	creative.dom.tl.fromTo(creative.dom.txt_4, 0.35, 
		{opacity: 0},
		{opacity: 1, ease: Power1.easeInOut},
		"-=0.2");
	creative.dom.tl.to(creative.dom.txt_4, 0.35, 
		{opacity: 0, ease: Power1.easeInOut},
		"+=0.3");
	//step 4
	creative.dom.tl.fromTo(creative.dom.img_5, 0.35, 
		{opacity: 0, scale: 0.3},
		{opacity: 1, scale: 1, ease: Power1.easeInOut},
		"+=0");
	creative.dom.tl.fromTo(creative.dom.txt_5, 0.35, 
		{opacity: 0},
		{opacity: 1, ease: Power1.easeInOut},
		"-=0.2");
	creative.dom.tl.to(creative.dom.txt_5, 0.35, 
		{opacity: 0, ease: Power1.easeInOut},
		"+=0.3");
	//step 5
	creative.dom.tl.to(".icon", 0.45, 
		{opacity: 0, ease: Power1.easeInOut},
		"-=0.5");
	creative.dom.tl.fromTo(creative.dom.txt_6, 0.45, 
		{opacity: 0},
		{opacity: 1, ease: Power1.easeInOut},
		"+=0.0");
	creative.dom.tl.to(creative.dom.txt_6, 0.45, 
		{opacity: 0, ease: Power1.easeInOut},
		"+=1.2");
	//step 7
	creative.dom.tl.fromTo(creative.dom.cta, 0.45, 
		{opacity: 0, scale: 0.3},
		{opacity: 1, scale: 1, ease: Power1.easeInOut},
		"+=0");
	creative.dom.tl.to(creative.dom.txt_7, 0.45, 
		{opacity: 1, ease: Power1.easeInOut},
		"+=0.5");
	creative.dom.tl.to(creative.dom.shimmer, 1.5, 
		{opacity: 1, left: '110%', ease: Power2.easeInOut},
		"-=0.7");
	creative.dom.tl.to(creative.dom.shimmer, 1.0, 
		{opacity: 0, ease: Power2.easeInOut, onComplete: restart},
		"+=0.0");
}

/*
 * DON'T MODIFY!: Ad initialisation.
 */
function init() {
	addListeners();
	// Polite loading
	if (Enabler.isPageLoaded()) {
		show();
	}
	else {
		Enabler.addEventListener(studio.events.StudioEvent.PAGE_LOADED, show);
		//Enabler.addEventListener(studio.events.StudioEvent.VISIBLE, show);
	}
}

/*
 * DON'T MODIFY!: Adds appropriate listeners at initialization time
 */
function addListeners() {
	creative.dom.exit.addEventListener('click', exitClickHandler);
}

/*
 *  Shows the ad.
 */
function show() {
	creative.dom.tl.play();	
}
function restart(){
	if(creative.count == 0){
		creative.count = 1;
		creative.dom.tl.restart();
	}
}

// ---------------------------------------------------------------------------------
// MAIN
// ---------------------------------------------------------------------------------

function exitClickHandler() {
	Enabler.exit('BackgroundExit');
}

/*
 *  Main onload handler
 */
window.addEventListener('load', preInit);