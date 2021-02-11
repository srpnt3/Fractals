let Loading = {
	init: init,
	progress: progress,
	stop: stop,
}

let loading = document.getElementById('loading')
let loadingLine1 = loading.children[0]
let loadingLine2 = loading.children[1]
let value = 0

function init() {
	loadingLine1.style.width = '0'
	loadingLine1.style.opacity = '1'
	loadingLine2.style.opacity = '0.5'
	requestAnimationFrame(draw)
}

function draw() {
	loadingLine1.style.width = value * 50 + '%'
	if (value !== -1) requestAnimationFrame(draw)
}

function progress(x) {
	value = x
}

function stop() {
	value = -1
	loadingLine1.style.opacity = '0'
	loadingLine2.style.opacity = '0'
	setTimeout(() => {
		loading.style.opacity = '0'
		loading.style.pointerEvents = 'none'
	}, 500)
}

export { Loading }