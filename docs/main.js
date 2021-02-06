let scrollBar = document.getElementById('scroll-bar')
let scrollValue = document.getElementById('scroll-value')
let scrollDown = document.getElementById('scroll-down')
let vid = document.getElementById('video')
let l = 0

function start() {
	for (let i = 0; i < S.pages; i++) {
		scrollBar.appendChild(document.createElement('line'))
	}

	vid.addEventListener('loadedmetadata', () => {
		l = vid.duration
		requestAnimationFrame(update)
	})
}

function update() {

	// scroll bar & scroll value
	scrollBar.children[Math.max(S.finalScroll - 1, 0)].className = ''
	scrollBar.children[Math.min(S.finalScroll + 1, S.pages - 1)].className = ''
	scrollBar.children[S.finalScroll].className = 'active'
	scrollValue.innerText = S.globalScroll.toFixed(1)

	// landing page / other pages
	if (S.finalScroll === 0) {
		scrollDown.style.opacity = '1'
		vid.style.filter = 'brightness(100%)'
		scrollDown.style.pointerEvents = 'all'
	} else {
		scrollDown.style.opacity = '0'
		vid.style.filter = 'brightness(80%)'
		scrollDown.style.pointerEvents = 'none'
	}

	// if I decide to enable the scroll bar
	//document.body.scrollTop = S.globalScroll * window.innerHeight

	// video & update
	vid.currentTime = S.globalScroll / (S.pages - 1) * l
	requestAnimationFrame(update)
}

Math.clamp = function(a, b, c) {
	return Math.max(b, Math.min(c, a))
}

let icon1 = document.getElementById('icon1')
let icon2 = document.getElementById('icon2')

matcher = window.matchMedia('(prefers-color-scheme: dark)');
matcher.onchange = iconUpdate
iconUpdate()

function iconUpdate() {
	if (matcher.matches) {
		document.head.append(icon1)
		icon2.remove()
	} else {
		document.head.append(icon2)
		icon1.remove()
	}
}