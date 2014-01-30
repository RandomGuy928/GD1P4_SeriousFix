using UnityEngine;
using System.Collections;

public class ItemPositionManagerScript : MonoBehaviour {

	public GUISkin skin;
	public GameObject start;

	ItemPositionScript[] positions;
	ItemPositionScript currentPosition;
	InputManagerScript input;
	bool placingItems = false;
	InventoryScript inventory;
	EventManagerScript eventManager;
	TransitionScript transition;

	float openDelay = 0f;
	float openDelayTime = .1f;
	bool waitingForInventory = false;

	// Use this for initialization
	void Start () {
		transition = GameObject.FindGameObjectWithTag("Transition").GetComponent<TransitionScript>();
		eventManager = GameObject.FindGameObjectWithTag("EventManager").GetComponent<EventManagerScript>();
		input = GameObject.Find ("EventManager").GetComponent<InputManagerScript>();
		inventory = GameObject.Find ("EventManager").GetComponent<InventoryScript>();
		currentPosition = start.GetComponent<ItemPositionScript>();
		positions = new ItemPositionScript[transform.childCount];
		for(int i = 0; i < transform.childCount; i++){
			positions[i] = transform.GetChild (i).GetComponent<ItemPositionScript>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(transition.blockMoney)
			return;
		if(placingItems && !waitingForInventory && openDelay > openDelayTime && Input.GetButtonDown ("Back")){
			EndPlacement ();
		}
		if(placingItems && !waitingForInventory){
			openDelay += Time.deltaTime;
			if(input.tickUp){
				currentPosition.Deselect ();
				currentPosition = currentPosition.up.GetComponent<ItemPositionScript>();
				currentPosition.Select ();
			}
			if(input.tickDown){
				currentPosition.Deselect ();
				currentPosition = currentPosition.down.GetComponent<ItemPositionScript>();
				currentPosition.Select ();
			}
			if(input.tickLeft){
				currentPosition.Deselect ();
				currentPosition = currentPosition.left.GetComponent<ItemPositionScript>();
				currentPosition.Select ();
			}
			if(input.tickRight){
				currentPosition.Deselect ();
				currentPosition = currentPosition.right.GetComponent<ItemPositionScript>();
				currentPosition.Select ();
			}
			if(openDelay > openDelayTime && Input.GetButtonDown ("Select")){
				waitingForInventory = true;
				inventory.GetItem ("position", gameObject);
			}
		}
	}

	public void StartPlacement(){
		openDelay = 0f;
		currentPosition.Select ();
		placingItems = true;
	}

	void EndPlacement(){
		currentPosition.Deselect ();
		placingItems = false;
		eventManager.EndSetup ();
	}

	public void ReceiveItem(GameObject newItem){
		openDelay = 0f;
		waitingForInventory = false;
		currentPosition.SetItem (newItem);
	}

	public GameObject GetRandomDisplayedItem(){
		ArrayList items = new ArrayList();
		for(int i = 0; i < positions.Length; i++){
			if(positions[i].item != null){
				items.Add (positions[i].item);
			}
		}
		if(items.Count > 0){
			return ((ItemPositionScript)items[(int)Random.Range ((int)0, (int)items.Count)]).item;
		}
		else{
			return null;
		}
	}

	public GameObject GetRandomDisplayedItem(string key, float price){
		ArrayList items = new ArrayList();
		for(int i = 0; i < positions.Length; i++){
			if(positions[i].item != null){
				ItemScript it = positions[i].item.GetComponent<ItemScript>();
				if(it.basePrice <= price && it.HasKeyword (key)){
					items.Add (positions[i].item);
				}
			}
		}
		if(items.Count > 0){
			return ((GameObject)items[(int)Random.Range ((int)0, (int)items.Count)]);
		}
		else{
			return null;
		}
	}

	void OnGUI(){
		if(!placingItems || waitingForInventory || transition.blockMoney){
			return;
		}

		float xGUI = Screen.width*.05f;
		float yGUI = Screen.height*.12f;
		float widthGUI = Screen.width;
		float heightGUI = Screen.height * .3f;
		float y2GUI = Screen.height * .55f;
		int fontSize = (int)(Screen.height/15f);

		GUI.skin = skin;
		GUI.skin.label.fontSize = fontSize;

		GUI.Label (new Rect(xGUI, yGUI, widthGUI, heightGUI), "Stock your shelves.");
		GUI.Label (new Rect(xGUI, y2GUI, widthGUI, heightGUI), "Press back to finish...");
	}
}
