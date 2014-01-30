using UnityEngine;
using System.Collections;

public class PersonScript : MonoBehaviour {

	public GUISkin skin;
	public string firstName;
	public bool isFamily;

	public bool isCold = false;
	public bool isHungry = false;
	public bool isSick = false;
	public bool isDead = false;

	public TextAsset lines;

	public float baseManhours = 10f;

	public string eventKeyword;
	bool displayText;
	Vector3 textPos;

	Color targetTextColor;
	Color textColor;

	int daysWithoutFood;
	int daysWithoutHeat;
	int daysWithoutMedicine;

	string[] healthyText;
	string[] hungryText;
	string[] coldText;
	string[] sickText;
	int lineMemory;
	EventManagerScript eventManager;

	SpriteRenderer sr;

	// Use this for initialization
	void Start () {
		eventManager = GameObject.FindGameObjectWithTag("EventManager").GetComponent<EventManagerScript>();
		sr = GetComponent<SpriteRenderer>();
		ParseLines ();
		textPos = transform.GetChild (0).transform.position;
		targetTextColor = Color.black;
		targetTextColor.a = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		if(isDead || (!isFamily && !eventManager.hasJews)){
			sr.enabled = false;
			collider2D.enabled = false;
		}
		else{
			sr.enabled = true;
			collider2D.enabled = true;
		}
		textColor = Color.Lerp (textColor, targetTextColor, 2f * Time.deltaTime);
		if(textColor.a == 0f){
			displayText = false;
		}
	}

	string[] ArrayFromArrayList(ArrayList al){
		string[] output = new string[al.Count];
		for(int i = 0; i < al.Count; i++){
			output[i] = (string)al[i];
		}
		return output;
	}

	void ParseLines(){
		string[] text = lines.text.Replace("\r", "").Split ('\n');
		ArrayList temp = new ArrayList();
		for(int i = 0; i < text.Length; i++){
			if(text[i] == "START"){
				temp = new ArrayList();
			}
			string[] tempStr = text[i].Split('=');
			if(tempStr[0] == "END"){
				if(tempStr[1] == "HEALTHY"){
					healthyText = ArrayFromArrayList (temp);
				}
				else if(tempStr[1] == "COLD"){
					coldText = ArrayFromArrayList (temp);
				}
				else if(tempStr[1] == "HUNGRY"){
					hungryText = ArrayFromArrayList (temp);
				}
				else if(tempStr[1] == "SICK"){
					sickText = ArrayFromArrayList (temp);
				}
			}
			if(tempStr[0] == "T"){
				temp.Add (tempStr[1]);
			}
		}
	}

	string GetLineFromArray(string[] text){
		if(lineMemory == -1){
			lineMemory = (int)Random.Range (0, text.Length);
		}
		return text[lineMemory];
	}

	public bool IsHealthy(){
		return !(isSick || isHungry || isCold || isDead);
	}

	public bool IsHungry(){
		return isHungry;
	}

	public bool IsCold(){
		return isCold;
	}

	public bool IsSick(){
		return isSick;
	}

	public bool IsDead(){
		return isDead;
	}

	public float GetManhours(){
		float multiplier = 1f;
		if(isDead){
			return 0;
		}
		if(isSick){
			multiplier -= .15f;
		}
		if(isCold){
			multiplier -= .15f;
		}
		if(isHungry){
			multiplier -= .15f;
		}
		return baseManhours * multiplier;
	}

	public string GetSpeech(){
		if(eventKeyword != ""){
			return "events aren't done yet go away";
		}
		else if(isSick){
			return GetLineFromArray(sickText);
		}
		else if(isHungry){
			return GetLineFromArray(hungryText);
		}
		else if(isCold){
			return GetLineFromArray(coldText);
		}
		else{
			return GetLineFromArray (healthyText);
		}
	}

	void DetermineSickness(bool medicine){
		if(!isSick){
			float chance = 0f;
			if(isHungry){
				chance = chance + (.25f * daysWithoutFood);
			}
			if(isCold){
				chance = chance + (.25f * daysWithoutHeat);
			}
			float r = Random.Range(0.0f, 1.0f);
			if(r <= chance){
				isSick = true;
			}
		}
		else{
			isSick = !medicine;
		}
	}

	void DetermineAlive(){
		if(isSick && daysWithoutMedicine > 1){
			float chance = daysWithoutMedicine * .25f;
			if(Random.Range(0.0f, 1.0f) < chance){
				isDead = true;
			}
		}
	}

	public void UpdatePerson(string key, bool food, bool heat, bool medicine){
		if(!eventManager.hasJews && !isFamily)
			return;
		eventKeyword = key;
		isHungry = !food;
		if(isHungry){
			daysWithoutFood += 1;
		}
		else{
			daysWithoutFood = 0;
		}
		isCold = !heat;
		if(isCold){
			daysWithoutHeat += 1;
		}
		else{
			daysWithoutHeat = 0;
		}
		DetermineSickness(medicine);
		if(isSick){
			daysWithoutMedicine += 1;
		}
		else{
			daysWithoutMedicine = 0;
		}
		DetermineAlive();
		lineMemory = -1;
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.GetComponent<SideScrollWalkScript>()){
			displayText = true;
			targetTextColor.a = 1f;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if(other.GetComponent<SideScrollWalkScript>()){
			targetTextColor.a = 0f;
		}
	}

	void OnGUI(){
		if(displayText){
			Vector3 screenPoint = Camera.main.WorldToScreenPoint(textPos);
			float xGUI = screenPoint.x;
			float yGUI = Screen.height - screenPoint.y;
			float widthGUI = Screen.width*.5f;
			float heightGUI = Screen.height*.2f;
			int fontSize = (int)(Screen.height/20f);

			GUI.skin = skin;
			GUI.skin.label.fontSize = fontSize;
			GUI.skin.label.normal.textColor = textColor;

			GUI.Label(new Rect(xGUI, yGUI, widthGUI, heightGUI), GetSpeech());
		}
	}
}
