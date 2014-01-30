using UnityEngine;
using System.Collections;

public class InputManagerScript : MonoBehaviour {

	public bool tickUp = false;
	public bool tickDown = false;
	public bool tickLeft = false;
	public bool tickRight = false;

	public float initialTickDelay;
	public float secondaryTickDelay;

	string lastInput = "none";
	float tick = 0f;
	bool onFirstTick = true;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		ResetTick ();
		if(Input.GetButtonDown ("Down")){
			tick = 0f;
			lastInput = "down";
			onFirstTick = true;
			tickDown = true;
		}
		else if(Input.GetButtonDown ("Up")){
			tick = 0f;
			lastInput = "up";
			onFirstTick = true;
			tickUp = true;
		}
		else if(Input.GetButtonDown ("Right")){
			tick = 0f;
			lastInput = "right";
			onFirstTick = true;
			tickRight = true;
		}
		else if(Input.GetButtonDown ("Left")){
			tick = 0f;
			lastInput = "left";
			onFirstTick = true;
			tickLeft = true;
		}
		if((Input.GetButtonUp ("Down") && lastInput == "down")
		   || (Input.GetButtonUp ("Up") && lastInput == "up")
		   || (Input.GetButtonUp ("Right") && lastInput == "right")
		   || (Input.GetButtonUp ("Left") && lastInput == "left")){
			lastInput = "none";
		}
		if(lastInput != "none" && (Input.GetButton ("Down") || Input.GetButton ("Up") || Input.GetButton ("Right") || Input.GetButton ("Left"))){
			tick += Time.deltaTime;
			if(onFirstTick){
				if(tick > initialTickDelay){
					tick = 0f;
					SetTick ();
					onFirstTick = false;
				}
			}
			else{
				if(tick > secondaryTickDelay){
					tick = 0f;
					SetTick ();
				}
			}
		}
	}

	void SetTick(){
		if(lastInput == "down"){
			tickDown = true;
		}
		else if(lastInput == "up"){
			tickUp = true;
		}
		else if(lastInput == "right"){
			tickRight = true;
		}
		else if(lastInput == "left"){
			tickLeft = true;
		}
	}

	void ResetTick(){
		tickDown = false;
		tickUp = false;
		tickLeft = false;
		tickRight = false;
	}
}
