let content = document.getElementById('content')
let S = {
	pages: 5,
	globalScroll: 0,
	finalScroll: 0,
	prev: 0,
	next: 1,
	scrolling: false,
	t: {
		deltaY: 0
	},
	a : {
		increment: 1,
		currentTime: 0,
		startValue: 0,
		time: 1000,
		fps: 30,
		deltaTime: function() {
			return 1000 / S.a.fps
		}
	}
}

function scrollStart() {
	S.pages = content.children.length

	document.addEventListener('touchstart', e => {
		S.t.deltaY = e.changedTouches[0].clientY;
	}, {passive: false})

	document.addEventListener('touchmove', e => {
		e.preventDefault()
		let d = S.t.deltaY - e.changedTouches[0].clientY;
		if (Math.abs(d) > 20) {
			doScroll(Math.sign(d))
			S.t.deltaY = e.changedTouches[0].clientY;
		}
	}, {passive: false})

	document.body.addEventListener('wheel', e => {
		e.preventDefault();
		if (Math.abs(e.deltaY * (e.deltaMode === 1 ? 17 : 1)) > 10) {
			doScroll(Math.sign(e.deltaY))
		}
	}, {passive: false})
}

function doScroll(deltaY) {
	if (!S.scrolling && isScrollValid(deltaY)) {
		S.scrolling = true
		S.finalScroll = S.globalScroll + deltaY
		S.prev = Math.max(S.finalScroll - 1, 0)
		S.next = Math.min(S.finalScroll + 1, S.pages - 1)
		S.a.currentTime = 0
		S.a.startValue = S.globalScroll
		onScroll()
		let scrollID = setInterval(scrollUpdate, S.a.deltaTime(), deltaY, () => {
			clearInterval(scrollID)
			S.scrolling = false
		})
	}
}

function onScroll() {
	content.children[S.prev].className = 'prev'
	content.children[S.next].className = 'next'
	setTimeout(() => {
		content.children[S.finalScroll].className = 'curr'
	}, 500)
}

function scrollUpdate(dir, callback) {
	let x = -(Math.cos(Math.PI * (S.a.currentTime / S.a.time)) - 1) / 2;
	S.globalScroll = S.a.startValue + dir * S.a.increment * x;
	S.globalScroll = Math.round(S.globalScroll * 100) / 100;
	S.a.currentTime += S.a.deltaTime();
	if (Math.abs(S.a.currentTime - S.a.time) < 0.0001) callback()
}

function isScrollValid(deltaY) {
	return S.globalScroll + deltaY >= 0 && S.globalScroll + deltaY <= S.pages - 1
}