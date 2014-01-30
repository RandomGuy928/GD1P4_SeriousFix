using UnityEngine;
using System.Collections;

public class BedScript : MonoBehaviour {

	public GUISkin skin;

	bool displayText;
	bool validInput;
	Vector3 textPos;
	
	Color targetTextColor;
	Color textColor;

	EventManagerScript eventManager;
	SideScrollWalkScript walker;

	float timer = 0f;
	float maxTime = 5f;

	// Use this for initialization
	void Start () {
		walker = GameObject.FindGameObjectWithTag("WalkHome").GetComponent<SideScrollWalkScript>();
		eventManager = GameObject.FindGameObjectWithTag ("EventManager").GetComponent<EventManagerScript>();
		textPos = transform.GetChild (0).transform.position;
		targetTextColor = Color.black;
		targetTextColor.a = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		timer+=Time.deltaTime;
		if(timer > maxTime && validInput && Input.GetButtonDown ("Select")){
			timer = 0f;
			walker.StopWalking ();
			eventManager.StopWalking();
			validInput = false;
		}
		textColor = Color.Lerp (textColor, targetTextColor, 2f * Time.deltaTime);
		if(textColor.a == 0f){
			displayText = false;
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.GetComponent<SideScrollWalkScript>() != null){
			displayText = true;
			validInput = true;
			targetTextColor.a = 1f;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if(other.GetComponent<SideScrollWalkScript>() != null){
			targetTextColor.a = 0f;
			validInput = false;
		}
	}

	void OnGUI(){
		if(displayText){
			Vector3 screenPos = Camera.main.WorldToScreenPoint(textPos);
			float xGUI = screenPos.x;
			float yGUI = Screen.height - screenPos.y;
			float widthGUI = Screen.width * .5f;
			float heightGUI = Screen.height * .2f;
			int textSize = (int)(Screen.height/20f);

			GUI.skin = skin;
			GUI.skin.label.fontSize = textSize;
			GUI.skin.label.normal.textColor = textColor;

			GUI.Label (new Rect(xGUI, yGUI, widthGUI, heightGUI), "Press confirm to sleep...");
		}
	}
}
