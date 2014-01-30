using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventManagerScript : MonoBehaviour {

	public GUISkin skin;
	public int maxCustomers = 3;
	public float timeBetweenCustomers = 2f;
	public float money = 50f;
	public float rent = 10f;
	public float taxRate = .1f;
	public float heat = 5f;
	public float medicine = 5f;
	public float food = 5f;
	public string phase;

	float customerTimer = -2f;
	int day = -1;
	ArrayList dailyKeywords = new ArrayList();
	ArrayList careerTransactions = new ArrayList();
	Transaction[] dailyTransactions;
	int transactionIndex = 0;
	int customersToday = 7;
	int currentCustomer = 0;
	public bool hasJews = false;

	CustomerManagerScript customerManager;
	ConversationManagerScript conversationManager;
	InventoryScript inventory;
	InputManagerScript input;
	FinancesScript finances;
	BakingScript baking;
	SideScrollWalkScript walkHome;
	TransitionScript transition;
	ItemPositionManagerScript itemPosManager;
	FamilyManagerScript familyManager;
	AudioManagerScript audioManager;
	SlidesScript slides;

	bool initialize = true;

	Dictionary<string, int> decisions = new Dictionary<string, int>();

	// Use this for initialization
	void Start () {
		slides = GetComponent<SlidesScript>();
		slides.starting = false;
		familyManager = GameObject.FindGameObjectWithTag("FamilyManager").GetComponent<FamilyManagerScript>();
		itemPosManager = GameObject.FindGameObjectWithTag("ItemPositions").GetComponent<ItemPositionManagerScript>();
		transition = GameObject.FindGameObjectWithTag("Transition").GetComponent<TransitionScript>();
		walkHome = GameObject.FindGameObjectWithTag("WalkHome").GetComponent<SideScrollWalkScript>();;
		audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManagerScript>();
		baking = GetComponent<BakingScript>();
		finances = GetComponent<FinancesScript>();
		customerManager = GetComponent<CustomerManagerScript>();
		conversationManager = GetComponent<ConversationManagerScript>();
		inventory = GetComponent<InventoryScript>();
		input = GetComponent<InputManagerScript>();
		phase = "setup";
		dailyTransactions = new Transaction[customersToday];

	}
	
	// Update is called once per frame
	void Update () {
		//if(Input.GetKeyDown (KeyCode.Z)){
		//	day++;
		//}
		if(initialize){
			initialize = false;
			itemPosManager.StartPlacement ();
		}
		if(phase == "shop"){
			audioManager.PlayBackgroundMusic();
			customerTimer += Time.deltaTime;
			SpawnCustomer();
			ShopEndCheck();
		}
		if (phase == "setup") {
			audioManager.PlayBackgroundMusic ();
		}
	}

	public void AddDecision(string reference, int choice){
		decisions[reference] = choice;
	}

	void SpawnCustomer(){
		if(customerTimer > timeBetweenCustomers && currentCustomer < customersToday && customerManager.NumCustomers () < maxCustomers){
			customerManager.SpawnCustomer (CustomerSpawner.SpawnCustomer (day+2,currentCustomer+1,decisions,dailyKeywords));
			customerTimer = 0f;
			currentCustomer++;
		}
	}

	public bool CheckEnd(){
		if(decisions.ContainsKey ("jewsave") && decisions["jewsave"] == 0){
			slides.StartEnding (1);
			return true;
		}
		if(decisions.ContainsKey ("jewsave") && decisions["jewsave"] == 1){
			hasJews = true;
		}
		if(decisions.ContainsKey ("lawyer20") && decisions["lawyer20"] == 0){
			slides.StartEnding (2);
			return true;
		}
		if(decisions.ContainsKey ("finalendingchoice")){
			slides.StartEnding (3);
			return true;
		}
		return false;
	}

	Bill[] MakeBills(){
		int acc = 4;
		int sickCount = familyManager.GetNumSick ();
		if(sickCount > 0){
			acc++;
		}
		if(hasJews){
			acc++;
		}

		float dailySold = 0f;
		for(int i = 0; i < dailyTransactions.Length; i++){
			if(dailyTransactions[i] != null){
				dailySold += dailyTransactions[i].GetBasePrice ();
			}
		}

		Bill[] bills = new Bill[acc];

		int count = 0;
		bills[count] = new Bill("Rent", rent, true);
		count++;
		bills[count] = new Bill("Taxes", Mathf.Floor ((dailySold * taxRate)*100f)/100f, true);
		count++;
		bills[count] = new Bill("Heat", heat, false);
		count++;
		bills[count] = new Bill("Food (family)", food, false);
		count++;
		if(hasJews){
			bills[count] = new Bill("Food (Jews)", food, false);
			count++;
		}
		if(sickCount > 0){
			bills[count] = new Bill("Medicine", medicine, false);
			bills[count].SetCount(sickCount);
			count++;
		}

		return bills;
	}

	void ShopEndCheck(){
		if(currentCustomer == customersToday && customerManager.NumCustomers () == 0 && GameObject.FindGameObjectsWithTag("Customer").Length==0){
			phase = "finances";
			careerTransactions.Add (dailyTransactions);
			finances.StartFinances (dailyTransactions, MakeBills(), money);
			transition.Transition (new Vector3(-20f, 0f, -10f));
		}
	}

	public void ReceiveFinances(Bill[] b){
		phase = "baking";
		for(int i = 0; i < b.Length; i++){
			if(b[i].paid){
				if(b[i].GetAmount() < 0f){
					inventory.RemoveOldestBread();
				}
				else{
					money -= (b[i].GetAmount() * Mathf.Max (b[i].howManyPaid,1f));
				}
			}
		}
		money = Mathf.Max (money, 0);
		familyManager.UpdateFamily ("", b);
		baking.StartBaking();
	}

	public void ReceiveBaking(GameObject[] bread){
		phase = "walking";
		audioManager.StopMusic ();
		for(int i = 0; i < bread.Length; i++){
			inventory.AddItem (bread[i]);
		}
		walkHome.StartWalking();
	}

	public void StopWalking(){
		StartDay ();
	}

	public void EndSetup(){
		customerTimer = 1f;
		phase = "shop";
	}

	void StartDay(){
		dailyKeywords = new ArrayList();
		itemPosManager.StartPlacement ();
		inventory.AgeItems ();
		transition.Transition (new Vector3(0f, 0f, -10f), "Day " + (day+3).ToString ());
		phase = "setup";
		audioManager.PlayBackgroundMusic();
		day+=1;
		dailyTransactions = new Transaction[customersToday];
		for(int i = 0; i < dailyTransactions.Length; i++){
			dailyTransactions[i] = new Transaction();
		}
		currentCustomer = 0;
		transactionIndex = 0;

	}

	public void AddTransaction(Transaction newTransaction){
		dailyTransactions[transactionIndex] = newTransaction;
		transactionIndex++;
		money += newTransaction.GetSalePrice ();
	}

	void OnGUI(){
		if((phase == "shop" || phase == "setup") && !transition.blockMoney){
			float xGUI = Screen.width * .7f;
			float ybGUI = 0f;
			float ylGUI = Screen.height * -.01f;
			float widthGUI = Screen.width * .3f;
			float heightbGUI = Screen.height * .06f;
			float heightlGUI = Screen.height * .08f;
			GUI.skin = skin;
			GUI.depth = 10;
			int fontSize = (int)(Screen.height / 20f);
			GUI.skin.label.fontSize = fontSize;

			GUI.Box(new Rect(xGUI, ybGUI, widthGUI, heightbGUI), "");
			GUI.skin.label.alignment = TextAnchor.UpperRight;
			GUI.Label (new Rect(xGUI, ylGUI, widthGUI, heightlGUI), ItemScript.GetPriceString(money));
		}
	}
}