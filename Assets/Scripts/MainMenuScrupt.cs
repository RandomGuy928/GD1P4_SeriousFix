using UnityEngine;
using System.Collections;

public class MainMenuScrupt : MonoBehaviour {

	public GUISkin skin;
	public Texture2D menu;

	void Update () {
		if(Input.GetKeyDown (KeyCode.Escape)){
			Application.Quit ();
		}
		if(Input.anyKeyDown){
			Application.LoadLevel ("prolog");
		}
	}
	
	void OnGUI () {
		GUI.skin = skin;
		GUI.skin.label.normal.background = menu;
		GUI.Label (new Rect(0, 0, Screen.width, Screen.height), "");
	}
}
