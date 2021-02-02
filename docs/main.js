let scrollbar = document.getElementById('scroll-bar')
let scrollbarA = scrollbar.children[0]
let scrollbarB = scrollbar.children[1]
let scrollDown = document.getElementById('scroll-down');
let vid = document.getElementById('video');
let l = 0

function start() {
	scrollbarA.style.opacity = '1'
	scrollbarB.style.opacity = '0.5'
	scrollbarA.style.height = 100 / (S.pages - 1) + '%'

	vid.addEventListener('loadedmetadata', () => {
		l = vid.duration
		requestAnimationFrame(update)
	})
}

function update() {
	if (S.finalScroll === 0) {
		scrollbar.style.opacity = '0'
		scrollDown.style.opacity = '1'
		vid.style.filter = 'brightness(100%)'
		scrollDown.style.pointerEvents = 'all'
	} else {
		scrollbar.style.opacity = '1'
		scrollDown.style.opacity = '0'
		vid.style.filter = 'brightness(50%)'
		scrollDown.style.pointerEvents = 'none'
	}
	document.body.scrollTop = S.globalScroll * window.innerHeight
	vid.currentTime = S.globalScroll / (S.pages - 1) * l
	scrollbarA.style.top = Math.clamp(((S.globalScroll - 1) / (S.pages - 1)) * 100, 0 , 100) + '%';
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