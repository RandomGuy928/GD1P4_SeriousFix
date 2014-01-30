using UnityEngine;
using System.Collections;

public class CustomerScript : MonoBehaviour {

	public GameObject currentNode;
	public float speed = 5f;
	public float waitTime = 5f;
	public string firstName = "Polish Man";
	public bool isMale = true;
	public float money = 10f;
	public float willingMarkup = 1.1f;
	public float markupLeniency = .05f;
	public string AIType = "customer";
	public string goal = "Delicacy";
	public string convID = "test_conversation";
	public bool findsOwnItem = false;

	string AIState = "browse";
	float currentWaitTime;
	bool wasStationary;
	Animator animator;
	Stack path;
	NavmanagerScript navManager;
	GameObject destination;
	CustomerManagerScript customerManager;
	ConversationManagerScript conversationManager;
	GameObject item;
	ItemPositionManagerScript itemManager;
	//AudioDoorScript audioDoorManager = GameObject.FindGameObjectWithTag("AudioDoorManager").GetComponent<AudioDoorScript>();

	// Use this for initialization
	void Awake () {
		itemManager = GameObject.FindGameObjectWithTag("ItemPositions").GetComponent<ItemPositionManagerScript>();
		customerManager = GameObject.FindGameObjectWithTag ("EventManager").GetComponent<CustomerManagerScript>();
		conversationManager = GameObject.FindGameObjectWithTag ("EventManager").GetComponent<ConversationManagerScript>();
		animator = GetComponent<Animator>();
		Physics2D.IgnoreLayerCollision(gameObject.layer, gameObject.layer, true);
		navManager = GameObject.FindGameObjectWithTag("NavManager").GetComponent<NavmanagerScript>();
	}

	void Start(){
		customerManager.AddCustomer(gameObject);

	}

	void FixedUpdate(){
		if(destination != null){
			Vector3 trajectory = -transform.position + destination.transform.position;
			rigidbody2D.velocity = trajectory.normalized * speed;
		}
		else{
			rigidbody2D.velocity = Vector2.zero;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(wasStationary && rigidbody2D.velocity != Vector2.zero){
			transform.up = -rigidbody2D.velocity.normalized;
		}
		
		wasStationary = true;
		if(rigidbody2D.velocity.normalized != Vector2.zero){
			animator.Play ("walking");
			//transform.up = -(rigidbody2D.velocity.normalized);
			wasStationary = false;
			transform.up = Vector3.Lerp (transform.up, -rigidbody2D.velocity.normalized, .7f);
		}
		else{
			animator.Play("standing");
		}

		if(AIState == "browse"){
			if(currentWaitTime < 0f){
				path = navManager.ShortestPath (currentNode, navManager.GetRandomDestination(currentNode));
				currentWaitTime = waitTime;
			}
			if(destination == null && (path == null || path.Count == 0)){
				currentWaitTime -= Time.deltaTime;
			}
		}
		if(AIState == "checkout1"){
			if(destination != null){
				path = navManager.ShortestPath (destination, navManager.GetNavPoint("CounterCheckout"));
			}
			else{
				path = navManager.ShortestPath (currentNode, navManager.GetNavPoint("CounterCheckout"));
			}
			AIState = "checkout2";
		}
		if(AIState == "checkout2"){
			if(currentNode == navManager.GetNavPoint ("CounterCheckout") && rigidbody2D.velocity.magnitude == 0){
				AIState = "talking";
				conversationManager.InitializeConversation(gameObject, goal, convID);
			}
		}
		if(AIState == "leave1"){
			path = navManager.ShortestPath (currentNode, navManager.GetNavPoint ("StreetNav"));
			AIState = "leave2";
		}

		if(destination != null && Vector3.Distance(transform.position, destination.transform.position) < .1f){
			currentNode = destination;
			destination = null;
		}
		if(destination == null && path != null && path.Count > 0){
			destination = (GameObject)path.Pop ();
		}
		if(currentNode != null && destination == null && path!= null && path.Count == 0){
			transform.up = -currentNode.transform.up;
		}
	}

	public void SendToRegister(){
		AIState = "checkout1";
	}

	public bool IsFocus(){
		return (AIState == "checkout1" || AIState == "checkout2" || AIState == "talking");
	}

	public bool Leaving(){
		return (AIState == "leave1" || AIState == "leave2");
	}

	public void SendToStreet(){
		AIState = "leave1";
		customerManager.RemoveCustomer (gameObject);
	}

	public void InitializeCustomer(string startNodeName){
		currentNode = navManager.GetNavPoint (startNodeName);
		transform.position = currentNode.transform.position;
		AIType = "browse";
	}

	public void SetProperties(CustomerSpawner cs){
		money = cs.money;
		isMale = cs.isMale;
		convID = cs.convID;
		goal = cs.goal;
		willingMarkup = cs.willingMarkup;
		markupLeniency = cs.markupLeniency;
	}

	public string GetAIState(){
		return AIState;
	}

	void OnTriggerEnter2D(Collider2D col){
		if(Leaving () && col.name.Equals ("StreetDoor")){
			Destroy (gameObject);
			//audioDoorManager.PlayAudioDoor();
		}
	}

	public GameObject GetItem(){
		return item;
	}

	public GameObject FindValidItem(){
		item = itemManager.GetRandomDisplayedItem(goal, money);
		return item;
	}

	public bool TestItemValidity(GameObject checkItem){
		if(checkItem == null){
			return false;
		}
		if(goal == "any"){
			return true;
		}
		return checkItem.GetComponent<ItemScript>().HasKeyword (goal.ToLower ());
	}

	public bool SetItem(GameObject newItem){
		if(newItem == null){
			return false;
		}
		if(goal == "any"){
			item = newItem;
			return true;
		}
		if(newItem.GetComponent<ItemScript>().HasKeyword(goal)){
			item = newItem;
			return true;
		}
		return false;
	}

	public bool TestIfWillingToPay(float proposedPrice){
		if(proposedPrice > money || proposedPrice > (item.GetComponent<ItemScript>().basePrice * willingMarkup)){
			willingMarkup = willingMarkup + markupLeniency;
			return false;
		}
		return true;
	}
}
