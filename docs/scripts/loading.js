let Loading={init:init,progress:progress,stop:stop},loading=document.getElementById("loading"),loadingLine1=loading.children[0],loadingLine2=loading.children[1],value=0;function init(){loadingLine1.style.width="0",loadingLine1.style.opacity="1",loadingLine2.style.opacity="0.5",requestAnimationFrame(draw)}function draw(){loadingLine1.style.width=50*value+"%",-1!==value&&requestAnimationFrame(draw)}function progress(i){value=i}function stop(){value=-1,loadingLine1.style.opacity="0",loadingLine2.style.opacity="0",setTimeout(()=>{loading.style.opacity="0",loading.style.pointerEvents="none"},500)}export{Loading};