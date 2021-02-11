let Scroll = {
	pages: 0,
	currentScroll: 0,
	targetScroll: 0,
	prevScroll: 0,
	nextScroll: 1,
	scrolling: false,
	init: init,
	doScroll: doScroll,
	setOnScroll: function(callback) {
		onScroll = callback
	}
}

let content = document.getElementById('content')
let tDeltaY // touch delta Y
let frame // current frame
let frames = 30
let time = 1000 // in ms
let deltaTime = time / frames
let onScroll

function init() {
	Scroll.pages = content.children.length

	// register events
	document.addEventListener('touchstart', e => {
		tDeltaY = e.changedTouches[0].clientY;
	}, { passive: false })

	document.addEventListener('touchmove', e => {
		e.preventDefault()
		let d = tDeltaY - e.changedTouches[0].clientY;
		if (Math.abs(d) > 20) {
			doScroll(d)
			tDeltaY = e.changedTouches[0].clientY;
		}
	}, { passive: false })

	document.body.addEventListener('wheel', e => {
		e.preventDefault();
		if (Math.abs(e.deltaY * (e.deltaMode === 1 ? 17 : 1)) > 10) {
			doScroll(e.deltaY)
		}
	}, { passive: false })
}

function doScroll(d) {
	d = Math.sign(d)
	if (!Scroll.scrolling && scrollIsValid(d)) {
		Scroll.scrolling = true
		Scroll.targetScroll = Scroll.currentScroll + d
		Scroll.prevScroll = Math.max(Scroll.targetScroll - 1, 0)
		Scroll.nextScroll = Math.min(Scroll.targetScroll + 1, Scroll.pages - 1)
		frame = 0
		onScroll()
		let id = setInterval(scrollUpdate, deltaTime, d, () => {
			clearInterval(id)
			Scroll.scrolling = false
			Scroll.currentScroll = Math.round(Scroll.currentScroll * 100) / 100
		})
	}

	function scrollIsValid(d) {
		return Scroll.currentScroll + d >= 0 && Scroll.currentScroll + d <= Scroll.pages - 1
	}
}

function scrollUpdate(d, callback) {
	frame++
	if (frame === frames) callback()
	let x = -(Math.cos(Math.PI * (frame / frames)) - 1) / 2
	Scroll.currentScroll = (Scroll.targetScroll - d) + d * x
}

export { Scroll }