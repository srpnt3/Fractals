let S = {
	pages: 5,
	globalScroll: 0,
	finalScroll: 0,
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
	document.getElementById('scroll-down').addEventListener('click', () => scroll(1))

	document.addEventListener('touchstart', e => {
		S.t.deltaY = e.changedTouches[0].clientY;
	}, {passive: false})

	document.addEventListener('touchmove', e => {
		e.preventDefault()
		let d = S.t.deltaY - e.changedTouches[0].clientY;
		if (Math.abs(d) > 20) {
			scroll(Math.sign(d))
			S.t.deltaY = e.changedTouches[0].clientY;
		}
	}, {passive: false})

	document.body.addEventListener('wheel', e => {
		e.preventDefault();
		if (Math.abs(e.deltaY * (e.deltaMode === 1 ? 17 : 1)) > 10) {
			scroll(Math.sign(e.deltaY))
		}
	}, {passive: false})
}

function scroll(deltaY) {
	if (!S.scrolling && isScrollValid(deltaY)) {
		S.scrolling = true
		S.finalScroll = S.globalScroll + deltaY
		S.a.currentTime = 0
		S.a.startValue = S.globalScroll
		let scrollID = setInterval(scrollUpdate, S.a.deltaTime(), deltaY, () => {
			clearInterval(scrollID)
			S.scrolling = false
		})
	}
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