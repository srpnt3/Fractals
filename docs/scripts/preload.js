import{Scroll}from"./scroll.js";import{Main}from"./main.js";import{Loading}from"./loading.js";const req=new XMLHttpRequest;let vid=document.getElementById("video"),content=document.getElementById("content");Loading.init(),req.open("GET","./media/video.mp4",!0),req.responseType="blob",req.onprogress=function(e){Loading.progress(e.loaded/e.total)},req.onload=function(){200===this.status&&(vid.src=window.URL.createObjectURL(this.response),vid.load(),vid.muted=!0,vid.defaultMuted=!0,vid.playsInline=!0,vid.controls=!1,vid.play(),vid.pause(),Scroll.pages=content.children.length,Scroll.init(),Main.init(),Loading.stop())},req.send();