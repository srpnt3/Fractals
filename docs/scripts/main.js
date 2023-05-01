import{Scroll}from"./scroll.js";import{Cursor}from"./cursor.js";let vidLength,Main={init:init},content=document.getElementById("content"),scrollBar=document.getElementById("scroll-bar"),scrollValue=document.getElementById("scroll-value"),scrollName=document.getElementById("scroll-name"),scrollDown=document.getElementById("scroll-down"),icon1=document.getElementById("icon1"),icon2=document.getElementById("icon2"),matcher=window.matchMedia("(prefers-color-scheme: dark)"),vid=document.getElementById("video"),updateVH=()=>document.documentElement.style.setProperty("--vh",.01*window.innerHeight+"px");function iconUpdate(){matcher.matches?(document.head.append(icon1),icon2.remove()):(document.head.append(icon2),icon1.remove())}function init(){Scroll.setOnScroll(onScroll),scrollDown.addEventListener("click",()=>{Scroll.doScroll(1)});for(let e=0;e<Scroll.pages;e++)scrollBar.appendChild(document.createElement("line"));vid.addEventListener("loadedmetadata",()=>{vidLength=vid.duration,requestAnimationFrame(update)})}function update(){scrollBar.children[Scroll.prevScroll].className="",scrollBar.children[Scroll.nextScroll].className="",scrollBar.children[Scroll.targetScroll].className="active",scrollValue.innerText=Scroll.currentScroll.toFixed(1),scrollName.innerText=content.children[Scroll.targetScroll].dataset.name,vid.currentTime=Scroll.currentScroll/(Scroll.pages-1)*vidLength,requestAnimationFrame(update)}function onScroll(){content.children[Scroll.prevScroll].className="prev",content.children[Scroll.nextScroll].className="next",setTimeout(()=>{content.children[Scroll.targetScroll].className="curr"},Scroll.time/2),Scroll.targetScroll===Scroll.pages-1?(scrollDown.style.opacity="0",scrollDown.style.pointerEvents="none"):(setTimeout(()=>scrollDown.style.opacity="1",Scroll.time/2),scrollDown.style.pointerEvents="all")}window.addEventListener("resize",updateVH),setInterval(updateVH,100),updateVH(),Cursor.init(),matcher.onchange=iconUpdate,iconUpdate(),"ontouchstart"in window||navigator.maxTouchPoints>0||navigator.msMaxTouchPoints>0||VanillaTilt.init(vid,{max:1,scale:1.05,speed:1e3}),Math.clamp=function(e,l,t){return Math.max(l,Math.min(t,e))};export{Main};