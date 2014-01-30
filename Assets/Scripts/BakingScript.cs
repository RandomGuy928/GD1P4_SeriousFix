using UnityEngine;
using System.Collections;

public class BakingScript: MonoBehaviour {
	
	public GUISkin skin;
	public GameObject[] bread;
	public GameObject[] allBreads;
	
	public Font bakingFont;
	
	int[] numToBake;
	float[] familyManhours;
	PersonScript[] family;
	FamilyManagerScript fms;
	EventManagerScript eventManager;
	int selectedWindow = 0;
	int selectedItem = 1;
	int selectedBLItem = 0;
	
	ArrayList breadList = new ArrayList();
	
	float inputDelay = .1f;
	float inputTimer = 0f;
	float totalManhours = 0f;
	bool baking = false;
	int top = 0;
	int bottom = 0;
	int familyTracker = 0;
	int[] fWorkers = new int[2];
	int hoursTracker = 0;
	// Use this for initialization
	void Start () {
		fms = GameObject.FindGameObjectWithTag("FamilyManager").GetComponent<FamilyManagerScript>();
		eventManager = GetComponent<EventManagerScript>();
		numToBake = new int[bread.Length];

		
		/*for (int i = 0; i < allBreads.Length; i++) {	// Need a total list of the breads available for baking.
			GameObject go = (GameObject)Instantiate(allBreads[i]);
			go.transform.position = new Vector3(999f, 999f, 999f);
			breadList.Add (allBreads[i]);
		}*/
	}
	
	// Update is called once per frame
	void Update () {
		if(baking){
			inputTimer += Time.deltaTime;
			if(Input.GetButtonDown ("Select") && inputTimer > inputDelay && selectedWindow == 2){
				EndBaking ();
			}
			
			if(Input.GetButtonDown ("Select") && inputTimer > inputDelay && selectedWindow == 0){
				bool addIt = false;
				if ( familyManhours[fWorkers[familyTracker]] >= allBreads[selectedItem].GetComponent<ItemScript>().bakeTime ) {
					familyManhours[fWorkers[familyTracker]] -= allBreads[selectedItem].GetComponent<ItemScript>().bakeTime;
					addIt = true;
				}
				
				else if (totalManhours <= allBreads[selectedItem].GetComponent<ItemScript>().bakeTime) {
					addIt = false;
				}
				
				else if (familyTracker < 1) {
					float hoursRemaining =  allBreads[selectedItem].GetComponent<ItemScript>().bakeTime - familyManhours[familyTracker];
					familyManhours[fWorkers[familyTracker]] = 0;
					familyTracker += 1;
					familyManhours[fWorkers[familyTracker]] -= hoursRemaining;
					addIt = true;
				}
				
				if (addIt) {
					GameObject go = (GameObject)Instantiate(allBreads[selectedItem]);
					go.transform.position = new Vector3(999f, 999f, 999f);
					breadList.Add (allBreads[selectedItem]);
					if (bottom < 6) {
						bottom += 1;
					}
					else {
						bottom += 1;
						top += 1;
						selectedBLItem += 1;
					}
				}
				
			}
			
			if(Input.GetKeyDown(KeyCode.UpArrow) && inputTimer > inputDelay && selectedWindow == 0) {
				selectedItem = (selectedItem + 1) % allBreads.Length;
			}
			
			if(Input.GetKeyDown(KeyCode.DownArrow) && inputTimer > inputDelay && selectedWindow == 0) {
				selectedItem = (selectedItem - 1) % allBreads.Length;
				if (selectedItem < 0)
					selectedItem = allBreads.Length - 1;
			}
			
			if(Input.GetKeyDown(KeyCode.DownArrow) && inputTimer > inputDelay && selectedWindow == 1) {
				if (selectedBLItem < breadList.Count-1)
					selectedBLItem += 1;
				else
					selectedBLItem = breadList.Count-1;
				
				if (selectedBLItem >= bottom) {
					top += 1;
					bottom += 1;
				}
			}
			
			if(Input.GetKeyDown(KeyCode.UpArrow) && inputTimer > inputDelay && selectedWindow == 1) {
				if (selectedBLItem > 0)
					selectedBLItem -= 1;
				else
					selectedBLItem = 0;
				
				if (selectedBLItem < top) {
					top -= 1;
					bottom -= 1;
				}
			}
			
			if(Input.GetButtonDown ("Back") && inputTimer > inputDelay && selectedWindow == 1){
				familyManhours[fWorkers[familyTracker]] += ((GameObject)breadList[selectedBLItem]).GetComponent<ItemScript>().bakeTime;
				breadList.RemoveAt (selectedBLItem);
				
				if (bottom > 6) {
					
				}
				else {
					bottom -= 1;
				}
				
			}
			
			if(Input.GetKeyDown(KeyCode.Tab) ){
				selectedWindow = (selectedWindow + 1) % 3;
			}
		}
		
	//	if (Input.GetKeyDown (KeyCode.B) ) {
	//		StartBaking ();
		//}
		//bread[2].GetComponent<ItemScript>();
	}
	
