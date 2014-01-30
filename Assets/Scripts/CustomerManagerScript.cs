using UnityEngine;
using System.Collections;

public class CustomerManagerScript : MonoBehaviour {

	ArrayList customers = new ArrayList();
	public GameObject customerPrefab;
	AudioManagerScript audioManager;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		/*if(Input.GetKeyDown ("e")){
			GameObject cust = (GameObject)Instantiate (customerPrefab);
			cust.GetComponent<CustomerScript>().InitializeCustomer("StreetNav");
		}*/
		audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManagerScript>();
		if(customers.Count > 0 && !((GameObject)customers[0]).GetComponent<CustomerScript>().IsFocus ()){
			((GameObject)customers[0]).GetComponent<CustomerScript>().SendToRegister ();
		}
	}

	public void SpawnCustomer(CustomerSpawner cs){
		GameObject cust = (GameObject)Instantiate (customerPrefab);
		cust.GetComponent<CustomerScript>().InitializeCustomer("StreetNav");
		cust.GetComponent<CustomerScript>().SetProperties(cs);
		if(cust.GetComponent<CustomerScript>().AIType == "Nazi"){
			audioManager.PlayAudioDoorNazi();
		}
		else{
			audioManager.PlayAudioDoor();
		}
	}

	public void AddCustomer(GameObject c){
		customers.Add (c);
	}

	public void RemoveCustomer(GameObject c){
		customers.Remove (c);
	}

	public int NumCustomers(){
		return customers.Count;
	}

}
