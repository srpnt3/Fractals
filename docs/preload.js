vid = document.getElementById('video')
let loading = document.getElementById('loading')
let loadingA = loading.children[0];
let loadingB = loading.children[1];
let loadingC = loading.children[2];
const req = new XMLHttpRequest()

req.open('GET', 'files/video.mp4', true)
req.responseType = 'blob'

req.onload = function() {
	if (this.status === 200) {
		vid.src = window.URL.createObjectURL(this.response)
		vid.load()
		vid.pause()
		hideLoading();
		scrollStart()
		start()
	}
}

req.onprogress = function(e) {
	loadingA.style.opacity = '1'
	loadingB.style.opacity = '0.5'
	loadingC.style.opacity = '1'
	loadingA.style.width = (e.loaded / e.total) * 50 + '%'
}

req.send()

function hideLoading() {
	loadingA.style.opacity = '0'
	loadingB.style.opacity = '0'
	loadingC.style.opacity = '0'
	setTimeout(() => {
		loading.style.opacity = '0'
		loading.style.pointerEvents = 'none'
	}, 800)
}