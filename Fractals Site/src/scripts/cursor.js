let Cursor = {
	init: init
};

let cursor;
let mouse = {
	x: 0,
	y: 0
};

function init() {

	// create element & hide cursor
	document.body.classList.add('no-cursor');
	cursor = document.createElement('div');
	cursor.id = 'cursor';
	cursor.style.visibility = 'hidden';
	document.body.append(cursor);

	// mouse events
	document.addEventListener('mouseover', e => {
		noTouch(e);
		cursor.className = '';
		let c = window.getComputedStyle(e.target).getPropertyValue('--c').replaceAll(' ', '');
		if (c !== 'default') cursor.className = 'cursor-' + c;
	});
	document.addEventListener('mousemove', e => noTouch(e));
	document.body.addEventListener('mouseenter', e => noTouch(e));
	document.body.addEventListener('mouseleave', () => cursor.style.visibility = 'hidden');

	// touch events
	document.addEventListener('touchmove', () => touch());
	document.addEventListener('touchstart', () => touch());

	requestAnimationFrame(update);
}

function touch() {
	document.body.classList.remove('no-cursor');
	cursor.style.visibility = 'hidden';
}

function noTouch(e) {
	mouse = {x: e.clientX, y: e.clientY};
	if (!document.body.classList.contains('no-cursor'))
		document.body.classList.add('no-cursor');
	cursor.style.visibility = 'visible';
}

function update() {
	cursor.style.top = mouse.y + 'px';
	cursor.style.left = mouse.x + 'px';
	requestAnimationFrame(update);
}

export {Cursor};