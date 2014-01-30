using UnityEngine;
using System.Collections;

public class FamilyManagerScript : MonoBehaviour {

	PersonScript[] family;
	PersonScript[] jews;
	bool jewded = false;

	TransitionScript transition;
	EventManagerScript eventManager;

	float wait = 0f;
	float time = .1f;

	// Use this for initialization
	void Start () {
		transition = GameObject.FindGameObjectWithTag("Transition").GetComponent<TransitionScript>();
		eventManager = GameObject.FindGameObjectWithTag("EventManager").GetComponent<EventManagerScript>();
		ArrayList temp = new ArrayList();
		for(int i = 0; i < transform.childCount; i++){
			if(transform.GetChild(i).GetComponent<PersonScript>().isFamily){
				temp.Add(transform.GetChild (i).gameObject);
			}
		}
		family = new PersonScript[temp.Count];
		for(int i = 0; i < temp.Count; i++){
			family[i] = ((GameObject)temp[i]).GetComponent<PersonScript>();
		}

		temp = new ArrayList();
		for(int i = 0; i < transform.childCount; i++){
			if(!transform.GetChild(i).GetComponent<PersonScript>().isFamily){
				temp.Add(transform.GetChild (i).gameObject);
			}
		}
		jews = new PersonScript[temp.Count];
		for(int i = 0; i < temp.Count; i++){
			jews[i] = ((GameObject)temp[i]).GetComponent<PersonScript>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		wait += Time.deltaTime;
		if(jewded && Input.GetButtonDown("Select") && wait > time){
			Application.LoadLevel("Menu");
		}
	
	}

	public PersonScript[] GetFamily(){
		return family;
	}

	public PersonScript[] GetJews(){
		return jews;
	}

	public int GetNumSick(){
		int count = 0;
		for(int i = 0; i < family.Length; i++){
			if(family[i].IsSick()){
				count++;
			}
		}
		for(int i = 0; i < jews.Length; i++){
			if(jews[i].IsSick ()){
				count++;
			}
		}
		return count;
	}

	public void UpdateFamily(string eventKeyword, Bill[] b){
		bool foodFam = false, foodJew = false, heat = false;
		ArrayList medicineNames = new ArrayList();
		int sickIndex = 0;
		for(int i = 0; i < b.Length; i++){
			string[] temp = b[i].GetName ().Split ('(');
			if(b[i].GetName () == "Food (family)"){
				foodFam = b[i].paid;
			}
			else if(b[i].GetName () == "Food (Jews)"){
				foodJew = b[i].paid;
			}
			else if(b[i].GetName () == "Heat"){
				heat = b[i].paid;
			}
			else if(temp[0] == "Medicine"){
				for(int k = 0; k < b[i].howManyPaid; k++){
					bool found = false;
					for(int j = sickIndex; j < family.Length; j++){
						if(family[j].IsSick ()){
							medicineNames.Add (family[j].firstName);
							sickIndex = j+1;
							found = true;
							break;
						}
					}
					if(!found){
						if(sickIndex < family.Length){
							sickIndex = family.Length;
						}
						for(int j = sickIndex-family.Length; j < jews.Length; j++){
							if(jews[j].IsSick()){
								medicineNames.Add (jews[j].firstName);
								sickIndex = j+1;
								found = true;
								break;
							}
						}
					}
				}
			}
		}
		for(int i = 0; i < family.Length; i++){
			family[i].UpdatePerson (eventKeyword, foodFam, heat, medicineNames.Contains (family[i].firstName));
		}
		for(int i = 0; i < jews.Length; i++){
			jews[i].UpdatePerson (eventKeyword, foodJew, heat, medicineNames.Contains (jews[i].firstName));
		}
	}

	public bool AllJewsDead(){
		//if(jews.Length == 0){
		//	return false;
		//}
		for(int i = 0; i < jews.Length; i++){
			if(!jews[i].IsDead()){
				return false;
			}
		}
		return true;
	}

	void OnGUI(){
		if(AllJewsDead ()){
			Camera.main.transform.position = new Vector3(-200, -200,-200);
			if(eventManager.phase != "baking"){
				if(jewded == false)
					wait = 0f;
				transition.blockMoney = true;
				jewded = true;
				GUI.Label(new Rect(0,0,Screen.width, Screen.height), "You let the Jews die - Game Over");
			}
		}
	}
}
