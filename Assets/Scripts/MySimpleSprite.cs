/* SimpleSprite Version 1.0
   
   Copyright (c) 2012 Black Rain Interactive
   All Rights Reserved
   
   MODIFIED
   Arthur Jones, for use in Game Dev I
*/

//#pragma strict

using UnityEngine;
using System.Collections;

public class MySimpleSprite : MonoBehaviour{

	// Setup Animations
	public Texture[] animation0;
	public Texture[] animation1;
	public Texture[] animation2;
	public Texture[] animation3;
	public Texture[] animation4;
	public Texture[] animation5;
	public Texture[] animation6;
	public Texture[] animation7;
	public Texture[] animation8;
	public Texture[] animation9;
	public float animationSpeed = 10f;
	public bool billboard = false;
	
	// Animation To Play
	int animationPlay;
	Camera cameraToLookAt;
	
	void Awake(){
	   cameraToLookAt = Camera.main;
	}
	
	void Update(){
	// Activate Billboard   
	   if (billboard == true){
	        Vector3 v = cameraToLookAt.transform.position - transform.position;
	        v.x = v.z = 0.0f;
	        transform.LookAt(cameraToLookAt.transform.position - v); 
	   }
	}
	
	// Prepare To Play Animation
	public void PrePlay() {
	   StopAllCoroutines();
	}
	
	public IEnumerator PlayAnimation(int animationPlay){
		while (true){
			if (animationPlay == 0){
				// Play Animation0
				int index0 = (int)(Time.time * animationSpeed);
				index0 = index0 % animation0.Length;
				renderer.material.mainTexture = animation0[index0];
			}
			
			if (animationPlay == 1){  
				// Play Animation1
				int index1 = (int)(Time.time * animationSpeed);
				index1 = index1 % animation1.Length;
				renderer.material.mainTexture = animation1[index1];
			}
			
			if (animationPlay == 2){
				// Play Animation2
				int index2 = (int)(Time.time * animationSpeed);
				index2 = index2 % animation2.Length;
				renderer.material.mainTexture = animation2[index2];
			}
			
			if (animationPlay == 3){
				// Play Animation3
				int index3 = (int)(Time.time * animationSpeed);
				index3 = index3 % animation3.Length;
				renderer.material.mainTexture = animation3[index3];
			}
			
			if (animationPlay == 4){
				// Play Animation4
				int index4 = (int)(Time.time * animationSpeed);
				index4 = index4 % animation4.Length;
				renderer.material.mainTexture = animation4[index4];
			}
			
			if (animationPlay == 5){
				// Play Animation5
				int index5 = (int)(Time.time * animationSpeed);
				index5 = index5 % animation5.Length;
				renderer.material.mainTexture = animation5[index5];
			}
			
			if (animationPlay == 6){
				// Play Animation6
				int index6 = (int)(Time.time * animationSpeed);
				index6 = index6 % animation6.Length;
				renderer.material.mainTexture = animation6[index6];
			}
			
			if (animationPlay == 7){
				// Play Animation7
				int index7 = (int)(Time.time * animationSpeed);
				index7 = index7 % animation7.Length;
				renderer.material.mainTexture = animation7[index7];
			}
			
			if (animationPlay == 8){
				// Play Animation8
				int index8 = (int)(Time.time * animationSpeed);
				index8 = index8 % animation8.Length;
				renderer.material.mainTexture = animation8[index8];
			}
			
			if (animationPlay == 9){
				// Play Animation9
				int index9 = (int)(Time.time * animationSpeed);
				index9 = index9 % animation9.Length;
				renderer.material.mainTexture = animation9[index9];
			}
			yield return new WaitForSeconds (0);
		}
		//yield break;
	}
	
	void SetSpeed(float speed){
	   animationSpeed = speed;
	}
}