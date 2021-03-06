import {Scroll} from './scroll.js';
import {Cursor} from './cursor.js';

let Main = {
	init: init
};

// vars
let content = document.getElementById('content');
let scrollBar = document.getElementById('scroll-bar');
let scrollValue = document.getElementById('scroll-value');
let scrollName = document.getElementById('scroll-name');
let scrollDown = document.getElementById('scroll-down');
let icon1 = document.getElementById('icon1');
let icon2 = document.getElementById('icon2');
let matcher = window.matchMedia('(prefers-color-scheme: dark)');
let vid = document.getElementById('video');
let vidLength;

// fix mobile problems
let updateVH = () => document.documentElement.style.setProperty('--vh', window.innerHeight * 0.01 + 'px');
window.addEventListener('resize', updateVH);
setInterval(updateVH, 100);
updateVH();

// init cursor
Cursor.init();

// icon
matcher.onchange = iconUpdate;
iconUpdate();

function iconUpdate() {
	if (matcher.matches) {
		document.head.append(icon1);
		icon2.remove();
	} else {
		document.head.append(icon2);
		icon1.remove();
	}
}

// tilt
if (!('ontouchstart' in window || navigator.maxTouchPoints > 0 || navigator.msMaxTouchPoints > 0))
	VanillaTilt.init(vid, {max: 1, scale: 1.05, speed: 1000});

// init
function init() {
	Scroll.setOnScroll(onScroll); // register event

	scrollDown.addEventListener('click', () => {
		Scroll.doScroll(1);
	});

	for (let i = 0; i < Scroll.pages; i++) {
		scrollBar.appendChild(document.createElement('line'));
	}
	vid.addEventListener('loadedmetadata', () => {
		vidLength = vid.duration;
		requestAnimationFrame(update);
	});
}

// update
function update() {

	// scroll bar & scroll value
	scrollBar.children[Scroll.prevScroll].className = '';
	scrollBar.children[Scroll.nextScroll].className = '';
	scrollBar.children[Scroll.targetScroll].className = 'active';
	scrollValue.innerText = Scroll.currentScroll.toFixed(1);
	scrollName.innerText = content.children[Scroll.targetScroll].dataset.name;

	// video & update
	vid.currentTime = Scroll.currentScroll / (Scroll.pages - 1) * vidLength;
	requestAnimationFrame(update);
}

// scroll event function
function onScroll() {
	content.children[Scroll.prevScroll].className = 'prev';
	content.children[Scroll.nextScroll].className = 'next';
	setTimeout(() => {
		content.children[Scroll.targetScroll].className = 'curr';
	}, Scroll.time / 2);

	// landing page / other pages
	/*if (Scroll.targetScroll === 0)
		vid.style.filter = 'brightness(90%)';
	else
		vid.style.filter = 'brightness(75%)';*/

	if (Scroll.targetScroll === Scroll.pages - 1) {
		scrollDown.style.opacity = '0';
		scrollDown.style.pointerEvents = 'none';
	} else {
		setTimeout(() => scrollDown.style.opacity = '1', Scroll.time / 2);
		scrollDown.style.pointerEvents = 'all';
	}
}

Math.clamp = function(a, b, c) {
	return Math.max(b, Math.min(c, a));
};

export {Main};