import {Scroll} from './scroll.js';
import {Main} from './main.js';
import {Loading} from './loading.js';

const req = new XMLHttpRequest();
let vid = document.getElementById('video');

// init loading screen
Loading.init();

req.open('GET', 'media/video.mp4', true);
req.responseType = 'blob';

req.onprogress = function(e) {
	Loading.progress(e.loaded / e.total);
};

req.onload = function() {
	if (this.status === 200) {
		vid.src = window.URL.createObjectURL(this.response);
		vid.load();
		vid.pause();
		Scroll.init();
		Main.init();
		Loading.stop();
	}
};

req.send();