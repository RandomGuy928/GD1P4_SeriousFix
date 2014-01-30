using UnityEngine;
using System.Collections;

public class InventoryScript : MonoBehaviour {

	public GUISkin skin;
	public GameObject[] startingPastries;
	public Texture2D GUILineTexture;

	ArrayList inventory = new ArrayList();
	bool viewingInventory = false;
	InputManagerScript input;
	string returnItemTo;
	GameObject returnItemToObject;
	float openDelay = 0f;
	float openDelayTime = .1f;
	TransitionScript transition;

	int top = 0;
	int bottom = 0;
	int selectedItem;

	// Use this for initialization
	void Start () {
		transition = GameObject.FindGameObjectWithTag("Transition").GetComponent<TransitionScript>();
		input = GetComponent<InputManagerScript>();

		GameObject go;

		for(int i = 0; i < startingPastries.Length; i++){
			go = (GameObject)Instantiate(startingPastries[i]);
			go.transform.position = new Vector3(999f, 999f, 999f);
			inventory.Add (go);
		}

		bottom = Mathf.Min (inventory.Count, 6);
	}

	public void AddItem(GameObject it){
		GameObject go = (GameObject)Instantiate (it);
		go.transform.position = new Vector3(999f, 999f, 999f);
		inventory.Add (go);
		bottom = Mathf.Min (inventory.Count, 6);
	}

	public void RemoveItem(GameObject it){
		it.GetComponent<ItemScript>().RemoveFromPosition ();
		inventory.Remove ((object)it);
	}

	public void RemoveOldestBread(){
		Debug.Log ("Remove Oldest Bread - BEHAVIOR NOT CLEARLY DEFINED - what breads count?");
	}

	public void AgeItems(){
		ArrayList itemsToRemove = new ArrayList();
		for(int i = 0; i < inventory.Count; i++){
			((GameObject)inventory[i]).GetComponent<ItemScript>().daysRemaining--;
			if(((GameObject)inventory[i]).GetComponent<ItemScript>().daysRemaining < 0){
				itemsToRemove.Add ((GameObject)inventory[i]);
			}
		}
		for(int i = 0; i < itemsToRemove.Count; i++){
			RemoveItem ((GameObject)itemsToRemove[i]);
		}
	}

	public void GetItem(string from, GameObject fromObject){
		returnItemTo = from;
		returnItemToObject = fromObject;
		viewingInventory = true;
		selectedItem = 0;
		top = 0;
		bottom = Mathf.Min (inventory.Count, 6);
		if(inventory.Count > 0){
			((GameObject)inventory[selectedItem]).GetComponent<ItemScript>().HighlightPosition();
		}
	}

	void ReturnItem(GameObject returnItem){
		if(inventory.Count > 0){
			((GameObject)inventory[selectedItem]).GetComponent<ItemScript>().DehighlightPosition();
		}
		openDelay = 0f;
		viewingInventory = false;
		if(returnItemTo == "position"){
			returnItemToObject.GetComponent<ItemPositionManagerScript>().ReceiveItem(returnItem);
		}
		if(returnItemTo == "conversation"){
			returnItemToObject.GetComponent<ConversationManagerScript>().ReceiveItem(returnItem);
		}
	}

	// Update is called once per frame
	void Update () {
		if(viewingInventory){
			openDelay+=Time.deltaTime;
			if(Input.GetButtonDown("Select") && openDelay > openDelayTime){
				if(inventory.Count > 0){
					ReturnItem ((GameObject)inventory[selectedItem]);
				}
				else{
					ReturnItem(null);
				}
			}
			if(Input.GetButtonDown ("Back")){
				ReturnItem(null);
			}
			if(input.tickUp){
				if(inventory.Count > 0){
					((GameObject)inventory[selectedItem]).GetComponent<ItemScript>().DehighlightPosition();
				}
				selectedItem -= 1;
				if(selectedItem == -1){
					selectedItem = inventory.Count - 1;
					top = Mathf.Max(inventory.Count - 6, 0);
					bottom = inventory.Count;
				}
				else{
					if(selectedItem < top){
						bottom-=1;
						top-=1;
					}
				}
				if(inventory.Count > 0){
					((GameObject)inventory[selectedItem]).GetComponent<ItemScript>().HighlightPosition();
				}
			}
			if(input.tickDown){
				if(inventory.Count > 0){
					((GameObject)inventory[selectedItem]).GetComponent<ItemScript>().DehighlightPosition();
				}				selectedItem = selectedItem + 1;
				if(selectedItem == inventory.Count){
					selectedItem = 0;
					top = 0;
					bottom = Mathf.Min (inventory.Count, 6);
				}
				else{
					if(selectedItem == bottom){
						top+=1;
						bottom+=1;
					}
				}
				if(inventory.Count > 0){
					((GameObject)inventory[selectedItem]).GetComponent<ItemScript>().HighlightPosition();
				}
			}
		}
	}

