using UnityEngine;
using System.Collections;

public class TransitionScript : MonoBehaviour {

	public GUISkin skin;
	public float lerpSpeed;
	public bool blockMoney;

	Color currentColor;
	Color targetColor;

	SpriteRenderer sr;

	Vector3 newCameraPos;

	string displayText;

	float delayTime = 0f;
	public float totalDelay = 2f;

	SideScrollWalkScript walker;

	// Use this for initialization
	void Start () {
		walker = GameObject.FindGameObjectWithTag("WalkHome").GetComponent<SideScrollWalkScript>();
		sr = GetComponent<SpriteRenderer>();
		currentColor = Color.black;
		currentColor.a = 1f;
		targetColor = currentColor;

		sr.color = currentColor;

		Transition (new Vector3(0f, 0f, -10f), "Day 1");
	}
	
	// Update is called once per frame
	void Update () {
		delayTime += Time.deltaTime;
		currentColor = Color.Lerp (currentColor, targetColor, lerpSpeed * Time.deltaTime);
		if(currentColor.a > .1f || targetColor.a == 1f){
			blockMoney = true;
		}
		else{
			blockMoney = false;
		}
		sr.color = currentColor;
		if(Mathf.Abs(currentColor.a - targetColor.a) < .02f && targetColor.a > 0f && delayTime > totalDelay){
			walker.ResetPosition ();
			targetColor.a = 0f;
			if(newCameraPos.z > -999f){
				Camera.main.transform.position = newCameraPos;
			}
		}
	}

	public void Transition(Vector3 pos){
		blockMoney = true;
		newCameraPos = pos;
		targetColor.a = 1f;
		displayText = "";
		delayTime = 2f;
	}

	public void Transition(Vector3 pos, string text){
		blockMoney = true;
		newCameraPos = pos;
		targetColor.a = 1f;
		displayText = text;
		delayTime = 0f;
	}

	public void QuickTransition(Vector3 pos){
		blockMoney = true;
		targetColor.a = 0f;
		currentColor.a = 1f;
		displayText = "";
		delayTime = 0f;
		sr.color = currentColor;
		if(pos.z > -999f){
			Camera.main.transform.position = pos;
		}
	}

	void OnGUI(){
		if(Mathf.Abs(currentColor.a - targetColor.a) < .05f && targetColor.a == 1f && displayText != ""){
			GUI.skin = skin;
			float xGUI = Screen.width * .2f;
			float yGUI = Screen.height * .4f;
			float widthGUI = Screen.width * .6f;
			float heightGUI = Screen.height * .2f;
			int fontSize = (int)(Screen.height/10f);

			GUI.skin.label.normal.textColor = Color.white;
			GUI.skin.label.fontSize = fontSize;
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;

			GUI.Label(new Rect(xGUI, yGUI, widthGUI, heightGUI), displayText);
	}
	}
}
