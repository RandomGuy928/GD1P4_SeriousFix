/* SimpleSprite Version 1.0
   
   Copyright (c) 2012 Black Rain Interactive
   All Rights Reserved
*/

#pragma strict

// Setup Animations
var animation0 : Texture[];
var animation1 : Texture[];
var animation2 : Texture[];
var animation3 : Texture[];
var animation4 : Texture[];
var animation5 : Texture[];
var animation6 : Texture[];
var animation7 : Texture[];
var animation8 : Texture[];
var animation9 : Texture[];
var animationSpeed : float = 10;
var billboard : boolean = false;

// Animation To Play
private var animationPlay : int;
private var cameraToLookAt : Camera;

function Awake(){
   cameraToLookAt = Camera.main;
}

function Update(){
// Activate Billboard   
   if (billboard == true){
        var v : Vector3 = cameraToLookAt.transform.position - transform.position;
        v.x = v.z = 0.0;
        transform.LookAt(cameraToLookAt.transform.position - v); 
   }
}

// Prepare To Play Animation
function PrePlay() {
   
   StopAllCoroutines();
   
}

function PlayAnimation(animationPlay : int){

while (true){
   
if (animationPlay == 0){
   // Play Animation0
   var index0 : int = Time.time * animationSpeed;
   index0 = index0 % animation0.length;
   renderer.material.mainTexture = animation0[index0];
   }
 
if (animationPlay == 1){  
   // Play Animation1
   var index1 : int = Time.time * animationSpeed;
   index1 = index1 % animation1.length;
   renderer.material.mainTexture = animation1[index1];
   }
   
if (animationPlay == 2){
   // Play Animation2
   var index2 : int = Time.time * animationSpeed;
   index2 = index2 % animation2.length;
   renderer.material.mainTexture = animation2[index2];
   }
   
if (animationPlay == 3){
   // Play Animation3
   var index3 : int = Time.time * animationSpeed;
   index3 = index3 % animation3.length;
   renderer.material.mainTexture = animation3[index3];
   }
   
if (animationPlay == 4){
   // Play Animation4
   var index4 : int = Time.time * animationSpeed;
   index4 = index4 % animation4.length;
   renderer.material.mainTexture = animation4[index4];
   }
   
if (animationPlay == 5){
   // Play Animation5
   var index5 : int = Time.time * animationSpeed;
   index5 = index5 % animation5.length;
   renderer.material.mainTexture = animation5[index5];
   }
   
if (animationPlay == 6){
   // Play Animation6
   var index6 : int = Time.time * animationSpeed;
   index6 = index6 % animation6.length;
   renderer.material.mainTexture = animation6[index6];
   }
   
if (animationPlay == 7){
   // Play Animation7
   var index7 : int = Time.time * animationSpeed;
   index7 = index7 % animation7.length;
   renderer.material.mainTexture = animation7[index7];
   }
   
if (animationPlay == 8){
   // Play Animation8
   var index8 : int = Time.time * animationSpeed;
   index8 = index8 % animation8.length;
   renderer.material.mainTexture = animation8[index8];
   }
   
if (animationPlay == 9){
   // Play Animation9
   var index9 : int = Time.time * animationSpeed;
   index9 = index9 % animation9.length;
   renderer.material.mainTexture = animation9[index9];
   }
      yield WaitForSeconds (0);
 }
   yield;
}

function SetSpeed(speed : float){
   animationSpeed = speed;
}