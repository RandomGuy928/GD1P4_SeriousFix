using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConversationManagerScript : MonoBehaviour {

	public GUISkin skin;
	public bool DEBUG;

	Dictionary<string, Conversation> conversations = new Dictionary<string, Conversation>();

	GameObject talker;
	GameObject convItem;
	string goal;

	AudioManagerScript audioManager;
	InventoryScript inventory;
	EventManagerScript eventManager;
	BarterScript barter;
	bool talking;
	bool gettingItem = false;
	Conversation currentConv;
	int numResponses;
	int selectedResponse;
	int minResponse;
	int maxResponse;
	float itemPrice;
	//AudioConversationScript acsManager = GameObject.FindGameObjectWithTag("AudioConversationManager").GetComponent<AudioConversationScript>();
	
	// Use this for initialization
	void Start () {
		/*currentConv = new Conversation();
		currentConv.currentText = "thisisawholelotofreallylongtextbuddyiknowitsgonnagotothenextlineisupertotallybetcha"; // Still greatest line of code in the game.
		currentConv.options = new string[5];
		currentConv.options[0] = "first";
		currentConv.options[1] = "secondy";
		currentConv.options[2] = "third";
		currentConv.options[3] = "fourth";
		currentConv.options[4] = "fifth";
		numResponses = 5;
		currentConv.currentInteractionType = "respond";
		minResponse = 0;
		maxResponse = 4;*/

		audioManager = GameObject.FindGameObjectWithTag ("AudioManager").GetComponent<AudioManagerScript>();;

		eventManager = GetComponent<EventManagerScript>();
		inventory = GetComponent<InventoryScript>();
		barter = GetComponent<BarterScript>();

		TextAsset[] files = Resources.LoadAll<TextAsset> ("Conversations");
		for(int i = 0; i < files.Length; i++){
			Conversation temp = ParseConversationFile(files[i]);
			if(temp.name != null){
				conversations[temp.name] = temp;
			}
		}
	}

	Conversation ParseConversationFile(TextAsset file){
		Conversation conv = new Conversation();
		string t = file.text.Replace("\r", "");
		string[] lines = t.Split('\n');
		ConversationNode node = null;
		bool vars = false;

		for(int i = 0; i < lines.Length; i++){
			if(DEBUG){
				Debug.Log (lines[i]);
			}
			if(lines[i] == ""){
				continue;
			}

			if(lines[i] == "START_VARS"){
				vars = true;
				continue;
			}
			if(lines[i] == "END_VARS"){
				vars = false;
				continue;
			}
			if(lines[i] == "START_NODE"){
				node = new ConversationNode();
				continue;
			}
			if(lines[i] == "END_NODE"){
				conv.AddNode(node);
				continue;
			}
			string[] text = lines[i].Split ('=');
			text[0] = text[0].Replace (" ", "");
			for(int j = 1; j < text.Length; j++){
				if(text[j][0] == ' '){
					text[j] = text[j].Substring (1);
				}
			}
			if(vars){
				conv.speakers[text[0]] = (Texture2D)Resources.Load ("Portraits/" + text[1]);
			}
			if(text[0] == "conversation_name"){
				conv.name = text[1];
			}
			if(text[0] == "node"){
				node.name = text[1];
			}
			if(text[0] == "type"){
				node.type = text[1];
			}
			if(text[0] == "text_count"){
				node.InitializeTextCount(int.Parse (text[1]));
			}
			if(text[0] == "text"){
				node.AddText (text[1]);
			}
			if(text[0] == "target"){
				node.AddTarget (text[1]);
			}
			if(text[0] == "left"){
				node.left = text[1];
			}
			if(text[0] == "right"){
				node.right = text[1];
			}
			if(text[0] == "speaker"){
				node.AddSpeaker (text[1]);
			}
		}

		return conv;
	}

	void PlayGenderHappy(){
		if(talker.GetComponent<CustomerScript>().isMale){
			audioManager.PlayManHappy();
		}
		else{
			audioManager.PlayWomanHappy();
		}
	}

	void PlayGenderSad(){
		if(talker.GetComponent<CustomerScript>().isMale){
			audioManager.PlayManSad();
		}
		else{
			audioManager.PlayWomanSad();
		}
	}

	void PlayGenderVoice(){
		if(talker.GetComponent<CustomerScript>().isMale){
			audioManager.PlayManVoice ();
		}
		else{
			audioManager.PlayWomanVoice();
		}
	}

	void PlaySoundsForAdvancing(int advanceBy){
		if(currentConv.currentInteractionType == "need_item"){
			if(advanceBy == 1){
				PlayGenderSad ();
			}
			else{
				PlayGenderHappy ();
			}
		}
		if(currentConv.currentInteractionType == "barter"){
			if(advanceBy == 0){
				audioManager.PlayAudioSuccesfulPurchase();
			}
			else{
				PlayGenderSad();
			}
		}
		audioManager.PlayAudioNext ();
	}

	void AdvanceConversation(int advanceBy){
		if(currentConv.currentInteractionType == "respond" || currentConv.currentInteractionType == "need_item"){
			eventManager.AddDecision (currentConv.GetCurrentNodeName(), selectedResponse);
		}
		PlaySoundsForAdvancing (advanceBy);
		currentConv.Advance(advanceBy);
		SetUpResponses ();
		if(currentConv.currentInteractionType == "end"){
			EndConversation ();
		}
		else if(currentConv.currentInteractionType == "find_item"){
			convItem = talker.GetComponent<CustomerScript>().FindValidItem ();
			if(convItem==null){
				AdvanceConversation (1);
			}
			else{
				AdvanceConversation(0);
			}
		}
		else if(currentConv.currentInteractionType == "need_item"){
			gettingItem = true;
			inventory.GetItem ("conversation", gameObject);
		}
		else if(currentConv.currentInteractionType == "barter"){
			gettingItem = true;
			float valueToSend = itemPrice;
			if(valueToSend == -1f){
				valueToSend = convItem.GetComponent<ItemScript>().basePrice;
			}
			barter.StartBartering(valueToSend, "conversation", gameObject, convItem);
		}
	}

	public void ReceiveItem(GameObject go){
		convItem = go;
		gettingItem = false;
		if(go == null){
			AdvanceConversation(1);
		}
		else if(!talker.GetComponent<CustomerScript>().SetItem (convItem)){
			AdvanceConversation (1);
		}
		else{
			if(currentConv.name == "homeless1" || currentConv.name == "homeless4" || currentConv.name == "homeless12"){
				inventory.RemoveItem (go);
			}
			AdvanceConversation (0);
		}
	}

	public void ReceivePrice(float price){
		gettingItem = false;
		itemPrice = price;
		if(talker.GetComponent<CustomerScript>().TestIfWillingToPay(price)){
			CompleteTransaction (price);
			AdvanceConversation (0);
		}
		else{
			AdvanceConversation (1);
		}
	}

	void CompleteTransaction(float price){
		Transaction t = new Transaction(convItem, talker, price);
		inventory.RemoveItem (convItem);
		eventManager.AddTransaction (t);
	}
	
	// Update is called once per frame
	void Update () {
		if(talking && !gettingItem){
			if(Input.GetButtonDown("Select")){
				AdvanceConversation(selectedResponse);

			}
			if(Input.GetButtonDown ("Up")){
				selectedResponse -= 1;
				audioManager.PlayAudioScroll();
				if(selectedResponse == -1){
					selectedResponse = numResponses - 1;
					minResponse = Mathf.Max(numResponses - 4, 0);
					maxResponse = numResponses;
				}
				else{
					if(selectedResponse < minResponse){
						minResponse-=1;
						maxResponse-=1;
					}
				}
			}
			if(Input.GetButtonDown ("Down")){
				selectedResponse = selectedResponse + 1;
				audioManager.PlayAudioScroll ();
				if(selectedResponse == numResponses){
					selectedResponse = 0;
					minResponse = 0;
					maxResponse = Mathf.Min (numResponses, 4);
				}
				else{
					if(selectedResponse == maxResponse){
						minResponse+=1;
						maxResponse+=1;
					}
				}
			}
		}
	}

	public void InitializeConversation(GameObject t, string g, string ID){
		talker = t;
		PlayGenderVoice ();
		goal = g;
		talking = true;
		currentConv = conversations[ID];
		currentConv.Initialize ();
		SetUpResponses ();
		itemPrice = -1f;
	}

	void SetUpResponses(){
		if(currentConv != null && currentConv.currentInteractionType=="respond"){
			numResponses = currentConv.options.Length;
			selectedResponse = 0;
			minResponse = 0;
			maxResponse = Mathf.Min (4, numResponses);
		}
	}

	public void EndConversation(){
		if(eventManager.CheckEnd()){
			talking = false;
		}
		else{
			talking = false;
			talker.GetComponent<CustomerScript>().SendToStreet ();
		}
	}

	string ReplaceKeywords(string text){
		if(convItem != null && text != null){
			text = text.Replace ("<ITEM>", convItem.GetComponent<ItemScript>().itemName);
		}
		if(talker!=null && text != null){
			text = text.Replace ("<GOAL>", talker.GetComponent<CustomerScript>().goal);
		}
		return text;
	}

	string AddSpeaker(string text){
		return currentConv.currentSpeaker + ":  " + text;
	}

	void OnGUI(){
		if(!talking){
			return;
		}

		GUI.skin = skin;

		float xGUI = 0f;
		float widthGUI = Screen.width;
		float heightGUI = Screen.height / 4f;
		float yGUI = Screen.height - heightGUI;
		int fontSize = (int)(heightGUI/5f);
		float textOffset = 4f;

		float xlGUI = Screen.width*.05f;
		float widthlGUI = Screen.width * .3f;
		float heightlGUI = (widthlGUI/3f)*4F;
		float ylGUI = Screen.height - heightlGUI - heightGUI;

		float widthrGUI = widthlGUI;
		float heightrGUI = heightlGUI;
		float yrGUI = ylGUI;
		float xrGUI = (Screen.width * .95f) - widthrGUI;

		GUI.Box (new Rect(xGUI, yGUI, widthGUI, heightGUI),"");
		GUI.skin.label.fontSize = fontSize;
		Color textColor = GUI.skin.label.normal.textColor;
		Color selectTextColor = GUI.skin.label.hover.textColor;

		GUI.depth = 5;

		if(currentConv.currentInteractionType != "need_item"
		   && currentConv.currentInteractionType != "barter"){
			if(currentConv.left != null){
				GUI.skin.label.normal.background = currentConv.left;
				GUI.Label (new Rect(xlGUI, ylGUI, widthlGUI, heightlGUI), "");
			}
			if(currentConv.right != null){
				GUI.skin.label.normal.background = currentConv.right;
				GUI.Label (new Rect(xrGUI, yrGUI, widthrGUI, heightrGUI), "");
			}
			GUI.skin.label.normal.background = null;
		}

		if(currentConv.currentInteractionType == "listen" 
		   || currentConv.currentInteractionType == "barter"
		   || currentConv.currentInteractionType == "find_item"
		   || currentConv.currentInteractionType == "need_item"
		   || currentConv.currentInteractionType == "listen_random"){
			GUI.Label (new Rect(xGUI + textOffset,yGUI + textOffset,widthGUI - (2*textOffset),heightGUI - (2*textOffset)), AddSpeaker(ReplaceKeywords(currentConv.currentText)));
		}
		else if(currentConv.currentInteractionType == "respond"){
			int pos = 0;
			for(int i = minResponse; i < maxResponse; i++){
				if(selectedResponse == i){
					GUI.skin.label.normal.textColor = selectTextColor;
				}
				else{
					GUI.skin.label.normal.textColor = textColor;
				}
				GUI.Label (new Rect(xGUI+textOffset,yGUI+textOffset+(heightGUI*.23f*(pos)),widthGUI - (2*textOffset),heightGUI * .3f), ReplaceKeywords(currentConv.options[i]));
				pos+=1;
			}
		}

		GUI.skin.label.normal.textColor = textColor;
	}
}
