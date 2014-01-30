using UnityEngine;
using System.Collections;

public class BarterScript : MonoBehaviour {

	public GUISkin skin;


	float enterDelay = 0f;
	float enterDelayTime = .1f;
	GameObject barterItem;
	bool bartering = false;
	int currentPosition = 0;
	float value;
	float startValue;
	InputManagerScript input;
	string returnToString;
	GameObject returnToObject;

	// Use this for initialization
	void Start () {
		input = GetComponent<InputManagerScript>();
	}
	
	// Update is called once per frame
	void Update () {
		if(bartering){
			enterDelay+=Time.deltaTime;
			if(Input.GetButtonDown ("Select") && enterDelay > enterDelayTime){
				ReturnPrice (value);
			}
			if(input.tickUp){
				value = Mathf.Round (100f*Mathf.Min (99999f, value + Mathf.Pow (10f, currentPosition)))/100f;
			}
			if(input.tickDown){
				value = Mathf.Round (100f*Mathf.Max (0f, value - Mathf.Pow (10f, currentPosition)))/100f;
			}
			if(input.tickRight){
				currentPosition = currentPosition - 1;
				if(currentPosition == -1){
					currentPosition = 4;
				}
			}
			if(input.tickLeft){
				currentPosition = currentPosition + 1;
				if(currentPosition == 5){
					currentPosition = 0;
				}
			}

		}
	}

	public void StartBartering(float startPrice, string returnString, GameObject returnObject, GameObject itemToBarter){
		barterItem = itemToBarter;
		value = Mathf.Round (startPrice * 100f);
		startValue = value;
		bartering = true;
		returnToString = returnString;
		returnToObject = returnObject;
		enterDelay = 0f;
		barterItem.GetComponent<ItemScript>().HighlightPosition();
	}

	void ReturnPrice(float returnPrice){
		barterItem.GetComponent<ItemScript>().DehighlightPosition();
		bartering = false;
		if(returnToString == "conversation"){
			returnToObject.GetComponent<ConversationManagerScript>().ReceivePrice (returnPrice/100f);
		}

	}

	string GetDigit(float v, int position){

		float temp = Mathf.Pow (10f, -position) * v;
		temp = Mathf.Floor (temp);
		return (Mathf.RoundToInt (temp)%10).ToString ();
	}

	void OnGUI(){
		if(!bartering)
			return;

		GUI.skin = skin;

		float xGUI = Screen.width * .35f;
		float widthGUI = Screen.width * .3f;
		float heightGUI = Screen.height * .11f;
		float yGUI = Screen.height * .6f;
		int fontSize = (int)(Screen.height/15f);
		float textOffset = 4f;
		float iconWidth = Screen.height * .075f;

		float x2GUI = Screen.width * .4f;
		float width2GUI = Screen.width * .2f;
		float height2GUI = Screen.height * .07f;
		float y2GUI = Screen.height * .53f;
		int fontSize2 = (int)(Screen.height/25f);

		float x3GUI = Screen.width * .125f;
		float width3GUI = Screen.width * .75f;
		float height3GUI = Screen.height * .1f;
		float y3GUI = Screen.height * .3f;
		int fontSize3 = (int)(Screen.height/20f);

		GUI.Box (new Rect(xGUI, yGUI, widthGUI, heightGUI),"");
		GUI.skin.label.fontSize = fontSize3;
		Color textColor = GUI.skin.label.normal.textColor;
		Color selectTextColor = GUI.skin.label.hover.textColor;

		//Draw item info
		GUI.Box (new Rect(x3GUI, y3GUI,width3GUI,height3GUI),"");
		ItemScript item = barterItem.GetComponent<ItemScript>();
		GUI.skin.label.normal.background = item.image;
		GUI.Label (new Rect(x3GUI+textOffset,y3GUI+textOffset,iconWidth, iconWidth),"");
		GUI.skin.label.normal.background = null;
		GUI.Label (new Rect(x3GUI+(2*textOffset) + iconWidth,y3GUI+textOffset,width3GUI * .5f,iconWidth), item.itemName);
		GUI.skin.label.alignment = TextAnchor.UpperRight;
		GUI.Label (new Rect(x3GUI+width3GUI-textOffset-(width3GUI*.6f), y3GUI+textOffset,width3GUI*.2f,iconWidth), item.daysRemaining.ToString());
		GUI.Label (new Rect(x3GUI+width3GUI-textOffset-(width3GUI*.5f), y3GUI+textOffset,width3GUI*.4f,iconWidth), item.GetPriceString());
		GUI.Label (new Rect(x3GUI+width3GUI-textOffset-(width3GUI*.15f), y3GUI+textOffset,width3GUI*.1f,iconWidth), item.GetPlaced());
		GUI.skin.label.alignment = TextAnchor.UpperLeft;

		//Draw previous price
		GUI.skin.label.fontSize = fontSize2;
		GUI.Box (new Rect(x2GUI, y2GUI, width2GUI, height2GUI), "");
		int i = 4, pos = 0;
		for(; i > 1; i--){
			GUI.Label (new Rect(x2GUI+textOffset+(pos*width2GUI*.115f), y2GUI+textOffset, width2GUI*.2f, height2GUI), GetDigit(startValue, i));
			pos++;
		}
		GUI.Label (new Rect(x2GUI+textOffset+width2GUI*.35f, y2GUI+textOffset, width2GUI*.2f, height2GUI), "zl");
		for(; i > -1; i--){
			GUI.Label (new Rect(x2GUI+textOffset+(width2GUI*.225f)+(pos*width2GUI*.115f), y2GUI+textOffset, width2GUI*.2f, height2GUI), GetDigit(startValue, i));
			pos++;
		}
		GUI.Label (new Rect(x2GUI+textOffset+width2GUI*.8f, y2GUI+textOffset, width2GUI*.2f, height2GUI), "gr");

		GUI.skin.label.fontSize = fontSize;

		//Draw user interface
		i = 4;
		pos = 0;
		for(; i > 1; i--){
			if(currentPosition == i){
				GUI.skin.label.normal.textColor = selectTextColor;
			}
			else{
				GUI.skin.label.normal.textColor = textColor;
			}
			GUI.Label (new Rect(xGUI+textOffset+(pos*widthGUI*.115f), yGUI+textOffset, widthGUI*.2f, heightGUI), GetDigit(value, i));
			pos++;
		}
		GUI.skin.label.normal.textColor = textColor;
		GUI.Label (new Rect(xGUI+textOffset+widthGUI*.35f, yGUI+textOffset, widthGUI*.2f, heightGUI), "zl");
		for(; i > -1; i--){
			if(currentPosition == i){
				GUI.skin.label.normal.textColor = selectTextColor;
			}
			else{
				GUI.skin.label.normal.textColor = textColor;
			}
			GUI.Label (new Rect(xGUI+textOffset+(widthGUI*.225f)+(pos*widthGUI*.115f), yGUI+textOffset, widthGUI*.2f, heightGUI), GetDigit(value, i));
			pos++;
		}
		GUI.skin.label.normal.textColor = textColor;
		GUI.Label (new Rect(xGUI+textOffset+widthGUI*.8f, yGUI+textOffset, widthGUI*.2f, heightGUI), "gr");
	}
}
