let tDelta,frame,onScroll,Scroll={pages:0,time:800,currentScroll:0,targetScroll:0,prevScroll:0,nextScroll:1,scrolling:!1,touchMode:"y",init:init,doScroll:doScroll,setOnScroll:function(l){onScroll=l}},frames=30,deltaTime=Scroll.time/frames;function init(){document.documentElement.style.setProperty("--scroll",Scroll.time+"ms"),"x"!==Scroll.touchMode&&"y"!==Scroll.touchMode&&(Scroll.touchMode="y"),document.addEventListener("touchstart",l=>{tDelta="y"===Scroll.touchMode?l.changedTouches[0].clientY:l.changedTouches[0].clientX},{passive:!1}),document.addEventListener("touchmove",l=>{l.preventDefault();let e=tDelta-("y"===Scroll.touchMode?l.changedTouches[0].clientY:l.changedTouches[0].clientX);Math.abs(e)>20&&(doScroll(e),tDelta="y"===Scroll.touchMode?l.changedTouches[0].clientY:l.changedTouches[0].clientX)},{passive:!1}),document.body.addEventListener("wheel",l=>{l.preventDefault(),Math.abs(l.deltaY*(1===l.deltaMode?17:1))>5&&doScroll(l.deltaY)},{passive:!1})}function doScroll(l){if(l=Math.sign(l),!Scroll.scrolling&&function(l){return Scroll.currentScroll+l>=0&&Scroll.currentScroll+l<=Scroll.pages-1}(l)){Scroll.scrolling=!0,Scroll.targetScroll=Scroll.currentScroll+l,Scroll.prevScroll=Math.max(Scroll.targetScroll-1,0),Scroll.nextScroll=Math.min(Scroll.targetScroll+1,Scroll.pages-1),frame=0,onScroll();let e=setInterval(scrollUpdate,deltaTime,l,()=>{clearInterval(e),Scroll.scrolling=!1,Scroll.currentScroll=Math.round(100*Scroll.currentScroll)/100})}}function scrollUpdate(l,e){++frame===frames&&e();let o=-(Math.cos(Math.PI*(frame/frames))-1)/2;Scroll.currentScroll=Scroll.targetScroll-l+l*o}export{Scroll};