	void OnGUI(){
		if(!viewingInventory || transition.blockMoney)
			return;

		//Setup Vars
		GUI.skin = skin;
		float xGUI = Screen.width * .01f;
		float widthGUI = Screen.width * .75f;
		float x2GUI = widthGUI + (2*xGUI);
		float width2GUI = Screen.width - widthGUI - (3* xGUI);
		float heightGUI = Screen.height * .6825f;
		float yGUI = Screen.height * .061f;
		int fontSize = (int)(Screen.height/20f);
		float textOffset = 4f;
		float iconWidth = Screen.height * .075f;

		//Draw boxes
		GUI.Box (new Rect(xGUI, yGUI, widthGUI, heightGUI),"");
		GUI.Box (new Rect(x2GUI, yGUI, width2GUI, heightGUI), "");
		GUI.skin.label.fontSize = fontSize;
		Color textColor = GUI.skin.label.normal.textColor;
		Color selectTextColor = GUI.skin.label.hover.textColor;

		//Setup column names
		GUI.Label (new Rect(xGUI+(widthGUI*.15f), yGUI+textOffset, widthGUI*.4f, Screen.height*.2f), "Name");
		GUI.skin.label.alignment = TextAnchor.UpperRight;
		GUI.Label (new Rect(xGUI+widthGUI-textOffset-(widthGUI*.76f), yGUI+textOffset, widthGUI*.4f, Screen.height*.2f), "Time");
		GUI.Label (new Rect(xGUI+widthGUI-textOffset-(widthGUI*.55f), yGUI+textOffset, widthGUI*.4f, Screen.height*.2f), "Price");
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		GUI.Label (new Rect(x2GUI+textOffset, yGUI+textOffset, width2GUI, Screen.height*.2f), "Keywords");

		//Draw lines
		GUI.skin.label.normal.background = GUILineTexture;
		GUI.Label (new Rect(xGUI, yGUI+Screen.height*.075f,widthGUI, Screen.height*.01f), "");
		GUI.Label (new Rect(x2GUI, yGUI+Screen.height*.075f,width2GUI, Screen.height*.01f), "");
		GUI.skin.label.normal.background = null;
		yGUI = yGUI + Screen.height*.085f;

		if(inventory.Count == 0){
			GUI.Label (new Rect(xGUI, yGUI, widthGUI, heightGUI), "There is no food");
			return;
		}

		//Write keywords
		ItemScript item = ((GameObject)inventory[selectedItem]).GetComponent<ItemScript>();
		for(int i = 0; i < item.keywords.Length; i++){
			GUI.Label (new Rect(x2GUI + textOffset, yGUI + textOffset + (i*Screen.height*.05f), width2GUI - (2*textOffset), Screen.height/10f), item.keywords[i]);
		}

		//Write item properties
		int pos = 0;
		for(int i = top; i < bottom; i++){
			item = ((GameObject)inventory[i]).GetComponent<ItemScript>();
			if(selectedItem == i){
				GUI.skin.label.normal.textColor = selectTextColor;
			}
			else{
				GUI.skin.label.normal.textColor = textColor;
			}
			GUI.skin.label.normal.background = item.image;
			GUI.Label (new Rect(xGUI+textOffset,yGUI+textOffset+(Screen.height*.1f*pos),iconWidth, iconWidth),"");
			GUI.skin.label.normal.background = null;
			GUI.Label (new Rect(xGUI+(2*textOffset) + iconWidth,yGUI+textOffset+(Screen.height*.1f*pos),widthGUI * .5f,iconWidth), item.itemName);
			GUI.skin.label.alignment = TextAnchor.UpperRight;
			GUI.Label (new Rect(xGUI+widthGUI-textOffset-(widthGUI*.6f), yGUI+textOffset+(Screen.height*.1f*pos),widthGUI*.2f,iconWidth), item.daysRemaining.ToString());
			GUI.Label (new Rect(xGUI+widthGUI-textOffset-(widthGUI*.5f), yGUI+textOffset+(Screen.height*.1f*pos),widthGUI*.4f,iconWidth), item.GetPriceString());
			GUI.Label (new Rect(xGUI+widthGUI+textOffset-(widthGUI*.15f), yGUI+textOffset+(Screen.height*.1f*pos),widthGUI*.1f,iconWidth), item.GetPlaced());
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
			pos+=1;
		}
		
		GUI.skin.label.normal.textColor = textColor;
	}

}
