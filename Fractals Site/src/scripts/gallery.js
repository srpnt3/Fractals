import {Scroll} from './scroll.js';
import {Cursor} from './cursor.js';

// vars
let scrollValue = document.getElementById('scroll-value');
let scrollName = document.getElementById('scroll-name');
let scrollLeft = document.getElementById('scroll-left');
let scrollRight = document.getElementById('scroll-right');
let gallery = document.getElementById('gallery');
let icon1 = document.getElementById('icon1');
let icon2 = document.getElementById('icon2');
let matcher = window.matchMedia('(prefers-color-scheme: dark)');
let type;

let types = [
	{
		name: 'images',
		amount: 19, // change this!!
		file: '.png',
		tag: 'img'
	}, {
		name: 'videos',
		amount: 10, // change this!!
		file: '.mp4',
		tag: 'video'
	}
];

// fix mobile problems
let updateVH = () => document.documentElement.style.setProperty('--vh', window.innerHeight * 0.01 + 'px');
window.addEventListener('resize', updateVH);
setInterval(updateVH, 100);
updateVH();

// init
init();

// tilt
if (!('ontouchstart' in window || navigator.maxTouchPoints > 0 || navigator.msMaxTouchPoints > 0))
	VanillaTilt.init(gallery, {max: 1, scale: 1.05, speed: 1000});

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

function init() {
	evalHash();
	loadMedia();

	document.getElementById('info').children[1].innerHTML = types[type].name;
	Scroll.pages = gallery.children[0].children.length;
	Scroll.touchMode = 'x';
	Scroll.time = 300;
	Scroll.setOnScroll(onScroll);
	Cursor.init();
	Scroll.init();

	scrollLeft.addEventListener('click', () => Scroll.doScroll(-1));
	scrollRight.addEventListener('click', () => Scroll.doScroll(1));

	requestAnimationFrame(update);
}

function update() {
	scrollValue.innerText = Scroll.currentScroll.toFixed(1);
	scrollName.innerText = gallery.children[0].children[Scroll.targetScroll].children[0].src.replace(/^.*[\\\/]/, '');
	gallery.scrollLeft = gallery.scrollWidth * Scroll.currentScroll / Scroll.pages;
	if (type === 1 && Scroll.currentScroll === Scroll.targetScroll) updateAutoplay();
	requestAnimationFrame(update);
}

function onScroll() {
	gallery.children[0].children[Scroll.prevScroll].className = 'prev';
	gallery.children[0].children[Scroll.nextScroll].className = 'next';
	gallery.children[0].children[Scroll.targetScroll].className = 'curr';

	if (Scroll.targetScroll === 0) {
		scrollLeft.style.opacity = '0';
		scrollLeft.style.pointerEvents = 'none';
	} else if (Scroll.targetScroll === Scroll.pages - 1) {
		scrollRight.style.opacity = '0';
		scrollRight.style.pointerEvents = 'none';
	} else {
		setTimeout(() => {
			scrollLeft.style.opacity = '1'
			scrollRight.style.opacity = '1'
		}, Scroll.time / 2);
		scrollLeft.style.pointerEvents = 'all';
		scrollRight.style.pointerEvents = 'all';
	}
}

function evalHash() {
	switch (window.location.hash) {
		case '#images':
			type = 0;
			break;
		case '#videos':
			type = 1;
			break;
		default:
			window.location = '#images';
			window.location.reload();
			break;
	}
}

function loadMedia() {
	let path = './media/gallery/' + types[type].name + '/';
	for (let i = 0; i < types[type].amount; i++) {
		let div = document.createElement('div');
		let element = document.createElement(types[type].tag);
		element.src = path + i + types[type].file;
		div.appendChild(element);
		gallery.children[0].appendChild(div);
		if (type === 1) {
			element.muted = true;
			element.defaultMuted = true;
			element.loop = true;
			element.playsInline = true;
			element.controls = false;
		}
	}
}

function updateAutoplay() {
	let elements = document.getElementById('gallery').children[0].children;
	for (let i = 0; i < elements.length; i++) {
		let e = elements[i].children[0];
		if (i === Scroll.targetScroll)
			e.play();
		else {
			e.pause();
			e.currentTime = 0;
		}
	}
}