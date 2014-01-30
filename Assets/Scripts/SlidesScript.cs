using UnityEngine;
using System.Collections;

public class SlidesScript : MonoBehaviour {

	public GUISkin skin;

	public Texture2D prolog;
	public Texture2D ending1;
	public Texture2D ending2;
	public Texture2D ending3;


	string[] pString = {"","In Nazi-controlled Poland, life wasn't easy.  The Polish fought their new rulers at every turn.",
	"One of the signature resistance movements, the Home Army, was the largest underground threat in all of the Nazi's seized lands.",
	"The Home Army was also highly supportive of Jewish rights, and sought to free them at all opportunities.",
	"We will visit a small bakery in a Polish village, to experience the brutality and the tough choices the people of these time had to make every day."};
		
	string[] e1String = {"","Without Michael and his family to support them, the Jewish family was taken by the Nazi government and sent to Auschwitz.",
		"They would die in the gas chambers before the year was out.  Michael went on to become a successful business man,",
		"with frequent dealings with the Nazis, until the Soviets invaded Poland and the Nazis sent him to die in the front lines."};
			
	string[] e2String = {"","Michael and his family were tried and convicted of treachery to the state and executed.",
		"The Jewish family were sent to Auschwitz where they died in the gas chambers within the year."};
			
	string[] e3String = {"","Dorota successfully brought the Jews to their new location, and the three of them would eventually make it to the United States and safety.",
		"Michael and his family were sent to Auschwitz along with the rest of the village, where their baking skills were useful enough to keep them alive.",
		"They were rescued when Auschwitz was overrun by the Allies, and they returned to Poland where they re-established the bakery and thrived."};

	int index = 0;

	public bool starting = true;
	public bool ending;

	string scene = "prolog";
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)){
			Application.Quit();
		}
		if(starting || ending){
			if(Input.GetButtonDown ("Select")){
				index++;
			}
			if(index >= GetArray ().Length){
				End();
			}
		}
	}

	void End(){
		if(scene == "prolog"){
			starting = false;
			ending = false;
			index = 0;
			Application.LoadLevel ("shop");
		}
		else{
			Application.LoadLevel("Menu");
		}
	}

	public void StartEnding(int endingNum){
		ending = true;
		Camera.main.transform.position = new Vector3(-200f, -200f, -200f);
		scene = "end" + endingNum.ToString ();
		Debug.Log (scene);
	}

	string[] GetArray(){
		if(scene == "prolog"){
			return pString;
		}
		else if(scene == "end1"){
			return e1String;
		}
		else if(scene == "end2"){
			return e2String;
		}
		else if(scene == "end3"){
			return e3String;
		}
		return e3String;
	}

	string GetText(){
		if(scene == "prolog"){
			return pString[index];
		}
		else if(scene == "end1"){
			return e1String[index];
		}
		else if(scene == "end2"){
			return e2String[index];
		}
		else if(scene == "end3"){
			return e3String[index];
		}
		return e3String[index];
	}

	void OnGUI(){
		if(!starting && !ending){
			return;
		}
		if(index >= GetArray ().Length){
			return;
		}

		GUI.skin = skin;

		if(scene == "prolog"){
			GUI.skin.box.normal.background = prolog;
		}
		else if(scene == "end1" || (scene == "end3" && index == 3)){
			GUI.skin.box.normal.background = ending1;
		}
		else if(scene == "end2"){
			GUI.skin.box.normal.background = ending2;
		}
		else if(scene == "end3" && index < 3){
			GUI.skin.box.normal.background = ending3;
		}
		GUI.Box(new Rect(0,0,Screen.width, Screen.height * .7f), "");

		GUI.skin = skin;
		
		float xGUI = 0f;
		float widthGUI = Screen.width;
		float heightGUI = Screen.height / 4f;
		float yGUI = Screen.height - heightGUI;
		int fontSize = (int)(heightGUI/5f);
		GUI.skin.label.fontSize = fontSize;
		float textOffset = 4f;

		GUI.Label (new Rect(xGUI + textOffset,yGUI + textOffset,widthGUI - (2*textOffset),heightGUI - (2*textOffset)), GetText());

	}
}