	public void StartBaking(){
		inputTimer = 0f;
		family = fms.GetFamily ();
		breadList = new ArrayList();
		Debug.Log ("Array List Size: " + breadList.Count);
		for (int i = 0; i < family.Length; i++) {
			totalManhours += family[i].GetManhours();
			Debug.Log (family[i].firstName);
		}

		int useless = 0;
		familyManhours = new float[family.Length];
		for (int i = 0; i < family.Length; i++) {
			if (family[i].firstName == "Daughter" || family[i].firstName == "Wife") {
				familyManhours[i] = family[i].GetManhours ();
				fWorkers[useless] = i;
				useless++;
			}
		}
		top = 0;
		bottom = 0;
		baking = true;
		selectedWindow = 0;
		selectedItem = 0;
		//selectedBLItem = 0;
		for(int i = 0; i < numToBake.Length; i++){
			numToBake[i] = 0;
		}
	}
	
	void EndBaking(){
		GameObject[] finalBread = new GameObject[breadList.Count];
		Debug.Log ("breadList Final Count: " + breadList.Count);
		for ( int i = 0; i < breadList.Count; i++) {
			finalBread[i] = (GameObject)breadList[i];
			Debug.Log(finalBread[i].GetComponent<ItemScript>().itemName);
		}
		eventManager.ReceiveBaking(finalBread);
		baking = false;
		familyTracker = 0;
	}
	
	void OnGUI(){
		if(!baking)
			return;
		
		float textOffset = 4f;
		float iconWidth = Screen.height * .075f;
		
		float xGUI = Screen.width * .55f;		// Only Box on the right, Total of what you are cooking
		float widthGUI = Screen.width * .4f;
		float heightGUI = Screen.height * .71f;
		float yGUI = Screen.height * .09f;
		
		float x2GUI = Screen.width * .05f;		// Top box on the left, your inventory and hours
		float y2GUI = yGUI;
		float width2GUI = Screen.width * .49f;
		float height2GUI = Screen.height * .35f;
		
		float x3GUI = x2GUI;					// Bottom Box on the left, your total man hours
		float y3GUI = Screen.height * .45f;
		float width3GUI = width2GUI;
		float height3GUI = height2GUI;
		
		float x4GUI = Screen.width * .35f;		// Bake Box
		float y4GUI = Screen.height * .825f;
		float width4GUI = Screen.width * .4f;
		float height4GUI = Screen.height * .1f;
		
		// Test Box
		
		// Background Box
		GUI.skin = skin;
		GUI.Box (new Rect(0, 0, Screen.width, Screen.height), "");
		GUI.skin = null;
		GUI.skin.label.font = bakingFont;
		
		
		// Draw boxes
		//GUI.skin.label.normal.background = sectionBG;
		GUI.Box(new Rect(xGUI, yGUI, widthGUI, heightGUI), "");
		GUI.Box(new Rect(x2GUI, y2GUI, width2GUI, height2GUI), "" );
		GUI.Box(new Rect(x3GUI, y3GUI, width3GUI, height3GUI), "" );
		GUI.Box(new Rect(x4GUI, y4GUI, width4GUI, height4GUI), "" );
		//GUI.skin.label.normal.background = null;
		int fontSize = GUI.skin.label.fontSize;
		//GUI.Label (new Rect(xGUI, yGUI, Screen.width* .5f, Screen.height*.5f), "I need this to show up on the screen 2");
		// Draw Top Left Box Contents
		ItemScript item = ((GameObject)allBreads[selectedItem]).GetComponent<ItemScript>();
		//GUIStyle styleTopLeft = new GUIStyle(GUI.skin.label);
		if (selectedWindow == 0) {
			GUI.skin.label.normal.textColor = Color.white;
		}
		else {
			GUI.skin.label.normal.textColor = Color.black;
		}
		
		GUI.skin.label.fontSize = 25;
		GUI.skin.label.normal.background = item.image;
		GUI.Label (new Rect(x2GUI + textOffset*5, y2GUI + textOffset * 2, iconWidth, iconWidth), "");
		GUI.skin.label.normal.background = null;
		GUI.skin.label.alignment = TextAnchor.UpperCenter;
		GUI.Label( new Rect(x2GUI + (Screen.width * .07f) + iconWidth, y2GUI + textOffset * 2, width2GUI * .5f, iconWidth * 2), item.itemName );
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		
		GUI.skin.label.fontSize = 15;
		GUI.Label (new Rect(x2GUI + (Screen.width * .36f) + iconWidth, y2GUI + textOffset * 2, width2GUI * .3f, iconWidth), (selectedItem+1) + "/" + allBreads.Length);
		
		GUI.skin.label.fontSize = 15;
		GUI.Label (new Rect(x2GUI + textOffset*5, y2GUI + textOffset * 2 + (Screen.height * .1f), width2GUI * .7f, iconWidth), "Price: " + item.GetPriceString());
		GUI.Label (new Rect(x2GUI + textOffset*5, y2GUI + textOffset * 2 + (Screen.height * .15f), width2GUI * .7f, iconWidth), "Experation: " + item.daysRemaining + " Days");
		GUI.Label (new Rect(x2GUI + textOffset*5, y2GUI + textOffset * 2 + (Screen.height * .2f), width2GUI * .7f, iconWidth), "Bake Time: " + item.bakeTime + " Hours");
		GUI.Label (new Rect(x2GUI + textOffset*5, y2GUI + textOffset * 2 + (Screen.height * .25f), width2GUI * .7f, iconWidth), "Keywords: ");
		
		/*string itemKeywords = item.keywords[0];
		for (int i = 1; i < item.keywords.Length - 1; i++) {
			itemKeywords = itemKeywords + ", " + item.keywords[i];
		}*/
		string itemKeywords = GetKeywords (item.keywords);
		GUI.Label (new Rect(x2GUI + textOffset * 7 + (Screen.width * .12f), y2GUI + textOffset * 2 + (Screen.height * .25f), width2GUI * .7f, iconWidth), itemKeywords);
		
		// Draw Bottom Left Contets
		GUI.skin.label.normal.textColor = Color.black;
		GUI.skin.label.fontSize = 20;
		GUI.skin.label.alignment = TextAnchor.UpperCenter;
		GUI.Label (new Rect(x3GUI + textOffset + (Screen.width * .05f), y3GUI + textOffset, width3GUI * .9f, iconWidth), "Total Manhours Remaining");
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		//GUI.Label (new Rect(x3GUI + textOffset + (Screen.width * .17f), y3GUI + textOffset + (Screen.width * .10f), width3GUI * .9f, iconWidth), totalManhours.ToString() + " Hours");
		
		int famPos = 1;
		for (int i  = 0; i < family.Length; i++) {
			if (family[i].firstName == "Daughter" || family[i].firstName == "Wife") {
				GUI.skin.label.fontSize = 17;
				GUI.Label (new Rect(x3GUI + textOffset*5, y3GUI + textOffset + (Screen.height * .07f * famPos), width3GUI * .9f, iconWidth), family[i].name );
				GUI.skin.label.alignment = TextAnchor.UpperRight;
				GUI.Label (new Rect(x3GUI + textOffset*5, y3GUI + textOffset + (Screen.height * .07f * famPos), width3GUI * .9f, iconWidth), familyManhours[i] + " Hours" );
				GUI.skin.label.alignment = TextAnchor.UpperLeft;
				fWorkers[famPos-1] = i;
				famPos += 1;
			}
		}
		// Draw Right Box Contents
		
		GUI.skin.label.fontSize = 18;
		GUI.Label (new Rect(xGUI+(widthGUI*.15f), yGUI+textOffset, widthGUI*.4f, Screen.height*.2f), "Name");
		GUI.skin.label.alignment = TextAnchor.UpperRight;
		GUI.Label (new Rect(xGUI+widthGUI-textOffset-(widthGUI*.80f), yGUI+textOffset, widthGUI*.4f, Screen.height*.2f), "Time");
		GUI.Label (new Rect(xGUI+widthGUI-textOffset-(widthGUI*.55f), yGUI+textOffset, widthGUI*.4f, Screen.height*.2f), "Price");
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		
		int pos = 1;
		for (int i = top; i < bottom; i++) {
			ItemScript itemBread = ((GameObject)breadList[i]).GetComponent<ItemScript>();
			if (selectedWindow == 1 && selectedBLItem == i) {
				GUI.skin.label.normal.textColor = Color.white;
			}
			GUI.skin.label.fontSize = 17;
			GUI.Label (new Rect(xGUI+(widthGUI*.10f), yGUI+textOffset+(Screen.height*.1f*pos) , widthGUI*.3f, Screen.height*.2f), itemBread.itemName);
			GUI.skin.label.alignment = TextAnchor.UpperRight;
			GUI.Label (new Rect(xGUI+widthGUI-textOffset-(widthGUI*.85f), yGUI+textOffset+(Screen.height*.1f*pos), widthGUI*.4f, Screen.height*.2f), itemBread.bakeTime.ToString ());
			GUI.Label (new Rect(xGUI+widthGUI-textOffset-(widthGUI*.55f), yGUI+textOffset+(Screen.height*.1f*pos), widthGUI*.4f, Screen.height*.2f), itemBread.GetPriceString());
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
			GUI.skin.label.normal.textColor = Color.black;
			pos += 1;
		}
		/*int pos = 0;
		for(int i = top; i < bottom; i++){
			item = ((GameObject)breadList[i]).GetComponent<ItemScript>();
			if(selectedItem == i){
				GUI.skin.label.normal.textColor = Color.white;
			}
			else{
				GUI.skin.label.normal.textColor = Color.black;
			}
			GUI.skin.label.normal.background = item.image;
			GUI.Label (new Rect(xGUI+textOffset,yGUI+textOffset+(Screen.height*.1f*pos),iconWidth, iconWidth),"");
			GUI.skin.label.normal.background = null;
			GUI.Label (new Rect(xGUI+(2*textOffset) + iconWidth,yGUI+textOffset+(Screen.height*.1f*pos),widthGUI * .5f,iconWidth), item.itemName);
			GUI.skin.label.alignment = TextAnchor.UpperRight;
			GUI.Label (new Rect(xGUI+widthGUI-textOffset-(widthGUI*.6f), yGUI+textOffset+(Screen.height*.1f*pos),widthGUI*.2f,iconWidth), item.daysRemaining.ToString());
			GUI.Label (new Rect(xGUI+widthGUI-textOffset-(widthGUI*.5f), yGUI+textOffset+(Screen.height*.1f*pos),widthGUI*.4f,iconWidth), item.GetPriceString());
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
			pos+=1;
		}*/
		
		// Draw Bottom 'BAKE' Box
		GUI.skin = skin;
		GUI.skin.label.fontSize = 20;
		
		GUIStyle style = new GUIStyle(GUI.skin.label);
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		if (selectedWindow == 2) {
			GUI.skin.label.normal.textColor = Color.white;
		}
		else {
			GUI.skin.label.normal.textColor = Color.black;
		}
		GUI.Label (new Rect(x4GUI, y4GUI, width4GUI, height4GUI), "BAKE");
		
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		GUI.skin.label.fontSize = fontSize;
	}

	string GetKeywords(string[] keywords) {
		string returnKeywords = keywords[0];
		for (int i = 1; i < keywords.Length; i++) {
			returnKeywords = returnKeywords + ", " + keywords[i];
		}
		Debug.Log (returnKeywords);
		return returnKeywords;
	}
}
